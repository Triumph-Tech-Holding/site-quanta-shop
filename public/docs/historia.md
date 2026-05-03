# Histórias de Usuário — Quanta Shop

> Guia oficial de Personas e Histórias de Usuário da Quanta Shop.
> Alinhado com a arquitetura de negócios definida em `CLAUDE.md` e `FEATURES.md`.
> Versão 2.0 — Mai 2026

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
- Constrói ativamente sua Rede Proprietária, gera links rastreáveis e recebe comissões residuais
- O Agente **sempre é um Consumidor primeiro**
- Rota: `/agencia/painel`
- Campo no banco: `Usuario.Perfil = 'A'`, `Usuario.Empreendedor = true`

---

### Persona 3 · Empresa Parceira (Lojista — ZEE DIGITAL)

> "Quero vender mais, fidelizar meus clientes com cashback e construir minha própria rede de consumidores recorrentes."

- Focado em **aumentar vendas e ROI** por meio da fidelização com cashback
- Oferece cashback configurável aos seus clientes
- A ZEE DIGITAL funciona como um mini-ecossistema autônomo de prosperidade local
- Rota futura: `/zee/painel`
- Campo no banco: `Usuario.Perfil = 'L'`, `Credenciamento` ativo

---

### Persona 4 · Admin / Gestor da Plataforma

> "Preciso controlar toda a operação: usuários, pagamentos, rede, cashback e documentação técnica."

- Acessa o Painel Admin (`/agencia/painel/admin`) e o LAB (`/lab`)
- Configura percentuais de rede, aprova saques, gera relatórios LGPD
- Usa o LAB como cockpit técnico de engenharia e governança
- Campo no banco: `Usuario.Master = true` ou `IdGrupo = Admin`

---

## 2. Histórias de Usuário — MVP 1 & 2 (Implementadas)

> Formato ágil: _Como [Persona], eu quero [Ação] para que [Benefício]._

| ID | Persona | Eu quero… | Para que… | Status |
|----|---------|-----------|-----------|--------|
| **US-01** | Consumidor / Agente | copiar meu Link de Indicação com 1 clique (botão WhatsApp) | possa divulgar facilmente | ✅ Done |
| **US-02** | Consumidor (sem HAF) | ver aviso claro ao tentar acessar funcionalidades de rede | entenda o valor do upgrade HAF | ✅ Done |
| **US-03** | Agente de Fidelização | visualizar cards com métricas de Cliques, Leads e Cashback Residual | meça a eficiência do meu funil | ✅ Done |
| **US-04** | Empresa Parceira (ZEE) | visualizar tamanho da minha Rede Proprietária e Cashback Distribuído | saiba o impacto nas minhas vendas | 🔵 Planned |
| **US-05** | Qualquer membro | visualizar status da Assinatura Plus e barra de progresso da meta | saiba o que fazer para destravar o Cashback Dobrado | ✅ Done |
| **US-06** | Consumidor | buscar parceiros por cashback mínimo e proximidade geográfica | encontre as melhores ofertas perto de mim | ✅ Done |
| **US-07** | Consumidor | usar cupom de desconto no checkout | economize ainda mais na compra | ✅ Done |
| **US-08** | Consumidor | usar meus Quanta Points como desconto no checkout | resgate benefícios acumulados | ✅ Done |
| **US-09** | Admin | mascarar dados sensíveis (CPF, conta) por padrão nos relatórios | esteja em conformidade com LGPD | ✅ Done |
| **US-10** | Admin (Master) | revelar dado sensível com auditoria de quem acessou | mantenha rastreabilidade LGPD | ✅ Done |

---

## 3. Histórias de Usuário — MVP 3 (Em Andamento / Planejado)

