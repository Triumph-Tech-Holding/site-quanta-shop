<template>
  <div class="v2-page">
    <HeroV2 />
    <OmniSearchV2 />
    <BrandsV2 />
    <OffersV2 />
    <LocalV2 />
    <CeoV2 />
  </div>
</template>

<script setup lang="ts">
import HeroV2 from '~/components/home-v2/HeroV2.vue'
import OmniSearchV2 from '~/components/home-v2/OmniSearchV2.vue'
import BrandsV2 from '~/components/home-v2/BrandsV2.vue'
import OffersV2 from '~/components/home-v2/OffersV2.vue'
import LocalV2 from '~/components/home-v2/LocalV2.vue'
import CeoV2 from '~/components/home-v2/CeoV2.vue'

definePageMeta({
  layout: 'layout-home',
})

useHead({
  title: 'Quanta Shop - Cashback de Verdade em Cada Compra',
  meta: [
    {
      name: 'description',
      content: 'Transforme seus gastos diários em saldo CCR que cresce todo dia. Dinheiro na conta, não pontos. O melhor cashback do Brasil.',
    },
    { name: 'robots', content: 'noindex' },
  ],
})
</script>

<style>
.v2-page {
  min-height: 100vh;
  background: #F4F4F5;
}
</style>
