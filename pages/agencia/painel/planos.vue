<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page">Planos</h2>
        </div>

        <div style="background:linear-gradient(135deg,#2f7785,#1a5c3a);padding:2rem;margin:0 1rem 1.5rem;border-radius:8px;color:#fff;">
          <h1 style="font-size:1.4rem;font-weight:700;text-transform:uppercase;margin-bottom:.5rem;">Planos disponíveis</h1>
          <h2 style="font-size:1rem;font-weight:400;margin:0;">Escolha o plano ideal e desbloqueie vantagens exclusivas</h2>
        </div>

        <div class="px-3 pb-3">
          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
          <div v-else>
            <div v-if="planos.length === 0" class="ag-empty-state">
              <h5>Nenhum plano disponível</h5>
            </div>
            <div v-else class="row g-3">
              <div v-for="(p, i) in planos" :key="i" class="col-12 col-md-4">
                <div class="ag-card h-100 text-center" :class="{ 'border border-2 border-ag-primary': p.destaque }">
                  <div v-if="p.destaque" class="badge-ag bg-ag-primary text-white mb-2 d-inline-block">Mais popular</div>
                  <h4 class="text-ag-primary mb-2">{{ p.nome }}</h4>
                  <div class="mb-3">
                    <span style="font-size:2rem;font-weight:700">{{ formatCurrency(p.valor || p.mensalidade) }}</span>
                    <small>/mês</small>
                  </div>
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
        </div>
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
