# ARCHITECTURE.md — Arquitetura do Sistema · Quanta Shop

> **Versão:** 1.0.0 | **Atualizado:** Mai 2026
> Visão técnica completa da arquitetura, fluxos e módulos da plataforma.

---

## 1. Visão Geral do Sistema

A Quanta Shop é uma plataforma de **Consumo Inteligente e Marketing de Fidelização com Cashback em Rede**. A arquitetura é composta por três camadas principais:

```
┌─────────────────────────────────────────────────────────────────┐
│                    CAMADA DE APRESENTAÇÃO                        │
│                                                                  │
│  ┌──────────────┐  ┌────────────────┐  ┌────────────────────┐   │
│  │   Públicos   │  │  ADF (Agência) │  │   Painel Admin     │   │
│  │  / /blog     │  │  /agencia/...  │  │  /agencia/admin/   │   │
│  │  /shop       │  │  /agencia/     │  │  /lab/             │   │
│  │  /parceiros  │  │  painel/       │  │                    │   │
│  └──────────────┘  └────────────────┘  └────────────────────┘   │
│                    NUXT 3 (SSR) + Vue 3 + TypeScript             │
└─────────────────────────────────────────────┬───────────────────┘
                                              │ HTTPS / Proxy Nitro
┌─────────────────────────────────────────────▼───────────────────┐
│                    CAMADA DE APLICAÇÃO                           │
│                                                                  │
│              .NET 8 API (ASP.NET Core)                          │
│              api.quantashop.com.br/api                          │
│                                                                  │
│  ┌──────────────┐  ┌────────────────┐  ┌────────────────────┐   │
│  │  Auth / JWT  │  │ Motor Financ.  │  │  Rede MLM          │   │
│  │  LGPD Guard  │  │ CashbackSvc    │  │  Binário/Residual  │   │
│  └──────────────┘  └────────────────┘  └────────────────────┘   │
│  ┌──────────────┐  ┌────────────────┐  ┌────────────────────┐   │
│  │  Afiliados   │  │  CMS / Blog    │  │  Suporte           │   │
│  │  Awin/Afilio │  │  Carrosseis    │  │  Comunicados       │   │
│  └──────────────┘  └────────────────┘  └────────────────────┘   │
└─────────────────────────────────────────────┬───────────────────┘
                                              │ Entity Framework Core
┌─────────────────────────────────────────────▼───────────────────┐
│                    CAMADA DE DADOS                               │
│                   SQL Server (Azure)                             │
│                                                                  │
│  Usuário · Transacao · Lancamento · Pedido · Saque              │
│  Anunciante · Credenciamento · RefreshToken · AuditoriaLgpd     │
└──────────────────────────────────────────────────────────────────┘
```

---

## 2. Módulos do Frontend (Nuxt 3)

### 2.1 Área Pública

| Rota | Componente Principal | Descrição |
|------|---------------------|-----------|
| `/` | `pages/index.vue` | Home V1: hero Swiper, parceiros, blog, CEO |
| `/para-voce` | `pages/para-voce.vue` | Landing Consumidores |
| `/para-sua-empresa` | `pages/para-sua-empresa.vue` | Landing Lojistas |
| `/seja-um-agente` | `pages/seja-um-agente.vue` | Landing Agentes |
| `/quanta-amizade` | `pages/quanta-amizade.vue` | Programa de indicações |
| `/blog` | `pages/blog/index.vue` | Listagem Apple-style |
| `/blog/:id` | `pages/blog/[id].vue` | Artigo com sidebar |
| `/busca-inteligente` | `pages/busca-inteligente.vue` | Motor de busca com filtros |
| `/shop` | `pages/shop.vue` | Marketplace de produtos |

### 2.2 ADF — Agência Digital de Fidelização (`/agencia`)

| Rota | Middleware | Descrição |
|------|------------|-----------|
| `/agencia` | público | Landing da agência |
| `/agencia/login` | público | Login JWT + Google |
| `/agencia/cadastro` | público | Cadastro com CPF/e-mail |
| `/agencia/painel` | agencia-auth | Dashboard do agente |
| `/agencia/painel/financeiro` | agencia-auth | Extrato e saldo |
| `/agencia/painel/minha-rede` | agencia-auth + HAF | Dashboard de rede |
| `/agencia/painel/saque` | agencia-auth | Saque PIX/TED |

### 2.3 Painel Admin (`/agencia/painel/admin`)

| Rota | Descrição |
|------|-----------|
| `/agencia/painel/admin` | Dashboard operacional (KPIs + nav) |
| `/agencia/painel/admin/usuarios` | Gestão de usuários (LGPD mascarado) |
| `/agencia/painel/admin/pagamentos` | Aprovação de pagamentos |
| `/agencia/painel/admin/compras` | Gestão de compras |
| `/agencia/painel/admin/credenciamento` | Credenciamento de parceiros ZEE |
| `/agencia/painel/admin/lancamentos` | Lançamentos financeiros |
| `/agencia/painel/admin/relatorio-cashback` | Relatório de cashback |
| `/agencia/painel/admin/relatorio-de-anunciantes` | Relatório de anunciantes |
| `/agencia/painel/admin/relatorio-de-faturas` | Faturas com reveal LGPD |
| `/agencia/painel/admin/configuracoes-rede` | Percentuais, quarentena, Plus |
| `/agencia/painel/admin/bi-financeiro` | BI analítico |
| `/agencia/painel/admin/features` | Features & MVP roadmap |
| `/agencia/painel/admin/progresso` | Kanban de progresso |
| `/agencia/painel/admin/docs` | Visualizador de documentação técnica |
| `/agencia/painel/admin/blog` | CRUD do blog |

