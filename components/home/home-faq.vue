<template>
  <section class="qs-faq" aria-labelledby="qs-faq-title">
    <div class="qs-faq__container">
      <div class="qs-faq__head">
        <span class="qs-faq__eyebrow">Tire suas dúvidas</span>
        <h2 id="qs-faq-title" class="qs-faq__h2">Perguntas e respostas</h2>
        <p class="qs-faq__lead">Respostas rápidas para começar bem.</p>
      </div>

      <div class="qs-faq__list">
        <details v-for="(item, i) in faqs" :key="i" class="qs-faq__item">
          <summary class="qs-faq__q">
            <span>{{ item.q }}</span>
            <span class="qs-faq__icon" aria-hidden="true">+</span>
          </summary>
          <p class="qs-faq__a">{{ item.a }}</p>
        </details>
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
    q: 'O cashback é dinheiro de verdade?',
    a: 'Sim. O valor acumulado pode ser sacado via PIX, de forma transparente e instantânea.',
  },
  {
    q: 'Como recebo e saco meu cashback?',
    a: 'O cashback cai automaticamente na sua carteira Quanta após a compra confirmada. O saque é via PIX, direto pela plataforma.',
  },
];
</script>

<style scoped>
.qs-faq {
  padding: 80px 0;
  background: #fff;
}
.qs-faq__container {
  width: 100%;
  max-width: 780px;
  margin: 0 auto;
  padding: 0 24px;
}

.qs-faq__head {
  text-align: center;
  margin-bottom: 44px;
}
.qs-faq__eyebrow {
  font-size: 12px;
  font-weight: 700;
  letter-spacing: .16em;
  text-transform: uppercase;
  color: #3A9AAD;
}
.qs-faq__h2 {
  font-family: 'Jost','Inter',sans-serif;
  font-size: clamp(28px, 3.2vw, 40px);
  font-weight: 700;
  color: #225F6B;
  line-height: 1.1;
  letter-spacing: -.02em;
  margin: 10px 0 12px;
}
.qs-faq__lead {
  font-family: 'Inter','Jost',sans-serif;
  font-size: 16px;
  color: #6b7280;
  margin: 0;
}

.qs-faq__list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.qs-faq__item {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 16px;
  overflow: hidden;
  transition: box-shadow .2s ease, border-color .2s ease;
}
.qs-faq__item:hover {
  box-shadow: 0 4px 16px rgba(1,15,28,.08);
}
.qs-faq__item[open] {
  box-shadow: 0 4px 20px rgba(1,15,28,.1);
  border-color: #c8dde0;
}

.qs-faq__q {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  padding: 20px 24px;
  cursor: pointer;
  list-style: none;
  font-family: 'Jost','Inter',sans-serif;
  font-size: 16px;
  font-weight: 600;
  color: #225F6B;
  user-select: none;
  margin: 0;
}
.qs-faq__q::-webkit-details-marker { display: none; }
.qs-faq__q::marker { display: none; }

.qs-faq__icon {
  font-size: 22px;
  font-weight: 300;
  color: #2F7785;
  line-height: 1;
  flex-shrink: 0;
  transition: transform .25s ease;
  display: inline-block;
}
.qs-faq__item[open] .qs-faq__icon {
  transform: rotate(45deg);
}

.qs-faq__a {
  font-family: 'Inter','Jost',sans-serif;
  font-size: 15px;
  color: #4b5563;
  line-height: 1.7;
  margin: 0;
  padding: 4px 24px 22px;
  border-top: 1px solid #f0f0f0;
  background: none;
}

@media (prefers-reduced-motion: reduce) {
  .qs-faq__icon { transition: none; }
  .qs-faq__item { transition: none; }
}
</style>
