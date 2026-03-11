<template>
  <section class="tp-product-sm-area">
    <div class="container">
      <div class="row">
        <div class="col-xl-4 col-md-6">
          <div class="tp-product-sm-list mb-50">
            <div class="tp-section-title-wrapper mb-40">
              <h3 class="tp-section-title tp-section-title-sm">
                Melhores ofertas
                <SvgSectionLineSm />
              </h3>
            </div>

            <div class="tp-product-sm-wrapper mr-20">
              <partners-local-sm-item
                v-for="(item, i) in bestDiscountsLocalPartners"
                :key="i"
                :item="item"
              />
            </div>
          </div>
        </div>
        <div class="col-xl-4 col-md-6">
          <div class="tp-product-sm-list mb-50">
            <div class="tp-section-title-wrapper mb-40">
              <h3 class="tp-section-title tp-section-title-sm">
                Parceiros em destaque
                <SvgSectionLineSm />
              </h3>
            </div>

            <div class="tp-product-sm-wrapper mr-20">
              <partners-local-sm-item
                v-for="(item, i) in featuredLocalPartners"
                :key="i"
                :item="item"
              />
            </div>
          </div>
        </div>
        <div class="col-xl-4 col-md-6">
          <div class="tp-product-sm-list mb-50">
            <div class="tp-section-title-wrapper mb-40">
              <h3 class="tp-section-title tp-section-title-sm">
                Mais vendidos
                <SvgSectionLineSm />
              </h3>
            </div>

            <div class="tp-product-sm-wrapper mr-20">
              <partners-local-sm-item
                v-for="(item, i) in topSellersLocalPartners"
                :key="i"
                :item="item"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { usePartnerStore } from "@/pinia/usePartnerStore";

const partnerStore = usePartnerStore();

const bestDiscountsLocalPartners = computed(
  () => partnerStore.bestDiscountsLocalPartners
);

const featuredLocalPartners = computed(
  () => partnerStore.featuredLocalPartners
);

const topSellersLocalPartners = computed(
  () => partnerStore.topSellersLocalPartners.slice(0, 3)
);
</script>
