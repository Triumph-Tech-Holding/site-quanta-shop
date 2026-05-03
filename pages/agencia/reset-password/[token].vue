<template>
  <div class="agencia-login-page">
    <div class="login-box">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h5 class="text-center mb-4">Redefinir senha</h5>
      <div v-if="success" class="text-center">
        <div style="font-size:2.5rem">✅</div>
        <p class="mt-2 text-muted">Senha redefinida com sucesso!</p>
        <NuxtLink to="/agencia/login" class="btn btn-ag-primary mt-2">Fazer login</NuxtLink>
      </div>
      <form v-else @submit.prevent="redefinir">
        <div class="mb-3">
          <input v-model="form.novaSenha" type="password" class="form-control" required minlength="6" placeholder="Nova senha" />
        </div>
        <div class="mb-3">
          <input v-model="form.confirmar" type="password" class="form-control" required minlength="6" placeholder="Confirmar nova senha" />
        </div>
        <div v-if="errorMsg" class="qs-alert-danger mb-3" style="font-size:.875rem">{{ errorMsg }}</div>
        <button type="submit" class="btn-login" :disabled="loading">
          <span v-if="loading" class="rp-spinner" />
          {{ loading ? 'Aguarde...' : 'Redefinir Senha' }}
        </button>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-login' });
const route = useRoute();
const api = useApi();
const loading = ref(false);
const success = ref(false);
const errorMsg = ref('');
const form = reactive({ novaSenha: '', confirmar: '' });

async function redefinir() {
  if (form.novaSenha !== form.confirmar) { errorMsg.value = 'As senhas não coincidem.'; return; }
  loading.value = true;
  errorMsg.value = '';
  try {
    await api.post('/UsuarioLogin/resetarSenha', { token: route.params.token, novaSenha: form.novaSenha });
    success.value = true;
  } catch (e: unknown) {
    errorMsg.value = extractApiErrorMessage(e, 'Erro ao redefinir senha.');
  } finally { loading.value = false; }
}
</script>

<style scoped>
.rp-spinner {
  display: inline-block;
  width: 14px; height: 14px;
  border: 2px solid rgba(255,255,255,.4);
  border-top-color: currentColor;
  border-radius: 50%;
  animation: rp-spin .7s linear infinite;
  vertical-align: middle;
  margin-right: 6px;
}
@keyframes rp-spin { to { transform: rotate(360deg); } }
</style>
