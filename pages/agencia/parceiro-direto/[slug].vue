<template>
  <div class="agencia-login-page" style="align-items:flex-start; padding:2rem 1rem; background:#ecf2f7; min-height:100vh">
    <div class="login-box" style="max-width:560px; margin:2rem auto">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div v-if="loading" class="pd-loading"><div class="qs-spinner" /></div>
      <template v-else-if="parceiro">
        <h3 class="pd-title">{{ parceiro.username || parceiro.nome }}</h3>
        <p class="pd-sub">te convida para a Quanta Shop</p>
        <div class="qs-card-section pd-invite-card">
          <p class="pd-p">Com o Quanta Shop você ganha cashback em cada compra nas lojas credenciadas.</p>
          <p class="pd-p pd-p--sm">Cadastre-se gratuitamente e comece a economizar!</p>
        </div>
        <NuxtLink :to="`/agencia/cadastro?indicador=${route.params.slug}`" class="qs-btn-primary pd-btn-main">
          Cadastrar com indicação de {{ parceiro.username || parceiro.nome }}
        </NuxtLink>
        <div class="pd-login-link-wrap">
          <NuxtLink to="/agencia/login" class="pd-link-login">Já tenho conta → Entrar</NuxtLink>
        </div>
      </template>
      <div v-else class="pd-not-found">
        <p class="pd-not-found__msg">Parceiro não encontrado.</p>
        <NuxtLink to="/agencia/cadastro" class="qs-btn-primary">Cadastrar mesmo assim</NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-login' });
const route = useRoute();
const api = useApi();
const loading = ref(true);
interface Parceiro { username?: string; nome?: string; [key: string]: unknown; }
const parceiro = ref<Parceiro | null>(null);
onMounted(async () => {
  try {
    const { data } = await api.get<Parceiro>(`/geral/parceiroDireto/${route.params.slug}`);
    parceiro.value = data ?? null;
  } catch (err: unknown) {
    console.error('Parceiro não encontrado:', extractApiErrorMessage(err));
  } finally { loading.value = false; }
});
</script>

<style scoped>
.pd-loading { display: flex; justify-content: center; padding: 1.5rem 0; }
.pd-title { text-align: center; color: #2f7785; font-size: 1.25rem; font-weight: 700; margin: 1rem 0 .25rem; }
.pd-sub { text-align: center; color: var(--qs-gray-500, #6b7280); font-size: .875rem; margin: 0 0 1.25rem; }
.pd-invite-card { margin-bottom: 1.25rem; text-align: center; }
.pd-p { margin-bottom: .5rem; }
.pd-p--sm { font-size: .85rem; color: var(--qs-gray-500); margin-bottom: 0; }
.pd-btn-main { display: block; width: 100%; text-align: center; margin-bottom: .75rem; }
.pd-login-link-wrap { text-align: center; margin-top: .75rem; }
.pd-link-login { font-size: .85rem; color: var(--qs-gray-400); text-decoration: none; }
.pd-link-login:hover { color: var(--qs-teal); }
.pd-not-found { text-align: center; margin-top: 1.5rem; }
.pd-not-found__msg { color: var(--qs-gray-500); margin-bottom: 1rem; }
</style>
