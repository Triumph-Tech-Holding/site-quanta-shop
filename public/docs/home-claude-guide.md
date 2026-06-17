# QUANTA SHOP — Guia completo para Claude: Home Page

## 1. CONTEXTO DO PROJETO

SPA Nuxt.js 3 de plataforma de cashback. Sem SSR (`ssr: false`). Sem Bootstrap. Sem Tailwind. CSS puro com variáveis CSS. A **home page** (`pages/index.vue`) é composta por componentes independentes montados em sequência.

---

## 2. STACK TÉCNICO

| Item | Valor |
|---|---|
| Framework | Nuxt.js 3, `ssr: false` (SPA puro) |
| Linguagem | TypeScript |
| Componentes | Vue 3 Composition API — `<script setup lang="ts">` |
| CSS | SCSS scoped por componente + `assets/scss/quanta-premium.scss` (design system global) |
| Estado | Pinia |
| Slider | Swiper.js (`swiper/vue` — `Swiper`, `SwiperSlide`) |
| HTTP | Composable `useApi` via proxy Nitro |
| CMS | localStorage keys `qs_home_config`, `qs_blog_artigos`, `qs_redes_sociais` |
| Roteamento | `<nuxt-link>` / `useRouter()` |

---

## 3. DESIGN SYSTEM — TOKENS OFICIAIS

```scss
/* Paleta principal */
--qs-primary:      #2F7785;   /* teal */
--qs-primary-dark: #225F6B;   /* teal escuro */
--qs-lime:         #98C73A;   /* verde lima */
--qs-lime-dark:    #7aad1f;
--qs-gold:         #FFB342;   /* estrelas */
--qs-near-black:   #1a2332;

/* Aliases semânticos */
--qs-teal:         #2F7785;
--qs-teal-dark:    #225F6B;
--qs-bg:           #F4F4F5;
--qs-ink:          #1d1d1f;

/* Cinzas */
--qs-gray-50:  #fafafa;
--qs-gray-100: #f5f5f7;
--qs-gray-200: #e5e7eb;
--qs-gray-400: #9ca3af;
--qs-gray-500: #6b7280;
--qs-gray-700: #374151;

/* Estados */
--qs-success: #16a34a;
--qs-warn:    #d97706;
--qs-danger:  #dc2626;

/* Raios, sombras, fontes */
--qs-radius-sm: 6px;
--qs-radius-md: 12px;
--qs-radius-lg: 20px;
--qs-radius-pill: 999px;
--qs-shadow-sm: 0 2px 8px rgba(1,15,28,.08);
--qs-shadow-md: 0 4px 20px rgba(1,15,28,.12);
--qs-font: 'Inter', 'Jost', sans-serif;
```

---

## 4. CONVENÇÕES DE CSS

- Prefixo de classe scoped por componente: 2–3 letras (ex: `hh-`, `fb-`, `qs-social__`)
- **NUNCA** usar `background-image` CSS para imagens externas — usar `<img>` com overlay `<div>` absoluto
- Fontes: `'Inter'` (principal) e `'Jost'` (headings)
- Cores de texto: `#225F6B` (títulos), `#374151` (corpo), `#9ca3af` (muted)
- Fundos de seção alternados: `#fff` e `#f7f8fa`

---

## 5. ORDEM DAS SEÇÕES DA HOME

```
layouts/layout-home.vue          ← encapsula header + conteúdo + footer
  └─ components/header/header-home.vue
  └─ pages/index.vue
       └─ home-hero.vue           (Swiper fullscreen, 6 slides)
       └─ home-brand-logos.vue    (grid 8-col de logos)
       └─ home-ofertas-dia.vue    (Swiper de cards de oferta)
       └─ home-parceiros-online.vue
       └─ home-parceiros-locais.vue
       └─ home-testimonials.vue   (grid 3-col)
       └─ home-blog.vue           (grid 4-col, badges Blog/Instagram/YouTube)
       └─ home-ceo.vue            (card CEO com modal de chat IA)
       └─ home-footer-cta.vue     (CTA final)
  └─ components/footer/footer-home.vue
```

---

## 6. ARQUIVOS COMPLETOS

---

### `pages/index.vue`

```vue
<template>
  <div>
    <home-hero />
    <home-brand-logos />
    <home-ofertas-dia />
    <home-parceiros-online />
    <home-parceiros-locais />
    <home-testimonials />
    <home-blog />
    <home-ceo />
    <home-footer-cta />
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' });

useSeoMeta({
  title: "Quanta Shop — Cashback Real em Centenas de Lojas",
  description: "Ganhe dinheiro de volta em suas compras nas maiores lojas do Brasil.",
  ogTitle: "Quanta Shop — Cashback Real em Centenas de Lojas",
  ogImage: "/logo.png", ogType: "website", twitterCard: "summary_large_image",
});
</script>
```

---

### `composables/useHomeConfig.ts`

