<template>
  <div class="qbd">

    <!-- Loading -->
    <div v-if="loading" class="qbd-loading">
      <div class="qbd-spinner" />
    </div>

    <!-- Not found -->
    <div v-else-if="!artigo" class="qbd-notfound">
      <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#aeaeb2" stroke-width="1.5"><circle cx="12" cy="12" r="10"/><path d="M12 8v4m0 4h.01"/></svg>
      <h2>Artigo não encontrado</h2>
      <p>O artigo que você procura não existe ou foi removido.</p>
      <NuxtLink to="/blog" class="qbd-btn-back">← Voltar ao Blog</NuxtLink>
    </div>

    <template v-else>

      <!-- Hero -->
      <header class="qbd-hero" :class="{ 'qbd-hero--img': !!artigo.imagemDestaque }">
        <div v-if="artigo.imagemDestaque" class="qbd-hero__bg">
          <img :src="artigo.imagemDestaque" :alt="artigo.titulo" class="qbd-hero__bg-img" />
          <div class="qbd-hero__scrim" />
        </div>
        <div class="container qbd-hero__inner">
          <NuxtLink to="/blog" class="qbd-back-link">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M19 12H5"/><path d="m12 5-7 7 7 7"/></svg>
            Blog
          </NuxtLink>
          <div class="qbd-hero__meta">
            <span v-if="artigo.categoria" class="qbd-tag">{{ artigo.categoria }}</span>
            <span class="qbd-hero__date">{{ formatDate(artigo.dataPublicacao) }}</span>
            <span v-if="artigo.autor" class="qbd-hero__author">{{ artigo.autor }}</span>
          </div>
          <h1 class="qbd-hero__title">{{ artigo.titulo }}</h1>
          <p v-if="artigo.resumo" class="qbd-hero__lead">{{ artigo.resumo }}</p>
        </div>
      </header>

      <!-- Body -->
      <section class="qbd-body">
        <div class="container">
          <div class="qbd-layout">

            <!-- Artigo -->
            <article class="qbd-article">
              <p v-for="(para, i) in paragrafos" :key="i" class="qbd-para">{{ para }}</p>
            </article>

            <!-- Sidebar -->
            <aside class="qbd-sidebar">
              <div class="qbd-sidebar__card" v-if="relacionados.length > 0">
                <h4 class="qbd-sidebar__title">Mais artigos</h4>
                <ul class="qbd-related">
                  <li v-for="rel in relacionados" :key="rel.id">
                    <NuxtLink :to="`/blog/${rel.id}`" class="qbd-related__item">
                      <div class="qbd-related__thumb">
                        <img v-if="rel.imagemDestaque" :src="rel.imagemDestaque" :alt="rel.titulo" />
                        <div v-else class="qbd-related__thumb-placeholder" />
                      </div>
                      <div class="qbd-related__text">
                        <p class="qbd-related__name">{{ rel.titulo }}</p>
                        <p class="qbd-related__date">{{ formatDate(rel.dataPublicacao) }}</p>
                      </div>
                    </NuxtLink>
                  </li>
                </ul>
                <NuxtLink to="/blog" class="qbd-sidebar__all">Ver todos os artigos →</NuxtLink>
              </div>
              <div class="qbd-sidebar__card" v-else>
                <NuxtLink to="/blog" class="qbd-btn-back qbd-btn-back--outline">← Voltar ao Blog</NuxtLink>
              </div>
            </aside>

          </div>
        </div>
      </section>

      <!-- Footer CTA -->
      <section class="qbd-cta">
        <div class="container qbd-cta__inner">
          <div>
            <h3>Continue lendo</h3>
            <p>Mais dicas de cashback e finanças no nosso blog.</p>
          </div>
          <NuxtLink to="/blog" class="qbd-btn-back">← Voltar ao Blog</NuxtLink>
        </div>
      </section>

    </template>

  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRoute } from 'vue-router';

definePageMeta({ layout: 'layout-home' });

const route = useRoute();

interface Artigo {
  id: number;
  titulo: string;
  slug: string;
  resumo?: string;
  conteudo: string;
  imagemDestaque?: string;
  categoria?: string;
  autor?: string;
  dataPublicacao?: string;
  publicado: boolean;
  destaque: boolean;
}

