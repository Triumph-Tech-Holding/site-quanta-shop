<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page">Conheça nossa assinatura mensal</h2>
          <small style="color:#6c757d;display:block;margin-top:.25rem;">Assine nosso plano mensal e receba cashback em dobro</small>
        </div>

        <div class="px-3 pb-3">
          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

          <template v-else>
            <div v-if="planoAtivo" class="ag-card" style="background:linear-gradient(135deg,#BF953F,#FCF6BA,#B38728,#FBF5B7,#AA771C);margin-bottom:1.25rem;">
              <div style="text-align:center;padding:.5rem;">
                <h3 style="font-size:1rem;font-weight:700;color:#225f6b;margin-bottom:.25rem;">Seu plano atual</h3>
                <p style="font-size:1.1rem;font-weight:700;color:#225f6b;margin:0;">{{ planoAtivo.nome }}</p>
                <p style="color:#555;font-size:.85rem;margin:.25rem 0 0;">{{ planoAtivo.descricao }}</p>
              </div>
            </div>

            <div v-if="!planoAtivo || planos.length > 1">
              <h3 style="font-size:1rem;font-weight:700;color:#225f6b;margin-bottom:1rem;">
                {{ planoAtivo ? 'Outros planos disponíveis' : 'Planos disponíveis' }}
              </h3>
              <div style="display:flex;flex-wrap:wrap;gap:1rem;">
                <div
                  v-for="plano in planosFiltrados"
                  :key="plano.idPlano"
                  class="ag-card"
                  style="flex:1;min-width:220px;text-align:center;cursor:pointer;"
                  :style="planoSelecionado?.idPlano === plano.idPlano ? 'border:2px solid #98c73a;' : 'border:2px solid transparent;'"
                  @click="planoSelecionado = plano"
                >
                  <div style="font-size:1rem;font-weight:700;color:#225f6b;margin-bottom:.5rem;">{{ plano.nome }}</div>
                  <div style="font-size:1.5rem;font-weight:700;color:#98c73a;margin-bottom:.5rem;">
                    {{ formatCurrency(Number(plano.valor || 0)) }}<span style="font-size:.8rem;color:#6c757d;">/mês</span>
                  </div>
                  <p style="font-size:.8rem;color:#555;">{{ plano.descricao }}</p>
                </div>
              </div>

              <div v-if="planoSelecionado" style="margin-top:1.5rem;text-align:center;">
                <button class="btn-ag-secondary" style="font-size:.9rem;padding:.75rem 2rem;" @click="assinar" :disabled="assinando">
                  {{ assinando ? 'Processando...' : `Assinar ${planoSelecionado.nome}` }}
                </button>
              </div>

              <div v-if="mensagem" style="margin-top:1rem;padding:.75rem;border-radius:6px;font-size:.875rem;"
                :style="sucesso ? 'color:#155724;background:#d4edda;' : 'color:#721c24;background:#f8d7da;'">
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
