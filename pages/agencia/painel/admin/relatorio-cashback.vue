<template>
  <div class="qs-page no-print">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Relatórios</div>
        <h1>Relatório de Cashback</h1>
        <p>Cashbacks concedidos pela plataforma por período</p>
      </div>
      <div class="qs-header-actions no-print">
        <button class="qs-btn-outline qs-btn-sm" :disabled="itens.length === 0" @click="gerarPdf">⬇ PDF</button>
        <button class="qs-btn-primary qs-btn-sm" :disabled="loading" @click="carregar">
          <span v-if="loading" class="qs-spinner-sm" />
          Atualizar
        </button>
      </div>
    </div>

    <div class="qs-filter-bar no-print">
      <span class="qs-filter-bar-label">Período:</span>
      <QsFilterChip
        v-for="preset in periodoPresets"
        :key="preset.id"
        :label="preset.label"
        :active="periodoAtivo === preset.id"
        @click="aplicarPreset(preset)"
      />
      <div class="qs-filter-date-range">
        <label class="qs-filter-label">De</label>
        <input v-model="filtro.dataInicial" type="date" class="qs-input-sm" @change="periodoAtivo = 'custom'" />
        <label class="qs-filter-label">Até</label>
        <input v-model="filtro.dataFinal" type="date" class="qs-input-sm" @change="periodoAtivo = 'custom'" />
      </div>
    </div>

    <div class="qs-kpi-strip">
      <QsKpiCard label="Total Concedido" :value="totais.totalConcedido" format="currency" dotColor="#2F7785" />
      <QsKpiCard label="Total de Lançamentos" :value="totais.totalLancamentos" format="number" />
      <QsKpiCard label="Períodos" :value="itens.length" format="number" />
    </div>

    <template v-if="loading">
      <div class="qs-card-section">
        <div class="qs-skeleton-table">
          <div v-for="n in 5" :key="n" class="qs-skeleton-row">
            <div class="qs-skeleton qs-sk-wide" />
            <div class="qs-skeleton qs-sk-md" />
            <div class="qs-skeleton qs-sk-md" />
            <div class="qs-skeleton qs-sk-sm" />
          </div>
        </div>
      </div>
    </template>

    <template v-else>
      <div class="qs-card-section">
        <div v-if="itens.length === 0" class="qs-empty-state">
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><rect x="2" y="5" width="20" height="14" rx="2"/><line x1="2" y1="10" x2="22" y2="10"/></svg>
          <h3>Nenhum registro encontrado para o período</h3>
          <p class="qs-empty-hint">Selecione um intervalo de datas e clique em Atualizar.</p>
        </div>

        <div v-for="periodo in itens" :key="periodo.data" class="qs-periodo-block">
          <h6 class="qs-periodo-title">{{ formatMes(periodo.data) }}</h6>
          <div class="qs-table-wrap">
            <table class="qs-table">
              <thead>
                <tr>
                  <th>Tipo Lançamento</th>
                  <th>Tipo Pedido</th>
                  <th>Status</th>
                  <th class="qs-text-right">Valor</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(lanc, idx) in periodo.Lancamentos" :key="idx">
                  <td>{{ lanc.TipoLancamento || '—' }}</td>
                  <td>{{ lanc.DescricaoTipoPedido || '—' }}</td>
                  <td><span class="qs-badge" :class="badgeStatus(lanc.StatusPagamento)">{{ lanc.StatusPagamento || '—' }}</span></td>
                  <td class="qs-text-right qs-cell-bold qs-num-teal">{{ formatCurrency(lanc.Valor) }}</td>
                </tr>
              </tbody>
              <tfoot>
                <tr>
                  <td colspan="3" class="qs-text-right qs-fw-bold">Subtotal</td>
                  <td class="qs-text-right qs-fw-bold qs-num-teal">{{ formatCurrency(periodo.Lancamentos.reduce((s: number, l: any) => s + (l.Valor || 0), 0)) }}</td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>
      </div>
    </template>

    <div v-if="erro" class="qs-alert-danger">{{ erro }}</div>
  </div>
</template>

<style>
@media print {
  .no-print, nav, aside, header, .agencia-sidebar { display: none !important; }
  body { background: white !important; }
}
</style>

<script setup lang="ts">
import { useAgenciaStore } from '~/pinia/useAgenciaStore';
import { useApi } from '~/composables/useApi';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(false);
const erro = ref('');
const itens = ref<any[]>([]);
const periodoAtivo = ref('3m');

