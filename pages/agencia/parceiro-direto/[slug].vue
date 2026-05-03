<template>
  <div class="agencia-login-page" style="align-items:flex-start; padding:2rem 1rem; background:#ecf2f7; min-height:100vh">
    <div class="login-box" style="max-width:560px; margin:2rem auto">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div v-if="loading" class="text-center"><div class="qs-spinner" style="margin:16px auto" /></div>
      <template v-else-if="parceiro">
        <h3 class="text-center mt-3 mb-1" style="color:#2f7785">{{ parceiro.username || parceiro.nome }}</h3>
        <p class="text-center text-muted mb-4">te convida para a Quanta Shop</p>
        <div class="qs-card-section pd-invite-card text-center">
          <p class="mb-2">Com o Quanta Shop você ganha cashback em cada compra nas lojas credenciadas.</p>
          <p class="mb-0" style="font-size:.85rem; color: var(--qs-gray-500);">Cadastre-se gratuitamente e comece a economizar!</p>
        </div>
        <NuxtLink :to="`/agencia/cadastro?indicador=${route.params.slug}`" class="qs-btn-primary pd-btn-main">
          Cadastrar com indicação de {{ parceiro.username || parceiro.nome }}
        </NuxtLink>
        <div class="text-center mt-3">
          <NuxtLink to="/agencia/login" class="pd-link-login">Já tenho conta → Entrar</NuxtLink>
        </div>
      </template>
      <div v-else class="text-center mt-4">
        <p style="color: var(--qs-gray-500);">Parceiro não encontrado.</p>
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
.pd-invite-card { margin-bottom: 1.25rem; }
.pd-btn-main { display: block; width: 100%; text-align: center; margin-bottom: .75rem; }
.pd-link-login { font-size: .85rem; color: var(--qs-gray-400); text-decoration: none; }
.pd-link-login:hover { color: var(--qs-teal); }
</style>
