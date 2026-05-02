<template>
  <div class="qb">

    <!-- Hero Apple-style -->
    <section class="qb-hero">
      <div class="qb-hero__inner">
        <p class="qb-hero__eyebrow">Insights &amp; ideias</p>
        <h1 class="qb-hero__title">Blog <em>Quanta Shop</em></h1>
        <p class="qb-hero__sub">Cashback, finanças e novidades para você ganhar mais todos os dias.</p>
      </div>
    </section>

    <!-- Filtros -->
    <nav class="qb-nav">
      <div class="container">
        <div class="qb-nav__list">
          <button
            v-for="cat in categories"
            :key="cat"
            class="qb-nav__btn"
            :class="{ active: activeCategory === cat }"
            @click="activeCategory = cat"
          >{{ cat }}</button>
        </div>
      </div>
    </nav>

    <!-- Conteúdo -->
    <main class="qb-main">
      <div class="container">

        <!-- Hero card em destaque -->
        <NuxtLink
          v-if="featuredPost && activeCategory === 'Todos'"
          :to="`/blog/${featuredPost.slug}`"
          class="qb-hero-card"
        >
          <div class="qb-hero-card__img-wrap">
            <img :src="featuredPost.img" :alt="featuredPost.title" class="qb-hero-card__img" loading="eager" />
            <div class="qb-hero-card__scrim" />
          </div>
          <div class="qb-hero-card__body">
            <span class="qb-tag">{{ featuredPost.category }}</span>
            <h2 class="qb-hero-card__title">{{ featuredPost.title }}</h2>
            <p class="qb-hero-card__excerpt">{{ featuredPost.excerpt }}</p>
            <div class="qb-hero-card__foot">
              <span class="qb-hero-card__date">{{ featuredPost.date }}</span>
              <span class="qb-hero-card__cta">
                Ler artigo
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M5 12h14"/><path d="m12 5 7 7-7 7"/></svg>
              </span>
            </div>
          </div>
        </NuxtLink>

        <!-- Grid -->
        <div v-if="gridPosts.length > 0" class="qb-grid">
          <NuxtLink
            v-for="post in gridPosts"
            :key="post.id"
            :to="`/blog/${post.slug}`"
            class="qb-card"
          >
            <div class="qb-card__img-wrap">
              <img :src="post.img" :alt="post.title" class="qb-card__img" loading="lazy" />
            </div>
            <div class="qb-card__body">
              <div class="qb-card__meta">
                <span class="qb-tag qb-tag--sm">{{ post.category }}</span>
                <span class="qb-card__date">{{ post.date }}</span>
              </div>
              <h2 class="qb-card__title">{{ post.title }}</h2>
              <p class="qb-card__excerpt">{{ post.excerpt }}</p>
              <span class="qb-card__read">
                Ler artigo
                <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14"/><path d="m12 5 7 7-7 7"/></svg>
              </span>
            </div>
          </NuxtLink>
        </div>

        <div v-if="filteredPosts.length === 0" class="qb-empty">
          Nenhum artigo nesta categoria ainda.
        </div>

      </div>
    </main>

    <!-- Newsletter -->
    <section class="qb-nl">
      <div class="container qb-nl__inner">
        <div class="qb-nl__copy">
          <h3>Fique por dentro</h3>
          <p>Novidades e dicas de cashback direto no seu e-mail. Grátis, sempre.</p>
        </div>
        <form class="qb-nl__form" @submit.prevent="email = ''">
          <input v-model="email" type="email" placeholder="seu@email.com" required />
          <button type="submit">Assinar</button>
        </form>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { useBlogStore } from '~/stores/blog';

definePageMeta({ layout: 'layout-home' });

const blogStore = useBlogStore();
const email = ref('');
const activeCategory = ref('Todos');

interface Post {
  id: string | number;
  slug: string;
  title: string;
  excerpt: string;
  img: string;
  date: string;
  readTime: string;
  category: string;
}

const posts = ref<Post[]>([]);

