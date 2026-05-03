<template>
  <div class="tp-shop-widget mb-50">
    <h3 class="tp-shop-widget-title">Limpar filtros</h3>
    <button @click="handleFilterReset" class="tp-btn w-100">Limpar</button>
  </div>
</template>

<script setup lang="ts">
import { useProductFilterStore } from '@/pinia/useProductFilterStore';
import { useProductStore } from '@/pinia/useProductStore';

const store = useProductFilterStore();
const productStore = useProductStore();
const router = useRouter();
const props = defineProps<{ shop_full?: boolean }>();

const handleFilterReset = async () => {
  store.handleResetFilter();
  if (props.shop_full) {
    router.push('/shop-full-width');
  } else {
    router.push('/shop');
    store.handleResetFilter();
    await productStore.fetchProducts(12, 1, null, null, null, null);
  }
};
</script>
