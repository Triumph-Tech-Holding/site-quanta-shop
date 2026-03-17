<template>
  <div>
    <div class="ag-page-header"><h1>Meus Cupons Cashback</h1><p>Cupons de cashback gerados por você</p></div>
    <div class="mb-3 text-end">
      <NuxtLink to="/agencia/painel/gerar-cupons" class="btn btn-ag-primary">+ Gerar Cupom</NuxtLink>
    </div>
    <div class="ag-card">
      <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
      <div v-else-if="cupons.length === 0" class="ag-empty-state"><h5>Nenhum cupom gerado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Código</th><th>Loja</th><th>Valor</th><th>Gerado em</th><th>Status</th></tr></thead>
          <tbody>
            <tr v-for="(c, i) in cupons" :key="i">
              <td><code>{{ c.codigo }}</code></td>
              <td>{{ c.nomeLoja || c.loja || '—' }}</td>
              <td class="text-ag-secondary fw-bold">{{ formatCurrency(c.valor || c.valorCashback || 0) }}</td>
              <td>{{ formatDate(c.dataCriacao || c.data) }}</td>
              <td><span class="badge-ag" :class="c.aprovado ? 'badge-ag-success' : 'badge-ag-warning'">{{ c.aprovado ? 'Aprovado' : 'Aguardando' }}</span></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
import type { Cupom } from "~/types/agencia";
const cupons = ref<Cupom[]>([]);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v); }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/cuponCashback/listarMeus', authHeader());
    cupons.value = Array.isArray(data) ? data : (data?.items || []);
  } catch { /**/ } finally { loading.value = false; }
});
</script>
