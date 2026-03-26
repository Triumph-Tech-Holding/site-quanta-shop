<template>
  <section class="qs-hero" style="max-height: 60vh; overflow: hidden;">
    <div v-if="sliderData.length === 0" class="qs-hero__loading">
      <img src="/img/ui/loading.gif" alt="Carregando..." width="48" />
    </div>

    <Swiper
      v-if="sliderData.length > 0"
      :slidesPerView="1"
      :loop="true"
      :autoplay="{ delay: 6000, disableOnInteraction: false }"
      :navigation="{ nextEl: '.qs-hero-next', prevEl: '.qs-hero-prev' }"
      :pagination="{ el: '.qs-hero-dots', clickable: true }"
      :modules="[Navigation, Pagination, Autoplay]"
      class="qs-hero__swiper"
    >
      <SwiperSlide v-for="(item, i) in sliderData" :key="i" class="qs-hero__slide">
        <div
          class="qs-hero__bg"
          :style="item.url ? { backgroundImage: `url(${item.url})`, backgroundPosition: item.objectPosition || '50% 50%' } : undefined"
        ></div>
        <div
          class="qs-hero__overlay"
          :style="{ background: getOverlayGradient(item), opacity: item.overlayIntensidade != null ? item.overlayIntensidade / 100 : undefined }"
        ></div>

        <div class="container qs-hero__content-wrap">
          <div class="row align-items-center qs-hero__row">
            <div class="col-xl-6 col-lg-7">
              <div class="qs-hero__content" :class="item.textoCor === 'dark' ? 'qs-hero__content--dark' : ''">
                <span class="qs-hero__badge">
                  <span class="qs-hero__badge-dot"></span>
                  {{ item.badge || config.hero.badge }}
                </span>

                <h1 class="qs-hero__title" :style="{ fontSize: getTitleFontSize(item) }" v-html="getSlideTitle(item)"></h1>

                <p class="qs-hero__subtitle">
                  {{ item.subtitulo || config.hero.subtitle }}
                </p>

                <div
                  class="qs-hero__actions"
                  :style="item.ctaAlinhamento === 'direita' ? { justifyContent: 'flex-end' } : item.ctaAlinhamento === 'centro' ? { justifyContent: 'center' } : {}"
                >
                  <nuxt-link
                    :href="item.ctaLink || config.hero.ctaPrimaryLink"
                    class="qs-hero__cta"
                    :style="item.ctaCor ? { background: item.ctaCor, borderColor: item.ctaCor } : undefined"
                  >
                    {{ item.ctaTexto || config.hero.ctaPrimaryText }}
                    <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
                  </nuxt-link>
                </div>

                <div class="qs-hero__social-proof">
                  <div class="qs-hero__avatars">
                    <img src="/img/users/user-1.jpg" alt="Usuário" />
                    <img src="/img/users/user-2.jpg" alt="Usuário" />
                    <img src="/img/users/user-3.jpg" alt="Usuário" />
                    <img src="/img/users/user-4.jpg" alt="Usuário" />
                  </div>
                  <span class="qs-hero__rating">4.9 ★ — Avaliação dos usuários</span>
                </div>
              </div>
            </div>

            <div class="col-xl-6 col-lg-5 d-none d-lg-flex justify-content-end align-items-center">
              <div class="qs-hero__cards">
                <div class="qs-hero__card">
                  <div class="qs-hero__card-icon">
                    <svg width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2"><rect x="2" y="5" width="20" height="14" rx="2"/><path d="M2 10h20"/></svg>
                  </div>
                  <div>
                    <div class="qs-hero__card-label">PIX INSTANTÂNEO</div>
                    <div class="qs-hero__card-value qs-hero__card-value--green">Saque em segundos ✓</div>
                  </div>
                </div>

                <div class="qs-hero__card qs-hero__card--offset">
                  <div class="qs-hero__card-icon qs-hero__card-icon--green">
                    <svg width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="#fff" stroke-width="2"><polyline points="22 7 13.5 15.5 8.5 10.5 2 17"/><polyline points="16 7 22 7 22 13"/></svg>
                  </div>
                  <div>
                    <div class="qs-hero__card-label">CASHBACK RECEBIDO</div>
                    <div class="qs-hero__card-value qs-hero__card-value--green">R$ 50,00</div>
                  </div>
                </div>

                <div class="qs-hero__card">
                  <div class="qs-hero__card-icon">
                    <svg width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2"><path d="M6 2L3 6v14a2 2 0 002 2h14a2 2 0 002-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 01-8 0"/></svg>
                  </div>
                  <div>
                    <div class="qs-hero__card-label">MARCAS PARCEIRAS</div>
                    <div class="qs-hero__card-value">+500 lojas</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="qs-hero-dots"></div>
        <div class="qs-hero-arrows d-none d-lg-flex">
          <button class="qs-hero-prev">
            <svg width="20" height="20" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M15 18l-6-6 6-6"/></svg>
          </button>
          <button class="qs-hero-next">
            <svg width="20" height="20" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M9 18l6-6-6-6"/></svg>
          </button>
        </div>
      </SwiperSlide>
    </Swiper>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useCarouselStore } from "@/pinia/useCarouselStore";
