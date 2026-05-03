<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Admin · Configurações" title="Ecossistemas" description="Gerenciar ecossistemas de parceiros">
      <button class="qs-btn-primary" @click="abrirNovo">+ Novo Ecossistema</button>
    </QsPageHeader>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><circle cx="12" cy="12" r="3"/><path d="M12 2v4M12 18v4M4.93 4.93l2.83 2.83M16.24 16.24l2.83 2.83M2 12h4M18 12h4M4.93 19.07l2.83-2.83M16.24 7.76l2.83-2.83"/></svg>
        <h3>Nenhum ecossistema encontrado</h3>
        <button class="qs-btn-primary" @click="abrirNovo">Criar primeiro ecossistema</button>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead><tr><th>Nome</th><th>Descrição</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.nome }}</td>
              <td>{{ item.descricao || '—' }}</td>
              <td class="qs-cell-actions">
                <button class="qs-btn-sm-outline" @click="abrirEditar(item)">Editar</button>
                <button class="qs-btn-sm-danger" @click="confirmarExcluir(item)">Excluir</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal">
        <div class="qs-modal-header">
          <h5>{{ form.id ? 'Editar Ecossistema' : 'Novo Ecossistema' }}</h5>
          <button class="qs-modal-close" @click="fecharModal"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-field"><label class="qs-label">Nome *</label><input v-model="form.nome" type="text" class="qs-input" /></div>
          <div class="qs-field" style="margin-top:14px"><label class="qs-label">Descrição</label><textarea v-model="form.descricao" class="qs-textarea" rows="3" /></div>
          <div v-if="modalError" class="qs-alert-danger" style="margin-top:12px">{{ modalError }}</div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Cancelar</button>
          <button class="qs-btn-primary" :disabled="saving" @click="salvar">{{ saving ? 'Salvando...' : 'Salvar' }}</button>
        </div>
      </div>
    </div>

    <div v-if="showConfirm" class="qs-modal-overlay" @click.self="showConfirm = false">
      <div class="qs-modal" style="max-width:420px">
        <div class="qs-modal-header"><h5>Confirmar exclusão</h5></div>
        <div class="qs-modal-body"><p>Excluir o ecossistema <strong>{{ itemParaExcluir?.nome }}</strong>?</p></div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="showConfirm = false">Cancelar</button>
          <button class="qs-btn-danger" :disabled="saving" @click="excluir">{{ saving ? 'Excluindo...' : 'Excluir' }}</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { EcossistemaAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const saving = ref(false);
const itens = ref<EcossistemaAdmin[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<EcossistemaAdmin | null>(null);
const form = reactive<{ id?: number; nome: string; descricao: string }>({ nome: '', descricao: '' });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function abrirNovo() { Object.assign(form, { id: undefined, nome: '', descricao: '' }); modalError.value = ''; showModal.value = true; }
function abrirEditar(item: EcossistemaAdmin) { Object.assign(form, { id: item.id, nome: item.nome, descricao: item.descricao || '' }); modalError.value = ''; showModal.value = true; }
function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: EcossistemaAdmin) { itemParaExcluir.value = item; showConfirm.value = true; }
async function salvar() {
  if (!form.nome.trim()) { modalError.value = 'Nome é obrigatório.'; return; }
  saving.value = true; modalError.value = '';
  try {
    const payload = { nome: form.nome, descricao: form.descricao };
    if (form.id) {
      await api.put(`/admin/ecossistemas/${form.id}`, payload, authHeader());
      const idx = itens.value.findIndex(i => i.id === form.id);
      if (idx !== -1) itens.value[idx] = { ...itens.value[idx], ...payload };
    } else {
      const { data } = await api.post('/admin/ecossistemas/criar', payload, authHeader());
      itens.value.unshift(data as EcossistemaAdmin);
    }
    fecharModal();
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao salvar ecossistema.'); }
  finally { saving.value = false; }
}
async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    await api.delete(`/admin/ecossistemas/${itemParaExcluir.value.id}`, authHeader());
    itens.value = itens.value.filter(i => i.id !== itemParaExcluir.value?.id);
    showConfirm.value = false;
  } catch (e: unknown) { console.error('Erro ao excluir:', extractApiErrorMessage(e)); }
  finally { saving.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/ecossistemas/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar ecossistemas:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-field { display: flex; flex-direction: column; gap: 6px; }
.qs-label { font-size: 13px; font-weight: 600; color: var(--qs-gray-700); }
.qs-textarea { width: 100%; padding: 10px 12px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 14px; resize: vertical; outline: none; }
.qs-textarea:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; }
</style>
