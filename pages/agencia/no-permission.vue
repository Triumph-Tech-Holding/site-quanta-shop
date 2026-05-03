<template>
  <div class="agencia-login-page">
    <div class="login-box np-center">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div style="font-size:3rem">🔒</div>
      <h5 class="np-title">Sem permissão</h5>
      <p class="np-hint">Você não tem permissão para acessar esta página.</p>
      <NuxtLink to="/agencia/painel" class="btn-ag-primary np-action">Ir ao Painel</NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });
</script>

<style scoped>
.np-center { text-align: center; }
.np-title { font-size: 1.125rem; font-weight: 700; color: #225f6b; margin: .5rem 0 .25rem; }
.np-hint { color: #6b7280; font-size: .9rem; margin-bottom: .75rem; }
.np-action { margin-top: .75rem; }
</style>
