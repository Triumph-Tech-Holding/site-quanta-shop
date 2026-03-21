<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page">Minhas Compras</h2>
        </div>

        <div class="px-3 pb-3">
          <div class="box-filter">
            <h2>Filtros</h2>
            <form @submit.prevent="buscarPedidos">
              <div style="display:flex;flex-wrap:wrap;gap:1rem;margin-bottom:1rem;">
                <div style="flex:1;min-width:180px;">
                  <label>Descrição</label>
                  <input type="text" v-model="filtro.descricao" class="form-control" placeholder="Descrição" />
                </div>
                <div style="flex:1;min-width:150px;">
                  <label>Data inicial</label>
                  <input type="date" v-model="filtro.dataInicio" class="form-control" />
                </div>
                <div style="flex:1;min-width:150px;">
                  <label>Data final</label>
                  <input type="date" v-model="filtro.dataFim" class="form-control" />
                </div>
                <div style="display:flex;align-items:flex-end;">
                  <button type="submit" class="btn-filtrar">Filtrar</button>
                </div>
              </div>
            </form>
          </div>

          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

          <template v-else>
            <div style="background:#fff;border-radius:8px;box-shadow:0 2px 8px rgba(0,0,0,.06);overflow:hidden;">
              <div style="padding:1rem;display:flex;justify-content:space-between;align-items:center;border-bottom:1px solid #eee;">
                <span style="font-weight:700;color:#225f6b;">Minhas Compras</span>
                <span style="font-size:.8rem;color:#6c757d;">{{ items.length }} registro(s)</span>
              </div>

              <div v-if="items.length === 0" class="ag-empty-state">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M19 7h-1V6c0-2.76-2.24-5-5-5S8 3.24 8 6v1H7c-1.1 0-2 .9-2 2v11c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V9c0-1.1-.9-2-2-2zm-7 9c-1.1 0-2-.9-2-2s.9-2 2-2 2 .9 2 2-.9 2-2 2zM10 7V6c0-1.1.9-2 2-2s2 .9 2 2v1h-4z"/></svg>
                <h5>Nenhuma compra encontrada</h5>
                <p>Tente ajustar os filtros de busca</p>
              </div>

              <div v-else style="overflow-x:auto;">
                <table class="table-custom" style="width:100%;">
                  <thead>
                    <tr>
                      <th>Descrição</th>
                      <th style="text-align:center;">Valor da compra</th>
                      <th style="text-align:center;">Cashback a receber</th>
                      <th style="text-align:center;">Data da compra</th>
                      <th style="text-align:center;">Status</th>
                      <th style="text-align:center;">Detalhes</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(item, i) in items" :key="i">
                      <td>{{ item.produto || '—' }}</td>
                      <td style="text-align:right;">{{ formatCurrency(Number(item.valor)) }}</td>
                      <td style="text-align:right;color:#98c73a;font-weight:600;">{{ item.cashbackReceber }}</td>
                      <td style="text-align:center;">{{ formatDate(String(item.data)) }}</td>
                      <td style="text-align:center;">
                        <span class="status-badge" :class="statusClass(String(item.status))">{{ item.status }}</span>
                      </td>
                      <td style="text-align:center;">
                        <a v-if="item.transacao || (item.detalhes && (item.detalhes as unknown[]).length)"
                           href="javascript:void(0)" @click="abrirDetalhes(item)"
                           style="color:#2f7785;font-weight:600;font-size:.8rem;">Mais detalhes</a>
                        <span v-else style="color:#aaa;font-size:.8rem;">—</span>
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
  </div>

  <!-- Modal Detalhes -->
  <teleport to="body">
    <div v-if="showModal" class="ag-modal-overlay" @click.self="showModal = false">
      <div class="ag-modal" style="max-width:750px;">
        <div class="ag-modal-header">
          <h2>Movimentação detalhada da compra</h2>
          <button @click="showModal = false" style="background:none;border:none;font-size:1.25rem;cursor:pointer;color:#6c757d;line-height:1;" title="Fechar">✕</button>
        </div>
        <div class="ag-modal-body">
          <template v-if="detalhesSelecionado.length">
            <h3 style="font-size:1rem;font-weight:700;color:#2f7785;margin-bottom:.75rem;">Movimentação detalhada da compra</h3>
            <div style="overflow-x:auto;margin-bottom:1.5rem;">
              <table class="table-custom" style="width:100%;">
                <thead>
                  <tr>
                    <th>Descrição</th>
                    <th style="text-align:center;">Data</th>
                    <th style="text-align:center;">Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(d, i) in detalhesSelecionado" :key="i">
                    <td>{{ (d as Record<string,unknown>).descricao || '—' }}</td>
                    <td style="text-align:center;">{{ formatDate(String((d as Record<string,unknown>).dataAtualizacao || '')) }}</td>
                    <td style="text-align:center;">
                      <span class="status-badge" :class="statusClass(String(((d as Record<string,unknown>).status as Record<string,unknown>)?.nome || ''))">
                        {{ ((d as Record<string,unknown>).status as Record<string,unknown>)?.nome || '—' }}
                      </span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </template>

          <template v-if="listaDistribuicao.length">
            <h3 style="font-size:1rem;font-weight:700;color:#2f7785;margin-bottom:.75rem;">Distribuição do cashback</h3>
            <div style="overflow-x:auto;">
              <table class="table-custom" style="width:100%;">
                <thead>
                  <tr>
                    <th>Descrição</th>
                    <th style="text-align:center;">Data</th>
                    <th style="text-align:center;">Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(d, i) in listaDistribuicao" :key="i">
                    <td>{{ (d as Record<string,unknown>).descricao || '—' }}</td>
                    <td style="text-align:center;">{{ formatDate(String((d as Record<string,unknown>).dataAtualizacao || '')) }}</td>
                    <td style="text-align:center;">
                      <span class="status-badge" :class="statusClass(String(((d as Record<string,unknown>).status as Record<string,unknown>)?.nome || ''))">
                        {{ ((d as Record<string,unknown>).status as Record<string,unknown>)?.nome || '—' }}
                      </span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </template>

          <p v-if="!detalhesSelecionado.length && !listaDistribuicao.length" style="text-align:center;color:#aaa;">
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
