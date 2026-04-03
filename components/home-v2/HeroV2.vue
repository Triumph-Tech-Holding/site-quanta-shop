<template>
  <section class="bg-gradient-to-br from-[#F4F4F5] to-white overflow-hidden">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10 lg:py-16">
      <div class="grid lg:grid-cols-2 gap-8 lg:gap-10 items-start">

        <!-- LEFT: Slides -->
        <div class="relative min-h-[380px] lg:min-h-[440px] flex flex-col justify-between">
          <div class="flex-1">
            <TransitionGroup
              enter-active-class="transition duration-500 ease-out"
              enter-from-class="opacity-0 translate-x-6"
              enter-to-class="opacity-100 translate-x-0"
              leave-active-class="transition duration-300 ease-in absolute inset-0"
              leave-from-class="opacity-100"
              leave-to-class="opacity-0"
            >
              <div
                v-for="(slide, index) in slides"
                :key="index"
                v-show="currentSlide === index"
              >
                <div v-if="slide.badge" class="inline-flex items-center gap-2 bg-white rounded-full px-4 py-1.5 shadow-sm mb-6">
                  <span class="w-2 h-2 rounded-full bg-[#98C73A]"></span>
                  <span class="text-sm font-medium text-[#225F6B]">{{ slide.badge }}</span>
                </div>
                <h1 class="text-4xl sm:text-5xl lg:text-5xl font-bold text-[#225F6B] leading-tight mb-5">
                  {{ slide.headline }}
                </h1>
                <p class="text-lg text-gray-500 mb-8 max-w-md leading-relaxed">
                  {{ slide.subheadline }}
                </p>
                <NuxtLink
                  :to="slide.ctaLink"
                  class="inline-flex items-center bg-[#98C73A] text-[#225F6B] font-bold px-8 py-3.5 rounded-full hover:bg-[#8ab832] transition-all duration-200 text-base shadow-md hover:shadow-lg"
                >
                  {{ slide.ctaText }}
                  <svg class="w-4 h-4 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M17 8l4 4m0 0l-4 4m4-4H3" />
                  </svg>
                </NuxtLink>
              </div>
            </TransitionGroup>
          </div>

          <!-- Dots + Arrows -->
          <div class="flex items-center gap-6 mt-10">
            <button
              @click="prevSlide"
              class="bg-white shadow-md rounded-full p-2.5 text-[#2F7785] hover:bg-[#F4F4F5] transition-colors border border-gray-100"
              aria-label="Slide anterior"
            >
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
              </svg>
            </button>
            <div class="flex items-center gap-2">
              <button
                v-for="(_, i) in slides"
                :key="i"
                @click="goToSlide(i)"
                :class="[
                  'rounded-full transition-all duration-300',
                  currentSlide === i ? 'bg-[#98C73A] w-8 h-3' : 'bg-gray-300 w-3 h-3 hover:bg-gray-400'
                ]"
                :aria-label="`Slide ${i + 1}`"
              />
            </div>
            <button
              @click="nextSlide"
              class="bg-white shadow-md rounded-full p-2.5 text-[#2F7785] hover:bg-[#F4F4F5] transition-colors border border-gray-100"
              aria-label="Próximo slide"
            >
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
              </svg>
            </button>
          </div>
        </div>

        <!-- RIGHT: Fixed "Destaques do Dia" panel -->
        <div class="bg-white rounded-3xl shadow-xl overflow-hidden">
          <div class="px-5 py-4 border-b border-gray-100 flex items-center justify-between">
            <h3 class="text-base font-bold text-[#225F6B]">Cuide da Elite</h3>
            <NuxtLink to="/ofertas" class="text-xs text-[#2F7785] hover:text-[#225F6B] font-medium transition-colors">
              Ver tudo →
            </NuxtLink>
          </div>
          <div class="grid grid-cols-2 gap-0 divide-x divide-y divide-gray-100">
            <div
              v-for="product in featuredProducts"
              :key="product.id"
              class="p-4 hover:bg-[#F4F4F5]/50 transition-colors group"
            >
              <div class="aspect-[4/3] rounded-xl overflow-hidden mb-3 bg-gray-50">
                <img
                  :src="product.image"
                  :alt="product.name"
                  class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                />
              </div>
              <div class="flex items-center gap-1.5 mb-1.5">
                <img :src="product.brandLogo" :alt="product.brand" class="h-4 w-auto object-contain" />
                <span class="text-[10px] text-gray-400 font-medium uppercase tracking-wide">{{ product.brand }}</span>
              </div>
              <p class="text-xs font-semibold text-[#225F6B] leading-snug mb-1">{{ product.name }}</p>
              <p class="text-[11px] text-[#2F7785] font-bold mb-2.5">{{ product.cashback }}% de cashback</p>
              <NuxtLink
                :to="product.link"
                class="block text-center bg-[#98C73A] text-[#225F6B] text-[11px] font-bold py-1.5 rounded-full hover:bg-[#8ab832] transition-colors"
              >
                Aprenda Agora
              </NuxtLink>
            </div>
          </div>
        </div>

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
  badge?: string
}

