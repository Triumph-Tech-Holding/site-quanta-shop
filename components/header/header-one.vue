<template>
  <header>
    <div class="tp-header-area p-relative z-index-11">
      <!-- header top start  -->
      <div
        class="tp-header-top qs-green-bg p-relative z-index-1 d-none d-md-block"
      >
        <div class="container">
          <div class="row align-items-center">
            <div class="col-md-12">
              <div
                class="tp-header-welcome d-flex align-items-center justify-content-center"
              >
                <span>
                  <svg-credit-card />
                </span>
                <p>
                  <a href="/agencia/painel/assinatura">
                  Assine o <strong>Quanta Plus</strong> e obtenha cashback em
                  dobro nas suas compras!
                </a>
                </p>
              </div>
            </div>
            <!-- <div class="col-md-6">
              <div class="tp-header-top-right d-flex align-items-center justify-content-end">
                <header-component-top-menu />
              </div>
            </div> -->
          </div>
        </div>
      </div>

      <!-- header main start -->
      <div class="tp-header-main tp-header-sticky">
        <div class="container">
          <div class="row align-items-center">
            <div class="col-xl-2 col-lg-2 col-md-4 col-6">
              <div class="logo">
                <nuxt-link href="/">
                  <img src="/img/logo/logo-white.png" alt="logo" />
                </nuxt-link>
              </div>
            </div>
            <div class="col-xl-7 col-lg-7 d-none d-lg-block">
              <!-- search form start -->
              <header-component-search />
              <!-- search form end -->
            </div>
            <div class="col-xl-3 col-lg-3 col-md-8 col-6">
              <!-- header right start -->
              <header-component-main-right />
              <!-- header right end -->
            </div>
          </div>
        </div>
      </div>

      <!-- header bottom start -->
      <div class="tp-header-bottom tp-header-bottom-border d-none d-lg-block">
        <div class="container">
          <div class="tp-mega-menu-wrapper p-relative">
            <div class="row align-items-center">
              <div class="col-xl-3 col-lg-3">
                <!-- categories start -->
                <header-component-top-categories />
                <!-- categories end -->
              </div>
              <div class="col-xl-9 col-lg-9">
                <div class="main-menu menu-style-1">
                  <nav class="tp-main-menu-content">
                    <!-- menus start -->
                    <header-component-menus />
                    <!-- menus end -->
                  </nav>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- header sticky start -->
    <div
      id="header-sticky-2"
      :class="`tp-header-sticky-area ${isSticky ? 'header-sticky-2' : ''}`"
    >
      <div class="container">
        <div class="tp-mega-menu-wrapper p-relative">
          <div class="row align-items-center">
            <div class="col-xl-3 col-lg-3 col-md-3 col-6">
              <div class="logo">
                <nuxt-link href="/">
                  <img src="/img/logo/logo.png" alt="logo" />
                </nuxt-link>
              </div>
            </div>
            <div class="col-xl-9 col-lg-9 col-md-9 d-none d-lg-block">
              <div class="tp-header-sticky-menu main-menu menu-style-1">
                <nav id="mobile-menu">
                  <!-- menus start -->
                  <header-component-menus-sticky />
                  <!-- menus end -->
                </nav>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- header sticky end -->
  </header>
  <!-- cart offcanvas start -->
  <offcanvas-cart-sidebar />
  <!-- cart offcanvas end -->

  <!-- cart offcanvas start -->
  <offcanvas-mobile-sidebar product-type="electronics" />
  <!-- cart offcanvas end -->
</template>

<script setup lang="ts">
const { isSticky } = useSticky();

</script>
