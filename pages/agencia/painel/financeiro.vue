<template>
  <div>
    <div class="ag-page-header">
      <h1>Financeiro</h1>
      <p>CCR | Conta de Consumo Remunerada</p>
    </div>

    <div class="row g-3 mb-4">
      <div class="col-12 col-md-4">
        <div class="ag-stat-card">
          <div class="stat-icon">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
          </div>
          <div>
            <div class="stat-label">Saldo disponível</div>
            <div class="stat-value text-ag-primary">{{ formatCurrency(saldo) }}</div>
          </div>
        </div>
      </div>
      <div class="col-12 col-md-4">
        <div class="ag-stat-card">
          <div class="stat-icon">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
          </div>
          <div>
            <div class="stat-label">Total ganho</div>
            <div class="stat-value text-ag-secondary">{{ formatCurrency(totalGanhos) }}</div>
          </div>
        </div>
      </div>
      <div class="col-12 col-md-4">
        <div class="ag-stat-card">
          <div class="stat-icon">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M20 4H4c-1.11 0-2 .89-2 2v12c0 1.11.89 2 2 2h16c1.11 0 2-.89 2-2V6c0-1.11-.89-2-2-2zm0 14H4v-6h16v6zm0-10H4V6h16v2z"/></svg>
          </div>
          <div>
            <div class="stat-label">Total sacado</div>
            <div class="stat-value">{{ formatCurrency(totalSacado) }}</div>
          </div>
        </div>
      </div>
    </div>

    <div class="ag-card">
      <ul class="nav nav-tabs ag-tabs mb-4" role="tablist">
        <li class="nav-item"><button class="nav-link" :class="{ active: tab === 'extrato' }" @click="tab = 'extrato'; loadExtrato()">Extrato Financeiro</button></li>
        <li class="nav-item"><button class="nav-link" :class="{ active: tab === 'saque' }" @click="tab = 'saque'">Solicitar Saque</button></li>
        <li class="nav-item"><button class="nav-link" :class="{ active: tab === 'historico' }" @click="tab = 'historico'; loadHistoricoSaque()">Histórico de Saques</button></li>
      </ul>

      <!-- EXTRATO -->
      <div v-show="tab === 'extrato'">
        <div v-if="loadingExtrato" class="ag-loading"><div class="spinner-border" /></div>
        <div v-else-if="movimentacoes.length === 0" class="ag-empty-state">
          <h5>Nenhuma movimentação encontrada</h5>
        </div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead>
              <tr><th>Data</th><th>Descrição</th><th>Tipo</th><th>Valor</th></tr>
            </thead>
            <tbody>
              <tr v-for="(m, i) in movimentacoes" :key="i">
                <td>{{ formatDate(m.data || m.dataCriacao) }}</td>
                <td>{{ m.descricao || '—' }}</td>
                <td>
                  <span class="badge-ag" :class="m.tipo === 'E' || m.tipo === 'Entrada' ? 'badge-ag-success' : 'badge-ag-danger'">
                    {{ m.tipo === 'E' || m.tipo === 'Entrada' ? 'Entrada' : 'Saída' }}
                  </span>
                </td>
                <td :class="m.tipo === 'E' || m.tipo === 'Entrada' ? 'text-success fw-bold' : 'text-danger fw-bold'">
                  {{ (m.tipo === 'E' || m.tipo === 'Entrada') ? '+' : '-' }}{{ formatCurrency(Math.abs(m.valor || 0)) }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- SOLICITAR SAQUE -->
      <div v-show="tab === 'saque'">
        <div class="row justify-content-center">
          <div class="col-12 col-md-6">
            <div class="alert alert-info mb-3" style="font-size:.875rem">
              <strong>Saldo disponível:</strong> {{ formatCurrency(saldo) }}
            </div>
            <form @submit.prevent="solicitarSaque" class="row g-3">
              <div class="col-12 ag-form-group">
                <label>Valor do saque *</label>
                <input v-model.number="saqueForm.valor" type="number" step="0.01" min="1" class="form-control" required placeholder="R$ 0,00" />
              </div>
              <div class="col-12 ag-form-group">
                <label>Senha financeira *</label>
                <input v-model="saqueForm.senha" type="password" class="form-control" required placeholder="Sua senha financeira" />
              </div>
              <div class="col-12 ag-form-group">
                <label>Conta bancária</label>
                <select v-model="saqueForm.idContaBancaria" class="form-select">
                  <option value="">Selecione uma conta</option>
                  <option v-for="c in contasBancarias" :key="c.id" :value="c.id">
                    {{ c.banco }} — Ag {{ c.agencia }} / C {{ c.conta }}
                  </option>
                </select>
              </div>
              <div class="col-12 text-end">
                <button type="submit" class="btn btn-ag-primary" :disabled="solicitandoSaque">
                  <span v-if="solicitandoSaque" class="spinner-border spinner-border-sm me-1" />
                  {{ solicitandoSaque ? 'Solicitando...' : 'Solicitar Saque' }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>

      <!-- HISTORICO SAQUE -->
      <div v-show="tab === 'historico'">
        <div v-if="loadingHistorico" class="ag-loading"><div class="spinner-border" /></div>
        <div v-else-if="historicoSaque.length === 0" class="ag-empty-state">
          <h5>Nenhum saque encontrado</h5>
        </div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead><tr><th>Data</th><th>Valor</th><th>Banco</th><th>Status</th></tr></thead>
            <tbody>
              <tr v-for="(s, i) in historicoSaque" :key="i">
                <td>{{ formatDate(s.data || s.dataCriacao) }}</td>
                <td class="fw-bold">{{ formatCurrency(s.valor || 0) }}</td>
                <td>{{ s.banco || s.nomeBanco || '—' }}</td>
                <td><span class="badge-ag" :class="statusClass(s.status)">{{ s.status || 'Pendente' }}</span></td>
              </tr>
            </tbody>
          </table>
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

const tab = ref('extrato');
const saldo = ref(0);
const totalGanhos = ref(0);
const totalSacado = ref(0);
import type { MovimentacaoFinanceira, ContaBancaria } from "~/types/agencia";
const movimentacoes = ref<MovimentacaoFinanceira[]>([]);
const historicoSaque = ref<MovimentacaoFinanceira[]>([]);
const contasBancarias = ref<ContaBancaria[]>([]);
const loadingExtrato = ref(false);
const loadingHistorico = ref(false);
const solicitandoSaque = ref(false);
const saqueForm = reactive({ valor: null as number | null, senha: '', idContaBancaria: '' });

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
  return { 'Aprovado': 'badge-ag-success', 'Pago': 'badge-ag-success', 'Pendente': 'badge-ag-warning', 'Cancelado': 'badge-ag-danger' }[s] || 'badge-ag-secondary';
}

async function loadExtrato() {
  loadingExtrato.value = true;
  try {
    const { data } = await api.get('/financeiro/movimentacoes', authHeader());
    movimentacoes.value = Array.isArray(data) ? data : (data?.items || []);
  } catch { /**/ } finally { loadingExtrato.value = false; }
}

async function loadHistoricoSaque() {
  loadingHistorico.value = true;
  try {
    const { data } = await api.get('/financeiro/historicoSaque', authHeader());
    historicoSaque.value = Array.isArray(data) ? data : (data?.items || []);
  } catch { /**/ } finally { loadingHistorico.value = false; }
}

async function solicitarSaque() {
  solicitandoSaque.value = true;
  try {
    await api.post('/financeiro/solicitarSaque', saqueForm, authHeader());
    $toast?.success('Saque solicitado com sucesso!');
    saqueForm.valor = null; saqueForm.senha = ''; saqueForm.idContaBancaria = '';
    await loadSaldo();
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao solicitar saque.'));
  } finally { solicitandoSaque.value = false; }
}

async function loadSaldo() {
  try {
    const { data } = await api.get('/financeiro/saldo', authHeader());
    saldo.value = typeof data === 'number' ? data : (data?.saldo ?? 0);
    totalGanhos.value = data?.totalGanhos ?? 0;
    totalSacado.value = data?.totalSacado ?? 0;
  } catch { /**/ }
}

async function loadContasBancarias() {
  try {
    const { data } = await api.get('/contasBancarias/listar', authHeader());
    contasBancarias.value = Array.isArray(data) ? data : [];
  } catch { /**/ }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await Promise.allSettled([loadSaldo(), loadExtrato(), loadContasBancarias()]);
});
</script>
