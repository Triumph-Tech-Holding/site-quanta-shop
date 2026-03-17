<template>
  <div>
    <div class="ag-page-header"><h1>Rede</h1><p>Estrutura de rede e indicações</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum registro encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Usuário</th><th>Nível</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.usuario }}</td>
              <td><span class="badge-ag badge-ag-primary">Nível {{ item.nivel }}</span></td>
              <td><span class="badge-ag" :class="item.status === 'Ativo' ? 'badge-ag-success' : 'badge-ag-warning'">{{ item.status }}</span></td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:440px">
        <div class="ag-modal-header"><h5 class="mb-0">Detalhes da Rede</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-12"><strong>Usuário:</strong> {{ selecionado.usuario }}</div>
            <div class="col-6"><strong>Nível:</strong> {{ selecionado.nivel }}</div>
            <div class="col-6"><strong>Status:</strong> {{ selecionado.status }}</div>
          </div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { RedeAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<RedeAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<RedeAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function verDetalhes(item: RedeAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/rede/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar rede:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
