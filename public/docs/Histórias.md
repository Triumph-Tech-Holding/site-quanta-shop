# Histórias.md — Personas e Histórias de Usuário

> Guia oficial de Personas e Histórias de Usuário da Quanta Shop.
> Alinhado com a arquitetura de negócios definida em `CLAUDE.md` e `FEATURES.md`.
> Última atualização: Mai 2026

---

## 1. Personas Oficiais

### Persona 1 · Consumidor (Membro Base)

> "Quero economizar nas minhas compras do dia a dia e ganhar cashback sem complicação."

- Busca cashback, facilidade de uso e praticidade
- Ganha apenas pelo próprio consumo e por indicações diretas simples
- **Não possui Licença HAF** — não tem acesso a dashboard de rede, links rastreáveis avançados nem comissões residuais multinível
- Rota: `/agencia/painel`
- Campo no banco: `Usuario.Perfil = 'C'`, `Usuario.Empreendedor = false`

---

### Persona 2 · Agente de Fidelização (Empreendedor com HAF)

> "Quero construir minha Rede Proprietária e gerar Renda por Minuto trabalhando com Social Commerce."

- Profissional focado na **Jornada Penta Meta Minuto** e na geração de Renda por Minuto
- Possui uma **Licença HAF** (Habilitação para Agente de Fidelização) em um dos níveis:
  - Vision · Shu Ra Ri · Disruptiva · Master Quantum
- Ao adquirir a HAF, ativa oficialmente sua **ADF (Agência Digital de Fidelização)**
- Constrói ativamente sua Rede Proprietária, gera links rastreáveis de Social Commerce e recebe comissões e bônus residuais
- O Agente **sempre é um Consumidor primeiro** — tem acesso a tudo que o Membro Base tem
- Rota: `/agencia/painel`
- Campo no banco: `Usuario.Perfil = 'A'`, `Usuario.Empreendedor = true` + pedido de licença HAF ativo

---

### Persona 3 · Empresa Parceira (Lojista — ZEE DIGITAL)

> "Quero vender mais, fidelizar meus clientes com cashback e construir minha própria rede de consumidores recorrentes."

- Focado em **aumentar vendas e ROI** por meio da fidelização com cashback
- Oferece cashback configurável aos seus clientes e constrói sua própria Rede Proprietária de compradores fidelizados
- A ZEE DIGITAL funciona como um mini-ecossistema autônomo de prosperidade local
- Rota futura: `/zee/painel`
- Campo no banco: `Usuario.Perfil = 'L'`, `Credenciamento` ativo

---

## 2. Histórias de Usuário

> Foco no Sprint Atual: **Social Commerce e Dashboards**
> Formato ágil: _Como [Persona], eu quero [Ação] para que [Benefício]._

| ID | Persona | Eu quero… | Para que… | Critério de aceite |
|----|---------|-----------|-----------|-------------------|
| **US-01** | Consumidor ou Agente | copiar meu Link de Indicação Rastreável com **1 clique** em um botão com o ícone do WhatsApp | possa colar facilmente nos meus grupos e atrair novos leads | Botão presente no dashboard; ao clicar, link copia para clipboard; feedback visual de "Copiado!" |
| **US-02** | Consumidor (sem HAF) | ver um **aviso claro** de que preciso adquirir uma Licença HAF ao tentar acessar funcionalidades de rede | entenda o valor de fazer o upgrade e me tornar Agente com acesso ao dashboard de rede completa | Paywall exibido quando `Empreendedor = false`; botão "Quero ser Agente" redireciona para planos HAF |
| **US-03** | Agente de Fidelização | visualizar cards com métricas de **Cliques no Link**, **Novos Leads** e **Cashback Residual Gerado** | possa medir a eficiência do meu funil de conversão no WhatsApp | Cards visíveis apenas para Agentes com HAF ativa; valores reais ou mock inicial |
| **US-04** | Empresa Parceira (ZEE DIGITAL) | visualizar o **tamanho da minha Rede Proprietária** e o total de **Cashback Distribuído** | saiba o impacto da plataforma nas minhas vendas e tome decisões de negócio | Dashboard ZEE exibe contador de clientes fidelizados e total de cashback pago no período |
| **US-05** | Qualquer membro (Consumidor, Agente ou Lojista) | visualizar meu **status da Assinatura Plus** e uma **barra de progresso** da meta de consumo mínimo dinâmico | saiba exatamente o que fazer para destravar meu Cashback Residual Dobrado | Badge de status (Ativo/Pendente) e barra de progresso lendo meta de `adminConfig.metaConsumoPlus` (configurável pelo Admin, não hardcoded) |

---

## 3. Regras de Negócio Críticas (Resumo)

| Regra | Detalhe |
|-------|---------|
| **Paywall HAF** | Funcionalidades avançadas de rede (links rastreáveis, dashboard de comissões, residual multinível) exigem `Usuario.Empreendedor = true` + Licença HAF válida |
| **Assinatura Plus — meta dinâmica** | O valor da meta de consumo mínimo mensal para destravar o Cashback Dobrado é configurável pelo Painel Admin (`adminConfig.metaConsumoPlus`). Nunca hardcoded no frontend |
| **Agente é Consumidor primeiro** | Todo Agente de Fidelização tem acesso completo às funcionalidades do Membro Base |
| **ZEE DIGITAL — rota futura** | A rota `/zee` ainda não existe; o desenvolvimento das telas ZEE seguirá nomenclatura oficial após criação do módulo |

---

*Este arquivo é o guia de produto para o time de desenvolvimento. Atualizar sempre que novas jornadas forem mapeadas ou regras de negócio forem alteradas.*
