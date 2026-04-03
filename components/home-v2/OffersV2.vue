<template>
  <section class="bg-[#F4F4F5] py-12 lg:py-20">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between mb-8">
        <h2 class="text-2xl lg:text-3xl font-bold text-[#225F6B]">
          Ofertas com Maior Cashback Hoje.
        </h2>
        <NuxtLink 
          to="/ofertas"
          class="hidden sm:flex items-center text-[#2F7785] font-medium hover:text-[#225F6B] transition-colors"
        >
          Ver todas
          <svg class="w-5 h-5 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
          </svg>
        </NuxtLink>
      </div>

      <!-- Desktop Grid -->
      <div class="hidden lg:grid grid-cols-4 gap-6">
        <ProductCard 
          v-for="product in products" 
          :key="product.id"
          :product="product"
        />
      </div>

      <!-- Mobile Swiper -->
      <div class="lg:hidden overflow-x-auto pb-4 -mx-4 px-4 scrollbar-hide">
        <div class="flex gap-4" style="width: max-content;">
          <div 
            v-for="product in products" 
            :key="product.id"
            class="w-72 flex-shrink-0"
          >
            <ProductCard :product="product" />
          </div>
        </div>
      </div>

      <!-- Mobile View All -->
      <div class="lg:hidden text-center mt-6">
        <NuxtLink 
          to="/ofertas"
          class="inline-flex items-center text-[#2F7785] font-medium"
        >
          Ver todas as ofertas
          <svg class="w-5 h-5 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
          </svg>
        </NuxtLink>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import ProductCard from '~/components/home-v2/ProductCard.vue'

interface Product {
  id: number
  name: string
  image: string
  storeLogo: string
  storeName: string
  cashbackPercent: number
  link: string
}

const products: Product[] = [
  {
    id: 1,
    name: 'Smart TV 55" 4K UHD',
    image: '/images/products/tv.png',
    storeLogo: '/images/brands/samsung.png',
    storeName: 'Samsung',
    cashbackPercent: 8,
    link: '/produto/1',
  },
  {
    id: 2,
    name: 'Tenis Air Max Plus',
    image: '/images/products/tenis.png',
    storeLogo: '/images/brands/nike.png',
    storeName: 'Nike',
    cashbackPercent: 12,
    link: '/produto/2',
  },
  {
    id: 3,
    name: 'Geladeira Frost Free 400L',
    image: '/images/products/geladeira.png',
    storeLogo: '/images/brands/casas-bahia.png',
    storeName: 'Casas Bahia',
    cashbackPercent: 6,
    link: '/produto/3',
  },
  {
    id: 4,
    name: 'Kit Roupas Inverno',
    image: '/images/products/roupas.png',
    storeLogo: '/images/brands/cea.png',
    storeName: 'C&A',
    cashbackPercent: 15,
    link: '/produto/4',
  },
]
</script>

<style scoped>
.scrollbar-hide {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
.scrollbar-hide::-webkit-scrollbar {
  display: none;
}
</style>
