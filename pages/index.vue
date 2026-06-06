<template>
  <div>
    <h1 class="sr-only">Quanta Shop — Cashback Real em Centenas de Lojas Parceiras</h1>
    <home-hero />
    <home-brand-logos />
    <home-ofertas-dia />
    <home-parceiros-online />
    <home-parceiros-locais />
    <home-testimonials />
    <home-blog />
    <home-footer-cta />
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: 'layout-home',
});

useSeoMeta({
  title: "Quanta Shop — Cashback Real em Centenas de Lojas",
  description: "Ganhe dinheiro de volta em suas compras nas maiores lojas do Brasil. O Quanta Shop é a plataforma de cashback real que te ajuda a economizar de verdade.",
  ogTitle: "Quanta Shop — Cashback Real em Centenas de Lojas",
  ogDescription: "Ganhe dinheiro de volta em suas compras nas maiores lojas do Brasil. O Quanta Shop é a plataforma de cashback real que te ajuda a economizar de verdade.",
  ogImage: "https://res.cloudinary.com/dryd9bfjj/image/upload/v1716503306/Quanta%20Shop/ectutuigpjez4ufngegb.png",
  ogType: "website",
  twitterCard: "summary_large_image",
});

useHead({
  link: [
    { rel: 'canonical', href: 'https://quantashop.com.br' }
  ],
});
</script>

