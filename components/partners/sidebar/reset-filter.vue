<template>
  <div class="tp-shop-widget mb-50">
    <h3 class="tp-shop-widget-title">Limpar filtros</h3>
    <button @click="handleFilterReset" class="tp-btn w-100">Limpar</button>
  </div>
</template>

<script setup lang="ts">
import { useProductFilterStore } from '@/pinia/useProductFilterStore';

const store = useProductFilterStore();
const router = useRouter();
const props = defineProps<{ shop_full?: boolean }>();

const handleFilterReset = () => {
  store.handleResetFilter();
  router.push(props.shop_full ? '/shop-full-width' : '/shop');
};
</script>
