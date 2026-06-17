<template>
  <section class="qs-social">
    <div class="container">
      <div class="qs-social__header">
        <span class="qs-social__label">{{ config.blog.label }}</span>
        <h2 class="qs-social__title">{{ config.blog.title }}</h2>
        <p class="qs-social__sub">{{ config.blog.subtitle }}</p>
      </div>

      <div class="qs-blog-grid">
        <component
          :is="post.externalUrl ? 'a' : 'NuxtLink'"
          v-for="post in allPosts"
          :key="post.id"
          v-bind="post.externalUrl
            ? { href: post.externalUrl, target: '_blank', rel: 'noopener' }
            : { to: `/blog/${post.slug || post.id}` }"
          class="qs-blog-card2"
        >
          <div class="qs-blog-card2__img-wrap">
            <img :src="post.img" :alt="post.title" loading="lazy" />
            <span
              class="qs-feed-badge"
              :class="{
                'qs-feed-badge--instagram': post.type === 'instagram',
                'qs-feed-badge--youtube': post.type === 'youtube',
                'qs-feed-badge--blog': !post.type || post.type === 'blog'
              }"
            >
              <svg v-if="post.type === 'instagram'" width="10" height="10" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z"/></svg>
              <svg v-else-if="post.type === 'youtube'" width="10" height="10" viewBox="0 0 24 24" fill="currentColor"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/></svg>
              <svg v-else width="10" height="10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M4 22h16a2 2 0 0 0 2-2V4a2 2 0 0 0-2-2H8a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2z"/><path d="M16 2v4"/><path d="M8 2v4"/><path d="M3 10h18"/></svg>
              {{ post.type === 'instagram' ? 'Instagram' : post.type === 'youtube' ? 'YouTube' : 'Blog' }}
            </span>
          </div>
          <div class="qs-blog-card2__body">
            <h3 class="qs-blog-card2__title">{{ post.title }}</h3>
            <div class="qs-blog-card2__foot">
              <span v-if="post.date" class="qs-blog-card2__date">{{ post.date }}</span>
              <span class="qs-blog-card2__read">
                Ler mais
                <svg width="11" height="11" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14"/><path d="m12 5 7 7-7 7"/></svg>
              </span>
            </div>
          </div>
        </component>
      </div>

      <div v-if="allPosts.length === 0" class="qs-blog-empty">
        <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="#9ca3af" stroke-width="1.5"><path d="M4 22h16a2 2 0 0 0 2-2V4a2 2 0 0 0-2-2H8a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2z"/></svg>
        <p>Nenhum conteúdo publicado ainda.</p>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useHomeConfig, DEFAULT_BLOG_POSTS, type BlogPostCms } from '@/composables/useHomeConfig';

const { config, loadConfig } = useHomeConfig();

interface UnifiedPost {
  id: number | string;
  slug: string;
  title: string;
  date: string;
  img: string;
  type: 'blog' | 'instagram' | 'youtube';
  externalUrl?: string;
}

const allPosts = ref<UnifiedPost[]>([]);

function lsArtigosBlog(): UnifiedPost[] {
  try {
    const raw = localStorage.getItem('qs_blog_artigos');
    if (!raw) return [];
    const artigos = JSON.parse(raw) as Array<{
      id: number; titulo: string; imagemDestaque?: string; categoria?: string;
      dataPublicacao?: string; slug: string; publicado: boolean;
    }>;
    return artigos
      .filter(a => a.publicado)
      .sort((a, b) => {
        const da = a.dataPublicacao ? new Date(a.dataPublicacao).getTime() : 0;
        const db = b.dataPublicacao ? new Date(b.dataPublicacao).getTime() : 0;
        return db - da;
      })
      .map(a => ({
        id: a.id,
        slug: a.slug || String(a.id),
        title: a.titulo,
        date: a.dataPublicacao
          ? new Date(a.dataPublicacao).toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', year: 'numeric' })
          : '',
        img: a.imagemDestaque || 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600&q=80&auto=format&fit=crop',
        type: 'blog' as const,
      }));
  } catch { return []; }
}