const loading = ref(true);
const artigo = ref<Artigo | null>(null);
const todos = ref<Artigo[]>([]);

function lsCarregar(): Artigo[] {
  try { return JSON.parse(localStorage.getItem('qs_blog_artigos') || '[]'); } catch { return []; }
}

function formatDate(d?: string | null) {
  if (!d) return '';
  try { return new Date(d).toLocaleDateString('pt-BR', { day: '2-digit', month: 'long', year: 'numeric' }); } catch { return d; }
}

const paragrafos = computed(() => {
  if (!artigo.value?.conteudo) return [];
  return artigo.value.conteudo.split('\n').filter(p => p.trim().length > 0);
});

const relacionados = computed(() =>
  todos.value.filter(a => a.publicado && a.id !== artigo.value?.id).slice(0, 4)
);

onMounted(() => {
  const id = Number(route.params.id);
  const lista = lsCarregar();
  todos.value = lista;
  artigo.value = lista.find(a => a.id === id) || null;
  loading.value = false;

  if (artigo.value) {
    useSeoMeta({
      title: artigo.value.titulo + ' | Blog Quanta Shop',
      description: artigo.value.resumo || artigo.value.conteudo.substring(0, 160),
    });
  }
});
</script>

<style scoped>
/* ── Base ─────────────────────────────────────────────── */
.qbd {
  font-family: -apple-system, BlinkMacSystemFont, 'Inter', 'Segoe UI', sans-serif;
  -webkit-font-smoothing: antialiased;
  background: #f5f5f7;
  min-height: 100vh;
}

