<template>
  <section class="qs-pon" aria-labelledby="qs-pon-title">
    <div class="container">
      <div class="qs-pon__head">
        <span class="qs-pon__eyebrow">Parceiros Online</span>
        <h2 id="qs-pon-title" class="qs-pon__h2">Compre online e receba cashback</h2>
        <p class="qs-pon__lead">Centenas de marcas com cashback garantido. Ative e compre normalmente.</p>
      </div>

      <div class="qs-pon__grid">
        <div v-for="(p, i) in partners" :key="i" class="qs-pon__cell">
          <img
            :src="logo(p.domain)" :alt="p.name"
            width="110" height="34" loading="lazy" decoding="async"
            @error="onImgError"
          />
          <p class="qs-pon__nm">{{ p.name }}</p>
          <p class="qs-pon__cb">Até {{ p.cb }} de cashback</p>
          <nuxt-link class="qs-pon__act" href="/login">Ative seu cashback</nuxt-link>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Partner { name: string; domain: string; cb: string }

const partners: Partner[] = [
  { name: 'Nike', domain: 'nike.com', cb: '12%' },
  { name: 'Renner', domain: 'lojasrenner.com.br', cb: '8%' },
  { name: 'Casas Bahia', domain: 'casasbahia.com.br', cb: '5%' },
  { name: 'Dafiti', domain: 'dafiti.com.br', cb: '10%' },
  { name: 'Puma', domain: 'puma.com', cb: '9%' },
  { name: 'MadeiraMadeira', domain: 'madeiramadeira.com.br', cb: '7%' },
  { name: 'Pernambucanas', domain: 'pernambucanas.com.br', cb: '6%' },
  { name: 'Cobasi', domain: 'cobasi.com.br', cb: '8%' },
];

const logo = (domain: string) => `https://logo.clearbit.com/${domain}`;
function onImgError(e: Event) { (e.target as HTMLImageElement).style.opacity = '0'; }
</script>

<style scoped>
.qs-pon { padding: 84px 0; background: #f7f8fa; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-pon__head { text-align: center; max-width: 680px; margin: 0 auto 52px; }
.qs-pon__eyebrow { font-size: 12px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #3A9AAD; }
.qs-pon__h2 { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: clamp(30px, 3.6vw, 44px); font-weight: 700; color: #225F6B; line-height: 1.08; letter-spacing: -.02em; margin: 12px 0 14px; }
.qs-pon__lead { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 17px; color: #6b7280; }

.qs-pon__grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 18px; }
.qs-pon__cell { background: #fff; border: 1px solid #e5e7eb; border-radius: 20px; padding: 24px; text-align: center; transition: transform .25s ease, box-shadow .25s ease; }
.qs-pon__cell:hover { transform: translateY(-4px); box-shadow: 0 4px 20px rgba(1,15,28,.12); }
.qs-pon__cell img { height: 34px; width: auto; margin: 0 auto 16px; object-fit: contain; filter: grayscale(20%); }
.qs-pon__nm { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-weight: 700; color: #225F6B; }
.qs-pon__cb { color: #7aad1f; font-weight: 600; font-size: 14px; margin: 4px 0 14px; }
.qs-pon__act { display: block; font-size: 13px; font-weight: 700; color: #2F7785; border: 1.5px solid #2F7785; border-radius: 999px; padding: 10px 0; min-height: 44px; text-decoration: none; transition: background .2s ease, color .2s ease; }
.qs-pon__act:hover { background: #2F7785; color: #fff; }

@media (max-width: 991px) { .qs-pon__grid { grid-template-columns: repeat(2, 1fr); } }
@media (prefers-reduced-motion: reduce) { .qs-pon__cell, .qs-pon__act { transition: none; } }
</style>
