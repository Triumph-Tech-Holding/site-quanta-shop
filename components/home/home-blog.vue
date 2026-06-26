<template>
  <section class="qs-blg" aria-labelledby="qs-blg-title">
    <div class="container">
      <div class="qs-blg__head">
        <span class="qs-blg__eyebrow">Sempre conectado</span>
        <h2 id="qs-blg-title" class="qs-blg__h2">Quanta em tempo real</h2>
        <p class="qs-blg__lead">Fique por dentro das últimas novidades, promoções e conteúdos exclusivos.</p>
      </div>

      <div class="qs-blg__grid">
        <article v-for="(p, i) in posts" :key="i" class="qs-blg__card">
          <div class="qs-blg__media">
            <img :src="p.img" :alt="p.title" width="600" height="375" loading="lazy" decoding="async" />
            <span class="qs-blg__tag" :class="'qs-blg__tag--' + p.kind">● {{ labels[p.kind] }}</span>
          </div>
          <div class="qs-blg__body">
            <p class="qs-blg__date">{{ p.date }}</p>
            <h3 class="qs-blg__title">{{ p.title }}</h3>
          </div>
        </article>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
type Kind = 'instagram' | 'youtube' | 'blog';
interface Post { kind: Kind; date: string; title: string; img: string }

const labels: Record<Kind, string> = { instagram: 'Instagram', youtube: 'YouTube', blog: 'Blog' };

const posts: Post[] = [
  { kind: 'instagram', date: '22 Mar 2025', title: 'Cashback de verdade! Veja como funciona na prática 💰', img: 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80&auto=format&fit=crop' },
  { kind: 'youtube', date: '20 Mar 2025', title: 'Tutorial: ative seu cashback em 3 passos', img: 'https://images.unsplash.com/photo-1574375927938-d5a98e8ffe85?w=600&q=80&auto=format&fit=crop' },
  { kind: 'blog', date: '18 Mar 2025', title: '5 dicas para maximizar seu cashback online', img: 'https://images.unsplash.com/photo-1563986768494-4dee2763ff3f?w=600&q=80&auto=format&fit=crop' },
  { kind: 'instagram', date: '15 Mar 2025', title: 'Novas marcas parceiras chegando! 🚀', img: 'https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=600&q=80&auto=format&fit=crop' },
];
</script>

<style scoped>
.qs-blg { padding: 84px 0; background: #fff; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-blg__head { text-align: center; max-width: 680px; margin: 0 auto 52px; }
.qs-blg__eyebrow { font-size: 12px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #3A9AAD; }
.qs-blg__h2 { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: clamp(30px, 3.6vw, 44px); font-weight: 700; color: #225F6B; line-height: 1.08; letter-spacing: -.02em; margin: 12px 0 14px; }
.qs-blg__lead { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 17px; color: #6b7280; }

.qs-blg__grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 22px; }
.qs-blg__card { background: #fff; border: 1px solid #e5e7eb; border-radius: 20px; overflow: hidden; transition: transform .25s ease, box-shadow .25s ease; }
.qs-blg__card:hover { transform: translateY(-4px); box-shadow: 0 4px 20px rgba(1,15,28,.12); }
.qs-blg__media { position: relative; aspect-ratio: 16 / 10; overflow: hidden; background: #eef1f3; }
.qs-blg__media img { width: 100%; height: 100%; object-fit: cover; transition: transform .5s ease; }
.qs-blg__card:hover .qs-blg__media img { transform: scale(1.06); }
.qs-blg__tag { position: absolute; top: 10px; left: 10px; font-size: 11px; font-weight: 700; color: #fff; padding: 4px 10px; border-radius: 999px; display: flex; align-items: center; gap: 5px; }
.qs-blg__tag--instagram { background: linear-gradient(135deg, #f09433, #dc2743, #bc1888); }
.qs-blg__tag--youtube { background: #ff0000; }
.qs-blg__tag--blog { background: #2F7785; }
.qs-blg__body { padding: 16px 18px 20px; }
.qs-blg__date { font-size: 12px; color: #9ca3af; }
.qs-blg__title { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: 15px; font-weight: 700; color: #225F6B; margin: 6px 0 0; line-height: 1.35; }

@media (max-width: 991px) { .qs-blg__grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 520px) { .qs-blg__grid { grid-template-columns: 1fr; } }
@media (prefers-reduced-motion: reduce) { .qs-blg__card, .qs-blg__media img { transition: none; } }
</style>
