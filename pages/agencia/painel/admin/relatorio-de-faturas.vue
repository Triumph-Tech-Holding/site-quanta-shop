<template>
  <div class="qs-page no-print">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Relatórios</div>
        <h1>Relatório de Faturas</h1>
        <p>Faturas de cashback dos credenciados</p>
      </div>
      <div class="qs-header-actions qs-filter-inline no-print">
        <div class="qs-filter-field">
          <label class="qs-filter-label">De</label>
          <input v-model="filtro.dataInicial" type="date" class="qs-input-sm" />
        </div>
        <div class="qs-filter-field">
          <label class="qs-filter-label">Login</label>
          <input v-model="filtro.loginCredenciado" type="text" class="qs-input-sm" placeholder="Filtrar por login..." />
        </div>
        <button class="qs-btn-primary qs-btn-sm" :disabled="loading" @click="carregar">
          <span v-if="loading" class="qs-spinner-sm" />
          Atualizar
        </button>
        <button class="qs-btn-outline qs-btn-sm no-print" :disabled="itens.length === 0" @click="gerarPdf">
          ⬇ PDF
        </button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <div class="qs-grid qs-grid-3 qs-kpi-row">
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Total Pago</div>
          <div class="qs-kpi-value qs-kpi-green">{{ formatCurrency(totais.totalPago) }}</div>
        </div>
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Pendente</div>
          <div class="qs-kpi-value qs-kpi-warn">{{ formatCurrency(totais.totalPendente) }}</div>
        </div>
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Total Faturas</div>
          <div class="qs-kpi-value">{{ itens.length }}</div>
        </div>
      </div>

      <div class="qs-card-section">
        <div v-if="itens.length === 0" class="qs-empty-state">
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><rect x="2" y="5" width="20" height="14" rx="2"/><line x1="2" y1="10" x2="22" y2="10"/></svg>
          <h3>Nenhuma fatura encontrada</h3>
          <p class="qs-empty-hint">Ajuste os filtros e clique em Atualizar.</p>
        </div>
        <div v-else class="qs-table-wrap">
          <table class="qs-table">
            <thead>
              <tr>
                <th>#</th>
                <th>Credenciado</th>
                <th>Patrocinador</th>
                <th>Cidade</th>
                <th>Cashback</th>
                <th class="qs-text-right">Valor</th>
                <th>Data Pedido</th>
                <th>Pagamento</th>
                <th>Status</th>
                <th class="no-print"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in itens" :key="item.IdPedido">
                <td class="qs-text-muted-sm">#{{ item.IdPedido }}</td>
                <td class="qs-cell-bold">{{ item.Login }}</td>
                <td>{{ item.Patrocinador || '—' }}</td>
                <td>{{ item.Cidade || '—' }}</td>
                <td>{{ item.Cashback }}</td>
                <td class="qs-text-right qs-cell-bold qs-num-teal">{{ formatCurrency(item.ValorPedido) }}</td>
                <td>{{ formatDate(item.DataPedido) }}</td>
                <td>{{ item.DataPagamento ? formatDate(item.DataPagamento) : '—' }}</td>
                <td><span class="qs-badge" :class="badgeStatus(item.Status)">{{ labelStatus(item.Status) }}</span></td>
                <td class="no-print"><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </template>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:500px">
        <div class="qs-modal-header">
          <h5>Fatura #{{ selecionado.IdPedido }}</h5>
          <button class="qs-modal-close" @click="fecharModal"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-info-grid">
            <div class="qs-info-item"><span class="qs-info-label">Credenciado</span><span class="qs-info-value">{{ selecionado.Login }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Nome</span><span class="qs-info-value">{{ selecionado.Nome }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Patrocinador</span><span class="qs-info-value">{{ selecionado.Patrocinador || '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Cidade</span><span class="qs-info-value">{{ selecionado.Cidade || '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Categoria</span><span class="qs-info-value">{{ selecionado.Categoria || '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Cashback</span><span class="qs-info-value">{{ selecionado.Cashback }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Valor</span><span class="qs-info-value qs-num-teal">{{ formatCurrency(selecionado.ValorPedido) }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Status</span><span class="qs-badge" :class="badgeStatus(selecionado.Status)">{{ labelStatus(selecionado.Status) }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Data Pedido</span><span class="qs-info-value">{{ formatDate(selecionado.DataPedido) }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Data Pagamento</span><span class="qs-info-value">{{ selecionado.DataPagamento ? formatDate(selecionado.DataPagamento) : '—' }}</span></div>
          </div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Fechar</button>
        </div>
      </div>
    </div>

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
const showModal = ref(false);
const selecionado = ref<any>(null);
const hoje = new Date();
const filtro = reactive({
  dataInicial: new Date(hoje.getFullYear(), hoje.getMonth() - 2, 1).toISOString().split('T')[0],
  loginCredenciado: '',
});
const totais = computed(() => ({
  totalPago: itens.value.filter(i => i.Status === 2 || i.DataPagamento).reduce((s, i) => s + (i.ValorPedido || 0), 0),
  totalPendente: itens.value.filter(i => i.Status !== 2 && !i.DataPagamento).reduce((s, i) => s + (i.ValorPedido || 0), 0),
}));
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verDetalhes(item: any) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
function gerarPdf() { window.print(); }
function badgeStatus(status: number) {
  if (status === 2) return 'qs-badge-success';
  if (status === 3) return 'qs-badge-danger';
  return 'qs-badge-warn';
}
function labelStatus(status: number) {
  if (status === 2) return 'Pago';
  if (status === 3) return 'Cancelado';
  return 'Pendente';
}
async function carregar() {
  loading.value = true; erro.value = '';
  try {
    const body: any = { pagina: 1, quantidadePorPagina: 200 };
    if (filtro.dataInicial) body.dataInicial = filtro.dataInicial;
    if (filtro.loginCredenciado) body.loginCredenciado = filtro.loginCredenciado;
    const { data } = await api.post('/Admin/ObterFaturas', body, authHeader());
    itens.value = Array.isArray(data) ? data : (data?.faturas || data?.items || []);
  } catch (e: any) {
    erro.value = e?.response?.data?.erros?.[0]?.mensagem || 'Erro ao carregar relatório de faturas.';
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
.qs-kpi-green { color: #16a34a; }
.qs-kpi-warn { color: #d97706; }
.qs-text-right { text-align: right; }
.qs-text-muted-sm { font-size: 12px; color: var(--qs-gray-400); }
.qs-num-teal { color: var(--qs-teal-dark); font-variant-numeric: tabular-nums; }
.qs-info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.qs-info-item { display: flex; flex-direction: column; gap: 4px; }
.qs-info-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-info-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-empty-hint { font-size: 13px; color: var(--qs-gray-400); margin: 0; }
.qs-spinner-sm { display: inline-block; width: 13px; height: 13px; border: 2px solid rgba(255,255,255,.4); border-top-color: #fff; border-radius: 50%; animation: spin .7s linear infinite; vertical-align: middle; margin-right: 5px; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-top: 16px; }
@keyframes spin { to { transform: rotate(360deg); } }
@media (max-width: 640px) { .qs-grid-3 { grid-template-columns: 1fr; } }
</style>
