<template>
  <section class="qs-brands">
    <div class="container">
      <p class="qs-brands__label">{{ config.brands.label }}</p>
      <div class="qs-brands__grid">
        <div
          v-for="brand in displayBrands"
          :key="brand.nome"
          class="qs-brands__item"
          :title="brand.nome"
          role="button"
          tabindex="0"
          style="cursor:pointer"
          @click="handleBrandClick(brand)"
          @keydown.enter="handleBrandClick(brand)"
        >
          <img
            :src="brand.imagem || brand.imagemPequena || '/img/placeholder.png'"
            :alt="brand.nome"
            loading="lazy"
            @error="(e) => (e.target as HTMLImageElement).style.opacity = '0'"
          />
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

const FALLBACK_BRANDS = [
  { nome: 'Nike', imagem: 'https://logo.clearbit.com/nike.com', link: '' },
  { nome: 'Renner', imagem: 'https://logo.clearbit.com/lojasrenner.com.br', link: '' },
  { nome: 'Puma', imagem: 'https://logo.clearbit.com/puma.com', link: '' },
  { nome: 'Casas Bahia', imagem: 'https://logo.clearbit.com/casasbahia.com.br', link: '' },
  { nome: 'Cobasi', imagem: 'https://logo.clearbit.com/cobasi.com.br', link: '' },
  { nome: 'MadeiraMadeira', imagem: 'https://logo.clearbit.com/madeiramadeira.com.br', link: '' },
  { nome: 'Dafiti', imagem: 'https://logo.clearbit.com/dafiti.com.br', link: '' },
  { nome: 'Vivara', imagem: 'https://logo.clearbit.com/vivara.com.br', link: '' },
  { nome: 'LG', imagem: 'https://logo.clearbit.com/lg.com', link: '' },
  { nome: 'Motorola', imagem: 'https://logo.clearbit.com/motorola.com', link: '' },
  { nome: 'Pernambucanas', imagem: 'https://logo.clearbit.com/pernambucanas.com.br', link: '' },
  { nome: 'Tok&Stok', imagem: 'https://logo.clearbit.com/tokstok.com.br', link: '' },
  { nome: 'Olympikus', imagem: 'https://logo.clearbit.com/olympikus.com.br', link: '' },
  { nome: 'Under Armour', imagem: 'https://logo.clearbit.com/underarmour.com', link: '' },
  { nome: 'Shoptime', imagem: 'https://logo.clearbit.com/shoptime.com.br', link: '' },
  { nome: 'Mizuno', imagem: 'https://logo.clearbit.com/mizuno.com', link: '' },
];

const displayBrands = computed(() => {
  const apiPartners = (partnerStore.newPartners || []).slice(0, 16);
  if (apiPartners.length >= 8) return apiPartners;
  const configItems = config.value.brands?.items;
  if (configItems && configItems.length >= 4) return configItems.slice(0, 16);
  return FALLBACK_BRANDS;
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
  } catch {
    console.warn('[Brands] Erro ao carregar parceiros');
  }
});
</script>

<style scoped>
.qs-brands {
  padding: 40px 0 36px;
  background: #fff;
  border-top: 1px solid #f0f0f0;
  border-bottom: 1px solid #f0f0f0;
}

.qs-brands__label {
  text-align: center;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.14em;
  text-transform: uppercase;
  color: #9ca3af;
  margin-bottom: 28px;
}

.qs-brands__grid {
  display: grid;
  grid-template-columns: repeat(8, 1fr);
  gap: 0;
}

@media (max-width: 991px) {
  .qs-brands__grid {
    grid-template-columns: repeat(6, 1fr);
  }
}

@media (max-width: 767px) {
  .qs-brands__grid {
    grid-template-columns: repeat(4, 1fr);
  }
}

@media (max-width: 479px) {
  .qs-brands__grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

.qs-brands__item {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 16px 20px;
  border-right: 1px solid #f0f4f6;
  border-bottom: 1px solid #f0f4f6;
  transition: background 0.2s ease;
  min-height: 72px;
}

.qs-brands__item:hover {
  background: #f7fbfc;
}

.qs-brands__item img {
  max-width: 90px;
  max-height: 36px;
  object-fit: contain;
  filter: grayscale(100%) opacity(0.65);
  transition: filter 0.25s ease;
}

.qs-brands__item:hover img {
  filter: grayscale(0%) opacity(1);
}
</style>
