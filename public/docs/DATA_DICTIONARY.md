# DATA_DICTIONARY.md — Quanta Shop

> Dicionário de dados da API .NET 8 + banco SQL Server Azure.
> Engenharia reversa das entidades em `api/MMN.Dominio/Model/`.
> **Conformidade LGPD** — campos sensíveis explicitamente marcados.
> Versão 1.1 — Mai 2026

---

## Convenções

| Símbolo | Significado |
|---------|-------------|
| 🔒 | **Dado sensível LGPD** — exige criptografia em trânsito (HTTPS) e mascaramento em logs |
| 🔑 | Chave primária |
| 🔗 | Chave estrangeira |
| 📊 | Campo financeiro / valor monetário |
| 🕒 | Auditoria temporal |

---

## 1. Núcleo de Identidade

### `Usuario` — perfil base de qualquer usuário
**Tabela:** `Usuario` · **PK:** `IdUsuario` (Guid)

| Coluna | Tipo | LGPD | Descrição |
|--------|------|------|-----------|
| 🔑 `IdUsuario` | Guid | — | ID único |
| 🔗 `IdUsuarioPai` | Guid? | — | Indicador/Upline na Rede Proprietária (auto-relacionamento) |
| 🔗 `IdGrupo` | int | — | Grupo de permissões legado (Admin = Admin, Comerciante = Empresa Parceira ZEE DIGITAL, Cliente/Agente = Membro ADF) |
| 🔒 `Nome` | string | **Sim** | Nome completo |
| 🔒 `NomeSocial` | string | **Sim** | Nome social (Lei 14.046/2020) |
| 🔒 `Email` | string | **Sim** | E-mail (login alternativo) |
| 🔒 `Login` | string | **Sim** | Login alfanumérico |
| 🔒 `Senha` | string | **Sim** | Hash bcrypt + `SaltKey` |
| 🔒 `SaltKey` | byte[] | **Sim** | Salt do bcrypt — **NUNCA expor** |
| 🔒 `Documento` | string | **Sim** | CPF/CNPJ — **NUNCA expor em URL ou log** |
| 🔒 `RG` | string | **Sim** | RG — somente para credenciamento |
| 🔒 `OrgaoEmissorRG` | string | **Sim** | — |
| 🔒 `EstadoRG` | string | **Sim** | — |
| 🔒 `Celular` | string | **Sim** | Telefone móvel |
| 🔒 `DataNascimento` | DateTime? | **Sim** | — |
| 🔒 `Genero` | string | **Sim** | — |
| `AssinaturaEletronica` | string | **Sim** 🔒 | Hash de aceite digital |
| 🔗 `IdGraduacao` | int? | — | Graduação atual (FK Graduacao) |
| `Cultura` | string | — | pt-BR / es-ES |
| `PosicaoBinario` | short? | — | Esquerda (1) ou Direita (2) na rede binária |
| 🕒 `DataReferencia` | DateTime? | — | Mês de referência para cálculo |
| 🕒 `DataQualificacao` | DateTime? | — | Quando atingiu graduação atual |
| 🕒 `DataCadastro` | DateTime? | — | — |
| 🕒 `DataUltimoAcesso` | DateTime? | — | — |
| 🔒 `EnderecoIPUltimoAcesso` | string | **Sim** | IP do último login (auditoria) |
| 🔒 `AgenteUltimoAcesso` | string | **Sim** | User-agent do último login |
| `Ativo` | bool | — | Soft-delete |
| `Bloqueado` | bool | — | Bloqueio por tentativas erradas |
| 🕒 `DataBloqueio` | DateTime? | — | — |
| `TentativasIncorretas` | short? | — | Contador (reset ao login OK) |
| `EmailConfirmado` | bool | — | — |
| `Master` | bool | — | Flag de super-admin |
| `UrlImg` | string | — | URL da foto |
| `Empreendedor` | bool | — | Indica se é Agente |
| `Perfil` | char | — | Categoria legada: C = Consumidor (ADF) / L = Empresa Parceira (ZEE DIGITAL) / A = Agente de Fidelização (ADF) |
| `TermosDeAceite` | bool | — | LGPD: aceite obrigatório no cadastro |
| `PreCadastro` | bool | — | Cadastro incompleto |
| `LoginAlterado` | bool | — | Sinaliza login customizado |
| `IndicadoPeloQS` | bool | — | Marca se foi indicado pela própria Quanta |
| `LinkAssistenteVirtual` | string | — | URL do assistente WhatsApp |
| 🔒 `AsaasCustomerId` | string | **Sim** | ID externo do cliente no Asaas (assinatura) |