```ts
import { computed } from 'vue';
import { useHomeCmsStore } from '@/pinia/useHomeCmsStore';

export interface HeroCard {
  ativo: boolean; label: string; value: string;
  valueColor: 'green' | 'teal' | 'white';
  icon: 'card' | 'chart' | 'bag' | 'star' | 'percent' | 'gift' | 'users' | 'zap';
  iconBg: 'teal' | 'green';
}
export interface HeroSection {
  badge: string; title: string; subtitle: string;
  ctaPrimaryText: string; ctaPrimaryLink: string;
}
export interface BrandItem { nome: string; imagem: string; link?: string; }
export interface BrandsSection { label: string; items?: BrandItem[]; }
export interface OfertasSection { label: string; title: string; subtitle: string; }
export interface LabelTitleSubtitleSection { label: string; title: string; subtitle: string; }
export interface TestimonialItem {
  name: string; initials: string; role: string; highlight: string; text: string;
}
export interface TestimonialsSection { title: string; subtitle: string; items: TestimonialItem[]; }
export interface BlogPostCms {
  id: number; title: string; excerpt: string; img: string;
  date: string; slug: string; type?: 'blog' | 'instagram' | 'youtube';
}
export interface BlogSection { label: string; title: string; subtitle: string; posts: BlogPostCms[]; }
export interface CeoSection {
  ativo?: boolean; imagemFundo?: string; overlayOpacity?: number; posicaoFundo?: string;
  tag: string; pre: string; name: string; desc: string;
  ctaText: string; whatsappLink: string; whatsappText?: string;
  badge1Label?: string; badge1Value?: string; badge2Label?: string; badge2Value?: string;
}
export interface FooterCtaSection {
  title: string; subtitle: string; primaryText: string; primaryLink: string;
  outlineText: string; outlineLink: string;
}
export interface HomeConfig {
  hero: HeroSection; heroCards: HeroCard[]; brands: BrandsSection;
  ofertas: OfertasSection; parceirosOnline: LabelTitleSubtitleSection;
  parceirosLocais: LabelTitleSubtitleSection; testimonials: TestimonialsSection;
  blog: BlogSection; ceo: CeoSection; footerCta: FooterCtaSection;
}

export const DEFAULT_TESTIMONIALS: TestimonialItem[] = [
  { name: 'Marina Costa', initials: 'MC', role: 'Usuária há 8 meses',
    highlight: 'Já recebi mais de R$ 1.240 de cashback',
    text: ' em apenas 6 meses. Nunca mais compro sem a Quanta!' },
  { name: 'Rafael Oliveira', initials: 'RO', role: 'Usuário há 1 ano',
    highlight: 'A cada compra, o dinheiro volta.',
    text: ' Já acumulei R$ 890 sem nenhum esforço extra. Recomendo demais.' },
  { name: 'Juliana Santos', initials: 'JS', role: 'Usuária há 1 ano',
    highlight: 'Minha família economizou R$ 2.100 no último ano.',
    text: ' É dinheiro de verdade voltando pro bolso.' },
];

export const DEFAULT_BLOG_POSTS: BlogPostCms[] = [
  { id:1, title:'Cashback de verdade! Veja como funciona na prática 💰', excerpt:'Descubra como acumular cashback.',
    img:'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80&auto=format&fit=crop',
    date:'22 Mar 2025', slug:'cashback-na-pratica', type:'instagram' },
  { id:2, title:'Tutorial: Como ativar seu cashback em 3 passos', excerpt:'Veja o passo a passo.',
    img:'https://images.unsplash.com/photo-1574375927938-d5a98e8ffe85?w=600&q=80&auto=format&fit=crop',
    date:'20 Mar 2025', slug:'tutorial-ativar-cashback', type:'youtube' },
  { id:3, title:'5 dicas para maximizar seu cashback online', excerpt:'Dicas e estratégias.',
    img:'https://images.unsplash.com/photo-1563986768494-4dee2763ff3f?w=600&q=80&auto=format&fit=crop',
    date:'18 Mar 2025', slug:'maximizar-cashback-online', type:'blog' },
  { id:4, title:'Novas marcas parceiras chegando! 🚀', excerpt:'Confira as novidades.',
    img:'https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=600&q=80&auto=format&fit=crop',
    date:'15 Mar 2025', slug:'novas-marcas-parceiras', type:'instagram' },
  { id:5, title:'Quanta Shop atinge 12 mil usuários ativos', excerpt:'Conheça nossa trajetória.',
    img:'https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=600&q=80&auto=format&fit=crop',
    date:'12 Mar 2025', slug:'quanta-12-mil-usuarios', type:'blog' },
  { id:6, title:'Entrevista exclusiva com o CEO Mauro Triumph', excerpt:'O fundador fala sobre a visão.',
    img:'https://images.unsplash.com/photo-1516738901601-6d0ee099431b?w=600&q=80&auto=format&fit=crop',
    date:'10 Mar 2025', slug:'entrevista-ceo-mauro', type:'youtube' },
  { id:7, title:'Promoção relâmpago: até 15% de cashback hoje! ⚡', excerpt:'Aproveite as promoções especiais.',
    img:'https://images.unsplash.com/photo-1607082348824-0a96f2a4b9da?w=600&q=80&auto=format&fit=crop',
    date:'8 Mar 2025', slug:'promocao-relampago-15', type:'instagram' },
  { id:8, title:'Como o cashback está transformando o varejo brasileiro', excerpt:'Análise do impacto.',
    img:'https://images.unsplash.com/photo-1579621970563-ebec7560ff3e?w=600&q=80&auto=format&fit=crop',
    date:'5 Mar 2025', slug:'cashback-varejo-brasileiro', type:'blog' },
];

export const DEFAULT_CONFIG: HomeConfig = {
  hero: { badge: '+12.000 usuários economizando',
    title: 'Seu dinheiro <highlight>volta</highlight> a cada compra',
    subtitle: 'Compre nas suas lojas favoritas e receba cashback de verdade. Simples, transparente e instantâneo.',
    ctaPrimaryText: 'Criar Conta Grátis', ctaPrimaryLink: '/register' },
  heroCards: [
    { ativo:true, label:'PIX INSTANTÂNEO', value:'Saque em segundos ✓', valueColor:'green', icon:'card', iconBg:'teal' },
    { ativo:true, label:'CASHBACK RECEBIDO', value:'R$ 50,00', valueColor:'green', icon:'chart', iconBg:'green' },
    { ativo:true, label:'MARCAS PARCEIRAS', value:'+500 lojas', valueColor:'teal', icon:'bag', iconBg:'teal' },
  ],
  brands: { label: 'AS MAIORES MARCAS CONFIAM NA QUANTA' },
  ofertas: { label:'TEMPO LIMITADO', title:'Ofertas do Dia',
    subtitle:'Produtos selecionados com cashback turbinado. Aproveite antes que acabe!' },
  parceirosOnline: { label:'Parceiros Online', title:'Compre online e receba cashback',
    subtitle:'Centenas de marcas com cashback garantido. Ative e compre normalmente.' },
  parceirosLocais: { label:'Parceiros Locais', title:'Cashback perto de você',
    subtitle:'Lojas, restaurantes e serviços no seu bairro com cashback automático.' },
  testimonials: { title:'Testemunhos de Usuários Reais',
    subtitle:'Veja quanto nossos usuários já economizaram.', items: DEFAULT_TESTIMONIALS },
  blog: { label:'SEMPRE CONECTADO', title:'Quanta em Tempo Real: Blog e Redes Sociais',
    subtitle:'Fique por dentro das últimas novidades, promoções e conteúdos exclusivos.', posts: [] },
  ceo: { ativo:true, imagemFundo:'', overlayOpacity:0.72, posicaoFundo:'center',
    tag:'CEO & Founder', pre:'FALE COM O CEO', name:'MAURO TRIUMPH',
    desc:'Clareza estratégica sem rodeios, sem burocracia, sem perda de tempo.',
    ctaText:'Iniciar Conversa com IA',
    whatsappLink:'https://api.whatsapp.com/send/?phone=552140404866&text&type=phone_number&app_absent=0',
    whatsappText:'Falar no WhatsApp', badge1Label:'RESPOSTAS', badge1Value:'Em até 24h',
    badge2Label:'PARCERIAS', badge2Value:'+200 fechadas' },
  footerCta: { title:'Pronto para começar a economizar?',
    subtitle:'Cadastre-se gratuitamente e aproveite cashback em milhares de lojas.',
    primaryText:'Cadastrar Agora →', primaryLink:'/register',
    outlineText:'Já tenho conta', outlineLink:'/login' },
};

export function useHomeConfig() {
  const homeCmsStore = useHomeCmsStore();
  const OLD_BRANDS_LABEL = 'GANHE CASHBACK COM AS MELHORES MARCAS';
  const config = computed<HomeConfig>(() => {
    if (homeCmsStore.config) {
      const cms = homeCmsStore.config;
      const brandsLabel = (cms.brands?.label && cms.brands.label !== OLD_BRANDS_LABEL)
        ? cms.brands.label : DEFAULT_CONFIG.brands.label;
      return {
        ...DEFAULT_CONFIG, ...cms,
        brands: { ...DEFAULT_CONFIG.brands, ...cms.brands, label: brandsLabel },
        ofertas: { ...DEFAULT_CONFIG.ofertas, ...cms.ofertas },
        testimonials: { ...DEFAULT_CONFIG.testimonials, ...cms.testimonials,
          items: cms.testimonials?.items?.length ? cms.testimonials.items : DEFAULT_TESTIMONIALS },
        blog: { ...DEFAULT_CONFIG.blog, ...cms.blog, posts: cms.blog?.posts ?? [] },
      };
    }
    return DEFAULT_CONFIG;
  });
  async function loadConfig() { await homeCmsStore.fetchConfig(); }
  return { config, loadConfig };
}
```

