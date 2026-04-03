<template>
  <div class="qblog-page">

    <!-- Hero -->
    <section class="qblog-hero">
      <div class="container qblog-hero__inner">
        <span class="qblog-label">Conteúdo e dicas</span>
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

    <section class="qblog-main">
      <div class="container">

        <!-- Artigo em destaque -->
        <div v-if="featuredPost && activeCategory === 'Todos'" class="qblog-featured">
          <nuxt-link :to="`/blog/${featuredPost.id}`" class="qblog-featured__link">
            <div class="qblog-featured__img-wrap">
              <img :src="featuredPost.img" :alt="featuredPost.title" class="qblog-featured__img" loading="eager" />
              <div class="qblog-featured__overlay" />
              <div class="qblog-featured__content">
                <span class="qblog-featured__cat">{{ featuredPost.category }}</span>
                <h2 class="qblog-featured__title">{{ featuredPost.title }}</h2>
                <p class="qblog-featured__excerpt">{{ featuredPost.excerpt }}</p>
                <div class="qblog-featured__meta">
                  <span v-if="featuredPost.date">{{ featuredPost.date }}</span>
                  <span v-if="featuredPost.readTime" class="qblog-featured__sep">·</span>
                  <span v-if="featuredPost.readTime">{{ featuredPost.readTime }}</span>
                </div>
                <span class="qblog-featured__cta">
                  Ler artigo completo
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14"/><path d="m12 5 7 7-7 7"/></svg>
                </span>
              </div>
            </div>
          </nuxt-link>
        </div>

        <!-- Grid de artigos -->
        <div v-if="gridPosts.length > 0" class="qblog-grid">
          <article v-for="post in gridPosts" :key="post.id" class="qblog-card">
            <nuxt-link :to="`/blog/${post.id}`" class="qblog-card__img-link">
              <div class="qblog-card__img-wrap">
                <img :src="post.img" :alt="post.title" class="qblog-card__img" loading="lazy" />
                <span class="qblog-card__cat">{{ post.category }}</span>
              </div>
            </nuxt-link>
            <div class="qblog-card__body">
              <div class="qblog-card__meta">
                <span class="qblog-card__date">{{ post.date }}</span>
                <span v-if="post.readTime" class="qblog-card__time">{{ post.readTime }}</span>
              </div>
              <h2 class="qblog-card__title">{{ post.title }}</h2>
              <p class="qblog-card__excerpt">{{ post.excerpt }}</p>
              <nuxt-link :to="`/blog/${post.id}`" class="qblog-card__read">
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
import { ref, computed, onMounted } from 'vue';
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ title: "Blog | Quanta Shop", description: "Dicas de cashback, finanças pessoais e novidades do ecossistema Quanta Shop.", canonical: "https://quantashop.com.br/blog" });

const email = ref('');
const activeCategory = ref('Todos');

interface Post {
  id: number;
  title: string;
  excerpt: string;
  img: string;
  date: string;
  readTime: string;
  category: string;
}

