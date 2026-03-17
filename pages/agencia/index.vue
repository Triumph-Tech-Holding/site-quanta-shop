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
          <a href="https://quantashop.com.br/forgot" target="_blank">Esqueci minha senha</a>
          <a href="https://quantashop.com.br/register" target="_blank">Não tem cadastro?</a>
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
        <a href="https://quantashop.com.br" style="font-size:.8rem; color:#6c757d">
          ← Ir para página inicial
        </a>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';

definePageMeta({ layout: 'agencia-login' });

const agenciaStore = useAgenciaStore();
const api = useApi();
const router = useRouter();

const loginForm = reactive({ login: '', senha: '' });
const loading = ref(false);
const errorMsg = ref('');

onMounted(() => {
  agenciaStore.loadFromStorage();
  if (agenciaStore.isLoggedIn) {
    const u = agenciaStore.dadosUser;
    if (u?.admin) router.push('/agencia/painel/admin');
    else router.push('/agencia/painel');
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

      if (data.admin) router.push('/agencia/painel/admin');
      else router.push('/agencia/painel');
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
