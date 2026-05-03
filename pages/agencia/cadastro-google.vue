<template>
  <div class="agencia-login-page">
    <div class="login-box" style="max-width:520px">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h5 class="cgg-title">Cadastro com Google</h5>
      <p class="cgg-sub">Complete os dados abaixo para criar sua conta.</p>
      <div v-if="!credential" class="qs-alert-warn" style="font-size:.875rem">
        Sessão expirada. <NuxtLink to="/agencia">Volte e tente novamente.</NuxtLink>
      </div>
      <form v-else @submit.prevent="cadastrar" autocomplete="off">
        <div class="cgg-grid">
          <div class="cgg-field cgg-field--full">
            <label class="cgg-label">Nome completo</label>
            <input v-model="form.nome" type="text" class="qs-input" required :disabled="loading" />
          </div>
          <div class="cgg-field">
            <label class="cgg-label">Login</label>
            <input v-model="form.login" type="text" class="qs-input" required :disabled="loading" />
          </div>
          <div class="cgg-field">
            <label class="cgg-label">CPF</label>
            <input v-model="form.documento" type="text" class="qs-input" maxlength="14" required :disabled="loading" />
          </div>
          <div class="cgg-field cgg-field--full">
            <label class="cgg-label">Celular</label>
            <input v-model="form.celular" type="text" class="qs-input" :disabled="loading" />
          </div>
          <div class="cgg-field cgg-field--full">
            <label class="cgg-label">Código do patrocinador</label>
            <input v-model="form.loginPatrocinador" type="text" class="qs-input" required :disabled="loading" />
          </div>
          <div class="cgg-field">
            <label class="cgg-label">Senha (opcional)</label>
            <input v-model="form.senha" type="password" class="qs-input" :disabled="loading" />
          </div>
          <div class="cgg-field">
            <label class="cgg-label">Confirmar senha</label>
            <input v-model="form.confirmarSenha" type="password" class="qs-input" :disabled="loading" />
          </div>
        </div>
        <div v-if="errorMsg" class="qs-alert-danger" style="font-size:.875rem">{{ errorMsg }}</div>
        <div v-if="successMsg" class="qs-alert-success" style="font-size:.875rem">{{ successMsg }}</div>
        <button type="submit" class="btn-login" :disabled="loading">
          <span v-if="loading" class="cg-spinner" />
          {{ loading ? 'Aguarde...' : 'Criar conta' }}
        </button>
      </form>
      <div class="cgg-footer">
        <NuxtLink to="/agencia" style="font-size:.85rem; color:#2f7785">← Voltar ao login</NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-login' });
const api = useApi();
const route = useRoute();
const credential = ref('');
const loading = ref(false);
const errorMsg = ref('');
const successMsg = ref('');
const form = reactive({
  credential: '',
  nome: '',
  login: '',
  documento: '',
  celular: '',
  loginPatrocinador: String(route.query.indicador ?? ''),
  senha: '',
  confirmarSenha: '',
});

onMounted(() => {
  const stored = sessionStorage.getItem('google_credential_pending');
  if (stored) {
    credential.value = stored;
    form.credential = stored;
  }
});

async function cadastrar() {
  if (form.senha && form.senha !== form.confirmarSenha) {
    errorMsg.value = 'As senhas não conferem.';
    return;
  }
  loading.value = true;
  errorMsg.value = '';
  successMsg.value = '';
  try {
    await api.post('/usuario/registrarGoogleCredential', {
      credential: form.credential,
      nome: form.nome,
      login: form.login,
      documento: form.documento,
      celular: form.celular,
      loginPatrocinador: form.loginPatrocinador,
      senha: form.senha,
    });
    sessionStorage.removeItem('google_credential_pending');
    successMsg.value = 'Conta criada com sucesso! Faça login para continuar.';
    setTimeout(() => navigateTo('/agencia'), 2000);
  } catch (err: unknown) {
    errorMsg.value = extractApiErrorMessage(err, 'Não foi possível criar a conta. Tente novamente.');
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.cgg-title { text-align: center; margin-bottom: .375rem; color: #2f7785; font-size: 1.125rem; font-weight: 700; }
.cgg-sub { text-align: center; color: #6b7280; font-size: .875rem; margin-bottom: 1.25rem; }
.cgg-grid { display: grid; grid-template-columns: 1fr 1fr; gap: .625rem; margin-bottom: .75rem; }
@media (max-width: 480px) { .cgg-grid { grid-template-columns: 1fr; } }
.cgg-field { display: flex; flex-direction: column; }
.cgg-field--full { grid-column: 1 / -1; }
.cgg-label { font-size: .8125rem; font-weight: 600; color: #374151; margin-bottom: .375rem; }
.cgg-footer { text-align: center; margin-top: .875rem; }
.cg-spinner {
  display: inline-block;
  width: 14px; height: 14px;
  border: 2px solid rgba(255,255,255,.4);
  border-top-color: currentColor;
  border-radius: 50%;
  animation: cg-spin .7s linear infinite;
  vertical-align: middle;
  margin-right: 6px;
}
@keyframes cg-spin { to { transform: rotate(360deg); } }
</style>
