<template>
  <ui-loading-indicator />
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-filter price__slider">
      <div id="slider-range" class="mb-10">
        <Slider
          :value="store.priceValues"
          :tooltips="false"
          @change="store.handlePriceChange"
          :max="store.maxProductPrice"
        />
      </div>
      <div
        class="tp-shop-widget-filter-info d-flex align-items-center justify-content-between"
      >
        <span class="input-range">
          {{ formatToBRL(store.priceValues[0]) }} - {{ formatToBRL(store.priceValues[1]) }}
        </span>
        <button
          @click="handlePriceFilter"
          class="tp-shop-widget-filter-btn"
          type="button"
        >
          Filtrar
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import Slider from "@vueform/slider";
import "@vueform/slider/themes/default.css";
import { useProductFilterStore } from "@/pinia/useProductFilterStore";
import { useProductStore } from "@/pinia/useProductStore";
import { useLoadingStore } from "@/pinia/useLoadingStore";

const loadingStore = useLoadingStore();

const props = defineProps<{
  filter_dropdown?: boolean;
  filter_offcanvas?: boolean;
  load_more?: boolean;
  right_side?: boolean;
  shop_full?: boolean;
  shop_1600?: boolean;
}>();
const store = useProductFilterStore();
const router = useRouter();
const route = useRoute();
const productStore = useProductStore();

const handlePriceFilter = () => {
  loadingStore.setLoading(true);

  const price_query = `minPrice=${store.priceValues[0]}&maxPrice=${store.priceValues[1]}`;
  const queryParamsInRoute = Object.keys(route.params);

  // Remove minPrice e maxPrice da query se já existirem
  const queryString = Object.entries(route.query)
    .filter(([key]) => !queryParamsInRoute.includes(key) && key !== 'minPrice' && key !== 'maxPrice')
    .map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(String(value))}`)
    .join('&');

  const fullQueryString = queryString ? `&${queryString}` : '';

  if (props.filter_dropdown) {
    router.push(`/shop-filter-dropdown?${price_query}${fullQueryString}`);
  } else if (props.filter_offcanvas) {
    router.push(`/shop-filter-offcanvas?${price_query}${fullQueryString}`);
  } else if (props.load_more) {
    router.push(`/shop-load-more?${price_query}${fullQueryString}`);
  } else if (props.right_side) {
    router.push(`/shop-right-sidebar?${price_query}${fullQueryString}`);
  } else if (props.shop_full) {
    router.push(`/shop-full-width?${price_query}${fullQueryString}`);
  } else if (props.shop_1600) {
    router.push(`/shop-1600?${price_query}${fullQueryString}`);
  } else {
    router.push(`/shop?${price_query}${fullQueryString}`);
  }

  setTimeout(() =>{
    loadingStore.setLoading(false);
    }, 1100);
  productStore.fetchProducts(store.totalItensPerPage, store.actualPage, store.priceValues[0], store.priceValues[1], store.selectVal, store.category);  
};

const formatToBRL = (value: number) => {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(value);
};

onMounted(() => {
  if (route.query.minPrice && route.query.maxPrice) {
    store.priceValues = [
      Number(route.query.minPrice),
      Number(route.query.maxPrice),
    ];
  }
});
</script>
