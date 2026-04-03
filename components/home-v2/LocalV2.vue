<template>
  <div>
    <div class="flex items-center justify-between mb-7">
      <h2 class="text-xl lg:text-2xl font-bold text-[#225F6B]">
        Ganhe Consumindo Localmente
      </h2>
    </div>
    <p class="text-sm text-gray-500 mb-6">Estabelecimentos perto de você com cashback exclusivo</p>

    <!-- Cards grid -->
    <div class="grid grid-cols-2 gap-3">
      <div
        v-for="est in establishments"
        :key="est.id"
        class="bg-white rounded-2xl overflow-hidden shadow-sm hover:shadow-md transition-all duration-200 group border border-gray-100"
      >
        <div class="relative h-28 bg-gray-100 overflow-hidden">
          <img
            :src="est.image"
            :alt="est.name"
            class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
          />
          <div class="absolute top-2 right-2 bg-[#98C73A] text-[#225F6B] text-[10px] font-bold px-2 py-0.5 rounded-full">
            {{ est.cashbackPercent }}% CB
          </div>
        </div>
        <div class="p-3">
          <p class="text-xs font-bold text-[#225F6B] leading-snug">{{ est.name }}</p>
          <p class="text-[10px] text-gray-400 mt-0.5">{{ est.category }}</p>
          <p class="text-[9px] text-gray-400 mt-1 line-clamp-1">{{ est.address }}</p>
          <a
            :href="est.mapsLink"
            class="mt-2 block text-center text-[10px] font-bold text-[#225F6B] bg-[#98C73A] rounded-full py-1.5 hover:bg-[#8ab832] transition-colors"
          >
            Ganhar Cashback
          </a>
        </div>
      </div>
    </div>

    <!-- Ver todos -->
    <div class="mt-5 text-center">
      <NuxtLink
        to="/mapa"
        class="inline-flex items-center gap-2 bg-[#225F6B] text-white font-semibold text-sm px-6 py-2.5 rounded-full hover:bg-[#1a4d57] transition-colors"
      >
        Ver todos os parceiros locais
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
        </svg>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
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
    category: 'Combustível',
    image: '/images/local/posto.png',
    cashbackPercent: 5,
    address: 'Av. Atlântica, 1702 - Rio de Janeiro',
    mapsLink: '#',
  },
  {
    id: 2,
    name: 'Drogasil Ipanema',
    category: 'Farmácia',
    image: '/images/local/farmacia.png',
    cashbackPercent: 8,
    address: 'Rua Visconde de Pirajá, 182 - Rio de Janeiro',
    mapsLink: '#',
  },
  {
    id: 3,
    name: 'Restaurante Azul Marinho',
    category: 'Restaurante',
    image: '/images/local/restaurante.png',
    cashbackPercent: 10,
    address: 'Av. Epitácio Pessoa, 1104 - Rio de Janeiro',
    mapsLink: '#',
  },
  {
    id: 4,
    name: 'Padaria Confeitaria Leblon',
    category: 'Padaria',
    image: '/images/local/padaria.png',
    cashbackPercent: 7,
    address: 'Rua Dias Ferreira, 256 - Rio de Janeiro',
    mapsLink: '#',
  },
]
</script>