---

### `components/header/header-home.vue`

```vue
<template>
  <header class="qs-home-header">
    <div class="qs-home-header__main">
      <div class="container">
        <div class="qs-home-header__row">
          <div class="qs-home-header__logo">
            <nuxt-link href="/"><img src="/img/logo/logo-trimmed.png" alt="Quanta Shop" /></nuxt-link>
          </div>
          <nav class="qs-home-header__nav d-none d-xl-flex">
            <nuxt-link href="/para-voce">Para Você</nuxt-link>
            <nuxt-link href="/para-sua-empresa">Para sua Empresa</nuxt-link>
            <nuxt-link href="/seja-um-agente">Seja um Agente</nuxt-link>
            <nuxt-link href="/quanta-amizade" class="qs-nav-icon-link">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
              Quanta Amizade
            </nuxt-link>
            <nuxt-link href="/blog">Blog</nuxt-link>
            <nuxt-link href="/contato" class="qs-nav-icon-link">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07A19.5 19.5 0 0 1 4.69 12 19.79 19.79 0 0 1 1.61 3.4 2 2 0 0 1 3.6 1.22h3a2 2 0 0 1 2 1.72c.127.96.361 1.903.7 2.81a2 2 0 0 1-.45 2.11L7.91 8.96a16 16 0 0 0 6.13 6.13l.96-.96a2 2 0 0 1 2.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0 1 22 16.92z"/></svg>
              Fale Conosco
            </nuxt-link>
          </nav>
          <div class="qs-home-header__actions">
            <nuxt-link href="/login" class="qs-btn-login">Login</nuxt-link>
            <nuxt-link href="/register" class="qs-btn-cadastro">Cadastro</nuxt-link>
            <button class="qs-mobile-menu-btn d-xl-none" @click="toggleMobile">
              <span></span><span></span><span></span>
            </button>
          </div>
        </div>
      </div>
    </div>

    <div class="qs-home-header__search">
      <div class="container">
        <div class="qs-home-header__search-inner">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#999" stroke-width="2"><circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/></svg>
          <input v-model="searchQuery" type="text"
            placeholder="Busque produtos, grandes marcas ou lojas por CEP..."
            @keyup.enter="handleSearch" />
          <button v-if="searchQuery" @click="searchQuery = ''" class="qs-search-clear">✕</button>
        </div>
      </div>
    </div>

    <div v-if="mobileOpen" class="qs-mobile-menu d-xl-none">
      <nav>
        <nuxt-link href="/para-voce" @click="mobileOpen = false">Para Você</nuxt-link>
        <nuxt-link href="/para-sua-empresa" @click="mobileOpen = false">Para sua Empresa</nuxt-link>
        <nuxt-link href="/seja-um-agente" @click="mobileOpen = false">Seja um Agente</nuxt-link>
        <nuxt-link href="/quanta-amizade" @click="mobileOpen = false">Quanta Amizade</nuxt-link>
        <nuxt-link href="/blog" @click="mobileOpen = false">Blog</nuxt-link>
        <nuxt-link href="/contato" @click="mobileOpen = false">Fale Conosco</nuxt-link>
        <div class="qs-mobile-menu__actions">
          <nuxt-link href="/login" class="qs-btn-login" @click="mobileOpen = false">Login</nuxt-link>
          <nuxt-link href="/register" class="qs-btn-cadastro" @click="mobileOpen = false">Cadastro</nuxt-link>
        </div>
      </nav>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref } from 'vue';
const router = useRouter();
const searchQuery = ref('');
const mobileOpen = ref(false);
function toggleMobile() { mobileOpen.value = !mobileOpen.value; }
function handleSearch() {
  if (searchQuery.value.trim())
    router.push(`/partners?nome=${encodeURIComponent(searchQuery.value.trim())}`);
}
</script>

<style scoped>
.qs-home-header { position: relative; z-index: 1000; }
.qs-home-header__main { background: #fff; border-bottom: 1px solid #f0f0f0; padding: 14px 0; }
.qs-home-header__row { display: flex; align-items: center; gap: 12px; }
.qs-home-header__logo img { height: 38px; width: auto; }
.qs-home-header__nav { flex: 1; align-items: center; gap: 4px; }
.qs-home-header__nav a {
  font-family: 'Inter','Jost',sans-serif; font-size: 13px; font-weight: 500; color: #374151;
  padding: 6px 8px; border-radius: 6px; text-decoration: none; white-space: nowrap; transition: all .2s;
}
.qs-home-header__nav a:hover,
.qs-home-header__nav a.router-link-active { color: #2F7785; background: rgba(47,119,133,.06); }
.qs-nav-icon-link { display: inline-flex !important; align-items: center; gap: 5px; }
.qs-home-header__actions { display: flex; align-items: center; gap: 10px; flex-shrink: 0; }
.qs-btn-login {
  font-family: 'Inter','Jost',sans-serif; font-size: 13px; font-weight: 500; color: #2F7785;
  border: 1.5px solid #2F7785; background: transparent; border-radius: 6px;
  padding: 6px 14px; text-decoration: none; transition: all .2s;
}
.qs-btn-login:hover { background: #2F7785; color: #fff; }
.qs-btn-cadastro {
  font-family: 'Inter','Jost',sans-serif; font-size: 13px; font-weight: 600; color: #fff;
  background: #98C73A; border: 1.5px solid #98C73A; border-radius: 6px;
  padding: 6px 14px; text-decoration: none; transition: all .2s;
}
.qs-btn-cadastro:hover { background: #7aad1f; border-color: #7aad1f; }
.qs-mobile-menu-btn { display: flex; flex-direction: column; gap: 5px; background: none; border: none; cursor: pointer; padding: 4px; }
.qs-mobile-menu-btn span { display: block; width: 24px; height: 2px; background: #374151; border-radius: 2px; }
.qs-home-header__search { background: #fff; padding: 5px 0 6px; border-bottom: 1px solid #f0f0f0; }
.qs-home-header__search-inner {
  display: flex; align-items: center; background: #f7f9fc; border: 1.5px solid #e2e8f0;
  border-radius: 999px; padding: 0 12px; gap: 8px; max-width: 600px; height: 34px;
  margin: 0 auto; box-shadow: 0 1px 4px rgba(47,119,133,.06); transition: all .25s;
}
.qs-home-header__search-inner:focus-within { border-color: #2F7785; background: #fff; box-shadow: 0 2px 10px rgba(47,119,133,.12); }
.qs-home-header__search-inner input { flex: 1; border: none; outline: none; font-family: 'Inter','Jost',sans-serif; font-size: 13px; color: #374151; background: transparent; }
.qs-home-header__search-inner input::placeholder { color: #9ca3af; }
.qs-search-clear { background: none; border: none; cursor: pointer; color: #9ca3af; font-size: 14px; }
.qs-mobile-menu { position: absolute; top: 100%; left: 0; right: 0; background: #fff; box-shadow: 0 8px 30px rgba(0,0,0,.12); z-index: 999; }
.qs-mobile-menu nav { display: flex; flex-direction: column; padding: 12px 0; }
.qs-mobile-menu nav a { font-family: 'Inter','Jost',sans-serif; font-size: 15px; font-weight: 500; color: #374151; padding: 12px 24px; text-decoration: none; border-bottom: 1px solid #f3f4f6; }
.qs-mobile-menu nav a:hover { color: #2F7785; background: rgba(47,119,133,.04); }
.qs-mobile-menu__actions { display: flex; gap: 12px; padding: 16px 24px; }
@media (max-width: 575px) { .qs-home-header__logo img { height: 30px; } .qs-btn-login, .qs-btn-cadastro { font-size: 12px; padding: 5px 10px; } }
</style>
```

