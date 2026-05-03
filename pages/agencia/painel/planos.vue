<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Plus" title="Planos disponíveis" description="Escolha o plano ideal e desbloqueie vantagens exclusivas na Quanta Shop." />

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div v-if="planos.length === 0" class="ag-empty-state">
            <h5>Nenhum plano disponível</h5>
          </div>

          <div v-else class="pl-grid">
            <div
              v-for="(p, i) in planos"
              :key="i"
              class="pl-card"
              :class="{ 'pl-card--destaque': p.destaque }"
            >
              <div v-if="p.destaque" class="pl-card__popular">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"/></svg>
                Mais popular
              </div>

              <div class="pl-card__nome">{{ p.nome }}</div>

              <div class="pl-card__valor">
                {{ formatCurrency(p.valor || p.mensalidade) }}
                <span class="pl-card__mes">/mês</span>
              </div>

              <p class="pl-card__desc">{{ p.descricao }}</p>

              <ul v-if="(p.beneficios || []).length" class="pl-card__beneficios">
                <li v-for="(b, j) in (p.beneficios || [])" :key="j">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                  {{ b }}
                </li>
              </ul>

              <button
                class="pl-card__btn"
                :class="p.destaque ? 'pl-card__btn--destaque' : 'pl-card__btn--default'"
                @click="assinar(p)"
                :disabled="assinando === p.id"
              >
                <svg v-if="assinando === p.id" class="pl-spin-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 4V2A10 10 0 0 0 2 12h2a8 8 0 0 1 8-8z"/></svg>
                {{ assinando === p.id ? 'Processando...' : 'Assinar agora' }}
              </button>
            </div>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { Plano } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp();
const loading = ref(true);
const planos = ref<Plano[]>([]);
const assinando = ref<number | null>(null);

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function formatCurrency(v: number) {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

async function assinar(plano: Plano) {
  assinando.value = plano.id ?? null;
  try {
    await api.post('/assinatura/assinar', { idPlano: plano.id }, authHeader());
    $toast?.success(`Plano "${plano.nome}" assinado com sucesso!`);
    navigateTo('/agencia/painel/assinatura');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao assinar plano.'));
  } finally {
    assinando.value = null;
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/planos/listar', authHeader());
    planos.value = Array.isArray(data) ? data : [];
  } catch {
    planos.value = [];
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.pl-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: 1.25rem;
  align-items: start;
}

.pl-card {
  background: #fff;
  border: 2px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-lg, 20px);
  padding: 1.75rem 1.5rem;
  display: flex;
  flex-direction: column;
  gap: .875rem;
  transition: all .2s;
  position: relative;
}
.pl-card:hover { box-shadow: var(--qs-shadow-md, 0 4px 20px rgba(1,15,28,.12)); transform: translateY(-2px); }
.pl-card--destaque {
  border-color: var(--qs-lime, #98C73A);
  box-shadow: 0 0 0 3px rgba(152,199,58,.15), var(--qs-shadow-md, 0 4px 20px rgba(1,15,28,.12));
}

.pl-card__popular {
  display: inline-flex;
  align-items: center;
  gap: .3rem;
  background: linear-gradient(135deg, var(--qs-lime, #98C73A), #7aad1f);
  color: #fff;
  font-size: .625rem;
  font-weight: 800;
  text-transform: uppercase;
  letter-spacing: .08em;
  padding: .25rem .7rem;
  border-radius: var(--qs-radius-pill, 999px);
  width: fit-content;
}
.pl-card__popular svg { width: 11px; height: 11px; }

.pl-card__nome {
  font-size: 1.125rem;
  font-weight: 800;
  color: var(--qs-teal-dark, #225F6B);
}

.pl-card__valor {
  font-size: 2rem;
  font-weight: 800;
  color: var(--qs-teal, #2F7785);
  line-height: 1.1;
  letter-spacing: -0.02em;
}
.pl-card__mes { font-size: .875rem; font-weight: 400; color: var(--qs-gray-400, #9ca3af); }

.pl-card__desc { font-size: .8125rem; color: var(--qs-gray-500, #6b7280); line-height: 1.55; margin: 0; }

.pl-card__beneficios {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: .4rem;
  flex: 1;
}
.pl-card__beneficios li {
  display: flex;
  align-items: flex-start;
  gap: .4rem;
  font-size: .8125rem;
  color: var(--qs-gray-700, #374151);
  line-height: 1.4;
}
.pl-card__beneficios svg {
  width: 15px;
  height: 15px;
  flex-shrink: 0;
  color: var(--qs-lime, #98C73A);
  margin-top: .05rem;
}

.pl-card__btn {
  width: 100%;
  padding: .75rem 1rem;
  border: none;
  border-radius: var(--qs-radius-md, 12px);
  font-size: .9375rem;
  font-weight: 700;
  cursor: pointer;
  transition: all .2s;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: .4rem;
  margin-top: auto;
}
.pl-card__btn--destaque {
  background: var(--qs-gradient-btn, linear-gradient(135deg, #225F6B, #2F7785));
  color: #fff;
  box-shadow: 0 3px 10px rgba(47,119,133,.3);
}
.pl-card__btn--destaque:hover:not(:disabled) { opacity: .9; transform: translateY(-1px); }
.pl-card__btn--default {
  background: var(--qs-gray-100, #f5f5f7);
  color: var(--qs-teal-dark, #225F6B);
}
.pl-card__btn--default:hover:not(:disabled) { background: var(--qs-gray-200, #e5e7eb); }
.pl-card__btn:disabled { opacity: .6; cursor: not-allowed; }

.pl-spin-icon { width: 15px; height: 15px; animation: pl-spin 1s linear infinite; }
@keyframes pl-spin { to { transform: rotate(360deg); } }
</style>