const hoje = new Date();
const filtro = reactive({
  dataInicial: new Date(hoje.getFullYear(), hoje.getMonth() - 2, 1).toISOString().split('T')[0],
  dataFinal: hoje.toISOString().split('T')[0],
});

const periodoPresets = [
  { id: 'mes', label: 'Mês atual', mesesAtras: 0 },
  { id: '3m', label: '3 meses', mesesAtras: 2 },
  { id: '6m', label: '6 meses', mesesAtras: 5 },
  { id: '12m', label: '12 meses', mesesAtras: 11 },
];

function aplicarPreset(preset: typeof periodoPresets[0]) {
  periodoAtivo.value = preset.id;
  filtro.dataInicial = new Date(hoje.getFullYear(), hoje.getMonth() - preset.mesesAtras, 1).toISOString().split('T')[0];
  filtro.dataFinal = hoje.toISOString().split('T')[0];
  carregar();
}

const totais = computed(() => ({
  totalConcedido: itens.value.reduce((s, p) => s + p.Lancamentos.reduce((ss: number, l: any) => ss + (l.Valor || 0), 0), 0),
  totalLancamentos: itens.value.reduce((s, p) => s + p.Lancamentos.length, 0),
}));

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatMes(dateStr: string) {
  if (!dateStr) return '—';
  const d = new Date(dateStr);
  return d.toLocaleDateString('pt-BR', { month: 'long', year: 'numeric' }).replace(/^\w/, c => c.toUpperCase());
}
function badgeStatus(status: string) {
  if (!status) return '';
  const s = status.toLowerCase();
  if (s.includes('pago') || s.includes('conclu')) return 'qs-badge-success';
  if (s.includes('pendente') || s.includes('aguard')) return 'qs-badge-warn';
  if (s.includes('cancel') || s.includes('recus')) return 'qs-badge-danger';
  return 'qs-badge-neutral';
}
function gerarPdf() { window.print(); }

async function carregar() {
  loading.value = true; erro.value = '';
  try {
    const params: Record<string, string> = {};
    if (filtro.dataInicial) params.dataInicial = filtro.dataInicial;
    if (filtro.dataFinal) params.datafinal = filtro.dataFinal;
    const { data } = await api.get('/Compra/relatorioMensalCashback', { ...authHeader(), params });
    itens.value = Array.isArray(data) ? data : [];
  } catch (e: any) {
    erro.value = e?.response?.data?.erros?.[0]?.mensagem || 'Erro ao carregar relatório de cashback.';
  } finally { loading.value = false; }
}
onMounted(() => { agenciaStore.loadFromStorage(); carregar(); });
</script>

<style scoped>
.qs-kpi-strip { display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; margin-bottom: 20px; }
@media (max-width: 640px) { .qs-kpi-strip { grid-template-columns: 1fr; } }

.qs-filter-bar { display: flex; align-items: center; gap: 8px; margin-bottom: 20px; flex-wrap: wrap; }
.qs-filter-bar-label { font-size: 12px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.05em; margin-right: 4px; }
.qs-filter-date-range { display: flex; align-items: center; gap: 6px; margin-left: 8px; }
.qs-filter-label { font-size: 11px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; }
.qs-input-sm { padding: 6px 10px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 13px; outline: none; }
.qs-input-sm:focus { border-color: var(--qs-teal); }
.qs-btn-sm { padding: 7px 14px; font-size: 13px; }

.qs-skeleton-table { display: flex; flex-direction: column; gap: 12px; padding: 8px 0; }
.qs-skeleton-row { display: flex; gap: 16px; align-items: center; }
.qs-sk-wide { flex: 2; height: 18px; border-radius: 6px; }
.qs-sk-md { flex: 1.5; height: 18px; border-radius: 6px; }
.qs-sk-sm { flex: 1; height: 18px; border-radius: 6px; }

.qs-periodo-block { margin-bottom: 28px; }
.qs-periodo-title { font-size: 14px; font-weight: 700; color: var(--qs-teal-dark); margin-bottom: 10px; text-transform: capitalize; }
.qs-text-right { text-align: right; }
.qs-fw-bold { font-weight: 700; }
.qs-num-teal { color: var(--qs-teal-dark); font-variant-numeric: tabular-nums; }
.qs-empty-hint { font-size: 13px; color: var(--qs-gray-400); margin: 0; }
.qs-spinner-sm { display: inline-block; width: 13px; height: 13px; border: 2px solid rgba(255,255,255,.4); border-top-color: #fff; border-radius: 50%; animation: spin .7s linear infinite; vertical-align: middle; margin-right: 5px; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-top: 16px; }
@keyframes spin { to { transform: rotate(360deg); } }
</style>