---

### `components/home/home-brand-logos.vue`

```vue
<template>
  <section class="qs-brands">
    <div class="container">
      <p class="qs-brands__label">{{ config.brands.label }}</p>
      <div class="qs-brands__grid">
        <div
          v-for="brand in displayBrands" :key="brand.nome"
          class="qs-brands__item" :title="brand.nome"
          role="button" tabindex="0" style="cursor:pointer"
          @click="handleBrandClick(brand)" @keydown.enter="handleBrandClick(brand)"
        >
          <img :src="brand.imagem || brand.imagemPequena || '/img/placeholder.png'"
            :alt="brand.nome" loading="lazy"
            @error="(e) => (e.target as HTMLImageElement).style.opacity = '0'" />
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';
import { usePartnerStore } from '@/pinia/usePartnerStore';
import { useUserStore } from '@/pinia/useUserStore';
import { useRouter } from 'vue-router';

const { config, loadConfig } = useHomeConfig();
const partnerStore = usePartnerStore();
const userStore = useUserStore();
const router = useRouter();

const FALLBACK_BRANDS = [
  { nome:'Nike', imagem:'https://logo.clearbit.com/nike.com', link:'' },
  { nome:'Renner', imagem:'https://logo.clearbit.com/lojasrenner.com.br', link:'' },
  { nome:'Puma', imagem:'https://logo.clearbit.com/puma.com', link:'' },
  { nome:'Casas Bahia', imagem:'https://logo.clearbit.com/casasbahia.com.br', link:'' },
  { nome:'Cobasi', imagem:'https://logo.clearbit.com/cobasi.com.br', link:'' },
  { nome:'MadeiraMadeira', imagem:'https://logo.clearbit.com/madeiramadeira.com.br', link:'' },
  { nome:'Dafiti', imagem:'https://logo.clearbit.com/dafiti.com.br', link:'' },
  { nome:'Vivara', imagem:'https://logo.clearbit.com/vivara.com.br', link:'' },
  { nome:'LG', imagem:'https://logo.clearbit.com/lg.com', link:'' },
  { nome:'Motorola', imagem:'https://logo.clearbit.com/motorola.com', link:'' },
  { nome:'Pernambucanas', imagem:'https://logo.clearbit.com/pernambucanas.com.br', link:'' },
  { nome:'Tok&Stok', imagem:'https://logo.clearbit.com/tokstok.com.br', link:'' },
  { nome:'Olympikus', imagem:'https://logo.clearbit.com/olympikus.com.br', link:'' },
  { nome:'Under Armour', imagem:'https://logo.clearbit.com/underarmour.com', link:'' },
  { nome:'Shoptime', imagem:'https://logo.clearbit.com/shoptime.com.br', link:'' },
  { nome:'Mizuno', imagem:'https://logo.clearbit.com/mizuno.com', link:'' },
];

const displayBrands = computed(() => {
  const apiPartners = (partnerStore.newPartners || []).slice(0, 16);
  if (apiPartners.length >= 8) return apiPartners;
  const configItems = config.value.brands?.items;
  if (configItems && configItems.length >= 4) return configItems.slice(0, 16);
  return FALLBACK_BRANDS;
});

function handleBrandClick(brand: any) {
  if (!userStore.isLoggedIn) { router.push('/login'); return; }
  if (brand.link) window.open(brand.link.replace('{userId}', userStore.userId || ''), '_blank', 'noopener');
}

onMounted(async () => {
  await loadConfig();
  try { await partnerStore.fetchNewPartners(); } catch {}
});
</script>

<style scoped>
.qs-brands { padding: 40px 0 36px; background: #fff; border-top: 1px solid #f0f0f0; border-bottom: 1px solid #f0f0f0; }
.qs-brands__label { text-align: center; font-family: 'Inter','Jost',sans-serif; font-size: 11px; font-weight: 700; letter-spacing: .14em; text-transform: uppercase; color: #9ca3af; margin-bottom: 28px; }
.qs-brands__grid { display: grid; grid-template-columns: repeat(8,1fr); gap: 0; }
@media (max-width: 991px) { .qs-brands__grid { grid-template-columns: repeat(6,1fr); } }
@media (max-width: 767px) { .qs-brands__grid { grid-template-columns: repeat(4,1fr); } }
@media (max-width: 479px) { .qs-brands__grid { grid-template-columns: repeat(3,1fr); } }
.qs-brands__item { display: flex; align-items: center; justify-content: center; padding: 16px 20px; border-right: 1px solid #f0f4f6; border-bottom: 1px solid #f0f4f6; transition: background .2s; min-height: 72px; }
.qs-brands__item:hover { background: #f7fbfc; }
.qs-brands__item img { max-width: 90px; max-height: 36px; object-fit: contain; filter: grayscale(100%) opacity(.65); transition: filter .25s; }
.qs-brands__item:hover img { filter: grayscale(0%) opacity(1); }
</style>
```

