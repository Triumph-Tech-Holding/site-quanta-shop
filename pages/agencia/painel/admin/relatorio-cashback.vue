<template>
  <div class="qs-page no-print">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Relatórios</div>
        <h1>Relatório de Cashback</h1>
        <p>Cashbacks concedidos pela plataforma por período</p>
      </div>
      <div class="qs-header-actions qs-filter-inline">
        <div class="qs-filter-field">
          <label class="qs-filter-label">De</label>
          <input v-model="filtro.dataInicial" type="date" class="qs-input-sm" />
        </div>
        <div class="qs-filter-field">
          <label class="qs-filter-label">Até</label>
          <input v-model="filtro.dataFinal" type="date" class="qs-input-sm" />
        </div>
        <button class="qs-btn-primary qs-btn-sm" :disabled="loading" @click="carregar">
          <span v-if="loading" class="qs-spinner-sm" />
          Atualizar
        </button>
        <button class="qs-btn-outline qs-btn-sm" :disabled="itens.length === 0" @click="gerarPdf">
          ⬇ PDF
        </button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <div class="qs-grid qs-grid-3 qs-kpi-row">
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Total Concedido</div>
          <div class="qs-kpi-value">{{ formatCurrency(totais.totalConcedido) }}</div>
        </div>
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Lançamentos</div>
          <div class="qs-kpi-value">{{ totais.totalLancamentos }}</div>
        </div>
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Períodos</div>
          <div class="qs-kpi-value">{{ itens.length }}</div>
        </div>
      </div>

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
const hoje = new Date();
const primeiroDiaMes = new Date(hoje.getFullYear(), hoje.getMonth() - 2, 1).toISOString().split('T')[0];
const filtro = reactive({ dataInicial: primeiroDiaMes, dataFinal: hoje.toISOString().split('T')[0] });
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
.qs-filter-inline { display: flex; align-items: flex-end; gap: 10px; flex-wrap: wrap; }
.qs-filter-field { display: flex; flex-direction: column; gap: 4px; }
.qs-filter-label { font-size: 11px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; }
.qs-input-sm { padding: 7px 10px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 13px; outline: none; }
.qs-input-sm:focus { border-color: var(--qs-teal); }
.qs-btn-sm { padding: 7px 14px; font-size: 13px; }
.qs-kpi-row { margin-bottom: 24px; }
.qs-grid-3 { display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; }
.qs-kpi-card { background: #fff; border: 1px solid var(--qs-gray-100); border-radius: var(--qs-radius-lg); padding: 20px 24px; box-shadow: var(--qs-shadow-sm); }
.qs-kpi-label { font-size: 11px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.06em; color: var(--qs-gray-500); margin-bottom: 8px; }
.qs-kpi-value { font-size: 24px; font-weight: 700; color: var(--qs-ink); letter-spacing: -0.02em; font-variant-numeric: tabular-nums; }
.qs-periodo-block { margin-bottom: 28px; }
.qs-periodo-title { font-size: 14px; font-weight: 700; color: var(--qs-teal-dark); margin-bottom: 10px; }
.qs-text-right { text-align: right; }
.qs-fw-bold { font-weight: 700; }
.qs-num-teal { color: var(--qs-teal-dark); font-variant-numeric: tabular-nums; }
.qs-empty-hint { font-size: 13px; color: var(--qs-gray-400); margin: 0; }
.qs-spinner-sm { display: inline-block; width: 13px; height: 13px; border: 2px solid rgba(255,255,255,.4); border-top-color: #fff; border-radius: 50%; animation: spin .7s linear infinite; vertical-align: middle; margin-right: 5px; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-top: 16px; }
@keyframes spin { to { transform: rotate(360deg); } }
@media (max-width: 640px) { .qs-grid-3 { grid-template-columns: 1fr; } }
</style>
