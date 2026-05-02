import { defineStore } from 'pinia';

export interface CmsParceiro {
  id: string;
  nome: string;
  logo: string;
  categoria: string;
  cashbackPct: number;
  destaque: boolean;
}

export interface CmsSocialCommerce {
  titulo: string;
  subtitulo: string;
  labelLink: string;
  whatsappCopy: string;
  incentivoCopy: string;
  parceiros: CmsParceiro[];
}

export interface CmsVitrinLinks {
  titulo: string;
  subtitulo: string;
  descLink: string;
  labelLink: string;
  whatsappCopy: string;
  labelCliques: string;
  labelConversoes: string;
  labelCashback: string;
  dicaCopy: string;
}

export interface CmsPaywallHaf {
  titulo: string;
  subtitulo: string;
  beneficios: string[];
  ctaTexto: string;
  ctaLink: string;
}

export interface CmsPlusModulo {
  labelBadgeAtivo: string;
  labelBadgeInativo: string;
  tituloProgresso: string;
  legendaProgresso: string;
  tituloKpi: string;
  tituloRadar: string;
  legendaRadar: string;
  metaConsumoPlus: number;
}

export interface CmsBrand {
  corPrimaria: string;
  corSecundaria: string;
  corLime: string;
}

export interface CmsConfig {
  socialCommerce: CmsSocialCommerce;
  vitrinLinks: CmsVitrinLinks;
  paywallHaf: CmsPaywallHaf;
  plusModulo: CmsPlusModulo;
  brand: CmsBrand;
}

const DEFAULT_CMS: CmsConfig = {
  brand: {
    corPrimaria: '#2F7785',
    corSecundaria: '#225F6B',
    corLime: '#98C73A',
  },

  socialCommerce: {
    titulo: 'Social Commerce — Compartilhe e Ganhe',
    subtitulo: 'Gere seu link de indicação rastreável e envie para amigos via WhatsApp. A cada compra indicada, você acumula cashback residual.',
    labelLink: 'Seu link de indicação',
    whatsappCopy: 'Oi! Comprei na {parceiro} via Quanta Shop e recebi cashback na hora. Usa meu link e você também ganha: {link}',
    incentivoCopy: 'Cada clique no seu link é um potencial ganho de cashback residual para você. Compartilhe com sua rede!',
    parceiros: [
      { id: 'amazon', nome: 'Amazon', logo: '/agencia/imgs/parceiros/amazon.png', categoria: 'E-commerce', cashbackPct: 4.5, destaque: true },
      { id: 'americanas', nome: 'Americanas', logo: '/agencia/imgs/parceiros/americanas.png', categoria: 'E-commerce', cashbackPct: 3.0, destaque: true },
      { id: 'magalu', nome: 'Magalu', logo: '/agencia/imgs/parceiros/magalu.png', categoria: 'E-commerce', cashbackPct: 3.5, destaque: false },
      { id: 'ifood', nome: 'iFood', logo: '/agencia/imgs/parceiros/ifood.png', categoria: 'Alimentação', cashbackPct: 5.0, destaque: true },
      { id: 'netshoes', nome: 'Netshoes', logo: '/agencia/imgs/parceiros/netshoes.png', categoria: 'Esportes', cashbackPct: 6.0, destaque: false },
      { id: 'booking', nome: 'Booking', logo: '/agencia/imgs/parceiros/booking.png', categoria: 'Viagens', cashbackPct: 4.0, destaque: false },
    ],
  },

  vitrinLinks: {
    titulo: 'Meu Link de Indicação Rastreável',
    subtitulo: 'Compartilhe seu link único com 1 clique. Cada pessoa que comprar pelo seu link gera cashback residual direto para você.',
    descLink: 'Link personalizado e rastreável — gerado exclusivamente para você.',
    labelLink: 'Seu link exclusivo',
    whatsappCopy: 'Ei! Você conhece a Quanta Shop? Compre em centenas de lojas e receba cashback na hora. Acesse meu link e cadastre-se grátis: {link}',
    labelCliques: 'Cliques Hoje',
    labelConversoes: 'Conversões',
    labelCashback: 'Cashback Gerado',
    dicaCopy: 'Dica: selecione um parceiro abaixo para gerar um link específico com oferta personalizada.',
  },

  paywallHaf: {
    titulo: 'Desbloqueie o Potencial da Sua Rede',
    subtitulo: 'A licença HAF libera acesso completo à rede de 12 níveis, ao Dashboard de Leads e ao Cashback Residual Dobrado com a Assinatura Plus.',
    beneficios: [
      'Dashboard de Rede e Leads em tempo real',
      'Cashback Residual em 12 níveis de profundidade',
      'Multiplicador 2× com Assinatura Plus ativa',
      'Radar de Assinaturas da sua Rede',
      'Compressão Dinâmica — bônus não se perde',
    ],
    ctaTexto: 'Ativar Licença HAF',
    ctaLink: '/agencia/painel/planos',
  },

  plusModulo: {
    labelBadgeAtivo: 'PLUS ATIVO',
    labelBadgeInativo: 'Assine o Plus',
    tituloProgresso: 'Meta de Consumo Plus',
    legendaProgresso: 'Atinja a meta para desbloquear o Cashback Residual Dobrado (2×)',
    tituloKpi: 'Ganhos Extras (Residual Dobrado)',
    tituloRadar: 'Radar de Assinaturas da Rede',
    legendaRadar: 'Membros da sua rede com Assinatura Plus ativa',
    metaConsumoPlus: 500,
  },
};

export const useCmsStore = defineStore('cms', () => {
  const config = ref<CmsConfig>({ ...DEFAULT_CMS });
  let _loaded = false;

  async function loadConfig(): Promise<void> {
    if (_loaded) return;
    try {
      const data = await $fetch<Partial<CmsConfig>>('/data/cms-config.json').catch(() => ({}));
      config.value = {
        brand: { ...DEFAULT_CMS.brand, ...(data.brand ?? {}) },
        socialCommerce: { ...DEFAULT_CMS.socialCommerce, ...(data.socialCommerce ?? {}) },
        vitrinLinks: { ...DEFAULT_CMS.vitrinLinks, ...(data.vitrinLinks ?? {}) },
        paywallHaf: { ...DEFAULT_CMS.paywallHaf, ...(data.paywallHaf ?? {}) },
        plusModulo: { ...DEFAULT_CMS.plusModulo, ...(data.plusModulo ?? {}) },
      };
    } catch {
      // fallback silencioso — usa DEFAULT_CMS
    } finally {
      _loaded = true;
    }
  }

  function applyBrandCssVars(): void {
    if (!import.meta.client) return;
    const { corPrimaria, corSecundaria, corLime } = config.value.brand;
    document.documentElement.style.setProperty('--qs-teal', corPrimaria);
    document.documentElement.style.setProperty('--qs-teal-dark', corSecundaria);
    document.documentElement.style.setProperty('--qs-lime', corLime);
  }

  return { config, loadConfig, applyBrandCssVars };
});
