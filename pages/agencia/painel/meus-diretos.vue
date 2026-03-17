<template>
  <div>
    <div class="ag-page-header"><h1>Meus Diretos</h1><p>Pessoas que você indicou diretamente</p></div>
    <div class="ag-card mb-3">
      <div class="mb-2" style="font-size:.875rem">Seu link de indicação:</div>
      <div class="d-flex align-items-center gap-2 flex-wrap">
        <code class="bg-light p-2 rounded flex-grow-1 text-break" style="font-size:.85rem">{{ linkIndicacao }}</code>
        <button class="btn btn-ag-outline btn-sm" @click="copiar">{{ copiado ? '✓ Copiado!' : 'Copiar' }}</button>
      </div>
    </div>
    <div class="ag-card">
      <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
      <div v-else-if="diretos.length === 0" class="ag-empty-state">
        <h5>Nenhum direto encontrado</h5>
        <p>Compartilhe seu link e comece a construir sua rede.</p>
      </div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Nome</th><th>Login</th><th>Cadastro</th><th>Status</th><th>Cashback gerado</th></tr></thead>
          <tbody>
            <tr v-for="(d, i) in diretos" :key="i">
              <td>{{ d.nome || d.username }}</td>
              <td class="text-muted">{{ d.login }}</td>
              <td>{{ formatDate(d.dataCadastro) }}</td>
              <td><span class="badge-ag" :class="d.ativo ? 'badge-ag-success' : 'badge-ag-secondary'">{{ d.ativo ? 'Ativo' : 'Inativo' }}</span></td>
              <td class="text-ag-secondary fw-bold">{{ formatCurrency(d.cashbackGerado || 0) }}</td>
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
const diretos = ref<any[]>([]);
const copiado = ref(false);
const linkIndicacao = computed(() => `https://quantashop.com.br/register/${agenciaStore.dadosUser?.login || ''}`);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v); }
async function copiar() {
  try { await navigator.clipboard.writeText(linkIndicacao.value); copiado.value = true; setTimeout(() => { copiado.value = false; }, 2000); } catch { /**/ }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/parceiroDireto/listar', authHeader());
    diretos.value = Array.isArray(data) ? data : (data?.items || []);
  } catch { /**/ } finally { loading.value = false; }
});
</script>
