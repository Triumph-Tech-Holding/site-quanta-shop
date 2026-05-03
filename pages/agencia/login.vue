<template>
  <div class="agl-page">
    <div class="agl-split">

      <!-- ── Lado esquerdo (branding) ── -->
      <div class="agl-side agl-side--brand">
        <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="agl-brand-logo" />
        <h2 class="agl-brand-h2">Bem-vindo de volta!</h2>
        <p class="agl-brand-sub">Acesse seu painel e acompanhe seus ganhos em tempo real.</p>
        <div class="agl-brand-stats">
          <div class="agl-brand-stat">
            <span class="agl-brand-stat__val">+12 mil</span>
            <span class="agl-brand-stat__lbl">usuários ativos</span>
          </div>
          <div class="agl-brand-stat">
            <span class="agl-brand-stat__val">Até 30%</span>
            <span class="agl-brand-stat__lbl">cashback por compra</span>
          </div>
          <div class="agl-brand-stat">
            <span class="agl-brand-stat__val">+500</span>
            <span class="agl-brand-stat__lbl">lojas parceiras</span>
          </div>
        </div>
      </div>

      <!-- ── Formulário ── -->
      <div class="agl-side agl-side--form">
        <div class="agl-form-box">
          <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="agl-form-logo" />
          <h1 class="agl-form-h1">Entrar</h1>
          <p class="agl-form-sub">
            Não tem conta?
            <NuxtLink to="/agencia/cadastro" class="agl-link">Criar conta grátis</NuxtLink>
          </p>

          <!-- Social login -->
          <LoginSocial />

          <div class="agl-divider">
            <span>ou entre com seu login</span>
          </div>

          <form @submit.prevent="handleLogin" autocomplete="off">
            <div class="agl-field">
              <label class="agl-label">Login ou e-mail</label>
              <input
                v-model="loginForm.login"
                type="text"
                class="agl-input"
                placeholder="seu_login ou email@exemplo.com"
                required
                :disabled="loading"
              />
            </div>

            <div class="agl-field">
              <div class="agl-field-header">
                <label class="agl-label">Senha</label>
                <NuxtLink to="/agencia/recuperar-senha" class="agl-link agl-link--sm">Esqueci a senha</NuxtLink>
              </div>
              <div class="agl-input-wrap">
                <input
                  v-model="loginForm.senha"
                  :type="showPassword ? 'text' : 'password'"
                  class="agl-input"
                  placeholder="••••••••"
                  required
                  :disabled="loading"
                />
                <button type="button" class="agl-eye" @click="showPassword = !showPassword" tabindex="-1">
                  <svg v-if="showPassword" width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"/><line x1="1" y1="1" x2="23" y2="23"/></svg>
                  <svg v-else width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                </button>
              </div>
            </div>

            <div v-if="errorMsg" class="agl-alert agl-alert--danger">
              <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
              {{ errorMsg }}
            </div>

            <button type="submit" class="agl-btn-submit" :disabled="loading">
              <span v-if="loading" class="agl-spinner" />
              {{ loading ? 'Aguarde...' : 'Entrar' }}
            </button>
          </form>

          <div class="agl-footer-links">
            <NuxtLink to="/" class="agl-link agl-link--sm">← Voltar ao início</NuxtLink>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import LoginSocial from '~/components/login/login-social.vue';

definePageMeta({ layout: 'agencia-login' });

const agenciaStore = useAgenciaStore();
const api = useApi();

const loginForm = reactive({ login: '', senha: '' });
const loading = ref(false);
const errorMsg = ref('');
const showPassword = ref(false);

