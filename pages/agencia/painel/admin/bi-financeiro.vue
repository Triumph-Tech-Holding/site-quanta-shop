<script setup lang="ts">
definePageMeta({ layout: false })
if (process.client) navigateTo('/lab/bi-financeiro', { replace: true })
else throw createError({ statusCode: 301, statusMessage: 'Movido para /lab/bi-financeiro' })
</script>
<template><div /></template>
