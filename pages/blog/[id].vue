<template>
  <div class="qblog-detail">

    <div v-if="loading" class="qblog-detail__loading">
      <div class="spinner-border text-secondary" />
    </div>

    <div v-else-if="!artigo" class="qblog-detail__notfound">
      <div class="container">
        <div class="qblog-detail__notfound-inner">
          <svg width="56" height="56" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.5"><circle cx="12" cy="12" r="10"/><path d="M12 8v4m0 4h.01"/></svg>
          <h2>Artigo não encontrado</h2>
          <p>O artigo que você procura não existe ou foi removido.</p>
          <nuxt-link to="/blog" class="qblog-btn-back">← Voltar ao Blog</nuxt-link>
        </div>
      </div>
    </div>

    <template v-else>
      <!-- Hero -->
      <section class="qblog-detail__hero" :style="artigo.imagemDestaque ? `background-image: url('${artigo.imagemDestaque}')` : ''">
        <div class="qblog-detail__hero-overlay" />
        <div class="container qblog-detail__hero-inner">
          <nuxt-link to="/blog" class="qblog-detail__back-top">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M19 12H5"/><path d="m12 5-7 7 7 7"/></svg>
            Blog
          </nuxt-link>
          <div class="qblog-detail__meta">
            <span v-if="artigo.categoria" class="qblog-detail__cat">{{ artigo.categoria }}</span>
            <span class="qblog-detail__date">{{ formatDate(artigo.dataPublicacao) }}</span>
            <span v-if="artigo.autor" class="qblog-detail__author">por {{ artigo.autor }}</span>
          </div>
          <h1 class="qblog-detail__title">{{ artigo.titulo }}</h1>
          <p v-if="artigo.resumo" class="qblog-detail__excerpt">{{ artigo.resumo }}</p>
        </div>
      </section>

      <!-- Conteúdo -->
      <section class="qblog-detail__body-section">
        <div class="container">
          <div class="qblog-detail__content-wrap">
            <article class="qblog-detail__content">
              <p v-for="(para, i) in paragrafos" :key="i" class="qblog-detail__para">{{ para }}</p>
            </article>
            <aside class="qblog-detail__sidebar">
              <div class="qblog-detail__sidebar-card">
                <h4>Outros artigos</h4>
                <ul class="qblog-detail__related">
                  <li v-for="rel in relacionados" :key="rel.id">
                    <nuxt-link :to="`/blog/${rel.id}`" class="qblog-detail__related-link">
                      <div class="qblog-detail__related-img-wrap">
                        <img v-if="rel.imagemDestaque" :src="rel.imagemDestaque" :alt="rel.titulo" />
                        <div v-else class="qblog-detail__related-img-placeholder" />
                      </div>
                      <div>
                        <p class="qblog-detail__related-title">{{ rel.titulo }}</p>
                        <p class="qblog-detail__related-date">{{ formatDate(rel.dataPublicacao) }}</p>
                      </div>
                    </nuxt-link>
                  </li>
                </ul>
                <nuxt-link to="/blog" class="qblog-detail__all-link">Ver todos os artigos →</nuxt-link>
              </div>
            </aside>
          </div>
        </div>
      </section>

      <!-- CTA -->
      <section class="qblog-detail__cta">
        <div class="container qblog-detail__cta-inner">
          <div>
            <h3>Receba mais conteúdo como este</h3>
            <p>Assine e receba dicas de cashback e novidades direto no seu e-mail.</p>
          </div>
          <nuxt-link to="/blog" class="qblog-btn-back">← Voltar ao Blog</nuxt-link>
        </div>
      </section>
    </template>

  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const LS_KEY = 'qs_blog_artigos';

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
  try { return JSON.parse(localStorage.getItem(LS_KEY) || '[]'); } catch { return []; }
}

function formatDate(d?: string | null) {
  if (!d) return '';
  try {
    return new Date(d).toLocaleDateString('pt-BR', { day: '2-digit', month: 'long', year: 'numeric' });
  } catch { return d; }
}

const paragrafos = computed(() => {
  if (!artigo.value?.conteudo) return [];
  return artigo.value.conteudo.split('\n').filter(p => p.trim().length > 0);
});

