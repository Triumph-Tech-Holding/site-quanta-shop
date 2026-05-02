<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Usuários</div>
        <h1>Aniversariantes</h1>
        <p>Usuários aniversariantes do período</p>
      </div>
      <div class="qs-header-actions">
        <select v-model="filtroMes" class="qs-select" @change="carregarDados">
          <option v-for="(m, i) in meses" :key="i" :value="i + 1">{{ m }}</option>
        </select>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
        <h3>Nenhum aniversariante encontrado para este mês</h3>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Data de Nascimento</th>
              <th>E-mail</th>
              <th>Telefone</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.nome }}</td>
              <td>{{ formatDate(item.dataNascimento) }}</td>
              <td>{{ item.email || '—' }}</td>
              <td>{{ item.telefone || '—' }}</td>
              <td><button class="qs-btn-sm-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:440px">
        <div class="qs-modal-header">
          <h5>{{ selecionado.nome }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-detail-grid">
            <div class="qs-detail-row"><span class="qs-detail-label">Nascimento</span><span class="qs-detail-value">{{ formatDate(selecionado.dataNascimento) }}</span></div>
            <div class="qs-detail-row"><span class="qs-detail-label">E-mail</span><span class="qs-detail-value">{{ selecionado.email || '—' }}</span></div>
            <div class="qs-detail-row"><span class="qs-detail-label">Telefone</span><span class="qs-detail-value">{{ selecionado.telefone || '—' }}</span></div>
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
import type { AniversarianteAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<AniversarianteAdmin[]>([]);
const showModal = ref(false);
const selecionado = ref<AniversarianteAdmin | null>(null);
const filtroMes = ref(new Date().getMonth() + 1);
const meses = ['Janeiro','Fevereiro','Março','Abril','Maio','Junho','Julho','Agosto','Setembro','Outubro','Novembro','Dezembro'];
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verDetalhes(item: AniversarianteAdmin) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function carregarDados() {
  loading.value = true;
  try {
    const { data } = await api.get(`/admin/aniversariantes/listar?mes=${filtroMes.value}`, authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar aniversariantes:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
}
onMounted(async () => { agenciaStore.loadFromStorage(); await carregarDados(); });
</script>

<style scoped>
.qs-select {
  padding: 8px 12px;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  font-size: 14px;
  color: var(--qs-ink);
  background: #fff;
  outline: none;
  cursor: pointer;
}
.qs-detail-grid { display: flex; flex-direction: column; gap: 12px; }
.qs-detail-row { display: flex; justify-content: space-between; gap: 16px; padding: 10px 0; border-bottom: 1px solid var(--qs-gray-100); }
.qs-detail-row:last-child { border-bottom: none; }
.qs-detail-label { font-size: 12px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; flex-shrink: 0; }
.qs-detail-value { font-size: 14px; color: var(--qs-ink); text-align: right; }
</style>
