<template>
  <div class="agencia-login-page">
    <div class="login-box" style="max-width:520px">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h5 class="text-center mb-4" style="color:#2f7785">Criar Conta</h5>
      <form @submit.prevent="cadastrar" autocomplete="off">
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
            <input v-model="form.cpf" type="text" class="form-control" maxlength="14" required :disabled="loading" />
          </div>
          <div class="col-12 ag-form-group">
            <label>E-mail</label>
            <input v-model="form.email" type="email" class="form-control" required :disabled="loading" />
          </div>
          <div class="col-12 col-md-6 ag-form-group">
            <label>Senha</label>
            <input v-model="form.senha" type="password" class="form-control" required :disabled="loading" />
          </div>
          <div class="col-12 col-md-6 ag-form-group">
            <label>Confirmar senha</label>
            <input v-model="form.confirmarSenha" type="password" class="form-control" required :disabled="loading" />
          </div>
          <div class="col-12 ag-form-group" v-if="indicadorSlug">
            <label>Código de indicação</label>
            <input v-model="form.loginIndicador" type="text" class="form-control" :disabled="loading" />
          </div>
        </div>
        <div v-if="errorMsg" class="alert alert-danger py-2 mb-3" style="font-size:.875rem">{{ errorMsg }}</div>
        <div v-if="successMsg" class="alert alert-success py-2 mb-3" style="font-size:.875rem">{{ successMsg }}</div>
        <button type="submit" class="btn-login" :disabled="loading">
          <span v-if="loading" class="spinner-border spinner-border-sm me-2" />
          {{ loading ? 'Aguarde...' : 'Cadastrar' }}
        </button>
      </form>
      <div class="text-center mt-3">
        <NuxtLink to="/agencia" style="font-size:.85rem; color:#2f7785">← Já tenho conta</NuxtLink>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-login' });
const route = useRoute();
const api = useApi();
const indicadorSlug = computed(() => String(route.query.indicador ?? ''));
const form = reactive({
  nome: '', login: '', cpf: '', email: '', senha: '', confirmarSenha: '',
  loginIndicador: indicadorSlug.value,
});
const loading = ref(false);
const errorMsg = ref('');
const successMsg = ref('');
async function cadastrar() {
  if (form.senha !== form.confirmarSenha) { errorMsg.value = 'As senhas não conferem.'; return; }
  loading.value = true; errorMsg.value = ''; successMsg.value = '';
  try {
    await api.post('/UsuarioLogin/cadastro', form);
    successMsg.value = 'Cadastro realizado! Verifique seu e-mail para ativar a conta.';
  } catch (err: unknown) {
    errorMsg.value = extractApiErrorMessage(err, 'Não foi possível criar a conta. Tente novamente.');
  } finally { loading.value = false; }
}
</script>
