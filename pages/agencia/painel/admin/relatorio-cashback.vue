<template>
  <div class="qs-page no-print">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Relatórios</div>
        <h1>Relatório de Cashback</h1>
        <p>Cashbacks concedidos pela plataforma por período</p>
      </div>
      <div class="qs-header-actions no-print">
        <div class="qs-period-switch">
          <QsFilterChip
            v-for="preset in periodoPresets"
            :key="preset.id"
            :active="periodoAtivo === preset.id"
            @click="aplicarPreset(preset)"
          >{{ preset.label }}</QsFilterChip>
        </div>
        <button class="qs-btn-outline" :disabled="itens.length === 0" @click="gerarPdf">⬇ PDF</button>
        <button class="qs-btn-primary" :disabled="loading" @click="carregar">Atualizar</button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <div class="qs-grid">
        <QsKpiCard label="Total Concedido" :value="totais.totalConcedido" format="currency" dot-color="var(--qs-teal)" />
        <QsKpiCard label="Total de Lançamentos" :value="totais.totalLancamentos" format="number" />
        <QsKpiCard label="Períodos" :value="itens.length" format="number" />
      </div>

      <section class="qs-card-section">
        <div class="section-head-row">
          <div>
            <h2 class="qs-section-title">Lançamentos por período</h2>
            <p class="qs-section-desc">Detalhamento por mês de referência — datas personalizadas abaixo.</p>
          </div>
          <div class="date-range-inline no-print">
            <label class="date-label">De</label>
            <input v-model="filtro.dataInicial" type="date" class="date-input" @change="periodoAtivo = 'custom'" />
            <label class="date-label">Até</label>
            <input v-model="filtro.dataFinal" type="date" class="date-input" @change="periodoAtivo = 'custom'" />
            <button class="qs-btn-sm-outline" @click="carregar">Filtrar</button>
          </div>
        </div>

        <div v-if="itens.length === 0" class="qs-empty-state">
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" color="var(--qs-gray-400)"><rect x="2" y="5" width="20" height="14" rx="2"/><line x1="2" y1="10" x2="22" y2="10"/></svg>
          <h3>Nenhum registro encontrado para o período</h3>
          <p>Selecione um intervalo de datas e clique em Atualizar.</p>
        </div>

        <div v-else>
          <div v-for="periodo in itens" :key="periodo.data" class="periodo-block">
            <h6 class="periodo-title">{{ formatMes(periodo.data) }}</h6>
            <div class="qs-table-wrap">
              <table class="qs-table">
                <thead>
                  <tr>
                    <th>Tipo Lançamento</th>
                    <th>Tipo Pedido</th>
                    <th>Status</th>
                    <th class="align-right">Valor</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(lanc, idx) in periodo.Lancamentos" :key="idx">
                    <td>{{ lanc.TipoLancamento || '—' }}</td>
                    <td>{{ lanc.DescricaoTipoPedido || '—' }}</td>
                    <td><span class="qs-badge" :class="badgeStatus(lanc.StatusPagamento)">{{ lanc.StatusPagamento || '—' }}</span></td>
                    <td class="align-right cell-bold num-teal">{{ formatCurrency(lanc.Valor) }}</td>
                  </tr>
                </tbody>
                <tfoot>
                  <tr>
                    <td colspan="3" class="align-right fw-bold">Subtotal</td>
                    <td class="align-right fw-bold num-teal">{{ formatCurrency(periodo.Lancamentos.reduce((s: number, l: any) => s + (l.Valor || 0), 0)) }}</td>
                  </tr>
                </tfoot>
              </table>
            </div>
          </div>
        </div>
      </section>
    </template>

    <div v-if="erro" class="alert-danger">{{ erro }}</div>
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
  if (!status) return 'qs-badge-neutral';
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
.qs-header-text { max-width: 720px; }
.qs-header-actions { display: flex; align-items: center; gap: 12px; flex-shrink: 0; flex-wrap: wrap; }
.qs-period-switch { display: flex; gap: 6px; }

/* ── Section head with date range ── */
.section-head-row {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 20px;
  flex-wrap: wrap;
  margin-bottom: 20px;
}
.section-head-row .qs-section-desc { margin-bottom: 0; }
.date-range-inline { display: flex; align-items: center; gap: 8px; flex-wrap: wrap; }
.date-label { font-size: 11px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; white-space: nowrap; }
.date-input {
  padding: 7px 10px;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  font-size: 13px; outline: none;
  font-family: inherit;
}
.date-input:focus { border-color: var(--qs-teal); }

/* ── Período block ── */
.periodo-block { margin-bottom: 28px; }
.periodo-block:last-child { margin-bottom: 0; }
.periodo-title {
  font-size: 13px; font-weight: 700;
  color: var(--qs-teal-dark);
  text-transform: capitalize;
  margin-bottom: 10px;
  padding-bottom: 8px;
  border-bottom: 1px solid var(--qs-gray-100);
}

/* ── Table ── */
.qs-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: 14px; }
.qs-table thead th {
  background: var(--qs-gray-50);
  color: var(--qs-gray-500);
  font-size: 11px; font-weight: 700;
  text-transform: uppercase; letter-spacing: 0.06em;
  padding: 10px 14px;
  border-bottom: 1px solid var(--qs-gray-200);
  white-space: nowrap;
}
.qs-table tbody td { padding: 11px 14px; border-bottom: 1px solid var(--qs-gray-100); vertical-align: middle; color: var(--qs-ink); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.qs-table tbody tr:hover td { background: var(--qs-gray-50); }
.qs-table tfoot td { padding: 10px 14px; border-top: 2px solid var(--qs-gray-200); background: var(--qs-gray-50); }
.align-right { text-align: right; }
.fw-bold { font-weight: 700; }
.cell-bold { font-weight: 600; }
.num-teal { color: var(--qs-teal-dark); font-variant-numeric: tabular-nums; }

/* ── Badges ── */
.qs-badge {
  display: inline-block;
  padding: 3px 10px;
  border-radius: var(--qs-radius-pill);
  font-size: 11px; font-weight: 700;
  letter-spacing: 0.02em; white-space: nowrap;
}
.qs-badge-success { background: #dcfce7; color: #16a34a; }
.qs-badge-warn    { background: #fef3c7; color: #d97706; }
.qs-badge-danger  { background: #fee2e2; color: #dc2626; }
.qs-badge-neutral { background: var(--qs-gray-100); color: var(--qs-gray-500); }

/* ── Small button ── */
.qs-btn-sm-outline {
  padding: 6px 12px; font-size: 12px; font-weight: 600;
  border: 1px solid var(--qs-teal); color: var(--qs-teal); background: #fff;
  border-radius: var(--qs-radius-md); cursor: pointer;
  transition: all var(--qs-duration) var(--qs-ease); white-space: nowrap;
}
.qs-btn-sm-outline:hover { background: var(--qs-teal); color: #fff; }
.qs-btn-sm-outline:disabled { opacity: 0.5; cursor: not-allowed; }

/* ── Empty state ── */
.qs-empty-state {
  display: flex; flex-direction: column; align-items: center;
  gap: 12px; padding: 56px 24px; text-align: center;
}
.qs-empty-state h3 { font-size: 18px; font-weight: 600; color: var(--qs-ink); margin: 0; }
.qs-empty-state p { font-size: 14px; color: var(--qs-gray-500); margin: 0; }

/* ── Alert ── */
.alert-danger {
  background: #fef2f2; color: var(--qs-danger);
  border: 1px solid #fecaca;
  border-radius: var(--qs-radius-md);
  padding: 12px 16px; font-size: 14px; margin-top: 16px;
}
</style>
