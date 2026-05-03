# TESTING.md — Diretrizes Oficiais de QA e Testes

> **Versão:** 1.1.0 | **Atualizado:** Mai 2026
> Documento inegociável para aceite de qualquer novo código na plataforma Quanta Shop.
> Todo commit deve satisfazer os critérios aqui definidos antes de ser considerado **Done**.

---

## 1. Testes do Motor Financeiro (Backend .NET 8 — xUnit)

O motor financeiro é o coração da plataforma. Qualquer alteração no pipeline de cashback exige cobertura completa na suíte xUnit (`api/MMN.Tests/`), atualmente com **13 cenários base**. Nenhum cenário pode regredir.

### 1.1 Split Exato de Cashback

| Cenário | Critério de aceite |
|---------|-------------------|
| Transação padrão | 50% → Quanta Shop · 25% → Consumidor · 25% → Rede (soma = 100%) |
| Sustentabilidade ativa | 10% do ganho do consumidor retido como crédito na plataforma |
| Rede zerada | Split da Rede vai integralmente para Quanta Shop quando não há uplines |
| Arredondamento | Nenhum centavo perdido — valores fecham com precisão decimal |

```csharp
Assert.Equal(valorTransacao * 0.50m, resultado.ParteEmpresa, precision: 4);
Assert.Equal(valorTransacao * 0.25m, resultado.ParteConsumidor, precision: 4);
Assert.Equal(valorTransacao * 0.25m, resultado.ParteRede, precision: 4);
```

### 1.2 Profundidade de 12 Níveis (Residual HAF)

- Bônus residual deve subir corretamente nos **12 níveis** de acordo com a licença HAF ativa
- Testar: nível 1, nível 6, nível 12 e além do 12º (deve parar sem erro)
- Testar: upline sem licença HAF ativa não recebe bônus residual

### 1.3 Compressão Dinâmica

- Usuários **inativos** (sem compra dentro de `DIAS_QUARENTENA`) devem ser **pulados**
- O bônus é repassado ao primeiro upline ativo encontrado
- Testar: 1 inativo, 3 inativos consecutivos, todos inativos (bônus reverte para Quanta Shop)
- Flag `COMPRESSAO_DINAMICA_ATIVA = false` desabilita completamente

### 1.4 Multiplicador Plus (2×)

- Assinatura Plus deve **dobrar estritamente o Bônus Residual**
- Multiplicador **nunca** se aplica ao cashback direto da compra do próprio usuário
- Testar: Plus recebe 2× residual · Padrão recebe 1× · cashback direto do Plus = igual ao Padrão

### 1.5 Sustentabilidade (10% Inegociável)

- Retenção de 10% deve ocorrer **sempre**, independente de outras configurações
- Testar: valor após sustentabilidade = `ganho × 0.90`
- `PERCENTUAL_RETENCAO_SUSTENTABILIDADE` fora do range lança exceção de validação

### 1.6 Execução

```bash
cd api && dotnet test MMN.Tests/ --verbosity normal
# Resultado esperado: 13 cenários · 0 falhas · 0 pulados
```

---

## 2. Testes de Frontend e Usabilidade (Nuxt 3)

### 2.1 Paywall e Controle de Acesso

| Cenário | Comportamento esperado |
|---------|----------------------|
| Consumidor sem HAF tenta `/agencia/painel/rede` | Redirecionado para tela de upsell |
| Usuário com HAF ativo | Acessa normalmente |
| Token de sessão expirado | Redirecionado para login — sem loop infinito |
| Não-admin tenta `/agencia/painel/admin/*` | HTTP 403 ou redirect para `/` |
| Não-admin tenta `/lab` | HTTP 403 ou redirect para `/` |

### 2.2 Variáveis Dinâmicas (Sem Hardcode)

- Barra de progresso da Assinatura Plus deve ler `adminConfig.metaConsumoPlus` via API
- Testar: alterar `metaConsumoPlus` no admin → barra reflete sem redeploy
- API indisponível → componente exibe estado de erro, não valor fictício

```bash
# Verificar ausência de hardcodes financeiros
grep -rn "metaConsumoPlus\s*=\s*[0-9]" pages/ components/
# Resultado esperado: nenhuma ocorrência
```

### 2.3 Responsividade Mobile-First (iPhone SE — 375×667)

| Tela | Critério |
|------|---------|
| Vitrine de Links (`/vitrine`) | Sem scroll horizontal · botões 44×44 px mínimo |
| Botão WhatsApp | Abre `wa.me/` corretamente no mobile |
| Dashboard Rede | Cards empilhados em coluna única |
| Checkout (`/checkout-verify`) | Campos de cupom e pontos visíveis sem zoom |
| LAB (`/lab`) | Grid de cards em coluna única abaixo de 600px |
| Busca Inteligente | Filtros colapsáveis · view lista em mobile |

Resolução base: **375×667** (iPhone SE). Testar também 390×844 (iPhone 14) e 412×915 (Android médio).

---

## 3. Testes E2E — Matriz de Cenários Críticos

### 3.1 Fluxo de Autenticação

