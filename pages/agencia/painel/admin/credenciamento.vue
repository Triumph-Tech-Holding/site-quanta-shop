<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Admin · Parceiros" title="Credenciamento" description="Solicitações de credenciamento de estabelecimentos">
      <select v-model="filtroStatus" class="qs-select-inline" @change="carregarDados">
        <option value="">Todos os status</option>
        <option value="Pendente">Pendente</option>
        <option value="Aprovado">Aprovado</option>
        <option value="Reprovado">Reprovado</option>
      </select>
    </QsPageHeader>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>
        <h3>Nenhum credenciamento encontrado</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Nome Fantasia</th><th>Razão Social</th><th>CNPJ</th><th>Status</th><th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.nomeFantasia }}</td>
              <td>{{ item.razaoSocial }}</td>
              <td class="qs-cell-mono">{{ item.cnpj }}</td>
              <td>
                <span class="qs-badge" :class="{
                  'qs-badge-success': item.status === 'Aprovado',
                  'qs-badge-warn': item.status === 'Pendente',
                  'qs-badge-danger': item.status === 'Reprovado'
                }">{{ item.status }}</span>
              </td>
              <td>
                <NuxtLink :to="`/agencia/finalizar-credenciamento/${item.id}`" class="qs-btn-sm-outline">Analisar</NuxtLink>
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

<style scoped>
.qs-select-inline { padding: 8px 12px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-family: inherit; font-size: 14px; background: #fff; outline: none; }
.qs-select-inline:focus { border-color: var(--qs-teal); }
.qs-cell-mono { font-family: 'SFMono-Regular', 'Consolas', monospace; font-size: 13px; color: var(--qs-gray-600); }
</style>
