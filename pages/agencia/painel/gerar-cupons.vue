<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Cashback" title="Gerar Cupom Cashback" description="Crie um cupom de cashback vinculado à sua compra." />

        <div style="display:grid;grid-template-columns:1fr 1fr;gap:1.25rem;align-items:start;" class="gc-layout">

          <div class="qs-card-section gc-form-card">
            <div class="qs-section-title" style="margin-bottom:1.25rem;">Dados do cupom</div>
            <form @submit.prevent="gerarCupom" class="gc-form">
              <div class="gc-field">
                <label class="qs-label">Selecionar Loja *</label>
                <select v-model="form.idLoja" class="qs-input" required>
                  <option value="">Selecione a loja</option>
                  <option v-for="l in lojas" :key="l.id" :value="l.id">{{ l.nome || l.razaoSocial }}</option>
                </select>
              </div>
              <div class="gc-field">
                <label class="qs-label">Valor da compra *</label>
                <div class="gc-valor-wrap">
                  <span class="gc-valor-prefix">R$</span>
                  <input v-model.number="form.valor" type="number" step="0.01" min="1" class="qs-input gc-valor-input" required placeholder="0,00" />
                </div>
              </div>
              <button type="submit" class="qs-btn-primary gc-submit" :disabled="gerando">
                <svg v-if="gerando" class="gc-spin" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 4V2A10 10 0 0 0 2 12h2a8 8 0 0 1 8-8z"/></svg>
                <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M20 6h-2.18c.07-.44.18-.86.18-1.3C18 2.12 15.88 0 13.3 0c-1.3 0-2.48.56-3.3 1.44L8 4 5.9 1.44C5.08.56 3.9 0 2.6 0 1.02 0 0 1.12 0 2.7c0 .44.11.86.18 1.3H0v2h20V6zm-6.7-4c1.48 0 2.7 1.22 2.7 2.7 0 .44-.11.86-.26 1.3H11.6c-.15-.44-.26-.86-.26-1.3C11.34 3.22 12.52 2 14 2zM0 8v14h20V8H0zm2 12V10h16v10H2z"/></svg>
                {{ gerando ? 'Gerando...' : 'Gerar Cupom' }}
              </button>
            </form>
          </div>

          <div class="gc-result-wrap">
            <div v-if="!cupomGerado" class="gc-empty-result">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M20 6h-2.18c.07-.44.18-.86.18-1.3C18 2.12 15.88 0 13.3 0c-1.3 0-2.48.56-3.3 1.44L8 4 5.9 1.44C5.08.56 3.9 0 2.6 0 1.02 0 0 1.12 0 2.7c0 .44.11.86.18 1.3H0v2h20V6zm-6.7-4c1.48 0 2.7 1.22 2.7 2.7 0 .44-.11.86-.26 1.3H11.6c-.15-.44-.26-.86-.26-1.3C11.34 3.22 12.52 2 14 2zM0 8v14h20V8H0zm2 12V10h16v10H2z"/></svg>
              <p>Seu cupom aparecerá aqui após a geração.</p>
            </div>
            <div v-else class="gc-cupom-display">
              <div class="gc-cupom-display__label">Seu cupom gerado</div>
              <div class="gc-cupom-display__code">{{ cupomGerado.codigo }}</div>
              <div class="gc-cupom-display__cashback">
                Cashback: <strong>{{ formatCurrency(cupomGerado.valorCashback || 0) }}</strong>
              </div>
              <button class="gc-cupom-display__copy" @click="copiar">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 1H4c-1.1 0-2 .9-2 2v14h2V3h12V1zm3 4H8c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h11c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2zm0 16H8V7h11v14z"/></svg>
                {{ copiado ? 'Copiado!' : 'Copiar código' }}
              </button>
            </div>
          </div>

        </div>

      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp();
