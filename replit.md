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
- `layouts/` — Layouts de página (default, layout-one a layout-four, primeira-compra)
- `composables/` — Composables Vue (useApi, useMask, useSticky)
- `pinia/` — Stores Pinia
- `data/` — Dados estáticos (produtos, blogs, categorias etc.)
- `services/` — Módulos de serviço de API
- `plugins/` — Plugins Nuxt (directives, filters, mask, vue3-toastify)
- `assets/` — Estilos CSS/SCSS e fontes
- `public/` — Assets estáticos (imagens, ícones)
- `server/routes/` — Nitro server-side API proxy (`api-proxy/[...path].ts`)
- `api/` — API .NET 8 (MMN.Api) rodando na porta 8000

## Módulos Integrados

### primeira-compra (migrado do React 18)
Rota: `/primeira-compra/[cnpj]` (CNPJ opcional, também aceita `?cnpj=xxx`)
- Página de cadastro de primeira compra com cashback
- Layout dedicado sem header/footer do site principal (`layouts/primeira-compra.vue`)
- Componentes em `components/primeira-compra/`
- Estilos em `assets/scss/primeira-compra.scss`
- Chamadas de API via proxy Nitro (`POST /api-proxy/user/primeiraCompra`)

### agencia (migrado do Vue 2)
Rota raiz: `/agencia` (login), painel: `/agencia/painel/**`, admin: `/agencia/painel/admin/**`
- Sistema completo de agência com login, painel do afiliado e área admin
- Layout de login (`layouts/agencia-login.vue`) e layout do painel (`layouts/agencia-painel.vue`)
- Auth store em `pinia/useAgenciaStore.ts`, exportado como composable em `composables/useAgenciaStore.ts`
- Middleware de rota `middleware/agencia-auth.ts` — protege todas as rotas `/agencia/painel/**`
- Menu lateral dinâmico em `components/agencia/AgenciaMenu.vue`
- Estilos em `assets/scss/agencia.scss`
- JWT armazenado em localStorage com chave `agencia_user`; perfil admin em `agencia_userAdmin`
- Menu cacheado em localStorage com chave `agencia_menu`, carregado da API `/geral/obterMenu/{perfil}`
- Cores: primary `#2f7785`, secondary `#98c73a`, bg `#ecf2f7`
- **Páginas painel:** index, meus-dados, minhas-compras, financeiro, minha-rede, contas-bancarias, assinatura, planos, meus-diretos, graduacoes, cupons, performance, suporte, solicitar-suporte, faq, material-apoio, meus-cupons, gerar-cupons, inserir-cupom, logout
- **Páginas admin:** index + usuarios, pagamentos, compras, credenciamento, categorias, ecossistemas, carrosseis, comunicados, rede, suporte, lojas-credenciados, alterar-dados-usuario, relatorio-de-faturas, relatorio-de-anunciantes, relatorio-cashback, aniversariantes, acessos, lancamentos, material-apoio-admin, assinaturas, gerenciar-grupos
- **Páginas públicas:** login (redirect para /agencia), cadastro, recuperar-senha, quem-somos, como-funciona, faq (público), privacidade, parceiro-direto/[slug], lojas-fisicas, confirm-email/[token], reset-password/[token], mais-vendas/[[login]], finalizar-credenciamento/[id], login-social/google, no-permission
- **Middleware admin:** `middleware/agencia-admin.ts` — protege `/agencia/painel/admin/**`, usa `useAgenciaStore()` para validar token e perfil admin; redireciona para `/agencia/no-permission` se não autorizado
- **ApexCharts:** `vue3-apexcharts` instalado; plugin em `plugins/apexcharts.client.ts`; usado na página de performance com gráfico de área
- **Tipos:** `types/agencia.ts` define todas as interfaces do domínio; `types/nuxt.d.ts` declara o tipo do plugin `$toast`; `composables/useAgenciaStore.ts` reexporta o store para auto-import do Nuxt
- **Axios timeout:** 30s (suficiente para cargas pesadas de produto via proxy com fallback remoto)
- **Build validado:** `npm run build:nuxt` passa sem erros; avisos de fontes/imagens externas são `WARN` ignoráveis

## API .NET 8

- Repositório clonado em `api/` (MMN.Api)
- Porta: 8000
- Workflow: "Start API" (`bash api/start-api.sh`)
- Banco de dados: Azure SQL (`bigcash.database.windows.net`)
- Connection string: secret `SQL_CONNECTION_STRING` (pode ser só a senha — o script monta o resto)
- O proxy Nitro (`/api-proxy`) encaminha para `localhost:8000` quando `USE_LOCAL_API=true`

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

## Parceiros Locais (components/partners-local/)

