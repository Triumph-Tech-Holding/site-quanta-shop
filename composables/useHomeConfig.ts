import { ref } from 'vue';

export interface HeroCard {
  ativo: boolean;
  label: string;
  value: string;
  valueColor: 'green' | 'teal' | 'white';
  icon: 'card' | 'chart' | 'bag' | 'star' | 'percent' | 'gift' | 'users' | 'zap';
  iconBg: 'teal' | 'green';
}

export interface HeroSection {
  badge: string;
  title: string;
  subtitle: string;
  ctaPrimaryText: string;
  ctaPrimaryLink: string;
}

export interface SimpleLabelSection {
  label: string;
}

export interface TitleSubtitleSection {
  title: string;
  subtitle: string;
}

export interface LabelTitleSubtitleSection {
  label: string;
  title: string;
  subtitle: string;
}

export interface CeoSection {
  tag: string;
  pre: string;
  name: string;
  desc: string;
  ctaText: string;
  whatsappLink: string;
}

export interface FooterCtaSection {
  title: string;
  subtitle: string;
  primaryText: string;
  primaryLink: string;
  outlineText: string;
  outlineLink: string;
}

export interface HomeConfig {
  hero: HeroSection;
  heroCards: HeroCard[];
  brands: SimpleLabelSection;
  ofertas: TitleSubtitleSection;
  parceirosOnline: LabelTitleSubtitleSection;
  parceirosLocais: LabelTitleSubtitleSection;
  testimonials: TitleSubtitleSection;
  blog: LabelTitleSubtitleSection;
  ceo: CeoSection;
  footerCta: FooterCtaSection;
}

const DEFAULT_CONFIG: HomeConfig = {
  hero: {
    badge: '+12.000 usuários economizando',
    title: 'Seu dinheiro <highlight>volta</highlight> a cada compra',
    subtitle: 'Compre nas suas lojas favoritas e receba cashback de verdade. Simples, transparente e instantâneo.',
    ctaPrimaryText: 'Criar Conta Grátis',
    ctaPrimaryLink: '/register',
  },
  heroCards: [
    { ativo: true, label: 'PIX INSTANTÂNEO', value: 'Saque em segundos ✓', valueColor: 'green', icon: 'card', iconBg: 'teal' },
    { ativo: true, label: 'CASHBACK RECEBIDO', value: 'R$ 50,00', valueColor: 'green', icon: 'chart', iconBg: 'green' },
    { ativo: true, label: 'MARCAS PARCEIRAS', value: '+500 lojas', valueColor: 'teal', icon: 'bag', iconBg: 'teal' },
  ],
  brands: {
    label: 'AS MAIORES MARCAS CONFIAM NA QUANTA',
  },
  ofertas: {
    title: 'Ofertas do Dia',
    subtitle: 'Produtos selecionados com cashback turbinado. Aproveite antes que acabe!',
  },
  parceirosOnline: {
    label: 'Parceiros Online',
    title: 'Compre online e receba cashback',
    subtitle: 'Centenas de marcas com cashback garantido. Ative e compre normalmente.',
  },
  parceirosLocais: {
    label: 'Parceiros Locais',
    title: 'Cashback perto de você',
    subtitle: 'Lojas, restaurantes e serviços no seu bairro com cashback automático.',
  },
  testimonials: {
    title: 'Testemunhos de Usuários Reais',
    subtitle: 'Veja quanto nossos usuários já economizaram.',
  },
  blog: {
    label: 'SEMPRE CONECTADO',
    title: 'Quanta em Tempo Real: Blog e Redes Sociais',
    subtitle: 'Fique por dentro das últimas novidades, promoções e conteúdos exclusivos.',
  },
  ceo: {
    tag: 'CEO & Founder',
    pre: 'Fale com o CEO',
    name: 'Mauro Triumph',
    desc: 'Clareza estratégica sem rodeios, sem burocracia, sem perda de tempo.',
    ctaText: 'Iniciar Conversa com IA',
    whatsappLink: 'https://api.whatsapp.com/send/?phone=552140404866&text&type=phone_number&app_absent=0',
  },
  footerCta: {
    title: 'Pronto para começar a economizar?',
    subtitle: 'Cadastre-se gratuitamente e aproveite cashback em milhares de lojas.',
    primaryText: 'Cadastrar Agora',
    primaryLink: '/register',
    outlineText: 'Já tenho conta',
    outlineLink: '/login',
  },
};

const _config = ref<HomeConfig>({ ...DEFAULT_CONFIG });
let _loaded = false;
let _fetchPromise: Promise<void> | null = null;

export function useHomeConfig() {
  const config = _config;

  async function loadConfig(): Promise<void> {
    if (_loaded) return;
    if (_fetchPromise) return _fetchPromise;

    _fetchPromise = $fetch<HomeConfig>('/data/home-config.json')
      .then((data) => {
        _config.value = { ...DEFAULT_CONFIG, ...data };
        _loaded = true;
      })
      .catch(() => {
        _loaded = true;
      });

    return _fetchPromise;
  }

  return { config, loadConfig };
}