---

### `components/home/home-testimonials.vue`

```vue
<template>
  <section class="qs-testimonials">
    <div class="container">
      <div class="qs-section-header">
        <h2 class="qs-section-title">{{ config.testimonials.title }}</h2>
        <p class="qs-section-sub">{{ config.testimonials.subtitle }}</p>
      </div>
      <div class="qs-testimonials__grid">
        <div v-for="t in testimonials" :key="t.name" class="qs-testimonial-card">
          <div class="qs-testimonial-card__stars">
            <svg v-for="n in 5" :key="n" width="14" height="14" viewBox="0 0 24 24" fill="#FFB342"><path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/></svg>
          </div>
          <p class="qs-testimonial-card__text">
            <span class="qs-testimonial-card__highlight">{{ t.highlight }}</span>
            {{ t.text }}
          </p>
          <div class="qs-testimonial-card__author">
            <div class="qs-testimonial-card__avatar">{{ t.initials }}</div>
            <div>
              <div class="qs-testimonial-card__name">{{ t.name }}</div>
              <div class="qs-testimonial-card__role">{{ t.role }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useHomeConfig, DEFAULT_TESTIMONIALS } from '@/composables/useHomeConfig';
const { config, loadConfig } = useHomeConfig();
onMounted(() => loadConfig());
const testimonials = computed(() =>
  config.value.testimonials?.items?.length ? config.value.testimonials.items : DEFAULT_TESTIMONIALS
);
</script>

<style scoped>
.qs-testimonials { padding: 48px 0; background: #f7f8fa; }
.qs-section-header { text-align: center; margin-bottom: 40px; }
.qs-section-title { font-family: 'Inter','Jost',sans-serif; font-size: clamp(24px,4vw,36px); font-weight: 800; color: #225F6B; letter-spacing: -.03em; margin-bottom: 8px; }
.qs-section-sub { font-family: 'Inter','Jost',sans-serif; font-size: 15px; color: #2F7785; }
.qs-testimonials__grid { display: grid; grid-template-columns: repeat(3,1fr); gap: 20px; }
@media (max-width: 991px) { .qs-testimonials__grid { grid-template-columns: repeat(2,1fr); } }
@media (max-width: 575px) { .qs-testimonials__grid { grid-template-columns: 1fr; } }
.qs-testimonial-card { background: #fff; border-radius: 16px; padding: 28px; box-shadow: 0 2px 12px rgba(0,0,0,.06); display: flex; flex-direction: column; gap: 16px; }
.qs-testimonial-card__stars { display: flex; gap: 2px; }
.qs-testimonial-card__text { font-family: 'Inter','Jost',sans-serif; font-size: 14px; color: #374151; line-height: 1.6; margin: 0; }
.qs-testimonial-card__highlight { color: #225F6B; font-weight: 700; }
.qs-testimonial-card__author { display: flex; align-items: center; gap: 12px; margin-top: auto; }
.qs-testimonial-card__avatar { width: 40px; height: 40px; border-radius: 50%; background: linear-gradient(135deg,#2F7785,#225F6B); color: #fff; font-family: 'Inter','Jost',sans-serif; font-size: 14px; font-weight: 700; display: flex; align-items: center; justify-content: center; flex-shrink: 0; }
.qs-testimonial-card__name { font-family: 'Inter','Jost',sans-serif; font-size: 13px; font-weight: 700; color: #225F6B; }
.qs-testimonial-card__role { font-size: 12px; color: #9ca3af; }
</style>
```

---

### `components/home/home-blog.vue`

