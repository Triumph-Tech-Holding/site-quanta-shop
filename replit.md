# Quanta Shop Web

## 🚀 Modernização Premium — Power Mode (Mai 2026)

### Documentação técnica oficial criada na raiz
- `DESIGN_SYSTEM.md` — padrão visual premium minimalista (paleta restrita, tipografia Inter, espaçamento 4px, componentes admin canônicos, anti-padrões)
- `FEATURES.md` — catálogo de features por 4 fases de MVP e 3 públicos (Consumidor, Lojista, Agente)
- `STORIES.md` — user stories com personas detalhadas (Maria, João, Carla)
- `DATA_DICTIONARY.md` — engenharia reversa das entidades `.NET 8` com marcação LGPD (🔒) por campo sensível
- `TESTING.md` — plano com 10 cenários canônicos do algoritmo de compensação + 7 cenários de compressão dinâmica
- `CHANGELOG.md` — histórico semver iniciado neste sprint

### Painel admin — Features & MVP (Q Cuida + Premium)
- `pages/agencia/painel/admin/features.vue` — nova tela com KPI strip, filtros (fase / status / público), barras de progresso lineares, lista expansível por feature, totalmente aderente ao Premium Design System
- `public/docs/features.json` — fonte de dados (4 MVPs, 4 públicos, 25 features)
- Link em `AgenciaMenu.vue` (admin block) + card no admin index
- `CLAUDE.md` agora referencia os novos docs como leitura prioritária

---

## Documentação Técnica + Blog API Híbrido (Tasks #102 + #103 — COMPLETO)

### CLAUDE.md (raiz)
Arquivo de contexto técnico permanente criado em `/CLAUDE.md`. Documenta stack, estrutura, padrões, paleta, proxy, LGPD e histórico de tasks. Deve ser lido antes de qualquer modificação no projeto.

### Admin Docs (Task #102)
- `pages/agencia/painel/admin/docs.vue` — painel de documentação técnica com 9 seções colapsáveis:
  1. Arquitetura & Stack
  2. Design System (paleta, classes, tipografia)
  3. Páginas Públicas (tabela com status)
  4. Painel Admin (todas as rotas)
  5. Blog — Arquitetura Híbrida (fluxo dev/prod)
  6. API & Proxy (env vars, endpoints, alerta LGPD)
  7. Histórico de Tasks (sprints)
  8. Pendentes (cards com prioridade)
  9. Convenções de Código
  - Botão "Baixar PDF" via `window.print()` com estilos `@media print`
  - Botão "Copiar CLAUDE.md" via `navigator.clipboard`
  - Badge de ambiente (dev amarelo / prod verde com animação pulse)
- Card "📋 Documentação Técnica" adicionado ao admin index

### Blog Arquitetura Híbrida (Task #103)
**Problema mapeado:** Proxy `server/routes/api-proxy/[...path].ts` tenta API local (porta 8000) e, ao falhar, cai para `api.quantashop.com.br` (produção). Escritas de teste contaminariam banco real.

**Solução implementada via `import.meta.dev`:**
| Ação | Dev (Replit) | Produção |
|------|-------------|---------|
| Admin escrita (POST/PUT/DELETE) | localStorage **apenas** | API real |
| Admin leitura (GET) | API → fallback localStorage | API real |
| Público leitura (GET) | API → fallback localStorage | API real |

**Arquivos alterados:**
- `pages/agencia/painel/admin/blog.vue` — adicionado `isDev = import.meta.dev`. Em dev: `usandoLocal` inicia como `true` e permanece `true` (banner warning mostra motivo LGPD). `salvar()` e `excluir()` gateados com `!isDev` antes de chamar a API. Em prod: se API retorna dados, `usandoLocal = false` e escritas vão para API.
- `pages/blog/index.vue` — `onMounted` agora tenta `api.get('/blog/artigos')` primeiro (leitura pública é segura). Se API falhar ou retornar vazio, cai para localStorage. Refatorado para `mapArtigos()` compartilhado.

## Blog Premium (Task #101 — COMPLETO + redesign Apple)

