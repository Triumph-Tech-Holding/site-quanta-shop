<template>
  <div :class="`tp-product-item-2 ${spacing ? 'mb-40' : ''}`">
    <div
      class="tp-product-thumb-2 p-relative z-index-1 fix w-img d-flex align-items-center"
      style="
        background-color: #ffffff;
        min-height: 306px;
        max-height: 306px;
        border: 1px solid #ddd;
      "
    >
      <nuxt-link
        :href="`/product-details/${item.id}`"
        class="w-100 h-100"
        v-if="item.type === 'PRODUCT'"
      >
        <img :src="item.img" alt="product-img" @error="onImageError" />
      </nuxt-link>

      <nuxt-link
        :href="`/partners/${item.id}`"
        class="w-100 h-100"
        v-if="item.type === 'LOCAL'"
      >
        <img :src="item.img" alt="product-img" @error="onImageError" />
      </nuxt-link>

      <nuxt-link
        :href="item.link?.replace('{userId}', userStore.userId)"
        class="w-100 h-100"
        v-if="item.type === 'ONLINE'"
        target="_blank"
      >
        <img :src="item.img" alt="product-img" @error="onImageError" />
      </nuxt-link>

      <!-- product badge -->
      <div class="tp-product-badge">
        <span v-if="item.merchant.name !== item.brand.name && item.type === 'PRODUCT'" class="product-hot">
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
      <div class="tp-product-action-2 tp-product-action-blackStyle">
        <div class="tp-product-action-item-2 d-flex flex-column">
          <!-- <button
            v-if="!isItemInCart(item)"
            @click="cartStore.addCartProduct(item)"
            type="button"
            :class="`tp-product-action-btn-2 tp-product-add-cart-btn ${
              isItemInCart(item) ? 'active' : ''
            }`"
          >
            <svg-add-cart />
            <span class="tp-product-tooltip tp-product-tooltip-right"
              >Add to Cart</span
            >
          </button> -->
          <!-- <nuxt-link
            v-if="isItemInCart(item)"
            href="/cart"
            :class="`tp-product-action-btn-2 tp-product-add-cart-btn ${
              isItemInCart(item) ? 'active' : ''
            }`"
          >
            <svg-add-cart />
            <span class="tp-product-tooltip tp-product-tooltip-right"
              >View Cart</span
            >
          </nuxt-link> -->

          <!-- <button
            type="button"
            class="tp-product-action-btn-2 tp-product-quick-view-btn"
            data-bs-toggle="modal"
            :data-bs-target="`#${utilityStore.modalId}`"
            @click="
              utilityStore.handleOpenModal(`product-modal-${item.id}`, item)
            "
          >
            <svg-quick-view />
            <span class="tp-product-tooltip tp-product-tooltip-right"
              >Ver mais</span
            >
          </button> -->

          <!-- <button
            @click="wishlistStore.add_wishlist_product(item)"
            type="button"
            :class="`tp-product-action-btn-2 tp-product-add-to-wishlist-btn ${
              isItemInWishlist(item) ? 'active' : ''
            }`"
          >
            <svg-wishlist />
            <span class="tp-product-tooltip tp-product-tooltip-right">
              {{
                isItemInWishlist(item)
                  ? "Remove From Wishlist"
                  : "Add To Wishlist"
              }}
            </span>
          </button> -->

          <!-- <button
            @click="compareStore.add_compare_product(item)"
            type="button"
            :class="`tp-product-action-btn-2 tp-product-add-to-compare-btn ${
              isItemInCompare(item) ? 'active' : ''
            }`"
          >
            <svg-compare-2 />
            <span class="tp-product-tooltip tp-product-tooltip-right">
              {{
                isItemInCompare(item) ? "Remove From Compare" : "Add To Compare"
              }}
            </span>
          </button> -->
        </div>
      </div>
    </div>

    <div
      class="tp-product-content-2 pt-15 d-flex flex-column justify-content-start"
    >
      <div class="tp-product-tag-2">
        <a href="#">{{ item.category.name }}</a>
      </div>
      <h3 class="tp-product-title-2">
        <span class="tp-product-code d-block mt-2">
          {{ padLeft(item.id.toString(), 11) }}
        </span>

        <nuxt-link
          :href="item.link?.replace('{userId}', userStore.userId)"
          :title="item.title"
          target="_blank"
          v-if="item.type === 'ONLINE'"
        >
          {{ item.title }}
        </nuxt-link>

        <nuxt-link
          :href="`/partners/${item.id}`"
          :title="item.title"
          v-if="item.type === 'LOCAL'"
        >
          {{ item.title }}
        </nuxt-link>

        <nuxt-link
          :href="`/product-details/${item.id}`"
          :title="item.title"
          v-if="item.type === 'PRODUCT'"
        >
          {{ item.title }}
        </nuxt-link>
      </h3>
      <div
        class="tp-product-rating d-flex align-items-center justify-content-start"
      >
        <div class="tp-product-rating-icon" v-if="item.type === 'PRODUCT'">
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
        <div class="tp-product-rating-text" v-if="item.type === 'PRODUCT'">
          <span>({{ getRandomReviewsCount() ?? 0 }} avaliações)</span>
        </div>
      </div>
      <div class="tp-product-price-wrapper-2">
        <div v-if="item.discount > 0">
          <span class="tp-product-price-2 new-price">
            {{
              formatToBRL(
                Number(item.price) -
                  (Number(item.price) * Number(item.discount)) / 100
              )
            }}
          </span>
          <span class="tp-product-price-2 old-price"> ${{ item.price }} </span>
        </div>
        <div
          v-else
          class="tp-product-price-2 new-price"
          :class="item.type !== 'PRODUCT' ? 'd-block w-100 text-center' : ''"
        >

        <span>
          {{
            item.type === "PRODUCT"
              ? formatToBRL(item.price)
              : "Até " + formatPercentage(item.price) + " de cashback"
          }}
        </span>
        <span v-if='item.type === "PRODUCT"' class="d-block">e até {{ formatToPercentage(item.merchant.cashback) }} de cashback</span>      
         
      </div>
      </div>

      <div class="tp-product-list-add-to-cart mt-2">
        <nuxt-link
          :to="userStore.userId == null ? '/login' : item.link"
          :target="userStore.userId == null ? '' : '_blank'"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
          v-if="item.type === 'ONLINE'"
        >
          {{
            userStore.userId == null
              ? "Faça login para ativar o cashback"
              : getButtonText(item.type ?? "")
          }}
        </nuxt-link>

        <nuxt-link
          :href="`/partners/${item.id}`"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
          v-if="item.type === 'LOCAL'"
        >
          {{ getButtonText(item.type ?? "") }}
        </nuxt-link>

        <nuxt-link
          :href="`/product-details/${item.id}`"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
          v-if="item.type === 'PRODUCT'"
        >
          {{ getButtonText(item.type ?? "") }}
        </nuxt-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { type IProduct } from "@/types/product-type";
import { useUserStore } from "@/pinia/useUserStore";
import {useProductFilterStore} from "@/pinia/useProductFilterStore";

const userStore = useUserStore();
const productFilterStore = useProductFilterStore();

const props = withDefaults(
  defineProps<{ item: IProduct; spacing?: boolean }>(),
  {
    spacing: true,
  }
);

const getRandomReviewsCount = () => {
  return Math.floor(Math.random() * (999 - 100 + 1)) + 100;
};

const getButtonText = (type: string) => {
  switch (type) {
    case "ONLINE":
      return "Ativar meu cashback";
    case "LOCAL":
      return "Ver local";
    case "PRODUCT":
      return "Comprar";
    default:
      return "Ver mais";
  }
};

const getAverageRating = (item: IProduct) => {
  // if (!item.reviews || item.reviews.length === 0) {
  //   return 0; // Retorna 0 ou outro valor padrão se não houver avaliações
  // }

  // const totalRatings = item.reviews.reduce(
  //   (acc, review) => acc + review.rating,
  //   0
  // );
  // const averageRating = totalRatings / item.reviews.length;

  // // Garante que a média esteja entre 0 e 5
  // return Math.min(5, Math.max(0, averageRating));

  return Math.random() * (5 - 3.8) + 3.8;
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

onBeforeMount(() => {
  productFilterStore.handleResetFilter();
});
</script>

<style scoped>
.tp-product-tag-2 a {
  font-size: 0.875rem;
}

.tp-product-title-2 {
  width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  margin-bottom: 0;
}

.tp-product-code {
  font-size: 0.7rem;
}

.tp-product-rating {
  min-height: 28px;
}
</style>
