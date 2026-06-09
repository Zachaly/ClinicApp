<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
    dateStr: string
}>()

const dateFormatted = computed(() => {
    const date = new Date(props.dateStr)

    return `${date.getUTCDay()}.${date.getUTCMonth()}.${date.getUTCFullYear()}`
})

</script>

<template>
    <span>{{dateFormatted}}</span>
</template>