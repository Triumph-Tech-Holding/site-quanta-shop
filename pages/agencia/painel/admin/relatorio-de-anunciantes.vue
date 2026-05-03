<template>
  <div class="qs-page no-print">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Relatórios</div>
        <h1>Relatório de Anunciantes</h1>
        <p>Parceiros de cashback ativos na plataforma</p>
      </div>
      <div class="qs-header-actions no-print">
        <div class="qs-period-switch">
          <QsFilterChip :active="filtroStatus === ''" @click="filtroStatus = ''">Todos</QsFilterChip>
          <QsFilterChip :active="filtroStatus === '1'" @click="filtroStatus = '1'">Ativo</QsFilterChip>
          <QsFilterChip :active="filtroStatus === '2'" @click="filtroStatus = '2'">Inativo</QsFilterChip>
        </div>
        <div class="search-field">
          <svg class="search-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
          <input v-model="filtro.nome" type="text" class="search-input" placeholder="Buscar por nome..." @keydown.enter="carregar" />
        </div>
        <button class="qs-btn-outline" :disabled="itens.length === 0" @click="gerarPdf">⬇ PDF</button>
        <button class="qs-btn-primary" :disabled="loading" @click="carregar">Atualizar</button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <div class="qs-grid">
        <QsKpiCard label="Encontrados" :value="itens.length" format="number" />
        <QsKpiCard label="Ativos" :value="itens.filter(i => i.Ativo).length" format="number" dot-color="var(--qs-success)" />
        <QsKpiCard label="Inativos" :value="itens.filter(i => !i.Ativo).length" format="number" dot-color="var(--qs-warn)" />
        <QsKpiCard label="Total na Base" :value="totalGeral" format="number" dot-color="var(--qs-teal)" />
      </div>

      <section class="qs-card-section">
        <h2 class="qs-section-title">Anunciantes / Parceiros</h2>
        <p class="qs-section-desc">{{ itens.length }} parceiro{{ itens.length !== 1 ? 's' : '' }} encontrado{{ itens.length !== 1 ? 's' : '' }}{{ filtroStatus === '1' ? ' — apenas ativos' : filtroStatus === '2' ? ' — apenas inativos' : '' }}.</p>

        <div v-if="itens.length === 0" class="qs-empty-state">
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" color="var(--qs-gray-400)"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/></svg>
          <h3>Nenhum anunciante encontrado</h3>
          <p>Ajuste os filtros e clique em Atualizar.</p>
        </div>
        <div v-else class="qs-table-wrap">
          <table class="qs-table">
            <thead>
              <tr>
                <th>Anunciante / Parceiro</th>
                <th>Cashback Mín.</th>
                <th>Cashback Máx.</th>
                <th>Moeda</th>
                <th>Status</th>
                <th class="col-action no-print"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in itens" :key="item.Id || item.id || item.Nome">
                <td class="cell-bold">{{ item.Nome }}</td>
                <td>{{ item.CashbackMin != null ? item.CashbackMin + '%' : '—' }}</td>
                <td>{{ item.CashbackMax != null ? item.CashbackMax + '%' : '—' }}</td>
                <td>{{ item.Moeda || 'BRL' }}</td>
                <td><span class="qs-badge" :class="item.Ativo ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.Ativo ? 'Ativo' : 'Inativo' }}</span></td>
                <td class="no-print"><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
              </tr>
            </tbody>
          </table>
        </div>
      </section>
    </template>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:480px">
        <div class="qs-modal-header">
          <h5>{{ selecionado.Nome }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="info-grid">
            <div class="info-item"><span class="info-label">Cashback Mín.</span><span class="info-value">{{ selecionado.CashbackMin != null ? selecionado.CashbackMin + '%' : '—' }}</span></div>
            <div class="info-item"><span class="info-label">Cashback Máx.</span><span class="info-value">{{ selecionado.CashbackMax != null ? selecionado.CashbackMax + '%' : '—' }}</span></div>
            <div class="info-item"><span class="info-label">Tipo Cashback</span><span class="info-value">{{ selecionado.TipoCashback || '—' }}</span></div>
            <div class="info-item"><span class="info-label">Moeda</span><span class="info-value">{{ selecionado.Moeda || 'BRL' }}</span></div>
            <div class="info-item">
              <span class="info-label">Status</span>
              <span class="qs-badge" :class="selecionado.Ativo ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.Ativo ? 'Ativo' : 'Inativo' }}</span>
            </div>
            <div class="info-item"><span class="info-label">Última Atualiz.</span><span class="info-value">{{ selecionado.UltimaAtualizacao ? new Date(selecionado.UltimaAtualizacao).toLocaleDateString('pt-BR') : '—' }}</span></div>
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
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(false);
const erro = ref('');
const itens = ref<any[]>([]);
const totalGeral = ref(0);
const showModal = ref(false);
const selecionado = ref<any>(null);
const filtro = reactive({ nome: '' });
const filtroStatus = ref('');

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function verDetalhes(item: any) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
function gerarPdf() { window.print(); }

async function carregar() {
  loading.value = true; erro.value = '';
  try {
    const body: any = { pagina: 1, quantidadePorPagina: 500, obterTodos: true };
    if (filtro.nome) body.nome = filtro.nome;
    if (filtroStatus.value === '1') body.status = 1;
    if (filtroStatus.value === '2') body.status = 2;
    const { data } = await api.post('/Admin/RelatorioAnunciantes', body, authHeader());
    const lista = data?.anunciantesFiltrados || data?.items || (Array.isArray(data) ? data : []);
    itens.value = lista;
    totalGeral.value = data?.quantidadeTotal ?? lista.length;
  } catch (e: any) {
    erro.value = e?.response?.data?.erros?.[0]?.mensagem || 'Erro ao carregar relatório de anunciantes.';
  } finally { loading.value = false; }
}

watch(filtroStatus, () => carregar());
onMounted(() => { agenciaStore.loadFromStorage(); carregar(); });
</script>

<style scoped>
.qs-header-text { max-width: 720px; }
.qs-header-actions { display: flex; align-items: center; gap: 12px; flex-shrink: 0; flex-wrap: wrap; }
.qs-period-switch { display: flex; gap: 6px; }

/* ── Search field ── */
.search-field { position: relative; }
.search-icon { position: absolute; left: 10px; top: 50%; transform: translateY(-50%); color: var(--qs-gray-400); pointer-events: none; }
.search-input {
  padding: 8px 12px 8px 30px;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  font-size: 13px; outline: none;
  font-family: inherit; min-width: 180px;
}
.search-input:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.1); }

