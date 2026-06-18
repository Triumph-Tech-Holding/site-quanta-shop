<template>
  <section ref="heroEl" class="qs-hero" aria-label="Destaque — Quanta IA" @mouseenter="pauseRotate" @mouseleave="resumeRotate" @focusin="pauseRotate" @focusout="resumeRotate">
    <span class="qs-hero__orb qs-hero__orb--1" aria-hidden="true"></span>
    <span class="qs-hero__orb qs-hero__orb--2" aria-hidden="true"></span>
    <QsLogo tone="white" :size="540" class="qs-hero__wm" aria-hidden="true" />

    <div class="container qs-hero__grid">
      <div class="qs-hero__copy">
        <div class="qs-hero__rotate" :class="{ 'is-fading': fading }">
          <span class="qs-hero__badge"><span class="qs-hero__dot" aria-hidden="true"></span> {{ active.badge }}</span>
          <h1 class="qs-hero__title" v-html="activeTitle" />
          <p class="qs-hero__sub">{{ active.sub }}</p>
          <nuxt-link class="qs-hero__cta" :href="active.href">{{ active.cta }} →</nuxt-link>
        </div>

        <div class="qs-hero__dots" role="tablist" aria-label="Trocar banner">
          <button
            v-for="(b, i) in banners" :key="i" type="button" role="tab"
            class="qs-hero__dot-btn" :class="{ active: i === index }"
            :aria-selected="i === index" :aria-label="'Banner ' + (i + 1)"
            @click="go(i, true)"
          />
        </div>

        <p class="qs-hero__composer-label">ou pergunte à <strong>Quanta IA</strong> — busque, cote, consulte saldo</p>

        <form class="qs-hero__composer" role="search" @submit.prevent="submitQuery">
          <span class="qs-hero__ai" aria-hidden="true">
            <QsLogo tone="color" :size="20" />
            Quanta IA
          </span>
          <input
            ref="composerInput"
            v-model="query" type="text" class="qs-hero__input"
            aria-label="Buscar produtos, lojas ou perguntar à Quanta IA"
            :placeholder="placeholder"
          />
          <button type="submit" class="qs-hero__send" aria-label="Enviar para a Quanta IA">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2">
              <path d="M22 2 11 13" /><path d="M22 2 15 22l-4-9-9-4z" />
            </svg>
          </button>
        </form>

        <div class="qs-hero__chips" aria-label="Sugestões de busca">
          <button v-for="(c, i) in chips" :key="i" type="button" class="qs-hero__chip" @click="useChip(c.q)">{{ c.label }}</button>
        </div>
      </div>

      <div class="qs-hero__chat" aria-label="Demonstração da Quanta IA">
        <div class="qs-hero__chat-top">
          <span class="qs-hero__chat-av" aria-hidden="true"><QsLogo tone="white" :size="26" /></span>
          <div>
            <p class="qs-hero__chat-nm">Quanta IA</p>
            <p class="qs-hero__chat-st"><i aria-hidden="true"></i> online · responde na hora</p>
          </div>
        </div>
        <div ref="chatBody" class="qs-hero__chat-body">
          <template v-for="(m, i) in visible" :key="i">
            <div v-if="m.typing" class="qs-hero__typing"><span /><span /><span /></div>
            <div v-else class="qs-hero__msg" :class="'qs-hero__msg--' + m.who" v-html="m.html" />
          </template>
        </div>
        <button type="button" class="qs-hero__chat-cta" @click="focusComposer">
          Converse você mesmo com a Quanta IA
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" aria-hidden="true"><path d="M5 12h14" /><path d="M13 6l6 6-6 6" /></svg>
        </button>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onBeforeUnmount, nextTick } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';

const router = useRouter();
const { config, loadConfig } = useHomeConfig();

const reduceMotion = ref(false);
const heroEl = ref<HTMLElement | null>(null);
const composerInput = ref<HTMLInputElement | null>(null);
let visObserver: IntersectionObserver | null = null;
let offscreen = false;

