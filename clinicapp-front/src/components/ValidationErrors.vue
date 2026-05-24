<script setup lang="ts">
const props = defineProps < {
    errors?: string[]
} > ()
</script>

<template>
    <div v-if="errors">
        <p v-for="error in errors" class="help is-danger">{{ error }}</p>
    </div>
</template>