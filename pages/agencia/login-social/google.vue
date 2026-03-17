<template>
  <div class="agencia-login-page">
    <div class="login-box text-center">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div class="spinner-border text-secondary mt-3" />
      <p class="mt-2 text-muted">Autenticando com Google...</p>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });
const route = useRoute();
const api = useApi();
const agenciaStore = useAgenciaStore();
const { $toast } = useNuxtApp();

onMounted(async () => {
  const code = route.query.code as string;
  const redirectUri = window.location.origin + '/agencia/login-social/google';
  try {
    const { data } = await api.post('/UsuarioLogin/autenticacaoGoogle', { code, redirectUri });
    if (data?.token) {
      agenciaStore.setUser(data);
      navigateTo('/agencia/painel');
    }
  } catch (e: any) {
    $toast?.error('Erro ao autenticar com Google. Tente novamente.');
    navigateTo('/agencia');
  }
});
</script>
