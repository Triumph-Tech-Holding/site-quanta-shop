<template>
  <div :class="`tp-product-item-3 ${primary_style?'tp-product-style-primary':''} mb-50 ${isCenter?'text-center':''}`">
    <div
      class="tp-product-thumb-3 mb-15 fix p-relative z-index-1"
      :style="`background-color: ${style_2 ? '#f6f6f6' : '#fff'};`"
    >
      <nuxt-link :href="`/product-details/${item.id}`">
        <img :src="item.img" alt="product-img" />
      </nuxt-link>
    </div>
    <div class="tp-product-content-3">
      <div class="tp-product-tag-3">
        <span>{{item.category.name}}</span>
      </div>
      <h3 class="tp-product-title-3">
        <nuxt-link :href="`/product-details/${item.id}`">{{ item.title }}</nuxt-link>
      </h3>
      <div class="tp-product-price-wrapper-3">
        <span v-if="item.discount > 0" class="tp-product-price-3">
          {{formatToBRL(Number(item.price) - (Number(item.price) * Number(item.discount)) / 100)}}
        </span>
        <span v-else class="tp-product-price-3">{{ formatToBRL(item.price) }}</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useCartStore } from "@/pinia/useCartStore";
import { useWishlistStore } from "@/pinia/useWishlistStore";
import { useUtilityStore } from "@/pinia/useUtilityStore";
import { type IProduct } from "@/types/product-type";
// props
defineProps<{ item: IProduct; style_2?: boolean;isCenter?: boolean;primary_style?:boolean }>();


const cartStore = useCartStore();
const wishlistStore = useWishlistStore();
const utilityStore = useUtilityStore();

function isItemInWishlist(product: IProduct) {
  return wishlistStore.wishlists.some((prd) => prd.id === product.id);
}

function isItemInCart(product: IProduct) {
  return cartStore.cart_products.some((prd) => prd.id === product.id);
}
</script>
