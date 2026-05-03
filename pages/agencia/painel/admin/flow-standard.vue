<script setup lang="ts">
definePageMeta({ layout: false })
if (process.client) navigateTo('/lab/flow-standard', { replace: true })
else throw createError({ statusCode: 301, statusMessage: 'Movido para /lab/flow-standard' })
</script>
<template><div /></template>
