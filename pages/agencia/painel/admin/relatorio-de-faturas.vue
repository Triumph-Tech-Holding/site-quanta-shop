<template>
  <div class="qs-page no-print">
    <QsPageHeader eyebrow="Admin · Relatórios" title="Relatório de Faturas" description="Faturas de cashback dos credenciados">
      <div class="qs-period-switch no-print">
        <QsFilterChip :active="filtroStatus === null" @click="filtroStatus = null">Todos</QsFilterChip>
        <QsFilterChip :active="filtroStatus === 2" @click="filtroStatus = 2">Pago</QsFilterChip>
        <QsFilterChip :active="filtroStatus === 1" @click="filtroStatus = 1">Pendente</QsFilterChip>
        <QsFilterChip :active="filtroStatus === 3" @click="filtroStatus = 3">Cancelado</QsFilterChip>
      </div>
      <button class="qs-btn-outline no-print" :disabled="itens.length === 0" @click="gerarPdf">⬇ PDF</button>
      <button class="qs-btn-primary no-print" :disabled="loading" @click="carregar">Atualizar</button>
    </QsPageHeader>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <div class="lgpd-banner" :class="isMaster ? 'lgpd-master' : 'lgpd-default'">
        <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>
        <span v-if="isMaster">Modo Master ativo — dados sensíveis visíveis. Cada revelação é registrada na auditoria LGPD.</span>
        <span v-else>Dados pessoais mascarados (LGPD). Apenas usuários Master podem revelar informações individuais.</span>
      </div>

      <div class="qs-grid">
        <QsKpiCard label="Total Pago" :value="totais.totalPago" format="currency" dot-color="var(--qs-success)" badge="Pago" badge-tone="success" />
        <QsKpiCard label="Pendente" :value="totais.totalPendente" format="currency" dot-color="var(--qs-warn)" badge="A receber" badge-tone="warn" />
        <QsKpiCard label="Total de Faturas" :value="itensFiltrados.length" format="number" dot-color="var(--qs-teal)" />
      </div>

      <section class="qs-card-section">
        <div class="section-head-row">
          <div>
            <h2 class="qs-section-title">Faturas</h2>
            <p class="qs-section-desc">{{ itensFiltrados.length }} fatura{{ itensFiltrados.length !== 1 ? 's' : '' }} encontrada{{ itensFiltrados.length !== 1 ? 's' : '' }}.</p>
          </div>
          <div class="filter-inline no-print">
            <label class="date-label">A partir de</label>
            <input v-model="filtro.dataInicial" type="date" class="date-input" @change="carregar" />
            <div class="search-field">
              <svg class="search-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
              <input v-model="filtro.loginCredenciado" type="text" class="search-input" placeholder="Filtrar por login..." @keydown.enter="carregar" />
            </div>
          </div>
        </div>

        <div v-if="itensFiltrados.length === 0" class="qs-empty-state">
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" color="var(--qs-gray-400)"><rect x="2" y="5" width="20" height="14" rx="2"/><line x1="2" y1="10" x2="22" y2="10"/></svg>
          <h3>Nenhuma fatura encontrada</h3>
          <p>Ajuste os filtros e clique em Atualizar.</p>
        </div>
        <div v-else class="qs-table-wrap">
          <table class="qs-table">
            <thead>
              <tr>
                <th>#</th>
                <th>Login / Credenciado <span class="lgpd-th-tag">🔒 LGPD</span></th>
                <th>Patrocinador</th>
                <th>Cidade</th>
                <th>Cashback</th>
                <th class="align-right">Valor</th>
                <th>Data Pedido</th>
                <th>Status</th>
                <th class="col-action no-print"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in itensFiltrados" :key="item.IdPedido">
                <td class="cell-muted">#{{ item.IdPedido }}</td>
                <td>
                  <span class="cell-bold masked-value">{{ maskEmail(item.Login) }}</span>
                  <span v-if="!isMaster" class="lgpd-lock" title="Dado protegido por LGPD — apenas Master revela">🔒</span>
                </td>
                <td>{{ item.Patrocinador ? maskName(item.Patrocinador) : '—' }}</td>
                <td>{{ item.Cidade || '—' }}</td>
                <td>{{ item.Cashback }}</td>
                <td class="align-right cell-bold num-teal">{{ formatCurrency(item.ValorPedido) }}</td>
                <td>{{ formatDate(item.DataPedido) }}</td>
                <td><span class="qs-badge" :class="badgeStatus(item.Status)">{{ labelStatus(item.Status) }}</span></td>
                <td class="no-print"><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
              </tr>
            </tbody>
          </table>
        </div>
      </section>
    </template>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:520px">
        <div class="qs-modal-header">
          <h5>Fatura #{{ selecionado.IdPedido }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="info-grid">
            <div class="info-item col-full">
              <span class="info-label">Login / Credenciado <span class="lgpd-tag">🔒 LGPD</span></span>
              <div class="reveal-row">
                <span class="info-value">{{ revelado.login ? selecionado.Login : maskEmail(selecionado.Login) }}</span>
                <button v-if="isMaster && !revelado.login" class="btn-reveal" @click="revelar('login')">Revelar</button>
                <span v-if="!isMaster" class="lgpd-lock" title="Apenas Master pode revelar">🔒</span>
              </div>
            </div>
            <div class="info-item col-full">
              <span class="info-label">Nome <span class="lgpd-tag">🔒 LGPD</span></span>
              <div class="reveal-row">
                <span class="info-value">{{ revelado.nome ? selecionado.Nome : maskName(selecionado.Nome) }}</span>
                <button v-if="isMaster && !revelado.nome" class="btn-reveal" @click="revelar('nome')">Revelar</button>
                <span v-if="!isMaster" class="lgpd-lock" title="Apenas Master pode revelar">🔒</span>
              </div>
            </div>
            <div class="info-item"><span class="info-label">Patrocinador</span><span class="info-value">{{ selecionado.Patrocinador ? maskName(selecionado.Patrocinador) : '—' }}</span></div>
            <div class="info-item"><span class="info-label">Cidade</span><span class="info-value">{{ selecionado.Cidade || '—' }}</span></div>
            <div class="info-item"><span class="info-label">Categoria</span><span class="info-value">{{ selecionado.Categoria || '—' }}</span></div>
            <div class="info-item"><span class="info-label">Cashback</span><span class="info-value">{{ selecionado.Cashback }}</span></div>
            <div class="info-item"><span class="info-label">Valor</span><span class="info-value num-teal">{{ formatCurrency(selecionado.ValorPedido) }}</span></div>
            <div class="info-item">
              <span class="info-label">Status</span>
              <span class="qs-badge" :class="badgeStatus(selecionado.Status)">{{ labelStatus(selecionado.Status) }}</span>
            </div>
            <div class="info-item"><span class="info-label">Data Pedido</span><span class="info-value">{{ formatDate(selecionado.DataPedido) }}</span></div>
            <div class="info-item"><span class="info-label">Data Pagamento</span><span class="info-value">{{ selecionado.DataPagamento ? formatDate(selecionado.DataPagamento) : '—' }}</span></div>
          </div>
          <div v-if="isMaster" class="master-note">
            <svg width="13" height="13" viewBox="0 0 24 24" fill="currentColor"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>
            Você é Master. Cada revelação é registrada na auditoria LGPD.
          </div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Fechar</button>
        </div>
      </div>
    </div>

    <div v-if="erro" class="alert-danger">{{ erro }}</div>
  </div>
