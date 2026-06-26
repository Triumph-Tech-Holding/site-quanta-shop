<template>
  <main class="cf-page">
    <!-- HERO -->
    <section class="hero"><img class="hero__wm" alt="" aria-hidden="true" src="/img/logo/quanta-icon-white.png" /><div class="wrap hero__in">
      <span class="eyebrow">Como funciona</span>
      <h1>Você já gasta todos os meses.<br>E recebe <span class="hl">R$ 0</span> de volta.</h1>
      <p class="hero__sub">Cada compra que você faz alimenta o caixa de alguém. A Quanta Shop vira esse jogo: o dinheiro volta pra você — e ainda constrói uma renda com a sua rede.</p>
      <div class="hero__cta">
        <a class="btn btn-lime" href="#calc">Ver quanto eu perco →</a>
        <a class="btn btn-ghost" href="#caminhos">Como eu ganho</a>
      </div>
      <p class="hero__mini">Milhares de pessoas já consomem de forma inteligente · 6 anos de mercado · Saque via PIX</p>
    </div></section>

    <!-- CALCULADORA -->
    <section class="calc" id="calc"><div class="wrap">
      <div class="calc__head">
        <span class="eyebrow">A conta que ninguém faz</span>
        <h2>Quanto você deixa na mesa?</h2>
        <p>Arraste pelo seu gasto mensal e veja o que vira fumaça por não receber cashback.</p>
      </div>
      <div class="calc__card">
        <div>
          <div class="calc__label">Seu gasto por mês</div>
          <div class="calc__spend" id="spend">R$ 2.000</div>
          <input type="range" id="range" min="200" max="10000" step="100" value="2000" />
          <div class="calc__hint">Mercado, farmácia, roupas, delivery, viagens… tudo o que você já compraria.</div>
        </div>
        <div class="calc__result">
          <div class="calc__rlabel">Você perde, em 5 anos</div>
          <div class="calc__big" id="loss">R$ 6.000</div>
          <div class="calc__sub" id="permonth">R$ 100/mês que não voltam pra você</div>
          <div class="calc__flip">Com a Quanta, isso vira <b id="back">R$ 6.000</b> de volta — <b>pra você e pra sua rede.</b></div>
        </div>
      </div>
    </div></section>

    <!-- A VIRADA -->
    <section class="turn"><div class="wrap">
      <h2>A virada é simples: você compra onde <span class="hl">já compraria</span> — e o dinheiro volta.</h2>
      <div class="turn__grid">
        <div class="turn__c"><div class="turn__ic"><svg viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="1.9" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="8.2"/><path d="M12 7.4v9.2M14.4 9.2c-.5-.8-1.4-1.2-2.4-1.2-1.3 0-2.4.8-2.4 1.9 0 2.6 4.9 1.4 4.9 4 0 1.1-1.1 2-2.5 2-1 0-1.9-.4-2.4-1.2"/></svg></div><h3>Cashback de verdade</h3><p>Dinheiro real de volta a cada compra, sacável via PIX. Não é ponto que vence nem desconto disfarçado.</p></div>
        <div class="turn__c"><div class="turn__ic"><svg viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="1.9" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="5" r="2.4"/><circle cx="5" cy="18" r="2.4"/><circle cx="19" cy="18" r="2.4"/><path d="M12 7.4 6.4 15.8M12 7.4l5.6 8.4M7.4 18h9.2"/></svg></div><h3>Sua rede proprietária</h3><p>Indique pessoas e lojas e ganhe também quando elas consomem. Seu sucesso cresce com o da rede.</p></div>
        <div class="turn__c"><div class="turn__ic"><svg viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="1.9" stroke-linecap="round" stroke-linejoin="round"><path d="M4 5.5h16v10H8.5L5 19v-3.5H4z"/><path d="M9.2 10.4l1 2.2 2.2 1-2.2 1-1 2.2" transform="translate(2 -2.2) scale(.62)" stroke="#98C73A"/><circle cx="15.2" cy="10.5" r=".9" fill="#98C73A" stroke="none"/><path d="M12.8 8.6l.8 1.7 1.7.8-1.7.8-.8 1.7-.8-1.7-1.7-.8 1.7-.8z" fill="#98C73A" stroke="none"/></svg></div><h3>A Quanta IA faz por você</h3><p>Busca, cota, acha cupom e mostra onde rende mais — numa conversa, como no WhatsApp.</p></div>
      </div>
    </div></section>

    <!-- GANHOS DO CONSUMIDOR (carteira) -->
    <section class="turn" style="padding-top:0"><div class="wrap">
      <div class="wallet">
        <div class="wallet__mesh"></div>
        <div class="wallet__l">
          <svg class="phone" viewBox="0 0 250 500" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
            <defs>
              <linearGradient id="scr" x1="0" y1="0" x2="0" y2="1"><stop offset="0" stop-color="#0e2932"/><stop offset="1" stop-color="#091a21"/></linearGradient>
              <linearGradient id="bal" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#173f49"/><stop offset="1" stop-color="#0c2a31"/></linearGradient>
            </defs>
            <rect x="14" y="6" width="222" height="488" rx="36" fill="#070f14" stroke="#2a4a52" stroke-width="2"/>
            <rect x="23" y="16" width="204" height="468" rx="28" fill="url(#scr)"/>
            <rect x="98" y="26" width="54" height="9" rx="4.5" fill="#070f14"/>
            <rect x="30" y="60" width="190" height="90" rx="16" fill="url(#bal)"/>
            <text x="46" y="88" fill="#9fd3a6" font-size="10" font-family="Inter,sans-serif" font-weight="600">Saldo disponível</text>
            <text x="46" y="116" fill="#fff" font-size="26" font-family="Jost,Inter,sans-serif" font-weight="800">R$ 487,30</text>
            <rect x="30" y="164" width="90" height="76" rx="13" fill="rgba(255,255,255,.05)" stroke="rgba(255,255,255,.08)" stroke-width="1"/>
            <text x="46" y="188" fill="#9fd3a6" font-size="8.5" font-family="Inter,sans-serif">Cashback</text>
            <text x="46" y="218" fill="#fff" font-size="17" font-family="Jost,sans-serif" font-weight="800">+R$ 7,40</text>
            <rect x="130" y="164" width="90" height="76" rx="13" fill="rgba(255,255,255,.05)" stroke="rgba(255,255,255,.08)" stroke-width="1"/>
            <text x="146" y="188" fill="#9fd3a6" font-size="8.5" font-family="Inter,sans-serif">Residual</text>
            <text x="146" y="218" fill="#fff" font-size="17" font-family="Jost,sans-serif" font-weight="800">+R$ 1,12</text>
            <rect x="30" y="252" width="190" height="1" fill="rgba(255,255,255,.07)"/>
            <text x="46" y="280" fill="#6a8d94" font-size="9" font-family="Inter,sans-serif">Últimas entradas</text>
            <rect x="30" y="292" width="190" height="40" rx="10" fill="rgba(255,255,255,.04)"/>
            <text x="46" y="316" fill="#c8dde0" font-size="11" font-family="Inter,sans-serif">Zé Delivery</text>
            <text x="170" y="316" fill="#aee05a" font-size="11" font-family="Jost,sans-serif" font-weight="700">+R$ 3,20</text>
            <rect x="30" y="338" width="190" height="40" rx="10" fill="rgba(255,255,255,.04)"/>
            <text x="46" y="362" fill="#c8dde0" font-size="11" font-family="Inter,sans-serif">Indicação Maria S.</text>
            <text x="158" y="362" fill="#aee05a" font-size="11" font-family="Jost,sans-serif" font-weight="700">+R$ 25,00</text>
            <rect x="30" y="384" width="190" height="40" rx="10" fill="rgba(255,255,255,.04)"/>
            <text x="46" y="408" fill="#c8dde0" font-size="11" font-family="Inter,sans-serif">Carrefour</text>
            <text x="170" y="408" fill="#aee05a" font-size="11" font-family="Jost,sans-serif" font-weight="700">+R$ 7,40</text>
            <rect x="62" y="454" width="126" height="32" rx="16" fill="url(#bal)" stroke="rgba(152,199,58,.4)" stroke-width="1"/>
            <text x="125" y="474" fill="#aee05a" font-size="12" font-family="Jost,sans-serif" font-weight="700" text-anchor="middle">Sacar via PIX</text>
          </svg>
          <div class="bub b1"><small>Cashback</small>+R$ 7,40</div>
          <div class="bub b2"><small>Cashback residual</small>+R$ 1,12</div>
          <div class="bub b3"><small>Quanta Amizade</small>+R$ 25,00</div>
          <div class="bub b4"><small>Credenciamento</small>+R$ 12,00</div>
        </div>
        <div class="wallet__r">
          <span class="eyebrow">Para o consumidor</span>
          <h3>Sua carteira enche enquanto você vive sua vida</h3>
          <p class="wallet__sub">Você recebe duas vezes: o cashback direto das suas compras e o cashback residual sempre que a sua rede consome.</p>
          <div class="flow">
            <div class="flow__s"><div class="flow__n">1</div><div class="flow__t">Cashback direto<span>nas suas compras</span></div></div>
            <div class="flow__arr">→</div>
            <div class="flow__s"><div class="flow__n">2</div><div class="flow__t">Cashback residual<span>sobre o consumo da rede</span></div></div>
          </div>
          <blockquote class="quote">"92% dos consumidores confiam mais em recomendações de pessoas que <b>conhecem</b>."<cite>— Nielsen</cite></blockquote>
        </div>
      </div>
    </div></section>

    <!-- 3 CAMINHOS -->
    <section class="paths" id="caminhos"><div class="wrap">
      <div class="paths__head">
        <span class="eyebrow">Escolha o seu jogo</span>
        <h2>Tem um jeito de ganhar pra cada pessoa</h2>
        <p>Você decide até onde quer ir. Comece grátis e evolua quando quiser.</p>
      </div>
      <div class="paths__grid">
        <div class="path">
          <span class="path__big">01</span>
          <span class="path__tag">Para Você</span>
          <h3>Consuma e receba</h3>
          <p class="path__p">Transforme cada compra em dinheiro de volta, sem mudar nada na sua rotina.</p>
          <ul class="path__list"><li>Cashback sacável via PIX</li><li>Cupons das maiores marcas</li><li>Quanta IA achando o melhor pra você</li></ul>
          <a class="path__cta" href="/para-voce">Descobrir como <span class="arr">→</span></a>
        </div>
        <div class="path">
          <span class="path__big">02</span>
          <span class="path__tag">Seja um Agente</span>
          <h3>Construa renda recorrente</h3>
          <p class="path__p">Sem estoque, sem capital inicial. Construa sua rede proprietária e ganhe enquanto ela consome.</p>
          <ul class="path__list"><li>Residual em 5 níveis</li><li>Bônus de indicação e credenciamento</li><li>Comece grátis no plano Vision</li></ul>
          <a class="path__cta" href="/seja-um-agente">Quero empreender <span class="arr">→</span></a>
        </div>
        <div class="path">
          <span class="path__big">03</span>
          <span class="path__tag">Para sua Empresa</span>
          <h3>Atraia e fidelize</h3>
          <p class="path__p">Pague só por resultado e ganhe acesso a uma rede de consumidores que volta sempre.</p>
          <ul class="path__list"><li>Clientes mais fiéis</li><li>Cashback na maquininha (Bônus Stone)</li><li>Visibilidade na rede Quanta</li></ul>
          <a class="path__cta" href="/para-sua-empresa">Credenciar minha loja <span class="arr">→</span></a>
        </div>
      </div>
    </div></section>

    <!-- STATS -->
    <section class="stats"><div class="wrap stats__grid">
      <div><div class="stat__n">Milhares</div><div class="stat__l">de pessoas economizando</div></div>
      <div><div class="stat__n">Centenas</div><div class="stat__l">de lojas parceiras</div></div>
      <div><div class="stat__n">6 anos</div><div class="stat__l">de mercado</div></div>
      <div><div class="stat__n">PIX</div><div class="stat__l">saque na hora</div></div>
    </div></section>

    <!-- A GRANDE IDEIA -->
    <section class="idea"><div class="wrap">
      <span class="eyebrow">Não é cashback comum</span>
      <h2>É um <span class="hl">ecossistema</span> onde o seu sucesso impulsiona o de todos.</h2>
      <p>A maioria dos programas faz você ganhar sozinho. A Quanta distribui o ganho por toda a comunidade de consumo — consumidores, lojistas e agentes crescem juntos. É consumo que vira renda compartilhada.</p>
      <div class="idea__found">
        <span class="chip">Inclusão financeira</span>
        <span class="chip">Riqueza na base da pirâmide</span>
        <span class="chip">Governança da rede</span>
        <span class="chip">Consumo inteligente</span>
      </div>
    </div></section>

    <!-- CTA -->
    <section class="cta"><div class="wrap">
      <h2>Pare de deixar dinheiro <span class="hl">na mesa</span>.</h2>
      <p>Criar conta é grátis. Em minutos você já está recebendo de volta.</p>
      <a class="btn btn-lime" href="/register">Criar conta grátis →</a>
    </div></section>
  </main>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' })
