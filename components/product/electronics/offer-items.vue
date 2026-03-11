<template>
  <section class="tp-product-offer grey-bg-2 pt-70 pb-80">
    <ui-loading-indicator />
    <div class="container">
      <div class="row align-items-end">
        <div class="col-xl-4 col-md-5 col-sm-6">
          <div class="tp-section-title-wrapper mb-40">
            <h3 class="tp-section-title">
              Ofertas do dia
              <SvgSectionLine />
            </h3>
          </div>
        </div>
        <div class="col-xl-8 col-md-7 col-sm-6">
          <div
            class="tp-product-offer-more-wrapper d-flex justify-content-sm-end p-relative z-index-1"
          >
            <div class="tp-product-offer-more mb-40 text-sm-end grey-bg-2">
              <nuxt-link href="/shop" class="tp-btn tp-btn-2 tp-btn-blue">
                Ver mais ofertas
                <SvgRightArrow />
              </nuxt-link>
              <span class="tp-product-offer-more-border"></span>
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-xl-12">
          <div class="tp-product-offer-slider fix">
            <Swiper
              :slidesPerView="4"
              :spaceBetween="30"
              :speed="1000"
              :loop="true"
              :cssMode="true"
              :autoplay="{
                delay: 5000,
                disableOnInteraction: false,
                pauseOnMouseEnter: true,
              }"
              :pagination="{
                el: '.tp-deals-slider-dot',
                clickable: true,
              }"
              :breakpoints="{
                '1200': {
                  slidesPerView: 3,
                },
                '992': {
                  slidesPerView: 2,
                },
                '768': {
                  slidesPerView: 2,
                },
                '576': {
                  slidesPerView: 1,
                },
                '0': {
                  slidesPerView: 1,
                },
              }"
              :modules="[Pagination, Autoplay]"
              class="tp-product-offer-slider-active swiper-container"
            >
              <SwiperSlide v-for="(item, i) in offer_products" :key="i">
                <ProductElectronicsItem :item="item" :offer_style="true" />
              </SwiperSlide>
              <div
                class="tp-deals-slider-dot tp-swiper-dot text-center mt-40"
              ></div>
            </Swiper>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { Swiper, SwiperSlide } from "swiper/vue";
import { Autoplay, Pagination } from "swiper/modules";
import { useProductStore } from "@/pinia/useProductStore";
import type { IProduct } from "@/types/product-type";
import { useProductFilterStore } from "@/pinia/useProductFilterStore";

const store = useProductFilterStore();
const productStore = useProductStore();
const product_data = computed(() => store.product_deal);
const offer_products = ref<IProduct[]>([]);

const updateOfferProducts = () => {
  offer_products.value = product_data.value
    //.filter((p) => p.productType === "SPORTSWEAR & SWIMWEAR")
    .slice(0, 8);
};

watch(
  product_data,
  (newProducts) => {
    updateOfferProducts();
  },
  { immediate: true }
);

onMounted(async () => {
  if (store.product_deal.length === 0) {
    await productStore.fetchProducts(
      store.totalItensPerPage,
      store.actualPage,
      store.priceValues[0],
      store.priceValues[1],
      store.selectVal,
      store.category
    );
  }
});
</script>
