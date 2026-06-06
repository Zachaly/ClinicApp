<script lang="ts" setup>
import { claimNames, useAuthStore } from '@/stores/authStore';
import { computed } from 'vue';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()

const isAdmin = computed(() => authStore.userData?.claims.includes(claimNames.admin))
const isDoctor = computed(() => authStore.userData?.claims.includes(claimNames.doctor))
const isReceptionist = computed(() => authStore.userData?.claims.includes(claimNames.receptionist))

const logout = () => {
    authStore.logout()

    router.clearRoutes()
    router.push('/login')
}

</script>

<template>
    <aside class="menu" v-if="authStore.isAuthorized">
        <p class="menu-label">General</p>
        <ul class="menu-list">
            <li>
                <RouterLink to="/">
                    Home
                </RouterLink>
            </li>
        </ul>
        <p class="menu-label" v-if="isAdmin">Admin</p>
        <ul class="menu-list" v-if="isAdmin">
            <li>
                <RouterLink to="/users">
                    Users
                </RouterLink>
            </li>
            <li>
                <RouterLink to="/users/add">
                    Add user
                </RouterLink>
            </li>
        </ul>
        <p class="menu-label" v-if="isDoctor">Doctor</p>
        <ul class="menu-list" v-if="isDoctor">

        </ul>
        <p class="menu-label" v-if="isReceptionist">Receptionist</p>
        <ul class="menu-list" v-if="isReceptionist">
            <li>
                <RouterLink to="/patient">
                    Manage patients
                </RouterLink>
            </li>
        </ul>
        <button class="button is-danger" @click="logout">Logout</button>
    </aside>
</template>