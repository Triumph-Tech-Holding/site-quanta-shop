<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin</div>
        <h1>Painel Administrativo</h1>
        <p>Gerencie a plataforma Quanta Shop</p>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-if="loadError" class="qs-alert-warn">
      Não foi possível carregar o resumo do painel. Os indicadores podem estar desatualizados.
    </div>

    <template v-if="!loading">
      <div class="qs-kpi-strip">
        <div class="qs-kpi-card" v-for="(s, i) in stats" :key="i">
          <div class="qs-kpi-icon">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M3 13h2v-2H3v2zm0 4h2v-2H3v2zm0-8h2V7H3v2zm4 4h14v-2H7v2zm0 4h14v-2H7v2zM7 7v2h14V7H7z"/></svg>
          </div>
          <div>
            <div class="qs-kpi-label">{{ s.label }}</div>
            <div class="qs-kpi-value">{{ s.valor }}</div>
          </div>
        </div>
      </div>

      <!-- Atalho discreto para o LAB (cockpit técnico interno) -->
      <NuxtLink to="/lab" class="qs-lab-shortcut">
        <span class="qs-lab-pill">LAB</span>
        <span class="qs-lab-shortcut-text">Cockpit técnico — backlog, sprints, arquitetura, Flow Standard e mais.</span>
        <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"/></svg>
      </NuxtLink>

      <div class="qs-nav-grid">
        <NuxtLink
          v-for="(link, i) in adminLinks"
          :key="i"
          :to="link.to"
          class="qs-nav-card"
        >
          <div class="qs-nav-icon">{{ link.icon }}</div>
          <div>
            <div class="qs-nav-label">{{ link.label }}</div>
            <div class="qs-nav-desc">{{ link.desc }}</div>
          </div>
        </NuxtLink>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const loadError = ref(false);
const stats = ref([
  { label: 'Total usuários', valor: '—' },
  { label: 'Compras pendentes', valor: '—' },
  { label: 'Saques pendentes', valor: '—' },
  { label: 'Suporte abertos', valor: '—' },
]);
const adminLinks = [
  { to: '/agencia/painel/admin/usuarios', label: 'Usuários', desc: 'Gerenciar usuários cadastrados', icon: '👤' },
  { to: '/agencia/painel/admin/pagamentos', label: 'Pagamentos', desc: 'Aprovar e gerenciar pagamentos', icon: '💳' },
  { to: '/agencia/painel/admin/compras', label: 'Compras', desc: 'Listar e aprovar compras', icon: '🛒' },
  { to: '/agencia/painel/admin/credenciamento', label: 'Credenciamento', desc: 'Credenciar novos parceiros', icon: '🏪' },
  { to: '/agencia/painel/admin/categorias', label: 'Categorias', desc: 'Gerenciar categorias de lojas', icon: '🏷️' },
  { to: '/agencia/painel/admin/ecossistemas', label: 'Ecossistemas', desc: 'Gerenciar ecossistemas', icon: '🌐' },
  { to: '/agencia/painel/admin/carrosseis', label: 'Carrosséis', desc: 'Banners e carrosséis do portal', icon: '🖼️' },
  { to: '/agencia/painel/admin/comunicados', label: 'Comunicados', desc: 'Comunicados aos usuários', icon: '📢' },
  { to: '/agencia/painel/admin/rede', label: 'Rede', desc: 'Visualizar rede de usuários', icon: '🕸️' },
  { to: '/agencia/painel/admin/suporte', label: 'Suporte', desc: 'Gerenciar tickets de suporte', icon: '💬' },
  { to: '/agencia/painel/admin/lojas-credenciados', label: 'Lojas Físicas', desc: 'Lojas físicas credenciadas', icon: '📍' },
  { to: '/agencia/painel/admin/relatorio-de-faturas', label: 'Relatório de Faturas', desc: 'Relatórios financeiros', icon: '📄' },
  { to: '/agencia/painel/admin/home-cms', label: 'CMS da Home', desc: 'Editar textos e seções da página inicial', icon: '✏️' },
  { to: '/agencia/painel/admin/marcas-home', label: 'Marcas da Home', desc: 'Gerenciar logos do carrossel de marcas', icon: '🏅' },
  { to: '/agencia/painel/admin/blog', label: 'Blog', desc: 'Criar e gerenciar artigos do blog', icon: '📝' },
  { to: '/agencia/painel/admin/redes-sociais', label: 'Redes Sociais', desc: 'Posts do Instagram, YouTube, TikTok e mais', icon: '📱' },
  { to: '/agencia/painel/admin/docs', label: 'Documentação Técnica', desc: 'Arquitetura, padrões, decisões e histórico do projeto', icon: '📋' },
  { to: '/agencia/painel/admin/progresso', label: 'Progresso do Produto', desc: 'Acompanhe o andamento das tarefas de desenvolvimento', icon: '🚀' },
  { to: '/agencia/painel/admin/features', label: 'Features & MVP', desc: 'Roadmap por fase, status e público', icon: '🎯' },
  { to: '/agencia/painel/admin/configuracoes-rede', label: 'Configurações de Rede', desc: 'Percentuais por nível, Quanta Points, quarentena', icon: '⚙️' },
  { to: '/agencia/painel/admin/bi-financeiro', label: 'BI Financeiro', desc: 'Faturamento por categoria, inadimplência, cashback reservado', icon: '📈' },
];

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  if (!agenciaStore.isAdmin) { navigateTo('/agencia/painel'); return; }
  try {
    const { data } = await api.get('/admin/painel/resumo', authHeader());
    if (data) {
      stats.value[0].valor = String(data.totalUsuarios ?? '—');
      stats.value[1].valor = String(data.comprasPendentes ?? '—');
      stats.value[2].valor = String(data.saquesPendentes ?? '—');
      stats.value[3].valor = String(data.suporteAbertos ?? '—');
    }
  } catch (e) {
    console.error('[admin/index] Falha ao carregar resumo do painel:', e);
    loadError.value = true;
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.qs-alert-warn { background: #fefce8; color: #a16207; border: 1px solid #fde68a; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-bottom: 20px; }

.qs-kpi-strip { display: grid; grid-template-columns: repeat(4, 1fr); gap: 16px; margin-bottom: 28px; }
@media (max-width: 768px) { .qs-kpi-strip { grid-template-columns: repeat(2, 1fr); } }

.qs-kpi-card { background: #fff; border-radius: var(--qs-radius-lg); padding: 18px 20px; box-shadow: var(--qs-shadow-xs); display: flex; align-items: center; gap: 14px; border: 1px solid var(--qs-gray-100); }
.qs-kpi-icon { width: 40px; height: 40px; border-radius: var(--qs-radius-md); background: rgba(47,119,133,.1); display: flex; align-items: center; justify-content: center; flex-shrink: 0; }
.qs-kpi-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); margin-bottom: 2px; }
.qs-kpi-value { font-size: 22px; font-weight: 700; color: var(--qs-ink); letter-spacing: -0.02em; font-variant-numeric: tabular-nums; }

.qs-nav-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 12px; }
@media (max-width: 900px) { .qs-nav-grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 560px) { .qs-nav-grid { grid-template-columns: 1fr; } }

