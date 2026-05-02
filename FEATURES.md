# FEATURES.md — Quanta Shop

> Mapeamento das funcionalidades da plataforma por **público** e por **fase de MVP**.
> Versão 1.0 — Mai 2026

---

## Públicos da Plataforma

A Quanta Shop atende três públicos distintos. Cada um tem necessidades, métricas de sucesso e jornadas próprias.

| Público | Persona | Métrica-chave |
|---------|---------|---------------|
| 🛍️ **Consumidor** | Pessoa física que compra em parceiros e acumula cashback | Cashback acumulado / mês |
| 🏪 **Lojista / Parceiro** | Empresa que oferece cashback aos clientes | Faturamento mensal via plataforma |
| 🤝 **Agente de Fidelização** | Empreendedor que constrói rede de indicações | Renda residual mensal |

---

## Fases de MVP

A entrega é dividida em **4 fases progressivas**, cada uma desbloqueando uma camada de valor.

| Fase | Foco | Status | % Concluído |
|------|------|--------|-------------|
| **MVP 1 — Fundação** | Cashback básico, autenticação, catálogo público | 🟢 Em produção | 100% |
| **MVP 2 — Premium UI** | Redesign completo, experiência minimalista, blog | 🟡 Em andamento | 75% |
| **MVP 3 — Rede e Compensação** | Plano de carreira, Quanta Amizade, residual binário | 🔵 Planejado | 20% |
| **MVP 4 — Inteligência** | Recomendações com IA, BI para lojista, app mobile | ⚪ Backlog | 0% |

---

## Catálogo de Features

Cada feature tem `id`, `audience`, `mvp`, `status` e `tasks` relacionadas. A fonte de verdade é `public/docs/features.json` (consumido pelo painel `/agencia/painel/admin/features`).

---

### MVP 1 — Fundação (concluído)

#### F-101 · Cadastro e autenticação JWT
**Público:** Todos · **Status:** ✅ Done
- Cadastro com CPF, e-mail e celular validados
- Login social Google
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

### MVP 2 — Premium UI (em andamento)

#### F-201 · Home V1 com hero premium
**Público:** Todos · **Status:** ✅ Done
- Hero com Swiper, floating cards, brand strip
- Seções: ofertas, online, locais, blog, CEO, footer-CTA
- Vídeo do CEO (Mauro Triumph) com WhatsApp CTA

#### F-202 · Páginas de público (Para Você / Empresa / Agente / Quanta Amizade)
**Público:** Todos · **Status:** ✅ Done
- Hero fullscreen com foto real, badge, stats
- Padrão `QsHero` reutilizável
- Layout `layout-home`

#### F-203 · Blog premium (Apple-style)
**Público:** Todos · **Status:** ✅ Done
- Listagem com hero card, grid 3-col, newsletter dark
- Detalhe com tipografia generosa, sidebar de relacionados
- CRUD admin com upload de imagem (base64)
- Arquitetura híbrida dev/prod com `import.meta.dev`

#### F-204 · Painel admin redesenhado
**Público:** Admin · **Status:** 🟡 In Progress
- Header limpo, KPIs no topo
- Tabelas leves, filter chips, badges semânticas
- Documentação técnica integrada (`/admin/docs`)
- **Pendente:** unificar telas legadas (relatórios, lançamentos)

#### F-205 · Sistema de design tokens
**Público:** Interno · **Status:** 🟡 In Progress
- `quanta-premium.scss` com tokens `--qs-*`
- `DESIGN_SYSTEM.md` formalizado (este sprint)
- **Pendente:** componentes `QsKpiCard`, `QsProgressBar`, `QsFilterChip` extraídos

#### F-206 · Painel de Progresso (Q Cuida-style)
**Público:** Admin · **Status:** ✅ Done
- Lista de tarefas com checkmarks coloridos
- Barra de progresso linear no topo
- Filtros: Todas / Concluídas / Ativas / Rascunhos

