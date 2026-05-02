<template>
  <div class="qs-features">
    <!-- HEADER -->
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Escritório Virtual · Produto</div>
        <h1>Features &amp; MVP</h1>
        <p>Acompanhe o roadmap da plataforma em tempo real, organizado por fase de entrega e público.</p>
      </div>
      <div class="qs-header-actions">
        <span v-if="generatedAt" class="qs-meta">
          Atualizado em {{ formatDate(generatedAt) }}
        </span>
        <button class="qs-btn qs-btn-outline" @click="reload" :disabled="loading">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M17.65 6.35A7.958 7.958 0 0 0 12 4c-4.42 0-7.99 3.58-7.99 8s3.57 8 7.99 8c3.73 0 6.84-2.55 7.73-6h-2.08A5.99 5.99 0 0 1 12 18c-3.31 0-6-2.69-6-6s2.69-6 6-6c1.66 0 3.14.69 4.22 1.78L13 11h7V4l-2.35 2.35z"/></svg>
          Recarregar
        </button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading">
      <div class="qs-spinner"></div>
    </div>

    <template v-else-if="data">
      <!-- KPI STRIP -->
      <div class="qs-kpi-grid">
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Progresso global</div>
          <div class="qs-kpi-value">{{ globalPct }}%</div>
          <div class="qs-progress-track">
            <div class="qs-progress-fill" :style="{ width: globalPct + '%' }"></div>
          </div>
          <div class="qs-kpi-meta">
            {{ doneCount }} de {{ totalCount }} features prontas
          </div>
        </div>
        <div class="qs-kpi-card" v-for="mvp in mvpStats" :key="mvp.id">
          <div class="qs-kpi-label">
            <span class="qs-kpi-dot" :style="{ background: mvp.color }"></span>
            {{ mvp.shortName }}
          </div>
          <div class="qs-kpi-value">{{ mvp.pct }}%</div>
          <div class="qs-progress-track">
            <div class="qs-progress-fill" :style="{ width: mvp.pct + '%', background: mvp.color }"></div>
          </div>
          <div class="qs-kpi-meta">{{ mvp.done }}/{{ mvp.total }} features</div>
        </div>
      </div>

      <!-- FILTERS -->
      <div class="qs-filter-bar">
        <div class="qs-filter-group">
          <span class="qs-filter-label">Fase</span>
          <button
            v-for="m in mvpFilters"
            :key="m.key"
            class="qs-chip"
            :class="{ active: activeMvp === m.key }"
            @click="activeMvp = m.key"
          >{{ m.label }}<span class="qs-chip-count">{{ m.count }}</span></button>
        </div>
        <div class="qs-filter-group">
          <span class="qs-filter-label">Status</span>
          <button
            v-for="s in statusFilters"
            :key="s.key"
            class="qs-chip"
            :class="{ active: activeStatus === s.key }"
            @click="activeStatus = s.key"
          >{{ s.label }}<span class="qs-chip-count">{{ s.count }}</span></button>
        </div>
        <div class="qs-filter-group">
          <span class="qs-filter-label">Público</span>
          <button
            v-for="a in audienceFilters"
            :key="a.key"
            class="qs-chip"
            :class="{ active: activeAudience === a.key }"
            @click="activeAudience = a.key"
          >{{ a.label }}<span class="qs-chip-count">{{ a.count }}</span></button>
        </div>
      </div>

      <!-- LIST POR MVP -->
      <div v-for="mvp in groupedFeatures" :key="mvp.id" class="qs-mvp-block">
        <div class="qs-mvp-header">
          <div class="qs-mvp-header-left">
            <div class="qs-mvp-bullet" :style="{ background: mvp.color }"></div>
            <div>
              <h2 class="qs-mvp-title">{{ mvp.name }}</h2>
              <p class="qs-mvp-desc">{{ mvp.description }}</p>
            </div>
          </div>
          <div class="qs-mvp-stats">
            <div class="qs-mvp-pct">{{ mvp.pct }}%</div>
            <div class="qs-mvp-pct-meta">{{ mvp.done }}/{{ mvp.total }}</div>
          </div>
        </div>

        <div class="qs-progress-track qs-progress-thick">
          <div class="qs-progress-fill" :style="{ width: mvp.pct + '%', background: mvp.color }"></div>
        </div>

        <div class="qs-feature-list">
          <div
            v-for="feat in mvp.features"
            :key="feat.id"
            class="qs-feature-item"
            @click="toggleExpand(feat.id)"
          >
            <div class="qs-feature-icon">
              <svg v-if="feat.status === 'done'" width="20" height="20" viewBox="0 0 24 24" fill="#16a34a"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
              <svg v-else-if="feat.status === 'in_progress'" width="20" height="20" viewBox="0 0 24 24" fill="#2F7785"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zM6 12c0-3.31 2.69-6 6-6v12c-3.31 0-6-2.69-6-6z"/></svg>
              <svg v-else-if="feat.status === 'planned'" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#9ca3af" stroke-width="2"><circle cx="12" cy="12" r="9"/><circle cx="12" cy="12" r="3" fill="#9ca3af"/></svg>
              <svg v-else width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#d1d5db" stroke-width="2"><circle cx="12" cy="12" r="9"/></svg>
            </div>
            <div class="qs-feature-id">{{ feat.id }}</div>
            <div class="qs-feature-content">
              <div class="qs-feature-title-row">
                <span class="qs-feature-title">{{ feat.title }}</span>
                <span class="qs-feature-aud">{{ audienceIcon(feat.audience) }} {{ audienceName(feat.audience) }}</span>
              </div>
              <div v-if="expanded[feat.id]" class="qs-feature-desc">
                {{ feat.description }}
                <div v-if="feat.stories && feat.stories.length" class="qs-feature-stories">
                  <span class="qs-stories-label">Stories:</span>
                  <span v-for="s in feat.stories" :key="s" class="qs-story-tag">{{ s }}</span>
                </div>
              </div>
            </div>
            <div class="qs-feature-status">
              <div class="qs-progress-mini-track">
                <div class="qs-progress-mini-fill" :style="{ width: feat.progress + '%', background: feat.status === 'done' ? '#16a34a' : (mvp.color) }"></div>
              </div>
              <span class="qs-feature-pct">{{ feat.progress }}%</span>
              <span class="qs-status-badge" :class="'badge-' + feat.status">{{ statusLabel(feat.status) }}</span>
            </div>
          </div>

          <div v-if="mvp.features.length === 0" class="qs-empty">
            Nenhuma feature corresponde ao filtro nesta fase.
          </div>
        </div>
      </div>
    </template>

    <div v-else class="qs-error">
      <p>Não foi possível carregar os dados de features. Verifique <code>public/docs/features.json</code>.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

