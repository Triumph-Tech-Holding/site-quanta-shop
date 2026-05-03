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
  brands: SimpleLabelSection;
  ofertas: TitleSubtitleSection;
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
    title: 'Como maximizar seu cashback nas compras online',
    excerpt: 'Dicas e estratégias para acumular mais cashback toda semana.',
    img: 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80&auto=format&fit=crop',
    date: '01 Mai 2025',
    slug: 'como-maximizar-cashback',
  },
  {
    id: 2,
    title: 'Quanta Shop: 3 anos transformando compras em economia real',
    excerpt: 'Conheça nossa trajetória e o impacto gerado para milhares de usuários.',
    img: 'https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=600&q=80&auto=format&fit=crop',
    date: '15 Abr 2025',
    slug: 'quanta-shop-3-anos',
  },
  {
    id: 3,
    title: 'PIX instantâneo: saque seu cashback em segundos',
    excerpt: 'Veja como funciona o saque via PIX e por que ele é o favorito dos usuários.',
    img: 'https://images.unsplash.com/photo-1563986768494-4dee2763ff3f?w=600&q=80&auto=format&fit=crop',
    date: '02 Abr 2025',
    slug: 'pix-instantaneo-cashback',
  },
];

export const DEFAULT_CONFIG: HomeConfig = {
  hero: {
    badge: '+12.000 usuários economizando',
    title: 'Sua Receita por Minuto <highlight>Começa Aqui</highlight>',
    subtitle: 'Ative o cashback com 1 clique e veja seu saldo crescer automaticamente em centenas de lojas.',
    ctaPrimaryText: 'Criar Conta Grátis',
    ctaPrimaryLink: '/register',
  },
  heroCards: [
    { ativo: true, label: 'PIX INSTANTÂNEO', value: 'Saque em segundos ✓', valueColor: 'green', icon: 'card', iconBg: 'teal' },
    { ativo: true, label: 'CASHBACK RECEBIDO', value: 'R$ 50,00', valueColor: 'green', icon: 'chart', iconBg: 'green' },
    { ativo: true, label: 'MARCAS PARCEIRAS', value: '+500 lojas', valueColor: 'teal', icon: 'bag', iconBg: 'teal' },
  ],
  brands: {
    label: 'GANHE CASHBACK COM AS MELHORES MARCAS',
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
    items: DEFAULT_TESTIMONIALS,
  },
  blog: {
    label: 'SEMPRE CONECTADO',
    title: 'Quanta em Tempo Real: Blog e Redes Sociais',
    subtitle: 'Fique por dentro das últimas novidades, promoções e conteúdos exclusivos.',
    posts: DEFAULT_BLOG_POSTS,
  },
  ceo: {
    ativo: true,
    imagemFundo: '',
    overlayOpacity: 0.72,
    posicaoFundo: 'center',
    tag: 'CEO & Founder',
    pre: 'Fale com o CEO',
    name: 'Mauro Triumph',
    desc: 'Clareza estratégica sem rodeios, sem burocracia, sem perda de tempo.',
    ctaText: 'Iniciar Conversa com IA',
    whatsappLink: 'https://api.whatsapp.com/send/?phone=552140404866&text&type=phone_number&app_absent=0',
    whatsappText: 'Falar no WhatsApp',
    badge1Label: 'Respostas',
    badge1Value: 'Em até 24h',
    badge2Label: 'Parcerias',
    badge2Value: '+200 fechadas',
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

export function useHomeConfig() {
  const homeCmsStore = useHomeCmsStore();

  const config = computed<HomeConfig>(() => {
    if (homeCmsStore.config) {
      const cms = homeCmsStore.config;
      return {
        ...DEFAULT_CONFIG,
        ...cms,
        testimonials: {
          ...DEFAULT_CONFIG.testimonials,
          ...cms.testimonials,
          items: cms.testimonials?.items?.length ? cms.testimonials.items : DEFAULT_TESTIMONIALS,
        },
        blog: {
          ...DEFAULT_CONFIG.blog,
          ...cms.blog,
          posts: cms.blog?.posts?.length ? cms.blog.posts : DEFAULT_BLOG_POSTS,
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
