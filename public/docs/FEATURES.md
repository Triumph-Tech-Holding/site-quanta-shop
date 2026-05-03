# FEATURES.md — Quanta Shop

> Mapeamento das funcionalidades da plataforma por **público** e por **fase de MVP**.
> Versão 1.3.0 — Mai 2026

---

## Públicos da Plataforma

| Público | Descrição | Métrica-chave |
|---------|-----------|---------------|
| 🛍️ **Consumidor** | Pessoa que compra em lojas parceiras e ganha cashback. | Cashback acumulado / mês |
| 🤝 **Agente de Fidelização** | Empreendedor com Licença HAF que constrói redes e ganha bônus residuais. | Renda residual mensal |
| 🏪 **Empresa Parceira (ZEE DIGITAL)** | Empresa que oferece cashback e constrói rede. | Faturamento mensal via plataforma |
| 🔧 **Interno (Dev/Admin)** | Equipe técnica que usa o LAB para governança do app. | Progresso do roadmap |

---

## Fases de MVP

| Fase | Foco | Status | % Concluído |
|------|------|--------|-------------|
| **MVP 1 — Fundação** | Cashback básico, autenticação, catálogo público | 🟢 Em produção | 100% |
| **MVP 2 — Premium UI** | Redesign completo, experiência minimalista, blog, LAB | ✅ Concluído | 100% |
| **MVP 3 — Rede e Compensação** | Plano de carreira, Quanta Amizade, residual binário | 🔵 Em andamento | ~40% |
| **MVP 4 — Inteligência** | Recomendações com IA, BI para lojista, app mobile | ⚪ Backlog | 0% |

---

## Catálogo de Features

A fonte de verdade é `public/docs/features.json` — consumida pelo painel `/agencia/painel/admin/features` e pelo LAB `/lab`.

---

### MVP 1 — Fundação (✅ concluído)

#### F-101 · Cadastro e autenticação JWT
**Público:** Todos · **Status:** ✅ Done
- Cadastro com CPF, e-mail e celular validados
- Login social Google (GIS)
- Recuperação de senha com token expirável
- JWT 60min + refresh 30 dias

#### F-102 · Catálogo de parceiros online
**Público:** Consumidor · **Status:** ✅ Done
- Listagem com filtro por categoria
- Cards com cashback %, logo, ativação 1 clique
- Integração Awin / Afilio / Zanox

#### F-103 · Carteira de saldo
**Público:** Consumidor · **Status:** ✅ Done
- Saldo principal e bloqueado
- Histórico de lançamentos
- Solicitação de saque com taxa configurável

#### F-104 · Painel financeiro do agente
**Público:** Agente · **Status:** ✅ Done
- Comissões diretas e residuais
- Extrato detalhado por período
- Cadastro de conta bancária

#### F-105 · Credenciamento de parceiros
**Público:** Lojista · **Status:** ✅ Done
- Formulário multi-etapa (dados, endereço, ecossistema)
- Aprovação manual por admin
- E-mail de confirmação

---

### MVP 2 — Premium UI (✅ concluído — Mai 2026)

#### F-201 · Home V1 com hero premium
**Público:** Todos · **Status:** ✅ Done
- Hero com Swiper, floating cards, brand strip
- Seções: ofertas, online, locais, blog, CEO, footer-CTA
- Vídeo do CEO (Mauro Triumph) com WhatsApp CTA

#### F-202 · Páginas de público
**Público:** Todos · **Status:** ✅ Done
- Para Você / Para Sua Empresa / Seja um Agente / Quanta Amizade
- Hero fullscreen com foto real, badge, stats — padrão `QsHero`
- Layout `layout-home`

#### F-203 · Blog premium (Apple-style)
**Público:** Todos · **Status:** ✅ Done
- Listagem com hero card, grid 3-col, newsletter dark
- Detalhe com tipografia generosa, sidebar de relacionados
- CRUD admin com upload de imagem (base64)
- Arquitetura híbrida dev/prod com `import.meta.dev`

