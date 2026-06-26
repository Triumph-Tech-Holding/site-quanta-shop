<template>
  <main class="pv-page">
    <svg width="0" height="0" style="position:absolute" aria-hidden="true"><symbol id="bq-white" viewBox="0 0 394 399" preserveAspectRatio="xMidYMid meet"><image href="/img/logo/quanta-icon-white.png" width="394" height="399"/></symbol></svg>

    <!-- HERO -->
    <section class="hero"><img class="hero__wm" alt="" aria-hidden="true" src="/img/logo/quanta-icon-white.png" /><div class="wrap hero__in">
      <div class="hero__copy">
        <span class="eyebrow">Para você</span>
        <h1>Toda compra vira <span class="hl">dinheiro de volta</span>.<br>Sem segredo, sem pegadinha.</h1>
        <p class="hero__sub">Cashback de verdade nas suas lojas favoritas — sacável via PIX. Com cupons todo dia e uma IA que acha o melhor pra você, numa conversa.</p>
        <div class="hero__cta">
          <a class="btn btn-lime" href="/register">Criar conta grátis →</a>
          <a class="btn btn-ghost" href="#calc">Ver quanto eu ganho</a>
        </div>
        <p class="hero__mini">Milhares de pessoas economizando · 6 anos de mercado · Saque via PIX</p>
      </div>
      <div class="hero__art">
        <svg viewBox="0 0 360 360" class="loop" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
          <defs>
            <radialGradient id="amb" cx="50%" cy="50%" r="50%">
              <stop offset="0" stop-color="#1f6b7d" stop-opacity=".55"/>
              <stop offset="55%" stop-color="#103440" stop-opacity=".22"/>
              <stop offset="100%" stop-color="#0c1c24" stop-opacity="0"/>
            </radialGradient>
            <radialGradient id="coinG" cx="38%" cy="28%" r="78%">
              <stop offset="0" stop-color="#e2f79a"/>
              <stop offset="40%" stop-color="#aedb52"/>
              <stop offset="100%" stop-color="#6c9b20"/>
            </radialGradient>
            <linearGradient id="ringG" x1="0" y1="0" x2="1" y2="1">
              <stop offset="0" stop-color="#aee05a"/>
              <stop offset="100%" stop-color="#3A9AAD"/>
            </linearGradient>
            <radialGradient id="nodeG" cx="40%" cy="30%" r="85%">
              <stop offset="0" stop-color="#143b46"/>
              <stop offset="100%" stop-color="#0a1b22"/>
            </radialGradient>
            <filter id="gl" x="-70%" y="-70%" width="240%" height="240%">
              <feGaussianBlur stdDeviation="3" result="b"/>
              <feMerge><feMergeNode in="b"/><feMergeNode in="SourceGraphic"/></feMerge>
            </filter>
            <filter id="glBig" x="-90%" y="-90%" width="280%" height="280%">
              <feGaussianBlur stdDeviation="10"/>
            </filter>
            <path id="orbit" d="M180,60 A120,120 0 1,1 180,300 A120,120 0 1,1 180,60"/>
            <clipPath id="coinClip"><circle cx="180" cy="180" r="47"/></clipPath>
          </defs>
          <circle cx="180" cy="180" r="152" fill="url(#amb)"/>
          <g fill="#bfe8ef" opacity=".5">
            <circle cx="64" cy="92" r="1.3"><animate attributeName="opacity" values=".15;.7;.15" dur="3.4s" repeatCount="indefinite"/></circle>
            <circle cx="300" cy="120" r="1.6"><animate attributeName="opacity" values=".1;.6;.1" dur="4.2s" begin="-1s" repeatCount="indefinite"/></circle>
            <circle cx="288" cy="312" r="1.2"><animate attributeName="opacity" values=".15;.65;.15" dur="3.8s" begin="-2s" repeatCount="indefinite"/></circle>
            <circle cx="58" cy="296" r="1.5"><animate attributeName="opacity" values=".1;.55;.1" dur="4.6s" begin="-3s" repeatCount="indefinite"/></circle>
          </g>
          <g><circle cx="180" cy="180" r="120" fill="none" stroke="rgba(152,199,58,.20)" stroke-width="1.4" stroke-dasharray="1.5 11" stroke-linecap="round"/>
             <animateTransform attributeName="transform" type="rotate" from="0 180 180" to="360 180 180" dur="64s" repeatCount="indefinite"/></g>
          <g><path d="M180,60 A120,120 0 0,1 283,121" fill="none" stroke="url(#ringG)" stroke-width="2.6" stroke-linecap="round" opacity=".95"/>
             <animateTransform attributeName="transform" type="rotate" from="0 180 180" to="360 180 180" dur="7.5s" repeatCount="indefinite"/></g>
          <g stroke="url(#ringG)" stroke-width="1" opacity=".22">
            <line x1="180" y1="180" x2="180" y2="60"/>
            <line x1="180" y1="180" x2="284" y2="240"/>
            <line x1="180" y1="180" x2="76" y2="240"/>
          </g>
          <g>
            <circle r="3" fill="#d6f580" filter="url(#gl)"><animateMotion dur="9s" repeatCount="indefinite"><mpath href="#orbit"/></animateMotion></circle>
            <circle r="2.3" fill="#8fe0ec" filter="url(#gl)"><animateMotion dur="9s" begin="-3s" repeatCount="indefinite"><mpath href="#orbit"/></animateMotion></circle>
            <circle r="2.6" fill="#d6f580" filter="url(#gl)"><animateMotion dur="9s" begin="-6s" repeatCount="indefinite"><mpath href="#orbit"/></animateMotion></circle>
          </g>
          <g filter="url(#gl)"><circle cx="180" cy="60" r="34" fill="url(#nodeG)" stroke="url(#ringG)" stroke-width="2"/></g>
          <g transform="translate(180,60)" stroke="#d4ef83" stroke-width="2.2" fill="none" stroke-linecap="round" stroke-linejoin="round">
            <path d="M-13 -8 h4 l3 16 h12 l3 -11 h-16"/>
            <circle cx="-4" cy="11" r="1.9" fill="#d4ef83" stroke="none"/><circle cx="6" cy="11" r="1.9" fill="#d4ef83" stroke="none"/>
          </g>
          <text x="180" y="16" text-anchor="middle" font-size="12.5" font-weight="600" fill="#dcecef" class="loop__lbl" letter-spacing=".3">Compra</text>
          <g filter="url(#gl)"><circle cx="284" cy="240" r="34" fill="url(#nodeG)" stroke="url(#ringG)" stroke-width="2"/></g>
          <g transform="translate(284,240)" stroke="#d4ef83" fill="none" stroke-width="2.1" stroke-linecap="round" stroke-linejoin="round">
            <path d="M9.5 1.5 A9.5 9.5 0 1 1 5 -8.3"/>
            <path d="M5 -8.3 l 5.6 1 M5 -8.3 l -1 5.4"/>
            <circle r="2.6" fill="#d4ef83" stroke="none"/>
          </g>
          <text x="284" y="293" text-anchor="middle" font-size="12.5" font-weight="600" fill="#dcecef" class="loop__lbl" letter-spacing=".3">Cashback</text>
          <g filter="url(#gl)"><circle cx="76" cy="240" r="34" fill="url(#nodeG)" stroke="url(#ringG)" stroke-width="2"/></g>
          <g transform="translate(76,240) rotate(45)" stroke="#d4ef83" stroke-width="2.2" fill="none" stroke-linejoin="round">
            <rect x="-9.5" y="-9.5" width="19" height="19" rx="4.5"/>
          </g>
          <g transform="translate(76,240)" fill="#d4ef83">
            <circle cx="0" cy="-13.4" r="1.7"/><circle cx="0" cy="13.4" r="1.7"/><circle cx="-13.4" cy="0" r="1.7"/><circle cx="13.4" cy="0" r="1.7"/>
          </g>
          <text x="76" y="293" text-anchor="middle" font-size="12.5" font-weight="600" fill="#dcecef" class="loop__lbl" letter-spacing=".3">Volta via PIX</text>
          <circle cx="180" cy="180" r="56" fill="#98C73A" filter="url(#glBig)">
            <animate attributeName="r" values="52;60;52" dur="4.2s" repeatCount="indefinite"/>
            <animate attributeName="opacity" values=".16;.30;.16" dur="4.2s" repeatCount="indefinite"/>
          </circle>
          <g><circle cx="180" cy="180" r="54" fill="none" stroke="#cdef7a" stroke-width="1.4" stroke-dasharray="2.5 9" opacity=".55"/>
             <animateTransform attributeName="transform" type="rotate" from="0 180 180" to="360 180 180" dur="20s" repeatCount="indefinite"/></g>
          <circle cx="180" cy="180" r="48" fill="url(#coinG)"/>
          <circle cx="180" cy="180" r="48" fill="none" stroke="#5f8e1c" stroke-width="1.6" opacity=".55"/>
          <ellipse cx="169" cy="165" rx="21" ry="11" fill="#ffffff" opacity=".22"/>
          <g clip-path="url(#coinClip)">
            <rect x="120" y="128" width="24" height="104" fill="#ffffff" opacity=".18" transform="skewX(-20)">
              <animate attributeName="x" values="118;250;250" keyTimes="0;0.55;1" dur="5s" repeatCount="indefinite"/>
            </rect>
          </g>
          <text x="180" y="177" text-anchor="middle" font-size="25" font-weight="800" fill="#143a02">R$</text>
          <text x="180" y="197" text-anchor="middle" font-size="10.5" font-weight="600" fill="#1c4a06">volta pra você</text>
        </svg>
      </div>
    </div></section>

    <!-- CALCULADORA -->
    <section class="calc" id="calc"><div class="wrap">
      <div class="calc__head">
        <span class="eyebrow">Faça a conta</span>
        <h2>Quanto volta pro seu bolso?</h2>
        <p>Arraste pelo seu gasto mensal e veja quanto volta pro seu bolso — todo mês, e acumulado.</p>
      </div>
      <div class="calc__card">
        <div>
          <div class="calc__label">Seu gasto por mês</div>
          <div class="calc__spend" id="spend">R$ 2.000</div>
          <input type="range" id="range" min="200" max="10000" step="100" value="2000" />
          <div class="calc__hint">Mercado, farmácia, roupas, delivery, viagens… tudo o que você já compraria.</div>
        </div>
        <div class="calc__result">
          <div class="calc__rlabel">De volta todo mês</div>
          <div class="calc__big" id="cbMonth">R$ 100</div>
          <div class="calc__sub">≈ 5% de cashback nas suas compras</div>
          <div class="calc__flip">São <b id="cbYear">R$ 1.200</b> por ano · <b id="cbFive">R$ 6.000</b> em 5 anos que voltam pra você — em vez de sumir.</div>
        </div>
      </div>
    </div></section>

    <!-- COMO FUNCIONA -->
    <section class="sec"><div class="wrap">
      <div class="sec__head"><span class="eyebrow">Simples assim</span><h2>Em 3 passos, sem mudar sua rotina</h2></div>
      <div class="steps">
        <div class="step"><div class="step__n">1</div><h3>Ative o cashback</h3><p>Crie sua conta grátis e ative o cashback nas suas lojas favoritas com um clique.</p></div>
        <div class="step"><div class="step__n">2</div><h3>Compre normalmente</h3><p>Compre online ou nas lojas do seu bairro — exatamente como você já faz.</p></div>
        <div class="step"><div class="step__n">3</div><h3>Receba via PIX</h3><p>O dinheiro cai na sua carteira Quanta e você saca na hora, via PIX.</p></div>
      </div>
    </div></section>

    <!-- QUANTA IA -->
    <section class="sec sec--alt"><div class="wrap ia">
      <div class="ia__copy">
        <span class="eyebrow" style="color:var(--teal)">Seu atalho inteligente</span>
        <h2>A <span class="hl">Quanta IA</span> faz o trabalho por você</h2>
        <p>Numa conversa, como no WhatsApp: ela busca, cota, acha cupom e mostra onde rende mais. Você só decide.</p>
        <ul class="ia__list">
          <li>Busca e cotação de produtos com o maior cashback</li>
          <li>Encontra o melhor cupom na hora</li>
          <li>Consulta o seu saldo e o resgate via PIX</li>
          <li>Lojas com cashback perto de você</li>
        </ul>
      </div>
      <div class="chat">
        <div class="chat__top"><span class="chat__av"><svg viewBox="0 0 394 399"><use href="#bq-white"/></svg></span><div><div class="chat__nm">Quanta IA</div><div class="chat__st"><i></i> online · responde na hora</div></div></div>
        <div class="chat__body" id="pvChatBody"></div>
        <button class="chat__cta" type="button">Converse você mesmo com a Quanta IA <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2"><path d="M5 12h14"/><path d="M13 6l6 6-6 6"/></svg></button>
      </div>
    </div></section>

    <!-- CUPONS + MARCAS -->
    <section class="sec"><div class="wrap">
      <div class="sec__head"><span class="eyebrow">Mais economia</span><h2>Cupons todo dia, nas marcas que você ama</h2><p>Use o cupom e ainda receba cashback por cima — nas centenas de lojas parceiras.</p></div>
      <div style="display:flex;gap:14px;justify-content:center;flex-wrap:wrap;">
        <span class="perk" style="color:var(--teal-d);background:rgba(47,119,133,.08);border-color:rgba(47,119,133,.16)">Carrefour</span>
        <span class="perk" style="color:var(--teal-d);background:rgba(47,119,133,.08);border-color:rgba(47,119,133,.16)">Casas Bahia</span>
        <span class="perk" style="color:var(--teal-d);background:rgba(47,119,133,.08);border-color:rgba(47,119,133,.16)">Lacoste</span>
        <span class="perk" style="color:var(--teal-d);background:rgba(47,119,133,.08);border-color:rgba(47,119,133,.16)">LG</span>
        <span class="perk" style="color:var(--teal-d);background:rgba(47,119,133,.08);border-color:rgba(47,119,133,.16)">Zé Delivery</span>
        <span class="perk" style="color:var(--teal-d);background:rgba(47,119,133,.08);border-color:rgba(47,119,133,.16)">+ centenas</span>
      </div>
      <div style="text-align:center;margin-top:24px;"><a class="btn btn-lime" href="/cupons">Ver cupons do dia →</a></div>
    </div></section>

    <!-- QUANTA PLUS -->
    <section class="sec sec--alt"><div class="wrap">
      <div class="plus">
        <span class="eyebrow">Quanta Plus ✦</span>
        <h2>Quer <span class="hl">ganhar ainda mais</span>?</h2>
        <p>A assinatura Plus turbina sua experiência — e, se você indicar amigos, ganha o dobro na sua rede.</p>
        <div class="plus__perks">
          <span class="perk">Cupons exclusivos</span>
          <span class="perk">Ofertas antecipadas</span>
          <span class="perk">Dobro nos ganhos de rede</span>
          <span class="perk">Agência digital premium</span>
        </div>
        <a class="btn btn-lime" href="/planos">Conhecer o Plus →</a>
        <p class="plus__price">12× de R$ 97 ou R$ 997/ano (à vista)</p>
      </div>
    </div></section>

    <!-- STATS -->
    <section class="stats"><div class="wrap stats__grid">
      <div><div class="stat__n">Milhares</div><div class="stat__l">de pessoas economizando</div></div>
      <div><div class="stat__n">Centenas</div><div class="stat__l">de lojas parceiras</div></div>
      <div><div class="stat__n">6 anos</div><div class="stat__l">de mercado</div></div>
      <div><div class="stat__n">PIX</div><div class="stat__l">saque na hora</div></div>
    </div></section>

    <!-- FAQ -->
    <section class="sec"><div class="wrap">
      <div class="sec__head"><span class="eyebrow">Dúvidas</span><h2>Perguntas rápidas</h2></div>
      <div class="faq">
        <details open><summary>O cashback é dinheiro de verdade?</summary><p>Sim. O valor acumulado vira saldo na sua carteira Quanta e pode ser sacado via PIX — não é ponto que vence nem desconto disfarçado.</p></details>
        <details><summary>Quanto custa para usar?</summary><p>Criar conta e receber cashback é grátis. Há um plano opcional (Quanta Plus) com benefícios extras.</p></details>
        <details><summary>Como eu recebo e saco?</summary><p>O cashback cai automaticamente na sua carteira após a compra confirmada. O saque é via PIX, direto pela plataforma.</p></details>
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
  title: 'Para Você — cashback de verdade | Quanta Shop',
  description: 'Toda compra vira dinheiro de volta, sacável via PIX. Cupons todo dia e a Quanta IA achando o melhor pra você.',
})

