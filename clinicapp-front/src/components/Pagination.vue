<script setup lang="ts">
const props = defineProps<{
    pageCount: number,
    currentIndex: number,
    changePage: (index: number) => void
}>()
</script>

<template>
    <nav class="pagination" role="navigation" aria-label="pagination" v-if="pageCount > 0">
        <span class="pagination-previous" v-if="currentIndex - 1 > pageCount" @click="changePage(currentIndex - 1)">Previous</span>
        <span class="pagination-next" v-if="currentIndex + 1 < pageCount" @click="changePage(currentIndex + 1)">Next page</span>
        <ul class="pagination-list">
            <li>
                <span href="#" class="pagination-link" @click="changePage(0)">1</span>
            </li>
            <li>
                <span class="pagination-ellipsis">&hellip;</span>
            </li>
            <li>
                <span href="#" class="pagination-link" @click="changePage(currentIndex - 1)" v-if="currentIndex > 0">{{ currentIndex }}</span>
            </li>
            <li>
                <span class="pagination-link is-current">{{ currentIndex + 1 }}</span>
            </li>
            <li>
                <span href="#" class="pagination-link" @click="changePage(currentIndex + 1)" v-if="currentIndex < pageCount - 1">{{ currentIndex + 2 }}</span>
            </li>
            <li>
                <span class="pagination-ellipsis">&hellip;</span>
            </li>
            <li>
                <span href="#" class="pagination-link" @click="changePage(pageCount - 1)">{{ pageCount }}</span>
            </li>
        </ul>
    </nav>
</template>