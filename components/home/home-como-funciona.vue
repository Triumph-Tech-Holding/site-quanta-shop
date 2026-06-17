<template>
  <section class="qs-hiw" aria-labelledby="qs-hiw-title">
    <div class="container">
      <div class="qs-hiw__header">
        <span class="qs-hiw__label">SIMPLES ASSIM</span>
        <h2 id="qs-hiw-title" class="qs-hiw__title">Como funciona a Quanta Shop</h2>
        <p class="qs-hiw__sub">Em três passos você já está ganhando dinheiro de volta.</p>
      </div>

      <div class="qs-hiw__steps">
        <div class="qs-hiw__step">
          <div class="qs-hiw__num" aria-hidden="true">01</div>
          <div class="qs-hiw__icon-wrap">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" aria-hidden="true"><path d="M20 7H4a2 2 0 0 0-2 2v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2z"/><path d="M16 21V5a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16"/></svg>
          </div>
          <h3 class="qs-hiw__step-title">Ative o cashback</h3>
          <p class="qs-hiw__step-desc">Crie sua conta grátis e ative o cashback nas suas lojas favoritas com um clique. Sem taxa, sem complicação.</p>
        </div>

        <div class="qs-hiw__connector" aria-hidden="true">
          <svg viewBox="0 0 80 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <defs>
              <linearGradient id="conn-grad" x1="0" y1="0" x2="80" y2="0" gradientUnits="userSpaceOnUse">
                <stop offset="0%" stop-color="#2F7785"/>
                <stop offset="100%" stop-color="#98C73A"/>
              </linearGradient>
            </defs>
            <line x1="0" y1="12" x2="72" y2="12" stroke="url(#conn-grad)" stroke-width="2" stroke-dasharray="6 4"/>
            <polygon points="72,7 80,12 72,17" fill="#98C73A"/>
          </svg>
        </div>

        <div class="qs-hiw__step">
          <div class="qs-hiw__num" aria-hidden="true">02</div>
          <div class="qs-hiw__icon-wrap">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" aria-hidden="true"><circle cx="9" cy="21" r="1"/><circle cx="20" cy="21" r="1"/><path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"/></svg>
          </div>
          <h3 class="qs-hiw__step-title">Compre normalmente</h3>
          <p class="qs-hiw__step-desc">Compre online ou nas lojas parceiras do seu bairro — exatamente como você já faz. Não precisa mudar nada.</p>
        </div>

        <div class="qs-hiw__connector" aria-hidden="true">
          <svg viewBox="0 0 80 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <defs>
              <linearGradient id="conn-grad2" x1="0" y1="0" x2="80" y2="0" gradientUnits="userSpaceOnUse">
                <stop offset="0%" stop-color="#2F7785"/>
                <stop offset="100%" stop-color="#98C73A"/>
              </linearGradient>
            </defs>
            <line x1="0" y1="12" x2="72" y2="12" stroke="url(#conn-grad2)" stroke-width="2" stroke-dasharray="6 4"/>
            <polygon points="72,7 80,12 72,17" fill="#98C73A"/>
          </svg>
        </div>

        <div class="qs-hiw__step">
          <div class="qs-hiw__num" aria-hidden="true">03</div>
          <div class="qs-hiw__icon-wrap qs-hiw__icon-wrap--lime">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" aria-hidden="true"><rect x="2" y="5" width="20" height="14" rx="2"/><path d="M2 10h20"/></svg>
          </div>
          <h3 class="qs-hiw__step-title">Receba via PIX</h3>
          <p class="qs-hiw__step-desc">O dinheiro volta automático pra sua carteira Quanta. Saque na hora, via PIX. Instantâneo e transparente.</p>
        </div>
      </div>

      <div class="qs-hiw__cta">
        <nuxt-link href="/register" class="qs-hiw__btn">
          Criar conta grátis e começar
          <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5" aria-hidden="true"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </nuxt-link>
        <nuxt-link href="/como-funciona" class="qs-hiw__link">Ver detalhes completos →</nuxt-link>
      </div>
    </div>
  </section>
