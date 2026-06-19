<template>
  <article class="qs-cup" :class="{ 'qs-cup--exclusive': cupom.exclusive }">
    <span v-if="cupom.exclusive" class="qs-cup__flag">Exclusivo Quanta</span>

    <div class="qs-cup__logo">
      <img
        v-if="!logoFailed" :src="cupom.store.logo" :alt="cupom.store.name"
        width="120" height="120" loading="lazy" decoding="async" @error="logoFailed = true"
      />
      <span v-else class="qs-cup__logo-fallback">{{ initials }}</span>
    </div>

    <p class="qs-cup__store">{{ cupom.store.name }}</p>

    <p class="qs-cup__discount">{{ cupom.discount }}</p>
    <p v-if="cupom.cashback" class="qs-cup__cb">até {{ cupom.cashback }} cashback</p>

    <span class="qs-cup__exp" :class="{ 'is-soon': diasRestantes <= 3 }">⏱ {{ expiraLabel }}</span>

    <button v-if="!revealed" type="button" class="qs-cup__reveal" @click="reveal">
      {{ cupom.type === 'voucher' ? 'Ver cupom' : 'Ativar oferta' }} →
    </button>

    <template v-else>
      <button v-if="cupom.type === 'voucher'" type="button" class="qs-cup__code" :class="{ 'is-copied': copied }" @click="copy">
        <span class="qs-cup__code-val">{{ copied ? 'Copiado!' : cupom.code }}</span>
        <span class="qs-cup__code-act">
          <svg v-if="!copied" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" aria-hidden="true"><rect x="9" y="9" width="11" height="11" rx="2" /><path d="M5 15V5a2 2 0 0 1 2-2h10" /></svg>
          <svg v-else viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" aria-hidden="true"><path d="M20 6 9 17l-5-5" /></svg>
        </span>
      </button>
      <a class="qs-cup__go" :href="cupom.url" target="_blank" rel="noopener">Ir para a loja →</a>
    </template>
  </article>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import type { Cupom } from '@/composables/useCupons';

const props = defineProps<{ cupom: Cupom; diasRestantes: number }>();

const revealed = ref(false);
const copied = ref(false);
const logoFailed = ref(false);

const initials = computed(() =>
  props.cupom.store.name.split(' ').slice(0, 2).map((w) => w[0]).join('').toUpperCase(),
);

const expiraLabel = computed(() => {
  const d = props.diasRestantes;
  if (d <= 0) return 'expira hoje';
  if (d === 1) return 'expira amanhã';
  return `expira em ${d} dias`;
});

function reveal() {
  revealed.value = true;
  if (props.cupom.url && props.cupom.url !== '#') {
    window.open(props.cupom.url, '_blank', 'noopener');
  }
}

async function copy() {
  if (!props.cupom.code) return;
  try { await navigator.clipboard.writeText(props.cupom.code); } catch { /* noop */ }
  copied.value = true;
  setTimeout(() => (copied.value = false), 2000);
}
</script>

<style scoped>
.qs-cup {
  position: relative; display: flex; flex-direction: column; align-items: center; text-align: center;
  background: #fff; border: 1px solid #e8eef0; border-radius: 18px; padding: 22px 18px 18px;
  box-shadow: 0 6px 20px rgba(15, 39, 48, .06); transition: transform .2s ease, box-shadow .2s ease;
  overflow: hidden;
}
.qs-cup:hover { transform: translateY(-4px); box-shadow: 0 18px 40px rgba(15, 39, 48, .12); }
.qs-cup--exclusive { border-color: rgba(152, 199, 58, .55); }

.qs-cup__flag {
  position: absolute; top: 13px; right: -34px; transform: rotate(45deg);
  background: linear-gradient(180deg, #98C73A, #7aad1f); color: #173a0a;
  font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 10px; font-weight: 800;
  letter-spacing: .04em; padding: 4px 40px; box-shadow: 0 4px 10px rgba(122, 173, 31, .35);
}

.qs-cup__logo {
  width: 84px; height: 84px; border-radius: 50%; background: #fff; border: 1px solid #eef2f3;
  display: grid; place-items: center; overflow: hidden; box-shadow: 0 4px 14px rgba(15, 39, 48, .07); margin-bottom: 14px;
}
.qs-cup__logo img { width: 60px; height: 60px; object-fit: contain; }
.qs-cup__logo-fallback { font-family: 'Bruum FY', 'Jost', 'Inter', sans-serif; font-weight: 800; font-size: 24px; color: #225F6B; }

.qs-cup__store { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 13px; font-weight: 600; color: #8a9ba0; margin: 0 0 8px; }
.qs-cup__discount { font-family: 'Bruum FY', 'Jost', 'Inter', sans-serif; font-weight: 800; line-height: 1.05; color: #1a2332; font-size: clamp(20px, 4vw, 26px); margin: 0; }
.qs-cup__cb { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 13px; font-weight: 700; color: #2F7785; margin: 6px 0 0; }
.qs-cup__exp { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 11.5px; color: #9aa9ae; margin: 10px 0 14px; }
.qs-cup__exp.is-soon { color: #d97706; font-weight: 700; }

.qs-cup__reveal {
  margin-top: auto; width: 100%; min-height: 44px; cursor: pointer; border: 0; border-radius: 12px;
  background: linear-gradient(180deg, #98C73A, #7aad1f); color: #173a0a;
  font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-weight: 700; font-size: 14px;
  transition: transform .15s ease, box-shadow .2s ease;
}
.qs-cup__reveal:hover { transform: translateY(-1px); box-shadow: 0 10px 22px rgba(152, 199, 58, .35); }

.qs-cup__code {
  margin-top: auto; display: flex; align-items: center; justify-content: space-between; gap: 8px; width: 100%;
  border: 1.5px dashed #b9d36a; background: rgba(152, 199, 58, .08); border-radius: 12px;
  padding: 11px 14px; cursor: pointer; min-height: 44px; transition: background .2s ease, border-color .2s ease;
}
.qs-cup__code:hover { background: rgba(152, 199, 58, .16); }
.qs-cup__code.is-copied { border-color: #2F7785; background: rgba(47, 119, 133, .1); }
.qs-cup__code-val { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-weight: 800; letter-spacing: .06em; color: #225F6B; font-size: 15px; text-transform: uppercase; }
.qs-cup__code-act { display: inline-flex; align-items: center; color: #7aad1f; }
.qs-cup__code-act svg { width: 16px; height: 16px; }
.qs-cup__code.is-copied .qs-cup__code-act { color: #2F7785; }

.qs-cup__go { margin-top: 8px; font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 12px; font-weight: 700; color: #2F7785; text-decoration: none; }
.qs-cup__go:hover { text-decoration: underline; }
</style>
