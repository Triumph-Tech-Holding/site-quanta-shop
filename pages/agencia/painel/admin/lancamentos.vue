<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Lançamentos</h1><p>Lançamentos financeiros da plataforma</p></div>
      <button class="btn btn-ag-primary" @click="abrirNovo">+ Novo Lançamento</button>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum lançamento encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Descrição</th><th>Tipo</th><th>Valor</th><th>Data</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td>{{ item.descricao }}</td>
              <td><span class="badge-ag" :class="item.tipo === 'Crédito' ? 'badge-ag-success' : 'badge-ag-warning'">{{ item.tipo }}</span></td>
              <td class="fw-bold" :class="item.tipo === 'Crédito' ? 'text-success' : 'text-danger'">{{ formatCurrency(item.valor) }}</td>
              <td>{{ formatDate(item.data) }}</td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal">
        <div class="ag-modal-header"><h5 class="mb-0">{{ selecionado ? 'Detalhes do Lançamento' : 'Novo Lançamento' }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <template v-if="selecionado">
            <div class="row g-2">
              <div class="col-12"><strong>Descrição:</strong> {{ selecionado.descricao }}</div>
              <div class="col-6"><strong>Tipo:</strong> {{ selecionado.tipo }}</div>
              <div class="col-6"><strong>Valor:</strong> {{ formatCurrency(selecionado.valor) }}</div>
              <div class="col-6"><strong>Data:</strong> {{ formatDate(selecionado.data) }}</div>
            </div>
          </template>
          <template v-else>
            <div class="mb-3"><label class="form-label fw-bold">Descrição *</label><input v-model="form.descricao" type="text" class="form-control" /></div>
            <div class="mb-3"><label class="form-label fw-bold">Tipo</label><select v-model="form.tipo" class="form-select"><option value="Crédito">Crédito</option><option value="Débito">Débito</option></select></div>
            <div class="mb-3"><label class="form-label fw-bold">Valor *</label><input v-model.number="form.valor" type="number" step="0.01" class="form-control" /></div>
            <div v-if="modalError" class="alert alert-danger py-2">{{ modalError }}</div>
          </template>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">{{ selecionado ? 'Fechar' : 'Cancelar' }}</button>
          <button v-if="!selecionado" class="btn btn-ag-primary" :disabled="saving" @click="salvar">{{ saving ? 'Salvando...' : 'Salvar' }}</button>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { LancamentoAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const saving = ref(false);
const itens = ref<LancamentoAdmin[]>([]);
const showModal = ref(false);
const modalError = ref('');
const selecionado = ref<LancamentoAdmin | null>(null);
const form = reactive<{ descricao: string; tipo: string; valor: number }>({ descricao: '', tipo: 'Crédito', valor: 0 });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function abrirNovo() { selecionado.value = null; Object.assign(form, { descricao: '', tipo: 'Crédito', valor: 0 }); modalError.value = ''; showModal.value = true; }
function verDetalhes(item: LancamentoAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function salvar() {
  if (!form.descricao.trim() || !form.valor) { modalError.value = 'Descrição e valor são obrigatórios.'; return; }
  saving.value = true; modalError.value = '';
  try {
    const { data } = await api.post('/admin/lancamentos/criar', { descricao: form.descricao, tipo: form.tipo, valor: form.valor }, authHeader());
    itens.value.unshift(data as LancamentoAdmin);
    fecharModal();
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao salvar lançamento.'); }
  finally { saving.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/lancamentos/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar lançamentos:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
