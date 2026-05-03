<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Admin · Financeiro" title="Lançamentos" description="Lançamentos financeiros manuais da plataforma">
      <div class="qs-period-switch">
        <QsFilterChip :active="filtroTipo === ''" @click="filtroTipo = ''">Todos</QsFilterChip>
        <QsFilterChip :active="filtroTipo === 'Crédito'" @click="filtroTipo = 'Crédito'">Crédito</QsFilterChip>
        <QsFilterChip :active="filtroTipo === 'Débito'" @click="filtroTipo = 'Débito'">Débito</QsFilterChip>
      </div>
      <button class="qs-btn-primary" @click="abrirNovo">+ Novo Lançamento</button>
    </QsPageHeader>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <div class="qs-grid">
        <QsKpiCard label="Total Créditos" :value="totais.creditos" format="currency" dot-color="var(--qs-success)" />
        <QsKpiCard label="Total Débitos" :value="totais.debitos" format="currency" dot-color="var(--qs-danger)" />
        <QsKpiCard
          label="Saldo Líquido"
          :value="totais.liquido"
          format="currency"
          :dot-color="totais.liquido >= 0 ? 'var(--qs-teal)' : 'var(--qs-danger)'"
        />
        <QsKpiCard label="Total de Registros" :value="itensFiltrados.length" format="number" />
      </div>

      <section class="qs-card-section">
        <h2 class="qs-section-title">Histórico de Lançamentos</h2>
        <p class="qs-section-desc">
          {{ filtroTipo ? `Filtrando por "${filtroTipo}" — ` : '' }}{{ itensFiltrados.length }} registro{{ itensFiltrados.length !== 1 ? 's' : '' }} encontrado{{ itensFiltrados.length !== 1 ? 's' : '' }}.
        </p>

        <div v-if="itensFiltrados.length === 0" class="qs-empty-state">
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" color="var(--qs-gray-400)"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"/></svg>
          <h3>Nenhum lançamento encontrado</h3>
          <p v-if="filtroTipo">Filtrando por "{{ filtroTipo }}" — <button class="qs-link" @click="filtroTipo = ''">Limpar filtro</button></p>
        </div>
        <div v-else class="qs-table-wrap">
          <table class="qs-table">
            <thead>
              <tr>
                <th>Descrição</th>
                <th>Tipo</th>
                <th class="align-right">Valor</th>
                <th>Data</th>
                <th class="col-action"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in itensFiltrados" :key="item.id">
                <td>{{ item.descricao }}</td>
                <td>
                  <span class="qs-badge" :class="item.tipo === 'Crédito' ? 'qs-badge-success' : 'qs-badge-danger'">{{ item.tipo }}</span>
                </td>
                <td class="align-right cell-bold" :class="item.tipo === 'Crédito' ? 'num-green' : 'num-red'">
                  {{ item.tipo === 'Débito' ? '−' : '+' }}{{ formatCurrency(item.valor) }}
                </td>
                <td>{{ formatDate(item.data) }}</td>
                <td><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
              </tr>
            </tbody>
          </table>
        </div>
      </section>
    </template>

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
            <div class="detail-grid">
              <div class="detail-item col-full">
                <span class="detail-label">Descrição</span>
                <span class="detail-value">{{ selecionado.descricao }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">Tipo</span>
                <span class="qs-badge" :class="selecionado.tipo === 'Crédito' ? 'qs-badge-success' : 'qs-badge-danger'">{{ selecionado.tipo }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">Valor</span>
                <span class="detail-value" :class="selecionado.tipo === 'Crédito' ? 'num-green' : 'num-red'">{{ formatCurrency(selecionado.valor) }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">Data</span>
                <span class="detail-value">{{ formatDate(selecionado.data) }}</span>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="field-group">
              <label class="field-label">Descrição *</label>
              <input v-model="form.descricao" type="text" class="field-input" placeholder="Ex: Pagamento fornecedor..." />
            </div>
            <div class="field-group">
              <label class="field-label">Tipo</label>
              <div class="chip-row">
                <QsFilterChip :active="form.tipo === 'Crédito'" @click="form.tipo = 'Crédito'">Crédito</QsFilterChip>
                <QsFilterChip :active="form.tipo === 'Débito'" @click="form.tipo = 'Débito'">Débito</QsFilterChip>
              </div>
            </div>
            <div class="field-group">
              <label class="field-label">Valor *</label>
              <input v-model.number="form.valor" type="number" step="0.01" class="field-input" placeholder="0,00" />
            </div>
            <div v-if="modalError" class="alert-danger">{{ modalError }}</div>
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
.qs-header-text { max-width: 720px; }
.qs-header-actions { display: flex; align-items: center; gap: 12px; flex-shrink: 0; flex-wrap: wrap; }
.qs-period-switch { display: flex; gap: 6px; }

/* ── Table ── */
.qs-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: 14px; }
.qs-table thead th {
  background: var(--qs-gray-50);
  color: var(--qs-gray-500);
  font-size: 11px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  padding: 10px 14px;
  border-bottom: 1px solid var(--qs-gray-200);
  white-space: nowrap;
}
.qs-table tbody td { padding: 12px 14px; border-bottom: 1px solid var(--qs-gray-100); vertical-align: middle; color: var(--qs-ink); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.qs-table tbody tr:hover td { background: var(--qs-gray-50); }
.align-right { text-align: right; }
.col-action { width: 60px; }
.cell-bold { font-weight: 600; font-variant-numeric: tabular-nums; }
.num-green { color: var(--qs-success); }
.num-red { color: var(--qs-danger); }

/* ── Badges ── */
.qs-badge {
  display: inline-block;
  padding: 3px 10px;
  border-radius: var(--qs-radius-pill);
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.02em;
  white-space: nowrap;
}
.qs-badge-success { background: #dcfce7; color: #16a34a; }
.qs-badge-warn    { background: #fef3c7; color: #d97706; }
.qs-badge-danger  { background: #fee2e2; color: #dc2626; }
.qs-badge-neutral { background: var(--qs-gray-100); color: var(--qs-gray-500); }

/* ── Small button ── */
.qs-btn-sm-outline {
  padding: 5px 12px;
  font-size: 12px;
  font-weight: 600;
  border: 1px solid var(--qs-teal);
  color: var(--qs-teal);
  background: #fff;
  border-radius: var(--qs-radius-md);
  cursor: pointer;
  transition: all var(--qs-duration) var(--qs-ease);
  white-space: nowrap;
}
.qs-btn-sm-outline:hover { background: var(--qs-teal); color: #fff; }
.qs-btn-secondary {
  padding: 10px 18px;
  font-size: 14px;
  font-weight: 500;
  border: 1px solid var(--qs-gray-200);
  color: var(--qs-gray-700);
  background: #fff;
  border-radius: var(--qs-radius-md);
  cursor: pointer;
  transition: all var(--qs-duration) var(--qs-ease);
}
.qs-btn-secondary:hover { background: var(--qs-gray-50); }

/* ── Empty state ── */
.qs-empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  padding: 56px 24px;
  text-align: center;
}
.qs-empty-state h3 { font-size: 18px; font-weight: 600; color: var(--qs-ink); margin: 0; }
.qs-empty-state p { font-size: 14px; color: var(--qs-gray-500); margin: 0; }
.qs-link { background: none; border: none; color: var(--qs-teal); font-size: 14px; cursor: pointer; padding: 0; text-decoration: underline; }

/* ── Modal ── */
.qs-modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 28, 35, 0.5);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9000;
  padding: 24px;
}
.qs-modal {
  background: #fff;
  border-radius: var(--qs-radius-lg);
  box-shadow: var(--qs-shadow-lg);
  width: 100%;
  max-width: 480px;
  overflow: hidden;
}
.qs-modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 20px 24px;
  border-bottom: 1px solid var(--qs-gray-100);
}
.qs-modal-header h5 { font-size: 17px; font-weight: 700; color: var(--qs-ink); margin: 0; }
.qs-modal-close {
  width: 32px; height: 32px;
  border: none; background: var(--qs-gray-100);
  border-radius: var(--qs-radius-md);
  cursor: pointer;
  display: flex; align-items: center; justify-content: center;
  color: var(--qs-gray-500);
  transition: all var(--qs-duration) var(--qs-ease);
}
.qs-modal-close:hover { background: var(--qs-gray-200); color: var(--qs-ink); }
.qs-modal-body { padding: 24px; }
.qs-modal-footer {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 10px;
  padding: 16px 24px;
  border-top: 1px solid var(--qs-gray-100);
  background: var(--qs-gray-50);
}

/* ── Detail grid ── */
.detail-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.col-full { grid-column: 1 / -1; }
.detail-item { display: flex; flex-direction: column; gap: 4px; }
.detail-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.detail-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }

/* ── Form ── */
.field-group { margin-bottom: 16px; }
.field-label { display: block; font-size: 13px; font-weight: 600; color: var(--qs-gray-700); margin-bottom: 6px; }
.field-input {
  width: 100%; padding: 10px 14px;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  font-family: inherit; font-size: 14px;
  outline: none; box-sizing: border-box;
  transition: border-color var(--qs-duration) var(--qs-ease);
}
.field-input:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.chip-row { display: flex; gap: 8px; flex-wrap: wrap; margin-top: 4px; }
.alert-danger {
  background: #fef2f2; color: var(--qs-danger);
  border: 1px solid #fecaca;
  border-radius: var(--qs-radius-md);
  padding: 10px 14px; font-size: 14px; margin-top: 8px;
}
</style>
