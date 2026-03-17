<template>
  <div>
    <div class="ag-page-header"><h1>Cupons</h1><p>Seus cupons de desconto disponíveis</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="cupons.length === 0" class="ag-empty-state"><h5>Nenhum cupom disponível</h5></div>
      <div v-else class="row g-3">
        <div v-for="(c, i) in cupons" :key="i" class="col-12 col-md-4">
          <div class="p-3 border rounded text-center" style="border-style:dashed!important;background:#f8fffe">
            <div class="text-muted mb-1" style="font-size:.75rem;text-transform:uppercase;letter-spacing:.08em">{{ c.nomeLoja || 'Quanta Shop' }}</div>
            <div class="text-ag-primary fw-bold" style="font-size:1.75rem;letter-spacing:.1em">{{ c.codigo }}</div>
            <div class="text-ag-secondary fw-bold">{{ c.desconto }}% de desconto</div>
            <div class="text-muted mt-1" style="font-size:.8rem">Válido até {{ formatDate(c.validade) }}</div>
            <button class="btn btn-ag-outline btn-sm mt-2" @click="copiarCupom(c.codigo)">
              {{ copiado === c.codigo ? '✓ Copiado!' : 'Copiar código' }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
import type { Cupom } from "~/types/agencia";
const cupons = ref<Cupom[]>([]);
const copiado = ref('');
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
async function copiarCupom(code: string) {
  try { await navigator.clipboard.writeText(code); copiado.value = code; setTimeout(() => { copiado.value = ''; }, 2000); } catch { /**/ }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/cupons/listar', authHeader());
    cupons.value = Array.isArray(data) ? data : [];
  } catch { /**/ } finally { loading.value = false; }
});
</script>