interface Banner { badge: string; title: string; sub: string; cta: string; href: string }
const DEFAULT_BANNERS: Banner[] = [
  { badge: '+12.000 usuários economizando', title: 'Seu dinheiro <highlight>volta</highlight> a cada compra', sub: 'Compre nas suas lojas favoritas e receba cashback de verdade. Simples, transparente e instantâneo.', cta: 'Criar Conta Grátis', href: '/register' },
  { badge: 'Simples e rápido', title: 'Transforme cada compra em <highlight>dinheiro de volta</highlight>', sub: 'Ative o cashback com um clique, compre normalmente e veja o saldo crescer automaticamente.', cta: 'Começar Agora', href: '/register' },
  { badge: 'Rede exclusiva', title: 'As melhores marcas em <highlight>um só lugar</highlight>', sub: 'Nike, Renner, Puma, Casas Bahia e centenas de outras marcas com cashback garantido.', cta: 'Ver Marcas Parceiras', href: '/partners' },
  { badge: 'Programa de pontos', title: 'Ganhe pontos, <highlight>resgate benefícios</highlight>', sub: 'Cada compra acumula pontos que viram cashback extra, descontos e experiências exclusivas.', cta: 'Conhecer Benefícios', href: '/quanta-amizade' },
  { badge: 'Quanta Plus ✦', title: 'Ative sua experiência <highlight>Premium</highlight>', sub: 'Cashback turbinado, ofertas antecipadas e benefícios exclusivos para assinantes Quanta Plus.', cta: 'Assinar Quanta Plus', href: '/planos' },
  { badge: 'Para lojistas', title: 'Aumente vendas com <highlight>tecnologia Quanta</highlight>', sub: 'Fidelize clientes, aumente o ticket médio e ganhe visibilidade na maior rede de cashback do Brasil.', cta: 'Credenciar Minha Loja', href: '/credenciar' },
];
const banners = computed<Banner[]>(() => {
  const cms = (config.value as any)?.heroBanners;
  return Array.isArray(cms) && cms.length ? cms : DEFAULT_BANNERS;
});

const index = ref(0);
const fading = ref(false);
const active = computed(() => banners.value[index.value]);
const activeTitle = computed(() =>
  active.value.title.replace(/<highlight>/g, '<span class="qs-hero__hl">').replace(/<\/highlight>/g, '</span>'),
);
let rotTimer: ReturnType<typeof setInterval> | null = null;
function go(i: number, user = false) {
  fading.value = true;
  setTimeout(() => { index.value = i; fading.value = false; }, 420);
  if (user) restart();
}
function restart() {
  if (rotTimer) clearInterval(rotTimer);
  if (reduceMotion.value || offscreen) return;
  rotTimer = setInterval(() => go((index.value + 1) % banners.value.length), 5600);
}
function pauseRotate() { if (rotTimer) { clearInterval(rotTimer); rotTimer = null; } }
function resumeRotate() { restart(); }

const placeholder = 'Ex.: tênis de corrida até R$400 com mais cashback…';
const chips = [
  { label: '⌚ Smartwatch com + cashback', q: 'Quero um smartwatch com o maior cashback' },
  { label: '💰 Meu saldo', q: 'Qual é o meu saldo?' },
  { label: '📍 Lojas no meu bairro', q: 'Tem padaria com cashback perto de mim?' },
];
const query = ref('');
function submitQuery() {
  const v = query.value.trim();
  if (v) router.push(`/partners?nome=${encodeURIComponent(v)}`);
}
function useChip(q: string) { query.value = q; composerInput.value?.focus(); }
function focusComposer() {
  composerInput.value?.focus();
  composerInput.value?.scrollIntoView({ behavior: reduceMotion.value ? 'auto' : 'smooth', block: 'center' });
}

