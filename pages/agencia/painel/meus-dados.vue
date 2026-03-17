<template>
  <div>
    <div class="ag-page-header">
      <h1>Meus Dados</h1>
      <p>Atualize suas informações pessoais</p>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <template v-else>
      <div v-if="dados.preCadastro" class="alert alert-warning mb-3">
        <strong>Atenção:</strong> Complete seu cadastro para ter acesso total ao sistema.
        Preencha todos os campos obrigatórios nas abas abaixo.
      </div>

      <div class="ag-card">
        <ul class="nav nav-tabs ag-tabs mb-4" role="tablist">
          <li class="nav-item"><button class="nav-link" :class="{ active: tab === 'dados' }" @click="tab = 'dados'">Meus Dados</button></li>
          <li class="nav-item"><button class="nav-link" :class="{ active: tab === 'endereco' }" @click="tab = 'endereco'">Endereço</button></li>
          <li class="nav-item"><button class="nav-link" :class="{ active: tab === 'seguranca' }" @click="tab = 'seguranca'">Segurança</button></li>
          <li class="nav-item"><button class="nav-link" :class="{ active: tab === 'foto' }" @click="tab = 'foto'">Foto de Perfil</button></li>
        </ul>

        <!-- DADOS PESSOAIS -->
        <div v-show="tab === 'dados'">
          <form @submit.prevent="salvarDados" class="row g-3">
            <div class="col-12 col-md-6 ag-form-group">
              <label>Nome completo *</label>
              <input v-model="dados.nome" type="text" class="form-control" required />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>E-mail *</label>
              <input v-model="dados.email" type="email" class="form-control" required />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>CPF *</label>
              <input v-model="dados.cpf" type="text" class="form-control" maxlength="14" placeholder="000.000.000-00" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>Celular</label>
              <input v-model="dados.celular" type="text" class="form-control" maxlength="15" placeholder="(00) 00000-0000" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>Data de nascimento</label>
              <input v-model="dados.dataNascimento" type="date" class="form-control" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>Sexo</label>
              <select v-model="dados.sexo" class="form-select">
                <option value="">Selecione</option>
                <option value="M">Masculino</option>
                <option value="F">Feminino</option>
                <option value="O">Outro</option>
              </select>
            </div>
            <div class="col-12 text-end">
              <button type="submit" class="btn btn-ag-primary" :disabled="salvando">
                <span v-if="salvando" class="spinner-border spinner-border-sm me-1" />
                {{ salvando ? 'Salvando...' : 'Salvar Dados' }}
              </button>
            </div>
          </form>
        </div>

        <!-- ENDERECO -->
        <div v-show="tab === 'endereco'">
          <form @submit.prevent="salvarEndereco" class="row g-3">
            <div class="col-12 col-md-4 ag-form-group">
              <label>CEP *</label>
              <div class="input-group">
                <input v-model="endereco.cep" type="text" class="form-control" maxlength="9" placeholder="00000-000" />
                <button type="button" class="btn btn-ag-outline" @click="buscarCep" :disabled="buscandoCep">
                  {{ buscandoCep ? '...' : 'Buscar' }}
                </button>
              </div>
            </div>
            <div class="col-12 col-md-8 ag-form-group">
              <label>Logradouro *</label>
              <input v-model="endereco.logradouro" type="text" class="form-control" required />
            </div>
            <div class="col-12 col-md-3 ag-form-group">
              <label>Número</label>
              <input v-model="endereco.numero" type="text" class="form-control" />
            </div>
            <div class="col-12 col-md-5 ag-form-group">
              <label>Complemento</label>
              <input v-model="endereco.complemento" type="text" class="form-control" />
            </div>
            <div class="col-12 col-md-4 ag-form-group">
              <label>Bairro</label>
              <input v-model="endereco.bairro" type="text" class="form-control" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>Cidade *</label>
              <input v-model="endereco.cidade" type="text" class="form-control" required />
            </div>
            <div class="col-12 col-md-2 ag-form-group">
              <label>UF *</label>
              <input v-model="endereco.uf" type="text" class="form-control" maxlength="2" required />
            </div>
            <div class="col-12 text-end">
              <button type="submit" class="btn btn-ag-primary" :disabled="salvandoEndereco">
                <span v-if="salvandoEndereco" class="spinner-border spinner-border-sm me-1" />
                {{ salvandoEndereco ? 'Salvando...' : 'Salvar Endereço' }}
              </button>
            </div>
          </form>
        </div>

        <!-- SEGURANCA -->
        <div v-show="tab === 'seguranca'">
          <form @submit.prevent="alterarSenha" class="row g-3" style="max-width:480px">
            <div class="col-12 ag-form-group">
              <label>Senha atual *</label>
              <input v-model="senhas.atual" type="password" class="form-control" required />
            </div>
            <div class="col-12 ag-form-group">
              <label>Nova senha *</label>
              <input v-model="senhas.nova" type="password" class="form-control" required minlength="6" />
            </div>
            <div class="col-12 ag-form-group">
              <label>Confirmar nova senha *</label>
              <input v-model="senhas.confirmar" type="password" class="form-control" required minlength="6" />
            </div>
            <div class="col-12 text-end">
              <button type="submit" class="btn btn-ag-primary" :disabled="salvandoSenha">
                <span v-if="salvandoSenha" class="spinner-border spinner-border-sm me-1" />
                {{ salvandoSenha ? 'Alterando...' : 'Alterar Senha' }}
              </button>
            </div>
          </form>
        </div>

        <!-- FOTO -->
        <div v-show="tab === 'foto'">
          <div class="text-center">
            <div class="mb-3">
              <img v-if="fotoPreview" :src="fotoPreview" alt="Foto de perfil" class="rounded-circle" style="width:120px;height:120px;object-fit:cover;border:3px solid #dee2e6" />
              <div v-else class="rounded-circle bg-secondary d-inline-flex align-items-center justify-content-center text-white" style="width:120px;height:120px;font-size:2.5rem">
                {{ user?.username?.charAt(0)?.toUpperCase() }}
              </div>
            </div>
            <input type="file" accept="image/*" class="form-control mb-3" style="max-width:300px;margin:0 auto" @change="onFotoChange" />
            <button class="btn btn-ag-primary" @click="uploadFoto" :disabled="!fotoFile || salvandoFoto">
              {{ salvandoFoto ? 'Enviando...' : 'Salvar foto' }}
            </button>
          </div>
        </div>
      </div>
    </template>
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
const dados = reactive<DadosUser>({
  nome: '', email: '', cpf: '', celular: '',
  dataNascimento: '', sexo: '', preCadastro: false,
});

