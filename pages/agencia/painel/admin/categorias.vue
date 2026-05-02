<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Configurações</div>
        <h1>Categorias</h1>
        <p>Gerenciar categorias de lojas</p>
      </div>
      <div class="qs-header-actions">
        <button class="qs-btn-primary" @click="abrirNovo">+ Nova Categoria</button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M4 6h16M4 10h16M4 14h16M4 18h16"/></svg>
        <h3>Nenhuma categoria encontrada</h3>
        <button class="qs-btn-primary" @click="abrirNovo">Criar primeira categoria</button>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead><tr><th>Nome</th><th>Descrição</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.nome }}</td>
              <td>{{ item.descricao || '—' }}</td>
              <td><span class="qs-badge" :class="item.ativo ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.ativo ? 'Ativo' : 'Inativo' }}</span></td>
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
          <h5>{{ form.id ? 'Editar Categoria' : 'Nova Categoria' }}</h5>
          <button class="qs-modal-close" @click="fecharModal"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-field"><label class="qs-label">Nome *</label><input v-model="form.nome" type="text" class="qs-input" placeholder="Nome da categoria" /></div>
          <div class="qs-field" style="margin-top:14px"><label class="qs-label">Descrição</label><input v-model="form.descricao" type="text" class="qs-input" placeholder="Descrição opcional" /></div>
          <label class="qs-checkbox-label" style="margin-top:14px">
            <input v-model="form.ativo" type="checkbox" class="qs-checkbox" />
            <span>Ativo</span>
          </label>
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
        <div class="qs-modal-body"><p>Excluir a categoria <strong>{{ itemParaExcluir?.nome }}</strong>? Esta ação não pode ser desfeita.</p></div>
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
import type { CategoriaAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const saving = ref(false);
const itens = ref<CategoriaAdmin[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<CategoriaAdmin | null>(null);
const form = reactive<{ id?: number; nome: string; descricao: string; ativo: boolean }>({ nome: '', descricao: '', ativo: true });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function abrirNovo() { Object.assign(form, { id: undefined, nome: '', descricao: '', ativo: true }); modalError.value = ''; showModal.value = true; }
function abrirEditar(item: CategoriaAdmin) { Object.assign(form, { id: item.id, nome: item.nome, descricao: item.descricao || '', ativo: item.ativo }); modalError.value = ''; showModal.value = true; }
function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: CategoriaAdmin) { itemParaExcluir.value = item; showConfirm.value = true; }
async function salvar() {
  if (!form.nome.trim()) { modalError.value = 'Nome é obrigatório.'; return; }
  saving.value = true; modalError.value = '';
  try {
    if (form.id) {
      await api.put(`/admin/categorias/${form.id}`, { nome: form.nome, descricao: form.descricao, ativo: form.ativo }, authHeader());
      const idx = itens.value.findIndex(i => i.id === form.id);
      if (idx !== -1) itens.value[idx] = { ...itens.value[idx], nome: form.nome, descricao: form.descricao, ativo: form.ativo };
    } else {
      const { data } = await api.post('/admin/categorias/criar', { nome: form.nome, descricao: form.descricao, ativo: form.ativo }, authHeader());
      itens.value.unshift(data as CategoriaAdmin);
    }
    fecharModal();
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao salvar categoria.'); }
  finally { saving.value = false; }
}
async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    await api.delete(`/admin/categorias/${itemParaExcluir.value.id}`, authHeader());
    itens.value = itens.value.filter(i => i.id !== itemParaExcluir.value?.id);
    showConfirm.value = false;
  } catch (e: unknown) { console.error('Erro ao excluir:', extractApiErrorMessage(e)); }
  finally { saving.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/categorias/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar categorias:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-field { display: flex; flex-direction: column; gap: 6px; }
.qs-label { font-size: 13px; font-weight: 600; color: var(--qs-gray-700); }
.qs-checkbox-label { display: flex; align-items: center; gap: 8px; cursor: pointer; font-size: 14px; color: var(--qs-ink); }
.qs-checkbox { width: 16px; height: 16px; accent-color: var(--qs-teal); }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; }
</style>
