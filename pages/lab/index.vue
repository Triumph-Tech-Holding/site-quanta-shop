<template>
  <div class="qs-page qs-lab">

    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">
          <span class="qs-lab-tag">LAB</span>
          Cockpit Técnico Interno · Quanta Tech
        </div>
        <h1>LAB — Engenharia &amp; Governança</h1>
        <p>Painel técnico interno com tudo sobre engenharia, documentação e progresso do app. <b>Visível somente para devs e equipe</b> — não aparece para o usuário final.</p>
      </div>
      <div class="qs-header-actions">
        <span class="qs-meta">v1.3.0 · Mai 2026</span>
        <button class="qs-btn-outline" @click="reload" :disabled="loading">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M17.65 6.35A7.958 7.958 0 0 0 12 4c-4.42 0-7.99 3.58-7.99 8s3.57 8 7.99 8c3.73 0 6.84-2.55 7.73-6h-2.08A5.99 5.99 0 0 1 12 18c-3.31 0-6-2.69-6-6s2.69-6 6-6c1.66 0 3.14.69 4.22 1.78L13 11h7V4l-2.35 2.35z"/></svg>
          Recarregar
        </button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner"></div></div>

    <template v-else>

      <!-- ─── KPIs do LAB ─────────────────────────────────────────────── -->
      <div class="qs-grid">
        <QsKpiCard
          label="Progresso global"
          :value="stats.donePct"
          suffix="%"
          :progress="stats.donePct"
          :meta="`${stats.done} de ${stats.total} features prontas`"
          dot-color="var(--qs-teal)"
        />
        <QsKpiCard
          label="Em curso"
          :value="stats.inProgress"
          :meta="`features ativas no momento`"
          dot-color="#f59e0b"
        />
        <QsKpiCard
          label="Backlog + Planejado"
          :value="stats.todo"
          :meta="`próximas entregas mapeadas`"
          dot-color="var(--qs-gray-300, #d1d5db)"
        />
        <QsKpiCard
          label="Versão atual"
          value="1.3.0"
          meta="Mai 2026 · Hardening & BI"
          dot-color="var(--qs-lime)"
        />
      </div>

      <!-- ─── Categorias do LAB ──────────────────────────────────────── -->
      <section
        v-for="cat in categories"
        :key="cat.id"
        class="qs-card-section"
      >
        <div class="qs-cat-head">
          <div class="qs-cat-icon" :style="{ background: cat.color + '15', color: cat.color }">{{ cat.icon }}</div>
          <div>
            <div class="qs-section-title">{{ cat.title }}</div>
            <div class="qs-section-desc">{{ cat.desc }}</div>
          </div>
        </div>

        <div class="qs-lab-grid">
          <component
            v-for="item in cat.items"
            :key="item.label"
            :is="item.disabled ? 'div' : (item.external ? 'a' : resolveComponent('NuxtLink'))"
            v-bind="linkProps(item)"
            class="qs-lab-card"
            :class="{ 'qs-lab-card--soon': item.disabled }"
          >
            <div class="qs-lab-card-icon">{{ item.icon }}</div>
            <div class="qs-lab-card-body">
              <div class="qs-lab-card-title">
                {{ item.label }}
                <span v-if="item.disabled" class="qs-lab-soon">em breve</span>
                <span v-else-if="item.badge" class="qs-lab-badge">{{ item.badge }}</span>
              </div>
              <div class="qs-lab-card-desc">{{ item.desc }}</div>
            </div>
            <svg v-if="!item.disabled" class="qs-lab-card-arrow" width="14" height="14" viewBox="0 0 24 24" fill="currentColor">
              <path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"/>
            </svg>
          </component>
        </div>
      </section>

      <!-- ─── Footer DoD ─────────────────────────────────────────────── -->
      <div class="qs-lab-footer-alert">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
        <span><b>DoD:</b> nenhuma funcionalidade é considerada concluída sem atualização do CHANGELOG e validação do Protocolo de Testes — vide <NuxtLink to="/lab/flow-standard">Flow Standard</NuxtLink>.</span>
      </div>

    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] })

const loading = ref(true)

interface Stats { done: number; inProgress: number; todo: number; total: number; donePct: number }
const stats = ref<Stats>({ done: 0, inProgress: 0, todo: 0, total: 0, donePct: 0 })

interface LabItem {
  label: string; desc: string; icon: string;
  to?: string; href?: string;
  external?: boolean; disabled?: boolean; badge?: string;
}
interface LabCategory { id: string; title: string; desc: string; icon: string; color: string; items: LabItem[] }

