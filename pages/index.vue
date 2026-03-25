<template>
  <div>
    <home-hero />
    <home-brand-logos />
    <home-ofertas-dia />
    <home-parceiros-online />
    <home-parceiros-locais />
    <home-testimonials />
    <home-blog />
    <home-ceo />
    <home-footer-cta />
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: 'layout-home',
});
useSeoMeta({ title: "Ganhe por minuto | Quanta Shop" });
</script>
