<template>
  <div
    :class="`${
      offer_style ? 'tp-product-offer-item' : 'mb-25'
    } tp-product-item transition-3`"
  >
    <div class="qs-partner-thumb">
      <nuxt-link
        v-if="partnerLink"
        :href="partnerLink"
        target="_blank"
      >
        <img
          :src="item.Imagem"
          alt="product-img"
          loading="lazy"
        />
      </nuxt-link>
      <img
        v-else
        :src="item.Imagem"
        alt="product-img"
        loading="lazy"
      />

      <div class="qs-cashback-badge qs-card-badge">
        <i class="fa-solid fa-tag" style="font-size: 9px;"></i>
        Até {{ item.PercentualCashback }}% de cashback
      </div>
    </div>

    <div class="tp-product-content text-center">
      <div class="tp-product-category">
        <nuxt-link
          v-if="partnerLink"
          :href="partnerLink"
          target="_blank"
          >{{ item.Categoria }}</nuxt-link
        >
        <span v-else>{{ item.Categoria }}</span>
      </div>
      <h3 class="tp-product-title" :title="item.Estabelecimento">
        <nuxt-link
          v-if="partnerLink"
          :href="partnerLink"
          target="_blank"
        >
          {{ truncateText(item.Estabelecimento) }}
        </nuxt-link>
        <span v-else>{{ truncateText(item.Estabelecimento) }}</span>
      </h3>

      <div class="tp-product-price-wrapper">
        <span class="tp-product-price new-price"
          >Cashback de até {{ item.PercentualCashback }}%</span
        >
      </div>

      <div class="tp-product-list-add-to-cart mt-3 w-100" v-if="partnerLink">
        <nuxt-link
          :to="partnerLink"
          target="_blank"
          class="tp-product-list-add-to-cart-btn w-100 text-center"
        >
          Ver oferta
        </nuxt-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{ item: any; offer_style?: boolean }>();
const isMobile = ref(false);

const partnerLink = computed(() => {
  return props.item.link || null;
});

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
    return text.substring(0, 22) + "...";
  }
  return text;
};
</script>

<style scoped>
.qs-partner-thumb {
  width: 100%;
  height: 180px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #fff;
  border-bottom: 1px solid rgba(202, 212, 219, 0.4);
  border-top-left-radius: 12px;
  border-top-right-radius: 12px;
  overflow: hidden;
  position: relative;
  padding: 16px;
}

.qs-partner-thumb img {
  max-height: 120px;
  max-width: 100%;
  object-fit: contain;
  transition: transform 0.25s ease;
}

.tp-product-item:hover .qs-partner-thumb img {
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
  width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  margin-bottom: 0;
}
</style>
