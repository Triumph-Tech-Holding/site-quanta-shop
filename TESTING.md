# TESTING.md — Quanta Shop

> Plano de testes da plataforma, com **foco rigoroso no algoritmo de compensação** e na **compressão dinâmica** da rede MLM.
> Versão 1.0 — Mai 2026

---

## Pirâmide de Testes

```
        ▲
       ╱ ╲   E2E (Playwright/Cypress)  ← 10%
      ╱___╲
     ╱     ╲  Integração (.NET + DB de teste) ← 30%
    ╱_______╲
   ╱         ╲ Unit (xUnit / Vitest)  ← 60%
  ╱___________╲
```

| Camada | Ferramenta | Onde |
|--------|-----------|------|
| Unit (.NET) | **xUnit** + **Moq** | `api/MMN.Negocio.Tests/` (a criar) |
| Unit (Front) | **Vitest** + **@vue/test-utils** | `tests/unit/` |
| Integração | **xUnit + WebApplicationFactory** + SQLite in-memory | `api/MMN.Api.Tests/` |
| E2E | **Playwright** | `tests/e2e/` |
| Carga | **k6** | `tests/load/` |

---

## 1. Plano de Testes — Algoritmo de Compensação

A correção do plano de carreira é **inegociável**. Erro aqui = perda financeira ou disputa jurídica.

### 1.1 Cenários canônicos (obrigatórios)

#### CT-COMP-01 — Cashback simples sem rede
- **Dado:** consumidor sem patrocinador faz compra de R$ 100 com 5% cashback.
- **Quando:** transação confirmada.
- **Então:** lançamento crédito de R$ 5,00, único, sem residual.

#### CT-COMP-02 — Cashback com 1 nível de upline ativo
- **Dado:** A patrocina B; B compra R$ 100; A está ativo (compra última 30d).
- **Quando:** transação confirmada.
- **Então:** B recebe R$ 5,00 (cashback); A recebe residual de N% (conforme `PercentualResidualCashback` para nível 1).

#### CT-COMP-03 — Compressão dinâmica com upline inativo
- **Dado:** A → B → C; B inativo; C compra R$ 100.
- **Quando:** transação confirmada.
- **Então:**
  - C recebe cashback direto.
  - B é **pulado** (inativo).
  - A recebe residual do **nível em que B estaria**, NÃO do nível seguinte. (Compressão: pula sem deslocar percentual.)

#### CT-COMP-04 — Compressão dinâmica em cadeia
- **Dado:** A → B → C → D; B e C inativos; D compra R$ 100.
- **Então:** A recebe residual do nível 1 (não nível 3).

#### CT-COMP-05 — Bloqueio de quarentena
- **Dado:** lançamento gerado.
- **Quando:** dentro de período de quarentena (típico 30 dias).
- **Então:** `Lancamento.Bloqueado = true`, não disponível para saque.

#### CT-COMP-06 — Estorno de pedido
- **Dado:** pedido pago, cashback distribuído.
- **Quando:** pedido cancelado.
- **Então:** lançamentos de débito espelham os créditos originais; saldo retorna ao estado anterior; auditoria em `AnuncianteCashBackLog`.

#### CT-COMP-07 — Limite de profundidade
- **Dado:** rede com 10 níveis ativos.
- **Quando:** compra na ponta.
- **Então:** apenas N níveis configurados em `PercentualResidualCashback` recebem residual; níveis acima do limite **não** recebem.

#### CT-COMP-08 — Plus dobra cashback
- **Dado:** consumidor assinante Plus ativo.
- **Quando:** compra de R$ 100 com 5% cashback.
- **Então:** lançamento direto de R$ 10,00 (2x); residual upline calculado sobre o valor **original** R$ 5,00 (não dobra residual).

#### CT-COMP-09 — Graduação dispara bônus
- **Dado:** agente atinge requisitos da próxima graduação.
- **Quando:** batch noturno roda.
- **Então:** `IdGraduacao` atualizada, `HistoricoGraduacao` registrado, lançamento de bônus em `Lancamento`, mensagem em `MensagemGraduacao`.

#### CT-COMP-10 — Concorrência (compras simultâneas)
- **Dado:** 50 compras processadas em paralelo no mesmo segundo.
- **Quando:** finalizadas.
- **Então:** todas geram lançamentos corretos; nenhum lançamento duplicado; `IdTransacao` único; soma de saldos confere.

### 1.2 Property-based tests (sugerido — FsCheck/Bogus)

```csharp
// Para qualquer rede aleatória de até 5 níveis e qualquer compra > 0:
// soma(lançamentos gerados) <= valor da compra.
[Property]
public void TotalCommissionsNeverExceedsPurchaseValue(NetworkTree tree, decimal purchase) { ... }
```

### 1.3 Snapshot tests (regressão de cálculo)

Salvar um JSON-snapshot por cenário. Mudanças no cálculo precisam atualizar o snapshot conscientemente (PR review obrigatório).

---

## 2. Plano de Testes — Compressão Dinâmica

A compressão é a regra mais sutil do MLM. Todos os cenários abaixo são **bloqueantes** para release de qualquer mudança em `RedeNegocio` / `LancamentoNegocio`.

