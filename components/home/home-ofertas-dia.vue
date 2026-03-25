<template>
  <section class="qs-ofertas" v-if="offers.length > 0">
    <div class="container">
      <div class="qs-section-header">
        <h2 class="qs-section-title">Ofertas do Dia</h2>
        <p class="qs-section-sub">Produtos selecionados com cashback turbinado. Aproveite antes que acabe!</p>
      </div>

      <div class="qs-ofertas__grid">
        <div
          v-for="item in offers.slice(0, 4)"
          :key="item.id"
          class="qs-ofertas__card"
        >
          <div class="qs-ofertas__badge" v-if="item.percentualCashback">
            {{ item.percentualCashback }}% Cashback
          </div>
          <div class="qs-ofertas__thumb">
            <img :src="item.imagemPequena || item.imagem || '/img/placeholder.png'" :alt="item.nome" />
          </div>
          <div class="qs-ofertas__body">
            <div class="qs-ofertas__brand">
              <img v-if="item.imagemParceiro" :src="item.imagemParceiro" :alt="item.parceiro" class="qs-ofertas__brand-logo" />
              <span>{{ item.parceiro }}</span>
            </div>
            <h3 class="qs-ofertas__name">{{ item.nome }}</h3>
            <div class="qs-ofertas__price">R$ {{ formatPrice(item.preco) }}</div>
            <div class="qs-ofertas__cashback-label">
              <svg width="12" height="12" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2.5"><circle cx="12" cy="12" r="10"/><path d="m9 12 2 2 4-4"/></svg>
              Cashback Interno
            </div>
            <a :href="item.link" target="_blank" rel="noopener" class="qs-ofertas__btn">
              Aproveitar Agora
              <svg width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
            </a>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { usePartnerStore } from "@/pinia/usePartnerStore";

const partnerStore = usePartnerStore();

const offers = computed(() => {
  return (partnerStore.featuredPartners || []).slice(0, 4);
});

function formatPrice(price: any): string {
  if (!price) return '0,00';
  const num = typeof price === 'string' ? parseFloat(price) : price;
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
  color: #111827;
  letter-spacing: -0.03em;
  margin-bottom: 8px;
}

.qs-section-sub {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  color: #6b7280;
  max-width: 480px;
  margin: 0 auto;
}

.qs-ofertas__grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
}

@media (max-width: 1199px) { .qs-ofertas__grid { grid-template-columns: repeat(2, 1fr); } }
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

.qs-ofertas__thumb img {
  max-height: 120px;
  max-width: 100%;
  object-fit: contain;
}

.qs-ofertas__body {
  padding: 16px;
}

.qs-ofertas__brand {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 6px;
}

.qs-ofertas__brand-logo {
  height: 20px;
  width: auto;
  object-fit: contain;
}

.qs-ofertas__brand span {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  color: #6b7280;
  font-weight: 500;
}

.qs-ofertas__name {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 600;
  color: #111827;
  line-height: 1.4;
  margin-bottom: 8px;
}

.qs-ofertas__price {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 20px;
  font-weight: 800;
  color: #111827;
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
</style>