| ID | Feature | Persona | Eu quero… | Para que… | Status |
|----|---------|---------|-----------|-----------|--------|
| **US-301** | F-303 | Consumidor | convidar amigos pelo meu link único de Quanta Amizade | ganhe recompensa quando eles atingirem o objetivo | 🟡 50% |
| **US-302** | F-303 | Consumidor | receber notificação push quando meu afilhado atingir progresso | saiba em tempo real sobre meu bônus | 🔵 Planned |
| **US-303** | F-306 | Agente | solicitar saque PIX com confirmação visual | receba meu saldo de forma segura e rápida | 🟡 60% |
| **US-304** | F-307 | Agente | baixar cards e vídeos prontos de marketing | divulgue minha ADF sem precisar criar material do zero | 🟡 40% |
| **US-305** | F-302 | Agente | visualizar minha rede em árvore interativa com zoom | entenda a estrutura da minha Rede Proprietária | 🔵 Planned |
| **US-306** | F-301 | Agente | acompanhar minha graduação atual e os requisitos da próxima | saiba exatamente o que falta para avançar de nível | 🔵 Planned |
| **US-307** | F-305 | Todos | assinar o plano Plus com débito recorrente (Asaas) | destrave Cashback Residual Dobrado automaticamente | 🔵 Planned |
| **US-308** | F-308 | Lojista (ZEE) | definir cashback variável por horário e dia da semana | atraia clientes em períodos de menor movimento (happy hour) | 🔵 Planned |

---

## 4. Histórias de Usuário — MVP 4 (Backlog)

| ID | Feature | Persona | Eu quero… | Para que… |
|----|---------|---------|-----------|-----------|
| **US-401** | F-401 | Consumidor | receber recomendações de parceiros baseadas no meu histórico | descubra lojas relevantes sem precisar buscar |
| **US-402** | F-402 | Lojista | ver dashboard de faturamento com ticket médio e cohort de recompra | tome decisões de negócio baseadas em dados |
| **US-403** | F-403 | Consumidor / Agente | usar o app mobile nativo com push notifications e QR code | acesse minha carteira e parceiros em qualquer lugar |
| **US-404** | F-404 | Todos | conversar com o Assistente Quanta via chat | tire dúvidas sobre saldo, indicações e parceiros sem abrir chamado |

---

## 5. Regras de Negócio Críticas

| Regra | Detalhe |
|-------|---------|
| **Paywall HAF** | Funcionalidades avançadas de rede exigem `Usuario.Empreendedor = true` + Licença HAF válida. Nunca expor a Consumidores base. |
| **Assinatura Plus — meta dinâmica** | Valor da meta de consumo mínimo configurável pelo Admin (`adminConfig.metaConsumoPlus`). Nunca hardcoded no frontend. |
| **Agente é Consumidor primeiro** | Todo Agente tem acesso completo às funcionalidades do Membro Base |
| **Split de Cashback** | 50% Quanta Shop · 25% Consumidor (10% retido como sustentabilidade) · 25% Rede (12 níveis) |
| **Compressão Dinâmica** | Uplines inativos são pulados. Bônus reverte para Quanta Shop se nenhum ativo encontrado. |
| **Multiplicador Plus** | Aplica-se EXCLUSIVAMENTE ao bônus residual (rede). Nunca ao cashback direto da própria compra. |
| **LGPD — mínimo necessário** | Não pedir RG se não for credenciamento. CPF mascarado na UI. Nunca logar dados sensíveis. |
| **BTC descontinuado** | Saques ocorrem exclusivamente em moeda corrente (PIX / TED). |
| **ZEE DIGITAL — rota futura** | A rota `/zee` ainda não existe. Desenvolvimento das telas ZEE seguirá após criação do módulo. |
| **LAB — somente devs/gestores** | O cockpit técnico `/lab` usa middleware `agencia-auth + agencia-admin`. Nunca aparece para usuário final. |

---

## 6. Critério de Done (por história)

Para qualquer história ser considerada **Aceita**:

1. ✅ Implementada no frontend (Nuxt 3) e backend (.NET 8) conforme especificado
2. ✅ Testa os critérios de aceite descritos na história
3. ✅ Paywall funcional — usuários sem permissão não veem a funcionalidade
4. ✅ Layout responsivo testado em iPhone SE (375×667)
5. ✅ Dados sensíveis mascarados ou não expostos (LGPD)
6. ✅ Nenhum valor de negócio hardcoded no frontend

---

*Este arquivo é o guia de produto para o time de desenvolvimento. Atualizar sempre que novas jornadas forem mapeadas ou regras de negócio forem alteradas.*
