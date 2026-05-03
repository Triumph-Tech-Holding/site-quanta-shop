<template>
  <div class="qs-page qs-flow">

    <QsPageHeader eyebrow="Admin · Gestão Técnica" title="Flow Standard — Checklist Técnico" description="Protocolo de excelência para desenvolvimento, qualidade e manutenção da plataforma.">
      <div class="qs-period-switch">
        <QsFilterChip v-for="s in sections" :key="s.key" :active="activeSection === s.key" @click="activeSection = s.key">{{ s.label }}</QsFilterChip>
      </div>
      <button class="qs-btn-outline" @click="reload" :disabled="loading">
        <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M17.65 6.35A7.958 7.958 0 0 0 12 4c-4.42 0-7.99 3.58-7.99 8s3.57 8 7.99 8c3.73 0 6.84-2.55 7.73-6h-2.08A5.99 5.99 0 0 1 12 18c-3.31 0-6-2.69-6-6s2.69-6 6-6c1.66 0 3.14.69 4.22 1.78L13 11h7V4l-2.35 2.35z"/></svg>
        Recarregar
      </button>
    </QsPageHeader>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner"></div></div>

    <template v-else>

      <!-- ─── KPIs ─────────────────────────────────────────────────────── -->
      <div class="qs-grid">
        <QsKpiCard
          label="Features concluídas"
          :value="doneCount"
          :suffix="`/ ${totalCount}`"
          :progress="globalPct"
          :meta="`${globalPct}% do roadmap completo`"
          dot-color="var(--qs-teal)"
        />
        <QsKpiCard
          v-for="mvp in mvpStats"
          :key="mvp.id"
          :label="mvp.shortName"
          :value="mvp.pct"
          suffix="%"
          :progress="mvp.pct"
          :meta="`${mvp.done}/${mvp.total} features`"
          :dot-color="mvp.color"
        />
        <QsKpiCard
          label="Versão atual"
          value="1.3.0"
          :meta="`Mai 2026 · Hardening & BI`"
          dot-color="var(--qs-lime)"
        />
      </div>

      <!-- ─── §1 INTRODUÇÃO ─────────────────────────────────────────────── -->
      <section v-if="activeSection === 'all' || activeSection === 'intro'" class="qs-card-section">
        <div class="qs-section-head">
          <div class="qs-flow-section-badge">§1</div>
          <div>
            <div class="qs-section-title">Introdução ao Protocolo</div>
            <div class="qs-section-desc">Padrão normativo de qualidade, performance e Clean Code para a Quanta Shop no Replit.</div>
          </div>
        </div>
        <div class="qs-intro-grid">
          <div class="qs-intro-card">
            <div class="qs-intro-icon">
              <svg width="22" height="22" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"/></svg>
            </div>
            <div class="qs-intro-title">Máxima qualidade técnica</div>
            <div class="qs-intro-desc">Código limpo, escalável e de fácil manutenção com padrões definidos e revisados.</div>
          </div>
          <div class="qs-intro-card">
            <div class="qs-intro-icon">
              <svg width="22" height="22" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M13 2.003A10 10 0 1 0 22 12h-2a8 8 0 1 1-7.8-8zM11 11V7h2v4h4v2h-6V11z" fill="var(--qs-teal)"/></svg>
            </div>
            <div class="qs-intro-title">Redução de custos operacionais</div>
            <div class="qs-intro-desc">Otimização de performance e arquitetura para minimizar gastos em infraestrutura.</div>
          </div>
          <div class="qs-intro-card">
            <div class="qs-intro-icon">
              <svg width="22" height="22" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4zm0 10.99h7c-.53 4.12-3.28 7.79-7 8.94V12H5V6.3l7-3.11v8.8z"/></svg>
            </div>
            <div class="qs-intro-title">Integridade arquitetural</div>
            <div class="qs-intro-desc">Aderência às diretrizes garante escalabilidade a longo prazo para novos e atuais módulos.</div>
          </div>
        </div>
        <div class="qs-meta-row">
          <span class="qs-meta-item"><b>Projeto:</b> Quanta Shop</span>
          <span class="qs-meta-item"><b>Data de Início:</b> Q4 2025</span>
          <span class="qs-meta-item"><b>Responsável:</b> Mauro / Flow</span>
          <span class="qs-meta-item"><b>Documento:</b> 29 de abril de 2026</span>
        </div>
      </section>

      <!-- ─── §2 CAMADA DE CONTEXTO E MEMÓRIA ──────────────────────────── -->
      <section v-if="activeSection === 'all' || activeSection === 'contexto'" class="qs-card-section">
        <div class="qs-section-head">
          <div class="qs-flow-section-badge">§2</div>
          <div>
            <div class="qs-section-title">Camada de Contexto e Memória</div>
            <div class="qs-section-desc">Preservação do conhecimento técnico e colaboração entre desenvolvedores e IAs.</div>
          </div>
        </div>
        <div class="qs-checklist">
          <div class="qs-check-item qs-check-done">
            <div class="qs-check-icon">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
            </div>
            <div class="qs-check-body">
              <div class="qs-check-title">CLAUDE.md — Contexto técnico permanente</div>
              <div class="qs-check-desc">Stack, padrões de nomenclatura, regras de negócio invioláveis, rotas e arquitetura. Atualizado em Mai 2026.</div>
            </div>
            <a href="/docs/CLAUDE.md" target="_blank" class="qs-btn-sm-outline">Ver</a>
          </div>
          <div class="qs-check-item qs-check-done">
            <div class="qs-check-icon">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
            </div>
            <div class="qs-check-body">
              <div class="qs-check-title">CHANGELOG.md — Registro semântico de versões</div>
              <div class="qs-check-desc">Todas as versões, hotfixes e funcionalidades em formato Keep a Changelog + SemVer. Versão atual: 1.3.0.</div>
            </div>
            <a href="/docs/CHANGELOG.md" target="_blank" class="qs-btn-sm-outline">Ver</a>
          </div>
          <div class="qs-check-item qs-check-done">
            <div class="qs-check-icon">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
            </div>
            <div class="qs-check-body">
              <div class="qs-check-title">Dicionário de Dados — features.json</div>
              <div class="qs-check-desc">Mapeamento de {{ totalCount }} features com IDs, status, progresso, público-alvo e user stories. Gerado automaticamente.</div>
            </div>
            <a href="/docs/features.json" target="_blank" class="qs-btn-sm-outline">Ver</a>
          </div>
          <div class="qs-check-item qs-check-done">
            <div class="qs-check-icon">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
            </div>
            <div class="qs-check-body">
              <div class="qs-check-title">FEATURES.md — Narrativa de produto</div>
              <div class="qs-check-desc">User stories detalhadas com critérios de aceitação, valor de negócio e dependências por feature.</div>
            </div>
            <a href="/docs/FEATURES.md" target="_blank" class="qs-btn-sm-outline">Ver</a>
          </div>
        </div>
      </section>

      <!-- ─── §3 GESTÃO E PROGRESSO ─────────────────────────────────────── -->
      <section v-if="activeSection === 'all' || activeSection === 'gestao'" class="qs-card-section">
        <div class="qs-section-head">
          <div class="qs-flow-section-badge">§3</div>
          <div>
            <div class="qs-section-title">Gestão e Progresso</div>
            <div class="qs-section-desc">Painel de status com visibilidade imediata sobre o estado real de cada feature do produto.</div>
          </div>
        </div>

        <!-- filtros inline -->
        <div class="qs-table-filters">
          <QsFilterChip
            v-for="f in statusFilterOptions"
            :key="f.key"
            :active="activeStatusFilter === f.key"
            :count="f.count"
            @click="activeStatusFilter = f.key"
          >{{ f.label }}</QsFilterChip>

          <span class="qs-table-filter-sep">|</span>

          <QsFilterChip
            v-for="m in mvpFilterOptions"
            :key="m.key"
            :active="activeMvpFilter === m.key"
            :count="m.count"
            @click="activeMvpFilter = m.key"
          >{{ m.label }}</QsFilterChip>
        </div>

        <div class="qs-table-wrap">
          <table class="qs-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Feature</th>
                <th>Fase</th>
                <th>Prioridade</th>
                <th>Status</th>
                <th>Progresso</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="f in filteredFeatures" :key="f.id">
                <td><span class="qs-id-chip">{{ f.id }}</span></td>
                <td class="qs-feature-title-cell">{{ f.title }}</td>
                <td><span class="qs-mvp-chip" :style="{ borderColor: mvpColor(f.mvp), color: mvpColor(f.mvp) }">{{ mvpShort(f.mvp) }}</span></td>
                <td><span class="qs-badge" :class="priorityBadge(f.mvp)">{{ priorityLabel(f.mvp) }}</span></td>
                <td><span class="qs-badge" :class="statusBadge(f.status)">{{ statusLabel(f.status) }}</span></td>
                <td>
                  <div class="qs-progress-inline">
                    <div class="qs-progress-track">
                      <div class="qs-progress-fill" :style="{ width: f.progress + '%', background: progressColor(f.progress) }"></div>
                    </div>
                    <span class="qs-progress-val">{{ f.progress }}%</span>
                  </div>
                </td>
              </tr>
              <tr v-if="filteredFeatures.length === 0">
                <td colspan="6">
                  <div class="qs-empty-state">Nenhuma feature encontrada com os filtros selecionados.</div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div class="qs-table-footer">
          <span class="qs-meta">{{ filteredFeatures.length }} de {{ totalCount }} features exibidas</span>
          <NuxtLink to="/lab/features" class="qs-btn-sm-outline">Ver painel completo</NuxtLink>
          <NuxtLink to="/lab" class="qs-btn-sm-outline" style="margin-left:.5rem">← Voltar ao LAB</NuxtLink>
        </div>
      </section>

      <!-- ─── §4 SETUP TÉCNICO REPLIT ───────────────────────────────────── -->
      <section v-if="activeSection === 'all' || activeSection === 'setup'" class="qs-card-section">
        <div class="qs-section-head">
          <div class="qs-flow-section-badge">§4</div>
          <div>
            <div class="qs-section-title">Setup Técnico Replit</div>
            <div class="qs-section-desc">Configurações de ambiente, segredos e deploy para extrair o máximo do Replit.</div>
          </div>
        </div>
        <div class="qs-setup-grid">
          <div class="qs-setup-card qs-setup-done">
            <div class="qs-setup-header">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
              <span class="qs-setup-title">.replit / nuxt.config.ts</span>
            </div>
            <div class="qs-setup-items">
              <div class="qs-setup-item">Comando de start: <code>npm run dev</code> (porta 5000)</div>
              <div class="qs-setup-item">API .NET: <code>bash api/start-api.sh</code> (porta 8000)</div>
              <div class="qs-setup-item">Proxy Nitro: <code>/api-proxy</code> → API remota com fallback</div>
              <div class="qs-setup-item">Hot-reload HMR ativo em desenvolvimento</div>
              <div class="qs-setup-item"><code>imports.dirs</code> explicitado: elimina 449 warnings/sessão</div>
            </div>
          </div>
          <div class="qs-setup-card qs-setup-done">
            <div class="qs-setup-header">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
              <span class="qs-setup-title">Gestão de Secrets</span>
            </div>
            <div class="qs-setup-items">
              <div class="qs-setup-item">Credenciais armazenadas no Replit Secrets (nunca no código)</div>
              <div class="qs-setup-item"><code>GOOGLE_CLIENT_ID</code> — Login social Google</div>
              <div class="qs-setup-item"><code>NUXT_API_BASE_URL</code> — Endpoint da API de produção</div>
              <div class="qs-setup-item">JWT 60min + refresh 30 dias via HttpOnly cookie</div>
            </div>
          </div>
          <div class="qs-setup-card qs-setup-done">
            <div class="qs-setup-header">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
              <span class="qs-setup-title">Guia de Deploy</span>
            </div>
            <div class="qs-setup-items">
              <div class="qs-setup-item">Produção: <code>https://quantashop.com.br</code></div>
              <div class="qs-setup-item">API: <code>https://api.quantashop.com.br</code> (Azure)</div>
              <div class="qs-setup-item">Build: <code>build-prod.sh</code> + <code>start-prod.sh</code></div>
              <div class="qs-setup-item">Deploy autoscale Replit com SSL automático</div>
              <div class="qs-setup-item">CORS restrito a domínios Quanta. Rate limit 10req/60s em auth.</div>
            </div>
          </div>
        </div>
      </section>

      <!-- ─── §5 QUALIDADE E CÓDIGO LIMPO ──────────────────────────────── -->
      <section v-if="activeSection === 'all' || activeSection === 'qualidade'" class="qs-card-section">
        <div class="qs-section-head">
          <div class="qs-flow-section-badge">§5</div>
          <div>
            <div class="qs-section-title">Qualidade e Código Limpo</div>
            <div class="qs-section-desc">Padrões de tratamento de erro, Definition of Done e práticas de Clean Code adotadas.</div>
          </div>
        </div>
        <div class="qs-quality-cols">
          <div class="qs-quality-block">
            <div class="qs-quality-block-title">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
              Tratamento de Erros
            </div>
            <div class="qs-checklist qs-checklist-compact">
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Captura global de exceções no useApi() com try/catch padrão</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Mensagens de erro amigáveis ao usuário em todas as telas admin</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Fallback mock em dev (import.meta.dev) para não contaminar produção</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Estado de loading explícito (skeleton / spinner) em todas as telas</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Rate limit: 10 req/60s em auth. DebugBypassGuard em produção.</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Axios timeout 30s alinhado com proxy Nitro (20s + margem)</span>
              </div>
            </div>
          </div>
          <div class="qs-quality-block">
            <div class="qs-quality-block-title">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
              Definition of Done (DoD)
            </div>
            <div class="qs-checklist qs-checklist-compact">
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>CHANGELOG.md atualizado com a versão da entrega</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>features.json e CLAUDE.md atualizados com novas features</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Design System (quanta-premium.scss): classes .qs-* globais; locais só em scoped</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>LGPD: dados sensíveis mascarados por padrão; Revelar exclusivo Master</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Zero alterações no backend .NET — diretriz máxima respeitada</span>
              </div>
              <div class="qs-check-item qs-check-done">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>Código morto e comentários desnecessários removidos antes do merge</span>
              </div>
            </div>
          </div>
        </div>

        <div class="qs-dod-alert">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
          <span><b>Atenção:</b> Nenhuma funcionalidade é considerada concluída sem a atualização correspondente no CHANGELOG e validação do Protocolo de Testes.</span>
        </div>
      </section>

    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] })