**Relações principais**
- `UsuarioPai` (auto-FK) — patrocinador
- `Filhos` (1:N) — diretos na rede
- `Lancamento`, `Transacao`, `Saque` — financeiro
- `UsuarioBanco` — contas bancárias
- `UsuarioCarteira` — carteiras BTC
- `Credenciamento` — para Empresas Parceiras (ZEE DIGITAL)
- `Pedido` (compras) e `Vendas` (vendas como comerciante)
- `RefreshToken` — tokens JWT ativos

**Recomendações LGPD para Usuario**
- ❌ Nunca enviar `Documento`, `Senha`, `SaltKey` em respostas JSON.
- ❌ Nunca logar `Email` ou `Documento` em texto plano.
- ✅ Mascarar CPF na UI: `123.***.**5-67`.
- ✅ Direito ao esquecimento: implementar soft-delete + anonimização (`Nome → "Usuário removido"`, `Email/Documento → null`).

---

### `UsuarioBanco` — contas bancárias para saque
**Tabela:** `UsuarioBanco` · **PK:** `IdUsuarioBanco`

| Coluna | Tipo | LGPD | Descrição |
|--------|------|------|-----------|
| 🔑 `IdUsuarioBanco` | int | — | — |
| 🔗 `IdUsuario` | Guid? | — | — |
| 🔗 `IdBanco` | int? | — | FK Banco |
| 🔒 `Agencia` | string | **Sim** | — |
| 🔒 `DigitoAgencia` | string | **Sim** | — |
| 🔒 `Conta` | string | **Sim** | — |
| 🔒 `DigitoConta` | string | **Sim** | — |
| 🔒 `Cpfcnpj` | string | **Sim** | CPF/CNPJ titular da conta |
| 🔒 `NomeConta` | string | **Sim** | Nome titular |
| `Ativo` | bool | — | — |
| `IdTipo` | int | — | Conta corrente / poupança / PIX |

> **Toda escrita** em `UsuarioBanco` exige **dupla autenticação** (idealmente 2FA) na produção.

---

### `UsuarioCarteira` — carteiras BTC (legado, em revisão)
**Tabela:** `UsuarioCarteira` · **PK:** `IdUsuarioCarteira`

| Coluna | Tipo | LGPD | Descrição |
|--------|------|------|-----------|
| 🔑 `IdUsuarioCarteira` | long | — | — |
| 🔗 `IdUsuario` | Guid | — | — |
| 🔒 `Endereco` | string | **Sim** | Endereço da carteira BTC |
| `Descricao` | string | — | Apelido |
| 🕒 `DataCadastro` | DateTime | — | — |
| `Ativo` / `Aprovado` | bool | — | Aprovado por admin antes do primeiro saque |
| 🕒 `DataAprovacao` | DateTime? | — | — |

> ⛔ **Status: Entidade DESCONTINUADA.** A Quanta Shop não opera como corretora cripto. Saques ocorrerão estritamente em moeda corrente (PIX/TED).

---

## 2. Movimentação Financeira

### `Transacao` — evento financeiro raiz
**Tabela:** `Transacao` · **PK:** `IdTransacao` (long)

