<template>
  <div>
    <div class="ag-page-header">
      <h1>Minhas Compras</h1>
      <p>CCR | Conta de Consumo Remunerada</p>
    </div>

    <div class="ag-card mb-3">
      <form @submit.prevent="buscar" class="row g-3 align-items-end">
        <div class="col-12 col-md-4 ag-form-group mb-0">
          <label>Descrição</label>
          <input v-model="filtro.descricao" type="text" class="form-control" placeholder="Nome da loja..." />
        </div>
        <div class="col-12 col-md-3 ag-form-group mb-0">
          <label>Data início</label>
          <input v-model="filtro.dataInicio" type="date" class="form-control" />
        </div>
        <div class="col-12 col-md-3 ag-form-group mb-0">
          <label>Data fim</label>
          <input v-model="filtro.dataFim" type="date" class="form-control" />
        </div>
        <div class="col-12 col-md-2">
          <button type="submit" class="btn btn-ag-primary w-100">Filtrar</button>
        </div>
      </form>
    </div>

    <div class="ag-card">
      <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

      <div v-else-if="compras.length === 0" class="ag-empty-state">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 3c1.93 0 3.5 1.57 3.5 3.5S13.93 13 12 13s-3.5-1.57-3.5-3.5S10.07 6 12 6zm7 13H5v-.23c0-.62.28-1.2.76-1.58C7.47 15.82 9.64 15 12 15s4.53.82 6.24 2.19c.48.38.76.97.76 1.58V19z"/></svg>
        <h5>Nenhuma compra encontrada</h5>
        <p>Ainda não há compras registradas no período selecionado.</p>
      </div>

      <template v-else>
        <div class="table-responsive">
          <table class="table ag-table">
            <thead>
              <tr>
                <th>#</th>
                <th>Loja / Descrição</th>
                <th>Data</th>
                <th>Valor</th>
                <th>Cashback</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(c, i) in compras" :key="i">
                <td>{{ c.idPedido || c.id || i + 1 }}</td>
                <td>{{ c.nomeLoja || c.descricao || '—' }}</td>
                <td>{{ formatDate(c.dataPedido || c.data) }}</td>
                <td>{{ formatCurrency(c.valorCompra || c.valor || 0) }}</td>
                <td class="fw-bold text-ag-secondary">{{ formatCurrency(c.cashback || c.valorCashback || 0) }}</td>
                <td>
                  <span class="badge-ag" :class="statusClass(c.status)">{{ c.status || 'Pendente' }}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div class="d-flex align-items-center justify-content-between mt-3 flex-wrap gap-2">
          <span class="text-muted" style="font-size:.85rem">
            Total: {{ total }} compras
          </span>
          <div class="d-flex gap-2">
            <button class="btn btn-ag-outline btn-sm" :disabled="page <= 1" @click="changePage(page - 1)">← Anterior</button>
            <span class="align-self-center" style="font-size:.85rem">{{ page }}/{{ totalPages }}</span>
            <button class="btn btn-ag-outline btn-sm" :disabled="page >= totalPages" @click="changePage(page + 1)">Próxima →</button>
          </div>
        </div>
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();

const compras = ref<any[]>([]);
const loading = ref(true);
const total = ref(0);
const page = ref(1);
const pageSize = 20;
const totalPages = computed(() => Math.max(1, Math.ceil(total.value / pageSize)));

const filtro = reactive({ descricao: '', dataInicio: '', dataFim: '' });

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
  const m: Record<string, string> = {
    'Aprovado': 'badge-ag-success', 'Pago': 'badge-ag-success',
    'Pendente': 'badge-ag-warning', 'Em análise': 'badge-ag-info',
    'Cancelado': 'badge-ag-danger', 'Recusado': 'badge-ag-danger',
  };
  return m[s] || 'badge-ag-secondary';
}

async function buscar() {
  loading.value = true;
  page.value = 1;
  await loadCompras();
}

async function changePage(p: number) {
  page.value = p;
  await loadCompras();
}

async function loadCompras() {
  try {
    const params = new URLSearchParams({
      page: String(page.value),
      pageSize: String(pageSize),
      ...(filtro.descricao && { descricao: filtro.descricao }),
      ...(filtro.dataInicio && { dataInicio: filtro.dataInicio }),
      ...(filtro.dataFim && { dataFim: filtro.dataFim }),
    });
    const { data } = await api.get(`/pedido/listar?${params}`, authHeader());
    if (Array.isArray(data)) {
      compras.value = data;
      total.value = data.length;
    } else {
      compras.value = data?.items || data?.pedidos || [];
      total.value = data?.total || compras.value.length;
    }
  } catch { compras.value = []; } finally {
    loading.value = false;
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await loadCompras();
});
</script>