- `item.client.vue` e `sm-item.vue`: links usam `item.link` (vindo da API) — não mais hardcoded para `agencia.quantashop.com.br`
- `pinia/usePartnerStore.ts`: funções `fetchLocalPartners`, `fetchBestDiscountsLocalPartners`, `fetchFeaturedLocalPartners`, `fetchTopSellersLocalPartners` — só substituem `{userId}` no link se o placeholder estiver presente; links diretos (sem `{userId}`) são preservados para qualquer estado de login
- Links dos parceiros locais apontam para `escritorio.quantashop.com.br` (retornado pela API)

## Segurança da API (.NET)

- **JWT access token**: expira em 60 minutos (`AddMinutes(60)` em `UsuarioNegocio.cs`)
- **Refresh token**: expira em 30 dias (`AddDays(30)` em `UsuarioNegocio.cs`)
- **CORS**: restrito a domínios Quanta via `WithOrigins(...)` em `Startup.cs`; lê variável de ambiente `ALLOWED_ORIGINS` (vírgula-separada); fallback para `quantashop.com.br`, `www.quantashop.com.br`, `escritorio.quantashop.com.br`, `app.quantashop.com.br`
- **Rate limiting**: `FixedWindowRateLimiter` nos endpoints de autenticação — 10 req/60s por IP, retorna HTTP 429; configurado em `Startup.cs`, aplicado com `[EnableRateLimiting("auth-limit")]` em `UsuarioLoginController.cs`
- **Cookie de debug removido**: bloco `oh_vida_oh_ceus` removido de `ExceptionHandler.cs` — erros internos são logados no stderr, nunca expostos na resposta HTTP

## Sistema de Design Premium (Task #33)

Arquivo: `assets/scss/quanta-premium.scss` — importado por último em `nuxt.config.ts` css array.

### Tokens (CSS vars `--qs-*`)
- **Cores**: `--qs-primary` `#1e5d68`, `--qs-primary-dark` `#0d3d47`, `--qs-lime` `#98c73a`
- **Gradientes**: `--qs-gradient-primary`, `--qs-gradient-btn`, `--qs-gradient-hero`
- **Sombras**: `--qs-shadow-xs/sm/md/lg`
- **Border-radius**: `--qs-radius-sm/md/lg/pill`

### Melhorias Visuais
- **Tipografia**: fonte Inter 300–800, anti-aliasing, letter-spacing negativo nos títulos
- **Botões**: `.tp-btn-2` usa gradiente teal, hover lift, box-shadow verde
- **Cards parceiros**: border-radius 12px, hover elevation + translate(-4px), cashback badge pill no canto superior esquerdo
- **Header sticky**: backdrop-blur(12px), search bar com border-radius pill + focus ring
- **Breadcrumb**: fundo `--qs-gradient-primary` (teal escuro → mid)
- **Login**: card branco com sombra lg, ícone circular degradê, campo de senha com focus ring
- **Footer**: fundo near-black `#0d1117`, texto `#a0aec0`, hover em lime `#98c73a`
- **Scrollbar**: 6px, thumb teal, border-radius pill
- **Seções**: padding 80px desktop / 48px mobile
- **Skeleton**: animação `qs-skeleton-wave` disponível via classe `.qs-skeleton`

## Bugs Conhecidos (Não Resolvidos)

- **EF Core 1-to-1 CredenciamentoMapping**: Carrega credenciamento errado para usuários com múltiplas linhas na tabela

## Deploy

Configurado como servidor Node.js (autoscale) com Nuxt + .NET API rodando juntos:

### Scripts de produção
- **`build-prod.sh`** — Compila a API .NET (`dotnet publish -c Release -o api/publish/`) e depois o Nuxt (`npm run build`)
- **`start-prod.sh`** — Inicia a API em background (requer binário publicado em `api/publish/`, falha com exit 1 se ausente), aguarda a API ficar saudável na porta 8000 (health check em `/api/v2/carousels`), então inicia o servidor Nuxt em foreground (`node .output/server/index.mjs`)

### Variáveis de ambiente necessárias
- `SQL_CONNECTION_STRING` (secret Replit) — pode ser só a senha, o script monta a connection string completa
- `USE_LOCAL_API=true` (configurado em `[userenv.shared]` do `.replit`) — garante que o proxy Nitro use `localhost:8000`
- `NUXT_API_BASE_URL=/api-proxy` (configurado em `[userenv.production]`) — URL base das chamadas de API

### Como verificar em produção
```bash
# A API está respondendo?
curl https://seudominio.replit.app/api-proxy/v2/carousels
# O Nuxt está servindo?
curl https://seudominio.replit.app/
```

### Observações
- O proxy Nitro (`server/routes/api-proxy/[...path].ts`) rota todas as chamadas para `localhost:8000` em produção
- O `start-prod.sh` aguarda até 120 segundos para a API iniciar antes de subir o Nuxt
- Sem fallback: se o binário publicado não existir em `api/publish/`, o script aborta com erro claro (exit 1)