function lsPostsSocial(): UnifiedPost[] {
  try {
    const raw = localStorage.getItem('qs_redes_sociais');
    if (!raw) return [];
    const posts = JSON.parse(raw) as Array<{
      id: number; plataforma: string; titulo: string; url: string; thumbnailUrl?: string; ativo: boolean;
    }>;
    return posts
      .filter(p => p.ativo)
      .map(p => ({
        id: p.id,
        slug: String(p.id),
        title: p.titulo,
        date: '',
        img: p.thumbnailUrl || 'https://images.unsplash.com/photo-1611162616305-c69b3fa7fbe0?w=600&q=80&auto=format&fit=crop',
        type: (p.plataforma?.toLowerCase() === 'youtube' ? 'youtube' : 'instagram') as 'youtube' | 'instagram',
        externalUrl: p.url,
      }));
  } catch { return []; }
}

onMounted(async () => {
  await loadConfig();

  const blog = lsArtigosBlog();
  const social = lsPostsSocial();

  if (blog.length > 0 || social.length > 0) {
    const merged = [...blog, ...social]
      .sort((a, b) => {
        if (!a.date && !b.date) return 0;
        if (!a.date) return 1;
        if (!b.date) return -1;
        return 0;
      })
      .slice(0, 8);
    allPosts.value = merged;
    return;
  }

  const cmsPosts = config.value.blog?.posts;
  if (cmsPosts && cmsPosts.length > 0) {
    allPosts.value = cmsPosts.map((p: BlogPostCms) => ({
      id: p.id,
      slug: p.slug,
      title: p.title,
      date: p.date,
      img: p.img,
      type: (p.type || 'blog') as 'blog' | 'instagram' | 'youtube',
    }));
    return;
  }

  allPosts.value = DEFAULT_BLOG_POSTS.map((p: BlogPostCms) => ({
    id: p.id,
    slug: p.slug,
    title: p.title,
    date: p.date,
    img: p.img,
    type: (p.type || 'blog') as 'blog' | 'instagram' | 'youtube',
  }));
});
</script>

<style scoped>
.qs-social {
  padding: 72px 0;
  background: #f9fafb;
}

.qs-social__header {
  text-align: center;
  margin-bottom: 52px;
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
  color: #6b7280;
  max-width: 540px;
  margin: 0 auto;
}

.qs-blog-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
}

@media (max-width: 1100px) {
  .qs-blog-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (max-width: 767px) {
  .qs-blog-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 479px) {
  .qs-blog-grid {
    grid-template-columns: 1fr;
  }
}

.qs-blog-card2 {
  background: #fff;
  border-radius: 14px;
  overflow: hidden;
  text-decoration: none;
  color: inherit;
  box-shadow: 0 2px 8px rgba(0,0,0,0.06);
  transition: transform 0.25s ease, box-shadow 0.25s ease;
  display: flex;
  flex-direction: column;
}

.qs-blog-card2:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 40px rgba(47, 119, 133, 0.15);
}

.qs-blog-card2__img-wrap {
  position: relative;
  overflow: hidden;
  height: 180px;
  flex-shrink: 0;
}

.qs-blog-card2__img-wrap img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.4s ease;
  display: block;
}

.qs-blog-card2:hover .qs-blog-card2__img-wrap img {
  transform: scale(1.06);
}

.qs-blog-card2__body {
  padding: 16px;
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.qs-blog-card2__title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: 14px;
  font-weight: 700;
  color: #225F6B;
  line-height: 1.4;
  margin: 0;
  flex: 1;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.qs-blog-card2__foot {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 8px;
  margin-top: auto;
}

.qs-blog-card2__date {
  font-size: 11px;
  color: #aeaeb2;
}

.qs-blog-card2__read {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  font-size: 12px;
  font-weight: 600;
  color: #2F7785;
  transition: gap 0.2s;
  white-space: nowrap;
}

.qs-blog-card2:hover .qs-blog-card2__read {
  gap: 7px;
}

.qs-feed-badge {
  position: absolute;
  top: 10px;
  left: 10px;
  display: inline-flex;
  align-items: center;
  gap: 4px;
  padding: 3px 9px;
  border-radius: 999px;
  font-size: 10px;
  font-weight: 700;
  color: #fff;
  letter-spacing: 0.02em;
  z-index: 1;
}

.qs-feed-badge--blog { background: #2F7785; }
.qs-feed-badge--instagram { background: #E1306C; }
.qs-feed-badge--youtube { background: #FF0000; }

.qs-blog-empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  padding: 60px 0;
  color: #9ca3af;
  font-size: 14px;
}
</style>