```vue
<template>
  <section class="qs-social">
    <div class="container">
      <div class="qs-social__header">
        <span class="qs-social__label">{{ config.blog.label }}</span>
        <h2 class="qs-social__title">{{ config.blog.title }}</h2>
        <p class="qs-social__sub">{{ config.blog.subtitle }}</p>
      </div>

      <div class="qs-blog-grid">
        <component
          :is="post.externalUrl ? 'a' : 'NuxtLink'"
          v-for="post in allPosts" :key="post.id"
          v-bind="post.externalUrl
            ? { href: post.externalUrl, target:'_blank', rel:'noopener' }
            : { to: `/blog/${post.slug || post.id}` }"
          class="qs-blog-card2"
        >
          <div class="qs-blog-card2__img-wrap">
            <img :src="post.img" :alt="post.title" loading="lazy" />
            <span class="qs-feed-badge" :class="{
              'qs-feed-badge--instagram': post.type === 'instagram',
              'qs-feed-badge--youtube': post.type === 'youtube',
              'qs-feed-badge--blog': !post.type || post.type === 'blog'
            }">
              <svg v-if="post.type === 'instagram'" width="10" height="10" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z"/></svg>
              <svg v-else-if="post.type === 'youtube'" width="10" height="10" viewBox="0 0 24 24" fill="currentColor"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/></svg>
              <svg v-else width="10" height="10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M4 22h16a2 2 0 0 0 2-2V4a2 2 0 0 0-2-2H8a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2z"/><path d="M16 2v4"/><path d="M8 2v4"/><path d="M3 10h18"/></svg>
              {{ post.type === 'instagram' ? 'Instagram' : post.type === 'youtube' ? 'YouTube' : 'Blog' }}
            </span>
          </div>
          <div class="qs-blog-card2__body">
            <h3 class="qs-blog-card2__title">{{ post.title }}</h3>
            <div class="qs-blog-card2__foot">
              <span v-if="post.date" class="qs-blog-card2__date">{{ post.date }}</span>
              <span class="qs-blog-card2__read">
                Ler mais
                <svg width="11" height="11" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14"/><path d="m12 5 7 7-7 7"/></svg>
              </span>
            </div>
          </div>
        </component>
      </div>

      <div v-if="allPosts.length === 0" class="qs-blog-empty">
        <p>Nenhum conteúdo publicado ainda.</p>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useHomeConfig, DEFAULT_BLOG_POSTS, type BlogPostCms } from '@/composables/useHomeConfig';

const { config, loadConfig } = useHomeConfig();

interface UnifiedPost {
  id: number | string; slug: string; title: string; date: string;
  img: string; type: 'blog' | 'instagram' | 'youtube'; externalUrl?: string;
}

const allPosts = ref<UnifiedPost[]>([]);

function lsArtigosBlog(): UnifiedPost[] {
  try {
    const raw = localStorage.getItem('qs_blog_artigos');
    if (!raw) return [];
    const artigos = JSON.parse(raw) as Array<{
      id: number; titulo: string; imagemDestaque?: string;
      dataPublicacao?: string; slug: string; publicado: boolean;
    }>;
    return artigos.filter(a => a.publicado)
      .sort((a, b) => {
        const da = a.dataPublicacao ? new Date(a.dataPublicacao).getTime() : 0;
        const db = b.dataPublicacao ? new Date(b.dataPublicacao).getTime() : 0;
        return db - da;
      })
      .map(a => ({
        id: a.id, slug: a.slug || String(a.id), title: a.titulo,
        date: a.dataPublicacao
          ? new Date(a.dataPublicacao).toLocaleDateString('pt-BR', { day:'2-digit', month:'short', year:'numeric' })
          : '',
        img: a.imagemDestaque || 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80',
        type: 'blog' as const,
      }));
  } catch { return []; }
}

function lsPostsSocial(): UnifiedPost[] {
  try {
    const raw = localStorage.getItem('qs_redes_sociais');
    if (!raw) return [];
    const posts = JSON.parse(raw) as Array<{
      id: number; plataforma: string; titulo: string; url: string; thumbnailUrl?: string; ativo: boolean;
    }>;
    return posts.filter(p => p.ativo).map(p => ({
      id: p.id, slug: String(p.id), title: p.titulo, date: '',
      img: p.thumbnailUrl || 'https://images.unsplash.com/photo-1611162616305-c69b3fa7fbe0?w=600&q=80',
      type: (p.plataforma?.toLowerCase() === 'youtube' ? 'youtube' : 'instagram') as 'youtube' | 'instagram',
      externalUrl: p.url,
    }));
  } catch { return []; }
}

onMounted(async () => {
  await loadConfig();
  const blog = lsArtigosBlog();
  const social = lsPostsSocial();
  if (blog.length > 0 || social.length > 0) {
    allPosts.value = [...blog, ...social].slice(0, 8);
    return;
  }
  const cmsPosts = config.value.blog?.posts;
  if (cmsPosts && cmsPosts.length > 0) {
    allPosts.value = cmsPosts.map((p: BlogPostCms) => ({
      id: p.id, slug: p.slug, title: p.title, date: p.date, img: p.img,
      type: (p.type || 'blog') as 'blog' | 'instagram' | 'youtube',
    }));
    return;
  }
  allPosts.value = DEFAULT_BLOG_POSTS.map((p: BlogPostCms) => ({
    id: p.id, slug: p.slug, title: p.title, date: p.date, img: p.img,
    type: (p.type || 'blog') as 'blog' | 'instagram' | 'youtube',
  }));
});
</script>

<style scoped>
.qs-social { padding: 72px 0; background: #f9fafb; }
.qs-social__header { text-align: center; margin-bottom: 52px; }
.qs-social__label { display: block; font-family: 'Jost','Inter',sans-serif; font-size: 11px; font-weight: 700; letter-spacing: .15em; text-transform: uppercase; color: #2F7785; margin-bottom: 12px; }
.qs-social__title { font-family: 'Jost','Inter',sans-serif; font-size: clamp(24px,4vw,38px); font-weight: 800; color: #225F6B; letter-spacing: -.02em; line-height: 1.2; margin-bottom: 12px; }
.qs-social__sub { font-size: 15px; color: #6b7280; max-width: 540px; margin: 0 auto; }
.qs-blog-grid { display: grid; grid-template-columns: repeat(4,1fr); gap: 20px; }
@media (max-width: 1100px) { .qs-blog-grid { grid-template-columns: repeat(3,1fr); } }
@media (max-width: 767px) { .qs-blog-grid { grid-template-columns: repeat(2,1fr); } }
@media (max-width: 479px) { .qs-blog-grid { grid-template-columns: 1fr; } }
.qs-blog-card2 { background: #fff; border-radius: 14px; overflow: hidden; text-decoration: none; color: inherit; box-shadow: 0 2px 8px rgba(0,0,0,.06); transition: transform .25s, box-shadow .25s; display: flex; flex-direction: column; }
.qs-blog-card2:hover { transform: translateY(-5px); box-shadow: 0 12px 40px rgba(47,119,133,.15); }
.qs-blog-card2__img-wrap { position: relative; overflow: hidden; height: 180px; flex-shrink: 0; }
.qs-blog-card2__img-wrap img { width: 100%; height: 100%; object-fit: cover; transition: transform .4s; display: block; }
.qs-blog-card2:hover .qs-blog-card2__img-wrap img { transform: scale(1.06); }
.qs-blog-card2__body { padding: 16px; flex: 1; display: flex; flex-direction: column; gap: 10px; }
.qs-blog-card2__title { font-family: 'Jost','Inter',sans-serif; font-size: 14px; font-weight: 700; color: #225F6B; line-height: 1.4; margin: 0; flex: 1; display: -webkit-box; -webkit-line-clamp: 3; -webkit-box-orient: vertical; overflow: hidden; }
.qs-blog-card2__foot { display: flex; align-items: center; justify-content: space-between; gap: 8px; margin-top: auto; }
.qs-blog-card2__date { font-size: 11px; color: #aeaeb2; }
.qs-blog-card2__read { display: inline-flex; align-items: center; gap: 4px; font-size: 12px; font-weight: 600; color: #2F7785; transition: gap .2s; white-space: nowrap; }
.qs-blog-card2:hover .qs-blog-card2__read { gap: 7px; }
.qs-feed-badge { position: absolute; top: 10px; left: 10px; display: inline-flex; align-items: center; gap: 4px; padding: 3px 9px; border-radius: 999px; font-size: 10px; font-weight: 700; color: #fff; z-index: 1; }
.qs-feed-badge--blog { background: #2F7785; }
.qs-feed-badge--instagram { background: #E1306C; }
.qs-feed-badge--youtube { background: #FF0000; }
.qs-blog-empty { display: flex; flex-direction: column; align-items: center; gap: 12px; padding: 60px 0; color: #9ca3af; font-size: 14px; }
</style>
```

