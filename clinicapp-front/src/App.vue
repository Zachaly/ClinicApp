<script setup lang="ts">
import { RouterView } from 'vue-router'
import Sidebar from './components/Sidebar.vue';
</script>

<template>
  <div class="columns p-2">
    <div class="column is-narrow">
      <Sidebar />
    </div>
    <div class="column">
      <RouterView />
    </div>
  </div>
</template>