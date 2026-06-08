<script setup lang="ts">
import { useAuthStore } from '@/stores/authStore';

const authStore = useAuthStore()
</script>

<template>
  <div v-if="!authStore.isAuthorized">
    <RouterLink to="/login">
      <button class="button is-link">Login</button>
    </RouterLink>
  </div>
  <div v-else>
    Hello {{ authStore.userData?.firstName }} {{ authStore.userData?.lastName }}
  </div>
</template>
