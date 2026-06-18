<template>
  <section class="qs-plo" aria-labelledby="qs-plo-title">
    <div class="container">
      <div class="qs-plo__head">
        <span class="qs-plo__eyebrow">Parceiros Locais</span>
        <h2 id="qs-plo-title" class="qs-plo__h2">Cashback perto de você</h2>
        <p class="qs-plo__lead">Lojas, restaurantes e serviços no seu bairro com cashback automático.</p>
      </div>

      <div class="qs-plo__grid">
        <article v-for="(l, i) in locals" :key="i" class="qs-plo__card">
          <img class="qs-plo__th" :src="l.img" :alt="l.name" width="64" height="64" loading="lazy" decoding="async" />
          <div>
            <h3 class="qs-plo__nm">{{ l.name }}</h3>
            <p class="qs-plo__cb">Até {{ l.cb }} de cashback</p>
            <p class="qs-plo__loc">{{ l.loc }}</p>
            <a class="qs-plo__wa" :href="l.wa" target="_blank" rel="noopener">
              <svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M.06 24l1.69-6.16a11.87 11.87 0 01-1.6-5.95C.16 5.32 5.5 0 12.06 0a11.8 11.8 0 018.4 3.49 11.76 11.76 0 013.48 8.39c0 6.55-5.34 11.88-11.9 11.88a11.9 11.9 0 01-5.7-1.45L.06 24zm6.6-3.8c1.68.99 3.28 1.59 5.4 1.59 5.45 0 9.9-4.43 9.9-9.88a9.8 9.8 0 00-2.9-7 9.78 9.78 0 00-7-2.9c-5.46 0-9.9 4.43-9.9 9.88 0 2.23.65 3.9 1.74 5.65l-1 3.66 3.76-1zm11.4-5.66c-.07-.12-.27-.2-.57-.35-.3-.15-1.76-.87-2.03-.97-.27-.1-.47-.15-.67.15-.2.3-.77.96-.94 1.16-.17.2-.35.22-.65.07-.3-.15-1.26-.46-2.4-1.48-.89-.79-1.49-1.77-1.66-2.07-.17-.3-.02-.46.13-.61.13-.13.3-.35.45-.52.15-.17.2-.3.3-.5.1-.2.05-.37-.02-.52-.07-.15-.67-1.62-.92-2.22-.24-.58-.49-.5-.67-.51l-.57-.01c-.2 0-.52.07-.8.37-.27.3-1.04 1.02-1.04 2.49 0 1.47 1.07 2.89 1.22 3.09.15.2 2.1 3.2 5.08 4.49.71.3 1.26.49 1.69.62.71.23 1.36.2 1.87.12.57-.08 1.76-.72 2-1.41.25-.7.25-1.29.18-1.42z"/></svg>
              WhatsApp
            </a>
          </div>
        </article>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Local { name: string; cb: string; loc: string; img: string; wa: string }

const locals: Local[] = [
  { name: 'Padaria Real', cb: '5%', loc: 'Vila Mariana — São Paulo, SP', img: 'https://images.unsplash.com/photo-1509440159596-0249088772ff?w=200&q=80&auto=format&fit=crop', wa: 'https://api.whatsapp.com/send/?phone=5511999999999' },
  { name: 'Restaurante Sabor & Arte', cb: '8%', loc: 'Botafogo — Rio de Janeiro, RJ', img: 'https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?w=200&q=80&auto=format&fit=crop', wa: 'https://api.whatsapp.com/send/?phone=5521999999999' },
  { name: 'Studio Bem Estar', cb: '6%', loc: 'Savassi — Belo Horizonte, MG', img: 'https://images.unsplash.com/photo-1540497077202-7c8a3999166f?w=200&q=80&auto=format&fit=crop', wa: 'https://api.whatsapp.com/send/?phone=5531999999999' },
];
</script>

<style scoped>
.qs-plo { padding: 84px 0; background: #fff; }
.container { width: 100%; max-width: 1200px; margin: 0 auto; padding: 0 24px; }

.qs-plo__head { text-align: center; max-width: 680px; margin: 0 auto 52px; }
.qs-plo__eyebrow { font-size: 12px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: #3A9AAD; }
.qs-plo__h2 { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-size: clamp(30px, 3.6vw, 44px); font-weight: 700; color: #225F6B; line-height: 1.08; letter-spacing: -.02em; margin: 12px 0 14px; }
.qs-plo__lead { font-family: 'Kiye Sans','Inter','Jost',sans-serif; font-size: 17px; color: #6b7280; }

.qs-plo__grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 22px; }
.qs-plo__card { display: flex; gap: 14px; background: #fff; border: 1px solid #e5e7eb; border-radius: 20px; padding: 18px; align-items: center; transition: transform .25s ease, box-shadow .25s ease; }
.qs-plo__card:hover { transform: translateY(-4px); box-shadow: 0 4px 20px rgba(1,15,28,.12); }
.qs-plo__th { width: 64px; height: 64px; border-radius: 14px; object-fit: cover; flex-shrink: 0; }
.qs-plo__nm { font-family: 'Bruum FY','Jost','Inter',sans-serif; font-weight: 700; color: #225F6B; font-size: 16px; }
.qs-plo__cb { color: #7aad1f; font-weight: 600; font-size: 13px; }
.qs-plo__loc { font-size: 12px; color: #9ca3af; margin-top: 2px; }
.qs-plo__wa { margin-top: 8px; display: inline-flex; align-items: center; gap: 6px; font-size: 12px; font-weight: 700; color: #16a34a; text-decoration: none; }
.qs-plo__wa svg { width: 14px; height: 14px; }

@media (max-width: 820px) { .qs-plo__grid { grid-template-columns: 1fr; } }
@media (prefers-reduced-motion: reduce) { .qs-plo__card { transition: none; } }
</style>
