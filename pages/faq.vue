<template>
  <main class="faq-page">
    <section class="hero">
      <div class="hero__grid"></div>
      <div class="container hero__in">
        <span class="eyebrow">Dúvidas frequentes</span>
        <h1>Perguntas e respostas</h1>
        <p>O essencial pra você começar com confiança.</p>
      </div>
    </section>

    <section class="faq">
      <div class="container reveal">
        <details class="item" open><summary class="q">O que é a Quanta Shop?<span class="pl">+</span></summary><div class="a">É uma plataforma de cashback em rede: você compra nas suas lojas favoritas e recebe dinheiro de volta, podendo ainda ganhar com a sua rede de consumo.</div></details>
        <details class="item"><summary class="q">Como funciona a Quanta IA?<span class="pl">+</span></summary><div class="a">Pelo chat você busca e cota produtos e lojas, consulta seu saldo de cashback e tira qualquer dúvida — a mesma praticidade de uma conversa no WhatsApp.</div></details>
        <details class="item"><summary class="q">O cashback é dinheiro de verdade?<span class="pl">+</span></summary><div class="a">Sim. O valor acumulado pode ser sacado via PIX, de forma transparente e instantânea.</div></details>
        <details class="item"><summary class="q">Quanto custa para usar?<span class="pl">+</span></summary><div class="a">Criar conta e receber cashback é grátis. Há um plano opcional (Quanta Plus) com cashback turbinado e benefícios exclusivos.</div></details>
        <details class="item"><summary class="q">Como recebo e saco meu cashback?<span class="pl">+</span></summary><div class="a">O cashback cai automaticamente na sua carteira Quanta após a compra confirmada. O saque é via PIX, direto pela plataforma.</div></details>
      </div>
    </section>

    <section class="cta">
      <div class="container reveal">
        <h2>Ainda com dúvidas?</h2>
        <p>Fale com a gente no atendimento — a Quanta IA responde na hora, como no WhatsApp.</p>
        <a class="btn btn--lime" href="#">Falar no atendimento →</a>
      </div>
    </section>
  </main>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' })
useSeoMeta({
  title: "Perguntas Frequentes | Quanta Shop",
  description: "Tire suas dúvidas sobre cashback, Quanta IA, saque via PIX e como começar na Quanta Shop.",
})
useHead({
  link: [
    { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
    { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' },
    { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&family=Jost:wght@400;500;600;700;800;900&display=swap' },
  ],
})

onMounted(() => {
  var io = new IntersectionObserver(function (es) {
    es.forEach(function (e) { if (e.isIntersecting) { e.target.classList.add('in'); io.unobserve(e.target); } });
  }, { threshold: .15 });
  document.querySelectorAll('.reveal').forEach(function (el) { io.observe(el); });
})
</script>

<style scoped>
.faq-page{--ink:#225F6B;--teal:#2F7785;--teal2:#3A9AAD;--lime:#98C73A;--lime-d:#7aad1f;--txt:#374151;--muted:#6b7280;--line:rgba(17,24,39,.08);}
*{box-sizing:border-box;} .faq-page{margin:0;font-family:'Inter',sans-serif;color:var(--txt);background:#fff;-webkit-font-smoothing:antialiased;}
.container{width:100%;max-width:860px;margin:0 auto;padding:0 24px;}
a{text-decoration:none;}
.reveal{opacity:0;transform:translateY(20px);transition:opacity .65s cubic-bezier(.16,1,.3,1),transform .65s cubic-bezier(.16,1,.3,1);}
.reveal.in{opacity:1;transform:none;}
.btn{display:inline-flex;align-items:center;gap:8px;font-family:'Kiye Sans','Inter',sans-serif;font-weight:700;font-size:15px;border-radius:999px;padding:14px 28px;transition:transform .2s,box-shadow .2s;}
.btn--lime{color:#173a0a;background:linear-gradient(180deg,var(--lime),var(--lime-d));box-shadow:0 10px 28px rgba(152,199,58,.35);}
.btn--lime:hover{transform:translateY(-2px);box-shadow:0 16px 36px rgba(152,199,58,.45);}
.eyebrow{font-size:12px;font-weight:800;letter-spacing:.16em;text-transform:uppercase;color:var(--teal2);}

.hero{position:relative;overflow:hidden;background:radial-gradient(700px 420px at 50% 0%,rgba(58,154,173,.28),transparent 62%),linear-gradient(150deg,#0e2a32,#16323a);color:#fff;padding:78px 0;text-align:center;}
.hero__grid{position:absolute;inset:0;background-image:radial-gradient(rgba(255,255,255,.06) 1px,transparent 1px);background-size:26px 26px;-webkit-mask-image:radial-gradient(60% 80% at 50% 10%,#000,transparent 75%);mask-image:radial-gradient(60% 80% at 50% 10%,#000,transparent 75%);}
.hero__in{position:relative;}
.hero .eyebrow{color:#bfe89a;}
.hero h1{font-family:'Jost','Inter',sans-serif;font-size:clamp(32px,4.6vw,52px);font-weight:800;margin:14px 0 0;}
.hero p{color:#cdd9dd;font-size:clamp(15px,1.8vw,18px);margin:16px auto 0;max-width:48ch;}

.faq{padding:72px 0;}
.item{background:#fff;border:1px solid #e5e7eb;border-radius:14px;margin-bottom:14px;overflow:hidden;transition:box-shadow .25s,border-color .25s;}
.item[open]{border-color:rgba(47,119,133,.35);box-shadow:0 12px 30px rgba(1,15,28,.07);}
.q{list-style:none;cursor:pointer;padding:20px 24px;min-height:44px;font-family:'Jost','Inter',sans-serif;font-weight:600;font-size:17.5px;color:var(--ink);display:flex;justify-content:space-between;align-items:center;gap:14px;}
.q::-webkit-details-marker{display:none;}
.pl{flex-shrink:0;width:28px;height:28px;border-radius:50%;background:rgba(47,119,133,.1);color:var(--teal);display:grid;place-items:center;font-size:19px;line-height:1;transition:transform .28s,background .28s,color .28s;}
.item[open] .pl{transform:rotate(45deg);background:var(--teal);color:#fff;}
.a{padding:0 24px 22px;font-family:'Kiye Sans','Inter',sans-serif;color:var(--muted);font-size:15.5px;line-height:1.65;}

.cta{background:#f7f8fa;text-align:center;padding:64px 0 84px;}
.cta h2{font-family:'Jost','Inter',sans-serif;font-size:clamp(24px,3vw,34px);font-weight:700;color:var(--ink);margin:0 0 10px;}
.cta p{color:var(--muted);margin:0 auto 24px;max-width:46ch;}
</style>
