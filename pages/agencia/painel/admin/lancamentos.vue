<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Financeiro</div>
        <h1>Lançamentos</h1>
        <p>Lançamentos financeiros da plataforma</p>
      </div>
      <div class="qs-header-actions">
        <button class="qs-btn-primary" @click="abrirNovo">+ Novo Lançamento</button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"/></svg>
        <h3>Nenhum lançamento encontrado</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Descrição</th><th>Tipo</th><th>Valor</th><th>Data</th><th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td>{{ item.descricao }}</td>
              <td>
                <span class="qs-badge" :class="item.tipo === 'Crédito' ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.tipo }}</span>
              </td>
              <td class="qs-cell-bold" :class="item.tipo === 'Crédito' ? 'qs-cell-credit' : 'qs-cell-debit'">
                {{ formatCurrency(item.valor) }}
              </td>
              <td>{{ formatDate(item.data) }}</td>
              <td><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal">
        <div class="qs-modal-header">
          <h5>{{ selecionado ? 'Detalhes do Lançamento' : 'Novo Lançamento' }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <template v-if="selecionado">
            <div class="qs-detail-grid">
              <div class="qs-detail-item qs-col-full"><span class="qs-detail-label">Descrição</span><span class="qs-detail-value">{{ selecionado.descricao }}</span></div>
              <div class="qs-detail-item"><span class="qs-detail-label">Tipo</span><span class="qs-badge" :class="selecionado.tipo === 'Crédito' ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.tipo }}</span></div>
              <div class="qs-detail-item"><span class="qs-detail-label">Valor</span><span class="qs-detail-value">{{ formatCurrency(selecionado.valor) }}</span></div>
              <div class="qs-detail-item"><span class="qs-detail-label">Data</span><span class="qs-detail-value">{{ formatDate(selecionado.data) }}</span></div>
            </div>
          </template>
          <template v-else>
            <div class="qs-field-group">
              <label class="qs-label">Descrição *</label>
              <input v-model="form.descricao" type="text" class="qs-input" placeholder="Ex: Pagamento fornecedor..." />
            </div>
            <div class="qs-field-group">
              <label class="qs-label">Tipo</label>
              <select v-model="form.tipo" class="qs-select">
                <option value="Crédito">Crédito</option>
                <option value="Débito">Débito</option>
              </select>
            </div>
            <div class="qs-field-group">
              <label class="qs-label">Valor *</label>
              <input v-model.number="form.valor" type="number" step="0.01" class="qs-input" placeholder="0,00" />
            </div>
            <div v-if="modalError" class="qs-alert-danger">{{ modalError }}</div>
          </template>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">{{ selecionado ? 'Fechar' : 'Cancelar' }}</button>
          <button v-if="!selecionado" class="qs-btn-primary" :disabled="saving" @click="salvar">
            {{ saving ? 'Salvando...' : 'Salvar' }}
          </button>
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

<style scoped>
.qs-detail-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.qs-col-full { grid-column: 1 / -1; }
.qs-detail-item { display: flex; flex-direction: column; gap: 4px; }
.qs-detail-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-field-group { margin-bottom: 16px; }
.qs-label { display: block; font-size: 13px; font-weight: 600; color: var(--qs-gray-700); margin-bottom: 6px; }
.qs-input { width: 100%; padding: 10px 14px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-family: inherit; font-size: 14px; outline: none; box-sizing: border-box; }
.qs-input:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.qs-select { width: 100%; padding: 10px 14px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-family: inherit; font-size: 14px; background: #fff; outline: none; }
.qs-select:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.qs-cell-credit { color: #16a34a; }
.qs-cell-debit { color: #dc2626; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; margin-top: 8px; }
</style>
