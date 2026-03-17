<template>
  <div>
    <div class="ag-page-header"><h1>Relatório de Faturas</h1><p>Faturas e cobranças da plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div class="row g-3 mb-4">
        <div class="col-md-4"><div class="ag-stat-card flex-column p-3 text-center"><div class="stat-value">{{ formatCurrency(totais.totalPago) }}</div><div class="stat-label mt-1">Total Pago</div></div></div>
        <div class="col-md-4"><div class="ag-stat-card flex-column p-3 text-center"><div class="stat-value">{{ formatCurrency(totais.totalPendente) }}</div><div class="stat-label mt-1">Pendente</div></div></div>
        <div class="col-md-4"><div class="ag-stat-card flex-column p-3 text-center"><div class="stat-value">{{ itens.length }}</div><div class="stat-label mt-1">Total Faturas</div></div></div>
      </div>
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhuma fatura encontrada</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Usuário</th><th>Valor</th><th>Vencimento</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.usuario }}</td>
              <td>{{ formatCurrency(item.valor) }}</td>
              <td>{{ formatDate(item.dataVencimento) }}</td>
              <td><span class="badge-ag" :class="{ 'badge-ag-success': item.status === 'Pago', 'badge-ag-danger': item.status === 'Vencido', 'badge-ag-warning': item.status === 'Pendente' }">{{ item.status }}</span></td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:440px">
        <div class="ag-modal-header"><h5 class="mb-0">Fatura #{{ selecionado.id }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-12"><strong>Usuário:</strong> {{ selecionado.usuario }}</div>
            <div class="col-6"><strong>Valor:</strong> {{ formatCurrency(selecionado.valor) }}</div>
            <div class="col-6"><strong>Status:</strong> {{ selecionado.status }}</div>
            <div class="col-6"><strong>Vencimento:</strong> {{ formatDate(selecionado.dataVencimento) }}</div>
          </div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { FaturaAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<FaturaAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<FaturaAdmin | null>(null);
const totais = computed(() => ({
  totalPago: itens.value.filter(i => i.status === 'Pago').reduce((s, i) => s + i.valor, 0),
  totalPendente: itens.value.filter(i => i.status !== 'Pago').reduce((s, i) => s + i.valor, 0),
}));
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verDetalhes(item: FaturaAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/relatorios/faturas', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar faturas:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
