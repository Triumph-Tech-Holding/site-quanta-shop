<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page">Performance</h2>
        </div>

        <div class="px-3 pb-3">
          <div class="box-filter">
            <h2>Filtro</h2>
            <form @submit.prevent="listarDados">
              <div style="display:flex;flex-wrap:wrap;gap:1rem;margin-bottom:1rem;align-items:flex-end;">
                <div style="display:flex;gap:1rem;align-items:center;flex-wrap:wrap;flex:1;">
                  <label style="display:flex;align-items:center;gap:.4rem;font-size:.875rem;cursor:pointer;">
                    <input type="radio" v-model="filtro.ordenacao" value="DESC" />
                    10 primeiros por consumo
                  </label>
                  <label style="display:flex;align-items:center;gap:.4rem;font-size:.875rem;cursor:pointer;">
                    <input type="radio" v-model="filtro.ordenacao" value="ASC" />
                    10 últimos por consumo
                  </label>
                  <input v-model="filtro.login" type="text" class="form-control" placeholder="Login" style="max-width:200px;" />
                </div>
                <button type="submit" class="btn-filtrar" style="white-space:nowrap;">Filtrar</button>
              </div>
            </form>
          </div>

          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

          <template v-else>
            <div style="background:#fff;border-radius:8px;box-shadow:0 2px 8px rgba(0,0,0,.06);overflow:hidden;">
              <div style="padding:1rem;border-bottom:1px solid #eee;">
                <span style="font-weight:700;color:#225f6b;">Listagem da equipe</span>
              </div>

              <div v-if="items.length === 0" class="ag-empty-state">
                <h5>Nenhum dado encontrado</h5>
              </div>

              <div v-else style="overflow-x:auto;">
                <table class="table-custom" style="width:100%;">
                  <thead>
                    <tr>
                      <th>Login</th>
                      <th>Nome</th>
                      <th style="text-align:center;">Graduação</th>
                      <th style="text-align:center;">Consumo</th>
                      <th style="text-align:center;">Nível</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(item, i) in items" :key="i">
                      <td>{{ item.login }}</td>
                      <td>{{ item.nome || item.nomeUsuario || '—' }}</td>
                      <td style="text-align:center;">{{ item.graduacao || '—' }}</td>
                      <td style="text-align:center;">{{ formatCurrency(Number(item.consumo || 0)) }}</td>
                      <td style="text-align:center;">{{ item.nivel || item.nivel || '—' }}</td>
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
