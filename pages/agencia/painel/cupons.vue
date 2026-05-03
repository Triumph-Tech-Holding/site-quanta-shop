<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Benefícios" title="Cupons de Desconto" description="Seus cupons disponíveis para usar nas compras." />

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div v-if="cupons.length === 0" class="ag-empty-state">
            <h5>Nenhum cupom disponível</h5>
          </div>
          <div v-else class="cup-grid">
            <div v-for="(c, i) in cupons" :key="i" class="cup-card">
              <div class="cup-card__store">{{ c.nomeLoja || 'Quanta Shop' }}</div>
              <div class="cup-card__code">{{ c.codigo }}</div>
              <div class="cup-card__discount">{{ c.desconto }}% de desconto</div>
              <div class="cup-card__validade">Válido até {{ formatDate(c.validade) }}</div>
              <button class="cup-card__copy" @click="copiarCupom(c.codigo)">
                <svg v-if="copiado === c.codigo" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 1H4c-1.1 0-2 .9-2 2v14h2V3h12V1zm3 4H8c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h11c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2zm0 16H8V7h11v14z"/></svg>
                {{ copiado === c.codigo ? 'Copiado!' : 'Copiar código' }}
              </button>
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

<style scoped>
.cup-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 1rem;
}
.cup-card {
  background: #fff;
  border: 2px dashed var(--qs-teal, #2F7785);
  border-radius: var(--qs-radius-lg, 20px);
  padding: 1.375rem 1.25rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: .375rem;
  text-align: center;
  transition: box-shadow .2s, transform .2s;
}
.cup-card:hover { box-shadow: var(--qs-shadow-md); transform: translateY(-2px); }
.cup-card__store {
  font-size: .625rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .1em;
  color: var(--qs-gray-400, #9ca3af);
}
.cup-card__code {
  font-size: 1.625rem;
  font-weight: 800;
  letter-spacing: .12em;
  color: var(--qs-teal-dark, #225F6B);
  line-height: 1.1;
  margin: .25rem 0;
}
.cup-card__discount {
  font-size: .875rem;
  font-weight: 700;
  color: var(--qs-lime-dark, #7aad1f);
}
.cup-card__validade {
  font-size: .75rem;
  color: var(--qs-gray-400, #9ca3af);
  margin-bottom: .25rem;
}
.cup-card__copy {
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  margin-top: .5rem;
  padding: .45rem .875rem;
  border: 1.5px solid var(--qs-teal, #2F7785);
  border-radius: var(--qs-radius-pill, 999px);
  background: transparent;
  color: var(--qs-teal, #2F7785);
  font-size: .75rem;
  font-weight: 700;
  cursor: pointer;
  transition: all .15s;
}
.cup-card__copy:hover { background: var(--qs-teal, #2F7785); color: #fff; }
.cup-card__copy svg { width: 13px; height: 13px; }
</style>
