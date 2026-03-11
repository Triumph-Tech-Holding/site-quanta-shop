<template>
  <div class="tp-product-list-item d-md-flex">
    <div
      class="tp-product-list-thumb p-relative fix"
      style="height: 321px; border: 1px solid #ddd; background-color: #ffffff"
    >
      <nuxt-link
        :href="`/product-details/${item.id}`"
        style="height: 100%; background-color: #ffffff"
      >
        <img :src="item.img" alt="product-img" @error="onImageError" />
      </nuxt-link>
    </div>
    <div class="tp-product-list-content">
      <div class="tp-product-content-2 pt-15">
        <div class="tp-product-tag-2">
          <a href="#">{{ item.category.name }}</a>
        </div>
        <h3 class="tp-product-title-2">
          <nuxt-link :href="`/product-details/${item.id}`">
            {{ item.title }}
          </nuxt-link>
        </h3>
        <div class="tp-product-rating-icon tp-product-rating-icon-2">
          <span><i class="fa-solid fa-star"></i></span>
          <span><i class="fa-solid fa-star"></i></span>
          <span><i class="fa-solid fa-star"></i></span>
          <span><i class="fa-solid fa-star"></i></span>
          <span><i class="fa-solid fa-star"></i></span>
        </div>

        <div class="tp-product-price-wrapper-2">
          <div v-if="item.discount > 0">
            <span class="tp-product-price-2 new-price"
              >{{
                formatToBRL(
                  Number(item.price) -
                    (Number(item.price) * Number(item.discount)) / 100
                )
              }}
              {{ " " }}</span
            >
            <span class="tp-product-price-2 old-price">
              ${{ formatToBRL(item.price) }}
            </span>
          </div>
          <span v-else class="tp-product-price-2 new-price">{{
            formatToBRL(item.price)
          }}</span>
        </div>

        <p>{{ item.description.slice(0, 100) }}</p>
        
        <div class="tp-product-list-add-to-cart" v-if="userStore.userId != null && item.link != null">
          <nuxt-link
            :to="item.link"
            target="_blank"
            class="tp-product-list-add-to-cart-btn w-100 text-center"
          >
            Comprar agora
          </nuxt-link>
        </div>
        <div class="tp-product-list-add-to-cart" v-else>
          <nuxt-link
            to="/login"
            class="tp-product-list-add-to-cart-btn w-100 text-center"
          >
            Faça login para comprar
          </nuxt-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { type IProduct } from "@/types/product-type";
import { useUserStore } from "@/pinia/useUserStore";

defineProps<{ item: IProduct }>();

const userStore = useUserStore();

const onImageError = (event: { target: { src: string } }) => {
  event.target.src = "/img/category/main/category-main.png";
};
</script>