interface FeaturedProduct {
  id: number
  name: string
  image: string
  brand: string
  brandLogo: string
  cashback: number
  link: string
}

const slides: Slide[] = [
  {
    badge: '+12.000 consumidores ganhando',
    headline: 'Receba Cashback de Verdade em Cada Compra.',
    subheadline: 'Transforme seus gastos diários em saldo CCR que cresce todo dia. Dinheiro na conta, não pontos.',
    ctaText: 'Começar a Ganhar',
    ctaLink: '/cadastro',
  },
  {
    headline: 'Sua Loja Ganha Dinheiro Enquanto Você Dorme.',
    subheadline: 'Fidelize clientes e crie novas fontes de renda passiva com a plataforma mais completa do Brasil.',
    ctaText: 'Credenciar Minha Loja',
    ctaLink: '/para-empresa',
  },
  {
    headline: 'Trabalhe com Fidelização e Ganhe Renda Recorrente.',
    subheadline: 'Recrute empresas e consumidores para a rede Quanta e receba 1% de tudo que eles gerarem.',
    ctaText: 'Seja um Agente Quanta',
    ctaLink: '/seja-agente',
  },
]

const featuredProducts: FeaturedProduct[] = [
  {
    id: 1,
    name: 'Smartphone Galaxy A55',
    image: '/images/products/smartphone.png',
    brand: 'Samsung',
    brandLogo: '/images/brands/samsung.png',
    cashback: 10,
    link: '/ofertas',
  },
  {
    id: 2,
    name: 'Kit Beleza Premium',
    image: '/images/products/beleza.png',
    brand: 'L\'Oreal',
    brandLogo: '/images/brands/loreal.png',
    cashback: 15,
    link: '/ofertas',
  },
  {
    id: 3,
    name: 'Air Max Pulse',
    image: '/images/products/tenis.png',
    brand: 'Nike',
    brandLogo: '/images/brands/nike.png',
    cashback: 12,
    link: '/ofertas',
  },
  {
    id: 4,
    name: 'Air Fryer 5L Digital',
    image: '/images/products/airfryer.png',
    brand: 'Carrefour',
    brandLogo: '/images/brands/carrefour.png',
    cashback: 8,
    link: '/ofertas',
  },
]

const currentSlide = ref(0)
let autoplayInterval: ReturnType<typeof setInterval> | null = null

const nextSlide = () => { currentSlide.value = (currentSlide.value + 1) % slides.length }
const prevSlide = () => { currentSlide.value = (currentSlide.value - 1 + slides.length) % slides.length }
const goToSlide = (i: number) => { currentSlide.value = i }

onMounted(() => { autoplayInterval = setInterval(nextSlide, 6000) })
onUnmounted(() => { if (autoplayInterval) clearInterval(autoplayInterval) })
</script>