interface Feature {
  id: string
  mvp: string
  audience: string
  title: string
  status: string
  progress: number
  stories: string[]
  description: string
}

interface MvpDef {
  id: string
  name: string
  description: string
  color: string
}

interface FeaturesJson {
  generatedAt: string
  version: string
  mvps: MvpDef[]
  features: Feature[]
}

const loading = ref(true)
const data = ref<FeaturesJson | null>(null)

async function reload() {
  loading.value = true
  try {
    const resp = await $fetch<FeaturesJson>('/docs/features.json', { cache: 'no-store' })
    data.value = resp
  } catch { /**/ } finally {
    loading.value = false
  }
}

onMounted(reload)

const sections = [
  { key: 'all',       label: 'Todos' },
  { key: 'intro',     label: '§1 Intro' },
  { key: 'contexto',  label: '§2 Contexto' },
  { key: 'gestao',    label: '§3 Gestão' },
  { key: 'setup',     label: '§4 Setup' },
  { key: 'qualidade', label: '§5 Qualidade' },
]
const activeSection = ref('all')

const mvpColorMap: Record<string, string> = {
  mvp1: '#225F6B',
  mvp2: '#2F7785',
  mvp3: '#98C73A',
  mvp4: '#9ca3af',
}
function mvpColor(mvpId: string) { return mvpColorMap[mvpId] ?? '#9ca3af' }
function mvpShort(mvpId: string) {
  const map: Record<string,string> = { mvp1: 'MVP 1', mvp2: 'MVP 2', mvp3: 'MVP 3', mvp4: 'MVP 4' }
  return map[mvpId] ?? mvpId
}

