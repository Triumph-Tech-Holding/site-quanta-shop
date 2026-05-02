<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Transações</div>
        <h1>Compras</h1>
        <p>Histórico de compras da plataforma</p>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>
        <h3>Nenhuma compra encontrada</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Usuário</th><th>Loja</th><th>Valor</th><th>Cashback</th><th>Status</th><th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.usuario }}</td>
              <td>{{ item.loja }}</td>
              <td>{{ formatCurrency(item.valor) }}</td>
              <td class="qs-cell-accent">{{ formatCurrency(item.cashback) }}</td>
              <td>
                <span class="qs-badge" :class="item.status === 'Aprovado' ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.status }}</span>
              </td>
              <td><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:520px">
        <div class="qs-modal-header">
          <h5>Detalhes da Compra #{{ selecionado.id }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-detail-grid">
            <div class="qs-detail-item"><span class="qs-detail-label">Usuário</span><span class="qs-detail-value">{{ selecionado.usuario }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Loja</span><span class="qs-detail-value">{{ selecionado.loja }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Valor</span><span class="qs-detail-value">{{ formatCurrency(selecionado.valor) }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Cashback</span><span class="qs-detail-value qs-cell-accent">{{ formatCurrency(selecionado.cashback) }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Status</span><span class="qs-badge" :class="selecionado.status === 'Aprovado' ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.status }}</span></div>
          </div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Fechar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { CompraAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<CompraAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<CompraAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function verDetalhes(item: CompraAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/compras/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar compras:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-detail-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.qs-detail-item { display: flex; flex-direction: column; gap: 4px; }
.qs-detail-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-cell-accent { color: var(--qs-teal); font-weight: 600; }
</style>
