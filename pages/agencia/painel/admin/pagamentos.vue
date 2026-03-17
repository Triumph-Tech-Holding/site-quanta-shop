<template>
  <div>
    <div class="ag-page-header"><h1>Pagamentos</h1><p>Pagamentos e transações da plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum pagamento encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Usuário</th><th>Valor</th><th>Status</th><th>Vencimento</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.usuario }}</td>
              <td>{{ formatCurrency(item.valor) }}</td>
              <td><span class="badge-ag" :class="item.status === 'Pago' ? 'badge-ag-success' : (item.status === 'Vencido' ? 'badge-ag-danger' : 'badge-ag-warning')">{{ item.status }}</span></td>
              <td>{{ formatDate(item.dataVencimento) }}</td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:480px">
        <div class="ag-modal-header"><h5 class="mb-0">Pagamento #{{ selecionado.id }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-6"><strong>Usuário:</strong><br/>{{ selecionado.usuario }}</div>
            <div class="col-6"><strong>Status:</strong><br/>{{ selecionado.status }}</div>
            <div class="col-6"><strong>Valor:</strong><br/>{{ formatCurrency(selecionado.valor) }}</div>
            <div class="col-6"><strong>Vencimento:</strong><br/>{{ formatDate(selecionado.dataVencimento) }}</div>
          </div>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Fechar</button>
          <button v-if="selecionado.status !== 'Pago'" class="btn btn-ag-primary" @click="marcarPago">Marcar como Pago</button>
        </div>
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
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verDetalhes(item: FaturaAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function marcarPago() {
  if (!selecionado.value) return;
  try {
    await api.put(`/admin/pagamentos/${selecionado.value.id}/pagar`, {}, authHeader());
    const idx = itens.value.findIndex(i => i.id === selecionado.value?.id);
    if (idx !== -1) itens.value[idx] = { ...itens.value[idx], status: 'Pago' };
    if (selecionado.value) selecionado.value = { ...selecionado.value, status: 'Pago' };
  } catch (e: unknown) { console.error('Erro ao marcar pago:', extractApiErrorMessage(e)); }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/pagamentos/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar pagamentos:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
