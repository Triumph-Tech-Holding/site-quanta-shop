<template>
  <section class="qs-bw" aria-label="Marcas parceiras">
    <p class="qs-bw__label">As maiores marcas confiam na Quanta</p>
    <div class="qs-bw__track-wrap" @mouseenter="pause" @mouseleave="resume">
      <div ref="track" class="qs-bw__track" :class="{ 'is-paused': paused }">
        <img
          v-for="(b, i) in doubled" :key="i"
          :src="'/img/brands/' + b.slug + '.png'"
          :alt="b.name"
          :title="b.name"
          width="120" height="42"
          loading="lazy" decoding="async"
          class="qs-bw__logo"
          @error="hide"
        />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';

interface Brand { name: string; slug: string }

const brands: Brand[] = [
  { name: 'Nike', slug: 'nike' },
  { name: 'Adidas', slug: 'adidas' },
  { name: 'Samsung', slug: 'samsung' },
  { name: 'Casas Bahia', slug: 'casas-bahia' },
  { name: 'Carrefour', slug: 'carrefour' },
  { name: 'Natura', slug: 'natura' },
  { name: 'Lojas Renner', slug: 'lojas-renner' },
  { name: 'C&A', slug: 'c-a' },
  { name: 'Centauro', slug: 'centauro' },
  { name: 'Decathlon', slug: 'decathlon' },
  { name: 'Motorola', slug: 'motorola' },
  { name: 'Puma', slug: 'puma' },
  { name: 'Vivara', slug: 'vivara' },
  { name: 'Cobasi', slug: 'cobasi' },
  { name: 'Lacoste', slug: 'lacoste' },
  { name: 'Tok&Stok', slug: 'tok-stok' },
  { name: 'LG', slug: 'lg' },
  { name: 'Electrolux', slug: 'electrolux' },
  { name: 'Calvin Klein', slug: 'calvin-klein' },
  { name: 'Riachuelo', slug: 'riachuelo' },
  { name: 'KaBuM!', slug: 'kabum' },
  { name: 'Dafiti', slug: 'dafiti' },
  { name: 'Fossil', slug: 'fossil' },
  { name: 'Mizuno', slug: 'mizuno' },
  { name: 'Kipling', slug: 'kipling' },
  { name: 'JBL', slug: 'jbl' },
];

const doubled = computed(() => [...brands, ...brands]);
const paused = ref(false);
const track = ref<HTMLElement | null>(null);

function pause() { paused.value = true; }
function resume() { paused.value = false; }
function hide(e: Event) {
  const el = e.target as HTMLImageElement;
  el.style.display = 'none';
}

onMounted(() => {
  if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
    paused.value = true;
  }
});
</script>

<style scoped>
.qs-bw {
  padding: 40px 0;
  background: #fff;
  border-top: 1px solid #f0f4f6;
  border-bottom: 1px solid #f0f4f6;
  overflow: hidden;
}
.qs-bw__label {
  text-align: center;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: .18em;
  text-transform: uppercase;
  color: #b0bec5;
  margin: 0 0 22px;
}

.qs-bw__track-wrap {
  overflow: hidden;
  mask-image: linear-gradient(to right, transparent 0%, #000 8%, #000 92%, transparent 100%);
  -webkit-mask-image: linear-gradient(to right, transparent 0%, #000 8%, #000 92%, transparent 100%);
}

.qs-bw__track {
  display: flex;
  align-items: center;
  gap: 48px;
  width: max-content;
  animation: qs-marquee 36s linear infinite;
}
.qs-bw__track.is-paused {
  animation-play-state: paused;
}

.qs-bw__logo {
  height: 42px;
  width: auto;
  max-width: 120px;
  object-fit: contain;
  flex-shrink: 0;
  filter: grayscale(100%) opacity(.55);
  transition: filter .3s ease;
  cursor: default;
}
.qs-bw__logo:hover {
  filter: grayscale(0%) opacity(1);
}

@keyframes qs-marquee {
  0%   { transform: translateX(0); }
  100% { transform: translateX(-50%); }
}

@media (prefers-reduced-motion: reduce) {
  .qs-bw__track { animation: none; }
}
</style>