</template>

<style>
@media print {
  .no-print, nav, aside, header, .agencia-sidebar { display: none !important; }
  body { background: white !important; }
}
</style>

<script setup lang="ts">
import { useAgenciaStore } from '~/pinia/useAgenciaStore';
import { useApi } from '~/composables/useApi';
import { maskEmail } from '~/utils/lgpd-mask';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(false);
const erro = ref('');
const itens = ref<any[]>([]);
const showModal = ref(false);
const selecionado = ref<any>(null);
const filtroStatus = ref<number | null>(null);
const revelado = reactive<{ login: boolean; nome: boolean }>({ login: false, nome: false });

const hoje = new Date();
const filtro = reactive({
  dataInicial: new Date(hoje.getFullYear(), hoje.getMonth() - 2, 1).toISOString().split('T')[0],
  loginCredenciado: '',
});

const isMaster = computed<boolean>(() => {
  const token = agenciaStore.getToken();
  if (!token) return false;
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.master === true || payload.Master === true || payload.perfil === 'Master';
  } catch { return false; }
});

const itensFiltrados = computed(() =>
  filtroStatus.value === null
    ? itens.value
    : itens.value.filter(i => i.Status === filtroStatus.value)
);

const totais = computed(() => ({
  totalPago: itensFiltrados.value.filter(i => i.Status === 2 || i.DataPagamento).reduce((s, i) => s + (i.ValorPedido || 0), 0),
  totalPendente: itensFiltrados.value.filter(i => i.Status !== 2 && !i.DataPagamento).reduce((s, i) => s + (i.ValorPedido || 0), 0),
}));

