<template>
  <section class="bg-white py-12 lg:py-16">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <h2 class="text-2xl lg:text-3xl font-bold text-[#225F6B] text-center mb-10">
        Grandes Marcas Com Cashback Quanta.
      </h2>

      <!-- Infinite Carousel -->
      <div class="relative overflow-hidden">
        <div 
          class="flex animate-scroll"
          :style="{ animationDuration: `${brands.length * 3}s` }"
        >
          <div 
            v-for="(brand, index) in [...brands, ...brands]" 
            :key="`${brand.name}-${index}`"
            class="flex-shrink-0 px-6 lg:px-10"
          >
            <div 
              class="grayscale hover:grayscale-0 opacity-60 hover:opacity-100 transition-all duration-300 cursor-pointer"
            >
              <img 
                :src="brand.logo" 
                :alt="brand.name"
                class="h-10 lg:h-14 w-auto object-contain"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Brand {
  name: string
  logo: string
}

const brands: Brand[] = [
  { name: 'Casas Bahia', logo: '/images/brands/casas-bahia.png' },
  { name: 'Carrefour', logo: '/images/brands/carrefour.png' },
  { name: 'C&A', logo: '/images/brands/cea.png' },
  { name: 'Samsung', logo: '/images/brands/samsung.png' },
  { name: 'L\'Oreal', logo: '/images/brands/loreal.png' },
  { name: 'Nike', logo: '/images/brands/nike.png' },
  { name: 'Renner', logo: '/images/brands/renner.png' },
  { name: 'Riachuelo', logo: '/images/brands/riachuelo.png' },
  { name: 'Ponto', logo: '/images/brands/ponto.png' },
]
</script>

<style scoped>
@keyframes scroll {
  0% {
    transform: translateX(0);
  }
  100% {
    transform: translateX(-50%);
  }
}

.animate-scroll {
  animation: scroll linear infinite;
}

.animate-scroll:hover {
  animation-play-state: paused;
}
</style>