import { Swiper, SwiperSlide } from "swiper/vue";
import { Navigation, Pagination, Autoplay } from "swiper/modules";
import { useHomeConfig } from '@/composables/useHomeConfig';
import type { HeroBannerSlide } from '~/types/agencia';

const { config, loadConfig } = useHomeConfig();
const carouselStore = useCarouselStore();
const heroSlides = ref<HeroBannerSlide[]>([]);

function getSlideTitle(item: HeroBannerSlide): string {
  const raw = item.headline || config.value.hero.title;
  return raw
    .replace('<highlight>', '<span style="color:#98C73A">')
    .replace('</highlight>', '</span>');
}

function getOverlayGradient(item: HeroBannerSlide): string {
  const d = item.overlayDirecao || 'esquerda';
  if (d === 'direita') return 'linear-gradient(to left, rgba(15,35,45,0.88) 0%, rgba(15,35,45,0.65) 45%, rgba(15,35,45,0.30) 100%)';
  if (d === 'centro') return 'linear-gradient(to right, rgba(15,35,45,0.20) 0%, rgba(15,35,45,0.88) 50%, rgba(15,35,45,0.20) 100%)';
  if (d === 'uniforme') return 'rgba(15,35,45,0.88)';
  return 'linear-gradient(to right, rgba(15,35,45,0.88) 0%, rgba(15,35,45,0.65) 45%, rgba(15,35,45,0.30) 100%)';
}

function getTitleFontSize(item: HeroBannerSlide): string {
  if (item.tituloFontSize === 'pequeno') return 'clamp(22px, 3.5vw, 36px)';
  if (item.tituloFontSize === 'grande') return 'clamp(42px, 6.5vw, 68px)';
  return 'clamp(32px, 5vw, 54px)';
}

onMounted(async () => {
  loadConfig();
  try {
    const slides = await $fetch<HeroBannerSlide[]>('/data/hero-banners.json');
    if (Array.isArray(slides)) heroSlides.value = slides;
  } catch {
    heroSlides.value = [];
  }
});

const sliderData = computed<HeroBannerSlide[]>(() => {
  const newSlides = heroSlides.value.filter(s => s.ativo);
  if (newSlides.length > 0) return newSlides;

  const carousels = carouselStore.carousels;
  const oldFiltered = carousels
    .filter((c: Record<string, unknown>) => c.posicao === "1" && c.ativo === true)
    .sort((a: Record<string, unknown>, b: Record<string, unknown>) => (a.ordemExibicao as number) - (b.ordemExibicao as number));

  if (oldFiltered.length > 0) {
    return oldFiltered.map((c: Record<string, unknown>): HeroBannerSlide => ({
      id: (c.idCarrossel as number) || 0,
      url: (c.imagem as string) || '',
      urlDestino: (c.link as string) || '',
      ativo: true,
      headline: '',
      subtitulo: '',
      badge: '',
      ctaTexto: '',
      ctaLink: (c.link as string) || '',
      ctaCor: '#98C73A',
      textoCor: 'light',
      overlayIntensidade: 70,
    }));
  }

  return [{
    id: 0,
    url: '',
    urlDestino: '/register',
    ativo: true,
    headline: '',
    subtitulo: '',
    badge: '',
    ctaTexto: '',
    ctaLink: '/register',
    ctaCor: '#98C73A',
    textoCor: 'light',
    overlayIntensidade: 70,
  }];
});
</script>

<style scoped>
.qs-hero {
  position: relative;
  width: 100%;
}

.qs-hero__loading {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 480px;
  background: linear-gradient(135deg, #1a4a54 0%, #225F6B 40%, #2F7785 100%);
}

.qs-hero__swiper {
  width: 100%;
}

.qs-hero__slide {
  position: relative;
  overflow: hidden;
  min-height: 480px;
}

.qs-hero__bg {
  position: absolute;
  inset: 0;
  /* Foto profissional fixa — ignora item.imagem da API (colagem de logos) */
  background-image: url('https://images.unsplash.com/photo-1483985988355-763728e1935b?w=1600&q=85&auto=format&fit=crop');
  background-size: cover;
  background-position: center top;
  background-repeat: no-repeat;
}

.qs-hero__overlay {
  position: absolute;
  inset: 0;
}

.qs-hero__content-wrap {
  position: relative;
  z-index: 2;
  padding-top: 60px;
  padding-bottom: 80px;
}

.qs-hero__badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: rgba(255,255,255,0.12);
  border: 1px solid rgba(255,255,255,0.20);
  border-radius: 999px;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  font-weight: 500;
  padding: 5px 14px;
  margin-bottom: 20px;
  backdrop-filter: blur(4px);
}

.qs-hero__title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(32px, 5vw, 54px);
  font-weight: 800;
  color: #fff;
  line-height: 1.1;
  letter-spacing: -0.03em;
  margin-bottom: 20px;
}

.qs-hero__title--lime {
  color: #98C73A;
}

.qs-hero__content--dark .qs-hero__badge {
  color: #1a2236;
  background: rgba(0,0,0,0.06);
  border-color: rgba(0,0,0,0.12);
}

