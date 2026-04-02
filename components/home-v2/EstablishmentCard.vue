<template>
  <div class="bg-white rounded-2xl shadow-md hover:shadow-xl transition-shadow duration-300 overflow-hidden">
    <div class="relative h-40">
      <img
        :src="establishment.image"
        :alt="establishment.name"
        class="w-full h-full object-cover"
      />
      <div class="absolute top-3 right-3 bg-[#98C73A] text-white text-sm font-bold px-3 py-1 rounded-full">
        {{ establishment.cashbackPercent }}% Cashback
      </div>
      <div class="absolute bottom-3 left-3 bg-white/90 backdrop-blur-sm text-gray-700 text-xs font-medium px-3 py-1 rounded-full">
        {{ establishment.category }}
      </div>
    </div>
    <div class="p-4">
      <h3 class="font-semibold text-gray-800 text-lg mb-1">
        {{ establishment.name }}
      </h3>
      <p class="text-gray-500 text-sm mb-4">
        {{ establishment.address }}
      </p>
      <button
        @click="handleClick"
        class="inline-flex items-center text-[#2F7785] font-medium hover:text-[#225F6B] transition-colors"
      >
        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
        </svg>
        Como Chegar
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '~/pinia/useUserStore'

interface Establishment {
  id: number | string
  name: string
  category: string
  image: string
  cashbackPercent: number
  address: string
  mapsLink: string
  whatsapp?: string
}

const props = defineProps<{ establishment: Establishment }>()
const userStore = useUserStore()

function handleClick() {
  if (!userStore.isLoggedIn) {
    navigateTo('/login')
    return
  }
  if (props.establishment.whatsapp) {
    window.open(`https://wa.me/55${props.establishment.whatsapp.replace(/\D/g, '')}`, '_blank')
  }
}
</script>