onMounted(() => {
  const fmt = (n: number) => 'R$ ' + Math.round(n).toLocaleString('pt-BR')
  const range = document.getElementById('range') as HTMLInputElement | null
  if (range) {
    function upd() {
      const v = +range!.value
      const pct = (v - 200) / (10000 - 200) * 100
      range!.style.background = `linear-gradient(90deg,var(--lime) 0%,var(--lime) ${pct}%,#dfe6e8 ${pct}%)`
      const m = v * 0.05
      const spendEl = document.getElementById('spend')
      const cbMonthEl = document.getElementById('cbMonth')
      const cbYearEl = document.getElementById('cbYear')
      const cbFiveEl = document.getElementById('cbFive')
      if (spendEl) spendEl.textContent = fmt(v)
      if (cbMonthEl) cbMonthEl.textContent = fmt(m)
      if (cbYearEl) cbYearEl.textContent = fmt(m * 12)
      if (cbFiveEl) cbFiveEl.textContent = fmt(m * 60)
    }
    range.addEventListener('input', upd)
    upd()
  }

  const body = document.getElementById('pvChatBody')
  if (!body) return
  const reduceMotion = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches
  const SCRIPT: Array<{ who?: string; html?: string; typing?: number }> = [
    { who: 'msg--u', html: 'Quero um tênis de corrida até R$400 com mais cashback 👟' },
    { typing: 1100 },
    { who: 'msg--b', html: '<small>Achei o melhor pra você:</small><div class="pcard"><img src="https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=160&q=80&auto=format&fit=crop" alt="Nike Revolution 7" width="62" height="62" loading="lazy" decoding="async"><div><div class="pn">Nike Revolution 7</div><div class="pmeta">Casas Bahia · entrega 2 dias</div><div class="price">R$ 389,90</div><span class="cb">12% cashback ≈ R$ 46,79 de volta</span></div></div>' },
    { who: 'msg--u', html: 'Tem cupom hoje?' },
    { typing: 900 },
    { who: 'msg--b', html: '<small>Sim!</small>Cupom CASA15 — 15% OFF + cashback por cima. Quer ativar?' },
  ]
  let idx = 0
  let tEl: HTMLElement | null = null
  function clearChat() { body.innerHTML = ''; idx = 0 }
  function step() {
    if (idx >= SCRIPT.length) { setTimeout(() => { clearChat(); step() }, 4200); return }
    const it = SCRIPT[idx++]
    if (it.typing) {
      tEl = document.createElement('div')
      tEl.className = 'typing'
      tEl.innerHTML = '<span></span><span></span><span></span>'
      body.appendChild(tEl)
      body.scrollTop = body.scrollHeight
      setTimeout(() => { tEl?.remove(); step() }, it.typing)
      return
    }
    const m = document.createElement('div')
    m.className = 'msg ' + it.who
    m.innerHTML = it.html!
    body.appendChild(m)
    body.scrollTop = body.scrollHeight
    setTimeout(step, it.who === 'msg--u' ? 700 : 1600)
  }
  if (reduceMotion) {
    SCRIPT.filter(s => !s.typing).forEach(it => {
      const m = document.createElement('div')
      m.className = 'msg ' + it.who
      m.innerHTML = it.html!
      body.appendChild(m)
    })
  } else {
    step()
  }
})
</script>

