<template>
  <div>
    <home-como-funciona />
    <home-capabilities />
    <home-faq />
    <home-footer-cta />
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' });

const SITE = 'https://quantashop.com.br';

useSeoMeta({
  title: 'Como funciona a Quanta Shop — Ative, compre e receba cashback via PIX',
  description:
    'Em três passos: ative o cashback, compre normalmente nas lojas parceiras e receba o dinheiro de volta via PIX. Entenda como a Quanta Shop devolve dinheiro a cada compra.',
  ogTitle: 'Como funciona a Quanta Shop',
  ogDescription: 'Ative o cashback, compre normalmente e receba via PIX. Simples, transparente e instantâneo.',
  ogType: 'website',
  ogUrl: SITE + '/como-funciona',
  ogImage: SITE + '/og-cover.jpg',
  ogLocale: 'pt_BR',
  ogSiteName: 'Quanta Shop',
  twitterCard: 'summary_large_image',
  twitterTitle: 'Como funciona a Quanta Shop',
  twitterDescription: 'Ative o cashback, compre normalmente e receba via PIX.',
  twitterImage: SITE + '/og-cover.jpg',
});

useHead({
  htmlAttrs: { lang: 'pt-BR' },
  link: [{ rel: 'canonical', href: SITE + '/como-funciona' }],
  meta: [{ name: 'robots', content: 'index, follow, max-image-preview:large' }],
  script: [
    {
      type: 'application/ld+json',
      innerHTML: JSON.stringify({
        '@context': 'https://schema.org',
        '@type': 'HowTo',
        name: 'Como funciona a Quanta Shop',
        description: 'Receba cashback em três passos: ative, compre e receba via PIX.',
        step: [
          { '@type': 'HowToStep', position: 1, name: 'Ative o cashback', text: 'Crie sua conta grátis e ative o cashback nas suas lojas favoritas com um clique.' },
          { '@type': 'HowToStep', position: 2, name: 'Compre normalmente', text: 'Compre online ou nas lojas parceiras do seu bairro — exatamente como você já faz.' },
          { '@type': 'HowToStep', position: 3, name: 'Receba via PIX', text: 'O dinheiro volta automático pra sua carteira Quanta. Saque na hora, via PIX.' },
        ],
      }),
    },
  ],
});
</script>
