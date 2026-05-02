# Quanta Shop Web

## 🧹 Sprint de Higiene 1.3.0 (Mai 2026 — Task #117) — Etapas 1-5 ✅

Eliminação dos 4 problemas crônicos de desenvolvimento que geravam ruído em cada sessão.

### Mudanças
- **`nuxt.config.ts`** — `imports.dirs` explicitado com `['pinia', 'composables', 'composables/**']` para estabilizar auto-import independente do `@nuxt/kit` embutido pelo `@nuxtjs/tailwindcss@6.14`.
- **`app.vue`** — `<VitePwaManifest />` removido (componente só existe quando o módulo PWA está ativo; o Nuxt emitia `Failed to resolve component: VitePwaManifest` em cada reload). `BottomSheetPWA` mantido (é nativo, independente de módulo).
- **`api/Directory.Build.props`** (novo) — supressão centralizada de CS1591, CA1416, NU1701. Warnings de segurança e lógica (NU1902/NU1903/CS1998/SYSLIB) continuam visíveis.
- **`api/Bigcash.sln`** — removidos 3 projetos fantasma (`LojasAwin`, `LojasAfilio`, `QSTestProject`) que causavam `Build FAILED` no `dotnet build`. `MMN.Tests` registrado corretamente.

### Etapa 5 — God classes .NET quebradas em partial classes
- **`AdminController`** (1584 linhas) → dividido em 8 partial class files: `AdminController.cs` (base), `.Relatorios.cs`, `.Blog.cs`, `.Banners.cs`, `.Usuarios.cs`, `.Rede.cs`, `.Credenciamento.cs`, `.Dashboard.cs`
- **`UsuarioController`** (1148 linhas) → dividido em 4 partial class files: `UsuarioController.cs` (base), `.Perfil.cs`, `.Financeiro.cs`, `.Rede.cs`
- **`CredenciamentoController`** mantido como está (lógica coesa de workflow multi-etapa)

### Métricas antes → depois
| | Antes | Depois |
|---|---|---|
| `manifest-route-rule` warnings/sessão | 449 | **0** |
| Browser `definePageMeta`/`useSticky` errors | recorrentes | **0** |
| `dotnet build` warnings | 1.553 | **~30 (reais)** |
| `dotnet build` erros | 3 | **0** |
| Vite warm up | ~9.7s | **~4.3s** |

### Pendente nesta task (#117)
- Etapa 6: Quebrar componentes Vue >600 linhas (carrosseis.vue 1090, home-cms.vue 942)
- Etapa 7: CI no GitHub Actions
- Etapa 8: Documentação final

---

## 💰 Wave 2 — Motor Financeiro + LGPD (Mai 2026 — Task #107) ✅

Implementação completa do motor de distribuição de cashback, busca inteligente, checkout com cupom + Quanta Points e LGPD reveal auditado.

### Backend (.NET 8 / `api/`)
- **`MMN.Negocio/Services/CashbackDistribuicaoService.cs`** — motor puro (sem DB) com 10% sustentabilidade + split 50/25/25 + 12 níveis residuais + compressão dinâmica + multiplicador Plus 2x. Tudo configurável via banco (chaves `Rede.*`).
- **`MMN.Tests/`** — projeto xUnit com 13 cenários cobrindo split, sustentabilidade, 12 níveis, compressão, Plus, validações.
- **`MMN.Dominio/Model/`** novas entidades: `Cupom`, `CupomUso`, `QuantaPontoLancamento`, `AuditoriaLgpd` (+ mappings + seeds + DbSets).
- **`MMN.Api/Controllers/v1/`** novos:
  - `SearchController` → `GET /busca-inteligente` (Haversine + filtros + sort).
  - `CupomController` → `POST /cupom/validar` (com fallback dev).
  - `QuantaPointsController` → `GET /usuario/quanta-points` + `POST .../resgatar` (transação atômica).
  - `AdminController` (extendido) → `GET/POST /admin/configuracoes-rede` com sustentabilidade + split base + 12 níveis; `POST /admin/revelar-dado-sensivel` gated por Master que loga `AuditoriaLgpd`.
