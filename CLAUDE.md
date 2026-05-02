# CLAUDE.md — Quanta Shop Platform

> Arquivo de contexto técnico permanente. Leia antes de qualquer modificação no projeto.
> Última atualização: Mai 2026

---

## Identidade Estratégica do Produto

**Quanta Shop é uma Plataforma Moderna de Consumo Inteligente e Marketing de Fidelização com Cashback em Rede.**

O produto NÃO é uma plataforma financeira tradicional. NÃO segue padrões de banco, fintech ou carteira digital. O modelo de negócio central é:

- **Cashback em Rede** — o consumidor ganha cashback em cada compra e recebe Cashback Residual de toda a sua Rede Proprietária em múltiplos níveis
- **Rede Proprietária** — cada usuário constrói e expande sua própria rede de indicados; a rede é o principal ativo do usuário na plataforma
- **Social Commerce + Viralização** — o crescimento é orgânico, orientado por indicação e gamificação de recompensas
- **Afiliados Multinível** — motor de compensação MLM com até 12 níveis de profundidade e compressão dinâmica

> **Diretriz de Arquitetura e Design**: Toda a arquitetura, interface e lógica de desenvolvimento a partir de agora deve seguir os padrões premium das melhores plataformas de cashback e afiliados do mercado (focadas em alta conversão, engajamento gamificado e viralização via Social Commerce), e NÃO em padrões de aplicativos bancários tradicionais.

> **Coração do Engajamento**: O engajamento do usuário é construído ao redor da sua Rede Proprietária e da geração de renda passiva através de Cashback Residual, distribuído em múltiplos níveis da rede com compressão dinâmica para inativos.

---

## Documentação Oficial (leitura prioritária)

Antes de qualquer mudança significativa, consulte os documentos abaixo na raiz do projeto:

| Arquivo | Quando ler |
|---------|------------|
| **`DESIGN_SYSTEM.md`** | Toda mudança visual, novo componente ou tela admin |
| **`FEATURES.md`** | Antes de propor uma nova feature ou alterar escopo |
| **`STORIES.md`** | Para entender a jornada por persona (Maria/João/Carla) |
| **`DATA_DICTIONARY.md`** | Antes de tocar em entidade `.NET 8` ou expor dado de usuário (LGPD) |
| **`TESTING.md`** | Antes de mexer no algoritmo de compensação ou compressão dinâmica |
| **`CHANGELOG.md`** | Sempre atualizar ao final de um sprint ou PR significativo |
| **`replit.md`** | Histórico operacional do projeto, comandos, deploy |

---

## Ecossistema da Plataforma

O ecossistema inclui:
- **Portal do Consumidor** — encontra lojas parceiras, acumula cashback, expande sua rede, resgata recompensas
- **Painel do Agente de Fidelização** — back-office para Agentes gerenciarem sua rede proprietária, cashback residual e indicações
- **Painel Admin** — gestão completa da plataforma (usuários, comissões, blog, banners, relatórios, suporte)
- **API .NET 8** — backend com motor de compensação MLM, autenticação JWT, regras de cashback dinâmico e integrações de afiliados (Awin, Afilio)

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

# Testes do motor de compensação
dotnet test api/
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
│   │       ├── minha-rede.vue       # Visualização da rede proprietária
│   │       └── admin/               # Área exclusiva admin
│   │           ├── index.vue        # Dashboard admin
│   │           ├── blog.vue         # CRUD do blog
│   │           ├── home-cms.vue     # Textos da home
│   │           ├── carrosseis.vue   # Banners/carrosséis
│   │           ├── marcas-home.vue  # Logos do carrossel
│   │           ├── usuarios.vue
│   │           ├── credenciamento.vue
│   │           ├── relatorio-cashback.vue      # Relatório de cashback por período
│   │           ├── relatorio-de-faturas.vue    # Relatório de faturas (credenciados)
│   │           ├── relatorio-de-anunciantes.vue # Relatório de parceiros afiliados
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
│   ├── useAgenciaStore.ts           # Re-export via path relativo (usa pinia/useAgenciaStore)
│   └── useHomeConfig.ts             # Config dinâmica da home (CMS)
│
├── pinia/                           # Stores Pinia (escaneadas diretamente pelo Nuxt)
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
├── nuxt.config.ts                   # Configuração principal (imports.dirs: ['pinia'])
├── tailwind.config.ts               # Tailwind SCOPED (ver aviso abaixo)
└── CLAUDE.md                        # Este arquivo
```

---

## Padrão do Painel Admin

Toda página do admin segue este template obrigatório:

```vue
<script setup lang="ts">
import { useAgenciaStore } from '~/composables/useAgenciaStore';
import { useApi } from '~/composables/useApi';

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

> **IMPORTANTE**: Sempre use imports explícitos para `useAgenciaStore` e `useApi` nas páginas admin. O auto-import via alias `~/` em re-exports não é confiável no Nuxt 3 (unimport não resolve aliases em re-exports).

