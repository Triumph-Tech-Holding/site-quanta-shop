<template>
  <section class="qs-social">
    <div class="container">
      <div class="qs-social__header">
        <span class="qs-social__label">{{ config.blog.label }}</span>
        <h2 class="qs-social__title">{{ config.blog.title }}</h2>
        <p class="qs-social__sub">{{ config.blog.subtitle }}</p>
      </div>

      <div class="qs-livefeed">
        <div class="qs-livefeed__main">
          <NuxtLink
            v-for="post in blogPosts.slice(0, 2)"
            :key="post.id"
            :to="`/blog/${post.slug || post.id}`"
            class="qs-blog-card"
          >
            <div class="qs-blog-card__img-wrap">
              <img :src="post.img" :alt="post.title" loading="lazy" />
              <span class="qs-feed-badge qs-feed-badge--blog">
                <svg width="11" height="11" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><polyline points="22 7 13.5 15.5 8.5 10.5 2 17"/><polyline points="16 7 22 7 22 13"/></svg>
                Blog
              </span>
            </div>
            <div class="qs-blog-card__body">
              <span v-if="post.tag" class="qs-blog-card__tag">{{ post.tag }}</span>
              <h3 class="qs-blog-card__title">{{ post.title }}</h3>
              <p v-if="post.excerpt" class="qs-blog-card__excerpt">{{ post.excerpt }}</p>
              <div class="qs-blog-card__foot">
                <span v-if="post.date" class="qs-blog-card__date">{{ post.date }}</span>
                <span class="qs-blog-card__read">
                  Ler artigo
                  <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14"/><path d="m12 5 7 7-7 7"/></svg>
                </span>
              </div>
            </div>
          </NuxtLink>

          <div v-if="blogPosts.length === 0" class="qs-livefeed__empty">
            <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="#9ca3af" stroke-width="1.5"><path d="M4 22h16a2 2 0 0 0 2-2V4a2 2 0 0 0-2-2H8a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2z"/><path d="M16 2v4"/><path d="M8 2v4"/><path d="M3 10h18"/></svg>
            <p>Nenhum artigo publicado ainda.</p>
          </div>
        </div>

        <div class="qs-livefeed__sidebar">
          <p class="qs-livefeed__sidebar-title">Redes Sociais</p>
          <div class="qs-social-thumbs">
            <a
              v-for="item in socialFeed.slice(0, 4)"
              :key="item.id"
              :href="item.url || (item.rede?.toLowerCase() === 'youtube' ? 'https://youtube.com/@quantashop' : 'https://instagram.com/quantashop')"
              target="_blank"
              rel="noopener"
              class="qs-social-thumb"
            >
              <div class="qs-social-thumb__img-wrap">
                <img :src="item.thumb" :alt="item.legenda" loading="lazy" />
                <span
                  class="qs-feed-badge"
                  :class="item.rede?.toLowerCase() === 'youtube' ? 'qs-feed-badge--youtube' : 'qs-feed-badge--instagram'"
                >
                  <svg v-if="item.rede?.toLowerCase() === 'instagram'" width="10" height="10" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z"/></svg>
                  <svg v-else width="10" height="10" viewBox="0 0 24 24" fill="currentColor"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/></svg>
                  {{ item.rede }}
                </span>
              </div>
              <p class="qs-social-thumb__caption">{{ item.legenda }}</p>
            </a>

            <template v-if="socialFeed.length === 0">
              <a
                v-for="fallback in fallbackSocial"
                :key="fallback.id"
                :href="fallback.url"
                target="_blank"
                rel="noopener"
                class="qs-social-thumb"
              >
                <div class="qs-social-thumb__img-wrap">
                  <img :src="fallback.thumb" :alt="fallback.legenda" loading="lazy" />
                  <span class="qs-feed-badge" :class="`qs-feed-badge--${fallback.rede.toLowerCase()}`">
                    <svg v-if="fallback.rede === 'Instagram'" width="10" height="10" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z"/></svg>
                    <svg v-else width="10" height="10" viewBox="0 0 24 24" fill="currentColor"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/></svg>
                    {{ fallback.rede }}
                  </span>
                </div>
                <p class="qs-social-thumb__caption">{{ fallback.legenda }}</p>
              </a>
            </template>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';

const { config, loadConfig } = useHomeConfig();

interface BlogPost {
  id: number;
  slug: string;
  title: string;
  excerpt: string;
  tag: string;
  date: string;
  readTime: number;
  img: string;
}

