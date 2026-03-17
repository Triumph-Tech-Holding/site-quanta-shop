<template>
  <div>
    <div class="ag-page-header"><h1>Assinaturas</h1><p>Assinaturas ativas na plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhuma assinatura encontrada</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Usuário</th><th>Plano</th><th>Status</th><th>Início</th><th>Fim</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.usuario }}</td>
              <td>{{ item.plano }}</td>
              <td><span class="badge-ag" :class="item.status === 'Ativo' ? 'badge-ag-success' : 'badge-ag-warning'">{{ item.status }}</span></td>
              <td>{{ formatDate(item.dataInicio) }}</td>
              <td>{{ formatDate(item.dataFim) }}</td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:480px">
        <div class="ag-modal-header"><h5 class="mb-0">Assinatura de {{ selecionado.usuario }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-6"><strong>Plano:</strong><br/>{{ selecionado.plano }}</div>
            <div class="col-6"><strong>Status:</strong><br/>{{ selecionado.status }}</div>
            <div class="col-6"><strong>Início:</strong><br/>{{ formatDate(selecionado.dataInicio) }}</div>
            <div class="col-6"><strong>Fim:</strong><br/>{{ formatDate(selecionado.dataFim) }}</div>
          </div>
          <div class="mt-3 d-flex gap-2">
            <button v-if="selecionado.status !== 'Ativo'" class="btn btn-sm btn-ag-outline" @click="alterarStatus(selecionado, 'Ativo')">Ativar</button>
            <button v-if="selecionado.status === 'Ativo'" class="btn btn-sm btn-outline-warning" @click="alterarStatus(selecionado, 'Cancelado')">Cancelar</button>
          </div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { AssinaturaAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<AssinaturaAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<AssinaturaAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verDetalhes(item: AssinaturaAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function alterarStatus(item: AssinaturaAdmin, novoStatus: string) {
  try {
    await api.put(`/admin/assinaturas/${item.id}/status`, { status: novoStatus }, authHeader());
    const idx = itens.value.findIndex(a => a.id === item.id);
    if (idx !== -1) itens.value[idx] = { ...itens.value[idx], status: novoStatus };
    if (selecionado.value?.id === item.id) selecionado.value = { ...selecionado.value, status: novoStatus };
  } catch (e: unknown) { console.error('Erro ao alterar status:', extractApiErrorMessage(e)); }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/assinaturas/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar assinaturas:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
