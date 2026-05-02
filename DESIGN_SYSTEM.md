# DESIGN_SYSTEM.md — Quanta Shop Premium

> Padrão Visual Premium e Minimalista da plataforma Quanta Shop.
> Versão 1.0 — Mai 2026 · _"Confiança financeira em cada pixel."_

---

## 1. Filosofia de Design

A Quanta Shop é uma **plataforma premium de fidelização com cashback em rede e consumo inteligente**. A interface deve transmitir **confiança financeira**, **clareza** e **eficiência operacional**. Inspiramo-nos em três referências:

| Referência | O que herdamos |
|------------|----------------|
| **Apple HIG** | Hierarquia tipográfica, generosidade de espaço, animações sutis |
| **Stripe Dashboard** | Densidade de dados sem ruído, badges discretos, microcopy preciso |
| **Q Cuida (app)** | Listas verticais com checkmarks, barras de progresso lineares, filtros por chip |

**Mandamentos inegociáveis**

1. **Respiro acima de densidade.** Whitespace é parte do conteúdo.
2. **Um foco por tela.** Cada página tem **um** call-to-action principal.
3. **Tipografia conta a história.** Tamanho > peso > cor. Negrito é informação, não decoração.
4. **Cor é semântica.** Lima = sucesso/positivo. Teal = institucional. Vermelho = somente erro real.
5. **Movimento é intenção.** Toda animação tem propósito (feedback, transição de estado).
6. **Mobile é tratamento de primeira classe** — desenhe primeiro o iPhone SE, depois o desktop.

---

## 2. Paleta Oficial (Tokens)

Apenas estas cinco cores compõem a identidade. **Proibido** introduzir novas matizes sem aprovação.

| Token | Hex | Uso |
|-------|-----|-----|
| `--qs-teal` | `#2F7785` | Cor institucional, links, ícones primários |
| `--qs-teal-dark` | `#225F6B` | Títulos, headers, hover de teal |
| `--qs-lime` | `#98C73A` | Sucesso, cashback, badges positivas, CTAs secundários |
| `--qs-bg` | `#F4F4F5` | Fundo de painel, fundo de seções |
| `--qs-ink` | `#1d1d1f` | Texto principal sobre fundo claro |

**Cinzas auxiliares** (escala neutra, derivadas — não são cor de marca)

```css
--qs-gray-50:  #fafafa;   /* fundo de hover */
--qs-gray-100: #f5f5f7;   /* divisores em fundo branco */
--qs-gray-200: #e5e7eb;   /* borders sutis */
--qs-gray-400: #9ca3af;   /* texto secundário */
--qs-gray-500: #6b7280;   /* metadados, captions */
--qs-gray-700: #374151;   /* texto de body sobre fundo cinza */
```

**Estados (mínimo absoluto)**

```css
--qs-success: #16a34a;  /* verde para confirmação inline */
--qs-warn:    #d97706;  /* alerta */
--qs-danger:  #dc2626;  /* erro real, somente em mensagens */
```

> ⚠️ **Lima ≠ Success.** Lima é cor de **marca** (cashback, identidade). Verde-success é apenas para mensagens de sistema.

---

## 3. Tipografia

**Família única:** `Inter` (300 / 400 / 500 / 600 / 700 / 800).
Anti-aliasing obrigatório (`-webkit-font-smoothing: antialiased`).

**Escala (mobile-first, escalonando até +25% no desktop)**

| Token | Mobile | Desktop | Peso | Letter-spacing | Uso |
|-------|--------|---------|------|----------------|-----|
| `--text-display` | 40px | 56px | 800 | -0.03em | Hero |
| `--text-h1` | 28px | 36px | 700 | -0.02em | Página |
| `--text-h2` | 22px | 28px | 700 | -0.015em | Seção |
| `--text-h3` | 18px | 20px | 600 | -0.01em | Card / sub-seção |
| `--text-body` | 15px | 16px | 400 | 0 | Parágrafo |
| `--text-meta` | 13px | 14px | 500 | 0 | Metadados, labels |
| `--text-micro` | 11px | 12px | 600 | 0.04em (UPPERCASE) | Eyebrow, badges |

