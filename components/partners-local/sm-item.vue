<template>
  <div class="tp-product-sm-item d-flex align-items-center">
    <div class="tp-product-thumb mr-25 fix d-flex align-items-center justify-content-center" style="width: 140px; height: 140px; background-color: #fff;">
      <nuxt-link
        :href="`https://quantashop.com.br/anunciante?idAnunciante=${item.IdCredenciamento}`"
        target="_blank"
      >
        <img
          :src="item.Imagem"
          alt="product-img"
          width="140"
          height="140"
          style="max-width: 140px;"
        />
      </nuxt-link>
    </div>
    <div class="tp-product-sm-content w-100 text-center">
      <div class="tp-product-category text-lowercase mb-0">
        <nuxt-link
          :href="`https://agencia.quantashop.com.br/anunciante?idAnunciante=${item.IdCredenciamento}`"
          target="_blank"
        >
          {{ item.Categoria }}
        </nuxt-link>
      </div>

      <h3 class="tp-product-title my-2">
        <nuxt-link
          :href="`https://agencia.quantashop.com.br/anunciante?idAnunciante=${item.IdCredenciamento}`"
          target="_blank"
          >{{ item.Estabelecimento }}</nuxt-link
        >
      </h3>

      <div class="tp-product-price-wrapper">
        <span class="tp-product-price"
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

const props = defineProps<{ item: any }>();
const userStore = useUserStore();
const isMobile = ref(false);

const checkScreenWidth = () => {
  isMobile.value = window.innerWidth <= 768;
};

onMounted(() => {
  checkScreenWidth();

  window.addEventListener("resize", checkScreenWidth);
});

const truncateText = (text: string) => {
  if (!text) return "";

  if (isMobile.value) return text;

  if (text.length > 28) {
    return text.substring(0, 24) + "...";
  }
  return text;
};
</script>

<style scoped>
.tp-product-sm-content {
  width: 100%;
  max-width: 170px;
}

.tp-product-title {
  font-size: 1rem !important;
  width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
