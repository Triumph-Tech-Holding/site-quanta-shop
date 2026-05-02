<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">CCR</div>
            <h1>Conta de Consumo Remunerada</h1>
            <p>Acompanhe seu extrato, solicite saques e veja o histórico de movimentações.</p>
          </div>
        </div>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>

          <!-- Summary KPIs -->
          <div class="fin-summary-grid">
            <div v-for="stat in summary" :key="stat.label" class="fin-kpi-card">
              <div class="fin-kpi-label">{{ stat.label }}</div>
              <div class="fin-kpi-valor">{{ stat.value }}</div>
            </div>
          </div>

          <!-- Tabs + Content -->
          <div class="qs-card-section fin-tabs-card">
            <div class="fin-tab-bar">
              <button
                v-for="tab in tabs" :key="tab.key"
                class="fin-tab-btn"
                :class="{ 'fin-tab-btn--ativo': activeTab === tab.key }"
                @click="activeTab = tab.key"
              >{{ tab.label }}</button>
            </div>

            <!-- Movimentações -->
            <div v-if="activeTab === 'movimentacoes'">
              <div class="qs-filter-bar">
                <form @submit.prevent="carregarMovimentacoes" class="fin-filter-form">
                  <div class="fin-filter-group">
                    <label class="qs-label">Data inicial</label>
                    <input type="date" v-model="filtroExtrato.dataInicio" class="qs-input" />
                  </div>
                  <div class="fin-filter-group">
                    <label class="qs-label">Data final</label>
                    <input type="date" v-model="filtroExtrato.dataFim" class="qs-input" />
                  </div>
                  <div class="fin-filter-group">
                    <label class="qs-label">Tipo</label>
                    <select v-model="filtroExtrato.tipo" class="qs-input">
                      <option value="">Todos</option>
                      <option value="credito">Crédito</option>
                      <option value="debito">Débito</option>
                    </select>
                  </div>
                  <button type="submit" class="qs-btn-primary fin-btn-filtrar">Filtrar</button>
                </form>
              </div>

              <div v-if="movimentacoesFiltradas.length === 0" class="ag-empty-state" style="min-height:100px;">
                <h5>Nenhuma movimentação encontrada</h5>
              </div>
              <div v-else class="fin-table-wrap">
                <table class="qs-table">
                  <thead>
                    <tr>
                      <th>Descrição</th>
                      <th class="tc">Data</th>
                      <th class="tr">Valor</th>
                      <th class="tc">Tipo</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(m, i) in movimentacoesFiltradas" :key="i">
                      <td>{{ m.descricao || '—' }}</td>
                      <td class="tc">{{ formatDate(String(m.data || m.dataCriacao || '')) }}</td>
                      <td class="tr" :class="Number(m.valor) >= 0 ? 'td-credito' : 'td-debito'">
                        {{ formatCurrency(Number(m.valor || 0)) }}
                      </td>
                      <td class="tc">
                        <span class="qs-badge" :class="Number(m.valor) >= 0 ? 'qs-badge--success' : 'qs-badge--danger'">
                          {{ Number(m.valor) >= 0 ? 'Crédito' : 'Débito' }}
                        </span>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>

            <!-- Saque -->
            <div v-if="activeTab === 'saque'" class="fin-saque-wrap">
              <div class="qs-section-title">Solicitar Saque</div>
              <form @submit.prevent="solicitarSaque" class="fin-saque-form">
                <div class="fin-filter-group" style="max-width:260px;">
                  <label class="qs-label">Valor do saque</label>
                  <input type="number" v-model="saque.valor" min="1" step="0.01" class="qs-input" placeholder="R$ 0,00" />
                </div>
                <button type="submit" class="qs-btn-primary" :disabled="saqueLoading">
                  {{ saqueLoading ? 'Solicitando...' : 'Solicitar Saque' }}
                </button>
              </form>
              <div v-if="saqueMensagem" class="fin-saque-msg" :class="saqueSucesso ? 'fin-saque-msg--ok' : 'fin-saque-msg--err'">
                {{ saqueMensagem }}
              </div>
            </div>

            <!-- Histórico -->
            <div v-if="activeTab === 'historico'">
              <div v-if="historico.length === 0" class="ag-empty-state" style="min-height:100px;">
                <h5>Nenhum histórico de saque</h5>
              </div>
              <div v-else class="fin-table-wrap">
                <table class="qs-table">
                  <thead>
                    <tr>
                      <th>Data solicitação</th>
                      <th class="tr">Valor</th>
                      <th class="tc">Status</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(s, i) in historico" :key="i">
                      <td>{{ formatDate(String(s.dataSolicitacao || s.data || '')) }}</td>
                      <td class="tr td-credito">{{ formatCurrency(Number(s.valor || 0)) }}</td>
                      <td class="tc">
                        <span class="qs-badge" :class="badgeClass(statusClass(String(s.status || s.statusNome || '')))">
                          {{ s.status || s.statusNome || '—' }}
                        </span>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
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
const loading = ref(true);
const activeTab = ref('movimentacoes');
const movimentacoes = ref<Record<string, unknown>[]>([]);
const historico = ref<Record<string, unknown>[]>([]);
const saqueLoading = ref(false);
const saqueMensagem = ref('');
const saqueSucesso = ref(false);