### Estrutura de páginas
- `pages/blog/index.vue` — Listagem Apple-style: hero preto clean, eyebrow "INSIGHTS & IDEIAS", nav sticky frosted-glass com pills, hero-card 2-col (imagem + copy), grid 3-col de cards, newsletter dark. Lê de `qs_blog_artigos` localStorage (publicado=true). Fallback com 6 artigos de exemplo.
- `pages/blog/[id].vue` — Detalhe de artigo Apple-style: hero preto (ou com imagem de fundo + scrim), título grande, lead, conteúdo em parágrafos com tipografia generosa, sidebar de artigos relacionados, CTA dark. Fallback "artigo não encontrado".
- Admin: `pages/agencia/painel/admin/blog.vue` — upload de imagem PNG/JPG (converte para base64, drag & drop ou colar URL). Thumbnail na tabela. Conteúdo separado em parágrafos.

### Roteamento (CORRIGIDO)
- Conflito Nuxt 3 corrigido: `pages/blog.vue` renomeado para `pages/blog/index.vue` (evita conflito com `pages/blog/[id].vue`)
- Blog items da home (`home-blog.vue`) usam `resolveComponent('NuxtLink')`. URL: `/blog/${id}` para artigos do admin, `/blog` para fallback mock.
- Artigos de redes sociais mantêm `<a target="_blank">` para URLs externas.

### Stubs antigos removidos
- `pages/blog-list.vue`, `pages/blog-grid.vue`, `pages/blog-details-2.vue`, `pages/blog-details/` (todos os arquivos)



Frontend Nuxt.js 3 da plataforma de cashback Quanta Shop.

## Design System Premium (Task #80 — COMPLETO)

### Componente QsHero (`components/qs/QsHero.vue`)
- Hero fullscreen com foto real via `<img>` (NUNCA `background-image` CSS para URLs externas)
- Overlay teal `rgba(34,95,107,0.80)` via `div` absoluto
- Symbol watermark do logo (`/img/logo/logo-symbol-white.png`) no canto inferior direito
- Pill badge com ponto verde + texto de categoria
- Título com acento lime (`titleAccent`)
- Dois floating badges (glass cards com `backdrop-filter: blur`)
- Barra de stats opcional na base
- Props: `badge`, `title`, `titleAccent`, `subtitle`, `ctaPrimary`, `ctaSecondary`, `photoUrl`, `photoAlt`, `floatingBadges[]`, `stats[]`

### Shared CSS (`assets/scss/quanta-premium.scss` — seção SHARED PAGE DESIGN SYSTEM)
Classes: `.qs-section`, `.qs-section--gray/dark`, `.qs-section-header`, `.qs-label`, `.qs-step-card`, `.qs-step-num`, `.qs-benefits-grid`, `.qs-benefit-card`, `.qs-benefit-card--light`, `.qs-testimonial`, `.qs-cta-section`, `.qs-cta-inner`, `.qs-faq`, `.qs-rewards-grid`, `.qs-reward-card`, `.qs-invite-wrap`, `.qs-link-box`, `.qs-levels-grid`, `.qs-level-card`, `.qs-example-grid`, `.qs-categories-grid`, `.qs-section-symbol`

### Páginas redesenhadas com QsHero
- `pages/para-voce.vue` — foto consumidores, badge "Para Consumidores", stats: +80mil/R$4,2M/+2.400
- `pages/para-sua-empresa.vue` — foto empresária, badge "Para Lojistas e Empresas", stats: +2.400 emp/R$4,2M/98%
- `pages/seja-um-agente.vue` — foto profissionais, badge "Oportunidade de renda extra"
- `pages/quanta-amizade.vue` — foto amigos, badge "Programa de Indicações"
- `pages/contato.vue` — hero com foto nova, pill badge, floating badges de horário

