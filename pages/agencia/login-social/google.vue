<template>
  <div class="agencia-login-page">
    <div class="login-box text-center">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div class="qs-spinner" style="margin:12px auto" />
      <p class="mt-2 text-muted">Autenticando com Google...</p>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });

// OAuth2 Authorization Code flow (autenticacaoGoogle) foi removido.
// O login com Google agora é feito exclusivamente via One Tap/GSI
// (autenticacaoGoogleCredential) via components/login/login-social.vue.
onMounted(() => {
  navigateTo('/agencia');
});
</script>
