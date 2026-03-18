<template>
  <div class="agencia-login-page">
    <div class="login-box">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />

      <form @submit.prevent="handleLogin" autocomplete="off">
        <div class="mb-3">
          <input
            v-model="loginForm.login"
            type="text"
            class="form-control"
            placeholder="Login ou e-mail"
            required
            :disabled="loading"
          />
        </div>

        <div class="mb-3">
          <input
            v-model="loginForm.senha"
            type="password"
            class="form-control"
            placeholder="Senha"
            required
            :disabled="loading"
          />
        </div>

        <div class="d-flex justify-content-between mb-4" style="font-size:.85rem">
          <NuxtLink to="/agencia/recuperar-senha">Esqueci minha senha</NuxtLink>
          <NuxtLink to="/agencia/cadastro">Não tem cadastro?</NuxtLink>
        </div>

        <div v-if="errorMsg" class="alert alert-danger py-2 mb-3" style="font-size:.875rem">
          {{ errorMsg }}
        </div>

        <button type="submit" class="btn-login" :disabled="loading">
          <span v-if="loading" class="spinner-border spinner-border-sm me-2" />
          {{ loading ? 'Aguarde...' : 'Acessar' }}
        </button>
      </form>

      <div class="text-center mt-4">
        <NuxtLink to="/" style="font-size:.8rem; color:#6c757d">
          ← Ir para página inicial
        </NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';

definePageMeta({ layout: 'agencia-login' });

const agenciaStore = useAgenciaStore();
const api = useApi();

const loginForm = reactive({ login: '', senha: '' });
const loading = ref(false);
const errorMsg = ref('');

onMounted(() => {
  try {
    const raw = localStorage.getItem('agencia_user');
    if (raw) {
      const parsed = JSON.parse(raw);
      if (!parsed?.token) {
        localStorage.removeItem('agencia_user');
      }
    }
  } catch {
    localStorage.removeItem('agencia_user');
  }
  agenciaStore.loadFromStorage();
  if (agenciaStore.isLoggedIn && agenciaStore.checkTokenExpiry()) {
    const u = agenciaStore.dadosUser;
    window.location.href = u?.admin ? '/agencia/painel/admin' : '/agencia/painel';
  }
});

async function handleLogin() {
  loading.value = true;
  errorMsg.value = '';
  try {
    const { data } = await api.post('/UsuarioLogin/autenticacao', {
      login: loginForm.login,
      senha: loginForm.senha,
    });

    if (data?.token) {
      agenciaStore.setUser(data);
      localStorage.setItem('agencia_showComunicado', 'true');
      localStorage.removeItem('agencia_menu');

      window.location.href = data.admin ? '/agencia/painel/admin' : '/agencia/painel';
    } else {
      errorMsg.value = 'Resposta inesperada do servidor. Tente novamente.';
    }
  } catch (err: unknown) {
    errorMsg.value = extractApiErrorMessage(err, 'Usuário ou senha inválidos. Tente novamente.');
  } finally {
    loading.value = false;
  }
}
</script>
