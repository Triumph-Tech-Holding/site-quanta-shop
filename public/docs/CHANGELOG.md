# CHANGELOG — Quanta Shop

Todas as mudanças relevantes da plataforma. Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/) e versionamento [SemVer](https://semver.org/lang/pt-BR/).

---

## [Não publicado] — Mai 2026

### 🌊 Sprint Power Phase 1 — Componentização premium + telas de modernização

Componentes do Premium Design System extraídos como SFCs reutilizáveis e construção das primeiras telas da Fase 1 do roadmap (Sprint Power) sobre os novos tokens.

#### Adicionado — Componentes premium reutilizáveis
- **`components/qs/QsKpiCard.vue`** — card de indicador com label, valor (formatos `currency` / `percent` / `number`), badge tonal (`success`/`warn`/`danger`), barra de progresso opcional, delta com seta e meta.
- **`components/qs/QsProgressBar.vue`** — barra linear com 3 tamanhos (`thin`/`md`/`thick`), cor customizável, label opcional, transição suave.
- **`components/qs/QsFilterChip.vue`** — chip de filtro com estado active, contador opcional, slot de ícone.

#### Adicionado — Telas Sprint Power (Fase 1)
- **`pages/agencia/painel/admin/configuracoes-rede.vue`** — Gestão Centralizada da rede MLM. Tabela de percentuais residuais por nível com toggle de ativação, switch de compressão dinâmica, configuração de Quanta Points (R$/ponto), multiplicador Plus, quarentena, profundidade máxima e tabela separada para bônus de credenciamento. FAB bar de salvar com estado dirty. Fallback de mock quando o endpoint `/admin/configuracoes-rede` ainda não existe.
- **`pages/agencia/painel/admin/bi-financeiro.vue`** — BI completo com switch Mês/Trimestre/Ano. KPI strip (faturamento, cashback reservado, inadimplência, margem), barras horizontais por categoria de receita, aging buckets coloridos por risco, tabela de safras de cashback com colunas gerado/estornado/liberado/a-pagar e top parceiros. Mock dataset por período enquanto `/admin/bi-financeiro` não existe.
- **`pages/busca-inteligente.vue`** — Motor de busca premium para o consumidor. Search bar gigante, sugestões rápidas, sliders de cashback mínimo (lime) e proximidade (km), botão "Usar minha localização" via `navigator.geolocation`, filtros de categoria por chip, ordenação multi-critério, view grid/lista, badges de cashback e cálculo de cashback retornado por preço.

#### Adicionado — Componentes consumer
- **`components/login/login-social.vue`** redesenhado: Google (já funcional via GIS) + **Apple Sign In** ativo com SDK on-demand (`appleid.cdn-apple.com`), aguarda `runtimeConfig.public.appleClientId` para entrar em produção.
- **`components/checkout/checkout-verify.vue`** redesenhado em PT-BR com 3 painéis colapsáveis: login retornante, **cupom de desconto** (validação `/cupom/validar` com fallback dev — códigos `QUANTA10` e `BEMVINDO`) e **Quanta Points** (saldo + slider de resgate com conversão em R$). Emite `coupon-applied` e `points-applied` para o checkout-area integrar.

#### Atualizado — Tokens & Helpers
- **`assets/scss/quanta-premium.scss`** ganhou aliases semânticos do `DESIGN_SYSTEM.md`: `--qs-teal`, `--qs-teal-dark`, `--qs-bg`, `--qs-ink`, escala `--qs-gray-50…700`, estados `--qs-success/warn/danger`, easings `--qs-ease`/`--qs-duration`. Também adiciona helpers `.qs-page`, `.qs-page-header`, `.qs-eyebrow`, `.qs-grid`, `.qs-card-section`, `.qs-section-title/desc`, `.qs-btn-primary/outline`, `.qs-loading/spinner`.
- **`pages/agencia/painel/admin/features.vue`** refatorado: remove tokens inline, agora consome `QsKpiCard`/`QsProgressBar`/`QsFilterChip` e helpers `.qs-page`. Reduz duplicação e padroniza visual.
- **`components/agencia/AgenciaMenu.vue`** + **`pages/agencia/painel/admin/index.vue`** — links e cards para as duas novas telas admin.
- **`public/docs/features.json`** — versão `1.1.0`. Adiciona F-208 (Configurações de Rede), F-209 (BI Financeiro), F-210 (Busca Inteligente), F-211 (Login social), F-212 (Cupom + Quanta Points). Marca F-205 (Design tokens) e F-207 (Painel features) como `done`.

---

### 🚀 Modernização Premium — Power Mode iniciada

Esta entrada marca o início oficial da **modernização premium** da plataforma Quanta Shop, com foco em padronização visual, documentação técnica formal e visualização de progresso integrada ao painel admin.

### Adicionado

- 📘 **`DESIGN_SYSTEM.md`** — Padrão visual premium e minimalista oficializado. Define paleta restrita (`#2F7785`, `#225F6B`, `#98C73A`, `#F4F4F5`, `#1d1d1f`), tipografia Inter com escala mobile-first, sistema de espaçamento 4px, tokens CSS `--qs-*`, componentes admin canônicos (KPI Card, Progress Bar, Filter Chips, List Item) e anti-padrões proibidos.
- 📋 **`FEATURES.md`** — Catálogo formal de features divididas em 4 fases de MVP (Fundação, Premium UI, Rede & Compensação, Inteligência), com status (Done / In Progress / Planned / Backlog) por público (Consumidor, Lojista, Agente).
- 👥 **`STORIES.md`** — User stories no formato canônico ("Como X, quero Y, para que Z"), com 3 personas detalhadas (Maria/Consumidora, João/Lojista, Carla/Agente) e mapeamento bidirecional com features.
- 🗄️ **`DATA_DICTIONARY.md`** — Dicionário de dados completo com engenharia reversa das entidades `.NET 8` em `api/MMN.Dominio/Model/`. Marca explicitamente campos sensíveis LGPD (🔒) com recomendações de mascaramento, retenção e anonimização. Cobre Usuário, Transação, Pedido, Saque, Lançamento, hierarquia MLM (QuantaAmizade, Graduação, CacheResumoBinário) e operacional.
- 🧪 **`TESTING.md`** — Plano de testes rigoroso com pirâmide xUnit + Vitest + Playwright. Foco em **10 cenários canônicos** do algoritmo de compensação (CT-COMP-01 a CT-COMP-10) e **7 cenários de compressão dinâmica** (CT-COMPR-01 a CT-COMPR-07), incluindo property-based testing, snapshot tests, k6 para carga e checklist LGPD.
- 📊 **`public/docs/features.json`** — Fonte de dados das features para o painel admin (consumido pela tela `/admin/features`).
- 🖥️ **`pages/agencia/painel/admin/features.vue`** — Nova tela do escritório virtual: visualização de FEATURES + STORIES + progresso de MVP em tempo real, no padrão **Q Cuida** (lista vertical com checkmarks, barras lineares, filtros por chip), totalmente aderente ao Premium Design System recém-criado.
- 🔗 Card **"🎯 Features & MVP"** no admin index e link **"📊 Features & MVP"** em `AgenciaMenu.vue`.

### Atualizado

- 📄 **`CLAUDE.md`** — Adicionada referência aos novos documentos técnicos como leitura obrigatória; nota sobre ordem de prioridade dos arquivos de contexto.
- 📓 **`replit.md`** — Histórico das modificações desta entrada do changelog.

---

## [Sprint 102+103] — Mai 2026

### Adicionado
- `pages/agencia/painel/admin/docs.vue` — painel de documentação técnica integrada com 9 seções colapsáveis e botão "Baixar PDF".
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
