<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Plus" title="Assinatura Mensal" description="Assine e receba cashback em dobro em toda a sua rede." />

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>

          <!-- Plano ativo -->
          <div v-if="planoAtivo" class="ass-plano-ativo">
            <div class="ass-plano-ativo__badge">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"/></svg>
              Plano ativo
            </div>
            <div class="ass-plano-ativo__nome">{{ planoAtivo.nome }}</div>
            <p class="ass-plano-ativo__desc">{{ planoAtivo.descricao }}</p>
          </div>

          <!-- Planos disponíveis -->
          <div v-if="!planoAtivo || planos.length > 1">
            <div class="qs-section-title" style="margin-bottom:1rem;">
              {{ planoAtivo ? 'Outros planos disponíveis' : 'Planos disponíveis' }}
            </div>

            <div class="ass-planos-grid">
              <div
                v-for="plano in planosFiltrados"
                :key="String(plano.idPlano)"
                class="ass-plano-card"
                :class="{ 'ass-plano-card--selected': planoSelecionado?.idPlano === plano.idPlano }"
                @click="planoSelecionado = plano"
              >
                <div class="ass-plano-card__check">
                  <svg v-if="planoSelecionado?.idPlano === plano.idPlano" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                </div>
                <div class="ass-plano-card__nome">{{ plano.nome }}</div>
                <div class="ass-plano-card__valor">
                  {{ formatCurrency(Number(plano.valor || 0)) }}
                  <span class="ass-plano-card__periodo">/mês</span>
                </div>
                <p class="ass-plano-card__desc">{{ plano.descricao }}</p>
              </div>
            </div>

            <div v-if="planoSelecionado" class="ass-cta">
              <button class="qs-btn-primary ass-cta__btn" @click="assinar" :disabled="assinando">
                <svg v-if="assinando" class="ass-spin-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 4V2A10 10 0 0 0 2 12h2a8 8 0 0 1 8-8z"/></svg>
                {{ assinando ? 'Processando...' : `Assinar ${planoSelecionado.nome}` }}
              </button>
            </div>

            <div v-if="mensagem" class="ass-msg" :class="sucesso ? 'ass-msg--ok' : 'ass-msg--err'">
              {{ mensagem }}
            </div>
          </div>

          <div v-if="planos.length === 0" class="ag-empty-state">
            <h5>Nenhum plano disponível no momento</h5>
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
const planos = ref<Array<Record<string, unknown>>>([]);
const planoAtivo = ref<Record<string, unknown> | null>(null);
const planoSelecionado = ref<Record<string, unknown> | null>(null);
const assinando = ref(false);
const mensagem = ref('');
const sucesso = ref(false);

const planosFiltrados = computed(() =>
  planos.value.filter(p => !planoAtivo.value || p.idPlano !== planoAtivo.value.idPlano)
);

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

async function assinar() {
  if (!planoSelecionado.value) return;
  assinando.value = true;
  mensagem.value = '';
  try {
    await api.post('/Assinatura/assinar', { idPlano: planoSelecionado.value.idPlano }, authHeader());
    sucesso.value = true;
    mensagem.value = `Plano ${planoSelecionado.value.nome} ativado com sucesso!`;
    planoAtivo.value = planoSelecionado.value;
    planoSelecionado.value = null;
  } catch (err: unknown) {
    sucesso.value = false;
    mensagem.value = (err as Record<string, Record<string, string>>)?.response?.data?.message || 'Erro ao ativar plano.';
  } finally {
    assinando.value = false;
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await Promise.allSettled([
    api.get('/Assinatura/listarPlanos', authHeader()).then(r => {
      planos.value = Array.isArray(r.data) ? r.data : (r.data?.items ?? []);
    }).catch(() => { planos.value = []; }),
    api.get('/Assinatura/planoAtivo', authHeader()).then(r => {
      planoAtivo.value = r.data ?? null;
    }).catch(() => { planoAtivo.value = null; }),
  ]);
  loading.value = false;
});
</script>

<style scoped>
/* Plano ativo */
.ass-plano-ativo {
  background: linear-gradient(135deg, #BF953F, #FCF6BA 50%, #AA771C);
  border-radius: var(--qs-radius-lg, 20px);
  padding: 1.5rem;
  text-align: center;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 20px rgba(191,149,63,.25);
}
.ass-plano-ativo__badge {
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  background: rgba(255,255,255,.35);
  padding: .25rem .75rem;
  border-radius: var(--qs-radius-pill, 999px);
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .06em;
  color: #78450a;
  margin-bottom: .75rem;
}
.ass-plano-ativo__badge svg { width: 13px; height: 13px; }
.ass-plano-ativo__nome {
  font-size: 1.375rem;
  font-weight: 800;
  color: var(--qs-teal-dark, #225F6B);
  margin-bottom: .35rem;
}
.ass-plano-ativo__desc { font-size: .875rem; color: #5a3a00; margin: 0; }

/* Planos grid */
.ass-planos-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 1rem;
  margin-bottom: 1.5rem;
}
.ass-plano-card {
  background: #fff;
  border: 2px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-lg, 20px);
  padding: 1.375rem 1.25rem;
  cursor: pointer;
  transition: all .2s;
  position: relative;
}
.ass-plano-card:hover { border-color: var(--qs-teal, #2F7785); box-shadow: var(--qs-shadow-md, 0 4px 20px rgba(1,15,28,.12)); }
.ass-plano-card--selected {
  border-color: var(--qs-lime, #98C73A);
  box-shadow: 0 0 0 3px rgba(152,199,58,.2);
}
.ass-plano-card__check {
  position: absolute;
  top: .75rem;
  right: .75rem;
  width: 22px;
  height: 22px;
  border-radius: 50%;
  background: var(--qs-lime, #98C73A);
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0;
  transition: opacity .15s;
}
.ass-plano-card--selected .ass-plano-card__check { opacity: 1; }
.ass-plano-card__check svg { width: 13px; height: 13px; color: #fff; }
.ass-plano-card__nome {
  font-size: 1rem;
  font-weight: 700;
  color: var(--qs-teal-dark, #225F6B);
  margin-bottom: .5rem;
}
.ass-plano-card__valor {
  font-size: 1.625rem;
  font-weight: 800;
  color: var(--qs-lime-dark, #7aad1f);
  margin-bottom: .5rem;
  line-height: 1.1;
}
.ass-plano-card__periodo { font-size: .8125rem; font-weight: 400; color: var(--qs-gray-400, #9ca3af); }
.ass-plano-card__desc { font-size: .8125rem; color: var(--qs-gray-500, #6b7280); margin: 0; line-height: 1.5; }

/* CTA */
.ass-cta { display: flex; justify-content: center; margin-bottom: 1.25rem; }
.ass-cta__btn {
  display: inline-flex;
  align-items: center;
  gap: .5rem;
  min-width: 200px;
  justify-content: center;
  font-size: 1rem;
  padding: .875rem 2rem;
}
.ass-spin-icon { width: 16px; height: 16px; animation: ass-spin 1s linear infinite; }
@keyframes ass-spin { to { transform: rotate(360deg); } }

/* Mensagem */
.ass-msg {
  padding: .875rem 1.25rem;
  border-radius: var(--qs-radius-md, 12px);
  font-size: .875rem;
  max-width: 480px;
  margin: 0 auto;
  text-align: center;
}
.ass-msg--ok { background: #dcfce7; color: #16a34a; }
.ass-msg--err { background: #fee2e2; color: #dc2626; }
</style>
