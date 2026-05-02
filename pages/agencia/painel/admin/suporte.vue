<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Atendimento</div>
        <h1>Suporte</h1>
        <p>Chamados de suporte dos usuários</p>
      </div>
      <div class="qs-header-actions">
        <select v-model="filtroStatus" class="qs-select-inline" @change="carregarDados">
          <option value="">Todos os status</option>
          <option value="Aberto">Aberto</option>
          <option value="Em Atendimento">Em Atendimento</option>
          <option value="Fechado">Fechado</option>
        </select>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"/></svg>
        <h3>Nenhum chamado encontrado</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Usuário</th><th>Assunto</th><th>Status</th><th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.usuario }}</td>
              <td>{{ item.assunto }}</td>
              <td>
                <span class="qs-badge" :class="{
                  'qs-badge-success': item.status === 'Fechado',
                  'qs-badge-warn': item.status === 'Aberto',
                  'qs-badge-info': item.status === 'Em Atendimento'
                }">{{ item.status }}</span>
              </td>
              <td><button class="qs-btn-sm-outline" @click="abrirChamado(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:560px">
        <div class="qs-modal-header">
          <h5>Chamado #{{ selecionado.id }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-field-group">
            <span class="qs-detail-label">Usuário</span>
            <span class="qs-detail-value">{{ selecionado.usuario }}</span>
          </div>
          <div class="qs-field-group">
            <span class="qs-detail-label">Assunto</span>
            <span class="qs-detail-value">{{ selecionado.assunto }}</span>
          </div>
          <div class="qs-msg-box">
            <div class="qs-detail-label" style="margin-bottom:6px">Mensagem</div>
            {{ selecionado.mensagem }}
          </div>
          <div class="qs-field-group">
            <label class="qs-label">Resposta</label>
            <textarea v-model="resposta" class="qs-textarea" rows="4" placeholder="Digite sua resposta..." />
          </div>
          <div class="qs-field-group">
            <label class="qs-label">Alterar status</label>
            <select v-model="novoStatus" class="qs-select-field">
              <option value="Aberto">Aberto</option>
              <option value="Em Atendimento">Em Atendimento</option>
              <option value="Fechado">Fechado</option>
            </select>
          </div>
          <div v-if="modalError" class="qs-alert-danger">{{ modalError }}</div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Fechar</button>
          <button class="qs-btn-primary" :disabled="saving" @click="responder">
            {{ saving ? 'Enviando...' : 'Responder' }}
          </button>
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
function abrirChamado(item: SuporteAdmin) {
  selecionado.value = item;
  resposta.value = '';
  novoStatus.value = item.status === 'Aberto' ? 'Em Atendimento' : item.status;
  modalError.value = '';
  showModal.value = true;
}
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

<style scoped>
.qs-select-inline { padding: 8px 12px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-family: inherit; font-size: 14px; background: #fff; outline: none; }
.qs-select-inline:focus { border-color: var(--qs-teal); }
.qs-field-group { margin-bottom: 14px; }
.qs-detail-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); display: block; margin-bottom: 2px; }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-msg-box { background: var(--qs-gray-50); border: 1px solid var(--qs-gray-100); border-radius: var(--qs-radius-md); padding: 12px 14px; font-size: 14px; color: var(--qs-gray-700); line-height: 1.5; margin-bottom: 16px; }
.qs-label { display: block; font-size: 13px; font-weight: 600; color: var(--qs-gray-700); margin-bottom: 6px; }
.qs-textarea { width: 100%; padding: 10px 14px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-family: inherit; font-size: 14px; resize: vertical; outline: none; box-sizing: border-box; }
.qs-textarea:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.qs-select-field { width: 100%; padding: 10px 14px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-family: inherit; font-size: 14px; background: #fff; outline: none; }
.qs-select-field:focus { border-color: var(--qs-teal); }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; margin-top: 8px; }
</style>