function fmt(d?: string | null) {
  if (!d) return '';
  try { return new Date(d).toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', year: 'numeric' }); } catch { return d; }
}

onMounted(() => {
  blogStore.carregarArtigos();
  posts.value = blogStore.artigos
    .filter(a => a.publicado)
    .sort((a, b) => {
      const da = a.dataPublicacao ? new Date(a.dataPublicacao).getTime() : 0;
      const db = b.dataPublicacao ? new Date(b.dataPublicacao).getTime() : 0;
      return db - da;
    })
    .map(a => ({
      id: a.id,
      slug: a.slug,
      title: a.titulo,
      excerpt: a.resumo || a.conteudo.substring(0, 140) + '…',
      img: a.imagemDestaque || 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=900&q=85&auto=format&fit=crop',
      date: fmt(a.dataPublicacao),
      readTime: '',
      category: a.category || 'Geral',
    }));
});

const categories = computed(() => ['Todos', ...Array.from(new Set(posts.value.map(p => p.category)))]);
const filteredPosts = computed(() =>
  activeCategory.value === 'Todos' ? posts.value : posts.value.filter(p => p.category === activeCategory.value)
);
const featuredPost = computed(() => filteredPosts.value[0] ?? null);
const gridPosts = computed(() =>
  activeCategory.value === 'Todos' ? filteredPosts.value.slice(1) : filteredPosts.value
);

useSeoMeta({
  title: 'Blog | Quanta Shop',
  description: 'Dicas de cashback, finanças pessoais e novidades do ecossistema Quanta Shop.',
});
</script>

<style scoped>
/* ── Reset / Base ─────────────────────────────────────── */
.qb {
  font-family: -apple-system, BlinkMacSystemFont, 'Inter', 'Segoe UI', sans-serif;
  -webkit-font-smoothing: antialiased;
  background: #f5f5f7;
}

/* ── Hero ─────────────────────────────────────────────── */
.qb-hero {
  background: #000;
  padding: 100px 24px 80px;
  text-align: center;
}
.qb-hero__inner { max-width: 680px; margin: 0 auto; }
.qb-hero__eyebrow {
  font-size: 13px;
  font-weight: 600;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: #98C73A;
  margin-bottom: 20px;
}
.qb-hero__title {
  font-size: clamp(40px, 6vw, 72px);
  font-weight: 700;
  letter-spacing: -0.04em;
  line-height: 1.05;
  color: #f5f5f7;
  margin-bottom: 20px;
}
.qb-hero__title em { font-style: normal; color: #98C73A; }
.qb-hero__sub {
  font-size: 18px;
  font-weight: 400;
  color: rgba(245,245,247,0.55);
  line-height: 1.6;
  max-width: 480px;
  margin: 0 auto;
}

/* ── Nav / Filtros ────────────────────────────────────── */
.qb-nav {
  background: rgba(245,245,247,0.85);
  backdrop-filter: saturate(180%) blur(20px);
  -webkit-backdrop-filter: saturate(180%) blur(20px);
  border-bottom: 1px solid rgba(0,0,0,0.08);
  position: sticky;
  top: 0;
  z-index: 20;
  padding: 0;
}
.qb-nav__list {
  display: flex;
  gap: 4px;
  padding: 12px 0;
  overflow-x: auto;
  scrollbar-width: none;
}
.qb-nav__list::-webkit-scrollbar { display: none; }
.qb-nav__btn {
  font-family: inherit;
  font-size: 13px;
  font-weight: 500;
  color: #6e6e73;
  background: transparent;
  border: none;
  border-radius: 980px;
  padding: 6px 16px;
  cursor: pointer;
  transition: all 0.15s;
  white-space: nowrap;
  flex-shrink: 0;
}
.qb-nav__btn:hover { color: #1d1d1f; background: rgba(0,0,0,0.05); }
.qb-nav__btn.active { color: #1d1d1f; background: #fff; box-shadow: 0 1px 4px rgba(0,0,0,0.12); font-weight: 600; }

/* ── Tags ─────────────────────────────────────────────── */
.qb-tag {
  display: inline-block;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: #fff;
  background: #2F7785;
  border-radius: 980px;
  padding: 3px 10px;
}
.qb-tag--sm { font-size: 10px; padding: 2px 8px; }

/* ── Main ─────────────────────────────────────────────── */
.qb-main { padding: 64px 0 80px; }

/* ── Hero Card ────────────────────────────────────────── */
.qb-hero-card {
  display: grid;
  grid-template-columns: 1fr 1fr;
  background: #fff;
  border-radius: 24px;
  overflow: hidden;
  text-decoration: none;
  margin-bottom: 40px;
  box-shadow: 0 2px 0 rgba(0,0,0,0.04), 0 8px 32px rgba(0,0,0,0.06);
  transition: box-shadow 0.3s, transform 0.3s;
  min-height: 400px;
}
.qb-hero-card:hover { transform: translateY(-3px); box-shadow: 0 12px 48px rgba(0,0,0,0.12); }
@media (max-width: 768px) { .qb-hero-card { grid-template-columns: 1fr; } }

.qb-hero-card__img-wrap { position: relative; overflow: hidden; min-height: 280px; }
.qb-hero-card__img { width: 100%; height: 100%; object-fit: cover; transition: transform 0.5s ease; display: block; }
.qb-hero-card:hover .qb-hero-card__img { transform: scale(1.04); }
.qb-hero-card__scrim {
  position: absolute;
  inset: 0;
  background: linear-gradient(to right, transparent 60%, rgba(0,0,0,0.04));
}

.qb-hero-card__body {
  padding: 48px 44px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 16px;
}
@media (max-width: 768px) { .qb-hero-card__body { padding: 32px 28px; } }

.qb-hero-card__title {
  font-size: clamp(22px, 2.4vw, 30px);
  font-weight: 700;
  letter-spacing: -0.025em;
  color: #1d1d1f;
  line-height: 1.2;
}
.qb-hero-card__excerpt {
  font-size: 15px;
  color: #6e6e73;
  line-height: 1.65;
  flex: 1;
}
.qb-hero-card__foot {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  margin-top: 8px;
}
.qb-hero-card__date { font-size: 12px; color: #aeaeb2; }
.qb-hero-card__cta {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: 13px;
  font-weight: 600;
  color: #2F7785;
  transition: gap 0.2s;
}
.qb-hero-card:hover .qb-hero-card__cta { gap: 10px; }

/* ── Grid ─────────────────────────────────────────────── */
.qb-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
}
@media (max-width: 1024px) { .qb-grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 580px) { .qb-grid { grid-template-columns: 1fr; } }

/* ── Card ─────────────────────────────────────────────── */
.qb-card {
  background: #fff;
  border-radius: 18px;
  overflow: hidden;
  text-decoration: none;
  display: flex;
  flex-direction: column;
  box-shadow: 0 1px 0 rgba(0,0,0,0.04), 0 4px 16px rgba(0,0,0,0.05);
  transition: transform 0.25s, box-shadow 0.25s;
}
.qb-card:hover { transform: translateY(-4px); box-shadow: 0 8px 40px rgba(0,0,0,0.10); }

.qb-card__img-wrap { aspect-ratio: 4/3; overflow: hidden; }
.qb-card__img { width: 100%; height: 100%; object-fit: cover; transition: transform 0.4s ease; display: block; }
.qb-card:hover .qb-card__img { transform: scale(1.05); }

.qb-card__body {
  padding: 24px 24px 28px;
  display: flex;
  flex-direction: column;
  flex: 1;
  gap: 10px;
}
.qb-card__meta { display: flex; align-items: center; gap: 10px; }
.qb-card__date { font-size: 11px; color: #aeaeb2; }

.qb-card__title {
  font-size: 16px;
  font-weight: 700;
  letter-spacing: -0.015em;
  color: #1d1d1f;
  line-height: 1.35;
}
.qb-card__excerpt {
  font-size: 13px;
  color: #6e6e73;
  line-height: 1.6;
  flex: 1;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
.qb-card__read {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  font-size: 12px;
  font-weight: 600;
  color: #2F7785;
  margin-top: 4px;
  transition: gap 0.2s;
}
.qb-card:hover .qb-card__read { gap: 8px; }

/* ── Empty ────────────────────────────────────────────── */
.qb-empty { text-align: center; padding: 80px 0; color: #aeaeb2; font-size: 15px; }

/* ── Newsletter ───────────────────────────────────────── */
.qb-nl {
  background: #1d1d1f;
  padding: 80px 0;
}
.qb-nl__inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 48px;
  flex-wrap: wrap;
}
.qb-nl__copy h3 {
  font-size: 26px;
  font-weight: 700;
  letter-spacing: -0.02em;
  color: #f5f5f7;
  margin-bottom: 8px;
}
.qb-nl__copy p { font-size: 15px; color: rgba(245,245,247,0.45); }

.qb-nl__form { display: flex; gap: 10px; flex-shrink: 0; }
.qb-nl__form input {
  font-family: inherit;
  font-size: 14px;
  color: #f5f5f7;
  background: rgba(255,255,255,0.08);
  border: 1px solid rgba(255,255,255,0.15);
  border-radius: 10px;
  padding: 13px 20px;
  width: 260px;
  outline: none;
  transition: border-color 0.2s;
}
.qb-nl__form input::placeholder { color: rgba(245,245,247,0.3); }
.qb-nl__form input:focus { border-color: rgba(255,255,255,0.4); }
.qb-nl__form button {
  font-family: inherit;
  font-size: 14px;
  font-weight: 600;
  color: #000;
  background: #98C73A;
  border: none;
  border-radius: 10px;
  padding: 13px 24px;
  cursor: pointer;
  transition: background 0.2s;
  white-space: nowrap;
}
.qb-nl__form button:hover { background: #82b030; }
@media (max-width: 640px) {
  .qb-nl__form { flex-direction: column; width: 100%; }
  .qb-nl__form input { width: 100%; }
}
</style>
