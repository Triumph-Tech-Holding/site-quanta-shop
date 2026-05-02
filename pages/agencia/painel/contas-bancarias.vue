<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">Financeiro</div>
            <h1>Contas Bancárias</h1>
            <p>Gerencie suas contas cadastradas para realizar saques.</p>
          </div>
          <button class="qs-btn-primary cb-btn-add" @click="showForm = !showForm">
            <svg v-if="!showForm" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z"/></svg>
            <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>
            {{ showForm ? 'Cancelar' : 'Adicionar conta' }}
          </button>
        </div>

        <!-- Form -->
        <div v-if="showForm" class="qs-card-section cb-form-card">
          <div class="qs-section-title" style="margin-bottom:1.25rem;">Nova conta bancária</div>
          <form @submit.prevent="salvarConta" class="cb-form-grid">
            <div class="cb-field cb-field--half">
              <label class="qs-label">Banco *</label>
              <select v-model="form.banco" class="qs-input" required>
                <option value="">Selecione o banco</option>
                <option v-for="b in bancos" :key="b.code" :value="b.name">{{ b.code }} — {{ b.name }}</option>
              </select>
            </div>
            <div class="cb-field" style="flex:0 0 160px;">
              <label class="qs-label">Tipo de conta *</label>
              <select v-model="form.tipoConta" class="qs-input" required>
                <option value="">Tipo</option>
                <option value="CC">Conta Corrente</option>
                <option value="CP">Conta Poupança</option>
              </select>
            </div>
            <div class="cb-field" style="flex:0 0 200px;">
              <label class="qs-label">Tipo de chave PIX</label>
              <select v-model="form.tipoChavePix" class="qs-input">
                <option value="">Nenhuma</option>
                <option value="CPF">CPF</option>
                <option value="CNPJ">CNPJ</option>
                <option value="EMAIL">E-mail</option>
                <option value="CELULAR">Celular</option>
                <option value="ALEATORIA">Chave aleatória</option>
              </select>
            </div>
            <div class="cb-field" style="flex:0 0 140px;">
              <label class="qs-label">Agência *</label>
              <input v-model="form.agencia" type="text" class="qs-input" required />
            </div>
            <div class="cb-field" style="flex:0 0 160px;">
              <label class="qs-label">Conta *</label>
              <input v-model="form.conta" type="text" class="qs-input" required />
            </div>
            <div v-if="form.tipoChavePix" class="cb-field cb-field--half">
              <label class="qs-label">Chave PIX</label>
              <input v-model="form.chavePix" type="text" class="qs-input" />
            </div>
            <div class="cb-field cb-field--full cb-actions">
              <button type="submit" class="qs-btn-primary" :disabled="salvando">
                <svg v-if="salvando" class="cb-spin" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 4V2A10 10 0 0 0 2 12h2a8 8 0 0 1 8-8z"/></svg>
                {{ salvando ? 'Salvando...' : 'Salvar Conta' }}
              </button>
            </div>
          </form>
        </div>

        <!-- Lista -->
        <div class="qs-card-section cb-list-card">
          <div class="qs-section-title" style="margin-bottom:1rem;">Minhas contas</div>
          <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>
          <div v-else-if="contas.length === 0" class="ag-empty-state">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="var(--qs-gray-200,#e5e7eb)"><path d="M4 6h16v2H4zm2-4h12v2H6zm14 8H4c-1.1 0-2 .9-2 2v8c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2v-8c0-1.1-.9-2-2-2zm0 10H4v-8h16v8z"/></svg>
            <h5>Nenhuma conta cadastrada</h5>
            <p>Adicione uma conta bancária para realizar saques.</p>
          </div>
          <div v-else class="cb-cards-grid">
            <div v-for="(c, i) in contas" :key="i" class="cb-conta-card">
              <div class="cb-conta-card__top">
                <div class="cb-conta-card__banco">{{ c.banco }}</div>
                <span class="qs-badge" :class="c.principal ? 'qs-badge--success' : 'qs-badge--secondary'">
                  {{ c.principal ? 'Principal' : 'Secundária' }}
                </span>
              </div>
              <div class="cb-conta-card__info">
                <div class="cb-conta-card__detalhe">
                  <span class="cb-conta-card__key">Agência</span>
                  <span class="cb-conta-card__val">{{ c.agencia }}</span>
                </div>
                <div class="cb-conta-card__detalhe">
                  <span class="cb-conta-card__key">Conta</span>
                  <span class="cb-conta-card__val">{{ c.conta }}</span>
                </div>
                <div class="cb-conta-card__detalhe">
                  <span class="cb-conta-card__key">Tipo</span>
                  <span class="cb-conta-card__val">{{ c.tipoConta === 'CP' ? 'Poupança' : 'Corrente' }}</span>
                </div>
                <div v-if="c.chavePix" class="cb-conta-card__detalhe">
                  <span class="cb-conta-card__key">PIX ({{ c.tipoChavePix }})</span>
                  <span class="cb-conta-card__val">{{ c.chavePix }}</span>
                </div>
              </div>
              <button class="cb-remove-btn" @click="excluirConta(c.id)">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"/></svg>
                Remover
              </button>
            </div>
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
      Agencia: form.agencia, Conta: form.conta, NomeConta: form.banco,
      Banco: bancoEncontrado ? { Nome: bancoEncontrado.name, Febraban: parseInt(bancoEncontrado.code) } : { Nome: form.banco },
    }, authHeader());
    $toast?.success('Conta cadastrada com sucesso!');
    showForm.value = false;
    Object.assign(form, { banco: '', tipoConta: '', agencia: '', conta: '', tipoChavePix: '', chavePix: '' });
    await loadContas();
  } catch (e: unknown) { $toast?.error(extractApiErrorMessage(e, 'Erro ao salvar conta.')); } finally { salvando.value = false; }
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

