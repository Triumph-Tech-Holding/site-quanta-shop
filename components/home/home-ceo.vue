<template>
  <section class="qs-ceo-sec" aria-labelledby="qs-ceo-title">
    <div class="container">
      <div class="qs-ceo">
        <div class="qs-ceo__film" aria-hidden="true"></div>

        <div class="qs-ceo__in">
          <span class="qs-ceo__tag"><span class="qs-ceo__d" aria-hidden="true"></span> CEO &amp; Founder</span>
          <p class="qs-ceo__pre">FALE COM O CEO</p>
          <h2 id="qs-ceo-title" class="qs-ceo__h2">Mauro Triumph</h2>
          <p class="qs-ceo__text">Clareza estratégica sem rodeios, sem burocracia, sem perda de tempo. Converse com a IA do CEO e leve sua parceria adiante.</p>
          <div class="qs-ceo__cta-row">
            <nuxt-link class="qs-ceo__btn qs-ceo__btn--primary" href="#main">Iniciar Conversa com IA</nuxt-link>
            <a class="qs-ceo__btn qs-ceo__btn--ghost" href="https://api.whatsapp.com/send/?phone=552140404866" target="_blank" rel="noopener">Falar no WhatsApp</a>
          </div>
          <div class="qs-ceo__stats qs-ceo__stats--inline">
            <div><b>Em até 24h</b><span>RESPOSTAS</span></div>
            <div><b>+200</b><span>PARCERIAS FECHADAS</span></div>
          </div>
        </div>

        <div class="qs-ceo__photo">
          <img
            src="/img/team/mauro-cutout.png"
            alt="Mauro Triumph, CEO e fundador da Quanta Shop"
            width="700" height="665" loading="lazy" decoding="async"
          />
          <div class="qs-ceo__card qs-ceo__card--1">
            <span class="qs-ceo__card-ic"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" aria-hidden="true"><path d="M21 15a4 4 0 0 1-4 4H8l-5 4V7a4 4 0 0 1 4-4h10a4 4 0 0 1 4 4z" /></svg></span>
            <div><span class="qs-ceo__card-lbl">RESPOSTAS</span><b>Em até 24h</b></div>
          </div>
          <div class="qs-ceo__card qs-ceo__card--2">
            <span class="qs-ceo__card-ic"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" aria-hidden="true"><path d="M3 17l6-6 4 4 7-7" /><path d="M17 8h4v4" /></svg></span>
            <div><span class="qs-ceo__card-lbl">PARCERIAS</span><b>+200 fechadas</b></div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts"></script>

