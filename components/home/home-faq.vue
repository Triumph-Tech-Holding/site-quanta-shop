<template>
  <section class="qs-faq" aria-labelledby="qs-faq-title">
    <div class="container">
      <div class="qs-faq__head">
        <span class="qs-faq__eyebrow">Dúvidas frequentes</span>
        <h2 id="qs-faq-title" class="qs-faq__h2">Perguntas e respostas</h2>
        <p class="qs-faq__lead">O essencial pra você começar com confiança.</p>
      </div>

      <div class="qs-faq__list">
        <details v-for="(f, i) in items" :key="i" class="qs-faq__item" :open="i === 0">
          <summary class="qs-faq__q">
            {{ f.q }}
            <span class="qs-faq__pl" aria-hidden="true">+</span>
          </summary>
          <div class="qs-faq__a">{{ f.a }}</div>
        </details>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Faq { q: string; a: string }

const items: Faq[] = [
  { q: 'O que é a Quanta Shop?', a: 'É uma plataforma de cashback em rede: você compra nas suas lojas favoritas e recebe dinheiro de volta, podendo ainda ganhar com a sua rede de consumo.' },
  { q: 'Como funciona a Quanta IA?', a: 'Pelo chat você busca e cota produtos e lojas, consulta seu saldo de cashback e tira qualquer dúvida — a mesma praticidade de uma conversa no WhatsApp.' },
  { q: 'O cashback é dinheiro de verdade?', a: 'Sim. O valor acumulado pode ser sacado via PIX, de forma transparente e instantânea.' },
  { q: 'Quanto custa para usar?', a: 'Criar conta e receber cashback é grátis. Há um plano opcional (Quanta Plus) com cashback turbinado e benefícios exclusivos.' },
  { q: 'Como recebo e saco meu cashback?', a: 'O cashback cai automaticamente na sua carteira Quanta após a compra confirmada. O saque é via PIX, direto pela plataforma.' },
];
</script>

<style scoped>
.qs-faq { padding: 84px 0; background: #fff; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-faq__head { text-align: center; max-width: 680px; margin: 0 auto 52px; }
.qs-faq__eyebrow { font-size: 12px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #3A9AAD; }
.qs-faq__h2 { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: clamp(30px, 3.6vw, 44px); font-weight: 700; color: #225F6B; line-height: 1.08; letter-spacing: -.02em; margin: 12px 0 14px; }
.qs-faq__lead { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 17px; color: #6b7280; }

.qs-faq__list { max-width: 780px; margin: 0 auto; }
.qs-faq__item { background: #fff; border: 1px solid #e5e7eb; border-radius: 12px; margin-bottom: 12px; overflow: hidden; }
.qs-faq__q { list-style: none; cursor: pointer; padding: 18px 22px; min-height: 44px; font-family: 'Bruum FY','Jost','Inter',sans-serif; font-weight: 600; font-size: 17px; color: #225F6B; display: flex; justify-content: space-between; align-items: center; gap: 12px; }
.qs-faq__q::-webkit-details-marker { display: none; }
.qs-faq__pl { flex-shrink: 0; width: 26px; height: 26px; border-radius: 50%; background: rgba(47,119,133,.1); color: #2F7785; display: grid; place-items: center; font-size: 18px; line-height: 1; transition: transform .25s ease, background .25s ease, color .25s ease; }
.qs-faq__item[open] .qs-faq__pl { transform: rotate(45deg); background: #2F7785; color: #fff; }
.qs-faq__a { padding: 0 22px 20px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; color: #6b7280; font-size: 15px; line-height: 1.65; }

@media (prefers-reduced-motion: reduce) { .qs-faq__pl { transition: none; } }
</style>
