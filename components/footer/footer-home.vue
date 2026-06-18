<template>
  <footer class="qs-ft">
    <div class="container">
      <div class="qs-ft__top">
        <div class="qs-ft__brand-col">
          <nuxt-link class="qs-ft__brand" href="/" aria-label="Quanta Shop">
            <img class="qs-ft__logo" src="/img/logo/logo-white.png" width="128" height="42" alt="Quanta Shop" decoding="async" />
          </nuxt-link>
          <div class="qs-ft__seals">
            <span class="qs-ft__seal"><b>PIX</b> saque instantâneo</span>
            <span class="qs-ft__seal"><b>LGPD</b> dados protegidos</span>
            <span class="qs-ft__seal"><b>SSL</b> site seguro</span>
          </div>
        </div>

        <div class="qs-ft__news-col">
          <p class="qs-ft__news-title">Receba ofertas e cashback turbinado</p>
          <p class="qs-ft__news-sub">Novidades e promoções no seu e-mail. Sem spam.</p>
          <form class="qs-ft__news" @submit.prevent="subscribe">
            <input v-model="email" type="email" aria-label="Seu e-mail" placeholder="seu@email.com" />
            <button type="submit">Assinar</button>
          </form>
        </div>
      </div>

      <div class="qs-ft__row">
        <nav class="qs-ft__nav" aria-label="Rodapé">
          <nuxt-link v-for="(l, i) in links" :key="i" :href="l.href">{{ l.label }}</nuxt-link>
        </nav>
        <div class="qs-ft__soc">
          <a href="https://www.instagram.com/quantashop" target="_blank" rel="noopener" aria-label="Instagram"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M12 2.2c3.2 0 3.6 0 4.85.07 3.25.15 4.77 1.69 4.92 4.92.06 1.27.07 1.65.07 4.85s-.01 3.58-.07 4.85c-.15 3.23-1.66 4.77-4.92 4.92-1.27.06-1.65.07-4.85.07s-3.58-.01-4.85-.07c-3.26-.15-4.77-1.7-4.92-4.92C2.16 15.58 2.15 15.2 2.15 12s.01-3.58.07-4.85C2.37 3.92 3.88 2.38 7.15 2.27 8.42 2.21 8.8 2.2 12 2.2zm0 3.64a6.16 6.16 0 100 12.32 6.16 6.16 0 000-12.32zm0 10.16a4 4 0 110-8 4 4 0 010 8zm6.4-10.4a1.44 1.44 0 100 2.88 1.44 1.44 0 000-2.88z" /></svg></a>
          <a href="https://www.facebook.com/quantashop" target="_blank" rel="noopener" aria-label="Facebook"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M24 12.07C24 5.44 18.63.07 12 .07S0 5.44 0 12.07c0 5.99 4.39 10.95 10.13 11.85v-8.38H7.08v-3.47h3.05V9.43c0-3.01 1.79-4.67 4.53-4.67 1.31 0 2.69.24 2.69.24v2.95h-1.51c-1.49 0-1.96.93-1.96 1.87v2.25h3.33l-.53 3.47h-2.8v8.38C19.61 23.02 24 18.06 24 12.07z" /></svg></a>
          <a href="https://www.youtube.com/@quantashop" target="_blank" rel="noopener" aria-label="YouTube"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M23.5 6.19a3.02 3.02 0 00-2.12-2.14C19.5 3.55 12 3.55 12 3.55s-7.5 0-9.38.5A3.02 3.02 0 00.5 6.19C0 8.07 0 12 0 12s0 3.93.5 5.81a3.02 3.02 0 002.12 2.14c1.88.5 9.38.5 9.38.5s7.5 0 9.38-.5a3.02 3.02 0 002.12-2.14C24 15.93 24 12 24 12s0-3.93-.5-5.81zM9.55 15.57V8.43L15.82 12l-6.27 3.57z" /></svg></a>
        </div>
      </div>

      <p class="qs-ft__bottom">© {{ year }} Quanta Shop · Consumo inteligente e prosperidade equitativa.</p>
    </div>
  </footer>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const year = new Date().getFullYear();
