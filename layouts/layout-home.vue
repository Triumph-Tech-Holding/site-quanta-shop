<template>
  <div>
    <header-home />
    <main>
      <slot />
    </main>
    <footer-home />
    <back-to-top />
  </div>
</template>

<script setup lang="ts">
useSeoMeta({
  description: "Lojistas, aumentem suas vendas com o programa de cashback da Quanta Shop. Cadastre-se agora em quantashop.com.br/credenciar e ofereça mais valor aos seus clientes!",
  ogUrl: "https://quantashop.com.br",
  ogType: "website",
  ogTitle: "Quanta Shop",
  ogDescription: "Lojistas, aumentem suas vendas com o programa de cashback da Quanta Shop. Cadastre-se agora em quantashop.com.br/credenciar e ofereça mais valor aos seus clientes!",
  ogImage: "https://res.cloudinary.com/dryd9bfjj/image/upload/v1716503306/Quanta%20Shop/ectutuigpjez4ufngegb.png",
  ogImageWidth: "500",
  ogImageHeight: "500",
  twitterCard: "summary_large_image",
  twitterTitle: "Quanta Shop",
  twitterDescription: "Lojistas, aumentem suas vendas com o programa de cashback da Quanta Shop. Cadastre-se agora em quantashop.com.br/credenciar e ofereça mais valor aos seus clientes!",
  twitterImage: "https://res.cloudinary.com/dryd9bfjj/image/upload/v1716503306/Quanta%20Shop/ectutuigpjez4ufngegb.png",
})

import { useCategoryStore } from "@/pinia/useCategoryStore";
import { useCarouselStore } from "@/pinia/useCarouselStore";
import { usePartnerStore } from "@/pinia/usePartnerStore";
import { useLoadingStore } from "@/pinia/useLoadingStore";
import { useHomeCmsStore } from "@/pinia/useHomeCmsStore";

const categoryStore = useCategoryStore();
const carouselStore = useCarouselStore();
const partnerStore = usePartnerStore();
const loadingStore = useLoadingStore();
const homeCmsStore = useHomeCmsStore();

onMounted(async () => {
  loadingStore.setLoading(true);

  const tasks = [];

  if (!homeCmsStore.loaded) tasks.push(homeCmsStore.fetchConfig());
  if (!carouselStore.carousels.length) tasks.push(carouselStore.fetchCarousels());
  if (!partnerStore.newPartners.length) tasks.push(partnerStore.fetchNewPartners());
  if (!partnerStore.featuredPartners.length) tasks.push(partnerStore.fetchFeaturedPartners());
  if (!partnerStore.localPartners.length) tasks.push(partnerStore.fetchLocalPartners());

  await Promise.allSettled(tasks);

  loadingStore.setLoading(false);
});
</script>
