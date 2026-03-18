<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const router = useRouter();
onMounted(() => { router.replace('/agencia/painel/performance'); });
</script>

<template>
  <div class="ag-loading"><div class="spinner-border" /></div>
</template>
