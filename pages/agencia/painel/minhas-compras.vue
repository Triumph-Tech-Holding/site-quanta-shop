<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="CCR" title="Minhas Compras" description="Histórico de compras e distribuição de cashback registrados no seu perfil." />

        <!-- Filter bar -->
        <div class="qs-filter-bar">
          <form @submit.prevent="buscarPedidos" class="mc-filter-form">
            <div class="mc-filter-group">
              <label class="qs-label">Descrição</label>
              <input type="text" v-model="filtro.descricao" class="qs-input" placeholder="Buscar por descrição" />
            </div>
            <div class="mc-filter-group">
              <label class="qs-label">Data inicial</label>
              <input type="date" v-model="filtro.dataInicio" class="qs-input" />
            </div>
            <div class="mc-filter-group">
              <label class="qs-label">Data final</label>
              <input type="date" v-model="filtro.dataFim" class="qs-input" />
            </div>
            <button type="submit" class="qs-btn-primary mc-btn-filtrar">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0 0 16 9.5 6.5 6.5 0 1 0 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
              Filtrar
            </button>
          </form>
        </div>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div class="qs-card-section mc-results-card">
            <div class="mc-results-header">
              <div class="qs-section-title">Minhas Compras</div>
              <span class="mc-count-badge">{{ items.length }} registro(s)</span>
            </div>

            <div v-if="items.length === 0" class="ag-empty-state">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="var(--qs-gray-200,#e5e7eb)"><path d="M19 7h-1V6c0-2.76-2.24-5-5-5S8 3.24 8 6v1H7c-1.1 0-2 .9-2 2v11c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V9c0-1.1-.9-2-2-2zm-7 9c-1.1 0-2-.9-2-2s.9-2 2-2 2 .9 2 2-.9 2-2 2zM10 7V6c0-1.1.9-2 2-2s2 .9 2 2v1h-4z"/></svg>
              <h5>Nenhuma compra encontrada</h5>
              <p>Tente ajustar os filtros de busca</p>
            </div>

            <div v-else class="mc-table-wrap">
              <table class="qs-table">
                <thead>
                  <tr>
                    <th>Descrição</th>
                    <th class="tr">Valor da compra</th>
                    <th class="tr">Cashback a receber</th>
                    <th class="tc">Data</th>
                    <th class="tc">Status</th>
                    <th class="tc">Detalhes</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, i) in items" :key="i">
                    <td>{{ item.produto || '—' }}</td>
                    <td class="tr">{{ formatCurrency(Number(item.valor)) }}</td>
                    <td class="tr td-lime">{{ item.cashbackReceber }}</td>
                    <td class="tc">{{ formatDate(String(item.data)) }}</td>
                    <td class="tc">
                      <span class="qs-badge" :class="badgeClass(statusClass(String(item.status)))">{{ item.status }}</span>
                    </td>
                    <td class="tc">
                      <button
                        v-if="item.transacao || (item.detalhes && (item.detalhes as unknown[]).length)"
                        class="mc-link-btn"
                        @click="abrirDetalhes(item)"
                      >Detalhes</button>
                      <span v-else class="mc-dash">—</span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </template>
      </div>
    </div>
  </div>

  <!-- Modal Detalhes -->
  <teleport to="body">
    <div v-if="showModal" class="qs-modal-overlay" @click.self="showModal = false">
      <div class="qs-modal">
        <div class="qs-modal-header">
          <h2>Movimentação detalhada da compra</h2>
          <button @click="showModal = false" class="qs-modal-close" title="Fechar">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <template v-if="detalhesSelecionado.length">
            <div class="qs-section-title" style="margin-bottom:.75rem;">Movimentação</div>
            <div class="mc-table-wrap" style="margin-bottom:1.5rem;">
              <table class="qs-table">
                <thead><tr><th>Descrição</th><th class="tc">Data</th><th class="tc">Status</th></tr></thead>
                <tbody>
                  <tr v-for="(d, i) in detalhesSelecionado" :key="i">
                    <td>{{ (d as Record<string,unknown>).descricao || '—' }}</td>
                    <td class="tc">{{ formatDate(String((d as Record<string,unknown>).dataAtualizacao || '')) }}</td>
                    <td class="tc">
                      <span class="qs-badge" :class="badgeClass(statusClass(String(((d as Record<string,unknown>).status as Record<string,unknown>)?.nome || '')))">
                        {{ ((d as Record<string,unknown>).status as Record<string,unknown>)?.nome || '—' }}
                      </span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </template>

          <template v-if="listaDistribuicao.length">
            <div class="qs-section-title" style="margin-bottom:.75rem;">Distribuição do cashback</div>
            <div class="mc-table-wrap">
              <table class="qs-table">
                <thead><tr><th>Descrição</th><th class="tc">Data</th><th class="tc">Status</th></tr></thead>
                <tbody>
                  <tr v-for="(d, i) in listaDistribuicao" :key="i">
                    <td>{{ (d as Record<string,unknown>).descricao || '—' }}</td>
                    <td class="tc">{{ formatDate(String((d as Record<string,unknown>).dataAtualizacao || '')) }}</td>
                    <td class="tc">
                      <span class="qs-badge" :class="badgeClass(statusClass(String(((d as Record<string,unknown>).status as Record<string,unknown>)?.nome || '')))">
                        {{ ((d as Record<string,unknown>).status as Record<string,unknown>)?.nome || '—' }}
                      </span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </template>

          <p v-if="!detalhesSelecionado.length && !listaDistribuicao.length" style="text-align:center;color:var(--qs-gray-400);">
            Nenhum detalhe disponível.
          </p>
        </div>
      </div>
    </div>
  </teleport>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(false);
