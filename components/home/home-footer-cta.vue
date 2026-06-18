<template>
  <section class="qs-fcta" aria-labelledby="qs-fcta-title">
    <QsLogo tone="white" :size="62" class="qs-fcta__ic" aria-hidden="true" />
    <h2 id="qs-fcta-title" class="qs-fcta__h2">Pronto para começar a economizar?</h2>
    <p class="qs-fcta__lead">Cadastre-se gratuitamente e aproveite cashback em milhares de lojas.</p>
    <div class="qs-fcta__row">
      <nuxt-link class="qs-fcta__btn qs-fcta__btn--primary" href="/register">Cadastrar Agora →</nuxt-link>
      <nuxt-link class="qs-fcta__btn qs-fcta__btn--ghost" href="/login">Já tenho conta</nuxt-link>
    </div>
  </section>
</template>

<script setup lang="ts"></script>

<style scoped>
.qs-fcta { background: radial-gradient(900px 500px at 50% -20%, rgba(58,154,173,.5), transparent 60%), linear-gradient(160deg, #225F6B, #143038); color: #fff; text-align: center; padding: 88px 24px; }
.qs-fcta__ic { margin: 0 auto 20px; display: block; }
.qs-fcta__h2 { font-family: 'Bruum FY','Jost','Inter',sans-serif; color: #fff; font-size: clamp(32px, 4vw, 48px); font-weight: 800; line-height: 1.08; letter-spacing: -.02em; }
.qs-fcta__lead { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 18px; color: #cfe6ea; margin: 14px 0 30px; }
.qs-fcta__row { display: flex; gap: 14px; justify-content: center; flex-wrap: wrap; }
.qs-fcta__btn { display: inline-flex; align-items: center; gap: 8px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-weight: 700; font-size: 15px; border-radius: 999px; padding: 14px 26px; min-height: 44px; text-decoration: none; border: 1.5px solid transparent; transition: transform .2s ease, box-shadow .2s ease, background .2s ease; }
.qs-fcta__btn--primary { background: linear-gradient(180deg, #98C73A, #7aad1f); color: #173a0a; box-shadow: 0 10px 28px rgba(152,199,58,.35); }
.qs-fcta__btn--primary:hover { transform: translateY(-2px); box-shadow: 0 16px 36px rgba(152,199,58,.45); }
.qs-fcta__btn--ghost { background: rgba(255,255,255,.08); color: #fff; border-color: rgba(255,255,255,.35); }
.qs-fcta__btn--ghost:hover { background: rgba(255,255,255,.16); }

@media (prefers-reduced-motion: reduce) { .qs-fcta__btn { transition: none; } }
</style>
