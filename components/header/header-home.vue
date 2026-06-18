<template>
  <header class="qs-hdr">
    <div class="container qs-hdr__row">
      <nuxt-link class="qs-hdr__brand" href="/" aria-label="Quanta Shop">
        <img class="qs-hdr__logo" src="/img/logo/quanta-logo.png" width="122" height="40" alt="Quanta Shop" decoding="async" />
      </nuxt-link>

      <nav class="qs-hdr__nav" aria-label="Principal">
        <nuxt-link v-for="(l, i) in links" :key="i" :href="l.href">{{ l.label }}</nuxt-link>
      </nav>

      <div class="qs-hdr__actions">
        <nuxt-link class="qs-hdr__login" href="/login">Login</nuxt-link>
        <nuxt-link class="qs-hdr__cad" href="/register">Cadastro</nuxt-link>
        <button
          class="qs-hdr__burger" :class="{ 'is-open': open }"
          type="button" :aria-label="open ? 'Fechar menu' : 'Abrir menu'"
          :aria-expanded="open" aria-controls="qs-hdr-mobile"
          @click="open = !open"
        >
          <span /><span /><span />
        </button>
      </div>
    </div>

    <div id="qs-hdr-mobile" class="qs-hdr__mobile" :class="{ 'is-show': open }">
      <nuxt-link v-for="(l, i) in links" :key="i" :href="l.href" @click="open = false">{{ l.label }}</nuxt-link>
      <div class="qs-hdr__mobile-actions">
        <nuxt-link class="qs-hdr__login" href="/login" @click="open = false">Login</nuxt-link>
        <nuxt-link class="qs-hdr__cad" href="/register" @click="open = false">Cadastro</nuxt-link>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const open = ref(false);

const links = [
  { label: 'Como funciona', href: '/como-funciona' },
  { label: 'Para Você', href: '/para-voce' },
  { label: 'Para sua Empresa', href: '/para-sua-empresa' },
  { label: 'Seja um Agente', href: '/seja-um-agente' },
  { label: 'Quanta Amizade', href: '/quanta-amizade' },
  { label: 'Blog', href: '/blog' },
  { label: 'Fale Conosco', href: '/contato' },
];
</script>

<style scoped>
.qs-hdr { position: sticky; top: 0; z-index: 1000; background: rgba(255,255,255,.82); backdrop-filter: saturate(160%) blur(14px); -webkit-backdrop-filter: saturate(160%) blur(14px); border-bottom: 1px solid rgba(17,24,39,.06); }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-hdr__row { display: flex; align-items: center; gap: 20px; height: 68px; }
.qs-hdr__brand { display: flex; align-items: center; text-decoration: none; }
.qs-hdr__logo { height: 40px; width: auto; display: block; }

.qs-hdr__nav { display: flex; gap: 4px; flex: 1; justify-content: center; }
.qs-hdr__nav a { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; font-weight: 500; color: #374151; padding: 8px 12px; border-radius: 6px; text-decoration: none; transition: color .2s ease, background .2s ease; }
.qs-hdr__nav a:hover { color: #2F7785; background: rgba(47,119,133,.07); }

.qs-hdr__actions { display: flex; align-items: center; gap: 10px; }
.qs-hdr__login { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; font-weight: 600; color: #2F7785; border: 1.5px solid #2F7785; border-radius: 999px; padding: 8px 18px; min-height: 40px; display: inline-flex; align-items: center; text-decoration: none; transition: background .2s ease, color .2s ease; }
.qs-hdr__login:hover { background: #2F7785; color: #fff; }
.qs-hdr__cad { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; font-weight: 700; color: #173a0a; background: #98C73A; border-radius: 999px; padding: 9px 20px; min-height: 40px; display: inline-flex; align-items: center; text-decoration: none; transition: background .2s ease; }
.qs-hdr__cad:hover { background: #7aad1f; }

.qs-hdr__burger { display: none; flex-direction: column; gap: 5px; background: none; border: 0; cursor: pointer; padding: 10px 8px; margin-left: 2px; min-width: 44px; min-height: 44px; align-items: center; justify-content: center; }
.qs-hdr__burger span { display: block; width: 24px; height: 2px; background: #374151; border-radius: 2px; transition: transform .25s ease, opacity .25s ease; }
.qs-hdr__burger.is-open span:nth-child(1) { transform: translateY(7px) rotate(45deg); }
.qs-hdr__burger.is-open span:nth-child(2) { opacity: 0; }
.qs-hdr__burger.is-open span:nth-child(3) { transform: translateY(-7px) rotate(-45deg); }

.qs-hdr__mobile { display: none; flex-direction: column; padding: 8px 24px 18px; background: #fff; border-bottom: 1px solid rgba(17,24,39,.06); }
.qs-hdr__mobile a { font-family: 'Kiye Sans','Inter','Jost',sans-serif; padding: 13px 4px; font-size: 15px; font-weight: 500; color: #374151; border-bottom: 1px solid #f5f5f7; text-decoration: none; min-height: 44px; display: flex; align-items: center; }
.qs-hdr__mobile a:hover { color: #2F7785; }
.qs-hdr__mobile-actions { display: flex; gap: 12px; margin-top: 14px; }
.qs-hdr__mobile-actions a { border-bottom: 0; justify-content: center; flex: 1; }

@media (max-width: 1024px) {
  .qs-hdr__nav { display: none; }
  .qs-hdr__burger { display: flex; }
  .qs-hdr__mobile.is-show { display: flex; }
}
@media (prefers-reduced-motion: reduce) {
  .qs-hdr__burger span, .qs-hdr__login, .qs-hdr__cad, .qs-hdr__nav a { transition: none; }
}
</style>
