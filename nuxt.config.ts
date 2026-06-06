export default defineNuxtConfig({
  devtools: { enabled: false },
  devServer: {
    host: '0.0.0.0',
    port: 5000,
  },
  nitro: {
    preset: 'node-server',
  },
  vite: {
    server: {
      allowedHosts: true,
      watch: {
        ignored: ['**/api/**', '**/api/bin/**', '**/api/obj/**'],
      },
    },
  },
  watch: ['!api/**'],
  imports: {
    dirs: ['pinia', 'composables', 'composables/**'],
  },
  modules: [
    '@nuxtjs/tailwindcss',
    [
      '@pinia/nuxt',
      {
        autoImports: [
          'defineStore',
          ['defineStore', 'definePiniaStore'],
        ],
      },
    ],
    "nuxt-gtag",
    '@vite-pwa/nuxt',
  ],
  plugins: ['~/plugins/directives.ts', '~/plugins/filters.ts', '~/plugins/mask.ts'],
  gtag: {
    id: 'G-3YM68FHXJW'
  },
  pwa: {
    selfDestroying: true,
    manifest: {
      name: "Quanta Shop",
      short_name: "Quanta Shop",
      description: "Lojistas, aumentem suas vendas com o programa de cashback da Quanta Shop. Cadastre-se agora em quantashop.com.br e ofereça mais valor aos seus clientes!",
      theme_color: "#1e5d68",
      background_color: "#FFFFFF",
      icons: [
        {
          src: "img/pwa/android-launchericon-48-48.png",
          sizes: "48x48",
          type: "image/png",
        },
        {
          src: "img/pwa/android-launchericon-72-72.png",
          sizes: "72x72",
          type: "image/png",
        },
        {
          src: "img/pwa/android-launchericon-96-96.png",
          sizes: "96x96",
          type: "image/png",
        },
        {
          src: "img/pwa/android-launchericon-144-144.png",
          sizes: "144x144",
          type: "image/png",
        },
        {
          src: "img/pwa/android-launchericon-192-192.png",
          sizes: "192x192",
          type: "image/png",
        },
        {
          src: "img/pwa/android-launchericon-512-512.png",
          sizes: "512x512",
          type: "image/png",
        },
      ],
    },
    workbox: {
      globDirectory: ".nuxt/dev-sw-dist",
      globPatterns: [
        "**/*.js",
        "**/*.map"
      ],
      globIgnores: [
        "**/node_modules/**/*",
        "sw.js",
        "workbox-*.js"
      ]
    },
    devOptions: {
      enabled: false,
      type: "module",
    },
  },
  app: {
    head: {
      title: "Quanta Shop — Cashback Real em Centenas de Lojas",
      charset: 'utf-8',
      viewport: 'width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no',
      meta: [
        { name: 'description', content: 'Ganhe dinheiro de volta em suas compras nas maiores lojas do Brasil. O Quanta Shop é a plataforma de cashback real que te ajuda a economizar de verdade.' },
        { name: 'format-detection', content: 'telephone=no' },
        { property: 'og:title', content: 'Quanta Shop — Cashback Real em Centenas de Lojas' },
        { property: 'og:description', content: 'Ganhe dinheiro de volta em suas compras nas maiores lojas do Brasil. O Quanta Shop é a plataforma de cashback real que te ajuda a economizar de verdade.' },
        { property: 'og:type', content: 'website' },
        { property: 'og:url', content: 'https://quantashop.com.br' },
        { property: 'og:image', content: 'https://res.cloudinary.com/dryd9bfjj/image/upload/v1716503306/Quanta%20Shop/ectutuigpjez4ufngegb.png' },
        { name: 'twitter:card', content: 'summary_large_image' },
        { name: 'twitter:title', content: 'Quanta Shop — Cashback Real em Centenas de Lojas' },
        { name: 'twitter:description', content: 'Ganhe dinheiro de volta em suas compras nas maiores lojas do Brasil. O Quanta Shop é a plataforma de cashback real que te ajuda a economizar de verdade.' },
        { name: 'subject', content: 'Política de Privacidade: https://quantashop.com.br/agencia/privacidade' },
      ],
      link: [
        { rel: 'canonical', href: 'https://quantashop.com.br' },
        { rel: 'privacy-policy', href: 'https://quantashop.com.br/agencia/privacidade' },
        {
          rel: 'preconnect',
          href: 'https://fonts.googleapis.com',
        },
        {
          rel: 'preconnect',
          href: 'https://fonts.gstatic.com',
          crossorigin: '',
        },
        {
          rel: 'stylesheet',
          href: 'https://fonts.googleapis.com/css2?family=Jost:ital,wght@0,400;0,500;0,600;0,700;0,800;1,400&display=swap',
        },
      ],
      script: [
        {
          type: 'application/ld+json',
          innerHTML: JSON.stringify({
            '@context': 'https://schema.org',
            '@type': 'Organization',
            'name': 'Quanta Shop',
            'url': 'https://quantashop.com.br',
            'logo': 'https://quantashop.com.br/img/logo/logo-white.png',
            'description': 'Plataforma de cashback real que conecta consumidores a centenas de lojas parceiras no Brasil.',
            'sameAs': [
              'https://www.instagram.com/quantashop.oficial',
              'https://www.facebook.com/quantashop',
            ],
            'contactPoint': {
              '@type': 'ContactPoint',
              'contactType': 'Customer Support',
              'telephone': '+55-21-4040-4866',
              'url': 'https://quantashop.com.br/contato'
            }
          }),
        },
        {
          type: 'application/ld+json',
          innerHTML: JSON.stringify({
            '@context': 'https://schema.org',
            '@type': 'WebSite',
            'url': 'https://quantashop.com.br',
            'name': 'Quanta Shop',
            'description': 'Plataforma de cashback real que conecta consumidores a centenas de lojas parceiras no Brasil.',
            'potentialAction': {
              '@type': 'SearchAction',
              'target': {
                '@type': 'EntryPoint',
                'urlTemplate': 'https://quantashop.com.br/shop?q={search_term_string}'
              },
              'query-input': 'required name=search_term_string'
            }
          }),
        },
      ],
    }
  },
  css: [
    "bootstrap/scss/bootstrap.scss",
    "swiper/css/bundle",
    "@/assets/css/font-awesome-pro.css",
    "@/assets/css/flaticon_shofy.css",
    "@/assets/scss/main.scss",
    "@/assets/scss/agencia.scss",
    "@/assets/scss/quanta-premium.scss",
  ],
  runtimeConfig: {
    jwtSecret: process.env.NUXT_JWT_SECRET || 'dev-secret-key-change-in-production',
    useLocalApi: process.env.NUXT_USE_LOCAL_API !== 'false',
    public: {
      apiBaseUrl: process.env.NUXT_API_BASE_URL || '/api-proxy',
      googleClientId: process.env.GOOGLE_CLIENT_ID || '',
      siteUrl: process.env.NUXT_PUBLIC_SITE_URL || '',
      appleClientId: process.env.APPLE_CLIENT_ID || '',
      appleRedirectUri: process.env.APPLE_REDIRECT_URI || '',
    },
  },
  ssr: false,
  hooks: {
    'pages:extend'(pages) {
    }
  }
})
