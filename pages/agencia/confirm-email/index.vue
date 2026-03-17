<template>
  <div class="agencia-login-page">
    <div class="login-box text-center">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div style="font-size:3rem; margin-bottom:1rem">✉️</div>
      <h2 style="color:#2f7785; font-weight:700; margin-bottom:.5rem">Confirme seu e-mail</h2>
      <p class="text-muted mb-4" style="font-size:.95rem">
        Um link de confirmação foi enviado para o seu e-mail. Clique no link recebido para ativar sua conta.
      </p>
      <p class="text-muted mb-4" style="font-size:.85rem">
        Caso não encontre o e-mail, verifique sua caixa de spam ou solicite um novo envio.
      </p>
      <NuxtLink to="/agencia/login" class="btn btn-ag-primary px-4">Ir para o login</NuxtLink>
    </div>
  </div>
</template>
<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });
</script>