| Coluna | Tipo | Financeiro | Descrição |
|--------|------|-----------|-----------|
| 🔑 `IdTransacao` | long | — | — |
| 🔗 `IdUsuario` | Guid | — | Beneficiário |
| 🔗 `IdTipo` | int | — | Crédito / Débito / Cashback / Comissão / Saque |
| 📊 `ValorPrincipal` | decimal | **Sim** | Valor base |
| 📊 `ComissaoTotal` | decimal? | **Sim** | Soma das comissões geradas |
| 🕒 `DataTransacao` | DateTime | — | — |
| 🕒 `DataReferencia` | DateTime? | — | Mês de referência (fechamento) |
| `Descricao` | string | — | Texto livre |
| 🔗 `IdStatus` | int | — | Pendente / Confirmado / Estornado |
| `Ativo` | bool | — | — |
| 🔗 `IdAnunciante` | int? | — | Origem da venda (parceiro) |
| `IdVendaZanox` | Guid? | — | ID da venda no Zanox |
| `IdVendaAfilio` | long? | — | ID Afilio |
| `IdVendaAwin` | long? | — | ID Awin |

### `Lancamento` — credito/débito derivado da transação (timeline)
**Tabela:** `Lancamento` · **PK:** `IdLancamento`

| Coluna | Tipo | Financeiro | Descrição |
|--------|------|-----------|-----------|
| 🔑 `IdLancamento` | long | — | — |
| 🔗 `IdUsuario` | Guid | — | — |
| 🔗 `IdTransacao` | long | — | Transação originária |
| 🔗 `IdTipo` | int | — | Cashback / Bônus / Saque / Estorno |
| 📊 `Valor` | decimal | **Sim** | Sinalizado (positivo = crédito) |
| 🕒 `DataLancamento` | DateTime | — | — |
| 🕒 `DataReferencia` | DateTime? | — | — |
| `Ativo` | bool | — | — |
| 🔗 `IdStatus` | int? | — | — |
| `Bloqueado` | bool | — | Bloqueado para saque (período de quarentena) |
| `OrdemExibicao` | int | — | Ordenação no extrato |

### `Pedido` — compra do consumidor
**Tabela:** `Pedido` · **PK:** `IdPedido` (long)

| Coluna | Tipo | Financeiro | Descrição |
|--------|------|-----------|-----------|
| 🔑 `IdPedido` | long | — | — |
| 🔗 `IdUsuario` | Guid | — | Comprador |
| 🔗 `IdUsuarioComerciante` | Guid? | — | Vendedor (se P2P) |
| 🔗 `IdTransacao` | long? | — | Transação financeira correspondente |
| `Tipo` | int | — | Online / Físico / Plus / Manual |
| `Status` | int | — | Aberto / Pago / Cancelado / Estornado |
| 🕒 `DataPedido` | DateTime | — | — |
| `Codigo` | string | — | Código humano |
| 📊 `ValorTaxa` | decimal | **Sim** | Taxa cobrada |
| 📊 `ValorProduto` | decimal | **Sim** | Valor do item |
| 📊 `ValorPedido` | decimal | **Sim** | Total |
| 📊 `ValorPago` | decimal? | **Sim** | Quanto foi efetivamente pago |
| 🕒 `DataPagamento` | DateTime? | — | — |
| `Pago` | bool | — | — |
| `Cancelado` | bool? | — | — |
| 🔒 `EnderecoDeposito` | string | — | Endereço de entrega (sensível) |
| `MeioPagamento` | int | — | PIX / Boleto / Cartão |
| `NumeroParcelas` | int | — | — |
| `Quantidade` | int | — | — |
| `UrlPagamento` | string | — | URL gateway |
| 📊 `Cotacao` | decimal | **Sim** | Cotação BTC/BRL no momento |
| `CodigoReferenciaBoleto` | string | — | — |
| `LinhaDigitavelBoleto` | string | — | — |
| 📊 `Cashback` | decimal? | **Sim** | Cashback gerado |
| 📊 `PercentualCashback` | decimal? | **Sim** | % aplicado |
| 📊 `ReaisPorPonto` | decimal? | **Sim** | Conversão R$/ponto |
| `ContabilizarPontuacao` | bool | — | — |
| `GeradoManualmente` | bool | — | Lançamento manual por admin |

### `Saque` — solicitação de retirada de saldo
**Tabela:** `Saque` · **PK:** `IdSaque` (int)

