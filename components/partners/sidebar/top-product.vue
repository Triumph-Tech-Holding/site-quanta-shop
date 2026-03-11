<template>
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-product">
      <div
        v-for="item in topRatedProducts"
        :key="item.product.id"
        class="tp-shop-widget-product-item d-flex align-items-center"
      >
        <div class="tp-shop-widget-product-thumb">
          <nuxt-link :href="`/product-details/${item.product.id}`">
            <img :src="item.product.img" alt="product-img" />
          </nuxt-link>
        </div>
        <div class="tp-shop-widget-product-content">
          <div
            class="tp-shop-widget-product-rating-wrapper d-flex align-items-center"
          >
            <div class="tp-shop-widget-product-rating">
              <!-- Star 1 -->
              <span>
                <i class="fa" :class="getStarClass(1, item.rating)"></i>
              </span>
              <!-- Star 2 -->
              <span>
                <i class="fa" :class="getStarClass(2, item.rating)"></i>
              </span>
              <!-- Star 3 -->
              <span>
                <i class="fa" :class="getStarClass(3, item.rating)"></i>
              </span>
              <!-- Star 4 -->
              <span>
                <i class="fa" :class="getStarClass(4, item.rating)"></i>
              </span>
              <!-- Star 5 -->
              <span>
                <i class="fa" :class="getStarClass(5, item.rating)"></i>
              </span>
            </div>
            <div class="tp-shop-widget-product-rating-number">
              <span>({{ item.rating }})</span>
            </div>
          </div>
          <h4 class="tp-shop-widget-product-title">
            <nuxt-link :href="`/product-details/${item.product.id}`">{{
              item.product.title
            }}</nuxt-link>
          </h4>
          <div class="tp-shop-widget-product-price-wrapper">
            <span class="tp-shop-widget-product-price"
              >{{ formatToBRL(item.product.price) }}</span
            >
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { type IReview, type IProduct } from "@/types/product-type";
import { useProductStore } from "@/pinia/useProductStore";

const productStore = useProductStore();
const product_data = computed(() => productStore.products);

const topRatedProducts = ref<{ product: IProduct; rating: number }[]>([]);

const updateTopRatedProducts = () => {
  const products = product_data.value;
  const getRandomRating = () =>
    parseFloat((Math.random() * (5 - 4) + 4).toFixed(1));
  const shuffledProducts = shuffleArray([...products]);

  topRatedProducts.value = shuffledProducts
    .slice(0, 4)
    .map((product) => ({
      product,
      rating: getRandomRating(),
    }));
};

const shuffleArray = <T>(array: T[]): T[] => {
  let currentIndex = array.length,
    randomIndex;
  while (currentIndex !== 0) {
    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex--;
    [array[currentIndex], array[randomIndex]] = [
      array[randomIndex],
      array[currentIndex],
    ];
  }
  return array;
};

const getAverageRating = (item: IProduct) => {
  if (!item.reviews || item.reviews.length === 0)
    return 0;

  const totalRatings = item.reviews.reduce(
    (acc, review) => acc + review.rating,
    0
  );
  const averageRating = totalRatings / item.reviews.length;

  return Math.min(5, Math.max(0, averageRating));
};

const getStarClass = (starIndex: number, rating: number) => {
  if (rating >= starIndex) {
    return "fa-solid fa-star";
  } else if (rating >= starIndex - 0.5) {
    return "fa-solid fa-star-half-stroke";
  } else {
    return "fa-regular fa-star";
  }
};

watch(
  product_data,
  (newProducts) => {
    updateTopRatedProducts();
  },
  { immediate: true }
);

onMounted(() => {
});
</script>