const endereco = reactive<EnderecoUser>({
  cep: '', logradouro: '', numero: '', complemento: '',
  bairro: '', cidade: '', uf: '',
});

const senhas = reactive({ atual: '', nova: '', confirmar: '' });
const fotoPreview = ref<string | null>(null);
const fotoFile = ref<File | null>(null);

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

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
  } catch { /**/ } finally {
    buscandoCep.value = false;
  }
}

async function salvarDados() {
  salvando.value = true;
  try {
    await api.put('/user/atualizar', dados, authHeader());
    $toast?.success('Dados salvos com sucesso!');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao salvar dados.'));
  } finally {
    salvando.value = false;
  }
}

async function salvarEndereco() {
  salvandoEndereco.value = true;
  try {
    await api.put('/user/atualizarEndereco', endereco, authHeader());
    $toast?.success('Endereço salvo com sucesso!');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao salvar endereço.'));
  } finally {
    salvandoEndereco.value = false;
  }
}

async function alterarSenha() {
  if (senhas.nova !== senhas.confirmar) {
    $toast?.error('As senhas não coincidem.');
    return;
  }
  salvandoSenha.value = true;
  try {
    await api.post('/user/alterarSenha', { senhaAtual: senhas.atual, novaSenha: senhas.nova }, authHeader());
    $toast?.success('Senha alterada com sucesso!');
    senhas.atual = ''; senhas.nova = ''; senhas.confirmar = '';
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao alterar senha.'));
  } finally {
    salvandoSenha.value = false;
  }
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
    await api.post('/user/uploadFoto', fd, {
      ...authHeader(),
      headers: { ...authHeader().headers, 'Content-Type': 'multipart/form-data' },
    });
    $toast?.success('Foto atualizada com sucesso!');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao enviar foto.'));
  } finally {
    salvandoFoto.value = false;
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/user/obterDados', authHeader());
    Object.assign(dados, data);
    if (data.endereco) Object.assign(endereco, data.endereco);
  } catch { /**/ } finally {
    loading.value = false;
  }
});
</script>
