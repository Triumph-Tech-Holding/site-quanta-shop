# CHANGELOG — Quanta Shop

Todas as mudanças relevantes da plataforma. Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/) e versionamento [SemVer](https://semver.org/lang/pt-BR/).

---

## [1.3.0] — Mai 2026

### 📋 Flow Standard — Checklist Técnico Admin

#### Adicionado — Frontend (Nuxt)
- **`pages/lab/index.vue`** — **LAB · Cockpit Técnico Interno** (rota `/lab`, middleware `agencia-admin`): área interna de engenharia e governança visível somente a devs/gestores. Hub com 5 categorias (Engenharia, Produto, Arquitetura, Qualidade & Governança, Versionamento) totalizando 20 atalhos para Backlog, Sprints, Kanban, Commits, Stories, Progresso, Features, Fluxograma, CLAUDE.md, Documentação, Configurações de Rede, Flow Standard, Matriz E2E, Relatórios, BI Financeiro, CHANGELOG, FEATURES.md, features.json e DATA_DICTIONARY. KPIs no topo (progresso global, em curso, backlog, versão) carregados de features.json. Itens "em breve" sinalizados.
- **`pages/lab/flow-standard.vue`** — Sub-página do LAB que implementa o protocolo FLOW DEVELOPMENT SYSTEMS com 5 seções: §1 Introdução, §2 Camada de Contexto e Memória (CLAUDE.md, CHANGELOG.md, features.json, FEATURES.md com links diretos), §3 Gestão e Progresso (matriz de status com todas as features reais do projeto — filtrável por status e fase/MVP), §4 Setup Técnico Replit (configurações, secrets, deploy), §5 Qualidade e Código Limpo (tratamento de erros e DoD). KPIs de progresso no topo com QsKpiCard por fase. Segue padrão canônico bi-financeiro.vue.
- **`components/agencia/AgenciaMenu.vue`** — Adicionado divisor "LAB" e link "🧪 LAB · Cockpit Técnico" → `/lab` (apenas para admin).
- **`pages/agencia/painel/admin/index.vue`** — Atalho discreto para `/lab` no topo (pill "LAB"). Painel admin permanece operacional, sem misturar com o cockpit técnico.

---

### 🔒 Hardening de Segurança & Estabilidade (Tasks #30 e #116)

#### Adicionado / Corrigido — Frontend (Nuxt)
- **Erro 404 crítico nas rotas da Agência Digital (ADF) (`/agencia/painel/**`)** — corrigido adicionando scan explícito da pasta `pinia/` via `imports.dirs` em `nuxt.config.ts` e reescrevendo o re-export de `useAgenciaStore.ts` com path relativo.

#### Adicionado / Corrigido — Backend (.NET 8)
- **CORS dinâmico** implementado com `SetIsOriginAllowed` — elimina erros de preflight em origens não previstas em lista estática.
- **Middleware `DebugBypassGuard`** — bloqueia tentativas de debug bypass em produção com HTTP 403.
- **Rate limit de auth** corrigido para usar `Connection.RemoteIpAddress` — previne IP Spoofing via header `X-Forwarded-For` manipulado.

---

### 🔑 Autenticação, Tracking e BI (Sprint Power)

#### Adicionado / Corrigido
- **Login com Google (One Tap / GSI)** — fluxo unificado com arquitetura fail-closed (exige `GOOGLE_CLIENT_ID` válido no env). Nova página de fallback `/agencia/cadastro-google`.
- **ClickRef AWIN** — bugfix no tracking: ID injetado em tempo real por projeção, eliminando vazamento via cache compartilhado entre requisições.
- **Relatórios de BI Admin (Agência Digital — ADF)** — refatoração completa dos relatórios de Cashback, Faturas e Anunciantes:
  - Mapeados para endpoints reais: `GET /Compra/relatorioMensalCashback`, `POST /Admin/ObterFaturas` e equivalentes.
  - Adicionados filtros funcionais, totalizadores em tempo real e exportação limpa em PDF via `window.print()`.

---

### 🔧 Bugfixes de Sessão — Serviços e Parciais

#### Corrigido
- **`services/*.ts` (7 arquivos)** — `useApi()` era chamado no nível do módulo (linha 1, fora de qualquer função), quebrando o composable system do Nuxt 3 com `ReferenceError: useApi is not defined`. Corrigido movendo `useApi()` para dentro de cada função exportada em `product-service.ts`, `brand-service.ts`, `carousel-service.ts`, `category-service.ts`, `contact-service.ts`, `partner-service.ts` e `user-service.ts`.
- **`AdminController` + `UsuarioController` — partial class files** (8 arquivos) — `using` directives faltando quebravam o build do workflow: `AdminController.Relatorios.cs` (EF Core), `AdminController.Usuarios.cs` (Extensions), `UsuarioController.Perfil.cs` (MMN.Util.Util), `UsuarioController.Financeiro.cs` (MMN.Util.Enum). Todos corrigidos adicionando os namespaces ausentes.
- **`CupomController.ValidarFallback`** — não checava `MinimoPedido` quando a tabela `Cupom` estava ausente no Azure SQL (migration Wave2 pendente). Corrigido para checar valor mínimo mesmo no path fallback.

---

## [1.2.0] — Mai 2026

### 💰 Wave 2 — Motor Financeiro + LGPD (Task #107)

Implementação completa do motor de distribuição de cashback, busca inteligente, checkout com cupom + Quanta Points e máscara LGPD auditada.

#### Adicionado — Backend (.NET 8)
- **`MMN.Negocio/Services/CashbackDistribuicaoService.cs`** — motor puro com 10% sustentabilidade + split 50/25/25 + 12 níveis residuais + compressão dinâmica + multiplicador Plus 2x. Tudo configurável via banco (chaves `Rede.*`).
- **`MMN.Tests/`** — projeto xUnit com 13 cenários: split, sustentabilidade, 12 níveis, compressão, Plus, validações.
- **Novas entidades** em `MMN.Dominio/Model/`: `Cupom`, `CupomUso`, `QuantaPontoLancamento`, `AuditoriaLgpd` (+ mappings + seeds + DbSets).
- **`SearchController`** → `GET /busca-inteligente` com Haversine, filtros e sort.
- **`CupomController`** → `POST /cupom/validar` com fallback dev (seeds `QUANTA10`/`BEMVINDO`).
- **`QuantaPointsController`** → `GET /usuario/quanta-points` + `POST .../resgatar` (transação atômica SERIALIZABLE).
- **`AdminController`** (extendido) → `GET/POST /admin/configuracoes-rede` + `POST /admin/revelar-dado-sensivel` (gated por `Usuario.Master`, loga `AuditoriaLgpd`).
- **`MMN.Util/Util/LgpdMask.cs`** — helpers `MaskCpfCnpj`/`MaskEmail`/`MaskTelefone`/`MaskConta`/`MaskAgencia`.

#### Adicionado — Frontend (Nuxt)
- **`utils/lgpd-mask.ts`** — espelho client-side dos helpers de mask.
- **`pages/agencia/painel/admin/usuarios.vue`** — e-mail mascarado por padrão; botão **Revelar** só para Master.
- **`pages/busca-inteligente.vue`** — consome `/busca-inteligente` real (mantém fallback mock).
- **`components/checkout/checkout-verify.vue`** — consome `/cupom/validar` e `/usuario/quanta-points` reais.

#### Adicionado — Migration (aplicar em produção)
- `20260502134039_Wave2_Cupom_QuantaPontos_AuditoriaLgpd.cs` — cria tabelas `Cupom`, `CupomUso`, `QuantaPontoLancamento`, `AuditoriaLgpd` + seeds + AddColumn em `Usuario` e `UsuarioProduto`.
- Comando: `dotnet ef database update --project MMN.Repositorio --startup-project MMN.Api`

#### Atualizado
- **`public/docs/features.json`** v1.2.0 — adiciona F-213 (Máscara LGPD), marca F-304 (Residual de cashback) como `done`.

---

## [1.1.0] — Mai 2026

### 🧹 Sprint de Higiene 1.3.0 — Etapas 1-5 (Task #117)

#### Corrigido — Infraestrutura de desenvolvimento
- **`nuxt.config.ts`** — `imports.dirs` explicitado com `['pinia', 'composables', 'composables/**']`. Eliminou 449 `manifest-route-rule` warnings/sessão.
- **`app.vue`** — `<VitePwaManifest />` removido. Eliminou erros `Failed to resolve component` recorrentes no browser.
- **`api/Directory.Build.props`** (novo) — supressão centralizada de CS1591, CA1416, NU1701. Reduziu warnings do `dotnet build` de 1.553 para ~30 reais.
- **`api/Bigcash.sln`** — removidos 3 projetos fantasma (`LojasAwin`, `LojasAfilio`, `QSTestProject`) que causavam `Build FAILED`. `MMN.Tests` registrado corretamente.

#### Refatorado — Etapa 5: God classes .NET quebradas em partial classes
- **`AdminController`** (1584 linhas) → 8 partial class files: base + `.Relatorios`, `.Blog`, `.Banners`, `.Usuarios`, `.Rede`, `.Credenciamento`, `.Dashboard`.
- **`UsuarioController`** (1148 linhas) → 4 partial class files: base + `.Perfil`, `.Financeiro`, `.Rede`.

### 🌊 Sprint Power Phase 1 — Componentização premium + telas de modernização

#### Adicionado — Componentes premium reutilizáveis
- **`components/qs/QsKpiCard.vue`** — card de indicador com label, valor (formatos `currency` / `percent` / `number`), badge tonal (`success`/`warn`/`danger`), barra de progresso opcional, delta com seta e meta.
- **`components/qs/QsProgressBar.vue`** — barra linear com 3 tamanhos (`thin`/`md`/`thick`), cor customizável, label opcional, transição suave.
- **`components/qs/QsFilterChip.vue`** — chip de filtro com estado active, contador opcional, slot de ícone.

#### Adicionado — Telas Sprint Power (Fase 1)
- **`pages/agencia/painel/admin/configuracoes-rede.vue`** — gestão centralizada da rede MLM com 12 níveis residuais, compressão dinâmica, Quanta Points, multiplicador Plus, quarentena e profundidade máxima. FAB bar de salvar com estado dirty.
- **`pages/agencia/painel/admin/bi-financeiro.vue`** — BI completo com switch Mês/Trimestre/Ano, KPI strip, faturamento por categoria, aging buckets, safras de cashback e top parceiros.
- **`pages/busca-inteligente.vue`** — motor de busca consumer com sliders de cashback mínimo e proximidade, geolocalização, filtros por chip, view grid/lista.

#### Adicionado — Componentes consumer
- **`components/login/login-social.vue`** — Google funcional via GIS; Apple Sign In preparado (aguarda `appleClientId` em produção).
- **`components/checkout/checkout-verify.vue`** — 3 painéis colapsáveis: login retornante, cupom de desconto, Quanta Points com slider de resgate.

#### Atualizado — Tokens & Helpers
- **`assets/scss/quanta-premium.scss`** — aliases semânticos `--qs-teal`, `--qs-teal-dark`, `--qs-bg`, `--qs-ink`, escala `--qs-gray-50…700`, estados `--qs-success/warn/danger`, helpers `.qs-page`, `.qs-btn-primary/outline`, `.qs-loading/spinner`.
- **`pages/agencia/painel/admin/features.vue`** — refatorado para `QsKpiCard`/`QsProgressBar`/`QsFilterChip` e helpers `.qs-page`.
- **`public/docs/features.json`** v1.1.0 — adiciona F-208 a F-212, marca F-205 e F-207 como `done`.

---

## [Sprint 102+103] — Mai 2026

### Adicionado
- `pages/agencia/painel/admin/docs.vue` — painel de documentação técnica com visualizador markdown, busca e download de PDF via `html2pdf.js`.
- `CLAUDE.md` na raiz do projeto, como contexto técnico permanente.
- Arquitetura híbrida do blog dev/prod (`import.meta.dev`): em dev, escritas vão apenas para `localStorage` para não contaminar o banco de produção (LGPD).

### Corrigido
- Conflito de roteamento Nuxt 3: `pages/blog.vue` renomeado para `pages/blog/index.vue` (evita conflito com `pages/blog/[id].vue`).

### Removido
- `pages/blog-list.vue`, `pages/blog-grid.vue`, `pages/blog-details-2.vue`, `pages/blog-details/` — stubs legados.

---

## [Sprint 80–101] — Abr 2026

### Adicionado
- Componente `QsHero.vue` — hero fullscreen reutilizável com foto real, overlay teal, watermark, pill badge, CTAs e floating glass cards.
- `assets/scss/quanta-premium.scss` — sistema de design premium com tokens CSS, classes `.qs-*` (sections, cards, grids), animações, scrollbar customizada.
- Páginas redesenhadas com `QsHero`: `para-voce.vue`, `para-sua-empresa.vue`, `seja-um-agente.vue`, `quanta-amizade.vue`, `contato.vue`.
- Blog premium com listagem Apple-style (hero card, grid 3-col, newsletter dark) e detalhe com tipografia generosa (`pages/blog/index.vue`, `pages/blog/[id].vue`).
- Admin do blog com upload de imagem (base64, drag & drop).

---

## [Sprint 33] — Mar 2026

### Adicionado
- Nova home V1 (`pages/index.vue` + `layout-home`) reimplementada a partir do mockup Lovable: hero, brand logos animados, ofertas do dia, parceiros online, parceiros locais, testimonials, blog, CEO Mauro Triumph, footer-CTA e footer dark.
- Header limpo (`header-home.vue`) com nav, search bar, login/cadastro.

### Alterado
- `axios timeout` aumentado para 30s (alinhado com proxy Nitro 20s + margem).

---

## [Sprint 1–24] — 2025/Q4 e Mar 2026

### Inicial
- Setup Nuxt 3.8.1 + Vue 3 + TypeScript.
- Migração Vue 2 → Nuxt 3 das páginas de agência.
- Migração React 18 → Vue 3 do módulo `primeira-compra`.
- Integração com API .NET 8 via proxy Nitro (`/api-proxy`).
- Autenticação JWT com middleware `agencia-auth` e `agencia-admin`.
- Sistema de roles via claim `ClaimTypes.Role` no JWT decodificado em `server/middleware/verify-jwt.ts`.
- Rate limiting nos endpoints de auth (10 req/60s).
- CORS restrito a domínios Quanta.
- Deploy autoscale Replit com `build-prod.sh` + `start-prod.sh`.

---

*Cada PR significativo deve adicionar uma entrada nesta lista em "Não publicado". Releases são consolidados por data e versão SemVer.*
