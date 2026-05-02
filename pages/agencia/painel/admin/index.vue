<template>
  <div>
    <div class="ag-page-header"><h1>Painel Administrativo</h1><p>Gerencie a plataforma Quanta Shop</p></div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <template v-else>
      <div class="row g-3 mb-4">
        <div class="col-6 col-md-3" v-for="(s, i) in stats" :key="i">
          <div class="ag-stat-card">
            <div class="stat-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M3 13h2v-2H3v2zm0 4h2v-2H3v2zm0-8h2V7H3v2zm4 4h14v-2H7v2zm0 4h14v-2H7v2zM7 7v2h14V7H7z"/></svg>
            </div>
            <div>
              <div class="stat-label">{{ s.label }}</div>
              <div class="stat-value">{{ s.valor }}</div>
            </div>
          </div>
        </div>
      </div>

      <div class="row g-3">
        <div class="col-12 col-md-4" v-for="(link, i) in adminLinks" :key="i">
          <NuxtLink :to="link.to" class="ag-card d-flex align-items-center gap-3 text-decoration-none" style="color:inherit">
            <div class="stat-icon" style="flex-shrink:0">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M3 13h2v-2H3v2zm0 4h2v-2H3v2zm0-8h2V7H3v2zm4 4h14v-2H7v2zm0 4h14v-2H7v2zM7 7v2h14V7H7z"/></svg>
            </div>
            <div>
              <div class="fw-bold" style="font-size:.9rem">{{ link.label }}</div>
              <div class="text-muted" style="font-size:.8rem">{{ link.desc }}</div>
            </div>
          </NuxtLink>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const stats = ref([
  { label: 'Total usuários', valor: '—' },
  { label: 'Compras pendentes', valor: '—' },
  { label: 'Saques pendentes', valor: '—' },
  { label: 'Suporte abertos', valor: '—' },
]);
const adminLinks = [
  { to: '/agencia/painel/admin/usuarios', label: 'Usuários', desc: 'Gerenciar usuários cadastrados' },
  { to: '/agencia/painel/admin/pagamentos', label: 'Pagamentos', desc: 'Aprovar e gerenciar pagamentos' },
  { to: '/agencia/painel/admin/compras', label: 'Compras', desc: 'Listar e aprovar compras' },
  { to: '/agencia/painel/admin/credenciamento', label: 'Credenciamento', desc: 'Credenciar novos parceiros' },
  { to: '/agencia/painel/admin/categorias', label: 'Categorias', desc: 'Gerenciar categorias de lojas' },
  { to: '/agencia/painel/admin/ecossistemas', label: 'Ecossistemas', desc: 'Gerenciar ecossistemas' },
  { to: '/agencia/painel/admin/carrosseis', label: 'Carrosséis', desc: 'Banners e carrosséis do portal' },
  { to: '/agencia/painel/admin/comunicados', label: 'Comunicados', desc: 'Comunicados aos usuários' },
  { to: '/agencia/painel/admin/rede', label: 'Rede', desc: 'Visualizar rede de usuários' },
  { to: '/agencia/painel/admin/suporte', label: 'Suporte', desc: 'Gerenciar tickets de suporte' },
  { to: '/agencia/painel/admin/lojas-credenciados', label: 'Lojas Físicas', desc: 'Lojas físicas credenciadas' },
  { to: '/agencia/painel/admin/relatorio-de-faturas', label: 'Relatório de Faturas', desc: 'Relatórios financeiros' },
  { to: '/agencia/painel/admin/home-cms', label: 'CMS da Home', desc: 'Editar textos e seções da página inicial' },
  { to: '/agencia/painel/admin/marcas-home', label: 'Marcas da Home', desc: 'Gerenciar logos do carrossel de marcas' },
  { to: '/agencia/painel/admin/blog', label: 'Blog', desc: 'Criar e gerenciar artigos do blog' },
  { to: '/agencia/painel/admin/redes-sociais', label: 'Redes Sociais', desc: 'Posts do Instagram, YouTube, TikTok e mais' },
  { to: '/agencia/painel/admin/docs', label: '📋 Documentação Técnica', desc: 'Arquitetura, padrões, decisões e histórico do projeto' },
  { to: '/agencia/painel/admin/progresso', label: '🚀 Progresso do Produto', desc: 'Acompanhe o andamento das tarefas de desenvolvimento' },
  { to: '/agencia/painel/admin/features', label: '🎯 Features & MVP', desc: 'Roadmap por fase, status e público (visual Q Cuida + Premium)' },
  { to: '/agencia/painel/admin/configuracoes-rede', label: '⚙️ Configurações de Rede', desc: 'Percentuais por nível, ativação de camadas, Quanta Points, quarentena' },
  { to: '/agencia/painel/admin/bi-financeiro', label: '📈 BI Financeiro', desc: 'Faturamento por categoria, inadimplência, cashback reservado' },
];
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  if (!agenciaStore.isAdmin) { navigateTo('/agencia/painel'); return; }
  try {
    const { data } = await api.get('/admin/painel/resumo', authHeader());
    if (data) {
      stats.value[0].valor = data.totalUsuarios ?? '—';
      stats.value[1].valor = data.comprasPendentes ?? '—';
      stats.value[2].valor = data.saquesPendentes ?? '—';
      stats.value[3].valor = data.suporteAbertos ?? '—';
    }
  } catch { /**/ } finally { loading.value = false; }
});
</script>