type Step = { who?: 'user' | 'bot'; html?: string; typing?: number };
const SCRIPT: Step[] = [
  { who: 'user', html: 'Quero um tênis de corrida até R$400 com mais cashback 👟' },
  { typing: 1100 },
  { who: 'bot', html:
      '<span class="qs-hero__lead">Achei a melhor relação cashback perto de você:</span>' +
      '<div class="qs-hero__pcard"><img src="https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=160&q=80&auto=format&fit=crop" alt="Tênis de corrida" loading="lazy" />' +
      '<div><div class="qs-hero__pn">Nike Revolution 7</div><div class="qs-hero__pmeta">Casas Bahia · entrega 2 dias</div>' +
      '<div class="qs-hero__price">R$ 389,90</div><span class="qs-hero__cb">12% cashback ≈ R$ 46,79 de volta</span></div></div>' },
  { who: 'user', html: 'Qual é o meu saldo?' },
  { typing: 900 },
  { who: 'bot', html:
      '<span class="qs-hero__lead">Sua carteira Quanta:</span>' +
      '<div class="qs-hero__scard"><div class="qs-hero__srow"><span class="qs-hero__lbl">Disponível</span><span class="qs-hero__lbl">Reservado</span></div>' +
      '<div class="qs-hero__srow"><span class="qs-hero__big">R$ 247,30</span><span class="qs-hero__ssub">R$ 88,10 a liberar</span></div>' +
      '<div class="qs-hero__ssub" style="margin-top:8px">💸 Resgate via PIX em segundos</div></div>' },
  { who: 'user', html: 'Tem padaria com cashback aqui no bairro?' },
  { typing: 900 },
  { who: 'bot', html:
      '<span class="qs-hero__lead">Pertinho de você:</span>' +
      '<div class="qs-hero__pcard"><img src="https://images.unsplash.com/photo-1509440159596-0249088772ff?w=160&q=80&auto=format&fit=crop" alt="Padaria" loading="lazy" />' +
      '<div><div class="qs-hero__pn">Padaria Real</div><div class="qs-hero__pmeta">Vila Mariana · 600 m</div>' +
      '<span class="qs-hero__cb">5% cashback · ativa no WhatsApp</span></div></div>' },
];
const visible = ref<Step[]>([]);
const chatBody = ref<HTMLElement | null>(null);
let chatTimer: ReturnType<typeof setTimeout> | null = null;
let i = 0;
async function scrollDown() { await nextTick(); if (chatBody.value) chatBody.value.scrollTop = chatBody.value.scrollHeight; }
function play() {
  if (i >= SCRIPT.length) { chatTimer = setTimeout(() => { visible.value = []; i = 0; play(); }, 4200); return; }
  const it = SCRIPT[i++];
  if (it.typing) { visible.value.push({ typing: it.typing }); scrollDown(); chatTimer = setTimeout(() => { visible.value.pop(); play(); }, it.typing); return; }
  visible.value.push(it); scrollDown(); chatTimer = setTimeout(play, it.who === 'user' ? 700 : 1500);
}

onMounted(async () => {
  reduceMotion.value = !!(window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches);
  await loadConfig();

  if (reduceMotion.value) {
    visible.value = SCRIPT.filter((s) => !s.typing);
  } else {
    restart();
    play();
    if (heroEl.value && 'IntersectionObserver' in window) {
      visObserver = new IntersectionObserver((es) => {
        es.forEach((e) => { offscreen = !e.isIntersecting; offscreen ? pauseRotate() : restart(); });
      }, { threshold: 0 });
      visObserver.observe(heroEl.value);
    }
  }
});
onBeforeUnmount(() => {
  if (rotTimer) clearInterval(rotTimer);
  if (chatTimer) clearTimeout(chatTimer);
  if (visObserver) visObserver.disconnect();
});
</script>

