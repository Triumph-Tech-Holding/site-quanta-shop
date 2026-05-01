<template>
  <div>
    <div class="ag-page-header">
      <div style="display:flex;align-items:center;justify-content:space-between;flex-wrap:wrap;gap:.5rem;">
        <div>
          <h1>Progresso do Produto</h1>
          <p>Acompanhe o desenvolvimento da Quanta Shop em tempo real.</p>
        </div>
        <div style="display:flex;align-items:center;gap:.75rem;flex-shrink:0;">
          <span v-if="generatedAt" style="font-size:.75rem;color:#888;">
            Atualizado em {{ formatDate(generatedAt) }}
          </span>
          <button class="btn-ag-outline" @click="reload" :disabled="loading">
            <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 24 24" fill="currentColor" style="margin-right:.3rem"><path d="M17.65 6.35A7.958 7.958 0 0 0 12 4c-4.42 0-7.99 3.58-7.99 8s3.57 8 7.99 8c3.73 0 6.84-2.55 7.73-6h-2.08A5.99 5.99 0 0 1 12 18c-3.31 0-6-2.69-6-6s2.69-6 6-6c1.66 0 3.14.69 4.22 1.78L13 11h7V4l-2.35 2.35z"/></svg>
            Recarregar
          </button>
        </div>
      </div>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <template v-else>
      <div class="ag-card mb-4" style="padding:1.25rem 1.5rem;">
        <div style="display:flex;align-items:center;gap:.75rem;">
          <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" viewBox="0 0 24 24" fill="#2F7785"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/></svg>
          <div>
            <div style="font-size:1.1rem;font-weight:700;color:#225F6B;">
              {{ mergedCount }}/{{ totalCount }} concluídas
            </div>
            <div style="font-size:.8rem;color:#888;">Tarefas de Desenvolvimento</div>
          </div>
          <div style="flex:1;margin-left:.5rem;">
            <div style="background:#e9ecef;border-radius:99px;height:8px;overflow:hidden;">
              <div :style="{ width: progressPct + '%', background: '#98C73A', height: '100%', borderRadius: '99px', transition: 'width .4s' }"></div>
            </div>
            <div style="font-size:.7rem;color:#888;margin-top:.25rem;text-align:right;">{{ progressPct }}%</div>
          </div>
        </div>
      </div>

      <div class="ag-card" style="padding:0;overflow:hidden;">
        <div style="padding:.75rem 1.25rem;border-bottom:1px solid #f0f0f0;display:flex;align-items:center;justify-content:space-between;">
          <span style="font-weight:600;font-size:.9rem;color:#225F6B;">Todas as Tarefas</span>
          <div style="display:flex;gap:.5rem;flex-wrap:wrap;">
            <button
              v-for="f in filters"
              :key="f.key"
              class="badge-filter"
              :class="{ active: activeFilter === f.key }"
              @click="activeFilter = f.key"
            >{{ f.label }} ({{ f.count }})</button>
          </div>
        </div>

        <div class="progresso-list">
          <div
            v-for="task in filteredTasks"
            :key="task.taskRef"
            class="progresso-item"
          >
            <div class="progresso-check">
              <svg v-if="task.state === 'MERGED'" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="#98C73A"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
              <svg v-else-if="task.state === 'IMPLEMENTED'" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="#2F7785"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
              <svg v-else-if="task.state === 'IN_PROGRESS'" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="#f59e0b"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="#d1d5db"><circle cx="12" cy="12" r="10"/></svg>
            </div>
            <div class="progresso-ref">{{ task.taskRef }}</div>
            <div class="progresso-title">{{ task.title }}</div>
            <div class="progresso-badge" :class="badgeClass(task.state)">
              {{ badgeLabel(task.state) }}
            </div>
          </div>
          <div v-if="filteredTasks.length === 0" style="padding:2rem;text-align:center;color:#888;font-size:.9rem;">
            Nenhuma tarefa nessa categoria.
          </div>
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
  try {
    const data = await $fetch<TasksFile>('/docs/tasks.json', { cache: 'no-store' });
    tasks.value = data.tasks ?? [];
    generatedAt.value = data.generatedAt ?? '';
  } catch { tasks.value = []; } finally { loading.value = false; }
}

function reload() { load(); }

onMounted(load);

const mergedCount = computed(() => tasks.value.filter(t => t.state === 'MERGED').length);
const totalCount = computed(() => tasks.value.length);
const progressPct = computed(() => totalCount.value ? Math.round((mergedCount.value / totalCount.value) * 100) : 0);

