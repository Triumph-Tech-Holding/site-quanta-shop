<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">Atendimento</div>
            <h1>Central de Suporte</h1>
            <p>Registre e acompanhe suas solicitações de atendimento.</p>
          </div>
          <NuxtLink to="/agencia/painel/solicitar-suporte" class="qs-btn-primary sup-btn-novo">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z"/></svg>
            Abrir chamado
          </NuxtLink>
        </div>

        <div class="qs-filter-bar">
          <form @submit.prevent="buscarSuporte" class="sup-filter-form">
            <div class="sup-filter-group">
              <label class="qs-label">Status</label>
              <select v-model="filtro.idStatus" class="qs-input">
                <option value="">Todos</option>
                <option v-for="s in statusOptions" :key="s.value" :value="s.value">{{ s.text }}</option>
              </select>
            </div>
            <div class="sup-filter-group">
              <label class="qs-label">Tipo</label>
              <select v-model="filtro.idTipo" class="qs-input">
                <option value="">Todos</option>
                <option v-for="t in tipoOptions" :key="t.value" :value="t.value">{{ t.text }}</option>
              </select>
            </div>
            <div class="sup-filter-group">
              <label class="qs-label">Abertura (início)</label>
              <input type="date" v-model="filtro.dataInicio" class="qs-input" />
            </div>
            <div class="sup-filter-group">
              <label class="qs-label">Abertura (fim)</label>
              <input type="date" v-model="filtro.dataFim" class="qs-input" />
            </div>
            <button type="submit" class="qs-btn-primary sup-btn-filtrar">Filtrar</button>
          </form>
        </div>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div class="qs-card-section sup-results-card">
            <div class="sup-results-header">
              <div class="qs-section-title">Solicitações</div>
              <span class="sup-count-badge">{{ items.length }} resultado(s)</span>
            </div>

            <div v-if="items.length === 0" class="ag-empty-state">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="var(--qs-gray-200,#e5e7eb)"><path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2z"/></svg>
              <h5>Nenhuma solicitação encontrada</h5>
              <p>Tente ajustar os filtros ou <NuxtLink to="/agencia/painel/solicitar-suporte" style="color:var(--qs-teal);">abra um novo chamado</NuxtLink></p>
            </div>

            <div v-else class="sup-table-wrap">
              <table class="qs-table">
                <thead>
                  <tr>
                    <th>#</th>
                    <th>Título</th>
                    <th>Tipo</th>
                    <th class="tc">Data abertura</th>
                    <th class="tc">Status</th>
                    <th class="tc">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, i) in items" :key="i">
                    <td class="sup-id">#{{ item.idSuporte }}</td>
                    <td>{{ item.titulo || item.assunto || '—' }}</td>
                    <td>{{ tipoLabel(Number(item.tipo)) }}</td>
                    <td class="tc">{{ formatDate(String(item.dataAbertura || item.dataCriacao || '')) }}</td>
                    <td class="tc">
                      <span class="qs-badge" :class="badgeClass(statusBadgeClass(Number(item.status)))">
                        {{ statusLabel(Number(item.status)) }}
                      </span>
                    </td>
                    <td class="tc">
                      <NuxtLink :to="`/agencia/painel/suporte/${item.idSuporte}`" class="sup-link">
                        Ver detalhes →
                      </NuxtLink>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(false);
const items = ref<Record<string, unknown>[]>([]);

const statusOptions = [
  { value: 1, text: 'Em processamento' },
  { value: 2, text: 'Finalizado' },
  { value: 3, text: 'Cancelado' },
  { value: 4, text: 'Em aprovação' },
  { value: 5, text: 'Recusado' },
  { value: 6, text: 'Aprovado' },
  { value: 7, text: 'Aguardando pagamento de fatura' },
];
const tipoOptions = [
  { value: 1, text: 'Contato' },
  { value: 2, text: 'Cashback não pago' },
  { value: 3, text: 'Cancelamento do parcelamento' },
  { value: 4, text: 'Reabertura do parcelamento' },
];

const filtro = reactive({ idStatus: '', idTipo: '', dataInicio: '', dataFim: '' });

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string): string { if (!d) return '—'; return new Date(d).toLocaleDateString('pt-BR'); }

const TIPO_MAP: Record<number, string> = {
  1: 'Contato', 2: 'Cashback não pago', 3: 'Cancelamento do parcelamento', 4: 'Reabertura do parcelamento',
};
function tipoLabel(code: number): string { return TIPO_MAP[code] ?? String(code || '—'); }

