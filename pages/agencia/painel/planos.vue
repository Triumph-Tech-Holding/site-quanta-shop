<template>
  <div>
    <div class="ag-page-header"><h1>Planos</h1><p>Escolha o plano ideal para você</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="row g-3">
      <div v-for="(p, i) in planos" :key="i" class="col-12 col-md-4">
        <div class="ag-card h-100 text-center" :class="{ 'border border-2 border-ag-primary': p.destaque }">
          <div v-if="p.destaque" class="badge-ag bg-ag-primary text-white mb-2 d-inline-block">Mais popular</div>
          <h4 class="text-ag-primary mb-2">{{ p.nome }}</h4>
          <div class="mb-3"><span style="font-size:2rem;font-weight:700">{{ formatCurrency(p.valor || p.mensalidade) }}</span><small>/mês</small></div>
          <p class="text-muted mb-3" style="font-size:.875rem">{{ p.descricao }}</p>
          <ul class="list-unstyled text-start mb-4">
            <li v-for="(b, j) in (p.beneficios || [])" :key="j" class="mb-1" style="font-size:.875rem">
              ✓ {{ b }}
            </li>
          </ul>
          <button class="btn btn-ag-primary w-100" @click="assinar(p)" :disabled="assinando === p.id">
            <span v-if="assinando === p.id" class="spinner-border spinner-border-sm me-1" />
            Assinar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp();
const loading = ref(true);
import type { Plano } from "~/types/agencia";
const planos = ref<Plano[]>([]);
const assinando = ref<number | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
async function assinar(plano: any) {
  assinando.value = plano.id;
  try {
    await api.post('/assinatura/assinar', { idPlano: plano.id }, authHeader());
    $toast?.success(`Plano "${plano.nome}" assinado com sucesso!`);
    navigateTo('/agencia/painel/assinatura');
  } catch (e: any) {
    $toast?.error(e?.response?.data?.message || 'Erro ao assinar plano.');
  } finally { assinando.value = null; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/planos/listar', authHeader());
    planos.value = Array.isArray(data) ? data : [];
  } catch { /**/ } finally { loading.value = false; }
});
</script>
