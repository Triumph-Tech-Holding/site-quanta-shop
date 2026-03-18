<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page" style="font-size:1.1rem;">CCR | CONTA DE CONSUMO REMUNERADA</h2>
        </div>

        <div class="px-3 pb-3">
          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

          <template v-else>
            <div class="ag-card">
              <div class="ag-card-title">Resumo Financeiro</div>
              <div style="display:flex;flex-wrap:wrap;gap:1rem;">
                <div v-for="stat in summary" :key="stat.label" style="flex:1;min-width:120px;text-align:center;padding:1rem;background:#f8f9fa;border-radius:8px;">
                  <div style="font-size:.75rem;color:#6c757d;text-transform:uppercase;margin-bottom:.4rem;">{{ stat.label }}</div>
                  <div style="font-size:1.25rem;font-weight:700;color:#225f6b;">{{ stat.value }}</div>
                </div>
              </div>
            </div>

            <div class="ag-card">
              <div style="display:flex;border-bottom:2px solid #eee;margin-bottom:1.25rem;">
                <button
                  v-for="tab in tabs" :key="tab.key"
                  @click="activeTab = tab.key"
                  style="padding:.65rem 1.25rem;border:none;background:transparent;font-weight:500;cursor:pointer;font-size:.875rem;"
                  :style="activeTab === tab.key ? 'color:#2f7785;border-bottom:2px solid #2f7785;margin-bottom:-2px;font-weight:700;' : 'color:#6c757d;'"
                >
                  {{ tab.label }}
                </button>
              </div>

              <div v-if="activeTab === 'movimentacoes'">
                <div v-if="movimentacoes.length === 0" class="ag-empty-state" style="min-height:100px;">
                  <h5>Nenhuma movimentação encontrada</h5>
                </div>
                <div v-else style="overflow-x:auto;">
                  <table class="table-custom" style="width:100%;">
                    <thead>
                      <tr>
                        <th>Descrição</th>
                        <th style="text-align:center;">Data</th>
                        <th style="text-align:right;">Valor</th>
                        <th style="text-align:center;">Tipo</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(m, i) in movimentacoes" :key="i">
                        <td>{{ m.descricao || '—' }}</td>
                        <td style="text-align:center;">{{ formatDate(String(m.data || m.dataCriacao || '')) }}</td>
                        <td style="text-align:right;" :style="Number(m.valor) >= 0 ? 'color:#98c73a;font-weight:600;' : 'color:#dc3545;font-weight:600;'">
                          {{ formatCurrency(Number(m.valor || 0)) }}
                        </td>
                        <td style="text-align:center;">
                          <span class="status-badge" :class="Number(m.valor) >= 0 ? 'aprovado' : 'recusado'">
                            {{ Number(m.valor) >= 0 ? 'Crédito' : 'Débito' }}
                          </span>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-if="activeTab === 'saque'">
                <div class="ag-card" style="box-shadow:none;padding:0;">
                  <h4 style="font-size:.9rem;font-weight:700;color:#225f6b;margin-bottom:1rem;">Solicitar Saque</h4>
                  <form @submit.prevent="solicitarSaque">
                    <div style="display:flex;flex-wrap:wrap;gap:1rem;margin-bottom:1rem;">
                      <div style="flex:1;min-width:200px;">
                        <label style="font-size:.875rem;font-weight:600;display:block;margin-bottom:.25rem;">Valor do saque</label>
                        <input type="number" v-model="saque.valor" min="1" step="0.01" class="form-control" placeholder="R$ 0,00" />
                      </div>
                      <div style="display:flex;align-items:flex-end;">
                        <button type="submit" class="btn-filtrar" :disabled="saqueLoading">
                          {{ saqueLoading ? 'Solicitando...' : 'Solicitar' }}
                        </button>
                      </div>
                    </div>
                    <div v-if="saqueMensagem" :style="saqueSucesso ? 'color:#155724;background:#d4edda;' : 'color:#721c24;background:#f8d7da;'" style="padding:.75rem;border-radius:6px;font-size:.875rem;">
                      {{ saqueMensagem }}
                    </div>
                  </form>
                </div>
              </div>

              <div v-if="activeTab === 'historico'">
                <div v-if="historico.length === 0" class="ag-empty-state" style="min-height:100px;">
                  <h5>Nenhum histórico de saque</h5>
                </div>
                <div v-else style="overflow-x:auto;">
                  <table class="table-custom" style="width:100%;">
                    <thead>
                      <tr>
                        <th>Data solicitação</th>
                        <th style="text-align:right;">Valor</th>
                        <th style="text-align:center;">Status</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(s, i) in historico" :key="i">
                        <td>{{ formatDate(String(s.dataSolicitacao || s.data || '')) }}</td>
                        <td style="text-align:right;color:#98c73a;font-weight:600;">{{ formatCurrency(Number(s.valor || 0)) }}</td>
                        <td style="text-align:center;">
                          <span class="status-badge" :class="statusClass(String(s.status || s.statusNome || ''))">
                            {{ s.status || s.statusNome || '—' }}
                          </span>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
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
const activeTab = ref('movimentacoes');
const movimentacoes = ref<Record<string, unknown>[]>([]);
const historico = ref<Record<string, unknown>[]>([]);
const saqueLoading = ref(false);
const saqueMensagem = ref('');
const saqueSucesso = ref(false);

