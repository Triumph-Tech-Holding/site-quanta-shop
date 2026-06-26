<template>
  <main class="sa-page">
    <section class="hero"><img class="hero__wm" alt="" aria-hidden="true" src="/img/logo/quanta-icon-white.png" /><div class="wrap hero__in">
      <span class="eyebrow">Seja um Agente de Fidelização</span>
      <h1>Um dia tem 1.440 minutos.<br>E se cada um virasse <span class="hl">R$ 1</span>?</h1>
      <p class="hero__sub">A Meta Minuto é simples: organize uma rede de consumo e ganhe por minuto. Você não vende nem inventa nada — só organiza o consumo que já existe. Comece de graça.</p>
      <div class="hero__cta">
        <a class="btn btn-lime" href="/register"><i class="ti ti-rocket"></i> Quero começar grátis</a>
        <a class="btn btn-ghost" href="#motor">Calcular minha meta</a>
      </div>
      <p class="hero__mini">Comece no plano Vision (grátis) · Método Meta Minuto · 6 anos de mercado</p>
    </div></section>

    <!-- MOTOR -->
    <section class="motor" id="motor"><div class="wrap reveal">
      <div class="motor__head">
        <span class="eyebrow">Motor Meta Minuto</span>
        <h2>Comece pelo fim: quanto você quer ganhar?</h2>
        <p>Defina a sua meta e veja o caminho — em pessoas na rede ou empresas organizadas.</p>
      </div>
      <div class="motor__card">
        <p class="motor__q">Eu quero ganhar por mês</p>
        <div class="motor__meta" id="meta">R$ 10.000</div>
        <div class="motor__permin" id="permin">≈ R$ 0,23 por minuto</div>
        <div class="motor__slider"><input type="range" id="range" min="1000" max="43200" step="500" value="10000" /></div>
        <div class="motor__paths">
          <div class="path path--ppl"><div class="pl"><i class="ti ti-users"></i> Pela sua rede</div><div class="pn" id="ppl">334</div><div class="pd">pessoas ativas consumindo o habitual (R$ 1/dia)</div></div>
          <div class="path path--biz"><div class="pl"><i class="ti ti-building-store"></i> Organizando empresas</div><div class="pn" id="biz">100</div><div class="pd">empresas gerando R$ 100/mês cada</div></div>
        </div>
        <div class="motor__how">Para chegar lá: cerca de <b id="howind">2</b> indicação(ões) por semana durante 5 semanas — um movimento por semana, sincronizado com a sua rede.</div>
        <div class="motor__feed">E tem mais: essas <b id="feedbiz">100</b> empresas ainda alimentam a sua rede com cerca de <b id="feedppl">3.000</b> pessoas/mês (cada empresa indica ~1 por dia). A rede da empresa vira a <b>sua</b> rede — e é esse volume que aciona os bônus combinados.</div>
        <p class="motor__disc">Simulação ilustrativa de organização de consumo; não é promessa de ganho. Os valores dependem do consumo real da rede e do seu esforço. Percentuais são mínimo garantido e ajustáveis.</p>
      </div>
    </div></section>

    <!-- DUPLICAÇÃO (pessoas) -->
    <section class="dup"><div class="wrap reveal">
      <div class="dup__head">
        <span class="eyebrow">O jeito mais fácil: indicar pessoas</span>
        <h2>Comece com 1 por semana. A rede faz o resto.</h2>
        <p>Indicar gente é simples — todo mundo conhece alguém. Quando cada pessoa faz o mesmo movimento, a sua rede duplica semana após semana. Você decide o ritmo.</p>
      </div>
      <div class="dup__card">
        <div class="dup__ctrl">
          <label>Cada pessoa indica</label>
          <input type="range" id="drange" min="1" max="3" step="1" value="1" />
          <span class="dup__val" id="dper">1</span>
          <label>por semana</label>
        </div>
        <div class="dup__levels" id="dlevels"></div>
        <div class="dup__total"><div class="n" id="dtotal">32</div><div class="t">pessoas no seu movimento em 5 semanas (você + sua rede), consumindo o habitual</div></div>
        <p class="dup__tip"><i class="ti ti-bulb"></i> Com apenas <b>1 por semana</b>, a sua rede já <b>dobra toda semana</b>. Aumente o ritmo para acelerar seus ganhos.</p>
      </div>
    </div></section>

    <!-- FAIXA 1440 -->
    <section class="band1440"><div class="wrap reveal">
      <h2>É por isso que chamamos de <span class="hl">Meta Minuto</span></h2>
      <div class="row">
        <div class="b1"><div class="n" data-count="1440">1.440</div><div class="l">minutos em um dia</div></div>
        <div class="b1"><div class="n" data-count="1440">1.440</div><div class="l">pessoas na sua rede</div></div>
        <div class="b1"><div class="n">R$ 1</div><div class="l">por pessoa, por dia</div></div>
        <div class="b1"><div class="n" data-count="1440" data-prefix="R$ ">R$ 1.440</div><div class="l">por dia pra você</div></div>
      </div>
      <p>Não é sorte nem milagre — é organização. Cada minuto do dia pode ter a sua contraparte em renda.</p>
    </div></section>

    <!-- ORGANIZAR -->
    <section class="sec"><div class="wrap org reveal">
      <div>
        <span class="eyebrow" style="color:var(--teal)">A virada de chave</span>
        <h2>Você <span class="hl">organiza</span> — não inventa nada pra ninguém comprar.</h2>
        <p>O consumo já acontece todo dia: mercado, farmácia, posto, padaria, delivery. A sua função é organizar esse consumo numa rede e direcionar para o ecossistema Quanta. Simples assim.</p>
        <ul class="org__list">
          <li><i class="ti ti-circle-check"></i> Você ganha <b>nas duas pontas</b>: quando alguém compra e quando uma empresa vende.</li>
          <li><i class="ti ti-circle-check"></i> Vários nichos de empresas = <b>múltiplas fontes de renda</b>.</li>
          <li><i class="ti ti-circle-check"></i> Sua renda só cresce: é consumo recorrente, não venda pontual.</li>
        </ul>
      </div>
      <div class="org__box">
        <div class="small">Imagine organizar, numa cidade:</div>
        <div class="calc">100 empresas × R$ 100/mês</div>
        <div class="small">retorno por empresa</div>
        <div style="margin:12px 0;color:#cdd9dc;font-size:24px;"><i class="ti ti-arrow-down"></i></div>
        <div class="eq">R$ 10.000/mês</div>
        <div class="small" style="margin-top:8px;">só por organizar a força de consumo local</div>
      </div>
    </div></section>

    <!-- RENDA CIRCULAR -->
    <section class="sec sec--alt"><div class="wrap reveal">
      <div class="sec__head"><span class="eyebrow">O ciclo que se fecha</span><h2>A Renda Circular</h2><p>No ecossistema Quanta tudo se alimenta: pessoas e empresas se atraem, e a renda circula entre todos.</p></div>
      <div class="circ">
        <div class="circ__ring"><div class="circ__lbl"><i class="ti ti-refresh"></i>Renda<br>Circular</div></div>
        <div class="circ__cards">
          <div class="cc"><i class="ti ti-user"></i><i class="ti ti-arrow-right ar"></i><i class="ti ti-building-store"></i> <span><b>Pessoas</b> trazem <b>empresas</b></span></div>
          <div class="cc"><i class="ti ti-building-store"></i><i class="ti ti-arrow-right ar"></i><i class="ti ti-users"></i> <span><b>Empresas</b> trazem <b>pessoas</b></span></div>
          <div class="cc"><i class="ti ti-building-store"></i><i class="ti ti-arrow-right ar"></i><i class="ti ti-building"></i> <span><b>Empresas</b> trazem <b>empresas</b></span></div>
          <div class="cc"><i class="ti ti-user"></i><i class="ti ti-arrow-right ar"></i><i class="ti ti-users"></i> <span><b>Pessoas</b> trazem <b>pessoas</b></span></div>
        </div>
      </div>
      <p style="text-align:center;color:#4a5b60;max-width:700px;margin:28px auto 0;font-size:16px;">O ciclo se fecha e a renda circula em torno das pessoas e das empresas. Quanto mais o ciclo gira, mais a sua rede — e a sua renda — cresce.</p>
    </div></section>

    <!-- CARTEIRA / SEMENTE -->
    <section class="seed"><div class="wrap reveal">
      <span class="eyebrow">Seu maior patrimônio</span>
      <h2>A sua carteira de clientes é <span class="hl">sua — pra sempre</span></h2>
      <p>A rede da empresa que você credencia vira a sua rede. E mesmo que uma empresa saia do ecossistema, as pessoas continuam na sua carteira. Você planta uma vez e colhe pra sempre.</p>
      <svg viewBox="0 0 460 400" class="tree" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
        <defs>
          <radialGradient id="cgFront" cx="42%" cy="32%" r="72%"><stop offset="0%" stop-color="#cdef7a"/><stop offset="55%" stop-color="#8ec53a"/><stop offset="100%" stop-color="#5f9e1f"/></radialGradient>
          <radialGradient id="cgBack" cx="50%" cy="40%" r="65%"><stop offset="0%" stop-color="#4f8a22"/><stop offset="100%" stop-color="#264f12"/></radialGradient>
          <radialGradient id="coinG" cx="38%" cy="30%" r="75%"><stop offset="0%" stop-color="#ffe79a"/><stop offset="60%" stop-color="#ffb342"/><stop offset="100%" stop-color="#e08e1e"/></radialGradient>
          <linearGradient id="trunkG" x1="0" y1="0" x2="0" y2="1"><stop offset="0%" stop-color="#2F7785"/><stop offset="100%" stop-color="#12330b"/></linearGradient>
          <radialGradient id="haloG" cx="50%" cy="45%" r="50%"><stop offset="0%" stop-color="#98C73A" stop-opacity=".40"/><stop offset="70%" stop-color="#98C73A" stop-opacity=".10"/><stop offset="100%" stop-color="#98C73A" stop-opacity="0"/></radialGradient>
          <filter id="fgP" x="-150%" y="-150%" width="400%" height="400%"><feGaussianBlur stdDeviation="2.2"/></filter>
        </defs>
        <ellipse cx="230" cy="150" rx="185" ry="142" fill="url(#haloG)"><animate attributeName="opacity" values=".7;1;.7" dur="5s" repeatCount="indefinite"/></ellipse>
        <ellipse cx="230" cy="372" rx="150" ry="16" fill="#0a1c24"/>
        <ellipse cx="230" cy="370" rx="120" ry="10" fill="#98C73A" opacity=".12"/>
        <path d="M216 352 C216 286 208 240 208 200 L252 200 C252 240 244 286 244 352 Z" fill="url(#trunkG)"/>
        <path d="M230 300 C214 280 204 268 196 250 M230 280 C246 262 256 250 264 234" stroke="#1c4a14" stroke-width="3" fill="none" stroke-linecap="round" opacity=".6"/>
        <circle r="2.6" fill="#d6f580" filter="url(#fgP)"><animate attributeName="cx" values="190;230" dur="3.8s" begin="0s" repeatCount="indefinite"/><animate attributeName="cy" values="356;150" dur="3.8s" begin="0s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;1;1;0" keyTimes="0;.15;.7;1" dur="3.8s" begin="0s" repeatCount="indefinite"/></circle>
        <circle r="2.6" fill="#d6f580" filter="url(#fgP)"><animate attributeName="cx" values="230;230" dur="3.8s" begin="-1.3s" repeatCount="indefinite"/><animate attributeName="cy" values="356;150" dur="3.8s" begin="-1.3s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;1;1;0" keyTimes="0;.15;.7;1" dur="3.8s" begin="-1.3s" repeatCount="indefinite"/></circle>
        <circle r="2.6" fill="#d6f580" filter="url(#fgP)"><animate attributeName="cx" values="270;230" dur="3.8s" begin="-2.6s" repeatCount="indefinite"/><animate attributeName="cy" values="356;150" dur="3.8s" begin="-2.6s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;1;1;0" keyTimes="0;.15;.7;1" dur="3.8s" begin="-2.6s" repeatCount="indefinite"/></circle>
        <circle r="2.6" fill="#d6f580" filter="url(#fgP)"><animate attributeName="cx" values="212;230" dur="3.8s" begin="-3.4s" repeatCount="indefinite"/><animate attributeName="cy" values="356;150" dur="3.8s" begin="-3.4s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;1;1;0" keyTimes="0;.15;.7;1" dur="3.8s" begin="-3.4s" repeatCount="indefinite"/></circle>
        <g class="tree__canopy">
          <circle cx="234" cy="146" r="99" fill="url(#cgBack)"/><circle cx="154" cy="176" r="62" fill="url(#cgBack)"/><circle cx="314" cy="176" r="62" fill="url(#cgBack)"/><circle cx="190" cy="106" r="56" fill="url(#cgBack)"/><circle cx="282" cy="106" r="56" fill="url(#cgBack)"/><circle cx="234" cy="188" r="62" fill="url(#cgBack)"/>
          <circle cx="230" cy="140" r="92" fill="url(#cgFront)"/><circle cx="150" cy="170" r="58" fill="url(#cgFront)"/><circle cx="310" cy="170" r="58" fill="url(#cgFront)"/><circle cx="186" cy="100" r="52" fill="url(#cgFront)"/><circle cx="278" cy="100" r="52" fill="url(#cgFront)"/><circle cx="230" cy="182" r="58" fill="url(#cgFront)"/>
          <ellipse cx="201" cy="101" rx="39" ry="24" fill="#ffffff" opacity=".16"/><ellipse cx="131" cy="146" rx="24" ry="15" fill="#ffffff" opacity=".16"/><ellipse cx="291" cy="146" rx="24" ry="15" fill="#ffffff" opacity=".16"/><ellipse cx="169" cy="78" rx="22" ry="14" fill="#ffffff" opacity=".16"/><ellipse cx="261" cy="78" rx="22" ry="14" fill="#ffffff" opacity=".16"/>
        </g>
        <g class="tree__fruits" filter="url(#fgP)"><circle cx="188" cy="150" r="9" fill="url(#coinG)"/><circle cx="262" cy="126" r="9" fill="url(#coinG)"/><circle cx="305" cy="166" r="8" fill="url(#coinG)"/><circle cx="156" cy="176" r="8" fill="url(#coinG)"/><circle cx="232" cy="150" r="9" fill="url(#coinG)"/><circle cx="210" cy="104" r="8" fill="url(#coinG)"/><circle cx="290" cy="150" r="8" fill="url(#coinG)"/><circle cx="170" cy="128" r="7" fill="url(#coinG)"/></g>
        <g class="tree__fruits"><circle cx="188" cy="150" r="9" fill="url(#coinG)"/><circle cx="262" cy="126" r="9" fill="url(#coinG)"/><circle cx="305" cy="166" r="8" fill="url(#coinG)"/><circle cx="156" cy="176" r="8" fill="url(#coinG)"/><circle cx="232" cy="150" r="9" fill="url(#coinG)"/><circle cx="210" cy="104" r="8" fill="url(#coinG)"/><circle cx="290" cy="150" r="8" fill="url(#coinG)"/><circle cx="170" cy="128" r="7" fill="url(#coinG)"/></g>
        <g><circle cx="200" cy="178" r="8" fill="url(#coinG)" stroke="#caa23a" stroke-width="1"/><ellipse cx="197.4" cy="175" rx="3" ry="1.8" fill="#fff" opacity=".5"/><animateTransform attributeName="transform" type="translate" values="0,0; 0,150" dur="4.4s" begin="0s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;1;1;0" keyTimes="0;.1;.7;1" dur="4.4s" begin="0s" repeatCount="indefinite"/></g>
        <g><circle cx="256" cy="166" r="8" fill="url(#coinG)" stroke="#caa23a" stroke-width="1"/><ellipse cx="253.4" cy="163" rx="3" ry="1.8" fill="#fff" opacity=".5"/><animateTransform attributeName="transform" type="translate" values="0,0; 0,150" dur="4.4s" begin="-1.5s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;1;1;0" keyTimes="0;.1;.7;1" dur="4.4s" begin="-1.5s" repeatCount="indefinite"/></g>
        <g><circle cx="228" cy="150" r="8" fill="url(#coinG)" stroke="#caa23a" stroke-width="1"/><ellipse cx="225.4" cy="147" rx="3" ry="1.8" fill="#fff" opacity=".5"/><animateTransform attributeName="transform" type="translate" values="0,0; 0,150" dur="4.4s" begin="-3s" repeatCount="indefinite"/><animate attributeName="opacity" values="0;1;1;0" keyTimes="0;.1;.7;1" dur="4.4s" begin="-3s" repeatCount="indefinite"/></g>
        <circle cx="120" cy="110" r="2" fill="#cdef7a" filter="url(#fgP)"><animate attributeName="opacity" values=".1;.85;.1" dur="3.4s" begin="-0s" repeatCount="indefinite"/><animateTransform attributeName="transform" type="translate" values="0,0; 3,-6; 0,0" dur="5.4s" begin="-0s" repeatCount="indefinite"/></circle>
        <circle cx="345" cy="120" r="2.4" fill="#cdef7a" filter="url(#fgP)"><animate attributeName="opacity" values=".1;.85;.1" dur="4.0s" begin="-1s" repeatCount="indefinite"/><animateTransform attributeName="transform" type="translate" values="0,0; 3,-6; 0,0" dur="6.0s" begin="-1s" repeatCount="indefinite"/></circle>
        <circle cx="330" cy="60" r="1.8" fill="#cdef7a" filter="url(#fgP)"><animate attributeName="opacity" values=".1;.85;.1" dur="3.7s" begin="-2s" repeatCount="indefinite"/><animateTransform attributeName="transform" type="translate" values="0,0; 3,-6; 0,0" dur="5.7s" begin="-2s" repeatCount="indefinite"/></circle>
        <circle cx="110" cy="180" r="2" fill="#cdef7a" filter="url(#fgP)"><animate attributeName="opacity" values=".1;.85;.1" dur="4.3s" begin="-3s" repeatCount="indefinite"/><animateTransform attributeName="transform" type="translate" values="0,0; 3,-6; 0,0" dur="6.3s" begin="-3s" repeatCount="indefinite"/></circle>
        <circle cx="250" cy="52" r="2.2" fill="#cdef7a" filter="url(#fgP)"><animate attributeName="opacity" values=".1;.85;.1" dur="3.9s" begin="-1.5s" repeatCount="indefinite"/><animateTransform attributeName="transform" type="translate" values="0,0; 3,-6; 0,0" dur="5.9s" begin="-1.5s" repeatCount="indefinite"/></circle>
        <circle cx="360" cy="200" r="1.8" fill="#cdef7a" filter="url(#fgP)"><animate attributeName="opacity" values=".1;.85;.1" dur="4.5s" begin="-2.5s" repeatCount="indefinite"/><animateTransform attributeName="transform" type="translate" values="0,0; 3,-6; 0,0" dur="6.5s" begin="-2.5s" repeatCount="indefinite"/></circle>
        <circle cx="150" cy="362" r="6" fill="#9fd3a6"><animate attributeName="opacity" values=".45;1;.45" dur="2.6s" begin="-0.0s" repeatCount="indefinite"/></circle><circle cx="174" cy="362" r="6" fill="#9fd3a6"><animate attributeName="opacity" values=".45;1;.45" dur="2.6s" begin="-0.3s" repeatCount="indefinite"/></circle><circle cx="198" cy="362" r="6" fill="#9fd3a6"><animate attributeName="opacity" values=".45;1;.45" dur="2.6s" begin="-0.6s" repeatCount="indefinite"/></circle><circle cx="222" cy="362" r="6" fill="#9fd3a6"><animate attributeName="opacity" values=".45;1;.45" dur="2.6s" begin="-0.9s" repeatCount="indefinite"/></circle><circle cx="246" cy="362" r="6" fill="#9fd3a6"><animate attributeName="opacity" values=".45;1;.45" dur="2.6s" begin="-1.2s" repeatCount="indefinite"/></circle><circle cx="270" cy="362" r="6" fill="#9fd3a6"><animate attributeName="opacity" values=".45;1;.45" dur="2.6s" begin="-1.5s" repeatCount="indefinite"/></circle><circle cx="294" cy="362" r="6" fill="#9fd3a6"><animate attributeName="opacity" values=".45;1;.45" dur="2.6s" begin="-1.8s" repeatCount="indefinite"/></circle><circle cx="318" cy="362" r="6" fill="#9fd3a6"><animate attributeName="opacity" values=".45;1;.45" dur="2.6s" begin="-2.1s" repeatCount="indefinite"/></circle>
      </svg>
      <div class="treecap"><div><b>60 clientes</b>a base que você planta</div><div><b>milhares de frutos</b>renda que se multiplica</div></div>
      <p class="seed__sub">Cada pessoa e cada empresa que você organiza viram novas sementes na sua rede. É renda que só cresce.</p>
    </div></section>

    <!-- COMECE -->
    <section class="sec"><div class="wrap reveal">
      <div class="sec__head"><span class="eyebrow">Comece hoje</span><h2>Em 3 passos, do zero</h2></div>
      <div class="steps">
        <div class="step"><div class="step__n"><i class="ti ti-flag"></i></div><h3>Comece grátis</h3><p>Ative seu canal no plano Vision, sem estoque e sem capital inicial.</p></div>
        <div class="step"><div class="step__n"><i class="ti ti-share"></i></div><h3>Organize o consumo</h3><p>Indique pessoas e empresas e direcione o consumo que já existe para a sua rede.</p></div>
        <div class="step"><div class="step__n"><i class="ti ti-cash"></i></div><h3>Ganhe todo mês</h3><p>Você recebe sobre o consumo da rede em 5 níveis — uma renda que se repete.</p></div>
      </div>
    </div></section>

    <!-- TUDO PELO CELULAR -->
    <section class="sec"><div class="wrap cel reveal">
      <div>
        <span class="eyebrow" style="color:var(--teal)">Tudo na palma da mão</span>
        <h2>Prospecte e feche adesões <span class="hl">pelo celular</span></h2>
        <p>Do primeiro "oi" à adesão confirmada, tudo acontece no seu celular — sem papelada, sem reunião. E cada assinatura Plus que você vende (PF ou PJ) te rende R$ 100 na hora.</p>
        <ul class="cel__list">
          <li><i class="ti ti-brand-whatsapp"></i> Prospecção e fechamento pelo WhatsApp</li>
          <li><i class="ti ti-device-mobile-check"></i> Adesão 100% digital, em minutos</li>
          <li><i class="ti ti-cash-banknote"></i> R$ 100 por assinatura Plus vendida (PF ou PJ) — renda rápida</li>
          <li><i class="ti ti-robot"></i> Quanta Flow: automação e IA trabalhando 24/7</li>
        </ul>
        <a class="btn btn-lime" href="/quanta-flow" style="margin-top:18px;"><i class="ti ti-robot"></i> Conheça o Quanta Flow</a>
      </div>
      <div class="phone">
        <div class="phone__scr">
          <div class="phone__top"><span class="phone__av">🥖</span><div><div class="phone__nm">Padaria do João</div><div class="phone__st">online</div></div></div>
          <div class="phone__chat" id="flowchat"></div>
        </div>
      </div>
    </div></section>

    <!-- FORMAS DE GANHO -->
    <section class="sec"><div class="wrap reveal">
      <div class="sec__head"><span class="eyebrow">Várias fontes, um só app</span><h2>De quantos jeitos você ganha</h2><p>Renda rápida, renda recorrente e bônus combinados — a base da sua Meta Minuto.</p></div>
      <div class="earn">
        <div class="ecard"><div class="ic"><i class="ti ti-coin"></i></div><h3>Cashback próprio</h3><p>Dinheiro de volta nas suas compras, sacável via PIX.</p></div>
        <div class="ecard"><div class="ic"><i class="ti ti-affiliate"></i></div><h3>Residual de cashback</h3><p>Ganhe sobre o consumo da sua rede, em 5 níveis.</p></div>
        <div class="ecard"><div class="ic"><i class="ti ti-bolt"></i></div><h3>Residual dobrado</h3><p>Assinante Plus recebe o dobro no residual.</p></div>
        <div class="ecard"><div class="ic"><i class="ti ti-heart-handshake"></i></div><h3>Quanta Amizade</h3><p>R$ 25 pra você e R$ 25 pro amigo a cada indicação qualificada.</p></div>
        <div class="ecard"><div class="ic"><i class="ti ti-building-store"></i></div><h3>Credenciamento + Stone</h3><p>Traga lojas e maquininhas e ganhe sobre o que elas geram (Shu Ra Ri).</p></div>
        <div class="ecard"><div class="ic"><i class="ti ti-school"></i></div><h3>Habilitação</h3><p>Comissão por indicar habilitações e licenças Plus na rede.</p></div>
        <div class="ecard"><div class="ic"><i class="ti ti-cash-banknote"></i></div><h3>Comissão de assinatura · <span style="color:var(--lime-d)">R$ 100</span></h3><p>Renda rápida: R$ 100 por cada assinatura Plus vendida — PF ou PJ.</p></div>
        <div class="ecard"><div class="ic"><i class="ti ti-calendar-repeat"></i></div><h3>Renda recorrente</h3><p>Toda assinatura ativa na sua rede gera cashback <b>todo mês</b> — renda previsível que se repete sozinha.</p></div>
      </div>
    </div></section>

    <!-- ZEE DIGITAL -->
    <section class="zee"><div class="wrap reveal">
      <div class="sec__head">
        <span class="eyebrow">ZEE Digital · Zona Econômica Especial</span>
        <h2>Cada agente constrói a sua <span class="hl">Zona Econômica Especial Digital</span></h2>
        <p>Com a agência digital, você começa com uma loja, depois outra, e outra. A cada 100 lojas, um ecossistema de renda. Multiplique isso e você terá sua própria zona econômica especial digital — funcionando 24h, na sua mão. <b>Lucro nas compras, lucro nas vendas.</b></p>
      </div>
      <div class="zee__stage">
        <svg viewBox="0 0 780 480" class="zee__svg" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
          <defs>
            <linearGradient id="ringZ" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#aee05a"/><stop offset="100%" stop-color="#3A9AAD"/></linearGradient>
            <radialGradient id="nodeZ" cx="40%" cy="30%" r="85%"><stop offset="0" stop-color="#143b46"/><stop offset="100%" stop-color="#0a1b22"/></radialGradient>
            <radialGradient id="coinZ" cx="38%" cy="30%" r="80%"><stop offset="0" stop-color="#e2f79a"/><stop offset="100%" stop-color="#7aad1f"/></radialGradient>
            <filter id="glowZ" x="-70%" y="-70%" width="240%" height="240%"><feGaussianBlur stdDeviation="3" result="b"/><feMerge><feMergeNode in="b"/><feMergeNode in="SourceGraphic"/></feMerge></filter>
            <filter id="glowP" x="-120%" y="-120%" width="340%" height="340%"><feGaussianBlur stdDeviation="2.4"/></filter>
          </defs>
          <rect x="16" y="16" width="748" height="448" rx="28" fill="none" stroke="rgba(152,199,58,.35)" stroke-width="1.6" stroke-dasharray="3 9"><animate attributeName="stroke-dashoffset" from="0" to="-48" dur="3.2s" repeatCount="indefinite"/></rect>
          <g transform="translate(34,32)"><rect x="0" y="-13" width="124" height="24" rx="12" fill="rgba(152,199,58,.14)" stroke="rgba(152,199,58,.4)"/><circle cx="15" cy="-1" r="3.2" fill="#98C73A"/><text x="27" y="3" fill="#cdef7a" font-family="Jost,sans-serif" font-weight="700" font-size="11.5" letter-spacing="1.4">ZEE DIGITAL</text></g>
          <circle cx="70" cy="210" r="1.3" fill="#bfe8ef"><animate attributeName="opacity" values=".12;.6;.12" dur="3.6s" begin="-0s" repeatCount="indefinite"/></circle><circle cx="710" cy="260" r="1.5" fill="#bfe8ef"><animate attributeName="opacity" values=".12;.6;.12" dur="4.2s" begin="-1s" repeatCount="indefinite"/></circle><circle cx="400" cy="60" r="1.2" fill="#bfe8ef"><animate attributeName="opacity" values=".12;.6;.12" dur="3.9s" begin="-2s" repeatCount="indefinite"/></circle><circle cx="380" cy="430" r="1.4" fill="#bfe8ef"><animate attributeName="opacity" values=".12;.6;.12" dur="4.6s" begin="-3s" repeatCount="indefinite"/></circle>
          <path id="con0" d="M150,120 L390,240" fill="none" stroke="url(#ringZ)" stroke-width="1.6" opacity=".4"/><path id="con1" d="M630,120 L390,240" fill="none" stroke="url(#ringZ)" stroke-width="1.6" opacity=".4"/><path id="con2" d="M150,360 L390,240" fill="none" stroke="url(#ringZ)" stroke-width="1.6" opacity=".4"/><path id="con3" d="M630,360 L390,240" fill="none" stroke="url(#ringZ)" stroke-width="1.6" opacity=".4"/>
          <line x1="134" y1="122" x2="120" y2="124" stroke="rgba(152,199,58,.40)" stroke-width="1"/><circle cx="120" cy="124" r="3.1" fill="#9fe0ea"/><line x1="136" y1="113" x2="123" y2="107" stroke="rgba(152,199,58,.40)" stroke-width="1"/><circle cx="123" cy="107" r="3.1" fill="#9fe0ea"/><line x1="142" y1="106" x2="135" y2="94" stroke="rgba(152,199,58,.40)" stroke-width="1"/><circle cx="135" cy="94" r="3.1" fill="#9fe0ea"/>
          <g filter="url(#glowZ)"><circle cx="150" cy="120" r="30" fill="url(#nodeZ)" stroke="url(#ringZ)" stroke-width="2"/></g>
          <g transform="translate(150,120)" stroke="#d4ef83" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round"><path d="M-12 -7 h4 l3 15 h12 l3 -10 h-15"/><circle cx="-3" cy="11" r="1.8" fill="#d4ef83" stroke="none"/><circle cx="7" cy="11" r="1.8" fill="#d4ef83" stroke="none"/></g>
          <text x="150" y="74" text-anchor="middle" fill="#cfe2e5" font-family="Inter,sans-serif" font-size="12.5" font-weight="600">Mercado</text>
          <line x1="638" y1="106" x2="645" y2="94" stroke="rgba(152,199,58,.40)" stroke-width="1"/><circle cx="645" cy="94" r="3.1" fill="#9fe0ea"/><line x1="644" y1="113" x2="657" y2="107" stroke="rgba(152,199,58,.40)" stroke-width="1"/><circle cx="657" cy="107" r="3.1" fill="#9fe0ea"/><line x1="646" y1="122" x2="660" y2="124" stroke="rgba(152,199,58,.40)" stroke-width="1"/><circle cx="660" cy="124" r="3.1" fill="#9fe0ea"/>
          <g filter="url(#glowZ)"><circle cx="630" cy="120" r="30" fill="url(#nodeZ)" stroke="url(#ringZ)" stroke-width="2"/></g>
          <g transform="translate(630,120)" stroke="#d4ef83" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round"><path d="M-3.5 -10 h7 v6.5 h6.5 v7 h-6.5 v6.5 h-7 v-6.5 h-6.5 v-7 h6.5 z"/></g>
          <text x="630" y="74" text-anchor="middle" fill="#cfe2e5" font-family="Inter,sans-serif" font-size="12.5" font-weight="600">Farmácia</text>
          <g filter="url(#glowZ)"><circle cx="150" cy="360" r="30" fill="url(#nodeZ)" stroke="url(#ringZ)" stroke-width="2"/></g>
          <g transform="translate(150,360)" stroke="#d4ef83" stroke-width="1.8" fill="none" stroke-linecap="round"><path d="M-10 -3 h20 M-10 3 h20 M-7 -9 v18"/></g>
          <text x="150" y="406" text-anchor="middle" fill="#cfe2e5" font-family="Inter,sans-serif" font-size="12.5" font-weight="600">Padaria</text>
          <g filter="url(#glowZ)"><circle cx="630" cy="360" r="30" fill="url(#nodeZ)" stroke="url(#ringZ)" stroke-width="2"/></g>
          <g transform="translate(630,360)" stroke="#d4ef83" stroke-width="2" fill="none" stroke-linecap="round"><circle cx="0" cy="-2" r="7" stroke="#d4ef83"/><path d="M-10 12 c0 -10 20 -10 20 0"/></g>
          <text x="630" y="406" text-anchor="middle" fill="#cfe2e5" font-family="Inter,sans-serif" font-size="12.5" font-weight="600">Posto</text>
          <circle r="2.7" fill="#d6f580" filter="url(#glowP)"><animateMotion dur="4.4s" begin="0s" repeatCount="indefinite"><mpath href="#con0"/></animateMotion></circle><circle r="2.7" fill="#d6f580" filter="url(#glowP)"><animateMotion dur="4.4s" begin="-2.2s" repeatCount="indefinite"><mpath href="#con0"/></animateMotion></circle>
          <circle r="2.7" fill="#d6f580" filter="url(#glowP)"><animateMotion dur="4.4s" begin="0s" repeatCount="indefinite"><mpath href="#con1"/></animateMotion></circle><circle r="2.7" fill="#d6f580" filter="url(#glowP)"><animateMotion dur="4.4s" begin="-2.2s" repeatCount="indefinite"><mpath href="#con1"/></animateMotion></circle>
          <circle r="2.7" fill="#d6f580" filter="url(#glowP)"><animateMotion dur="4.4s" begin="0s" repeatCount="indefinite"><mpath href="#con2"/></animateMotion></circle><circle r="2.7" fill="#d6f580" filter="url(#glowP)"><animateMotion dur="4.4s" begin="-2.2s" repeatCount="indefinite"><mpath href="#con2"/></animateMotion></circle>
          <circle r="2.7" fill="#d6f580" filter="url(#glowP)"><animateMotion dur="4.4s" begin="0s" repeatCount="indefinite"><mpath href="#con3"/></animateMotion></circle><circle r="2.7" fill="#d6f580" filter="url(#glowP)"><animateMotion dur="4.4s" begin="-2.2s" repeatCount="indefinite"><mpath href="#con3"/></animateMotion></circle>
          <g filter="url(#glowZ)"><rect x="356" y="150" width="68" height="180" rx="15" fill="#0a1c23" stroke="url(#ringZ)" stroke-width="2"/></g>
          <rect x="361" y="158" width="58" height="164" rx="11" fill="#0e2932"/>
          <rect x="383" y="163" width="14" height="3.5" rx="1.75" fill="#0a1c23"/>
          <rect x="367" y="176" width="46" height="20" rx="6" fill="rgba(152,199,58,.14)"/>
          <circle cx="376" cy="186" r="5" fill="url(#coinZ)"/>
          <text x="386" y="189" fill="#dcecef" font-family="Inter,sans-serif" font-size="7.5">Agência</text>
          <text x="390" y="238" text-anchor="middle" fill="#aee05a" font-family="Jost,sans-serif" font-weight="800" font-size="21">R$</text>
          <text x="390" y="254" text-anchor="middle" fill="#cfe2e5" font-family="Inter,sans-serif" font-size="7.5">renda da rede</text>
          <g fill="#98C73A"><rect x="372" y="290" width="7" height="14" rx="2"/><rect x="384" y="282" width="7" height="22" rx="2"/><rect x="396" y="272" width="7" height="32" rx="2"/></g>
          <path d="M375 288 L400 268" stroke="#aee05a" stroke-width="2" fill="none" stroke-linecap="round"/>
          <path d="M400 268 l-6 -1 M400 268 l1 6" stroke="#aee05a" stroke-width="2" fill="none" stroke-linecap="round"/>
          <text x="390" y="354" text-anchor="middle" fill="#9fd3a6" font-family="Inter,sans-serif" font-size="12.5" font-weight="600">Sua agência no bolso</text>
        </svg>
      </div>
      <div class="zee__grid">
        <div class="zee__card"><div class="zee__ic"><i class="ti ti-building-store"></i></div><h3>Cada loja, uma fonte de renda</h3><p>Toda loja que você credencia entra na sua ZEE e passa a gerar receita recorrente sobre o consumo.</p></div>
        <div class="zee__card"><div class="zee__ic"><i class="ti ti-arrows-exchange-2"></i></div><h3>Ganha na compra e na venda</h3><p>Receita quando um cliente consome na rede e também quando uma nova loja ou assinatura é vendida.</p></div>
        <div class="zee__card"><div class="zee__ic"><i class="ti ti-device-mobile"></i></div><h3>Vários ecossistemas, um app</h3><p>Múltiplas fontes de renda criadas e gerenciadas pelo celular — a sua agência digital trabalha por você.</p></div>
      </div>
    </div></section>

    <section class="sec"><div class="wrap central reveal">
      <div>
        <span class="eyebrow" style="color:var(--teal)">Sua central de comando</span>
        <h2>A <span class="hl">agência digital</span> faz o trabalho pesado</h2>
        <p>A sua área de membros é uma central de comando: tudo o que você precisa pra organizar a rede e bater a meta por minuto, num lugar só.</p>
        <p style="color:#6b7c81;font-size:14px;">Faltou gente? Aumente empresas ou consumo. Você comanda as alavancas — a agência mostra o caminho e a sua renda só cresce.</p>
      </div>
      <div class="central__panel">
        <div class="ttl">Painel da Agência Digital</div>
        <div class="tool"><i class="ti ti-gauge"></i> Meta Minuto e progresso do dia <span class="d">ao vivo</span></div>
        <div class="tool"><i class="ti ti-sitemap"></i> Minha rede em 5 níveis <span class="d">mapa</span></div>
        <div class="tool"><i class="ti ti-link"></i> Links de indicação e convites <span class="d">1 clique</span></div>
        <div class="tool"><i class="ti ti-building-store"></i> Empresas credenciadas e Stone <span class="d">gestão</span></div>
        <div class="tool"><i class="ti ti-adjustments"></i> Alavancas: pessoas, empresas, consumo <span class="d">ajuste</span></div>
        <div class="tool"><i class="ti ti-cash"></i> Extrato, saldo e resgate via PIX <span class="d">na hora</span></div>
        <div class="tool"><i class="ti ti-robot"></i> Quanta Flow — automação e IA <span class="d">24/7</span></div>
        <div class="tool"><i class="ti ti-map-2"></i> Jornada Penta Meta Minuto <span class="d">25 semanas</span></div>
      </div>
    </div></section>

    <!-- PLANOS -->
    <section class="sec sec--alt"><div class="wrap reveal">
      <div class="sec__head"><span class="eyebrow">Evolua quando quiser</span><h2>Três planos, um objetivo</h2></div>
      <div class="plans">
        <div class="plan"><h3>Vision</h3><div class="price">Grátis</div><ul><li><i class="ti ti-check"></i> Cashback próprio nas suas compras</li><li><i class="ti ti-check"></i> Bônus Residual de Cashback (5 níveis)</li><li><i class="ti ti-check"></i> Bônus Quanta Amizade (R$ 50/indicação)</li></ul><a class="btn btn-lime" href="/register" style="margin-top:auto;justify-content:center;">Começar grátis</a></div>
        <div class="plan plan--feat"><h3>Vision Plus</h3><div class="price">12× R$ 97 ou R$ 997/ano</div><ul><li><i class="ti ti-check"></i> Tudo do Vision</li><li><i class="ti ti-check"></i> Bônus Residual de Cashback <b>DOBRADO</b></li><li><i class="ti ti-check"></i> Bônus de Habilitação</li><li><i class="ti ti-check"></i> Bônus Residual de Habilitação</li><li><i class="ti ti-check"></i> Comissão de Assinatura (R$ 100)</li><li><i class="ti ti-check"></i> Cupons e ofertas exclusivos</li><li><i class="ti ti-check"></i> Agência digital premium</li><li><i class="ti ti-check"></i> R$ 1.000 de crédito para upgrade</li></ul><a class="btn btn-lime" href="/planos" style="margin-top:auto;justify-content:center;">Assinar Plus</a></div>
        <div class="plan"><h3>Shu Ra Ri</h3><div class="price">R$ 2.497 ou 12× R$ 249</div><div style="font-family:'Jost',sans-serif;font-weight:800;font-size:13px;color:var(--lime-d);letter-spacing:.04em;text-transform:uppercase;margin:-4px 0 12px;">As 9 formas de ganho</div><ul><li><i class="ti ti-check"></i> Cashback próprio nas suas compras</li><li><i class="ti ti-check"></i> Bônus Residual de Cashback (5 níveis)</li><li><i class="ti ti-check"></i> Bônus de Habilitação</li><li><i class="ti ti-check"></i> Bônus Residual de Habilitação</li><li><i class="ti ti-check"></i> Bônus de Credenciamento</li><li><i class="ti ti-check"></i> Bônus Residual de Credenciamento</li><li><i class="ti ti-check"></i> Bônus Stone (cashback da maquininha)</li><li><i class="ti ti-check"></i> Bônus Quanta Amizade (R$ 50/indicação)</li><li><i class="ti ti-check"></i> Comissão de Assinatura (R$ 100)</li></ul><p style="font-size:12.5px;color:#6b7c81;margin:2px 0 14px;line-height:1.45;">+ Some o <b>Plus</b> e dobre o residual — a 10ª forma de ganho.</p><a class="btn btn-lime" href="/planos" style="margin-top:auto;justify-content:center;">Quero o Shu Ra Ri</a></div>
      </div>
    </div></section>

    <!-- SEM BARREIRAS -->
    <section class="sec"><div class="wrap reveal">
      <div class="sec__head"><span class="eyebrow">Por que agora</span><h2>Empreender sem as barreiras de sempre</h2></div>
      <div class="bar">
        <div class="bcard"><div class="ic"><i class="ti ti-package-off"></i></div><h3>Sem estoque</h3><p>Você não compra nem revende nada.</p></div>
        <div class="bcard"><div class="ic"><i class="ti ti-coin-off"></i></div><h3>Sem capital inicial</h3><p>Comece de graça no Vision.</p></div>
        <div class="bcard"><div class="ic"><i class="ti ti-user-check"></i></div><h3>Você é o dono</h3><p>Atua como PJ, sem vínculo trabalhista.</p></div>
        <div class="bcard"><div class="ic"><i class="ti ti-target-arrow"></i></div><h3>Com treinamento</h3><p>Agência digital + Meta Minuto pra te guiar.</p></div>
      </div>
    </div></section>

    <!-- CTA -->
    <section class="cta"><div class="wrap reveal">
      <h2>Comece a sua <span class="hl">Meta Minuto</span> hoje.</h2>
      <p>Ative seu canal grátis no Vision e dê o primeiro movimento da sua rede do bem.</p>
      <a class="btn btn-lime" href="/register"><i class="ti ti-rocket"></i> Quero ser um Agente</a>
    </div></section>
  </main>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' })