const fallbackPosts: Post[] = [
  { id: 1, title: 'Como maximizar seu cashback em compras do dia a dia', excerpt: 'Descubra estratégias simples para acumular mais cashback toda vez que você compra em lojas parceiras da Quanta Shop.', img: 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=800&q=80&auto=format&fit=crop', date: '18 mar 2026', readTime: '5 min de leitura', category: 'Cashback' },
  { id: 2, title: 'Educação financeira: faça seu dinheiro trabalhar por você', excerpt: 'Pequenas mudanças de hábito no consumo podem gerar uma renda extra relevante ao longo do ano com o cashback acumulado.', img: 'https://images.unsplash.com/photo-1579621970795-87facc2f976d?w=800&q=80&auto=format&fit=crop', date: '12 mar 2026', readTime: '7 min de leitura', category: 'Finanças' },
  { id: 3, title: 'Conheça os 10 parceiros mais usados em março', excerpt: 'Levantamos os estabelecimentos que geraram mais cashback para os usuários da Quanta Shop no mês de março de 2026.', img: 'https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=800&q=80&auto=format&fit=crop', date: '05 mar 2026', readTime: '4 min de leitura', category: 'Parceiros' },
  { id: 4, title: 'Novidade: cashback agora disponível em farmácias', excerpt: 'A Quanta Shop acaba de credenciar as maiores redes de farmácias do Brasil. Ganhe cashback nas suas compras de saúde.', img: 'https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?w=800&q=80&auto=format&fit=crop', date: '28 fev 2026', readTime: '3 min de leitura', category: 'Novidades' },
  { id: 5, title: '5 erros que impedem você de acumular cashback', excerpt: 'Muitos usuários perdem dinheiro sem perceber. Veja os erros mais comuns e como evitá-los para maximizar seus ganhos.', img: 'https://images.unsplash.com/photo-1554224155-6726b3ff858f?w=800&q=80&auto=format&fit=crop', date: '20 fev 2026', readTime: '6 min de leitura', category: 'Cashback' },
  { id: 6, title: 'Como ser agente Quanta Shop e criar renda recorrente', excerpt: 'Entenda como funciona o modelo de agentes da Quanta Shop e quanto você pode ganhar indicando lojas e consumidores.', img: 'https://images.unsplash.com/photo-1521737604893-d14cc237f11d?w=800&q=80&auto=format&fit=crop', date: '10 fev 2026', readTime: '8 min de leitura', category: 'Novidades' },
];

const posts = ref<Post[]>(fallbackPosts);
const categories = computed(() => {
  const cats = new Set(posts.value.map(p => p.category));
  return ['Todos', ...Array.from(cats)];
});

function subscribeNewsletter() { email.value = ''; }

function formatDate(d?: string | null) {
  if (!d) return '';
  try { return new Date(d).toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', year: 'numeric' }); } catch { return d; }
}

onMounted(() => {
  try {
    const raw = localStorage.getItem('qs_blog_artigos');
    if (raw) {
      const artigos = JSON.parse(raw) as Array<{
        id: number; titulo: string; resumo?: string; conteudo: string;
        imagemDestaque?: string; categoria?: string; dataPublicacao?: string;
        publicado: boolean;
      }>;
      const publicados = artigos
        .filter(a => a.publicado)
        .sort((a, b) => {
          const da = a.dataPublicacao ? new Date(a.dataPublicacao).getTime() : 0;
          const db = b.dataPublicacao ? new Date(b.dataPublicacao).getTime() : 0;
          return db - da;
        })
        .map(a => ({
          id: a.id,
          title: a.titulo,
          excerpt: a.resumo || a.conteudo.substring(0, 160) + '...',
          img: a.imagemDestaque || 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=800&q=80&auto=format&fit=crop',
          date: formatDate(a.dataPublicacao),
          readTime: '',
          category: a.categoria || 'Geral',
        }));
      if (publicados.length > 0) posts.value = publicados;
    }
  } catch {}
});

const filteredPosts = computed(() =>
  activeCategory.value === 'Todos'
    ? posts.value
    : posts.value.filter(p => p.category === activeCategory.value)
);

const featuredPost = computed(() => filteredPosts.value[0] ?? null);
const gridPosts = computed(() =>
  activeCategory.value === 'Todos'
    ? filteredPosts.value.slice(1)
    : filteredPosts.value
);
</script>

<style scoped>
.qblog-page { font-family: 'Inter', 'Jost', sans-serif; }

/* Hero */
.qblog-hero {
  background: linear-gradient(135deg, #0f232d 0%, #225F6B 55%, #2F7785 100%);
  padding: 80px 0 64px;
  text-align: center;
}
.qblog-hero__inner { position: relative; z-index: 1; }
.qblog-label {
  display: inline-block;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: #98C73A;
  background: rgba(152,199,58,0.12);
  padding: 4px 14px;
  border-radius: 999px;
  margin-bottom: 16px;
}
.qblog-hero h1 { font-family: 'Jost', 'Inter', sans-serif; font-size: clamp(32px, 4vw, 56px); font-weight: 800; color: #fff; margin: 0 0 16px; letter-spacing: -0.02em; }
.qblog-accent { color: #98C73A; }
.qblog-hero p { font-size: 17px; color: rgba(255,255,255,0.7); max-width: 520px; margin: 0 auto; line-height: 1.65; }

/* Filters */
.qblog-filters { background: #fff; border-bottom: 1px solid #eee; padding: 18px 0; position: sticky; top: 0; z-index: 10; }
.qblog-cat-list { display: flex; gap: 8px; flex-wrap: wrap; }
.qblog-cat-btn {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 500;
  color: #6b7280;
  background: #f4f5f7;
  border: 1.5px solid transparent;
  border-radius: 999px;
  padding: 7px 18px;
  cursor: pointer;
  transition: all 0.2s;
  white-space: nowrap;
}
.qblog-cat-btn:hover { color: #2F7785; background: rgba(47,119,133,0.07); }
.qblog-cat-btn--active { color: #fff; background: #2F7785; border-color: #2F7785; font-weight: 600; }

/* Main */
.qblog-main { padding: 56px 0 72px; background: #f4f5f7; }

/* Featured */
.qblog-featured { margin-bottom: 40px; }
.qblog-featured__link { display: block; text-decoration: none; border-radius: 20px; overflow: hidden; }
.qblog-featured__img-wrap {
  position: relative;
  height: clamp(320px, 50vw, 520px);
  overflow: hidden;
}
.qblog-featured__img { width: 100%; height: 100%; object-fit: cover; transition: transform 0.5s ease; }
.qblog-featured__link:hover .qblog-featured__img { transform: scale(1.03); }
.qblog-featured__overlay {
  position: absolute;
  inset: 0;
  background: linear-gradient(to top, rgba(15,35,45,0.92) 0%, rgba(15,35,45,0.5) 50%, rgba(15,35,45,0.1) 100%);
}
.qblog-featured__content {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 40px 44px;
}
@media (max-width: 600px) { .qblog-featured__content { padding: 24px 20px; } }
.qblog-featured__cat {
  display: inline-block;
  background: #98C73A;
  color: #fff;
  font-size: 11px;
  font-weight: 700;
  padding: 3px 12px;
  border-radius: 999px;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  margin-bottom: 12px;
}
.qblog-featured__title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: clamp(22px, 3vw, 36px);
  font-weight: 800;
  color: #fff;
  margin-bottom: 12px;
  line-height: 1.2;
  letter-spacing: -0.01em;
}
.qblog-featured__excerpt { font-size: 15px; color: rgba(255,255,255,0.72); max-width: 600px; line-height: 1.6; margin-bottom: 16px; }
.qblog-featured__meta { font-size: 13px; color: rgba(255,255,255,0.5); margin-bottom: 16px; }
.qblog-featured__sep { margin: 0 6px; }
.qblog-featured__cta {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  font-size: 14px;
  font-weight: 700;
  color: #98C73A;
  transition: gap 0.2s;
}
.qblog-featured__link:hover .qblog-featured__cta { gap: 12px; }

/* Grid */
.qblog-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 28px;
}
@media (max-width: 960px) { .qblog-grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 560px) { .qblog-grid { grid-template-columns: 1fr; } }

.qblog-card {
  background: #fff;
  border-radius: 16px;
  overflow: hidden;
  box-shadow: 0 2px 12px rgba(0,0,0,0.05);
  transition: transform 0.25s, box-shadow 0.25s;
  display: flex;
  flex-direction: column;
}
.qblog-card:hover { transform: translateY(-5px); box-shadow: 0 12px 32px rgba(0,0,0,0.1); }

.qblog-card__img-link { display: block; text-decoration: none; }
.qblog-card__img-wrap { position: relative; aspect-ratio: 16/9; overflow: hidden; }
.qblog-card__img { width: 100%; height: 100%; object-fit: cover; transition: transform 0.35s; }
.qblog-card:hover .qblog-card__img { transform: scale(1.05); }
.qblog-card__cat {
  position: absolute;
  top: 12px;
  left: 14px;
  background: #2F7785;
  color: #fff;
  font-size: 10px;
  font-weight: 700;
  padding: 3px 10px;
  border-radius: 999px;
  text-transform: uppercase;
  letter-spacing: 0.07em;
}

.qblog-card__body { padding: 22px 24px 26px; display: flex; flex-direction: column; flex: 1; }
.qblog-card__meta { display: flex; gap: 8px; align-items: center; margin-bottom: 10px; }
.qblog-card__date { font-size: 12px; color: #9ca3af; }
.qblog-card__time { font-size: 12px; color: #9ca3af; }
.qblog-card__time::before { content: '·'; margin-right: 8px; }
.qblog-card__title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: 17px;
  font-weight: 700;
  color: #111827;
  margin-bottom: 10px;
  line-height: 1.4;
}
.qblog-card__excerpt { font-size: 14px; color: #6b7280; line-height: 1.65; margin-bottom: 18px; flex: 1; }
.qblog-card__read {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: 13px;
  font-weight: 700;
  color: #2F7785;
  text-decoration: none;
  transition: gap 0.2s;
  align-self: flex-start;
}
.qblog-card__read:hover { gap: 10px; color: #225F6B; }

.qblog-empty { text-align: center; padding: 80px 0; color: #9ca3af; font-size: 16px; }

/* Newsletter */
.qblog-newsletter {
  background: linear-gradient(135deg, #225F6B 0%, #2F7785 100%);
  padding: 72px 0;
}
.qblog-newsletter__inner { display: flex; align-items: center; justify-content: space-between; gap: 40px; flex-wrap: wrap; }
.qblog-newsletter__inner h2 { font-family: 'Jost', sans-serif; font-size: 26px; font-weight: 700; color: #fff; margin-bottom: 8px; }
.qblog-newsletter__inner p { font-size: 15px; color: rgba(255,255,255,0.7); }
.qblog-newsletter__form { display: flex; gap: 10px; flex-wrap: wrap; }
.qblog-newsletter__form input {
  flex: 1;
  min-width: 260px;
  background: rgba(255,255,255,0.13);
  border: 1.5px solid rgba(255,255,255,0.3);
  border-radius: 10px;
  padding: 13px 18px;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  color: #fff;
  outline: none;
  transition: border-color 0.2s;
}
.qblog-newsletter__form input::placeholder { color: rgba(255,255,255,0.45); }
.qblog-newsletter__form input:focus { border-color: rgba(255,255,255,0.7); }
.qblog-newsletter__form button {
  background: #98C73A;
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 13px 28px;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 700;
  cursor: pointer;
  transition: background 0.2s;
  white-space: nowrap;
}
.qblog-newsletter__form button:hover { background: #7aad1f; }
</style>
