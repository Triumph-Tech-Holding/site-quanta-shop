<template>
  <div>
    <!-- breadcrumb start -->
    <product-details-breadcrumb :product="product" />
    <!-- breadcrumb end -->

    <!-- product details area start -->
    <product-details-gallery-area :product="product" />
    <!-- product details area end -->

    <!-- related products start -->
    <product-related
      :product-id="product.id"
      :category="product.category.name"
    />
    <!-- related products end -->
  </div>
</template>

<script setup lang="ts">
useSeoMeta({ title: "Product Details With Gallery Page" });

import { useProductStore } from "@/pinia/useProductStore";

const productStore = useProductStore();
const product_data = computed(() => productStore.products);
const product = product_data.value[3];

onMounted(() => {
  productStore.activeImg = product.img;
});
</script>