### Logo symbols
- `/img/logo/logo-symbol.png` — símbolo colorido (bQ)
- `/img/logo/logo-symbol-white.png` — símbolo branco para uso em fundos teal/dark

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
- **Axios timeout:** 30s — alinhado com o proxy Nitro (timeout 20s) + margem de rede; elimina erros `timeout of 10000ms exceeded` em chamadas de produtos pesados
- **Build validado (Fase 5):** `npm run build:nuxt` passa sem erros (1224 módulos); warnings de fontes/imagens externas são WARN ignoráveis; `.output/server/index.mjs` gerado corretamente
- **Rotas validadas (Fase 5):** `/` ✅ `/agencia` ✅ `/agencia/login` ✅ `/agencia/painel` (→ redireciona para `/agencia/login` sem token) ✅ `/primeira-compra` ✅
- **Proteção de rotas (Fase 5):** `agencia-auth.ts` redireciona `/agencia/painel` sem token para `/agencia/login`; redireciona `/agencia/painel/admin` sem permissão de admin para `/agencia/painel`; `agencia-admin.ts` é camada adicional de proteção para admin routes, redireciona para `/agencia/no-permission` (página existente)
- **Preview validado (Fase 5):** `node .output/server/index.mjs` inicia sem erros, porta 3000, serve HTTP 200 em `/` e `/agencia/login`

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

## Home Page Redesign (Nova Home — Task #33)

Nova home page implementada a partir do mockup PDF do Lovable (design de referência visual apenas — código reimplementado em Vue 3).

**Layout:** `layouts/layout-home.vue` — usado exclusivamente pela `pages/index.vue`
- Carrega: carousels, newPartners, featuredPartners, localPartners via Pinia stores

**Seções da nova home:**
1. `header/header-home.vue` — Novo header limpo: logo + nav (Para Você, Para sua Empresa, Seja um Agente, Quanta Amizade, Blog, Fale Conosco) + botões Login/Cadastre-se + search bar
2. `home/home-hero.vue` — Hero com overlay escuro no banner da API + floating UI cards ("Ativação rápida: 1 clique", "Seu saldo: R$ 127,50") + CTA "Começar Agora"
3. `home/home-brand-logos.vue` — Strip animado de logos de parceiros da API
4. `home/home-ofertas-dia.vue` — Grid 4-col com featuredPartners como "Ofertas do Dia"
5. `home/home-parceiros-online.vue` — Grid 4-col de newPartners com botão "Ative seu cashback"
6. `home/home-parceiros-locais.vue` — Grid 3-col de localPartners com badge WhatsApp
7. `home/home-testimonials.vue` — Testemunhos (Marina Costa, Rafael Oliveira, Juliana Santos)
8. `home/home-blog.vue` — Grid 3-col de artigos estáticos com imagens Unsplash
9. `home/home-ceo.vue` — Seção Mauro Triumph com botão WhatsApp e badges ("Em até 24h", "+200 fechadas")
10. `home/home-footer-cta.vue` — CTA pre-footer "Pronto para começar a economizar?"
11. `footer/footer-home.vue` — Footer escuro (#111827) com 4 colunas de links + redes sociais

**Cores da nova home:** Primary teal `#2F7785`, lime `#98C73A`, dark footer `#111827`

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

## Auth Admin (Nuxt server routes)

- **JWT emitido pelo .NET** (`GenerateTokenAndRefreshToken` em `UsuarioNegocio.cs`) contém claims via URIs `ClaimTypes.*`:
  - `ClaimTypes.Name` → usuário ID
  - `ClaimTypes.Role` → `usuario.Grupo.Descricao` (ex: `"Admin"`, `"Comerciante"`, etc.)
  - `ClaimTypes.PrimarySid` → `IdGrupo`
- **`NUXT_JWT_SECRET`** deve bater com o `AppSettings.Secret` da API .NET (configurado como env var no Replit)
- **`server/middleware/verify-jwt.ts`**: decodifica o JWT e extrai o role claim via sua URI completa (`http://schemas.microsoft.com/ws/2008/06/identity/claims/role`). Se for `"Admin"`, seta `event.context.user.admin = true`
- **`server/routes/api/admin/upload-banner.ts`**: salva arquivos localmente em `public/uploads/banners/` (sem depender da API .NET). Imagens ficam disponíveis via URL `/uploads/banners/filename.ext`
- **`AdminController.cs`** usa `[Authorize(Roles = "Admin")]` — exige role `Admin` no JWT para chamar a API .NET diretamente

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
