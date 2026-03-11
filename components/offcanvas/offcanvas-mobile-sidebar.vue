<template>
  <div
    :class="`offcanvas__area offcanvas__radius ${
      utilsStore.openMobileMenus ? 'offcanvas-opened' : ''
    }`"
  >
    <div class="offcanvas__wrapper">
      <div class="offcanvas__close">
        <button
          @click="utilsStore.handleOpenMobileMenu()"
          class="offcanvas__close-btn offcanvas-close-btn"
        >
          <svg-close-2 />
        </button>
      </div>
      <div class="offcanvas__content">
        <div
          class="offcanvas__top mb-70 d-flex justify-content-between align-items-center"
        >
          <div class="offcanvas__logo logo">
            <nuxt-link href="/">
              <img src="/img/logo/logo.png" alt="logo" />
            </nuxt-link>
          </div>
        </div>
        <!-- dados usuario start -->
        <header-component-top-menu/>
        <!-- dados usuario end -->

        <!-- mobile category start -->
        <header-component-mobile-categories :product-type="productType" />
        
        <!-- mobile category end -->
        <div class="tp-main-menu-mobile fix d-lg-none mb-40">
          <!-- mobile menus start -->
          <header-component-mobile-menus />
          <!-- mobile menus end -->
        </div>

        <div class="offcanvas__contact align-items-center d-none">
          <div class="offcanvas__contact-icon mr-20">
            <span>
              <img src="/img/icon/contact.png" alt="contact_img" />
            </span>
          </div>
          <div class="offcanvas__contact-content">
            <h3 class="offcanvas__contact-title">
              <a href="tel:098-852-987">004524865</a>
            </h3>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div
    @click="utilsStore.handleOpenMobileMenu()"
    :class="`body-overlay ${utilsStore.openMobileMenus ? 'opened' : ''}`"
  ></div>
</template>

<script setup lang="ts">
import { useUtilityStore } from "@/pinia/useUtilityStore";
const props = defineProps<{ productType: string }>();
const utilsStore = useUtilityStore();

let isToggleActive = ref<string>("");
// handle active
const handleToggleActive = (type: string) => {
  if (type === isToggleActive.value) {
    isToggleActive.value = "";
  } else {
    isToggleActive.value = type;
  }
};
</script>
