<template>
  <section class="qs-ofd" aria-labelledby="qs-ofd-title">
    <div class="container">
      <div class="qs-ofd__head">
        <span class="qs-ofd__eyebrow">Tempo limitado</span>
        <h2 id="qs-ofd-title" class="qs-ofd__h2">Ofertas do Dia</h2>
        <p class="qs-ofd__lead">Produtos selecionados com cashback turbinado. Aproveite antes que acabe.</p>
      </div>

      <div class="qs-ofd__grid">
        <article v-for="(o, i) in offers" :key="i" class="qs-ofd__card">
          <div class="qs-ofd__media">
            <img :src="o.img" :alt="o.name" width="600" height="450" loading="lazy" decoding="async" />
            <span class="qs-ofd__cb">⚡ {{ o.cb }} cashback</span>
          </div>
          <div class="qs-ofd__body">
            <p class="qs-ofd__brand">{{ o.brand }}</p>
            <h3 class="qs-ofd__name">{{ o.name }}</h3>
            <p class="qs-ofd__price">{{ o.price }}</p>
            <nuxt-link class="qs-ofd__go" :href="o.href">Aproveitar Agora →</nuxt-link>
          </div>
        </article>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Offer { name: string; brand: string; price: string; cb: string; img: string; href: string }

const offers: Offer[] = [
  { name: 'Tênis Air Max Premium', brand: 'Nike', price: 'R$ 599,90', cb: '12%', img: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600&q=80&auto=format&fit=crop', href: '/partners?nome=Tenis%20Air%20Max%20Premium' },
  { name: 'Bolsa Couro Elegance', brand: 'Vivara', price: 'R$ 349,90', cb: '9%', img: 'https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=600&q=80&auto=format&fit=crop', href: '/partners?nome=Bolsa%20Couro%20Elegance' },
  { name: 'Smartwatch GT Series', brand: 'Motorola', price: 'R$ 899,90', cb: '10%', img: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=600&q=80&auto=format&fit=crop', href: '/partners?nome=Smartwatch%20GT%20Series' },
  { name: 'Kit Café Gourmet', brand: 'Nescafé', price: 'R$ 89,90', cb: '8%', img: 'https://images.unsplash.com/photo-1447933601403-0c6688de566e?w=600&q=80&auto=format&fit=crop', href: '/partners?nome=Kit%20Cafe%20Gourmet' },
];
</script>

<style scoped>
.qs-ofd { padding: 84px 0; background: #fff; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-ofd__head { text-align: center; max-width: 680px; margin: 0 auto 52px; }
.qs-ofd__eyebrow { font-size: 12px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #3A9AAD; }
.qs-ofd__h2 { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: clamp(30px, 3.6vw, 44px); font-weight: 700; color: #225F6B; line-height: 1.08; letter-spacing: -.02em; margin: 12px 0 14px; }
.qs-ofd__lead { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 17px; color: #6b7280; }

.qs-ofd__grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 22px; }
.qs-ofd__card { background: #fff; border: 1px solid #e5e7eb; border-radius: 20px; overflow: hidden; transition: transform .25s ease, box-shadow .25s ease; }
.qs-ofd__card:hover { transform: translateY(-4px); box-shadow: 0 4px 20px rgba(1,15,28,.12); }
.qs-ofd__media { position: relative; aspect-ratio: 4 / 3; overflow: hidden; background: #eef1f3; }
.qs-ofd__media img { width: 100%; height: 100%; object-fit: cover; transition: transform .5s ease; }
.qs-ofd__card:hover .qs-ofd__media img { transform: scale(1.06); }
.qs-ofd__cb { position: absolute; top: 12px; left: 12px; background: rgba(26,35,50,.86); color: #98C73A; font-size: 12px; font-weight: 700; padding: 5px 11px; border-radius: 999px; backdrop-filter: blur(6px); }
.qs-ofd__body { padding: 16px 18px 20px; }
.qs-ofd__brand { font-size: 12px; color: #9ca3af; font-weight: 600; }
.qs-ofd__name { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: 16px; font-weight: 700; color: #225F6B; margin: 4px 0 8px; }
.qs-ofd__price { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: 22px; font-weight: 700; color: #225F6B; }
.qs-ofd__go { display: flex; align-items: center; justify-content: center; gap: 8px; margin-top: 14px; width: 100%; min-height: 44px; font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-weight: 700; font-size: 15px; color: #173a0a; background: linear-gradient(180deg, #98C73A, #7aad1f); border-radius: 999px; padding: 12px 20px; text-decoration: none; box-shadow: 0 10px 28px rgba(152,199,58,.35); transition: transform .2s ease, box-shadow .2s ease; }
.qs-ofd__go:hover { transform: translateY(-2px); box-shadow: 0 16px 36px rgba(152,199,58,.45); }

@media (max-width: 991px) { .qs-ofd__grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 520px) { .qs-ofd__grid { grid-template-columns: 1fr; } }
@media (prefers-reduced-motion: reduce) { .qs-ofd__card, .qs-ofd__media img, .qs-ofd__go { transition: none; } }
</style>
