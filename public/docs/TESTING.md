# TESTING.md — Diretrizes Oficiais de QA e Testes

> **Versão:** 1.0.0 | **Data:** 2026-05-02
> Documento inegociável para aceite de qualquer novo código na plataforma Quanta Shop.
> Todo Pull Request / commit deve satisfazer os critérios aqui definidos antes de ser considerado **Done**.

---

## 1. Testes do Motor Financeiro (Backend .NET 8 — xUnit)

O motor financeiro é o coração da plataforma. Qualquer alteração no pipeline de cashback exige cobertura completa na suíte xUnit (`api/MMN.Tests/`), atualmente com **13 cenários base**. Nenhum cenário pode regredir.

### 1.1 Split Exato de Cashback

| Cenário | Critério de aceite |
|---|---|
| Transação padrão | 50% → Quanta Shop · 25% → Consumidor · 25% → Rede (soma = 100%) |
| Sustentabilidade ativa | 10% do ganho do consumidor retido como crédito na plataforma antes do repasse |
| Rede zerada | Split da Rede vai integralmente para Quanta Shop quando não há uplines |
| Arredondamento | Nenhum centavo perdido — valores devem fechar com precisão decimal |

```csharp
Assert.Equal(valorTransacao * 0.50m, resultado.ParteEmpresa, precision: 4);
Assert.Equal(valorTransacao * 0.25m, resultado.ParteConsumidor, precision: 4);
Assert.Equal(valorTransacao * 0.25m, resultado.ParteRede, precision: 4);
```

### 1.2 Profundidade de 12 Níveis (Residual HAF)

- O bônus residual deve subir corretamente nos **12 níveis** da rede de acordo com a licença HAF ativa do upline (Vision → Shu Ra Ri → Disruptiva → Master Quantum).
- Testar: nível 1, nível 6, nível 12 e além do 12º (deve parar sem erro).
- Testar: upline sem licença HAF ativa não recebe bônus residual.

### 1.3 Compressão Dinâmica

- Usuários **inativos** (sem compra dentro de `DIAS_QUARENTENA`) devem ser **pulados**.
- O bônus é repassado ao primeiro upline ativo encontrado.
- Testar: 1 inativo, 3 inativos consecutivos, todos inativos (bônus reverte para Quanta Shop).
- Flag `COMPRESSAO_DINAMICA_ATIVA = false` desabilita completamente o comportamento.

### 1.4 Multiplicador Plus (2×)

- A Assinatura Plus deve **dobrar estritamente o Bônus Residual** do usuário.
- O multiplicador **nunca** se aplica ao cashback da compra direta do próprio usuário.
- Testar: usuário Plus recebe 2× residual · usuário Padrão recebe 1× residual · cashback direto do Plus = igual ao Padrão.

### 1.5 Sustentabilidade (10% Inegociável)

- A retenção de 10% dos ganhos em crédito de plataforma deve ocorrer **sempre**, independente de qualquer outra configuração.
- Testar: valor após sustentabilidade = `ganho × 0.90`.
- Testar: `PERCENTUAL_RETENCAO_SUSTENTABILIDADE` fora do range permitido lança exceção de validação.

### 1.6 Execução

```bash
cd api && dotnet test MMN.Tests/ --verbosity normal
# Resultado esperado: 13 cenários · 0 falhas · 0 pulados
```

---

## 2. Testes de Frontend e Usabilidade (Nuxt 3)

### 2.1 Paywall e Controle de Acesso

| Cenário | Comportamento esperado |
|---|---|
| Consumidor sem licença HAF tenta acessar `/agencia/painel/rede` | Redirecionado para tela de upsell — nunca vê o dashboard |
| Usuário com HAF ativo | Acessa normalmente |
| Token de sessão expirado | Redirecionado para login — sem loop infinito |
| Middleware `agencia-admin` com usuário não-admin | HTTP 403 ou redirect para `/` |

### 2.2 Variáveis Dinâmicas (Sem Hardcode)

- A barra de progresso da Assinatura Plus deve ler `adminConfig.metaConsumoPlus` do banco via API — **nenhum valor fixo no código**.
- Testar: alterar `metaConsumoPlus` no admin → barra reflete o novo valor sem redeploy.
- Testar: API indisponível → componente exibe estado de erro, não um valor fictício.

```bash
# Verificar ausência de hardcodes
grep -rn "metaConsumoPlus\s*=\s*[0-9]" pages/ components/
# Resultado esperado: nenhuma ocorrência
```

### 2.3 Responsividade Mobile-First (iPhone SE — 375×667)

| Tela | Critério |
|---|---|
| Vitrine de Links (`/vitrine`) | Sem scroll horizontal · botões com área mínima de 44×44 px |
| Botão "Compartilhar via WhatsApp" | Abre `wa.me/` corretamente no mobile · não quebra layout |
| Dashboard Rede | Cards empilhados em coluna única |
| Checkout (`/checkout-verify`) | Campos de cupom e pontos visíveis sem zoom |

Resolução base: **375×667** (iPhone SE). Testar também 390×844 (iPhone 14) e 412×915 (Android médio).

---

## 3. Segurança e Auditoria LGPD

### 3.1 Mascaramento de Dados Sensíveis

Dados pessoais **nunca** devem trafegar abertos nas respostas da API, exceto mediante revelação explícita por usuário `Master`.

| Campo | Formato mascarado obrigatório |
|---|---|
| CPF | `123.***.***-67` |
| CNPJ | `12.***.***/**01-**` |
| Conta bancária | `***456-7` |
| Agência | `****` |
| E-mail | `us***@dominio.com` |
| Telefone | `(11) 9****-1234` |

- Endpoint `POST Admin/RevelarDadoSensivel` deve rejeitar usuários não-Master com HTTP 403.
- Toda revelação deve gravar registro em `AuditoriaLgpd` com timestamp, usuário solicitante e campo revelado.
- Dado real retornado **apenas** para `Usuario.Master = true`.

### 3.2 Fail-Closed Auth (Google Sign In)

O sistema deve rejeitar **sumariamente** qualquer acesso com credenciais inválidas — sem fallback silencioso.

| Cenário | Comportamento esperado |
|---|---|
| Token Google válido + `GOOGLE_CLIENT_ID` correto | Login bem-sucedido |
| Token Google expirado | HTTP 401 — mensagem clara |
| `GOOGLE_CLIENT_ID` errado (env incorreto) | HTTP 401 — nunca faz bypass |
| Token de outro projeto Google | HTTP 401 — audience mismatch |
| Token ausente / malformado | HTTP 400 |

**Princípio:** Em caso de qualquer dúvida sobre a validade do token, o sistema **nega o acesso**. Nenhuma exceção por conveniência.

---

## Critérios Globais de Done

Para qualquer feature ser considerada concluída, **todos** os itens abaixo devem estar satisfeitos:

- [ ] `dotnet test api/` — todos os cenários xUnit passando (0 falhas)
- [ ] `npm run build` — compilação Nuxt sem erros
- [ ] Nenhum dado sensível exposto nas respostas da API (CPF, conta, e-mail completo)
- [ ] Paywall funcional — usuários sem permissão não acessam rotas protegidas
- [ ] Layout mobile testado em iPhone SE (375×667) sem quebras
- [ ] Variáveis dinâmicas lidas do banco — nenhum hardcode de configuração de negócio
- [ ] Autenticação Google fail-closed — token inválido → rejeição imediata

---

*Mantenha este documento atualizado a cada nova feature ou mudança arquitetural significativa.*
