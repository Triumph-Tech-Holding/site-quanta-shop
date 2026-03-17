<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Lojas Credenciadas</h1><p>Estabelecimentos ativos na plataforma</p></div>
      <input v-model="busca" type="text" class="form-control" style="max-width:240px" placeholder="Buscar por nome ou CNPJ..." />
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itensFiltrados.length === 0" class="ag-empty-state"><h5>Nenhuma loja encontrada</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Nome Fantasia</th><th>CNPJ</th><th>Cidade</th><th>Estado</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itensFiltrados" :key="item.id">
              <td class="fw-bold">{{ item.nomeFantasia }}</td>
              <td>{{ item.cnpj }}</td>
              <td>{{ item.cidade }}</td>
              <td>{{ item.estado }}</td>
              <td><span class="badge-ag" :class="item.status === 'Ativo' ? 'badge-ag-success' : 'badge-ag-warning'">{{ item.status }}</span></td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verLoja(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:560px">
        <div class="ag-modal-header"><h5 class="mb-0">{{ selecionado.nomeFantasia }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-6"><strong>CNPJ:</strong><br/>{{ selecionado.cnpj }}</div>
            <div class="col-6"><strong>Status:</strong><br/>{{ selecionado.status }}</div>
            <div class="col-6"><strong>Cidade:</strong><br/>{{ selecionado.cidade }}</div>
            <div class="col-6"><strong>Estado:</strong><br/>{{ selecionado.estado }}</div>
          </div>
          <div class="mt-3 d-flex gap-2">
            <button class="btn btn-sm btn-ag-outline" @click="alterarStatus(selecionado, 'Ativo')">Ativar</button>
            <button class="btn btn-sm btn-outline-warning" @click="alterarStatus(selecionado, 'Inativo')">Desativar</button>
          </div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { LojaCredenciadaAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<LojaCredenciadaAdmin[]>([]);
const showModal = ref(false);
const busca = ref('');
const selecionado = ref<LojaCredenciadaAdmin | null>(null);
const itensFiltrados = computed(() => {
  if (!busca.value) return itens.value;
  const b = busca.value.toLowerCase();
  return itens.value.filter(i => i.nomeFantasia?.toLowerCase().includes(b) || i.cnpj?.includes(b));
});
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function verLoja(item: LojaCredenciadaAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function alterarStatus(item: LojaCredenciadaAdmin, novoStatus: string) {
  try {
    await api.put(`/admin/lojas/${item.id}/status`, { status: novoStatus }, authHeader());
    const idx = itens.value.findIndex(l => l.id === item.id);
    if (idx !== -1) itens.value[idx] = { ...itens.value[idx], status: novoStatus };
    if (selecionado.value?.id === item.id) selecionado.value = { ...selecionado.value, status: novoStatus };
  } catch (e: unknown) { console.error('Erro ao alterar status:', extractApiErrorMessage(e)); }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/lojas/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar lojas:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
