<template>
  <header class="qs-hdr">
    <div class="container qs-hdr__row">
      <nuxt-link class="qs-hdr__brand" href="/" aria-label="Quanta Shop">
        <img class="qs-hdr__logo" src="/img/logo/quanta-logo.png" width="122" height="40" alt="Quanta Shop" decoding="async" />
      </nuxt-link>

      <nav class="qs-hdr__nav" aria-label="Principal">
        <div
          v-for="m in menus" :key="m.key"
          class="mm" :class="{ 'mm--right': m.align === 'right', 'is-open': openMenu === m.key }"
          @mouseenter="openMenu = m.key" @mouseleave="openMenu = null"
        >
          <button
            class="mm__btn" type="button"
            :aria-expanded="openMenu === m.key" aria-haspopup="true"
            @click="openMenu = openMenu === m.key ? null : m.key"
            @keydown.escape="openMenu = null"
          >
            {{ m.label }}
            <svg class="chev" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" aria-hidden="true"><path d="m6 9 6 6 6-6" /></svg>
          </button>

          <div class="mm__panel" @keydown.escape="openMenu = null">
            <div class="mm__grid" :class="m.mini ? 'mm__grid--mini' : 'mm__grid--feat'">
              <div v-if="m.feat" class="feat">
                <span class="feat__eyebrow">{{ m.feat.eyebrow }}</span>
                <p class="feat__title">{{ m.feat.title }}</p>
                <p class="feat__blurb">{{ m.feat.blurb }}</p>
                <nuxt-link class="feat__link" :href="m.feat.linkHref" @click="closeAll">{{ m.feat.linkLabel }} →</nuxt-link>
                <div class="feat__badge"><img src="/img/logo/quanta-icon.png" alt="" /><span>{{ m.feat.badge }}</span></div>
              </div>
              <div v-for="(col, ci) in m.cols" :key="ci">
                <div v-if="col.h" class="col__h">{{ col.h }}</div>
                <nuxt-link v-for="(l, li) in col.links" :key="li" class="mlink" :href="l.href" @click="closeAll">
                  <b>{{ l.label }} <span class="ar">→</span></b>
                  <small>{{ l.desc }}</small>
                </nuxt-link>
              </div>
            </div>
          </div>
        </div>
      </nav>

      <div class="qs-hdr__actions">
        <nuxt-link class="qs-hdr__wa" href="/contato">
          <svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M12 2a10 10 0 0 0-8.6 15l-1.3 4.8 4.9-1.3A10 10 0 1 0 12 2Zm5.8 14.2c-.2.7-1.4 1.3-2 1.4-.5.1-1.2.1-1.9-.1-.4-.1-1-.3-1.7-.6-3-1.3-4.9-4.3-5-4.5-.2-.2-1.2-1.6-1.2-3s.7-2.1 1-2.4c.2-.3.5-.3.7-.3h.5c.2 0 .4 0 .6.5l.8 2c.1.2.1.4 0 .5l-.4.5-.3.3c-.2.2-.3.4-.1.7.2.3.9 1.4 1.9 2.3 1.3 1.1 2.3 1.5 2.6 1.6.3.1.5.1.7-.1l.8-1c.2-.2.4-.2.6-.1l2 .9c.2.1.4.2.4.3.1.2.1.6-.1 1.2Z" /></svg>
          Atendimento
        </nuxt-link>
        <nuxt-link class="qs-hdr__login" href="/login">Login</nuxt-link>
        <nuxt-link class="qs-hdr__cad" href="/register">Cadastro</nuxt-link>
        <button
          class="qs-hdr__burger" :class="{ 'is-open': mobileOpen }"
          type="button" :aria-label="mobileOpen ? 'Fechar menu' : 'Abrir menu'"
          :aria-expanded="mobileOpen" aria-controls="qs-hdr-mobile"
          @click="mobileOpen = !mobileOpen"
        ><span /><span /><span /></button>
      </div>
    </div>

    <!-- MOBILE: acordeão -->
    <div id="qs-hdr-mobile" class="qs-hdr__mobile" :class="{ 'is-show': mobileOpen }">
      <div v-for="m in menus" :key="m.key" class="acc">
        <button
          class="acc__btn" type="button"
          :aria-expanded="mobileSection === m.key"
          @click="mobileSection = mobileSection === m.key ? null : m.key"
        >
          {{ m.label }}
          <svg class="chev" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" aria-hidden="true"><path d="m6 9 6 6 6-6" /></svg>
        </button>
        <div class="acc__body" :class="{ 'is-open': mobileSection === m.key }">
          <template v-for="(col, ci) in m.cols" :key="ci">
            <nuxt-link v-for="(l, li) in col.links" :key="ci + '-' + li" class="acc__link" :href="l.href" @click="closeAll">{{ l.label }}</nuxt-link>
          </template>
        </div>
      </div>

      <div class="qs-hdr__mobile-actions">
        <nuxt-link class="qs-hdr__login" href="/login" @click="closeAll">Login</nuxt-link>
        <nuxt-link class="qs-hdr__cad" href="/register" @click="closeAll">Cadastro</nuxt-link>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue';