const email = ref('');
function subscribe() {
  email.value = '';
}

const links = [
  { label: 'Como funciona', href: '/como-funciona' },
  { label: 'Quem somos', href: '/agencia/quem-somos' },
  { label: 'Credenciamento', href: '/agencia/credenciar' },
  { label: 'Suporte', href: '/contato' },
  { label: 'Dúvidas', href: '/agencia/faq' },
  { label: 'Termos e condições', href: '/agencia/privacidade' },
  { label: 'LGPD', href: '/agencia/privacidade' },
];
</script>

<style scoped>
.qs-ft { background: #111827; color: #9ca3af; padding: 54px 0 30px; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-ft__top { display: grid; grid-template-columns: 1.25fr 1fr; gap: 40px; align-items: start; margin-bottom: 34px; padding-bottom: 30px; border-bottom: 1px solid rgba(255,255,255,.08); }
.qs-ft__brand { display: inline-flex; text-decoration: none; }
.qs-ft__logo { height: 42px; width: auto; display: block; }
.qs-ft__seals { display: flex; gap: 14px; flex-wrap: wrap; margin-top: 18px; align-items: center; }
.qs-ft__seal { display: flex; align-items: center; gap: 7px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 12px; color: #9ca3af; border: 1px solid rgba(255,255,255,.1); border-radius: 999px; padding: 7px 13px; }
.qs-ft__seal b { color: #cbd5e1; font-weight: 700; }

.qs-ft__news-title { color: #fff; font-family: 'Bruum FY','Jost','Inter',sans-serif; font-weight: 700; font-size: 18px; }
.qs-ft__news-sub { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; margin-top: 4px; }
.qs-ft__news { display: flex; gap: 10px; max-width: 420px; margin-top: 14px; }
.qs-ft__news input { flex: 1; min-width: 0; background: rgba(255,255,255,.06); border: 1px solid rgba(255,255,255,.16); border-radius: 999px; padding: 11px 16px; color: #fff; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; outline: none; }
.qs-ft__news input::placeholder { color: #8b95a5; }
.qs-ft__news button { flex-shrink: 0; background: #98C73A; color: #173a0a; border: 0; border-radius: 999px; padding: 0 20px; min-height: 44px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-weight: 700; cursor: pointer; transition: background .2s ease; }
.qs-ft__news button:hover { background: #7aad1f; }

.qs-ft__row { display: flex; justify-content: space-between; gap: 30px; flex-wrap: wrap; align-items: center; }
.qs-ft__nav { display: flex; gap: 22px; flex-wrap: wrap; }
.qs-ft__nav a { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; color: #9ca3af; text-decoration: none; transition: color .2s ease; }
.qs-ft__nav a:hover { color: #fff; }
.qs-ft__soc { display: flex; gap: 12px; }
.qs-ft__soc a { width: 38px; height: 38px; border-radius: 50%; border: 1px solid rgba(255,255,255,.14); display: grid; place-items: center; color: #cbd5e1; transition: background .2s ease, border-color .2s ease, color .2s ease; }
.qs-ft__soc a:hover { background: #2F7785; border-color: #2F7785; color: #fff; }
.qs-ft__soc svg { width: 17px; height: 17px; fill: currentColor; }

.qs-ft__bottom { margin-top: 30px; padding-top: 22px; border-top: 1px solid rgba(255,255,255,.08); font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 13px; text-align: center; }

@media (max-width: 720px) { .qs-ft__top { grid-template-columns: 1fr; gap: 24px; } }
@media (prefers-reduced-motion: reduce) { .qs-ft__news button, .qs-ft__nav a, .qs-ft__soc a { transition: none; } }
</style>
