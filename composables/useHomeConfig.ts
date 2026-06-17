import { computed } from 'vue';
import { useHomeCmsStore } from '@/pinia/useHomeCmsStore';

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

export interface BrandItem {
  nome: string;
  imagem: string;
  link?: string;
}

export interface BrandsSection {
  label: string;
  items?: BrandItem[];
}

export interface TitleSubtitleSection {
  title: string;
  subtitle: string;
}

export interface OfertasSection {
  label: string;
  title: string;
  subtitle: string;
}

export interface LabelTitleSubtitleSection {
  label: string;
  title: string;
  subtitle: string;
}

export interface TestimonialItem {
  name: string;
  initials: string;
  role: string;
  highlight: string;
  text: string;
}

export interface TestimonialsSection {
  title: string;
  subtitle: string;
  items: TestimonialItem[];
}

export interface BlogPostCms {
  id: number;
  title: string;
  excerpt: string;
  img: string;
  date: string;
  slug: string;
  type?: 'blog' | 'instagram' | 'youtube';
}

export interface BlogSection {
  label: string;
  title: string;
  subtitle: string;
  posts: BlogPostCms[];
}

export interface CeoSection {
  ativo?: boolean;
  imagemFundo?: string;
  overlayOpacity?: number;
  posicaoFundo?: string;
  tag: string;
  pre: string;
  name: string;
  desc: string;
  ctaText: string;
  whatsappLink: string;
  whatsappText?: string;
  badge1Label?: string;
  badge1Value?: string;
  badge2Label?: string;
  badge2Value?: string;
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
  brands: BrandsSection;
  ofertas: OfertasSection;
  parceirosOnline: LabelTitleSubtitleSection;
  parceirosLocais: LabelTitleSubtitleSection;
  testimonials: TestimonialsSection;
  blog: BlogSection;
  ceo: CeoSection;
  footerCta: FooterCtaSection;
}

export const DEFAULT_TESTIMONIALS: TestimonialItem[] = [
  {
    name: 'Marina Costa',
    initials: 'MC',
    role: 'Usuária há 8 meses',
    highlight: 'Já recebi mais de R$ 1.240 de cashback',
    text: ' em apenas 6 meses. Nunca mais compro sem a Quanta!',
  },
  {
    name: 'Rafael Oliveira',
    initials: 'RO',
    role: 'Usuário há 1 ano',
    highlight: 'A cada compra, o dinheiro volta.',
    text: ' Já acumulei R$ 890 sem nenhum esforço extra. Recomendo demais.',
  },
  {
    name: 'Juliana Santos',
    initials: 'JS',
    role: 'Usuária há 1 ano',
    highlight: 'Minha família economizou R$ 2.100 no último ano.',
    text: ' É dinheiro de verdade voltando pro bolso.',
  },
];

export const DEFAULT_BLOG_POSTS: BlogPostCms[] = [
  {
    id: 1,
    title: 'Cashback de verdade! Veja como funciona na prática 💰',
    excerpt: 'Descubra como acumular cashback em cada compra do seu dia a dia.',
    img: 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80&auto=format&fit=crop',
    date: '22 Mar 2025',
    slug: 'cashback-na-pratica',
    type: 'instagram',
  },
  {
    id: 2,
    title: 'Tutorial: Como ativar seu cashback em 3 passos',
    excerpt: 'Veja o passo a passo para começar a ganhar cashback hoje mesmo.',
    img: 'https://images.unsplash.com/photo-1574375927938-d5a98e8ffe85?w=600&q=80&auto=format&fit=crop',
    date: '20 Mar 2025',
    slug: 'tutorial-ativar-cashback',
    type: 'youtube',
  },
  {
    id: 3,
    title: '5 dicas para maximizar seu cashback nas compras online',
    excerpt: 'Dicas e estratégias para acumular mais cashback toda semana.',
    img: 'https://images.unsplash.com/photo-1563986768494-4dee2763ff3f?w=600&q=80&auto=format&fit=crop',
    date: '18 Mar 2025',
    slug: 'maximizar-cashback-online',
    type: 'blog',
  },
  {
    id: 4,
    title: 'Novas marcas parceiras chegando! Fique ligado 🚀',
    excerpt: 'Confira as novidades que estão chegando ao ecossistema Quanta.',
    img: 'https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=600&q=80&auto=format&fit=crop',
    date: '15 Mar 2025',
    slug: 'novas-marcas-parceiras',
    type: 'instagram',
  },
  {
    id: 5,
    title: 'Quanta Shop atinge 12 mil usuários ativos',
    excerpt: 'Conheça nossa trajetória e o impacto gerado para milhares de usuários.',
    img: 'https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=600&q=80&auto=format&fit=crop',
    date: '12 Mar 2025',
    slug: 'quanta-12-mil-usuarios',
    type: 'blog',
  },
  {
    id: 6,
    title: 'Entrevista exclusiva com o CEO Mauro Triumph',
    excerpt: 'O fundador fala sobre a visão de futuro da plataforma de cashback.',
    img: 'https://images.unsplash.com/photo-1516738901601-6d0ee099431b?w=600&q=80&auto=format&fit=crop',
    date: '10 Mar 2025',
    slug: 'entrevista-ceo-mauro',
    type: 'youtube',
  },
  {
    id: 7,
    title: 'Promoção relâmpago: até 15% de cashback hoje! ⚡',
    excerpt: 'Aproveite as promoções especiais com cashback extra nas marcas parceiras.',
    img: 'https://images.unsplash.com/photo-1607082348824-0a96f2a4b9da?w=600&q=80&auto=format&fit=crop',
    date: '8 Mar 2025',
    slug: 'promocao-relampago-15',
    type: 'instagram',
  },
  {
    id: 8,
    title: 'Como o cashback está transformando o varejo brasileiro',
    excerpt: 'Análise do impacto do cashback no comportamento de compra dos consumidores.',
    img: 'https://images.unsplash.com/photo-1579621970563-ebec7560ff3e?w=600&q=80&auto=format&fit=crop',
    date: '5 Mar 2025',
    slug: 'cashback-varejo-brasileiro',
    type: 'blog',
  },
];

