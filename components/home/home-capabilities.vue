<template>
  <section class="qs-cap" aria-labelledby="qs-cap-title">
    <div class="container">
      <div class="qs-cap__header">
        <span class="qs-cap__label">QUANTA IA</span>
        <h2 id="qs-cap-title" class="qs-cap__title">Uma conversa resolve</h2>
        <p class="qs-cap__sub">Busque, cote, consulte seu saldo e tire dúvidas — tudo pelo chat, como no WhatsApp.</p>
      </div>

      <div class="qs-cap__grid">
        <div class="qs-cap__card">
          <div class="qs-cap__icon">
            <svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" aria-hidden="true"><circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/></svg>
          </div>
          <h3 class="qs-cap__card-title">Busque e cote</h3>
          <p class="qs-cap__card-desc">Pergunte "qual loja tem mais cashback em tênis?" e a IA compara preços e cashback pra você em segundos.</p>
          <div class="qs-cap__chat-bubble">
            <span class="qs-cap__bubble qs-cap__bubble--user">Quero comprar um tênis. Qual loja dá mais cashback?</span>
            <span class="qs-cap__bubble qs-cap__bubble--ai">A Nike tem 12% e a Puma 10% esta semana. Quer que eu ative o cashback para você? 🎯</span>
          </div>
        </div>

        <div class="qs-cap__card qs-cap__card--featured">
          <div class="qs-cap__icon qs-cap__icon--lime">
            <svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" aria-hidden="true"><rect x="2" y="5" width="20" height="14" rx="2"/><path d="M2 10h20"/></svg>
          </div>
          <h3 class="qs-cap__card-title">Consulte seu saldo</h3>
          <p class="qs-cap__card-desc">Seu extrato de cashback, histórico de compras e saldo disponível para saque — tudo em uma pergunta.</p>
          <div class="qs-cap__chat-bubble">
            <span class="qs-cap__bubble qs-cap__bubble--user">Quanto tenho de cashback disponível?</span>
            <span class="qs-cap__bubble qs-cap__bubble--ai">Você tem R$ 87,40 disponíveis para saque. Quer receber via PIX agora? 💰</span>
          </div>
        </div>

        <div class="qs-cap__card">
          <div class="qs-cap__icon">
            <svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" aria-hidden="true"><path d="M21 15a4 4 0 0 1-4 4H8l-5 4V7a4 4 0 0 1 4-4h10a4 4 0 0 1 4 4z"/></svg>
          </div>
          <h3 class="qs-cap__card-title">Tire qualquer dúvida</h3>
          <p class="qs-cap__card-desc">Sobre como funciona, prazos de cashback, parceiros, planos ou qualquer coisa — a IA responde na hora.</p>
          <div class="qs-cap__chat-bubble">
            <span class="qs-cap__bubble qs-cap__bubble--user">Em quantos dias o cashback da minha compra cai?</span>
            <span class="qs-cap__bubble qs-cap__bubble--ai">Compras online caem em até 30 dias após confirmação. Lojas físicas parceiras são instantâneas! ✓</span>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.qs-cap { padding: 80px 0; background: #f9fafb; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-cap__header { text-align: center; margin-bottom: 52px; }
.qs-cap__label { display: inline-block; font-family: 'Inter','Jost',sans-serif; font-size: 11px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #98C73A; margin-bottom: 12px; background: rgba(152,199,58,.1); padding: 4px 12px; border-radius: 999px; }
.qs-cap__title { font-family: 'Jost','Inter',sans-serif; font-size: clamp(26px,4vw,40px); font-weight: 800; color: #225F6B; letter-spacing: -.02em; margin-bottom: 12px; }
.qs-cap__sub { font-family: 'Inter','Jost',sans-serif; font-size: 16px; color: #6b7280; max-width: 480px; margin: 0 auto; }

.qs-cap__grid { display: grid; grid-template-columns: repeat(3,1fr); gap: 24px; }
@media (max-width: 900px) { .qs-cap__grid { grid-template-columns: 1fr; max-width: 420px; margin: 0 auto; } }

.qs-cap__card { background: #fff; border-radius: 20px; padding: 32px 28px; border: 1.5px solid #f0f4f6; display: flex; flex-direction: column; gap: 16px; transition: transform .25s, box-shadow .25s; }
.qs-cap__card:hover { transform: translateY(-6px); box-shadow: 0 14px 44px rgba(47,119,133,.12); }
.qs-cap__card--featured { border-color: #2F7785; box-shadow: 0 4px 24px rgba(47,119,133,.1); }

.qs-cap__icon { width: 56px; height: 56px; border-radius: 16px; background: linear-gradient(135deg,#2F7785,#225F6B); color: #fff; display: flex; align-items: center; justify-content: center; flex-shrink: 0; }
.qs-cap__icon--lime { background: linear-gradient(135deg,#7aad1f,#98C73A); }

.qs-cap__card-title { font-family: 'Jost','Inter',sans-serif; font-size: 18px; font-weight: 700; color: #225F6B; }
.qs-cap__card-desc { font-family: 'Inter','Jost',sans-serif; font-size: 14px; color: #6b7280; line-height: 1.6; }

.qs-cap__chat-bubble { display: flex; flex-direction: column; gap: 8px; margin-top: 4px; }
.qs-cap__bubble { display: block; font-family: 'Inter','Jost',sans-serif; font-size: 13px; line-height: 1.5; padding: 10px 14px; border-radius: 12px; max-width: 94%; }
.qs-cap__bubble--user { background: #f0f4f6; color: #374151; border-radius: 12px 12px 4px 12px; align-self: flex-end; }
.qs-cap__bubble--ai { background: linear-gradient(135deg,#225F6B,#2F7785); color: #fff; border-radius: 4px 12px 12px 12px; align-self: flex-start; }

@media (prefers-reduced-motion: reduce) {
  .qs-cap__card { transition: none; }
}
</style>
