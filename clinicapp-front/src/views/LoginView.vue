<script lang="ts" setup>
import { useAuthStore } from '@/stores/authStore';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()

const loginRequest = ref({
    login: '',
    password: ''
})

const login = () => {
    authStore.authorize(loginRequest.value.login, loginRequest.value.password).then((success) => {
        if (success) {
            router.push('/')
        }
    })
}
</script>

<template>
    <div>
        <div class="field">
            <label class="label">Name</label>
            <div class="control">
                <input class="input" type="text" v-model="loginRequest.login">
            </div>
        </div>
        <div class="field">
            <label class="label">Password</label>
            <div class="control">
                <input class="input" type="password" v-model="loginRequest.password">
            </div>
        </div>
        <div class="field">
            <div class="control">
                <button class="button is-link" @click="login">Login</button>
            </div>
        </div>
    </div>
</template>