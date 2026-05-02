<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · MLM</div>
        <h1>Rede</h1>
        <p>Estrutura de rede e indicações</p>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><circle cx="12" cy="5" r="3"/><line x1="12" y1="8" x2="12" y2="12"/><circle cx="5" cy="17" r="3"/><circle cx="19" cy="17" r="3"/><line x1="12" y1="12" x2="5" y2="14"/><line x1="12" y1="12" x2="19" y2="14"/></svg>
        <h3>Nenhum registro encontrado</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Usuário</th><th>Nível</th><th>Status</th><th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.usuario }}</td>
              <td>
                <span class="qs-badge qs-badge-teal">Nível {{ item.nivel }}</span>
              </td>
              <td>
                <span class="qs-badge" :class="item.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.status }}</span>
              </td>
              <td><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:440px">
        <div class="qs-modal-header">
          <h5>Detalhes da Rede</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-detail-grid">
            <div class="qs-detail-item qs-col-full"><span class="qs-detail-label">Usuário</span><span class="qs-detail-value">{{ selecionado.usuario }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Nível</span><span class="qs-detail-value">{{ selecionado.nivel }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Status</span><span class="qs-badge" :class="selecionado.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.status }}</span></div>
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
import type { RedeAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<RedeAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<RedeAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function verDetalhes(item: RedeAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/rede/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar rede:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-detail-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.qs-col-full { grid-column: 1 / -1; }
.qs-detail-item { display: flex; flex-direction: column; gap: 4px; }
.qs-detail-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-badge-teal { background: rgba(47,119,133,.12); color: var(--qs-teal-dark); }
</style>