const features = computed<Feature[]>(() => data.value?.features ?? [])
const totalCount = computed(() => features.value.length)
const doneCount = computed(() => features.value.filter(f => f.status === 'done').length)
const globalPct = computed(() => totalCount.value ? Math.round(doneCount.value / totalCount.value * 100) : 0)

const mvpStats = computed(() => {
  const mvps = data.value?.mvps ?? []
  return mvps.map(m => {
    const all = features.value.filter(f => f.mvp === m.id)
    const done = all.filter(f => f.status === 'done').length
    const pct = all.length ? Math.round(done / all.length * 100) : 0
    return { id: m.id, shortName: m.name.split('—')[0].trim(), done, total: all.length, pct, color: m.color }
  })
})

function priorityLabel(mvp: string) {
  if (mvp === 'mvp1' || mvp === 'mvp2') return 'Alta'
  if (mvp === 'mvp3') return 'Média'
  return 'Baixa'
}
function priorityBadge(mvp: string) {
  if (mvp === 'mvp1' || mvp === 'mvp2') return 'qs-badge-danger'
  if (mvp === 'mvp3') return 'qs-badge-warn'
  return 'qs-badge-neutral'
}
function statusLabel(s: string) {
  const map: Record<string,string> = { done: 'Concluído', in_progress: 'Em curso', planned: 'Planejado', backlog: 'Backlog' }
  return map[s] ?? s
}
function statusBadge(s: string) {
  const map: Record<string,string> = { done: 'qs-badge-success', in_progress: 'qs-badge-warn', planned: 'qs-badge-neutral', backlog: 'qs-badge-neutral' }
  return map[s] ?? 'qs-badge-neutral'
}
function progressColor(p: number) {
  if (p === 100) return 'var(--qs-lime)'
  if (p >= 50) return 'var(--qs-teal)'
  if (p > 0) return '#f59e0b'
  return 'var(--qs-gray-200)'
}

