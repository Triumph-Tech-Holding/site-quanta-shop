<template>
  <div class="agencia-login-page">
    <div class="login-box text-center">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div v-if="loading">
        <div class="spinner-border text-secondary mt-3" />
        <p class="mt-2 text-muted">Confirmando seu e-mail...</p>
      </div>
      <div v-else-if="success">
        <div style="font-size:3rem">✅</div>
        <h5 class="mt-3">E-mail confirmado!</h5>
        <p class="text-muted">Sua conta foi ativada com sucesso.</p>
        <NuxtLink to="/agencia" class="btn btn-ag-primary mt-2">Fazer login</NuxtLink>
      </div>
      <div v-else>
        <div style="font-size:3rem">❌</div>
        <h5 class="mt-3">Link inválido</h5>
        <p class="text-muted">{{ errorMsg || 'Token de confirmação inválido ou expirado.' }}</p>
        <NuxtLink to="/agencia" class="btn btn-ag-outline mt-2">Voltar ao login</NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });
const route = useRoute();
const api = useApi();
const loading = ref(true);
const success = ref(false);
const errorMsg = ref('');

onMounted(async () => {
  const token = route.params.token as string;
  try {
    await api.get(`/UsuarioLogin/confirmarEmail/${token}`);
    success.value = true;
  } catch (e: any) {
    errorMsg.value = e?.response?.data?.message || 'Erro ao confirmar e-mail.';
  } finally {
    loading.value = false;
  }
});
</script>
