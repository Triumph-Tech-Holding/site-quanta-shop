<template>
  <div
    :class="`${
      offer_style ? 'tp-product-offer-item' : 'mb-25'
    } tp-product-item transition-3`"
    style="min-height: 560px"
  >
    <div class="tp-product-thumb p-relative fix m-img">
      <nuxt-link :href="`/product-details/${item.id}`">
        <img :src="item.img" alt="product-electronic" @error="onImageError" />
      </nuxt-link>

      <!-- product badge -->
      <div class="tp-product-badge">
        <span v-if="item.merchant.name !== item.brand.name" class="product-hot">
          {{ item.merchant.name }}
        </span>
        <span v-if="item.status === 'out-of-stock'" class="product-hot mx-2">
          {{ item.brand.name }}
        </span>
        <span v-if="item.merchant.cashback > 0" class="product-hot">
          {{ formatToPercentage(item.merchant.cashback, 0) }}
        </span>
      </div>

      <!-- product action -->
      <!-- <div class="tp-product-action">
        <div class="tp-product-action-item d-flex flex-column">
          <button
            v-if="!isItemInCart(item)"
            @click="cartStore.addCartProduct(item)"
            type="button"
            :class="`tp-product-action-btn tp-product-add-cart-btn ${
              isItemInCart(item) ? 'active' : ''
            }`"
          >
            <svg-add-cart />
            <span class="tp-product-tooltip">Add to Cart</span>
          </button>
          <nuxt-link
            v-if="isItemInCart(item)"
            href="/cart"
            :class="`tp-product-action-btn tp-product-add-cart-btn ${
              isItemInCart(item) ? 'active' : ''
            }`"
          >
            <svg-add-cart />
            <span class="tp-product-tooltip">View Cart</span>
          </nuxt-link>

          <button
            type="button"
            class="tp-product-action-btn tp-product-quick-view-btn"
            data-bs-toggle="modal"
            :data-bs-target="`#${utilityStore.modalId}`"
            @click="
              utilityStore.handleOpenModal(`product-modal-${item.id}`, item)
            "
          >
            <svg-quick-view />
            <span class="tp-product-tooltip">Quick View</span>
          </button>
          <button
            @click="wishlistStore.add_wishlist_product(item)"
            type="button"
            :class="`tp-product-action-btn tp-product-add-to-wishlist-btn ${
              isItemInWishlist(item) ? 'active' : ''
            }`"
          >
            <svg-wishlist />
            <span class="tp-product-tooltip">
              {{
                isItemInWishlist(item)
                  ? "Remove From Wishlist"
                  : "Add To Wishlist"
              }}
            </span>
          </button>
        </div>
      </div> -->
    </div>

    <!-- product content -->
    <div
      class="tp-product-content d-flex flex-column align-items-start justify-content-between"
    >
      <!-- START: product category -->
      <div class="tp-product-category">
        <nuxt-link :href="`/product-details/${item.id}`">
          {{ item.category.name }}
        </nuxt-link>
      </div>
      <!-- END: product category -->

      <h3 class="tp-product-title">
        <nuxt-link :href="`/product-details/${item.id}`">
          {{ item.title }}
        </nuxt-link>
      </h3>

      <!-- START: product rating -->
      <div
        class="tp-product-rating d-flex align-items-center justify-content-start"
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
          <span>({{ item.reviews?.length ?? 0 }} avaliações)</span>
        </div>
      </div>
      <!-- END: product rating -->

      <!-- START: product price -->
      <div class="tp-product-price-wrapper mb-0">
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
        <div v-else class="tp-product-price new-price">
          <span>{{ formatToBRL(item.price) }}</span>
          <span class="d-block">e até {{ formatToPercentage(item.merchant.cashback) }} de cashback</span>          
        </div>
      </div>
      <!-- END: product price -->

      <div
        class="tp-product-list-add-to-cart mt-2 w-100"
        v-if="userStore.userId != null && item.link != null"
      >
        <nuxt-link
          :to="`/product-details/${item.id}`"
          target="_blank"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
        >
          Comprar
        </nuxt-link>
      </div>
      <div class="tp-product-list-add-to-cart w-100" v-else>
        <nuxt-link
          to="/login"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
        >
          Faça login para comprar
        </nuxt-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from "@/pinia/useUserStore";
import { type IProduct } from "@/types/product-type";

const props = defineProps<{ item: IProduct; offer_style?: boolean }>();

const userStore = useUserStore();

onMounted(async () => {
  await userStore.loadUserFromStorage();
});

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

const onImageError = (event: { target: { src: string } }) => {
  event.target.src = "/img/category/main/category-main.png";
};
</script>
<style scoped>
.tp-product-title {
  width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  margin-bottom: 0;
}
</style>
