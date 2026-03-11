<template>
  <div>
    <nuxt-layout name="layout-one">
      <!-- search start -->
      <div class="container-fluid" style="background-color: #1E5d68;">
        <div class="row">
          <div class="col-12 d-md-none mt-20 mb-20">
            <header-component-search />
          </div>
        </div>
      </div>
      <!-- search end -->

      <!-- hero banner start -->
      <hero-banner-one />
      <!-- hero banner end -->

      <!-- partners area start -->
      <partners />
      <!-- partners area end -->

      <!-- feature area start -->
      <feature-one />
      <!-- feature area end -->

      <!-- banner area start -->
      <banner-area />
      <!-- banner area en -->

      <!-- offer product start -->
      <product-electronics-offer-items />
      <!-- offer product end -->

      <!-- category start -->
      <categories-featured />
      <!-- category end -->

      <!-- super humano area start -->
      <super-humano />
      <!-- super humano area end -->

      <!-- product banner start -->
      <banner-area-3 />
      <!-- product banner end -->

      <!-- local partners area start -->
      <partners-local />
      <!-- local partners area end -->

      <!-- product sm items start -->
      <partners-local-sm-items />
      <!-- product sm items end -->

      <!-- blog item start -->
      <blog />
      <!-- blog item end -->

      <!-- instagram area start -->
      <instagram-area-1 />
      <!-- instagram area end -->

      <!-- subscribe area start -->
      <subscribe-1 />
      <!-- subscribe area end -->
    </nuxt-layout>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: false,
});
useSeoMeta({ title: "Ganhe por minuto | Quanta Shop" });
</script>
