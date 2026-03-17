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
  modules: [[
    '@pinia/nuxt',
    {
      autoImports: [
        'defineStore',
        ['defineStore', 'definePiniaStore'],
      ],
    },
  ], "nuxt-gtag", '@vite-pwa/nuxt'],
  plugins: ['~/plugins/directives.ts', '~/plugins/filters.ts', '~/plugins/mask.ts'],
  gtag: {
    id: 'G-3YM68FHXJW'
  },
  pwa: {
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
      navigateFallback: "/",
      globDirectory: ".nuxt/dev-sw-dist",
      globPatterns: [
        "**/*.js",   // Incluir arquivos .js
        "**/*.map"   // Incluir arquivos .map
      ],
      globIgnores: [
        "**/node_modules/**/*",
        "sw.js",
        "workbox-*.js"
      ]
    },
    devOptions: {
      enabled: true,
      type: "module",
    },
  },
  app: {
    head: {
      title: "Quanta Shop",
      charset: 'utf-8',
      viewport: 'width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no',
      script: [
        {
          src: "https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/js/bootstrap.bundle.min.js",
        },
      ],
      htmlAttrs: {
        lang: 'pt-BR',
      },
    }
  },
  css: [
    "bootstrap/scss/bootstrap.scss",
    "swiper/css/bundle",
    "@/assets/css/font-awesome-pro.css",
    "@/assets/css/flaticon_shofy.css",
    "@/assets/scss/main.scss",
  ],
  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.NUXT_API_BASE_URL || '/api-proxy'
    },
  },
  ssr: false,
  hooks: {
    'pages:extend'(pages) {
      // add a route
      // const newRoutes = [
      //   {
      //     name: 'contact',
      //     path: '/contato',
      //     file: '~/Pages/contact.vue'
      //   },
      //   {
      //     name: 'about',
      //     path: '/quem-somos',
      //     file: '~/Pages/about.vue'
      //   },
      //   {
      //     name: 'search',
      //     path: '/busca',
      //     file: '~/Pages/search.vue'
      //   }
      // ];

      // newRoutes.forEach(route => {
      //   pages.push(route);
      // });

      // remove routes
      // function removePagesMatching(pattern: RegExp, pages: NuxtPage[] = []) {
      //   const pagesToRemove = []
      //   for (const page of pages) {
      //     if (pattern.test(page.file as string)) {
      //       pagesToRemove.push(page)
      //     } else {
      //       removePagesMatching(pattern, page.children)
      //     }
      //   }
      //   for (const page of pagesToRemove) {
      //     pages.splice(pages.indexOf(page), 1)
      //   }
      // }
      // removePagesMatching(/\.ts$/, pages)
    }
  }
})