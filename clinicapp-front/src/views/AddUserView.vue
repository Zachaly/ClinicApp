<script lang="ts" setup>
import { ref } from 'vue';
import type CreateUserRequest from '../model/user/CreateUserRequest';
import axios, { AxiosError, type AxiosResponse } from 'axios';
import type { ValidationResponseModel } from '@/model/Response';
import ValidationErrors from '@/components/ValidationErrors.vue';


const addRequest = ref<CreateUserRequest>({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    userName: ''
})

const validationErrors = ref<{[key: string]: string[]}>({})

const addUser = (type: string) => {
    axios.post('user/' + type, addRequest.value).then(() => {
        alert('User added')
        addRequest.value = {
            email: '',
            lastName: '',
            firstName: '',
            password: '',
            userName: ''
        }
        
    }).catch((error: AxiosError<ValidationResponseModel>) => {
        validationErrors.value = error.response?.data.validationErrors ?? {}
        console.log(error.response?.data)
        alert(error.response?.data.error)
    })
}

</script>

<template>
    <div>
        <div class="field">
            <label class="label">First name</label>
            <div class="control">
                <input class="input" type="text" v-model="addRequest.firstName">
                <ValidationErrors :errors="validationErrors['FirstName']"/>
            </div>
        </div>
        <div class="field">
            <label class="label">Last name</label>
            <div class="control">
                <input class="input" type="text" v-model="addRequest.lastName">
                <ValidationErrors :errors="validationErrors['LastName']"/>
            </div>
        </div>
        <div class="field">
            <label class="label">Email</label>
            <div class="control">
                <input class="input" type="text" v-model="addRequest.email">
                <ValidationErrors :errors="validationErrors['Email']"/>
            </div>
        </div>
        <div class="field">
            <label class="label">Username</label>
            <div class="control">
                <input class="input" type="text" v-model="addRequest.userName">
                <ValidationErrors :errors="validationErrors['UserName']"/>
            </div>
        </div>
        <div class="field">
            <label class="label">Password</label>
            <div class="control">
                <input class="input" type="password" v-model="addRequest.password">
                <ValidationErrors :errors="validationErrors['Password']"/>
            </div>
        </div>
        <div class="field">
            <button class="button is-primary" @click="addUser('doctor')">Add doctor</button>
            <button class="button is-receptionist" @click="addUser('receptionist')">Add receptionist</button>
            <button class="button is-danger" @click="addUser('admin')">Add admin</button> 
        </div>
    </div>
</template>
