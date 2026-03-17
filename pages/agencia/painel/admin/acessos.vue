<template>
  <div>
    <div class="ag-page-header"><h1>Acessos</h1><p>Registro de acessos à plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum acesso encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Usuário</th><th>IP</th><th>Dispositivo</th><th>Data/Hora</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.usuario }}</td>
              <td>{{ item.ip }}</td>
              <td>{{ item.dispositivo || '—' }}</td>
              <td>{{ formatDateTime(item.data) }}</td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:480px">
        <div class="ag-modal-header"><h5 class="mb-0">Detalhes do Acesso</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-12"><strong>Usuário:</strong> {{ selecionado.usuario }}</div>
            <div class="col-12"><strong>IP:</strong> {{ selecionado.ip }}</div>
            <div class="col-12"><strong>Dispositivo:</strong> {{ selecionado.dispositivo || '—' }}</div>
            <div class="col-12"><strong>Data/Hora:</strong> {{ formatDateTime(selecionado.data) }}</div>
          </div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { AcessoAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<AcessoAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<AcessoAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDateTime(d: string) { return d ? new Date(d).toLocaleString('pt-BR') : '—'; }
function verDetalhes(item: AcessoAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/acessos/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar acessos:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
