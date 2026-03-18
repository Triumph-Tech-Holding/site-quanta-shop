<template>
  <div class="ag-wrapper">
    <div class="ag-role-bar">
      <div class="ag-role-bar-inner">
        <span class="ag-role-label">
          Acessando como
          <strong>{{ perfilLabel }}</strong>
        </span>
        <div class="ag-role-bar-right">
          <span v-if="isAcessoRemoto" class="ag-acesso-remoto-badge">Acesso remoto</span>
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="rgba(255,255,255,0.7)"><path d="M12 22c1.1 0 2-.9 2-2h-4c0 1.1.9 2 2 2zm6-6v-5c0-3.07-1.64-5.64-4.5-6.32V4c0-.83-.67-1.5-1.5-1.5s-1.5.67-1.5 1.5v.68C7.63 5.36 6 7.92 6 11v5l-2 2v1h16v-1l-2-2z"/></svg>
        </div>
      </div>
    </div>

    <div class="ag-account-bar">
      <div class="ag-account-bar-left">
        <div class="ag-account-avatar">{{ userInitial }}</div>
        <div class="ag-account-info">
          <div class="ag-account-name">{{ user?.username }}</div>
          <div class="ag-account-plan">{{ plano }}</div>
          <div class="ag-account-saldo">{{ formatCurrency(saldo) }}</div>
        </div>
      </div>
      <div class="ag-account-bar-actions">
        <button class="ag-btn-action ag-btn-action-primary" @click="goToAssistente">
          <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2z"/></svg>
          Assistente virtual
        </button>
        <NuxtLink to="/agencia/painel/meus-dados" class="ag-btn-action">Meus dados</NuxtLink>
        <button class="ag-btn-action ag-btn-action-outline" @click="doLogout">Logout</button>
      </div>
    </div>

    <div class="ag-body">
      <AgenciaMenu :is-open="sidebarOpen" @close="sidebarOpen = false" />
      <div v-if="sidebarOpen" class="ag-sidebar-overlay" @click="sidebarOpen = false" />

      <div class="ag-main">
        <div class="ag-mobile-topbar">
          <button class="btn-hamburger" @click="sidebarOpen = !sidebarOpen" aria-label="Menu">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 24 24"><path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"/></svg>
          </button>
          <span class="ag-mobile-title">{{ pageTitle }}</span>
        </div>

        <div class="ag-content">
          <slot />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const agenciaStore = useAgenciaStore();
const api = useApi();
const route = useRoute();

const sidebarOpen = ref(false);
const user = computed(() => agenciaStore.dadosUser);
const isAcessoRemoto = computed(() => agenciaStore.isAcessoRemoto);

const saldo = ref(0);
const plano = ref('');

const userInitial = computed(() => user.value?.username?.charAt(0)?.toUpperCase() || 'U');

const perfilLabel = computed(() => {
  const u = user.value;
  if (!u) return 'USUÁRIO';
  if (u.admin) return 'ADMINISTRADOR';
  if (u.comerciante && u.perfil === 'C') return 'COMERCIANTE';
  if (u.perfil === 'E') return 'EMPREENDEDOR';
  return 'USUÁRIO';
});

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

function goToAssistente() {
  window.open('https://api.whatsapp.com/send?phone=5521996983881&text=Preciso%20de%20atendimento', '_blank');
}

function doLogout() {
  agenciaStore.logout();
  window.location.href = '/agencia/login';
}

const pageTitles: Record<string, string> = {
  '/agencia/painel': 'Painel Geral',
  '/agencia/painel/meus-dados': 'Meus Dados',
  '/agencia/painel/minhas-compras': 'Minhas Compras',
  '/agencia/painel/minha-rede': 'Minha Rede',
  '/agencia/painel/financeiro': 'Financeiro',
  '/agencia/painel/assinatura': 'Assinatura',
  '/agencia/painel/planos': 'Planos',
  '/agencia/painel/contas-bancarias': 'Contas Bancárias',
  '/agencia/painel/suporte': 'Suporte',
  '/agencia/painel/meus-diretos': 'Meus Diretos',
  '/agencia/painel/minha-rede': 'Minha Rede',
  '/agencia/painel/meus-credenciamentos': 'Meus Credenciamentos',
  '/agencia/painel/desempenho': 'Performance',
  '/agencia/painel/performance': 'Performance',
  '/agencia/painel/faq': 'Perguntas Frequentes',
  '/agencia/painel/material-apoio': 'Material de Apoio',
  '/agencia/painel/tutoriais-usuario': 'Tutoriais',
  '/agencia/painel/inserir-cupom': 'Inserir Cupom',
  '/agencia/painel/admin': 'Painel Administrativo',
};

const pageTitle = computed(() => pageTitles[route.path] || 'Quanta Shop');

async function loadAccountData() {
  const token = agenciaStore.getToken();
  if (!token) return;
  const headers = { Authorization: `Bearer ${token}` };
  try {
    const { data } = await api.get('/Extrato/obterSaldoPorTipo', { headers });
    saldo.value = (data?.totalEntradas ?? 0) + (data?.totalSaidas ?? 0);
  } catch { /**/ }
  try {
    const { data } = await api.get('/Dashboard/obterBarraStatus', { headers });
    plano.value = data?.plano || data?.Plano || data?.produto || data?.Produto || '';
  } catch { /**/ }
}

onMounted(() => {
  agenciaStore.loadFromStorage();
  loadAccountData();
});
</script>