</template>

<style scoped>
.qs-hiw { padding: 80px 0; background: #fff; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-hiw__header { text-align: center; margin-bottom: 56px; }
.qs-hiw__label { display: inline-block; font-family: 'Inter','Jost',sans-serif; font-size: 11px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #2F7785; margin-bottom: 14px; }
.qs-hiw__title { font-family: 'Jost','Inter',sans-serif; font-size: clamp(26px,4vw,40px); font-weight: 800; color: #225F6B; letter-spacing: -.02em; line-height: 1.15; margin-bottom: 12px; }
.qs-hiw__sub { font-family: 'Inter','Jost',sans-serif; font-size: 16px; color: #6b7280; max-width: 460px; margin: 0 auto; }

.qs-hiw__steps { display: flex; align-items: center; gap: 0; }

.qs-hiw__step { flex: 1; display: flex; flex-direction: column; align-items: center; text-align: center; padding: 36px 28px; background: #f9fafb; border-radius: 20px; border: 1.5px solid #f0f4f6; transition: transform .25s, box-shadow .25s; }
.qs-hiw__step:hover { transform: translateY(-6px); box-shadow: 0 12px 40px rgba(47,119,133,.12); }

.qs-hiw__connector { flex-shrink: 0; width: 80px; margin: 0 -4px; }
.qs-hiw__connector svg { width: 80px; height: 24px; display: block; }

.qs-hiw__num { font-family: 'Jost','Inter',sans-serif; font-size: 13px; font-weight: 800; color: #2F7785; letter-spacing: .08em; margin-bottom: 16px; }

.qs-hiw__icon-wrap { width: 64px; height: 64px; border-radius: 18px; background: linear-gradient(135deg, #2F7785, #225F6B); color: #fff; display: flex; align-items: center; justify-content: center; margin-bottom: 20px; box-shadow: 0 6px 24px rgba(47,119,133,.28); }
.qs-hiw__icon-wrap--lime { background: linear-gradient(135deg, #7aad1f, #98C73A); box-shadow: 0 6px 24px rgba(152,199,58,.28); }

.qs-hiw__step-title { font-family: 'Jost','Inter',sans-serif; font-size: 18px; font-weight: 700; color: #225F6B; margin-bottom: 10px; }
.qs-hiw__step-desc { font-family: 'Inter','Jost',sans-serif; font-size: 14px; color: #6b7280; line-height: 1.6; max-width: 240px; }

.qs-hiw__cta { display: flex; align-items: center; justify-content: center; gap: 20px; margin-top: 48px; flex-wrap: wrap; }
.qs-hiw__btn { display: inline-flex; align-items: center; gap: 8px; background: #98C73A; color: #fff; font-family: 'Inter','Jost',sans-serif; font-size: 15px; font-weight: 700; padding: 14px 28px; border-radius: 8px; text-decoration: none; transition: background .2s, transform .2s; box-shadow: 0 4px 20px rgba(152,199,58,.3); }
.qs-hiw__btn:hover { background: #7aad1f; transform: translateY(-2px); }
.qs-hiw__link { font-family: 'Inter','Jost',sans-serif; font-size: 14px; font-weight: 600; color: #2F7785; text-decoration: none; transition: color .2s; }
.qs-hiw__link:hover { color: #225F6B; }

@media (max-width: 860px) {
  .qs-hiw__steps { flex-direction: column; gap: 0; }
  .qs-hiw__connector { width: 24px; height: 48px; margin: -4px 0; transform: rotate(90deg); }
  .qs-hiw__step { width: 100%; max-width: 360px; }
}
@media (prefers-reduced-motion: reduce) {
  .qs-hiw__step, .qs-hiw__btn { transition: none; }
}
</style>