<style scoped>
.qs-hero {
  position: relative; overflow: hidden; isolation: isolate; color: #eaf3f5;
  background:
    radial-gradient(1200px 700px at 78% -8%, rgba(58,154,173,.55), transparent 60%),
    radial-gradient(900px 600px at 8% 110%, rgba(152,199,58,.22), transparent 55%),
    linear-gradient(160deg, #0f2730 0%, #1a2332 48%, #0c1c24 100%);
}
.qs-hero__orb { position: absolute; border-radius: 50%; filter: blur(60px); z-index: 0; opacity: .5; animation: qsFloat 14s ease-in-out infinite; }
.qs-hero__orb--1 { width: 360px; height: 360px; background: #3A9AAD; top: -80px; right: 18%; }
.qs-hero__orb--2 { width: 280px; height: 280px; background: #98C73A; bottom: -100px; left: 6%; animation-delay: -5s; }
.qs-hero__wm { position: absolute; right: -130px; top: 50%; transform: translateY(-50%); opacity: .06; z-index: 1; pointer-events: none; }
@keyframes qsFloat { 0%,100% { transform: translateY(0); } 50% { transform: translateY(-26px); } }

.qs-hero__grid { position: relative; z-index: 2; display: grid; grid-template-columns: 1.05fr .95fr; gap: 48px; align-items: center; padding: 60px 0 80px; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-hero__rotate { transition: opacity .45s ease; }
.qs-hero__rotate.is-fading { opacity: 0; }
.qs-hero__badge { display: inline-flex; align-items: center; gap: 8px; background: rgba(255,255,255,.1); border: 1px solid rgba(255,255,255,.18); border-radius: 999px; padding: 7px 14px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 13px; font-weight: 600; color: #dff3e0; }
.qs-hero__dot { width: 8px; height: 8px; border-radius: 50%; background: #98C73A; animation: qsPulse 2s infinite; }
@keyframes qsPulse { 0% { box-shadow: 0 0 0 0 rgba(152,199,58,.6); } 70% { box-shadow: 0 0 0 10px rgba(152,199,58,0); } 100% { box-shadow: 0 0 0 0 rgba(152,199,58,0); } }
.qs-hero__title { font-family: 'Bruum FY','Jost','Inter',sans-serif; color: #fff; font-size: clamp(38px, 5vw, 62px); font-weight: 800; line-height: 1.07; letter-spacing: -.02em; margin: 20px 0 16px; min-height: 2.1em; }
.qs-hero__title :deep(.qs-hero__hl) { color: #98C73A; position: relative; white-space: nowrap; }
.qs-hero__title :deep(.qs-hero__hl)::after { content: ""; position: absolute; left: 0; right: 0; bottom: 6px; height: 10px; background: rgba(152,199,58,.28); border-radius: 6px; z-index: -1; }
.qs-hero__sub { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 18px; line-height: 1.6; color: #c8dde0; max-width: 520px; }
.qs-hero__cta { display: inline-flex; align-items: center; gap: 8px; margin-top: 22px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-weight: 700; font-size: 15px; color: #173a0a; background: linear-gradient(180deg,#98C73A,#7aad1f); border-radius: 999px; padding: 13px 24px; text-decoration: none; box-shadow: 0 10px 28px rgba(152,199,58,.35); transition: transform .2s ease, box-shadow .2s ease; }
.qs-hero__cta:hover { transform: translateY(-2px); box-shadow: 0 16px 36px rgba(152,199,58,.45); }

.qs-hero__dots { display: flex; gap: 8px; margin-top: 24px; }
.qs-hero__dot-btn { width: 9px; height: 9px; border-radius: 50%; border: 0; padding: 0; cursor: pointer; background: rgba(255,255,255,.28); transition: .25s; }
.qs-hero__dot-btn.active { background: #98C73A; width: 26px; border-radius: 5px; }

.qs-hero__composer-label { margin-top: 30px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 13px; color: #bcd6da; }
.qs-hero__composer-label strong { color: #fff; font-weight: 700; }

.qs-hero__composer { margin-top: 12px; background: rgba(255,255,255,.97); border-radius: 20px; padding: 10px 10px 10px 18px; box-shadow: 0 24px 60px rgba(1,15,28,.28); display: flex; align-items: center; gap: 12px; }
.qs-hero__ai { display: flex; align-items: center; gap: 8px; color: #225F6B; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-weight: 700; font-size: 13px; white-space: nowrap; border-right: 1px solid #e5e7eb; padding-right: 12px; }
.qs-hero__input { flex: 1; min-width: 0; border: 0; outline: 0; background: transparent; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 16px; color: #1d1d1f; }
.qs-hero__input::placeholder { color: #9ca3af; }
.qs-hero__send { flex-shrink: 0; width: 46px; height: 46px; border: 0; border-radius: 14px; cursor: pointer; background: linear-gradient(180deg, #98C73A, #7aad1f); color: #173a0a; display: grid; place-items: center; transition: transform .2s ease; }
.qs-hero__send:hover { transform: scale(1.05); }
.qs-hero__send svg { width: 20px; height: 20px; }

.qs-hero__chips { display: flex; flex-wrap: wrap; gap: 8px; margin-top: 14px; }
.qs-hero__chip { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 13px; font-weight: 500; color: #dff3e0; background: rgba(255,255,255,.08); border: 1px solid rgba(255,255,255,.16); border-radius: 999px; padding: 7px 13px; cursor: pointer; transition: all .2s ease; }
.qs-hero__chip:hover { background: rgba(255,255,255,.18); color: #fff; }

.qs-hero__chat { position: relative; z-index: 2; width: 100%; max-width: 420px; margin-left: auto; background: #0c1a21; border: 1px solid rgba(255,255,255,.1); border-radius: 26px; box-shadow: 0 24px 60px rgba(1,15,28,.4); padding: 16px; }
.qs-hero__chat-top { display: flex; align-items: center; gap: 12px; padding: 6px 6px 14px; border-bottom: 1px solid rgba(255,255,255,.08); }
.qs-hero__chat-av { width: 44px; height: 44px; border-radius: 50%; background: linear-gradient(135deg, #3A9AAD, #98C73A); display: grid; place-items: center; }
.qs-hero__chat-nm { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-weight: 700; color: #fff; font-size: 15px; margin: 0; }
.qs-hero__chat-st { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 12px; color: #9fd3a6; margin: 0; display: flex; align-items: center; gap: 6px; }
.qs-hero__chat-st i { width: 7px; height: 7px; border-radius: 50%; background: #98C73A; display: inline-block; }
.qs-hero__chat-body { display: flex; flex-direction: column; gap: 10px; padding: 16px 6px 6px; min-height: 360px; max-height: 420px; overflow: hidden; }

.qs-hero__msg { max-width: 86%; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 14px; line-height: 1.45; padding: 11px 14px; border-radius: 16px; animation: qsPop .5s both; }
@keyframes qsPop { from { opacity: 0; transform: translateY(10px) scale(.98); } to { opacity: 1; transform: none; } }
.qs-hero__msg--user { align-self: flex-end; background: linear-gradient(180deg, #2f7785, #266472); color: #eafaff; border-bottom-right-radius: 5px; }
.qs-hero__msg--bot { align-self: flex-start; background: #15262e; color: #d7e7ea; border: 1px solid rgba(255,255,255,.06); border-bottom-left-radius: 5px; }
.qs-hero__msg :deep(.qs-hero__lead) { color: #9fd3a6; font-weight: 600; font-size: 12px; display: block; margin-bottom: 8px; }
.qs-hero__msg :deep(.qs-hero__pcard) { display: flex; gap: 12px; background: #0c1a21; border: 1px solid rgba(255,255,255,.08); border-radius: 14px; padding: 10px; margin-top: 4px; }
.qs-hero__msg :deep(.qs-hero__pcard img) { width: 62px; height: 62px; border-radius: 10px; object-fit: cover; flex-shrink: 0; }
.qs-hero__msg :deep(.qs-hero__pn) { font-weight: 700; color: #fff; font-size: 13px; }
.qs-hero__msg :deep(.qs-hero__pmeta) { font-size: 11px; color: #8fb3b8; margin: 2px 0 6px; }
.qs-hero__msg :deep(.qs-hero__price) { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-weight: 700; color: #fff; font-size: 15px; }
.qs-hero__msg :deep(.qs-hero__cb) { display: inline-block; background: rgba(152,199,58,.16); color: #98C73A; font-size: 11px; font-weight: 700; padding: 2px 8px; border-radius: 999px; margin-top: 4px; }
.qs-hero__msg :deep(.qs-hero__scard) { background: #0c1a21; border: 1px solid rgba(255,255,255,.08); border-radius: 14px; padding: 12px; margin-top: 4px; }
.qs-hero__msg :deep(.qs-hero__srow) { display: flex; justify-content: space-between; align-items: center; }
.qs-hero__msg :deep(.qs-hero__lbl) { font-size: 11px; color: #8fb3b8; text-transform: uppercase; letter-spacing: .08em; }
.qs-hero__msg :deep(.qs-hero__big) { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-weight: 800; color: #98C73A; font-size: 24px; }
.qs-hero__msg :deep(.qs-hero__ssub) { font-size: 12px; color: #aac4c8; margin-top: 2px; }

.qs-hero__chat-cta { margin-top: 10px; width: 100%; display: flex; align-items: center; justify-content: center; gap: 8px; background: rgba(152,199,58,.14); color: #d4ef9f; border: 1px solid rgba(152,199,58,.3); border-radius: 14px; padding: 12px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-weight: 700; font-size: 14px; min-height: 44px; cursor: pointer; transition: background .2s ease; }
.qs-hero__chat-cta:hover { background: rgba(152,199,58,.22); }
.qs-hero__chat-cta svg { width: 16px; height: 16px; }

.qs-hero__typing { align-self: flex-start; background: #15262e; border: 1px solid rgba(255,255,255,.06); border-radius: 16px; border-bottom-left-radius: 5px; padding: 14px 16px; display: flex; gap: 5px; }
.qs-hero__typing span { width: 7px; height: 7px; border-radius: 50%; background: #6f9aa1; animation: qsBlink 1.2s infinite; }
.qs-hero__typing span:nth-child(2) { animation-delay: .2s; }
.qs-hero__typing span:nth-child(3) { animation-delay: .4s; }
@keyframes qsBlink { 0%,60%,100% { opacity: .3; transform: translateY(0); } 30% { opacity: 1; transform: translateY(-3px); } }

@media (max-width: 980px) {
  .qs-hero__grid { grid-template-columns: 1fr; gap: 36px; padding: 44px 0 60px; }
  .qs-hero__chat { margin: 0 auto; }
  .qs-hero__wm { right: -90px; top: 16%; transform: none; }
  .qs-hero__wm :deep(img) { height: 320px !important; }
  .qs-hero__title { min-height: 0; }
}
@media (prefers-reduced-motion: reduce) {
  .qs-hero__orb, .qs-hero__dot, .qs-hero__msg, .qs-hero__typing span { animation: none; }
}
</style>