**Classes CSS do admin** (definidas em `assets/scss/agencia.scss`):
- `.ag-page-header` — cabeçalho da página com título e subtítulo
- `.ag-card` — card branco com sombra suave
- `.ag-table` — tabela estilizada
- `.ag-loading` — spinner centralizado
- `.ag-empty-state` — estado vazio
- `.badge-ag`, `.badge-ag-success`, `.badge-ag-warning`, `.badge-ag-danger` — badges coloridos
- `.btn-ag-primary`, `.btn-ag-outline` — botões do painel

---

## Endpoints da API — Referência Rápida (Admin)

| Funcionalidade | Método | Endpoint |
|---|---|---|
| Relatório cashback mensal | GET | `/Compra/relatorioMensalCashback?dataInicial=&datafinal=` |
| Relatório anunciantes/parceiros | POST | `/Admin/RelatorioAnunciantes` |
| Relatório faturas credenciados | POST | `/Admin/ObterFaturas` |
| Cadastros do mês | GET | `/Admin/RelatorioCadastrosMes` |
| Usuários (lista) | GET | `/Admin/...` |
| Saques admin | GET | `/Saque/listarSaquesAdmin` |

---

## Motor de Cashback e Rede (Wave 2)

O motor financeiro implementado inclui:
- **CashbackDistribuicaoService** — distribui cashback em até 12 níveis com compressão dinâmica (3 inativos consecutivos = pula)
- **Configurações editáveis no banco** — percentual de sustentabilidade, split base, multiplicadores por nível
- **QuantaPontos** — 1 ponto = R$ 1,00; acúmulo e resgate no checkout
- **Cupons** — validação e desconto no checkout
- **LGPD Mask** — CPF e dados bancários mascarados no admin; revelação apenas para perfil Master com log de auditoria

Tabelas adicionadas (migration `Wave2_Cupom_QuantaPontos_AuditoriaLgpd`):
- `Cupom`, `CupomUso`
- `QuantaPontoLancamento`
- `AuditoriaLgpd`

---

## Padrão do Blog

### Fluxo de dados (arquitetura híbrida — decisão de segurança)

```
Dev (Replit):
  ADMIN ESCRITA → localStorage apenas (isola do banco de produção)
  ADMIN LEITURA → API primeiro → fallback localStorage
  PÚBLICO LEITURA → API primeiro → fallback localStorage

Produção:
  ADMIN ESCRITA → API real (.NET 8 + SQL Server)
  PÚBLICO LEITURA → API real
```

**localStorage key:** `qs_blog_artigos`
**IDs em dev:** `Date.now()` (timestamp numérico grande, ex: `1775252000000`)
**Roteamento:** `pages/blog/index.vue` + `pages/blog/[id].vue`

> ⚠️ NUNCA criar `pages/blog.vue` — conflita com subrotas no Nuxt 3.

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

- Tokens gerenciados pelo `useAgenciaStore` (Pinia — pasta `pinia/`)
- `agenciaStore.getToken()` → retorna o JWT
- Rotas protegidas por middlewares:
  - `agencia-auth` → verifica login
  - `agencia-admin` → verifica `isAdmin`
- Tokens armazenados em `localStorage`
- O `nuxt.config.ts` inclui `imports: { dirs: ['pinia'] }` para auto-import direto da pasta `pinia/`

---

## Diretivas de Privacidade e Dados (LGPD)

A Quanta Shop coleta e processa dados pessoais de acordo com a LGPD:

1. **Dados sensíveis** (CPF, dados bancários) → apenas via API autenticada + HTTPS; mascarados no admin
2. **Revelação de dado sensível** → endpoint `POST Admin/RevelarDadoSensivel`, exclusivo para perfil `Master=true`, gera log de auditoria em `AuditoriaLgpd`
3. **Nunca** armazenar dados sensíveis em `localStorage` ou `sessionStorage`
4. **localStorage** aceito APENAS para: artigos do blog em dev, configurações de UI, cache não sensível
5. **Logs de auditoria** para operações de cashback e LGPD são responsabilidade da API .NET 8
6. **Banco de produção** não deve ser acessado pelo ambiente Replit/dev

---

## Convenções de Código

### Vue / Nuxt
- `<script setup lang="ts">` em todos os componentes
- `definePageMeta` sempre no início do script
- Imports explícitos de composables: `import { useApi } from '~/composables/useApi'`
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
- **#102**: Painel de documentação + CLAUDE.md
- **#103**: Conexão blog público à API real (híbrido dev/prod)
- **#107**: Wave 2 — Motor Financeiro (CashbackDistribuicaoService, QuantaPontos, Cupons, LGPD Mask, Busca Inteligente)
- **#116**: Corrigir auto-import `useAgenciaStore` no painel admin

---

*Este arquivo é mantido pelo agente Replit e deve ser atualizado a cada mudança arquitetônica significativa.*