export const DEFAULT_CONFIG: HomeConfig = {
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
    label: 'TEMPO LIMITADO',
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
    items: DEFAULT_TESTIMONIALS,
  },
  blog: {
    label: 'SEMPRE CONECTADO',
    title: 'Quanta em Tempo Real: Blog e Redes Sociais',
    subtitle: 'Fique por dentro das últimas novidades, promoções e conteúdos exclusivos.',
    posts: [],
  },
  ceo: {
    ativo: true,
    imagemFundo: '',
    overlayOpacity: 0.72,
    posicaoFundo: 'center',
    tag: 'CEO & Founder',
    pre: 'FALE COM O CEO',
    name: 'MAURO TRIUMPH',
    desc: 'Clareza estratégica sem rodeios, sem burocracia, sem perda de tempo.',
    ctaText: 'Iniciar Conversa com IA',
    whatsappLink: 'https://api.whatsapp.com/send/?phone=552140404866&text&type=phone_number&app_absent=0',
    whatsappText: 'Falar no WhatsApp',
    badge1Label: 'RESPOSTAS',
    badge1Value: 'Em até 24h',
    badge2Label: 'PARCERIAS',
    badge2Value: '+200 fechadas',
  },
  footerCta: {
    title: 'Pronto para começar a economizar?',
    subtitle: 'Cadastre-se gratuitamente e aproveite cashback em milhares de lojas.',
    primaryText: 'Cadastrar Agora →',
    primaryLink: '/register',
    outlineText: 'Já tenho conta',
    outlineLink: '/login',
  },
};

export function useHomeConfig() {
  const homeCmsStore = useHomeCmsStore();

  const OLD_BRANDS_LABEL = 'GANHE CASHBACK COM AS MELHORES MARCAS';

  const config = computed<HomeConfig>(() => {
    if (homeCmsStore.config) {
      const cms = homeCmsStore.config;
      const brandsLabel = (cms.brands?.label && cms.brands.label !== OLD_BRANDS_LABEL)
        ? cms.brands.label
        : DEFAULT_CONFIG.brands.label;
      return {
        ...DEFAULT_CONFIG,
        ...cms,
        brands: {
          ...DEFAULT_CONFIG.brands,
          ...cms.brands,
          label: brandsLabel,
        },
        ofertas: {
          ...DEFAULT_CONFIG.ofertas,
          ...cms.ofertas,
        },
        testimonials: {
          ...DEFAULT_CONFIG.testimonials,
          ...cms.testimonials,
          items: cms.testimonials?.items?.length ? cms.testimonials.items : DEFAULT_TESTIMONIALS,
        },
        blog: {
          ...DEFAULT_CONFIG.blog,
          ...cms.blog,
          posts: cms.blog?.posts ?? [],
        },
      };
    }
    return DEFAULT_CONFIG;
  });

  async function loadConfig(): Promise<void> {
    await homeCmsStore.fetchConfig();
  }

  return { config, loadConfig };
}