const saque = reactive({ valor: '' });

const now = new Date();
const firstDayOfMonth = new Date(now.getFullYear(), now.getMonth(), 1).toISOString().split('T')[0];
const lastDayOfMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0).toISOString().split('T')[0];
const filtroExtrato = reactive({ dataInicio: firstDayOfMonth, dataFim: lastDayOfMonth, tipo: '' });

const movimentacoesFiltradas = computed(() => {
  let list = movimentacoes.value;
  if (filtroExtrato.dataInicio) {
    const from = new Date(filtroExtrato.dataInicio);
    list = list.filter(m => new Date(String(m.data || m.dataCriacao || '')) >= from);
  }
  if (filtroExtrato.dataFim) {
    const to = new Date(filtroExtrato.dataFim + 'T23:59:59');
    list = list.filter(m => new Date(String(m.data || m.dataCriacao || '')) <= to);
  }
  if (filtroExtrato.tipo === 'credito') list = list.filter(m => Number(m.valor) >= 0);
  if (filtroExtrato.tipo === 'debito') list = list.filter(m => Number(m.valor) < 0);
  return list;
});

const summary = ref([
  { label: 'Saldo disponível', value: '—' },
  { label: 'Total recebido', value: '—' },
  { label: 'Total sacado', value: '—' },
]);

const tabs = [
  { key: 'movimentacoes', label: 'Movimentações' },
  { key: 'saque', label: 'Solicitar Saque' },
  { key: 'historico', label: 'Histórico de Saques' },
];

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

function formatDate(d: string): string {
  if (!d) return '—';
  return new Date(d).toLocaleDateString('pt-BR');
}

function statusClass(s: string): string {
  const lower = (s || '').toLowerCase();
  if (lower.includes('aprov') || lower.includes('pago') || lower.includes('conclu')) return 'aprovado';
  if (lower.includes('recus') || lower.includes('cancel')) return 'recusado';
  if (lower.includes('process') || lower.includes('andamento')) return 'processando';
  return 'pendente';
}

function badgeClass(cls: string): string {
  if (cls === 'aprovado') return 'qs-badge--success';
  if (cls === 'recusado') return 'qs-badge--danger';
  if (cls === 'processando') return 'qs-badge--info';
  return 'qs-badge--warn';
}

async function solicitarSaque() {
  if (!saque.valor || Number(saque.valor) <= 0) return;
  saqueLoading.value = true;
  saqueMensagem.value = '';
  try {
    await api.post('/Saque/solicitarSaque', { valor: Number(saque.valor) }, authHeader());
    saqueSucesso.value = true;
    saqueMensagem.value = 'Saque solicitado com sucesso!';
    saque.valor = '';
    await carregarHistorico();
  } catch (err: unknown) {
    saqueSucesso.value = false;
    saqueMensagem.value = (err as Record<string, Record<string, string>>)?.response?.data?.message || 'Erro ao solicitar saque.';
  } finally {
    saqueLoading.value = false;
  }
}

async function carregarMovimentacoes() {
  try {
    const body: Record<string, unknown> = {};
    if (filtroExtrato.dataInicio) body.dataInicio = new Date(filtroExtrato.dataInicio).toISOString();
    if (filtroExtrato.dataFim) body.dataFim = new Date(filtroExtrato.dataFim + 'T23:59:59').toISOString();
    try {
      const r = await api.post('/Extrato/buscarExtrato', body, authHeader());
      movimentacoes.value = Array.isArray(r.data) ? r.data : (r.data?.items ?? []);
    } catch {
      const r = await api.post('/financeiro/movimentacoes', body, authHeader());
      movimentacoes.value = Array.isArray(r.data) ? r.data : (r.data?.items ?? []);
    }
  } catch {
    movimentacoes.value = [];
  }
}

async function carregarHistorico() {
  try {
    const { data } = await api.get('/Saque/historicoSaque', authHeader());
    historico.value = Array.isArray(data) ? data : (data?.items ?? []);
  } catch {
    historico.value = [];
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await Promise.allSettled([
    api.get('/financeiro/resumo', authHeader()).then(r => {
      const d = r.data as Record<string, number> | null;
      if (d) {
        summary.value = [
          { label: 'Saldo disponível', value: formatCurrency(d.saldo ?? d.Saldo ?? 0) },
          { label: 'Total recebido', value: formatCurrency(d.totalRecebido ?? d.TotalRecebido ?? 0) },
          { label: 'Total sacado', value: formatCurrency(d.totalSacado ?? d.TotalSacado ?? 0) },
        ];
      }
    }).catch(() => {}),
    carregarMovimentacoes(),
    carregarHistorico(),
  ]);
  loading.value = false;
});
</script>

