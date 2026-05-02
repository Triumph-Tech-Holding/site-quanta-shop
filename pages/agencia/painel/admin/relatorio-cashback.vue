<template>
  <div>
    <div class="ag-page-header d-flex align-items-start justify-content-between flex-wrap gap-2 no-print">
      <div>
        <h1>Relatório de Cashback</h1>
        <p>Cashbacks concedidos pela plataforma por período</p>
      </div>
      <div class="d-flex gap-2 align-items-end flex-wrap">
        <div>
          <label class="form-label mb-1 small fw-semibold">De</label>
          <input v-model="filtro.dataInicial" type="date" class="form-control form-control-sm" />
        </div>
        <div>
          <label class="form-label mb-1 small fw-semibold">Até</label>
          <input v-model="filtro.dataFinal" type="date" class="form-control form-control-sm" />
        </div>
        <button class="btn btn-ag-primary btn-sm" :disabled="loading" @click="carregar">
          <span v-if="loading" class="spinner-border spinner-border-sm me-1" />
          Atualizar
        </button>
        <button class="btn btn-ag-outline btn-sm" :disabled="itens.length === 0" @click="gerarPdf">
          ⬇ Gerar PDF
        </button>
      </div>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <div v-else>
      <div class="row g-3 mb-4">
        <div class="col-md-4">
          <div class="ag-stat-card flex-column p-3 text-center">
            <div class="stat-value">{{ formatCurrency(totais.totalConcedido) }}</div>
            <div class="stat-label mt-1">Total Concedido</div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="ag-stat-card flex-column p-3 text-center">
            <div class="stat-value">{{ totais.totalLancamentos }}</div>
            <div class="stat-label mt-1">Lançamentos</div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="ag-stat-card flex-column p-3 text-center">
            <div class="stat-value">{{ itens.length }}</div>
            <div class="stat-label mt-1">Períodos</div>
          </div>
        </div>
      </div>

      <div class="ag-card">
        <div v-if="itens.length === 0" class="ag-empty-state">
          <h5>Nenhum registro encontrado para o período</h5>
          <p class="text-muted small">Selecione um intervalo de datas e clique em Atualizar.</p>
        </div>

        <div v-for="periodo in itens" :key="periodo.data" class="mb-4">
          <h6 class="fw-bold mb-2 text-ag-secondary">
            {{ formatMes(periodo.data) }}
          </h6>
          <div class="table-responsive">
            <table class="table ag-table">
              <thead>
                <tr>
                  <th>Tipo Lançamento</th>
                  <th>Tipo Pedido</th>
                  <th>Status</th>
                  <th class="text-end">Valor</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(lanc, idx) in periodo.Lancamentos" :key="idx">
                  <td>{{ lanc.TipoLancamento || '—' }}</td>
                  <td>{{ lanc.DescricaoTipoPedido || '—' }}</td>
                  <td>
                    <span class="badge-ag" :class="badgeStatus(lanc.StatusPagamento)">
                      {{ lanc.StatusPagamento || '—' }}
                    </span>
                  </td>
                  <td class="text-end fw-bold text-ag-secondary">{{ formatCurrency(lanc.Valor) }}</td>
                </tr>
              </tbody>
              <tfoot>
                <tr class="fw-bold">
                  <td colspan="3" class="text-end">Subtotal</td>
                  <td class="text-end text-ag-secondary">
                    {{ formatCurrency(periodo.Lancamentos.reduce((s, l) => s + (l.Valor || 0), 0)) }}
                  </td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>
      </div>
    </div>

    <div v-if="erro" class="alert alert-danger mt-3">{{ erro }}</div>

  </div>
</template>

<style>
@media print {
  .no-print, nav, aside, header, .agencia-sidebar { display: none !important; }
  body { background: white !important; }
  .ag-card { box-shadow: none !important; border: 1px solid #ddd !important; }
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
  if (s.includes('pago') || s.includes('conclu')) return 'badge-ag-success';
  if (s.includes('pendente') || s.includes('aguard')) return 'badge-ag-warning';
  if (s.includes('cancel') || s.includes('recus')) return 'badge-ag-danger';
  return '';
}
function gerarPdf() { window.print(); }

async function carregar() {
  loading.value = true;
  erro.value = '';
  try {
    const params: Record<string, string> = {};
    if (filtro.dataInicial) params.dataInicial = filtro.dataInicial;
    if (filtro.dataFinal) params.datafinal = filtro.dataFinal;
    const { data } = await api.get('/Compra/relatorioMensalCashback', { ...authHeader(), params });
    itens.value = Array.isArray(data) ? data : [];
  } catch (e: any) {
    erro.value = e?.response?.data?.erros?.[0]?.mensagem || 'Erro ao carregar relatório de cashback.';
    console.error(erro.value, e);
  } finally {
    loading.value = false;
  }
}

onMounted(() => { agenciaStore.loadFromStorage(); carregar(); });
</script>
