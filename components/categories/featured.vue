<template>
  <section class="tp-product-arrival-area pt-55">
    <div class="container">
      <div class="row align-items-end">
        <div class="col-xl-5 col-sm-6">
          <div class="tp-section-title-wrapper mb-40">
            <h3 class="tp-section-title">
              Categorias em destaque
              <SvgSectionLine />
            </h3>
          </div>
        </div>
        <div class="col-xl-7 col-sm-6">
          <div
            class="tp-product-arrival-more-wrapper d-flex justify-content-end"
          >
            <div
              class="tp-product-arrival-arrow tp-swiper-arrow mb-40 text-end tp-product-arrival-border"
            >
              <button type="button" class="tp-arrival-slider-button-prev me-2">
                <SvgPrevArrow />
              </button>
              <button type="button" class="tp-arrival-slider-button-next">
                <SvgNextArrow />
              </button>
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-xl-12">
          <div class="tp-product-arrival-slider fix">
            <Swiper
              :slidesPerView="5"
              :spaceBetween="10"
              :speed="2000"
              :loop="true"
              :autoplay="{
                delay: 2000,
                disableOnInteraction: false,
                pauseOnMouseEnter: true,
              }"
              :navigation="{
                prevEl: '.tp-arrival-slider-button-prev',
                nextEl: '.tp-arrival-slider-button-next',
              }"
              :breakpoints="{
                '1200': {
                  slidesPerView: 4,
                },
                '992': {
                  slidesPerView: 3,
                },
                '768': {
                  slidesPerView: 2,
                },
                '576': {
                  slidesPerView: 2,
                },
                '0': {
                  slidesPerView: 1,
                },
              }"
              :modules="[Navigation, Pagination, EffectFade, Autoplay]"
              class="tp-product-arrival-active swiper-container"
            >
              <SwiperSlide v-for="(item, i) in featuredCategories" :key="i">
                <section class="tp-product-category pt-60 pb-15">
                  <div class="container">
                    <div class="tp-product-category-item text-center mb-40">
                      <div class="tp-product-category-thumb fix">
                        <a
                          class="cursor-pointer"
                          @click="handleParentCategory(item.Nome)"
                        >
                          <img
                            :src="
                              item.Icone ||
                              '/img/category/main/category-main.png'
                            "
                            alt="featured-category"
                          />
                        </a>
                      </div>
                      <div class="tp-product-category-content">
                        <h3 class="tp-product-category-title">
                          <a
                            class="cursor-pointer"
                            @click="handleParentCategory(item.Nome)"
                          >
                            {{ item.Nome }}
                          </a>
                        </h3>
                        <p>
                          {{ item.Total }} parceiro{{
                            item.Total !== 1 ? "s" : ""
                          }}
                        </p>
                      </div>
                    </div>
                  </div>
                </section>
              </SwiperSlide>
            </Swiper>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { useCategoryStore } from "@/pinia/useCategoryStore";
import { Swiper, SwiperSlide } from "swiper/vue";
import { Navigation, Pagination, EffectFade, Autoplay } from "swiper/modules";

const router = useRouter();
const categoryStore = useCategoryStore();
const featuredCategories = computed(() => categoryStore.featuredCategories);

// handle parent
const handleParentCategory = (value: string) => {
  const newCategory = value.toLowerCase().replace("&", "").split(" ").join("-");
  router.push(`/partners?category=${newCategory}`);
};
</script>

<style lang="scss" scoped>
.tp-product-category-thumb {
  a {
    img {
      max-width: 150px;
    }
  }
}
</style>