interface SocialItem {
  id: number;
  rede: string;
  legenda: string;
  thumb: string;
  url?: string;
}

interface MockData {
  blog: BlogPost[];
  social: SocialItem[];
}

const blogPosts = ref<BlogPost[]>([]);
const socialFeed = ref<SocialItem[]>([]);

const fallbackSocial: SocialItem[] = [
  { id: 1, rede: 'Instagram', legenda: 'Cashback em tempo real ✨', thumb: 'https://images.unsplash.com/photo-1611162616305-c69b3fa7fbe0?w=300&q=80&auto=format&fit=crop', url: 'https://instagram.com/quantashop' },
  { id: 2, rede: 'YouTube', legenda: 'Como começar agora', thumb: 'https://images.unsplash.com/photo-1574375927938-d5a98e8ffe85?w=300&q=80&auto=format&fit=crop', url: 'https://youtube.com/@quantashop' },
  { id: 3, rede: 'Instagram', legenda: 'Histórias de sucesso', thumb: 'https://images.unsplash.com/photo-1552664730-d307ca884978?w=300&q=80&auto=format&fit=crop', url: 'https://instagram.com/quantashop' },
  { id: 4, rede: 'YouTube', legenda: 'Dicas e truques', thumb: 'https://images.unsplash.com/photo-1516738901601-6d0ee099431b?w=300&q=80&auto=format&fit=crop', url: 'https://youtube.com/@quantashop' },
];

function lsArtigosBlog(): BlogPost[] {
  try {
    const raw = localStorage.getItem('qs_blog_artigos');
    if (!raw) return [];
    const artigos = JSON.parse(raw) as Array<{
      id: number; titulo: string; resumo?: string; conteudo: string;
      imagemDestaque?: string; categoria?: string; dataPublicacao?: string;
      slug: string; publicado: boolean;
    }>;
    return artigos
      .filter(a => a.publicado)
      .sort((a, b) => {
        const da = a.dataPublicacao ? new Date(a.dataPublicacao).getTime() : 0;
        const db = b.dataPublicacao ? new Date(b.dataPublicacao).getTime() : 0;
        return db - da;
      })
      .slice(0, 2)
      .map(a => ({
        id: a.id,
        slug: a.slug || String(a.id),
        title: a.titulo,
        excerpt: a.resumo || '',
        tag: a.categoria || 'Blog',
        date: a.dataPublicacao ? new Date(a.dataPublicacao).toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', year: 'numeric' }) : '',
        readTime: 0,
        img: a.imagemDestaque || 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80&auto=format&fit=crop',
      }));
  } catch { return []; }
}

function lsPostsSocial(): SocialItem[] {
  try {
    const raw = localStorage.getItem('qs_redes_sociais');
    if (!raw) return [];
    const posts = JSON.parse(raw) as Array<{
      id: number; plataforma: string; titulo: string; url: string; thumbnailUrl?: string; ativo: boolean;
    }>;
    return posts
      .filter(p => p.ativo)
      .slice(0, 4)
      .map(p => ({
        id: p.id,
        rede: p.plataforma,
        legenda: p.titulo,
        thumb: p.thumbnailUrl || 'https://images.unsplash.com/photo-1611162616305-c69b3fa7fbe0?w=300&q=80&auto=format&fit=crop',
        url: p.url,
      }));
  } catch { return []; }
}

onMounted(async () => {
  await loadConfig();

  const lsBlog = lsArtigosBlog();
  const lsSocial = lsPostsSocial();

  if (lsBlog.length > 0 || lsSocial.length > 0) {
    blogPosts.value = lsBlog;
    socialFeed.value = lsSocial;
    return;
  }

  const cmsPosts = config.value.blog?.posts;
  if (cmsPosts && cmsPosts.length > 0) {
    blogPosts.value = cmsPosts.map(p => ({
      id: p.id,
      slug: p.slug,
      title: p.title,
      excerpt: p.excerpt,
      tag: 'Blog',
      date: p.date,
      readTime: 0,
      img: p.img,
    }));
    return;
  }

  try {
    const data = await $fetch<MockData>('/data/mock-data.json');
    blogPosts.value = (data.blog ?? []).slice(0, 2);
    socialFeed.value = data.social ?? [];
  } catch {
    console.warn('[Blog] Failed to load mock-data.json');
  }
});
</script>

<style scoped>
.qs-social {
  padding: 64px 0;
  background: #f9fafb;
}

.qs-social__header {
  text-align: center;
  margin-bottom: 48px;
}

