<template>
  <main class="qa-page">
    <!-- ============ HERO ============ -->
    <section class="hero">
      <span class="hero__orb hero__orb--1"></span><span class="hero__orb hero__orb--2"></span><svg class="hero__sparks" viewBox="0 0 1200 600" preserveAspectRatio="xMidYMid slice" aria-hidden="true"><defs><filter id="sparkGl" x="-200%" y="-200%" width="500%" height="500%"><feGaussianBlur stdDeviation="1.6"/></filter></defs><circle cx="120" cy="420" r="2.4" fill="#cdef7a" filter="url(#sparkGl)"><animate attributeName="cy" values="420;350" dur="6.0s" begin="-0.0s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;.8;0" dur="6.0s" begin="-0.0s" repeatCount="indefinite"/></circle><circle cx="280" cy="520" r="1.8" fill="#cdef7a" filter="url(#sparkGl)"><animate attributeName="cy" values="520;450" dur="7.3s" begin="-0.7s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;.8;0" dur="7.3s" begin="-0.7s" repeatCount="indefinite"/></circle><circle cx="430" cy="460" r="2.1" fill="#cdef7a" filter="url(#sparkGl)"><animate attributeName="cy" values="460;390" dur="8.6s" begin="-1.4s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;.8;0" dur="8.6s" begin="-1.4s" repeatCount="indefinite"/></circle><circle cx="610" cy="540" r="2.6" fill="#cdef7a" filter="url(#sparkGl)"><animate attributeName="cy" values="540;470" dur="9.9s" begin="-2.1s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;.8;0" dur="9.9s" begin="-2.1s" repeatCount="indefinite"/></circle><circle cx="760" cy="470" r="1.9" fill="#cdef7a" filter="url(#sparkGl)"><animate attributeName="cy" values="470;400" dur="6.0s" begin="-2.8s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;.8;0" dur="6.0s" begin="-2.8s" repeatCount="indefinite"/></circle><circle cx="900" cy="520" r="2.3" fill="#cdef7a" filter="url(#sparkGl)"><animate attributeName="cy" values="520;450" dur="7.3s" begin="-3.5s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;.8;0" dur="7.3s" begin="-3.5s" repeatCount="indefinite"/></circle><circle cx="1050" cy="460" r="2.0" fill="#cdef7a" filter="url(#sparkGl)"><animate attributeName="cy" values="460;390" dur="8.6s" begin="-4.2s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;.8;0" dur="8.6s" begin="-4.2s" repeatCount="indefinite"/></circle><circle cx="1180" cy="510" r="2.4" fill="#cdef7a" filter="url(#sparkGl)"><animate attributeName="cy" values="510;440" dur="9.9s" begin="-4.9s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;.8;0" dur="9.9s" begin="-4.9s" repeatCount="indefinite"/></circle></svg>
      <img class="hero__wm" alt="" aria-hidden="true" src="/img/logo/quanta-icon-white.png" />
      <!-- selo R$50 por indicação -->
      <svg class="seal" viewBox="0 0 140 140" aria-hidden="true">
        <defs><path id="sArc" d="M70,70 m-41,0 a41,41 0 1,1 82,0 a41,41 0 1,1 -82,0" fill="none"/></defs>
        <ellipse cx="70" cy="74" rx="60" ry="60" fill="rgba(8,30,20,.28)" opacity=".0"/>
        <circle cx="125.0" cy="70.0" r="7.6" fill="#8cbf38"/><circle cx="123.1" cy="84.2" r="7.6" fill="#8cbf38"/><circle cx="117.6" cy="97.5" r="7.6" fill="#8cbf38"/><circle cx="108.9" cy="108.9" r="7.6" fill="#8cbf38"/><circle cx="97.5" cy="117.6" r="7.6" fill="#8cbf38"/><circle cx="84.2" cy="123.1" r="7.6" fill="#8cbf38"/><circle cx="70.0" cy="125.0" r="7.6" fill="#8cbf38"/><circle cx="55.8" cy="123.1" r="7.6" fill="#8cbf38"/><circle cx="42.5" cy="117.6" r="7.6" fill="#8cbf38"/><circle cx="31.1" cy="108.9" r="7.6" fill="#8cbf38"/><circle cx="22.4" cy="97.5" r="7.6" fill="#8cbf38"/><circle cx="16.9" cy="84.2" r="7.6" fill="#8cbf38"/><circle cx="15.0" cy="70.0" r="7.6" fill="#8cbf38"/><circle cx="16.9" cy="55.8" r="7.6" fill="#8cbf38"/><circle cx="22.4" cy="42.5" r="7.6" fill="#8cbf38"/><circle cx="31.1" cy="31.1" r="7.6" fill="#8cbf38"/><circle cx="42.5" cy="22.4" r="7.6" fill="#8cbf38"/><circle cx="55.8" cy="16.9" r="7.6" fill="#8cbf38"/><circle cx="70.0" cy="15.0" r="7.6" fill="#8cbf38"/><circle cx="84.2" cy="16.9" r="7.6" fill="#8cbf38"/><circle cx="97.5" cy="22.4" r="7.6" fill="#8cbf38"/><circle cx="108.9" cy="31.1" r="7.6" fill="#8cbf38"/><circle cx="117.6" cy="42.5" r="7.6" fill="#8cbf38"/><circle cx="123.1" cy="55.8" r="7.6" fill="#8cbf38"/>
        <circle cx="70" cy="70" r="55" fill="#98C73A"/>
        <circle cx="70" cy="70" r="55" fill="none" stroke="#7aad2f" stroke-width="1.5"/>
        <circle cx="70" cy="70" r="49.5" fill="none" stroke="#eef6dd" stroke-width="2.4"/>
        <circle cx="70" cy="70" r="47" fill="#0f3a43"/>
        <g><text font-family="Jost,sans-serif" font-size="8" font-weight="700" fill="#cdef7a" letter-spacing="2.4"><textPath href="#sArc" startOffset="0">CADA&#160;INDICA&#199;&#195;O&#160;&#160;&#8226;&#160;&#160;CADA&#160;INDICA&#199;&#195;O&#160;&#160;&#8226;&#160;&#160;</textPath></text>
           <animateTransform attributeName="transform" type="rotate" from="0 70 70" to="360 70 70" dur="28s" repeatCount="indefinite"/></g>
        <circle cx="70" cy="70" r="33" fill="#a6db4a"/>
        <circle cx="70" cy="70" r="33" fill="none" stroke="#0f3a43" stroke-width="1.5"/>
        <path d="M48,67 a22,22 0 0 1 44,0" fill="none" stroke="#ffffff" stroke-opacity=".30" stroke-width="1.5"/>
        <text x="70" y="66" text-anchor="middle" font-family="Jost,sans-serif" font-size="13" font-weight="800" fill="#0f3a43" letter-spacing="1">R$</text>
        <text x="70" y="92" text-anchor="middle" font-family="Jost,sans-serif" font-size="33" font-weight="900" fill="#0f3a43" letter-spacing="-1">50</text>
      </svg>

      <div class="hero__in wrap">
        <span class="eyebrow">Quanta Amizade &middot; Campanha</span>
        <h1>Compre aqui e <span class="hl">ganhe dinheiro</span></h1>
        <div class="hero__pill"><b>Indique</b> amigos e <b>ganhe</b> muito mais</div>

        <div class="ladder">
          <div class="rung">
            <div class="rung__icons">
              <i><svg width="26" height="36" viewBox="0 0 26 36"><circle cx="13" cy="9" r="8" fill="none" stroke="currentColor" stroke-width="2.4"/><path d="M3 35c0-7 4.5-12 10-12s10 5 10 12" fill="none" stroke="currentColor" stroke-width="2.4"/></svg></i>
            </div>
            <div class="rung__people">1 amigo</div>
            <div class="rung__eq">=</div>
            <div class="rung__val"><span>R$</span> 50</div>
          </div>
          <div class="rung">
            <div class="rung__icons">
              <i><svg width="20" height="28" viewBox="0 0 26 36"><circle cx="13" cy="9" r="8" fill="none" stroke="currentColor" stroke-width="2.4"/><path d="M3 35c0-7 4.5-12 10-12s10 5 10 12" fill="none" stroke="currentColor" stroke-width="2.4"/></svg></i>
              <i><svg width="26" height="36" viewBox="0 0 26 36"><circle cx="13" cy="9" r="8" fill="none" stroke="currentColor" stroke-width="2.4"/><path d="M3 35c0-7 4.5-12 10-12s10 5 10 12" fill="none" stroke="currentColor" stroke-width="2.4"/></svg></i>
              <i><svg width="20" height="28" viewBox="0 0 26 36"><circle cx="13" cy="9" r="8" fill="none" stroke="currentColor" stroke-width="2.4"/><path d="M3 35c0-7 4.5-12 10-12s10 5 10 12" fill="none" stroke="currentColor" stroke-width="2.4"/></svg></i>
            </div>
            <div class="rung__people">10 amigos</div>
            <div class="rung__eq">=</div>
            <div class="rung__val"><span>R$</span> 500</div>
          </div>
          <div class="rung">
            <div class="rung__icons">
              <i><svg width="20" height="28" viewBox="0 0 26 36"><circle cx="13" cy="9" r="8" fill="none" stroke="currentColor" stroke-width="2.4"/><path d="M3 35c0-7 4.5-12 10-12s10 5 10 12" fill="none" stroke="currentColor" stroke-width="2.4"/></svg></i>
              <i><svg width="26" height="36" viewBox="0 0 26 36"><circle cx="13" cy="9" r="8" fill="none" stroke="currentColor" stroke-width="2.4"/><path d="M3 35c0-7 4.5-12 10-12s10 5 10 12" fill="none" stroke="currentColor" stroke-width="2.4"/></svg></i>
              <i><svg width="20" height="28" viewBox="0 0 26 36"><circle cx="13" cy="9" r="8" fill="none" stroke="currentColor" stroke-width="2.4"/><path d="M3 35c0-7 4.5-12 10-12s10 5 10 12" fill="none" stroke="currentColor" stroke-width="2.4"/></svg></i>
            </div>
            <div class="rung__people">100 amigos</div>
            <div class="rung__eq">=</div>
            <div class="rung__val"><span>R$</span> 5.000</div>
          </div>
        </div>

        <div class="hero__cta">
          <a class="btn btn-lime" href="/register">Indicar e ganhar &rarr;</a>
          <a class="btn btn-ghost" href="#como">Como funciona</a>
        </div>
        <p class="hero__legal">Crédito sacável via PIX &middot; Aberto a todos os membros &middot; *Veja regulamento e regras abaixo.</p>
      </div>
    </section>

    <!-- ============ COMO FUNCIONA ============ -->
    <section class="sec" id="como"><div class="wrap">
      <div class="sec__head">
        <span class="eyebrow">Simples assim</span>
        <h2>Como você ganha por indicar</h2>
        <p>Cada indicação que vira consumo de verdade rende R$ 50 — divididos entre você e quem você indicou.</p>
      </div>
      <div class="steps">
        <div class="step reveal">
          <div class="step__n">1</div>
          <h3>Indique</h3>
          <p>Compartilhe seu link ou QR code com amigos, familiares e lojas. Sem limite de indicações.</p>
        </div>
        <div class="step reveal">
          <div class="step__n">2</div>
          <h3>Ambos consomem</h3>
          <p>A indicação qualifica quando você e seu amigo acumulam, cada um, R$ 50 de cashback nas próprias compras.</p>
        </div>
        <div class="step reveal">
          <div class="step__n">3</div>
          <h3>Cai na carteira</h3>
          <p>O bônus é creditado em até 24h na carteira Quanta de cada um — e pode ser sacado via PIX.</p>
        </div>
      </div>

      <!-- explicação do rateio 25/25 -->
      <div class="split reveal">
        <div class="split__top">
          <span class="eyebrow">Transparência total</span>
          <h3>Os R$ 50 são divididos: metade sua, metade do amigo</h3>
          <p>No material impresso você vê &ldquo;R$ 50 por indicação&rdquo;. Aqui mostramos como esse valor chega a cada um — porque na Quanta o ganho é compartilhado.</p>
        </div>
        <div class="split__viz">
          <div class="half">
            <div class="half__who">Para você</div>
            <div class="half__val">R$ 25</div>
            <div class="half__cap">quem indicou</div>
          </div>
          <div class="split__plus">+</div>
          <div class="half">
            <div class="half__who">Para o amigo</div>
            <div class="half__val">R$ 25</div>
            <div class="half__cap">quem foi indicado</div>
          </div>
        </div>
        <div class="split__foot">
          <span class="chip">Crédito em até 24h</span>
          <span class="chip">Sacável via PIX</span>
          <span class="chip">Liberado quando ambos somam R$ 50 de cashback</span>
        </div>
      </div>
    </div></section>

    <!-- ============ CALCULADORA ============ -->
    <section class="sec sec--alt"><div class="wrap">
      <div class="sec__head">
        <span class="eyebrow">Faça as contas</span>
        <h2>Quanto sua rede de amigos rende</h2>
        <p>Arraste e veja seu bônus de indicação — R$ 25 para cada amigo que qualifica.</p>
      </div>
      <div class="calc__card">
        <div>
          <div class="calc__label">Amigos que você indica</div>
          <div class="calc__spend" id="friends">20 amigos</div>
          <input type="range" id="range" min="1" max="100" value="20" />
          <div class="calc__hint">Considera indicações qualificadas (você e o amigo acumulando R$ 50 de cashback cada).</div>
        </div>
        <div class="calc__result">
          <div class="calc__rlabel">Seu bônus de indicação</div>
          <div class="calc__big" id="total">R$ 500</div>
          <div class="calc__sub" id="friendsmsg">20 amigos &times; R$ 25 para você</div>
          <div class="calc__flip">E ainda <b id="shared">R$ 500</b> distribuídos aos seus amigos — a rede inteira ganha junto.</div>
        </div>
      </div>
    </div></section>

    <!-- ============ PRÊMIO CARRO ============ -->
    <section class="cars">
      <span class="coin c1"></span><span class="coin c2"></span><span class="coin c3"></span><span class="coin c4"></span>
      <svg width="0" height="0" style="position:absolute" aria-hidden="true"><defs><linearGradient id="bodyQ" x1="0" y1="0" x2="0" y2="1"><stop offset="0" stop-color="#ffffff"/><stop offset="42%" stop-color="#eef2f4"/><stop offset="100%" stop-color="#aeb7bd"/></linearGradient><linearGradient id="glassQ" x1="0" y1="0" x2="0" y2="1"><stop offset="0" stop-color="#33414c"/><stop offset="100%" stop-color="#0f1820"/></linearGradient><radialGradient id="rimQ" cx="42%" cy="38%" r="65%"><stop offset="0" stop-color="#f4f7f9"/><stop offset="100%" stop-color="#aab2b9"/></radialGradient><symbol id="argoSym" viewBox="0 0 520 240"><ellipse cx="262" cy="212" rx="216" ry="15" fill="rgba(0,0,0,.28)"/><path d="M40,178 C38,150 42,140 62,136 C150,126 372,126 446,140 C470,145 480,158 480,176 L480,178 Z" fill="url(#bodyQ)"/><path d="M150,132 L200,90 L320,86 L372,132 Z" fill="url(#bodyQ)"/><path d="M166,130 L204,98 L256,96 L256,130 Z" fill="url(#glassQ)"/><path d="M266,96 L312,94 L352,130 L266,130 Z" fill="url(#glassQ)"/><path d="M44,176 C150,172 380,172 480,176 L480,184 C380,182 150,182 44,182 Z" fill="#16181c"/><path d="M44,142 L62,138 L66,150 L46,152 Z" fill="#eaf2f6"/><path d="M474,146 L462,144 L462,156 L474,156 Z" fill="#c8302a"/><rect x="226" y="120" width="20" height="4" rx="2" fill="#8b969d"/><path d="M104,178 a36,36 0 0 1 72,0 Z" fill="#16181c"/><path d="M356,178 a36,36 0 0 1 72,0 Z" fill="#16181c"/><circle cx="140" cy="178" r="37" fill="#10151b"/><circle cx="140" cy="178" r="36" fill="none" stroke="#2a3138" stroke-width="2"/><circle cx="140" cy="178" r="23" fill="url(#rimQ)"/><path d="M140,178 L136.4,158.3 L140.0,158.0 Z" fill="#cfd6db"/><path d="M140,178 L157.6,168.5 L159.0,171.8 Z" fill="#cfd6db"/><path d="M140,178 L154.5,191.8 L151.8,194.2 Z" fill="#cfd6db"/><path d="M140,178 L131.3,196.0 L128.2,194.2 Z" fill="#cfd6db"/><path d="M140,178 L120.2,175.3 L121.0,171.8 Z" fill="#cfd6db"/><circle cx="140" cy="178" r="6.5" fill="#9aa3ab"/><circle cx="392" cy="178" r="37" fill="#10151b"/><circle cx="392" cy="178" r="36" fill="none" stroke="#2a3138" stroke-width="2"/><circle cx="392" cy="178" r="23" fill="url(#rimQ)"/><path d="M392,178 L388.4,158.3 L392.0,158.0 Z" fill="#cfd6db"/><path d="M392,178 L409.6,168.5 L411.0,171.8 Z" fill="#cfd6db"/><path d="M392,178 L406.5,191.8 L403.8,194.2 Z" fill="#cfd6db"/><path d="M392,178 L383.3,196.0 L380.2,194.2 Z" fill="#cfd6db"/><path d="M392,178 L372.2,175.3 L373.0,171.8 Z" fill="#cfd6db"/><circle cx="392" cy="178" r="6.5" fill="#9aa3ab"/></symbol></defs></svg>
      <div class="wrap">
        <div class="cars__word reveal"><span class="q">Quanta</span><span class="a">amizade</span></div>
        <p class="cars__tag">É fácil — se você fizer isso de forma inteligente.</p>
        <div class="cars__bar reveal"><div class="cars__num">2</div><div class="cars__bartxt">Carros <b>sem sorteio</b></div></div>
        <div class="showroom reveal">
          <div class="showroom__grid">
            <div class="prizecar"><span class="prizecar__spot"></span><div class="prizecar__ribbon"><svg viewBox="0 0 24 24" width="14" height="14" fill="none" stroke="#0f3a43" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M8 21h8M12 17v4M6 4h12v4a6 6 0 0 1-12 0z"/><path d="M6 6H4a2 2 0 0 0 2 4M18 6h2a2 2 0 0 1-2 4"/></svg> LOJA</div><div class="prizecar__stagecar"><img class="prizecar__img" src="https://production.autoforce.com/uploads/version/profile_image/12535/model_main_webp_comprar-1-0-mt-flex_3d22a86194.png.webp" alt="Fiat Argo 0 km" loading="lazy" decoding="async" referrerpolicy="no-referrer" onerror="this.style.display='none';var s=this.parentNode.querySelector('.prizecar__svg');if(s){s.style.display='block';}"><svg class="prizecar__svg" style="display:none" viewBox="0 0 520 240"><use href="#argoSym"/></svg></div><div class="prizecar__cap"><b>Para a loja</b> que mais indica quem compra</div></div>
            <div class="prizecar"><span class="prizecar__spot"></span><div class="prizecar__ribbon"><svg viewBox="0 0 24 24" width="14" height="14" fill="none" stroke="#0f3a43" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M8 21h8M12 17v4M6 4h12v4a6 6 0 0 1-12 0z"/><path d="M6 6H4a2 2 0 0 0 2 4M18 6h2a2 2 0 0 1-2 4"/></svg> CLIENTE</div><div class="prizecar__stagecar"><img class="prizecar__img" src="https://production.autoforce.com/uploads/version/profile_image/12535/model_main_webp_comprar-1-0-mt-flex_3d22a86194.png.webp" alt="Fiat Argo 0 km" loading="lazy" decoding="async" referrerpolicy="no-referrer" onerror="this.style.display='none';var s=this.parentNode.querySelector('.prizecar__svg');if(s){s.style.display='block';}"><svg class="prizecar__svg" style="display:none" viewBox="0 0 520 240"><use href="#argoSym"/></svg></div><div class="prizecar__cap"><b>Para o cliente</b> que mais indica quem compra</div></div>
          </div>
          <div class="showroom__cap"><span class="showroom__brand">Fiat Argo</span> · 2 unidades 0 km · zero sorteio · por produção</div>
        </div>
        <p class="cars__fine">*Imagem do veículo meramente ilustrativa; cor e versão podem variar. Premiação por produção do ecossistema, sem sorteio, conforme regulamento.</p>
      </div>
    </section>

    <!-- ============ FAQ / REGRAS ============ -->
    <section class="sec"><div class="wrap">
      <div class="sec__head"><span class="eyebrow">Regras rápidas</span><h2>Perguntas frequentes</h2></div>
      <div class="faq">
        <details open><summary>Quem pode participar?</summary><p>Todos os membros da Quanta — consumidores, lojistas, empresas e Agentes de Fidelização. A campanha Quanta Amizade é aberta a todos, sem custo para participar.</p></details>
        <details><summary>O material impresso diz R$ 50. Por que aqui aparece R$ 25 + R$ 25?</summary><p>São a mesma coisa: cada indicação qualificada gera R$ 50, divididos em R$ 25 para quem indicou e R$ 25 para quem foi indicado. O impresso mostra o total; aqui detalhamos o rateio.</p></details>
        <details><summary>Quando o bônus é liberado?</summary><p>Quando você e seu amigo acumularem, cada um, R$ 50 de cashback nas próprias compras. A partir daí o crédito cai em até 24h na carteira de cada um.</p></details>
        <details><summary>Posso sacar?</summary><p>Sim. Os créditos da Quanta Amizade podem ser sacados via PIX, direto pela plataforma.</p></details>
        <details><summary>E se meu amigo sair logo depois?</summary><p>Se o indicado deixar a plataforma antes de 6 meses, o bônus correspondente é estornado. A campanha premia relacionamentos reais e duradouros.</p></details>
        <details><summary>Como funcionam os carros?</summary><p>Dois carros, sem sorteio: um para a loja que mais indica quem compra e um para o cliente que mais indica quem compra, por ecossistema, conforme a produção definida no regulamento.</p></details>
      </div>
    </div></section>

    <!-- ============ LEGAL ============ -->
    <section class="legal"><div class="wrap">
      <div class="legal__box">
        Campanha <b>Quanta Amizade</b> &middot; vigência de <b>01/06/2026 a 01/06/2027</b>. Bônus custeado pelo fundo de Marketing e Promoções. Crédito em até 24h após a qualificação; estorno em caso de saída do indicado antes de 6 meses. Premiação de carros por produção do ecossistema, sem sorteio, conforme regulamento.<br/>
        Realização: <b>TRIUMPH BANK MARKETING LTDA</b> &middot; CNPJ 34.985.528/0001-89 &middot; contato@quantashop.com.br. Consulte o regulamento completo antes de participar.
      </div>
    </div></section>

    <!-- ============ CTA ============ -->
    <section class="cta"><div class="wrap">
      <h2>Indique amigos. <span class="hl">Ganhem juntos.</span></h2>
      <p>Pegue seu link, compartilhe e veja a rede crescer — com cashback de verdade para todo mundo.</p>
      <a class="btn btn-lime" href="/register">Começar a indicar &rarr;</a>
    </div></section>
  </main>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' })
