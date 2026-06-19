<template>
  <header class="qs-hdr">
    <div class="container qs-hdr__row">
      <nuxt-link class="qs-hdr__brand" href="/" aria-label="Quanta Shop">
        <img class="qs-hdr__logo" src="/img/logo/quanta-logo.png" width="122" height="40" alt="Quanta Shop" decoding="async" />
      </nuxt-link>

      <nav class="qs-hdr__nav" aria-label="Principal">
        <nuxt-link v-for="(l, i) in links" :key="i" :href="l.href" :class="{ 'qs-hdr__nav-wa': l.wa }">
          <svg v-if="l.wa" class="qs-hdr__wa-ic" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">
            <path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/>
          </svg>
          {{ l.label }}
        </nuxt-link>
      </nav>

      <div class="qs-hdr__actions">
        <nuxt-link class="qs-hdr__login" href="/login">Login</nuxt-link>
        <nuxt-link class="qs-hdr__cad" href="/agencia/cadastro">Cadastro</nuxt-link>
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
      <nuxt-link v-for="(l, i) in links" :key="i" :href="l.href" :class="{ 'qs-hdr__mobile-wa': l.wa }" @click="open = false">
        <svg v-if="l.wa" class="qs-hdr__wa-ic" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">
          <path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/>
        </svg>
        {{ l.label }}
      </nuxt-link>
      <div class="qs-hdr__mobile-actions">
        <nuxt-link class="qs-hdr__login" href="/login" @click="open = false">Login</nuxt-link>
        <nuxt-link class="qs-hdr__cad" href="/agencia/cadastro" @click="open = false">Cadastro</nuxt-link>
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
  { label: 'Cupons', href: '/cupons' },
  { label: 'Seja um Agente', href: '/seja-um-agente' },
  { label: 'Quanta Amizade', href: '/quanta-amizade' },
  { label: 'Blog', href: '/blog' },
  { label: 'Atendimento', href: '/contato', wa: true },
];
</script>

<style scoped>
.qs-hdr { position: sticky; top: 0; z-index: 1000; background: rgba(255,255,255,.82); backdrop-filter: saturate(160%) blur(14px); -webkit-backdrop-filter: saturate(160%) blur(14px); border-bottom: 1px solid rgba(17,24,39,.06); }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-hdr__row { display: flex; align-items: center; gap: 20px; height: 68px; }
.qs-hdr__brand { display: flex; align-items: center; text-decoration: none; }
.qs-hdr__logo { height: 40px; width: auto; display: block; }

.qs-hdr__nav { display: flex; gap: 4px; flex: 1; justify-content: center; }
.qs-hdr__nav a { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; font-weight: 500; color: #374151; padding: 8px 12px; border-radius: 6px; text-decoration: none; transition: color .2s ease, background .2s ease; display: inline-flex; align-items: center; gap: 6px; }
.qs-hdr__nav a:hover { color: #2F7785; background: rgba(47,119,133,.07); }
.qs-hdr__nav-wa { color: #25D366 !important; }
.qs-hdr__nav-wa:hover { color: #1aab52 !important; background: rgba(37,211,102,.08) !important; }

.qs-hdr__wa-ic { width: 15px; height: 15px; flex-shrink: 0; }

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
.qs-hdr__mobile a { font-family: 'Kiye Sans','Inter','Jost',sans-serif; padding: 13px 4px; font-size: 15px; font-weight: 500; color: #374151; border-bottom: 1px solid #f5f5f7; text-decoration: none; min-height: 44px; display: flex; align-items: center; gap: 8px; }
.qs-hdr__mobile a:hover { color: #2F7785; }
.qs-hdr__mobile-wa { color: #25D366 !important; }
.qs-hdr__mobile-wa .qs-hdr__wa-ic { width: 18px; height: 18px; }
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
