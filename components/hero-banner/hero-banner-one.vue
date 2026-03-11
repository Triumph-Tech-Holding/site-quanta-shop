<template>
  <section class="tp-slider-area p-relative z-index-1">
    <Swiper
      v-if="sliderData.length > 0"
      :slidesPerView="1"
      :spaceBetween="0"
      :loop="true"
      :autoplay="{
        delay: 6000,
        disableOnInteraction: false,
      }"
      :navigation="{
        nextEl: '.tp-slider-button-next',
        prevEl: '.tp-slider-button-prev',
      }"
      :pagination="{
        el: '.tp-slider-dot',
        clickable: true,
      }"
      :modules="[Navigation, Pagination, EffectFade, Autoplay]"
      @swiper="setInitialActiveIndex"
      @slideChange="handleActiveIndex"
      :initialSlide="0"
      :class="`tp-slider-active tp-slider-variation swiper-container ${
        isActive ? 'is-light' : ''
      }`"
    >
      <SwiperSlide
        v-for="(item, i) in sliderData"
        :key="i"
        :class="`tp-slider-item tp-slider-height d-flex align-items-center ${
          item.corFundo == 'green-dark-bg' ? 'green-dark-bg' : 'is-light'
        } ${i === 0 && isActive ? 'active-slide' : ''}`"
        :style="item.corFundo == 'is_light' && `background-color:#E3EDF6`"
      >
        <div class="tp-slider-shape">
          <img
            class="tp-slider-shape-1"
            src="/img/slider/shape/slider-shape-1.png"
            alt="slider-shape"
          />
          <img
            class="tp-slider-shape-2"
            src="/img/slider/shape/slider-shape-2.png"
            alt="slider-shape"
          />
          <img
            class="tp-slider-shape-3"
            src="/img/slider/shape/slider-shape-3.png"
            alt="slider-shape"
          />
          <img
            class="tp-slider-shape-4"
            src="/img/slider/shape/slider-shape-4.png"
            alt="slider-shape"
          />
        </div>
        <div class="container">
          <div class="row align-items-center" v-if="!item.somenteImagem">
            <div class="col-xl-5 col-lg-6 col-md-6">
              <div class="tp-slider-content p-relative z-index-1">
                <span>
                  {{ item.texto1 }}
                </span>
                <h3 class="tp-slider-title">{{ item.texto2 }}</h3>
                <p>
                  {{ item.texto3 }}
                </p>

                <div class="tp-slider-btn" v-if="item.link">
                  <nuxt-link
                    :href="item.link"
                    class="tp-btn tp-btn-2 tp-btn-white"
                  >
                    {{ item.textoLink }}
                    <SvgRightArrow />
                  </nuxt-link>
                </div>
              </div>
            </div>
            <div class="col-xl-7 col-lg-6 col-md-6">
              <div class="tp-slider-thumb text-end">
                <img :src="item.imagem" alt="slider-img" />
              </div>
            </div>
          </div>
          <div class="row align-items-center" v-if="item.somenteImagem">
            <div class="col-xl-12 col-lg-12 col-md-12">
              <div class="tp-slider-thumb text-end">
                <img :src="item.imagem" alt="slider-img" />
              </div>
            </div>
          </div>
        </div>
      </SwiperSlide>
      <div class="tp-slider-arrow tp-swiper-arrow d-none d-lg-block">
        <button type="button" class="tp-slider-button-prev">
          <SvgPrevArrow />
        </button>
        <button type="button" class="tp-slider-button-next">
          <SvgNextArrow />
        </button>
      </div>
      <div class="tp-slider-dot tp-swiper-dot"></div>
    </Swiper>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, nextTick, onMounted, watch } from 'vue';
import { useCarouselStore } from "@/pinia/useCarouselStore";
import { Swiper, SwiperSlide } from "swiper/vue";
import { Navigation, Pagination, EffectFade, Autoplay } from "swiper/modules";

const carouselStore = useCarouselStore();
let isActive = ref<boolean>(true);
let swiperInstance = ref(null);

const transformData = (): any => {
  const carousels = carouselStore.carousels;
  const filteredCarousels = carousels
    .filter((carousel) => carousel.posicao === "1" && carousel.ativo === true)
    .sort((a, b) => a.ordemExibicao - b.ordemExibicao);

  return filteredCarousels.length > 0 ? filteredCarousels : [];
};

const sliderData = computed(() => {
  const data = transformData();
  console.log('sliderData computed:', data);
  return data;
});

const handleActiveIndex = () => {
  if (swiperInstance.value && typeof swiperInstance.value.activeIndex === 'number') {
    isActive.value = swiperInstance.value.activeIndex === 0;
  } else {
    console.log('Swiper instance or activeIndex not available');
  }
};

const setInitialActiveIndex = (swiper) => {
  console.log('setInitialActiveIndex called');
  swiperInstance.value = swiper;
  nextTick(() => {
    handleActiveIndex();
  });
};

watch(sliderData, () => {
  console.log('sliderData changed');
  nextTick(() => {
    if (swiperInstance.value) {
      swiperInstance.value.update();
      handleActiveIndex();
    }
  });
}, { deep: true });

onMounted(() => {
  console.log('Component mounted');
  nextTick(() => {
    handleActiveIndex();
  });
});

</script>

<style scoped>
.tp-slider-thumb img {
  max-height: 500px !important;
}

.swiper-wrapper {
  transition-duration: 200ms !important;
}

.active-slide {
  /* Add any styles you want for the active slide */
  opacity: 1;
  z-index: 1;
}
</style>
