<script setup lang="ts">
import type GetPatientRequest from '@/model/patient/GetPatientRequest';
import type PatientModel from '@/model/patient/PatientModel';
import { onMounted, ref } from 'vue';
import axios, { AxiosError } from 'axios';
import Pagination from '@/components/Pagination.vue';
import type AddPatientRequest from '@/model/patient/AddPatientRequest';
import type { ValidationResponseModel } from '../model/Response';
import ValidationErrors from '@/components/ValidationErrors.vue';

const getRequest = ref<GetPatientRequest>({
    PageSize: 10,
    Index: 0,
})

const patients = ref<PatientModel[]>([])
const pageCount = ref(0)

const addRequest = ref<AddPatientRequest>({
    peselNumber: '',
    lastName: '',
    firstName: '',
    address: '',
    postalCode: '',
    birthDate: '',
    city: ''
})
const validationErrors = ref<{ [key: string]: string[] }>({})

const loadPatients = () => {
    axios.get<number>('patient/count', { params: getRequest.value }).then(response =>
        pageCount.value = Math.ceil(response.data / getRequest.value.PageSize!))
    axios.get<PatientModel[]>('patient', { params: getRequest.value }).then(response => {
        patients.value = response.data
    })
}

const searchPatients = () => {
    getRequest.value.Index = 0;
    loadPatients()
}

const changePage = (index: number) => {
    getRequest.value.Index = index;
    loadPatients();
}

const clearSearch = () => {
    getRequest.value.LastName = undefined;
    getRequest.value.PeselNumber = undefined;
}

const addPatient = () => {
    axios.post("patient", addRequest.value).then(() => {
        alert("patient added");
        addRequest.value = {
            peselNumber: '',
            lastName: '',
            firstName: '',
            address: '',
            postalCode: '',
            birthDate: '',
            city: ''
        }

        loadPatients()
    }).catch((error: AxiosError<ValidationResponseModel>) => {

        const data = error.response?.data;

        if (data) {
            alert(data.error);

            if (data.validationErrors) {
                validationErrors.value = data.validationErrors
            }
        }
    })
}

onMounted(() => {
    loadPatients()
})

</script>

<template>
    <div class="columns">
        <div class="column is-half">
            <table class="table">
                <thead>
                    <tr>
                        <th>Pesel</th>
                        <th>First name</th>
                        <th>Last name</th>
                        <th>Actions</th>
                    </tr>
                    <tr>
                        <th><input class="input" v-model="getRequest.PeselNumber"></th>
                        <th></th>
                        <th><input class="input" v-model="getRequest.LastName"></th>
                        <th>
                            <button class="button is-primary" @click="searchPatients">Search</button>
                            <button class="button is-info" @click="clearSearch">Clear</button>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="patient in patients">
                        <th>
                            <RouterLink :to="`/patient/${patient.id}`">{{ patient.peselNumber }}</RouterLink>
                        </th>
                        <th>{{ patient.firstName }}</th>
                        <th>{{ patient.lastName }}</th>
                    </tr>
                </tbody>
            </table>
            <Pagination :change-page="changePage" :page-count="pageCount" :current-index="getRequest.Index!">
            </Pagination>
        </div>
        <div class="column">
            <div class="field">
                <label class="label">First name</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.firstName">
                    <ValidationErrors :errors="validationErrors['FirstName']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Last name</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.lastName">
                    <ValidationErrors :errors="validationErrors['LastName']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Pesel</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.peselNumber">
                    <ValidationErrors :errors="validationErrors['PeselNumber']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Address</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.address">
                    <ValidationErrors :errors="validationErrors['Address']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Postal code</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.postalCode">
                    <ValidationErrors :errors="validationErrors['PostalCode']" />
                </div>
            </div>
            <div class="field">
                <label class="label">City</label>
                <div class="control">
                    <input class="input" type="text" v-model="addRequest.city">
                    <ValidationErrors :errors="validationErrors['City']" />
                </div>
            </div>
            <div class="field">
                <label class="label">Birth date</label>
                <div class="control">
                    <input type="date" v-model="addRequest.birthDate">
                    <ValidationErrors :errors="validationErrors['BirthDate']" />
                </div>
            </div>
            <div class="field">
                <button class="button is-primary" @click="addPatient">Add patient</button>
            </div>
        </div>
    </div>

</template>