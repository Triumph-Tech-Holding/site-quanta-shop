<template>
  <main class="cbr-page">
    <!-- HERO -->
    <section class="hero">
      <div class="hero__grid"></div>
      <div class="container hero__in">
        <span class="eyebrow">Cashback Residual</span>
        <h1>Ganhe mesmo quando <span class="lime">não é você</span> quem compra.</h1>
        <p class="hero__lead">No cashback comum você ganha sozinho. No residual, você também recebe quando a sua rede consome — uma renda que cresce junto com ela.</p>
        <div class="hero__cta">
          <a class="btn btn--lime" href="#">Começar a construir minha rede →</a>
          <a class="btn btn--ghost" href="#">Ver o plano de compensação</a>
        </div>
      </div>
    </section>

    <!-- COMUM x RESIDUAL -->
    <section class="block">
      <div class="container">
        <div class="head reveal"><span class="eyebrow">A diferença</span><h2>Cashback comum vs. Cashback Residual</h2><p>O mesmo consumo, dois tamanhos de ganho.</p></div>
        <div class="vs reveal">
          <div class="vcard">
            <span class="vcard__tag">Cashback comum</span>
            <h3>Você ganha só nas suas compras</h3>
            <ul>
              <li><span class="dot dot--no">✕</span> O ganho para quando você para de comprar.</li>
              <li><span class="dot dot--no">✕</span> Depende só do seu próprio consumo.</li>
              <li><span class="dot dot--no">✕</span> Sem efeito de rede: ninguém soma ao seu resultado.</li>
            </ul>
          </div>
          <div class="vcard vcard--win">
            <span class="vcard__tag">Cashback Residual · Quanta</span>
            <h3>Você ganha quando a sua rede consome</h3>
            <ul>
              <li><span class="dot dot--yes">✓</span> Renda recorrente, que continua mesmo quando você não compra.</li>
              <li><span class="dot dot--yes">✓</span> Cada pessoa e loja da sua rede soma ao seu ganho.</li>
              <li><span class="dot dot--yes">✓</span> Quanto mais a rede cresce e usa, mais você recebe.</li>
            </ul>
          </div>
        </div>
      </div>
    </section>

    <!-- COMO FUNCIONA -->
    <section class="block bg-soft">
      <div class="container">
        <div class="head reveal"><span class="eyebrow">Como funciona</span><h2>Sua rede proprietária trabalhando por você</h2></div>
        <div class="flow reveal">
          <div class="fstep"><div class="fstep__n">01</div><h3>Você indica</h3><p>Convida pessoas e lojas pra plataforma. Elas entram na sua rede proprietária.</p></div>
          <div class="fstep"><div class="fstep__n">02</div><h3>A rede consome</h3><p>Cada compra feita pela sua rede gera cashback — e uma parte volta pra você.</p></div>
          <div class="fstep"><div class="fstep__n">03</div><h3>Você recebe o residual</h3><p>O ganho cai na sua carteira de forma recorrente, com total transparência do plano.</p></div>
        </div>
      </div>
    </section>

    <!-- EXEMPLO -->
    <section class="block">
      <div class="container reveal">
        <div class="sim">
          <h3>Um exemplo do efeito de rede</h3>
          <p class="sub">Cenário ilustrativo — os valores reais dependem do consumo da sua rede e do seu plano.</p>
          <div class="sim__grid">
            <div class="sim__b"><div class="v" data-to="50">0</div><div class="l">pessoas na sua rede</div></div>
            <div class="sim__b"><div class="v" data-prefix="R$ " data-to="600">0</div><div class="l">consumo médio/mês cada</div></div>
            <div class="sim__b"><div class="v" data-prefix="R$ " data-to="450">0</div><div class="l">residual estimado/mês*</div></div>
          </div>
          <p class="sim__foot">*Exemplo ilustrativo, não é promessa de ganho. O residual segue o plano de compensação (distribuição transparente, com reforço via Fundo de Bonificação).</p>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="cta">
      <div class="container reveal">
        <h2>Comece a construir sua renda recorrente</h2>
        <p>Sem capital inicial e sem estoque. Você indica, a rede consome, o residual chega.</p>
        <a class="btn btn--lime" href="#">Quero começar agora →</a>
      </div>
    </section>
  </main>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' })
