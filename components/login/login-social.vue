<template>
  <div class="qs-login-social">
    <!-- Google -->
    <div class="qs-login-option">
      <div
        id="g_id_onload"
        :data-client_id="googleClientId"
        data-callback="handleGoogleCredential"
        data-auto_prompt="false"
      ></div>
      <div
        class="g_id_signin"
        data-type="standard"
        data-size="large"
        data-theme="outline"
        data-text="sign_in_with"
        data-shape="rectangular"
        data-logo_alignment="left"
      ></div>
    </div>

    <!-- Apple Sign In -->
    <button
      type="button"
      class="qs-login-apple"
      @click="handleAppleSignIn"
      :disabled="appleLoading"
    >
      <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">
        <path d="M17.05 20.28c-.98.95-2.05.8-3.08.35-1.09-.46-2.09-.48-3.24 0-1.44.62-2.2.44-3.06-.35C2.79 15.25 3.51 7.59 9.05 7.31c1.35.07 2.29.74 3.08.8 1.18-.24 2.31-.93 3.57-.84 1.51.12 2.65.72 3.4 1.8-3.12 1.87-2.38 5.98.48 7.13-.57 1.5-1.31 2.99-2.54 4.09l.01-.01zM12.03 7.25c-.15-2.23 1.66-4.07 3.74-4.25.29 2.58-2.34 4.5-3.74 4.25z"/>
      </svg>
      <span v-if="appleLoading">Conectando…</span>
      <span v-else>Entrar com Apple</span>
    </button>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useUserStore } from '@/pinia/useUserStore';
import { useRouter } from 'vue-router';

const config = useRuntimeConfig();
const googleClientId = config.public.googleClientId;
const appleClientId = (config.public as Record<string, unknown>).appleClientId as string | undefined;
const userStore = useUserStore();
const router = useRouter();
const appleLoading = ref(false);

onMounted(() => {
  // Google Identity Services
  const gscript = document.createElement('script');
  gscript.src = 'https://accounts.google.com/gsi/client';
  gscript.async = true;
  document.head.appendChild(gscript);

  (window as Window & { handleGoogleCredential?: (response: { credential: string }) => void }).handleGoogleCredential = async (response: { credential: string }) => {
    if (!response?.credential) return;
    try {
      const apiBase = config.public.apiBaseUrl;
      const res = await fetch(`${apiBase}/UsuarioLogin/autenticacaoGoogleCredential`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ credential: response.credential }),
      });
      if (!res.ok) {
        const err = await res.json().catch(() => ({}));
        const msg: string = err?.message || err?.errorCode || '';
        if (msg.includes('usuario_nao_encontrado') || res.status === 401) {
          sessionStorage.setItem('google_credential_pending', response.credential);
          router.push('/agencia/cadastro-google');
          return;
        }
        alert(err?.message || 'Erro ao autenticar com Google. Tente novamente.');
        return;
      }
      const data = await res.json();
      localStorage.setItem('user', JSON.stringify(data));
      localStorage.setItem('agencia_user', JSON.stringify(data));
      await userStore.loadUserFromStorage();
      router.push('/agencia/painel');
    } catch {
      alert('Erro ao autenticar com Google. Tente novamente.');
    }
  };

  // Apple Sign In JS SDK (carregado on-demand quando o botão é clicado)
});

async function handleAppleSignIn() {
  if (appleLoading.value) return;
  appleLoading.value = true;
  try {
    if (!appleClientId) {
      alert('Login com Apple ainda não está configurado. Em breve!');
      return;
    }
    const w = window as Window & { AppleID?: { auth: { init: (opts: Record<string, unknown>) => void; signIn: () => Promise<{ authorization: { id_token: string } }>; }; }; };
    if (!w.AppleID) {
      await new Promise<void>((resolve, reject) => {
        const s = document.createElement('script');
        s.src = 'https://appleid.cdn-apple.com/appleauth/static/jsapi/appleid/1/en_US/appleid.auth.js';
        s.async = true;
        s.onload = () => resolve();
        s.onerror = () => reject(new Error('Falha ao carregar Apple SDK'));
        document.head.appendChild(s);
      });
    }
    w.AppleID!.auth.init({
      clientId: appleClientId,
      scope: 'name email',
      redirectURI: `${window.location.origin}/login`,
      usePopup: true,
    });
    const result = await w.AppleID!.auth.signIn();
    const apiBase = config.public.apiBaseUrl;
    const res = await fetch(`${apiBase}/UsuarioLogin/autenticacaoAppleCredential`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ id_token: result.authorization.id_token }),
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      alert(err?.message || 'Erro ao autenticar com Apple.');
      return;
    }
    const data = await res.json();
    localStorage.setItem('user', JSON.stringify(data));
    localStorage.setItem('agencia_user', JSON.stringify(data));
    await userStore.loadUserFromStorage();
    router.push('/agencia/painel');
  } catch (e) {
    console.warn('[apple-signin]', e);
    alert('Não foi possível concluir o login com Apple.');
  } finally {
    appleLoading.value = false;
  }
}
</script>

<style scoped>
.qs-login-social {
  display: flex;
  flex-direction: column;
  gap: 10px;
  align-items: stretch;
  margin-bottom: 24px;
  max-width: 320px;
  margin-left: auto;
  margin-right: auto;
}
.qs-login-option {
  display: flex;
  justify-content: center;
}
.qs-login-apple {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  background: #000;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 11px 16px;
  font-family: inherit;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: background .2s, transform .1s;
  width: 100%;
  min-height: 44px;
}
.qs-login-apple:hover:not(:disabled) { background: #1a1a1a; }
.qs-login-apple:active { transform: scale(0.98); }
.qs-login-apple:disabled { opacity: .6; cursor: not-allowed; }
</style>
