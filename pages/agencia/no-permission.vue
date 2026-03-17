<template>
  <div class="agencia-login-page">
    <div class="login-box text-center">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div style="font-size:3rem">🔒</div>
      <h5 class="mt-2">Sem permissão</h5>
      <p class="text-muted">Você não tem permissão para acessar esta página.</p>
      <NuxtLink to="/agencia/painel" class="btn btn-ag-primary mt-2">Ir ao Painel</NuxtLink>
    </div>
  </div>
</template>
<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });
</script>
