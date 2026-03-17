<template>
  <div>
    <div class="ag-page-header"><h1>Gerar Cupom Cashback</h1><p>Crie cupons de cashback para suas compras</p></div>
    <div class="ag-card" style="max-width:600px">
      <form @submit.prevent="gerarCupom" class="row g-3">
        <div class="col-12 ag-form-group">
          <label>Selecionar Loja *</label>
          <select v-model="form.idLoja" class="form-select" required>
            <option value="">Selecione a loja</option>
            <option v-for="l in lojas" :key="l.id" :value="l.id">{{ l.nome || l.razaoSocial }}</option>
          </select>
        </div>
        <div class="col-12 ag-form-group">
          <label>Valor da compra *</label>
          <input v-model.number="form.valor" type="number" step="0.01" min="1" class="form-control" required placeholder="R$ 0,00" />
        </div>
        <div class="col-12">
          <button type="submit" class="btn btn-ag-primary" :disabled="gerando">
            <span v-if="gerando" class="spinner-border spinner-border-sm me-1" />
            {{ gerando ? 'Gerando...' : 'Gerar Cupom' }}
          </button>
        </div>
      </form>
      <div v-if="cupomGerado" class="mt-4 p-3 border rounded text-center" style="border-style:dashed!important;background:#f8fffe">
        <div class="text-muted mb-1" style="font-size:.8rem">Seu cupom:</div>
        <div class="text-ag-primary fw-bold" style="font-size:2rem;letter-spacing:.15em">{{ cupomGerado.codigo }}</div>
        <div class="text-ag-secondary fw-bold">Cashback: {{ formatCurrency(cupomGerado.valorCashback || 0) }}</div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp() as any;
const gerando = ref(false);
const lojas = ref<any[]>([]);
const cupomGerado = ref<any>(null);
const form = reactive({ idLoja: '', valor: null as number | null });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v); }
async function gerarCupom() {
  gerando.value = true;
  try {
    const { data } = await api.post('/cuponCashback/gerar', form, authHeader());
    cupomGerado.value = data;
    $toast?.success('Cupom gerado com sucesso!');
  } catch (e: any) {
    $toast?.error(e?.response?.data?.message || 'Erro ao gerar cupom.');
  } finally { gerando.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/loja/listar', authHeader());
    lojas.value = Array.isArray(data) ? data : (data?.items || []);
  } catch { /**/ }
});
</script>