<style scoped>
.cb-btn-add { display: inline-flex; align-items: center; gap: .4rem; flex-shrink: 0; }
.cb-btn-add svg { width: 16px; height: 16px; }

.cb-form-card { background: #fff; }
.cb-form-grid { display: flex; flex-wrap: wrap; gap: 1rem; }
.cb-field { display: flex; flex-direction: column; gap: .35rem; }
.cb-field--half { flex: 1; min-width: 200px; }
.cb-field--full { width: 100%; }
.cb-actions { justify-content: flex-start; }
.cb-spin { width: 14px; height: 14px; animation: cb-spin 1s linear infinite; }
@keyframes cb-spin { to { transform: rotate(360deg); } }

.cb-list-card { background: #fff; }
.cb-cards-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 1rem; }
.cb-conta-card {
  border: 1.5px solid var(--qs-gray-200,#e5e7eb);
  border-radius: var(--qs-radius-md,12px);
  padding: 1.125rem 1.25rem;
  display: flex;
  flex-direction: column;
  gap: .75rem;
  transition: all .2s;
}
.cb-conta-card:hover { box-shadow: var(--qs-shadow-sm); border-color: var(--qs-teal,#2F7785); }
.cb-conta-card__top { display: flex; align-items: center; justify-content: space-between; gap: .5rem; }
.cb-conta-card__banco { font-size: .9375rem; font-weight: 700; color: var(--qs-teal-dark,#225F6B); }
.cb-conta-card__info { display: flex; flex-direction: column; gap: .35rem; }
.cb-conta-card__detalhe { display: flex; gap: .5rem; font-size: .8125rem; }
.cb-conta-card__key { color: var(--qs-gray-400,#9ca3af); min-width: 70px; flex-shrink: 0; }
.cb-conta-card__val { color: var(--qs-gray-700,#374151); font-weight: 500; }
.cb-remove-btn {
  display: inline-flex;
  align-items: center;
  gap: .3rem;
  background: none;
  border: none;
  color: var(--qs-danger,#dc2626);
  font-size: .75rem;
  font-weight: 600;
  cursor: pointer;
  padding: 0;
  text-transform: uppercase;
  letter-spacing: .04em;
}
.cb-remove-btn svg { width: 14px; height: 14px; }
.cb-remove-btn:hover { opacity: .75; }

.qs-badge { display: inline-flex; padding: .2rem .55rem; border-radius: var(--qs-radius-pill,999px); font-size: .6875rem; font-weight: 700; text-transform: uppercase; white-space: nowrap; }
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--secondary { background: var(--qs-gray-100,#f5f5f7); color: var(--qs-gray-500,#6b7280); }

.qs-label { font-size: .75rem; font-weight: 600; color: var(--qs-gray-700,#374151); text-transform: uppercase; letter-spacing: .04em; }
.qs-input { width: 100%; padding: .625rem .875rem; border: 1.5px solid var(--qs-gray-200,#e5e7eb); border-radius: var(--qs-radius-md,12px); font-size: .875rem; color: var(--qs-ink,#1d1d1f); background: #fff; transition: border-color .15s; box-sizing: border-box; }
.qs-input:focus { outline: none; border-color: var(--qs-teal,#2F7785); }
</style>
