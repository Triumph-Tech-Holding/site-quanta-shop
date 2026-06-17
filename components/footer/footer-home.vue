<template>
  <footer class="qs-footer">

    <!-- Newsletter + Selos -->
    <div class="qs-footer__upper">
      <div class="container">
        <div class="qs-footer__upper-row">
          <div class="qs-footer__brand-col">
            <nuxt-link class="qs-footer__brand" href="/" aria-label="Quanta Shop">
              <img class="qs-footer__logo-top" src="/img/logo/logo-white.png" width="128" height="42" alt="Quanta Shop" decoding="async" />
            </nuxt-link>
            <div class="qs-footer__seals">
              <span class="qs-footer__seal"><b>PIX</b> saque instantâneo</span>
              <span class="qs-footer__seal"><b>LGPD</b> dados protegidos</span>
              <span class="qs-footer__seal"><b>SSL</b> site seguro</span>
            </div>
          </div>
          <div class="qs-footer__news-col">
            <p class="qs-footer__news-title">Receba ofertas e cashback turbinado</p>
            <p class="qs-footer__news-sub">Novidades e promoções no seu e-mail. Sem spam.</p>
            <form class="qs-footer__news-form" @submit.prevent="subscribe">
              <input v-model="newsletterEmail" type="email" aria-label="Seu e-mail" placeholder="seu@email.com" />
              <button type="submit">Assinar</button>
            </form>
          </div>
        </div>
      </div>
    </div>

    <div class="qs-footer__top">
      <div class="container">
        <div class="qs-footer__grid">
          <div class="qs-footer__col qs-footer__col--brand">
            <nuxt-link href="/">
              <img src="/img/logo/logo-white.png" alt="Quanta Shop" class="qs-footer__logo" />
            </nuxt-link>
            <p class="qs-footer__desc">
              Somos uma equipe dedicada a oferecer as melhores oportunidades de economia e renda através de um consumo inteligente.
            </p>
            <div class="qs-footer__socials">
              <a href="https://www.instagram.com/quantashop.oficial/" target="_blank" rel="noopener" aria-label="Instagram" class="qs-footer__social">
                <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z"/></svg>
              </a>
              <a href="https://www.facebook.com/quantashop" target="_blank" rel="noopener" aria-label="Facebook" class="qs-footer__social">
                <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z"/></svg>
              </a>
              <a href="https://www.linkedin.com/company/quanta-shop" target="_blank" rel="noopener" aria-label="LinkedIn" class="qs-footer__social">
                <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433a2.062 2.062 0 01-2.063-2.065 2.064 2.064 0 112.063 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z"/></svg>
              </a>
              <a href="https://www.youtube.com/@QuantaShop" target="_blank" rel="noopener" aria-label="YouTube" class="qs-footer__social">
                <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/></svg>
              </a>
            </div>
          </div>

          <div class="qs-footer__col">
            <h4 class="qs-footer__heading">Minha Conta</h4>
            <ul class="qs-footer__links">
              <li><nuxt-link to="/agencia/painel/minhas-compras">Minhas compras</nuxt-link></li>
              <li><nuxt-link to="/agencia/painel/financeiro">Meu extrato</nuxt-link></li>
              <li><nuxt-link to="/agencia/painel/minha-rede">Meus indicados</nuxt-link></li>
              <li><nuxt-link to="/agencia/painel/meus-dados">Minha conta</nuxt-link></li>
              <li><nuxt-link to="/agencia/painel/assinatura">Assinatura</nuxt-link></li>
            </ul>
          </div>

          <div class="qs-footer__col">
            <h4 class="qs-footer__heading">Empresa</h4>
            <ul class="qs-footer__links">
              <li><a href="/about">Quem somos</a></li>
              <li><nuxt-link to="/agencia/como-funciona">Como funciona</nuxt-link></li>
              <li><nuxt-link to="/agencia/cadastro">Credenciamento</nuxt-link></li>
              <li><nuxt-link to="/primeira-compra">Seja um Agente</nuxt-link></li>
              <li><a href="/contact">Contato</a></li>
            </ul>
          </div>

          <div class="qs-footer__col">
            <h4 class="qs-footer__heading">Suporte</h4>
            <ul class="qs-footer__links">
              <li><a href="https://bigcash.blob.core.windows.net/documentos/QB%20-%20TERMOS%20DE%20USO%20E%20BONIFICA%C3%87%C3%83O%20ESPEC%C3%8DFICA%20v102023.pdf" target="_blank">Termos e condições</a></li>
              <li><a href="/blog">Blog</a></li>
              <li><a href="https://api.whatsapp.com/send/?phone=552140404866" target="_blank">WhatsApp</a></li>
              <li><nuxt-link to="/agencia/painel/assinatura">Quanta Plus</nuxt-link></li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="qs-footer__bottom">
      <div class="container">
        <div class="qs-footer__bottom-row">
          <p>© {{ new Date().getFullYear() }} Quanta Shop — Todos os direitos reservados</p>
          <img src="/img/footer/footer-pay.png" alt="Métodos de pagamento" class="qs-footer__pay" />
        </div>
      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
