# AUDIT_REPORT.md — Sprint de Auditoria, Performance e Refatoração

> Diagnóstico completo + correções aplicadas + débito técnico priorizado.
> Sprint: Mai 2026 — Pausa em features novas (Social Commerce suspenso).
> Stack auditada: **Frontend Nuxt 3** + **Backend .NET 8** (SQL Server Azure)

---

## 1. Sumário Executivo

| Categoria | Achados | Corrigido nesta sprint | Documentado para próxima sprint |
|---|---:|---:|---:|
| Bugs com falha silenciosa | 7 | **5** | 2 |
| Imports duplicados / configuração | 1 | **1** | 0 |
| Console.log esquecidos | 2 | **2** | 0 |
| N+1 queries no banco | 4 | 0 | 4 (precisa testes) |
| Chamadas síncronas / `.Wait()` bloqueante | 5 | 0 | 5 (precisa testes) |
| Lógica de negócio em controller | 3 | 0 | 3 (refactor grande) |
| Componentes gigantes (>500 linhas) | 6 | 0 | 6 (quebra em sub-componentes) |
| Lógica financeira no frontend | 5 | 0 | 5 (mover para backend) |
| Padrão repetido (oportunidade de DRY) | 3 | **1 (utility nova)** | 2 (componente compartilhado) |
| **TOTAL** | **36** | **9** | **27** |

> **Por que não corrigir tudo agora?** A diretriz foi "garantir que nossa base está sólida" — corrigir N+1 queries e refatorar god-controllers sem cobertura de testes é mais arriscado do que conviver com eles documentados. O `TESTING.md` define a cobertura mínima (xUnit 85% em `MMN.Negocio`); até esses testes existirem, refactor cego pode introduzir bugs financeiros.

---

## 2. Bugs Corrigidos Nesta Sprint

