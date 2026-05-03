<script setup lang="ts">
definePageMeta({ layout: false })
if (process.client) navigateTo('/lab/features', { replace: true })
else throw createError({ statusCode: 301, statusMessage: 'Movido para /lab/features' })
</script>
<template><div /></template>
