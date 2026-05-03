<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Produto</div>
        <h1>Progresso do Produto</h1>
        <p>Acompanhe o desenvolvimento da Quanta Shop em tempo real.</p>
      </div>
      <div class="qs-header-actions">
        <span v-if="generatedAt" class="qs-meta-text">Atualizado em {{ formatDate(generatedAt) }}</span>
        <button class="qs-btn-outline" @click="reload" :disabled="loading">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M17.65 6.35A7.958 7.958 0 0 0 12 4c-4.42 0-7.99 3.58-7.99 8s3.57 8 7.99 8c3.73 0 6.84-2.55 7.73-6h-2.08A5.99 5.99 0 0 1 12 18c-3.31 0-6-2.69-6-6s2.69-6 6-6c1.66 0 3.14.69 4.22 1.78L13 11h7V4l-2.35 2.35z"/></svg>
          Recarregar
        </button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <div class="qs-card-section qs-progress-summary">
        <div class="qs-progress-info">
          <svg width="22" height="22" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/></svg>
          <div>
            <div class="qs-progress-count">{{ mergedCount }}/{{ totalCount }} concluídas</div>
            <div class="qs-progress-sub">Tarefas de Desenvolvimento</div>
          </div>
        </div>
        <div class="qs-progress-bar-wrap">
          <div class="qs-progress-bar-track">
            <div class="qs-progress-bar-fill" :style="{ width: progressPct + '%' }"></div>
          </div>
          <span class="qs-progress-pct">{{ progressPct }}%</span>
        </div>
      </div>

      <div class="qs-card-section" style="padding:0;overflow:hidden">
        <div class="qs-tasks-toolbar">
          <span class="qs-tasks-title">Todas as Tarefas</span>
          <div class="qs-filter-chips">
            <button v-for="f in filters" :key="f.key" class="qs-filter-chip-sm" :class="{ active: activeFilter === f.key }" @click="activeFilter = f.key">
              {{ f.label }} ({{ f.count }})
            </button>
          </div>
        </div>

        <div class="qs-tasks-list">
          <div v-for="task in filteredTasks" :key="task.taskRef" class="qs-task-item">
            <div class="qs-task-icon">
              <svg v-if="task.state === 'MERGED'" width="18" height="18" viewBox="0 0 24 24" fill="var(--qs-lime)"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
              <svg v-else-if="task.state === 'IMPLEMENTED'" width="18" height="18" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
              <svg v-else-if="task.state === 'IN_PROGRESS'" width="18" height="18" viewBox="0 0 24 24" fill="#f59e0b"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
              <svg v-else width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="2"><circle cx="12" cy="12" r="10"/></svg>
            </div>
            <div class="qs-task-ref">{{ task.taskRef }}</div>
            <div class="qs-task-title">{{ task.title }}</div>
            <span class="qs-task-badge" :class="badgeClass(task.state)">{{ badgeLabel(task.state) }}</span>
          </div>
          <div v-if="filteredTasks.length === 0" class="qs-empty-inline">Nenhuma tarefa nessa categoria.</div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