const items = ref<Record<string, unknown>[]>([]);
const showModal = ref(false);
const detalhesSelecionado = ref<unknown[]>([]);
const listaDistribuicao = ref<unknown[]>([]);

function abrirDetalhes(item: Record<string, unknown>) {
  const pd = (item.pedidoDetalhe as unknown[] | undefined) ?? (item.detalhes as unknown[] | undefined) ?? [];
  const tx = item.transacao as Record<string, unknown> | undefined;
  detalhesSelecionado.value = Array.isArray(pd) ? pd : [];
  listaDistribuicao.value = tx?.distribuicao ? (tx.distribuicao as unknown[]) : [];
  showModal.value = true;
}

const now = new Date();
const firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
const lastDay = new Date(now.getFullYear(), now.getMonth() + 1, 0);

const filtro = reactive({
  descricao: '',
  dataInicio: firstDay.toISOString().split('T')[0],
  dataFim: lastDay.toISOString().split('T')[0],
  idStatus: 0,
});

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

function formatDate(d: string): string {
  if (!d) return '—';
  return new Date(d).toLocaleDateString('pt-BR');
}

function statusClass(s: string): string {
  const lower = (s || '').toLowerCase();
  if (lower.includes('aprov') || lower.includes('pago')) return 'aprovado';
  if (lower.includes('recus') || lower.includes('cancel')) return 'recusado';
  return 'pendente';
}

function badgeClass(cls: string): string {
  if (cls === 'aprovado') return 'qs-badge-success';
  if (cls === 'recusado') return 'qs-badge-danger';
  return 'qs-badge-warn';
}

async function buscarPedidos() {
  loading.value = true;
  items.value = [];
  try {
    const body = {
      descricao: filtro.descricao || null,
      dataInicio: filtro.dataInicio ? new Date(filtro.dataInicio).toISOString() : null,
      dataFim: filtro.dataFim ? new Date(filtro.dataFim + 'T23:59:59').toISOString() : null,
      idStatus: filtro.idStatus || 0,
    };
    let rawData: unknown;
    try {
      const r = await api.post('/MinhasCompras/buscarPedidos', body, authHeader());
      rawData = r.data;
    } catch {
      const r = await api.post('/Pedidos/listaPedidosAfiliados', body, authHeader());
      rawData = r.data;
    }
    const list: Record<string, unknown>[] = Array.isArray(rawData) ? rawData : [];
    items.value = list.map((item) => {
      let descricao = '';
      const pd = (item.pedidoDetalhe as Record<string, unknown>[] | undefined) ?? [];
      const tx = item.transacao as Record<string, unknown> | undefined;
      if (item.idUsuarioComerciante && tx) {
        descricao = String(tx.descricao ?? '');
      } else if (pd[0]) {
        descricao = String((pd[0] as Record<string, unknown>).descricao ?? '');
      }
      let status = 'Pendente';
      if (item.idUsuarioComerciante && tx) {
        const sv = tx.statusViewModel as Record<string, unknown> | undefined;
        status = sv?.idStatus === 7 ? 'Pendente' : String(sv?.nome ?? 'Pendente');
      } else if (pd.length > 0) {
        const lastPd = pd[pd.length - 1] as Record<string, unknown>;
        status = String((lastPd.status as Record<string, unknown>)?.nome ?? 'Pendente');
      }
      const cb = item.cashbackReceber as number;
      const cashbackReceber = cb ? formatCurrency(cb) : '----';
      return { ...item, produto: descricao, data: item.dataPedido, valor: item.valorPedido, status, cashbackReceber };
    });
  } catch {
    items.value = [];
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  agenciaStore.loadFromStorage();
  buscarPedidos();
});
</script>

