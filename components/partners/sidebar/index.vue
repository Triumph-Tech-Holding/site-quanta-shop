<template>
  <div class="tp-shop-sidebar mr-10">
    <!-- filter -->
    <!-- <div class="tp-shop-widget mb-35"> -->
      <!-- <h3 class="tp-shop-widget-title no-border">Filtro por preço</h3> -->
      <!-- price filter start -->
      <!-- <partners-sidebar-price-filter
        :right_side="shop_right"
        :shop_full="shop_full"
        :shop_1600="shop_1600"
      /> -->
      <!-- price filter end -->
    <!-- </div> -->
    <!-- status -->
    <!-- <div class="tp-shop-widget mb-50">
      <h3 class="tp-shop-widget-title">Product Status</h3> -->
    <!-- filter status start -->
    <!-- <shop-sidebar-filter-status
        :right_side="shop_right"
        :shop_full="shop_full"
        :shop_1600="shop_1600"
      /> -->
    <!-- filter status end -->
    <!-- </div> -->
    
    <!-- filter type start -->
    <div class="tp-shop-widget mb-30">
      <h3 class="tp-shop-widget-title">Tipo de parceiro</h3>
      <!-- filter type start -->
      <partners-sidebar-filter-type
        :right_side="shop_right"
        :shop_full="shop_full"
        :shop_1600="shop_1600"
      />
    </div>
    <!-- filter type end -->
    
    <!-- categories -->
    <div class="tp-shop-widget mb-50">
      <h3 class="tp-shop-widget-title">Categorias</h3>
      <!-- filter categories start -->
      <partners-sidebar-filter-categories
        :right_side="shop_right"
        :shop_full="shop_full"
        :shop_1600="shop_1600"
      />
      <!-- filter categories end -->
    </div>
    <!-- product rating -->
    <div class="tp-shop-widget mb-50">
      <h3 class="tp-shop-widget-title">Produtos mais bem avaliados</h3>
      <!-- filter top-product start -->
      <partners-sidebar-top-product />
      <!-- filter top-product end -->
    </div>
    <!-- brand -->
    <div class="tp-shop-widget mb-50">
      <h3 class="tp-shop-widget-title">Marcas populares</h3>
      <!-- filter brand start -->
      <partners-sidebar-filter-brand
        :right_side="shop_right"
        :shop_full="shop_full"
        :shop_1600="shop_1600"
      />
      <!-- filter brand end -->
    </div>
    <!-- reset filter start -->
    <partners-sidebar-reset-filter
      :right_side="shop_right"
      :shop_full="shop_full"
      :shop_1600="shop_1600"
    />
    <!-- reset filter end -->
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  shop_right?: boolean;
  shop_full?: boolean;
  shop_1600?: boolean;
}>();
</script>
