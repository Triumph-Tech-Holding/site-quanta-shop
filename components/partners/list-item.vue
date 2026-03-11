<template>
  <div class="tp-product-list-item d-md-flex">
    <div class="tp-product-list-thumb p-relative fix" style="height: 321px">
      <nuxt-link
        :href="`/partners/${item.IdAnunciante}`"
        style="height: 310px; background-color: #ffffff; border: 1px solid #ddd"
      >
        <img :src="item.Icone" alt="product-img" @error="onImageError" />
      </nuxt-link>
    </div>
    <div class="tp-product-list-content">
      <div class="tp-product-content-2 pt-15">
        <div class="tp-product-tag-2">
          <a href="#">{{ item.Categoria }}</a>
        </div>
        <h3 class="tp-product-title-2">
          <nuxt-link :href="`/partners/${item.IdAnunciante}`">
            {{ item.Nome }}
          </nuxt-link>
        </h3>

        <div class="tp-product-price-wrapper-2">
          <span class="tp-product-price-2 new-price">
            Até {{ formatToPercentage(item.MaxCashback) }} de cashback
          </span>
        </div>

        <p>{{ item.Nome.slice(0, 100) }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { formatToPercentage } from "@/utils/filters";

defineProps<{ item: any }>();

const onImageError = (event: { target: { src: string } }) => {
  event.target.src = "/img/category/main/category-main.png";
};
</script>
