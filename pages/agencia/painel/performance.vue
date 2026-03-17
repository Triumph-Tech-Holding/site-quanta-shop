<template>
  <div>
    <div class="ag-page-header"><h1>Performance</h1><p>Acompanhe seu desempenho na plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <template v-else>
      <div class="row g-3 mb-4">
        <div class="col-6 col-md-3" v-for="(stat, i) in statsCards" :key="i">
          <div class="ag-stat-card flex-column text-center p-3">
            <div class="stat-value" style="font-size:1.8rem">{{ stat.valor }}</div>
            <div class="stat-label mt-1">{{ stat.label }}</div>
          </div>
        </div>
      </div>

      <div class="ag-card mb-4" v-if="chartSeries[0].data.length > 0">
        <div class="ag-card-title">Cashback mensal (R$)</div>
        <ClientOnly>
          <apexchart type="area" height="260" :options="chartOptions" :series="chartSeries" />
        </ClientOnly>
      </div>

      <div class="ag-card">
        <div class="ag-card-title">Histórico mensal</div>
        <div v-if="historico.length === 0" class="ag-empty-state"><h5>Sem dados disponíveis</h5></div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead><tr><th>Período</th><th>Compras</th><th>Cashback</th></tr></thead>
            <tbody>
              <tr v-for="(h, i) in historico" :key="i">
                <td>{{ h.mes }}</td>
                <td>{{ h.compras ?? 0 }}</td>
                <td class="text-ag-secondary fw-bold">{{ formatCurrency(Number(h.cashback ?? 0)) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import type { PerformancePeriodo } from '~/types/agencia';
import { extractApiErrorMessage } from '~/types/agencia';

definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const historico = ref<PerformancePeriodo[]>([]);
const statsCards = ref<Array<{ label: string; valor: string | number }>>([]);

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v); }

const chartOptions = computed(() => ({
  chart: { id: 'perf-chart', toolbar: { show: false }, fontFamily: 'inherit' },
  colors: ['#2f7785'],
  fill: { type: 'gradient', gradient: { opacityFrom: 0.4, opacityTo: 0.05, stops: [0, 100] } },
  stroke: { curve: 'smooth', width: 2 },
  xaxis: { categories: historico.value.map((h) => String(h.mes ?? '')) },
  yaxis: { labels: { formatter: (v: number) => 'R$ ' + v.toFixed(0) } },
  tooltip: { y: { formatter: (v: number) => formatCurrency(v) } },
  dataLabels: { enabled: false },
  grid: { borderColor: '#ecf2f7' },
}));

const chartSeries = computed(() => [{ name: 'Cashback', data: historico.value.map((h) => Number(h.cashback ?? 0)) }]);

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get<{ stats?: Array<{ label: string; valor: string | number }>; historico?: PerformancePeriodo[] }>('/performance/resumo', authHeader());
    if (data?.stats) statsCards.value = data.stats;
    if (data?.historico) historico.value = data.historico;
  } catch (err: unknown) {
    console.error('Erro ao carregar performance:', extractApiErrorMessage(err));
  } finally {
    loading.value = false;
  }
});
</script>
