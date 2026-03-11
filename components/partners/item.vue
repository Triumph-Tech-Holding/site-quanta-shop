<template>
  <div class="mb-25 tp-product-item transition-3 text-center">
    <div class="tp-product-thumb p-relative fix m-img">
      <nuxt-link :href="item.link" v-if="isAuthenticated" target="_blank">
        <img :src="item.Icone" alt="product-electronic" />
      </nuxt-link>

      <nuxt-link href="/login?redirect=/" v-else>
        <img :src="item.Icone" alt="product-electronic" />
      </nuxt-link>
    </div>
    <!-- product content -->
    <div class="tp-product-content">
      <div class="tp-product-category">
        <nuxt-link :href="item.link" v-if="isAuthenticated" target="_blank">
          {{ item.Categoria }}
        </nuxt-link>

        <nuxt-link href="/login?redirect=/" v-else>
          {{ item.Categoria }}
        </nuxt-link>
      </div>
      <h3 class="tp-product-title mb-0" :title="item.Nome">
        <nuxt-link :href="item.link" v-if="isAuthenticated" target="_blank">
          {{ item.Nome }}
        </nuxt-link>

        <nuxt-link href="/login?redirect=/" v-else>
          {{ item.Nome }}
        </nuxt-link>
      </h3>
      <!-- <div class="tp-product-rating d-flex align-items-center">
        <div class="tp-product-rating-icon">
          <span><i class="fa-solid fa-star"></i></span>
          <span><i class="fa-solid fa-star"></i></span>
          <span><i class="fa-solid fa-star"></i></span>
          <span><i class="fa-solid fa-star"></i></span>
          <span><i class="fa-solid fa-star-half-stroke"></i></span>
        </div>
        <div class="tp-product-rating-text">
          <span>({{item.reviews?.length}} Review)</span>
        </div>
      </div> -->
      <div class="tp-product-price-wrapper">
        <span class="tp-product-price new-price">
          Até {{ formatToPercentage(item.MaxCashback, 0) }} de cashback
        </span>
      </div>

      <div
        class="tp-product-list-add-to-cart mt-3 w-100"
        v-if="isAuthenticated"
      >
        <nuxt-link
          :to="item.link"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
          target="_blank"
          >Ativar meu cashback</nuxt-link
        >
      </div>
      <div class="tp-product-list-add-to-cart mt-3 w-100" v-else>
        <nuxt-link
          to="/login?redirect=/"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
          style="font-size: 0.8rem"
          >Faça login para ativar seu cashback</nuxt-link
        >
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from "@/pinia/useUserStore";

const props = defineProps<{ item: any }>();
const userStore = useUserStore();
const isAuthenticated = computed(() => userStore.isLoggedIn);

onMounted(() => {
  
});
</script>

<style lang="scss" scoped>
.tp-product-thumb {
  width: 100%;
  height: 200px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.tp-product-title {
  max-height: 60px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
