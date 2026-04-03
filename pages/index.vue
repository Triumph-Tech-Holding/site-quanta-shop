<template>
  <div>
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
  title: "Ganhe por minuto | Quanta Shop",
  script: [
    {
      type: 'application/ld+json',
      innerHTML: JSON.stringify({
        '@context': 'https://schema.org',
        '@type': 'Organization',
        'name': 'Quanta Shop',
        'url': 'https://quantashop.com.br',
        'logo': 'https://quantashop.com.br/logo.png',
        'sameAs': [
          'https://www.instagram.com/quantashop',
          'https://www.linkedin.com/company/quanta-shop',
          'https://www.youtube.com/@QuantaShop'
        ],
        'contactPoint': {
          '@type': 'ContactPoint',
          'contactType': 'Customer Support',
          'telephone': '+55-21-99698-3881',
          'url': 'https://quantashop.com.br/contato'
        }
      })
    },
    {
      type: 'application/ld+json',
      innerHTML: JSON.stringify({
        '@context': 'https://schema.org',
        '@type': 'WebSite',
        'url': 'https://quantashop.com.br',
        'name': 'Quanta Shop',
        'potentialAction': {
          '@type': 'SearchAction',
          'target': {
            '@type': 'EntryPoint',
            'urlTemplate': 'https://quantashop.com.br/shop?q={search_term_string}'
          },
          'query-input': 'required name=search_term_string'
        }
      })
    }
  ]
});
</script>
