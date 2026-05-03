<template>
  <div class="agencia-login-page">
    <div class="login-box">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h5 class="text-center mb-4" style="color:#2f7785">Recuperar Senha</h5>
      <form @submit.prevent="enviar" v-if="!enviado" autocomplete="off">
        <div class="mb-3">
          <label class="form-label">Seu e-mail ou login</label>
          <input v-model="email" type="text" class="form-control" placeholder="E-mail ou login" required :disabled="loading" />
        </div>
        <div v-if="errorMsg" class="qs-alert-danger mb-3" style="font-size:.875rem">{{ errorMsg }}</div>
        <button type="submit" class="btn-login" :disabled="loading">
          <span v-if="loading" class="rs-spinner" />
          {{ loading ? 'Aguarde...' : 'Enviar link de recuperação' }}
        </button>
      </form>
      <div v-else class="text-center">
        <div class="qs-alert-success mb-3">Link enviado! Verifique seu e-mail.</div>
      </div>
      <div class="text-center mt-4">
        <NuxtLink to="/agencia/login" style="font-size:.85rem; color:#2f7785">← Voltar para login</NuxtLink>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-login' });
const api = useApi();
const email = ref('');
const loading = ref(false);
const enviado = ref(false);
const errorMsg = ref('');
async function enviar() {
  loading.value = true;
  errorMsg.value = '';
  try {
    await api.post('/UsuarioLogin/recuperarSenha', { login: email.value });
    enviado.value = true;
  } catch (err: unknown) {
    errorMsg.value = extractApiErrorMessage(err, 'Não foi possível enviar o link. Verifique o e-mail informado.');
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.rs-spinner {
  display: inline-block;
  width: 14px; height: 14px;
  border: 2px solid rgba(255,255,255,.4);
  border-top-color: currentColor;
  border-radius: 50%;
  animation: rs-spin .7s linear infinite;
  vertical-align: middle;
  margin-right: 6px;
}
@keyframes rs-spin { to { transform: rotate(360deg); } }
</style>
