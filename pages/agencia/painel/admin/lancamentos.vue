<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Financeiro</div>
        <h1>Lançamentos</h1>
        <p>Lançamentos financeiros manuais da plataforma</p>
      </div>
      <div class="qs-header-actions">
        <button class="qs-btn-primary" @click="abrirNovo">+ Novo Lançamento</button>
      </div>
    </div>

    <div class="qs-kpi-strip">
      <QsKpiCard label="Total Créditos" :value="totais.creditos" format="currency" dotColor="#16a34a" />
      <QsKpiCard label="Total Débitos" :value="totais.debitos" format="currency" dotColor="#dc2626" />
      <QsKpiCard label="Saldo Líquido" :value="totais.liquido" format="currency" :dotColor="totais.liquido >= 0 ? '#2F7785' : '#dc2626'" />
      <QsKpiCard label="Total de Registros" :value="itensFiltrados.length" format="number" />
    </div>

    <div class="qs-filter-bar">
      <span class="qs-filter-bar-label">Tipo:</span>
      <QsFilterChip label="Todos" :active="filtroTipo === ''" @click="filtroTipo = ''" />
      <QsFilterChip label="Crédito" :active="filtroTipo === 'Crédito'" @click="filtroTipo = 'Crédito'" />
      <QsFilterChip label="Débito" :active="filtroTipo === 'Débito'" @click="filtroTipo = 'Débito'" />
    </div>

    <template v-if="loading">
      <div class="qs-card-section">
        <div class="qs-skeleton-table">
          <div v-for="n in 6" :key="n" class="qs-skeleton-row">
            <div class="qs-skeleton qs-skeleton-td-wide" />
            <div class="qs-skeleton qs-skeleton-td-sm" />
            <div class="qs-skeleton qs-skeleton-td-sm" />
            <div class="qs-skeleton qs-skeleton-td-sm" />
          </div>
        </div>
      </div>
    </template>

    <div v-else class="qs-card-section">
      <div v-if="itensFiltrados.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"/></svg>
        <h3>Nenhum lançamento encontrado</h3>
        <p v-if="filtroTipo" class="qs-empty-hint">Filtrando por "{{ filtroTipo }}" — <button class="qs-link" @click="filtroTipo = ''">Limpar filtro</button></p>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Descrição</th>
              <th>Tipo</th>
              <th class="qs-text-right">Valor</th>
              <th>Data</th>
              <th class="no-print"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itensFiltrados" :key="item.id">
              <td>{{ item.descricao }}</td>
              <td>
                <span class="qs-badge" :class="item.tipo === 'Crédito' ? 'qs-badge-success' : 'qs-badge-danger'">{{ item.tipo }}</span>
              </td>
              <td class="qs-text-right qs-cell-bold" :class="item.tipo === 'Crédito' ? 'qs-num-green' : 'qs-num-red'">
                {{ item.tipo === 'Débito' ? '−' : '+' }}{{ formatCurrency(item.valor) }}
              </td>
              <td>{{ formatDate(item.data) }}</td>
              <td class="no-print"><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
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
              <div class="qs-detail-item"><span class="qs-detail-label">Tipo</span><span class="qs-badge" :class="selecionado.tipo === 'Crédito' ? 'qs-badge-success' : 'qs-badge-danger'">{{ selecionado.tipo }}</span></div>
              <div class="qs-detail-item"><span class="qs-detail-label">Valor</span><span class="qs-detail-value" :class="selecionado.tipo === 'Crédito' ? 'qs-num-green' : 'qs-num-red'">{{ formatCurrency(selecionado.valor) }}</span></div>
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
              <div class="qs-chip-row">
                <QsFilterChip label="Crédito" :active="form.tipo === 'Crédito'" @click="form.tipo = 'Crédito'" />
                <QsFilterChip label="Débito" :active="form.tipo === 'Débito'" @click="form.tipo = 'Débito'" />
              </div>
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
const filtroTipo = ref('');
const form = reactive<{ descricao: string; tipo: string; valor: number }>({ descricao: '', tipo: 'Crédito', valor: 0 });

const itensFiltrados = computed(() =>
  filtroTipo.value ? itens.value.filter(i => i.tipo === filtroTipo.value) : itens.value
);

const totais = computed(() => {
  const lista = itens.value;
  const creditos = lista.filter(i => i.tipo === 'Crédito').reduce((s, i) => s + (i.valor || 0), 0);
  const debitos = lista.filter(i => i.tipo === 'Débito').reduce((s, i) => s + (i.valor || 0), 0);
  return { creditos, debitos, liquido: creditos - debitos };
});

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
.qs-kpi-strip { display: grid; grid-template-columns: repeat(4, 1fr); gap: 16px; margin-bottom: 20px; }
@media (max-width: 768px) { .qs-kpi-strip { grid-template-columns: 1fr 1fr; } }
@media (max-width: 480px) { .qs-kpi-strip { grid-template-columns: 1fr; } }

.qs-filter-bar { display: flex; align-items: center; gap: 8px; margin-bottom: 20px; flex-wrap: wrap; }
.qs-filter-bar-label { font-size: 12px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.05em; margin-right: 4px; }
.qs-chip-row { display: flex; gap: 8px; flex-wrap: wrap; margin-top: 6px; }

.qs-skeleton-table { display: flex; flex-direction: column; gap: 12px; padding: 8px 0; }
.qs-skeleton-row { display: flex; gap: 16px; align-items: center; }
.qs-skeleton-td-wide { flex: 2; height: 18px; border-radius: 6px; }
.qs-skeleton-td-sm { flex: 1; height: 18px; border-radius: 6px; }

.qs-text-right { text-align: right; }
.qs-num-green { color: #16a34a; font-variant-numeric: tabular-nums; }
.qs-num-red { color: #dc2626; font-variant-numeric: tabular-nums; }
.qs-empty-hint { font-size: 13px; color: var(--qs-gray-400); margin: 0; }
.qs-link { background: none; border: none; color: var(--qs-teal); font-size: 13px; cursor: pointer; padding: 0; text-decoration: underline; }

.qs-detail-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.qs-col-full { grid-column: 1 / -1; }
.qs-detail-item { display: flex; flex-direction: column; gap: 4px; }
.qs-detail-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }

.qs-field-group { margin-bottom: 16px; }
.qs-label { display: block; font-size: 13px; font-weight: 600; color: var(--qs-gray-700); margin-bottom: 6px; }
.qs-input { width: 100%; padding: 10px 14px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-family: inherit; font-size: 14px; outline: none; box-sizing: border-box; }
.qs-input:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; margin-top: 8px; }
</style>
