<template>
  <div class="agencia-login-page">
    <div class="login-box" style="max-width:520px">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h5 class="text-center mb-1" style="color:#2f7785">Cadastro com Google</h5>
      <p class="text-center text-muted mb-4" style="font-size:.875rem">
        Complete os dados abaixo para criar sua conta.
      </p>
      <div v-if="!credential" class="qs-alert-warn mb-3" style="font-size:.875rem">
        Sessão expirada. <NuxtLink to="/agencia">Volte e tente novamente.</NuxtLink>
      </div>
      <form v-else @submit.prevent="cadastrar" autocomplete="off">
        <div class="row g-2 mb-2">
          <div class="col-12 ag-form-group">
            <label>Nome completo</label>
            <input v-model="form.nome" type="text" class="form-control" required :disabled="loading" />
          </div>
          <div class="col-12 col-md-6 ag-form-group">
            <label>Login</label>
            <input v-model="form.login" type="text" class="form-control" required :disabled="loading" />
          </div>
          <div class="col-12 col-md-6 ag-form-group">
            <label>CPF</label>
            <input v-model="form.documento" type="text" class="form-control" maxlength="14" required :disabled="loading" />
          </div>
          <div class="col-12 ag-form-group">
            <label>Celular</label>
            <input v-model="form.celular" type="text" class="form-control" :disabled="loading" />
          </div>
          <div class="col-12 ag-form-group">
            <label>Código do patrocinador</label>
            <input v-model="form.loginPatrocinador" type="text" class="form-control" required :disabled="loading" />
          </div>
          <div class="col-12 col-md-6 ag-form-group">
            <label>Senha (opcional)</label>
            <input v-model="form.senha" type="password" class="form-control" :disabled="loading" />
          </div>
          <div class="col-12 col-md-6 ag-form-group">
            <label>Confirmar senha</label>
            <input v-model="form.confirmarSenha" type="password" class="form-control" :disabled="loading" />
          </div>
        </div>
        <div v-if="errorMsg" class="qs-alert-danger mb-3" style="font-size:.875rem">{{ errorMsg }}</div>
        <div v-if="successMsg" class="qs-alert-success mb-3" style="font-size:.875rem">{{ successMsg }}</div>
        <button type="submit" class="btn-login" :disabled="loading">
          <span v-if="loading" class="cg-spinner" />
          {{ loading ? 'Aguarde...' : 'Criar conta' }}
        </button>
      </form>
      <div class="text-center mt-3">
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
