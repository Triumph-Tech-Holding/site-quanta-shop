<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Ecossistemas</h1><p>Gerenciar ecossistemas de parceiros</p></div>
      <button class="btn btn-ag-primary" @click="abrirNovo">+ Novo Ecossistema</button>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum ecossistema encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Nome</th><th>Descrição</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.nome }}</td>
              <td>{{ item.descricao || '—' }}</td>
              <td class="d-flex gap-1">
                <button class="btn btn-sm btn-ag-outline" @click="abrirEditar(item)">Editar</button>
                <button class="btn btn-sm btn-outline-danger" @click="confirmarExcluir(item)">Excluir</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal">
        <div class="ag-modal-header"><h5 class="mb-0">{{ form.id ? 'Editar Ecossistema' : 'Novo Ecossistema' }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="mb-3"><label class="form-label fw-bold">Nome *</label><input v-model="form.nome" type="text" class="form-control" /></div>
          <div class="mb-3"><label class="form-label fw-bold">Descrição</label><textarea v-model="form.descricao" class="form-control" rows="3" /></div>
          <div v-if="modalError" class="alert alert-danger py-2">{{ modalError }}</div>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Cancelar</button>
          <button class="btn btn-ag-primary" :disabled="saving" @click="salvar">{{ saving ? 'Salvando...' : 'Salvar' }}</button>
        </div>
      </div>
    </div>
    <div v-if="showConfirm" class="ag-modal-overlay" @click.self="showConfirm = false">
      <div class="ag-modal" style="max-width:420px">
        <div class="ag-modal-header"><h5 class="mb-0">Confirmar exclusão</h5></div>
        <div class="ag-modal-body"><p>Excluir o ecossistema <strong>{{ itemParaExcluir?.nome }}</strong>?</p></div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="showConfirm = false">Cancelar</button>
          <button class="btn btn-danger" :disabled="saving" @click="excluir">{{ saving ? 'Excluindo...' : 'Excluir' }}</button>
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
