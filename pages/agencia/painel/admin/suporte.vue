<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Suporte</h1><p>Chamados de suporte dos usuários</p></div>
      <select v-model="filtroStatus" class="form-select" style="width:auto" @change="carregarDados">
        <option value="">Todos</option>
        <option value="Aberto">Aberto</option>
        <option value="Em Atendimento">Em Atendimento</option>
        <option value="Fechado">Fechado</option>
      </select>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum chamado encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Usuário</th><th>Assunto</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.usuario }}</td>
              <td>{{ item.assunto }}</td>
              <td><span class="badge-ag" :class="{ 'badge-ag-success': item.status === 'Fechado', 'badge-ag-warning': item.status === 'Aberto', 'badge-ag-primary': item.status === 'Em Atendimento' }">{{ item.status }}</span></td>
              <td><button class="btn btn-sm btn-ag-outline" @click="abrirChamado(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:560px">
        <div class="ag-modal-header"><h5 class="mb-0">Chamado #{{ selecionado.id }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="mb-2"><strong>Usuário:</strong> {{ selecionado.usuario }}</div>
          <div class="mb-2"><strong>Assunto:</strong> {{ selecionado.assunto }}</div>
          <div class="mb-3 p-3 bg-light rounded"><strong>Mensagem:</strong><br/>{{ selecionado.mensagem }}</div>
          <div class="mb-3">
            <label class="form-label fw-bold">Resposta</label>
            <textarea v-model="resposta" class="form-control" rows="4" placeholder="Digite sua resposta..." />
          </div>
          <div class="mb-3">
            <label class="form-label fw-bold">Alterar status</label>
            <select v-model="novoStatus" class="form-select"><option value="Aberto">Aberto</option><option value="Em Atendimento">Em Atendimento</option><option value="Fechado">Fechado</option></select>
          </div>
          <div v-if="modalError" class="alert alert-danger py-2">{{ modalError }}</div>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Fechar</button>
          <button class="btn btn-ag-primary" :disabled="saving" @click="responder">{{ saving ? 'Enviando...' : 'Responder' }}</button>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { SuporteAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const saving = ref(false);
const itens = ref<SuporteAdmin[]>([]);
const showModal = ref(false);
const modalError = ref('');
const filtroStatus = ref('');
const resposta = ref('');
const novoStatus = ref('Em Atendimento');
const selecionado = ref<SuporteAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function abrirChamado(item: SuporteAdmin) { selecionado.value = item; resposta.value = ''; novoStatus.value = item.status === 'Aberto' ? 'Em Atendimento' : item.status; modalError.value = ''; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function responder() {
  if (!selecionado.value) return;
  saving.value = true; modalError.value = '';
  try {
    await api.put(`/admin/suporte/${selecionado.value.id}/responder`, { resposta: resposta.value, status: novoStatus.value }, authHeader());
    const idx = itens.value.findIndex(i => i.id === selecionado.value?.id);
    if (idx !== -1) itens.value[idx] = { ...itens.value[idx], status: novoStatus.value };
    fecharModal();
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao enviar resposta.'); }
  finally { saving.value = false; }
}
async function carregarDados() {
  loading.value = true;
  try {
    const params = filtroStatus.value ? `?status=${encodeURIComponent(filtroStatus.value)}` : '';
    const { data } = await api.get(`/admin/suporte/listar${params}`, authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar chamados:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  await carregarDados();
});
</script>
