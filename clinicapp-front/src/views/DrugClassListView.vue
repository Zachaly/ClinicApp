<script setup lang="ts">
import type AddDrugClassRequest from '@/model/drugClass/AddDrugClassRequest';
import type DrugClassModel from '@/model/drugClass/DrugClassModel';
import type UpdateDrugClassRequest from '@/model/drugClass/UpdateDrugClassRequest';
import type { ValidationResponseModel } from '@/model/Response';
import axios, { AxiosError } from 'axios';
import { onMounted, ref } from 'vue';
import ValidationErrors from '@/components/ValidationErrors.vue';

const drugClasses = ref<DrugClassModel[]>([])
const addRequest = ref<AddDrugClassRequest>({
    name: ''
})
const updateRequest = ref<UpdateDrugClassRequest | null>(null)

const validationErrors = ref<{[key: string]: string[]}>({})


const loadData = () => {
    axios.get<DrugClassModel[]>('drugClass', { params: { SkipPagination: true }}).then(response => {
        drugClasses.value = response.data
    })
}

const addDrugClass = () => {
    axios.post('drugClass', addRequest.value).then(() => {
        loadData()
        addRequest.value.name = ''
        validationErrors.value = {}
    }).catch((error: AxiosError<ValidationResponseModel>) => {
        const data = error.response?.data;

        if(data?.error) {
            alert(data.error);
        }

        if(data?.validationErrors) {
            validationErrors.value = data.validationErrors;
        }
    })
}

const deleteDrugClass = (drugClass: DrugClassModel) => {
    axios.delete(`drugClass/${drugClass.id}`).then(() => {
        drugClasses.value = drugClasses.value.filter(x => x != drugClass)
    })
}

const selectForUpdate = (drugClass: DrugClassModel) => {
    updateRequest.value = {
        id: drugClass.id,
        name: drugClass.name
    }
}

const update = () => {
    axios.put('drugClass', updateRequest.value).then(() => {
        loadData()
        updateRequest.value = null
    }).catch((error: AxiosError<ValidationResponseModel>) => {
        const data = error.response?.data

        if(data?.error) {
            alert(data.error)
        }
        if(data?.validationErrors) {
            validationErrors.value = data.validationErrors
        }
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
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="drugClass in drugClasses">
                        <th>
                            {{ drugClass.name }}
                        </th>
                        <th><button class="button is-danger" @click="deleteDrugClass(drugClass)">Delete</button>
                        <button class="button is-warning" @click="selectForUpdate(drugClass)">Update</button></th>
                    </tr>
                </tbody>
            </table>
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
                <button class="button is-primary" @click="addDrugClass">Add drug class</button>
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
                <button class="button" @click="updateRequest = null">Cancel</button>
                <button class="button is-primary" @click="update">Update</button>
            </div>
        </div>
    </div>
</template>