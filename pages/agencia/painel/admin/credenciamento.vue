<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Credenciamento</h1><p>Solicitações de credenciamento de estabelecimentos</p></div>
      <div>
        <select v-model="filtroStatus" class="form-select" style="width:auto" @change="carregarDados">
          <option value="">Todos</option>
          <option value="Pendente">Pendente</option>
          <option value="Aprovado">Aprovado</option>
          <option value="Reprovado">Reprovado</option>
        </select>
      </div>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum credenciamento encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Nome Fantasia</th><th>Razão Social</th><th>CNPJ</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.nomeFantasia }}</td>
              <td>{{ item.razaoSocial }}</td>
              <td>{{ item.cnpj }}</td>
              <td><span class="badge-ag" :class="{ 'badge-ag-success': item.status === 'Aprovado', 'badge-ag-warning': item.status === 'Pendente', 'badge-ag-danger': item.status === 'Reprovado' }">{{ item.status }}</span></td>
              <td>
                <NuxtLink :to="`/agencia/finalizar-credenciamento/${item.id}`" class="btn btn-sm btn-ag-outline">Analisar</NuxtLink>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { CredenciamentoAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<CredenciamentoAdmin[]>([]);
const filtroStatus = ref('');
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
async function carregarDados() {
  loading.value = true;
  try {
    const params = filtroStatus.value ? `?status=${encodeURIComponent(filtroStatus.value)}` : '';
    const { data } = await api.get(`/admin/credenciamento/listar${params}`, authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar credenciamentos:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  await carregarDados();
});
</script>
