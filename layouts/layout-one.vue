<template>
  <div>
    <header-one />
    <main>
      <slot />
    </main>
    <footer-one />
    <back-to-top />
    <!-- <modal-product /> -->
  </div>
</template>

<script setup lang="ts">
useSeoMeta({
  description: "Lojistas, aumentem suas vendas com o programa de cashback da Quanta Shop. Cadastre-se agora em quantashop.com.br/credenciar e ofereça mais valor aos seus clientes!",
  
  // Open Graph Meta Tags
  ogUrl: "https://quantashop.com.br",
  ogType: "website",
  ogTitle: "Quanta Shop",
  ogDescription: "Lojistas, aumentem suas vendas com o programa de cashback da Quanta Shop. Cadastre-se agora em quantashop.com.br/credenciar e ofereça mais valor aos seus clientes!",
  ogImage: "https://res.cloudinary.com/dryd9bfjj/image/upload/v1716503306/Quanta%20Shop/ectutuigpjez4ufngegb.png",
  ogImageWidth: "500",
  ogImageHeight: "500",
  
  // Twitter Meta Tags
  twitterCard: "summary_large_image",
  twitterTitle: "Quanta Shop",
  twitterDescription: "Lojistas, aumentem suas vendas com o programa de cashback da Quanta Shop. Cadastre-se agora em quantashop.com.br/credenciar e ofereça mais valor aos seus clientes!",
  twitterImage: "https://res.cloudinary.com/dryd9bfjj/image/upload/v1716503306/Quanta%20Shop/ectutuigpjez4ufngegb.png",
})

import { useCategoryStore } from "@/pinia/useCategoryStore";
import { useCarouselStore } from "@/pinia/useCarouselStore";
import { usePartnerStore } from "@/pinia/usePartnerStore";

const categoryStore = useCategoryStore();
const carouselStore = useCarouselStore();
const partnerStore = usePartnerStore();

onMounted(async () => {
  // Categories
  if (!categoryStore.categories.length) await categoryStore.fetchCategories();
  if (!categoryStore.featuredCategories.length) await categoryStore.fetchFeaturedCategories();
  
  // Carousels
  if (!carouselStore.carousels.length) await carouselStore.fetchCarousels();

  // Partners
  if (!partnerStore.newPartners.length) await partnerStore.fetchNewPartners();
  if (!partnerStore.featuredPartners.length) await partnerStore.fetchFeaturedPartners();
  if (!partnerStore.topSellersPartners.length) await partnerStore.fetchTopSellersPartners();
  
  // Local Partners
  if (!partnerStore.localPartners.length) await partnerStore.fetchLocalPartners();
  if (!partnerStore.bestDiscountsLocalPartners.length) await partnerStore.fetchBestDiscountsLocalPartners();
  if (!partnerStore.featuredLocalPartners.length) await partnerStore.fetchFeaturedLocalPartners();
  if (!partnerStore.topSellersLocalPartners.length) await partnerStore.fetchTopSellersLocalPartners();
});
</script>