<style scoped>
.qs-ceo-sec { padding: 84px 0; background: #f7f8fa; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-ceo { position: relative; display: grid; grid-template-columns: 1.05fr .95fr; align-items: end; border-radius: 20px; overflow: hidden; color: #fff; min-height: 470px; background: linear-gradient(135deg, #225F6B, #163038); }
.qs-ceo__film { position: absolute; inset: 0; z-index: 2; pointer-events: none; background: linear-gradient(115deg, rgba(20,48,56,.92) 0%, rgba(26,74,84,.66) 42%, rgba(47,119,133,.48) 66%, rgba(152,199,58,.28) 100%); }

.qs-ceo__in { position: relative; z-index: 3; padding: 56px; align-self: center; }
.qs-ceo__tag { display: inline-flex; align-items: center; gap: 7px; background: rgba(255,255,255,.12); border: 1px solid rgba(255,255,255,.22); border-radius: 999px; padding: 6px 13px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 12px; font-weight: 700; letter-spacing: .06em; color: #fff; }
.qs-ceo__d { width: 7px; height: 7px; border-radius: 50%; background: #98C73A; }
.qs-ceo__pre { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; letter-spacing: .22em; margin-top: 18px; color: #cfe6ea; }
.qs-ceo__h2 { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: clamp(28px, 4.6vw, 54px); font-weight: 800; text-transform: uppercase; letter-spacing: -.01em; margin: 4px 0 14px; background: linear-gradient(92deg, #ffffff 0%, #e6fff0 36%, #98C73A 100%); -webkit-background-clip: text; background-clip: text; -webkit-text-fill-color: transparent; color: transparent; text-shadow: 0 2px 30px rgba(152,199,58,.22); }
.qs-ceo__text { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 17px; color: #e8f3f4; max-width: 430px; text-shadow: 0 1px 12px rgba(0,0,0,.3); }
.qs-ceo__cta-row { display: flex; gap: 12px; margin-top: 26px; flex-wrap: wrap; }
.qs-ceo__btn { display: inline-flex; align-items: center; gap: 8px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-weight: 700; font-size: 15px; border-radius: 999px; padding: 14px 26px; min-height: 44px; text-decoration: none; border: 1.5px solid transparent; transition: transform .2s ease, box-shadow .2s ease, background .2s ease; }
.qs-ceo__btn--primary { background: linear-gradient(180deg, #98C73A, #7aad1f); color: #173a0a; box-shadow: 0 10px 28px rgba(152,199,58,.35); }
.qs-ceo__btn--primary:hover { transform: translateY(-2px); box-shadow: 0 16px 36px rgba(152,199,58,.45); }
.qs-ceo__btn--ghost { background: rgba(255,255,255,.08); color: #fff; border-color: rgba(255,255,255,.35); }
.qs-ceo__btn--ghost:hover { background: rgba(255,255,255,.16); }

.qs-ceo__stats { display: flex; gap: 30px; margin-top: 28px; }
.qs-ceo__stats b { display: block; font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: 20px; color: #98C73A; }
.qs-ceo__stats span { font-size: 12px; color: #bcd6da; letter-spacing: .08em; }
.qs-ceo__stats--inline { display: none; }

.qs-ceo__photo { position: relative; z-index: 1; align-self: end; justify-self: center; display: flex; align-items: flex-end; height: 100%; }
.qs-ceo__photo img { max-height: 480px; width: auto; display: block; }

.qs-ceo__card { position: absolute; z-index: 4; display: flex; align-items: center; gap: 11px; background: rgba(12,26,33,.5); backdrop-filter: blur(10px); -webkit-backdrop-filter: blur(10px); border: 1px solid rgba(255,255,255,.16); border-radius: 16px; padding: 12px 16px; box-shadow: 0 4px 20px rgba(1,15,28,.12); }
.qs-ceo__card--1 { top: 8%; right: 0; }
.qs-ceo__card--2 { bottom: 18%; left: -14%; }
.qs-ceo__card-ic { width: 38px; height: 38px; border-radius: 11px; display: grid; place-items: center; background: linear-gradient(135deg, #7aad1f, #98C73A); color: #0c1a21; flex-shrink: 0; }
.qs-ceo__card--2 .qs-ceo__card-ic { background: linear-gradient(135deg, #2F7785, #3A9AAD); color: #fff; }
.qs-ceo__card-ic svg { width: 19px; height: 19px; }
.qs-ceo__card-lbl { display: block; font-size: 11px; letter-spacing: .07em; color: #bcd6da; text-transform: uppercase; }
.qs-ceo__card b { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: 17px; color: #fff; }

@media (max-width: 880px) {
  .qs-ceo { grid-template-columns: 1fr; text-align: center; min-height: 0; }
  .qs-ceo__in { padding: 40px 24px 0; }
  .qs-ceo__photo { justify-self: center; }
  .qs-ceo__photo img { max-height: 340px; }
  .qs-ceo__film { background: linear-gradient(0deg, rgba(20,48,56,.93) 35%, rgba(26,74,84,.55) 70%, rgba(152,199,58,.18) 100%); }
  .qs-ceo__card { display: none; }
  .qs-ceo__stats--inline { display: flex; justify-content: center; }
}
@media (prefers-reduced-motion: reduce) { .qs-ceo__btn { transition: none; } }
</style>
