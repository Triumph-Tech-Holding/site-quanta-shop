<template>
  <div class="mb-25 tp-product-item transition-3 text-center">
    <div class="tp-product-thumb p-relative fix m-img">
      <nuxt-link :href="item.link" v-if="isAuthenticated" target="_blank">
        <img :src="item.Icone" alt="product-electronic" loading="lazy" />
      </nuxt-link>

      <nuxt-link href="/login?redirect=/" v-else>
        <img :src="item.Icone" alt="product-electronic" loading="lazy" />
      </nuxt-link>

      <div class="qs-cashback-badge qs-card-badge">
        <i class="fa-solid fa-tag" style="font-size: 9px;"></i>
        Até {{ formatToPercentage(item.MaxCashback, 0) }}
      </div>
    </div>
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

      <div class="tp-product-price-wrapper">
        <span class="tp-product-price new-price">
          Cashback de até {{ formatToPercentage(item.MaxCashback, 0) }}
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
          >Ativar cashback</nuxt-link
        >
      </div>
      <div class="tp-product-list-add-to-cart mt-3 w-100" v-else>
        <nuxt-link
          to="/login?redirect=/"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
          style="font-size: 0.8rem"
          >Entrar para ativar cashback</nuxt-link
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

onMounted(() => {});
</script>

<style lang="scss" scoped>
.tp-product-thumb {
  width: 100%;
  height: 180px;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  padding: 16px;

  img {
    max-height: 120px;
    object-fit: contain;
    transition: transform 0.25s ease;
  }
}

.tp-product-item:hover .tp-product-thumb img {
  transform: scale(1.06);
}

.qs-card-badge {
  position: absolute;
  top: 10px;
  right: 10px;
  left: auto;
  font-size: 10px !important;
  padding: 3px 8px !important;
  z-index: 2;
}

.tp-product-title {
  max-height: 44px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