<style scoped>
.pv-page { --teal: #2F7785; --teal-d: #225F6B; --lime: #98C73A; --lime-d: #7aad1f; --ink: #225F6B; --bg: #f4f6f7; --gold: #FFB342; }
* { box-sizing: border-box; margin: 0; padding: 0; }
.pv-page { font-family: 'Inter', 'Jost', sans-serif; color: var(--ink); background: #fff; -webkit-font-smoothing: antialiased; }
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
.hero__wm { position: absolute; left: -120px; top: 50%; transform: translateY(-50%); width: 460px; max-width: 42%; opacity: .05; z-index: 0; pointer-events: none; user-select: none; }
.hero__in { position: relative; z-index: 1; padding: 84px 0 72px; display: grid; grid-template-columns: 1.06fr .94fr; gap: 40px; align-items: center; }
.loop { width: 100%; max-width: 380px; margin: 0 auto; display: block; }
.loop__lbl { font-family: 'Inter', sans-serif; }
@media (max-width: 860px) { .hero__in { grid-template-columns: 1fr; } .hero__art { order: 2; max-width: 340px; margin: 8px auto 0; } }
.hero .eyebrow { color: #9fd3a6; }
.hero h1 { color: #fff; font-size: clamp(34px, 6vw, 64px); font-weight: 800; margin: 14px 0 0; }
.hero h1 .hl { color: var(--lime); }
.hero__sub { font-size: clamp(16px, 2.2vw, 20px); color: #c8dde0; max-width: 620px; margin: 20px 0 0; line-height: 1.55; }
.hero__cta { display: flex; gap: 14px; flex-wrap: wrap; margin-top: 32px; }
.hero__mini { margin-top: 24px; font-size: 13px; color: #9fb6bb; }

/* CALC */
.calc { background: var(--bg); padding: 80px 0; }
.calc__head { text-align: center; max-width: 680px; margin: 0 auto 36px; }
.calc__head .eyebrow { color: var(--teal); }
.calc__head h2 { font-size: clamp(26px, 4vw, 42px); font-weight: 800; margin: 10px 0; color: #225F6B; }
.calc__head p { color: #4a5b60; font-size: 17px; }
.calc__card { background: #fff; border: 1px solid #e8eef0; border-radius: 24px; box-shadow: 0 20px 50px rgba(15, 39, 48, .08); padding: 36px; max-width: 860px; margin: 0 auto; display: grid; grid-template-columns: 1fr 1fr; gap: 36px; align-items: center; }
.calc__label { font-size: 14px; color: #6b7c81; font-weight: 600; }
.calc__spend { font-family: 'Jost', 'Inter', sans-serif; font-weight: 800; font-size: 40px; color: var(--ink); margin: 6px 0 18px; }
input[type=range] { width: 100%; height: 6px; border-radius: 999px; -webkit-appearance: none; appearance: none; background: #dfe6e8; outline: none; }
input[type=range]::-webkit-slider-thumb { -webkit-appearance: none; width: 26px; height: 26px; border-radius: 50%; background: #fff; border: 4px solid var(--lime-d); cursor: pointer; box-shadow: 0 4px 12px rgba(0, 0, 0, .18); }
.calc__hint { font-size: 12px; color: #9aa9ae; margin-top: 10px; }
.calc__result { background: linear-gradient(160deg, #102a32, #1a2332); border-radius: 18px; padding: 28px; color: #eaf3f5; text-align: center; }
.calc__rlabel { font-size: 13px; color: #9fd3a6; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; }
.calc__big { font-family: 'Jost', 'Inter', sans-serif; font-weight: 800; font-size: clamp(38px, 7vw, 56px); color: var(--lime); margin: 8px 0; line-height: 1; }
.calc__sub { font-size: 14px; color: #c8dde0; }
.calc__flip { margin-top: 16px; padding-top: 16px; border-top: 1px solid rgba(255, 255, 255, .12); font-size: 14px; color: #dff3e0; }
.calc__flip b { color: #fff; }

/* SECTIONS */
.sec { padding: 80px 0; }
.sec--alt { background: var(--bg); }
.sec__head { text-align: center; max-width: 720px; margin: 0 auto 44px; }
.sec__head .eyebrow { color: var(--teal); }
.sec__head h2 { font-size: clamp(26px, 4vw, 40px); font-weight: 800; margin: 10px 0; color: var(--ink); }
.sec__head p { color: #4a5b60; font-size: 17px; }

/* STEPS */
.steps { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; }
.step { background: #fff; border: 1px solid #e8eef0; border-radius: 18px; padding: 28px 22px; }
.step__n { width: 42px; height: 42px; border-radius: 12px; display: grid; place-items: center; font-family: 'Jost', sans-serif; font-weight: 800; color: #173a0a; background: var(--lime); margin-bottom: 14px; }
.step h3 { font-size: 18px; font-weight: 800; color: var(--ink); }
.step p { color: #4a5b60; font-size: 14px; line-height: 1.5; margin-top: 6px; }

/* QUANTA IA */
.ia { display: grid; grid-template-columns: 1.05fr .95fr; gap: 40px; align-items: center; }
.ia__copy h2 { font-size: clamp(26px, 3.6vw, 38px); font-weight: 800; color: var(--ink); }
.ia__copy h2 .hl { color: var(--lime-d); }
.ia__copy p { color: #4a5b60; font-size: 16px; margin: 14px 0 18px; line-height: 1.55; }
.ia__list { list-style: none; display: flex; flex-direction: column; gap: 10px; }
.ia__list li { display: flex; gap: 10px; font-size: 15px; color: #33454a; }
.ia__list li::before { content: '✓'; color: var(--lime-d); font-weight: 800; }
.chat { background: #0c1a21; border: 1px solid rgba(255, 255, 255, .1); border-radius: 24px; padding: 18px; box-shadow: 0 24px 60px rgba(1, 15, 28, .25); }
.chat__top { display: flex; align-items: center; gap: 10px; padding: 4px 4px 14px; border-bottom: 1px solid rgba(255, 255, 255, .08); }
.chat__av { width: 44px; height: 44px; border-radius: 50%; background: linear-gradient(135deg, #3A9AAD, #98C73A); display: grid; place-items: center; }
.chat__av svg { width: 26px; height: 26px; }
.chat__nm { color: #fff; font-weight: 700; font-size: 15px; font-family: 'Jost', 'Inter', sans-serif; }
.chat__st { color: #9fd3a6; font-size: 12px; display: flex; align-items: center; gap: 6px; }
.chat__st i { width: 7px; height: 7px; border-radius: 50%; background: #98C73A; display: inline-block; }
.chat__cta { margin-top: 12px; width: 100%; display: flex; align-items: center; justify-content: center; gap: 8px; background: rgba(152, 199, 58, .14); color: #d4ef9f; border: 1px solid rgba(152, 199, 58, .3); border-radius: 14px; padding: 12px; font-family: 'Inter', sans-serif; font-weight: 700; font-size: 14px; cursor: pointer; transition: .2s; }
.chat__cta:hover { background: rgba(152, 199, 58, .22); }
.chat__cta svg { width: 16px; height: 16px; }
.chat__body { padding: 16px 4px 4px; display: flex; flex-direction: column; gap: 10px; min-height: 300px; max-height: 360px; overflow: hidden; }
@keyframes pop { to { opacity: 1; transform: none; } }
@keyframes blink { 0%, 60%, 100% { opacity: .3; transform: translateY(0) } 30% { opacity: 1; transform: translateY(-3px) } }
.chat__body :deep(.msg) { max-width: 86%; font-size: 14px; line-height: 1.45; padding: 11px 14px; border-radius: 16px; opacity: 0; transform: translateY(10px) scale(.98); animation: pop .5s forwards; }
.chat__body :deep(.msg--u) { align-self: flex-end; background: linear-gradient(180deg, #2f7785, #266472); color: #eafaff; border-bottom-right-radius: 5px; }
.chat__body :deep(.msg--b) { align-self: flex-start; background: #15262e; color: #d7e7ea; border: 1px solid rgba(255, 255, 255, .06); border-bottom-left-radius: 5px; }
.chat__body :deep(.typing) { align-self: flex-start; background: #15262e; border: 1px solid rgba(255, 255, 255, .06); border-radius: 16px; border-bottom-left-radius: 5px; padding: 14px 16px; display: flex; gap: 5px; }
.chat__body :deep(.typing span) { width: 7px; height: 7px; border-radius: 50%; background: #6f9aa1; animation: blink 1.2s infinite; }
.chat__body :deep(.typing span:nth-child(2)) { animation-delay: .2s; }
.chat__body :deep(.typing span:nth-child(3)) { animation-delay: .4s; }
.chat__body :deep(.msg small) { display: block; color: #9fd3a6; font-weight: 600; font-size: 12px; margin-bottom: 8px; }
.chat__body :deep(.pcard) { display: flex; gap: 12px; background: #0c1a21; border: 1px solid rgba(255, 255, 255, .08); border-radius: 14px; padding: 10px; margin-top: 6px; }
.chat__body :deep(.pcard img) { width: 62px; height: 62px; border-radius: 10px; object-fit: cover; flex-shrink: 0; background: #15262e; }
.chat__body :deep(.pcard .pn) { font-weight: 700; color: #fff; font-size: 13px; }
.chat__body :deep(.pcard .pmeta) { font-size: 11px; color: #8fb3b8; margin: 2px 0 6px; }
.chat__body :deep(.pcard .price) { font-family: 'Jost', 'Inter', sans-serif; font-weight: 700; color: #fff; font-size: 15px; }
.chat__body :deep(.pcard .cb) { display: inline-block; background: rgba(152, 199, 58, .16); color: #98C73A; font-size: 11px; font-weight: 700; padding: 2px 8px; border-radius: 999px; margin-top: 4px; }

/* PLUS */
.plus { background: linear-gradient(160deg, #102a32, #0c1c24); color: #eaf3f5; border-radius: 26px; padding: 44px; text-align: center; max-width: 860px; margin: 0 auto; }
.plus .eyebrow { color: var(--gold); }
.plus h2 { color: #fff; font-size: clamp(24px, 3.4vw, 36px); font-weight: 800; margin: 10px 0 8px; }
.plus h2 .hl { color: var(--lime); }
.plus p { color: #c8dde0; max-width: 560px; margin: 0 auto 18px; }
.plus__perks { display: flex; flex-wrap: wrap; gap: 10px; justify-content: center; margin-bottom: 24px; }
.perk { font-size: 13px; font-weight: 600; color: #dff3e0; background: rgba(255, 255, 255, .08); border: 1px solid rgba(255, 255, 255, .16); border-radius: 999px; padding: 8px 14px; }
.plus__price { font-size: 14px; color: #9fb6bb; margin-top: 14px; }

/* STATS */
.stats { padding: 64px 0; background: var(--bg); }
.stats__grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 18px; text-align: center; }
.stat__n { font-family: 'Jost', 'Inter', sans-serif; font-weight: 800; font-size: clamp(26px, 4vw, 40px); color: var(--teal); }
.stat__l { font-size: 13px; color: #6b7c81; margin-top: 4px; }

/* FAQ */
.faq { max-width: 760px; margin: 0 auto; }
.faq details { background: transparent; border: 1px solid #e8eef0; border-radius: 14px; padding: 4px 18px; margin-bottom: 12px; }
.faq summary { cursor: pointer; font-weight: 700; color: var(--ink); padding: 14px 0; font-size: 15.5px; list-style: none; }
.faq summary::-webkit-details-marker { display: none; }
.faq summary::after { content: '+'; float: right; color: var(--teal); font-weight: 800; }
.faq details[open] summary::after { content: '–'; }
.faq p { color: #4a5b60; font-size: 14.5px; line-height: 1.55; padding: 0 0 16px; }

/* CTA */
.cta { background: radial-gradient(700px 400px at 80% 0, rgba(58, 154, 173, .5), transparent 60%), linear-gradient(160deg, #0f2730, #0c1c24); color: #fff; padding: 84px 0; text-align: center; }
.cta h2 { font-size: clamp(28px, 4.5vw, 46px); font-weight: 800; color: #fff; }
.cta h2 .hl { color: var(--lime); }
.cta p { color: #c8dde0; margin: 16px 0 30px; font-size: 17px; }

@media (max-width: 860px) { .calc__card, .ia, .steps, .stats__grid { grid-template-columns: 1fr; } .stats__grid { grid-template-columns: repeat(2, 1fr); } }
</style>