useSeoMeta({
  title: 'Seja um Agente de Fidelização — Quanta Shop',
  description: 'Empreenda sem estoque e sem capital inicial. Construa sua rede proprietária, crie sua Zona Econômica Especial Digital e ganhe renda recorrente com o Método Meta Minuto.',
})
useHead({
  link: [
    { rel: 'stylesheet', href: 'https://cdn.jsdelivr.net/npm/@tabler/icons-webfont@3.11.0/dist/tabler-icons.min.css' },
  ],
})

onMounted(() => {
  const fmt = (n: number) => 'R$ ' + Math.round(n).toLocaleString('pt-BR')

  /* Motor */
  const range = document.getElementById('range') as HTMLInputElement | null
  if (range) {
    function motorUpd() {
      const v = +range!.value, pct = (v - 1000) / (43200 - 1000) * 100
      range!.style.background = `linear-gradient(90deg,var(--lime) 0%,var(--lime) ${pct}%,#dfe6e8 ${pct}%)`
      const metaEl = document.getElementById('meta')
      const perminEl = document.getElementById('permin')
      const bizEl = document.getElementById('biz')
      const pplEl = document.getElementById('ppl')
      const feedbizEl = document.getElementById('feedbiz')
      const feedpplEl = document.getElementById('feedppl')
      const howindEl = document.getElementById('howind')
      if (metaEl) metaEl.textContent = fmt(v)
      const permin = (v / 30) / 1440
      if (perminEl) perminEl.textContent = '≈ ' + (permin >= 1 ? fmt(permin) : ('R$ ' + permin.toFixed(2).replace('.', ','))) + ' por minuto'
      const b = Math.ceil(v / 100)
      if (bizEl) bizEl.textContent = b.toLocaleString('pt-BR')
      const pplN = Math.ceil(v / 30)
      if (pplEl) pplEl.textContent = pplN.toLocaleString('pt-BR')
      if (feedbizEl) feedbizEl.textContent = b.toLocaleString('pt-BR')
      if (feedpplEl) feedpplEl.textContent = (b * 30).toLocaleString('pt-BR')
      if (howindEl) howindEl.textContent = String(Math.max(1, Math.round(Math.pow(pplN, 0.2) - 1)))
    }
    range.addEventListener('input', motorUpd)
    motorUpd()
  }

  /* Duplicação */
  const drange = document.getElementById('drange') as HTMLInputElement | null
  function animateNum(el: HTMLElement, to: number) {
    const from = parseInt((el.textContent || '0').replace(/\D/g, '')) || 0
    const dur = 500, t0 = performance.now()
    function step(t: number) {
      const p = Math.min(1, (t - t0) / dur)
      const val = Math.round(from + (to - from) * (1 - Math.pow(1 - p, 3)))
      el.textContent = val.toLocaleString('pt-BR')
      if (p < 1) requestAnimationFrame(step)
    }
    requestAnimationFrame(step)
  }
  function dupUpd(animate: boolean) {
    if (!drange) return
    const r = +drange.value, pct = (r - 1) / (3 - 1) * 100
    drange.style.background = `linear-gradient(90deg,var(--lime) 0%,var(--lime) ${pct}%,rgba(255,255,255,.15) ${pct}%)`
    const dperEl = document.getElementById('dper')
    if (dperEl) dperEl.textContent = String(r)
    let cur = 1
    const lv: number[] = []
    for (let w = 0; w < 5; w++) { cur = cur * (1 + r); lv.push(Math.round(cur)) }
    const dlevelsEl = document.getElementById('dlevels')
    if (dlevelsEl) dlevelsEl.innerHTML = lv.map((n, i) => `<div class="dlvl"><b>${n.toLocaleString('pt-BR')}</b><span>Semana ${i + 1}</span></div>`).join('')
    const total = lv[4]
    const tot = document.getElementById('dtotal')
    if (tot) {
      if (animate) { animateNum(tot, total) } else { tot.textContent = total.toLocaleString('pt-BR') }
    }
  }
  if (drange) { drange.addEventListener('input', () => dupUpd(true)); dupUpd(false) }

  /* Chat do celular */
  const FLOWSEQ: Array<{ who?: string; html?: string; type?: number }> = [
    { who: 'out', html: 'Oi João! Sua padaria é referência no bairro 👏 Posso te mostrar como atrair mais clientes pagando só por resultado?' },
    { type: 1100 },
    { who: 'in', html: 'Pode sim, como funciona?' },
    { who: 'out', html: 'Seus clientes ganham cashback e voltam mais. Sem mensalidade fixa — você só paga sobre o que vende.' },
    { who: 'out', html: 'E você ainda recebe R$ 450 de crédito pra dar os primeiros cashbacks (vale 90 dias) 🚀' },
    { type: 1000 },
    { who: 'in', html: 'Gostei! Como faço pra entrar?' },
    { who: 'out', html: 'Te mando o link da adesão Plus aqui mesmo 📲' },
    { who: 'sys', html: '✅ Adesão Plus confirmada · R$ 100 de comissão na sua conta' },
  ]
  let flowStarted = false
  function playFlow() {
    if (flowStarted) return; flowStarted = true
    const box = document.getElementById('flowchat'); if (!box) return; let i = 0
    const scroll = () => { box.scrollTop = box.scrollHeight }
    function next() {
      if (i >= FLOWSEQ.length) { setTimeout(() => { box.innerHTML = ''; i = 0; next() }, 4800); return }
      const it = FLOWSEQ[i++]
      if (it.type) {
        const t = document.createElement('div'); t.className = 'typing'
        t.innerHTML = '<span></span><span></span><span></span>'
        box.appendChild(t); scroll()
        setTimeout(() => { t.remove(); next() }, it.type); return
      }
      const d = document.createElement('div'); d.className = 'bub bub--' + it.who; d.innerHTML = it.html || ''
      box.appendChild(d); scroll()
      setTimeout(next, it.who === 'out' ? 950 : 700)
    }
    next()
  }

  /* Reveal + count-up on scroll */
  const io = new IntersectionObserver((es) => {
    es.forEach(e => {
      if (e.isIntersecting) {
        e.target.classList.add('in')
        if (e.target.classList.contains('cel')) playFlow()
        e.target.querySelectorAll('[data-count]').forEach((el) => {
          const to = +(el.getAttribute('data-count') || '0')
          const pre = el.getAttribute('data-prefix') || ''
          const t0 = performance.now()
          function st(t: number) {
            const p = Math.min(1, (t - t0) / 900)
            el.textContent = pre + Math.round(to * (1 - Math.pow(1 - p, 3))).toLocaleString('pt-BR')
            if (p < 1) requestAnimationFrame(st)
          }
          requestAnimationFrame(st)
        })
        io.unobserve(e.target)
      }
    })
  }, { threshold: .18 })
  document.querySelectorAll('.reveal').forEach(el => io.observe(el))
})
</script>

