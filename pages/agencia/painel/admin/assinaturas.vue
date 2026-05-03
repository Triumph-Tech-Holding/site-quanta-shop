<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Admin · Planos" title="Assinaturas" description="Assinaturas ativas na plataforma" />

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/></svg>
        <h3>Nenhuma assinatura encontrada</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Usuário</th><th>Plano</th><th>Status</th><th>Início</th><th>Fim</th><th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.usuario }}</td>
              <td>{{ item.plano }}</td>
              <td>
                <span class="qs-badge" :class="item.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.status }}</span>
              </td>
              <td>{{ formatDate(item.dataInicio) }}</td>
              <td>{{ formatDate(item.dataFim) }}</td>
              <td><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:480px">
        <div class="qs-modal-header">
          <h5>Assinatura de {{ selecionado.usuario }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-detail-grid">
            <div class="qs-detail-item"><span class="qs-detail-label">Plano</span><span class="qs-detail-value">{{ selecionado.plano }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Status</span><span class="qs-badge" :class="selecionado.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.status }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Início</span><span class="qs-detail-value">{{ formatDate(selecionado.dataInicio) }}</span></div>
            <div class="qs-detail-item"><span class="qs-detail-label">Fim</span><span class="qs-detail-value">{{ formatDate(selecionado.dataFim) }}</span></div>
          </div>
          <div class="qs-modal-actions-inline">
            <button v-if="selecionado.status !== 'Ativo'" class="qs-btn-sm-outline" @click="alterarStatus(selecionado, 'Ativo')">Ativar</button>
            <button v-if="selecionado.status === 'Ativo'" class="qs-btn-sm-warn" @click="alterarStatus(selecionado, 'Cancelado')">Cancelar</button>
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
import type { AssinaturaAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<AssinaturaAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<AssinaturaAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verDetalhes(item: AssinaturaAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function alterarStatus(item: AssinaturaAdmin, novoStatus: string) {
  try {
    await api.put(`/admin/assinaturas/${item.id}/status`, { status: novoStatus }, authHeader());
    const idx = itens.value.findIndex(a => a.id === item.id);
    if (idx !== -1) itens.value[idx] = { ...itens.value[idx], status: novoStatus };
    if (selecionado.value?.id === item.id) selecionado.value = { ...selecionado.value, status: novoStatus };
  } catch (e: unknown) { console.error('Erro ao alterar status:', extractApiErrorMessage(e)); }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/assinaturas/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar assinaturas:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-detail-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; margin-bottom: 16px; }
.qs-detail-item { display: flex; flex-direction: column; gap: 4px; }
.qs-detail-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-modal-actions-inline { display: flex; gap: 8px; flex-wrap: wrap; margin-top: 16px; }
.qs-btn-sm-warn { padding: 6px 14px; border: 1px solid #f59e0b; color: #d97706; background: #fff; border-radius: var(--qs-radius-md); font-size: 13px; font-weight: 600; cursor: pointer; }
.qs-btn-sm-warn:hover { background: #fef3c7; }
</style>