onMounted(() => {
  try {
    const raw = localStorage.getItem('agencia_user');
    if (raw) {
      const parsed = JSON.parse(raw);
      if (!parsed?.token) localStorage.removeItem('agencia_user');
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

<style scoped>
.agl-page {
  min-height: 100vh;
  background: #ecf2f7;
  display: flex;
  align-items: stretch;
}
.agl-split {
  display: grid;
  grid-template-columns: 1fr 1fr;
  width: 100%;
  min-height: 100vh;
}
@media (max-width: 768px) {
  .agl-split { grid-template-columns: 1fr; }
  .agl-side--brand { display: none; }
}

/* Brand side */
.agl-side--brand {
  background: linear-gradient(135deg, #0f2e35 0%, #225f6b 100%);
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 60px 56px;
  position: relative;
  overflow: hidden;
}
.agl-side--brand::before {
  content: '';
  position: absolute;
  top: -20%;
  right: -20%;
  width: 500px; height: 500px;
  border-radius: 50%;
  background: radial-gradient(circle, rgba(152,199,58,0.08) 0%, transparent 70%);
  pointer-events: none;
}
.agl-brand-logo {
  width: 140px;
  margin-bottom: 40px;
  filter: brightness(0) invert(1);
  opacity: 0.9;
}
.agl-brand-h2 {
  font-size: clamp(28px, 3vw, 40px);
  font-weight: 900;
  color: #fff;
  letter-spacing: -0.03em;
  margin-bottom: 12px;
}
.agl-brand-sub {
  font-size: 16px;
  color: rgba(255,255,255,0.65);
  line-height: 1.65;
  margin-bottom: 48px;
  max-width: 340px;
}
.agl-brand-stats { display: flex; flex-direction: column; gap: 20px; }
.agl-brand-stat { display: flex; flex-direction: column; gap: 2px; }
.agl-brand-stat__val { font-size: 22px; font-weight: 800; color: #98c73a; }
.agl-brand-stat__lbl { font-size: 13px; color: rgba(255,255,255,0.5); }

/* Form side */
.agl-side--form {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px 24px;
  background: #fff;
}
.agl-form-box {
  width: 100%;
  max-width: 400px;
}
.agl-form-logo {
  display: block;
  width: 130px;
  margin: 0 auto 28px;
}
.agl-form-h1 {
  font-size: 28px;
  font-weight: 800;
  color: #225f6b;
  letter-spacing: -0.02em;
  margin-bottom: 6px;
  text-align: center;
}
.agl-form-sub {
  font-size: 14px;
  color: #6b7280;
  text-align: center;
  margin-bottom: 24px;
}
.agl-divider {
  display: flex;
  align-items: center;
  gap: 12px;
  margin: 20px 0;
  color: #9ca3af;
  font-size: 13px;
}
.agl-divider::before,
.agl-divider::after {
  content: '';
  flex: 1;
  height: 1px;
  background: #e5e7eb;
}

/* Fields */
.agl-field { margin-bottom: 16px; }
.agl-field-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 6px; }
.agl-label { display: block; font-size: 13px; font-weight: 600; color: #374151; margin-bottom: 6px; }
.agl-input-wrap { position: relative; }
.agl-input {
  width: 100%;
  padding: 12px 16px;
  border: 1.5px solid #d1d5db;
  border-radius: 10px;
  font-size: 15px;
  color: #111;
  background: #f9fafb;
  box-sizing: border-box;
  transition: border-color 0.2s, box-shadow 0.2s;
  outline: none;
}
.agl-input:focus { border-color: #2f7785; box-shadow: 0 0 0 3px rgba(47,119,133,0.12); background: #fff; }
.agl-input:disabled { opacity: 0.6; cursor: not-allowed; }
.agl-input-wrap .agl-input { padding-right: 44px; }
.agl-eye {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  cursor: pointer;
  color: #9ca3af;
  padding: 4px;
  display: flex;
  align-items: center;
}
.agl-eye:hover { color: #2f7785; }

/* Alert */
.agl-alert {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 14px;
  border-radius: 8px;
  font-size: 13.5px;
  margin-bottom: 16px;
  font-weight: 500;
}
.agl-alert--danger { background: #fee2e2; color: #dc2626; }

/* Submit */
.agl-btn-submit {
  width: 100%;
  background: linear-gradient(135deg, #225f6b, #2f7785);
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 14px;
  font-size: 15px;
  font-weight: 700;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  transition: all 0.2s;
  margin-top: 4px;
}
.agl-btn-submit:hover:not(:disabled) { transform: translateY(-1px); box-shadow: 0 6px 20px rgba(34,95,107,0.35); }
.agl-btn-submit:disabled { opacity: 0.6; cursor: not-allowed; }

.agl-spinner {
  width: 16px; height: 16px;
  border: 2px solid rgba(255,255,255,0.4);
  border-top-color: #fff;
  border-radius: 50%;
  animation: agl-spin 0.7s linear infinite;
}
@keyframes agl-spin { to { transform: rotate(360deg); } }

/* Links */
.agl-link { color: #2f7785; font-weight: 600; text-decoration: none; }
.agl-link:hover { color: #98c73a; text-decoration: underline; }
.agl-link--sm { font-size: 13px; }

.agl-footer-links {
  text-align: center;
  margin-top: 24px;
}
</style>
