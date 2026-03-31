<template>
  <section class="qs-brands">
    <div class="container">
      <p class="qs-brands__label">{{ config.brands.label }}</p>
      <div class="qs-brands__track-wrap">
        <div class="qs-brands__track" :class="{ 'qs-brands__track--paused': paused }">
          <div
            v-for="(brand, i) in loopedBrands"
            :key="`${brand.id}-${i}`"
            class="qs-brands__item"
            role="link"
            tabindex="0"
            :title="brand.nome"
            style="cursor: pointer"
            @click="handleBrandClick(brand)"
            @keydown.enter="handleBrandClick(brand)"
            @mouseenter="paused = true"
            @mouseleave="paused = false"
          >
            <img :src="brand.imagem || brand.imagemPequena || '/img/placeholder.png'" :alt="brand.nome" loading="lazy" @error="(e) => (e.target as HTMLImageElement).style.display = 'none'" />
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
import { useUserStore } from '@/pinia/useUserStore';
import { useRouter } from 'vue-router';

const { config, loadConfig } = useHomeConfig();
const partnerStore = usePartnerStore();
const userStore = useUserStore();
const router = useRouter();
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
  if (!userStore.isLoggedIn) {
    router.push('/login');
    return;
  }
  
  if (brand.link) {
    const url = brand.link.replace('{userId}', userStore.userId || '');
    window.open(url, '_blank', 'noopener');
  }
}

onMounted(async () => {
  await loadConfig();
  try {
    await partnerStore.fetchNewPartners();
  } catch (error) {
    console.error('[Brands] Erro ao carregar parceiros:', error);
  }
});
</script>

<style scoped>
.qs-brands {
  padding: 32px 0;
  background: #fff;
  border-top: 1px solid #f0f0f0;
  border-bottom: 1px solid #f0f0f0;
  overflow: hidden;
}

.qs-brands__label {
  text-align: center;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: #9ca3af;
  margin-bottom: 20px;
}

.qs-brands__track-wrap {
  overflow: hidden;
  width: 100%;
  position: relative;
}

.qs-brands__track-wrap::before,
.qs-brands__track-wrap::after {
  content: '';
  position: absolute;
  top: 0;
  bottom: 0;
  width: 60px;
  z-index: 2;
  pointer-events: none;
}

.qs-brands__track-wrap::before {
  left: 0;
  background: linear-gradient(to right, #fff, transparent);
}

.qs-brands__track-wrap::after {
  right: 0;
  background: linear-gradient(to left, #fff, transparent);
}

.qs-brands__track {
  display: flex;
  align-items: center;
  gap: 0;
  width: max-content;
  animation: qs-brands-scroll 35s linear infinite;
}

.qs-brands__track--paused {
  animation-play-state: paused;
}

@keyframes qs-brands-scroll {
  0% { transform: translateX(0); }
  100% { transform: translateX(-33.333%); }
}

.qs-brands__item {
  display: flex;
  flex-shrink: 0;
  align-items: center;
  justify-content: center;
  width: 210px;
  height: 108px;
  padding: 0 24px;
  text-decoration: none;
  transition: filter 0.3s ease;
  filter: none;
}

.qs-brands__item:hover {
  filter: none;
}

.qs-brands__item img {
  max-width: 100%;
  max-height: 80px;
  object-fit: contain;
}

@media (max-width: 575px) {
  .qs-brands__item {
    width: 110px;
    padding: 0 12px;
  }
}
</style>