const statusFilterOptions = computed(() => {
  const opts = [
    { key: 'all', label: 'Todos', count: features.value.length },
    { key: 'done', label: 'Concluído', count: features.value.filter(f => f.status === 'done').length },
    { key: 'in_progress', label: 'Em curso', count: features.value.filter(f => f.status === 'in_progress').length },
    { key: 'planned', label: 'Planejado', count: features.value.filter(f => f.status === 'planned').length },
    { key: 'backlog', label: 'Backlog', count: features.value.filter(f => f.status === 'backlog').length },
  ]
  return opts.filter(o => o.key === 'all' || o.count > 0)
})
const activeStatusFilter = ref('all')

const mvpFilterOptions = computed(() => {
  const mvps = data.value?.mvps ?? []
  return [
    { key: 'all', label: 'Todas as fases', count: features.value.length },
    ...mvps.map(m => ({
      key: m.id,
      label: m.name.split('—')[0].trim(),
      count: features.value.filter(f => f.mvp === m.id).length,
    }))
  ]
})
const activeMvpFilter = ref('all')

const filteredFeatures = computed(() => {
  return features.value.filter(f => {
    const statusOk = activeStatusFilter.value === 'all' || f.status === activeStatusFilter.value
    const mvpOk = activeMvpFilter.value === 'all' || f.mvp === activeMvpFilter.value
    return statusOk && mvpOk
  })
})
</script>

