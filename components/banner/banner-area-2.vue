<template>
  <section class="tp-product-gadget-area pt-80 pb-75">
    <div class="container">
      <div class="row">
        <div class="col-xl-4 col-lg-5">
          <div class="tp-product-gadget-sidebar mb-40">
            <div class="tp-product-gadget-categories p-relative fix mb-10">
              <div class="tp-product-gadget-thumb">
                <img src="/img/product/gadget/gadget-girl.png" alt="" />
              </div>
              <h3 class="tp-product-gadget-categories-title">
                Eletrônicos <br />
                Gadgets
              </h3>

              <div class="tp-product-gadget-categories-list">
                <ul>
                  <li><nuxt-link href="/shop">Micrscope</nuxt-link></li>
                  <li><nuxt-link href="/shop">Remote Control</nuxt-link></li>
                  <li><nuxt-link href="/shop">Monitor</nuxt-link></li>
                  <li><nuxt-link href="/shop">Thermometer</nuxt-link></li>
                  <li><nuxt-link href="/shop">Backpack</nuxt-link></li>
                  <li><nuxt-link href="/shop">Headphones</nuxt-link></li>
                </ul>
              </div>

              <div class="tp-product-gadget-btn">
                <nuxt-link href="/shop" class="tp-link-btn">
                  More Products
                  <SvgRightArrow2 />
                </nuxt-link>
              </div>
            </div>
            <div class="tp-product-gadget-banner">
              <Swiper :slidesPerView="1" :spaceBetween="0" :loop="true" :effect="'fade'" :pagination="{
                el: '.tp-product-gadget-banner-slider-dot',
                clickable: true,
              }" :autoplay="{
                delay: 5000,
                disableOnInteraction: false,
                pauseOnMouseEnter: true,
              }" :modules="[EffectFade, Pagination, Autoplay]"
                class="tp-product-gadget-banner-slider-active swiper-container  rounded">
                <SwiperSlide v-for="(item, i) in bannerProducts" :key="i" class="tp-product-gadget-banner-item banner-area-bg">
                  <div class="container">
                    <nuxt-link :href="item.link">
                      <div class="row align-items-center">
                        <div class="col-xl-6 col-lg-6 col-md-6">
                          <div class="tp-slider-content p-relative z-index-1 ml-20">
                            <span class="tp-product-gadget-banner-price">
                              {{ item.texto1 }}
                            </span>

                            <h3 class="tp-slider-title  tp-product-gadget-banner-title">{{ item.texto2 }}</h3>
                          </div>
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6">
                          <div class="tp-slider-thumb text-end mr-25">
                            <img :src="item.imagem" alt="slider-img" />
                          </div>
                        </div>
                      </div>
                    </nuxt-link>
                  </div>
                </SwiperSlide>
                <div class="tp-product-gadget-banner-slider-dot tp-swiper-dot"></div>
              </Swiper>
            </div>
          </div>
        </div>
        <div class="col-xl-8 col-lg-7">
          <div class="tp-product-gadget-wrapper">
            <div class="row">
              <div v-for="item in product_data.slice(0, 6)" :key="item.id" class="col-xl-4 col-sm-6">
                <ProductElectronicsItem :item="item" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { Swiper, SwiperSlide } from "swiper/vue";
import { Autoplay, EffectFade, Pagination } from "swiper/modules";
import { useCarouselStore } from "@/pinia/useCarouselStore";
import { useProductStore } from "@/pinia/useProductStore";

const carouselStore = useCarouselStore();
const productStore = useProductStore();
const product_data = computed(() => productStore.products);
const bannerProducts = computed(() => transformData());

const transformData = (): any => {
  const carousels = carouselStore.carousels;
  const filteredCarousels = carousels.filter(carousel => carousel.posicao === "3");

  if (filteredCarousels && filteredCarousels.length > 0) {
    return filteredCarousels;
  } else {
    return [];
  }
};

onMounted(() => {
});

</script>

<style scoped>
.banner-area-bg {
  background: rgb(61, 72, 150);
  background: linear-gradient(90deg, rgba(61, 72, 150, 1) 0%, rgba(211, 56, 149, 1) 100%);
}

.tp-slider-thumb img {
  max-height: 160px;
}
</style>