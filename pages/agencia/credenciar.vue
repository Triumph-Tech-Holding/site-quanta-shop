<template>
  <div class="credenciar-page">
    <div class="credenciar-container">
      <div class="text-center mb-4">
        <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" style="height:48px" />
        <h2 class="mt-3 mb-1 credenciar-titulo">QUERO ME TORNAR UM PARCEIRO LOCAL</h2>
        <p class="credenciar-subtitulo">BEM-VINDO AO PROCESSO DE CREDENCIAMENTO DE UM PARCEIRO LOCAL JUNTO AO QUANTA SHOP.</p>
        <p class="text-muted" style="font-size:.9rem; max-width:740px; margin:0 auto">
          O credenciamento é o primeiro passo para estabelecer uma colaboração bem-sucedida entre sua empresa e a nossa.
          Ao se tornar um parceiro credenciado, você terá acesso a uma variedade de oportunidades e benefícios exclusivos.
        </p>
      </div>

      <div v-if="step === 1" class="row g-4 justify-content-center mt-2">
        <div class="col-12 col-md-5">
          <button class="credenciar-tipo-card w-100" @click="selecionarTipo('pf')">
            <span class="credenciar-tipo-icon">👤</span>
            <span>Sou pessoa física</span>
          </button>
        </div>
        <div class="col-12 col-md-5">
          <button class="credenciar-tipo-card w-100" @click="selecionarTipo('pj')">
            <span class="credenciar-tipo-icon">🏢</span>
            <span>Sou pessoa jurídica</span>
          </button>
        </div>
      </div>

      <div v-if="step === 2 && tipo === 'pj'" class="credenciar-card mt-2">
        <p class="text-muted mb-3" style="font-size:.9rem">
          Para começar, informe seu CNPJ! Vamos consultar os dados da sua empresa na Receita Federal.
        </p>
        <div class="ag-form-group">
          <label>CNPJ</label>
          <input
            v-model="cnpjInput"
            type="text"
            class="form-control"
            placeholder="00.000.000/0000-00"
            maxlength="18"
            :disabled="consultando"
            @input="mascararCnpj"
            :class="{ 'is-invalid': cnpjErro, 'is-valid': cnpjInput.length === 18 }"
          />
          <div v-if="cnpjErro" class="invalid-feedback">{{ cnpjErro }}</div>
        </div>
        <div v-if="erroConsulta" :class="situacaoIrregular ? 'alert alert-warning' : 'alert alert-danger'" class="mt-3 py-2" style="font-size:.875rem">{{ erroConsulta }}</div>
        <div class="d-flex gap-3 mt-4">
          <button class="btn btn-ag-outline flex-fill" @click="step = 1" :disabled="consultando">Voltar</button>
          <button class="btn btn-ag-primary flex-fill" @click="consultarCnpj" :disabled="consultando || cnpjInput.length < 18">
            <span v-if="consultando" class="spinner-border spinner-border-sm me-2" />
            {{ consultando ? 'Consultando...' : 'Consultar CNPJ' }}
          </button>
        </div>
      </div>

      <form v-if="step === 3" @submit.prevent="enviar" autocomplete="off">
        <div class="credenciar-card mt-2">
          <h5 class="credenciar-section-title">DADOS DA EMPRESA</h5>
          <div class="row g-3">
            <div v-if="tipo === 'pj'" class="col-12 col-md-6 ag-form-group">
              <label>CNPJ</label>
              <input v-model="form.CNPJ" type="text" class="form-control" disabled />
            </div>
            <div v-else class="col-12 col-md-6 ag-form-group">
              <label>CPF do responsável / Documento *</label>
              <input v-model="form.CNPJ" type="text" class="form-control" placeholder="000.000.000-00" maxlength="14" required :disabled="enviando" @input="mascararCpf" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>{{ tipo === 'pj' ? 'Razão Social' : 'Nome completo' }} *</label>
              <input v-model="form.RazaoSocial" type="text" class="form-control" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>{{ tipo === 'pj' ? 'Nome Fantasia' : 'Nome do negócio' }} *</label>
              <input v-model="form.NomeFantasia" type="text" class="form-control" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-4 ag-form-group">
              <label>CEP *</label>
              <input v-model="form.CEP" type="text" class="form-control" placeholder="00000-000" maxlength="9" required :disabled="enviando" @blur="buscarCep" />
            </div>
            <div class="col-12 col-md-8 ag-form-group">
              <label>Logradouro (Rua/Av.) *</label>
              <input v-model="form.Logradouro" type="text" class="form-control" required :disabled="enviando" />
            </div>
            <div class="col-6 col-md-3 ag-form-group">
              <label>Número *</label>
              <input v-model="form.Numero" type="text" class="form-control" required :disabled="enviando" />
            </div>
            <div class="col-6 col-md-4 ag-form-group">
              <label>Complemento</label>
              <input v-model="form.Complemento" type="text" class="form-control" :disabled="enviando" />
            </div>
            <div class="col-12 col-md-5 ag-form-group">
              <label>Bairro *</label>
              <input v-model="form.Bairro" type="text" class="form-control" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-4 ag-form-group">
              <label>Estado *</label>
              <select v-model="form.IdEstado" class="form-select" required :disabled="enviando" @change="carregarCidades">
                <option value="">Selecione o estado</option>
                <option v-for="e in estados" :key="e.IdEstado" :value="e.IdEstado">{{ e.Nome }} ({{ e.Uf }})</option>
              </select>
            </div>
            <div class="col-12 col-md-4 ag-form-group">
              <label>Cidade *</label>
              <select v-model="form.IdCidade" class="form-select" required :disabled="enviando || !form.IdEstado">
                <option value="">{{ form.IdEstado ? (carregandoCidades ? 'Carregando...' : 'Selecione a cidade') : 'Selecione o estado primeiro' }}</option>
                <option v-for="c in cidades" :key="c.IdCidade" :value="c.IdCidade">{{ c.Nome }}</option>
              </select>
            </div>
            <div class="col-12 col-md-4 ag-form-group">
              <label>Telefone da Empresa *</label>
              <input v-model="form.TelefoneEmpresa" type="text" class="form-control" placeholder="(00) 00000-0000" required :disabled="enviando" />
            </div>
            <div class="col-12 ag-form-group">
              <label>Logomarca (imagem PNG ou JPG)</label>
              <input type="file" class="form-control" accept="image/png,image/jpeg,image/jpg" :disabled="enviando" @change="processarImagem" />
              <div v-if="form.Logomarca" class="mt-2">
                <img :src="form.Logomarca" alt="Pré-visualização da logo" style="height:64px; border-radius:6px; border:1px solid #dee2e6" />
              </div>
            </div>
          </div>
        </div>

        <div class="credenciar-card mt-3">
          <h5 class="credenciar-section-title">DADOS DO RESPONSÁVEL</h5>
          <div class="row g-3">
            <div class="col-12 col-md-6 ag-form-group">
              <label>Nome do responsável *</label>
              <input v-model="form.NomeResponsavel" type="text" class="form-control" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>Data de nascimento *</label>
              <input v-model="form.DataNascimento" type="date" class="form-control" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>CPF do responsável *</label>
              <input v-model="form.CPFResponsavel" type="text" class="form-control" placeholder="000.000.000-00" maxlength="14" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>WhatsApp/Celular *</label>
              <input v-model="form.TelefoneResponsavel" type="text" class="form-control" placeholder="(00) 00000-0000" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>E-mail *</label>
              <input v-model="form.EmailResponsavel" type="email" class="form-control" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>Login (nome de usuário) *</label>
              <input v-model="form.Indicador" type="text" class="form-control" placeholder="Mínimo 5 caracteres, sem espaços" :disabled="enviando" />
              <small class="text-muted" style="font-size:.78rem">Este será seu login de acesso à plataforma</small>
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>Senha *</label>
              <input v-model="form.SenhaResponsavel" type="password" class="form-control" placeholder="Mínimo 8 caracteres (letras e números)" required :disabled="enviando" />
            </div>
            <div class="col-12 col-md-6 ag-form-group">
              <label>Confirmar senha *</label>
              <input v-model="confirmarSenha" type="password" class="form-control" :class="{ 'is-invalid': confirmarSenha && confirmarSenha !== form.SenhaResponsavel }" required :disabled="enviando" />
              <div class="invalid-feedback">As senhas não conferem.</div>
            </div>
            <div class="col-12 ag-form-group">
              <label>Código de quem indicou (opcional)</label>
              <input v-model="loginIndicador" type="text" class="form-control" placeholder="Login do seu indicador" :disabled="enviando" />
            </div>
          </div>
        </div>

        <div class="credenciar-card mt-3">
          <h5 class="credenciar-section-title">DADOS DO CREDENCIAMENTO</h5>
          <div class="row g-3">
            <div class="col-12 col-md-4 ag-form-group">
              <label>Percentual de cashback (%) *</label>
              <select v-model="form.PercentualCashback" class="form-select" required :disabled="enviando">
                <option value="">Selecione</option>
                <option v-for="p in percentuais" :key="p" :value="p">{{ p }}%</option>
              </select>
            </div>
            <div class="col-12 col-md-4 ag-form-group">
              <label>Categoria *</label>
              <select v-model="form.IdCategoria" class="form-select" required :disabled="enviando">
                <option value="">Selecione a categoria</option>
                <option v-for="cat in categorias" :key="cat.key" :value="cat.key">{{ cat.value }}</option>
              </select>
            </div>
            <div class="col-12 col-md-4 ag-form-group">
              <label>Localização (Latitude, Longitude)</label>
              <div class="d-flex gap-2">
                <input v-model="form.Latitude" type="number" step="any" class="form-control" placeholder="Lat." :disabled="enviando" />
                <input v-model="form.Longitude" type="number" step="any" class="form-control" placeholder="Long." :disabled="enviando" />
              </div>
              <small class="text-muted" style="font-size:.78rem">
                <button type="button" class="btn btn-link p-0" style="font-size:.78rem" @click="obterLocalizacao" :disabled="buscandoGeo">
                  {{ buscandoGeo ? 'Obtendo...' : '📍 Usar minha localização atual' }}
                </button>
              </small>
            </div>
          </div>
        </div>

        <div v-if="erroEnvio" class="alert alert-danger mt-3 py-2" style="font-size:.875rem">{{ erroEnvio }}</div>
        <div v-if="sucessoEnvio" class="alert alert-success mt-3" style="font-size:.875rem">
          <strong>Formulário enviado com sucesso!</strong><br/>
          Sua solicitação de credenciamento foi recebida e está em análise. Você receberá um e-mail de confirmação em breve.
          <div class="mt-2"><NuxtLink to="/agencia" class="btn btn-ag-outline btn-sm">Ir para o início</NuxtLink></div>
        </div>

        <div class="d-flex gap-3 mt-4 mb-5">
          <button type="button" class="btn btn-ag-outline" @click="voltarStep" :disabled="enviando">Voltar</button>
          <button type="submit" class="btn btn-ag-primary flex-fill" :disabled="enviando || !!sucessoEnvio">
            <span v-if="enviando" class="spinner-border spinner-border-sm me-2" />
            {{ enviando ? 'Enviando...' : 'Enviar para análise' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';

definePageMeta({ layout: 'layout-home' });

useSeoMeta({
  title: 'Credenciamento de Parceiro Local | Quanta Shop',
  description: 'Seja um parceiro local do Quanta Shop e aumente suas vendas oferecendo cashback real aos seus clientes. Cadastro simples e rápido.',
  ogTitle: 'Credenciamento de Parceiro Local | Quanta Shop',
  ogDescription: 'Seja um parceiro local do Quanta Shop e aumente suas vendas oferecendo cashback real aos seus clientes. Cadastro simples e rápido.',
  ogImage: '/logo.png'
});

useHead({
  link: [
    { rel: 'canonical', href: 'https://quantashop.com.br/agencia/credenciar' }
  ]
});

const api = useApi();
const route = useRoute();

const step = ref(1);
const tipo = ref<'pf' | 'pj' | ''>('');
const cnpjInput = ref('');
const cnpjErro = ref('');
const erroConsulta = ref('');
const consultando = ref(false);
const situacaoIrregular = ref(false);
const enviando = ref(false);
const erroEnvio = ref('');
const sucessoEnvio = ref(false);
const confirmarSenha = ref('');
const loginIndicador = ref((route.query.indicador as string) || '');
const buscandoGeo = ref(false);
const carregandoCidades = ref(false);

interface EstadoItem { IdEstado: number; Nome: string; Uf: string }
interface CidadeItem { IdCidade: number; Nome: string }
interface CategoriaItem { key: number; value: string }

const estados = ref<EstadoItem[]>([]);
const cidades = ref<CidadeItem[]>([]);
const categorias = ref<CategoriaItem[]>([]);

const percentuais = [5, 8, 10, 12, 15, 20, 25, 30];

const form = reactive({
  CNPJ: '',
  RazaoSocial: '',
  NomeFantasia: '',
  CEP: '',
  Logradouro: '',
  Numero: '',
  Complemento: '',
  Bairro: '',
  IdEstado: 0,
  IdCidade: 0,
  Latitude: null as number | null,
  Longitude: null as number | null,
  Logomarca: '',
  NomeResponsavel: '',
  DataNascimento: '',
  TelefoneResponsavel: '',
  EmailResponsavel: '',
  CPFResponsavel: '',
  SenhaResponsavel: '',
  PercentualCashback: '' as number | string,
  TelefoneEmpresa: '',
  IdCategoria: '' as number | string,
  Indicador: '',
});

onMounted(async () => {
  await Promise.all([carregarEstados(), carregarCategorias()]);
});

async function carregarEstados() {
  try {
    const { data } = await api.get('/geral/buscaEstado');
    estados.value = Array.isArray(data) ? data : [];
  } catch (e) {
    console.error('Erro ao carregar estados:', e);
  }
}

async function carregarCategorias() {
  try {
    const { data } = await api.get('/v2/categories/get-featured-categories');
    if (data?.data && Array.isArray(data.data)) {
      categorias.value = data.data.map((c: Record<string, unknown>) => ({
        key: c['IdCategoria'] ?? c['id'],
        value: c['Nome'] ?? c['nome'] ?? c['name'],
      }));
    }
  } catch (e) {
    console.error('Erro ao carregar categorias:', e);
    categorias.value = [
      { key: 1, value: 'Alimentação' }, { key: 2, value: 'Saúde e Beleza' },
      { key: 3, value: 'Educação' }, { key: 4, value: 'Moda e Vestuário' },
      { key: 5, value: 'Serviços' }, { key: 6, value: 'Pet Shop' },
      { key: 7, value: 'Farmácia' }, { key: 8, value: 'Mercado' },
      { key: 9, value: 'Eletrônicos' }, { key: 10, value: 'Outros' },
    ];
  }
}

async function carregarCidades() {
  if (!form.IdEstado) return;
  carregandoCidades.value = true;
  form.IdCidade = 0;
  try {
    const { data } = await api.get(`/geral/buscaCidade/${form.IdEstado}`);
    cidades.value = Array.isArray(data) ? data : [];
  } catch (e) {
    console.error('Erro ao carregar cidades:', e);
  } finally {
    carregandoCidades.value = false;
  }
}

function selecionarTipo(t: 'pf' | 'pj') {
  tipo.value = t;
  if (t === 'pf') {
    step.value = 3;
  } else {
    step.value = 2;
  }
}

function voltarStep() {
  erroEnvio.value = '';
  if (step.value === 3 && tipo.value === 'pj') {
    step.value = 2;
  } else {
    step.value = 1;
    tipo.value = '';
    cnpjInput.value = '';
    cnpjErro.value = '';
    erroConsulta.value = '';
  }
}

function mascararCnpj() {
  let v = cnpjInput.value.replace(/\D/g, '');
  if (v.length > 14) v = v.slice(0, 14);
  v = v.replace(/^(\d{2})(\d)/, '$1.$2');
  v = v.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
  v = v.replace(/\.(\d{3})(\d)/, '.$1/$2');
  v = v.replace(/(\d{4})(\d)/, '$1-$2');
  cnpjInput.value = v;
  cnpjErro.value = '';
  erroConsulta.value = '';
}

function mascararCpf() {
  let v = form.CNPJ.replace(/\D/g, '');
  if (v.length > 11) v = v.slice(0, 11);
  v = v.replace(/(\d{3})(\d)/, '$1.$2');
  v = v.replace(/(\d{3})(\d)/, '$1.$2');
  v = v.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
  form.CNPJ = v;
}

function validarCnpj(cnpj: string): boolean {
  const c = cnpj.replace(/\D/g, '');
  if (c.length !== 14) return false;
  if (/^(\d)\1+$/.test(c)) return false;
  const calc = (n: number) => {
    let s = 0; let p = n - 7;
    for (let i = 0; i < n; i++) {
      s += parseInt(c[i]) * p--;
      if (p < 2) p = 9;
    }
    const r = s % 11;
    return r < 2 ? 0 : 11 - r;
  };
  return calc(12) === parseInt(c[12]) && calc(13) === parseInt(c[13]);
}

async function consultarCnpj() {
  cnpjErro.value = '';
  erroConsulta.value = '';
  const cnpjNumeros = cnpjInput.value.replace(/\D/g, '');
  if (!validarCnpj(cnpjNumeros)) {
    cnpjErro.value = 'CNPJ inválido. Verifique os dígitos e tente novamente.';
    return;
  }
  consultando.value = true;
  try {
    const resp = await fetch(`https://brasilapi.com.br/api/cnpj/v1/${cnpjNumeros}`);
    if (!resp.ok) {
      if (resp.status === 404) throw new Error('CNPJ não encontrado.');
      throw new Error('Erro ao consultar o CNPJ. Tente novamente.');
    }
    const dados = await resp.json();
    situacaoIrregular.value = dados.descricao_situacao_cadastral && ['BAIXADA', 'INAPTA'].includes(dados.descricao_situacao_cadastral);
    
    if (situacaoIrregular.value) {
       erroConsulta.value = `CNPJ com situação irregular: ${dados.descricao_situacao_cadastral}`;
    }
    
    form.CNPJ = cnpjInput.value;
    form.RazaoSocial = dados.razao_social || '';
    form.NomeFantasia = dados.nome_fantasia || dados.razao_social || '';
    const cepRaw = (dados.cep || '').replace(/\D/g, '');
    form.CEP = cepRaw ? `${cepRaw.slice(0, 5)}-${cepRaw.slice(5)}` : '';
    form.Logradouro = dados.logradouro || '';
    form.Numero = dados.numero || '';
    form.Complemento = dados.complemento || '';
    form.Bairro = dados.bairro || '';
    const telefone = dados.ddd_telefone_1 || '';
    form.TelefoneEmpresa = telefone.replace(/[^\d\s\-()]/g, '');
    const uf = dados.uf || '';
    if (uf && estados.value.length > 0) {
      const estadoMatch = estados.value.find(e => e.Uf === uf);
      if (estadoMatch) {
        form.IdEstado = estadoMatch.IdEstado;
        await carregarCidades();
        const municipio = (dados.municipio || '').toUpperCase();
        const cidadeMatch = cidades.value.find(c => c.Nome.toUpperCase() === municipio);
        if (cidadeMatch) form.IdCidade = cidadeMatch.IdCidade;
      }
    }

    step.value = 3;
  } catch (err: unknown) {
    erroConsulta.value = err instanceof Error ? err.message : 'Erro ao consultar o CNPJ.';
  } finally {
    consultando.value = false;
  }
}

async function buscarCep() {
  const cep = form.CEP.replace(/\D/g, '');
  if (cep.length !== 8) return;
  try {
    const resp = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
    const dados = await resp.json();
    if (!dados.erro) {
      if (!form.Logradouro) form.Logradouro = dados.logradouro || '';
      if (!form.Bairro) form.Bairro = dados.bairro || '';
      const uf = dados.uf || '';
      if (uf && estados.value.length > 0 && !form.IdEstado) {
        const estadoMatch = estados.value.find(e => e.Uf === uf);
        if (estadoMatch) {
          form.IdEstado = estadoMatch.IdEstado;
          await carregarCidades();
          const cidadeMatch = cidades.value.find(c => c.Nome.toUpperCase() === (dados.localidade || '').toUpperCase());
          if (cidadeMatch) form.IdCidade = cidadeMatch.IdCidade;
        }
      }
    }
  } catch {
  }
}

function obterLocalizacao() {
  if (!navigator.geolocation) return;
  buscandoGeo.value = true;
  navigator.geolocation.getCurrentPosition(
    (pos) => {
      form.Latitude = parseFloat(pos.coords.latitude.toFixed(7));
      form.Longitude = parseFloat(pos.coords.longitude.toFixed(7));
      buscandoGeo.value = false;
    },
    () => { buscandoGeo.value = false; }
  );
}

function processarImagem(event: Event) {
  const input = event.target as HTMLInputElement;
  const file = input.files?.[0];
  if (!file) return;
  if (file.size > 2 * 1024 * 1024) {
    alert('A imagem deve ter no máximo 2MB.');
    input.value = '';
    return;
  }
  const reader = new FileReader();
  reader.onload = (e) => {
    form.Logomarca = (e.target?.result as string) || '';
  };
  reader.readAsDataURL(file);
}

async function enviar() {
  erroEnvio.value = '';
  if (confirmarSenha.value !== form.SenhaResponsavel) {
    erroEnvio.value = 'As senhas não conferem.';
    return;
  }
  if (!form.IdEstado || !form.IdCidade) {
    erroEnvio.value = 'Selecione o estado e a cidade do estabelecimento.';
    return;
  }
  enviando.value = true;
  try {
    const payload = {
      CNPJ: form.CNPJ.replace(/\D/g, ''),
      RazaoSocial: form.RazaoSocial,
      NomeFantasia: form.NomeFantasia,
      CEP: form.CEP.replace(/\D/g, ''),
      Logradouro: form.Logradouro,
      Numero: form.Numero,
      Complemento: form.Complemento,
      Bairro: form.Bairro,
      IdEstado: Number(form.IdEstado),
      IdCidade: Number(form.IdCidade),
      Latitude: form.Latitude ?? 0,
      Longitude: form.Longitude ?? 0,
      Logomarca: form.Logomarca || null,
      NomeResponsavel: form.NomeResponsavel,
      DataNascimento: form.DataNascimento,
      TelefoneResponsavel: form.TelefoneResponsavel,
      EmailResponsavel: form.EmailResponsavel,
      CPFResponsavel: form.CPFResponsavel.replace(/\D/g, ''),
      SenhaResponsavel: form.SenhaResponsavel,
      PercentualCashback: Number(form.PercentualCashback),
      TelefoneEmpresa: form.TelefoneEmpresa,
      IdCategoria: Number(form.IdCategoria),
      Indicador: loginIndicador.value || '',
    };
    await api.post('/credenciamento/v2/credenciar', payload);
    sucessoEnvio.value = true;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  } catch (err: unknown) {
    erroEnvio.value = extractApiErrorMessage(err, 'Erro ao enviar formulário. Verifique os dados e tente novamente.');
  } finally {
    enviando.value = false;
  }
}
</script>

<style scoped>
.credenciar-page {
  background: #ecf2f7;
  min-height: 100vh;
  padding: 2rem 1rem;
}
.credenciar-container {
  max-width: 860px;
  margin: 0 auto;
}
.credenciar-titulo {
  font-size: 1.5rem;
  font-weight: 700;
  color: #2f7785;
  letter-spacing: .02em;
}
.credenciar-subtitulo {
  font-size: .82rem;
  font-weight: 600;
  color: #444;
  margin-bottom: .5rem;
}
.credenciar-tipo-card {
  background: #2f7785;
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 2.5rem 1.5rem;
  font-size: 1.1rem;
  font-weight: 600;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: .75rem;
  transition: background .2s, transform .15s;
  cursor: pointer;
}
.credenciar-tipo-card:hover {
  background: #225f6b;
  transform: translateY(-2px);
}
.credenciar-tipo-icon {
  font-size: 2.5rem;
}
.credenciar-card {
  background: #fff;
  border-radius: 10px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0,0,0,.06);
}
.credenciar-section-title {
  font-size: .85rem;
  font-weight: 700;
  color: #2f7785;
  letter-spacing: .06em;
  text-transform: uppercase;
  border-bottom: 2px solid #ecf2f7;
  padding-bottom: .5rem;
  margin-bottom: 1rem;
}
.ag-form-group label {
  font-size: .82rem;
  font-weight: 600;
  color: #444;
  margin-bottom: .25rem;
  display: block;
}
.btn-ag-primary {
  background: #2f7785;
  color: #fff;
  border: none;
  border-radius: 6px;
  padding: .6rem 1.5rem;
  font-weight: 600;
  transition: background .2s;
}
.btn-ag-primary:hover:not(:disabled) { background: #225f6b; }
.btn-ag-primary:disabled { opacity: .65; cursor: not-allowed; }
.btn-ag-outline {
  background: transparent;
  color: #2f7785;
  border: 2px solid #2f7785;
  border-radius: 6px;
  padding: .6rem 1.5rem;
  font-weight: 600;
  transition: all .2s;
  text-decoration: none;
  display: inline-block;
}
.btn-ag-outline:hover:not(:disabled) { background: #2f7785; color: #fff; }
</style>
