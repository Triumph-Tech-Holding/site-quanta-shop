# STORIES.md — Quanta Shop

> Histórias de usuário organizadas por público e fase de MVP.
> Formato: **Como** [persona], **quero** [ação], **para que** [valor].
> Versão 1.0 — Mai 2026

---

## Personas Detalhadas

### 🛍️ Maria — Consumidora (32 anos, professora)
- Mora em Belo Horizonte, faz compras online toda semana.
- Já usou outros sites de cashback (Méliuz, Ame), mas perdeu confiança por demora no resgate.
- **Dor:** Quer cashback rápido, sem letras miúdas, e um lugar para acompanhar quanto economizou no mês.

### 🏪 João — Lojista (45 anos, dono de restaurante local)
- Tem um restaurante de bairro com 50% dos clientes recorrentes.
- Não entende plataformas digitais e desconfia de "marketing multinível".
- **Dor:** Quer **mais clientes na porta** sem investir em mídia paga e com risco controlado (paga só por venda).

### 🤝 Carla — Agente (38 anos, consultora de beleza)
- Já trabalha com vendas diretas (Natura) e busca renda complementar.
- Tem rede de 200 contatos no WhatsApp.
- **Dor:** Quer ferramenta profissional que não a faça parecer "amadora", com material de divulgação pronto e relatórios claros.

---

## MVP 1 — Fundação (Done)

### Consumidor

- **US-101** · Como **Maria**, quero **me cadastrar com CPF e e-mail**, para que eu **comece a acumular cashback rapidamente**.
  - ✅ Implementada em `pages/agencia/cadastro.vue`
  - **Critério:** cadastro completo em < 90 segundos

- **US-102** · Como **Maria**, quero **ver lojas parceiras com o cashback em destaque**, para que eu **decida onde comprar primeiro**.
  - ✅ `pages/index.vue` + `home/home-ofertas-dia.vue`

- **US-103** · Como **Maria**, quero **clicar em "ativar cashback" e ser redirecionada para a loja**, para que **minha compra seja rastreada automaticamente**.
  - ✅ Integração Awin/Afilio/Zanox

- **US-104** · Como **Maria**, quero **solicitar saque do meu saldo via PIX**, para que **eu receba o dinheiro no mesmo dia**.
  - ✅ `pages/agencia/painel/financeiro.vue` + entidade `Saque`

### Lojista

- **US-105** · Como **João**, quero **me credenciar fornecendo CNPJ e dados do estabelecimento**, para que **minha loja apareça no catálogo**.
  - ✅ `pages/agencia/finalizar-credenciamento/[id].vue`

### Agente

- **US-106** · Como **Carla**, quero **ter um link de indicação único**, para que **eu ganhe comissão sobre cada cadastro feito a partir dele**.
  - ✅ Hierarquia `IdUsuarioPai` em `Usuario`

---

## MVP 2 — Premium UI (em andamento)

### Consumidor

- **US-201** · Como **Maria**, quero **uma home page bonita e clara**, para que **eu sinta confiança em deixar meu dinheiro circulando aqui**.
  - ✅ Done · `pages/index.vue` redesenhado

- **US-202** · Como **Maria**, quero **ler artigos do blog sobre como economizar**, para que **eu aprenda a usar melhor a plataforma**.
  - ✅ Done · `/blog`

- **US-203** · Como **Maria**, quero **navegar pelas páginas "Para Você" / "Quanta Amizade"**, para que **eu entenda o que ganho como consumidora**.
  - ✅ Done · páginas com `QsHero`

### Lojista

- **US-204** · Como **João**, quero **uma página "Para Sua Empresa" que explique benefícios e custos**, para que **eu decida se vale me cadastrar**.
  - ✅ Done · `pages/para-sua-empresa.vue`

### Agente

- **US-205** · Como **Carla**, quero **um painel admin minimalista, sem ruído visual**, para que **eu encontre rapidamente o que preciso fazer hoje**.
  - 🟡 In Progress · 50%