- **`MMN.Util/Util/LgpdMask.cs`** — helpers `MaskCpfCnpj`/`MaskEmail`/`MaskTelefone`/`MaskConta`/`MaskAgencia`.

### Frontend (Nuxt)
- **`utils/lgpd-mask.ts`** — espelho client-side dos helpers de mask.
- **`pages/agencia/painel/admin/configuracoes-rede.vue`** — UI nova para sustentabilidade (slider 0-50%) e split base (3 inputs com badge de soma=100%). Persiste 12 níveis (era 5).
- **`pages/agencia/painel/admin/usuarios.vue`** — e-mail mascarado por padrão; botão **Revelar** aparece só para Master, chama `/admin/revelar-dado-sensivel` e exibe o cru no modal.
- **`pages/busca-inteligente.vue`** — consome `/busca-inteligente` real (mantém fallback mock).
- **`components/checkout/checkout-verify.vue`** — consome `/cupom/validar` e `/usuario/quanta-points` reais.

### Migration gerada (aplicar em produção)
- `20260502134039_Wave2_Cupom_QuantaPontos_AuditoriaLgpd.cs` — cria tabelas `Cupom`, `CupomUso`, `QuantaPontoLancamento`, `AuditoriaLgpd` + seeds QUANTA10/BEMVINDO + AddColumn em `Usuario` e `UsuarioProduto`.
- Comando para aplicar: `dotnet ef database update --project MMN.Repositorio --startup-project MMN.Api`

### xUnit — 13 testes passando
- `cd api && dotnet test MMN.Tests/MMN.Tests.csproj` → Passed: 13, Failed: 0

### Out of scope desta wave
- Apple Sign In (re-avaliar em wave futura).
- CRUD admin de cupons (admin gera apenas via seed/migration por ora).
- Criptografia em repouso de CPF.

---

## 🌊 Sprint Power Phase 1 (Mai 2026 — em andamento)

### Componentes premium (`components/qs/`)
- **`QsKpiCard.vue`** — card de indicador (label, valor, badge tonal, progresso opcional, delta com seta, formatos `currency`/`percent`/`number`).
- **`QsProgressBar.vue`** — barra linear com 3 tamanhos (`thin`/`md`/`thick`), cor customizável, label opcional.
- **`QsFilterChip.vue`** — chip de filtro com active/count/icon slot.

### Telas Sprint Power (Fase 1)
- **`pages/agencia/painel/admin/configuracoes-rede.vue`** — gestão de percentuais residuais por nível MLM, switch de compressão dinâmica, valor do Quanta Point, multiplicador Plus, quarentena, profundidade máxima. FAB bar de salvar com estado dirty.
- **`pages/agencia/painel/admin/bi-financeiro.vue`** — BI completo com switch Mês/Trimestre/Ano: KPI strip, faturamento por categoria, aging buckets, safras de cashback e top parceiros.
- **`pages/busca-inteligente.vue`** — motor de busca consumer com sliders de cashback mínimo e proximidade (km), geolocalização, filtros por chip, view grid/lista.

### Endpoints API implementados (Wave 2) ✅
- `GET/POST /admin/configuracoes-rede` ✅
- `GET /admin/bi-financeiro?periodo=month|quarter|year` ✅
- `GET /busca-inteligente?q=&minCashback=&maxDistance=&categoria=&sort=` ✅
- `POST /cupom/validar` ✅
- `GET /usuario/quanta-points` ✅
- `POST /usuario/quanta-points/resgatar` ✅
- `POST /admin/revelar-dado-sensivel` ✅

---

## 🚀 Modernização Premium — Power Mode (Mai 2026)

### Documentação técnica oficial criada na raiz
- `DESIGN_SYSTEM.md` — padrão visual premium minimalista
- `FEATURES.md` — catálogo de features por 4 fases de MVP e 3 públicos
- `DATA_DICTIONARY.md` — engenharia reversa das entidades `.NET 8` com marcação LGPD (🔒)
- `CHANGELOG.md` — histórico semver iniciado neste sprint