<style scoped>
.qs-period-switch { display: flex; gap: .4rem; flex-wrap: wrap; }

/* ── Section head ───────────────────────────────────── */
.qs-section-head { display: flex; align-items: flex-start; gap: 1rem; margin-bottom: 1.25rem; }
.qs-flow-section-badge {
  min-width: 36px; height: 36px;
  background: var(--qs-teal); color: #fff;
  border-radius: 8px; display: flex; align-items: center; justify-content: center;
  font-size: .75rem; font-weight: 700; letter-spacing: .03em; flex-shrink: 0;
}

/* ── §1 Intro grid ──────────────────────────────────── */
.qs-intro-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(220px, 1fr)); gap: 1rem; margin-bottom: 1.25rem; }
.qs-intro-card {
  background: var(--qs-gray-50, #f8f9fa); border: 1px solid var(--qs-gray-100, #e9ecef);
  border-radius: 10px; padding: 1rem 1.1rem;
}
.qs-intro-icon { margin-bottom: .6rem; }
.qs-intro-title { font-size: .875rem; font-weight: 600; color: var(--qs-ink); margin-bottom: .3rem; }
.qs-intro-desc  { font-size: .8rem; color: var(--qs-gray-400); line-height: 1.45; }
.qs-meta-row {
  display: flex; gap: 1.5rem; flex-wrap: wrap;
  padding: .75rem 1rem; background: var(--qs-gray-50, #f8f9fa);
  border-radius: 8px; font-size: .82rem; color: var(--qs-gray-500, #6b7280);
}
.qs-meta-row b { color: var(--qs-ink); }

/* ── §2 Checklist ───────────────────────────────────── */
.qs-checklist { display: flex; flex-direction: column; gap: .75rem; }
.qs-check-item {
  display: flex; align-items: flex-start; gap: .75rem;
  padding: .85rem 1rem; border-radius: 8px;
  background: var(--qs-gray-50, #f8f9fa); border: 1px solid var(--qs-gray-100, #e9ecef);
}
.qs-check-item.qs-check-done { border-left: 3px solid var(--qs-lime, #98C73A); }
.qs-check-icon { flex-shrink: 0; margin-top: 1px; }
.qs-check-body { flex: 1; }
.qs-check-title { font-size: .875rem; font-weight: 600; color: var(--qs-ink); margin-bottom: .2rem; }
.qs-check-desc  { font-size: .8rem; color: var(--qs-gray-400); line-height: 1.4; }
.qs-btn-sm-outline {
  flex-shrink: 0; font-size: .75rem; padding: .3rem .75rem;
  border: 1px solid var(--qs-teal); color: var(--qs-teal); border-radius: 6px;
  background: transparent; cursor: pointer; white-space: nowrap;
  text-decoration: none; display: inline-flex; align-items: center;
  transition: background .15s, color .15s;
}
.qs-btn-sm-outline:hover { background: var(--qs-teal); color: #fff; }

/* ── §3 Table ───────────────────────────────────────── */
.qs-table-filters { display: flex; gap: .4rem; flex-wrap: wrap; align-items: center; margin-bottom: 1rem; }
.qs-table-filter-sep { color: var(--qs-gray-200, #e5e7eb); font-size: .9rem; margin: 0 .15rem; }
.qs-table-wrap { overflow-x: auto; border-radius: 8px; border: 1px solid var(--qs-gray-100, #e9ecef); }
.qs-table { width: 100%; border-collapse: collapse; font-size: .82rem; }
.qs-table th {
  background: var(--qs-gray-50, #f8f9fa); padding: .65rem 1rem;
  font-weight: 600; font-size: .75rem; text-transform: uppercase;
  letter-spacing: .05em; color: var(--qs-gray-400, #9ca3af);
  text-align: left; border-bottom: 1px solid var(--qs-gray-100, #e9ecef);
  white-space: nowrap;
}
.qs-table td {
  padding: .65rem 1rem; border-bottom: 1px solid var(--qs-gray-100, #e9ecef);
  color: var(--qs-ink); vertical-align: middle;
}
.qs-table tr:last-child td { border-bottom: none; }
.qs-table tr:hover td { background: var(--qs-gray-50, #f8f9fa); }
.qs-feature-title-cell { font-weight: 500; max-width: 320px; }

.qs-id-chip {
  font-size: .7rem; font-weight: 700; padding: .2rem .5rem;
  background: #e8f4f7; color: var(--qs-teal); border-radius: 4px;
  letter-spacing: .04em; white-space: nowrap;
}
.qs-mvp-chip {
  font-size: .7rem; font-weight: 600; padding: .15rem .5rem;
  border: 1px solid; border-radius: 4px; white-space: nowrap;
}
.qs-badge { font-size: .72rem; font-weight: 600; padding: .2rem .6rem; border-radius: 100px; white-space: nowrap; }
.qs-badge-success { background: #d1fae5; color: #065f46; }
.qs-badge-warn    { background: #fef3c7; color: #92400e; }
.qs-badge-danger  { background: #fee2e2; color: #991b1b; }
.qs-badge-neutral { background: var(--qs-gray-100, #f3f4f6); color: var(--qs-gray-400, #6b7280); }

.qs-progress-inline { display: flex; align-items: center; gap: .5rem; min-width: 100px; }
.qs-progress-track  { flex: 1; height: 5px; background: var(--qs-gray-100, #e9ecef); border-radius: 100px; overflow: hidden; }
.qs-progress-fill   { height: 100%; border-radius: 100px; transition: width .3s; }
.qs-progress-val    { font-size: .72rem; color: var(--qs-gray-400); min-width: 30px; text-align: right; }

.qs-table-footer {
  display: flex; justify-content: space-between; align-items: center;
  padding: .75rem 1rem 0; margin-top: .5rem;
}
.qs-meta { font-size: .8rem; color: var(--qs-gray-400); }

.qs-empty-state {
  text-align: center; padding: 2rem; color: var(--qs-gray-400); font-size: .88rem;
}

/* ── §4 Setup ───────────────────────────────────────── */
.qs-setup-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(260px, 1fr)); gap: 1rem; }
.qs-setup-card {
  background: var(--qs-gray-50, #f8f9fa); border: 1px solid var(--qs-gray-100, #e9ecef);
  border-radius: 10px; padding: 1rem 1.1rem;
}
.qs-setup-card.qs-setup-done { border-left: 3px solid var(--qs-lime, #98C73A); }
.qs-setup-header { display: flex; align-items: center; gap: .5rem; margin-bottom: .75rem; }
.qs-setup-title  { font-size: .875rem; font-weight: 600; color: var(--qs-ink); }
.qs-setup-items  { display: flex; flex-direction: column; gap: .4rem; }
.qs-setup-item   { font-size: .8rem; color: var(--qs-gray-500, #6b7280); line-height: 1.4; }
.qs-setup-item code { font-size: .75rem; background: #e8f4f7; color: var(--qs-teal); padding: .1rem .35rem; border-radius: 3px; }

/* ── §5 Quality ─────────────────────────────────────── */
.qs-quality-cols { display: grid; grid-template-columns: 1fr 1fr; gap: 1.25rem; margin-bottom: 1rem; }
@media (max-width: 768px) { .qs-quality-cols { grid-template-columns: 1fr; } }
.qs-quality-block { background: var(--qs-gray-50, #f8f9fa); border: 1px solid var(--qs-gray-100, #e9ecef); border-radius: 10px; padding: 1rem 1.1rem; }
.qs-quality-block-title { display: flex; align-items: center; gap: .4rem; font-size: .875rem; font-weight: 600; color: var(--qs-ink); margin-bottom: .75rem; }
.qs-checklist-compact .qs-check-item {
  padding: .45rem .5rem; background: transparent; border: none;
  border-radius: 0; border-bottom: 1px solid var(--qs-gray-100, #e9ecef);
  font-size: .82rem; display: flex; align-items: flex-start; gap: .5rem; color: var(--qs-gray-500, #6b7280);
}
.qs-checklist-compact .qs-check-item:last-child { border-bottom: none; }

.qs-dod-alert {
  display: flex; align-items: flex-start; gap: .6rem;
  background: #fef3c7; border: 1px solid #fde68a; border-radius: 8px;
  padding: .85rem 1rem; font-size: .82rem; color: #78350f; margin-top: .5rem;
}
.qs-dod-alert svg { flex-shrink: 0; margin-top: 1px; fill: #92400e; }
</style>