- **US-206** · Como **Carla** (admin), quero **ver o progresso do produto numa tela só**, para que **eu acompanhe entregas sem perguntar para o time**.
  - ✅ Done · `/admin/progresso`

- **US-207** · Como **Carla** (admin), quero **ver as features do MVP organizadas por fase**, para que **eu saiba o que está pronto e o que vem por aí**.
  - 🟡 In Progress · `/admin/features` (este sprint)

---

## MVP 3 — Rede e Compensação (planejado)

### Consumidor

- **US-301** · Como **Maria**, quero **convidar amigas pelo Quanta Amizade e ganhar bônus quando elas comprarem**, para que **minha economia escale junto com a delas**.
  - 🔵 Planned · 50% (entidade `QuantaAmizade` existe, falta UI completa)

- **US-302** · Como **Maria**, quero **assinar o Plus por R$ 19/mês e ter cashback dobrado**, para que **eu maximize meus ganhos sem mudar meu hábito**.
  - 🔵 Planned · F-305

### Lojista

- **US-303** · Como **João**, quero **definir cashback diferente por horário ou dia**, para que **eu use a plataforma como ferramenta de ocupação de mesas vazias**.
  - 🔵 Planned · `Promocao` + `AnuncianteCashBack` já existem

- **US-304** · Como **João**, quero **ver quantos clientes vieram pela Quanta no mês**, para que **eu saiba o ROI da plataforma**.
  - 🔵 Planned · F-402 (BI Lojista, MVP 4)

### Agente

- **US-305** · Como **Carla**, quero **ver minha rede em árvore visual com posição binária**, para que **eu saiba onde colocar o próximo cadastro**.
  - 🔵 Planned · F-302

- **US-306** · Como **Carla**, quero **acompanhar minha graduação atual e o que falta para subir**, para que **eu me motive a bater meta**.
  - 🔵 Planned · F-301

- **US-307** · Como **Carla**, quero **receber comissão residual mesmo se um direto fica inativo (compressão dinâmica)**, para que **minha renda não seja punida pela inatividade alheia**.
  - 🔵 Planned · F-304

- **US-308** · Como **Carla**, quero **baixar materiais de marketing prontos (cards, vídeos, copy)**, para que **eu divulgue de forma profissional**.
  - 🟡 In Progress · `pages/agencia/painel/material-apoio.vue`

---

## MVP 4 — Inteligência (backlog)

### Consumidor

- **US-401** · Como **Maria**, quero **receber recomendações de lojas que combinam com meu perfil de compra**, para que **eu descubra parceiros novos sem garimpar**.
  - ⚪ Backlog · F-401

- **US-402** · Como **Maria**, quero **um app no celular com notificações de cashback liberado**, para que **eu acompanhe sem precisar abrir o navegador**.
  - ⚪ Backlog · F-403

### Lojista

- **US-403** · Como **João**, quero **dashboard com faturamento, ticket médio e cohort de recompra**, para que **eu use os dados para promover ações**.
  - ⚪ Backlog · F-402

### Agente

- **US-404** · Como **Carla**, quero **um assistente IA que responda dúvidas dos meus indicados no WhatsApp**, para que **eu não precise responder a mesma pergunta 50 vezes**.
  - ⚪ Backlog · F-404

---

## Estados das Stories

| Estado | Significado |
|--------|-------------|
| ✅ Done | Implementada em produção |
| 🟡 In Progress | Em desenvolvimento ativo |
| 🔵 Planned | Especificada, aguardando MVP |
| ⚪ Backlog | Mapeada mas sem priorização |

---

## Mapeamento Stories ↔ Features

Cada story tem uma feature pai (`F-XYZ`). Use o painel `/agencia/painel/admin/features` para ver o mapeamento completo e o progresso percentual por MVP.

---

*Atualize sempre que uma story mudar de estado ou for adicionada.*
