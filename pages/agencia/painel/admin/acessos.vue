<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Admin · Segurança" title="Acessos" description="Registro de acessos à plataforma" />

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>
        <h3>Nenhum acesso encontrado</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Usuário</th>
              <th>IP</th>
              <th>Dispositivo</th>
              <th>Data/Hora</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.usuario }}</td>
              <td>{{ item.ip }}</td>
              <td>{{ item.dispositivo || '—' }}</td>
              <td>{{ formatDateTime(item.data) }}</td>
              <td><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:480px">
        <div class="qs-modal-header">
          <h5>Detalhes do Acesso</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-detail-grid">
            <div class="qs-detail-row"><span class="qs-detail-label">Usuário</span><span class="qs-detail-value">{{ selecionado.usuario }}</span></div>
            <div class="qs-detail-row"><span class="qs-detail-label">IP</span><span class="qs-detail-value">{{ selecionado.ip }}</span></div>
            <div class="qs-detail-row"><span class="qs-detail-label">Dispositivo</span><span class="qs-detail-value">{{ selecionado.dispositivo || '—' }}</span></div>
            <div class="qs-detail-row"><span class="qs-detail-label">Data/Hora</span><span class="qs-detail-value">{{ formatDateTime(selecionado.data) }}</span></div>
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
import type { AcessoAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<AcessoAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<AcessoAdmin | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDateTime(d: string) { return d ? new Date(d).toLocaleString('pt-BR') : '—'; }
function verDetalhes(item: AcessoAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/acessos/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar acessos:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-detail-grid { display: flex; flex-direction: column; gap: 12px; }
.qs-detail-row { display: flex; justify-content: space-between; gap: 16px; padding: 10px 0; border-bottom: 1px solid var(--qs-gray-100); }
.qs-detail-row:last-child { border-bottom: none; }
.qs-detail-label { font-size: 12px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; flex-shrink: 0; }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); text-align: right; }
</style>
