<template>
  <section class="qs-pon" aria-labelledby="qs-pon-title">
    <div class="container">
      <div class="qs-pon__head">
        <span class="qs-pon__eyebrow">Parceiros Online</span>
        <h2 id="qs-pon-title" class="qs-pon__h2">Compre online e receba cashback</h2>
        <p class="qs-pon__lead">Centenas de marcas com cashback garantido. Ative e compre normalmente.</p>
      </div>

      <div class="qs-pon__grid">
        <div v-for="(p, i) in partners" :key="i" class="qs-pon__card">
          <div class="qs-pon__logo-wrap">
            <img
              :src="'/img/brands/' + p.slug + '.png'"
              :alt="p.name"
              width="110" height="40"
              loading="lazy" decoding="async"
              class="qs-pon__logo"
              @error="hide"
            />
          </div>
          <p class="qs-pon__nm">{{ p.name }}</p>
          <nuxt-link class="qs-pon__act" href="/login">Ativar cashback</nuxt-link>
        </div>
      </div>

      <div class="qs-pon__footer">
        <nuxt-link class="qs-pon__more" href="/busca-inteligente">
          Ver todas as +100 lojas →
        </nuxt-link>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Partner { name: string; slug: string }

const partners: Partner[] = [
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
  { name: 'AliExpress', slug: 'aliexpress' },
  { name: 'Booking.com', slug: 'booking-com' },
];

function hide(e: Event) {
  const img = e.target as HTMLImageElement;
  img.closest('.qs-pon__logo-wrap')?.classList.add('no-logo');
}
</script>

<style scoped>
.qs-pon { padding: 84px 0; background: #f7f8fa; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-pon__head { text-align: center; max-width: 680px; margin: 0 auto 52px; }
.qs-pon__eyebrow { font-size: 12px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #3A9AAD; }
.qs-pon__h2 { font-family: 'Jost','Inter',sans-serif; font-size: clamp(26px, 3.8vw, 44px); font-weight: 700; color: #225F6B; line-height: 1.08; letter-spacing: -.02em; margin: 12px 0 14px; }
.qs-pon__lead { font-family: 'Inter','Jost',sans-serif; font-size: 17px; color: #6b7280; }

.qs-pon__grid {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  gap: 14px;
}

.qs-pon__card {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  padding: 22px 14px 18px;
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0;
  transition: transform .22s ease, box-shadow .22s ease;
}
.qs-pon__card:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 24px rgba(1,15,28,.1);
}

.qs-pon__logo-wrap {
  height: 48px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 14px;
}
.qs-pon__logo-wrap.no-logo::after {
  content: '🏷️';
  font-size: 28px;
}
.qs-pon__logo {
  max-height: 40px;
  width: auto;
  max-width: 100px;
  object-fit: contain;
}

.qs-pon__nm {
  font-family: 'Jost','Inter',sans-serif;
  font-weight: 700;
  font-size: 13px;
  color: #225F6B;
  margin: 0 0 14px;
  line-height: 1.3;
}

.qs-pon__act {
  display: block;
  width: 100%;
  font-size: 12px;
  font-weight: 700;
  font-family: 'Inter','Jost',sans-serif;
  color: #2F7785;
  border: 1.5px solid #2F7785;
  border-radius: 999px;
  padding: 9px 0;
  min-height: 36px;
  text-decoration: none;
  transition: background .2s ease, color .2s ease;
  margin-top: auto;
}
.qs-pon__act:hover { background: #2F7785; color: #fff; }

.qs-pon__footer {
  text-align: center;
  margin-top: 36px;
}
.qs-pon__more {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-family: 'Inter','Jost',sans-serif;
  font-weight: 700;
  font-size: 15px;
  color: #2F7785;
  border: 2px solid #2F7785;
  border-radius: 999px;
  padding: 13px 32px;
  text-decoration: none;
  transition: background .2s ease, color .2s ease;
}
.qs-pon__more:hover { background: #2F7785; color: #fff; }

@media (max-width: 1100px) { .qs-pon__grid { grid-template-columns: repeat(4, 1fr); } }
@media (max-width: 700px) { .qs-pon__grid { grid-template-columns: repeat(3, 1fr); gap: 10px; } }
@media (max-width: 480px) { .qs-pon__grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 760px) { .qs-pon { padding: 48px 0; } .qs-pon__head { margin-bottom: 30px; } }
@media (prefers-reduced-motion: reduce) { .qs-pon__card, .qs-pon__act, .qs-pon__more { transition: none; } }
</style>