useSeoMeta({
  title: 'Como Funciona — Quanta Shop',
  description: 'Você já gasta todos os meses e recebe R$ 0 de volta. Veja como a Quanta transforma seu consumo em dinheiro de volta — cashback real, sacável via PIX.',
})

onMounted(() => {
  const fmt = (n: number) => 'R$ ' + Math.round(n).toLocaleString('pt-BR')
  const range = document.getElementById('range') as HTMLInputElement
  if (!range) return
  function upd() {
    const v = +range.value
    const pct = (v - 200) / (10000 - 200) * 100
    range.style.background = `linear-gradient(90deg,var(--lime) 0%,var(--lime) ${pct}%,#dfe6e8 ${pct}%)`
    const month = v * 0.05
    const spendEl = document.getElementById('spend')
    const permonthEl = document.getElementById('permonth')
    const lossEl = document.getElementById('loss')
    const backEl = document.getElementById('back')
    if (spendEl) spendEl.textContent = fmt(v)
    if (permonthEl) permonthEl.textContent = fmt(month) + '/mês que não voltam pra você'
    if (lossEl) lossEl.textContent = fmt(month * 60)
    if (backEl) backEl.textContent = fmt(month * 60)
  }
  range.addEventListener('input', upd)
  upd()
})
</script>

