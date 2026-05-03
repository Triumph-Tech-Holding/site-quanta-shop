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

      <!-- ─── Lab Flow Standard ───────────────────────────────────────── -->
      <section class="qs-lab-flow">
        <div class="qs-lab-header">
          <div class="qs-lab-title-row">
            <span class="qs-lab-pill">LAB</span>
            <h2 class="qs-lab-title">Flow Standard — Checklist Técnico</h2>
            <span class="qs-lab-version">v1.3.0 · Mai 2026</span>
          </div>
          <NuxtLink to="/agencia/painel/admin/flow-standard" class="qs-lab-cta">
            Abrir painel completo
            <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"/></svg>
          </NuxtLink>
        </div>
        <p class="qs-lab-desc">Protocolo FLOW DEVELOPMENT SYSTEMS — qualidade, performance e Clean Code aplicados ao projeto Quanta Shop.</p>

        <div class="qs-lab-sections">
          <NuxtLink
            v-for="s in flowSections"
            :key="s.key"
            :to="`/agencia/painel/admin/flow-standard`"
            class="qs-lab-section-card"
          >
            <div class="qs-lab-section-badge">§{{ s.num }}</div>
            <div class="qs-lab-section-content">
              <div class="qs-lab-section-title">{{ s.title }}</div>
              <div class="qs-lab-section-meta">
                <span class="qs-lab-status-dot" :class="s.statusClass"></span>
                <span class="qs-lab-status-text">{{ s.statusLabel }}</span>
              </div>
            </div>
          </NuxtLink>
        </div>

        <div class="qs-lab-progress" v-if="flowFeatures">
          <div class="qs-lab-progress-row">
            <div class="qs-lab-progress-label">
              <b>Painel de Status</b> — {{ flowFeatures.done }} de {{ flowFeatures.total }} features concluídas
            </div>
            <div class="qs-lab-progress-pct">{{ flowFeatures.pct }}%</div>
          </div>
          <div class="qs-lab-progress-track">
            <div class="qs-lab-progress-fill" :style="{ width: flowFeatures.pct + '%' }"></div>
          </div>
          <div class="qs-lab-mvp-row">
            <div v-for="m in flowFeatures.mvps" :key="m.id" class="qs-lab-mvp-chip" :style="{ borderColor: m.color, color: m.color }">
              {{ m.name }} · {{ m.pct }}%
            </div>
          </div>
        </div>

        <div class="qs-lab-footer">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
          <span><b>Atenção:</b> nenhuma funcionalidade é considerada concluída sem atualização do CHANGELOG e validação do Protocolo de Testes.</span>
        </div>
      </section>

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
  { to: '/agencia/painel/admin/flow-standard', label: 'Flow Standard', desc: 'Checklist técnico de qualidade, contexto e DoD', icon: '✅' },
];

// ─── Lab Flow Standard ─────────────────────────────────────────────
const flowSections = [
  { num: 1, key: 'intro',     title: 'Introdução',        statusClass: 'qs-status-done', statusLabel: 'Documentado' },
  { num: 2, key: 'contexto',  title: 'Contexto e Memória', statusClass: 'qs-status-done', statusLabel: '4 docs vivos' },
  { num: 3, key: 'gestao',    title: 'Gestão e Progresso', statusClass: 'qs-status-done', statusLabel: 'Matriz ativa' },
  { num: 4, key: 'setup',     title: 'Setup Técnico Replit', statusClass: 'qs-status-done', statusLabel: 'Configurado' },
  { num: 5, key: 'qualidade', title: 'Qualidade / DoD',   statusClass: 'qs-status-done', statusLabel: 'Aplicado' },
];
interface FlowMvp { id: string; name: string; color: string; pct: number }
interface FlowFeatures { done: number; total: number; pct: number; mvps: FlowMvp[] }
const flowFeatures = ref<FlowFeatures | null>(null);

async function loadFlowFeatures() {
  try {
    const j = await $fetch<any>('/docs/features.json', { cache: 'no-store' });
    const feats = j?.features ?? [];
    const mvps = j?.mvps ?? [];
    const total = feats.length;
    const done = feats.filter((f: any) => f.status === 'done').length;
    const pct = total ? Math.round(done / total * 100) : 0;
    const mvpStats = mvps.map((m: any) => {
      const arr = feats.filter((f: any) => f.mvp === m.id);
      const d = arr.filter((f: any) => f.status === 'done').length;
      return { id: m.id, name: m.name.split('—')[0].trim(), color: m.color, pct: arr.length ? Math.round(d / arr.length * 100) : 0 };
    });
    flowFeatures.value = { done, total, pct, mvps: mvpStats };
  } catch { /**/ }
}

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  if (!agenciaStore.isAdmin) { navigateTo('/agencia/painel'); return; }
  loadFlowFeatures();
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