const STATUS_MAP: Record<number, { label: string; cls: string }> = {
  1: { label: 'Em processamento', cls: 'warn' },
  2: { label: 'Finalizado', cls: 'success' },
  3: { label: 'Cancelado', cls: 'danger' },
  4: { label: 'Em aprovação', cls: 'info' },
  5: { label: 'Recusado', cls: 'danger' },
  6: { label: 'Aprovado', cls: 'success' },
  7: { label: 'Aguardando pagamento', cls: 'warn' },
};
function statusLabel(code: number): string { return STATUS_MAP[code]?.label ?? String(code || '—'); }
function statusBadgeClass(code: number): string { return STATUS_MAP[code]?.cls ?? 'warn'; }
function badgeClass(cls: string): string {
  if (cls === 'success') return 'qs-badge--success';
  if (cls === 'danger') return 'qs-badge--danger';
  if (cls === 'info') return 'qs-badge--info';
  return 'qs-badge--warn';
}

async function buscarSuporte() {
  loading.value = true;
  try {
    const body: Record<string, unknown> = {};
    if (filtro.idStatus) body.idStatus = filtro.idStatus;
    if (filtro.idTipo) body.idTipo = filtro.idTipo;
    if (filtro.dataInicio) body.dataInicio = new Date(filtro.dataInicio).toISOString();
    if (filtro.dataFim) body.dataFim = new Date(filtro.dataFim + 'T23:59:59').toISOString();
    const { data } = await api.post('/Suporte/listaSuporte', body, authHeader());
    items.value = Array.isArray(data) ? data : [];
  } catch {
    try {
      const { data } = await api.get('/Suporte/listaSuporte', authHeader());
      items.value = Array.isArray(data) ? data : [];
    } catch { items.value = []; }
  } finally { loading.value = false; }
}

onMounted(() => { agenciaStore.loadFromStorage(); buscarSuporte(); });
</script>

<style scoped>
.sup-btn-novo {
  display: inline-flex;
  align-items: center;
  gap: .4rem;
  flex-shrink: 0;
  font-size: .875rem;
}
.sup-btn-novo svg { width: 16px; height: 16px; }

.sup-filter-form { display: flex; flex-wrap: wrap; gap: .875rem; align-items: flex-end; }
.sup-filter-group { display: flex; flex-direction: column; gap: .35rem; flex: 1; min-width: 150px; }
.sup-btn-filtrar { flex-shrink: 0; }

.sup-results-card { background: #fff; }
.sup-results-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 1rem; flex-wrap: wrap; gap: .5rem; }
.sup-count-badge { font-size: .75rem; font-weight: 600; background: var(--qs-gray-100,#f5f5f7); color: var(--qs-gray-500,#6b7280); padding: .2rem .625rem; border-radius: var(--qs-radius-pill,999px); }

.sup-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: .875rem; }
.qs-table thead tr { border-bottom: 2px solid var(--qs-gray-100,#f5f5f7); }
.qs-table th { padding: .625rem .875rem; font-size: .6875rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-500,#6b7280); white-space: nowrap; }
.qs-table td { padding: .75rem .875rem; color: var(--qs-gray-700,#374151); border-bottom: 1px solid var(--qs-gray-100,#f5f5f7); }
.qs-table tbody tr:hover td { background: var(--qs-gray-50,#fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }
.sup-id { color: var(--qs-gray-400,#9ca3af); font-size: .8125rem; font-variant-numeric: tabular-nums; }
.sup-link { color: var(--qs-teal,#2F7785); font-weight: 600; font-size: .8125rem; text-decoration: none; }
.sup-link:hover { text-decoration: underline; }

.qs-badge { display: inline-flex; padding: .2rem .55rem; border-radius: var(--qs-radius-pill,999px); font-size: .6875rem; font-weight: 700; text-transform: uppercase; letter-spacing: .04em; white-space: nowrap; }
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--danger  { background: #fee2e2; color: #dc2626; }
.qs-badge--warn    { background: #fef9c3; color: #ca8a04; }
.qs-badge--info    { background: #dbeafe; color: #1d4ed8; }

.qs-label { font-size: .75rem; font-weight: 600; color: var(--qs-gray-700,#374151); text-transform: uppercase; letter-spacing: .04em; }
.qs-input { width: 100%; padding: .625rem .875rem; border: 1.5px solid var(--qs-gray-200,#e5e7eb); border-radius: var(--qs-radius-md,12px); font-size: .875rem; color: var(--qs-ink,#1d1d1f); background: #fff; transition: border-color .15s; box-sizing: border-box; }
.qs-input:focus { outline: none; border-color: var(--qs-teal,#2F7785); }
</style>
