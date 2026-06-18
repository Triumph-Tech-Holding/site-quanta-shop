<template>
  <section class="qs-cap" aria-labelledby="qs-cap-title">
    <div class="container">
      <div class="qs-cap__head">
        <span class="qs-cap__eyebrow">Tudo pelo chat</span>
        <h2 id="qs-cap-title" class="qs-cap__h2">Uma conversa resolve</h2>
        <p class="qs-cap__lead">A Quanta IA reúne busca, carteira e suporte num só lugar — fale como você falaria no WhatsApp.</p>
      </div>

      <div class="qs-cap__grid">
        <article v-for="(c, i) in caps" :key="i" class="qs-cap__card">
          <div class="qs-cap__ic" v-html="c.icon" />
          <h3 class="qs-cap__title">{{ c.title }}</h3>
          <p class="qs-cap__text">{{ c.text }}</p>
        </article>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Cap { title: string; text: string; icon: string }

const caps: Cap[] = [
  {
    title: 'Buscar & cotar',
    text: 'Encontre produtos e lojas por nome, categoria ou proximidade e receba a cotação com o cashback já calculado.',
    icon: '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="7"/><path d="m21 21-4.3-4.3"/></svg>',
  },
  {
    title: 'Consultar saldo & sacar',
    text: 'Veja seu saldo disponível e o reservado, acompanhe a rede e resgate via PIX sem sair da conversa.',
    icon: '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="2" y="5" width="20" height="14" rx="3"/><path d="M2 10h20"/></svg>',
  },
  {
    title: 'Tirar dúvidas 24/7',
    text: 'Do credenciamento ao primeiro saque: pergunte qualquer coisa e tenha resposta clara, na hora.',
    icon: '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M21 15a4 4 0 0 1-4 4H8l-5 4V7a4 4 0 0 1 4-4h10a4 4 0 0 1 4 4z"/></svg>',
  },
];
</script>

<style scoped>
.qs-cap { padding: 84px 0; background: #f7f8fa; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-cap__head { text-align: center; max-width: 680px; margin: 0 auto 52px; }
.qs-cap__eyebrow { font-size: 12px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #3A9AAD; }
.qs-cap__h2 { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: clamp(30px, 3.6vw, 44px); font-weight: 700; color: #225F6B; line-height: 1.08; letter-spacing: -.02em; margin: 12px 0 14px; }
.qs-cap__lead { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 17px; color: #6b7280; }

.qs-cap__grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 22px; }
.qs-cap__card { background: #fff; border: 1px solid #e5e7eb; border-radius: 20px; padding: 30px; transition: transform .25s ease, box-shadow .25s ease; }
.qs-cap__card:hover { transform: translateY(-4px); box-shadow: 0 4px 20px rgba(1,15,28,.12); }
.qs-cap__ic { width: 54px; height: 54px; border-radius: 16px; display: grid; place-items: center; color: #fff; margin-bottom: 18px; background: linear-gradient(135deg, #2F7785, #3A9AAD); }
.qs-cap__card:nth-child(2) .qs-cap__ic { background: linear-gradient(135deg, #7aad1f, #98C73A); }
.qs-cap__card:nth-child(3) .qs-cap__ic { background: linear-gradient(135deg, #FFB342, #f59e0b); }
.qs-cap__ic :deep(svg) { width: 26px; height: 26px; }
.qs-cap__title { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: 20px; color: #225F6B; margin-bottom: 8px; }
.qs-cap__text { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 15px; color: #6b7280; }

@media (max-width: 880px) { .qs-cap__grid { grid-template-columns: 1fr; } }
@media (prefers-reduced-motion: reduce) { .qs-cap__card { transition: none; } }
</style>
