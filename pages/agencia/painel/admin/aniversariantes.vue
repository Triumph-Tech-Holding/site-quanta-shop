<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Aniversariantes</h1><p>Usuários aniversariantes do período</p></div>
      <div class="d-flex gap-2">
        <select v-model="filtroMes" class="form-select" style="width:auto" @change="carregarDados">
          <option v-for="(m, i) in meses" :key="i" :value="i+1">{{ m }}</option>
        </select>
      </div>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum aniversariante encontrado para este mês</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Nome</th><th>Data de Nascimento</th><th>E-mail</th><th>Telefone</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.nome }}</td>
              <td>{{ formatDate(item.dataNascimento) }}</td>
              <td>{{ item.email || '—' }}</td>
              <td>{{ item.telefone || '—' }}</td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verDetalhes(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:440px">
        <div class="ag-modal-header"><h5 class="mb-0">{{ selecionado.nome }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-2">
            <div class="col-12"><strong>Data de Nascimento:</strong> {{ formatDate(selecionado.dataNascimento) }}</div>
            <div class="col-12"><strong>E-mail:</strong> {{ selecionado.email || '—' }}</div>
            <div class="col-12"><strong>Telefone:</strong> {{ selecionado.telefone || '—' }}</div>
          </div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
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
onMounted(async () => {
  agenciaStore.loadFromStorage();
  await carregarDados();
});
</script>
