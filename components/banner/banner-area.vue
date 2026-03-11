<template>
  <section v-if="banners.length > 0" class="tp-banner-area pb-70">
    <div class="container">
      <div class="row">
        <div class="col-xl-8 col-lg-7 mb-30">
          <div
            class="tp-banner-item tp-banner-height p-relative z-index-1 fix h-100 bg-body-secondary p-0"
          >
            <div
              class="tp-banner-thumb tp-banner-thumb-1 include-bg transition-3"
            ></div>
            <div class="tp-banner-content">
              <div class="row">
                <div class="col-12 col-md-8 p-5 d-flex flex-column justify-content-center">
                  <span>{{ banners[0]?.texto1 }}</span>
                  <h3 class="tp-banner-title">
                    {{ banners[0]?.texto2 }}
                  </h3>

                  <p>
                    {{ banners[0]?.texto3 }}
                  </p>

                  <div class="tp-banner-btn">
                    <nuxt-link :href="banners[0]?.link" class="tp-link-btn">
                      {{ banners[0]?.textoLink }}
                      <SvgRightArrow2 />
                    </nuxt-link>
                  </div>
                </div>
                <div class="col-12 col-md-4 d-flex align-items-center justify-content-center">
                  <img :src="banners[0]?.imagem" class="tp-banner-image" />
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="col-xl-4 col-lg-5 mb-30">
          <div
            class="tp-banner-item tp-banner-item-sm tp-banner-height p-relative z-index-1 fix bg-body-info p-0"
          >
            <div
              class="tp-banner-thumb tp-banner-thumb-2 include-bg transition-3"
              v-background-image="banners[1]?.imagem"
            ></div>
            <div class="tp-banner-content">
              <div class="row">
                <div class="col-12 col-md-7 p-5 d-flex flex-column justify-content-center">
                  <span>{{ banners[1]?.texto1 }}</span>
                  <h3 class="tp-banner-title">
                    {{ banners[1]?.texto2 }}
                  </h3>

                  <p>
                    {{ banners[1]?.texto3 }}
                  </p>

                  <div class="tp-banner-btn">
                    <nuxt-link :href="banners[1]?.link" class="tp-link-btn">
                      {{ banners[1]?.textoLink }}
                      <SvgRightArrow2 />
                    </nuxt-link>
                  </div>
                </div>
                <div class="col-12 col-md-5 d-flex align-items-center justify-content-center">
                  <img :src="banners[1]?.imagem" class="tp-banner-image" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useCarouselStore } from "@/pinia/useCarouselStore";

const carouselStore = useCarouselStore();
const banners = computed(() => transformData());

const transformData = (): any => {
  const carousels = carouselStore.carousels;
  return carousels.filter((carousel) => carousel.posicao === "2");
};
</script>

<style scoped>
.bg-body-secondary {
  --bs-bg-opacity: 1;
  background-color: rgb(241 241 241) !important;
}

.bg-body-info {
  --bs-bg-opacity: 1;
  background-color: #90daff !important;
}

.tp-banner-image {
  max-height: 300px;
}
</style>