| Cenário | Configuração da rede | Inativos | Resultado esperado |
|---------|---------------------|----------|---------------------|
| CT-COMPR-01 | A→B→C | nenhum | A: nível 2, B: nível 1, C: cashback |
| CT-COMPR-02 | A→B→C | B | A: nível 1 (pulou B), C: cashback |
| CT-COMPR-03 | A→B→C→D | B, C | A: nível 1, D: cashback |
| CT-COMPR-04 | A→B→C→D | A, B | C: nível 1, D: cashback |
| CT-COMPR-05 | A→B→C→D→E→F | B, D | A: nível 2, C: nível 1, E: nível 1, F: cashback |
| CT-COMPR-06 | A→B (B é Master) | — | Master nunca recebe residual de própria rede (regra de governança) |
| CT-COMPR-07 | Rede circular detectada | — | Lança exceção `CircularNetworkException`, log em `Acesso` |

**Definição de "inativo":** `Usuario.Ativo = false` **OU** `DataUltimoAcesso < hoje - 30 dias` **OU** sem compra própria nos últimos 90 dias. (Ajustar conforme regra atual de negócio.)

---

## 3. Testes Frontend (Vitest)

### 3.1 Unit — composables
- `useApi.ts` — fallback proxy → API direta com timeout 30s.
- `useAgenciaStore.ts` — `getToken()` retorna null sem login; `isAdmin` true só quando `user.admin === true`.
- `useHomeConfig.ts` — merge de defaults com overrides de CMS.

### 3.2 Component — críticos
- `QsHero.vue` — renderiza badge, título, CTAs, floating badges.
- `home-ofertas-dia.vue` — degrada para mock quando API falha.
- `pages/agencia/painel/admin/features.vue` — calcula % de progresso por MVP correto.

### 3.3 Snapshot dos painéis admin
Garantir que mudanças visuais sejam intencionais.

---

## 4. Testes E2E (Playwright)

### Fluxos críticos (smoke obrigatório por release)

| Fluxo | Passos |
|-------|--------|
| **E2E-01** Login agente | `/agencia/login` → preencher → painel `/agencia/painel` carrega |
| **E2E-02** Cadastro consumidor | Home → cadastro → confirmar e-mail (mock) → login |
| **E2E-03** Solicitar saque | Login → financeiro → solicitar R$ 50 → ver pendente |
| **E2E-04** Admin aprovar saque | Login admin → `/admin/pagamentos` → aprovar → status mudou |
| **E2E-05** Blog público lê do localStorage em dev | Admin cria artigo → `/blog` lista o artigo |
| **E2E-06** Indicação Quanta Amizade | A convida B → B se cadastra com link → A vê em `quanta-amizade` |

---

## 5. Testes de Carga (k6)

Endpoint mais crítico: `POST /api/v2/usuario/login`.

Meta: **100 req/s sustentadas, p95 < 500ms**, sem erros 5xx.

```javascript
// tests/load/login.k6.js
export const options = {
  stages: [
    { duration: '30s', target: 50 },
    { duration: '2m', target: 100 },
    { duration: '30s', target: 0 },
  ],
  thresholds: { http_req_duration: ['p(95)<500'] }
};
```

Outros endpoints prioritários:
- `GET /api/v2/awin-feed/get-products` (cache hit > 90% esperado)
- `POST /api/v2/saque/solicitar` (validar rate-limiting)

---

## 6. Cobertura Mínima

| Camada | Cobertura mínima |
|--------|------------------|
| `MMN.Negocio` | **85%** |
| `MMN.Repositorio` | 70% |
| Composables | 80% |
| Pages críticas (admin) | 60% |

CI quebra abaixo do mínimo.

---

## 7. Ambiente de Teste

- **DB:** SQLite in-memory para unit; Postgres em container Docker para integração.
- **Seeds:** scripts em `api/MMN.Api.Tests/Seeds/` recriam grupos, percentuais, usuário admin/teste.
- **Mocks externos:** Asaas, Awin, Afilio, Zanox via WireMock.
- **JWT secret de teste:** fixo em `appsettings.Test.json` — nunca compartilhado com produção.

---

## 8. Segurança (testes obrigatórios)

| Teste | Validação |
|-------|-----------|
| SQL Injection | Tentativa de payload em login, busca, filtros — todos parametrizados |
| XSS | Markdown do blog renderizado sem `<script>` ativo |
| CSRF | Requisições admin sem cookie/JWT são rejeitadas (401) |
| Rate limit | Exceder 10 req/60s no login retorna 429 |
| JWT expirado | Retorna 401 com mensagem `Token expired` |
| JWT manipulado | Retorna 401 sem mensagem detalhada |
| LGPD endpoints | `GET /usuario/{id}` só retorna dados do próprio usuário ou admin |

---

## 9. Definition of Done — Tester

Uma feature **só passa para "Done"** quando:

1. ✅ Todos os testes unitários da camada relevante passam.
2. ✅ Cobertura não diminuiu.
3. ✅ Smoke E2E passou no ambiente de staging.
4. ✅ Logs sem erro 500 nas últimas 24h em staging.
5. ✅ Validação manual em pelo menos 1 navegador desktop + 1 mobile.

---

## 10. Roadmap de Implementação dos Testes

| Sprint | Entrega |
|--------|---------|
| **Sprint atual** | Estrutura de pastas + 3 cenários canônicos do CT-COMP |
| **+1** | CT-COMP-01 a CT-COMP-10 implementados |
| **+2** | CT-COMPR-01 a CT-COMPR-07 implementados |
| **+3** | E2E-01 a E2E-06 + k6 base |
| **+4** | Property tests + snapshots completos |

---

*Mantido pelo time de QA. Qualquer alteração no algoritmo de compensação exige PR com testes novos cobrindo o caso adicionado.*
