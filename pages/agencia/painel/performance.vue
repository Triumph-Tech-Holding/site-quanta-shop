<template>
  <div>
    <div class="ag-page-header"><h1>Performance</h1><p>Acompanhe seu desempenho na plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <template v-else>
      <div class="row g-3 mb-4">
        <div class="col-6 col-md-3" v-for="(stat, i) in stats" :key="i">
          <div class="ag-stat-card flex-column text-center p-3">
            <div class="stat-value" style="font-size:1.8rem">{{ stat.valor }}</div>
            <div class="stat-label mt-1">{{ stat.label }}</div>
          </div>
        </div>
      </div>
      <div class="ag-card">
        <div class="ag-card-title">Histórico mensal</div>
        <div v-if="historico.length === 0" class="ag-empty-state"><h5>Sem dados disponíveis</h5></div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead><tr><th>Período</th><th>Compras</th><th>Cashback</th><th>Rede ativa</th><th>Graduação</th></tr></thead>
            <tbody>
              <tr v-for="(h, i) in historico" :key="i">
                <td>{{ h.periodo || h.mes }}</td>
                <td>{{ h.totalCompras || 0 }}</td>
                <td class="text-ag-secondary fw-bold">{{ formatCurrency(h.cashback || 0) }}</td>
                <td>{{ h.redeAtiva || 0 }}</td>
                <td>{{ h.graduacao || '—' }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const historico = ref<any[]>([]);
const stats = ref<any[]>([]);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v); }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/performance/resumo', authHeader());
    if (data?.stats) stats.value = data.stats;
    if (data?.historico) historico.value = data.historico;
    else if (Array.isArray(data)) historico.value = data;
  } catch { /**/ } finally { loading.value = false; }
});
</script>