#### F-204 · Painel admin redesenhado
**Público:** Admin · **Status:** ✅ Done
- Cabeçalhos padronizados `.qs-page-header` + eyebrow contextual
- KPIs com `QsKpiCard` (currency/number/percent) em strip 3–4 colunas
- `QsFilterChip` para filtros de categoria, tipo e período
- **LGPD:** Login e Nome mascarados; reveal exclusivo para Master com auditoria
- Telas: `lancamentos`, `relatorio-cashback`, `relatorio-de-anunciantes`, `relatorio-de-faturas`

#### F-205 · Sistema de design tokens
**Público:** Interno · **Status:** ✅ Done
- `quanta-premium.scss` com tokens `--qs-*` e aliases semânticos
- `DESIGN_SYSTEM.md` formalizado
- Componentes `QsKpiCard`, `QsProgressBar`, `QsFilterChip` extraídos

#### F-206 · Painel de Progresso
**Público:** Admin · **Status:** ✅ Done
- Lista de tarefas com checkmarks coloridos e barra linear no topo
- Filtros: Todas / Concluídas / Ativas / Rascunhos

#### F-207 · Painel de Features e MVP
**Público:** Admin · **Status:** ✅ Done
- Visualização de features por MVP com progresso em tempo real
- Drill-down em estórias de usuário
- Refatorado para `QsKpiCard` / `QsProgressBar` / `QsFilterChip`

#### F-208 · Configurações de Rede
**Público:** Admin · **Status:** ✅ Done
- Tabela de 12 percentuais residuais por nível com toggle
- Switch de compressão dinâmica
- Quanta Points (R$/ponto), multiplicador Plus, quarentena, profundidade máxima
- Endpoint `GET/POST /admin/configuracoes-rede`

#### F-209 · BI Financeiro
**Público:** Admin · **Status:** ✅ Done
- Switch Mês/Trimestre/Ano, KPI strip (faturamento, cashback, inadimplência, margem)
- Faturamento por categoria (barras), aging buckets coloridos por risco
- Safras de cashback + top parceiros
- Endpoint `GET /admin/bi-financeiro?periodo=month|quarter|year`

#### F-210 · Busca Inteligente
**Público:** Consumidor · **Status:** ✅ Done
- Filtros minCashback / maxDistance (Haversine sobre lat/lng)
- Filtro por categoria, ordenação cashback/distance/popular/price, view grid/lista
- Endpoint `GET /busca-inteligente`

#### F-211 · Login social (Google)
**Público:** Todos · **Status:** ✅ Done
- Google funcional via GIS no consumidor e na agência
- Apple Sign In removido do escopo (reavaliação futura)

#### F-212 · Cupom + Quanta Points no checkout
**Público:** Consumidor · **Status:** ✅ Done
- Campo de cupom com validação `/cupom/validar` (seeds `QUANTA10`/`BEMVINDO`)
- Slider de resgate de Quanta Points com débito atômico SERIALIZABLE

#### F-213 · Máscara LGPD + reveal auditado
**Público:** Admin · **Status:** ✅ Done
- Helper `LgpdMask` (CPF/CNPJ, e-mail, telefone, conta, agência)
- Endpoint `POST /admin/revelar-dado-sensivel` gated por `Master=true`
- Audit trail em `AuditoriaLgpd`

#### F-214 · LAB — Cockpit Técnico Interno
**Público:** Interno (Dev/Admin) · **Status:** ✅ Done
- Hub em `/lab` com middleware `agencia-auth + agencia-admin`
- 5 categorias: Engenharia, Produto, Arquitetura, Qualidade & Governança, Versionamento
- ~20 atalhos para toda documentação e ferramentas técnicas
- Sub-página `/lab/flow-standard` — checklist FLOW DEVELOPMENT SYSTEMS (5 seções)
- KPIs de progresso carregados em tempo real de `features.json`

---

### MVP 3 — Rede e Compensação (🔵 em andamento)

#### F-301 · Plano de carreira (graduações)
**Público:** Agente · **Status:** 🔵 Planned
- 8 níveis (Bronze → Diamante Negro)
- Requisitos: vendas pessoais + volume de equipe + ativação de diretos
- Bonificações por promoção e histórico de graduações

