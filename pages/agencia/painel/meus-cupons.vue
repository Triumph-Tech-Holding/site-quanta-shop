<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Cashback" title="Meus Cupons Cashback" description="Cupons de cashback gerados por você.">
          <NuxtLink to="/agencia/painel/gerar-cupons" class="qs-btn-primary mc-btn-gerar">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z"/></svg>
            Gerar Cupom
          </NuxtLink>
        </QsPageHeader>

        <div class="qs-card-section mc-card">
          <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>
          <div v-else-if="cupons.length === 0" class="ag-empty-state" style="min-height:140px;">
            <h5>Nenhum cupom gerado</h5>
            <p>Gere seu primeiro cupom de cashback.</p>
          </div>
          <div v-else class="mc-table-wrap">
            <table class="qs-table">
              <thead>
                <tr>
                  <th>Código</th>
                  <th>Loja</th>
                  <th class="tr">Valor</th>
                  <th class="tc">Gerado em</th>
                  <th class="tc">Status</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(c, i) in cupons" :key="i">
                  <td><code class="mc-code">{{ c.codigo }}</code></td>
                  <td>{{ c.nomeLoja || c.loja || '—' }}</td>
                  <td class="tr mc-valor">{{ formatCurrency(c.valor || c.valorCashback || 0) }}</td>
                  <td class="tc">{{ formatDate(c.dataCriacao || c.data) }}</td>
                  <td class="tc">
                    <span class="qs-badge" :class="c.aprovado ? 'qs-badge--success' : 'qs-badge--warn'">
                      {{ c.aprovado ? 'Aprovado' : 'Aguardando' }}
                    </span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
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
import type { Cupom } from "~/types/agencia";
const cupons = ref<Cupom[]>([]);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v); }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/cuponCashback/listarMeus', authHeader());
    cupons.value = Array.isArray(data) ? data : (data?.items || []);
  } catch { /**/ } finally { loading.value = false; }
});
</script>

<style scoped>
.mc-btn-gerar { display: inline-flex; align-items: center; gap: .4rem; flex-shrink: 0; }
.mc-btn-gerar svg { width: 16px; height: 16px; }

.mc-card { background: #fff; }
.mc-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: .875rem; }
.qs-table thead tr { border-bottom: 2px solid var(--qs-gray-100, #f5f5f7); }
.qs-table th { padding: .625rem .875rem; font-size: .6875rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-500, #6b7280); white-space: nowrap; }
.qs-table td { padding: .75rem .875rem; color: var(--qs-gray-700, #374151); border-bottom: 1px solid var(--qs-gray-100, #f5f5f7); }
.qs-table tbody tr:hover td { background: var(--qs-gray-50, #fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }
.tr { text-align: right !important; }

.mc-code { font-family: monospace; font-size: .875rem; color: var(--qs-teal-dark, #225F6B); background: var(--qs-gray-50, #fafafa); padding: .15rem .4rem; border-radius: 4px; }
.mc-valor { color: var(--qs-lime-dark, #7aad1f) !important; font-weight: 700; }

.qs-badge { display: inline-flex; padding: .2rem .55rem; border-radius: var(--qs-radius-pill, 999px); font-size: .6875rem; font-weight: 700; text-transform: uppercase; white-space: nowrap; }
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--warn { background: #fef9c3; color: #ca8a04; }
</style>
