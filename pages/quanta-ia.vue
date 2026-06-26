<template>
  <main class="qia-page">
    <!-- HERO -->
    <section class="hero">
      <div class="hero__grid"></div>
      <div class="container hero__row">
        <div>
          <span class="eyebrow">Quanta IA</span>
          <h1>A Quanta IA faz o trabalho.<br><span class="lime">Você só conversa.</span></h1>
          <p class="hero__lead">Busca e cota produtos e lojas, consulta seu saldo, acha cupons e tira qualquer dúvida — tudo num chat, do jeito que você já fala no WhatsApp.</p>
          <div class="hero__cta">
            <a class="btn btn--lime" href="#">Falar com a Quanta IA →</a>
            <a class="btn btn--ghost" href="#">Criar conta grátis</a>
          </div>
        </div>

        <div class="qc" aria-label="Demonstração da Quanta IA">
          <div class="qc__top">
            <span class="qc__av" aria-hidden="true"><img src="/img/logo/quanta-icon-white.png" alt=""></span>
            <div>
              <p class="qc__nm">Quanta IA</p>
              <p class="qc__st"><i aria-hidden="true"></i> online · responde na hora</p>
            </div>
          </div>
          <div ref="chatBody" class="qc__body">
            <template v-for="(m, i) in visible" :key="i">
              <div v-if="m.typing" class="qc__typing"><span /><span /><span /></div>
              <div v-else class="qc__msg" :class="'qc__msg--' + m.who" v-html="m.html" />
            </template>
          </div>
          <a class="qc__cta" href="#">
            Converse você mesmo com a Quanta IA
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" aria-hidden="true"><path d="M5 12h14"/><path d="M13 6l6 6-6 6"/></svg>
          </a>
        </div>
      </div>
    </section>

    <!-- CAPACIDADES -->
    <section class="block">
      <div class="container">
        <div class="head reveal"><span class="eyebrow">Tudo pelo chat</span><h2>Uma conversa resolve</h2><p>A Quanta IA reúne busca, carteira e suporte num só lugar.</p></div>
        <div class="cards reveal">
          <div class="card"><div class="card__ic"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="7"/><path d="m21 21-4.3-4.3"/></svg></div><h3>Buscar & cotar</h3><p>Ache produtos e lojas por nome, categoria ou proximidade — já com o cashback calculado.</p></div>
          <div class="card"><div class="card__ic"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="2" y="5" width="20" height="14" rx="2"/><path d="M2 10h20"/></svg></div><h3>Consultar saldo & sacar</h3><p>Veja seu saldo disponível, acompanhe a rede e resgate via PIX sem sair da conversa.</p></div>
          <div class="card"><div class="card__ic"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M9 11V6a3 3 0 0 1 6 0v5"/><rect x="4" y="11" width="16" height="10" rx="2"/></svg></div><h3>Cupons sob medida</h3><p>Ela acha os melhores cupons e ofertas pro seu perfil de consumo, todo dia.</p></div>
          <div class="card"><div class="card__ic"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"/></svg></div><h3>Dúvidas 24/7</h3><p>Do credenciamento ao primeiro saque: pergunte qualquer coisa e tenha resposta na hora.</p></div>
        </div>
      </div>
    </section>

    <!-- COMO CONVERSAR -->
    <section class="block bg-soft">
      <div class="container">
        <div class="head reveal"><span class="eyebrow">É simples assim</span><h2>Como conversar com a Quanta IA</h2></div>
        <div class="steps reveal">
          <div class="step"><div class="step__n">1</div><h3>Abra o chat</h3><p>Direto na plataforma ou no app — sem instalar nada novo.</p></div>
          <div class="step"><div class="step__n">2</div><h3>Escreva como fala</h3><p>"Procuro um fogão", "qual meu saldo", "tem cupom de farmácia?". Linguagem natural.</p></div>
          <div class="step"><div class="step__n">3</div><h3>Receba e aja</h3><p>Ela cota, mostra o cashback e te leva pra comprar ou sacar em um toque.</p></div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="cta">
      <div class="container reveal">
        <h2>Pronto pra deixar a IA trabalhar por você?</h2>
        <p>Crie sua conta grátis e comece a conversar — o cashback vem junto.</p>
        <a class="btn btn--lime" href="#">Criar conta grátis →</a>
      </div>
    </section>
  </main>
</template>

<script setup lang="ts">
import { ref, nextTick, onMounted, onBeforeUnmount } from 'vue'

