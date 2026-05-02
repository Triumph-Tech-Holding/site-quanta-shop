<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">Conta</div>
            <h1>Meus Dados</h1>
            <p>Atualize suas informações pessoais, endereço, senha e foto de perfil.</p>
          </div>
        </div>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>

          <div v-if="dados.preCadastro" class="md-alert">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            <span><strong>Atenção:</strong> Complete seu cadastro para ter acesso total ao sistema. Preencha todos os campos obrigatórios nas abas abaixo.</span>
          </div>

          <div class="qs-card-section md-card">

            <!-- Tabs -->
            <div class="md-tab-bar">
              <button class="md-tab-btn" :class="{ 'md-tab-btn--ativo': tab === 'dados' }" @click="tab = 'dados'">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z"/></svg>
                Dados
              </button>
              <button class="md-tab-btn" :class="{ 'md-tab-btn--ativo': tab === 'endereco' }" @click="tab = 'endereco'">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z"/></svg>
                Endereço
              </button>
              <button class="md-tab-btn" :class="{ 'md-tab-btn--ativo': tab === 'seguranca' }" @click="tab = 'seguranca'">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M18 8h-1V6c0-2.76-2.24-5-5-5S7 3.24 7 6v2H6c-1.1 0-2 .9-2 2v10c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V10c0-1.1-.9-2-2-2zm-6 9c-1.1 0-2-.9-2-2s.9-2 2-2 2 .9 2 2-.9 2-2 2zm3.1-9H8.9V6c0-1.71 1.39-3.1 3.1-3.1 1.71 0 3.1 1.39 3.1 3.1v2z"/></svg>
                Segurança
              </button>
              <button class="md-tab-btn" :class="{ 'md-tab-btn--ativo': tab === 'foto' }" @click="tab = 'foto'">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z"/></svg>
                Foto
              </button>
            </div>

            <!-- DADOS PESSOAIS -->
            <div v-show="tab === 'dados'" class="md-form-section">
              <form @submit.prevent="salvarDados" class="md-form-grid">
                <div class="md-field md-field--half">
                  <label class="qs-label">Nome completo *</label>
                  <input v-model="dados.nome" type="text" class="qs-input" required />
                </div>
                <div class="md-field md-field--half">
                  <label class="qs-label">E-mail *</label>
                  <input v-model="dados.email" type="email" class="qs-input" required />
                </div>
                <div class="md-field md-field--half">
                  <label class="qs-label">CPF *</label>
                  <input v-model="dados.cpf" type="text" class="qs-input" maxlength="14" placeholder="000.000.000-00" />
                </div>
                <div class="md-field md-field--half">
                  <label class="qs-label">Celular</label>
                  <input v-model="dados.celular" type="text" class="qs-input" maxlength="15" placeholder="(00) 00000-0000" />
                </div>
                <div class="md-field md-field--half">
                  <label class="qs-label">Data de nascimento</label>
                  <input v-model="dados.dataNascimento" type="date" class="qs-input" />
                </div>
                <div class="md-field md-field--half">
                  <label class="qs-label">Sexo</label>
                  <select v-model="dados.sexo" class="qs-input">
                    <option value="">Selecione</option>
                    <option value="M">Masculino</option>
                    <option value="F">Feminino</option>
                    <option value="O">Outro</option>
                  </select>
                </div>
                <div class="md-field md-field--full md-actions">
                  <button type="submit" class="qs-btn-primary" :disabled="salvando">
                    <svg v-if="salvando" class="md-spinner-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 4V2A10 10 0 0 0 2 12h2a8 8 0 0 1 8-8z"/></svg>
                    {{ salvando ? 'Salvando...' : 'Salvar Dados' }}
                  </button>
                </div>
              </form>
            </div>

            <!-- ENDEREÇO -->
            <div v-show="tab === 'endereco'" class="md-form-section">
              <form @submit.prevent="salvarEndereco" class="md-form-grid">
                <div class="md-field" style="flex:0 0 200px;">
                  <label class="qs-label">CEP *</label>
                  <div class="md-cep-row">
                    <input v-model="endereco.cep" type="text" class="qs-input" maxlength="9" placeholder="00000-000" />
                    <button type="button" class="qs-btn-secondary md-cep-btn" @click="buscarCep" :disabled="buscandoCep">
                      {{ buscandoCep ? '...' : 'Buscar' }}
                    </button>
                  </div>
                </div>
                <div class="md-field" style="flex:1;min-width:200px;">
                  <label class="qs-label">Logradouro *</label>
                  <input v-model="endereco.logradouro" type="text" class="qs-input" required />
                </div>
                <div class="md-field" style="flex:0 0 120px;">
                  <label class="qs-label">Número</label>
                  <input v-model="endereco.numero" type="text" class="qs-input" />
                </div>
                <div class="md-field md-field--half">
                  <label class="qs-label">Complemento</label>
                  <input v-model="endereco.complemento" type="text" class="qs-input" />
                </div>
                <div class="md-field md-field--half">
                  <label class="qs-label">Bairro</label>
                  <input v-model="endereco.bairro" type="text" class="qs-input" />
                </div>
                <div class="md-field" style="flex:1;min-width:180px;">
                  <label class="qs-label">Cidade *</label>
                  <input v-model="endereco.cidade" type="text" class="qs-input" required />
                </div>
                <div class="md-field" style="flex:0 0 80px;">
                  <label class="qs-label">UF *</label>
                  <input v-model="endereco.uf" type="text" class="qs-input" maxlength="2" required />
                </div>
                <div class="md-field md-field--full md-actions">
                  <button type="submit" class="qs-btn-primary" :disabled="salvandoEndereco">
                    {{ salvandoEndereco ? 'Salvando...' : 'Salvar Endereço' }}
                  </button>
                </div>
              </form>
            </div>

            <!-- SEGURANÇA -->
            <div v-show="tab === 'seguranca'" class="md-form-section">
              <form @submit.prevent="alterarSenha" class="md-form-narrow">
                <div class="md-field">
                  <label class="qs-label">Senha atual *</label>
                  <input v-model="senhas.atual" type="password" class="qs-input" required autocomplete="current-password" />
                </div>
                <div class="md-field">
                  <label class="qs-label">Nova senha *</label>
                  <input v-model="senhas.nova" type="password" class="qs-input" required minlength="6" autocomplete="new-password" />
                </div>
                <div class="md-field">
                  <label class="qs-label">Confirmar nova senha *</label>
                  <input v-model="senhas.confirmar" type="password" class="qs-input" required minlength="6" autocomplete="new-password" />
                </div>
                <div class="md-actions">
                  <button type="submit" class="qs-btn-primary" :disabled="salvandoSenha">
                    {{ salvandoSenha ? 'Alterando...' : 'Alterar Senha' }}
                  </button>
                </div>
              </form>
            </div>

            <!-- FOTO -->
            <div v-show="tab === 'foto'" class="md-form-section md-foto-section">
              <div class="md-foto-preview">
                <img v-if="fotoPreview" :src="fotoPreview" alt="Foto de perfil" class="md-foto-img" />
                <div v-else class="md-foto-placeholder">
                  {{ user?.username?.charAt(0)?.toUpperCase() }}
                </div>
              </div>
              <div class="md-foto-actions">
                <label class="qs-label">Selecionar foto</label>
                <input type="file" accept="image/*" class="qs-input" style="padding:.5rem;" @change="onFotoChange" />
                <button class="qs-btn-primary" style="margin-top:.75rem;" @click="uploadFoto" :disabled="!fotoFile || salvandoFoto">
                  {{ salvandoFoto ? 'Enviando...' : 'Salvar foto' }}
                </button>
              </div>
            </div>

          </div>
        </template>

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

