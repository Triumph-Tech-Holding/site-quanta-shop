# CLAUDE.md — Quanta Shop Platform

> Arquivo de contexto técnico permanente. Leia antes de qualquer modificação no projeto.
> Última atualização: Mai 2026

---

## Visão Geral

**Quanta Shop** é uma plataforma financeira de cashback + MLM (rede de afiliados multinível) brasileira.
O ecossistema inclui:
- **Portal do consumidor** — encontra lojas parceiras, acumula e resgata cashback
- **Painel do Agente** — back-office para Agentes de Fidelização gerenciarem sua rede
- **Painel Admin** — gestão completa da plataforma (usuários, comissões, blog, banners, suporte)
- **API .NET 8** — backend responsável por toda lógica financeira, autenticação JWT, MLM multinível e integrações

**Domínio de produção:** `https://quantashop.com.br`  
**API de produção:** `https://api.quantashop.com.br`

---

## Stack Técnica

| Camada | Tecnologia | Versão |
|--------|-----------|--------|
| Framework Frontend | Nuxt 3 | 3.8.1 |
| UI Framework | Vue 3 + Composition API | — |
| Estado global | Pinia | 2.1.6 |
| Slider/Carousel | Swiper.js | 10.0.4 |
| CSS Framework | Bootstrap | 5.3.0 |
| Estilos extras | SCSS (`assets/scss/`) | — |
| Utility CSS (scope limitado) | Tailwind CSS | — |
| Backend API | .NET 8 (ASP.NET Core) | — |
| Banco de Dados | SQL Server (Azure) | — |
| Build server | Nitro (Nuxt) | 2.7.2 |

---

## Comandos Essenciais

```bash
# Frontend (Nuxt) — porta 5000
npm run dev

# API .NET 8 — porta 8000
bash api/start-api.sh

# Build para produção
npm run build
```

---

## Paleta de Cores (Brand Tokens)

```
Teal principal:  #2F7785
Teal escuro:     #225F6B
Lima/verde:      #98C73A
Preto Apple:     #1d1d1f
Cinza fundo:     #f5f5f7
```

Não use cores hardcoded além dessas. Use sempre os tokens acima via variáveis CSS ou classes utilitárias.

---

## Estrutura de Diretórios

```
/
├── pages/
│   ├── index.vue                    # Home V1 (ATIVO - V2 foi abandonada)
│   ├── home-v2.vue                  # ABANDONADO - não usar como padrão
│   ├── para-voce.vue                # Página Para Consumidores
│   ├── para-sua-empresa.vue         # Página Para Lojistas
│   ├── seja-um-agente.vue           # Página Seja um Agente
│   ├── quanta-amizade.vue           # Programa de indicações
│   ├── blog/
│   │   ├── index.vue                # Listagem pública do blog
│   │   └── [id].vue                 # Detalhe de artigo (ID numérico)
│   ├── agencia/
│   │   ├── index.vue                # Landing da agência
│   │   ├── login.vue / cadastro.vue
│   │   └── painel/
│   │       ├── index.vue            # Dashboard do agente
│   │       ├── financeiro.vue
│   │       ├── minha-rede.vue
│   │       └── admin/               # Área exclusiva admin
│   │           ├── index.vue        # Dashboard admin
│   │           ├── blog.vue         # CRUD do blog
│   │           ├── home-cms.vue     # Textos da home
│   │           ├── carrosseis.vue   # Banners/carrosséis
│   │           ├── marcas-home.vue  # Logos do carrossel
│   │           ├── usuarios.vue
│   │           ├── credenciamento.vue
│   │           └── docs.vue         # Documentação técnica (este painel)
│
├── components/
│   ├── home/                        # Componentes da Home V1
│   │   ├── home-hero.vue            # Banner hero com Swiper
│   │   ├── home-blog.vue            # Seção blog na home
│   │   └── ...
│   ├── home-v2/                     # V2 ABANDONADA (Tailwind scoped)
│   ├── qs/                          # Design system compartilhado
│   │   └── QsHero.vue               # Hero fullscreen reutilizável
│   └── agencia/                     # Componentes do painel
│
├── layouts/
│   ├── layout-home.vue              # Layout padrão das páginas públicas
│   ├── agencia-painel.vue           # Layout do painel do agente/admin
│   └── agencia.vue                  # Layout das páginas de landing da agência
│
├── composables/
│   ├── useApi.ts                    # Wrapper axios para chamadas à API
│   ├── useAgenciaStore.ts           # Alias (usa pinia/useAgenciaStore)
│   └── useHomeConfig.ts             # Config dinâmica da home (CMS)
│
├── pinia/                           # Stores Pinia
│   ├── useAgenciaStore.ts           # Auth do agente + JWT token
│   ├── useCarouselStore.ts          # Dados dos carrosséis (legado)
│   └── ...
│
├── assets/scss/
│   ├── main.scss                    # Estilos globais
│   ├── quanta-premium.scss          # Design system premium (QS)
│   └── agencia.scss                 # Estilos do painel da agência
│
├── server/
│   └── routes/api-proxy/[...path].ts  # Proxy reverso para a API
│
├── types/
│   ├── agencia.ts                   # Tipos do painel (HeroBannerSlide, etc.)
│   └── blog-type.ts                 # Tipos do blog
│
├── public/
│   ├── data/hero-banners.json       # Configuração dos slides do hero
│   └── img/                         # Imagens estáticas
│
├── nuxt.config.ts                   # Configuração principal
├── tailwind.config.ts               # Tailwind SCOPED (ver aviso abaixo)
└── CLAUDE.md                        # Este arquivo
```

