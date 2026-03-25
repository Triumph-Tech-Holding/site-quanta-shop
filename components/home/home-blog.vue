<template>
  <section class="qs-blog">
    <div class="container">
      <div class="qs-section-header">
        <span class="qs-section-label">Blog Quanta</span>
        <h2 class="qs-section-title">Dicas para economizar mais</h2>
        <p class="qs-section-sub">Conteúdo prático sobre cashback, finanças e consumo inteligente.</p>
      </div>

      <div class="qs-blog__grid">
        <a
          v-for="post in posts"
          :key="post.slug"
          href="/blog"
          class="qs-blog-card"
        >
          <div class="qs-blog-card__thumb">
            <img :src="post.img" :alt="post.title" />
            <span class="qs-blog-card__tag">{{ post.tag }}</span>
          </div>
          <div class="qs-blog-card__body">
            <div class="qs-blog-card__meta">
              <span>{{ post.date }}</span>
              <span>·</span>
              <span>{{ post.readTime }} min leitura</span>
            </div>
            <h3 class="qs-blog-card__title">{{ post.title }}</h3>
            <p class="qs-blog-card__excerpt">{{ post.excerpt }}</p>
            <span class="qs-blog-card__link">
              Ler artigo
              <svg width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
            </span>
          </div>
        </a>
      </div>

      <div class="qs-blog__more">
        <nuxt-link href="/blog" class="qs-btn-outline-primary">
          Ver todos os artigos
          <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </nuxt-link>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
const posts = [
  {
    slug: 'como-maximizar-cashback',
    title: 'Como maximizar seu cashback em compras online',
    excerpt: 'Estratégias simples para multiplicar o dinheiro que volta para o seu bolso a cada compra.',
    tag: 'Finanças',
    date: '15 Mar 2026',
    readTime: 4,
    img: 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80&auto=format&fit=crop',
  },
  {
    slug: 'parceiros-locais-quanta',
    title: 'Parceiros locais: cashback na sua cidade',
    excerpt: 'Descubra como aproveitar o cashback em lojas, restaurantes e serviços perto de você.',
    tag: 'Cashback Local',
    date: '10 Mar 2026',
    readTime: 3,
    img: 'https://images.unsplash.com/photo-1534531173927-aeb928d54385?w=600&q=80&auto=format&fit=crop',
  },
  {
    slug: 'quanta-plus-vale-a-pena',
    title: 'Quanta Plus vale a pena? Calculamos pra você',
    excerpt: 'Fizemos as contas: saiba exatamente quando o plano premium se paga.',
    tag: 'Plano Plus',
    date: '05 Mar 2026',
    readTime: 5,
    img: 'https://images.unsplash.com/photo-1554224155-8d04cb21cd6c?w=600&q=80&auto=format&fit=crop',
  },
];
</script>

<style scoped>
.qs-blog {
  padding: 72px 0;
  background: #f7f8fa;
}

.qs-section-header {
  text-align: center;
  margin-bottom: 40px;
}

.qs-section-label {
  display: inline-block;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: #98C73A;
  margin-bottom: 10px;
}

.qs-section-title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(24px, 4vw, 36px);
  font-weight: 800;
  color: #111827;
  letter-spacing: -0.03em;
  margin-bottom: 8px;
}

.qs-section-sub {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  color: #6b7280;
}

.qs-blog__grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
  margin-bottom: 36px;
}

@media (max-width: 991px) { .qs-blog__grid { grid-template-columns: 1fr; } }
@media (max-width: 1199px) { .qs-blog__grid { grid-template-columns: repeat(2, 1fr); } }

.qs-blog-card {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  overflow: hidden;
  text-decoration: none;
  transition: all 0.25s ease;
  display: block;
}

.qs-blog-card:hover {
  box-shadow: 0 8px 32px rgba(0,0,0,0.10);
  transform: translateY(-3px);
  border-color: transparent;
}

.qs-blog-card__thumb {
  position: relative;
  height: 180px;
  overflow: hidden;
}

.qs-blog-card__thumb img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.qs-blog-card:hover .qs-blog-card__thumb img {
  transform: scale(1.04);
}

.qs-blog-card__tag {
  position: absolute;
  top: 12px;
  left: 12px;
  background: #2F7785;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 10px;
  font-weight: 700;
  padding: 3px 10px;
  border-radius: 999px;
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.qs-blog-card__body {
  padding: 20px;
}

.qs-blog-card__meta {
  display: flex;
  align-items: center;
  gap: 8px;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  color: #9ca3af;
  margin-bottom: 10px;
}

.qs-blog-card__title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 16px;
  font-weight: 700;
  color: #111827;
  line-height: 1.4;
  margin-bottom: 8px;
}

.qs-blog-card__excerpt {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  color: #6b7280;
  line-height: 1.5;
  margin-bottom: 16px;
}

.qs-blog-card__link {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  color: #2F7785;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 600;
  transition: gap 0.2s;
}

.qs-blog-card:hover .qs-blog-card__link {
  gap: 8px;
}

.qs-blog__more {
  text-align: center;
}

.qs-btn-outline-primary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  border: 2px solid #2F7785;
  color: #2F7785;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 600;
  padding: 12px 28px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.qs-btn-outline-primary:hover {
  background: #2F7785;
  color: #fff;
}
</style>
