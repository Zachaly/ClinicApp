<script lang="ts" setup>
import type PatientModel from '@/model/patient/PatientModel';
import type UpdatePatientRequest from '@/model/patient/UpdatePatientRequest';
import type { ResponseModel } from '@/model/Response';
import axios, { AxiosError } from 'axios';
import { onMounted, ref } from 'vue';
import { onBeforeRouteUpdate, useRoute, useRouter } from 'vue-router';
import type { ValidationResponseModel } from '../model/Response';
import ValidationErrors from '@/components/ValidationErrors.vue';
import DateFormat from '@/components/DateFormat.vue';

const patient = ref<PatientModel>()
const updateRequest = ref<UpdatePatientRequest>()
const validationErrors = ref<{ [key: string]: string[] }>({})
const router = useRouter()
const route = useRoute()

const loadPatient = async (id: string) => {
    try {
        const response = await axios.get<PatientModel>("patient/" + id);
        console.log(response)
        patient.value = response.data;
        updateRequest.value = {
            id: response.data.id,
            lastName: response.data.lastName,
            firstName: response.data.firstName,
            address: response.data.address,
            postalCode: response.data.postalCode,
            city: response.data.city
        }
    }
    catch (error) {
        router.back();
    }
}

const deletePatient = () => {
    axios.delete("patient/" + patient.value!.id).then(() => {
        router.back();
    })
}

const updatePatient = () => {
    axios.put("patient", updateRequest.value).then(() => {
        alert("patient updated")
        loadPatient(updateRequest.value!.id)
    }).catch((error: AxiosError<ValidationResponseModel>) => {
        const data = error.response?.data;

        if (data) {
            alert(data.error)
            if (data.validationErrors) {
                validationErrors.value = data.validationErrors
            }
        }
    })
}

onMounted(() => {
    loadPatient(route.params.id as string)
})

</script>

<template>
    <div class="columns" v-if="patient">
        <div class="column is-half">
            <div class="content">
                <h3>Pesel: {{ patient.peselNumber }}</h3>
                <h3>Name: {{ patient.firstName }} {{ patient.lastName }}</h3>
                <h3>Birth date: <DateFormat :date-str="patient.birthDate"></DateFormat></h3>
                <h3>Address: {{ patient.address }}, {{ patient.postalCode }} {{ patient.city }}</h3>
                <button class="button is-warning" @click="deletePatient">Delete</button>
            </div>
        </div>
        <div class="column" v-if="updateRequest">
            <div class="column">
                <div class="field">
                    <label class="label">First name</label>
                    <div class="control">
                        <input class="input" type="text" v-model="updateRequest.firstName">
                        <ValidationErrors :errors="validationErrors['FirstName']" />
                    </div>
                </div>
                <div class="field">
                    <label class="label">Last name</label>
                    <div class="control">
                        <input class="input" type="text" v-model="updateRequest.lastName">
                        <ValidationErrors :errors="validationErrors['LastName']" />
                    </div>
                </div>
                <div class="field">
                    <label class="label">Address</label>
                    <div class="control">
                        <input class="input" type="text" v-model="updateRequest.address">
                        <ValidationErrors :errors="validationErrors['Address']" />
                    </div>
                </div>
                <div class="field">
                    <label class="label">Postal code</label>
                    <div class="control">
                        <input class="input" type="text" v-model="updateRequest.postalCode">
                        <ValidationErrors :errors="validationErrors['PostalCode']" />
                    </div>
                </div>
                <div class="field">
                    <label class="label">City</label>
                    <div class="control">
                        <input class="input" type="text" v-model="updateRequest.city">
                        <ValidationErrors :errors="validationErrors['City']" />
                    </div>
                </div>
                <div class="field">
                    <button class="button is-primary" @click="updatePatient">Update patient</button>
                </div>
            </div>
        </div>
    </div>
</template>