const categories: LabCategory[] = [
  {
    id: 'engenharia', title: 'Engenharia', icon: '⚙️', color: '#2F7785',
    desc: 'Backlog, sprints, kanban e registro de commits.',
    items: [
      { label: 'Backlog',   desc: 'Histórico priorizado de features e melhorias do produto.', icon: '📚', to: '/agencia/painel/admin/features', badge: 'features.json' },
      { label: 'Sprints',   desc: 'Planejamento e entrega por iteração. Em construção.', icon: '🏃', disabled: true },
      { label: 'Kanban',    desc: 'Quadro Todo / Em curso / Concluído derivado das tasks.', icon: '🗂️', to: '/agencia/painel/admin/progresso' },
      { label: 'Commits',   desc: 'CHANGELOG semântico — todas as versões e hotfixes.', icon: '📝', href: '/docs/CHANGELOG.md', external: true },
    ],
  },
  {
    id: 'produto', title: 'Produto', icon: '🎯', color: '#225F6B',
    desc: 'User stories, progresso por módulo e roadmap por fase.',
    items: [
      { label: 'Histórias',    desc: 'User stories detalhadas com critérios de aceitação.', icon: '📖', href: '/docs/FEATURES.md', external: true },
      { label: 'Progresso',    desc: 'Andamento real das tarefas de desenvolvimento.', icon: '🚀', to: '/agencia/painel/admin/progresso' },
      { label: 'Features & MVP', desc: 'Roadmap por fase, status, público e drill-down.', icon: '🎯', to: '/agencia/painel/admin/features' },
      { label: 'Fluxograma do app', desc: 'Diagrama de fluxos principais. Em construção.', icon: '🗺️', disabled: true },
    ],
  },
  {
    id: 'arquitetura', title: 'Arquitetura', icon: '🏛️', color: '#98C73A',
    desc: 'Stack, módulos, dependências e desenho do sistema.',
    items: [
      { label: 'CLAUDE.md',         desc: 'Contexto técnico permanente: stack, padrões, regras invioláveis.', icon: '🧠', href: '/docs/CLAUDE.md', external: true },
      { label: 'Documentação Técnica', desc: 'Visualizador interno (markdown + busca + PDF).', icon: '📋', to: '/agencia/painel/admin/docs' },
      { label: 'Configurações de Rede', desc: 'Splits 50/25/25, 12 níveis, Quanta Points, quarentena.', icon: '⚙️', to: '/agencia/painel/admin/configuracoes-rede' },
      { label: 'Dicionário de Dados', desc: 'Tabelas, relacionamentos e schemas de API. Em construção.', icon: '🗄️', disabled: true },
    ],
  },
  {
    id: 'qualidade', title: 'Qualidade & Governança', icon: '✅', color: '#7c3aed',
    desc: 'Flow Standard, testes e Definition of Done.',
    items: [
      { label: 'Flow Standard',    desc: 'Checklist técnico FLOW DEVELOPMENT SYSTEMS — 5 seções.', icon: '✅', to: '/lab/flow-standard', badge: 'novo' },
      { label: 'Matriz de Testes E2E', desc: 'Cenários críticos e protocolos de regressão. Em construção.', icon: '🧪', disabled: true },
      { label: 'Relatórios admin',   desc: 'Lançamentos, Cashback, Anunciantes e Faturas (LGPD).', icon: '📊', to: '/agencia/painel/admin/relatorio-de-faturas' },
      { label: 'BI Financeiro',      desc: 'Dashboard analítico de faturamento, aging e cashback.', icon: '📈', to: '/agencia/painel/admin/bi-financeiro' },
    ],
  },
  {
    id: 'versionamento', title: 'Versionamento & Histórico', icon: '🏷️', color: '#0891b2',
    desc: 'CHANGELOG, FEATURES.md, dicionário de versões.',
    items: [
      { label: 'CHANGELOG.md',  desc: 'Versões e hotfixes em formato Keep a Changelog + SemVer.', icon: '📜', href: '/docs/CHANGELOG.md', external: true },
      { label: 'FEATURES.md',   desc: 'Narrativa de produto — features e dependências.', icon: '📓', href: '/docs/FEATURES.md', external: true },
      { label: 'features.json', desc: 'Fonte de verdade do roadmap (24 features mapeadas).', icon: '🔖', href: '/docs/features.json', external: true },
      { label: 'DATA_DICTIONARY', desc: 'Dicionário de dados versionado. Em construção.', icon: '📘', disabled: true },
    ],
  },
]