function maskName(nome: string | null | undefined): string {
  if (!nome) return '—';
  const parts = nome.trim().split(' ');
  if (parts.length === 1) return parts[0][0] + '*'.repeat(Math.max(2, parts[0].length - 1));
  return parts[0] + ' ' + '*'.repeat(parts.slice(1).join(' ').length);
}

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }

function verDetalhes(item: any) {
  selecionado.value = item;
  revelado.login = false;
  revelado.nome = false;
  showModal.value = true;
}
function fecharModal() { showModal.value = false; selecionado.value = null; }
function gerarPdf() { window.print(); }
function revelar(campo: 'login' | 'nome') { if (isMaster.value) revelado[campo] = true; }

function badgeStatus(status: number) {
  if (status === 2) return 'qs-badge-success';
  if (status === 3) return 'qs-badge-danger';
  return 'qs-badge-warn';
}
function labelStatus(status: number) {
  if (status === 2) return 'Pago';
  if (status === 3) return 'Cancelado';
  return 'Pendente';
}

async function carregar() {
  loading.value = true; erro.value = '';
  try {
    const body: any = { pagina: 1, quantidadePorPagina: 200 };
    if (filtro.dataInicial) body.dataInicial = filtro.dataInicial;
    if (filtro.loginCredenciado) body.loginCredenciado = filtro.loginCredenciado;
    const { data } = await api.post('/Admin/ObterFaturas', body, authHeader());
    itens.value = Array.isArray(data) ? data : (data?.faturas || data?.items || []);
  } catch (e: any) {
    erro.value = e?.response?.data?.erros?.[0]?.mensagem || 'Erro ao carregar relatório de faturas.';
  } finally { loading.value = false; }
}
onMounted(() => { agenciaStore.loadFromStorage(); carregar(); });
</script>

<style scoped>
.qs-header-text { max-width: 720px; }
.qs-header-actions { display: flex; align-items: center; gap: 12px; flex-shrink: 0; flex-wrap: wrap; }
.qs-period-switch { display: flex; gap: 6px; }

/* ── LGPD banner ── */
.lgpd-banner {
  display: flex; align-items: center; gap: 8px;
  border-radius: var(--qs-radius-md);
  padding: 10px 16px; font-size: 13px; font-weight: 500;
  margin-bottom: 24px;
}
.lgpd-master { background: #fefce8; color: #854d0e; border: 1px solid #fde68a; }
.lgpd-default { background: var(--qs-gray-50); color: var(--qs-gray-500); border: 1px solid var(--qs-gray-200); }

/* ── Section head ── */
.section-head-row {
  display: flex; align-items: flex-start; justify-content: space-between;
  gap: 20px; flex-wrap: wrap; margin-bottom: 20px;
}
.section-head-row .qs-section-desc { margin-bottom: 0; }
.filter-inline { display: flex; align-items: center; gap: 8px; flex-wrap: wrap; }
.date-label { font-size: 11px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; white-space: nowrap; }
.date-input {
  padding: 7px 10px; border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md); font-size: 13px; outline: none; font-family: inherit;
}
.date-input:focus { border-color: var(--qs-teal); }
.search-field { position: relative; }
.search-icon { position: absolute; left: 10px; top: 50%; transform: translateY(-50%); color: var(--qs-gray-400); pointer-events: none; }
.search-input {
  padding: 7px 12px 7px 30px; border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md); font-size: 13px; outline: none;
  font-family: inherit; min-width: 170px;
}
.search-input:focus { border-color: var(--qs-teal); }

