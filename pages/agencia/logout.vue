<template>
  <div class="agencia-login-page">
    <div class="login-box text-center">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div class="qs-spinner" style="margin:12px auto" />
      <p class="mt-3 text-muted">Saindo...</p>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });
const agenciaStore = useAgenciaStore();
const router = useRouter();

onMounted(() => {
  agenciaStore.logout();
  localStorage.removeItem('agencia_menu');
  router.push('/agencia');
});
</script>