.qs-hero__content--dark .qs-hero__title {
  color: #1a2236;
}

.qs-hero__content--dark .qs-hero__subtitle {
  color: rgba(26,34,54,0.80);
}

.qs-hero__subtitle {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 16px;
  color: rgba(255,255,255,0.80);
  line-height: 1.6;
  max-width: 440px;
  margin-bottom: 32px;
}

.qs-hero__cta {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 700;
  padding: 14px 28px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
  box-shadow: 0 4px 20px rgba(152, 199, 58, 0.40);
  white-space: nowrap;
}

.qs-hero__cta:hover {
  background: #7aad1f;
  transform: translateY(-2px);
  box-shadow: 0 6px 28px rgba(152, 199, 58, 0.50);
  color: #fff;
}

.qs-hero__actions {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}

.qs-hero__cta-sec {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  border: 1.5px solid rgba(255,255,255,0.55);
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 600;
  padding: 13px 26px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
  backdrop-filter: blur(4px);
  background: rgba(255,255,255,0.08);
}

.qs-hero__cta-sec:hover {
  border-color: #fff;
  background: rgba(255,255,255,0.18);
  color: #fff;
  transform: translateY(-2px);
}

.qs-hero__cards {
  display: flex;
  flex-direction: column;
  gap: 14px;
  align-items: flex-end;
}

.qs-hero__card {
  display: flex;
  align-items: center;
  gap: 12px;
  background: rgba(255,255,255,0.95);
  backdrop-filter: blur(12px);
  border-radius: 12px;
  padding: 14px 20px;
  box-shadow: 0 8px 32px rgba(0,0,0,0.15);
  min-width: 180px;
  transition: all 0.3s ease;
}

.qs-hero__card:hover {
  transform: translateY(-3px);
  box-shadow: 0 12px 40px rgba(0,0,0,0.25);
}

.qs-hero__card-icon {
  width: 38px;
  height: 38px;
  border-radius: 8px;
  background: rgba(47, 119, 133, 0.10);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.qs-hero__card-icon--green {
  background: #98C73A;
}

.qs-hero__card-label {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  color: #6b7280;
  font-weight: 500;
  margin-bottom: 2px;
}

.qs-hero__card-value {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 700;
  color: #111827;
}

.qs-hero__card-value--green {
  color: #2F7785;
  font-size: 13px;
}

.qs-hero__card--offset {
  align-self: flex-end;
  margin-right: -20px;
}

.qs-hero-dots {
  position: absolute;
  bottom: 24px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 3;
  display: flex;
  gap: 8px;
}

:deep(.qs-hero-dots .swiper-pagination-bullet) {
  width: 8px !important;
  height: 8px !important;
  background: rgba(255,255,255,0.45) !important;
  opacity: 1 !important;
  border-radius: 50%;
  transition: all 0.3s;
}

:deep(.qs-hero-dots .swiper-pagination-bullet-active) {
  background: #98C73A !important;
  width: 24px !important;
  border-radius: 999px !important;
}

.qs-hero-arrows {
  position: absolute;
  bottom: 20px;
  right: 24px;
  z-index: 3;
  gap: 8px;
}

.qs-hero-prev, .qs-hero-next {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: rgba(255,255,255,0.15);
  backdrop-filter: blur(4px);
  border: 1px solid rgba(255,255,255,0.25);
  color: #fff;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s;
}

.qs-hero-prev:hover, .qs-hero-next:hover {
  background: rgba(255,255,255,0.30);
}

/* Badge dot pulsante */
.qs-hero__badge-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: #98C73A;
  flex-shrink: 0;
  animation: pulse-dot 2s infinite;
}

@keyframes pulse-dot {
  0%, 100% { opacity: 1; transform: scale(1); }
  50% { opacity: 0.6; transform: scale(1.3); }
}

/* Social proof */
.qs-hero__social-proof {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-top: 24px;
}

.qs-hero__avatars {
  display: flex;
}

.qs-hero__avatars img {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 2px solid rgba(255,255,255,0.8);
  object-fit: cover;
  margin-left: -8px;
}

.qs-hero__avatars img:first-child {
  margin-left: 0;
}

.qs-hero__rating {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  color: rgba(255,255,255,0.85);
  font-weight: 500;
}

.qs-hero__row {
  min-height: 480px;
}

@media (max-width: 991px) {
  .qs-hero__content-wrap {
    padding-top: 40px;
    padding-bottom: 60px;
  }
  .qs-hero__row {
    min-height: 380px;
  }
}

@media (max-width: 575px) {
  .qs-hero__row {
    min-height: 0;
    padding-top: 20px;
    padding-bottom: 20px;
  }
  .qs-hero__content-wrap {
    padding-top: 24px;
    padding-bottom: 40px;
  }
  .qs-hero__title {
    font-size: clamp(26px, 8vw, 36px);
  }
  .qs-hero__subtitle {
    font-size: 14px;
  }
  .qs-hero__cta {
    font-size: 14px;
    padding: 12px 20px;
  }
  .qs-hero__social-proof {
    flex-wrap: wrap;
    gap: 8px;
  }
}
</style>
