<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">Rede</div>
            <h1>Performance da Equipe</h1>
            <p>Visualize o consumo e desempenho dos membros da sua rede.</p>
          </div>
        </div>

        <!-- Filter bar -->
        <div class="qs-filter-bar">
          <form @submit.prevent="listarDados" class="pf-filter-form">
            <div class="pf-radio-group">
              <label class="qs-label" style="margin-bottom:.5rem;">Ordenação</label>
              <div class="pf-radios">
                <label class="pf-radio" :class="{ 'pf-radio--ativo': filtro.ordenacao === 'DESC' }">
                  <input type="radio" v-model="filtro.ordenacao" value="DESC" />
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M7.41 15.41L12 10.83l4.59 4.58L18 14l-6-6-6 6z"/></svg>
                  Top 10 consumo
                </label>
                <label class="pf-radio" :class="{ 'pf-radio--ativo': filtro.ordenacao === 'ASC' }">
                  <input type="radio" v-model="filtro.ordenacao" value="ASC" />
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M7.41 8.59L12 13.17l4.59-4.58L18 10l-6 6-6-6z"/></svg>
                  Últimos 10
                </label>
              </div>
            </div>
            <div class="pf-login-group">
              <label class="qs-label">Filtrar por Login</label>
              <input v-model="filtro.login" type="text" class="qs-input" placeholder="Login do membro" />
            </div>
            <button type="submit" class="qs-btn-primary pf-btn-filtrar">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0 0 16 9.5 6.5 6.5 0 1 0 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
              Filtrar
            </button>
          </form>
        </div>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div class="qs-card-section pf-results-card">
            <div class="pf-results-header">
              <div class="qs-section-title">Listagem da equipe</div>
              <span class="pf-count-badge">{{ items.length }} resultado(s)</span>
            </div>

            <div v-if="items.length === 0" class="ag-empty-state" style="min-height:120px;">
              <h5>Nenhum dado encontrado</h5>
              <p>Tente ajustar os filtros</p>
            </div>

            <div v-else class="pf-table-wrap">
              <table class="qs-table">
                <thead>
                  <tr>
                    <th>#</th>
                    <th>Login</th>
                    <th>Nome</th>
                    <th class="tc">Graduação</th>
                    <th class="tr">Consumo</th>
                    <th class="tc">Nível</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, i) in items" :key="i">
                    <td class="pf-rank">
                      <span class="pf-rank-num" :class="i < 3 ? `pf-rank-num--top${i+1}` : ''">{{ i + 1 }}</span>
                    </td>
                    <td>{{ item.login }}</td>
                    <td>{{ item.nome || item.nomeUsuario || '—' }}</td>
                    <td class="tc">{{ item.graduacao || '—' }}</td>
                    <td class="tr td-lime">{{ formatCurrency(Number(item.consumo || 0)) }}</td>
                    <td class="tc">
                      <span class="mr-nivel-badge">N{{ item.nivel || '—' }}</span>
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
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(false);
const items = ref<Record<string, unknown>[]>([]);

const filtro = reactive({ ordenacao: 'DESC', login: '' });

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

async function listarDados() {
  loading.value = true;
  try {
    const body: Record<string, unknown> = { ordenacao: filtro.ordenacao };
    if (filtro.login) body.login = filtro.login;
    const { data } = await api.post('/Dashboard/obterPerformance', body, authHeader());
    items.value = Array.isArray(data) ? data : [];
  } catch {
    try {
      const { data } = await api.get('/Dashboard/obterPerformance', authHeader());
      items.value = Array.isArray(data) ? data : [];
    } catch {
      items.value = [];
    }
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  agenciaStore.loadFromStorage();
  listarDados();
});
</script>

<style scoped>
/* Filter form */
.pf-filter-form {
  display: flex;
  flex-wrap: wrap;
  gap: 1.25rem;
  align-items: flex-end;
}
.pf-radio-group { display: flex; flex-direction: column; }
.pf-radios { display: flex; gap: .625rem; flex-wrap: wrap; }
.pf-radio {
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  padding: .5rem .875rem;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  font-size: .8125rem;
  font-weight: 500;
  color: var(--qs-gray-500, #6b7280);
  cursor: pointer;
  transition: all .15s;
}
.pf-radio input[type="radio"] { position: absolute; opacity: 0; pointer-events: none; }
.pf-radio svg { width: 14px; height: 14px; }
.pf-radio--ativo {
  border-color: var(--qs-teal, #2F7785);
  background: rgba(47,119,133,.06);
  color: var(--qs-teal, #2F7785);
  font-weight: 600;
}
.pf-login-group { display: flex; flex-direction: column; gap: .35rem; min-width: 180px; flex: 1; max-width: 260px; }
.pf-btn-filtrar {
  display: inline-flex;
  align-items: center;
  gap: .4rem;
  flex-shrink: 0;
}
.pf-btn-filtrar svg { width: 15px; height: 15px; }

/* Results */
.pf-results-card { background: #fff; }
.pf-results-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1rem;
  flex-wrap: wrap;
  gap: .5rem;
}
.pf-count-badge {
  font-size: .75rem;
  font-weight: 600;
  background: var(--qs-gray-100, #f5f5f7);
  color: var(--qs-gray-500, #6b7280);
  padding: .2rem .625rem;
  border-radius: var(--qs-radius-pill, 999px);
}

/* Table */
.pf-table-wrap { overflow-x: auto; }
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

/* Rank */
.pf-rank { width: 48px; }
.pf-rank-num {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 26px;
  height: 26px;
  border-radius: 50%;
  font-size: .75rem;
  font-weight: 700;
  background: var(--qs-gray-100, #f5f5f7);
  color: var(--qs-gray-500, #6b7280);
}
.pf-rank-num--top1 { background: linear-gradient(135deg, #FFD700, #FFA500); color: #fff; }
.pf-rank-num--top2 { background: linear-gradient(135deg, #C0C0C0, #a8a8a8); color: #fff; }
.pf-rank-num--top3 { background: linear-gradient(135deg, #CD7F32, #b5622a); color: #fff; }

/* Nivel badge */
.mr-nivel-badge {
  display: inline-flex;
  padding: .2rem .5rem;
  background: #dbeafe;
  color: #1d4ed8;
  border-radius: var(--qs-radius-pill, 999px);
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
}

/* Shared */
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
  box-sizing: border-box;
}
.qs-input:focus { outline: none; border-color: var(--qs-teal, #2F7785); }
</style>
