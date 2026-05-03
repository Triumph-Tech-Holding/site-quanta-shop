<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Comerciante" title="Painel Comerciante" description="Gerencie seu estabelecimento credenciado e acompanhe as vendas." />

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>

          <!-- KPIs -->
          <div class="com-kpi-grid">
            <div class="com-kpi">
              <div class="com-kpi__icon com-kpi__icon--teal">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96C5 16.1 6.1 17 7 17h11v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12.9-1.63H15c.75 0 1.41-.41 1.75-1.03l3.58-6.49A1 1 0 0 0 19.5 4H5.21L4.54 2H1zm16 16c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z"/></svg>
              </div>
              <div class="com-kpi__valor">{{ resumo.totalVendas }}</div>
              <div class="com-kpi__label">Vendas este mês</div>
            </div>
            <div class="com-kpi">
              <div class="com-kpi__icon com-kpi__icon--lime">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
              </div>
              <div class="com-kpi__valor com-kpi__valor--lime">{{ formatCurrency(resumo.faturamento) }}</div>
              <div class="com-kpi__label">Faturamento</div>
            </div>
            <div class="com-kpi">
              <div class="com-kpi__icon com-kpi__icon--gold">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
              </div>
              <div class="com-kpi__valor com-kpi__valor--teal">{{ formatCurrency(resumo.cashbackPago) }}</div>
              <div class="com-kpi__label">Cashback pago</div>
            </div>
            <div class="com-kpi">
              <div class="com-kpi__icon com-kpi__icon--teal">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-8 0c1.66 0 2.99-1.34 2.99-3S9.66 5 8 5C6.34 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
              </div>
              <div class="com-kpi__valor">{{ resumo.totalClientes }}</div>
              <div class="com-kpi__label">Clientes únicos</div>
            </div>
          </div>

          <!-- Últimas vendas -->
          <div class="qs-card-section com-vendas-card">
            <div class="qs-section-title" style="margin-bottom:1rem;">Últimas vendas</div>
            <div v-if="vendas.length === 0" class="ag-empty-state" style="min-height:100px;">
              <h5>Nenhuma venda registrada</h5>
            </div>
            <div v-else class="com-table-wrap">
              <table class="qs-table">
                <thead>
                  <tr>
                    <th class="tc">Data</th>
                    <th>Cliente</th>
                    <th class="tr">Valor</th>
                    <th class="tr">Cashback</th>
                    <th class="tc">Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(v, i) in vendas" :key="i">
                    <td class="tc">{{ formatDate(v.data) }}</td>
                    <td>{{ v.cliente }}</td>
                    <td class="tr com-valor">{{ formatCurrency(v.valor) }}</td>
                    <td class="tr com-cashback">{{ formatCurrency(v.cashback) }}</td>
                    <td class="tc">
                      <span class="qs-badge" :class="v.status === 'Aprovado' ? 'qs-badge--success' : 'qs-badge--warn'">{{ v.status }}</span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

        </template>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);

interface VendaComerciante { data: string; cliente: string; valor: number; cashback: number; status: string; }
interface ResumoComerciante { totalVendas: number; faturamento: number; cashbackPago: number; totalClientes: number; }

const resumo = reactive<ResumoComerciante>({ totalVendas: 0, faturamento: 0, cashbackPago: 0, totalClientes: 0 });
const vendas = ref<VendaComerciante[]>([]);

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get<{ resumo?: ResumoComerciante; vendas?: VendaComerciante[] }>('/comerciante/painel', authHeader());
    if (data?.resumo) Object.assign(resumo, data.resumo);
    if (data?.vendas) vendas.value = data.vendas;
  } catch (err: unknown) {
    console.error('Erro ao carregar painel comerciante:', extractApiErrorMessage(err));
  } finally { loading.value = false; }
});
</script>

<style scoped>
.com-kpi-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(150px, 1fr)); gap: .875rem; margin-bottom: 1.25rem; }
.com-kpi { background: #fff; border-radius: var(--qs-radius-md,12px); box-shadow: var(--qs-shadow-sm,0 2px 8px rgba(1,15,28,.08)); padding: 1.125rem 1rem; display: flex; flex-direction: column; align-items: center; gap: .4rem; text-align: center; transition: all .2s; }
.com-kpi:hover { box-shadow: var(--qs-shadow-md); transform: translateY(-2px); }
.com-kpi__icon { width: 40px; height: 40px; border-radius: 10px; display: flex; align-items: center; justify-content: center; margin-bottom: .2rem; }
.com-kpi__icon svg { width: 20px; height: 20px; color: #fff; }
.com-kpi__icon--teal { background: var(--qs-teal,#2F7785); }
.com-kpi__icon--lime { background: var(--qs-lime,#98C73A); }
.com-kpi__icon--gold { background: #FFB342; }
.com-kpi__valor { font-size: 1.5rem; font-weight: 800; color: var(--qs-ink,#1d1d1f); letter-spacing: -0.02em; line-height: 1; }
.com-kpi__valor--lime { color: var(--qs-lime-dark,#7aad1f); }
.com-kpi__valor--teal { color: var(--qs-teal,#2F7785); }
.com-kpi__label { font-size: .625rem; font-weight: 700; text-transform: uppercase; letter-spacing: .07em; color: var(--qs-gray-400,#9ca3af); }

.com-vendas-card { background: #fff; }
.com-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: .875rem; }
.qs-table thead tr { border-bottom: 2px solid var(--qs-gray-100,#f5f5f7); }
.qs-table th { padding: .625rem .875rem; font-size: .6875rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-500,#6b7280); white-space: nowrap; }
.qs-table td { padding: .75rem .875rem; color: var(--qs-gray-700,#374151); border-bottom: 1px solid var(--qs-gray-100,#f5f5f7); }
.qs-table tbody tr:hover td { background: var(--qs-gray-50,#fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }
.tr { text-align: right !important; }
.com-valor { font-weight: 600; }
.com-cashback { color: var(--qs-lime-dark,#7aad1f) !important; font-weight: 700; }

.qs-badge { display: inline-flex; padding: .2rem .55rem; border-radius: var(--qs-radius-pill,999px); font-size: .6875rem; font-weight: 700; text-transform: uppercase; white-space: nowrap; }
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--warn { background: #fef9c3; color: #ca8a04; }
</style>
