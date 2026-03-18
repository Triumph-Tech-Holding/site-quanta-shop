<template>
  <div>
    <div class="ag-page-header">
      <h1>Contas Bancárias</h1>
      <p>Gerencie suas contas para saque</p>
    </div>

    <div class="ag-card mb-3">
      <button class="btn btn-ag-primary" @click="showForm = !showForm">
        {{ showForm ? 'Cancelar' : '+ Adicionar Conta' }}
      </button>
      <div v-if="showForm" class="mt-4">
        <form @submit.prevent="salvarConta" class="row g-3" style="max-width:600px">
          <div class="col-12 col-md-6 ag-form-group">
            <label>Banco *</label>
            <select v-model="form.banco" class="form-select" required>
              <option value="">Selecione o banco</option>
              <option v-for="b in bancos" :key="b.code" :value="b.name">{{ b.code }} - {{ b.name }}</option>
            </select>
          </div>
          <div class="col-12 col-md-3 ag-form-group">
            <label>Tipo de conta *</label>
            <select v-model="form.tipoConta" class="form-select" required>
              <option value="">Tipo</option>
              <option value="CC">Conta Corrente</option>
              <option value="CP">Conta Poupança</option>
            </select>
          </div>
          <div class="col-12 col-md-3 ag-form-group">
            <label>Tipo de chave PIX</label>
            <select v-model="form.tipoChavePix" class="form-select">
              <option value="">Nenhuma</option>
              <option value="CPF">CPF</option>
              <option value="CNPJ">CNPJ</option>
              <option value="EMAIL">E-mail</option>
              <option value="CELULAR">Celular</option>
              <option value="ALEATORIA">Chave aleatória</option>
            </select>
          </div>
          <div class="col-12 col-md-4 ag-form-group">
            <label>Agência *</label>
            <input v-model="form.agencia" type="text" class="form-control" required />
          </div>
          <div class="col-12 col-md-4 ag-form-group">
            <label>Conta *</label>
            <input v-model="form.conta" type="text" class="form-control" required />
          </div>
          <div class="col-12 col-md-4 ag-form-group" v-if="form.tipoChavePix">
            <label>Chave PIX</label>
            <input v-model="form.chavePix" type="text" class="form-control" />
          </div>
          <div class="col-12">
            <button type="submit" class="btn btn-ag-primary" :disabled="salvando">
              <span v-if="salvando" class="spinner-border spinner-border-sm me-1" />
              {{ salvando ? 'Salvando...' : 'Salvar Conta' }}
            </button>
          </div>
        </form>
      </div>
    </div>

    <div class="ag-card">
      <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
      <div v-else-if="contas.length === 0" class="ag-empty-state">
        <h5>Nenhuma conta cadastrada</h5>
        <p>Adicione uma conta bancária para realizar saques.</p>
      </div>
      <div v-else class="row g-3">
        <div v-for="(c, i) in contas" :key="i" class="col-12 col-md-6">
          <div class="p-3 border rounded bg-light position-relative">
            <div class="fw-bold mb-1">{{ c.banco }}</div>
            <div class="text-muted" style="font-size:.875rem">
              Ag: {{ c.agencia }} | Conta: {{ c.conta }} — {{ c.tipoConta === 'CP' ? 'Poupança' : 'Corrente' }}
            </div>
            <div v-if="c.chavePix" class="text-muted mt-1" style="font-size:.8rem">
              PIX ({{ c.tipoChavePix }}): {{ c.chavePix }}
            </div>
            <div class="mt-2">
              <span class="badge-ag" :class="c.principal ? 'badge-ag-success' : 'badge-ag-secondary'">
                {{ c.principal ? 'Principal' : 'Secundária' }}
              </span>
            </div>
            <button class="btn btn-sm text-danger p-0 mt-2" @click="excluirConta(c.id)" style="font-size:.8rem">
              Remover
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp();
const loading = ref(true);
const salvando = ref(false);
const showForm = ref(false);
import type { ContaBancaria } from "~/types/agencia";
const contas = ref<ContaBancaria[]>([]);
const form = reactive({ banco: '', tipoConta: '', agencia: '', conta: '', tipoChavePix: '', chavePix: '' });
const bancos = [
  { code: '001', name: 'Banco do Brasil' }, { code: '033', name: 'Santander' },
  { code: '104', name: 'Caixa Econômica' }, { code: '237', name: 'Bradesco' },
  { code: '341', name: 'Itaú' }, { code: '260', name: 'Nubank' },
  { code: '336', name: 'C6 Bank' }, { code: '077', name: 'Inter' },
  { code: '290', name: 'PagBank' }, { code: '655', name: 'Votorantim' },
];
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
async function loadContas() {
  try {
    const { data } = await api.get('/UsuarioBanco/listarContasCadastradas', authHeader());
    const lista = Array.isArray(data) ? data : [];
    contas.value = lista.map((c: Record<string, unknown>) => ({
      id: c.IdUsuarioBanco ?? c.idUsuarioBanco,
      banco: (c.Banco as Record<string, unknown>)?.Nome ?? (c.banco as Record<string, unknown>)?.nome ?? c.banco ?? '—',
      agencia: c.Agencia ?? c.agencia ?? '',
      conta: `${c.Conta ?? c.conta ?? ''}${c.DigitoConta ? '-' + c.DigitoConta : ''}`,
      tipoConta: c.TipoConta ?? c.tipoConta ?? 'CC',
      chavePix: c.ChavePix ?? c.chavePix ?? '',
      tipoChavePix: c.TipoChavePix ?? c.tipoChavePix ?? '',
      principal: c.NomeConta === 'Conta Principal' || c.principal,
    }));
  } catch(e: unknown) { console.error("Erro ao carregar contas:", extractApiErrorMessage(e)); } finally { loading.value = false; }
}
async function salvarConta() {
  salvando.value = true;
  try {
    const bancoEncontrado = bancos.find(b => b.name === form.banco);
    await api.post('/UsuarioBanco/cadastrarUsuarioBanco', {
      Agencia: form.agencia,
      Conta: form.conta,
      NomeConta: form.banco,
      Banco: bancoEncontrado ? { Nome: bancoEncontrado.name, Febraban: parseInt(bancoEncontrado.code) } : { Nome: form.banco },
    }, authHeader());
    $toast?.success('Conta cadastrada com sucesso!');
    showForm.value = false;
    Object.assign(form, { banco: '', tipoConta: '', agencia: '', conta: '', tipoChavePix: '', chavePix: '' });
    await loadContas();
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao salvar conta.'));
  } finally { salvando.value = false; }
}
async function excluirConta(id: number) {
  if (!confirm('Remover esta conta?')) return;
  try {
    await api.delete(`/UsuarioBanco/ExcluirContaBancaria/${id}`, authHeader());
    $toast?.success('Conta removida.');
    await loadContas();
  } catch(e: unknown) { $toast?.error(extractApiErrorMessage(e, 'Erro ao remover conta.')); }
}
onMounted(async () => { agenciaStore.loadFromStorage(); await loadContas(); });
</script>
