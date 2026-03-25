<template>
  <section class="qs-hero">
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
        <!-- Fundo sempre fixo — a API de carrosseis fornece apenas texto, nunca o background -->
        <div class="qs-hero__bg"></div>
        <div class="qs-hero__overlay"></div>

        <div class="container qs-hero__content-wrap">
          <div class="row align-items-center" style="min-height: 480px;">
            <div class="col-xl-6 col-lg-7">
              <div class="qs-hero__content">
                <span class="qs-hero__badge">
                  <svg width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="#98C73A" stroke-width="2"><path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/></svg>
                  Simples e rápido
                </span>

                <h1 class="qs-hero__title">
                  {{ item.texto2 || 'Transforme cada compra em' }}
                  <span class="qs-hero__title--lime">{{ item.highlightText || 'dinheiro de volta' }}</span>
                </h1>

                <p class="qs-hero__subtitle">
                  {{ item.texto3 || 'Ative o cashback com um clique, compre normalmente e veja o saldo crescer automaticamente.' }}
                </p>

                <div class="qs-hero__actions">
                  <nuxt-link :href="item.link || '/register'" class="qs-hero__cta">
                    Começar Agora
                    <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
                  </nuxt-link>
                  <nuxt-link href="/partners" class="qs-hero__cta-sec">
                    Ver parceiros
                  </nuxt-link>
                </div>
              </div>
            </div>

            <div class="col-xl-6 col-lg-5 d-none d-lg-flex justify-content-end align-items-center">
              <div class="qs-hero__cards">
                <div class="qs-hero__card qs-hero__card--activation">
                  <div class="qs-hero__card-icon">
                    <svg width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2.5"><path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/></svg>
                  </div>
                  <div>
                    <div class="qs-hero__card-label">Ativação rápida</div>
                    <div class="qs-hero__card-value">1 clique</div>
                  </div>
                </div>

                <div class="qs-hero__card qs-hero__card--balance">
                  <div class="qs-hero__card-icon qs-hero__card-icon--green">
                    <svg width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="#fff" stroke-width="2"><path d="M12 2v20M17 5H9.5a3.5 3.5 0 000 7h5a3.5 3.5 0 010 7H6"/></svg>
                  </div>
                  <div>
                    <div class="qs-hero__card-label">Seu saldo</div>
                    <div class="qs-hero__card-value qs-hero__card-value--green">R$ 127,50</div>
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
import { computed } from 'vue';
import { useCarouselStore } from "@/pinia/useCarouselStore";
import { Swiper, SwiperSlide } from "swiper/vue";
import { Navigation, Pagination, Autoplay } from "swiper/modules";

const carouselStore = useCarouselStore();

const sliderData = computed(() => {
  const carousels = carouselStore.carousels;
  const filtered = carousels
    .filter(c => c.posicao === "1" && c.ativo === true)
    .sort((a, b) => a.ordemExibicao - b.ordemExibicao);
  return filtered.length > 0 ? filtered : [{ texto2: 'Transforme cada compra em', texto3: 'Ative o cashback com um clique, compre normalmente e veja o saldo crescer automaticamente.', link: '/register', imagem: null }];
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
  background: linear-gradient(to right, rgba(15,35,45,0.88) 0%, rgba(15,35,45,0.65) 45%, rgba(15,35,45,0.30) 100%);
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
  display: block;
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

@media (max-width: 991px) {
  .qs-hero__content-wrap {
    padding-top: 40px;
    padding-bottom: 60px;
  }
}
</style>
