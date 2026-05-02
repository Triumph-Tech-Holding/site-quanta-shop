<template>
  <div>
    <div class="ag-page-header d-flex align-items-start justify-content-between flex-wrap gap-2 no-print">
      <div>
        <h1>Relatório de Faturas</h1>
        <p>Faturas de cashback dos credenciados</p>
      </div>
      <div class="d-flex gap-2 align-items-end flex-wrap">
        <div>
          <label class="form-label mb-1 small fw-semibold">De</label>
          <input v-model="filtro.dataInicial" type="date" class="form-control form-control-sm" />
        </div>
        <div>
          <label class="form-label mb-1 small fw-semibold">Login</label>
          <input v-model="filtro.loginCredenciado" type="text" class="form-control form-control-sm" placeholder="Filtrar por login..." />
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
            <div class="stat-value">{{ formatCurrency(totais.totalPago) }}</div>
            <div class="stat-label mt-1">Total Pago</div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="ag-stat-card flex-column p-3 text-center">
            <div class="stat-value">{{ formatCurrency(totais.totalPendente) }}</div>
            <div class="stat-label mt-1">Pendente</div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="ag-stat-card flex-column p-3 text-center">
            <div class="stat-value">{{ itens.length }}</div>
            <div class="stat-label mt-1">Total Faturas</div>
          </div>
        </div>
      </div>

      <div class="ag-card">
        <div v-if="itens.length === 0" class="ag-empty-state">
          <h5>Nenhuma fatura encontrada</h5>
          <p class="text-muted small">Ajuste os filtros e clique em Atualizar.</p>
        </div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead>
              <tr>
                <th>#</th>
                <th>Credenciado</th>
                <th>Patrocinador</th>
                <th>Cidade</th>
                <th>Cashback</th>
                <th class="text-end">Valor</th>
                <th>Data Pedido</th>
                <th>Pagamento</th>
                <th>Status</th>
                <th class="no-print"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in itens" :key="item.IdPedido">
                <td class="text-muted small">#{{ item.IdPedido }}</td>
                <td class="fw-bold">{{ item.Login }}</td>
                <td>{{ item.Patrocinador || '—' }}</td>
                <td>{{ item.Cidade || '—' }}</td>
                <td>{{ item.Cashback }}</td>
                <td class="text-end fw-bold text-ag-secondary">{{ formatCurrency(item.ValorPedido) }}</td>
                <td>{{ formatDate(item.DataPedido) }}</td>
                <td>{{ item.DataPagamento ? formatDate(item.DataPagamento) : '—' }}</td>
                <td>
                  <span class="badge-ag" :class="badgeStatus(item.Status)">
                    {{ labelStatus(item.Status) }}
                  </span>
                </td>
                <td class="no-print">
                  <button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:500px">
        <div class="ag-modal-header">
          <h5 class="mb-0">Fatura #{{ selecionado.IdPedido }}</h5>
          <button class="btn-close" @click="fecharModal" />
        </div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-6"><strong>Credenciado:</strong> {{ selecionado.Login }}</div>
            <div class="col-6"><strong>Nome:</strong> {{ selecionado.Nome }}</div>
            <div class="col-6"><strong>Patrocinador:</strong> {{ selecionado.Patrocinador || '—' }}</div>
            <div class="col-6"><strong>Cidade:</strong> {{ selecionado.Cidade || '—' }}</div>
            <div class="col-6"><strong>Categoria:</strong> {{ selecionado.Categoria || '—' }}</div>
            <div class="col-6"><strong>Cashback:</strong> {{ selecionado.Cashback }}</div>
            <div class="col-6"><strong>Valor:</strong> {{ formatCurrency(selecionado.ValorPedido) }}</div>
            <div class="col-6"><strong>Status:</strong> {{ labelStatus(selecionado.Status) }}</div>
            <div class="col-6"><strong>Data Pedido:</strong> {{ formatDate(selecionado.DataPedido) }}</div>
            <div class="col-6"><strong>Data Pagamento:</strong> {{ selecionado.DataPagamento ? formatDate(selecionado.DataPagamento) : '—' }}</div>
          </div>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Fechar</button>
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
import { useAgenciaStore } from '~/composables/useAgenciaStore';
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
  if (status === 2) return 'badge-ag-success';
  if (status === 3) return 'badge-ag-danger';
  return 'badge-ag-warning';
}
function labelStatus(status: number) {
  if (status === 2) return 'Pago';
  if (status === 3) return 'Cancelado';
  return 'Pendente';
}

async function carregar() {
  loading.value = true;
  erro.value = '';
  try {
    const body: any = { pagina: 1, quantidadePorPagina: 200 };
    if (filtro.dataInicial) body.dataInicial = filtro.dataInicial;
    if (filtro.loginCredenciado) body.loginCredenciado = filtro.loginCredenciado;
    const { data } = await api.post('/Admin/ObterFaturas', body, authHeader());
    itens.value = Array.isArray(data) ? data : (data?.faturas || data?.items || []);
  } catch (e: any) {
    erro.value = e?.response?.data?.erros?.[0]?.mensagem || 'Erro ao carregar relatório de faturas.';
    console.error(erro.value, e);
  } finally {
    loading.value = false;
  }
}

onMounted(() => { agenciaStore.loadFromStorage(); carregar(); });
</script>
