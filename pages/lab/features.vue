<template>
  <div class="qs-page qs-features">
    <QsPageHeader eyebrow="Escritório Virtual · Produto" title="Features &amp; MVP" description="Acompanhe o roadmap da plataforma em tempo real, organizado por fase de entrega e público.">
      <span v-if="generatedAt" class="qs-meta">Atualizado em {{ formatDate(generatedAt) }}</span>
      <button class="qs-btn-outline" @click="reload" :disabled="loading">
        <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M17.65 6.35A7.958 7.958 0 0 0 12 4c-4.42 0-7.99 3.58-7.99 8s3.57 8 7.99 8c3.73 0 6.84-2.55 7.73-6h-2.08A5.99 5.99 0 0 1 12 18c-3.31 0-6-2.69-6-6s2.69-6 6-6c1.66 0 3.14.69 4.22 1.78L13 11h7V4l-2.35 2.35z"/></svg>
        Recarregar
      </button>
    </QsPageHeader>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner"></div></div>

    <template v-else-if="data">
      <div class="qs-grid">
        <QsKpiCard
          label="Progresso global"
          :value="globalPct"
          suffix="%"
          :progress="globalPct"
          :meta="`${doneCount} de ${totalCount} features prontas`"
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
      </div>

      <div class="qs-card-section qs-filter-bar">
        <div class="qs-filter-group">
          <span class="qs-filter-label">Fase</span>
          <QsFilterChip
            v-for="m in mvpFilters"
            :key="m.key"
            :active="activeMvp === m.key"
            :count="m.count"
            @click="activeMvp = m.key"
          >{{ m.label }}</QsFilterChip>
        </div>
        <div class="qs-filter-group">
          <span class="qs-filter-label">Status</span>
          <QsFilterChip
            v-for="s in statusFilters"
            :key="s.key"
            :active="activeStatus === s.key"
            :count="s.count"
            @click="activeStatus = s.key"
          >{{ s.label }}</QsFilterChip>
        </div>
        <div class="qs-filter-group">
          <span class="qs-filter-label">Público</span>
          <QsFilterChip
            v-for="a in audienceFilters"
            :key="a.key"
            :active="activeAudience === a.key"
            :count="a.count"
            @click="activeAudience = a.key"
          >{{ a.label }}</QsFilterChip>
        </div>
      </div>

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

        <div class="qs-mvp-progress">
          <QsProgressBar :value="mvp.pct" :color="mvp.color" size="thick" />
        </div>

        <div class="qs-feature-list">
          <div
            v-for="feat in mvp.features"
            :key="feat.id"
            class="qs-feature-item"
            role="button"
            tabindex="0"
            :aria-expanded="!!expanded[feat.id]"
            :aria-label="`Ver detalhes da feature ${feat.id}: ${feat.title}`"
            @click="toggleExpand(feat.id)"
            @keydown.enter.prevent="toggleExpand(feat.id)"
            @keydown.space.prevent="toggleExpand(feat.id)"
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
              <div class="qs-mini-progress">
                <QsProgressBar
                  :value="feat.progress"
                  :color="feat.status === 'done' ? '#16a34a' : mvp.color"
                  size="thin"
                />
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
.qs-features { /* tokens herdados de quanta-premium.scss */ }

.qs-filter-bar {
  display: flex;
  flex-direction: column;
  gap: 16px;
  padding: 20px 24px;
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
.qs-mvp-header-left { display: flex; gap: 16px; align-items: flex-start; }
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
.qs-mvp-progress { padding: 0 28px 8px; }

.qs-feature-list { padding: 8px 0; }
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
.qs-feature-aud { font-size: 11px; color: var(--qs-gray-400); font-weight: 500; }
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
.qs-mini-progress { width: 80px; }
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

.qs-empty { text-align: center; padding: 32px; color: var(--qs-gray-400); font-size: 14px; }
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

@media (max-width: 768px) {
  .qs-page-header h1 { font-size: 28px; }
  .qs-mvp-header { flex-direction: column; align-items: stretch; padding: 20px; }
  .qs-mvp-progress { padding: 0 20px 8px; }
  .qs-feature-item { padding: 14px 20px; flex-wrap: wrap; }
  .qs-feature-status { width: 100%; justify-content: flex-end; padding-top: 4px; }
  .qs-mini-progress { width: 60px; }
  .qs-filter-label { min-width: 100%; }
}
</style>