interface MLink { label: string; href: string; desc?: string }
interface MCol { h?: string; links: MLink[] }
interface MFeat { eyebrow: string; title: string; blurb: string; linkLabel: string; linkHref: string; badge: string }
interface Menu { key: string; label: string; align?: 'right'; mini?: boolean; feat?: MFeat; cols: MCol[] }

const openMenu = ref<string | null>(null);
const mobileOpen = ref(false);
const mobileSection = ref<string | null>(null);

function closeAll() {
  openMenu.value = null;
  mobileOpen.value = false;
  mobileSection.value = null;
}
function onDocClick(e: MouseEvent) {
  if (!(e.target as HTMLElement)?.closest('.qs-hdr__nav')) openMenu.value = null;
}
onMounted(() => document.addEventListener('click', onDocClick));
onBeforeUnmount(() => document.removeEventListener('click', onDocClick));

const menus: Menu[] = [
  {
    key: 'quem', label: 'Para quem é',
    feat: { eyebrow: 'Para quem é', title: 'Todo mundo ganha quando a rede cresce.', blurb: 'Consumidor, lojista ou empreendedor — o efeito de rede trabalha a seu favor.', linkLabel: 'Ver o plano de compensação', linkHref: '/como-funciona', badge: 'Consumo inteligente em rede' },
    cols: [{ h: 'Perfis', links: [
      { label: 'Para Você', href: '/para-voce', desc: 'Cashback de verdade em cada compra' },
      { label: 'Para sua Empresa', href: '/para-sua-empresa', desc: 'Fidelize clientes e venda mais' },
      { label: 'Seja um Agente', href: '/seja-um-agente', desc: 'Renda recorrente, sem estoque nem capital' },
    ] }],
  },
  {
    key: 'plataforma', label: 'A plataforma',
    feat: { eyebrow: 'A plataforma', title: 'Consumo inteligente com cashback em rede.', blurb: 'Você compra o que já compraria — e recebe dinheiro de volta, sacável via PIX.', linkLabel: 'Como funciona', linkHref: '/como-funciona', badge: 'Tudo num só lugar' },
    cols: [{ h: 'Como funciona', links: [
      { label: 'Como funciona', href: '/como-funciona', desc: 'Do cadastro ao PIX em 3 passos' },
      { label: 'Quanta Flow', href: '/quanta-flow', desc: 'O fluxo do cashback em rede' },
      { label: 'Quanta IA', href: '/quanta-ia', desc: 'Busca, cota e responde no chat' },
    ] }],
  },
  {
    key: 'beneficios', label: 'Benefícios',
    feat: { eyebrow: 'Benefícios', title: 'Quanto mais a rede usa, mais você ganha.', blurb: 'Ganho coletivo e escalável: seu sucesso impulsiona o de quem está na sua rede.', linkLabel: 'Ver todos os ganhos', linkHref: '/como-funciona', badge: 'Distribuição equitativa' },
    cols: [{ h: 'Seus ganhos', links: [
      { label: 'Cashback', href: '/para-voce', desc: 'Dinheiro de volta em cada compra' },
      { label: 'Cashback Residual', href: '/cashback-residual', desc: 'Ganhe quando sua rede consome' },
      { label: 'Quanta Amizade', href: '/quanta-amizade', desc: 'R$ 25 por amigo que você indica' },
      { label: 'Cupons do dia', href: '/cupons', desc: 'Ofertas com cashback turbinado' },
    ] }],
  },
  {
    key: 'conteudo', label: 'Conteúdo', align: 'right', mini: true,
    cols: [{ links: [
      { label: 'Blog', href: '/blog', desc: 'Dicas de consumo e renda' },
      { label: 'Tese do CEO', href: '/manifesto', desc: 'O manifesto de Mauro Triumph' },
      { label: 'Perguntas frequentes', href: '/#faq', desc: 'Tire suas dúvidas' },
    ] }],
  },
];
</script>