.qs-social__label {
  display: block;
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.15em;
  text-transform: uppercase;
  color: #2F7785;
  margin-bottom: 12px;
}

.qs-social__title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: clamp(24px, 4vw, 38px);
  font-weight: 800;
  color: #225F6B;
  letter-spacing: -0.02em;
  line-height: 1.2;
  margin-bottom: 12px;
}

.qs-social__sub {
  font-size: 15px;
  color: #2F7785;
  max-width: 540px;
  margin: 0 auto;
}

.qs-livefeed {
  display: grid;
  grid-template-columns: 1fr 280px;
  gap: 24px;
  align-items: start;
}

@media (max-width: 900px) {
  .qs-livefeed {
    grid-template-columns: 1fr;
  }
}

.qs-livefeed__main {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.qs-livefeed__empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  padding: 60px 0;
  color: #9ca3af;
  font-size: 14px;
}

.qs-blog-card {
  display: grid;
  grid-template-columns: 260px 1fr;
  background: #fff;
  border-radius: 16px;
  overflow: hidden;
  text-decoration: none;
  color: inherit;
  box-shadow: 0 2px 8px rgba(0,0,0,0.06);
  transition: transform 0.25s ease, box-shadow 0.25s ease;
}

.qs-blog-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 10px 36px rgba(47, 119, 133, 0.14);
}

@media (max-width: 620px) {
  .qs-blog-card {
    grid-template-columns: 1fr;
  }
}

.qs-blog-card__img-wrap {
  position: relative;
  overflow: hidden;
}

.qs-blog-card__img-wrap img {
  width: 100%;
  height: 100%;
  min-height: 180px;
  object-fit: cover;
  transition: transform 0.4s ease;
  display: block;
}

.qs-blog-card:hover .qs-blog-card__img-wrap img {
  transform: scale(1.05);
}

.qs-blog-card__body {
  padding: 24px 28px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.qs-blog-card__tag {
  display: inline-block;
  font-size: 10px;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: #2F7785;
  background: rgba(47,119,133,0.08);
  border-radius: 999px;
  padding: 3px 10px;
}

.qs-blog-card__title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: 17px;
  font-weight: 700;
  color: #225F6B;
  line-height: 1.35;
  margin: 0;
}

.qs-blog-card__excerpt {
  font-size: 13px;
  color: #6b7280;
  line-height: 1.65;
  flex: 1;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  margin: 0;
}

.qs-blog-card__foot {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 8px;
  margin-top: 4px;
}

.qs-blog-card__date {
  font-size: 11px;
  color: #aeaeb2;
}

.qs-blog-card__read {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  font-size: 12px;
  font-weight: 600;
  color: #2F7785;
  transition: gap 0.2s;
}

.qs-blog-card:hover .qs-blog-card__read {
  gap: 8px;
}

.qs-livefeed__sidebar {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.qs-livefeed__sidebar-title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: #9ca3af;
  margin: 0 0 4px;
}

.qs-social-thumbs {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

@media (max-width: 900px) {
  .qs-social-thumbs {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
  }
}

@media (max-width: 480px) {
  .qs-social-thumbs {
    grid-template-columns: 1fr;
  }
}

.qs-social-thumb {
  display: flex;
  align-items: center;
  gap: 12px;
  background: #fff;
  border-radius: 12px;
  overflow: hidden;
  text-decoration: none;
  color: inherit;
  box-shadow: 0 1px 6px rgba(0,0,0,0.05);
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.qs-social-thumb:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(47, 119, 133, 0.12);
}

.qs-social-thumb__img-wrap {
  position: relative;
  width: 70px;
  height: 70px;
  flex-shrink: 0;
  overflow: hidden;
}

.qs-social-thumb__img-wrap img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.35s ease;
  display: block;
}

.qs-social-thumb:hover .qs-social-thumb__img-wrap img {
  transform: scale(1.08);
}

.qs-social-thumb__caption {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  font-weight: 500;
  color: #374151;
  line-height: 1.4;
  padding-right: 12px;
  margin: 0;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.qs-feed-badge {
  position: absolute;
  top: 8px;
  left: 8px;
  display: inline-flex;
  align-items: center;
  gap: 4px;
  padding: 3px 8px;
  border-radius: 999px;
  font-size: 10px;
  font-weight: 700;
  color: #fff;
  letter-spacing: 0.02em;
}

.qs-feed-badge--blog { background: #2F7785; }
.qs-feed-badge--instagram { background: #E1306C; }
.qs-feed-badge--youtube { background: #FF0000; }
</style>
