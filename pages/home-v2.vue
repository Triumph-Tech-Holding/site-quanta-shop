<template>
  <div class="v2-page">
    <!-- 1. Hero: slides esquerda + painel produtos direita -->
    <HeroV2 />

    <!-- 2. OmniSearch -->
    <OmniSearchV2 />

    <!-- 3. Brands carousel -->
    <BrandsV2 />

    <!-- 4. Lojas online + Locais (lado a lado) -->
    <section class="bg-[#F4F4F5] py-12 lg:py-16">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="grid lg:grid-cols-[3fr_2fr] gap-8 lg:gap-10">
          <!-- Lojas online -->
          <div>
            <OnlineStoresV2 />
          </div>
          <!-- Locais -->
          <div>
            <LocalV2 />
          </div>
        </div>
      </div>
    </section>

    <!-- 5. Blog / Fankoo -->
    <BlogSection />

    <!-- 6. CTA Banner -->
    <CtaBanner />
  </div>
</template>

<script setup lang="ts">
import HeroV2 from '~/components/home-v2/HeroV2.vue'
import OmniSearchV2 from '~/components/home-v2/OmniSearchV2.vue'
import BrandsV2 from '~/components/home-v2/BrandsV2.vue'
import OnlineStoresV2 from '~/components/home-v2/OnlineStoresV2.vue'
import LocalV2 from '~/components/home-v2/LocalV2.vue'
import BlogSection from '~/components/home-v2/BlogSection.vue'
import CtaBanner from '~/components/home-v2/CtaBanner.vue'

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
