<template>
  <section class="bg-white py-12 lg:py-16">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <h2 class="text-2xl lg:text-3xl font-bold text-[#225F6B] text-center mb-10">
        Grandes Marcas Que Você Ama, Com Cashback Quanta.
      </h2>

      <div class="relative overflow-hidden">
        <div 
          class="flex animate-scroll"
          :style="{ animationDuration: `${activeBrands.length * 3}s` }"
          @mouseenter="paused = true"
          @mouseleave="paused = false"
        >
          <div 
            v-for="(brand, index) in [...activeBrands, ...activeBrands]" 
            :key="`${brand.id}-${index}`"
            class="flex-shrink-0 px-6 lg:px-10 cursor-pointer"
            @click="handleBrandClick(brand)"
            @keydown.enter="handleBrandClick(brand)"
            tabindex="0"
            role="link"
          >
            <div 
              class="grayscale hover:grayscale-0 opacity-60 hover:opacity-100 transition-all duration-300"
            >
              <img 
                :src="brand.imagem || brand.imagemPequena || '/img/placeholder.png'" 
                :alt="brand.nome"
                class="h-10 lg:h-14 w-auto object-contain"
                loading="lazy"
                @error="(e) => (e.target as HTMLImageElement).style.display = 'none'"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';
import { usePartnerStore } from '@/pinia/usePartnerStore';
import { useAgenciaStore } from '@/pinia/useAgenciaStore';

const { config, loadConfig } = useHomeConfig();
const partnerStore = usePartnerStore();
const agenciaStore = useAgenciaStore();
const paused = ref(false);

const activeBrands = computed(() => {
  const partners = partnerStore.newPartners || [];
  return partners.slice(0, 20);
});

const loopedBrands = computed(() => {
  const list = activeBrands.value;
  if (list.length === 0) return [];
  return [...list, ...list, ...list];
});

function handleBrandClick(brand: any) {
  if (!agenciaStore.isLoggedIn) {
    navigateTo('/login');
    return;
  }
  
  if (brand.link) {
    const url = brand.link.replace('{userId}', String(agenciaStore.user?.idUsuario || ''));
    window.open(url, '_blank', 'noopener');
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await loadConfig();
  try {
    await partnerStore.fetchNewPartners();
  } catch (error) {
    console.error('[Brands] Erro ao carregar parceiros:', error);
  }
});
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