<style scoped>
.qs-hdr { position: sticky; top: 0; z-index: 1000; background: rgba(255,255,255,.85); backdrop-filter: saturate(160%) blur(14px); -webkit-backdrop-filter: saturate(160%) blur(14px); border-bottom: 1px solid rgba(17,24,39,.08); }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-hdr__row { display: flex; align-items: center; gap: 18px; height: 70px; }
.qs-hdr__brand { display: flex; align-items: center; text-decoration: none; }
.qs-hdr__logo { height: 38px; width: auto; display: block; }

.qs-hdr__nav { display: flex; gap: 2px; flex: 1; justify-content: center; }
.mm { position: relative; }
.mm__btn { display: inline-flex; align-items: center; gap: 6px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14.5px; font-weight: 600; color: #374151; padding: 10px 14px; border: 0; background: none; border-radius: 8px; cursor: pointer; transition: color .2s ease, background .2s ease; }
.mm__btn .chev { width: 14px; height: 14px; transition: transform .25s ease; color: #9aa3af; }
.mm__btn:hover { color: #2F7785; background: rgba(47,119,133,.06); }
.mm.is-open .mm__btn { color: #2F7785; }
.mm.is-open .chev { transform: rotate(180deg); color: #2F7785; }

.mm__panel { position: absolute; top: calc(100% + 14px); left: 50%; transform: translateX(-50%) translateY(8px); background: #fff; border: 1px solid rgba(17,24,39,.08); border-radius: 18px; box-shadow: 0 24px 60px rgba(1,15,28,.16); padding: 26px; opacity: 0; visibility: hidden; pointer-events: none; transition: opacity .22s ease, transform .22s ease; z-index: 50; }
.mm__panel::before { content: ""; position: absolute; top: -14px; left: 0; right: 0; height: 14px; }
.mm.is-open .mm__panel { opacity: 1; visibility: visible; pointer-events: auto; transform: translateX(-50%) translateY(0); }
.mm--right .mm__panel { left: auto; right: 0; transform: translateX(0) translateY(8px); }
.mm--right.is-open .mm__panel { transform: translateX(0) translateY(0); }

.mm__grid { display: grid; gap: 30px; }
.mm__grid--feat { grid-template-columns: 248px 1fr; width: 620px; }
.mm__grid--mini { width: 240px; }

.feat { padding-right: 26px; border-right: 1px solid rgba(17,24,39,.08); display: flex; flex-direction: column; }
.feat__eyebrow { font-size: 11px; font-weight: 800; letter-spacing: .16em; text-transform: uppercase; color: #3A9AAD; }
.feat__title { font-family: 'Jost','Inter',sans-serif; font-size: 19px; font-weight: 700; color: #225F6B; line-height: 1.18; margin: 8px 0; }
.feat__blurb { font-size: 13px; color: #6b7280; line-height: 1.55; }
.feat__link { margin-top: 14px; font-family: 'Kiye Sans','Inter',sans-serif; font-weight: 700; font-size: 13.5px; color: #2F7785; border-bottom: 1px solid rgba(47,119,133,.3); align-self: flex-start; padding-bottom: 2px; text-decoration: none; transition: color .2s ease, border-color .2s ease; }
.feat__link:hover { color: #225F6B; border-color: #2F7785; }
.feat__badge { margin-top: auto; padding-top: 18px; display: flex; align-items: center; gap: 9px; }
.feat__badge img { height: 30px; width: auto; }
.feat__badge span { font-size: 11px; color: #9aa3af; font-weight: 600; }

.col__h { font-size: 11px; font-weight: 800; letter-spacing: .14em; text-transform: uppercase; color: #9aa3af; margin-bottom: 6px; padding-left: 10px; }
.mlink { display: block; padding: 9px 10px; border-radius: 10px; text-decoration: none; transition: background .18s ease; }
.mlink:hover { background: rgba(47,119,133,.06); }
.mlink b { display: flex; align-items: center; gap: 7px; font-family: 'Kiye Sans','Inter',sans-serif; font-size: 14.5px; font-weight: 600; color: #1f2937; }
.mlink:hover b { color: #2F7785; }
.mlink .ar { opacity: 0; transform: translateX(-4px); transition: opacity .18s ease, transform .18s ease; color: #2F7785; font-weight: 700; }
.mlink:hover .ar { opacity: 1; transform: translateX(0); }
.mlink small { display: block; font-size: 12px; color: #6b7280; margin-top: 2px; }

.qs-hdr__actions { display: flex; align-items: center; gap: 12px; }
.qs-hdr__wa { display: inline-flex; align-items: center; gap: 7px; font-family: 'Kiye Sans','Inter',sans-serif; font-size: 14px; font-weight: 700; color: #7aad1f; text-decoration: none; }
.qs-hdr__wa svg { width: 18px; height: 18px; }
.qs-hdr__login { font-family: 'Kiye Sans','Inter',sans-serif; font-size: 14px; font-weight: 600; color: #2F7785; border: 1.5px solid #2F7785; border-radius: 999px; padding: 8px 18px; min-height: 40px; display: inline-flex; align-items: center; text-decoration: none; transition: background .2s ease, color .2s ease; }
.qs-hdr__login:hover { background: #2F7785; color: #fff; }
.qs-hdr__cad { font-family: 'Kiye Sans','Inter',sans-serif; font-size: 14px; font-weight: 700; color: #173a0a; background: #98C73A; border-radius: 999px; padding: 9px 20px; min-height: 40px; display: inline-flex; align-items: center; text-decoration: none; transition: background .2s ease; }
.qs-hdr__cad:hover { background: #7aad1f; }

.qs-hdr__burger { display: none; flex-direction: column; gap: 5px; background: none; border: 0; cursor: pointer; padding: 10px 8px; min-width: 44px; min-height: 44px; align-items: center; justify-content: center; }
.qs-hdr__burger span { display: block; width: 24px; height: 2px; background: #374151; border-radius: 2px; transition: transform .25s ease, opacity .25s ease; }
.qs-hdr__burger.is-open span:nth-child(1) { transform: translateY(7px) rotate(45deg); }
.qs-hdr__burger.is-open span:nth-child(2) { opacity: 0; }
.qs-hdr__burger.is-open span:nth-child(3) { transform: translateY(-7px) rotate(-45deg); }

.qs-hdr__mobile { display: none; flex-direction: column; padding: 6px 24px 18px; background: #fff; border-bottom: 1px solid rgba(17,24,39,.06); }
.acc { border-bottom: 1px solid #f3f4f6; }
.acc__btn { width: 100%; display: flex; align-items: center; justify-content: space-between; background: none; border: 0; cursor: pointer; padding: 14px 4px; font-family: 'Kiye Sans','Inter',sans-serif; font-size: 15px; font-weight: 600; color: #225F6B; }
.acc__btn .chev { width: 16px; height: 16px; color: #9aa3af; transition: transform .25s ease; }
.acc__btn[aria-expanded="true"] .chev { transform: rotate(180deg); }
.acc__body { display: none; flex-direction: column; padding: 0 4px 10px; }
.acc__body.is-open { display: flex; }
.acc__link { padding: 10px 12px; font-family: 'Kiye Sans','Inter',sans-serif; font-size: 14.5px; color: #374151; text-decoration: none; border-radius: 8px; }
.acc__link:hover { background: rgba(47,119,133,.06); color: #2F7785; }
.qs-hdr__mobile-actions { display: flex; gap: 12px; margin-top: 16px; }
.qs-hdr__mobile-actions a { justify-content: center; flex: 1; }

@media (max-width: 1024px) {
  .qs-hdr__nav { display: none; }
  .qs-hdr__wa { display: none; }
  .qs-hdr__login, .qs-hdr__cad { display: none; }
  .qs-hdr__burger { display: flex; }
  .qs-hdr__mobile.is-show { display: flex; }
  .qs-hdr__mobile-actions a { display: inline-flex; }
}
@media (prefers-reduced-motion: reduce) {
  .qs-hdr__burger span, .qs-hdr__login, .qs-hdr__cad, .mm__btn, .mm__panel, .chev, .mlink, .mlink .ar { transition: none; }
}
</style>
