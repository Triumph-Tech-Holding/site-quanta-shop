<script setup lang="ts">
definePageMeta({ layout: false })
if (process.client) navigateTo('/lab/docs', { replace: true })
else throw createError({ statusCode: 301, statusMessage: 'Movido para /lab/docs' })
</script>
<template><div /></template>