useSeoMeta({
  title: "Cashback Residual — ganhe quando sua rede consome | Quanta Shop",
  description: "No cashback residual da Quanta você também recebe quando a sua rede consome: uma renda recorrente que cresce com ela.",
})
useHead({
  link: [
    { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
    { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' },
    { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&family=Jost:wght@400;500;600;700;800;900&display=swap' },
  ],
})

onMounted(() => {
  function count(el: HTMLElement) {
    var to = parseFloat((el as any).dataset.to), pre = (el as any).dataset.prefix || '', t0: number | null = null, dur = 1400;
    function fr(t: number) { if (!t0) t0 = t; var e = Math.min((t - t0) / dur, 1); e = 1 - Math.pow(1 - e, 3); el.textContent = pre + Math.round(to * e).toLocaleString('pt-BR'); if (e < 1) requestAnimationFrame(fr); }
    requestAnimationFrame(fr);
  }
  var io = new IntersectionObserver(function (es) {
    es.forEach(function (e) {
      if (e.isIntersecting) {
        e.target.classList.add('in');
        e.target.querySelectorAll<HTMLElement>('[data-to]').forEach(count);
        io.unobserve(e.target);
      }
    });
  }, { threshold: .18 });
  document.querySelectorAll('.reveal').forEach(function (el) { io.observe(el); });
})
</script>

<style scoped>
.cbr-page{--ink:#225F6B;--teal:#2F7785;--teal2:#3A9AAD;--lime:#98C73A;--lime-d:#7aad1f;--txt:#374151;--muted:#6b7280;--line:rgba(17,24,39,.08);}
*{box-sizing:border-box;} .cbr-page{margin:0;font-family:'Inter',sans-serif;color:var(--txt);background:#fff;-webkit-font-smoothing:antialiased;}
.container{width:100%;max-width:1120px;margin:0 auto;padding:0 24px;}
a{text-decoration:none;}
.reveal{opacity:0;transform:translateY(22px);transition:opacity .7s cubic-bezier(.16,1,.3,1),transform .7s cubic-bezier(.16,1,.3,1);}
.reveal.in{opacity:1;transform:none;}
.btn{display:inline-flex;align-items:center;gap:8px;font-family:'Kiye Sans','Inter',sans-serif;font-weight:700;font-size:15px;border-radius:999px;padding:14px 28px;transition:transform .2s,box-shadow .2s;}
.btn--lime{color:#173a0a;background:linear-gradient(180deg,var(--lime),var(--lime-d));box-shadow:0 10px 28px rgba(152,199,58,.35);}
.btn--lime:hover{transform:translateY(-2px);box-shadow:0 16px 36px rgba(152,199,58,.45);}
.btn--ghost{color:#fff;border:1.5px solid rgba(255,255,255,.5);} .btn--ghost:hover{background:rgba(255,255,255,.1);}
.eyebrow{font-size:12px;font-weight:800;letter-spacing:.16em;text-transform:uppercase;color:var(--teal2);}

.hero{position:relative;overflow:hidden;background:radial-gradient(800px 460px at 80% 10%,rgba(152,199,58,.22),transparent 60%),linear-gradient(150deg,#0e2a32,#123038 55%,#16323a);color:#fff;padding:88px 0;}
.hero__grid{position:absolute;inset:0;background-image:radial-gradient(rgba(255,255,255,.06) 1px,transparent 1px);background-size:26px 26px;-webkit-mask-image:radial-gradient(70% 70% at 78% 24%,#000,transparent 75%);mask-image:radial-gradient(70% 70% at 78% 24%,#000,transparent 75%);}
.hero__in{position:relative;max-width:760px;}
.hero .eyebrow{color:#bfe89a;}
.hero h1{font-family:'Jost','Inter',sans-serif;font-size:clamp(34px,5.2vw,60px);font-weight:800;line-height:1.04;margin:14px 0 0;color:#fff;}
.hero h1 .lime{color:var(--lime);}
.hero__lead{font-size:clamp(16px,2vw,19px);color:#cdd9dd;line-height:1.6;margin:18px 0 28px;max-width:54ch;}
.hero__cta{display:flex;gap:14px;flex-wrap:wrap;}

section.block{padding:84px 0;} .bg-soft{background:#f7f8fa;}
.head{text-align:center;max-width:720px;margin:0 auto 50px;}
.head h2{font-family:'Jost','Inter',sans-serif;font-size:clamp(28px,3.6vw,42px);font-weight:700;color:var(--ink);line-height:1.1;letter-spacing:-.02em;margin:12px 0 12px;}
.head p{font-size:17px;color:var(--muted);}

/* COMUM x RESIDUAL */
.vs{display:grid;grid-template-columns:1fr 1fr;gap:24px;max-width:920px;margin:0 auto;}
@media(max-width:760px){.vs{grid-template-columns:1fr;}}
.vcard{border-radius:20px;padding:30px;border:1px solid var(--line);background:#fff;}
.vcard--win{border-color:rgba(152,199,58,.5);background:linear-gradient(180deg,#fff,#f7fbef);box-shadow:0 14px 40px rgba(152,199,58,.14);}
.vcard__tag{font-size:12px;font-weight:800;letter-spacing:.12em;text-transform:uppercase;color:#9aa3af;}
.vcard--win .vcard__tag{color:var(--lime-d);}
.vcard h3{font-family:'Jost','Inter',sans-serif;font-size:22px;color:var(--ink);margin:6px 0 14px;}
.vcard li{list-style:none;display:flex;gap:10px;align-items:flex-start;font-size:14.5px;color:var(--txt);margin-bottom:10px;}
.vcard ul{margin:0;padding:0;}
.vcard .dot{flex-shrink:0;width:20px;height:20px;border-radius:50%;display:grid;place-items:center;font-size:12px;margin-top:1px;}
.dot--no{background:#fde8e8;color:#c0392b;} .dot--yes{background:rgba(152,199,58,.18);color:var(--lime-d);}

/* COMO FUNCIONA */
.flow{display:grid;grid-template-columns:repeat(3,1fr);gap:24px;max-width:980px;margin:0 auto;}
@media(max-width:760px){.flow{grid-template-columns:1fr;}}
.fstep{background:#fff;border:1px solid var(--line);border-radius:18px;padding:26px;}
.fstep__n{font-family:'Jost',sans-serif;font-weight:800;color:var(--lime-d);font-size:14px;letter-spacing:.1em;}
.fstep h3{font-family:'Jost',sans-serif;color:var(--ink);font-size:18px;margin:8px 0 6px;}
.fstep p{color:var(--muted);font-size:14.5px;line-height:1.55;margin:0;}

/* EXEMPLO / SIMULAÇÃO */
.sim{max-width:880px;margin:0 auto;background:linear-gradient(150deg,#0e2a32,#16323a);border-radius:24px;padding:40px;color:#fff;text-align:center;}
.sim h3{font-family:'Jost',sans-serif;font-size:22px;margin:0 0 6px;}
.sim .sub{color:#9fb6bb;font-size:14px;margin-bottom:26px;}
.sim__grid{display:grid;grid-template-columns:repeat(3,1fr);gap:20px;}
@media(max-width:640px){.sim__grid{grid-template-columns:1fr;}}
.sim__b{background:rgba(255,255,255,.05);border:1px solid rgba(255,255,255,.08);border-radius:16px;padding:22px;}
.sim__b .v{font-family:'Jost',sans-serif;font-weight:900;font-size:clamp(26px,4vw,38px);color:var(--lime);line-height:1;}
.sim__b .l{font-size:13px;color:#bcccd0;margin-top:8px;}
.sim__foot{font-size:12px;color:#8aa0a5;margin-top:22px;}

.cta{background:radial-gradient(700px 400px at 50% 0%,rgba(152,199,58,.18),transparent 60%),linear-gradient(150deg,#0e2a32,#16323a);color:#fff;text-align:center;padding:84px 0;}
.cta h2{font-family:'Jost','Inter',sans-serif;font-size:clamp(26px,3.4vw,40px);font-weight:800;margin:0 0 14px;color:#fff;}
.cta p{color:#cdd9dd;max-width:54ch;margin:0 auto 26px;}
</style>
