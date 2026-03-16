# Quanta Shop Web

Frontend Nuxt.js 3 da plataforma de cashback Quanta Shop.

## Stack

- **Framework:** Nuxt.js 3 (Vue 3, TypeScript)
- **Estilização:** Bootstrap 5 + SCSS
- **Estado:** Pinia
- **Componentes:** Swiper, Vue3 Carousel, VeeValidate
- **PWA:** @vite-pwa/nuxt
- **Renderização:** SSR desabilitado (SPA client-side)

## Estrutura

- `pages/` — Páginas/rotas da aplicação
- `components/` — Componentes Vue reutilizáveis
- `layouts/` — Layouts de página (default, layout-one a layout-four)
- `composables/` — Composables Vue (useApi, useMask, useSticky)
- `pinia/` — Stores Pinia
- `data/` — Dados estáticos (produtos, blogs, categorias etc.)
- `services/` — Módulos de serviço de API
- `plugins/` — Plugins Nuxt (directives, filters, mask)
- `assets/` — Estilos CSS/SCSS e fontes
- `public/` — Assets estáticos (imagens, ícones)
- `server/routes/` — Nitro server-side API proxy (`api-proxy/[...path].ts`)

## Desenvolvimento

```bash
npm run dev       # Servidor dev na porta 5000
npm run build     # Build para produção
npm run generate  # Gerar site estático
```

## Configuração

- Dev server: `0.0.0.0:5000` (configurado em `nuxt.config.ts`)
- API base URL via variável de ambiente `NUXT_API_BASE_URL` (padrão: `/api-proxy`)
- Env de desenvolvimento: `.env.development` (`NUXT_API_BASE_URL=/api-proxy`)
- Env de produção: `NUXT_API_BASE_URL=/api-proxy` (configurado via env vars do Replit)

## Arquitetura do Proxy API

O site original fazia chamadas diretas para `https://api.quantashop.com.br/api` que eram bloqueadas por CORS no navegador. A solução:

1. **Proxy Nitro** (`server/routes/api-proxy/[...path].ts`): Rota catch-all em `/api-proxy/*` que encaminha requisições do browser para a API externa, servidor-a-servidor (sem CORS). Localizado em `server/routes/` (não `server/api/`) para criar a rota `/api-proxy/` em vez de `/api/`
2. **useApi composable** (`composables/useApi.ts`): Cria instância Axios com `baseURL: /api-proxy`, direcionando tudo pelo proxy. Usa lazy singleton para funcionar quando chamado no nível do módulo pelos services
3. **Layout paralelo** (`layouts/layout-one.vue`): Todas as chamadas de API (categorias, carrosséis, parceiros) são feitas em paralelo via `Promise.allSettled` para evitar bloqueio

## Deploy

Configurado como servidor Node.js (autoscale):
- Build: `npm run build` (Nitro preset: `node-server`)
- Run: `node .output/server/index.mjs`
- O proxy API funciona em produção porque o Nitro roda como servidor
- Variáveis de produção (env vars Replit): `PORT=5000`, `HOST=0.0.0.0`, `NUXT_API_BASE_URL=/api-proxy`
- Nota: Nitro lê `HOST`/`PORT` nativamente das env vars em runtime (não precisa configurar em nuxt.config.ts)