| Coluna | Tipo | LGPD/Financeiro | Descrição |
|--------|------|-----------------|-----------|
| 🔑 `IdSaque` | int | — | — |
| 🕒 `DataSolicitacao` | DateTime | — | — |
| 📊 `Valor` | decimal | **Sim** | Bruto |
| 📊 `TaxaSaque` | decimal | **Sim** | Taxa aplicada |
| 📊 `Cotacao` | decimal? | **Sim** | Para BTC |
| 🔒 `EnderecoBTC` | string | **Sim** | Endereço destino BTC |
| `Processado` | bool | — | Concluído |
| 🕒 `DataProcessado` | DateTime? | — | — |
| 🕒 `DataAprovacao` | DateTime? | — | — |
| `Aprovador` | string | — | Login do admin que aprovou |
| `Historico` | string | — | Log de mudanças de status |
| 🔗 `IdUsuario` | Guid | — | Solicitante |
| 🔗 `IdStatus` | int | — | Pendente / Aprovado / Rejeitado / Pago |
| 🔗 `IdTipo` | int | — | PIX / TED / BTC |
| 🔗 `IdTransacao` | long | — | Transação espelho |
| 🔗 `IdUsuarioBanco` | int | — | Conta destino |
| 🔒 `UrlTransacao` | string | — | Comprovante (URL no S3/blob) |

---

## 3. Hierarquia da Rede Proprietária

### `QuantaAmizade` — programa de indicações
**Tabela:** `QuantaAmizade` · **PK:** `IdQuantaAmizade`

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| 🔑 `IdQuantaAmizade` | int | — |
| 🔗 `IdUsuarioPai` | Guid | Padrinho |
| 🔗 `IdUsuarioFilho` | Guid | Afilhado |
| 🕒 `DataCadastro` | DateTime | — |
| 🕒 `DataFim` | DateTime | Prazo para o objetivo |
| `ObjetivoAtingido` | bool | Trigger de bonificação |

### `QuantaAmizadeHistorico` — log de cashback do programa
| Coluna | Tipo | Descrição |
|--------|------|-----------|
| 📊 `ValorCashback` | decimal | Bonificado |

### `Graduacao` — níveis do plano de carreira
**Relacionada com:** `GraduacaoRequisitos`, `HistoricoGraduacao`, `LogGraduacao`, `MensagemGraduacao`

### `CacheResumoBinario` — cache do volume binário do usuário
> Atualizado em batch noturno. **Não é fonte de verdade** — apenas otimização de leitura.

### `PercentualResidualCashback`, `PercentualBonusCredenciamento`, `PercentualBonusResidualCredenciamento`
Tabelas de configuração dos percentuais por nível.

### `PremiacaoDownline`, `AuditoriaPremiacao`, `UsuarioPremiacao`
Premiações pontuais para a rede.

---

## 4. Catálogo e Comércio

### `Anunciante` — Empresa Parceira (Dona da ZEE DIGITAL)
| Coluna | Tipo | Descrição |
|--------|------|-----------|
| 🔑 `IdAnunciante` | int | — |
| `Nome` | string | — |
| `ImagemUrl` | string | Logo |
| 📊 `Cashback` | decimal | % padrão |
| `Ativo` | bool | — |
| `IdProgramZanox` / `IdAfilio` / `IdAwin` | string | IDs em redes de afiliados |
| `EditadoUsuario` | bool | — |
| 🕒 `DataCadastro` / `DataAtualizacao` | DateTime | — |
| `AccountId` | string | Conta da rede de afiliados |
| `Ancora` | bool | Loja-âncora (destaque) |

**Relações:** `AnuncianteCashBack`, `CategoriaAnunciante`, `Transacao`

### `Produto`, `ProdutoNivel`, `LogProduto`
Produtos do marketplace interno.

### `Promocao`, `OrdenacaoAnuncio`, `AnuncianteCashBackLog`
Configurações de promoção e auditoria.

### `Pagamento`, `PagamentoPedido`
Registros de pagamento (gateway).

### `Fatura`
Faturas da Empresa Parceira / ZEE DIGITAL (mensalidade da plataforma).

### `Credenciamento`
Cadastro estendido para Empresas Parceiras (ZEE DIGITAL): ecossistema, categoria, taxa, percentuais.

### `CupomCashback`, `CupomCashbackDadosNF`, `CupomCashbackItemNF`, `CuponCashbackPedido`
Sistema de cupons fiscais (NF-e) para cashback presencial.