const saque = reactive({ valor: '' });

const summary = ref([
  { label: 'Saldo disponível', value: '—' },
  { label: 'Total recebido', value: '—' },
  { label: 'Total sacado', value: '—' },
]);

const tabs = [
  { key: 'movimentacoes', label: 'Movimentações' },
  { key: 'saque', label: 'Solicitar Saque' },
  { key: 'historico', label: 'Histórico de Saques' },
];

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

function formatDate(d: string): string {
  if (!d) return '—';
  return new Date(d).toLocaleDateString('pt-BR');
}

function statusClass(s: string): string {
  const lower = (s || '').toLowerCase();
  if (lower.includes('aprov') || lower.includes('pago') || lower.includes('conclu')) return 'aprovado';
  if (lower.includes('recus') || lower.includes('cancel')) return 'recusado';
  if (lower.includes('process') || lower.includes('andamento')) return 'pago';
  return 'pendente';
}

async function solicitarSaque() {
  if (!saque.valor || Number(saque.valor) <= 0) return;
  saqueLoading.value = true;
  saqueMensagem.value = '';
  try {
    await api.post('/Saque/solicitarSaque', { valor: Number(saque.valor) }, authHeader());
    saqueSucesso.value = true;
    saqueMensagem.value = 'Saque solicitado com sucesso!';
    saque.valor = '';
    await carregarHistorico();
  } catch (err: unknown) {
    saqueSucesso.value = false;
    saqueMensagem.value = (err as Record<string, Record<string, string>>)?.response?.data?.message || 'Erro ao solicitar saque.';
  } finally {
    saqueLoading.value = false;
  }
}

async function carregarHistorico() {
  try {
    const { data } = await api.get('/Saque/historicoSaque', authHeader());
    historico.value = Array.isArray(data) ? data : (data?.items ?? []);
  } catch {
    historico.value = [];
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await Promise.allSettled([
    api.get('/financeiro/resumo', authHeader()).then(r => {
      const d = r.data as Record<string, number> | null;
      if (d) {
        summary.value = [
          { label: 'Saldo disponível', value: formatCurrency(d.saldo ?? d.Saldo ?? 0) },
          { label: 'Total recebido', value: formatCurrency(d.totalRecebido ?? d.TotalRecebido ?? 0) },
          { label: 'Total sacado', value: formatCurrency(d.totalSacado ?? d.TotalSacado ?? 0) },
        ];
      }
    }).catch(() => {}),
    api.post('/financeiro/movimentacoes', {}, authHeader()).then(r => {
      movimentacoes.value = Array.isArray(r.data) ? r.data : (r.data?.items ?? []);
    }).catch(async () => {
      try {
        const { data } = await api.get('/financeiro/movimentacoes', authHeader());
        movimentacoes.value = Array.isArray(data) ? data : (data?.items ?? []);
      } catch { movimentacoes.value = []; }
    }),
    carregarHistorico(),
  ]);
  loading.value = false;
});
</script>