**Regras**

- Línea de leitura **máximo 72 caracteres** por linha em parágrafos.
- Line-height: títulos `1.1`, body `1.55`, captions `1.4`.
- Nunca use `text-align: justify`.

---

## 4. Espaçamento (Sistema 4px)

```css
--space-1:  4px;
--space-2:  8px;
--space-3:  12px;
--space-4:  16px;
--space-5:  24px;
--space-6:  32px;
--space-7:  48px;
--space-8:  64px;
--space-9:  96px;
--space-10: 128px;
```

**Padding mínimo**

- Card: `--space-5` mobile / `--space-6` desktop
- Section: `--space-8` mobile / `--space-9` desktop
- Linha de lista: `--space-4` vertical

---

## 5. Border-radius e Sombras

```css
--qs-radius-sm:   6px;   /* badges, tags */
--qs-radius-md:   10px;  /* botões, inputs */
--qs-radius-lg:   16px;  /* cards */
--qs-radius-xl:   24px;  /* sections destacadas */
--qs-radius-pill: 999px; /* pills, chips, avatares */

--qs-shadow-xs: 0 1px 2px rgba(15, 23, 42, .04);
--qs-shadow-sm: 0 2px 8px rgba(15, 23, 42, .06);
--qs-shadow-md: 0 8px 24px rgba(15, 23, 42, .08);
--qs-shadow-lg: 0 20px 60px rgba(15, 23, 42, .10);
```

**Regra**: cards do painel admin usam `--qs-shadow-sm` em estado normal; `--qs-shadow-md` em hover.

---

## 6. Componentes do Painel Admin (Premium)

A partir desta especificação, o admin **deixa de ser confuso** e passa a seguir layouts canônicos.

### 6.1 Estrutura da página

```
┌─────────────────────────────────────────────┐
│  EYEBROW (uppercase 11px)                    │
│  H1 Página                       [Ação]     │  ← page header
│  Sub-line de contexto / breadcrumb          │
├─────────────────────────────────────────────┤
│                                              │
│  [ KPI 1 ]  [ KPI 2 ]  [ KPI 3 ]            │  ← KPI strip (opcional)
│                                              │
├─────────────────────────────────────────────┤
│                                              │
│  Conteúdo principal (cards / tabela / lista)│
│                                              │
└─────────────────────────────────────────────┘
```

### 6.2 Classes canônicas (já existem em `assets/scss/agencia.scss`)

| Classe | Função |
|--------|--------|
| `.ag-page-header` | Header da página: H1 + descrição |
| `.ag-card` | Card branco com `--qs-shadow-sm` e `--qs-radius-lg` |
| `.ag-table` | Tabela leve, header em `--qs-bg`, linhas com hover |
| `.ag-loading` | Spinner centralizado |
| `.ag-empty-state` | Estado vazio com ícone + texto + CTA |
| `.badge-ag` (`-success`, `-warning`, `-danger`) | Badges arredondadas |
| `.btn-ag-primary`, `.btn-ag-outline` | Botões padrão |

### 6.3 Componentes novos do Premium (a serem usados)

| Componente | Quando usar |
|------------|-------------|
| **KPI Card** | Métrica única (R$, %, contagem) com label, número grande e delta |
| **Progress Bar Linear** | Indicador de % concluído (Q Cuida-style) — altura `8px`, fundo `--qs-gray-200`, fill `--qs-lime` |
| **Filter Chips** | Filtros por categoria. Chip ativo: fundo `--qs-teal`, texto branco; chip inativo: fundo `--qs-gray-50`, borda `--qs-gray-200` |
| **List Item com Checkmark** | Linha de tarefa/feature: ícone de status à esquerda + título + badge à direita |
| **Section Tabs** | Navegação por aba dentro de uma página (subordinada ao menu lateral) |

