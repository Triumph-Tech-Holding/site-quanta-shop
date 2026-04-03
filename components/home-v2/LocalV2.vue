<template>
  <section class="bg-white py-12 lg:py-20 relative overflow-hidden">
    <!-- Background Map Pattern -->
    <div class="absolute inset-0 opacity-5">
      <svg class="w-full h-full" viewBox="0 0 100 100" preserveAspectRatio="none">
        <pattern id="mapPattern" patternUnits="userSpaceOnUse" width="20" height="20">
          <circle cx="10" cy="10" r="1" fill="#2F7785" />
          <path d="M0 10 L20 10 M10 0 L10 20" stroke="#2F7785" stroke-width="0.2" />
        </pattern>
        <rect fill="url(#mapPattern)" width="100%" height="100%" />
      </svg>
    </div>

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative">
      <div class="flex items-center justify-between mb-8">
        <div>
          <h2 class="text-2xl lg:text-3xl font-bold text-[#225F6B]">
            Ganhe Consumindo Localmente no Rio.
          </h2>
          <p class="text-gray-600 mt-2">Estabelecimentos perto de voce com cashback exclusivo</p>
        </div>
        <NuxtLink 
          to="/mapa"
          class="hidden sm:flex items-center text-[#2F7785] font-medium hover:text-[#225F6B] transition-colors"
        >
          Ver mapa completo
          <svg class="w-5 h-5 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
          </svg>
        </NuxtLink>
      </div>

      <!-- Grid -->
      <div class="grid sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <EstablishmentCard 
          v-for="establishment in establishments" 
          :key="establishment.id"
          :establishment="establishment"
        />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import EstablishmentCard from '~/components/home-v2/EstablishmentCard.vue'

interface Establishment {
  id: number
  name: string
  category: string
  image: string
  cashbackPercent: number
  address: string
  mapsLink: string
}

const establishments: Establishment[] = [
  {
    id: 1,
    name: 'Posto Shell Copacabana',
    category: 'Combustivel',
    image: '/images/local/posto.png',
    cashbackPercent: 3,
    address: 'Av. Atlantica, 1702',
    mapsLink: 'https://maps.google.com',
  },
  {
    id: 2,
    name: 'Drogaria Pacheco Centro',
    category: 'Farmacia',
    image: '/images/local/farmacia.png',
    cashbackPercent: 5,
    address: 'Rua do Ouvidor, 45',
    mapsLink: 'https://maps.google.com',
  },
  {
    id: 3,
    name: 'Restaurante Marius',
    category: 'Gastronomia',
    image: '/images/local/restaurante.png',
    cashbackPercent: 8,
    address: 'Av. Atlantica, 290',
    mapsLink: 'https://maps.google.com',
  },
]
</script>
