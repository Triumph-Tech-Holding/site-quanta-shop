<template>
  <section class="qs-blog">
    <div class="container">
      <div class="qs-section-header">
        <span class="qs-section-label">Blog Quanta</span>
        <h2 class="qs-section-title">Dicas para economizar mais</h2>
        <p class="qs-section-sub">Conteúdo prático sobre cashback, finanças e consumo inteligente.</p>
      </div>

      <div class="qs-livefeed">
        <!-- Coluna Principal: Blog Cards -->
        <div class="qs-livefeed__blog">
          <a
            v-for="(post, i) in blogPosts.slice(0, 2)"
            :key="i"
            href="/blog"
            class="qs-livefeed-card"
          >
            <div class="qs-livefeed-card__thumb">
              <img :src="post.img" :alt="post.title" />
              <span class="qs-livefeed-card__tag">{{ post.tag }}</span>
            </div>
            <div class="qs-livefeed-card__body">
              <div class="qs-livefeed-card__meta">
                <span>{{ post.date }}</span>
                <span>·</span>
                <span>{{ post.readTime }} min leitura</span>
              </div>
              <h3 class="qs-livefeed-card__title">{{ post.title }}</h3>
              <p class="qs-livefeed-card__excerpt">{{ post.excerpt }}</p>
              <span class="qs-livefeed-card__link">
                Ler artigo
                <svg width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
              </span>
            </div>
          </a>
        </div>

        <!-- Coluna Lateral: Redes Sociais -->
        <div class="qs-livefeed__social">
          <div
            v-for="(item, i) in socialFeed.slice(0, 4)"
            :key="i"
            class="qs-social-thumbnail"
          >
            <img :src="item.thumb" :alt="item.rede" />
            <div class="qs-social-badge">{{ item.rede }}</div>
            <p class="qs-social-caption">{{ item.legenda }}</p>
          </div>
        </div>
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
import { ref, onMounted } from 'vue';

const blogPosts = ref([]);
const socialFeed = ref([]);

onMounted(async () => {
  try {
    const res = await fetch('/data/mock-data.json');
    const data = await res.json();
    blogPosts.value = data.blog || [];
    socialFeed.value = data.social || [];
  } catch (e) {
    console.warn('Failed to load mock-data.json');
  }
});
</script>

<style scoped>
.qs-blog {
  padding: 72px 0;
  background: #fff;
}

.qs-section-header {
  text-align: center;
  margin-bottom: 48px;
}

.qs-section-label {
  display: inline-block;
  background: rgba(47, 119, 133, 0.08);
  color: #2F7785;
  border-radius: 999px;
  padding: 4px 14px;
  font-size: 11px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  margin-bottom: 12px;
}

.qs-section-title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: clamp(24px, 4vw, 40px);
  font-weight: 800;
  color: #111827;
  letter-spacing: -0.03em;
  margin-bottom: 8px;
}

.qs-section-sub {
  font-size: 15px;
  color: #6B7280;
  max-width: 540px;
  margin: 0 auto;
}

.qs-livefeed {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 32px;
  margin-bottom: 40px;
}

.qs-livefeed__blog {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.qs-livefeed__social {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.qs-livefeed-card {
  background: #fff;
  border: 1px solid #E5E7EB;
  border-radius: 12px;
  overflow: hidden;
  transition: all 0.3s ease;
  text-decoration: none;
  color: inherit;
}

.qs-livefeed-card:hover {
  border-color: #2F7785;
  transform: translateY(-4px);
  box-shadow: 0 10px 30px rgba(47, 119, 133, 0.12);
}

.qs-livefeed-card__thumb {
  position: relative;
  overflow: hidden;
  height: 280px;
}

.qs-livefeed-card__thumb img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.qs-livefeed-card__tag {
  position: absolute;
  top: 12px;
  right: 12px;
  background: #98C73A;
  color: #225F6B;
  padding: 4px 12px;
  border-radius: 999px;
  font-size: 11px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.qs-livefeed-card__body {
  padding: 20px;
}

.qs-livefeed-card__meta {
  font-size: 12px;
  color: #6B7280;
  margin-bottom: 8px;
}

.qs-livefeed-card__title {
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: 16px;
  font-weight: 700;
  color: #111827;
  margin-bottom: 8px;
  line-height: 1.4;
}

.qs-livefeed-card__excerpt {
  font-size: 13px;
  color: #6B7280;
  margin-bottom: 12px;
  line-height: 1.5;
}

.qs-livefeed-card__link {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  color: #2F7785;
  font-size: 12px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  transition: gap 0.2s ease;
}

.qs-livefeed-card:hover .qs-livefeed-card__link {
  gap: 10px;
}

.qs-social-thumbnail {
  position: relative;
  overflow: hidden;
  border-radius: 12px;
  aspect-ratio: 1;
  background: #F4F4F5;
  transition: all 0.3s ease;
  cursor: pointer;
}

.qs-social-thumbnail img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.qs-social-thumbnail:hover {
  transform: scale(1.05);
}

.qs-social-thumbnail:hover .qs-social-caption {
  opacity: 1;
}

.qs-social-badge {
  position: absolute;
  top: 8px;
  left: 8px;
  background: rgba(47, 119, 133, 0.9);
  color: #fff;
  padding: 4px 10px;
  border-radius: 4px;
  font-size: 10px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.qs-social-caption {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  background: linear-gradient(to top, rgba(0,0,0,0.8), transparent);
  color: #fff;
  padding: 12px 8px 8px;
  font-size: 11px;
  font-weight: 600;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.qs-blog__more {
  text-align: center;
}

.qs-btn-outline-primary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 12px 28px;
  border: 2px solid #2F7785;
  border-radius: 8px;
  color: #2F7785;
  background: transparent;
  font-family: 'Jost', 'Inter', sans-serif;
  font-size: 13px;
  font-weight: 700;
  text-decoration: none;
  cursor: pointer;
  transition: all 0.3s ease;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.qs-btn-outline-primary:hover {
  background: #2F7785;
  color: #fff;
  gap: 12px;
}

@media (max-width: 768px) {
  .qs-livefeed {
    grid-template-columns: 1fr;
    gap: 24px;
  }

  .qs-livefeed__social {
    grid-template-columns: 1fr 1fr;
  }
}

@media (max-width: 576px) {
  .qs-section-header {
    margin-bottom: 32px;
  }

  .qs-livefeed-card__thumb {
    height: 200px;
  }

  .qs-section-title {
    font-size: 24px;
  }
}
</style>