/* ── Table ── */
.qs-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: 14px; }
.qs-table thead th {
  background: var(--qs-gray-50);
  color: var(--qs-gray-500);
  font-size: 11px; font-weight: 700;
  text-transform: uppercase; letter-spacing: 0.06em;
  padding: 10px 14px;
  border-bottom: 1px solid var(--qs-gray-200);
  white-space: nowrap;
}
.qs-table tbody td { padding: 12px 14px; border-bottom: 1px solid var(--qs-gray-100); vertical-align: middle; color: var(--qs-ink); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.qs-table tbody tr:hover td { background: var(--qs-gray-50); }
.col-action { width: 60px; }
.cell-bold { font-weight: 600; }

/* ── Badges ── */
.qs-badge {
  display: inline-block;
  padding: 3px 10px;
  border-radius: var(--qs-radius-pill);
  font-size: 11px; font-weight: 700;
  letter-spacing: 0.02em; white-space: nowrap;
}
.qs-badge-success { background: #dcfce7; color: #16a34a; }
.qs-badge-warn    { background: #fef3c7; color: #d97706; }
.qs-badge-danger  { background: #fee2e2; color: #dc2626; }

/* ── Small button ── */
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
  position: fixed; inset: 0;
  background: rgba(15, 28, 35, 0.5);
  backdrop-filter: blur(4px);
  display: flex; align-items: center; justify-content: center;
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
  padding: 16px 24px; border-top: 1px solid var(--qs-gray-100);
  background: var(--qs-gray-50);
}

/* ── Info grid ── */
.info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.info-item { display: flex; flex-direction: column; gap: 4px; }
.info-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.info-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }

/* ── Alert ── */
.alert-danger {
  background: #fef2f2; color: var(--qs-danger);
  border: 1px solid #fecaca;
  border-radius: var(--qs-radius-md);
  padding: 12px 16px; font-size: 14px; margin-top: 16px;
}
</style>