.qs-nav-card { display: flex; align-items: center; gap: 14px; background: #fff; border-radius: var(--qs-radius-lg); padding: 16px 18px; box-shadow: var(--qs-shadow-xs); border: 1px solid var(--qs-gray-100); text-decoration: none; color: inherit; transition: box-shadow .2s, transform .2s; }
.qs-nav-card:hover { box-shadow: var(--qs-shadow-md); transform: translateY(-1px); }
.qs-nav-icon { font-size: 22px; width: 40px; height: 40px; background: var(--qs-gray-50); border-radius: var(--qs-radius-md); display: flex; align-items: center; justify-content: center; flex-shrink: 0; }
.qs-nav-label { font-size: 14px; font-weight: 600; color: var(--qs-ink); margin-bottom: 2px; }
.qs-nav-desc { font-size: 12px; color: var(--qs-gray-400); line-height: 1.3; }

/* ─── Atalho discreto para o LAB ─────────────────────────────────── */
.qs-lab-shortcut {
  display: flex; align-items: center; gap: 12px;
  padding: 10px 16px; margin-bottom: 24px;
  background: #fff; border: 1px solid var(--qs-gray-100);
  border-left: 3px solid var(--qs-teal);
  border-radius: var(--qs-radius-md);
  text-decoration: none; color: var(--qs-ink);
  font-size: 13px; transition: box-shadow .15s, transform .15s;
}
.qs-lab-shortcut:hover { box-shadow: var(--qs-shadow-sm); transform: translateY(-1px); }
.qs-lab-shortcut-text { flex: 1; color: var(--qs-gray-500, #6b7280); }
.qs-lab-pill {
  background: var(--qs-teal); color: #fff;
  font-size: 10px; font-weight: 700; letter-spacing: 0.1em;
  padding: 3px 9px; border-radius: 100px;
}
.qs-lab-shortcut svg { color: var(--qs-teal); flex-shrink: 0; }
</style>