/* ── Loading ──────────────────────────────────────────── */
.qbd-loading {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 60vh;
}
.qbd-spinner {
  width: 36px;
  height: 36px;
  border: 3px solid rgba(47,119,133,0.2);
  border-top-color: #2F7785;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}
@keyframes spin { to { transform: rotate(360deg); } }

/* ── Not Found ────────────────────────────────────────── */
.qbd-notfound {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 16px;
  min-height: 70vh;
  text-align: center;
  padding: 48px 24px;
}
.qbd-notfound h2 { font-size: 24px; font-weight: 700; color: #1d1d1f; }
.qbd-notfound p { font-size: 15px; color: #6e6e73; }

/* ── Hero ─────────────────────────────────────────────── */
.qbd-hero {
  background: #000;
  padding: 40px 0 72px;
}
.qbd-hero--img {
  position: relative;
  min-height: 540px;
  display: flex;
  align-items: flex-end;
  padding: 0;
}
.qbd-hero__bg {
  position: absolute;
  inset: 0;
  overflow: hidden;
}
.qbd-hero__bg-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
  display: block;
}
.qbd-hero__scrim {
  position: absolute;
  inset: 0;
  background: linear-gradient(
    to bottom,
    rgba(0,0,0,0.2) 0%,
    rgba(0,0,0,0.65) 55%,
    rgba(0,0,0,0.92) 100%
  );
}
.qbd-hero__inner {
  position: relative;
  z-index: 1;
  padding-top: 48px;
  padding-bottom: 72px;
  max-width: 820px;
}
.qbd-hero--img .qbd-hero__inner {
  padding-top: 48px;
  padding-bottom: 72px;
}

/* Back link */
.qbd-back-link {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: 13px;
  font-weight: 500;
  color: rgba(255,255,255,0.5);
  text-decoration: none;
  margin-bottom: 28px;
  transition: color 0.2s;
}
.qbd-back-link:hover { color: #98C73A; }

/* Meta */
.qbd-hero__meta {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
  margin-bottom: 16px;
}
.qbd-tag {
  display: inline-block;
  font-size: 10px;
  font-weight: 700;
  letter-spacing: 0.07em;
  text-transform: uppercase;
  color: #fff;
  background: #2F7785;
  border-radius: 980px;
  padding: 3px 10px;
}
.qbd-hero__date,
.qbd-hero__author {
  font-size: 13px;
  color: rgba(255,255,255,0.45);
}
.qbd-hero__author::before { content: '·'; margin-right: 8px; }

/* Title */
.qbd-hero__title {
  font-size: clamp(28px, 4.5vw, 56px);
  font-weight: 700;
  letter-spacing: -0.03em;
  color: #f5f5f7;
  line-height: 1.1;
  margin-bottom: 20px;
}
.qbd-hero__lead {
  font-size: 18px;
  color: rgba(245,245,247,0.55);
  line-height: 1.65;
  max-width: 620px;
  font-weight: 400;
}

/* ── Body ─────────────────────────────────────────────── */
.qbd-body { padding: 64px 0 80px; }
.qbd-layout {
  display: grid;
  grid-template-columns: 1fr 300px;
  gap: 48px;
  align-items: start;
}
@media (max-width: 900px) { .qbd-layout { grid-template-columns: 1fr; } }

/* Article */
.qbd-article {
  background: #fff;
  border-radius: 20px;
  padding: 56px 60px;
  box-shadow: 0 1px 0 rgba(0,0,0,0.04), 0 8px 32px rgba(0,0,0,0.06);
}
@media (max-width: 640px) { .qbd-article { padding: 36px 28px; } }

.qbd-para {
  font-size: 17px;
  color: #1d1d1f;
  line-height: 1.88;
  margin-bottom: 28px;
  letter-spacing: -0.005em;
}
.qbd-para:last-child { margin-bottom: 0; }

/* Sidebar */
.qbd-sidebar__card {
  background: #fff;
  border-radius: 18px;
  padding: 28px 24px;
  box-shadow: 0 1px 0 rgba(0,0,0,0.04), 0 4px 16px rgba(0,0,0,0.05);
  position: sticky;
  top: 24px;
}
.qbd-sidebar__title {
  font-size: 13px;
  font-weight: 700;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  color: #aeaeb2;
  margin-bottom: 20px;
  padding-bottom: 14px;
  border-bottom: 1px solid #f0f0f0;
}
.qbd-related {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.qbd-related__item {
  display: flex;
  gap: 12px;
  text-decoration: none;
  align-items: flex-start;
  transition: opacity 0.18s;
}
.qbd-related__item:hover { opacity: 0.65; }
.qbd-related__thumb {
  width: 52px;
  height: 52px;
  border-radius: 10px;
  overflow: hidden;
  flex-shrink: 0;
}
.qbd-related__thumb img { width: 100%; height: 100%; object-fit: cover; display: block; }
.qbd-related__thumb-placeholder { width: 100%; height: 100%; background: linear-gradient(135deg, #2F7785, #98C73A); }
.qbd-related__name {
  font-size: 13px;
  font-weight: 600;
  color: #1d1d1f;
  line-height: 1.35;
  margin-bottom: 4px;
}
.qbd-related__date { font-size: 11px; color: #aeaeb2; }
.qbd-sidebar__all {
  display: block;
  margin-top: 20px;
  text-align: center;
  font-size: 12px;
  font-weight: 600;
  color: #2F7785;
  text-decoration: none;
  padding: 10px;
  border: 1px solid rgba(47,119,133,0.2);
  border-radius: 10px;
  transition: all 0.18s;
}
.qbd-sidebar__all:hover { background: rgba(47,119,133,0.05); border-color: #2F7785; }

/* ── CTA ──────────────────────────────────────────────── */
.qbd-cta {
  background: #1d1d1f;
  padding: 64px 0;
}
.qbd-cta__inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 32px;
  flex-wrap: wrap;
}
.qbd-cta__inner h3 {
  font-size: 22px;
  font-weight: 700;
  letter-spacing: -0.02em;
  color: #f5f5f7;
  margin-bottom: 6px;
}
.qbd-cta__inner p { font-size: 14px; color: rgba(245,245,247,0.4); }

/* ── Buttons ──────────────────────────────────────────── */
.qbd-btn-back {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: rgba(255,255,255,0.1);
  color: #f5f5f7;
  border: 1px solid rgba(255,255,255,0.2);
  border-radius: 980px;
  padding: 12px 24px;
  font-size: 14px;
  font-weight: 600;
  text-decoration: none;
  white-space: nowrap;
  transition: all 0.2s;
}
.qbd-btn-back:hover { background: rgba(255,255,255,0.18); color: #f5f5f7; }

.qbd-btn-back--outline {
  background: transparent;
  color: #2F7785;
  border-color: rgba(47,119,133,0.35);
  width: 100%;
  justify-content: center;
}
.qbd-btn-back--outline:hover { background: rgba(47,119,133,0.05); color: #2F7785; border-color: #2F7785; }
</style>
