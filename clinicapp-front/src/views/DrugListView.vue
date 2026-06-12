<script setup lang="ts">
import type AddDrugRequest from '@/model/drug/AddDrugRequest';
import type DrugModel from '@/model/drug/DrugModel';
import type GetDrugRequest from '@/model/drug/GetDrugRequest';
import type UpdateDrugRequest from '@/model/drug/UpdateDrugRequest';
import type DrugClassModel from '@/model/drugClass/DrugClassModel';
import type { ValidationResponseModel } from '@/model/Response';
import axios, { AxiosError } from 'axios';
import { onMounted, ref } from 'vue';
import ValidationErrors from '@/components/ValidationErrors.vue';
import Pagination from '@/components/Pagination.vue';

const drugs = ref<DrugModel[]>([])
const drugClasses = ref<DrugClassModel[]>([])
const pageCount = ref(0)

const addRequest = ref<AddDrugRequest>({
    brandName: '',
    genericName: '',
    classId: '',
    price: 0
}) 
const updateRequest = ref<UpdateDrugRequest | null>(null)
const validationErrors = ref<{[key: string]: string[]}>({})

const getRequest = ref<GetDrugRequest>({
    PageSize: 10,
    Index: 0
})

const addDrug = () => {
    axios.post('drug', addRequest.value).then(() => {
        loadData()
        validationErrors.value = {}
        addRequest.value = {
            brandName: '',
            genericName: '',
            classId: '',
            price: 0
        }
    }).catch((error: AxiosError<ValidationResponseModel>) => {
        const data = error.response?.data;
        console.log(data)

        if(data?.error) {
            alert(data.error)
        }
        if(data?.validationErrors) {
            validationErrors.value = data.validationErrors
        }
    })
}

const selectForUpdate = (drug: DrugModel) => {
    const { id, brandName, genericName, price, classId} = drug;
    updateRequest.value = { id, brandName, genericName, price, classId};
}

const update = () => {
    axios.put('drug', updateRequest.value).then(() => {
        loadData()
        updateRequest.value = null
        validationErrors.value = {}
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

const deleteDrug = (drug: DrugModel) => {
    axios.delete(`drug/${drug.id}`).then(() => {
        drugs.value = drugs.value.filter(x => x != drug)
    }) 
}

const changePage = (pageIndex: number) => {
    getRequest.value.Index = pageIndex;
    loadData()
}

const loadData = () => {
    axios.get<DrugClassModel[]>('drugClass', { params: {SkipPagination: true}}).then(response => {
        drugClasses.value = response.data
    })

    axios.get<DrugModel[]>('drug', {params: getRequest.value}).then(response => {
        drugs.value = response.data
    })

    axios.get<number>('drug/count', { params: getRequest.value}).then(response => {
        pageCount.value = Math.ceil(response.data / getRequest.value.PageSize!)
    })
}
const clearSearch = () => {
    getRequest.value = { PageSize: 10, Index: 0 }
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
                        <th>Brand Name</th>
                        <th>Generic Name</th>
                        <th>Class Name</th>
                        <th>Price</th>
                        <th>Actions</th>
                    </tr>
                    <tr>
                        <th><input class="input" v-model="getRequest.BrandName"></th>
                        <th><input class="input" v-model="getRequest.GenericName"></th>
                        <th><div class="select">
                            <select v-model="getRequest.ClassId">
                                <option :value="undefined"></option>
                                <option v-for="c in drugClasses" :value="c.id">{{c.name}}</option>
                            </select>
                        </div></th>
                        <th><input class="input" type="number" min="0" v-model="getRequest.MaxPrice"></th>
                        <th>
                            <button class="button is-primary" @click="loadData">Search</button>
                            <button class="button is-info" @click="clearSearch">Clear</button>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="drug in drugs">
                        <th>
                            {{ drug.brandName }}
                        </th>
                        <th>{{ drug.genericName }}</th>
                        <th>{{ drug.className }}</th>
                        <th>{{ drug.price }}</th>
                        <th>
                            <button class="button" @click="selectForUpdate(drug)">Update</button>
                            <button class="button is-danger" @click="deleteDrug(drug)">Delete</button>
                        </th>
                    </tr>
                </tbody>
            </table>
            <Pagination :change-page="changePage" :page-count="pageCount" :current-index="getRequest.Index!">
            </Pagination>
        </div>
        <div class="column" v-if="!updateRequest">
            <div class="field">
                <label class="label">Brand name</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.brandName">
                    <ValidationErrors :errors="validationErrors['BrandName']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Generic name</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.genericName">
                    <ValidationErrors :errors="validationErrors['GenericName']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Price</label>
                <div class="control">
                    <input class="input" type="number" v-model="addRequest.price">
                    <ValidationErrors :errors="validationErrors['Price']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Class</label>
                <div class="select">
                    <select v-model="addRequest.classId">
                        <option v-for="c in drugClasses" :value="c.id">{{c.name}}</option>
                    </select>
                </div>
            </div>
            <div class="field">
                <button class="button is-primary" @click="addDrug">Add drug</button>
            </div>
        </div>
        <div class="column" v-else>
            <div class="field">
                <label class="label">Brand name</label>
                <div class="control">
                    <input class="input" type="text" v-model="updateRequest.brandName">
                    <ValidationErrors :errors="validationErrors['BrandName']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Generic name</label>
                <div class="control">
                    <input class="input" type="text" v-model="updateRequest.genericName">
                    <ValidationErrors :errors="validationErrors['GenericName']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Price</label>
                <div class="control">
                    <input class="input" type="number" v-model="updateRequest.price">
                    <ValidationErrors :errors="validationErrors['Price']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Class</label>
                <div class="select">
                    <select v-model="updateRequest.classId">
                        <option v-for="c in drugClasses" :value="c.id">{{c.name}}</option>
                    </select>
                </div>
            </div>
            <div class="field">
                <button class="button" @click="updateRequest = null">Cancel</button>
                <button class="button is-primary" @click="update">Update</button>
            </div>
        </div>
    </div>
</template>