/* ── Table ── */
.qs-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: 14px; }
.qs-table thead th {
  background: var(--qs-gray-50);
  color: var(--qs-gray-500);
  font-size: 11px; font-weight: 700;
  text-transform: uppercase; letter-spacing: 0.06em;
  padding: 10px 14px; border-bottom: 1px solid var(--qs-gray-200); white-space: nowrap;
}
.qs-table tbody td { padding: 12px 14px; border-bottom: 1px solid var(--qs-gray-100); vertical-align: middle; color: var(--qs-ink); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.qs-table tbody tr:hover td { background: var(--qs-gray-50); }
.col-action { width: 60px; }
.cell-bold { font-weight: 600; }
.cell-muted { font-size: 12px; color: var(--qs-gray-400); }
.align-right { text-align: right; }
.num-teal { color: var(--qs-teal-dark); font-variant-numeric: tabular-nums; }
.masked-value { font-variant-numeric: tabular-nums; }
.lgpd-lock { font-size: 11px; opacity: 0.55; margin-left: 4px; }
.lgpd-th-tag { font-size: 10px; font-weight: 400; color: var(--qs-gray-400); margin-left: 4px; }

/* ── Badges ── */
.qs-badge {
  display: inline-block; padding: 3px 10px;
  border-radius: var(--qs-radius-pill);
  font-size: 11px; font-weight: 700; letter-spacing: 0.02em; white-space: nowrap;
}
.qs-badge-success { background: #dcfce7; color: #16a34a; }
.qs-badge-warn    { background: #fef3c7; color: #d97706; }
.qs-badge-danger  { background: #fee2e2; color: #dc2626; }

/* ── Small buttons ── */
.qs-btn-sm-outline {
  padding: 5px 12px; font-size: 12px; font-weight: 600;
  border: 1px solid var(--qs-teal); color: var(--qs-teal); background: #fff;
  border-radius: var(--qs-radius-md); cursor: pointer;
  transition: all var(--qs-duration) var(--qs-ease); white-space: nowrap;
}
.qs-btn-sm-outline:hover { background: var(--qs-teal); color: #fff; }
.qs-btn-secondary {
  padding: 10px 18px; font-size: 14px; font-weight: 500;
  border: 1px solid var(--qs-gray-200); color: var(--qs-gray-700); background: #fff;
  border-radius: var(--qs-radius-md); cursor: pointer;
  transition: all var(--qs-duration) var(--qs-ease);
}
.qs-btn-secondary:hover { background: var(--qs-gray-50); }

/* ── Empty state ── */
.qs-empty-state {
  display: flex; flex-direction: column; align-items: center;
  gap: 12px; padding: 56px 24px; text-align: center;
}
.qs-empty-state h3 { font-size: 18px; font-weight: 600; color: var(--qs-ink); margin: 0; }
.qs-empty-state p { font-size: 14px; color: var(--qs-gray-500); margin: 0; }

/* ── Modal ── */
.qs-modal-overlay {
  position: fixed; inset: 0; background: rgba(15, 28, 35, 0.5);
  backdrop-filter: blur(4px); display: flex; align-items: center; justify-content: center;
  z-index: 9000; padding: 24px;
}
.qs-modal {
  background: #fff; border-radius: var(--qs-radius-lg);
  box-shadow: var(--qs-shadow-lg); width: 100%; overflow: hidden;
}
.qs-modal-header {
  display: flex; align-items: center; justify-content: space-between;
  padding: 20px 24px; border-bottom: 1px solid var(--qs-gray-100);
}
.qs-modal-header h5 { font-size: 17px; font-weight: 700; color: var(--qs-ink); margin: 0; }
.qs-modal-close {
  width: 32px; height: 32px; border: none; background: var(--qs-gray-100);
  border-radius: var(--qs-radius-md); cursor: pointer;
  display: flex; align-items: center; justify-content: center;
  color: var(--qs-gray-500); transition: all var(--qs-duration) var(--qs-ease);
}
.qs-modal-close:hover { background: var(--qs-gray-200); color: var(--qs-ink); }
.qs-modal-body { padding: 24px; }
.qs-modal-footer {
  display: flex; align-items: center; justify-content: flex-end; gap: 10px;
  padding: 16px 24px; border-top: 1px solid var(--qs-gray-100); background: var(--qs-gray-50);
}

/* ── Info grid (modal) ── */
.info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.col-full { grid-column: 1 / -1; }
.info-item { display: flex; flex-direction: column; gap: 4px; }
.info-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.info-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.lgpd-tag { font-size: 10px; font-weight: 400; color: var(--qs-gray-400); }
.reveal-row { display: flex; align-items: center; gap: 10px; margin-top: 2px; }
.btn-reveal {
  font-size: 11px; font-weight: 700; padding: 3px 10px;
  border-radius: var(--qs-radius-md); border: 1px solid var(--qs-teal);
  color: var(--qs-teal); background: transparent; cursor: pointer;
  transition: all .15s; white-space: nowrap;
}
.btn-reveal:hover { background: var(--qs-teal); color: #fff; }
.master-note {
  display: flex; align-items: center; gap: 6px;
  margin-top: 16px; padding: 10px 14px;
  background: #fefce8; border: 1px solid #fde68a;
  border-radius: var(--qs-radius-md); font-size: 12px; color: #854d0e;
}

/* ── Alert ── */
.alert-danger {
  background: #fef2f2; color: var(--qs-danger);
  border: 1px solid #fecaca;
  border-radius: var(--qs-radius-md);
  padding: 12px 16px; font-size: 14px; margin-top: 16px;
}
</style>
