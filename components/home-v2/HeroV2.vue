<template>
  <section class="v2-hero">
    <div class="v2-hero__container">
      <div class="v2-hero__inner">
        <TransitionGroup
          enter-active-class="v2-hero__enter-active"
          enter-from-class="v2-hero__enter-from"
          enter-to-class="v2-hero__enter-to"
          leave-active-class="v2-hero__leave-active"
          leave-from-class="v2-hero__leave-from"
          leave-to-class="v2-hero__leave-to"
        >
          <div
            v-for="(slide, index) in slides"
            :key="index"
            v-show="currentSlide === index"
            class="v2-hero__slide"
          >
            <div class="v2-hero__content">
              <div v-if="slide.logo" class="v2-hero__logo-wrap">
                <img :src="slide.logo" :alt="slide.logoAlt" class="v2-hero__logo" />
              </div>
              <h1 class="v2-hero__title">{{ slide.headline }}</h1>
              <p class="v2-hero__subtitle">{{ slide.subheadline }}</p>
              <NuxtLink :to="slide.ctaLink" class="v2-hero__cta">
                {{ slide.ctaText }}
                <svg width="20" height="20" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 8l4 4m0 0l-4 4m4-4H3" />
                </svg>
              </NuxtLink>
            </div>
            <div class="v2-hero__image-wrap">
              <div class="v2-hero__image-bg"></div>
              <img
                :src="slide.image"
                :alt="slide.imageAlt"
                class="v2-hero__image"
              />
            </div>
          </div>
        </TransitionGroup>

        <div class="v2-hero__dots">
          <button
            v-for="(_, index) in slides"
            :key="index"
            @click="goToSlide(index)"
            :class="['v2-hero__dot', currentSlide === index ? 'v2-hero__dot--active' : '']"
            :aria-label="`Ir para slide ${index + 1}`"
          />
        </div>

        <button @click="prevSlide" class="v2-hero__arrow v2-hero__arrow--prev" aria-label="Slide anterior">
          <svg width="24" height="24" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
          </svg>
        </button>
        <button @click="nextSlide" class="v2-hero__arrow v2-hero__arrow--next" aria-label="Próximo slide">
          <svg width="24" height="24" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
          </svg>
        </button>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Slide {
  headline: string
  subheadline: string
  ctaText: string
  ctaLink: string
  image: string
  imageAlt: string
  logo?: string
  logoAlt?: string
}

const slides: Slide[] = [
  {
    headline: 'Receba Cashback de Verdade em Cada Compra.',
    subheadline: 'Transforme seus gastos diários em saldo CCR que cresce todo dia. Dinheiro na conta, não pontos.',
    ctaText: 'Começar a Ganhar',
    ctaLink: '/para-voce',
    image: '/img/banner/banner-slider-1.png',
    imageAlt: 'Cashback de verdade com a Quanta Shop',
  },
  {
    headline: 'Sua Loja Pode Ganhar Dinheiro Enquanto Você Dorme.',
    subheadline: 'Fidelize clientes e crie novas fontes de renda passiva com a plataforma mais completa do Brasil.',
    ctaText: 'Credenciar Minha Loja',
    ctaLink: '/para-sua-empresa',
    image: '/img/banner/banner-slider-2.png',
    imageAlt: 'Soluções para empresas com cashback',
  },
  {
    headline: 'Trabalhe com Fidelização e Ganhe Renda Recorrente.',
    subheadline: 'Recrute empresas e consumidores para a rede Quanta e receba 1% de tudo que eles gerarem.',
    ctaText: 'Seja um Agente Quanta',
    ctaLink: '/seja-um-agente',
    image: '/img/banner/banner-slider-3.png',
    imageAlt: 'Seja um agente Quanta e ganhe renda recorrente',
  },
]

const currentSlide = ref(0)
let autoplayInterval: ReturnType<typeof setInterval> | null = null

const nextSlide = () => { currentSlide.value = (currentSlide.value + 1) % slides.length }
const prevSlide = () => { currentSlide.value = (currentSlide.value - 1 + slides.length) % slides.length }
const goToSlide = (index: number) => { currentSlide.value = index }

onMounted(() => { autoplayInterval = setInterval(nextSlide, 5000) })
onUnmounted(() => { if (autoplayInterval) clearInterval(autoplayInterval) })
</script>

