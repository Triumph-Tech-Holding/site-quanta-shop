<template>
  <div class="qs-page no-print">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Relatórios</div>
        <h1>Relatório de Anunciantes</h1>
        <p>Parceiros de cashback ativos na plataforma</p>
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
      <QsFilterChip label="Todos" :active="filtroStatus === ''" @click="filtroStatus = ''" />
      <QsFilterChip label="Ativo" :active="filtroStatus === '1'" @click="filtroStatus = '1'" />
      <QsFilterChip label="Inativo" :active="filtroStatus === '2'" @click="filtroStatus = '2'" />
      <div class="qs-filter-search">
        <input v-model="filtro.nome" type="text" class="qs-input-sm" placeholder="Buscar por nome..." @keydown.enter="carregar" />
      </div>
    </div>

    <div class="qs-kpi-strip">
      <QsKpiCard label="Encontrados" :value="itens.length" format="number" />
      <QsKpiCard label="Ativos" :value="itens.filter(i => i.Ativo).length" format="number" dotColor="#16a34a" />
      <QsKpiCard label="Inativos" :value="itens.filter(i => !i.Ativo).length" format="number" dotColor="#d97706" />
      <QsKpiCard label="Total na Base" :value="totalGeral" format="number" dotColor="#2F7785" />
    </div>

    <template v-if="loading">
      <div class="qs-card-section">
        <div class="qs-skeleton-table">
          <div v-for="n in 8" :key="n" class="qs-skeleton-row">
            <div class="qs-skeleton qs-sk-wide" />
            <div class="qs-skeleton qs-sk-sm" />
            <div class="qs-skeleton qs-sk-sm" />
            <div class="qs-skeleton qs-sk-sm" />
            <div class="qs-skeleton qs-sk-xs" />
          </div>
        </div>
      </div>
    </template>

    <template v-else>
      <div class="qs-card-section">
        <div v-if="itens.length === 0" class="qs-empty-state">
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/></svg>
          <h3>Nenhum anunciante encontrado</h3>
          <p class="qs-empty-hint">Ajuste os filtros e clique em Atualizar.</p>
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
                <th class="no-print"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in itens" :key="item.Id || item.id || item.Nome">
                <td class="qs-cell-bold">{{ item.Nome }}</td>
                <td>{{ item.CashbackMin != null ? item.CashbackMin + '%' : '—' }}</td>
                <td>{{ item.CashbackMax != null ? item.CashbackMax + '%' : '—' }}</td>
                <td>{{ item.Moeda || 'BRL' }}</td>
                <td><span class="qs-badge" :class="item.Ativo ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.Ativo ? 'Ativo' : 'Inativo' }}</span></td>
                <td class="no-print"><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
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
          <div class="qs-info-grid">
            <div class="qs-info-item"><span class="qs-info-label">Cashback Mín.</span><span class="qs-info-value">{{ selecionado.CashbackMin != null ? selecionado.CashbackMin + '%' : '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Cashback Máx.</span><span class="qs-info-value">{{ selecionado.CashbackMax != null ? selecionado.CashbackMax + '%' : '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Tipo Cashback</span><span class="qs-info-value">{{ selecionado.TipoCashback || '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Moeda</span><span class="qs-info-value">{{ selecionado.Moeda || 'BRL' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Status</span><span class="qs-badge" :class="selecionado.Ativo ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.Ativo ? 'Ativo' : 'Inativo' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Última Atualiz.</span><span class="qs-info-value">{{ selecionado.UltimaAtualizacao ? new Date(selecionado.UltimaAtualizacao).toLocaleDateString('pt-BR') : '—' }}</span></div>
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
.qs-kpi-strip { display: grid; grid-template-columns: repeat(4, 1fr); gap: 16px; margin-bottom: 20px; }
@media (max-width: 768px) { .qs-kpi-strip { grid-template-columns: 1fr 1fr; } }

.qs-filter-bar { display: flex; align-items: center; gap: 8px; margin-bottom: 20px; flex-wrap: wrap; }
.qs-filter-bar-label { font-size: 12px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.05em; margin-right: 4px; }
.qs-filter-search { margin-left: 16px; }
.qs-input-sm { padding: 6px 10px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 13px; outline: none; min-width: 180px; }
.qs-input-sm:focus { border-color: var(--qs-teal); }
.qs-btn-sm { padding: 7px 14px; font-size: 13px; }

.qs-skeleton-table { display: flex; flex-direction: column; gap: 12px; padding: 8px 0; }
.qs-skeleton-row { display: flex; gap: 16px; align-items: center; }
.qs-sk-wide { flex: 2; height: 18px; border-radius: 6px; }
.qs-sk-sm { flex: 1; height: 18px; border-radius: 6px; }
.qs-sk-xs { flex: 0.6; height: 22px; border-radius: 999px; }

.qs-info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.qs-info-item { display: flex; flex-direction: column; gap: 4px; }
.qs-info-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-info-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-empty-hint { font-size: 13px; color: var(--qs-gray-400); margin: 0; }
.qs-spinner-sm { display: inline-block; width: 13px; height: 13px; border: 2px solid rgba(255,255,255,.4); border-top-color: #fff; border-radius: 50%; animation: spin .7s linear infinite; vertical-align: middle; margin-right: 5px; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-top: 16px; }
@keyframes spin { to { transform: rotate(360deg); } }
</style>
