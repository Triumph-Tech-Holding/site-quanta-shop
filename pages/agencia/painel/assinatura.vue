<template>
  <div>
    <div class="ag-page-header">
      <h1>Assinatura</h1>
      <p>Gerencie seu plano de assinatura</p>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <template v-else>
      <div v-if="assinatura" class="ag-card mb-4">
        <div class="ag-card-title">Plano Atual</div>
        <div class="row align-items-center">
          <div class="col-12 col-md-8">
            <h3 class="text-ag-primary mb-1">{{ assinatura.nomePlano || assinatura.plano }}</h3>
            <div class="text-muted mb-2" style="font-size:.9rem">{{ assinatura.descricao }}</div>
            <div class="d-flex gap-3 flex-wrap">
              <div><span class="text-muted" style="font-size:.8rem">Valor mensal</span><br/><strong>{{ formatCurrency(assinatura.valor || assinatura.mensalidade) }}</strong></div>
              <div><span class="text-muted" style="font-size:.8rem">Vencimento</span><br/><strong>{{ formatDate(assinatura.vencimento || assinatura.dataVencimento) }}</strong></div>
              <div><span class="text-muted" style="font-size:.8rem">Status</span><br/>
                <span class="badge-ag" :class="assinatura.ativo ? 'badge-ag-success' : 'badge-ag-danger'">
                  {{ assinatura.ativo ? 'Ativo' : 'Inativo' }}
                </span>
              </div>
            </div>
          </div>
          <div class="col-12 col-md-4 text-end mt-3 mt-md-0">
            <NuxtLink to="/agencia/painel/planos" class="btn btn-ag-outline">Ver outros planos</NuxtLink>
          </div>
        </div>
      </div>

      <div class="ag-card">
        <div class="ag-card-title">Histórico de pagamentos</div>
        <div v-if="historico.length === 0" class="ag-empty-state">
          <h5>Nenhum pagamento encontrado</h5>
        </div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead><tr><th>Data</th><th>Plano</th><th>Valor</th><th>Status</th></tr></thead>
            <tbody>
              <tr v-for="(h, i) in historico" :key="i">
                <td>{{ formatDate(h.data || h.dataPagamento) }}</td>
                <td>{{ h.plano || h.nomePlano }}</td>
                <td>{{ formatCurrency(h.valor) }}</td>
                <td><span class="badge-ag" :class="h.pago ? 'badge-ag-success' : 'badge-ag-warning'">{{ h.pago ? 'Pago' : 'Pendente' }}</span></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);

interface AssinaturaUser {
  nomePlano?: string;
  plano?: string;
  descricao?: string;
  valor?: number;
  mensalidade?: number;
  vencimento?: string;
  dataVencimento?: string;
  ativo?: boolean;
  [key: string]: unknown;
}
const assinatura = ref<AssinaturaUser | null>(null);
import type { MovimentacaoFinanceira } from "~/types/agencia";
const historico = ref<MovimentacaoFinanceira[]>([]);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  await Promise.allSettled([
    api.get('/assinatura/obter', authHeader()).then(r => { assinatura.value = r.data; }),
    api.get('/assinatura/historico', authHeader()).then(r => { historico.value = Array.isArray(r.data) ? r.data : []; }),
  ]);
  loading.value = false;
});
</script>
