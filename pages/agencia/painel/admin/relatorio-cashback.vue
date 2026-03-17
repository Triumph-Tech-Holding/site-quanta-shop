<template>
  <div>
    <div class="ag-page-header"><h1>Relatório de Cashback</h1><p>Cashbacks concedidos pela plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div class="row g-3 mb-4">
        <div class="col-md-4"><div class="ag-stat-card flex-column p-3 text-center"><div class="stat-value">{{ formatCurrency(totais.totalConcedido) }}</div><div class="stat-label mt-1">Total Concedido</div></div></div>
        <div class="col-md-4"><div class="ag-stat-card flex-column p-3 text-center"><div class="stat-value">{{ itens.length }}</div><div class="stat-label mt-1">Transações</div></div></div>
        <div class="col-md-4"><div class="ag-stat-card flex-column p-3 text-center"><div class="stat-value">{{ formatCurrency(totais.mediaCashback) }}</div><div class="stat-label mt-1">Média por Transação</div></div></div>
      </div>
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum registro encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Usuário</th><th>Loja</th><th>Valor</th><th>Data</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.usuario }}</td>
              <td>{{ item.loja }}</td>
              <td class="text-ag-secondary fw-bold">{{ formatCurrency(item.valor) }}</td>
              <td>{{ formatDate(item.data) }}</td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:440px">
        <div class="ag-modal-header"><h5 class="mb-0">Detalhes do Cashback</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-12"><strong>Usuário:</strong> {{ selecionado.usuario }}</div>
            <div class="col-12"><strong>Loja:</strong> {{ selecionado.loja }}</div>
            <div class="col-6"><strong>Valor Cashback:</strong> {{ formatCurrency(selecionado.valor) }}</div>
            <div class="col-6"><strong>Data:</strong> {{ formatDate(selecionado.data) }}</div>
          </div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { CashbackAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<CashbackAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<CashbackAdmin | null>(null);
const totais = computed(() => ({
  totalConcedido: itens.value.reduce((sum, i) => sum + (i.valor || 0), 0),
  mediaCashback: itens.value.length ? itens.value.reduce((s, i) => s + (i.valor || 0), 0) / itens.value.length : 0,
}));
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verDetalhes(item: CashbackAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/relatorios/cashback', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar relatório:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
