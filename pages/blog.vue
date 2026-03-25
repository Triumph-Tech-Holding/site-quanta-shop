<template>
  <div class="qblog-page">

    <!-- Hero -->
    <section class="qblog-hero">
      <div class="container qblog-hero__inner">
        <span class="qblog-label qblog-label--white">Conteúdo e dicas</span>
        <h1>Blog <span class="qblog-accent">Quanta Shop</span></h1>
        <p>Dicas de cashback, finanças pessoais, novidades dos parceiros e muito mais.</p>
      </div>
    </section>

    <!-- Filtros -->
    <section class="qblog-filters">
      <div class="container">
        <div class="qblog-cat-list">
          <button
            v-for="cat in categories"
            :key="cat"
            class="qblog-cat-btn"
            :class="{ 'qblog-cat-btn--active': activeCategory === cat }"
            @click="activeCategory = cat"
          >{{ cat }}</button>
        </div>
      </div>
    </section>

    <!-- Posts grid -->
    <section class="qblog-grid-section">
      <div class="container">
        <div class="qblog-grid">
          <article v-for="post in filteredPosts" :key="post.id" class="qblog-card">
            <div class="qblog-card__img-wrap">
              <img :src="post.img" :alt="post.title" class="qblog-card__img" loading="lazy" />
              <span class="qblog-card__cat">{{ post.category }}</span>
            </div>
            <div class="qblog-card__body">
              <div class="qblog-card__meta">
                <span class="qblog-card__date">{{ post.date }}</span>
                <span class="qblog-card__time">{{ post.readTime }}</span>
              </div>
              <h2 class="qblog-card__title">{{ post.title }}</h2>
              <p class="qblog-card__excerpt">{{ post.excerpt }}</p>
              <nuxt-link :href="post.slug" class="qblog-card__read">
                Ler artigo
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14"/><path d="m12 5 7 7-7 7"/></svg>
              </nuxt-link>
            </div>
          </article>
        </div>

        <div v-if="filteredPosts.length === 0" class="qblog-empty">
          Nenhum artigo encontrado nesta categoria.
        </div>
      </div>
    </section>

    <!-- Newsletter -->
    <section class="qblog-newsletter">
      <div class="container qblog-newsletter__inner">
        <div>
          <h2>Receba novidades por e-mail</h2>
          <p>Dicas de cashback e ofertas exclusivas direto na sua caixa de entrada.</p>
        </div>
        <form class="qblog-newsletter__form" @submit.prevent="subscribeNewsletter">
          <input v-model="email" type="email" placeholder="Seu melhor e-mail" required />
          <button type="submit">Assinar grátis</button>
        </form>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ title: "Blog | Quanta Shop", description: "Dicas de cashback, finanças pessoais e novidades do ecossistema Quanta Shop." });

const email = ref('');
const activeCategory = ref('Todos');
const categories = ['Todos', 'Cashback', 'Finanças', 'Parceiros', 'Novidades'];

function subscribeNewsletter() {
  email.value = '';
}

const posts = [
  {
    id: 1,
    title: 'Como maximizar seu cashback em compras do dia a dia',
    excerpt: 'Descubra estratégias simples para acumular mais cashback toda vez que você compra em lojas parceiras da Quanta Shop.',
    img: 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80&auto=format&fit=crop',
    date: '18 mar 2026',
    readTime: '5 min de leitura',
    category: 'Cashback',
    slug: '/blog',
  },
  {
    id: 2,
    title: 'Educação financeira: faça seu dinheiro trabalhar por você',
    excerpt: 'Pequenas mudanças de hábito no consumo podem gerar uma renda extra relevante ao longo do ano com o cashback acumulado.',
    img: 'https://images.unsplash.com/photo-1579621970795-87facc2f976d?w=600&q=80&auto=format&fit=crop',
    date: '12 mar 2026',
    readTime: '7 min de leitura',
    category: 'Finanças',
    slug: '/blog',
  },
  {
    id: 3,
    title: 'Conheça os 10 parceiros mais usados em março',
    excerpt: 'Levantamos os estabelecimentos que geraram mais cashback para os usuários da Quanta Shop no mês de março de 2026.',
    img: 'https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=600&q=80&auto=format&fit=crop',
    date: '05 mar 2026',
    readTime: '4 min de leitura',
    category: 'Parceiros',
    slug: '/blog',
  },
  {
    id: 4,
    title: 'Novidade: cashback agora disponível em farmácias',
    excerpt: 'A Quanta Shop acaba de credenciar as maiores redes de farmácias do Brasil. Ganhe cashback nas suas compras de saúde.',
    img: 'https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?w=600&q=80&auto=format&fit=crop',
    date: '28 fev 2026',
    readTime: '3 min de leitura',
    category: 'Novidades',
    slug: '/blog',
  },
  {
    id: 5,
    title: '5 erros que impedem você de acumular cashback',
    excerpt: 'Muitos usuários perdem dinheiro sem perceber. Veja os erros mais comuns e como evitá-los para maximizar seus ganhos.',
    img: 'https://images.unsplash.com/photo-1554224155-6726b3ff858f?w=600&q=80&auto=format&fit=crop',
    date: '20 fev 2026',
    readTime: '6 min de leitura',
    category: 'Cashback',
    slug: '/blog',
  },
  {
    id: 6,
    title: 'Como ser agente Quanta Shop e criar renda recorrente',
    excerpt: 'Entenda como funciona o modelo de agentes da Quanta Shop e quanto você pode ganhar indicando lojas e consumidores.',
    img: 'https://images.unsplash.com/photo-1521737604893-d14cc237f11d?w=600&q=80&auto=format&fit=crop',
    date: '10 fev 2026',
    readTime: '8 min de leitura',
    category: 'Novidades',
    slug: '/seja-um-agente',
  },
];

