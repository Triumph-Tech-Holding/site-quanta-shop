<template>
  <div>
    <div class="ag-page-header"><h1>Compras</h1><p>Histórico de compras da plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhuma compra encontrada</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Usuário</th><th>Loja</th><th>Valor</th><th>Cashback</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.usuario }}</td>
              <td>{{ item.loja }}</td>
              <td>{{ formatCurrency(item.valor) }}</td>
              <td class="text-ag-secondary">{{ formatCurrency(item.cashback) }}</td>
              <td><span class="badge-ag" :class="item.status === 'Aprovado' ? 'badge-ag-success' : 'badge-ag-warning'">{{ item.status }}</span></td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:520px">
        <div class="ag-modal-header"><h5 class="mb-0">Detalhes da Compra #{{ selecionado.id }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-6"><strong>Usuário:</strong><br/>{{ selecionado.usuario }}</div>
            <div class="col-6"><strong>Loja:</strong><br/>{{ selecionado.loja }}</div>
            <div class="col-6"><strong>Valor:</strong><br/>{{ formatCurrency(selecionado.valor) }}</div>
            <div class="col-6"><strong>Cashback:</strong><br/>{{ formatCurrency(selecionado.cashback) }}</div>
            <div class="col-6"><strong>Status:</strong><br/>{{ selecionado.status }}</div>
          </div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { CompraAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<CompraAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<CompraAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function verDetalhes(item: CompraAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/compras/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar compras:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
