<template>
  <div
    class="tp-login-social mb-10 d-flex flex-wrap align-items-center justify-content-center"
  >
    <div class="tp-login-option-item has-google">
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
    <div class="tp-login-option-item d-none">
      <a href="#">
        <img src="/img/icon/login/facebook.svg" alt="" />
      </a>
    </div>
    <div class="tp-login-option-item d-none">
      <a href="#">
        <img class="apple" src="/img/icon/login/apple.svg" alt="" />
      </a>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useUserStore } from '@/pinia/useUserStore';
import { useRouter } from 'vue-router';

const config = useRuntimeConfig();
const googleClientId = config.public.googleClientId;
const userStore = useUserStore();
const router = useRouter();

onMounted(() => {
  const script = document.createElement('script');
  script.src = 'https://accounts.google.com/gsi/client';
  script.async = true;
  document.head.appendChild(script);

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
        alert(err?.message || 'Erro ao autenticar com Google. Verifique se seu cadastro já existe.');
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
});
</script>