<style scoped>
.sa-page { --teal: #2F7785; --teal-d: #225F6B; --lime: #98C73A; --lime-d: #7aad1f; --ink: #225F6B; --bg: #f4f6f7; --gold: #FFB342; }
* { box-sizing: border-box; margin: 0; padding: 0; }
.sa-page { font-family: 'Inter', 'Jost', sans-serif; color: var(--ink); background: #fff; -webkit-font-smoothing: antialiased; }
.wrap { max-width: 1120px; margin: 0 auto; padding: 0 24px; }
h1, h2, h3 { font-family: 'Jost', 'Inter', sans-serif; letter-spacing: -.02em; line-height: 1.05; }
.eyebrow { font-size: 13px; font-weight: 700; letter-spacing: .08em; text-transform: uppercase; }
.ti { line-height: 1; }
.btn { display: inline-flex; align-items: center; gap: 8px; font-weight: 700; font-size: 15px; border-radius: 999px; padding: 14px 26px; text-decoration: none; cursor: pointer; border: 0; transition: .2s; }
.btn-lime { background: linear-gradient(180deg, var(--lime), var(--lime-d)); color: #173a0a; box-shadow: 0 12px 30px rgba(152, 199, 58, .35); }
.btn-lime:hover { transform: translateY(-2px); box-shadow: 0 18px 40px rgba(152, 199, 58, .45); }
.btn-ghost { background: transparent; border: 1.5px solid rgba(255, 255, 255, .35); color: #fff; }
.btn-ghost:hover { background: rgba(255, 255, 255, .1); }

.reveal { opacity: 0; transform: translateY(18px); transition: opacity .6s ease, transform .6s ease; }
.reveal.in { opacity: 1; transform: none; }

.hero { position: relative; overflow: hidden; color: #eaf3f5; background: radial-gradient(900px 520px at 82% -10%, rgba(58, 154, 173, .5), transparent 60%), radial-gradient(700px 500px at 5% 110%, rgba(152, 199, 58, .18), transparent 55%), linear-gradient(160deg, #0f2730 0%, #1a2332 55%, #0c1c24 100%); }
.hero__wm { position: absolute; right: -90px; top: 50%; transform: translateY(-50%); width: 520px; max-width: 56%; opacity: .07; z-index: 0; pointer-events: none; animation: float 14s ease-in-out infinite; }
@keyframes float { 0%, 100% { transform: translateY(-50%); } 50% { transform: translateY(calc(-50% - 18px)); } }
.hero__in { position: relative; z-index: 1; padding: 84px 0 72px; }
.hero .eyebrow { color: #9fd3a6; }
.hero h1 { color: #fff; font-size: clamp(34px, 6vw, 64px); font-weight: 800; margin: 14px 0 0; }
.hero h1 .hl { color: var(--lime); }
.hero__sub { font-size: clamp(16px, 2.2vw, 20px); color: #c8dde0; max-width: 660px; margin: 20px 0 0; line-height: 1.55; }
.hero__cta { display: flex; gap: 14px; flex-wrap: wrap; margin-top: 32px; }
.hero__mini { margin-top: 24px; font-size: 13px; color: #9fb6bb; }

.motor { background: var(--bg); padding: 80px 0; }
.motor__head { text-align: center; max-width: 720px; margin: 0 auto 34px; }
.motor__head .eyebrow { color: var(--teal); }
.motor__head h2 { font-size: clamp(26px, 4vw, 42px); font-weight: 800; margin: 10px 0; color: var(--ink); }
.motor__head p { color: #4a5b60; font-size: 17px; }
.motor__card { background: #fff; border: 1px solid #e8eef0; border-radius: 24px; box-shadow: 0 20px 50px rgba(15, 39, 48, .08); padding: 36px; max-width: 920px; margin: 0 auto; }
.motor__q { font-size: 15px; color: #4a5b60; font-weight: 600; text-align: center; }
.motor__meta { font-family: 'Jost', sans-serif; font-weight: 800; font-size: clamp(40px, 8vw, 64px); color: var(--teal-d); text-align: center; margin: 6px 0 2px; line-height: 1; }
.motor__permin { text-align: center; color: var(--lime-d); font-weight: 700; font-size: 15px; margin-bottom: 18px; }
.motor__slider { max-width: 560px; margin: 0 auto 28px; }
input[type=range] { width: 100%; height: 6px; border-radius: 999px; -webkit-appearance: none; appearance: none; background: #dfe6e8; outline: none; }
input[type=range]::-webkit-slider-thumb { -webkit-appearance: none; width: 28px; height: 28px; border-radius: 50%; background: #fff; border: 4px solid var(--lime-d); cursor: pointer; box-shadow: 0 4px 12px rgba(0, 0, 0, .18); }
.motor__paths { display: grid; grid-template-columns: 1fr 1fr; gap: 18px; }
.path { border-radius: 18px; padding: 24px; text-align: center; }
.path--biz { background: linear-gradient(160deg, #102a32, #1a2332); color: #eaf3f5; }
.path--ppl { background: #eef5f6; border: 1px solid #dde9eb; }
.path .pl { font-size: 13px; font-weight: 700; text-transform: uppercase; letter-spacing: .05em; display: flex; align-items: center; justify-content: center; gap: 6px; }
.path--biz .pl { color: #9fd3a6; } .path--ppl .pl { color: var(--teal); }
.path .pn { font-family: 'Jost', sans-serif; font-weight: 800; font-size: clamp(32px, 6vw, 46px); line-height: 1; margin: 8px 0 4px; }
.path--biz .pn { color: var(--lime); } .path--ppl .pn { color: var(--ink); }
.path .pd { font-size: 13px; opacity: .9; }
.path--biz .pd { color: #c8dde0; } .path--ppl .pd { color: #6b7c81; }
.motor__how { margin-top: 16px; background: rgba(152, 199, 58, .12); border: 1px solid rgba(152, 199, 58, .32); border-radius: 14px; padding: 14px 18px; text-align: center; font-size: 14.5px; color: #3b5a0a; }
.motor__how b { color: var(--lime-d); font-size: 17px; }
.motor__feed { margin-top: 14px; background: rgba(47, 119, 133, .08); border: 1px solid rgba(47, 119, 133, .18); border-radius: 14px; padding: 14px 18px; text-align: center; font-size: 14px; color: #274d56; }
.motor__feed b { color: var(--teal-d); }
.motor__disc { font-size: 12px; color: #9aa9ae; text-align: center; margin-top: 14px; }

.dup { padding: 80px 0; }
.dup__head { text-align: center; max-width: 720px; margin: 0 auto 32px; }
.dup__head .eyebrow { color: var(--teal); }
.dup__head h2 { font-size: clamp(26px, 4vw, 40px); font-weight: 800; margin: 10px 0; color: var(--ink); }
.dup__head p { color: #4a5b60; font-size: 17px; }
.dup__card { background: linear-gradient(160deg, #0f2730, #0c1c24); border-radius: 24px; padding: 36px; max-width: 920px; margin: 0 auto; color: #eaf3f5; }
.dup__ctrl { display: flex; align-items: center; gap: 14px; justify-content: center; flex-wrap: wrap; margin-bottom: 24px; }
.dup__ctrl label { font-size: 15px; color: #c8dde0; font-weight: 600; }
.dup__ctrl input[type=range] { max-width: 300px; background: rgba(255, 255, 255, .15); }
.dup__val { font-family: 'Jost', sans-serif; font-weight: 800; font-size: 24px; color: var(--lime); min-width: 30px; text-align: center; }
.dup__levels { display: grid; grid-template-columns: repeat(5, 1fr); gap: 10px; margin-bottom: 18px; }
.dlvl { background: rgba(255, 255, 255, .06); border: 1px solid rgba(255, 255, 255, .1); border-radius: 14px; padding: 16px 6px; text-align: center; }
.dlvl b { display: block; font-family: 'Jost', sans-serif; font-weight: 800; font-size: clamp(18px, 3vw, 28px); color: #fff; }
.dlvl span { font-size: 11px; color: #9fb6bb; }
.dup__total { text-align: center; background: rgba(152, 199, 58, .12); border: 1px solid rgba(152, 199, 58, .3); border-radius: 16px; padding: 22px; }
.dup__total .n { font-family: 'Jost', sans-serif; font-weight: 800; font-size: clamp(34px, 7vw, 52px); color: var(--lime); line-height: 1; }
.dup__total .t { font-size: 14px; color: #c8dde0; margin-top: 6px; }
.dup__tip { text-align: center; font-size: 13px; color: #9fd3a6; margin-top: 14px; }

.band1440 { background: linear-gradient(160deg, #0f2730, #0c1c24); color: #eaf3f5; padding: 64px 0; text-align: center; border-top: 1px solid rgba(255, 255, 255, .06); }
.band1440 h2 { color: #fff; font-size: clamp(24px, 3.6vw, 38px); font-weight: 800; }
.band1440 h2 .hl { color: var(--lime); }
.band1440 .row { display: flex; justify-content: center; gap: 28px; flex-wrap: wrap; margin-top: 24px; }
.b1 { text-align: center; }
.b1 .n { font-family: 'Jost', sans-serif; font-weight: 800; font-size: clamp(28px, 5vw, 42px); color: var(--lime); }
.b1 .l { font-size: 12.5px; color: #aac4c8; margin-top: 2px; }
.band1440 p { color: #c8dde0; margin-top: 22px; font-size: 15px; }

.sec { padding: 80px 0; }
.sec--alt { background: var(--bg); }
.sec__head { text-align: center; max-width: 740px; margin: 0 auto 40px; }
.sec__head .eyebrow { color: var(--teal); }
.sec__head h2 { font-size: clamp(26px, 4vw, 40px); font-weight: 800; margin: 10px 0; color: var(--ink); }
.sec__head p { color: #4a5b60; font-size: 17px; }

.org { display: grid; grid-template-columns: 1.05fr .95fr; gap: 40px; align-items: center; }
.org h2 { font-size: clamp(26px, 3.6vw, 38px); font-weight: 800; color: var(--ink); }
.org h2 .hl { color: var(--lime-d); }
.org p { color: #4a5b60; font-size: 16px; margin: 14px 0; line-height: 1.6; }
.org__list { list-style: none; display: flex; flex-direction: column; gap: 10px; }
.org__list li { display: flex; gap: 10px; font-size: 15px; color: #33454a; align-items: flex-start; }
.org__list li .ti { color: var(--lime-d); font-size: 20px; margin-top: 1px; }
.org__box { background: #fff; border: 1px solid #e8eef0; border-radius: 20px; padding: 30px; text-align: center; box-shadow: 0 16px 40px rgba(15, 39, 48, .07); }
.org__box .small { font-size: 13px; color: #8a9ba0; }
.org__box .calc { font-family: 'Jost', sans-serif; font-weight: 800; color: var(--ink); font-size: clamp(20px, 3vw, 26px); margin: 8px 0; }
.org__box .eq { font-family: 'Jost', sans-serif; font-weight: 800; color: var(--lime-d); font-size: clamp(34px, 6vw, 48px); line-height: 1; }

.circ { display: grid; grid-template-columns: auto 1fr; gap: 40px; align-items: center; max-width: 940px; margin: 0 auto; }
.circ__ring { position: relative; width: 180px; height: 180px; display: grid; place-items: center; flex-shrink: 0; }
.circ__ring::before { content: ''; position: absolute; inset: 0; border-radius: 50%; border: 3px dashed var(--lime); animation: spin 18s linear infinite; }
.circ__ring::after { content: ''; position: absolute; inset: 26px; border-radius: 50%; border: 2px dotted rgba(47, 119, 133, .3); animation: spin 26s linear infinite reverse; }
@keyframes spin { to { transform: rotate(360deg); } }
.circ__lbl { position: relative; text-align: center; font-family: 'Jost', 'Inter', sans-serif; font-weight: 800; color: var(--teal-d); font-size: 15px; line-height: 1.15; }
.circ__lbl .ti { display: block; font-size: 42px; color: var(--lime-d); margin-bottom: 2px; }
.circ__cards { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.cc { background: #fff; border: 1px solid #e8eef0; border-radius: 14px; padding: 15px 16px; font-size: 14px; font-weight: 600; color: #33454a; display: flex; align-items: center; gap: 10px; }
.cc .ti { font-size: 20px; color: var(--teal); } .cc .ar { color: var(--lime-d); }
.cc b { color: var(--teal-d); }
@media (max-width: 760px) { .circ { grid-template-columns: 1fr; justify-items: center; } .circ__cards { grid-template-columns: 1fr; } }

.seed { background: linear-gradient(160deg, #0f2730, #0c1c24); color: #eaf3f5; padding: 80px 0; text-align: center; }
.seed .eyebrow { color: var(--gold); }
.seed h2 { color: #fff; font-size: clamp(26px, 4vw, 42px); font-weight: 800; margin: 10px 0; }
.seed h2 .hl { color: var(--lime); }
.seed p { color: #c8dde0; max-width: 700px; margin: 14px auto 0; line-height: 1.6; font-size: 16px; }
.seed__sub { font-size: 14px; color: #aac4c8; }
.tree { width: 100%; max-width: 430px; margin: 26px auto 6px; display: block; overflow: visible; }
.tree__canopy { transform-box: view-box; transform-origin: 230px 330px; animation: sway 6s ease-in-out infinite; }
@keyframes sway { 0%, 100% { transform: rotate(-1.4deg) } 50% { transform: rotate(1.4deg) } }
.tree__fruits circle { transform-box: fill-box; transform-origin: center; animation: twk 2.8s ease-in-out infinite; }
.tree__fruits circle:nth-child(2) { animation-delay: .3s } .tree__fruits circle:nth-child(3) { animation-delay: .6s } .tree__fruits circle:nth-child(4) { animation-delay: .9s } .tree__fruits circle:nth-child(5) { animation-delay: 1.2s } .tree__fruits circle:nth-child(6) { animation-delay: 1.5s } .tree__fruits circle:nth-child(7) { animation-delay: 1.8s }
@keyframes twk { 0%, 100% { opacity: .65; transform: scale(.9) } 50% { opacity: 1; transform: scale(1.12) } }
.treecap { display: flex; justify-content: space-between; max-width: 430px; margin: 0 auto; gap: 14px; }
.treecap div { font-size: 12px; color: #aac4c8; text-align: center; flex: 1; }
.treecap b { display: block; font-family: 'Jost', sans-serif; font-weight: 800; font-size: 20px; color: var(--lime); }

.steps { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; }
.step { background: #fff; border: 1px solid #e8eef0; border-radius: 18px; padding: 28px 22px; }
.step__n { width: 46px; height: 46px; border-radius: 14px; display: grid; place-items: center; color: #173a0a; background: var(--lime); margin-bottom: 14px; font-size: 24px; }
.step h3 { font-size: 18px; font-weight: 800; color: var(--ink); }
.step p { color: #4a5b60; font-size: 14px; line-height: 1.5; margin-top: 6px; }

.earn { display: flex; flex-wrap: wrap; justify-content: center; gap: 18px; }
.ecard { flex: 1 1 300px; max-width: 340px; background: #fff; border: 1px solid #e8eef0; border-radius: 16px; padding: 24px; transition: transform .2s, box-shadow .2s; }
.ecard:hover { transform: translateY(-4px); box-shadow: 0 16px 36px rgba(15, 39, 48, .1); }
.ecard .ic { width: 48px; height: 48px; border-radius: 14px; display: grid; place-items: center; background: rgba(47, 119, 133, .1); color: var(--teal); font-size: 24px; margin-bottom: 12px; }
.ecard h3 { font-size: 16px; font-weight: 800; color: var(--ink); }
.ecard p { color: #4a5b60; font-size: 13.5px; line-height: 1.5; margin-top: 5px; }

.plans { display: grid; grid-template-columns: repeat(3, 1fr); gap: 18px; }
.plan { background: #fff; border: 1px solid #e8eef0; border-radius: 18px; padding: 26px 22px; display: flex; flex-direction: column; }
.plan--feat { border: 2px solid var(--lime); }
.plan h3 { font-family: 'Jost', sans-serif; font-size: 22px; font-weight: 800; color: var(--ink); }
.plan .price { color: var(--teal-d); font-weight: 700; margin: 6px 0 14px; }
.plan ul { list-style: none; display: flex; flex-direction: column; gap: 8px; margin-bottom: 18px; }
.plan li { font-size: 13.5px; color: #4a5b60; display: flex; gap: 8px; align-items: center; }
.plan li .ti { color: var(--lime-d); font-size: 18px; }

.central { display: grid; grid-template-columns: .95fr 1.05fr; gap: 40px; align-items: center; }
.central h2 { font-size: clamp(26px, 3.6vw, 38px); font-weight: 800; color: var(--ink); }
.central h2 .hl { color: var(--lime-d); }
.central p { color: #4a5b60; font-size: 16px; margin: 14px 0; line-height: 1.6; }
.central__panel { background: linear-gradient(160deg, #102a32, #0c1c24); border-radius: 22px; padding: 26px; color: #eaf3f5; }
.central__panel .ttl { font-size: 13px; color: #9fd3a6; font-weight: 700; text-transform: uppercase; letter-spacing: .05em; margin-bottom: 14px; }
.tool { display: flex; align-items: center; gap: 11px; background: rgba(255, 255, 255, .05); border: 1px solid rgba(255, 255, 255, .08); border-radius: 12px; padding: 11px 14px; margin-bottom: 9px; font-size: 14px; color: #dbe9eb; }
.tool .ti { font-size: 19px; color: var(--lime); }
.tool .d { margin-left: auto; font-size: 11px; color: #9fd3a6; }

.bar { display: grid; grid-template-columns: repeat(4, 1fr); gap: 14px; }
.bcard { text-align: center; padding: 18px; }
.bcard .ic { font-size: 30px; color: var(--teal); }
.bcard h3 { font-size: 15px; font-weight: 800; margin-top: 8px; color: var(--ink); }
.bcard p { font-size: 12.5px; color: #6b7c81; margin-top: 4px; }

.cel { display: grid; grid-template-columns: 1fr .9fr; gap: 44px; align-items: center; }
.cel h2 { font-size: clamp(26px, 3.6vw, 38px); font-weight: 800; color: var(--ink); }
.cel h2 .hl { color: var(--lime-d); }
.cel p { color: #4a5b60; font-size: 16px; margin: 14px 0; line-height: 1.6; }
.cel__list { list-style: none; display: flex; flex-direction: column; gap: 10px; }
.cel__list li { display: flex; gap: 10px; align-items: center; font-size: 15px; color: #33454a; }
.cel__list li .ti { color: var(--lime-d); font-size: 20px; }
.phone { justify-self: center; width: 300px; max-width: 100%; background: #0c1a21; border-radius: 40px; padding: 12px; box-shadow: 0 30px 60px rgba(1, 15, 28, .28); }
.phone__scr { background: #e9eef0; border-radius: 30px; overflow: hidden; }
.phone__top { background: var(--teal); color: #fff; padding: 14px 16px; display: flex; align-items: center; gap: 10px; }
.phone__av { width: 34px; height: 34px; border-radius: 50%; background: rgba(255, 255, 255, .25); display: grid; place-items: center; font-size: 16px; }
.phone__nm { font-weight: 700; font-size: 14px; line-height: 1.1; }
.phone__st { font-size: 11px; color: #cfe7ea; }
.phone__chat { padding: 16px 12px; display: flex; flex-direction: column; gap: 9px; height: 360px; overflow: hidden; }
@keyframes pop { from { opacity: 0; transform: translateY(8px) scale(.98); } to { opacity: 1; transform: none; } }
@keyframes blink { 0%, 60%, 100% { opacity: .3 } 30% { opacity: 1 } }
.phone__chat :deep(.bub) { max-width: 82%; font-size: 12.5px; line-height: 1.4; padding: 9px 12px; border-radius: 14px; animation: pop .35s ease both; }
.phone__chat :deep(.bub--in) { align-self: flex-start; background: #fff; color: #1d2b2f; border-bottom-left-radius: 5px; }
.phone__chat :deep(.bub--out) { align-self: flex-end; background: #dcf6c4; color: #1f3a0a; border-bottom-right-radius: 5px; }
.phone__chat :deep(.bub--sys) { align-self: center; background: rgba(47, 119, 133, .12); color: var(--teal-d); font-weight: 700; text-align: center; font-size: 12px; border-radius: 10px; }
.phone__chat :deep(.typing) { align-self: flex-start; background: #fff; border-radius: 14px; border-bottom-left-radius: 5px; padding: 11px 13px; display: flex; gap: 4px; animation: pop .35s ease both; }
.phone__chat :deep(.typing span) { width: 6px; height: 6px; border-radius: 50%; background: #9aa9ae; animation: blink 1.2s infinite; }
.phone__chat :deep(.typing span:nth-child(2)) { animation-delay: .2s; }
.phone__chat :deep(.typing span:nth-child(3)) { animation-delay: .4s; }
@media (max-width: 860px) { .cel { grid-template-columns: 1fr; } }

.cta { background: radial-gradient(700px 400px at 80% 0, rgba(58, 154, 173, .5), transparent 60%), linear-gradient(160deg, #0f2730, #0c1c24); color: #fff; padding: 84px 0; text-align: center; }
.cta h2 { font-size: clamp(28px, 4.5vw, 46px); font-weight: 800; color: #fff; }
.cta h2 .hl { color: var(--lime); }
.cta p { color: #c8dde0; margin: 16px 0 30px; font-size: 17px; }

.zee { padding: 80px 0; background: var(--bg); }
.zee .sec__head { text-align: center; max-width: 740px; margin: 0 auto; }
.zee .sec__head .eyebrow { color: var(--teal); }
.zee .sec__head h2 { font-size: clamp(26px, 4vw, 42px); font-weight: 800; margin: 10px 0; color: var(--ink); }
.zee .sec__head p { color: #4a5b60; font-size: 17px; line-height: 1.55; }
.zee__stage { margin: 34px auto 0; max-width: 940px; border-radius: 30px; padding: 18px; overflow: hidden; background: radial-gradient(720px 480px at 50% -10%, rgba(58, 154, 173, .30), transparent 60%), linear-gradient(160deg, #0f2e36, #0a1d24); box-shadow: 0 30px 70px rgba(8, 24, 30, .28); }
.zee__svg { width: 100%; height: auto; display: block; }
.zee__grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 18px; max-width: 940px; margin: 24px auto 0; }
.zee__card { background: #fff; border: 1px solid #e8eef0; border-radius: 18px; padding: 24px; transition: transform .25s ease, box-shadow .25s ease; }
.zee__card:hover { transform: translateY(-4px); box-shadow: 0 18px 40px rgba(15, 39, 48, .10); }
.zee__ic { width: 50px; height: 50px; border-radius: 15px; display: grid; place-items: center; color: #173a0a; background: linear-gradient(145deg, #aee05a, #7aad1f); margin-bottom: 14px; }
.zee__ic i { font-size: 26px; }
.zee__card h3 { font-size: 17px; font-weight: 800; color: var(--ink); }
.zee__card p { color: #4a5b60; font-size: 14px; line-height: 1.5; margin-top: 6px; }
@media (max-width: 820px) { .zee__grid { grid-template-columns: 1fr; } }

@media (max-width: 860px) {
  .motor__paths, .org, .central, .steps, .plans, .bar { grid-template-columns: 1fr; }
  .bar { grid-template-columns: repeat(2, 1fr); }
  .dup__levels { grid-template-columns: repeat(3, 1fr); }
}
@media (prefers-reduced-motion: reduce) {
  * { animation: none !important; }
  .reveal { opacity: 1; transform: none; transition: none; }
}
</style>