---

## 7. Padrões de Interação

### 7.1 Botões

```
┌──────────────────────┐   Primário (teal sólido)
│   Salvar alterações  │   bg: --qs-teal, txt: #fff, radius: md
└──────────────────────┘   hover: --qs-teal-dark + shadow-sm

┌──────────────────────┐   Secundário (outline teal)
│   Cancelar           │   border: --qs-teal, txt: --qs-teal
└──────────────────────┘   hover: bg --qs-teal, txt #fff

┌──────────────────────┐   Sucesso (lima sólido — APENAS para confirmação positiva)
│   Aprovar saque      │   bg: --qs-lime, txt: --qs-ink
└──────────────────────┘
```

**Tamanhos**

- `sm`: padding `8px 12px`, font `13px`
- `md` (padrão): padding `10px 18px`, font `14px`
- `lg`: padding `14px 24px`, font `16px`

### 7.2 Inputs

- Borda `1px solid --qs-gray-200`, radius `--qs-radius-md`, padding `10px 14px`.
- Focus: borda `--qs-teal`, sombra `0 0 0 3px rgba(47,119,133,.12)`.
- Label sempre **acima** do input (nunca placeholder como label).
- Mensagem de erro: 12px, `--qs-danger`, abaixo do input.

### 7.3 Animações

```css
--qs-ease:     cubic-bezier(0.4, 0, 0.2, 1);
--qs-duration: 200ms;
```

- Hover de cards: `transform: translateY(-2px)` + sombra md.
- Mudança de filtro: `opacity` fade `200ms`, **sem** `display:none`.
- Loading: skeleton com gradiente animado (`--qs-skeleton`), nunca spinners centralizados em telas inteiras.

---

## 8. Iconografia

- Família única: **Material Symbols Outlined** (já incluído via Bootstrap Icons como fallback).
- Tamanhos canônicos: 16, 20, 24, 28, 32 px.
- Cor: herdar `currentColor`. Nunca aplicar gradientes em ícones.

---

## 9. Acessibilidade (WCAG AA mínimo)

1. Contraste mínimo **4.5:1** texto sobre fundo. Lima `#98C73A` sobre branco **NÃO** atende — use lima apenas em fundos escuros ou em badges com fundo claro semitransparente.
2. Toda interação clicável tem `aria-label` ou texto visível.
3. Foco sempre visível (`outline: 2px solid --qs-teal; outline-offset: 2px`).
4. Hierarquia semântica: nunca pule de `h1` para `h3`.

---

## 10. Anti-Padrões (Proibido)

- ❌ Cores fora da paleta oficial.
- ❌ Sombras pretas duras (`box-shadow: 0 4px 8px #000`).
- ❌ Ícones em emoji em produção (use SVG/Material).
- ❌ `text-align: justify`.
- ❌ Animações maiores que 400ms para feedback de UI.
- ❌ Botões em cor `--qs-danger` para ações reversíveis (cancelar ≠ excluir).
- ❌ Modais sobrepostos (no máximo 1 modal aberto por vez).
- ❌ Tabelas com mais de 7 colunas — converta em cards ou colapse colunas secundárias.

---

## 11. Onde implementar

| Camada | Arquivo |
|--------|---------|
| Tokens CSS | `assets/scss/quanta-premium.scss` (já existe — expandir conforme tabela acima) |
| Classes admin | `assets/scss/agencia.scss` |
| Componentes Vue compartilhados | `components/qs/Qs*.vue` |
| Tela de referência | `pages/agencia/painel/admin/features.vue` (Q Cuida-style) |

---

*Mantido pela equipe de produto Quanta Shop. Atualize este arquivo a cada novo padrão visual aprovado.*
