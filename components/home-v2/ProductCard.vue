<template>
  <div class="bg-white rounded-2xl shadow-md hover:shadow-xl transition-shadow duration-300 overflow-hidden group">
    <!-- Image Container -->
    <div class="relative">
      <img 
        :src="product.image" 
        :alt="product.name"
        class="w-full h-48 object-cover group-hover:scale-105 transition-transform duration-300"
      />
      
      <!-- Store Logo -->
      <div class="absolute top-3 left-3 bg-white rounded-lg p-1.5 shadow-sm">
        <img 
          :src="product.storeLogo" 
          :alt="product.storeName"
          class="h-6 w-auto"
        />
      </div>

      <!-- Cashback Badge -->
      <div class="absolute top-3 right-3 bg-[#98C73A] text-white text-sm font-bold px-3 py-1 rounded-full">
        Ate {{ product.cashbackPercent }}% Cashback
      </div>
    </div>

    <!-- Content -->
    <div class="p-4">
      <p class="text-xs text-gray-500 mb-1">{{ product.storeName }}</p>
      <h3 class="font-semibold text-gray-800 mb-4 line-clamp-2">
        {{ product.name }}
      </h3>
      
      <NuxtLink 
        :to="product.link"
        class="block w-full bg-[#2F7785] text-white text-center font-medium py-2.5 rounded-full hover:bg-[#225F6B] transition-colors"
      >
        Aproveitar Agora
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Product {
  id: number
  name: string
  image: string
  storeLogo: string
  storeName: string
  cashbackPercent: number
  link: string
}

defineProps<{
  product: Product
}>()
</script>
