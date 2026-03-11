<template>
  <div class="tp-product-details-wrapper has-sticky">
    <div class="tp-product-details-category">
      <span>{{ product.parent }}</span>
    </div>
    <h3 class="tp-product-details-title">{{ product.title }}</h3>

    <!-- inventory details -->
    <div class="tp-product-details-inventory d-flex align-items-center mb-10">
      <div class="tp-product-details-stock mb-10">
        <span>{{ product.brand.name }}</span>
      </div>
      <div
        class="tp-product-details-rating-wrapper d-flex align-items-center mb-10"
      >
        <div class="tp-product-details-rating">
          <span v-for="i in Math.floor(productReview.rating)" :key="i">
            <i class="fa-solid fa-star"></i>
          </span>
          <span v-if="productReview.rating % 1 !== 0">
            <i class="fa-solid fa-star-half"></i>
          </span>
          <span v-for="i in 5 - Math.ceil(productReview.rating)" :key="i + 5">
            <i class="fa-regular fa-star"></i>
          </span>
        </div>
        <div class="tp-product-details-reviews">
          <span>({{ productReview.reviews?.length ?? 0 }} avaliações)</span>
        </div>
      </div>
    </div>
    <p>
      {{
        textMore
          ? product.description
          : `${product.description.substring(0, 100)}...`
      }}
      <span @click="textMore = !textMore" class="cursor-pointer">{{
        textMore ? "Ver menos" : "Ver mais"
      }}</span>
    </p>

    <!-- price -->
    <div class="tp-product-details-price-wrapper mb-20">
      <div v-if="product.discount > 0">
        <span class="tp-product-details-price old-price">{{
          formatToBRL(product.price)
        }}</span>
        <span class="tp-product-details-price new-price">
          {{
            formatToBRL(
              Number(product.price) -
                (Number(product.price) * Number(product.discount)) / 100
            )
          }}
        </span>
      </div>
      <span v-else class="tp-product-details-price">{{
        formatToBRL(product.price)
      }}</span>
    </div>

    <!-- variations -->
    <div v-if="hasColorData" class="tp-product-details-variation">
      <div class="tp-product-details-variation-item">
        <h4 class="tp-product-details-variation-title">Color :</h4>
        <div class="tp-product-details-variation-list">
          <button
            v-for="(item, i) in product.imageURLs"
            :key="i"
            @click="productStore.handleImageActive(item.img)"
            type="button"
            :class="[
              'color',
              'tp-color-variation-btn',
              item.img === productStore.activeImg ? 'active' : '',
            ]"
            style="margin-right: 5px"
          >
            <span
              :data-bg-color="item.color?.clrCode"
              :style="`background-color:${item.color?.clrCode}`"
            ></span>
            <span
              v-if="item.color && item.color.name"
              class="tp-color-variation-tootltip"
            >
              {{ item.color.name }}
            </span>
          </button>
        </div>
      </div>
    </div>

    <!-- product countdown start -->
    <div v-if="product.offerDate?.endDate">
      <product-details-countdown :product="product" />
    </div>
    <!-- product countdown end -->

    <!-- actions -->
    <div class="tp-product-details-action-wrapper" v-if="product.link != null">
      <nuxt-link
        :href="product.link"
        class="tp-product-details-buy-now-btn w-100 text-center"
        >Comprar agora</nuxt-link
      >
    </div>
    <div class="tp-product-details-action-wrapper" v-else>
      <nuxt-link
        :to="`/login?redirect=product-details/${product.id}`"
        class="tp-product-details-buy-now-btn w-100 text-center"
        >Faça login para comprar</nuxt-link
      >
    </div>

    <div v-if="isShowBottom">
      <div class="tp-product-details-query">
        <div class="tp-product-details-query-item d-flex align-items-center">
          <span>SKU: </span>
          <p>{{ product.sku }}</p>
        </div>
        <div class="tp-product-details-query-item d-flex align-items-center">
          <span>Categoria: </span>
          <p>{{ product.parent }}</p>
        </div>
      </div>
      <div class="tp-product-details-social">
        <span>Compartilhar: </span>
        <a target="_blank" href="https://www.facebook.com/quantabank"
          ><i class="fa-brands fa-facebook-f"></i
        ></a>
        <a target="_blank" href="https://www.instagram.com/quanta_bank"
          ><i class="fa-brands fa-instagram"></i
        ></a>
        <a target="_blank" href="https://wa.me/message/QFVP2ILERA4LD1"
          ><i class="fa-brands fa-whatsapp"></i
        ></a>
      </div>
      <div
        class="tp-product-details-payment d-flex align-items-center flex-wrap justify-content-between"
      >
        <p>
          Segurança garantida <br />
          & check-out seguro
        </p>
        <img src="/img/product/icons/payment-option.png" alt="" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useProductStore } from "@/pinia/useProductStore";
import { useUserStore } from "@/pinia/useUserStore";
import { useLoadingStore } from "@/pinia/useLoadingStore";
import { type IProduct } from "@/types/product-type";

// store
const productStore = useProductStore();

// props
const props = withDefaults(
  defineProps<{ product: IProduct; isShowBottom?: boolean }>(),
  {
    isShowBottom: true,
  }
);

interface ProductReview {
  rating: number;
  reviews: any[]; // ou use um tipo específico se você tiver
}

const productReview = ref<ProductReview>({
  rating: 0,
  reviews: [],
});
let textMore = ref<boolean>(false);

const hasColorData = computed(() =>
  props.product.imageURLs.some((item) => item?.color && item?.color?.name)
);

const userStore = useUserStore();

const generateRandomRating = () => {
  return (Math.random() * (5 - 3.8) + 3.8).toFixed(1);
};

const generateRandomReviewsCount = () => {
  return Math.floor(Math.random() * 100); // Gera um número entre 0 e 99
};

onMounted(async () => {
  await userStore.loadUserFromStorage();

  productReview.value.rating = parseFloat(generateRandomRating());
  productReview.value.reviews = new Array(generateRandomReviewsCount()).fill(
    null
  );
});
</script>

<style scoped>
.tp-product-details-stock {
  background-color: #1e5d68;
}

.tp-product-details-stock span {
  color: white;
}
</style>
