<template>
  <div
    :class="`${
      offer_style ? 'tp-product-offer-item' : 'mb-25'
    } tp-product-item transition-3`"
  >
    <div class="tp-product-thumb p-relative fix m-img">
      <nuxt-link target="_blank" :href="`${item.url}`">
        <img :src="item.img" alt="product-electronic" />
      </nuxt-link>

      <!-- product badge -->
      <div class="tp-product-badge">
        <span v-if="item.status === 'out-of-stock'" class="product-hot"
          >out-of-stock</span
        >
      </div>
    </div>
    <!-- product content -->
    <div class="tp-product-content text-center">
      <div class="tp-product-category">
        <nuxt-link target="_blank" :href="`${item.url}`">
          {{ item.category.name }}
        </nuxt-link>
      </div>
      <h3 class="tp-product-title">
        <nuxt-link target="_blank" :href="`${item.url}`">
          {{ item.title }}
        </nuxt-link>
      </h3>
      <div
        class="tp-product-rating d-flex align-items-center justify-content-center"
      >
        <div class="tp-product-rating-icon">
          <!-- Star 1 -->
          <span>
            <i class="fa" :class="getStarClass(1)"></i>
          </span>
          <!-- Star 2 -->
          <span>
            <i class="fa" :class="getStarClass(2)"></i>
          </span>
          <!-- Star 3 -->
          <span>
            <i class="fa" :class="getStarClass(3)"></i>
          </span>
          <!-- Star 4 -->
          <span>
            <i class="fa" :class="getStarClass(4)"></i>
          </span>
          <!-- Star 5 -->
          <span>
            <i class="fa" :class="getStarClass(5)"></i>
          </span>
        </div>
        <div class="tp-product-rating-text">
          <span>({{ item.reviews?.length }} avaliações)</span>
        </div>
      </div>
      <div class="tp-product-price-wrapper">
        <div v-if="item.discount > 0">
          <span class="tp-product-price old-price">${{ item.price }}</span>
          <span class="tp-product-price new-price">
            ${{
              (
                Number(item.price) -
                (Number(item.price) * Number(item.discount)) / 100
              ).toFixed(2)
            }}
          </span>
        </div>
        <span v-else class="tp-product-price new-price d-block text-center">{{
          formatToBRL(item.price)
        }}</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { type IProduct } from "@/types/product-type";

const props = defineProps<{ item: IProduct; offer_style?: boolean }>();

const getAverageRating = (item: IProduct) => {
  if (!item.reviews || item.reviews.length === 0) {
    return 0; // Retorna 0 ou outro valor padrão se não houver avaliações
  }

  const totalRatings = item.reviews.reduce(
    (acc, review) => acc + review.rating,
    0
  );
  const averageRating = totalRatings / item.reviews.length;

  // Garante que a média esteja entre 0 e 5
  return Math.min(5, Math.max(0, averageRating));
};

const getStarClass = (starIndex: number) => {
  const rating = getAverageRating(props.item);

  if (rating >= starIndex) {
    return "fa-solid fa-star";
  } else if (rating >= starIndex - 0.5) {
    return "fa-solid fa-star-half-stroke";
  } else {
    return "fa-regular fa-star";
  }
};

const formatToBRL = (value: number) => {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(value);
};
</script>

<style scoped>
.tp-product-price {
  font-size: 25px;
  margin-top: 10px;
}
</style>