const user = computed(() => agenciaStore.dadosUser);
const tab = ref('dados');
const loading = ref(true);
const salvando = ref(false);
const salvandoEndereco = ref(false);
const salvandoSenha = ref(false);
const salvandoFoto = ref(false);
const buscandoCep = ref(false);

interface DadosUser { nome: string; email: string; cpf: string; celular: string; dataNascimento: string; sexo: string; preCadastro: boolean; [key: string]: unknown; }
interface EnderecoUser { cep: string; logradouro: string; numero: string; complemento: string; bairro: string; cidade: string; uf: string; [key: string]: unknown; }
const dados = reactive<DadosUser>({ nome: '', email: '', cpf: '', celular: '', dataNascimento: '', sexo: '', preCadastro: false });
const endereco = reactive<EnderecoUser>({ cep: '', logradouro: '', numero: '', complemento: '', bairro: '', cidade: '', uf: '' });
const senhas = reactive({ atual: '', nova: '', confirmar: '' });
const fotoPreview = ref<string | null>(null);
const fotoFile = ref<File | null>(null);

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }

async function buscarCep() {
  const cep = endereco.cep.replace(/\D/g, '');
  if (cep.length !== 8) return;
  buscandoCep.value = true;
  try {
    const res = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
    const data = await res.json();
    if (!data.erro) {
      endereco.logradouro = data.logradouro;
      endereco.bairro = data.bairro;
      endereco.cidade = data.localidade;
      endereco.uf = data.uf;
    }
  } catch { /**/ } finally { buscandoCep.value = false; }
}

async function salvarDados() {
  salvando.value = true;
  try {
    await api.put('/user/atualizar', dados, authHeader());
    $toast?.success('Dados salvos com sucesso!');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao salvar dados.'));
  } finally { salvando.value = false; }
}

async function salvarEndereco() {
  salvandoEndereco.value = true;
  try {
    await api.put('/user/atualizarEndereco', endereco, authHeader());
    $toast?.success('Endereço salvo com sucesso!');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao salvar endereço.'));
  } finally { salvandoEndereco.value = false; }
}

