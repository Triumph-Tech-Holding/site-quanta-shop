<template>
  <div class="qs-page no-print">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Relatórios</div>
        <h1>Relatório de Faturas</h1>
        <p>Faturas de cashback dos credenciados</p>
      </div>
      <div class="qs-header-actions no-print">
        <button class="qs-btn-outline qs-btn-sm" :disabled="itens.length === 0" @click="gerarPdf">⬇ PDF</button>
        <button class="qs-btn-primary qs-btn-sm" :disabled="loading" @click="carregar">
          <span v-if="loading" class="qs-spinner-sm" />
          Atualizar
        </button>
      </div>
    </div>

    <div class="qs-filter-bar no-print">
      <span class="qs-filter-bar-label">Status:</span>
      <QsFilterChip label="Todos" :active="filtroStatus === null" @click="filtroStatus = null" />
      <QsFilterChip label="Pago" :active="filtroStatus === 2" @click="filtroStatus = 2" />
      <QsFilterChip label="Pendente" :active="filtroStatus === 1" @click="filtroStatus = 1" />
      <QsFilterChip label="Cancelado" :active="filtroStatus === 3" @click="filtroStatus = 3" />
      <div class="qs-filter-date-range">
        <label class="qs-filter-label">A partir de</label>
        <input v-model="filtro.dataInicial" type="date" class="qs-input-sm" />
      </div>
      <div class="qs-filter-search">
        <input v-model="filtro.loginCredenciado" type="text" class="qs-input-sm" placeholder="Filtrar por login..." @keydown.enter="carregar" />
      </div>
    </div>

    <div v-if="isMaster" class="qs-lgpd-master-banner">
      🔑 Modo Master ativo — dados sensíveis visíveis. Cada revelação é registrada na auditoria LGPD.
    </div>
    <div v-else class="qs-lgpd-info-banner">
      🔒 Dados pessoais mascarados (LGPD). Apenas usuários Master podem revelar informações individuais.
    </div>

    <div class="qs-kpi-strip">
      <QsKpiCard label="Total Pago" :value="totais.totalPago" format="currency" dotColor="#16a34a" badge="Pago" badgeTone="success" />
      <QsKpiCard label="Pendente" :value="totais.totalPendente" format="currency" dotColor="#d97706" badge="A receber" badgeTone="warn" />
      <QsKpiCard label="Total de Faturas" :value="itensFiltrados.length" format="number" dotColor="#2F7785" />
    </div>

    <template v-if="loading">
      <div class="qs-card-section">
        <div class="qs-skeleton-table">
          <div v-for="n in 8" :key="n" class="qs-skeleton-row">
            <div class="qs-skeleton qs-sk-xs" />
            <div class="qs-skeleton qs-sk-md" />
            <div class="qs-skeleton qs-sk-md" />
            <div class="qs-skeleton qs-sk-sm" />
            <div class="qs-skeleton qs-sk-sm" />
            <div class="qs-skeleton qs-sk-xs" />
          </div>
        </div>
      </div>
    </template>

    <template v-else>
      <div class="qs-card-section">
        <div v-if="itensFiltrados.length === 0" class="qs-empty-state">
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><rect x="2" y="5" width="20" height="14" rx="2"/><line x1="2" y1="10" x2="22" y2="10"/></svg>
          <h3>Nenhuma fatura encontrada</h3>
          <p class="qs-empty-hint">Ajuste os filtros e clique em Atualizar.</p>
        </div>
        <div v-else class="qs-table-wrap">
          <table class="qs-table">
            <thead>
              <tr>
                <th>#</th>
                <th>
                  Login / Credenciado
                  <span class="qs-th-lgpd-tag">🔒 LGPD</span>
                </th>
                <th>Patrocinador</th>
                <th>Cidade</th>
                <th>Cashback</th>
                <th class="qs-text-right">Valor</th>
                <th>Data Pedido</th>
                <th>Status</th>
                <th class="no-print"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in itensFiltrados" :key="item.IdPedido">
                <td class="qs-text-muted-sm">#{{ item.IdPedido }}</td>
                <td>
                  <span class="qs-cell-bold qs-masked-value">{{ maskEmail(item.Login) }}</span>
                  <span v-if="!isMaster" class="qs-lgpd-lock-icon" title="Dado protegido por LGPD — apenas Master revela">🔒</span>
                </td>
                <td>{{ item.Patrocinador ? maskName(item.Patrocinador) : '—' }}</td>
                <td>{{ item.Cidade || '—' }}</td>
                <td>{{ item.Cashback }}</td>
                <td class="qs-text-right qs-cell-bold qs-num-teal">{{ formatCurrency(item.ValorPedido) }}</td>
                <td>{{ formatDate(item.DataPedido) }}</td>
                <td><span class="qs-badge" :class="badgeStatus(item.Status)">{{ labelStatus(item.Status) }}</span></td>
                <td class="no-print"><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
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
          <div class="qs-info-grid">
            <div class="qs-info-item qs-col-full">
              <span class="qs-info-label">Login / Credenciado <span class="qs-lgpd-tag">🔒 LGPD</span></span>
              <div class="qs-lgpd-reveal-row">
                <span class="qs-info-value">{{ revelado.login ? selecionado.Login : maskEmail(selecionado.Login) }}</span>
                <button v-if="isMaster && !revelado.login" class="qs-btn-reveal" @click="revelar('login')">Revelar</button>
                <span v-if="!isMaster" class="qs-lgpd-lock-inline" title="Apenas Master pode revelar">🔒</span>
              </div>
            </div>
            <div class="qs-info-item qs-col-full">
              <span class="qs-info-label">Nome <span class="qs-lgpd-tag">🔒 LGPD</span></span>
              <div class="qs-lgpd-reveal-row">
                <span class="qs-info-value">{{ revelado.nome ? selecionado.Nome : maskName(selecionado.Nome) }}</span>
                <button v-if="isMaster && !revelado.nome" class="qs-btn-reveal" @click="revelar('nome')">Revelar</button>
                <span v-if="!isMaster" class="qs-lgpd-lock-inline" title="Apenas Master pode revelar">🔒</span>
              </div>
            </div>
            <div class="qs-info-item"><span class="qs-info-label">Patrocinador</span><span class="qs-info-value">{{ selecionado.Patrocinador ? maskName(selecionado.Patrocinador) : '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Cidade</span><span class="qs-info-value">{{ selecionado.Cidade || '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Categoria</span><span class="qs-info-value">{{ selecionado.Categoria || '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Cashback</span><span class="qs-info-value">{{ selecionado.Cashback }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Valor</span><span class="qs-info-value qs-num-teal">{{ formatCurrency(selecionado.ValorPedido) }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Status</span><span class="qs-badge" :class="badgeStatus(selecionado.Status)">{{ labelStatus(selecionado.Status) }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Data Pedido</span><span class="qs-info-value">{{ formatDate(selecionado.DataPedido) }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Data Pagamento</span><span class="qs-info-value">{{ selecionado.DataPagamento ? formatDate(selecionado.DataPagamento) : '—' }}</span></div>
          </div>
          <div v-if="isMaster" class="qs-lgpd-master-note">
            🔑 Você é Master. Cada revelação é registrada na auditoria LGPD.
          </div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Fechar</button>
        </div>
      </div>
    </div>

    <div v-if="erro" class="qs-alert-danger">{{ erro }}</div>
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

function revelar(campo: 'login' | 'nome') {
  if (!isMaster.value) return;
  revelado[campo] = true;
}

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
.qs-kpi-strip { display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; margin-bottom: 20px; }
@media (max-width: 640px) { .qs-kpi-strip { grid-template-columns: 1fr; } }

.qs-filter-bar { display: flex; align-items: center; gap: 8px; margin-bottom: 16px; flex-wrap: wrap; }
.qs-filter-bar-label { font-size: 12px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.05em; margin-right: 4px; }
.qs-filter-date-range, .qs-filter-search { display: flex; align-items: center; gap: 6px; margin-left: 8px; }
.qs-filter-label { font-size: 11px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; }
.qs-input-sm { padding: 6px 10px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 13px; outline: none; }
.qs-input-sm:focus { border-color: var(--qs-teal); }
.qs-btn-sm { padding: 7px 14px; font-size: 13px; }

.qs-lgpd-master-banner {
  background: #fefce8; color: #854d0e; border: 1px solid #fde68a;
  border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 13px;
  font-weight: 500; margin-bottom: 16px;
}
.qs-lgpd-info-banner {
  background: var(--qs-gray-50); color: var(--qs-gray-600); border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 13px; margin-bottom: 16px;
}

.qs-skeleton-table { display: flex; flex-direction: column; gap: 12px; padding: 8px 0; }
.qs-skeleton-row { display: flex; gap: 16px; align-items: center; }
.qs-sk-wide { flex: 2; height: 18px; border-radius: 6px; }
.qs-sk-md { flex: 1.5; height: 18px; border-radius: 6px; }
.qs-sk-sm { flex: 1; height: 18px; border-radius: 6px; }
.qs-sk-xs { flex: 0.5; height: 18px; border-radius: 6px; }

.qs-th-lgpd-tag { font-size: 10px; font-weight: 400; color: var(--qs-gray-400); margin-left: 4px; }
.qs-masked-value { font-variant-numeric: tabular-nums; }
.qs-lgpd-lock-icon { margin-left: 4px; font-size: 11px; opacity: 0.5; }

.qs-info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.qs-col-full { grid-column: 1 / -1; }
.qs-info-item { display: flex; flex-direction: column; gap: 4px; }
.qs-info-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-info-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-lgpd-tag { font-size: 10px; font-weight: 400; color: var(--qs-gray-400); }

.qs-lgpd-reveal-row { display: flex; align-items: center; gap: 10px; }
.qs-btn-reveal {
  font-size: 11px; font-weight: 600; padding: 3px 10px;
  border-radius: var(--qs-radius-md); border: 1px solid var(--qs-teal);
  color: var(--qs-teal); background: transparent; cursor: pointer;
  transition: all .15s;
}
.qs-btn-reveal:hover { background: var(--qs-teal); color: #fff; }
.qs-lgpd-lock-inline { font-size: 12px; opacity: 0.5; }
.qs-lgpd-master-note {
  margin-top: 16px; padding: 10px 14px; background: #fefce8;
  border-radius: var(--qs-radius-md); font-size: 12px; color: #854d0e;
  border: 1px solid #fde68a;
}

.qs-text-right { text-align: right; }
.qs-text-muted-sm { font-size: 12px; color: var(--qs-gray-400); }
.qs-num-teal { color: var(--qs-teal-dark); font-variant-numeric: tabular-nums; }
.qs-empty-hint { font-size: 13px; color: var(--qs-gray-400); margin: 0; }
.qs-spinner-sm { display: inline-block; width: 13px; height: 13px; border: 2px solid rgba(255,255,255,.4); border-top-color: #fff; border-radius: 50%; animation: spin .7s linear infinite; vertical-align: middle; margin-right: 5px; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-top: 16px; }
@keyframes spin { to { transform: rotate(360deg); } }
</style>
