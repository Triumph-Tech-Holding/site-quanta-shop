<template>
  <section class="tp-product-area mt-70 pb-55">
    <div class="container">
      <div class="row align-items-end">
        <div class="col-xl-5 col-lg-6 col-md-5">
          <div class="tp-section-title-wrapper mb-40">
            <h3 class="tp-section-title">
              Parceiros online
              <SvgSectionLine />
            </h3>
          </div>
        </div>
        <div class="col-xl-7 col-lg-6 col-md-7">
          <div
            class="tp-product-tab tp-product-tab-border mb-45 tp-tab d-flex justify-content-md-end"
          >
            <ul class="nav nav-tabs justify-content-sm-end" id="productTab">
              <li v-for="(tab, i) in tabs" :key="i" class="nav-item">
                <button
                  @click="handleActiveTab(tab)"
                  :class="`nav-link ${active_tab === tab ? 'active' : ''}`"
                >
                  {{ tab }}
                  <span class="tp-product-tab-line">
                    <SvgActiveLine />
                  </span>
                </button>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-xl-12">
          <div class="tp-product-tab-content">
            <div class="row">
              <div
                v-for="(item, i) in filteredProducts"
                :key="i"
                class="col-xl-3 col-lg-3 col-sm-6"
              >
                <PartnersItem :item="item" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { usePartnerStore } from "@/pinia/usePartnerStore";
import { useUserStore } from "@/pinia/useUserStore";

const partnerStore = usePartnerStore();
const userStore = useUserStore();
const newPartners = computed(() => partnerStore.newPartners);
const featuredPartners = computed(() => partnerStore.featuredPartners);
const topSellersPartners = computed(() => partnerStore.topSellersPartners);

let active_tab = ref("Destaque");

const tabs = ["Destaque", "Mais vendidos", "Novos"];
// handleActiveTab
const handleActiveTab = (tab: string) => {
  active_tab.value = tab;
};

const filteredProducts = computed(() => {
  if (active_tab.value === "Novos") {
    newPartners.value.forEach((partner) => {
      if (partner.link && userStore.isLoggedIn) {
        partner.link = partner.link.replace("{userId}", userStore.userId);
      }
    });

    return newPartners.value;
  } else if (active_tab.value === "Destaque") {
    featuredPartners.value.forEach((partner) => {
      if (partner.link && userStore.isLoggedIn) {
        partner.link = partner.link.replace("{userId}", userStore.userId);
      }
    });

    return featuredPartners.value;
  } else if (active_tab.value === "Mais vendidos") {
    topSellersPartners.value.forEach((partner) => {
      if (partner.link && userStore.isLoggedIn) {
        partner.link = partner.link.replace("{userId}", userStore.userId);
      }
    });

    return topSellersPartners.value;
  } else {
    return [];
  }
});
</script>
