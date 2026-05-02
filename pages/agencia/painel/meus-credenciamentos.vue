<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">Rede</div>
            <h1>Meus Credenciamentos</h1>
            <p>Acompanhe os estabelecimentos que você indicou para a rede.</p>
          </div>
        </div>

        <!-- Link de indicação -->
        <div class="qs-card-section cred-link-card">
          <div class="cred-link-label">Seu link de indicação da rede</div>
          <div class="cred-link-row">
            <code class="cred-link-code">{{ linkIndicacao }}</code>
            <button class="qs-btn-secondary cred-copy-btn" @click="copiarLink">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 1H4c-1.1 0-2 .9-2 2v14h2V3h12V1zm3 4H8c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h11c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2zm0 16H8V7h11v14z"/></svg>
              {{ copiado ? '✓ Copiado!' : 'Copiar link' }}
            </button>
          </div>
        </div>

        <!-- Filtros -->
        <div class="qs-filter-bar">
          <form @submit.prevent="buscar" class="cred-filter-form">
            <div class="cred-filter-group" style="flex:2;min-width:200px;">
              <label class="qs-label">Estabelecimento</label>
              <input v-model="filtro.estabelecimento" type="text" class="qs-input" placeholder="Nome do estabelecimento..." />
            </div>
            <div class="cred-filter-group">
              <label class="qs-label">Categoria</label>
              <select v-model="filtro.idCategoria" class="qs-input">
                <option :value="null">Todas</option>
                <option v-for="c in categorias" :key="c.id" :value="c.id">{{ c.nome }}</option>
              </select>
            </div>
            <div class="cred-filter-group">
              <label class="qs-label">Status</label>
              <select v-model="filtro.idStatus" class="qs-input">
                <option :value="null">Todos</option>
                <option :value="1">Aprovado</option>
                <option :value="0">Pendente</option>
                <option :value="2">Recusado</option>
              </select>
            </div>
            <button type="submit" class="qs-btn-primary cred-btn-filtrar">Filtrar</button>
          </form>
        </div>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else-if="credenciamentos.length === 0">
          <div class="ag-empty-state">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="var(--qs-gray-200,#e5e7eb)"><path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4z"/></svg>
            <h5>Nenhum credenciamento encontrado</h5>
            <p>Você ainda não possui estabelecimentos credenciados na sua rede.</p>
          </div>
        </template>

        <template v-else>
          <div class="cred-cards-grid">
            <div v-for="(c, i) in credenciamentos" :key="i" class="cred-card">
              <div class="cred-card__header">
                <div class="cred-card__nome">{{ c.Estabelecimento || c.estabelecimento }}</div>
                <span class="qs-badge" :class="statusBadge(c.StatusDesc || c.statusDesc || c.Status || c.status)">
                  {{ c.StatusDesc || c.statusDesc || c.Status || c.status || 'Pendente' }}
                </span>
              </div>
              <div class="cred-card__details">
                <div class="cred-card__row">
                  <span class="cred-card__key">Categoria</span>
                  <span class="cred-card__val">{{ c.Categoria || c.categoria || '—' }}</span>
                </div>
                <div class="cred-card__row">
                  <span class="cred-card__key">Endereço</span>
                  <span class="cred-card__val">{{ [c.Rua || c.rua, c.Numero || c.numero, c.Bairro || c.bairro, c.Cidade || c.cidade].filter(Boolean).join(', ') || '—' }}</span>
                </div>
                <div class="cred-card__row">
                  <span class="cred-card__key">CNPJ</span>
                  <span class="cred-card__val">{{ c.Cnpj || c.cnpj || '—' }}</span>
                </div>
                <div class="cred-card__row">
                  <span class="cred-card__key">Última venda</span>
                  <span class="cred-card__val">{{ formatDate(c.DataUltimaVenda || c.dataUltimaVenda) }}</span>
                </div>
                <div class="cred-card__row">
                  <span class="cred-card__key">Total vendido</span>
                  <span class="cred-card__val cred-card__val--lime">{{ formatCurrency(c.TotalVendas || c.totalVendas || 0) }}</span>
                </div>
              </div>
            </div>
          </div>
        </template>

      </div>
    </div>
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
const filtro = reactive<{ estabelecimento: string; idCategoria: number | null; idStatus: number | null }>({ estabelecimento: '', idCategoria: null, idStatus: null });
const linkIndicacao = computed(() => `https://quantashop.com.br/register/${agenciaStore.dadosUser?.login || ''}`);
function formatDate(d: string | null | undefined): string { if (!d) return 'Nenhuma venda'; return new Date(d).toLocaleDateString('pt-BR'); }
function formatCurrency(v: number | null | undefined): string { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function statusBadge(s: string | number | null | undefined): string {
  const str = String(s || '').toLowerCase();
  if (str === 'aprovado' || str === '1') return 'qs-badge--success';
  if (str === 'pendente' || str === '0') return 'qs-badge--warn';
  if (str === 'recusado' || str === '2') return 'qs-badge--danger';
  return 'qs-badge--secondary';
}
async function copiarLink() {
  try { await navigator.clipboard.writeText(linkIndicacao.value); copiado.value = true; setTimeout(() => { copiado.value = false; }, 2000); } catch { /**/ }
}
async function buscar() { loading.value = true; await loadCredenciamentos(); }
async function loadCredenciamentos() {
  try {
    const { data } = await api.post('/Credenciamento/meusCredenciamentos', { estabelecimento: filtro.estabelecimento || '', idCategoria: filtro.idCategoria, idStatus: filtro.idStatus }, { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } });
    credenciamentos.value = Array.isArray(data) ? data : [];
  } catch { credenciamentos.value = []; } finally { loading.value = false; }
}
async function loadCategorias() {
  try {
    const { data } = await api.get('/Categoria/obterCategorias');
    categorias.value = Array.isArray(data) ? data.map((c: Record<string, unknown>) => ({ id: (c.IdCategoria ?? c.idCategoria ?? c.id) as number, nome: (c.Nome ?? c.nome) as string })) : [];
  } catch { /**/ }
}
onMounted(async () => { agenciaStore.loadFromStorage(); await Promise.allSettled([loadCredenciamentos(), loadCategorias()]); });
</script>