interface Mvp { id: string; name: string; description: string; color: string; }
interface Audience { id: string; name: string; icon: string; }
interface Feature {
  id: string;
  mvp: string;
  audience: string;
  title: string;
  status: 'done' | 'in_progress' | 'planned' | 'backlog';
  progress: number;
  stories: string[];
  description: string;
}
interface FeaturesData {
  generatedAt: string;
  version: string;
  mvps: Mvp[];
  audiences: Audience[];
  features: Feature[];
}

const loading = ref(true);
const data = ref<FeaturesData | null>(null);
const generatedAt = ref('');
const expanded = reactive<Record<string, boolean>>({});

const activeMvp = ref('all');
const activeStatus = ref('all');
const activeAudience = ref('all');

async function load() {
  loading.value = true;
  try {
    const json = await $fetch<FeaturesData>('/docs/features.json', { cache: 'no-store' });
    data.value = json;
    generatedAt.value = json.generatedAt ?? '';
  } catch (e) {
    console.error('[features] erro ao carregar features.json', e);
    data.value = null;
  } finally {
    loading.value = false;
  }
}

function reload() { load(); }
function toggleExpand(id: string) { expanded[id] = !expanded[id]; }

onMounted(load);

const allFeatures = computed(() => data.value?.features ?? []);

