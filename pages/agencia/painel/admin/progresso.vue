<script setup lang="ts">
definePageMeta({ layout: false })
if (process.client) navigateTo('/lab/progresso', { replace: true })
else throw createError({ statusCode: 301, statusMessage: 'Movido para /lab/progresso' })
</script>
<template><div /></template>