### Painel admin — Features & MVP
- `pages/agencia/painel/admin/features.vue` — nova tela com KPI strip, filtros, barras de progresso
- `public/docs/features.json` — fonte de dados (4 MVPs, 4 públicos, 25+ features)

---

## Documentação Técnica + Blog API Híbrido (Tasks #102 + #103 — COMPLETO)

### CLAUDE.md (raiz)
Arquivo de contexto técnico permanente criado em `/CLAUDE.md`. Documenta stack, estrutura, padrões, paleta, proxy, LGPD e histórico de tasks. Deve ser lido antes de qualquer modificação no projeto.

### Admin Docs (Task #102)
- `pages/agencia/painel/admin/docs.vue` — painel de documentação técnica com visualizador markdown, busca e download de PDF via `html2pdf.js`. Carrega `CLAUDE.md` automaticamente ao abrir.
- Card "📋 Documentação Técnica" adicionado ao admin index

### Blog Arquitetura Híbrida (Task #103)
**Solução implementada via `import.meta.dev`:**
| Ação | Dev (Replit) | Produção |
|------|-------------|---------|
| Admin escrita (POST/PUT/DELETE) | localStorage **apenas** | API real |
| Admin leitura (GET) | API → fallback localStorage | API real |
| Público leitura (GET) | API → fallback localStorage | API real |

## Blog Premium (Task #101 — COMPLETO + redesign Apple)

- `pages/blog/index.vue` — Listagem Apple-style com hero card, grid 3-col, newsletter dark.
- `pages/blog/[id].vue` — Detalhe com tipografia generosa, sidebar de relacionados.
- Admin: `pages/agencia/painel/admin/blog.vue` — upload base64, drag & drop.

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
- `services/` — Módulos de serviço de API (useApi() chamado dentro das funções, não no nível do módulo)
- `assets/` — Estilos CSS/SCSS e fontes
- `public/` — Assets estáticos (imagens, ícones, docs/*.md)
- `server/routes/` — Nitro server-side API proxy (`api-proxy/[...path].ts`)
- `api/` — API .NET 8 (MMN.Api) rodando na porta 8000

## API .NET 8

- Repositório clonado em `api/` (MMN.Api)
- Porta: 8000
- Workflow: "Start API" (`bash api/start-api.sh`)
- Banco de dados: **SQL Server (Azure)** — `bigcash.database.windows.net`
- Connection string: secret `SQL_CONNECTION_STRING`
- O proxy Nitro (`/api-proxy`) encaminha para `localhost:8000` quando `USE_LOCAL_API=true`

## Segurança da API (.NET)

- **JWT access token**: expira em 60 minutos
- **Refresh token**: expira em 30 dias
- **CORS**: restrito a domínios Quanta via `ALLOWED_ORIGINS`
- **Rate limiting**: 10 req/60s por IP nos endpoints de auth
- **Cookie de debug removido**: `oh_vida_oh_ceus` removido de `ExceptionHandler.cs`

## Auth Admin (Nuxt server routes)

- **JWT emitido pelo .NET** contém `ClaimTypes.Role` → `usuario.Grupo.Descricao` (ex: `"Admin"`)
- **`NUXT_JWT_SECRET`** deve bater com o `AppSettings.Secret` da API .NET
- **`server/middleware/verify-jwt.ts`**: extrai role claim via URI completa; se `"Admin"`, seta `event.context.user.admin = true`

## Bugs Conhecidos (Não Resolvidos)

- **EF Core 1-to-1 CredenciamentoMapping**: Carrega credenciamento errado para usuários com múltiplas linhas na tabela

## Deploy

### Scripts de produção
- **`build-prod.sh`** — Compila a API .NET e depois o Nuxt
- **`start-prod.sh`** — Inicia a API em background, aguarda health check, então inicia o Nuxt em foreground

### Variáveis de ambiente necessárias
- `SQL_CONNECTION_STRING` (secret Replit)
- `USE_LOCAL_API=true`
- `NUXT_API_BASE_URL=/api-proxy`
