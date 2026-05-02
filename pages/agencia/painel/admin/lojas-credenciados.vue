<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Credenciamento</div>
        <h1>Lojas Credenciadas</h1>
        <p>Estabelecimentos ativos na plataforma</p>
      </div>
      <div class="qs-header-actions">
        <div class="qs-search-field">
          <svg class="qs-search-icon" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
          <input v-model="busca" type="text" class="qs-search-input" placeholder="Buscar por nome ou CNPJ..." />
        </div>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itensFiltrados.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>
        <h3>Nenhuma loja encontrada</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead><tr><th>Nome Fantasia</th><th>CNPJ</th><th>Cidade</th><th>Estado</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itensFiltrados" :key="item.id">
              <td class="qs-cell-bold">{{ item.nomeFantasia }}</td>
              <td>{{ item.cnpj }}</td>
              <td>{{ item.cidade }}</td>
              <td>{{ item.estado }}</td>
              <td><span class="qs-badge" :class="item.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.status }}</span></td>
              <td><button class="qs-btn-sm-outline" @click="verLoja(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:560px">
        <div class="qs-modal-header">
          <h5>{{ selecionado.nomeFantasia }}</h5>
          <button class="qs-modal-close" @click="fecharModal"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-info-grid">
            <div class="qs-info-item"><span class="qs-info-label">CNPJ</span><span class="qs-info-value">{{ selecionado.cnpj }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Status</span><span class="qs-badge" :class="selecionado.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.status }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Cidade</span><span class="qs-info-value">{{ selecionado.cidade }}</span></div>
            <div class="qs-info-item"><span class="qs-info-label">Estado</span><span class="qs-info-value">{{ selecionado.estado }}</span></div>
          </div>
          <div class="qs-modal-btn-group" style="margin-top:20px">
            <button class="qs-btn-sm-outline" @click="alterarStatus(selecionado, 'Ativo')">Ativar</button>
            <button class="qs-btn-sm-warn" @click="alterarStatus(selecionado, 'Inativo')">Desativar</button>
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
import type { LojaCredenciadaAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<LojaCredenciadaAdmin[]>([]);
const showModal = ref(false);
const busca = ref('');
const selecionado = ref<LojaCredenciadaAdmin | null>(null);
const itensFiltrados = computed(() => {
  if (!busca.value) return itens.value;
  const b = busca.value.toLowerCase();
  return itens.value.filter(i => i.nomeFantasia?.toLowerCase().includes(b) || i.cnpj?.includes(b));
});
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function verLoja(item: LojaCredenciadaAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function alterarStatus(item: LojaCredenciadaAdmin, novoStatus: string) {
  try {
    await api.put(`/admin/lojas/${item.id}/status`, { status: novoStatus }, authHeader());
    const idx = itens.value.findIndex(l => l.id === item.id);
    if (idx !== -1) itens.value[idx] = { ...itens.value[idx], status: novoStatus };
    if (selecionado.value?.id === item.id) selecionado.value = { ...selecionado.value, status: novoStatus };
  } catch (e: unknown) { console.error('Erro ao alterar status:', extractApiErrorMessage(e)); }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/lojas/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar lojas:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.qs-search-field { position: relative; }
.qs-search-icon { position: absolute; left: 10px; top: 50%; transform: translateY(-50%); color: var(--qs-gray-400); pointer-events: none; }
.qs-search-input { padding: 8px 12px 8px 32px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 14px; outline: none; min-width: 240px; }
.qs-search-input:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.qs-info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.qs-info-item { display: flex; flex-direction: column; gap: 4px; }
.qs-info-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-info-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }
.qs-modal-btn-group { display: flex; gap: 10px; }
.qs-btn-sm-warn { padding: 6px 14px; border: 1px solid #f59e0b; color: #d97706; background: #fff; border-radius: var(--qs-radius-md); font-size: 13px; font-weight: 600; cursor: pointer; transition: background 0.15s; }
.qs-btn-sm-warn:hover { background: #fef3c7; }
</style>
