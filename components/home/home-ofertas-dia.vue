<template>
  <section class="qs-ofertas">
    <div class="container">
      <div class="qs-section-header">
        <h2 class="qs-section-title">{{ config.ofertas.title }}</h2>
        <p class="qs-section-sub">{{ config.ofertas.subtitle }}</p>
      </div>

      <div v-if="isLoading" class="qs-ofertas__grid">
        <div v-for="n in 4" :key="n" class="qs-ofertas__card qs-ofertas__card--skeleton">
          <div class="qs-ofertas__thumb qs-ofertas__thumb--skeleton">
            <div class="qs-skeleton" style="height:120px;width:80%;border-radius:8px;"></div>
          </div>
          <div class="qs-ofertas__body">
            <div class="qs-skeleton" style="height:11px;width:50%;border-radius:4px;margin-bottom:8px;"></div>
            <div class="qs-skeleton" style="height:14px;width:90%;border-radius:4px;margin-bottom:6px;"></div>
            <div class="qs-skeleton" style="height:14px;width:70%;border-radius:4px;margin-bottom:10px;"></div>
            <div class="qs-skeleton" style="height:24px;width:45%;border-radius:4px;margin-bottom:14px;"></div>
            <div class="qs-skeleton" style="height:36px;width:100%;border-radius:8px;"></div>
          </div>
        </div>
      </div>

      <div v-else-if="offers.length > 0" class="qs-ofertas__grid">
        <div
          v-for="item in offers"
          :key="item.id"
          class="qs-ofertas__card"
        >
          <div class="qs-ofertas__badge" v-if="item.percentualCashback">
            {{ item.percentualCashback }}% Cashback
          </div>
          <div class="qs-ofertas__thumb">
            <img :src="item.imagemPequena || '/img/placeholder.png'" :alt="item.nome" />
          </div>
          <div class="qs-ofertas__body">
            <div class="qs-ofertas__brand">
              <span>{{ item.parceiro }}</span>
            </div>
            <h3 class="qs-ofertas__name">{{ item.nome }}</h3>
            <div class="qs-ofertas__price">R$ {{ formatPrice(item.preco) }}</div>
            <div class="qs-ofertas__cashback-label">
              <svg width="12" height="12" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2.5"><circle cx="12" cy="12" r="10"/><path d="m9 12 2 2 4-4"/></svg>
              Cashback Garantido
            </div>
            <a :href="item.link" target="_blank" rel="noopener" class="qs-ofertas__btn">
              Aproveitar Agora
              <svg width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
            </a>
          </div>
        </div>
      </div>

      <div v-if="!isLoading && offers.length > 0" class="qs-ofertas__more">
        <nuxt-link to="/shop" class="qs-btn-outline-primary">
          Ver Todas as Ofertas
          <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </nuxt-link>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, ref, onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';
import { getProducts } from '@/services/product-service';

const { config, loadConfig } = useHomeConfig();

const isLoading = ref(true);
const apiProducts = ref<Array<Record<string, unknown>>>([]);
const mockOffers = ref<Array<Record<string, unknown>>>([]);

function normalizeProduct(p: Record<string, unknown>) {
  return {
    id: p['aw_product_id'] as string,
    nome: p['product_name'] as string,
    imagemPequena: p['merchant_image_url'] as string,
    parceiro: p['merchant_name'] as string,
    percentualCashback: p['cashback'] as number,
    preco: p['search_price'] as string,
    link: p['aw_deep_link'] as string,
  };
}

onMounted(async () => {
  loadConfig();

  try {
    const result = await getProducts(4, 1, null, null, null, null) as { data?: { products?: unknown[] } };
    const products = result?.data?.products;
    if (Array.isArray(products) && products.length > 0) {
      apiProducts.value = products.map(p => normalizeProduct(p as Record<string, unknown>));
    }
  } catch {
    console.warn('[home-ofertas-dia] API de produtos indisponível, usando fallback');
  }

  if (apiProducts.value.length === 0) {
    try {
      const res = await fetch('/data/mock-data.json');
      const data = await res.json();
      mockOffers.value = (data.ofertas || []).slice(0, 4);
    } catch {
      console.warn('[home-ofertas-dia] Fallback mock também falhou');
    }
  }

  isLoading.value = false;
});

const offers = computed(() =>
  apiProducts.value.length > 0 ? apiProducts.value : mockOffers.value
);

function formatPrice(price: unknown): string {
  if (!price) return '0,00';
  const num = typeof price === 'string' ? parseFloat(price) : Number(price);
  if (isNaN(num)) return '0,00';
  return num.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
</script>

<style scoped>
.qs-ofertas {
  padding: 72px 0;
  background: #fff;
}

.qs-section-header {
  text-align: center;
  margin-bottom: 40px;
}

.qs-section-title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(24px, 4vw, 36px);
  font-weight: 800;
  color: #225F6B;
  letter-spacing: -0.03em;
  margin-bottom: 8px;
}

.qs-section-sub {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  color: #2F7785;
  max-width: 480px;
  margin: 0 auto;
}

.qs-ofertas__grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  margin-bottom: 36px;
}

@media (max-width: 1199px) { .qs-ofertas__grid { grid-template-columns: repeat(3, 1fr); } }
@media (max-width: 767px) { .qs-ofertas__grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 575px) { .qs-ofertas__grid { grid-template-columns: 1fr; } }

.qs-ofertas__card {
  position: relative;
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  overflow: hidden;
  transition: all 0.25s ease;
}

.qs-ofertas__card:hover {
  box-shadow: 0 8px 32px rgba(0,0,0,0.12);
  transform: translateY(-4px);
  border-color: transparent;
}

.qs-ofertas__card--skeleton {
  pointer-events: none;
}

.qs-ofertas__badge {
  position: absolute;
  top: 12px;
  left: 12px;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 700;
  padding: 3px 10px;
  border-radius: 999px;
  z-index: 1;
}

.qs-ofertas__thumb {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 160px;
  background: #f9fafb;
  padding: 16px;
  border-bottom: 1px solid #f0f0f0;
}

.qs-ofertas__thumb--skeleton {
  border-bottom: none;
}

.qs-ofertas__thumb img {
  max-height: 120px;
  max-width: 100%;
  object-fit: contain;
}

.qs-ofertas__body {
  padding: 16px;
}

.qs-ofertas__brand {
  margin-bottom: 6px;
}

.qs-ofertas__brand span {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  color: #6b7280;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.qs-ofertas__name {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 600;
  color: #225F6B;
  line-height: 1.4;
  margin-bottom: 8px;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.qs-ofertas__price {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 20px;
  font-weight: 800;
  color: #225F6B;
  margin-bottom: 6px;
}

.qs-ofertas__cashback-label {
  display: flex;
  align-items: center;
  gap: 4px;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  color: #2F7785;
  font-weight: 500;
  margin-bottom: 14px;
}

.qs-ofertas__btn {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
  width: 100%;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 700;
  padding: 10px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.qs-ofertas__btn:hover {
  background: #7aad1f;
  color: #fff;
  transform: translateY(-1px);
}

.qs-ofertas__more {
  text-align: center;
}

.qs-btn-outline-primary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  border: 2px solid #2F7785;
  color: #2F7785;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 600;
  padding: 12px 28px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.qs-btn-outline-primary:hover {
  background: #2F7785;
  color: #fff;
}

.qs-skeleton {
  background: linear-gradient(90deg, #e8edf0 25%, #f5f8fa 50%, #e8edf0 75%);
  background-size: 200% 100%;
  animation: qs-skeleton-wave 1.5s infinite;
}

@keyframes qs-skeleton-wave {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
</style>