const filteredFeatures = computed(() =>
  allFeatures.value.filter(f =>
    (activeMvp.value === 'all' || f.mvp === activeMvp.value) &&
    (activeStatus.value === 'all' || f.status === activeStatus.value) &&
    (activeAudience.value === 'all' || f.audience === activeAudience.value || f.audience === 'all')
  )
);

const totalCount = computed(() => allFeatures.value.length);
const doneCount = computed(() => allFeatures.value.filter(f => f.status === 'done').length);
const globalPct = computed(() => {
  if (!allFeatures.value.length) return 0;
  const sum = allFeatures.value.reduce((acc, f) => acc + (f.progress || 0), 0);
  return Math.round(sum / allFeatures.value.length);
});

const mvpStats = computed(() => {
  if (!data.value) return [];
  return data.value.mvps.map(m => {
    const feats = allFeatures.value.filter(f => f.mvp === m.id);
    const done = feats.filter(f => f.status === 'done').length;
    const sum = feats.reduce((a, f) => a + (f.progress || 0), 0);
    const pct = feats.length ? Math.round(sum / feats.length) : 0;
    return {
      id: m.id,
      shortName: m.name.replace(/^MVP\s+\d+\s+—\s+/, ''),
      color: m.color,
      done,
      total: feats.length,
      pct,
    };
  });
});

const groupedFeatures = computed(() => {
  if (!data.value) return [];
  const list = filteredFeatures.value;
  return data.value.mvps
    .map(m => {
      const feats = list.filter(f => f.mvp === m.id);
      const allInMvp = allFeatures.value.filter(f => f.mvp === m.id);
      const done = allInMvp.filter(f => f.status === 'done').length;
      const sum = allInMvp.reduce((a, f) => a + (f.progress || 0), 0);
      const pct = allInMvp.length ? Math.round(sum / allInMvp.length) : 0;
      return {
        id: m.id,
        name: m.name,
        description: m.description,
        color: m.color,
        features: feats,
        done,
        total: allInMvp.length,
        pct,
      };
    })
    .filter(m => activeMvp.value === 'all' || m.id === activeMvp.value);
});

const mvpFilters = computed(() => {
  if (!data.value) return [];
  const all = { key: 'all', label: 'Todas', count: allFeatures.value.length };
  const items = data.value.mvps.map(m => ({
    key: m.id,
    label: m.name.replace(/^MVP\s+\d+\s+—\s+/, ''),
    count: allFeatures.value.filter(f => f.mvp === m.id).length,
  }));
  return [all, ...items];
});

const statusFilters = computed(() => {
  const all = { key: 'all', label: 'Todos', count: allFeatures.value.length };
  const states = [
    { key: 'done', label: 'Pronto' },
    { key: 'in_progress', label: 'Em andamento' },
    { key: 'planned', label: 'Planejado' },
    { key: 'backlog', label: 'Backlog' },
  ].map(s => ({
    ...s,
    count: allFeatures.value.filter(f => f.status === s.key).length,
  }));
  return [all, ...states];
});

const audienceFilters = computed(() => {
  if (!data.value) return [];
  const all = { key: 'all', label: 'Todos', count: allFeatures.value.length };
  const items = data.value.audiences.map(a => ({
    key: a.id,
    label: `${a.icon} ${a.name}`,
    count: allFeatures.value.filter(f => f.audience === a.id || f.audience === 'all').length,
  }));
  return [all, ...items];
});

function audienceName(id: string): string {
  if (id === 'all') return 'Todos';
  return data.value?.audiences.find(a => a.id === id)?.name ?? id;
}
function audienceIcon(id: string): string {
  if (id === 'all') return '👥';
  return data.value?.audiences.find(a => a.id === id)?.icon ?? '•';
}