interface Task { taskRef: string; title: string; state: string; updatedAt: string; }
interface TasksFile { generatedAt: string; tasks: Task[]; }
const loading = ref(true);
const tasks = ref<Task[]>([]);
const generatedAt = ref('');
const activeFilter = ref('all');
async function load() {
  loading.value = true;
  try { const data = await $fetch<TasksFile>('/docs/tasks.json', { cache: 'no-store' }); tasks.value = data.tasks ?? []; generatedAt.value = data.generatedAt ?? ''; }
  catch { tasks.value = []; } finally { loading.value = false; }
}
function reload() { load(); }
onMounted(load);
const mergedCount = computed(() => tasks.value.filter(t => t.state === 'MERGED').length);
const totalCount = computed(() => tasks.value.length);
const progressPct = computed(() => totalCount.value ? Math.round((mergedCount.value / totalCount.value) * 100) : 0);
const STATE_ORDER: Record<string, number> = { IN_PROGRESS: 0, IMPLEMENTED: 1, PENDING: 2, PROPOSED: 3, MERGED: 4, CANCELLED: 5 };
const sortedTasks = computed(() => [...tasks.value].sort((a, b) => (STATE_ORDER[a.state] ?? 9) - (STATE_ORDER[b.state] ?? 9)));
function filterGroup(state: string): string {
  if (state === 'MERGED' || state === 'IMPLEMENTED') return 'done';
  if (state === 'PENDING' || state === 'IN_PROGRESS') return 'active';
  return 'draft';
}
const filters = computed(() => {
  const counts: Record<string, number> = { all: tasks.value.length };
  for (const t of tasks.value) { const g = filterGroup(t.state); counts[g] = (counts[g] ?? 0) + 1; }
  return [{ key: 'all', label: 'Todas', count: counts['all'] ?? 0 }, { key: 'done', label: 'Concluídas', count: counts['done'] ?? 0 }, { key: 'active', label: 'Ativas', count: counts['active'] ?? 0 }, { key: 'draft', label: 'Rascunhos', count: counts['draft'] ?? 0 }];
});
const filteredTasks = computed(() => { if (activeFilter.value === 'all') return sortedTasks.value; return sortedTasks.value.filter(t => filterGroup(t.state) === activeFilter.value); });
function badgeLabel(state: string): string {
  const map: Record<string, string> = { MERGED: 'Concluído', IMPLEMENTED: 'Pronto', IN_PROGRESS: 'Em andamento', PENDING: 'Ativo', PROPOSED: 'Rascunho', CANCELLED: 'Arquivado' };
  return map[state] ?? state;
}
function badgeClass(state: string): string {
  if (state === 'MERGED') return 'tb-done';
  if (state === 'IMPLEMENTED') return 'tb-ready';
  if (state === 'IN_PROGRESS') return 'tb-progress';
  if (state === 'PENDING') return 'tb-active';
  return 'tb-draft';
}
function formatDate(iso: string): string {
  try { return new Date(iso).toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' }); } catch { return iso; }
}
</script>

<style scoped>
.qs-meta-text { font-size: 12px; color: var(--qs-gray-400); }
.qs-progress-summary { display: flex; align-items: center; gap: 24px; flex-wrap: wrap; padding: 20px 24px; margin-bottom: 20px; }
.qs-progress-info { display: flex; align-items: center; gap: 12px; flex-shrink: 0; }
.qs-progress-count { font-size: 18px; font-weight: 700; color: var(--qs-teal-dark); }
.qs-progress-sub { font-size: 12px; color: var(--qs-gray-400); margin-top: 2px; }
.qs-progress-bar-wrap { flex: 1; display: flex; align-items: center; gap: 12px; min-width: 160px; }
.qs-progress-bar-track { flex: 1; height: 8px; background: var(--qs-gray-100); border-radius: 99px; overflow: hidden; }
.qs-progress-bar-fill { height: 100%; background: var(--qs-lime); border-radius: 99px; transition: width 0.4s; }
.qs-progress-pct { font-size: 13px; font-weight: 700; color: var(--qs-gray-500); flex-shrink: 0; }
.qs-tasks-toolbar { display: flex; align-items: center; justify-content: space-between; gap: 12px; padding: 12px 20px; border-bottom: 1px solid var(--qs-gray-100); flex-wrap: wrap; }
.qs-tasks-title { font-size: 14px; font-weight: 600; color: var(--qs-teal-dark); }
.qs-filter-chips { display: flex; gap: 6px; flex-wrap: wrap; }
.qs-filter-chip-sm { font-size: 12px; padding: 4px 10px; border-radius: 99px; border: 1px solid var(--qs-gray-200); background: var(--qs-gray-50); color: var(--qs-gray-500); cursor: pointer; transition: all 0.15s; }
.qs-filter-chip-sm:hover, .qs-filter-chip-sm.active { background: var(--qs-teal); color: #fff; border-color: var(--qs-teal); }
.qs-tasks-list { max-height: 65vh; overflow-y: auto; }
.qs-task-item { display: flex; align-items: center; gap: 12px; padding: 11px 20px; border-bottom: 1px solid var(--qs-gray-50); transition: background 0.1s; }
.qs-task-item:last-child { border-bottom: none; }
.qs-task-item:hover { background: var(--qs-gray-50); }
.qs-task-icon { flex-shrink: 0; display: flex; align-items: center; }
.qs-task-ref { font-size: 12px; color: var(--qs-gray-400); flex-shrink: 0; min-width: 36px; }
.qs-task-title { flex: 1; font-size: 13px; color: var(--qs-gray-700); line-height: 1.3; }
.qs-task-badge { flex-shrink: 0; font-size: 11px; font-weight: 600; padding: 3px 9px; border-radius: 99px; }
.tb-done { background: #dcfce7; color: #16a34a; }
.tb-ready { background: #dbeafe; color: #1d4ed8; }
.tb-progress { background: #fef3c7; color: #d97706; }
.tb-active { background: #e0f2fe; color: #0369a1; }
.tb-draft { background: var(--qs-gray-100); color: var(--qs-gray-500); }
.qs-empty-inline { padding: 32px; text-align: center; color: var(--qs-gray-400); font-size: 14px; }
</style>
