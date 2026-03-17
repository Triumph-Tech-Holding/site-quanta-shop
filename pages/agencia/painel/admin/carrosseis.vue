<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Carrosseis</h1><p>Gerenciar banners e carrosseis</p></div>
      <button class="btn btn-ag-primary" @click="abrirNovo">+ Novo Banner</button>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum banner encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Título</th><th>URL</th><th>Ativo</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.titulo }}</td>
              <td><a v-if="item.url" :href="item.url" target="_blank" class="text-truncate d-block" style="max-width:200px">{{ item.url }}</a><span v-else>—</span></td>
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
      <div class="ag-modal">
        <div class="ag-modal-header"><h5 class="mb-0">{{ form.id ? 'Editar Banner' : 'Novo Banner' }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="mb-3"><label class="form-label fw-bold">Título *</label><input v-model="form.titulo" type="text" class="form-control" /></div>
          <div class="mb-3"><label class="form-label fw-bold">URL da Imagem</label><input v-model="form.url" type="url" class="form-control" placeholder="https://..." /></div>
          <div class="mb-3"><label class="form-label fw-bold">URL de Destino</label><input v-model="form.urlDestino" type="url" class="form-control" placeholder="https://..." /></div>
          <div class="mb-3"><div class="form-check"><input v-model="form.ativo" type="checkbox" class="form-check-input" id="car-ativo" /><label class="form-check-label" for="car-ativo">Ativo</label></div></div>
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
        <div class="ag-modal-body"><p>Excluir o banner <strong>{{ itemParaExcluir?.titulo }}</strong>?</p></div>
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
import type { CarrosselAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const saving = ref(false);
const itens = ref<CarrosselAdmin[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<CarrosselAdmin | null>(null);
const form = reactive<{ id?: number; titulo: string; url: string; urlDestino: string; ativo: boolean }>({ titulo: '', url: '', urlDestino: '', ativo: true });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function abrirNovo() { Object.assign(form, { id: undefined, titulo: '', url: '', urlDestino: '', ativo: true }); modalError.value = ''; showModal.value = true; }
function abrirEditar(item: CarrosselAdmin) { Object.assign(form, { id: item.id, titulo: item.titulo, url: item.url || '', urlDestino: (item.urlDestino as string) || '', ativo: item.ativo }); modalError.value = ''; showModal.value = true; }
function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: CarrosselAdmin) { itemParaExcluir.value = item; showConfirm.value = true; }
async function salvar() {
  if (!form.titulo.trim()) { modalError.value = 'Título é obrigatório.'; return; }
  saving.value = true; modalError.value = '';
  try {
    const payload = { titulo: form.titulo, url: form.url, urlDestino: form.urlDestino, ativo: form.ativo };
    if (form.id) {
      await api.put(`/admin/carrosseis/${form.id}`, payload, authHeader());
      const idx = itens.value.findIndex(i => i.id === form.id);
      if (idx !== -1) itens.value[idx] = { ...itens.value[idx], ...payload };
    } else {
      const { data } = await api.post('/admin/carrosseis/criar', payload, authHeader());
      itens.value.unshift(data as CarrosselAdmin);
    }
    fecharModal();
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao salvar banner.'); }
  finally { saving.value = false; }
}
async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    await api.delete(`/admin/carrosseis/${itemParaExcluir.value.id}`, authHeader());
    itens.value = itens.value.filter(i => i.id !== itemParaExcluir.value?.id);
    showConfirm.value = false;
  } catch (e: unknown) { console.error('Erro ao excluir:', extractApiErrorMessage(e)); }
  finally { saving.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/carrosseis/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar carrosseis:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