function statusLabel(s: string): string {
  const map: Record<string, string> = {
    done: 'Pronto',
    in_progress: 'Em andamento',
    planned: 'Planejado',
    backlog: 'Backlog',
  };
  return map[s] ?? s;
}

function formatDate(iso: string): string {
  try {
    return new Date(iso).toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
  } catch { return iso; }
}
</script>

<style scoped>
/* ──────────────── PREMIUM DESIGN SYSTEM TOKENS (locais) ──────────────── */
.qs-features {
  --qs-teal: #2F7785;
  --qs-teal-dark: #225F6B;
  --qs-lime: #98C73A;
  --qs-bg: #F4F4F5;
  --qs-ink: #1d1d1f;
  --qs-gray-50: #fafafa;
  --qs-gray-100: #f5f5f7;
  --qs-gray-200: #e5e7eb;
  --qs-gray-400: #9ca3af;
  --qs-gray-500: #6b7280;
  --qs-gray-700: #374151;
  --qs-radius-md: 10px;
  --qs-radius-lg: 16px;
  --qs-radius-pill: 999px;
  --qs-shadow-sm: 0 2px 8px rgba(15, 23, 42, .06);
  --qs-shadow-md: 0 8px 24px rgba(15, 23, 42, .08);
  --qs-ease: cubic-bezier(0.4, 0, 0.2, 1);
  --qs-duration: 200ms;

  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  -webkit-font-smoothing: antialiased;
  color: var(--qs-ink);
  padding-bottom: 64px;
}

/* ──────────────── PAGE HEADER ──────────────── */
.qs-page-header {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 1.5rem;
  padding: 0 0 32px;
  border-bottom: 1px solid var(--qs-gray-100);
  margin-bottom: 32px;
}
.qs-header-text { max-width: 720px; }
.qs-eyebrow {
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--qs-teal);
  margin-bottom: 8px;
}
.qs-page-header h1 {
  font-size: 36px;
  font-weight: 700;
  letter-spacing: -0.02em;
  color: var(--qs-ink);
  margin: 0 0 8px;
  line-height: 1.1;
}
.qs-page-header p {
  font-size: 16px;
  color: var(--qs-gray-500);
  margin: 0;
  line-height: 1.55;
}
.qs-header-actions {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-shrink: 0;
}
.qs-meta {
  font-size: 12px;
  color: var(--qs-gray-400);
}

/* ──────────────── BUTTONS ──────────────── */
.qs-btn {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 10px 18px;
  font-size: 14px;
  font-weight: 500;
  border-radius: var(--qs-radius-md);
  border: none;
  cursor: pointer;
  transition: all var(--qs-duration) var(--qs-ease);
  font-family: inherit;
}
.qs-btn-outline {
  background: #fff;
  border: 1px solid var(--qs-teal);
  color: var(--qs-teal);
}
.qs-btn-outline:hover:not(:disabled) {
  background: var(--qs-teal);
  color: #fff;
  box-shadow: var(--qs-shadow-sm);
}
.qs-btn:disabled { opacity: 0.5; cursor: not-allowed; }

/* ──────────────── KPI GRID ──────────────── */
.qs-kpi-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 16px;
  margin-bottom: 40px;
}
.qs-kpi-card {
  background: #fff;
  border-radius: var(--qs-radius-lg);
  padding: 24px;
  box-shadow: var(--qs-shadow-sm);
  transition: transform var(--qs-duration) var(--qs-ease), box-shadow var(--qs-duration) var(--qs-ease);
}
.qs-kpi-card:hover {
  transform: translateY(-2px);
  box-shadow: var(--qs-shadow-md);
}
.qs-kpi-label {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13px;
  font-weight: 500;
  color: var(--qs-gray-500);
  margin-bottom: 8px;
}
.qs-kpi-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
}
.qs-kpi-value {
  font-size: 32px;
  font-weight: 700;
  color: var(--qs-ink);
  letter-spacing: -0.02em;
  line-height: 1;
  margin-bottom: 12px;
}
.qs-kpi-meta {
  font-size: 12px;
  color: var(--qs-gray-400);
  margin-top: 8px;
}

