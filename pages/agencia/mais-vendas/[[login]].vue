<template>
  <div class="agencia-login-page">
    <div class="login-box">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h5 class="mv-title">Credenciamento de Parceiro</h5>
      <p class="mv-sub">
        Cadastre-se como parceiro Quanta Shop e comece a ganhar cashback nas suas vendas.
      </p>
      <div class="mv-actions">
        <NuxtLink to="/mais-vendas" class="qs-btn-primary mv-btn">
          Quero ser parceiro →
        </NuxtLink>
        <NuxtLink to="/agencia" class="qs-btn-outline mv-btn">Já tenho conta — Entrar</NuxtLink>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });
</script>

<style scoped>
.mv-title {
  text-align: center;
  font-size: 1.1rem;
  font-weight: 700;
  color: var(--qs-text, #111827);
  margin: 0 0 1rem;
}
.mv-sub {
  font-size: .875rem;
  color: var(--qs-gray-500, #6b7280);
  text-align: center;
  line-height: 1.55;
  margin: 0 0 1.5rem;
}
.mv-actions {
  display: flex;
  flex-direction: column;
  gap: .75rem;
}
.mv-btn { text-align: center; width: 100%; }
</style>
