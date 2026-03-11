<template>
  <div class="tp-product-banner-area pb-90">
    <div class="container">
      <div class="tp-product-banner-slider fix">
        <Swiper
          :slidesPerView="1"
          :spaceBetween="0"
          :effect="'fade'"
          :speed="1000"
          :loop="true"
          :autoplay="{
            delay: 5000,
            disableOnInteraction: false,
            pauseOnMouseEnter: true,
          }"
          :pagination="{
            el: '.tp-product-banner-slider-dot',
            clickable: true,
          }"
          :modules="[EffectFade, Pagination, Autoplay]"
          class="tp-product-banner-slider-active swiper-container"
        >
          <SwiperSlide
            v-for="(item, i) in bannerProducts"
            :key="i"
            class="tp-product-banner-inner theme-bg p-relative z-index-1 fix"
          >
            <h4 class="tp-product-banner-bg-text">{{ item.texto2 }}</h4>
            <div class="row align-items-center">
              <div class="col-xl-6 col-lg-6">
                <div class="tp-product-banner-content p-relative z-index-1">
                  <span class="tp-product-banner-subtitle">
                    {{ item.texto1 }}
                  </span>
                  <h3 class="tp-product-banner-title">
                    {{ item.texto2 }}
                  </h3>

                  <p>
                    {{ item.texto3 }}
                  </p>

                  <div class="tp-product-banner-btn">
                    <nuxt-link :href="item.link" class="tp-btn tp-btn-2">{{
                      item.textoLink
                    }}</nuxt-link>
                  </div>
                </div>
              </div>
              <div class="col-xl-6 col-lg-6">
                <div class="tp-product-banner-thumb-wrapper p-relative">
                  <div
                    class="tp-product-banner-thumb text-end p-relative z-index-1"
                  >
                    <img :src="item.imagem" alt="slider-img" />
                  </div>
                </div>
              </div>
            </div>
          </SwiperSlide>
          <div class="tp-product-banner-slider-dot tp-swiper-dot"></div>
        </Swiper>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Swiper, SwiperSlide } from "swiper/vue";
import { EffectFade, Pagination, Autoplay } from "swiper/modules";
import { useCarouselStore } from "@/pinia/useCarouselStore";

const carouselStore = useCarouselStore();
const bannerProducts = computed(() => transformData());

const transformData = (): any => {
  const carousels = carouselStore.carousels;
  const filteredCarousels = carousels.filter(
    (carousel) => carousel.posicao === "4"
  );

  // Ordena o array filtrado por idCarrossel
  const sortedCarousels = filteredCarousels.sort((a, b) => {
    return b.idCarrossel - a.idCarrossel; // Ordem decrescente
  });

  return sortedCarousels.length > 0 ? sortedCarousels : [];
};
</script>

<style scoped>
.tp-product-banner-thumb img {
  max-height: 367px;
}

.tp-product-banner-content p {
  color: var(--tp-common-white);
  font-family: var(--tp-ff-oregano);
  font-weight: 400;
  font-size: 28px;
  margin-bottom: 40px;
  animation-delay: 0.7s;
  animation-duration: 1s;
}

.tp-slider-item.is-light .tp-product-banner-content p {
  color: var(--tp-heading-secondary);
}
</style>