---

### `components/home/home-footer-cta.vue`

```vue
<template>
  <section class="qs-footer-cta">
    <div class="container">
      <h2 class="qs-footer-cta__title">{{ config.footerCta.title }}</h2>
      <p class="qs-footer-cta__sub">{{ config.footerCta.subtitle }}</p>
      <div class="qs-footer-cta__actions">
        <nuxt-link :href="config.footerCta.primaryLink" class="qs-footer-cta__btn-primary">
          {{ config.footerCta.primaryText }}
          <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </nuxt-link>
        <nuxt-link :href="config.footerCta.outlineLink" class="qs-footer-cta__btn-outline">
          {{ config.footerCta.outlineText }}
        </nuxt-link>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';
const { config, loadConfig } = useHomeConfig();
onMounted(() => loadConfig());
</script>

<style scoped>
.qs-footer-cta { padding: 48px 0; background: linear-gradient(135deg,#f7f8fa 0%,#fff 50%,#f7f8fa 100%); text-align: center; border-top: 1px solid #e5e7eb; }
.qs-footer-cta__title { font-family: 'Inter','Jost',sans-serif; font-size: clamp(28px,4vw,42px); font-weight: 800; color: #225F6B; letter-spacing: -.03em; margin-bottom: 12px; }
.qs-footer-cta__sub { font-family: 'Inter','Jost',sans-serif; font-size: 16px; color: #2F7785; margin-bottom: 32px; }
.qs-footer-cta__actions { display: flex; align-items: center; justify-content: center; gap: 14px; flex-wrap: wrap; }
.qs-footer-cta__btn-primary { display: inline-flex; align-items: center; gap: 8px; background: #98C73A; color: #fff; font-family: 'Inter','Jost',sans-serif; font-size: 15px; font-weight: 700; padding: 14px 32px; border-radius: 8px; text-decoration: none; transition: all .2s; box-shadow: 0 4px 20px rgba(152,199,58,.35); }
.qs-footer-cta__btn-primary:hover { background: #7aad1f; color: #fff; transform: translateY(-2px); }
.qs-footer-cta__btn-outline { display: inline-flex; align-items: center; gap: 8px; border: 2px solid #e5e7eb; color: #374151; font-family: 'Inter','Jost',sans-serif; font-size: 15px; font-weight: 600; padding: 14px 32px; border-radius: 8px; text-decoration: none; transition: all .2s; }
.qs-footer-cta__btn-outline:hover { border-color: #2F7785; color: #2F7785; }
</style>
```

---

### `components/footer/footer-home.vue`

