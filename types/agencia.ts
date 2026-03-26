export interface AgenciaMenuItem {
  texto: string;
  url: string;
  rotaPublica?: boolean;
  icon?: string;
  subMenus?: AgenciaMenuItem[];
}

export interface AgenciaApiError {
  response?: {
    data?: { message?: string } | string;
    status?: number;
  };
  message?: string;
}

export function extractApiErrorMessage(err: unknown, fallback = 'Ocorreu um erro. Tente novamente.'): string {
  const e = err as AgenciaApiError;
  const data = e?.response?.data;
  if (typeof data === 'string' && data.length < 300) return data;
  if (typeof data === 'object' && data?.message) return data.message;
  if (e?.message) return e.message;
  return fallback;
}

export interface UsuarioAdmin {
  id: number;
  login: string;
  username: string;
  email: string;
  perfil: string;
  status: string;
  dataCadastro?: string;
  [key: string]: unknown;
}

export interface PagamentoAdmin {
  id: number;
  usuario: string;
  valor: number;
  status: string;
  data: string;
  tipo: string;
  chavePix?: string;
  [key: string]: unknown;
}

export interface CompraAdmin {
  id: number;
  usuario: string;
  loja: string;
  valor: number;
  cashback: number;
  status: string;
  data: string;
  [key: string]: unknown;
}

export interface CredenciamentoAdmin {
  id: number;
  nomeFantasia: string;
  razaoSocial: string;
  cnpj: string;
  status: string;
  data: string;
  [key: string]: unknown;
}

export interface CategoriaAdmin {
  id: number;
  nome: string;
  descricao?: string;
  ativo: boolean;
  [key: string]: unknown;
}

export interface EcossistemaAdmin {
  id: number;
  nome: string;
  descricao?: string;
  [key: string]: unknown;
}

export interface HeroBannerSlide {
  id: number;
  titulo: string;
  url: string;
  urlDestino: string;
  ativo: boolean;
  headline: string;
  subtitulo: string;
  badge: string;
  ctaTexto: string;
  ctaLink: string;
  ctaCor: string;
  textoCor: 'light' | 'dark';
  overlayIntensidade: number;
  objectPosition?: string;
  tituloFontSize?: 'pequeno' | 'medio' | 'grande';
  overlayDirecao?: 'esquerda' | 'direita' | 'centro' | 'uniforme';
  ctaAlinhamento?: 'esquerda' | 'centro' | 'direita';
  ctaTamanho?: 'pequeno' | 'medio' | 'grande';
  badgeCor?: string;
  headlineCor?: string;
  subtituloCor?: string;
  subtituloFontSize?: 'pequeno' | 'medio' | 'grande';
  ctaTextoCor?: string;
  overlayCor?: string;
  headlineEspacamento?: 'compacto' | 'normal' | 'amplo';
}

export interface CarrosselAdmin {
  id: number;
  titulo: string;
  url?: string;
  urlDestino?: string;
  ativo: boolean;
}

export interface ComunicadoAdmin {
  id: number;
  titulo: string;
  conteudo: string;
  data: string;
  ativo: boolean;
  [key: string]: unknown;
}

export interface RedeAdmin {
  id: number;
  usuario: string;
  nivel: number;
  status: string;
  [key: string]: unknown;
}

export interface SuporteAdmin {
  id: number;
  usuario: string;
  assunto: string;
  mensagem: string;
  status: string;
  data: string;
  [key: string]: unknown;
}

export interface LojaCredenciadaAdmin {
  id: number;
  nomeFantasia: string;
  cnpj: string;
  cidade: string;
  estado: string;
  ativo: boolean;
  [key: string]: unknown;
}

export interface FaturaAdmin {
  id: number;
  usuario: string;
  valor: number;
  status: string;
  dataVencimento: string;
  dataPagamento?: string;
  [key: string]: unknown;
}

export interface AnuncianteAdmin {
  id: number;
  nome: string;
  cashback: number;
  status: string;
  [key: string]: unknown;
}

export interface CashbackAdmin {
  id: number;
  usuario: string;
  loja: string;
  valor: number;
  data: string;
  [key: string]: unknown;
}

export interface AniversarianteAdmin {
  id: number;
  nome: string;
  dataNascimento: string;
  email?: string;
  telefone?: string;
  [key: string]: unknown;
}

export interface AcessoAdmin {
  id: number;
  usuario: string;
  ip: string;
  data: string;
  dispositivo?: string;
  [key: string]: unknown;
}

export interface LancamentoAdmin {
  id: number;
  descricao: string;
  valor: number;
  tipo: string;
  data: string;
  [key: string]: unknown;
}

export interface MaterialApoioAdmin {
  id: number;
  titulo: string;
  descricao?: string;
  url?: string;
  tipo: string;
  [key: string]: unknown;
}

export interface AssinaturaAdmin {
  id: number;
  usuario: string;
  plano: string;
  status: string;
  dataInicio: string;
  dataFim?: string;
  valor: number;
  [key: string]: unknown;
}

export interface GrupoAdmin {
  id: number;
  nome: string;
  descricao?: string;
  membros: number;
  [key: string]: unknown;
}

export interface Compra {
  id: number;
  loja: string;
  valor: number;
  cashback: number;
  status: string;
  data: string;
  [key: string]: unknown;
}

export interface Comunicado {
  id: number;
  titulo: string;
  conteudo: string;
  data: string;
  [key: string]: unknown;
}

export interface Direto {
  id: number;
  nome: string;
  login: string;
  status: string;
  dataCadastro: string;
  [key: string]: unknown;
}

export interface Graduacao {
  id: number;
  nome: string;
  nivel: number;
  dataAlcancado?: string;
  [key: string]: unknown;
}

export interface Cupom {
  id: number;
  codigo: string;
  percentualCashback?: number;
  valor?: number;
  loja?: string;
  dataValidade?: string;
  status?: string;
  [key: string]: unknown;
}

export interface TicketSuporte {
  id: number;
  assunto: string;
  status: string;
  data: string;
  resposta?: string;
  [key: string]: unknown;
}

export interface FaqItem {
  id: number;
  pergunta: string;
  resposta: string;
  categoria?: string;
  [key: string]: unknown;
}

export interface MaterialApoio {
  id: number;
  titulo: string;
  url?: string;
  tipo: string;
  descricao?: string;
  [key: string]: unknown;
}

export interface Plano {
  id: number;
  nome: string;
  descricao?: string;
  valor: number;
  periodo: string;
  [key: string]: unknown;
}

export interface MovimentacaoFinanceira {
  id: number;
  tipo: string;
  valor: number;
  data: string;
  descricao?: string;
  [key: string]: unknown;
}

export interface ContaBancaria {
  id: number;
  banco: string;
  agencia: string;
  conta: string;
  tipo: string;
  titular: string;
  principal: boolean;
  [key: string]: unknown;
}

export interface MembroRede {
  id: number;
  nome: string;
  login: string;
  nivel: number;
  status: string;
  dataCadastro: string;
  [key: string]: unknown;
}

export interface PerformancePeriodo {
  mes: string;
  compras: number;
  cashback: number;
  [key: string]: unknown;
}

export interface Loja {
  id: number;
  nome: string;
  cashback: number;
  url?: string;
  [key: string]: unknown;
}