<style scoped>
.mc-filter-form {
  display: flex;
  flex-wrap: wrap;
  gap: .875rem;
  align-items: flex-end;
}
.mc-filter-group {
  display: flex;
  flex-direction: column;
  gap: .35rem;
  flex: 1;
  min-width: 160px;
}
.mc-btn-filtrar {
  flex-shrink: 0;
  display: inline-flex;
  align-items: center;
  gap: .35rem;
}
.mc-btn-filtrar svg { width: 16px; height: 16px; }

.mc-results-card { background: #fff; }
.mc-results-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1rem;
  flex-wrap: wrap;
  gap: .5rem;
}
.mc-count-badge {
  font-size: .75rem;
  font-weight: 600;
  background: var(--qs-gray-100, #f5f5f7);
  color: var(--qs-gray-500, #6b7280);
  padding: .2rem .625rem;
  border-radius: var(--qs-radius-pill, 999px);
}

.mc-table-wrap { overflow-x: auto; }
.mc-link-btn {
  background: none;
  border: none;
  font-size: .8125rem;
  font-weight: 600;
  color: var(--qs-teal, #2F7785);
  cursor: pointer;
  padding: 0;
  text-decoration: underline;
}
.mc-link-btn:hover { color: var(--qs-teal-dark, #225F6B); }
.mc-dash { color: var(--qs-gray-400, #9ca3af); font-size: .8125rem; }

/* Shared table */
.qs-table { width: 100%; border-collapse: collapse; font-size: .875rem; }
.qs-table thead tr { border-bottom: 2px solid var(--qs-gray-100, #f5f5f7); }
.qs-table th {
  padding: .625rem .875rem;
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .06em;
  color: var(--qs-gray-500, #6b7280);
  white-space: nowrap;
}
.qs-table td {
  padding: .75rem .875rem;
  color: var(--qs-gray-700, #374151);
  border-bottom: 1px solid var(--qs-gray-100, #f5f5f7);
}
.qs-table tbody tr:hover td { background: var(--qs-gray-50, #fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }
.tr { text-align: right !important; }
.td-lime { color: var(--qs-lime-dark, #7aad1f) !important; font-weight: 600; }

/* Badges */
.qs-badge {
  display: inline-flex;
  padding: .2rem .55rem;
  border-radius: var(--qs-radius-pill, 999px);
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .04em;
  white-space: nowrap;
}
/* Modal */
.qs-modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,.55);
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 1rem;
}
.qs-modal {
  background: #fff;
  border-radius: var(--qs-radius-lg, 20px);
  width: 100%;
  max-width: 780px;
  max-height: 90vh;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  box-shadow: var(--qs-shadow-lg, 0 8px 40px rgba(1,15,28,.16));
}
.qs-modal-header {
  background: var(--qs-gradient-btn, linear-gradient(135deg, #225F6B, #2F7785));
  color: #fff;
  padding: 1rem 1.25rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.qs-modal-header h2 { font-size: 1rem; font-weight: 700; margin: 0; }
.qs-modal-close {
  background: none;
  border: none;
  color: #fff;
  cursor: pointer;
  padding: 0;
  display: flex;
  align-items: center;
}
.qs-modal-close svg { width: 20px; height: 20px; }
.qs-modal-body { padding: 1.5rem; overflow-y: auto; }

/* Shared inputs */
.qs-label {
  font-size: .75rem;
  font-weight: 600;
  color: var(--qs-gray-700, #374151);
  text-transform: uppercase;
  letter-spacing: .04em;
}
.qs-input {
  width: 100%;
  padding: .625rem .875rem;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  font-size: .875rem;
  color: var(--qs-ink, #1d1d1f);
  background: #fff;
  transition: border-color .15s;
}
.qs-input:focus { outline: none; border-color: var(--qs-teal, #2F7785); }
</style>