useSeoMeta({
  title: 'Quanta Amizade — indique amigos e ganhe | Quanta Shop',
  description: 'Indique e ganhe: R$ 50 por indicação qualificada (R$ 25 para você + R$ 25 para o amigo), sacável via PIX. E concorra a 2 carros 0 km, sem sorteio, por produção.',
})
useHead({
  link: [
    { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
    { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' },
    { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&family=Jost:wght@400;500;600;700;800;900&display=swap' },
  ],
})

onMounted(() => {
  const fmt = (n: number) => 'R$ ' + Math.round(n).toLocaleString('pt-BR')
  const range = document.getElementById('range') as HTMLInputElement | null
  if (range) {
    function upd() {
      const v = +range!.value, pct = (v - 1) / (100 - 1) * 100
      range!.style.background = `linear-gradient(90deg,var(--lime) 0%,var(--lime) ${pct}%,#dfe6e8 ${pct}%)`
      const yours = v * 25
      const friendsEl = document.getElementById('friends')
      const totalEl = document.getElementById('total')
      const msgEl = document.getElementById('friendsmsg')
      const sharedEl = document.getElementById('shared')
      if (friendsEl) friendsEl.textContent = v + (v === 1 ? ' amigo' : ' amigos')
      if (totalEl) totalEl.textContent = fmt(yours)
      if (msgEl) msgEl.textContent = v + ' ' + (v === 1 ? 'amigo' : 'amigos') + ' × R$ 25 para você'
      if (sharedEl) sharedEl.textContent = fmt(yours)
    }
    range.addEventListener('input', upd)
    upd()
  }

  const io = new IntersectionObserver((es) => {
    es.forEach(e => { if (e.isIntersecting) { e.target.classList.add('in'); io.unobserve(e.target) } })
  }, { threshold: .15 })
  document.querySelectorAll('.reveal').forEach(el => io.observe(el))
})
</script>

<style scoped>
.qa-page { --teal: #2F7785; --teal-d: #225F6B; --lime: #98C73A; --lime-d: #7aad1f; --ink: #225F6B; --bg: #f4f6f7; --gold: #FFB342; }
* { box-sizing: border-box; margin: 0; padding: 0; }
.qa-page { font-family: 'Inter', 'Jost', sans-serif; color: var(--ink); background: #fff; -webkit-font-smoothing: antialiased; }
.wrap { max-width: 1120px; margin: 0 auto; padding: 0 24px; }
h1, h2, h3 { font-family: 'Jost', 'Inter', sans-serif; letter-spacing: -.02em; line-height: 1.05; }
.eyebrow { font-size: 13px; font-weight: 700; letter-spacing: .08em; text-transform: uppercase; }
.btn { display: inline-flex; align-items: center; gap: 8px; font-weight: 700; font-size: 15px; border-radius: 999px; padding: 14px 26px; text-decoration: none; cursor: pointer; border: 0; transition: .2s; }
.btn-lime { background: linear-gradient(180deg, var(--lime), var(--lime-d)); color: #173a0a; box-shadow: 0 12px 30px rgba(152,199,58,.35); }
.btn-lime:hover { transform: translateY(-2px); box-shadow: 0 18px 40px rgba(152,199,58,.45); }
.btn-ghost { background: transparent; border: 1.5px solid rgba(255,255,255,.35); color: #fff; }
.btn-ghost:hover { background: rgba(255,255,255,.1); }

/* ============ HERO ============ */
.hero { position: relative; overflow: hidden; color: #eaf3f5; background: radial-gradient(900px 520px at 85% -10%, rgba(58,154,173,.45), transparent 60%), linear-gradient(160deg, #114049 0%, #16505b 48%, #0f3a43 100%); }
.hero__wm { position: absolute; right: -90px; top: 8%; width: 420px; max-width: 40%; opacity: .06; z-index: 0; pointer-events: none; user-select: none; }
.hero__in { position: relative; z-index: 1; padding: 80px 0 64px; text-align: center; }
.hero .eyebrow { color: #cfe8a6; }
.hero h1 { color: #fff; font-size: clamp(34px, 6.4vw, 68px); font-weight: 900; margin: 14px 0 0; }
.hero h1 .hl { color: var(--lime); display: block; }
.hero__pill { display: inline-block; margin: 22px 0 0; background: linear-gradient(180deg, var(--lime), var(--lime-d)); color: #173a0a; font-weight: 800; font-size: clamp(15px, 2.2vw, 20px); padding: 11px 26px; border-radius: 999px; box-shadow: 0 12px 30px rgba(152,199,58,.3); }
.hero__pill b { color: #0f2a06; }

/* escada 1/10/100 */
.ladder { display: grid; grid-template-columns: repeat(3, 1fr); gap: 0; max-width: 760px; margin: 44px auto 0; }
.rung { padding: 14px 16px; position: relative; border-radius: 18px; transition: transform .25s ease, background .25s ease; }
.rung:hover { transform: translateY(-3px); background: rgba(255,255,255,.04); }
.rung + .rung { border-left: 1px solid rgba(255,255,255,.14); }
.rung__icons { height: 46px; display: flex; align-items: flex-end; justify-content: center; gap: 3px; margin-bottom: 12px; }
.rung__icons i { display: block; color: var(--lime); filter: drop-shadow(0 0 7px rgba(152,199,58,.55)); }
.rung__people { font-family: 'Jost', sans-serif; font-weight: 700; font-size: clamp(15px, 2.4vw, 20px); color: var(--lime); text-shadow: 0 0 13px rgba(152,199,58,.4); }
.rung__eq { color: #8fb0b6; font-size: 13px; margin: 2px 0 4px; }
.rung__val { font-family: 'Jost', sans-serif; font-weight: 800; font-size: clamp(22px, 4.6vw, 40px); color: #fff; text-shadow: 0 3px 18px rgba(6,24,22,.45); }
.rung__val span { font-size: .6em; font-weight: 700; color: #cfe2e5; vertical-align: top; }
.hero__cta { display: flex; gap: 14px; flex-wrap: wrap; justify-content: center; margin-top: 40px; }
.hero__legal { margin-top: 18px; font-size: 12px; color: #8fb0b6; }

/* selo R$50 */
.seal { position: absolute; right: 5%; top: 64px; width: 128px; height: 128px; z-index: 2; filter: drop-shadow(0 10px 24px rgba(152,199,58,.45)); }
@media(max-width:760px) { .seal { width: 92px; height: 92px; right: 18px; top: 18px; } }

/* orbs + sparks */
.hero__orb { position: absolute; border-radius: 50%; filter: blur(72px); z-index: 0; opacity: .5; pointer-events: none; animation: float 17s ease-in-out infinite; }
.hero__orb--1 { width: 360px; height: 360px; background: #3A9AAD; top: -120px; left: -70px; }
.hero__orb--2 { width: 300px; height: 300px; background: #98C73A; bottom: -130px; right: 6%; opacity: .26; animation-delay: -7s; }
.hero__sparks { position: absolute; inset: 0; width: 100%; height: 100%; z-index: 0; pointer-events: none; }
@keyframes float { 0%, 100% { transform: translateY(0); } 50% { transform: translateY(-28px); } }

.sec { padding: 80px 0; }
.sec--alt { background: var(--bg); }
.sec__head { text-align: center; max-width: 720px; margin: 0 auto 44px; }
.sec__head .eyebrow { color: var(--teal); }
.sec__head h2 { font-size: clamp(26px, 4vw, 42px); font-weight: 800; margin: 10px 0; color: var(--ink); }
.sec__head p { color: #4a5b60; font-size: 17px; line-height: 1.5; }

/* steps */
.steps { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; }
.step { background: #fff; border: 1px solid #e8eef0; border-radius: 18px; padding: 28px 22px; }
.step__n { width: 42px; height: 42px; border-radius: 12px; display: grid; place-items: center; font-family: 'Jost', sans-serif; font-weight: 800; color: #173a0a; background: var(--lime); margin-bottom: 14px; font-size: 18px; }
.step h3 { font-size: 18px; font-weight: 800; color: var(--ink); }
.step p { color: #4a5b60; font-size: 14px; line-height: 1.5; margin-top: 6px; }

/* 25/25 split */
.split { background: linear-gradient(160deg, #102a32, #0c1c24); border-radius: 26px; padding: 44px; max-width: 920px; margin: 40px auto 0; color: #eaf3f5; }
.split__top { text-align: center; margin-bottom: 30px; }
.split__top .eyebrow { color: var(--gold); }
.split__top h3 { color: #fff; font-size: clamp(22px, 3.2vw, 32px); font-weight: 800; margin-top: 8px; }
.split__top p { color: #c8dde0; max-width: 560px; margin: 10px auto 0; font-size: 15px; line-height: 1.5; }
.split__viz { display: grid; grid-template-columns: 1fr auto 1fr; gap: 18px; align-items: center; }
@media(max-width:680px) { .split__viz { grid-template-columns: 1fr; } }
.half { background: rgba(255,255,255,.06); border: 1px solid rgba(255,255,255,.14); border-radius: 18px; padding: 24px; text-align: center; }
.half__who { font-size: 13px; font-weight: 700; letter-spacing: .04em; text-transform: uppercase; color: #9fd3a6; }
.half__val { font-family: 'Jost', sans-serif; font-weight: 800; font-size: 44px; color: var(--lime); margin: 6px 0 2px; }
.half__cap { font-size: 13px; color: #c8dde0; }
.split__plus { font-family: 'Jost', sans-serif; font-weight: 800; font-size: 34px; color: #fff; text-align: center; }
.split__foot { margin-top: 24px; display: flex; flex-wrap: wrap; gap: 10px; justify-content: center; }
.chip { font-size: 13px; font-weight: 600; color: #dff3e0; background: rgba(152,199,58,.14); border: 1px solid rgba(152,199,58,.3); border-radius: 999px; padding: 8px 14px; }

/* calculadora */
.calc__card { background: #fff; border: 1px solid #e8eef0; border-radius: 24px; box-shadow: 0 20px 50px rgba(15,39,48,.08); padding: 36px; max-width: 860px; margin: 0 auto; display: grid; grid-template-columns: 1fr 1fr; gap: 36px; align-items: center; }
.calc__label { font-size: 14px; color: #6b7c81; font-weight: 600; }
.calc__spend { font-family: 'Jost', sans-serif; font-weight: 800; font-size: 40px; color: var(--ink); margin: 6px 0 18px; }
input[type=range] { width: 100%; height: 6px; border-radius: 999px; -webkit-appearance: none; appearance: none; background: #dfe6e8; outline: none; }
input[type=range]::-webkit-slider-thumb { -webkit-appearance: none; width: 26px; height: 26px; border-radius: 50%; background: #fff; border: 4px solid var(--lime-d); cursor: pointer; box-shadow: 0 4px 12px rgba(0,0,0,.18); }
.calc__hint { font-size: 12px; color: #9aa9ae; margin-top: 10px; }
.calc__result { background: linear-gradient(160deg, #102a32, #1a2332); border-radius: 18px; padding: 28px; color: #eaf3f5; text-align: center; }
.calc__rlabel { font-size: 13px; color: #9fd3a6; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; }
.calc__big { font-family: 'Jost', sans-serif; font-weight: 800; font-size: clamp(38px, 7vw, 56px); color: var(--lime); margin: 8px 0; line-height: 1; }
.calc__sub { font-size: 14px; color: #c8dde0; }
.calc__flip { margin-top: 16px; padding-top: 16px; border-top: 1px solid rgba(255,255,255,.12); font-size: 14px; color: #dff3e0; }
.calc__flip b { color: #fff; }

/* ============ PRÊMIO CARRO ============ */
.cars { position: relative; overflow: hidden;
  background: radial-gradient(760px 520px at 18% -5%, rgba(255,255,255,.22), transparent 55%),
              radial-gradient(700px 600px at 85% 110%, rgba(16,58,67,.28), transparent 60%),
              linear-gradient(160deg, #9ecb46 0%, #83b836 58%, #6fa42b 100%);
  color: #143b15; padding: 80px 0; }
.cars .coin { position: absolute; width: 54px; height: 54px; border-radius: 50%; background: radial-gradient(circle at 35% 30%, #ffd970, #f0a92e); box-shadow: 0 6px 16px rgba(0,0,0,.12), inset 0 -4px 8px rgba(0,0,0,.12); opacity: .9; }
.cars .coin.c1 { top: 60px; left: 6%; }
.cars .coin.c2 { top: 220px; right: 8%; width: 38px; height: 38px; }
.cars .coin.c3 { bottom: 80px; left: 10%; width: 42px; height: 42px; }
.cars .coin.c4 { bottom: 42%; right: 5%; width: 30px; height: 30px; }
.cars__word { font-family: 'Jost', sans-serif; font-weight: 900; text-align: center; line-height: .92; letter-spacing: -.03em; }
.cars__word .q { color: var(--teal-d); font-size: clamp(40px, 9vw, 78px); display: block; }
.cars__word .a { color: #2a6f7d; font-size: clamp(38px, 8.4vw, 72px); display: block; font-style: italic; opacity: .92; }
.cars__tag { text-align: center; color: #1f5a23; font-size: clamp(15px, 2.2vw, 19px); font-weight: 600; margin-top: 10px; }
.cars__bar { display: flex; align-items: center; justify-content: center; gap: 12px; margin: 40px auto 30px; max-width: 520px; }
.cars__num { width: 48px; height: 48px; border-radius: 50%; background: #103a43; color: var(--lime); font-family: 'Jost', sans-serif; font-weight: 800; font-size: 24px; display: grid; place-items: center; flex: 0 0 auto; box-shadow: 0 8px 18px rgba(0,0,0,.18); }
.cars__bartxt { background: rgba(16,58,67,.12); border: 1.5px solid rgba(16,58,67,.5); border-radius: 999px; padding: 11px 30px; font-family: 'Jost', sans-serif; font-weight: 600; font-size: clamp(18px, 3vw, 26px); color: #103a43; }
.cars__bartxt b { font-weight: 800; }
.cars__fine { text-align: center; color: #2a5f33; font-size: 12px; margin-top: 24px; opacity: .85; }

/* showroom */
.showroom { position: relative; max-width: 940px; margin: 8px auto 26px; border-radius: 28px; overflow: hidden; padding: 30px 22px 22px;
  background: radial-gradient(560px 280px at 28% 8%, rgba(58,154,173,.34), transparent 62%),
              radial-gradient(560px 280px at 74% 6%, rgba(152,199,58,.20), transparent 62%),
              linear-gradient(180deg, #103c45 0%, #071e24 100%);
  border: 1px solid rgba(255,255,255,.14); box-shadow: 0 36px 76px rgba(6,26,18,.42), inset 0 1px 0 rgba(255,255,255,.09); }
.showroom__grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; position: relative; z-index: 1; }
@media(max-width:680px) { .showroom__grid { grid-template-columns: 1fr; gap: 26px; } }
.prizecar { position: relative; text-align: center; padding: 6px 6px 0; }
.prizecar + .prizecar::before { content: ""; position: absolute; left: -7px; top: 18%; bottom: 24%; width: 1px; background: linear-gradient(180deg, transparent, rgba(255,255,255,.18), transparent); }
@media(max-width:680px) { .prizecar + .prizecar::before { display: none; } }
.prizecar__spot { position: absolute; left: 50%; top: 34%; width: 86%; height: 60%; transform: translateX(-50%); background: radial-gradient(closest-side, rgba(255,255,255,.20), transparent 72%); filter: blur(10px); z-index: 0; pointer-events: none; }
.prizecar__ribbon { position: relative; z-index: 2; display: inline-flex; align-items: center; gap: 7px; font-family: 'Jost', sans-serif; font-weight: 800; font-size: 13px; letter-spacing: .09em; color: #0f3a43; background: linear-gradient(180deg, #e0f79a, #9fd23f); padding: 7px 18px; border-radius: 999px; box-shadow: 0 8px 20px rgba(152,199,58,.45); }
.prizecar__stagecar { position: relative; z-index: 1; }
.prizecar__img, .prizecar__svg { width: 100%; max-width: 330px; height: auto; display: block; margin: 14px auto 0; }
.prizecar__img { filter: drop-shadow(0 20px 20px rgba(0,0,0,.55)); }
.prizecar__cap { position: relative; z-index: 2; margin-top: 14px; color: #cfe7df; font-size: 14px; line-height: 1.4; }
.prizecar__cap b { display: block; color: #fff; font-family: 'Jost', sans-serif; font-weight: 800; font-size: 18px; letter-spacing: .01em; }
.showroom__cap { position: relative; z-index: 1; text-align: center; margin-top: 18px; padding-top: 16px; border-top: 1px solid rgba(255,255,255,.10); color: #bcd6da; font-size: 13px; font-weight: 600; letter-spacing: .02em; text-transform: uppercase; }
.showroom__brand { font-family: 'Jost', sans-serif; font-weight: 900; font-size: 18px; color: #cdef7a; text-transform: none; letter-spacing: 0; }

/* faq */
.faq { max-width: 760px; margin: 0 auto; }
.faq details { background: #fff; border: 1px solid #e8eef0; border-radius: 14px; padding: 4px 18px; margin-bottom: 12px; }
.faq summary { cursor: pointer; font-weight: 700; color: var(--ink); padding: 14px 0; font-size: 15.5px; list-style: none; }
.faq summary::-webkit-details-marker { display: none; }
.faq summary::after { content: '+'; float: right; color: var(--teal); font-weight: 800; }
.faq details[open] summary::after { content: '\2013'; }
.faq p { color: #4a5b60; font-size: 14.5px; line-height: 1.55; padding: 0 0 16px; }

/* legal */
.legal { background: var(--bg); padding: 40px 0; }
.legal__box { max-width: 860px; margin: 0 auto; text-align: center; color: #6b7c81; font-size: 12.5px; line-height: 1.6; }
.legal__box b { color: #33454a; }

/* cta */
.cta { background: radial-gradient(700px 400px at 80% 0, rgba(58,154,173,.5), transparent 60%), linear-gradient(160deg, #0f2730, #0c1c24); color: #fff; padding: 84px 0; text-align: center; }
.cta h2 { font-size: clamp(28px, 4.5vw, 46px); font-weight: 800; color: #eaf3f5; }
.cta h2 .hl { color: var(--lime); }
.cta p { color: #c8dde0; margin: 16px 0 30px; font-size: 17px; }

.reveal { opacity: 0; transform: translateY(22px); transition: opacity .7s ease, transform .7s ease; }
.reveal.in { opacity: 1; transform: none; }
@media(max-width:760px) { .calc__card, .steps { grid-template-columns: 1fr; } .rung__people { font-size: 14px; } }
</style>