---

## 5. Operacional / Suporte

| Entidade | Função |
|----------|--------|
| `Suporte`, `SuporteLog` | Tickets de suporte |
| `Mensagem` | Comunicados internos |
| `Faq` | Perguntas frequentes |
| `MaterialApoio` | Materiais de marketing para agentes |
| `Acesso` | Log de acessos (auditoria) |
| `Configuracao` | Parâmetros globais (taxas, percentuais default) |
| `Tutorial` | Guias passo-a-passo |
| `Carrossel` | Banners da home |
| `Categoria`, `CategoriaAnunciante`, `Cidade`, `Estado` | Tabelas de domínio |

---

## 6. Autenticação Externa

| Entidade | Função |
|----------|--------|
| `AutenticacaoExterna` | Vínculo com providers (Google, etc.) |
| `ProvedorAutenticacao` | Catálogo de providers |
| `RefreshToken` | Tokens ativos JWT (rotacionados a cada refresh) |
| `Enum.IdentityProvider` / `IdentityProviderProtocol` | Enums |

---

## 7. Auditoria e Conformidade

### `AuditoriaLgpd` — Reveal de dados sensíveis por usuário Master
**Tabela:** `AuditoriaLgpd` · **PK:** `IdAuditoriaLgpd`

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| 🔑 `IdAuditoriaLgpd` | long | — |
| 🔗 `IdUsuarioMaster` | Guid | Quem revelou |
| 🔗 `IdUsuarioAlvo` | Guid | De quem o dado foi revelado |
| `Campo` | string | Nome do campo revelado (`Documento`, `Conta`, `Email`, etc.) |
| `Motivo` | string | Justificativa informada pelo Master |
| 🔒 `EnderecoIP` | string | IP do solicitante |
| `AgenteUsuario` | string | User-agent do navegador |
| 🕒 `DataAcesso` | DateTime | Timestamp da revelação |

> Toda revelação via `POST /admin/revelar-dado-sensivel` grava obrigatoriamente nesta tabela antes de retornar o dado real.

### Tabelas de Log Obrigatórias
- `Acesso` — IP, user-agent, timestamp.
- `LogProduto` — alterações de catálogo.
- `LogGraduacao` — promoções/rebaixamentos.
- `AnuncianteCashBackLog` — mudança de % de cashback.
- `AuditoriaPremiacao` — premiações distribuídas.
- `SuporteLog` — eventos do ticket.
- `AlteracaoPerfil` — alterações sensíveis no perfil (CPF, banco, e-mail).

### Práticas LGPD obrigatórias
1. **Consentimento** — `TermosDeAceite` no `Usuario` é gravado **com timestamp**.
2. **Acesso** — usuário pode baixar todos os seus dados (endpoint a implementar — ver `FEATURES.md#backlog`).
3. **Esquecimento** — soft-delete com anonimização preserva integridade financeira (lançamentos referenciam `IdUsuario`).
4. **Retenção** — dados financeiros mantidos por 5 anos (CTN art. 195 + Marco Civil).
5. **Mínimo necessário** — não pedir RG se não for credenciamento.
6. **Encarregado de Dados (DPO)** — deve estar identificado no `/privacidade`.

### Endpoints sensíveis (sempre `[Authorize]` + `Role` específica)
- `POST /api/v2/usuario/alterar-banco` → 2FA recomendado
- `POST /api/v2/saque/solicitar` → 2FA obrigatório > R$ 500
- `GET /api/admin/usuarios/{id}/dados-completos` → log em `Acesso`
- `DELETE /api/admin/usuarios/{id}` → exige razão registrada

---

## Observações de Schema

Os warnings do EF Core no startup indicam **decimais sem precisão explícita** em várias entidades (Pagamento, Pedido, Saque, Transacao, Saldo etc.). **Risco:** truncagem silenciosa em valores monetários. **Recomendação:** adicionar `[Precision(18, 2)]` ou `HasColumnType("decimal(18,2)")` em todas as colunas `decimal`.

---

*Atualize este arquivo quando uma nova entidade for adicionada ou um campo sensível for tocado.*