import { ref } from 'vue';
const newsletterEmail = ref('');
function subscribe() {
  newsletterEmail.value = '';
}
</script>

<style scoped>
/* ── Newsletter + Selos ── */
.qs-footer__upper { background: #0d1520; border-bottom: 1px solid rgba(255,255,255,.07); padding: 44px 0; }
.qs-footer__upper-row { display: grid; grid-template-columns: 1.25fr 1fr; gap: 40px; align-items: start; }
@media (max-width: 720px) { .qs-footer__upper-row { grid-template-columns: 1fr; gap: 24px; } }

.qs-footer__brand { display: inline-flex; text-decoration: none; }
.qs-footer__logo-top { height: 40px; width: auto; display: block; }

.qs-footer__seals { display: flex; gap: 10px; flex-wrap: wrap; margin-top: 16px; }
.qs-footer__seal { display: flex; align-items: center; gap: 6px; font-family: 'Inter','Jost',sans-serif; font-size: 12px; color: #9ca3af; border: 1px solid rgba(255,255,255,.1); border-radius: 999px; padding: 6px 12px; }
.qs-footer__seal b { color: #cbd5e1; font-weight: 700; }

.qs-footer__news-title { color: #fff; font-family: 'Jost','Inter',sans-serif; font-weight: 700; font-size: 17px; margin: 0 0 4px; }
.qs-footer__news-sub { font-family: 'Inter','Jost',sans-serif; font-size: 13px; color: #9ca3af; margin: 0 0 14px; }
.qs-footer__news-form { display: flex; gap: 10px; max-width: 420px; }
.qs-footer__news-form input { flex: 1; min-width: 0; background: rgba(255,255,255,.06); border: 1px solid rgba(255,255,255,.16); border-radius: 999px; padding: 10px 16px; color: #fff; font-family: 'Inter','Jost',sans-serif; font-size: 14px; outline: none; }
.qs-footer__news-form input::placeholder { color: #8b95a5; }
.qs-footer__news-form button { flex-shrink: 0; background: #98C73A; color: #173a0a; border: 0; border-radius: 999px; padding: 0 20px; min-height: 42px; font-family: 'Inter','Jost',sans-serif; font-weight: 700; font-size: 13px; cursor: pointer; transition: background .2s; }
.qs-footer__news-form button:hover { background: #7aad1f; }
@media (prefers-reduced-motion: reduce) { .qs-footer__news-form button { transition: none; } }

/* ── Base ── */
.qs-footer {
  background: #111827;
  color: #d1d5db;
}

.qs-footer__top {
  padding: 64px 0 40px;
}

.qs-footer__grid {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr 1fr;
  gap: 40px;
}

@media (max-width: 991px) { .qs-footer__grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 575px) { .qs-footer__grid { grid-template-columns: 1fr; } }

.qs-footer__logo {
  height: 36px;
  width: auto;
  margin-bottom: 16px;
  display: block;
}

.qs-footer__desc {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  color: #9ca3af;
  line-height: 1.6;
  margin-bottom: 20px;
}

.qs-footer__socials {
  display: flex;
  gap: 10px;
}

.qs-footer__social {
  width: 36px;
  height: 36px;
  border-radius: 8px;
  background: rgba(255,255,255,0.07);
  color: #9ca3af;
  display: flex;
  align-items: center;
  justify-content: center;
  text-decoration: none;
  transition: all 0.2s;
}

.qs-footer__social:hover {
  background: #2F7785;
  color: #fff;
}

.qs-footer__heading {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 700;
  color: #fff;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  margin-bottom: 16px;
}

.qs-footer__links {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.qs-footer__links a {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  color: #9ca3af;
  text-decoration: none;
  transition: color 0.2s;
}

.qs-footer__links a:hover {
  color: #98C73A;
}

.qs-footer__bottom {
  border-top: 1px solid rgba(255,255,255,0.06);
  padding: 20px 0;
}

.qs-footer__bottom-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 12px;
}

.qs-footer__bottom-row p {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  color: #6b7280;
  margin: 0;
}

.qs-footer__pay {
  height: 24px;
  width: auto;
  opacity: 0.6;
}
</style>
