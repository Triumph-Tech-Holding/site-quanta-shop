<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Financeiro</div>
        <h1>Pagamentos</h1>
        <p>Pagamentos e transações da plataforma</p>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><rect x="2" y="5" width="20" height="14" rx="2"/><line x1="2" y1="10" x2="22" y2="10"/></svg>
        <h3>Nenhum pagamento encontrado</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Usuário</th><th>Valor</th><th>Status</th><th>Vencimento</th><th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.usuario }}</td>
              <td>{{ formatCurrency(item.valor) }}</td>
              <td>
                <span class="qs-badge" :class="item.status === 'Pago' ? 'qs-badge-success' : (item.status === 'Vencido' ? 'qs-badge-danger' : 'qs-badge-warn')">
                  {{ item.status }}
                </span>
              </td>
              <td>{{ formatDate(item.dataVencimento) }}</td>
              <td><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:480px">
        <div class="qs-modal-header">
          <h5>Pagamento #{{ selecionado.id }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-detail-grid">
            <div class="qs-detail-item"><span class="qs-detail-label">Usuário</span><span class="qs-detail-value">{{ selecionado.usuario }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Status</span><span class="qs-badge" :class="selecionado.status === 'Pago' ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.status }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Valor</span><span class="qs-detail-value">{{ formatCurrency(selecionado.valor) }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Vencimento</span><span class="qs-detail-value">{{ formatDate(selecionado.dataVencimento) }}</span></div>
          </div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Fechar</button>
          <button v-if="selecionado.status !== 'Pago'" class="qs-btn-primary" @click="marcarPago">Marcar como Pago</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { FaturaAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<FaturaAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<FaturaAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verDetalhes(item: FaturaAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function marcarPago() {
  if (!selecionado.value) return;
  try {
    await api.put(`/admin/pagamentos/${selecionado.value.id}/pagar`, {}, authHeader());
    const idx = itens.value.findIndex(i => i.id === selecionado.value?.id);
    if (idx !== -1) itens.value[idx] = { ...itens.value[idx], status: 'Pago' };
    if (selecionado.value) selecionado.value = { ...selecionado.value, status: 'Pago' };
  } catch (e: unknown) { console.error('Erro ao marcar pago:', extractApiErrorMessage(e)); }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/pagamentos/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar pagamentos:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-detail-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.qs-detail-item { display: flex; flex-direction: column; gap: 4px; }
.qs-detail-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
</style>
