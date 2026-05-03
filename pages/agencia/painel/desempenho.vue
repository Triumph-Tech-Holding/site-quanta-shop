<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const router = useRouter();
onMounted(() => { router.replace('/agencia/painel/performance'); });
</script>

<template>
  <div class="ag-loading"><div class="qs-spinner" style="margin:0 auto" /></div>
</template>
