<template>
  <div>
    <div class="ag-page-header"><h1>Painel Comerciante</h1><p>Gerencie seu estabelecimento credenciado</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <template v-else>
      <div class="row g-3 mb-4">
        <div class="col-6 col-md-3">
          <div class="ag-stat-card flex-column text-center p-3">
            <div class="stat-value">{{ resumo.totalVendas }}</div>
            <div class="stat-label mt-1">Vendas este mês</div>
          </div>
        </div>
        <div class="col-6 col-md-3">
          <div class="ag-stat-card flex-column text-center p-3">
            <div class="stat-value">{{ formatCurrency(resumo.faturamento) }}</div>
            <div class="stat-label mt-1">Faturamento</div>
          </div>
        </div>
        <div class="col-6 col-md-3">
          <div class="ag-stat-card flex-column text-center p-3">
            <div class="stat-value">{{ formatCurrency(resumo.cashbackPago) }}</div>
            <div class="stat-label mt-1">Cashback pago</div>
          </div>
        </div>
        <div class="col-6 col-md-3">
          <div class="ag-stat-card flex-column text-center p-3">
            <div class="stat-value">{{ resumo.totalClientes }}</div>
            <div class="stat-label mt-1">Clientes</div>
          </div>
        </div>
      </div>
      <div class="ag-card">
        <div class="ag-card-title">Últimas vendas</div>
        <div v-if="vendas.length === 0" class="ag-empty-state"><h5>Nenhuma venda registrada</h5></div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead><tr><th>Data</th><th>Cliente</th><th>Valor</th><th>Cashback</th><th>Status</th></tr></thead>
            <tbody>
              <tr v-for="(v, i) in vendas" :key="i">
                <td>{{ formatDate(v.data) }}</td>
                <td>{{ v.cliente }}</td>
                <td class="fw-bold">{{ formatCurrency(v.valor) }}</td>
                <td class="text-ag-secondary">{{ formatCurrency(v.cashback) }}</td>
                <td><span class="badge-ag" :class="v.status === 'Aprovado' ? 'badge-ag-success' : 'badge-ag-warning'">{{ v.status }}</span></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';

definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);

interface VendaComerciante {
  data: string;
  cliente: string;
  valor: number;
  cashback: number;
  status: string;
}

interface ResumoComerciante {
  totalVendas: number;
  faturamento: number;
  cashbackPago: number;
  totalClientes: number;
}

const resumo = reactive<ResumoComerciante>({ totalVendas: 0, faturamento: 0, cashbackPago: 0, totalClientes: 0 });
const vendas = ref<VendaComerciante[]>([]);

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get<{ resumo?: ResumoComerciante; vendas?: VendaComerciante[] }>('/comerciante/painel', authHeader());
    if (data?.resumo) Object.assign(resumo, data.resumo);
    if (data?.vendas) vendas.value = data.vendas;
  } catch (err: unknown) {
    console.error('Erro ao carregar painel comerciante:', extractApiErrorMessage(err));
  } finally {
    loading.value = false;
  }
});
</script>
