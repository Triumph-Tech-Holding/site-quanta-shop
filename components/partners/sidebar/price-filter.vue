<template>
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-filter price__slider">
      <div id="slider-range" class="mb-10">
        <Slider :value="store.priceValues" :tooltips="false" @change="store.handlePriceChange" :max="store.maxProductPrice" />
      </div>
      <div class="tp-shop-widget-filter-info d-flex align-items-center justify-content-between">
        <span class="input-range">{{ formatToBRL(store.priceValues[0]) }} - {{ formatToBRL(store.priceValues[1]) }}</span>
        <button @click="handlePriceFilter" class="tp-shop-widget-filter-btn" type="button">Filtrar</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import Slider from "@vueform/slider";
import "@vueform/slider/themes/default.css";
import { usePartnerFilterStore } from "@/pinia/usePartnerFilterStore";

const props = defineProps<{ shop_full?: boolean }>();
const store = usePartnerFilterStore();
const router = useRouter();
const route = useRoute();

const handlePriceFilter = () => {
  const price_query = `minPrice=${store.priceValues[0]}&maxPrice=${store.priceValues[1]}`;
  const target = props.shop_full ? '/shop-full-width' : '/shop';
  router.push(`${target}?${price_query}`);
};

const formatToBRL = (value: number) =>
  new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(value);

onMounted(() => {
  if (route.query.minPrice && route.query.maxPrice) {
    store.partners = [Number(route.query.minPrice), Number(route.query.maxPrice)];
  }
});
</script>
