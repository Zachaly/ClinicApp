<script lang="ts" setup>
import Pagination from '@/components/Pagination.vue';
import ValidationErrors from '@/components/ValidationErrors.vue';
import type AddMedicalProcedureRequest from '@/model/medicalProcedure/AddMedicalProcedureRequest';
import type GetMedicalProcedureRequest from '@/model/medicalProcedure/GetMedicalProcedureRequest';
import type MedicalProcedureModel from '@/model/medicalProcedure/MedicalProcedureModel';
import type UpdateMedicalProcedureRequest from '@/model/medicalProcedure/UpdateMedicalProcedureRequest';
import type { ValidationResponseModel } from '@/model/Response';
import axios, { AxiosError } from 'axios';
import { onMounted, ref } from 'vue';


const procedures = ref<MedicalProcedureModel[]>([])
const getRequest = ref<GetMedicalProcedureRequest>({
    PageSize: 10,
    Index: 0
})
const pageCount = ref(1)
const currentIndex = ref(0)

const addRequest = ref<AddMedicalProcedureRequest>({
    name: '',
    description: '',
    cost: 0
})
const validationErrors = ref<{ [key: string]: string[] }>({})
const updateRequest = ref<UpdateMedicalProcedureRequest | null>(null)

const addMedicalProcedure = () => {
    axios.post('medicalProcedure', addRequest.value).then(() => {
        addRequest.value = {
            name: '',
            description: '',
            cost: 0
        }
        validationErrors.value = {}
        loadData()
    }).catch((error: AxiosError<ValidationResponseModel>) => {
        const data = error.response?.data

        if (data?.error) {
            alert(data.error)
        }
        if (data?.validationErrors) {
            validationErrors.value = data.validationErrors
        }
    })
}

const loadData = () => {
    axios.get<number>('medicalProcedure/count', { params: getRequest.value }).then(response => {
        pageCount.value = Math.ceil(response.data)

        axios.get<MedicalProcedureModel[]>('medicalProcedure', { params: getRequest.value }).then(response => {
            procedures.value = response.data;
        })
    })
}

const changePage = (index: number) => {
    getRequest.value.Index = index
    loadData()
}

const clearSearch = () => {
    getRequest.value = {
        PageSize: 10,
        Index: 0
    }
}

const selectForUpdate = (procedure: MedicalProcedureModel) => {
    updateRequest.value = {
        name: procedure.name,
        description: procedure.description,
        cost: procedure.cost,
        id: procedure.id
    }
}

const update = () => {
    axios.put('medicalProcedure', updateRequest.value).then(() => {
        updateRequest.value = null
        validationErrors.value = {}
    }).catch((error: AxiosError<ValidationResponseModel>) => {
        const data = error.response?.data

        if (data?.error) {
            alert(data.error)
        }
        if (data?.validationErrors) {
            validationErrors.value = data.validationErrors
        }
    })
}

const deleteProcedure = (procedure: MedicalProcedureModel) => {
    axios.delete(`medicalProcedure/${procedure.id}`).then(() => {
        procedures.value = procedures.value.filter(x => x != procedure)
    })
}

onMounted(() => {
    loadData()
})
</script>

<template>
    <div class="columns">
        <div class="column is-half">
            <table class="table">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Cost</th>
                        <th>Description</th>
                        <th>Actions</th>
                    </tr>
                    <tr>
                        <th>
                            <input class="input" type="text" v-model="getRequest.Name">
                        </th>
                        <th></th>
                        <th></th>
                        <th>
                            <button class="button" @click="loadData">Search</button>
                            <button class="button" @click="clearSearch">Clear search</button>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="procedure in procedures">
                        <th>
                            {{ procedure.name }}
                        </th>
                        <th>
                            {{ procedure.cost }}
                        </th>
                        <th>
                            {{ procedure.description }}
                        </th>
                        <th>
                            <button class="button is-danger" @click="deleteProcedure(procedure)">Delete</button>
                            <button class="button is-warning" @click="selectForUpdate(procedure)">Update</button>
                        </th>
                    </tr>
                </tbody>
            </table>
            <Pagination :current-index="currentIndex" :page-count="pageCount" :change-page="changePage"></Pagination>
        </div>
        <div class="column" v-if="!updateRequest">
            <div class="field">
                <label class="label">Name</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.name">
                    <ValidationErrors :errors="validationErrors['Name']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Description</label>
                <div class="control">
                    <textarea class="textarea" v-model="addRequest.description"></textarea>
                    <ValidationErrors :errors="validationErrors['Description']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Cost</label>
                <div class="control">
                    <input class="input" type="number" v-model="addRequest.cost">
                    <ValidationErrors :errors="validationErrors['Cost']" />
                </div>
            </div>
            <div class="field">
                <button class="button is-primary" @click="addMedicalProcedure">Add drug class</button>
            </div>
        </div>
        <div class="column" v-else>
            <div class="field">
                <label class="label">Name</label>
                <div class="control">
                    <input class="input" type="text" v-model="updateRequest.name">
                    <ValidationErrors :errors="validationErrors['Name']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Description</label>
                <div class="control">
                    <textarea class="textarea" v-model="updateRequest.description"></textarea>
                    <ValidationErrors :errors="validationErrors['Description']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Cost</label>
                <div class="control">
                    <input class="input" type="number" v-model="updateRequest.cost">
                    <ValidationErrors :errors="validationErrors['Cost']" />
                </div>
            </div>
            <div class="field">
                <button class="button" @click="updateRequest = null">Cancel</button>
                <button class="button is-primary" @click="update">Update</button>
            </div>
        </div>
    </div>
</template>