/* ──────────────── PROGRESS BARS ──────────────── */
.qs-progress-track {
  background: var(--qs-gray-200);
  border-radius: var(--qs-radius-pill);
  height: 6px;
  overflow: hidden;
}
.qs-progress-thick { height: 8px; }
.qs-progress-fill {
  height: 100%;
  background: var(--qs-lime);
  border-radius: var(--qs-radius-pill);
  transition: width 400ms var(--qs-ease);
}

/* ──────────────── FILTER BAR ──────────────── */
.qs-filter-bar {
  background: #fff;
  border-radius: var(--qs-radius-lg);
  padding: 20px 24px;
  box-shadow: var(--qs-shadow-sm);
  margin-bottom: 32px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.qs-filter-group {
  display: flex;
  align-items: center;
  gap: 8px;
  flex-wrap: wrap;
}
.qs-filter-label {
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--qs-gray-500);
  margin-right: 4px;
  min-width: 60px;
}
.qs-chip {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: 13px;
  padding: 6px 14px;
  border-radius: var(--qs-radius-pill);
  border: 1px solid var(--qs-gray-200);
  background: var(--qs-gray-50);
  color: var(--qs-gray-700);
  cursor: pointer;
  transition: all var(--qs-duration) var(--qs-ease);
  font-family: inherit;
  font-weight: 500;
}
.qs-chip:hover { border-color: var(--qs-teal); color: var(--qs-teal); }
.qs-chip.active {
  background: var(--qs-teal);
  border-color: var(--qs-teal);
  color: #fff;
}
.qs-chip-count {
  background: rgba(0, 0, 0, 0.08);
  border-radius: var(--qs-radius-pill);
  padding: 1px 8px;
  font-size: 11px;
  font-weight: 600;
}
.qs-chip.active .qs-chip-count {
  background: rgba(255, 255, 255, 0.25);
}

/* ──────────────── MVP BLOCKS ──────────────── */
.qs-mvp-block {
  background: #fff;
  border-radius: var(--qs-radius-lg);
  box-shadow: var(--qs-shadow-sm);
  margin-bottom: 24px;
  overflow: hidden;
}
.qs-mvp-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 16px;
  padding: 24px 28px 16px;
}
.qs-mvp-header-left {
  display: flex;
  gap: 16px;
  align-items: flex-start;
}
.qs-mvp-bullet {
  width: 4px;
  align-self: stretch;
  border-radius: var(--qs-radius-pill);
  margin-top: 4px;
  min-height: 40px;
}
.qs-mvp-title {
  font-size: 22px;
  font-weight: 700;
  color: var(--qs-ink);
  letter-spacing: -0.015em;
  margin: 0 0 4px;
}
.qs-mvp-desc {
  font-size: 14px;
  color: var(--qs-gray-500);
  margin: 0;
  line-height: 1.5;
}
.qs-mvp-stats { text-align: right; flex-shrink: 0; }
.qs-mvp-pct {
  font-size: 28px;
  font-weight: 700;
  color: var(--qs-ink);
  letter-spacing: -0.02em;
  line-height: 1;
}
.qs-mvp-pct-meta {
  font-size: 12px;
  color: var(--qs-gray-400);
  margin-top: 4px;
}
.qs-mvp-block .qs-progress-track {
  margin: 0 28px 8px;
}