---

## Padrão do Painel Admin

Toda página do admin segue este template obrigatório:

```vue
<script setup lang="ts">
definePageMeta({
  layout: 'agencia-painel',
  middleware: ['agencia-auth', 'agencia-admin']
});
const agenciaStore = useAgenciaStore();
const api = useApi();

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}
</script>
```

**Classes CSS do admin** (definidas em `assets/scss/agencia.scss`):
- `.ag-page-header` — cabeçalho da página com título e subtítulo
- `.ag-card` — card branco com sombra suave
- `.ag-table` — tabela estilizada
- `.ag-loading` — spinner centralizado
- `.ag-empty-state` — estado vazio
- `.badge-ag`, `.badge-ag-success`, `.badge-ag-warning`, `.badge-ag-danger` — badges coloridos
- `.btn-ag-primary`, `.btn-ag-outline` — botões do painel

---

## Padrão do Blog

### Fluxo de dados (arquitetura híbrida — decisão de segurança)

```
Dev (Replit):
  ADMIN ESCRITA → localStorage apenas (isola do banco de produção)
  ADMIN LEITURA → API primeiro → fallback localStorage
  PÚBLICO LEITURA → API primeiro → fallback localStorage

Produção:
  ADMIN ESCRITA → API real (.NET 8 + SQL Server Azure)
  PÚBLICO LEITURA → API real
```

**Por que localStorage em dev?**
O proxy de API (`server/routes/api-proxy/[...path].ts`) tenta primeiro a API local (porta 8000) e, ao falhar, cai automaticamente para `https://api.quantashop.com.br/api` (produção). Sem isolamento explícito via `import.meta.dev`, escritas de teste contaminariam o banco de produção, violando as políticas de integridade de dados e LGPD da plataforma.

**Detecção de ambiente:**
```javascript
// Em Nuxt 3, import.meta.dev é true no dev server e false no build de produção
const isDev = import.meta.dev;
if (isDev) {
  // localStorage only — nunca toca a API em dev
} else {
  // API real em produção
}
```

**localStorage key:** `qs_blog_artigos`  
**IDs em dev:** `Date.now()` (timestamp numérico grande, ex: `1775252000000`)  
**Roteamento:** `pages/blog/index.vue` + `pages/blog/[id].vue`

> ⚠️ NUNCA criar `pages/blog.vue` — conflita com subrotas no Nuxt 3.
> O arquivo correto é `pages/blog/index.vue`.

---

## Proxy de API (Arquitetura de Segurança)

Arquivo: `server/routes/api-proxy/[...path].ts`

```
Dev (useLocalApi=true):
  → tenta http://localhost:8000/api
  → se falhar: cai para https://api.quantashop.com.br/api ← RISCO
  
Produção (NUXT_USE_LOCAL_API=false):
  → vai direto para https://api.quantashop.com.br/api
```

**Variáveis de ambiente:**

| Variável | Dev (`.env.development`) | Produção (`.env.production`) |
|---------|--------------------------|------------------------------|
| `NUXT_API_BASE_URL` | `/api-proxy` | `https://api.quantashop.com.br` |
| `NUXT_USE_LOCAL_API` | não definida (default: `true`) | `false` |
| `NUXT_JWT_SECRET` | `dev-secret-key-...` | **secret real** |

---

## Tailwind CSS (Atenção: Escopo Limitado)

O Tailwind está configurado com escopo restrito:

```typescript
// tailwind.config.ts
important: '.v2-page',
preflight: false,
content: ['components/home-v2/**', 'pages/home-v2.vue']
```

**Tailwind SÓ funciona dentro de `.v2-page`** (página Home V2 abandonada).  
Em todas as outras páginas, use Bootstrap 5 + classes do design system Quanta (`ag-*`, `qs-*`).

