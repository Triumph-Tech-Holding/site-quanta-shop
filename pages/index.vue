<template>
  <div>
    <home-hero />
    <home-como-funciona />
    <home-capabilities />
    <home-brand-logos />
    <home-ofertas-dia />
    <home-cupons />
    <home-parceiros-online />
    <home-parceiros-locais />
    <home-lojistas />
    <home-testimonials />
    <home-blog />
    <home-faq />
    <home-ceo />
    <home-footer-cta />
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' });

const SITE = 'https://quantashop.com.br';

useSeoMeta({
  title: 'Quanta Shop — Cashback real e a Quanta IA que busca, cota e responde por você',
  description:
    'A Quanta Shop devolve dinheiro a cada compra nas maiores lojas do Brasil. Busque, cote produtos e lojas, consulte seu saldo e tire dúvidas pela Quanta IA — direto no chat, como no WhatsApp.',
  ogTitle: 'Quanta Shop — Cashback real com a Quanta IA',
  ogDescription:
    'Busque, cote, consulte saldo e tire dúvidas pelo chat. Cashback de verdade nas maiores marcas.',
  ogType: 'website',
  ogUrl: SITE + '/',
  ogImage: SITE + '/og-cover.jpg',
  ogLocale: 'pt_BR',
  ogSiteName: 'Quanta Shop',
  twitterCard: 'summary_large_image',
  twitterTitle: 'Quanta Shop — Cashback real com a Quanta IA',
  twitterDescription:
    'Busque, cote, consulte saldo e tire dúvidas pelo chat. Cashback de verdade nas maiores marcas.',
  twitterImage: SITE + '/og-cover.jpg',
});

useHead({
  htmlAttrs: { lang: 'pt-BR' },
  link: [{ rel: 'canonical', href: SITE + '/' }],
  meta: [{ name: 'robots', content: 'index, follow, max-image-preview:large' }],
  script: [
    {
      type: 'application/ld+json',
      innerHTML: JSON.stringify({
        '@context': 'https://schema.org',
        '@graph': [
          {
            '@type': 'Organization',
            '@id': SITE + '/#org',
            name: 'Quanta Shop',
            url: SITE + '/',
            logo: SITE + '/logo.png',
            description: 'Plataforma brasileira de consumo inteligente com cashback em rede.',
            sameAs: [
              'https://www.instagram.com/quantashop',
              'https://www.facebook.com/quantashop',
              'https://www.youtube.com/@quantashop',
            ],
          },
          {
            '@type': 'WebSite',
            '@id': SITE + '/#website',
            url: SITE + '/',
            name: 'Quanta Shop',
            inLanguage: 'pt-BR',
            publisher: { '@id': SITE + '/#org' },
            potentialAction: {
              '@type': 'SearchAction',
              target: {
                '@type': 'EntryPoint',
                urlTemplate: SITE + '/partners?nome={search_term_string}',
              },
              'query-input': 'required name=search_term_string',
            },
          },
          {
            '@type': 'FAQPage',
            '@id': SITE + '/#faq',
            mainEntity: [
              {
                '@type': 'Question',
                name: 'O que é a Quanta Shop?',
                acceptedAnswer: { '@type': 'Answer', text: 'É uma plataforma de cashback em rede: você compra nas suas lojas favoritas e recebe dinheiro de volta, podendo ainda ganhar com a sua rede de consumo.' },
              },
              {
                '@type': 'Question',
                name: 'O cashback é dinheiro de verdade?',
                acceptedAnswer: { '@type': 'Answer', text: 'Sim. O valor acumulado pode ser sacado via PIX, de forma transparente e instantânea.' },
              },
              {
                '@type': 'Question',
                name: 'Como recebo e saco meu cashback?',
                acceptedAnswer: { '@type': 'Answer', text: 'O cashback cai automaticamente na sua carteira Quanta após a compra confirmada. O saque é via PIX, direto pela plataforma.' },
              },
            ],
          },
          {
            '@type': 'ItemList',
            '@id': SITE + '/#ofertas',
            name: 'Ofertas do Dia — Quanta Shop',
            itemListElement: [
              { '@type': 'ListItem', position: 1, item: { '@type': 'Product', name: 'Tênis Air Max Premium', brand: 'Nike', offers: { '@type': 'Offer', price: '599.90', priceCurrency: 'BRL', availability: 'https://schema.org/InStock' } } },
              { '@type': 'ListItem', position: 2, item: { '@type': 'Product', name: 'Bolsa Couro Elegance', brand: 'Vivara', offers: { '@type': 'Offer', price: '349.90', priceCurrency: 'BRL', availability: 'https://schema.org/InStock' } } },
              { '@type': 'ListItem', position: 3, item: { '@type': 'Product', name: 'Smartwatch GT Series', brand: 'Motorola', offers: { '@type': 'Offer', price: '899.90', priceCurrency: 'BRL', availability: 'https://schema.org/InStock' } } },
              { '@type': 'ListItem', position: 4, item: { '@type': 'Product', name: 'Kit Café Gourmet', brand: 'Nescafé', offers: { '@type': 'Offer', price: '89.90', priceCurrency: 'BRL', availability: 'https://schema.org/InStock' } } },
            ],
          },
        ],
      }),
    },
  ],
});
</script>