/* ──────────────── FEATURE LIST ──────────────── */
.qs-feature-list {
  padding: 8px 0 8px;
}
.qs-feature-item {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 16px 28px;
  border-top: 1px solid var(--qs-gray-100);
  cursor: pointer;
  transition: background var(--qs-duration) var(--qs-ease);
}
.qs-feature-item:hover { background: var(--qs-gray-50); }
.qs-feature-icon { flex-shrink: 0; display: flex; align-items: center; }
.qs-feature-id {
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.04em;
  color: var(--qs-gray-400);
  flex-shrink: 0;
  min-width: 48px;
}
.qs-feature-content { flex: 1; min-width: 0; }
.qs-feature-title-row {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}
.qs-feature-title {
  font-size: 15px;
  font-weight: 500;
  color: var(--qs-ink);
  line-height: 1.4;
}
.qs-feature-aud {
  font-size: 11px;
  color: var(--qs-gray-400);
  font-weight: 500;
}
.qs-feature-desc {
  font-size: 13px;
  color: var(--qs-gray-700);
  line-height: 1.55;
  margin-top: 8px;
  padding-right: 12px;
}
.qs-feature-stories {
  margin-top: 8px;
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 6px;
}
.qs-stories-label {
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.04em;
  color: var(--qs-gray-500);
  text-transform: uppercase;
}
.qs-story-tag {
  background: var(--qs-bg);
  padding: 2px 8px;
  border-radius: var(--qs-radius-pill);
  font-size: 11px;
  font-weight: 600;
  color: var(--qs-teal-dark);
}
.qs-feature-status {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-shrink: 0;
}
.qs-progress-mini-track {
  width: 80px;
  height: 4px;
  background: var(--qs-gray-200);
  border-radius: var(--qs-radius-pill);
  overflow: hidden;
}
.qs-progress-mini-fill {
  height: 100%;
  border-radius: var(--qs-radius-pill);
  transition: width 400ms var(--qs-ease);
}
.qs-feature-pct {
  font-size: 12px;
  font-weight: 600;
  color: var(--qs-gray-500);
  min-width: 36px;
  text-align: right;
}
.qs-status-badge {
  font-size: 11px;
  font-weight: 600;
  padding: 4px 10px;
  border-radius: var(--qs-radius-pill);
  letter-spacing: 0.02em;
  white-space: nowrap;
}
.badge-done { background: #dcfce7; color: #16a34a; }
.badge-in_progress { background: #dbeafe; color: #1d4ed8; }
.badge-planned { background: #fef3c7; color: #d97706; }
.badge-backlog { background: var(--qs-gray-100); color: var(--qs-gray-500); }

/* ──────────────── EMPTY/LOADING/ERROR ──────────────── */
.qs-empty {
  text-align: center;
  padding: 32px;
  color: var(--qs-gray-400);
  font-size: 14px;
}
.qs-loading {
  display: flex;
  justify-content: center;
  padding: 80px 0;
}
.qs-spinner {
  width: 36px;
  height: 36px;
  border: 3px solid var(--qs-gray-200);
  border-top-color: var(--qs-teal);
  border-radius: 50%;
  animation: qs-spin 0.8s linear infinite;
}
@keyframes qs-spin { to { transform: rotate(360deg); } }
.qs-error {
  background: #fff;
  border-radius: var(--qs-radius-lg);
  padding: 40px;
  text-align: center;
  color: var(--qs-gray-500);
  box-shadow: var(--qs-shadow-sm);
}
.qs-error code {
  background: var(--qs-bg);
  padding: 2px 6px;
  border-radius: 4px;
  font-size: 12px;
  color: var(--qs-teal-dark);
}

/* ──────────────── RESPONSIVE ──────────────── */
@media (max-width: 768px) {
  .qs-page-header h1 { font-size: 28px; }
  .qs-page-header p { font-size: 15px; }
  .qs-mvp-header { flex-direction: column; align-items: stretch; padding: 20px; }
  .qs-mvp-block .qs-progress-track { margin: 0 20px 8px; }
  .qs-feature-item { padding: 14px 20px; flex-wrap: wrap; }
  .qs-feature-status { width: 100%; justify-content: flex-end; padding-top: 4px; }
  .qs-progress-mini-track { width: 60px; }
  .qs-filter-label { min-width: 100%; }
}
</style>
