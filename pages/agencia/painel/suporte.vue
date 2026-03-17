<template>
  <div>
    <div class="ag-page-header"><h1>Suporte</h1><p>Acompanhe suas solicitações de suporte</p></div>
    <div class="mb-3 text-end">
      <NuxtLink to="/agencia/painel/solicitar-suporte" class="btn btn-ag-primary">+ Nova Solicitação</NuxtLink>
    </div>
    <div class="ag-card">
      <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
      <div v-else-if="tickets.length === 0" class="ag-empty-state">
        <h5>Nenhuma solicitação encontrada</h5>
        <p>Abra uma nova solicitação para receber ajuda da nossa equipe.</p>
      </div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>#</th><th>Assunto</th><th>Data</th><th>Status</th></tr></thead>
          <tbody>
            <tr v-for="(t, i) in tickets" :key="i">
              <td>{{ t.id || t.idSuporte }}</td>
              <td>{{ t.assunto || t.titulo || '—' }}</td>
              <td>{{ formatDate(t.data || t.dataCriacao) }}</td>
              <td>
                <span class="badge-ag" :class="statusClass(t.status)">{{ t.status || 'Aberto' }}</span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const tickets = ref<any[]>([]);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function statusClass(s: string) { return { 'Aberto': 'badge-ag-info', 'Em andamento': 'badge-ag-warning', 'Resolvido': 'badge-ag-success', 'Fechado': 'badge-ag-secondary' }[s] || 'badge-ag-secondary'; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/suporte/listar', authHeader());
    tickets.value = Array.isArray(data) ? data : (data?.items || []);
  } catch { /**/ } finally { loading.value = false; }
});
</script>