<style scoped>
.cred-link-card { background: #fff; }
.cred-link-label { font-size: .75rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-500,#6b7280); margin-bottom: .625rem; }
.cred-link-row { display: flex; align-items: center; gap: .75rem; flex-wrap: wrap; }
.cred-link-code { font-size: .8125rem; color: var(--qs-teal-dark,#225F6B); background: var(--qs-gray-50,#fafafa); border: 1px solid var(--qs-gray-200,#e5e7eb); border-radius: var(--qs-radius-md,12px); padding: .5rem .875rem; flex: 1; word-break: break-all; font-family: monospace; }
.cred-copy-btn { flex-shrink: 0; display: inline-flex; align-items: center; gap: .35rem; font-size: .8125rem; }
.cred-copy-btn svg { width: 14px; height: 14px; }

.cred-filter-form { display: flex; flex-wrap: wrap; gap: .875rem; align-items: flex-end; }
.cred-filter-group { display: flex; flex-direction: column; gap: .35rem; flex: 1; min-width: 150px; }
.cred-btn-filtrar { flex-shrink: 0; }

.cred-cards-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1rem; }
.cred-card { background: #fff; border: 1.5px solid var(--qs-gray-200,#e5e7eb); border-radius: var(--qs-radius-md,12px); padding: 1.25rem; display: flex; flex-direction: column; gap: .875rem; transition: all .2s; box-shadow: var(--qs-shadow-sm); }
.cred-card:hover { box-shadow: var(--qs-shadow-md); transform: translateY(-2px); }
.cred-card__header { display: flex; align-items: flex-start; justify-content: space-between; gap: .5rem; }
.cred-card__nome { font-size: 1rem; font-weight: 700; color: var(--qs-teal-dark,#225F6B); line-height: 1.3; }
.cred-card__details { display: flex; flex-direction: column; gap: .3rem; }
.cred-card__row { display: flex; gap: .5rem; font-size: .8125rem; }
.cred-card__key { color: var(--qs-gray-400,#9ca3af); min-width: 90px; flex-shrink: 0; }
.cred-card__val { color: var(--qs-gray-700,#374151); font-weight: 500; }
.cred-card__val--lime { color: var(--qs-lime-dark,#7aad1f) !important; font-weight: 700; }

.qs-badge { display: inline-flex; padding: .2rem .55rem; border-radius: var(--qs-radius-pill,999px); font-size: .6875rem; font-weight: 700; text-transform: uppercase; white-space: nowrap; flex-shrink: 0; }
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--warn { background: #fef9c3; color: #ca8a04; }
.qs-badge--danger { background: #fee2e2; color: #dc2626; }
.qs-badge--secondary { background: var(--qs-gray-100,#f5f5f7); color: var(--qs-gray-500,#6b7280); }

.qs-label { font-size: .75rem; font-weight: 600; color: var(--qs-gray-700,#374151); text-transform: uppercase; letter-spacing: .04em; }
.qs-input { width: 100%; padding: .625rem .875rem; border: 1.5px solid var(--qs-gray-200,#e5e7eb); border-radius: var(--qs-radius-md,12px); font-size: .875rem; color: var(--qs-ink,#1d1d1f); background: #fff; transition: border-color .15s; box-sizing: border-box; }
.qs-input:focus { outline: none; border-color: var(--qs-teal,#2F7785); }
</style>
