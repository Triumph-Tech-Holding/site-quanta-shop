<template>
  <section class="relative -mt-8 z-10 px-4 sm:px-6 lg:px-8">
    <div class="max-w-4xl mx-auto">
      <div class="bg-white rounded-2xl shadow-xl p-4 sm:p-6">
        <div class="flex items-center gap-4">
          <div class="flex-shrink-0">
            <svg class="w-6 h-6 text-[#2F7785]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </div>
          <input 
            v-model="searchQuery"
            type="text"
            placeholder="Busque produtos, grandes marcas ou lojas por CEP..."
            class="flex-1 text-gray-700 placeholder-gray-400 text-lg focus:outline-none"
            @keyup.enter="handleSearch"
          />
          <button 
            @click="handleVoiceSearch"
            class="flex-shrink-0 p-2 text-[#2F7785] hover:bg-[#F4F4F5] rounded-full transition-colors"
            aria-label="Busca por voz"
          >
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11a7 7 0 01-7 7m0 0a7 7 0 01-7-7m7 7v4m0 0H8m4 0h4m-4-8a3 3 0 01-3-3V5a3 3 0 116 0v6a3 3 0 01-3 3z" />
            </svg>
          </button>
          <button 
            @click="handleSearch"
            class="flex-shrink-0 bg-[#98C73A] text-[#225F6B] font-semibold px-6 py-2.5 rounded-full hover:bg-[#8ab832] transition-colors hidden sm:block"
          >
            Buscar
          </button>
        </div>
        <div class="flex flex-wrap gap-2 mt-4 pt-4 border-t border-gray-100">
          <span class="text-sm text-gray-500">Populares:</span>
          <button 
            v-for="tag in popularTags" 
            :key="tag"
            @click="searchQuery = tag"
            class="text-sm text-[#2F7785] hover:text-[#225F6B] hover:underline"
          >
            {{ tag }}
          </button>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
const searchQuery = ref('')

const popularTags = [
  'Eletrônicos',
  'Moda',
  'Restaurantes',
  'Supermercados',
  'Combustível',
]

const handleSearch = () => {
  if (searchQuery.value.trim()) {
    navigateTo(`/shop?q=${encodeURIComponent(searchQuery.value)}`)
  }
}

const handleVoiceSearch = () => {
  console.log('Voice search triggered')
}
</script>
