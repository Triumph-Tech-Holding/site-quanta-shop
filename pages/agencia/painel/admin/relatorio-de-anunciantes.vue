<template>
  <div>
    <div class="ag-page-header d-flex align-items-start justify-content-between flex-wrap gap-2 no-print">
      <div>
        <h1>Relatório de Anunciantes</h1>
        <p>Parceiros de cashback ativos na plataforma</p>
      </div>
      <div class="d-flex gap-2 align-items-end flex-wrap">
        <div>
          <label class="form-label mb-1 small fw-semibold">Nome</label>
          <input v-model="filtro.nome" type="text" class="form-control form-control-sm" placeholder="Buscar..." />
        </div>
        <div>
          <label class="form-label mb-1 small fw-semibold">Status</label>
          <select v-model="filtro.status" class="form-select form-select-sm">
            <option value="">Todos</option>
            <option value="1">Ativo</option>
            <option value="2">Inativo</option>
          </select>
        </div>
        <button class="btn btn-ag-primary btn-sm" :disabled="loading" @click="carregar">
          <span v-if="loading" class="spinner-border spinner-border-sm me-1" />
          Atualizar
        </button>
        <button class="btn btn-ag-outline btn-sm" :disabled="itens.length === 0" @click="gerarPdf">
          ⬇ Gerar PDF
        </button>
      </div>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <div v-else>
      <div class="row g-3 mb-4">
        <div class="col-md-4">
          <div class="ag-stat-card flex-column p-3 text-center">
            <div class="stat-value">{{ itens.length }}</div>
            <div class="stat-label mt-1">Total Encontrados</div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="ag-stat-card flex-column p-3 text-center">
            <div class="stat-value">{{ itens.filter(i => i.Ativo).length }}</div>
            <div class="stat-label mt-1">Ativos</div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="ag-stat-card flex-column p-3 text-center">
            <div class="stat-value">{{ totalGeral }}</div>
            <div class="stat-label mt-1">Total na Base</div>
          </div>
        </div>
      </div>

      <div class="ag-card">
        <div v-if="itens.length === 0" class="ag-empty-state">
          <h5>Nenhum anunciante encontrado</h5>
          <p class="text-muted small">Ajuste os filtros e clique em Atualizar.</p>
        </div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
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
                <td class="fw-bold">{{ item.Nome }}</td>
                <td>{{ item.CashbackMin != null ? item.CashbackMin + '%' : '—' }}</td>
                <td>{{ item.CashbackMax != null ? item.CashbackMax + '%' : '—' }}</td>
                <td>{{ item.Moeda || 'BRL' }}</td>
                <td>
                  <span class="badge-ag" :class="item.Ativo ? 'badge-ag-success' : 'badge-ag-warning'">
                    {{ item.Ativo ? 'Ativo' : 'Inativo' }}
                  </span>
                </td>
                <td class="no-print">
                  <button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:480px">
        <div class="ag-modal-header">
          <h5 class="mb-0">{{ selecionado.Nome }}</h5>
          <button class="btn-close" @click="fecharModal" />
        </div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-6"><strong>Cashback Mín.:</strong> {{ selecionado.CashbackMin != null ? selecionado.CashbackMin + '%' : '—' }}</div>
            <div class="col-6"><strong>Cashback Máx.:</strong> {{ selecionado.CashbackMax != null ? selecionado.CashbackMax + '%' : '—' }}</div>
            <div class="col-6"><strong>Tipo:</strong> {{ selecionado.TipoCashback || '—' }}</div>
            <div class="col-6"><strong>Moeda:</strong> {{ selecionado.Moeda || 'BRL' }}</div>
            <div class="col-6"><strong>Status:</strong> {{ selecionado.Ativo ? 'Ativo' : 'Inativo' }}</div>
            <div class="col-6"><strong>Última Atualização:</strong> {{ selecionado.UltimaAtualizacao ? new Date(selecionado.UltimaAtualizacao).toLocaleDateString('pt-BR') : '—' }}</div>
          </div>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Fechar</button>
        </div>
      </div>
    </div>

    <div v-if="erro" class="alert alert-danger mt-3">{{ erro }}</div>

  </div>
</template>

<style>
@media print {
  .no-print, nav, aside, header, .agencia-sidebar { display: none !important; }
  body { background: white !important; }
  .ag-card { box-shadow: none !important; border: 1px solid #ddd !important; }
}
</style>

<script setup lang="ts">
import { useAgenciaStore } from '~/composables/useAgenciaStore';
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
  loading.value = true;
  erro.value = '';
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
    console.error(erro.value, e);
  } finally {
    loading.value = false;
  }
}

onMounted(() => { agenciaStore.loadFromStorage(); carregar(); });
</script>
