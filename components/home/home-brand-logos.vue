<template>
  <section class="qs-bw" aria-label="Marcas parceiras">
    <div class="container">
      <p class="qs-bw__label">As maiores marcas confiam na Quanta</p>
      <div class="qs-bw__grid">
        <div v-for="(b, i) in brands" :key="i" class="qs-bw__cell" :title="b.name">
          <img
            :src="logo(b.domain)" :alt="b.name"
            width="120" height="48" loading="lazy" decoding="async"
            @error="onImgError"
          />
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Brand { name: string; domain: string }

const brands: Brand[] = [
  { name: 'Nike', domain: 'nike.com' },
  { name: 'Renner', domain: 'lojasrenner.com.br' },
  { name: 'Puma', domain: 'puma.com' },
  { name: 'Casas Bahia', domain: 'casasbahia.com.br' },
  { name: 'Cobasi', domain: 'cobasi.com.br' },
  { name: 'MadeiraMadeira', domain: 'madeiramadeira.com.br' },
  { name: 'Dafiti', domain: 'dafiti.com.br' },
  { name: 'Vivara', domain: 'vivara.com.br' },
  { name: 'LG', domain: 'lg.com' },
  { name: 'Motorola', domain: 'motorola.com' },
  { name: 'Pernambucanas', domain: 'pernambucanas.com.br' },
  { name: 'Tok&Stok', domain: 'tokstok.com.br' },
  { name: 'Olympikus', domain: 'olympikus.com.br' },
  { name: 'Under Armour', domain: 'underarmour.com' },
  { name: 'Shoptime', domain: 'shoptime.com.br' },
  { name: 'Mizuno', domain: 'mizuno.com' },
];

const logo = (domain: string) => `https://logo.clearbit.com/${domain}`;
function onImgError(e: Event) { (e.target as HTMLImageElement).style.opacity = '0'; }
</script>

<style scoped>
.qs-bw { padding: 84px 0; background: #f7f8fa; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-bw__label { text-align: center; font-size: 12px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #9ca3af; margin-bottom: 30px; }
.qs-bw__grid { display: grid; grid-template-columns: repeat(8, 1fr); border: 1px solid #eef1f3; border-radius: 12px; overflow: hidden; background: #fff; }
.qs-bw__cell { min-height: 88px; display: grid; place-items: center; padding: 16px; border-right: 1px solid #f0f4f6; border-bottom: 1px solid #f0f4f6; }
.qs-bw__cell img { max-width: 96px; max-height: 38px; width: auto; height: auto; object-fit: contain; filter: grayscale(100%) opacity(.6); transition: filter .25s ease; }
.qs-bw__cell:hover img { filter: grayscale(0) opacity(1); }

@media (max-width: 991px) { .qs-bw__grid { grid-template-columns: repeat(4, 1fr); } }
@media (max-width: 560px) { .qs-bw__grid { grid-template-columns: repeat(3, 1fr); } }
@media (prefers-reduced-motion: reduce) { .qs-bw__cell img { transition: none; } }
</style>