const gerando = ref(false);
const copiado = ref(false);
import type { Loja } from "~/types/agencia";
const lojas = ref<Loja[]>([]);
interface CupomGerado { codigo: string; valorCashback?: number; [key: string]: unknown; }
const cupomGerado = ref<CupomGerado | null>(null);
const form = reactive({ idLoja: '', valor: null as number | null });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v); }
async function copiar() {
  if (!cupomGerado.value) return;
  try { await navigator.clipboard.writeText(cupomGerado.value.codigo); copiado.value = true; setTimeout(() => { copiado.value = false; }, 2000); } catch { /**/ }
}
async function gerarCupom() {
  gerando.value = true;
  try {
    const { data } = await api.post('/cuponCashback/gerar', form, authHeader());
    cupomGerado.value = data;
    $toast?.success('Cupom gerado com sucesso!');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao gerar cupom.'));
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

<style scoped>
.gc-layout { grid-template-columns: 1fr 1fr; }
@media (max-width: 640px) { .gc-layout { grid-template-columns: 1fr; } }

.gc-form-card { background: #fff; }
.gc-form { display: flex; flex-direction: column; gap: 1rem; }
.gc-field { display: flex; flex-direction: column; gap: .375rem; }
.gc-valor-wrap { position: relative; }
.gc-valor-prefix { position: absolute; left: .875rem; top: 50%; transform: translateY(-50%); font-size: .875rem; color: var(--qs-gray-500, #6b7280); font-weight: 600; }
.gc-valor-input { padding-left: 2.5rem !important; }
.gc-submit { display: inline-flex; align-items: center; gap: .4rem; }
.gc-submit svg { width: 16px; height: 16px; }
.gc-spin { animation: gc-spin 1s linear infinite; }
@keyframes gc-spin { to { transform: rotate(360deg); } }

.gc-result-wrap { min-height: 200px; display: flex; align-items: center; justify-content: center; }
.gc-empty-result {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: .75rem;
  text-align: center;
  color: var(--qs-gray-300, #d1d5db);
  padding: 2rem;
}
.gc-empty-result svg { width: 52px; height: 52px; }
.gc-empty-result p { font-size: .875rem; color: var(--qs-gray-400, #9ca3af); margin: 0; }

.gc-cupom-display {
  background: #fff;
  border: 2px dashed var(--qs-teal, #2F7785);
  border-radius: var(--qs-radius-lg, 20px);
  padding: 2rem 1.5rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: .5rem;
  text-align: center;
  width: 100%;
  box-shadow: var(--qs-shadow-sm);
}
.gc-cupom-display__label { font-size: .625rem; font-weight: 700; text-transform: uppercase; letter-spacing: .1em; color: var(--qs-gray-400, #9ca3af); }
.gc-cupom-display__code { font-size: 2.25rem; font-weight: 800; letter-spacing: .15em; color: var(--qs-teal-dark, #225F6B); line-height: 1.1; margin: .25rem 0; }
.gc-cupom-display__cashback { font-size: .9375rem; color: var(--qs-gray-600, #4b5563); }
.gc-cupom-display__cashback strong { color: var(--qs-lime-dark, #7aad1f); }
.gc-cupom-display__copy {
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  margin-top: .75rem;
  padding: .5rem 1.125rem;
  border: 1.5px solid var(--qs-teal, #2F7785);
  border-radius: var(--qs-radius-pill, 999px);
  background: transparent;
  color: var(--qs-teal, #2F7785);
  font-size: .8125rem;
  font-weight: 700;
  cursor: pointer;
  transition: all .15s;
}
.gc-cupom-display__copy:hover { background: var(--qs-teal, #2F7785); color: #fff; }
.gc-cupom-display__copy svg { width: 13px; height: 13px; }

.qs-label { font-size: .75rem; font-weight: 600; color: var(--qs-gray-700, #374151); text-transform: uppercase; letter-spacing: .04em; }
.qs-input { width: 100%; padding: .625rem .875rem; border: 1.5px solid var(--qs-gray-200, #e5e7eb); border-radius: var(--qs-radius-md, 12px); font-size: .875rem; color: var(--qs-ink, #1d1d1f); background: #fff; transition: border-color .15s; box-sizing: border-box; }
.qs-input:focus { outline: none; border-color: var(--qs-teal, #2F7785); }
</style>