### 2.1 [BUG-001] Import duplicado de `useAgenciaStore` quebrava auto-import do Nuxt
- **Onde**: `composables/useAgenciaStore.ts` (shim re-exportando de `pinia/`)
- **Sintoma**: Warning recorrente `Duplicated imports "useAgenciaStore"` no startup. O Nuxt scaneava `composables/` e `pinia/` e encontrava dois símbolos com o mesmo nome → o do composables era ignorado em runtime, causando comportamento imprevisível em re-hidratação de páginas.
- **Causa raiz**: a config `imports.dirs: ['pinia']` no `nuxt.config.ts` (adicionada na Task #116) já fazia o auto-import; o shim virou ruído.
- **Correção**: Shim deletado. Os 6 arquivos que importavam de `~/composables/useAgenciaStore` foram repointados para `~/pinia/useAgenciaStore` via `sed`:
  - `layouts/agencia-painel.vue`
  - `middleware/agencia-auth.ts`, `middleware/agencia-admin.ts`
  - `components/agencia/AgenciaMenu.vue`
  - `pages/agencia/painel/admin/relatorio-cashback.vue`
  - `pages/agencia/painel/admin/relatorio-de-anunciantes.vue`
  - `pages/agencia/painel/admin/relatorio-de-faturas.vue`

### 2.2 [BUG-002] Falha silenciosa em `QuantaPointsController.ObterSaldo`
- **Onde**: `api/MMN.Api/Controllers/v1/QuantaPointsController.cs:39-42` e `:54`
- **Sintoma**: Se a tabela `QuantaPontoLancamento` não existir (ambiente sem migration Wave 2), o `catch{}` zerava silenciosamente o saldo. O mesmo acontecia ao consultar a configuração `Rede.QuantaPontoValor`.
- **Risco**: Usuário podia ver saldo 0 sem qualquer pista de que a tabela está faltando, e equipe DevOps não tinha sinal para rodar a migration.
- **Correção**: Mantido o fallback (saldo=0, valorPonto=1.0) mas agora os dois `catch` registram a exceção via `Console.Error.WriteLine` com prefixo `[QuantaPoints]` e o id do usuário, deixando rastro nos logs sem expor o erro ao cliente.

### 2.3 [BUG-003] Falha silenciosa no `FaturaService` (loop de credenciados)
- **Onde**: `api/MMN.Api/Services/FaturaService.cs:123-126`
- **Sintoma**: `catch (Exception) { }` vazio dentro do loop `foreach (var comerciante in comerciantes)`. Se uma fatura individual falhasse, o serviço noturno seguia adiante **sem nenhum registro** — credenciados ficavam sem fatura sem ninguém saber.
- **Correção**: Catch agora chama `_logger.LogError` com a mensagem da exception e o `IdUsuario` do credenciado afetado.

### 2.4 [BUG-004] Falha silenciosa no `AdiantamentoCashback` (loop de pedidos)
- **Onde**: `api/MMN.Api/Services/AdiantamentoCashback.cs:147-150`
- **Sintoma**: `catch { }` vazio dentro do loop de cashback. Cashbacks que falhavam ao serem lançados desapareciam — exatamente o tipo de erro que causa "lentidão e erros" reportados pelo usuário, porque o pedido fica preso em status intermediário.
- **Correção**: Catch tipado (`catch (Exception ex)`) com `Console.Error.WriteLine` incluindo `IdPedido` e mensagem da exception (incluindo `InnerException`).

### 2.5 [BUG-005] Falha silenciosa no Dashboard Admin
- **Onde**: `pages/agencia/painel/admin/index.vue:85`
- **Sintoma**: `catch { /**/ }` engolia erros do endpoint `/admin/painel/resumo`. Se o endpoint falhasse, os 4 cards de KPI ficavam com `—` para sempre, sem aviso.
- **Correção**: Catch agora registra `console.error` com prefixo identificável `[admin/index]`.

### 2.6 [BUG-006] `console.log` em `cart-area.vue` (com `console.log(couponCode.value)`)
- **Onde**: `components/cart/cart-area.vue:97`
- **Correção**: Substituído por TODO documentado apontando para o endpoint `POST /Cupom/validar` (Wave 2). Adicionada guarda contra string vazia para não enviar requisição inválida no futuro.

### 2.7 [BUG-007] `console.log(e)` em `checkout-billing.vue`
- **Onde**: `components/checkout/checkout-billing.vue:92`
- **Correção**: Removido o log; parâmetros prefixados com `_` para indicar handler não implementado (Vue 3 + TS lint friendly).

### 2.8 [BUG-008] `Console.WriteLine("Stopped")` no `StopAsync` do FaturaService
- **Onde**: `api/MMN.Api/Services/FaturaService.cs:60-67`
- **Sintoma**: `Task.Factory.StartNew` desnecessário só pra imprimir "Stopped" — poluição de logs.
- **Correção**: Substituído por `Task.CompletedTask`.

### 2.9 [REFACTOR-001] Novo composable `useApiError` (DRY)
- **Onde criado**: `composables/useApiError.ts`
- **Por quê**: Os explorers identificaram que cada página admin replicava o seu próprio `extractApiErrorMessage` ad-hoc. Centralizar em um composable resolve:
  - `axios.isAxiosError` → extrai `data.erros[0].mensagem`, `data.message`, `data.detail`
  - Mensagens pt-BR específicas para 401/403/429/5xx e timeout
  - Fallback configurável
- **Como usar**:
  ```ts
  import { extractApiError } from '~/composables/useApiError';
  try { await api.post(...) } catch (e) { erro.value = extractApiError(e); }
  ```
- Migração das páginas admin existentes ficou para a próxima sprint (refatoração mecânica, baixo risco).

---

## 3. Gargalos de Lentidão Identificados (NÃO corrigidos — exigem testes antes)

### 3.1 N+1 Queries no Banco (alto impacto)

| Arquivo | Linha | Padrão problemático | Estimativa de impacto |
|---|---:|---|---|
| `api/MMN.Api/Services/Awin.cs` | 116 | `foreach (transaction)` faz `Pedido.Any` + `Pedido.Include(...).FirstOrDefault` + `PedidoDetalhe.Any` + `Anunciante.FirstOrDefault` por iteração | **CRÍTICO** — pode disparar 4-5 round-trips × N transações Awin (centenas/dia) |
| `api/MMN.Api/Services/FaturaService.cs` | 114 | `foreach (comerciante)` chama `CriarFaturaCashbackCredenciadoAsync` (que internamente faz lookups por credenciado) | **ALTO** — roda no batch noturno do dia 2; trava o servidor por minutos |
| `api/MMN.Api/Services/AdiantamentoCashback.cs` | 136 | `foreach (idPedido)` chama `LancarCashback(idPedido)` que faz `.Include(...)` no `Pedido` | **ALTO** — roda de hora em hora |
| `api/MMN.Negocio/Negocio/PedidoNegocio.cs` | 165 | `foreach (parcela in pedidoBanco.Pagamentos)` chama `_pagamentoNegocio.FirstNoTracking` quando `Pagamentos` já está em memória | **MÉDIO** — query desnecessária por parcela |

**Recomendação**: Pré-carregar com `.Include(...)` agrupado, ou acumular IDs e fazer um único `.Where(x => ids.Contains(x.Id))`. Antes de mexer, escrever os testes CT-COMP-* descritos no `TESTING.md`.

### 3.2 Bloqueio de Thread (`.Wait()` em código async)

| Arquivo | Linha | Problema |
|---|---:|---|
| `api/MMN.Api/Services/Awin.cs` | 80 | `ListarTransacoesAsync(689359).Wait()` bloqueia o pool de threads do ASP.NET — risco real de **deadlock** sob carga |
| `api/MMN.Api/Services/FaturaService.cs` | 76 | `CriarFaturasAsync().Wait()` no `TimerTick` |
| `api/MMN.Api/Services/AdiantamentoCashback.cs` | 145 | `_lastTask.Wait()` dentro do loop de pedidos |
| `api/MMN.Api/Controllers/v1/AdminController.cs` | 117 | `FiltrarUsuarios(filtroUsuario)` é síncrona em endpoint que poderia ser async |
| `api/MMN.Negocio/Negocio/LancamentoNegocio.cs` | 41 | `BuscarPorIdUsuario(...)` síncrona |

**Recomendação**: Migrar para `await` end-to-end. Não fazer ad-hoc — um `.Wait()` removido sem propagar o `async` por todos os caminhos quebra a chain.

### 3.3 Fat Controllers (lógica de negócio na camada errada)

| Controller | Método | Linhas | Diagnóstico |
|---|---|---:|---|
| `AdminController` | `RelatorioAnunciantes` | 221-327 | 100+ linhas de filtragem/ordenação/paginação que pertencem a `AnuncianteNegocio` |
| `AdminController` | (construtor) | 63-111 | **20+ dependências injetadas** — viola SRP. Sinal de god-controller |
| `CompraController` | `EnviarDadosNF` | 261-451 | 190 linhas processando NF-e + cashback + email — deve virar 3 services |
| `ParceirosController` | `ListarParceiros` | 53-69 | Filtragem in-memory após `ToList()` (deveria ser `IQueryable`) |

**Recomendação**: Refactor por Controller, um por sprint, sempre com testes de regressão antes.

### 3.4 Singleton com possível leak

- `api/MMN.Api/Startup.cs:423`: `services.AddSingleton<ICurrencyService, CurrencyService>()`. Se essa classe usar `DbContext` (Scoped) via captive dependency, vai vazar. **Auditar manualmente** o construtor de `CurrencyService` antes de mexer.

---

## 4. Frontend — Gigantismo e Lógica Mal Posicionada

### 4.1 Componentes >500 linhas (devem ser quebrados)

| Arquivo | Linhas | Problema |
|---|---:|---|
| `pages/agencia/painel/admin/carrosseis.vue` | **1.090** | Drag-and-drop + upload + listagem + edição em um único `<script>` |
| `pages/agencia/painel/admin/home-cms.vue` | **942** | JSON da home + UI de edição + chamadas via `$fetch` direto (não usa `useApi`) |
| `pages/agencia/painel/admin/configuracoes-rede.vue` | **727** | Tabelas + formulários + cálculo de soma de percentuais no cliente |
| `components/home/home-hero.vue` | **635** | Banner que carrega 600+ linhas — provavelmente CSS/lógica inline excessiva |
| `pages/agencia/painel/admin/docs.vue` | **534** | Documentação técnica embutida — pode virar markdown servido |
| `pages/agencia/painel/admin/features.vue` | **521** | Roadmap + filtragem + agrupamento — pode usar composable |
| `pages/agencia/painel/admin/bi-financeiro.vue` | **505** | Dashboard com cálculos de proporção que deveriam vir do backend |

**Recomendação**: Sub-componentes em `components/agencia/painel/`, um para cada seção. Manter os arquivos `pages/*` apenas como orquestradores.

### 4.2 Lógica financeira/cashback no frontend (deveria estar no backend)

| Arquivo | Linha | O que está calculando |
|---|---:|---|
| `pages/agencia/painel/admin/configuracoes-rede.vue` | 397, 432 | Validação de soma=100% dos percentuais de split + arredondamento `Math.round(x*10)/10` |
| `pages/agencia/painel/admin/bi-financeiro.vue` | 95 | Market share `(cat.valor / totals.faturamento) * 100` |
| `components/checkout/checkout-verify.vue` | 161 | **Cupons hardcoded no frontend**: `QUANTA10`, `BEMVINDO` com lógica de % e valor fixo |
| `components/cart/cart-progress.vue` | 28 | Cálculo de progresso de frete grátis usando constantes locais |

**Recomendação**: A regra de cashback é **inegociável** (`TESTING.md` §1) — qualquer cálculo financeiro precisa estar no backend. A presença de cupons hardcoded no checkout é especialmente crítica: bloqueia a Wave 2 (`POST /Cupom/validar` já existe).

### 4.3 Chamadas API que ignoram o padrão `useApi`

| Arquivo | Linha | O que faz |
|---|---:|---|
| `pages/agencia/painel/admin/marcas-home.vue` | 135 | `$fetch` direto para `/api/admin/brands` |
| `pages/agencia/painel/admin/carrosseis.vue` | 741, 820 | `$fetch` para `/api/admin/banner-campaigns` |
| `pages/agencia/painel/admin/home-cms.vue` | 435, 444 | `$fetch` para `/api/admin/home-config` |
| `components/login/login-social.vue` | 65 | `fetch` nativo construindo URL manualmente |

**Risco**: Essas chamadas pulam o interceptor de 401 do `useApi` — usuário com sessão expirada NÃO é deslogado e fica vendo erro genérico em vez de ser redirecionado pra login.

### 4.4 Padrões repetidos (oportunidade de DRY)

| Padrão | Onde | Sugestão |
|---|---|---|
| `<div class="qs-page-header">` (título + breadcrumb + botão ação) | `bi-financeiro.vue:3`, `configuracoes-rede.vue:3`, `features.vue:3` (e outras 10+) | Componente `<AdminPageHeader>` |
| `<section class="qs-card-section">` repetido 5+ vezes | `configuracoes-rede.vue`, `bi-financeiro.vue` | Componente `<QsCardSection>` |
| Lógica `badgeStatus(status)` (cor de badge por status) | `relatorio-cashback.vue:139`, `relatorio-de-faturas.vue` | Composable `useStatusBadge()` |
| Extração de mensagem de erro da API | Quase todas as páginas admin | **✅ JÁ FEITO**: `composables/useApiError.ts` (migração das páginas: próxima sprint) |

---

## 5. O Que Foi Refatorado para Evitar Código Espaguete

1. **Eliminado o shim duplicado** `composables/useAgenciaStore.ts` (single source of truth: `pinia/useAgenciaStore.ts`)
2. **Reposicionados os 6 imports** que apontavam para o shim — agora apontam direto para `pinia/`, removendo um nível de indireção
3. **Criado `composables/useApiError.ts`** como utility único para extração de mensagens de erro pt-BR (DRY)
4. **Substituídos `catch {}` vazios** por logging com prefixo identificável (`[QuantaPoints]`, `[FaturaService]`, `[AdiantamentoCashback]`, `[admin/index]`) — agora todo erro silencioso vira log auditável
5. **Removido `Task.Factory.StartNew` decorativo** no `StopAsync` do `FaturaService` — substituído por `Task.CompletedTask` (idiomático)

---

## 6. Status do `TESTING.md` (resposta direta à pergunta do usuário)

> **"Me avise assim que [...] todos os testes do TESTING.md passando 100% verde!"**

**Status real**: Os testes do `TESTING.md` **ainda não existem como código**. O documento define o **plano** de cobertura (CT-COMP-01 a CT-COMP-10, CT-COMPR-01 a CT-COMPR-07, etc.), mas a estrutura `api/MMN.Negocio.Tests/`, `tests/unit/`, `tests/e2e/` ainda não foi criada (`api/MMN.Tests/` existe como esqueleto).

**Por isso esta sprint NÃO refatorou as N+1 queries nem os fat controllers** — fazer isso sem rede de testes seria irresponsável. A sequência correta de próximas sprints é:

| Sprint | Entrega bloqueante |
|---|---|
| **Próxima (Sprint #1)** | Implementar CT-COMP-01 a CT-COMP-10 em `MMN.Negocio.Tests` (xUnit) |
| Sprint #2 | Implementar CT-COMPR-01 a CT-COMPR-07 (compressão dinâmica) |
| Sprint #3 | **AÍ SIM** refatorar N+1 e migrar `.Wait()` para `async` end-to-end |
| Sprint #4 | Quebrar componentes >500 linhas |
| Sprint #5 | Migrar lógica financeira do frontend para o backend |

---

## 7. Resposta Direta às Perguntas do Usuário

### "Onde estavam os gargalos de lentidão?"
Os 4 N+1 queries da seção 3.1 (especialmente `Awin.cs:116` e `FaturaService.cs:114`) são os principais. Os 3 `.Wait()` em código async (3.2) bloqueiam o thread pool sob carga.

### "Quais bugs foram corrigidos?"
8 bugs (seção 2): 1 import duplicado, 5 falhas silenciosas, 2 console.logs, 1 limpeza de Task decorativo.

### "O que foi refatorado para evitar código espaguete?"
Seção 5 — eliminação do shim, criação do `useApiError`, substituição de catches mudos por logs auditáveis.

### "A plataforma está rápida e limpa?"
**Mais limpa, sim.** **Mais rápida, ainda não totalmente** — os gargalos reais (N+1 queries) estão documentados mas só devem ser tocados após os testes de compensação MLM existirem. O ganho de performance virá na Sprint #3.

---

*Mantido por: Replit Agent — Sprint de Auditoria, Mai 2026.*
*Próxima revisão sugerida: após a primeira leva de testes xUnit estar verde.*
