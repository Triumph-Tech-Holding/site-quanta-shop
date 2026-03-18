<template>
  <div class="ag-layout">
    <AgenciaMenu :is-open="sidebarOpen" @close="sidebarOpen = false" />

    <div v-if="sidebarOpen" class="ag-sidebar-overlay" @click="sidebarOpen = false" />

    <div class="ag-main">
      <div class="ag-topbar">
        <button class="btn-hamburger" @click="sidebarOpen = !sidebarOpen" aria-label="Menu">
          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 24 24"><path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"/></svg>
        </button>

        <p class="page-title">{{ pageTitle }}</p>

        <div class="ag-topbar-actions">
          <span v-if="isAcessoRemoto" class="btn-acesso-remoto">Acesso remoto</span>
          <span v-if="user" class="d-none d-md-block text-muted" style="font-size:.85rem">
            Olá, {{ user.username }}
          </span>
        </div>
      </div>

      <div class="ag-content">
        <slot />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const agenciaStore = useAgenciaStore();
const route = useRoute();

const sidebarOpen = ref(false);
const user = computed(() => agenciaStore.dadosUser);
const isAcessoRemoto = computed(() => agenciaStore.isAcessoRemoto);

const pageTitles: Record<string, string> = {
  '/agencia/painel': 'Painel Geral',
  '/agencia/painel/meus-dados': 'Meus Dados',
  '/agencia/painel/minhas-compras': 'Minhas Compras',
  '/agencia/painel/minha-rede': 'Minha Rede',
  '/agencia/painel/financeiro': 'Financeiro',
  '/agencia/painel/assinatura': 'Assinatura',
  '/agencia/painel/planos': 'Planos',
  '/agencia/painel/graduacoes': 'Graduações',
  '/agencia/painel/cupons': 'Cupons',
  '/agencia/painel/performance': 'Performance',
  '/agencia/painel/contas-bancarias': 'Contas Bancárias',
  '/agencia/painel/suporte': 'Suporte',
  '/agencia/painel/meus-diretos': 'Meus Diretos',
  '/agencia/painel/faq': 'Perguntas Frequentes',
  '/agencia/painel/material-apoio': 'Material de Apoio',
  '/agencia/painel/meus-cupons': 'Meus Cupons',
  '/agencia/painel/gerar-cupons': 'Gerar Cupons',
  '/agencia/painel/inserir-cupom': 'Inserir Cupom',
  '/agencia/painel/admin': 'Painel Administrativo',
};

const pageTitle = computed(() => {
  return pageTitles[route.path] || 'Quanta Shop';
});

onMounted(() => {
  agenciaStore.loadFromStorage();
});
</script>