```vue
<template>
  <footer class="qs-footer">
    <div class="qs-footer__top">
      <div class="container">
        <div class="qs-footer__grid">
          <div class="qs-footer__col qs-footer__col--brand">
            <nuxt-link href="/"><img src="/img/logo/logo-white.png" alt="Quanta Shop" class="qs-footer__logo" /></nuxt-link>
            <p class="qs-footer__desc">Somos uma equipe dedicada a oferecer as melhores oportunidades de economia e renda através de um consumo inteligente.</p>
            <div class="qs-footer__socials">
              <a href="https://www.instagram.com/quantashop.oficial/" target="_blank" rel="noopener" aria-label="Instagram" class="qs-footer__social">
                <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z"/></svg>
              </a>
              <a href="https://www.facebook.com/quantashop" target="_blank" rel="noopener" aria-label="Facebook" class="qs-footer__social">
                <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z"/></svg>
              </a>
              <a href="https://www.linkedin.com/company/quanta-shop" target="_blank" rel="noopener" aria-label="LinkedIn" class="qs-footer__social">
                <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433a2.062 2.062 0 01-2.063-2.065 2.064 2.064 0 112.063 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z"/></svg>
              </a>
              <a href="https://www.youtube.com/@QuantaShop" target="_blank" rel="noopener" aria-label="YouTube" class="qs-footer__social">
                <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/></svg>
              </a>
            </div>
          </div>

          <div class="qs-footer__col">
            <h4 class="qs-footer__heading">Minha Conta</h4>
            <ul class="qs-footer__links">
              <li><nuxt-link to="/agencia/painel/minhas-compras">Minhas compras</nuxt-link></li>
              <li><nuxt-link to="/agencia/painel/financeiro">Meu extrato</nuxt-link></li>
              <li><nuxt-link to="/agencia/painel/minha-rede">Meus indicados</nuxt-link></li>
              <li><nuxt-link to="/agencia/painel/meus-dados">Minha conta</nuxt-link></li>
              <li><nuxt-link to="/agencia/painel/assinatura">Assinatura</nuxt-link></li>
            </ul>
          </div>

          <div class="qs-footer__col">
            <h4 class="qs-footer__heading">Empresa</h4>
            <ul class="qs-footer__links">
              <li><a href="/about">Quem somos</a></li>
              <li><nuxt-link to="/agencia/como-funciona">Como funciona</nuxt-link></li>
              <li><nuxt-link to="/agencia/cadastro">Credenciamento</nuxt-link></li>
              <li><nuxt-link to="/primeira-compra">Seja um Agente</nuxt-link></li>
              <li><a href="/contact">Contato</a></li>
            </ul>
          </div>

          <div class="qs-footer__col">
            <h4 class="qs-footer__heading">Suporte</h4>
            <ul class="qs-footer__links">
              <li><a href="https://bigcash.blob.core.windows.net/documentos/QB-%20TERMOS.pdf" target="_blank">Termos e condições</a></li>
              <li><a href="/blog">Blog</a></li>
              <li><a href="https://api.whatsapp.com/send/?phone=552140404866" target="_blank">WhatsApp</a></li>
              <li><nuxt-link to="/agencia/painel/assinatura">Quanta Plus</nuxt-link></li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="qs-footer__bottom">
      <div class="container">
        <div class="qs-footer__bottom-row">
          <p>© {{ new Date().getFullYear() }} Quanta Shop — Todos os direitos reservados</p>
          <img src="/img/footer/footer-pay.png" alt="Métodos de pagamento" class="qs-footer__pay" />
        </div>
      </div>
    </div>
  </footer>
</template>

<style scoped>
.qs-footer { background: #111827; color: #d1d5db; }
.qs-footer__top { padding: 64px 0 40px; }
.qs-footer__grid { display: grid; grid-template-columns: 2fr 1fr 1fr 1fr; gap: 40px; }
@media (max-width: 991px) { .qs-footer__grid { grid-template-columns: repeat(2,1fr); } }
@media (max-width: 575px) { .qs-footer__grid { grid-template-columns: 1fr; } }
.qs-footer__logo { height: 36px; width: auto; margin-bottom: 16px; display: block; }
.qs-footer__desc { font-family: 'Inter','Jost',sans-serif; font-size: 13px; color: #9ca3af; line-height: 1.6; margin-bottom: 20px; }
.qs-footer__socials { display: flex; gap: 10px; }
.qs-footer__social { width: 36px; height: 36px; border-radius: 8px; background: rgba(255,255,255,.07); color: #9ca3af; display: flex; align-items: center; justify-content: center; text-decoration: none; transition: all .2s; }
.qs-footer__social:hover { background: #2F7785; color: #fff; }
.qs-footer__heading { font-family: 'Inter','Jost',sans-serif; font-size: 13px; font-weight: 700; color: #fff; text-transform: uppercase; letter-spacing: .08em; margin-bottom: 16px; }
.qs-footer__links { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 10px; }
.qs-footer__links a { font-family: 'Inter','Jost',sans-serif; font-size: 13px; color: #9ca3af; text-decoration: none; transition: color .2s; }
.qs-footer__links a:hover { color: #98C73A; }
.qs-footer__bottom { border-top: 1px solid rgba(255,255,255,.06); padding: 20px 0; }
.qs-footer__bottom-row { display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 12px; }
.qs-footer__bottom-row p { font-family: 'Inter','Jost',sans-serif; font-size: 12px; color: #6b7280; margin: 0; }
.qs-footer__pay { height: 24px; width: auto; opacity: .6; }
</style>
```

---

## 7. HERO — SWIPER E ESTRUTURA DE SLIDES

As seções `home-hero.vue` e `home-ofertas-dia.vue` usam Swiper:

```ts
// Importação padrão Swiper em SFCs:
import { Swiper, SwiperSlide } from 'swiper/vue';
import { Navigation, Pagination, Autoplay } from 'swiper/modules';
// CSS importado em main.scss: 'swiper/css', 'swiper/css/navigation', 'swiper/css/pagination'
```

**Hero config:** `slidesPerView:1`, `loop:true`, `autoplay:{delay:6000}`, dots customizados via `.qs-hero-dots`, setas via `.qs-hero-next/.qs-hero-prev`

**Ofertas config:** `slidesPerView:1.2`, breakpoints `640→2`, `1024→3.2`, `spaceBetween:20`

### Estrutura de dados `public/data/hero-banners.json`

Cada slide do hero tem os campos:

```json
{
  "id": 1,
  "titulo": "Nome do slide (admin apenas)",
  "url": "/uploads/banners/arquivo.png",
  "ativo": true,
  "headline": "Texto com <highlight>palavra</highlight> em destaque",
  "subtitulo": "Subtítulo do slide",
  "badge": "Texto do pill badge",
  "badgeCor": "",
  "headlineCor": "#f4f4f5",
  "subtituloCor": "#e5f0f2",
  "ctaTexto": "Criar Conta Grátis",
  "ctaLink": "/register",
  "ctaCor": "#98C73A",
  "ctaTextoCor": "#225f6b",
  "textoCor": "light",
  "overlayIntensidade": 75,
  "overlayCor": "#225f6b",
  "overlayDirecao": "esquerda",
  "objectPosition": "50% 30%",
  "tituloFontSize": "grande",
  "ctaAlinhamento": "esquerda"
}
```

A tag `<highlight>` é transformada em `<span style="color:#98C73A">` pelo método `getSlideTitle()` no hero.

---

## 8. REGRAS QUE O CLAUDE DEVE SEGUIR

1. **Sem Bootstrap, sem Tailwind.** CSS puro scoped.
2. **Prefixo de classe por componente:** `qs-brands__`, `qs-social__`, `qs-footer__`. Nunca misturar prefixos entre componentes.
3. **Responsivo mobile-first** com `@media (max-width: ...)` no `<style scoped>`.
4. **Fontes:** `font-family: 'Inter', 'Jost', sans-serif` em todo texto.
5. **Cores exclusivas:** `#2F7785` (teal), `#225F6B` (teal escuro), `#98C73A` (lime), `#374151` (texto), `#9ca3af` (muted), `#f9fafb` (bg cinza), `#111827` (footer).
6. **Imagens externas:** sempre `<img>`, nunca `background-image` CSS para URLs externas.
7. **CMS:** dados chegam via `useHomeConfig()` — sempre fornecer fallback/default.
8. **Skeleton loading:** divs com class `qs-skeleton` (fundo `#e5e7eb` com shimmer animation) enquanto dados carregam.
9. **Hover:** `transform: translateY(-4px)` + `box-shadow` ampliado para cards. `transition: all 0.25s ease`.
10. **Botões primários:** fundo `#98C73A`, texto branco ou `#225F6B`. **Botões outline:** borda `#2F7785`, fundo transparente.
11. **`<highlight>`** no hero title é substituído por `<span style="color:#98C73A">` via `v-html`.
12. **Auto-import Nuxt:** `useRouter`, `useHead`, `useSeoMeta`, `definePageMeta` são globais — não precisam de import. Componentes em `components/` são auto-importados.
