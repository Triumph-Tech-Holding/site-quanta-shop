<template>
  <section class="relative bg-gradient-to-br from-[#F4F4F5] to-white overflow-hidden">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 lg:py-20">
      <div class="relative">
        <!-- Slides -->
        <TransitionGroup
          enter-active-class="transition duration-500 ease-out"
          enter-from-class="opacity-0 translate-x-8"
          enter-to-class="opacity-100 translate-x-0"
          leave-active-class="transition duration-300 ease-in absolute inset-0"
          leave-from-class="opacity-100"
          leave-to-class="opacity-0"
        >
          <div 
            v-for="(slide, index) in slides" 
            :key="index"
            v-show="currentSlide === index"
            class="grid lg:grid-cols-2 gap-8 lg:gap-12 items-center"
          >
            <!-- Content -->
            <div class="order-2 lg:order-1">
              <div v-if="slide.logo" class="mb-6">
                <img :src="slide.logo" :alt="slide.logoAlt" class="h-12 w-auto" />
              </div>
              <h1 class="text-3xl sm:text-4xl lg:text-5xl font-bold text-[#225F6B] leading-tight mb-4">
                {{ slide.headline }}
              </h1>
              <p class="text-lg text-gray-600 mb-8 max-w-lg">
                {{ slide.subheadline }}
              </p>
              <NuxtLink 
                :to="slide.ctaLink"
                class="inline-flex items-center bg-[#98C73A] text-[#225F6B] font-semibold px-8 py-3.5 rounded-full hover:bg-[#8ab832] transition-colors text-lg"
              >
                {{ slide.ctaText }}
                <svg class="w-5 h-5 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 8l4 4m0 0l-4 4m4-4H3" />
                </svg>
              </NuxtLink>
            </div>

            <!-- Image -->
            <div class="order-1 lg:order-2">
              <div class="relative">
                <div class="absolute inset-0 bg-[#98C73A]/20 rounded-3xl transform rotate-3"></div>
                <img 
                  :src="slide.image" 
                  :alt="slide.imageAlt"
                  class="relative rounded-3xl shadow-2xl w-full h-64 sm:h-80 lg:h-[400px] object-cover"
                />
              </div>
            </div>
          </div>
        </TransitionGroup>

        <!-- Navigation Dots -->
        <div class="flex justify-center space-x-3 mt-8 lg:mt-12">
          <button 
            v-for="(_, index) in slides" 
            :key="index"
            @click="goToSlide(index)"
            :class="[
              'w-3 h-3 rounded-full transition-all duration-300',
              currentSlide === index 
                ? 'bg-[#98C73A] w-8' 
                : 'bg-gray-300 hover:bg-gray-400'
            ]"
            :aria-label="`Ir para slide ${index + 1}`"
          />
        </div>

        <!-- Navigation Arrows -->
        <button 
          @click="prevSlide"
          class="absolute left-0 top-1/2 -translate-y-1/2 -translate-x-4 lg:-translate-x-12 bg-white shadow-lg rounded-full p-3 text-[#2F7785] hover:bg-[#F4F4F5] transition-colors hidden sm:block"
          aria-label="Slide anterior"
        >
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
          </svg>
        </button>
        <button 
          @click="nextSlide"
          class="absolute right-0 top-1/2 -translate-y-1/2 translate-x-4 lg:translate-x-12 bg-white shadow-lg rounded-full p-3 text-[#2F7785] hover:bg-[#F4F4F5] transition-colors hidden sm:block"
          aria-label="Proximo slide"
        >
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
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
    subheadline: 'Transforme seus gastos diarios em saldo CCR que cresce todo dia. Dinheiro na conta, nao pontos.',
    ctaText: 'Comecar a Ganhar',
    ctaLink: '/cadastro',
    image: '/images/hero-consumidor.png',
    imageAlt: 'Pessoa feliz usando smartphone no Rio de Janeiro',
  },
  {
    headline: 'Sua Loja Pode Ganhar Dinheiro Enquanto Voce Dorme.',
    subheadline: 'Fidelize clientes e crie novas fontes de renda passiva com a plataforma mais completa do Brasil.',
    ctaText: 'Credenciar Minha Loja',
    ctaLink: '/para-empresa',
    image: '/images/hero-lojista.png',
    imageAlt: 'Lojista sorrindo em sua loja',
    logo: '/img/banner/4.png',
    logoAlt: 'Para sua Empresa',
  },
  {
    headline: 'Trabalhe com Fidelizacao e Ganhe Renda Recorrente.',
    subheadline: 'Recrute empresas e consumidores para a rede Quanta e receba 1% de tudo que eles gerarem.',
    ctaText: 'Seja um Agente Quanta',
    ctaLink: '/seja-agente',
    image: '/images/hero-agente.png',
    imageAlt: 'Agente Quanta apresentando oportunidades',
    logo: '/img/banner/5.png',
    logoAlt: 'Seja um Agente',
  },
]

const currentSlide = ref(0)
let autoplayInterval: ReturnType<typeof setInterval> | null = null

const nextSlide = () => {
  currentSlide.value = (currentSlide.value + 1) % slides.length
}

const prevSlide = () => {
  currentSlide.value = (currentSlide.value - 1 + slides.length) % slides.length
}

const goToSlide = (index: number) => {
  currentSlide.value = index
}

const startAutoplay = () => {
  autoplayInterval = setInterval(nextSlide, 5000)
}

const stopAutoplay = () => {
  if (autoplayInterval) {
    clearInterval(autoplayInterval)
  }
}

onMounted(() => {
  startAutoplay()
})

onUnmounted(() => {
  stopAutoplay()
})
</script>