#### F-207 · Painel de Features e MVP (este documento + tela)
**Público:** Admin · **Status:** 🟡 In Progress
- Visualização de features por MVP
- Progresso por fase em tempo real
- Drill-down em estórias de usuário

---

### MVP 3 — Rede e Compensação (planejado)

#### F-301 · Plano de carreira (graduações)
**Público:** Agente · **Status:** 🔵 Planned
- 8 níveis (Bronze → Diamante Negro)
- Requisitos: vendas pessoais + volume de equipe + ativação de diretos
- Bonificações por promoção
- Histórico de graduações na timeline

#### F-302 · Rede MLM binária visual
**Público:** Agente / Admin · **Status:** 🔵 Planned
- Árvore interativa com zoom (recharts ou d3)
- Posição binária (esquerda / direita)
- Indicador de pernas balanceadas
- Cache de resumo binário (já existe na API)

#### F-303 · Quanta Amizade — programa de indicações
**Público:** Consumidor · **Status:** 🟡 In Progress (50%)
- Convite por link único
- Recompensa para padrinho e afilhado quando o objetivo é atingido
- Histórico em `quanta-amizade.vue`
- **Pendente:** notificações push de progresso

#### F-304 · Residual de cashback (compressão dinâmica)
**Público:** Agente · **Status:** 🔵 Planned
- Algoritmo de compressão: pula upline inativo
- Percentuais por nível (até 5 níveis profundo)
- Cálculo em batch noturno + on-demand
- Tabela `PercentualResidualCashback` já existe

#### F-305 · Assinatura Plus
**Público:** Consumidor · **Status:** 🔵 Planned
- Plano mensal recorrente (Asaas)
- Benefícios: cashback dobrado, suporte prioritário
- Gestão em `/agencia/painel/assinatura`
- **Pendente:** integração webhook Asaas

#### F-306 · Saque BTC + PIX
**Público:** Consumidor / Agente · **Status:** 🟡 In Progress
- PIX já implementado
- BTC: campo `EnderecoBTC` na entidade `Saque` (legado)
- **Decisão pendente:** descontinuar BTC ou modernizar

---

### MVP 4 — Inteligência (backlog)

#### F-401 · Recomendações personalizadas
**Público:** Consumidor · **Status:** ⚪ Backlog
- Modelo collaborative filtering com histórico de compras
- "Parceiros que você ainda não usou" na home

#### F-402 · BI para lojista
**Público:** Lojista · **Status:** ⚪ Backlog
- Dashboard de faturamento, ticket médio, recompra
- Cohort de clientes ativados via Quanta

#### F-403 · App mobile nativo
**Público:** Consumidor / Agente · **Status:** ⚪ Backlog
- React Native ou Expo
- Push notifications, geolocalização para parceiros locais
- Carteira com QR code para pagamento offline

#### F-404 · IA conversacional (assistente Quanta)
**Público:** Todos · **Status:** ⚪ Backlog
- Chat com perguntas sobre saldo, indicações, parceiros
- Integração WhatsApp Business

---

## Critério de "Done"

Uma feature só é marcada como **✅ Done** quando:

1. ✅ Implementada no frontend e backend
2. ✅ Testada (unitária + E2E quando aplicável — ver `TESTING.md`)
3. ✅ Documentada em `replit.md` ou `CLAUDE.md`
4. ✅ Acessível em produção
5. ✅ Conforme com LGPD (ver `DATA_DICTIONARY.md` para campos sensíveis)
6. ✅ Aderente ao `DESIGN_SYSTEM.md`

---

## Como Atualizar

1. Edite `public/docs/features.json` com a nova feature ou mudança de status.
2. Atualize este `FEATURES.md` com a descrição humana.
3. O painel `/agencia/painel/admin/features` reflete automaticamente.

---

*Mantido pela equipe de produto. Cada PR que adiciona/altera feature deve atualizar este arquivo.*
