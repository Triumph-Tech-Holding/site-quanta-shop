<template>
  <div>
    <div class="flex items-center justify-between mb-7">
      <h2 class="text-xl lg:text-2xl font-bold text-[#225F6B]">
        Compre online e receba cashback.
      </h2>
    </div>
    <p class="text-sm text-gray-500 mb-6">Centenas de marcas e lojas parceiras online</p>

    <!-- Store grid -->
    <div class="grid grid-cols-4 gap-3">
      <div
        v-for="store in stores"
        :key="store.id"
        class="bg-white rounded-xl p-3 flex flex-col items-center gap-2 shadow-sm hover:shadow-md transition-all duration-200 group border border-gray-100"
      >
        <div class="w-10 h-10 rounded-lg overflow-hidden flex items-center justify-center bg-gray-50">
          <img
            :src="store.logo"
            :alt="store.name"
            class="w-full h-full object-contain"
          />
        </div>
        <p class="text-[10px] font-semibold text-[#225F6B] text-center leading-tight">{{ store.name }}</p>
        <p class="text-[9px] text-[#2F7785] font-medium">{{ store.cashback }}</p>
        <a
          :href="store.link"
          class="text-[10px] font-bold text-[#225F6B] bg-[#98C73A] rounded-full px-3 py-1 hover:bg-[#8ab832] transition-colors group-hover:shadow-sm"
        >
          Ir lá
        </a>
      </div>
    </div>

    <!-- Ver todos -->
    <div class="mt-6 text-center">
      <NuxtLink
        to="/parceiros"
        class="inline-flex items-center gap-2 bg-[#225F6B] text-white font-semibold text-sm px-6 py-2.5 rounded-full hover:bg-[#1a4d57] transition-colors"
      >
        Ver todos os parceiros online
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
        </svg>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Store {
  id: number
  name: string
  logo: string
  cashback: string
  link: string
}

const stores: Store[] = [
  { id: 1,  name: 'Shopee',          logo: '/images/stores/shopee.png',      cashback: 'Até 8%',  link: '/parceiros' },
  { id: 2,  name: 'Magazine Luiza',  logo: '/images/stores/magalu.png',      cashback: 'Até 10%', link: '/parceiros' },
  { id: 3,  name: 'Americanas',      logo: '/images/stores/americanas.png',  cashback: 'Até 7%',  link: '/parceiros' },
  { id: 4,  name: 'Amazon',          logo: '/images/stores/amazon.png',      cashback: 'Até 6%',  link: '/parceiros' },
  { id: 5,  name: 'Mercado Livre',   logo: '/images/stores/mercadolivre.png',cashback: 'Até 9%',  link: '/parceiros' },
  { id: 6,  name: 'Netshoes',        logo: '/images/stores/netshoes.png',    cashback: 'Até 12%', link: '/parceiros' },
  { id: 7,  name: 'Centauro',        logo: '/images/stores/centauro.png',    cashback: 'Até 11%', link: '/parceiros' },
  { id: 8,  name: 'Renner',          logo: '/images/stores/renner.png',      cashback: 'Até 15%', link: '/parceiros' },
  { id: 9,  name: 'Karsten',         logo: '/images/stores/karsten.png',     cashback: 'Até 10%', link: '/parceiros' },
  { id: 10, name: 'JBL',             logo: '/images/stores/jbl.png',         cashback: 'Até 8%',  link: '/parceiros' },
  { id: 11, name: 'Carrefour',       logo: '/images/brands/carrefour.png',   cashback: 'Até 6%',  link: '/parceiros' },
  { id: 12, name: 'Ponto',           logo: '/images/brands/ponto.png',       cashback: 'Até 9%',  link: '/parceiros' },
  { id: 13, name: 'C&A',             logo: '/images/brands/cea.png',         cashback: 'Até 13%', link: '/parceiros' },
  { id: 14, name: 'Riachuelo',       logo: '/images/brands/riachuelo.png',   cashback: 'Até 14%', link: '/parceiros' },
  { id: 15, name: 'Samsung',         logo: '/images/brands/samsung.png',     cashback: 'Até 10%', link: '/parceiros' },
  { id: 16, name: 'Nike',            logo: '/images/brands/nike.png',        cashback: 'Até 12%', link: '/parceiros' },
]
</script>
