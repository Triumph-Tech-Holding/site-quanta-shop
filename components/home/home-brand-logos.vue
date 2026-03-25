<template>
  <section class="qs-brands">
    <div class="container">
      <p class="qs-brands__label">AS MAIORES MARCAS CONFIAM NA QUANTA</p>
      <div class="qs-brands__grid">
        <a
          v-for="brand in activeBrands"
          :key="brand.id"
          :href="brand.url"
          target="_blank"
          rel="noopener"
          class="qs-brands__item"
          :title="brand.nome"
        >
          <img :src="brand.logo" :alt="brand.nome" loading="lazy" />
        </a>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';

interface Brand {
  id: number;
  nome: string;
  logo: string;
  url: string;
  ativo: boolean;
  ordem: number;
}

const brands = ref<Brand[]>([]);

const activeBrands = computed(() => 
  brands.value.filter(b => b.ativo).sort((a, b) => a.ordem - b.ordem)
);

onMounted(async () => {
  try {
    brands.value = await $fetch('/data/brands.json');
  } catch (error) {
    console.error('[Brands] Erro ao carregar:', error);
  }
});
</script>

<style scoped>
.qs-brands {
  padding: 36px 0;
  background: #fff;
  border-top: 1px solid #f0f0f0;
  border-bottom: 1px solid #f0f0f0;
}

.qs-brands__label {
  text-align: center;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: #9ca3af;
  margin-bottom: 24px;
}

.qs-brands__grid {
  display: grid;
  grid-template-columns: repeat(8, 1fr);
  gap: 20px;
  align-items: center;
}

@media (max-width: 991px) {
  .qs-brands__grid {
    grid-template-columns: repeat(4, 1fr);
  }
}

@media (max-width: 575px) {
  .qs-brands__grid {
    grid-template-columns: repeat(3, 1fr);
    gap: 12px;
  }
}

.qs-brands__item {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 60px;
  text-decoration: none;
  transition: all 0.3s ease;
  filter: grayscale(1) opacity(0.65);
}

.qs-brands__item:hover {
  filter: grayscale(0) opacity(1);
  transform: scale(1.05);
}

.qs-brands__item img {
  max-width: 100%;
  max-height: 100%;
  object-fit: contain;
}
</style>
