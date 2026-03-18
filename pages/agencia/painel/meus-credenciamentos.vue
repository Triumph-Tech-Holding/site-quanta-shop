<template>
  <div>
    <div class="ag-page-header">
      <h1>
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width:24px;height:24px;vertical-align:middle;margin-right:.4rem"><path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4z"/></svg>
        Meus Credenciamentos
      </h1>
    </div>

    <div class="ag-card mb-3">
      <div class="d-flex align-items-center gap-2 flex-wrap mb-3">
        <span style="font-size:.85rem;color:#6c757d">Link de indicação da rede:</span>
        <code class="bg-light p-2 rounded flex-grow-1 text-break" style="font-size:.82rem">{{ linkIndicacao }}</code>
        <button class="btn btn-ag-outline btn-sm" @click="copiarLink">{{ copiado ? '✓ Copiado!' : 'Copiar link' }}</button>
      </div>

      <form @submit.prevent="buscar" class="row g-2 align-items-end">
        <div class="col-12 col-md-4 ag-form-group mb-0">
          <label>Estabelecimento</label>
          <input v-model="filtro.estabelecimento" type="text" class="form-control" placeholder="Nome do estabelecimento..." />
        </div>
        <div class="col-12 col-md-3 ag-form-group mb-0">
          <label>Categoria</label>
          <select v-model="filtro.idCategoria" class="form-select">
            <option :value="null">Selecione a categoria</option>
            <option v-for="c in categorias" :key="c.id" :value="c.id">{{ c.nome }}</option>
          </select>
        </div>
        <div class="col-12 col-md-3 ag-form-group mb-0">
          <label>Status</label>
          <select v-model="filtro.idStatus" class="form-select">
            <option :value="null">Todos</option>
            <option :value="1">Aprovado</option>
            <option :value="0">Pendente</option>
            <option :value="2">Recusado</option>
          </select>
        </div>
        <div class="col-12 col-md-2">
          <button type="submit" class="btn btn-ag-primary w-100">Filtrar</button>
        </div>
      </form>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <template v-else-if="credenciamentos.length === 0">
      <div class="ag-empty-state">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4z"/></svg>
        <h5>Nenhum credenciamento encontrado</h5>
        <p>Você ainda não possui estabelecimentos credenciados na sua rede.</p>
      </div>
    </template>

    <template v-else>
      <div v-for="(c, i) in credenciamentos" :key="i" class="ag-cred-card">
        <div class="ag-cred-info">
          <div class="ag-cred-name">{{ c.Estabelecimento || c.estabelecimento }}</div>
          <div class="ag-cred-detail">Categoria: <span>{{ c.Categoria || c.categoria }}</span></div>
          <div class="ag-cred-detail">
            Endereço: <span>{{ [c.Rua || c.rua, c.Numero || c.numero, c.Bairro || c.bairro, c.Cidade || c.cidade].filter(Boolean).join(', ') }}</span>
          </div>
          <div class="ag-cred-detail">CNPJ: <span>{{ c.Cnpj || c.cnpj || '—' }}</span></div>
          <div class="ag-cred-detail">Data da última venda: <span>{{ formatDate(c.DataUltimaVenda || c.dataUltimaVenda) }}</span></div>
          <div class="ag-cred-detail">Total vendido: <span class="text-ag-primary fw-bold">{{ formatCurrency(c.TotalVendas || c.totalVendas || 0) }}</span></div>
        </div>
        <div class="ag-cred-status">
          <span class="badge-ag" :class="statusClass(c.StatusDesc || c.statusDesc || c.Status || c.status)">
            {{ c.StatusDesc || c.statusDesc || c.Status || c.status || 'Pendente' }}
          </span>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();

const loading = ref(true);
const copiado = ref(false);
const credenciamentos = ref<Record<string, unknown>[]>([]);
const categorias = ref<{ id: number; nome: string }[]>([]);

const filtro = reactive<{ estabelecimento: string; idCategoria: number | null; idStatus: number | null }>({
  estabelecimento: '',
  idCategoria: null,
  idStatus: null,
});

const linkIndicacao = computed(() => {
  const login = agenciaStore.dadosUser?.login || '';
  return `https://quantashop.com.br/register/${login}`;
});

function formatDate(d: string | null | undefined): string {
  if (!d) return 'Nenhuma venda registrada';
  return new Date(d).toLocaleDateString('pt-BR');
}

function formatCurrency(v: number | null | undefined): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

function statusClass(s: string | number | null | undefined): string {
  const str = String(s || '').toLowerCase();
  if (str === 'aprovado' || str === '1') return 'badge-ag-success';
  if (str === 'pendente' || str === '0') return 'badge-ag-warning';
  if (str === 'recusado' || str === '2') return 'badge-ag-danger';
  return 'badge-ag-secondary';
}

async function copiarLink() {
  try {
    await navigator.clipboard.writeText(linkIndicacao.value);
    copiado.value = true;
    setTimeout(() => { copiado.value = false; }, 2000);
  } catch { /**/ }
}

async function buscar() {
  loading.value = true;
  await loadCredenciamentos();
}

async function loadCredenciamentos() {
  try {
    const token = agenciaStore.getToken();
    const { data } = await api.post('/Credenciamento/meusCredenciamentos', {
      estabelecimento: filtro.estabelecimento || '',
      idCategoria: filtro.idCategoria,
      idStatus: filtro.idStatus,
    }, { headers: { Authorization: `Bearer ${token}` } });
    credenciamentos.value = Array.isArray(data) ? data : [];
  } catch (e) {
    console.error('Erro ao carregar credenciamentos:', e);
    credenciamentos.value = [];
  } finally {
    loading.value = false;
  }
}

async function loadCategorias() {
  try {
    const { data } = await api.get('/Categoria/obterCategorias');
    categorias.value = Array.isArray(data) ? data.map((c: Record<string, unknown>) => ({
      id: (c.IdCategoria ?? c.idCategoria ?? c.id) as number,
      nome: (c.Nome ?? c.nome) as string,
    })) : [];
  } catch { /**/ }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await Promise.allSettled([loadCredenciamentos(), loadCategorias()]);
});
</script>