function linkProps(item: LabItem) {
  if (item.disabled) return {}
  if (item.external && item.href) return { href: item.href, target: '_blank', rel: 'noopener' }
  if (item.to) return { to: item.to }
  return {}
}

async function loadStats() {
  try {
    const j = await $fetch<any>('/docs/features.json', { cache: 'no-store' })
    const feats = j?.features ?? []
    const done = feats.filter((f: any) => f.status === 'done').length
    const inProgress = feats.filter((f: any) => f.status === 'in_progress').length
    const todo = feats.filter((f: any) => f.status === 'planned' || f.status === 'backlog').length
    const total = feats.length
    stats.value = { done, inProgress, todo, total, donePct: total ? Math.round(done / total * 100) : 0 }
  } catch { /**/ }
}

async function reload() {
  loading.value = true
  await loadStats()
  loading.value = false
}

onMounted(reload)
</script>

<style scoped>
.qs-header-text h1 { font-size: 1.6rem; font-weight: 700; color: var(--qs-ink); margin: .25rem 0 .4rem; }
.qs-header-text p  { font-size: .9rem; color: var(--qs-gray-400); margin: 0; max-width: 720px; }
.qs-header-text p b { color: var(--qs-teal); }
.qs-header-actions { display: flex; align-items: center; gap: .75rem; flex-wrap: wrap; }

.qs-eyebrow { display: flex; align-items: center; gap: .5rem; }
.qs-lab-tag {
  background: var(--qs-teal); color: #fff;
  font-size: 9px; font-weight: 700; letter-spacing: .12em;
  padding: 3px 8px; border-radius: 100px;
}

.qs-cat-head { display: flex; align-items: flex-start; gap: 1rem; margin-bottom: 1.25rem; }
.qs-cat-icon {
  width: 40px; height: 40px; border-radius: 10px;
  display: flex; align-items: center; justify-content: center;
  font-size: 1.1rem; flex-shrink: 0;
}

.qs-lab-grid {
  display: grid; grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: .75rem;
}

.qs-lab-card {
  display: flex; align-items: flex-start; gap: .75rem;
  padding: .85rem 1rem;
  background: var(--qs-gray-50, #f8f9fa);
  border: 1px solid var(--qs-gray-100, #e9ecef);
  border-radius: 10px;
  text-decoration: none; color: inherit;
  transition: transform .15s, box-shadow .15s, border-color .15s;
  position: relative;
}
.qs-lab-card:hover:not(.qs-lab-card--soon) {
  background: #fff; transform: translateY(-1px);
  box-shadow: var(--qs-shadow-sm); border-color: var(--qs-teal);
}
.qs-lab-card--soon { opacity: .6; cursor: not-allowed; }
.qs-lab-card-icon { font-size: 1.2rem; flex-shrink: 0; line-height: 1; padding-top: 2px; }
.qs-lab-card-body { flex: 1; min-width: 0; }
.qs-lab-card-title {
  font-size: .88rem; font-weight: 600; color: var(--qs-ink);
  margin-bottom: .3rem; display: flex; align-items: center; gap: .4rem; flex-wrap: wrap;
}
.qs-lab-card-desc { font-size: .78rem; color: var(--qs-gray-400); line-height: 1.4; }
.qs-lab-card-arrow { color: var(--qs-gray-300, #d1d5db); flex-shrink: 0; align-self: center; }
.qs-lab-card:hover:not(.qs-lab-card--soon) .qs-lab-card-arrow { color: var(--qs-teal); }

.qs-lab-soon {
  font-size: 9px; font-weight: 700; letter-spacing: .08em;
  background: var(--qs-gray-100, #f3f4f6); color: var(--qs-gray-400, #6b7280);
  padding: 2px 7px; border-radius: 100px; text-transform: uppercase;
}
.qs-lab-badge {
  font-size: 9px; font-weight: 700; letter-spacing: .08em;
  background: var(--qs-lime, #98C73A); color: #fff;
  padding: 2px 7px; border-radius: 100px; text-transform: uppercase;
}

.qs-lab-footer-alert {
  display: flex; align-items: center; gap: .6rem;
  background: #fef3c7; border: 1px solid #fde68a; border-radius: 8px;
  padding: .85rem 1rem; font-size: .82rem; color: #78350f;
  margin-top: .5rem;
}
.qs-lab-footer-alert svg { fill: #92400e; flex-shrink: 0; }
.qs-lab-footer-alert b { color: #78350f; }
.qs-lab-footer-alert a { color: var(--qs-teal); font-weight: 600; }

.qs-meta { font-size: .82rem; color: var(--qs-gray-400); }
</style>