### 2.4 LAB — Cockpit Técnico Interno (`/lab`)

| Rota | Descrição |
|------|-----------|
| `/lab` | Hub do LAB (5 categorias, ~20 atalhos) |
| `/lab/flow-standard` | Checklist técnico FLOW DEVELOPMENT SYSTEMS |

---

## 3. Fluxo de Autenticação

```
[Usuario] → POST /api/v2/auth/login
               │
               ▼
         [.NET API]
         Valida CPF/Senha (bcrypt)
         Gera JWT (exp: 60min) + RefreshToken (exp: 30 dias)
               │
               ▼
         [Nuxt Frontend]
         Armazena em Pinia (useAgenciaStore)
         localStorage (persistência)
               │
               ▼
         Middleware agencia-auth
         └─ Verifica JWT → se inválido: redireciona /login
         Middleware agencia-admin
         └─ Verifica claim Role=Admin → se não: 403
```

### 3.1 Google Sign In

```
[Usuario] → Clica "Entrar com Google"
               │
               ▼
         [Google Identity Services]
         Retorna id_token
               │
               ▼
         [Nuxt] POST /api-proxy/v2/auth/google
               │
               ▼
         [.NET API] Valida id_token (audience match)
         Cria/vincula AutenticacaoExterna
         Retorna JWT + RefreshToken
```

---

## 4. Fluxo do Motor Financeiro (Cashback)

```
[Compra confirmada no Parceiro]
        │
        ▼
[Webhook Awin/Afilio/Zanox] → POST /api/v2/transacao/entrada
        │
        ▼
[CashbackDistribuicaoService]
        │
        ├─ Aplica split 50/25/25:
        │   ├─ 50% → Quanta Shop
        │   ├─ 25% → Consumidor (com 10% sustentabilidade)
        │   └─ 25% → Rede (residual multinível)
        │
        ├─ Percorre 12 níveis da rede:
        │   ├─ Verifica Licença HAF por nível
        │   ├─ Verifica inatividade (compressão dinâmica)
        │   └─ Aplica multiplicador Plus (2×) se ativo
        │
        └─ Gera Lancamentos para cada beneficiário
                │
                ▼
        [SQL Server] — Atômico (transação SERIALIZABLE)
```

---

## 5. Proxy Nitro (Camada de Segurança)

O servidor Nitro intercepta todas as chamadas `/api-proxy/*`:

```
[Browser] → GET /api-proxy/v2/partners/get-online-partners
                │
                ▼
        [Nitro Server Middleware]
        server/routes/api-proxy/[...path].ts
                │
                ├─ Dev: tenta localhost:8000/api
                │        └─ Falha? Fallback para api.quantashop.com.br
                │
                └─ Prod: direto para api.quantashop.com.br/api
                │
                ▼
        Rate Limiting (10 req/min/IP em endpoints de auth)
        Header de Authorization passado transparentemente
```

---

## 6. Componentes Reutilizáveis (Design System)

### 6.1 Componentes QS

| Componente | Arquivo | Uso |
|-----------|---------|-----|
| `QsHero` | `components/qs/QsHero.vue` | Hero fullscreen em páginas públicas |
| `QsKpiCard` | `components/qs/QsKpiCard.vue` | Indicadores no admin (currency/number/%) |
| `QsProgressBar` | `components/qs/QsProgressBar.vue` | Barras de progresso |
| `QsFilterChip` | `components/qs/QsFilterChip.vue` | Chips de filtro com contador |

### 6.2 Design Tokens (quanta-premium.scss)

```scss
// Cores principais
--qs-teal:       #2F7785;
--qs-teal-dark:  #225F6B;
--qs-lime:       #98C73A;
--qs-ink:        #1d1d1f;
--qs-bg:         #f5f5f7;

// Escala de cinza
--qs-gray-50:  #f9fafb;
--qs-gray-100: #e9ecef;
--qs-gray-300: #d1d5db;
--qs-gray-400: #6b7280;
--qs-gray-500: #4b5563;

// Estados
--qs-success: #22c55e;
--qs-warn:    #f59e0b;
--qs-danger:  #ef4444;

// Helpers de layout
.qs-page           { padding: 24px; max-width: 1280px; }
.qs-page-header    { display: flex; justify-content: space-between; }
.qs-grid           { display: grid; grid-template-columns: repeat(4, 1fr); }
.qs-card-section   { background: #fff; border-radius: 14px; padding: 24px; }
.qs-loading        { display: flex; justify-content: center; }
.qs-spinner        { width: 28px; height: 28px; border-radius: 50%; }
```