async function alterarSenha() {
  if (senhas.nova !== senhas.confirmar) { $toast?.error('As senhas não coincidem.'); return; }
  salvandoSenha.value = true;
  try {
    await api.post('/user/alterarSenha', { senhaAtual: senhas.atual, novaSenha: senhas.nova }, authHeader());
    $toast?.success('Senha alterada com sucesso!');
    senhas.atual = ''; senhas.nova = ''; senhas.confirmar = '';
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao alterar senha.'));
  } finally { salvandoSenha.value = false; }
}

function onFotoChange(e: Event) {
  const file = (e.target as HTMLInputElement).files?.[0];
  if (!file) return;
  fotoFile.value = file;
  fotoPreview.value = URL.createObjectURL(file);
}

async function uploadFoto() {
  if (!fotoFile.value) return;
  salvandoFoto.value = true;
  try {
    const fd = new FormData();
    fd.append('foto', fotoFile.value);
    await api.post('/user/uploadFoto', fd, { ...authHeader(), headers: { ...authHeader().headers, 'Content-Type': 'multipart/form-data' } });
    $toast?.success('Foto atualizada com sucesso!');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao enviar foto.'));
  } finally { salvandoFoto.value = false; }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/user/obterDados', authHeader());
    Object.assign(dados, data);
    if (data.endereco) Object.assign(endereco, data.endereco);
  } catch { /**/ } finally { loading.value = false; }
});
</script>

<style scoped>
/* Alert */
.md-alert {
  display: flex;
  align-items: flex-start;
  gap: .625rem;
  background: #fef9c3;
  border-left: 4px solid #ca8a04;
  border-radius: 0 var(--qs-radius-md, 12px) var(--qs-radius-md, 12px) 0;
  padding: .875rem 1rem;
  margin-bottom: 1.25rem;
  font-size: .875rem;
  color: #92400e;
  line-height: 1.5;
}
.md-alert svg { width: 18px; height: 18px; flex-shrink: 0; color: #ca8a04; margin-top: 1px; }

/* Card */
.md-card { background: #fff; }

/* Tabs */
.md-tab-bar {
  display: flex;
  border-bottom: 2px solid var(--qs-gray-100, #f5f5f7);
  margin-bottom: 1.5rem;
  gap: 0;
  overflow-x: auto;
}
.md-tab-btn {
  display: inline-flex;
  align-items: center;
  gap: .375rem;
  padding: .625rem 1.125rem;
  border: none;
  border-bottom: 2px solid transparent;
  background: transparent;
  font-size: .875rem;
  font-weight: 500;
  color: var(--qs-gray-500, #6b7280);
  cursor: pointer;
  white-space: nowrap;
  margin-bottom: -2px;
  transition: all .15s;
}
.md-tab-btn svg { width: 15px; height: 15px; }
.md-tab-btn--ativo {
  color: var(--qs-teal, #2F7785);
  border-bottom-color: var(--qs-teal, #2F7785);
  font-weight: 700;
}
.md-tab-btn:hover:not(.md-tab-btn--ativo) { color: var(--qs-ink, #1d1d1f); }

/* Form */
.md-form-section { }
.md-form-grid { display: flex; flex-wrap: wrap; gap: 1rem; }
.md-form-narrow { display: flex; flex-direction: column; gap: 1rem; max-width: 480px; }
.md-field { display: flex; flex-direction: column; gap: .35rem; }
.md-field--half { flex: 1; min-width: 200px; }
.md-field--full { width: 100%; }
.md-actions { display: flex; justify-content: flex-end; width: 100%; }
.md-cep-row { display: flex; gap: .5rem; }
.md-cep-btn { flex-shrink: 0; padding: .625rem .875rem; font-size: .8125rem; }

/* Foto */
.md-foto-section { display: flex; align-items: flex-start; gap: 2rem; flex-wrap: wrap; }
.md-foto-preview { flex-shrink: 0; }
.md-foto-img {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  object-fit: cover;
  border: 3px solid var(--qs-gray-200, #e5e7eb);
  box-shadow: var(--qs-shadow-sm);
}
.md-foto-placeholder {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  background: var(--qs-teal, #2F7785);
  color: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2.5rem;
  font-weight: 700;
}
.md-foto-actions { display: flex; flex-direction: column; gap: .5rem; flex: 1; min-width: 200px; max-width: 320px; }

/* Spinner icon */
.md-spinner-icon {
  width: 14px;
  height: 14px;
  animation: md-spin 1s linear infinite;
}
@keyframes md-spin { to { transform: rotate(360deg); } }

/* Shared */
.qs-label {
  font-size: .75rem;
  font-weight: 600;
  color: var(--qs-gray-700, #374151);
  text-transform: uppercase;
  letter-spacing: .04em;
}
.qs-input {
  width: 100%;
  padding: .625rem .875rem;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  font-size: .875rem;
  color: var(--qs-ink, #1d1d1f);
  background: #fff;
  transition: border-color .15s;
  box-sizing: border-box;
}
.qs-input:focus { outline: none; border-color: var(--qs-teal, #2F7785); }
</style>