<style scoped>
/* Summary KPIs */
.fin-summary-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
  gap: .75rem;
  margin-bottom: 1.5rem;
}
.fin-kpi-card {
  background: #fff;
  border-radius: var(--qs-radius-md, 12px);
  box-shadow: var(--qs-shadow-sm, 0 2px 8px rgba(1,15,28,.08));
  padding: 1.125rem 1.25rem;
  transition: all .2s;
}
.fin-kpi-card:hover { box-shadow: var(--qs-shadow-md); transform: translateY(-2px); }
.fin-kpi-label {
  font-size: .625rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .07em;
  color: var(--qs-gray-400, #9ca3af);
  margin-bottom: .35rem;
}
.fin-kpi-valor {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--qs-teal-dark, #225F6B);
  letter-spacing: -0.02em;
}

/* Tabs */
.fin-tabs-card { background: #fff; }
.fin-tab-bar {
  display: flex;
  border-bottom: 2px solid var(--qs-gray-100, #f5f5f7);
  margin-bottom: 1.25rem;
  gap: 0;
  overflow-x: auto;
}
.fin-tab-btn {
  padding: .625rem 1.25rem;
  border: none;
  border-bottom: 2px solid transparent;
  background: transparent;
  font-size: .875rem;
  font-weight: 500;
  color: var(--qs-gray-500, #6b7280);
  cursor: pointer;
  white-space: nowrap;
  margin-bottom: -2px;
  transition: all .15s;
}
.fin-tab-btn--ativo {
  color: var(--qs-teal, #2F7785);
  border-bottom-color: var(--qs-teal, #2F7785);
  font-weight: 700;
}
.fin-tab-btn:hover:not(.fin-tab-btn--ativo) { color: var(--qs-ink, #1d1d1f); }

/* Filter bar */
.fin-filter-form {
  display: flex;
  flex-wrap: wrap;
  gap: .875rem;
  align-items: flex-end;
}
.fin-filter-group {
  display: flex;
  flex-direction: column;
  gap: .35rem;
  flex: 1;
  min-width: 140px;
}
.fin-btn-filtrar { flex-shrink: 0; }

/* Table */
.fin-table-wrap { overflow-x: auto; }
.qs-table {
  width: 100%;
  border-collapse: collapse;
  font-size: .875rem;
}
.qs-table thead tr {
  border-bottom: 2px solid var(--qs-gray-100, #f5f5f7);
}
.qs-table th {
  padding: .625rem .875rem;
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .06em;
  color: var(--qs-gray-500, #6b7280);
  white-space: nowrap;
}
.qs-table td {
  padding: .75rem .875rem;
  color: var(--qs-gray-700, #374151);
  border-bottom: 1px solid var(--qs-gray-100, #f5f5f7);
}
.qs-table tbody tr:hover td { background: var(--qs-gray-50, #fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }
.tr { text-align: right !important; }
.td-credito { color: var(--qs-lime-dark, #7aad1f) !important; font-weight: 600; }
.td-debito { color: var(--qs-danger, #dc2626) !important; font-weight: 600; }

/* Badges */
.qs-badge {
  display: inline-flex;
  align-items: center;
  padding: .2rem .55rem;
  border-radius: var(--qs-radius-pill, 999px);
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .04em;
  white-space: nowrap;
}
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--danger  { background: #fee2e2; color: #dc2626; }
.qs-badge--warn    { background: #fef9c3; color: #ca8a04; }
.qs-badge--info    { background: #dbeafe; color: #1d4ed8; }

/* Saque */
.fin-saque-wrap { display: flex; flex-direction: column; gap: 1rem; }
.fin-saque-form { display: flex; flex-wrap: wrap; gap: 1rem; align-items: flex-end; }
.fin-saque-msg {
  padding: .75rem 1rem;
  border-radius: var(--qs-radius-md, 12px);
  font-size: .875rem;
  max-width: 480px;
}
.fin-saque-msg--ok { background: #dcfce7; color: #16a34a; }
.fin-saque-msg--err { background: #fee2e2; color: #dc2626; }

/* Shared inputs */
.qs-label {
  font-size: .75rem;
  font-weight: 600;
  color: var(--qs-gray-700, #374151);
  text-transform: uppercase;
  letter-spacing: .04em;
}
.qs-input {
  width: 100%;
  padding: .625rem .875rem;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  font-size: .875rem;
  color: var(--qs-ink, #1d1d1f);
  background: #fff;
  transition: border-color .15s;
}
.qs-input:focus { outline: none; border-color: var(--qs-teal, #2F7785); }
</style>