#### F-302 · Rede MLM binária visual
**Público:** Agente / Admin · **Status:** 🔵 Planned
- Árvore interativa com zoom (recharts ou d3)
- Posição binária (esquerda/direita), indicador de pernas balanceadas
- `CacheResumoBinario` já existe na API

#### F-303 · Quanta Amizade — programa de indicações
**Público:** Consumidor · **Status:** 🟡 In Progress (50%)
- Convite por link único, recompensa para padrinho e afilhado
- Histórico em `quanta-amizade.vue`
- **Pendente:** notificações push de progresso

#### F-304 · Residual de cashback (compressão dinâmica)
**Público:** Agente · **Status:** ✅ Done
- `CashbackDistribuicaoService`: split 50/25/25 + 10% sustentabilidade
- 12 níveis residuais + compressão dinâmica + multiplicador Plus 2×
- Cobertura xUnit completa (13 cenários)

#### F-305 · Assinatura Plus
**Público:** Todos · **Status:** 🔵 Planned (15%)
- Plano mensal recorrente (Asaas) — upgrade transversal
- Cashback Residual Dobrado ao atingir meta de consumo mínimo
- Meta configurável pelo Admin (`metaConsumoPlus`) — nunca hardcoded
- Multiplicador 2× aplica-se EXCLUSIVAMENTE ao bônus residual
- **Pendente:** integração webhook Asaas

#### F-306 · Saque PIX
**Público:** Consumidor / Agente · **Status:** 🟡 In Progress (60%)
- PIX implementado; fluxo de confirmação e validações em finalização
- BTC descontinuado (Quanta Shop não opera como corretora)

#### F-307 · Material de apoio (marketing kit)
**Público:** Agente · **Status:** 🟡 In Progress (40%)
- Cards, vídeos, copy prontos para o agente baixar e divulgar
- `material-apoio.vue` existe; **pendente:** CMS para upload pelo admin

#### F-308 · Promoções por horário/dia
**Público:** Lojista · **Status:** 🔵 Planned
- Cashback variável (happy hour, dias específicos)
- Tabelas `Promocao` + `AnuncianteCashBack` já existem na API

---

### MVP 4 — Inteligência (⚪ backlog)

#### F-401 · Recomendações personalizadas (IA)
**Público:** Consumidor · **Status:** ⚪ Backlog
- Collaborative filtering com histórico de compras
- "Parceiros que você ainda não usou" na home

#### F-402 · BI para lojista
**Público:** Lojista · **Status:** ⚪ Backlog
- Dashboard de faturamento, ticket médio, recompra
- Cohort de clientes ativados via Quanta

#### F-403 · App mobile nativo
**Público:** Consumidor / Agente · **Status:** ⚪ Backlog
- React Native ou Expo
- Push notifications, geolocalização, carteira com QR code

#### F-404 · IA conversacional (assistente Quanta)
**Público:** Todos · **Status:** ⚪ Backlog
- Chat com perguntas sobre saldo, indicações, parceiros
- Integração WhatsApp Business

---

## Critério de "Done"

Uma feature só é marcada como **✅ Done** quando:

1. ✅ Implementada no frontend (Nuxt 3) e backend (.NET 8)
2. ✅ Testada conforme `TESTING.md` (unitária + E2E quando aplicável)
3. ✅ Documentada em `CLAUDE.md` e/ou `ARCHITECTURE.md`
4. ✅ Acessível em produção (ou protegida por middleware adequado)
5. ✅ Conforme com LGPD (ver `DATA_DICTIONARY.md`)
6. ✅ Aderente ao `DESIGN_SYSTEM.md` (classes `qs-*`)
7. ✅ CHANGELOG.md atualizado

---

## Como Atualizar

1. Edite `public/docs/features.json` com a nova feature ou mudança de status.
2. Atualize este `FEATURES.md` com a descrição humana.
3. O painel `/agencia/painel/admin/features` e o `/lab` refletem automaticamente.

---

*Mantido pela equipe de produto e engenharia. Cada PR que adiciona/altera feature deve atualizar este arquivo.*
