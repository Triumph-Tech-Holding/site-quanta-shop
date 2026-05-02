<template>
  <div class="qs-page no-print">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Relatórios</div>
        <h1>Relatório de Anunciantes</h1>
        <p>Parceiros de cashback ativos na plataforma</p>
      </div>
      <div class="qs-header-actions qs-filter-inline">
        <div class="qs-filter-field">
          <label class="qs-filter-label">Nome</label>
          <input v-model="filtro.nome" type="text" class="qs-input-sm" placeholder="Buscar..." />
        </div>
        <div class="qs-filter-field">
          <label class="qs-filter-label">Status</label>
          <select v-model="filtro.status" class="qs-select-sm">
            <option value="">Todos</option>
            <option value="1">Ativo</option>
            <option value="2">Inativo</option>
          </select>
        </div>
        <button class="qs-btn-primary qs-btn-sm" :disabled="loading" @click="carregar">
          <span v-if="loading" class="qs-spinner-sm" />
          Atualizar
        </button>
        <button class="qs-btn-outline qs-btn-sm no-print" :disabled="itens.length === 0" @click="gerarPdf">
          ⬇ PDF
        </button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <div class="qs-grid qs-grid-3 qs-kpi-row">
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Total Encontrados</div>
          <div class="qs-kpi-value">{{ itens.length }}</div>
        </div>
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Ativos</div>
          <div class="qs-kpi-value" style="color:var(--qs-lime)">{{ itens.filter(i => i.Ativo).length }}</div>
        </div>
        <div class="qs-kpi-card">
          <div class="qs-kpi-label">Total na Base</div>
          <div class="qs-kpi-value">{{ totalGeral }}</div>
        </div>
      </div>

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
          <button class="qs-modal-close" @click="fecharModal"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-info-grid">
            <div class="qs-info-item"><span class="qs-info-label">Cashback Mín.</span><span class="qs-info-value">{{ selecionado.CashbackMin != null ? selecionado.CashbackMin + '%' : '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Cashback Máx.</span><span class="qs-info-value">{{ selecionado.CashbackMax != null ? selecionado.CashbackMax + '%' : '—' }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Tipo</span><span class="qs-info-value">{{ selecionado.TipoCashback || '—' }}</span></div>
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
const filtro = reactive({ nome: '', status: '' });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function verDetalhes(item: any) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
function gerarPdf() { window.print(); }
async function carregar() {
  loading.value = true; erro.value = '';
  try {
    const body: any = { pagina: 1, quantidadePorPagina: 500, obterTodos: true };
    if (filtro.nome) body.nome = filtro.nome;
    if (filtro.status === '1') body.status = 1;
    if (filtro.status === '2') body.status = 2;
    const { data } = await api.post('/Admin/RelatorioAnunciantes', body, authHeader());
    const lista = data?.anunciantesFiltrados || data?.items || (Array.isArray(data) ? data : []);
    itens.value = lista;
    totalGeral.value = data?.quantidadeTotal ?? lista.length;
  } catch (e: any) {
    erro.value = e?.response?.data?.erros?.[0]?.mensagem || 'Erro ao carregar relatório de anunciantes.';
  } finally { loading.value = false; }
}
onMounted(() => { agenciaStore.loadFromStorage(); carregar(); });
</script>

<style scoped>
.qs-filter-inline { display: flex; align-items: flex-end; gap: 10px; flex-wrap: wrap; }
.qs-filter-field { display: flex; flex-direction: column; gap: 4px; }
.qs-filter-label { font-size: 11px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; }
.qs-input-sm { padding: 7px 10px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 13px; outline: none; }
.qs-input-sm:focus { border-color: var(--qs-teal); }
.qs-select-sm { padding: 7px 10px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 13px; background: #fff; outline: none; cursor: pointer; }
.qs-btn-sm { padding: 7px 14px; font-size: 13px; }
.qs-kpi-row { margin-bottom: 24px; }
.qs-grid-3 { display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; }
.qs-kpi-card { background: #fff; border: 1px solid var(--qs-gray-100); border-radius: var(--qs-radius-lg); padding: 20px 24px; box-shadow: var(--qs-shadow-sm); }
.qs-kpi-label { font-size: 11px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.06em; color: var(--qs-gray-500); margin-bottom: 8px; }
.qs-kpi-value { font-size: 24px; font-weight: 700; color: var(--qs-ink); letter-spacing: -0.02em; font-variant-numeric: tabular-nums; }
.qs-info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.qs-info-item { display: flex; flex-direction: column; gap: 4px; }
.qs-info-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-info-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-empty-hint { font-size: 13px; color: var(--qs-gray-400); margin: 0; }
.qs-spinner-sm { display: inline-block; width: 13px; height: 13px; border: 2px solid rgba(255,255,255,.4); border-top-color: #fff; border-radius: 50%; animation: spin .7s linear infinite; vertical-align: middle; margin-right: 5px; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-top: 16px; }
@keyframes spin { to { transform: rotate(360deg); } }
@media (max-width: 640px) { .qs-grid-3 { grid-template-columns: 1fr; } }
</style>