<style scoped>
.v2-hero {
  background: linear-gradient(135deg, #F4F4F5 0%, #ffffff 100%);
  overflow: hidden;
}
.v2-hero__container {
  max-width: 1280px;
  margin: 0 auto;
  padding: 3rem 1rem;
}
@media (min-width: 1024px) {
  .v2-hero__container { padding: 5rem 2rem; }
}
.v2-hero__inner { position: relative; }
.v2-hero__slide {
  display: grid;
  gap: 2rem;
  align-items: center;
}
@media (min-width: 1024px) {
  .v2-hero__slide {
    grid-template-columns: 1fr 1fr;
    gap: 3rem;
  }
}
.v2-hero__content { order: 2; }
@media (min-width: 1024px) { .v2-hero__content { order: 1; } }
.v2-hero__logo-wrap { margin-bottom: 1.5rem; }
.v2-hero__logo { height: 48px; width: auto; }
.v2-hero__title {
  font-size: 2rem;
  font-weight: 700;
  color: #225F6B;
  line-height: 1.2;
  margin-bottom: 1rem;
}
@media (min-width: 768px) { .v2-hero__title { font-size: 2.5rem; } }
@media (min-width: 1024px) { .v2-hero__title { font-size: 3rem; } }
.v2-hero__subtitle {
  font-size: 1.1rem;
  color: #6b7280;
  margin-bottom: 2rem;
  max-width: 480px;
}
.v2-hero__cta {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  background: #98C73A;
  color: #225F6B;
  font-weight: 700;
  font-size: 1.1rem;
  padding: 0.9rem 2rem;
  border-radius: 9999px;
  text-decoration: none;
  transition: background 0.2s;
}
.v2-hero__cta:hover { background: #8ab832; color: #225F6B; }
.v2-hero__image-wrap {
  order: 1;
  position: relative;
}
@media (min-width: 1024px) { .v2-hero__image-wrap { order: 2; } }
.v2-hero__image-bg {
  position: absolute;
  inset: 0;
  background: rgba(152, 199, 58, 0.2);
  border-radius: 1.5rem;
  transform: rotate(3deg);
}
.v2-hero__image {
  position: relative;
  width: 100%;
  height: 260px;
  object-fit: cover;
  border-radius: 1.5rem;
  box-shadow: 0 25px 50px rgba(0,0,0,0.15);
}
@media (min-width: 768px) { .v2-hero__image { height: 320px; } }
@media (min-width: 1024px) { .v2-hero__image { height: 400px; } }
.v2-hero__dots {
  display: flex;
  justify-content: center;
  gap: 0.75rem;
  margin-top: 2rem;
}
.v2-hero__dot {
  width: 12px;
  height: 12px;
  border-radius: 9999px;
  background: #d1d5db;
  border: none;
  cursor: pointer;
  transition: all 0.3s;
  padding: 0;
}
.v2-hero__dot--active {
  background: #98C73A;
  width: 32px;
}
.v2-hero__arrow {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: white;
  border: none;
  border-radius: 9999px;
  width: 48px;
  height: 48px;
  display: none;
  align-items: center;
  justify-content: center;
  box-shadow: 0 4px 12px rgba(0,0,0,0.12);
  cursor: pointer;
  color: #2F7785;
  transition: background 0.2s;
}
.v2-hero__arrow:hover { background: #F4F4F5; }
@media (min-width: 768px) { .v2-hero__arrow { display: flex; } }
.v2-hero__arrow--prev { left: -1rem; }
@media (min-width: 1024px) { .v2-hero__arrow--prev { left: -3rem; } }
.v2-hero__arrow--next { right: -1rem; }
@media (min-width: 1024px) { .v2-hero__arrow--next { right: -3rem; } }
.v2-hero__enter-active { transition: opacity 0.5s, transform 0.5s; }
.v2-hero__enter-from { opacity: 0; transform: translateX(2rem); }
.v2-hero__enter-to { opacity: 1; transform: translateX(0); }
.v2-hero__leave-active { transition: opacity 0.3s; position: absolute; inset: 0; }
.v2-hero__leave-from { opacity: 1; }
.v2-hero__leave-to { opacity: 0; }
</style>
