<template>
  <section class="qs-faq" aria-labelledby="qs-faq-title">
    <div class="container">
      <div class="qs-faq__header">
        <span class="qs-faq__label">DÚVIDAS FREQUENTES</span>
        <h2 id="qs-faq-title" class="qs-faq__title">Perguntas e respostas</h2>
        <p class="qs-faq__sub">Tudo que você precisa saber antes de começar.</p>
      </div>

      <div class="qs-faq__list">
        <details class="qs-faq__item" v-for="(item, i) in faqs" :key="i">
          <summary class="qs-faq__q">
            <span>{{ item.q }}</span>
            <span class="qs-faq__chevron" aria-hidden="true">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="m6 9 6 6 6-6"/></svg>
            </span>
          </summary>
          <div class="qs-faq__a">
            <p>{{ item.a }}</p>
          </div>
        </details>
      </div>

      <div class="qs-faq__more">
        <p>Ainda tem dúvidas?</p>
        <nuxt-link href="/contato" class="qs-faq__link">Fale com a gente →</nuxt-link>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
const faqs = [
  {
    q: 'O que é a Quanta Shop?',
    a: 'É uma plataforma de cashback em rede: você compra nas suas lojas favoritas e recebe dinheiro de volta, podendo ainda ganhar com a sua rede de consumo.',
  },
  {
    q: 'Como funciona a Quanta IA?',
    a: 'Pelo chat você busca e cota produtos e lojas, consulta seu saldo de cashback e tira qualquer dúvida — a mesma praticidade de uma conversa no WhatsApp.',
  },
  {
    q: 'O cashback é dinheiro de verdade?',
    a: 'Sim. O valor acumulado pode ser sacado via PIX, de forma transparente e instantânea. Não é ponto, não é crédito — é dinheiro de volta na sua conta.',
  },
  {
    q: 'Quanto custa para usar?',
    a: 'Criar conta e receber cashback é grátis. Há um plano opcional (Quanta Plus) com cashback turbinado e benefícios exclusivos para quem quer maximizar os ganhos.',
  },
  {
    q: 'Como recebo e saco meu cashback?',
    a: 'O cashback cai automaticamente na sua carteira Quanta após a compra confirmada. O saque é via PIX, direto pela plataforma — sem burocracia.',
  },
];
</script>

<style scoped>
.qs-faq { padding: 80px 0; background: #f9fafb; }
.container { width: 100%; max-width: 800px; margin: 0 auto; padding: 0 24px; }

.qs-faq__header { text-align: center; margin-bottom: 48px; }
.qs-faq__label { display: inline-block; font-family: 'Inter','Jost',sans-serif; font-size: 11px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #2F7785; margin-bottom: 12px; }
.qs-faq__title { font-family: 'Jost','Inter',sans-serif; font-size: clamp(26px,4vw,38px); font-weight: 800; color: #225F6B; letter-spacing: -.02em; margin-bottom: 10px; }
.qs-faq__sub { font-family: 'Inter','Jost',sans-serif; font-size: 16px; color: #6b7280; }

.qs-faq__list { display: flex; flex-direction: column; gap: 0; }

.qs-faq__item { border-bottom: 1px solid #e5e7eb; }
.qs-faq__item:first-child { border-top: 1px solid #e5e7eb; }

.qs-faq__q { display: flex; align-items: center; justify-content: space-between; gap: 16px; list-style: none; cursor: pointer; padding: 22px 4px; font-family: 'Jost','Inter',sans-serif; font-size: 17px; font-weight: 700; color: #225F6B; user-select: none; transition: color .2s; }
.qs-faq__q::-webkit-details-marker { display: none; }
.qs-faq__q:hover { color: #2F7785; }

.qs-faq__chevron { flex-shrink: 0; color: #9ca3af; transition: transform .25s; }
.qs-faq__item[open] .qs-faq__chevron { transform: rotate(180deg); }
.qs-faq__item[open] .qs-faq__q { color: #2F7785; }

.qs-faq__a { padding: 0 4px 22px; }
.qs-faq__a p { font-family: 'Inter','Jost',sans-serif; font-size: 15px; color: #6b7280; line-height: 1.7; margin: 0; }

.qs-faq__more { display: flex; align-items: center; gap: 10px; justify-content: center; margin-top: 40px; font-family: 'Inter','Jost',sans-serif; font-size: 15px; color: #6b7280; }
.qs-faq__link { font-weight: 700; color: #2F7785; text-decoration: none; transition: color .2s; }
.qs-faq__link:hover { color: #225F6B; }

@media (prefers-reduced-motion: reduce) {
  .qs-faq__q, .qs-faq__chevron { transition: none; }
}
</style>