const filteredPosts = computed(() =>
  activeCategory.value === 'Todos'
    ? posts
    : posts.filter(p => p.category === activeCategory.value)
);
</script>

<style scoped>
.qblog-page { font-family: 'Inter', 'Jost', sans-serif; }

.qblog-hero {
  background: linear-gradient(135deg, #0f232d 0%, #225F6B 60%, #2F7785 100%);
  padding: 72px 0 56px;
  text-align: center;
}
.qblog-hero__inner { position: relative; z-index: 1; }
.qblog-hero h1 { font-size: clamp(28px, 3.5vw, 48px); font-weight: 800; color: #fff; margin: 12px 0 14px; }
.qblog-accent { color: #98C73A; }
.qblog-hero p { font-size: 16px; color: rgba(255,255,255,0.72); max-width: 480px; margin: 0 auto; line-height: 1.6; }

.qblog-label {
  display: inline-block;
  font-size: 12px;
  font-weight: 600;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: #2F7785;
  background: rgba(47,119,133,0.08);
  padding: 4px 12px;
  border-radius: 999px;
  margin-bottom: 12px;
}
.qblog-label--white { color: #98C73A; background: rgba(152,199,58,0.12); }

/* Filters */
.qblog-filters { background: #fff; border-bottom: 1px solid #f0f0f0; padding: 16px 0; }
.qblog-cat-list { display: flex; gap: 8px; flex-wrap: wrap; }
.qblog-cat-btn {
  font-family: 'Inter','Jost',sans-serif;
  font-size: 13px;
  font-weight: 500;
  color: #6b7280;
  background: #f7f8fa;
  border: 1.5px solid transparent;
  border-radius: 999px;
  padding: 6px 16px;
  cursor: pointer;
  transition: all 0.2s;
}
.qblog-cat-btn:hover { color: #2F7785; background: rgba(47,119,133,0.06); }
.qblog-cat-btn--active { color: #2F7785; background: rgba(47,119,133,0.1); border-color: #2F7785; font-weight: 600; }

/* Grid */
.qblog-grid-section { padding: 60px 0; background: #f7f8fa; }
.qblog-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 28px; }
@media (max-width: 960px) { .qblog-grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 580px) { .qblog-grid { grid-template-columns: 1fr; } }

.qblog-card { background: #fff; border-radius: 16px; overflow: hidden; box-shadow: 0 2px 12px rgba(0,0,0,0.05); transition: transform 0.2s, box-shadow 0.2s; }
.qblog-card:hover { transform: translateY(-4px); box-shadow: 0 8px 28px rgba(0,0,0,0.1); }

.qblog-card__img-wrap { position: relative; aspect-ratio: 16/9; overflow: hidden; }
.qblog-card__img { width: 100%; height: 100%; object-fit: cover; transition: transform 0.3s; }
.qblog-card:hover .qblog-card__img { transform: scale(1.04); }
.qblog-card__cat {
  position: absolute;
  top: 12px;
  left: 14px;
  background: #2F7785;
  color: #fff;
  font-size: 11px;
  font-weight: 600;
  padding: 3px 10px;
  border-radius: 999px;
}

.qblog-card__body { padding: 20px 22px 24px; }
.qblog-card__meta { display: flex; gap: 12px; margin-bottom: 10px; }
.qblog-card__date, .qblog-card__time { font-size: 12px; color: #9ca3af; }
.qblog-card__time::before { content: '·'; margin-right: 12px; }
.qblog-card__title { font-size: 16px; font-weight: 700; color: #111827; margin-bottom: 10px; line-height: 1.45; }
.qblog-card__excerpt { font-size: 14px; color: #6b7280; line-height: 1.6; margin-bottom: 16px; }
.qblog-card__read {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: 13px;
  font-weight: 600;
  color: #2F7785;
  text-decoration: none;
  transition: gap 0.2s;
}
.qblog-card__read:hover { gap: 10px; }

.qblog-empty { text-align: center; padding: 60px 0; color: #9ca3af; font-size: 15px; }

/* Newsletter */
.qblog-newsletter {
  background: linear-gradient(135deg, #225F6B 0%, #2F7785 100%);
  padding: 64px 0;
}
.qblog-newsletter__inner { display: flex; align-items: center; justify-content: space-between; gap: 40px; flex-wrap: wrap; }
.qblog-newsletter__inner h2 { font-size: 24px; font-weight: 700; color: #fff; margin-bottom: 8px; }
.qblog-newsletter__inner p { font-size: 14px; color: rgba(255,255,255,0.7); }
.qblog-newsletter__form { display: flex; gap: 10px; flex-wrap: wrap; }
.qblog-newsletter__form input {
  flex: 1;
  min-width: 240px;
  background: rgba(255,255,255,0.12);
  border: 1.5px solid rgba(255,255,255,0.3);
  border-radius: 8px;
  padding: 12px 16px;
  font-family: 'Inter','Jost',sans-serif;
  font-size: 14px;
  color: #fff;
  outline: none;
  transition: border-color 0.2s;
}
.qblog-newsletter__form input::placeholder { color: rgba(255,255,255,0.5); }
.qblog-newsletter__form input:focus { border-color: rgba(255,255,255,0.7); }
.qblog-newsletter__form button {
  background: #98C73A;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 12px 24px;
  font-family: 'Inter','Jost',sans-serif;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
}
.qblog-newsletter__form button:hover { background: #7aad1f; }
</style>
