<template>
  <div class="agencia-login-page">
    <div class="login-box ce-center">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div v-if="loading">
        <div class="qs-spinner" style="margin:16px auto" />
        <p class="ce-hint">Confirmando seu e-mail...</p>
      </div>
      <div v-else-if="success">
        <div style="font-size:3rem">✅</div>
        <h5 class="ce-title">E-mail confirmado!</h5>
        <p class="ce-hint">Sua conta foi ativada com sucesso.</p>
        <NuxtLink to="/agencia/login" class="btn-ag-primary ce-action">Fazer login</NuxtLink>
      </div>
      <div v-else>
        <div style="font-size:3rem">❌</div>
        <h5 class="ce-title">Link inválido</h5>
        <p class="ce-hint">{{ errorMsg || 'Token de confirmação inválido ou expirado.' }}</p>
        <NuxtLink to="/agencia/login" class="btn-ag-outline ce-action">Voltar ao login</NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
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
  } catch (e: unknown) {
    errorMsg.value = extractApiErrorMessage(e, 'Erro ao confirmar e-mail.');
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.ce-center { text-align: center; }
.ce-title { font-size: 1.125rem; font-weight: 700; color: #225f6b; margin: .75rem 0 .25rem; }
.ce-hint { color: #6b7280; font-size: .9rem; margin: .5rem 0; }
.ce-action { margin-top: .75rem; }
</style>
