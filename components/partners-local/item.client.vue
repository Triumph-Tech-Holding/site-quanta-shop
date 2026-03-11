<template>
  <div
    :class="`${
      offer_style ? 'tp-product-offer-item' : 'mb-25'
    } tp-product-item transition-3`"
  >
    <div class="tp-product-thumb p-relative fix m-img d-flex align-items-center justify-content-center" style="height: 200px; background-color: #ffffff;">
      <nuxt-link
        :href="`https://agencia.quantashop.com.br/anunciante?idAnunciante=${item.IdCredenciamento}`"
        target="_blank"
      >
        <!-- <img
          src="/img/category/main/category-main.png"
          alt="product-electronic"
        /> -->
        <img
          :src="item.Imagem"
          alt="product-img"
          style="max-height: 140px;"
        />
      </nuxt-link>
      <!-- product badge -->
      <!-- <div class="tp-product-badge">
        <span v-if="item.status === 'out-of-stock'" class="product-hot">out-of-stock</span>
      </div> -->
    </div>
    <!-- product content -->
    <div class="tp-product-content text-center">
      <div class="tp-product-category">
        <nuxt-link
          :href="`https://agencia.quantashop.com.br/anunciante?idAnunciante=${item.IdCredenciamento}`"
          target="_blank"
          >{{ item.Categoria }}</nuxt-link
        >
      </div>
      <h3 class="tp-product-title" :title="item.Estabelecimento">
        <nuxt-link
          :href="`https://agencia.quantashop.com.br/anunciante?idAnunciante=${item.IdCredenciamento}`"
          target="_blank"
        >
          {{ truncateText(item.Estabelecimento) }}
        </nuxt-link>
      </h3>

      <div class="tp-product-price-wrapper">
        <span class="tp-product-price new-price"
          >Até {{ item.PercentualCashback }}% de cashback</span
        >
      </div>

      <div class="tp-product-list-add-to-cart mt-3 w-100" v-if="userStore.isLoggedIn && item.link != null">
        <nuxt-link
          :to="item.link"
          target="_blank"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
        >
          Ver mais
        </nuxt-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from "@/pinia/useUserStore";

const props = defineProps<{ item: any; offer_style?: boolean }>();
const userStore = useUserStore();
const isMobile = ref(false);

const checkScreenWidth = () => {
  isMobile.value = window.innerWidth <= 768;
};

onMounted(() => {
  checkScreenWidth();

  window.addEventListener('resize', checkScreenWidth);
});

const truncateText = (text: string) => {
  if (!text) return "";

  if (isMobile.value) return text;

  if (text.length > 28) {
    return text.substring(0, 20) + "...";
  }
  return text;
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