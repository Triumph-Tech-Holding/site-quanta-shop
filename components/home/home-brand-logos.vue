<template>
  <section class="qs-brands">
    <div class="container">
      <p class="qs-brands__label">{{ config.brands.label }}</p>
      <div class="qs-brands__track-wrap">
        <div class="qs-brands__track" :class="{ 'qs-brands__track--paused': paused }">
          <a
            v-for="(brand, i) in loopedBrands"
            :key="`${brand.id}-${i}`"
            :href="brand.url"
            target="_blank"
            rel="noopener"
            class="qs-brands__item"
            :title="brand.nome"
            @mouseenter="paused = true"
            @mouseleave="paused = false"
          >
            <img :src="brand.logo" :alt="brand.nome" loading="lazy" />
          </a>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';

interface Brand {
  id: number;
  nome: string;
  logo: string;
  url: string;
  ativo: boolean;
  ordem: number;
}

const { config, loadConfig } = useHomeConfig();
const brands = ref<Brand[]>([]);
const paused = ref(false);

const activeBrands = computed(() =>
  brands.value.filter(b => b.ativo).sort((a, b) => a.ordem - b.ordem)
);

const loopedBrands = computed(() => {
  const list = activeBrands.value;
  if (list.length === 0) return [];
  return [...list, ...list, ...list];
});

onMounted(async () => {
  await loadConfig();
  try {
    brands.value = await $fetch<Brand[]>('/data/brands.json');
  } catch (error) {
    console.error('[Brands] Erro ao carregar:', error);
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
  animation: qs-brands-scroll 28s linear infinite;
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
  width: 160px;
  height: 64px;
  padding: 0 20px;
  text-decoration: none;
  transition: filter 0.3s ease;
  filter: grayscale(1) opacity(0.55);
}

.qs-brands__item:hover {
  filter: grayscale(0) opacity(1);
}

.qs-brands__item img {
  max-width: 100%;
  max-height: 40px;
  object-fit: contain;
}

@media (max-width: 575px) {
  .qs-brands__item {
    width: 110px;
    padding: 0 12px;
  }
}
</style>