definePageMeta({ layout: 'layout-home' })
useSeoMeta({
  title: "Quanta IA — busque, cote e resolva pelo chat | Quanta Shop",
  description: "A Quanta IA busca e cota produtos e lojas, consulta seu saldo, acha cupons e tira dúvidas — tudo num chat, como no WhatsApp.",
})
useHead({
  link: [
    { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
    { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' },
    { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&family=Jost:wght@400;500;600;700;800;900&display=swap' },
  ],
})

type Step = { who?: 'user' | 'bot'; html?: string; typing?: number }

const SCRIPT: Step[] = [
  { who: 'user', html: 'Quero um tênis de corrida até R$400 com mais cashback 👟' },
  { typing: 1100 },
  { who: 'bot', html:
      '<span class="qc__lead">Achei a melhor relação cashback perto de você:</span>' +
      '<div class="qc__pcard"><img src="https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=160&q=80&auto=format&fit=crop" alt="Tênis de corrida" loading="lazy" />' +
      '<div><div class="qc__pn">Nike Revolution 7</div><div class="qc__pmeta">Casas Bahia · entrega 2 dias</div>' +
      '<div class="qc__price">R$ 389,90</div><span class="qc__cb">12% cashback ≈ R$ 46,79 de volta</span></div></div>' },
  { who: 'user', html: 'Qual é o meu saldo?' },
  { typing: 900 },
  { who: 'bot', html:
      '<span class="qc__lead">Sua carteira Quanta:</span>' +
      '<div class="qc__scard"><div class="qc__srow"><span class="qc__lbl">Disponível</span><span class="qc__lbl">Reservado</span></div>' +
      '<div class="qc__srow"><span class="qc__big">R$ 247,30</span><span class="qc__ssub">R$ 88,10 a liberar</span></div>' +
      '<div class="qc__ssub" style="margin-top:8px">💸 Resgate via PIX em segundos</div></div>' },
  { who: 'user', html: 'Tem padaria com cashback aqui no bairro?' },
  { typing: 900 },
  { who: 'bot', html:
      '<span class="qc__lead">Pertinho de você:</span>' +
      '<div class="qc__pcard"><img src="https://images.unsplash.com/photo-1509440159596-0249088772ff?w=160&q=80&auto=format&fit=crop" alt="Padaria" loading="lazy" />' +
      '<div><div class="qc__pn">Padaria Real</div><div class="qc__pmeta">Vila Mariana · 600 m</div>' +
      '<span class="qc__cb">5% cashback · ativa no WhatsApp</span></div></div>' },
]

const visible = ref<Step[]>([])
const chatBody = ref<HTMLElement | null>(null)
let chatTimer: ReturnType<typeof setTimeout> | null = null
let idx = 0

async function scrollDown() {
  await nextTick()
  if (chatBody.value) chatBody.value.scrollTop = chatBody.value.scrollHeight
}

function play() {
  if (idx >= SCRIPT.length) {
    chatTimer = setTimeout(() => { visible.value = []; idx = 0; play() }, 4200)
    return
  }
  const it = SCRIPT[idx++]
  if (it.typing) {
    visible.value.push({ typing: it.typing })
    scrollDown()
    chatTimer = setTimeout(() => { visible.value.pop(); play() }, it.typing)
    return
  }
  visible.value.push(it)
  scrollDown()
  chatTimer = setTimeout(play, it.who === 'user' ? 700 : 1500)
}

onMounted(() => {
  const reduceMotion = !!(window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches)

  const io = new IntersectionObserver((es) => {
    es.forEach((e) => { if (e.isIntersecting) { e.target.classList.add('in'); io.unobserve(e.target) } })
  }, { threshold: .18 })
  document.querySelectorAll('.reveal').forEach((el) => io.observe(el))

  if (reduceMotion) {
    visible.value = SCRIPT.filter((s) => !s.typing)
  } else {
    play()
  }
})

onBeforeUnmount(() => { if (chatTimer) clearTimeout(chatTimer) })
</script>

<style scoped>
.qia-page{--ink:#225F6B;--teal:#2F7785;--teal2:#3A9AAD;--lime:#98C73A;--lime-d:#7aad1f;--txt:#374151;--muted:#6b7280;--line:rgba(17,24,39,.08);}
*{box-sizing:border-box;} .qia-page{margin:0;font-family:'Inter',sans-serif;color:var(--txt);background:#fff;-webkit-font-smoothing:antialiased;}
.container{width:100%;max-width:1120px;margin:0 auto;padding:0 24px;}
a{text-decoration:none;}
.reveal{opacity:0;transform:translateY(22px);transition:opacity .7s cubic-bezier(.16,1,.3,1),transform .7s cubic-bezier(.16,1,.3,1);}
.reveal.in{opacity:1;transform:none;}
.btn{display:inline-flex;align-items:center;gap:8px;font-family:'Kiye Sans','Inter',sans-serif;font-weight:700;font-size:15px;border-radius:999px;padding:14px 28px;transition:transform .2s,box-shadow .2s,background .2s;}
.btn--lime{color:#173a0a;background:linear-gradient(180deg,var(--lime),var(--lime-d));box-shadow:0 10px 28px rgba(152,199,58,.35);}
.btn--lime:hover{transform:translateY(-2px);box-shadow:0 16px 36px rgba(152,199,58,.45);}
.btn--ghost{color:#fff;border:1.5px solid rgba(255,255,255,.5);}
.btn--ghost:hover{background:rgba(255,255,255,.1);}
.eyebrow{font-size:12px;font-weight:800;letter-spacing:.16em;text-transform:uppercase;color:var(--teal2);}

/* HERO */
.hero{position:relative;overflow:hidden;background:radial-gradient(800px 460px at 78% 12%,rgba(58,154,173,.28),transparent 60%),linear-gradient(150deg,#0e2a32,#123038 55%,#16323a);color:#fff;padding:84px 0 92px;}
.hero__grid{position:absolute;inset:0;background-image:radial-gradient(rgba(255,255,255,.06) 1px,transparent 1px);background-size:26px 26px;-webkit-mask-image:radial-gradient(70% 70% at 80% 20%,#000,transparent 75%);mask-image:radial-gradient(70% 70% at 80% 20%,#000,transparent 75%);}
.hero__row{position:relative;display:grid;grid-template-columns:1.1fr .9fr;gap:48px;align-items:center;}
.hero h1{font-family:'Jost','Inter',sans-serif;font-size:clamp(34px,5vw,58px);font-weight:800;line-height:1.05;margin:14px 0 0;color:#fff;}
.hero h1 .lime{color:var(--lime);}
.hero__lead{font-size:clamp(16px,2vw,19px);color:#cdd9dd;line-height:1.6;margin:18px 0 28px;max-width:48ch;}
.hero__cta{display:flex;gap:14px;flex-wrap:wrap;}
.hero .eyebrow{color:#bfe89a;}

/* CHAT CARD */
.qc{width:100%;max-width:420px;margin-left:auto;background:#0c1a21;border:1px solid rgba(255,255,255,.1);border-radius:26px;box-shadow:0 24px 60px rgba(1,15,28,.4);padding:16px;}
.qc__top{display:flex;align-items:center;gap:12px;padding:6px 6px 14px;border-bottom:1px solid rgba(255,255,255,.08);}
.qc__av{width:44px;height:44px;border-radius:50%;background:linear-gradient(135deg,#3A9AAD,#98C73A);display:grid;place-items:center;}
.qc__av img{width:26px;height:26px;}
.qc__nm{font-family:'Jost','Inter',sans-serif;font-weight:700;color:#fff;font-size:15px;margin:0;}
.qc__st{font-size:12px;color:#9fd3a6;margin:0;display:flex;align-items:center;gap:6px;}
.qc__st i{width:7px;height:7px;border-radius:50%;background:#98C73A;display:inline-block;}
.qc__body{display:flex;flex-direction:column;gap:10px;padding:16px 6px 6px;min-height:296px;max-height:360px;overflow:hidden;}

/* MESSAGES — use :deep() so v-html content gets styled */
.qc__msg{max-width:86%;font-family:'Kiye Sans','Inter',sans-serif;font-size:14px;line-height:1.45;padding:11px 14px;border-radius:16px;animation:qsPop .5s both;}
@keyframes qsPop{from{opacity:0;transform:translateY(10px) scale(.98);}to{opacity:1;transform:none;}}
.qc__msg--user{align-self:flex-end;background:linear-gradient(180deg,#2f7785,#266472);color:#eafaff;border-bottom-right-radius:5px;}
.qc__msg--bot{align-self:flex-start;background:#15262e;color:#d7e7ea;border:1px solid rgba(255,255,255,.06);border-bottom-left-radius:5px;}

.qc__msg :deep(.qc__lead){color:#9fd3a6;font-weight:600;font-size:12px;display:block;margin-bottom:8px;}

.qc__msg :deep(.qc__pcard){display:flex;gap:12px;background:#0c1a21;border:1px solid rgba(255,255,255,.08);border-radius:14px;padding:10px;margin-top:4px;}
.qc__msg :deep(.qc__pcard img){width:62px;height:62px;border-radius:10px;object-fit:cover;flex-shrink:0;}
.qc__msg :deep(.qc__pn){font-weight:700;color:#fff;font-size:13px;}
.qc__msg :deep(.qc__pmeta){font-size:11px;color:#8fb3b8;margin:2px 0 6px;}
.qc__msg :deep(.qc__price){font-family:'Jost','Inter',sans-serif;font-weight:700;color:#fff;font-size:15px;}
.qc__msg :deep(.qc__cb){display:inline-block;background:rgba(152,199,58,.16);color:#98C73A;font-size:11px;font-weight:700;padding:2px 8px;border-radius:999px;margin-top:4px;}

.qc__msg :deep(.qc__scard){background:#0c1a21;border:1px solid rgba(255,255,255,.08);border-radius:14px;padding:12px;margin-top:4px;}
.qc__msg :deep(.qc__srow){display:flex;justify-content:space-between;align-items:center;}
.qc__msg :deep(.qc__lbl){font-size:11px;color:#8fb3b8;text-transform:uppercase;letter-spacing:.08em;}
.qc__msg :deep(.qc__big){font-family:'Jost','Inter',sans-serif;font-weight:800;color:#98C73A;font-size:24px;}
.qc__msg :deep(.qc__ssub){font-size:12px;color:#aac4c8;margin-top:2px;}

.qc__typing{align-self:flex-start;background:#15262e;border:1px solid rgba(255,255,255,.06);border-radius:16px;border-bottom-left-radius:5px;padding:14px 16px;display:flex;gap:5px;}
.qc__typing span{width:7px;height:7px;border-radius:50%;background:#6f9aa1;animation:qsBlink 1.2s infinite;}
.qc__typing span:nth-child(2){animation-delay:.2s;} .qc__typing span:nth-child(3){animation-delay:.4s;}
@keyframes qsBlink{0%,60%,100%{opacity:.3;transform:translateY(0);}30%{opacity:1;transform:translateY(-3px);}}

.qc__cta{margin-top:10px;width:100%;display:flex;align-items:center;justify-content:center;gap:8px;background:rgba(152,199,58,.14);color:#d4ef9f;border:1px solid rgba(152,199,58,.3);border-radius:14px;padding:12px;font-family:'Kiye Sans','Inter',sans-serif;font-weight:700;font-size:14px;min-height:44px;cursor:pointer;transition:background .2s;}
.qc__cta:hover{background:rgba(152,199,58,.22);}
.qc__cta svg{width:16px;height:16px;}

/* SECTIONS */
section.block{padding:84px 0;}
.bg-soft{background:#f7f8fa;}
.head{text-align:center;max-width:680px;margin:0 auto 50px;}
.head h2{font-family:'Jost','Inter',sans-serif;font-size:clamp(28px,3.6vw,42px);font-weight:700;color:var(--ink);line-height:1.1;letter-spacing:-.02em;margin:12px 0 12px;}
.head p{font-size:17px;color:var(--muted);}

.cards{display:grid;grid-template-columns:repeat(4,1fr);gap:20px;}
@media(max-width:880px){.cards{grid-template-columns:1fr 1fr;}}
.card{background:#fff;border:1px solid var(--line);border-radius:18px;padding:26px 22px;transition:transform .25s,box-shadow .25s;}
.card:hover{transform:translateY(-5px);box-shadow:0 18px 40px rgba(1,15,28,.10);}
.card__ic{width:46px;height:46px;border-radius:13px;display:grid;place-items:center;color:#fff;background:linear-gradient(135deg,#2F7785,#3A9AAD);margin-bottom:14px;}
.card:nth-child(4) .card__ic{background:linear-gradient(135deg,#7aad1f,#98C73A);color:#173a0a;}
.card__ic svg{width:22px;height:22px;}
.card h3{font-family:'Jost','Inter',sans-serif;font-size:18px;color:var(--ink);margin:0 0 6px;}
.card p{font-size:14px;color:var(--muted);line-height:1.55;margin:0;}

.steps{display:grid;grid-template-columns:repeat(3,1fr);gap:26px;max-width:900px;margin:0 auto;}
@media(max-width:760px){.steps{grid-template-columns:1fr;}.cards{grid-template-columns:1fr;}.hero__row{grid-template-columns:1fr;}}
.step{text-align:center;}
.step__n{width:54px;height:54px;border-radius:50%;margin:0 auto 14px;display:grid;place-items:center;font-family:'Jost',sans-serif;font-weight:800;font-size:22px;color:#fff;background:linear-gradient(135deg,#2F7785,#3A9AAD);}
.step h3{font-family:'Jost',sans-serif;color:var(--ink);font-size:18px;margin:0 0 6px;}
.step p{color:var(--muted);font-size:14.5px;line-height:1.55;max-width:260px;margin:0 auto;}

/* CTA */
.cta{background:radial-gradient(700px 400px at 50% 0%,rgba(152,199,58,.18),transparent 60%),linear-gradient(150deg,#0e2a32,#16323a);color:#fff;text-align:center;padding:84px 0;}
.cta h2{font-family:'Jost','Inter',sans-serif;font-size:clamp(26px,3.4vw,40px);font-weight:800;margin:0 0 14px;color:#fff;}
.cta p{color:#cdd9dd;max-width:54ch;margin:0 auto 26px;}
</style>