/* ─── Lab Flow Standard ──────────────────────────────────────────── */
.qs-lab-flow {
  background: linear-gradient(135deg, #fff 0%, #f8fbfc 100%);
  border: 1px solid var(--qs-gray-100);
  border-left: 4px solid var(--qs-teal);
  border-radius: var(--qs-radius-lg);
  padding: 22px 24px;
  margin-bottom: 28px;
  box-shadow: var(--qs-shadow-xs);
}
.qs-lab-header { display: flex; justify-content: space-between; align-items: flex-start; gap: 16px; flex-wrap: wrap; }
.qs-lab-title-row { display: flex; align-items: center; gap: 12px; flex-wrap: wrap; }
.qs-lab-pill {
  background: var(--qs-teal); color: #fff;
  font-size: 10px; font-weight: 700; letter-spacing: 0.1em;
  padding: 3px 9px; border-radius: 100px;
}
.qs-lab-title { font-size: 18px; font-weight: 700; color: var(--qs-ink); margin: 0; letter-spacing: -0.01em; }
.qs-lab-version { font-size: 12px; color: var(--qs-gray-400); }
.qs-lab-cta {
  display: inline-flex; align-items: center; gap: 4px;
  font-size: 13px; font-weight: 600; color: var(--qs-teal);
  text-decoration: none; padding: 6px 12px;
  border: 1px solid var(--qs-teal); border-radius: 6px;
  transition: background .15s, color .15s;
}
.qs-lab-cta:hover { background: var(--qs-teal); color: #fff; }
.qs-lab-desc { font-size: 13px; color: var(--qs-gray-400); margin: 8px 0 16px; }

.qs-lab-sections {
  display: grid; grid-template-columns: repeat(5, 1fr);
  gap: 10px; margin-bottom: 18px;
}
@media (max-width: 900px) { .qs-lab-sections { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 480px) { .qs-lab-sections { grid-template-columns: 1fr; } }

.qs-lab-section-card {
  display: flex; align-items: center; gap: 10px;
  background: #fff; border: 1px solid var(--qs-gray-100);
  border-radius: 8px; padding: 10px 12px;
  text-decoration: none; color: inherit;
  transition: transform .15s, box-shadow .15s, border-color .15s;
}
.qs-lab-section-card:hover {
  transform: translateY(-1px); box-shadow: var(--qs-shadow-sm);
  border-color: var(--qs-teal);
}
.qs-lab-section-badge {
  width: 30px; height: 30px; border-radius: 6px;
  background: rgba(47,119,133,.1); color: var(--qs-teal);
  display: flex; align-items: center; justify-content: center;
  font-size: 11px; font-weight: 700; flex-shrink: 0;
}
.qs-lab-section-content { min-width: 0; flex: 1; }
.qs-lab-section-title { font-size: 12px; font-weight: 600; color: var(--qs-ink); line-height: 1.2; margin-bottom: 3px; }
.qs-lab-section-meta { display: flex; align-items: center; gap: 5px; }
.qs-lab-status-dot {
  width: 6px; height: 6px; border-radius: 100px; display: inline-block;
}
.qs-lab-status-dot.qs-status-done { background: var(--qs-lime, #98C73A); }
.qs-lab-status-text { font-size: 11px; color: var(--qs-gray-400); }

.qs-lab-progress {
  background: #fff; border: 1px solid var(--qs-gray-100);
  border-radius: 8px; padding: 12px 14px; margin-bottom: 12px;
}
.qs-lab-progress-row { display: flex; justify-content: space-between; align-items: center; gap: 12px; margin-bottom: 8px; }
.qs-lab-progress-label { font-size: 13px; color: var(--qs-gray-500, #6b7280); }
.qs-lab-progress-label b { color: var(--qs-ink); }
.qs-lab-progress-pct { font-size: 16px; font-weight: 700; color: var(--qs-teal); font-variant-numeric: tabular-nums; }
.qs-lab-progress-track { height: 6px; background: var(--qs-gray-100); border-radius: 100px; overflow: hidden; }
.qs-lab-progress-fill { height: 100%; background: linear-gradient(90deg, var(--qs-teal), var(--qs-lime, #98C73A)); border-radius: 100px; transition: width .4s; }
.qs-lab-mvp-row { display: flex; gap: 6px; flex-wrap: wrap; margin-top: 10px; }
.qs-lab-mvp-chip {
  font-size: 11px; font-weight: 600;
  padding: 3px 9px; border: 1px solid;
  border-radius: 100px; background: #fff;
}

.qs-lab-footer {
  display: flex; align-items: center; gap: 8px;
  background: #fef3c7; border: 1px solid #fde68a;
  border-radius: 6px; padding: 8px 12px;
  font-size: 12px; color: #78350f;
}
.qs-lab-footer svg { fill: #92400e; flex-shrink: 0; }
.qs-lab-footer b { color: #78350f; }
</style>