<style scoped>
.cf-page { --teal: #2F7785; --teal-d: #225F6B; --lime: #98C73A; --lime-d: #7aad1f; --ink: #1a2332; --bg: #f4f6f7; --gold: #FFB342; }
* { box-sizing: border-box; margin: 0; padding: 0; }
.cf-page { font-family: 'Inter', 'Jost', sans-serif; color: var(--ink); background: #fff; -webkit-font-smoothing: antialiased; }
.wrap { max-width: 1120px; margin: 0 auto; padding: 0 24px; }
h1, h2, h3 { font-family: 'Jost', 'Inter', sans-serif; letter-spacing: -.02em; line-height: 1.05; }
.eyebrow { font-size: 13px; font-weight: 700; letter-spacing: .08em; text-transform: uppercase; }
.btn { display: inline-flex; align-items: center; gap: 8px; font-weight: 700; font-size: 15px; border-radius: 999px; padding: 14px 26px; text-decoration: none; cursor: pointer; border: 0; transition: .2s; }
.btn-lime { background: linear-gradient(180deg, var(--lime), var(--lime-d)); color: #173a0a; box-shadow: 0 12px 30px rgba(152, 199, 58, .35); }
.btn-lime:hover { transform: translateY(-2px); box-shadow: 0 18px 40px rgba(152, 199, 58, .45); }
.btn-ghost { background: transparent; border: 1.5px solid rgba(255, 255, 255, .35); color: #fff; }
.btn-ghost:hover { background: rgba(255, 255, 255, .1); }

/* HERO */
.hero { position: relative; overflow: hidden; color: #eaf3f5; background: radial-gradient(900px 520px at 82% -10%, rgba(58, 154, 173, .5), transparent 60%), radial-gradient(700px 500px at 5% 110%, rgba(152, 199, 58, .18), transparent 55%), linear-gradient(160deg, #0f2730 0%, #1a2332 55%, #0c1c24 100%); }
.hero__in { padding: 84px 0 72px; }
.hero .eyebrow { color: #9fd3a6; }
.hero h1 { color: #fff; font-size: clamp(34px, 6vw, 68px); font-weight: 800; margin: 14px 0 0; }
.hero h1 .hl { color: var(--lime); }
.hero__sub { font-size: clamp(16px, 2.2vw, 20px); color: #c8dde0; max-width: 640px; margin: 20px 0 0; line-height: 1.55; }
.hero__cta { display: flex; gap: 14px; flex-wrap: wrap; margin-top: 32px; }
.hero__mini { margin-top: 26px; font-size: 13px; color: #9fb6bb; }
.hero__wm { position: absolute; right: -90px; top: 50%; transform: translateY(-50%); width: 540px; max-width: 58%; opacity: .07; z-index: 0; pointer-events: none; user-select: none; }
.hero__in { position: relative; z-index: 1; }
@media (max-width: 860px) { .hero__wm { right: -120px; width: 360px; opacity: .05; } }

/* CALC */
.calc { background: var(--bg); padding: 80px 0; }
.calc__head { text-align: center; max-width: 680px; margin: 0 auto 36px; }
.calc__head .eyebrow { color: var(--teal); }
.calc__head h2 { font-size: clamp(26px, 4vw, 42px); font-weight: 800; margin: 10px 0; color: #225F6B; }
.calc__head p { color: #4a5b60; font-size: 17px; }
.calc__card { background: #fff; border: 1px solid #e8eef0; border-radius: 24px; box-shadow: 0 20px 50px rgba(15, 39, 48, .08); padding: 36px; max-width: 860px; margin: 0 auto; display: grid; grid-template-columns: 1fr 1fr; gap: 36px; align-items: center; }
.calc__label { font-size: 14px; color: #6b7c81; font-weight: 600; }
.calc__spend { font-family: 'Jost', 'Inter', sans-serif; font-weight: 800; font-size: 40px; color: var(--ink); margin: 6px 0 18px; }
input[type=range] { width: 100%; height: 6px; border-radius: 999px; -webkit-appearance: none; appearance: none; background: linear-gradient(90deg, var(--lime) 0%, var(--lime) 20%, #dfe6e8 20%); outline: none; }
input[type=range]::-webkit-slider-thumb { -webkit-appearance: none; width: 26px; height: 26px; border-radius: 50%; background: #fff; border: 4px solid var(--lime-d); cursor: pointer; box-shadow: 0 4px 12px rgba(0, 0, 0, .18); }
.calc__hint { font-size: 12px; color: #9aa9ae; margin-top: 10px; }
.calc__result { background: linear-gradient(160deg, #102a32, #1a2332); border-radius: 18px; padding: 28px; color: #eaf3f5; text-align: center; }
.calc__rlabel { font-size: 13px; color: #9fd3a6; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; }
.calc__big { font-family: 'Jost', 'Inter', sans-serif; font-weight: 800; font-size: clamp(38px, 7vw, 56px); color: var(--lime); margin: 8px 0; line-height: 1; }
.calc__sub { font-size: 14px; color: #c8dde0; }
.calc__flip { margin-top: 16px; padding-top: 16px; border-top: 1px solid rgba(255, 255, 255, .12); font-size: 14px; color: #dff3e0; }
.calc__flip b { color: #fff; }

/* TURN */
.turn { padding: 80px 0; text-align: center; }
.turn h2 { font-size: clamp(24px, 3.6vw, 38px); font-weight: 800; max-width: 820px; margin: 0 auto; }
.turn h2 .hl { color: var(--lime-d); }
.turn__grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; margin-top: 40px; }
.turn__c { background: var(--bg); border-radius: 18px; padding: 28px 22px; text-align: left; transition: transform .25s ease, box-shadow .25s ease; }
.turn__c:hover { transform: translateY(-4px); box-shadow: 0 18px 40px rgba(15, 39, 48, .10); }
.turn__ic { width: 58px; height: 58px; border-radius: 17px; display: grid; place-items: center; margin-bottom: 16px; position: relative; overflow: hidden; background: linear-gradient(150deg, #163f49, #0d2a31); box-shadow: 0 12px 26px rgba(15, 39, 48, .20), inset 0 1px 0 rgba(255, 255, 255, .07); }
.turn__ic::after { content: ''; position: absolute; inset: 0; background: radial-gradient(70px 46px at 72% -10%, rgba(152, 199, 58, .35), transparent 62%); }
.turn__ic svg { width: 30px; height: 30px; position: relative; z-index: 1; }
.turn__c h3 { font-size: 18px; font-weight: 800; }
.turn__c p { color: #4a5b60; font-size: 14px; line-height: 1.5; margin-top: 6px; }

/* WALLET */
.wallet { margin-top: 56px; position: relative; overflow: hidden; text-align: left; border-radius: 30px; padding: 50px; display: grid; grid-template-columns: .92fr 1.08fr; gap: 44px; align-items: center; background: radial-gradient(720px 480px at 10% -10%, rgba(58, 154, 173, .34), transparent 60%), linear-gradient(155deg, #0f2e36 0%, #0a1d24 100%); }
.wallet__mesh { position: absolute; inset: 0; background-image: linear-gradient(rgba(152, 199, 58, .06) 1px, transparent 1px), linear-gradient(90deg, rgba(152, 199, 58, .06) 1px, transparent 1px); background-size: 36px 36px; opacity: .6; mask-image: radial-gradient(70% 70% at 50% 40%, #000, transparent); -webkit-mask-image: radial-gradient(70% 70% at 50% 40%, #000, transparent); }
.wallet__l { position: relative; display: flex; justify-content: center; align-items: center; min-height: 420px; }
.phone { width: 236px; max-width: 80%; filter: drop-shadow(0 30px 60px rgba(0, 0, 0, .45)); }
.bub { position: absolute; background: linear-gradient(180deg, #aee05a, #7aad1f); color: #173a0a; font-weight: 800; font-size: 13px; line-height: 1; padding: 8px 12px; border-radius: 999px; box-shadow: 0 10px 22px rgba(152, 199, 58, .45); white-space: nowrap; animation: bfloat 3.8s ease-in-out infinite; }
.bub small { display: block; font-weight: 700; font-size: 8.5px; letter-spacing: .02em; color: #2c4d0c; margin-bottom: 2px; text-transform: uppercase; }
.bub.b1 { top: 14%; right: 2%; animation-delay: 0s; }
.bub.b2 { top: 40%; right: -4%; animation-delay: .7s; }
.bub.b3 { top: 64%; right: 4%; animation-delay: 1.4s; }
.bub.b4 { top: 30%; left: -2%; animation-delay: 1.0s; }
@keyframes bfloat { 0%, 100% { transform: translateY(0) } 50% { transform: translateY(-9px) } }
@media (max-width: 820px) { .bub.b4 { left: 2% } .bub.b2 { right: 2% } .wallet { grid-template-columns: 1fr; padding: 30px; gap: 28px; } .wallet__l { min-height: 360px; } }
.wallet__r { position: relative; z-index: 1; }
.wallet .eyebrow { color: #9fd3a6; }
.wallet h3 { color: #fff; font-size: clamp(23px, 3.1vw, 32px); font-weight: 800; margin: 8px 0 6px; letter-spacing: -.02em; }
.wallet__sub { color: #bcd6da; font-size: 15.5px; margin-bottom: 24px; line-height: 1.5; }
.flow { display: flex; align-items: center; gap: 16px; margin-bottom: 24px; flex-wrap: wrap; }
.flow__s { display: flex; align-items: center; gap: 11px; }
.flow__n { width: 36px; height: 36px; border-radius: 50%; border: 2px solid var(--lime); color: var(--lime); font-weight: 800; display: grid; place-items: center; font-family: 'Jost', sans-serif; font-size: 16px; box-shadow: 0 0 16px rgba(152, 199, 58, .45); flex: 0 0 auto; }
.flow__t { color: #eaf3f5; font-size: 14px; font-weight: 700; line-height: 1.18; }
.flow__t span { display: block; font-weight: 500; color: #9fb6bb; font-size: 11.5px; margin-top: 2px; }
.flow__arr { color: var(--lime); font-size: 22px; font-weight: 800; }
.quote { background: transparent; border-left: 3px solid var(--lime); padding: 8px 0 8px 18px; color: #dff3e0; font-size: 16px; line-height: 1.55; font-style: italic; }
.quote b { color: #fff; font-style: normal; }
.quote cite { display: block; font-style: normal; color: #8fb0b6; font-size: 12.5px; margin-top: 6px; }

/* PATHS */
.paths { background: #0c1a21; color: #eaf3f5; padding: 84px 0; }
.paths__head { text-align: center; max-width: 680px; margin: 0 auto 44px; }
.paths__head .eyebrow { color: #9fd3a6; }
.paths__head h2 { color: #fff; font-size: clamp(26px, 4vw, 42px); font-weight: 800; margin-top: 10px; }
.paths__head p { color: #aac4c8; margin-top: 12px; font-size: 16px; }
.paths__grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 22px; }
.path { position: relative; border-radius: 22px; padding: 30px 26px 26px; background: linear-gradient(165deg, #15262e, #0e1d24); border: 1px solid rgba(255, 255, 255, .08); overflow: hidden; transition: .25s; display: flex; flex-direction: column; }
.path:hover { transform: translateY(-6px); border-color: rgba(152, 199, 58, .5); }
.path__tag { font-size: 12px; font-weight: 700; color: #173a0a; background: var(--lime); display: inline-block; padding: 4px 12px; border-radius: 999px; align-self: flex-start; }
.path h3 { color: #fff; font-size: 24px; font-weight: 800; margin: 16px 0 8px; }
.path__p { color: #c0d4d8; font-size: 14.5px; line-height: 1.55; }
.path__list { list-style: none; margin: 16px 0 22px; display: flex; flex-direction: column; gap: 9px; }
.path__list li { font-size: 13.5px; color: #dbe9eb; display: flex; gap: 9px; align-items: flex-start; }
.path__list li::before { content: '✓'; color: var(--lime); font-weight: 800; }
.path__cta { margin-top: auto; display: inline-flex; align-items: center; gap: 8px; font-weight: 700; font-size: 15px; color: #fff; text-decoration: none; }
.path__cta .arr { transition: .2s; }
.path:hover .path__cta .arr { transform: translateX(5px); }
.path__big { position: absolute; right: -10px; top: -18px; font-size: 120px; font-weight: 800; font-family: 'Jost', sans-serif; color: rgba(255, 255, 255, .04); }

/* STATS */
.stats { padding: 64px 0; background: var(--bg); }
.stats__grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 18px; text-align: center; }
.stat__n { font-family: 'Jost', 'Inter', sans-serif; font-weight: 800; font-size: clamp(28px, 4vw, 40px); color: var(--teal); }
.stat__l { font-size: 13px; color: #6b7c81; margin-top: 4px; }

/* IDEA */
.idea { padding: 84px 0; text-align: center; }
.idea .eyebrow { color: var(--teal); }
.idea h2 { font-size: clamp(26px, 4vw, 44px); font-weight: 800; max-width: 860px; margin: 12px auto 0; }
.idea h2 .hl { color: var(--lime-d); }
.idea p { color: #4a5b60; font-size: 17px; max-width: 680px; margin: 18px auto 0; line-height: 1.6; }
.idea__found { display: flex; flex-wrap: wrap; gap: 10px; justify-content: center; margin-top: 28px; }
.chip { font-size: 13px; font-weight: 600; color: var(--teal-d); background: rgba(47, 119, 133, .08); border: 1px solid rgba(47, 119, 133, .16); border-radius: 999px; padding: 8px 14px; }

/* CTA */
.cta { background: radial-gradient(700px 400px at 80% 0, rgba(58, 154, 173, .5), transparent 60%), linear-gradient(160deg, #0f2730, #0c1c24); color: #fff; padding: 84px 0; text-align: center; }
.cta h2 { font-size: clamp(28px, 4.5vw, 48px); font-weight: 800; color: #fff; }
.cta h2 .hl { color: var(--lime); }
.cta p { color: #c8dde0; margin: 16px 0 30px; font-size: 17px; }

@media (max-width: 860px) {
  .calc__card { grid-template-columns: 1fr; }
  .turn__grid, .paths__grid { grid-template-columns: 1fr; }
  .stats__grid { grid-template-columns: repeat(2, 1fr); }
}
</style>
