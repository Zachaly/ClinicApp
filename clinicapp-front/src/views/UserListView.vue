<script setup lang="ts">
import Pagination from '@/components/Pagination.vue';
import type { ResponseModel } from '@/model/Response';
import type GetUserRequest from '@/model/user/GetUserRequest';
import type UserModel from '@/model/user/UserModel';
import { claimNames, useAuthStore } from '@/stores/authStore';
import axios, { AxiosError } from 'axios';
import { onMounted, ref } from 'vue';

const getRequest = ref<GetUserRequest>({
    PageSize: 10,
    Index: 0
})

const authStore = useAuthStore()

const users = ref<UserModel[]>([])
const pageCount = ref(0)

const loadUsers = () => {
    axios.get<number>('user/count', { params: getRequest.value }).then(response =>
        pageCount.value = Math.ceil(response.data / getRequest.value.PageSize!))
    axios.get<UserModel[]>('user', { params: getRequest.value }).then(response => {
        users.value = response.data
    })
}

const deleteUser = (id: string) => {
    axios.delete(`user/${id}`).then(() => {
        users.value = users.value.filter(x => x.id != id)
    })
}

const addClaim = (user: UserModel, claim: string) => {
    axios.post('authorization/claim/' + claim, { id: user.id })
        .then(() => { user.claims.push(claim) })
        .catch((error: AxiosError<ResponseModel>) => {
            alert(error.response?.data.error);
        })
}

const searchUsers = () => {
    getRequest.value.Index = 0;
    loadUsers()
}

const changePage = (index: number) => {
    getRequest.value.Index = index;
    loadUsers();
}

const clearSearch = () => {
    getRequest.value.FirstName = undefined;
    getRequest.value.LastName = undefined;
    getRequest.value.UserName = undefined;
}

onMounted(() => {
    loadUsers()
})

</script>

<template>
    <table class="table">
        <thead>
            <tr>
                <th>Id</th>
                <th>First name</th>
                <th>Last name</th>
                <th>UserName</th>
                <th>Email</th>
                <th>Roles</th>
                <th>Actions</th>
            </tr>
            <tr>
                <th></th>
                <th><input class="input" v-model="getRequest.FirstName"></th>
                <th><input class="input" v-model="getRequest.LastName"></th>
                <th><input class="input" v-model="getRequest.UserName"></th>
                <th></th>
                <th></th>
                <th>
                    <button class="button is-primary" @click="searchUsers">Search</button>
                    <button class="button is-info" @click="clearSearch">Clear</button>
                </th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="user in users">
                <th>{{ user.id }}</th>
                <th>{{ user.firstName }}</th>
                <th>{{ user.lastName }}</th>
                <th>{{ user.userName }}</th>
                <th>{{ user.email }}</th>
                <th><span v-for="role in user.claims">{{ role }}, </span></th>
                <th>
                    <button class="button is-danger" v-if="authStore.userData?.id != user.id"
                        @click="deleteUser(user.id)">Delete</button>
                    <button class="button is-danger" v-if="!user.claims.includes(claimNames.admin)"
                        @click="addClaim(user, claimNames.admin)">Add admin
                        claim</button>
                    <button class="button is-warning" v-if="!user.claims.includes(claimNames.doctor)"
                        @click="addClaim(user, claimNames.doctor)">Add doctor
                        claim</button>
                    <button class="button is-warning" v-if="!user.claims.includes(claimNames.receptionist)"
                        @click="addClaim(user, claimNames.receptionist)">Add
                        receptionist claim</button>
                </th>
            </tr>
        </tbody>
    </table>
    <Pagination :change-page="changePage" :page-count="pageCount" :current-index="getRequest.Index!"></Pagination>
</template>