const relacionados = computed(() =>
  todos.value
    .filter(a => a.publicado && a.id !== artigo.value?.id)
    .slice(0, 4)
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

definePageMeta({ layout: 'layout-home' });
</script>

<style scoped>
.qblog-detail { font-family: 'Inter', 'Jost', sans-serif; }

/* Loading / Not found */
.qblog-detail__loading { display: flex; align-items: center; justify-content: center; min-height: 320px; }
.qblog-detail__notfound { padding: 80px 0; text-align: center; }
.qblog-detail__notfound-inner { display: flex; flex-direction: column; align-items: center; gap: 16px; }
.qblog-detail__notfound-inner h2 { font-size: 28px; font-weight: 700; color: #225F6B; }
.qblog-detail__notfound-inner p { color: #6b7280; font-size: 16px; }

/* Hero */
.qblog-detail__hero {
  position: relative;
  min-height: 480px;
  background-color: #0f232d;
  background-size: cover;
  background-position: center;
  display: flex;
  align-items: flex-end;
  padding-bottom: 0;
}
.qblog-detail__hero-overlay {
  position: absolute;
  inset: 0;
  background: linear-gradient(to bottom, rgba(15,35,45,0.35) 0%, rgba(15,35,45,0.85) 60%, #0f232d 100%);
}
.qblog-detail__hero-inner {
  position: relative;
  z-index: 1;
  padding: 80px 0 56px;
  max-width: 820px;
}
.qblog-detail__back-top {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  color: rgba(255,255,255,0.65);
  font-size: 13px;
  font-weight: 500;
  text-decoration: none;
  margin-bottom: 24px;
  transition: color 0.2s;
}
.qblog-detail__back-top:hover { color: #98C73A; }
.qblog-detail__meta {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
  margin-bottom: 16px;
}
.qblog-detail__cat {
  background: #98C73A;
  color: #fff;
  font-size: 11px;
  font-weight: 700;
  padding: 3px 12px;
  border-radius: 999px;
  text-transform: uppercase;
  letter-spacing: 0.06em;
}
.qblog-detail__date, .qblog-detail__author {
  color: rgba(255,255,255,0.6);
  font-size: 13px;
}
.qblog-detail__author::before { content: '·'; margin-right: 12px; }
.qblog-detail__title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: clamp(28px, 4vw, 52px);
  font-weight: 800;
  color: #fff;
  line-height: 1.15;
  margin-bottom: 16px;
  letter-spacing: -0.02em;
}
.qblog-detail__excerpt {
  font-size: 17px;
  color: rgba(255,255,255,0.7);
  max-width: 620px;
  line-height: 1.7;
}

/* Body */
.qblog-detail__body-section {
  padding: 64px 0;
  background: #f7f8fa;
}
.qblog-detail__content-wrap {
  display: grid;
  grid-template-columns: 1fr 300px;
  gap: 48px;
  align-items: start;
}
@media (max-width: 900px) {
  .qblog-detail__content-wrap { grid-template-columns: 1fr; }
}

/* Article content */
.qblog-detail__content {
  background: #fff;
  border-radius: 20px;
  padding: 48px 52px;
  box-shadow: 0 2px 20px rgba(0,0,0,0.06);
}
@media (max-width: 600px) { .qblog-detail__content { padding: 32px 24px; } }
.qblog-detail__para {
  font-size: 17px;
  color: #1f2937;
  line-height: 1.85;
  margin-bottom: 24px;
}
.qblog-detail__para:last-child { margin-bottom: 0; }

/* Sidebar */
.qblog-detail__sidebar-card {
  background: #fff;
  border-radius: 16px;
  padding: 28px 24px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.05);
  position: sticky;
  top: 24px;
}
.qblog-detail__sidebar-card h4 {
  font-family: 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 700;
  color: #225F6B;
  margin-bottom: 20px;
  padding-bottom: 12px;
  border-bottom: 2px solid #f0f0f0;
}
.qblog-detail__related { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 14px; }
.qblog-detail__related-link {
  display: flex;
  gap: 12px;
  text-decoration: none;
  align-items: flex-start;
  transition: opacity 0.2s;
}
.qblog-detail__related-link:hover { opacity: 0.75; }
.qblog-detail__related-img-wrap {
  width: 56px;
  height: 56px;
  border-radius: 8px;
  overflow: hidden;
  flex-shrink: 0;
}
.qblog-detail__related-img-wrap img { width: 100%; height: 100%; object-fit: cover; }
.qblog-detail__related-img-placeholder { width: 100%; height: 100%; background: linear-gradient(135deg, #2F7785, #98C73A); }
.qblog-detail__related-title {
  font-size: 13px;
  font-weight: 600;
  color: #111827;
  line-height: 1.4;
  margin-bottom: 4px;
}
.qblog-detail__related-date { font-size: 11px; color: #9ca3af; }
.qblog-detail__all-link {
  display: block;
  margin-top: 20px;
  text-align: center;
  font-size: 13px;
  font-weight: 600;
  color: #2F7785;
  text-decoration: none;
  padding: 10px;
  border: 1.5px solid rgba(47,119,133,0.25);
  border-radius: 8px;
  transition: all 0.2s;
}
.qblog-detail__all-link:hover { background: rgba(47,119,133,0.06); border-color: #2F7785; }

/* CTA */
.qblog-detail__cta {
  background: linear-gradient(135deg, #225F6B 0%, #2F7785 100%);
  padding: 56px 0;
}
.qblog-detail__cta-inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 32px;
  flex-wrap: wrap;
}
.qblog-detail__cta-inner h3 {
  font-family: 'Jost', sans-serif;
  font-size: 22px;
  font-weight: 700;
  color: #fff;
  margin-bottom: 6px;
}
.qblog-detail__cta-inner p { font-size: 14px; color: rgba(255,255,255,0.7); }

/* Buttons */
.qblog-btn-back {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: rgba(255,255,255,0.15);
  color: #fff;
  border: 1.5px solid rgba(255,255,255,0.35);
  border-radius: 10px;
  padding: 12px 24px;
  font-size: 14px;
  font-weight: 600;
  text-decoration: none;
  white-space: nowrap;
  transition: all 0.2s;
}
.qblog-btn-back:hover { background: rgba(255,255,255,0.25); border-color: rgba(255,255,255,0.6); color: #fff; }
</style>