---

## Autenticação (JWT)

- Tokens gerenciados pelo `useAgenciaStore` (Pinia)
- `agenciaStore.getToken()` → retorna o JWT
- Rotas protegidas por middlewares:
  - `agencia-auth` → verifica login
  - `agencia-admin` → verifica `isAdmin`
- Tokens armazenados em `localStorage` com chave da store Pinia

---

## Páginas Públicas Disponíveis

| Rota | Página | Layout |
|------|--------|--------|
| `/` | Home V1 (padrão) | layout-home |
| `/para-voce` | Para Consumidores | layout-home |
| `/para-sua-empresa` | Para Lojistas | layout-home |
| `/seja-um-agente` | Seja um Agente | layout-home |
| `/quanta-amizade` | Programa de Indicações | layout-home |
| `/blog` | Listagem do Blog | layout-home |
| `/blog/:id` | Artigo do Blog | layout-home |
| `/shop` | Loja de produtos | layout-home |
| `/partners` | Parceiros | layout-home |
| `/agencia` | Landing Agência | agencia |
| `/agencia/login` | Login | agencia-login |
| `/agencia/cadastro` | Cadastro | agencia |

---

## Painel Admin — Telas Disponíveis

| Rota | Descrição |
|------|-----------|
| `/agencia/painel/admin` | Dashboard admin |
| `/agencia/painel/admin/usuarios` | Gestão de usuários |
| `/agencia/painel/admin/pagamentos` | Aprovação de pagamentos |
| `/agencia/painel/admin/compras` | Gestão de compras |
| `/agencia/painel/admin/credenciamento` | Credenciamento de parceiros |
| `/agencia/painel/admin/categorias` | Categorias de lojas |
| `/agencia/painel/admin/ecossistemas` | Ecossistemas |
| `/agencia/painel/admin/carrosseis` | Banners e carrosséis |
| `/agencia/painel/admin/home-cms` | CMS da home |
| `/agencia/painel/admin/marcas-home` | Logos do carrossel |
| `/agencia/painel/admin/blog` | CRUD do blog |
| `/agencia/painel/admin/redes-sociais` | Posts das redes sociais |
| `/agencia/painel/admin/suporte` | Tickets de suporte |
| `/agencia/painel/admin/rede` | Visualização da rede MLM |
| `/agencia/painel/admin/comunicados` | Comunicados |
| `/agencia/painel/admin/docs` | **Esta documentação** |

---

## Diretivas de Segurança (LGPD)

A Quanta Shop opera como plataforma financeira sujeita à LGPD:

1. **Dados sensíveis** (CPF, dados bancários) → apenas via API autenticada + HTTPS
2. **Nunca** armazenar dados sensíveis em `localStorage` ou `sessionStorage`
3. **localStorage** é aceito APENAS para: artigos do blog em dev, configurações de UI, cache não sensível
4. **Logs de auditoria** para operações financeiras são responsabilidade da API .NET 8
5. **JWT** expiração: 1440 min (24h) em produção — reduzir se necessário
6. **Banco de produção** não deve ser acessado pelo ambiente Replit/dev

---

## Convenções de Código

### Vue / Nuxt
- `<script setup lang="ts">` em todos os componentes
- `definePageMeta` sempre no início do script
- Imports de composables: `useApi()`, `useAgenciaStore()`, `useHomeConfig()`
- Não usar `Options API`

### CSS
- Componentes públicos: `scoped` CSS vanilla ou SCSS
- Admin: classes Bootstrap 5 + `.ag-*`
- Design system: `.qs-*` (`quanta-premium.scss`)
- Animações: CSS `@keyframes`, evitar JS para animações simples

### Nomenclatura
- Páginas: `kebab-case.vue`
- Componentes: `PascalCase.vue`
- Composables: `useNomeCamelCase.ts`
- Stores Pinia: `useNomeStore.ts`

### Commits
- Prefixo por tipo: `feat:`, `fix:`, `style:`, `docs:`, `refactor:`

---

## Tasks do Projeto (Histórico)

- **#1–#24**: Configuração inicial, migração Vue2→Nuxt3, deploy, autenticação
- **#25**: Cancelado (coberto pela #22)
- **#26–#27**: Documentação técnica e auditoria
- **#75–#101**: Sprint de design premium — páginas públicas, blog, admin CMS
- **#102**: Este painel de documentação + CLAUDE.md
- **#103**: Conexão blog público à API real (híbrido dev/prod)

---

*Este arquivo é mantido pelo agente Replit e deve ser atualizado a cada mudança arquitetônica significativa.*
