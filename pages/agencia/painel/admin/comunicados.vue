<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Comunicados</h1><p>Gerenciar comunicados aos usuários</p></div>
      <button class="btn btn-ag-primary" @click="abrirNovo">+ Novo Comunicado</button>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum comunicado encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Título</th><th>Data</th><th>Ativo</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.titulo }}</td>
              <td>{{ formatDate(item.data) }}</td>
              <td><span class="badge-ag" :class="item.ativo ? 'badge-ag-success' : 'badge-ag-warning'">{{ item.ativo ? 'Ativo' : 'Inativo' }}</span></td>
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
      <div class="ag-modal" style="max-width:600px">
        <div class="ag-modal-header"><h5 class="mb-0">{{ form.id ? 'Editar Comunicado' : 'Novo Comunicado' }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="mb-3"><label class="form-label fw-bold">Título *</label><input v-model="form.titulo" type="text" class="form-control" /></div>
          <div class="mb-3"><label class="form-label fw-bold">Conteúdo *</label><textarea v-model="form.conteudo" class="form-control" rows="5" /></div>
          <div class="mb-3"><div class="form-check"><input v-model="form.ativo" type="checkbox" class="form-check-input" id="com-ativo" /><label class="form-check-label" for="com-ativo">Ativo</label></div></div>
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
        <div class="ag-modal-body"><p>Excluir o comunicado <strong>{{ itemParaExcluir?.titulo }}</strong>?</p></div>
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
import type { ComunicadoAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const saving = ref(false);
const itens = ref<ComunicadoAdmin[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<ComunicadoAdmin | null>(null);
const form = reactive<{ id?: number; titulo: string; conteudo: string; ativo: boolean }>({ titulo: '', conteudo: '', ativo: true });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function abrirNovo() { Object.assign(form, { id: undefined, titulo: '', conteudo: '', ativo: true }); modalError.value = ''; showModal.value = true; }
function abrirEditar(item: ComunicadoAdmin) { Object.assign(form, { id: item.id, titulo: item.titulo, conteudo: item.conteudo, ativo: item.ativo }); modalError.value = ''; showModal.value = true; }
function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: ComunicadoAdmin) { itemParaExcluir.value = item; showConfirm.value = true; }
async function salvar() {
  if (!form.titulo.trim() || !form.conteudo.trim()) { modalError.value = 'Título e conteúdo são obrigatórios.'; return; }
  saving.value = true; modalError.value = '';
  try {
    if (form.id) {
      await api.put(`/admin/comunicados/${form.id}`, { titulo: form.titulo, conteudo: form.conteudo, ativo: form.ativo }, authHeader());
      const idx = itens.value.findIndex(i => i.id === form.id);
      if (idx !== -1) itens.value[idx] = { ...itens.value[idx], titulo: form.titulo, conteudo: form.conteudo, ativo: form.ativo };
    } else {
      const { data } = await api.post('/admin/comunicados/criar', { titulo: form.titulo, conteudo: form.conteudo, ativo: form.ativo }, authHeader());
      itens.value.unshift(data as ComunicadoAdmin);
    }
    fecharModal();
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao salvar comunicado.'); }
  finally { saving.value = false; }
}
async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    await api.delete(`/admin/comunicados/${itemParaExcluir.value.id}`, authHeader());
    itens.value = itens.value.filter(i => i.id !== itemParaExcluir.value?.id);
    showConfirm.value = false;
  } catch (e: unknown) { console.error('Erro ao excluir:', extractApiErrorMessage(e)); }
  finally { saving.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/comunicados/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar comunicados:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
