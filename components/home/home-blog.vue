<template>
  <section class="qs-social">
    <div class="container">
      <div class="qs-social__header">
        <span class="qs-social__label">{{ config.blog.label }}</span>
        <h2 class="qs-social__title">{{ config.blog.title }}</h2>
        <p class="qs-social__sub">{{ config.blog.subtitle }}</p>
      </div>

      <div class="qs-social__grid">
        <a
          v-for="(item, i) in feedItems"
          :key="i"
          :href="item.url"
          class="qs-feed-card"
          target="_blank"
          rel="noopener"
        >
          <div class="qs-feed-card__img-wrap">
            <img :src="item.img" :alt="item.title" loading="lazy" />
            <span class="qs-feed-card__badge" :class="`qs-feed-card__badge--${item.type}`">
              <!-- Instagram -->
              <svg v-if="item.type === 'instagram'" width="13" height="13" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z"/></svg>
              <!-- YouTube -->
              <svg v-else-if="item.type === 'youtube'" width="13" height="13" viewBox="0 0 24 24" fill="currentColor"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"/></svg>
              <!-- Blog -->
              <svg v-else width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="22 7 13.5 15.5 8.5 10.5 2 17"/><polyline points="16 7 22 7 22 13"/></svg>
              {{ item.badgeLabel }}
            </span>
          </div>
          <div class="qs-feed-card__body">
            <h3 class="qs-feed-card__title">{{ item.title }}</h3>
            <p v-if="item.date" class="qs-feed-card__date">{{ item.date }}</p>
            <span class="qs-feed-card__link">Ler mais →</span>
          </div>
        </a>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';

const { config, loadConfig } = useHomeConfig();

type FeedType = 'blog' | 'instagram' | 'youtube';

interface FeedItem {
  type: FeedType;
  badgeLabel: string;
  title: string;
  img: string;
  date: string;
  url: string;
}

interface BlogPost {
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
}

interface MockData {
  blog: BlogPost[];
  social: SocialItem[];
}

const blogPosts = ref<BlogPost[]>([]);
const socialFeed = ref<SocialItem[]>([]);

const feedItems = computed<FeedItem[]>(() => {
  const items: FeedItem[] = [];

  for (const post of blogPosts.value) {
    items.push({
      type: 'blog',
      badgeLabel: 'Blog',
      title: post.title,
      img: post.img,
      date: post.date,
      url: `/blog/${post.slug}`,
    });
  }

  for (const social of socialFeed.value) {
    const rede = (social.rede ?? '').toLowerCase();
    items.push({
      type: rede === 'youtube' ? 'youtube' : 'instagram',
      badgeLabel: social.rede,
      title: social.legenda,
      img: social.thumb,
      date: '',
      url: rede === 'youtube' ? 'https://youtube.com/@quantashop' : 'https://instagram.com/quantashop',
    });
  }

  return items.slice(0, 4);
});

onMounted(async () => {
  await loadConfig();
  try {
    const data = await $fetch<MockData>('/data/mock-data.json');
    blogPosts.value = data.blog ?? [];
    socialFeed.value = data.social ?? [];
  } catch (e) {
    console.warn('[Blog] Failed to load mock-data.json');
  }
});
</script>

<style scoped>
.qs-social {
  padding: 72px 0;
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

.qs-social__grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;
}

@media (max-width: 991px) {
  .qs-social__grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 575px) {
  .qs-social__grid {
    grid-template-columns: 1fr;
  }
}

.qs-feed-card {
  background: #fff;
  border-radius: 16px;
  overflow: hidden;
  text-decoration: none;
  color: inherit;
  display: flex;
  flex-direction: column;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.qs-feed-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 32px rgba(47, 119, 133, 0.14);
}

.qs-feed-card__img-wrap {
  position: relative;
  overflow: hidden;
  aspect-ratio: 4 / 3;
}

.qs-feed-card__img-wrap img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.4s ease;
}

.qs-feed-card:hover .qs-feed-card__img-wrap img {
  transform: scale(1.04);
}

.qs-feed-card__badge {
  position: absolute;
  top: 12px;
  left: 12px;
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 5px 10px;
  border-radius: 999px;
  font-size: 11px;
  font-weight: 700;
  color: #fff;
  letter-spacing: 0.02em;
}

.qs-feed-card__badge--instagram {
  background: #E1306C;
}

.qs-feed-card__badge--youtube {
  background: #FF0000;
}

.qs-feed-card__badge--blog {
  background: #2F7785;
}

.qs-feed-card__body {
  padding: 20px;
  display: flex;
  flex-direction: column;
  gap: 6px;
  flex: 1;
}

.qs-feed-card__title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: 15px;
  font-weight: 700;
  color: #225F6B;
  line-height: 1.45;
  margin: 0;
}

.qs-feed-card__date {
  font-size: 12px;
  color: #9CA3AF;
  margin: 0;
}

.qs-feed-card__link {
  display: inline-block;
  margin-top: auto;
  padding-top: 10px;
  font-size: 13px;
  font-weight: 700;
  color: #2F7785;
  transition: letter-spacing 0.2s;
}

.qs-feed-card:hover .qs-feed-card__link {
  letter-spacing: 0.03em;
}
</style>
