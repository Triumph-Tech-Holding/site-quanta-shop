<template>
  <div class="agencia-login-page">
    <div class="login-box">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h5 class="text-center mb-4">Credenciamento de Parceiro</h5>
      <p class="text-muted text-center mb-4" style="font-size:.875rem">
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