| ID | Cenário | Passos | Resultado esperado |
|----|---------|--------|-------------------|
| E2E-01 | Login com CPF/Senha válidos | POST `/auth/login` → JWT retornado | Redirect para `/agencia/painel` com token salvo |
| E2E-02 | Login Google válido | GIS token → POST `/auth/google` | JWT retornado, usuário criado ou vinculado |
| E2E-03 | Token expirado durante sessão | JWT expirado no request | Redirect para `/agencia/login` sem loop |
| E2E-04 | Login com senha errada | 3 tentativas → `TentativasIncorretas` | Conta bloqueada; mensagem clara |
| E2E-05 | Refresh token | POST `/auth/refresh` com token válido | Novo JWT retornado |

### 3.2 Fluxo de Cashback

| ID | Cenário | Verificação |
|----|---------|-------------|
| E2E-10 | Compra aprovada em parceiro Awin | Lançamento criado para consumidor + rede |
| E2E-11 | Compra com cupom `QUANTA10` | Desconto de 10% aplicado; `CupomUso` registrado |
| E2E-12 | Resgate de Quanta Points | Saldo reduzido atomicamente; débito em `QuantaPontoLancamento` |
| E2E-13 | Cashback em quarentena | Lançamento com `Bloqueado = true`; saldo indisponível para saque |
| E2E-14 | Cashback liberado após quarentena | `Bloqueado = false`; disponível para saque |

### 3.3 Fluxo de Saque

| ID | Cenário | Verificação |
|----|---------|-------------|
| E2E-20 | Saque PIX abaixo do mínimo | Erro com valor mínimo informado |
| E2E-21 | Saque PIX válido | Entidade `Saque` criada com `Status=Pendente` |
| E2E-22 | Admin aprova saque | `Status=Aprovado`; `Processado=true`; log em `Historico` |
| E2E-23 | Admin rejeita saque | `Status=Rejeitado`; cashback estornado ao consumidor |
| E2E-24 | Saque com saldo insuficiente | Erro 400; nenhuma entidade criada |

### 3.4 LGPD e Segurança

| ID | Cenário | Verificação |
|----|---------|-------------|
| E2E-30 | CPF na tela de faturas (não-Master) | Exibido mascarado: `123.***.***-67` |
| E2E-31 | Reveal de CPF por usuário Master | `AuditoriaLgpd` registrado; dado real retornado somente para Master |
| E2E-32 | Reveal de CPF por não-Master | HTTP 403; nenhum dado exposto |
| E2E-33 | Rate limit em login (11ª tentativa/min) | HTTP 429 com `Retry-After` |
| E2E-34 | JWT com issuer/audience errado | HTTP 401; acesso negado |
| E2E-35 | Cookie de debug em produção | Ignorado — sem efeito nem bloqueio |

### 3.5 LAB e Documentação

| ID | Cenário | Verificação |
|----|---------|-------------|
| E2E-40 | Não-admin tenta acessar `/lab` | Redirect para `/agencia/painel` |
| E2E-41 | Admin acessa `/lab` | Hub carregado com KPIs de features.json |
| E2E-42 | Admin acessa `/lab/flow-standard` | 5 seções navegáveis por chips |
| E2E-43 | Download de PDF no viewer de docs | PDF gerado com nome do arquivo correto |
| E2E-44 | Busca no visualizador de docs | Filtra documentos em tempo real |

---

## 4. Testes de Build

```bash
# Compilação TypeScript + Vue sem erros
npm run build

# Verificações pós-build
ls -la .output/server/    # servidor Nitro
ls -la .output/public/    # assets estáticos
node .output/server/index.mjs &   # deve subir na porta 3000
curl -s -o /dev/null -w "%{http_code}" http://localhost:3000/  # deve retornar 200
```

---

## 5. Segurança — Fail-Closed Auth

| Cenário | Comportamento esperado |
|---------|----------------------|
| Token Google válido + `GOOGLE_CLIENT_ID` correto | Login bem-sucedido |
| Token Google expirado | HTTP 401 — mensagem clara |
| `GOOGLE_CLIENT_ID` errado | HTTP 401 — nunca faz bypass |
| Token de outro projeto Google | HTTP 401 — audience mismatch |
| Token ausente / malformado | HTTP 400 |

**Princípio:** Em qualquer dúvida sobre validade, o sistema **nega o acesso**. Nenhuma exceção por conveniência.

---

## 6. Critérios Globais de Done

Para qualquer feature ser considerada concluída, **todos** os itens abaixo devem estar satisfeitos:

- [ ] `dotnet test api/` — todos os cenários xUnit passando (0 falhas)
- [ ] `npm run build` — compilação Nuxt sem erros de TypeScript
- [ ] Nenhum dado sensível exposto nas respostas da API (CPF, conta, e-mail completo)
- [ ] Paywall funcional — usuários sem permissão não acessam rotas protegidas
- [ ] Layout mobile testado em iPhone SE (375×667) sem quebras
- [ ] Variáveis dinâmicas lidas do banco — nenhum hardcode de configuração de negócio
- [ ] Autenticação Google fail-closed — token inválido → rejeição imediata
- [ ] CHANGELOG.md atualizado com a mudança
- [ ] features.json com status correto da feature

---

*Mantenha este documento atualizado a cada nova feature ou mudança arquitetural significativa.*
