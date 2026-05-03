<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Admin · Conteúdo" title="Material de Apoio" description="Gerenciar materiais de apoio para parceiros">
      <button class="qs-btn-primary" @click="abrirNovo">+ Novo Material</button>
    </QsPageHeader>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/></svg>
        <h3>Nenhum material encontrado</h3>
        <button class="qs-btn-primary" @click="abrirNovo">Adicionar material</button>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead><tr><th>Título</th><th>Tipo</th><th>URL</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.titulo }}</td>
              <td><span class="qs-badge qs-badge-neutral">{{ item.tipo }}</span></td>
              <td>
                <a v-if="item.url" :href="item.url" target="_blank" class="qs-link qs-link-truncate">{{ item.url }}</a>
                <span v-else class="qs-text-muted">—</span>
              </td>
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
          <h5>{{ form.id ? 'Editar Material' : 'Novo Material' }}</h5>
          <button class="qs-modal-close" @click="fecharModal"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-form-rows">
            <div class="qs-field"><label class="qs-label">Título *</label><input v-model="form.titulo" type="text" class="qs-input" /></div>
            <div class="qs-field">
              <label class="qs-label">Tipo</label>
              <select v-model="form.tipo" class="qs-select-input">
                <option value="PDF">PDF</option>
                <option value="Vídeo">Vídeo</option>
                <option value="Link">Link</option>
                <option value="Imagem">Imagem</option>
              </select>
            </div>
            <div class="qs-field"><label class="qs-label">URL</label><input v-model="form.url" type="url" class="qs-input" placeholder="https://..." /></div>
            <div class="qs-field"><label class="qs-label">Descrição</label><textarea v-model="form.descricao" class="qs-textarea" rows="3" /></div>
          </div>
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
        <div class="qs-modal-body"><p>Excluir o material <strong>{{ itemParaExcluir?.titulo }}</strong>?</p></div>
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
import type { MaterialApoioAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const saving = ref(false);
const itens = ref<MaterialApoioAdmin[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<MaterialApoioAdmin | null>(null);
const form = reactive<{ id?: number; titulo: string; tipo: string; url: string; descricao: string }>({ titulo: '', tipo: 'PDF', url: '', descricao: '' });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function abrirNovo() { Object.assign(form, { id: undefined, titulo: '', tipo: 'PDF', url: '', descricao: '' }); modalError.value = ''; showModal.value = true; }
function abrirEditar(item: MaterialApoioAdmin) { Object.assign(form, { id: item.id, titulo: item.titulo, tipo: item.tipo, url: item.url || '', descricao: item.descricao || '' }); modalError.value = ''; showModal.value = true; }
function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: MaterialApoioAdmin) { itemParaExcluir.value = item; showConfirm.value = true; }
async function salvar() {
  if (!form.titulo.trim()) { modalError.value = 'Título é obrigatório.'; return; }
  saving.value = true; modalError.value = '';
  try {
    const payload = { titulo: form.titulo, tipo: form.tipo, url: form.url, descricao: form.descricao };
    if (form.id) {
      await api.put(`/admin/material-apoio/${form.id}`, payload, authHeader());
      const idx = itens.value.findIndex(i => i.id === form.id);
      if (idx !== -1) itens.value[idx] = { ...itens.value[idx], ...payload };
    } else {
      const { data } = await api.post('/admin/material-apoio/criar', payload, authHeader());
      itens.value.unshift(data as MaterialApoioAdmin);
    }
    fecharModal();
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao salvar material.'); }
  finally { saving.value = false; }
}
async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    await api.delete(`/admin/material-apoio/${itemParaExcluir.value.id}`, authHeader());
    itens.value = itens.value.filter(i => i.id !== itemParaExcluir.value?.id);
    showConfirm.value = false;
  } catch (e: unknown) { console.error('Erro ao excluir:', extractApiErrorMessage(e)); }
  finally { saving.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/material-apoio/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar material de apoio:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-form-rows { display: flex; flex-direction: column; gap: 14px; }
.qs-field { display: flex; flex-direction: column; gap: 6px; }
.qs-label { font-size: 13px; font-weight: 600; color: var(--qs-gray-700); }
.qs-select-input { padding: 9px 12px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 14px; background: #fff; outline: none; cursor: pointer; }
.qs-select-input:focus { border-color: var(--qs-teal); }
.qs-textarea { width: 100%; padding: 10px 12px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 14px; resize: vertical; outline: none; }
.qs-textarea:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.qs-link { color: var(--qs-teal); text-decoration: none; font-size: 13px; }
.qs-link:hover { text-decoration: underline; }
.qs-link-truncate { display: inline-block; max-width: 200px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; vertical-align: bottom; }
.qs-text-muted { color: var(--qs-gray-400); font-size: 14px; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; }
</style>