const STATE_ORDER: Record<string, number> = { IN_PROGRESS: 0, IMPLEMENTED: 1, PENDING: 2, PROPOSED: 3, MERGED: 4, CANCELLED: 5 };
const sortedTasks = computed(() =>
  [...tasks.value].sort((a, b) => (STATE_ORDER[a.state] ?? 9) - (STATE_ORDER[b.state] ?? 9))
);

const filters = computed(() => {
  const counts: Record<string, number> = { all: tasks.value.length };
  for (const t of tasks.value) {
    const g = filterGroup(t.state);
    counts[g] = (counts[g] ?? 0) + 1;
  }
  return [
    { key: 'all', label: 'Todas', count: counts['all'] ?? 0 },
    { key: 'done', label: 'Concluídas', count: counts['done'] ?? 0 },
    { key: 'active', label: 'Ativas', count: counts['active'] ?? 0 },
    { key: 'draft', label: 'Rascunhos', count: counts['draft'] ?? 0 },
  ];
});

function filterGroup(state: string): string {
  if (state === 'MERGED' || state === 'IMPLEMENTED') return 'done';
  if (state === 'PENDING' || state === 'IN_PROGRESS') return 'active';
  return 'draft';
}

const filteredTasks = computed(() => {
  if (activeFilter.value === 'all') return sortedTasks.value;
  return sortedTasks.value.filter(t => filterGroup(t.state) === activeFilter.value);
});

function badgeLabel(state: string): string {
  const map: Record<string, string> = {
    MERGED: 'Concluído', IMPLEMENTED: 'Pronto', IN_PROGRESS: 'Em andamento',
    PENDING: 'Ativo', PROPOSED: 'Rascunho', CANCELLED: 'Arquivado',
  };
  return map[state] ?? state;
}

function badgeClass(state: string): string {
  if (state === 'MERGED') return 'badge-done';
  if (state === 'IMPLEMENTED') return 'badge-ready';
  if (state === 'IN_PROGRESS') return 'badge-progress';
  if (state === 'PENDING') return 'badge-active';
  if (state === 'PROPOSED') return 'badge-draft';
  return 'badge-draft';
}

function formatDate(iso: string): string {
  try {
    return new Date(iso).toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
  } catch { return iso; }
}
</script>

<style scoped>
.btn-ag-outline {
  display: inline-flex;
  align-items: center;
  padding: .35rem .85rem;
  font-size: .8rem;
  border: 1px solid #2F7785;
  color: #2F7785;
  background: #fff;
  border-radius: 6px;
  cursor: pointer;
  transition: background .15s, color .15s;
}
.btn-ag-outline:hover { background: #2F7785; color: #fff; }
.btn-ag-outline:disabled { opacity: .5; cursor: not-allowed; }

.badge-filter {
  font-size: .75rem;
  padding: .2rem .6rem;
  border-radius: 99px;
  border: 1px solid #d1d5db;
  background: #f9f9f9;
  color: #555;
  cursor: pointer;
  transition: all .15s;
}
.badge-filter.active, .badge-filter:hover {
  background: #2F7785;
  color: #fff;
  border-color: #2F7785;
}

.progresso-list { max-height: 65vh; overflow-y: auto; }

.progresso-item {
  display: flex;
  align-items: center;
  gap: .75rem;
  padding: .7rem 1.25rem;
  border-bottom: 1px solid #f5f5f5;
  transition: background .1s;
}
.progresso-item:last-child { border-bottom: none; }
.progresso-item:hover { background: #f9fbfc; }

.progresso-check { flex-shrink: 0; display: flex; align-items: center; }
.progresso-ref { font-size: .75rem; color: #aaa; flex-shrink: 0; min-width: 2.5rem; }
.progresso-title { flex: 1; font-size: .85rem; color: #333; line-height: 1.3; }

.progresso-badge {
  flex-shrink: 0;
  font-size: .7rem;
  font-weight: 600;
  padding: .2rem .55rem;
  border-radius: 99px;
}
.badge-done { background: #dcfce7; color: #16a34a; }
.badge-ready { background: #dbeafe; color: #1d4ed8; }
.badge-progress { background: #fef3c7; color: #d97706; }
.badge-active { background: #e0f2fe; color: #0369a1; }
.badge-draft { background: #f3f4f6; color: #6b7280; }
</style>
