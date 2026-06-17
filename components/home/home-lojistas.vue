<template>
  <section class="qs-biz" aria-labelledby="qs-biz-title">
    <div class="container">
      <div class="qs-biz__inner">
        <div class="qs-biz__content">
          <span class="qs-biz__label">PARA LOJISTAS</span>
          <h2 id="qs-biz-title" class="qs-biz__title">Aumente suas vendas com cashback</h2>
          <p class="qs-biz__desc">Credenciando sua empresa na Quanta Shop, você passa a receber clientes que já têm saldo de cashback para gastar — e ainda conquista recompra automática.</p>

          <ul class="qs-biz__benefits" role="list">
            <li class="qs-biz__benefit">
              <span class="qs-biz__check" aria-hidden="true">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><path d="M20 6 9 17l-5-5"/></svg>
              </span>
              <span>Clientes com saldo pronto para gastar</span>
            </li>
            <li class="qs-biz__benefit">
              <span class="qs-biz__check" aria-hidden="true">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><path d="M20 6 9 17l-5-5"/></svg>
              </span>
              <span>Exposição na plataforma para +12.000 usuários</span>
            </li>
            <li class="qs-biz__benefit">
              <span class="qs-biz__check" aria-hidden="true">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><path d="M20 6 9 17l-5-5"/></svg>
              </span>
              <span>Recompra automática — o cashback traz o cliente de volta</span>
            </li>
            <li class="qs-biz__benefit">
              <span class="qs-biz__check" aria-hidden="true">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><path d="M20 6 9 17l-5-5"/></svg>
              </span>
              <span>Dashboard de vendas e cashback em tempo real</span>
            </li>
            <li class="qs-biz__benefit">
              <span class="qs-biz__check" aria-hidden="true">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><path d="M20 6 9 17l-5-5"/></svg>
              </span>
              <span>Credenciamento simples e sem burocracia</span>
            </li>
          </ul>

          <div class="qs-biz__cta-row">
            <nuxt-link href="/agencia/cadastro" class="qs-biz__btn-primary">
              Credenciar minha empresa
              <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5" aria-hidden="true"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
            </nuxt-link>
            <nuxt-link href="/para-sua-empresa" class="qs-biz__btn-ghost">Saber mais</nuxt-link>
          </div>
        </div>

        <div class="qs-biz__visual" aria-hidden="true">
          <div class="qs-biz__card-stack">
            <div class="qs-biz__stat-card qs-biz__stat-card--1">
              <div class="qs-biz__stat-icon">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M3 17l6-6 4 4 7-7"/><path d="M17 8h4v4"/></svg>
              </div>
              <div>
                <b>+38%</b>
                <span>taxa de recompra</span>
              </div>
            </div>

            <div class="qs-biz__main-card">
              <div class="qs-biz__main-icon">
                <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>
              </div>
              <h4>Sua loja na Quanta</h4>
              <p>Visível para +12.000 consumidores ativos prontos para comprar.</p>
              <div class="qs-biz__pill">✓ Credenciamento aprovado</div>
            </div>

            <div class="qs-biz__stat-card qs-biz__stat-card--2">
              <div class="qs-biz__stat-icon qs-biz__stat-icon--lime">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="2" y="5" width="20" height="14" rx="2"/><path d="M2 10h20"/></svg>
              </div>
              <div>
                <b>R$ 0</b>
                <span>custo de adesão</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.qs-biz { padding: 80px 0; background: #fff; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-biz__inner { display: grid; grid-template-columns: 1fr 1fr; gap: 64px; align-items: center; }
@media (max-width: 860px) { .qs-biz__inner { grid-template-columns: 1fr; gap: 40px; } }

.qs-biz__label { display: inline-block; font-family: 'Inter','Jost',sans-serif; font-size: 11px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #2F7785; margin-bottom: 14px; }
.qs-biz__title { font-family: 'Jost','Inter',sans-serif; font-size: clamp(26px,3.5vw,38px); font-weight: 800; color: #225F6B; letter-spacing: -.02em; line-height: 1.15; margin-bottom: 14px; }
.qs-biz__desc { font-family: 'Inter','Jost',sans-serif; font-size: 15px; color: #6b7280; line-height: 1.7; margin-bottom: 28px; }

.qs-biz__benefits { list-style: none; padding: 0; margin: 0 0 32px; display: flex; flex-direction: column; gap: 12px; }
.qs-biz__benefit { display: flex; align-items: flex-start; gap: 10px; font-family: 'Inter','Jost',sans-serif; font-size: 14px; color: #374151; }
.qs-biz__check { flex-shrink: 0; width: 22px; height: 22px; border-radius: 50%; background: linear-gradient(135deg,#7aad1f,#98C73A); color: #fff; display: flex; align-items: center; justify-content: center; margin-top: 1px; }

.qs-biz__cta-row { display: flex; align-items: center; gap: 16px; flex-wrap: wrap; }
.qs-biz__btn-primary { display: inline-flex; align-items: center; gap: 8px; background: #98C73A; color: #fff; font-family: 'Inter','Jost',sans-serif; font-size: 14px; font-weight: 700; padding: 13px 24px; border-radius: 8px; text-decoration: none; transition: background .2s, transform .2s; box-shadow: 0 4px 20px rgba(152,199,58,.3); }
.qs-biz__btn-primary:hover { background: #7aad1f; transform: translateY(-2px); }
.qs-biz__btn-ghost { display: inline-flex; align-items: center; font-family: 'Inter','Jost',sans-serif; font-size: 14px; font-weight: 600; color: #2F7785; text-decoration: none; border-bottom: 1.5px solid #2F7785; padding-bottom: 2px; transition: color .2s; }
.qs-biz__btn-ghost:hover { color: #225F6B; border-color: #225F6B; }

.qs-biz__visual { display: flex; align-items: center; justify-content: center; }
.qs-biz__card-stack { position: relative; width: 300px; height: 340px; }

.qs-biz__main-card { position: absolute; top: 50%; left: 50%; transform: translate(-50%,-50%); width: 220px; background: linear-gradient(135deg,#225F6B,#2F7785); border-radius: 20px; padding: 28px 24px; color: #fff; text-align: center; box-shadow: 0 12px 40px rgba(34,95,107,.35); }
.qs-biz__main-icon { width: 56px; height: 56px; border-radius: 16px; background: rgba(255,255,255,.15); display: flex; align-items: center; justify-content: center; margin: 0 auto 16px; }
.qs-biz__main-card h4 { font-family: 'Jost','Inter',sans-serif; font-size: 17px; font-weight: 700; margin-bottom: 8px; }
.qs-biz__main-card p { font-family: 'Inter','Jost',sans-serif; font-size: 13px; opacity: .85; line-height: 1.5; margin-bottom: 14px; }
.qs-biz__pill { display: inline-block; background: rgba(152,199,58,.2); border: 1px solid rgba(152,199,58,.5); border-radius: 999px; padding: 5px 12px; font-family: 'Inter','Jost',sans-serif; font-size: 12px; font-weight: 600; color: #98C73A; }

.qs-biz__stat-card { position: absolute; display: flex; align-items: center; gap: 10px; background: #fff; border-radius: 14px; padding: 14px 16px; box-shadow: 0 4px 20px rgba(0,0,0,.1); border: 1px solid #f0f4f6; min-width: 140px; }
.qs-biz__stat-card--1 { top: 8%; right: -10%; }
.qs-biz__stat-card--2 { bottom: 8%; left: -10%; }
.qs-biz__stat-card b { display: block; font-family: 'Jost','Inter',sans-serif; font-size: 20px; font-weight: 800; color: #225F6B; }
.qs-biz__stat-card span { font-family: 'Inter','Jost',sans-serif; font-size: 11px; color: #9ca3af; text-transform: uppercase; letter-spacing: .06em; }
.qs-biz__stat-icon { width: 36px; height: 36px; border-radius: 10px; background: linear-gradient(135deg,#2F7785,#225F6B); color: #fff; display: flex; align-items: center; justify-content: center; flex-shrink: 0; }
.qs-biz__stat-icon--lime { background: linear-gradient(135deg,#7aad1f,#98C73A); }

@media (max-width: 860px) { .qs-biz__visual { display: none; } }
@media (prefers-reduced-motion: reduce) { .qs-biz__btn-primary { transition: none; } }
</style>