---

## 7. Estrutura de Diretórios Completa

```
/
├── pages/
│   ├── index.vue                    # Home V1
│   ├── para-voce.vue
│   ├── para-sua-empresa.vue
│   ├── seja-um-agente.vue
│   ├── quanta-amizade.vue
│   ├── busca-inteligente.vue
│   ├── blog/
│   │   ├── index.vue
│   │   └── [id].vue
│   ├── agencia/
│   │   ├── index.vue
│   │   ├── login.vue
│   │   ├── cadastro.vue
│   │   └── painel/
│   │       ├── index.vue
│   │       ├── financeiro.vue
│   │       ├── minha-rede.vue
│   │       └── admin/
│   │           ├── index.vue          # Dashboard operacional
│   │           ├── usuarios.vue
│   │           ├── pagamentos.vue
│   │           ├── lancamentos.vue
│   │           ├── relatorio-cashback.vue
│   │           ├── relatorio-de-anunciantes.vue
│   │           ├── relatorio-de-faturas.vue
│   │           ├── configuracoes-rede.vue
│   │           ├── bi-financeiro.vue
│   │           ├── features.vue
│   │           ├── progresso.vue
│   │           ├── docs.vue
│   │           └── blog.vue
│   └── lab/
│       ├── index.vue                  # Hub LAB (cockpit técnico)
│       └── flow-standard.vue          # Checklist FLOW DEVELOPMENT SYSTEMS
│
├── components/
│   ├── qs/
│   │   ├── QsHero.vue
│   │   ├── QsKpiCard.vue
│   │   ├── QsProgressBar.vue
│   │   └── QsFilterChip.vue
│   ├── home/
│   │   ├── home-hero.vue
│   │   ├── home-blog.vue
│   │   └── home-ofertas-dia.vue
│   ├── agencia/
│   │   └── AgenciaMenu.vue
│   ├── login/
│   │   └── login-social.vue
│   └── checkout/
│       └── checkout-verify.vue
│
├── layouts/
│   ├── layout-home.vue
│   ├── agencia-painel.vue
│   └── agencia.vue
│
├── pinia/
│   ├── useAgenciaStore.ts             # Auth + JWT
│   └── useCarouselStore.ts
│
├── composables/
│   ├── useApi.ts
│   ├── useAgenciaStore.ts
│   └── useHomeConfig.ts
│
├── server/
│   ├── routes/api-proxy/[...path].ts  # Proxy reverso
│   └── middleware/
│       └── verify-jwt.ts              # Validação JWT server-side
│
├── assets/scss/
│   ├── main.scss
│   ├── quanta-premium.scss            # Design system QS
│   └── agencia.scss
│
├── public/docs/                       # LAB — documentação técnica
│   ├── CLAUDE.md
│   ├── ARCHITECTURE.md               # Este arquivo
│   ├── DEPLOY.md
│   ├── FEATURES.md
│   ├── TESTING.md
│   ├── DATA_DICTIONARY.md
│   ├── DESIGN_SYSTEM.md
│   ├── CHANGELOG.md
│   ├── historia.md
│   └── features.json
│
└── nuxt.config.ts
```

---

## 8. Integrações Externas

| Serviço | Tipo | Uso |
|---------|------|-----|
| **Awin** | API REST | Cashback de parceiros online |
| **Afilio** | API REST | Cashback de parceiros online |
| **Zanox** | API REST | Cashback de parceiros (legado) |
| **Google Identity Services** | OAuth 2.0 | Login social |
| **Asaas** | API REST + Webhook | Assinatura Plus (recorrência) |
| **Azure SQL** | SQL Server | Banco de dados principal |
| **Replit** | PaaS | Hosting + deploy autoscale |

---

## 9. Decisões Arquiteturais Registradas

| # | Decisão | Motivo | Data |
|---|---------|--------|------|
| ADR-001 | Vue 3 + Nuxt 3 (migração de Vue 2) | SSR + melhor DX + ecosystem moderno | 2025-Q4 |
| ADR-002 | Tailwind limitado ao escopo `.v2-page` | Conflito com Bootstrap 5 global | Mar 2026 |
| ADR-003 | Blog: localStorage em dev | Evitar contaminação do banco de produção via LGPD | Abr 2026 |
| ADR-004 | Proxy Nitro para a API | Ocultar API key, centralizar CORS e auth | Mar 2026 |
| ADR-005 | Apple Sign In removido | Exige domínio customizado verificado + custo de dev | Mai 2026 |
| ADR-006 | BTC descontinuado em Saque | Quanta Shop não opera como corretora | Mai 2026 |
| ADR-007 | LAB em `/lab` separado do admin | Cockpit técnico ≠ painel operacional | Mai 2026 |
| ADR-008 | LGPD mascaramento client-side | Performance + auditoria server no endpoint de reveal | Mai 2026 |

---

*Atualizar sempre que uma nova decisão arquitetural for tomada ou um módulo significativo for adicionado.*
