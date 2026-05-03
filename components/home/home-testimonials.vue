<template>
  <section class="qs-testimonials">
    <div class="container">
      <div class="qs-section-header">
        <h2 class="qs-section-title">{{ config.testimonials.title }}</h2>
        <p class="qs-section-sub">{{ config.testimonials.subtitle }}</p>
      </div>

      <div class="qs-testimonials__grid">
        <div v-for="t in testimonials" :key="t.name" class="qs-testimonial-card">
          <div class="qs-testimonial-card__stars">
            <svg v-for="n in 5" :key="n" width="14" height="14" viewBox="0 0 24 24" fill="#FFB342"><path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/></svg>
          </div>
          <p class="qs-testimonial-card__text">
            <span class="qs-testimonial-card__highlight">{{ t.highlight }}</span>
            {{ t.text }}
          </p>
          <div class="qs-testimonial-card__author">
            <div class="qs-testimonial-card__avatar">{{ t.initials }}</div>
            <div>
              <div class="qs-testimonial-card__name">{{ t.name }}</div>
              <div class="qs-testimonial-card__role">{{ t.role }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';
import { DEFAULT_TESTIMONIALS } from '@/composables/useHomeConfig';

const { config, loadConfig } = useHomeConfig();

onMounted(() => loadConfig());

const testimonials = computed(() =>
  config.value.testimonials?.items?.length
    ? config.value.testimonials.items
    : DEFAULT_TESTIMONIALS
);
</script>

<style scoped>
.qs-testimonials {
  padding: 48px 0;
  background: #f7f8fa;
}

.qs-section-header {
  text-align: center;
  margin-bottom: 40px;
}

.qs-section-title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(24px, 4vw, 36px);
  font-weight: 800;
  color: #225F6B;
  letter-spacing: -0.03em;
  margin-bottom: 8px;
}

.qs-section-sub {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  color: #2F7785;
}

.qs-testimonials__grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
}

@media (max-width: 991px) { .qs-testimonials__grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 575px) { .qs-testimonials__grid { grid-template-columns: 1fr; } }

.qs-testimonial-card {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 24px;
  transition: all 0.3s ease;
}

.qs-testimonial-card:hover {
  border-color: #2F7785;
  box-shadow: 0 12px 32px rgba(47, 119, 133, 0.12);
  transform: translateY(-4px);
}


.qs-testimonial-card__stars {
  display: flex;
  gap: 2px;
  margin-bottom: 14px;
}

.qs-testimonial-card__text {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  color: #374151;
  line-height: 1.6;
  margin-bottom: 20px;
}

.qs-testimonial-card__highlight {
  color: #2F7785;
  font-weight: 700;
}

.qs-testimonial-card__author {
  display: flex;
  align-items: center;
  gap: 12px;
}

.qs-testimonial-card__avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: linear-gradient(135deg, #225F6B, #2F7785);
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.qs-testimonial-card__name {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 700;
  color: #225F6B;
}

.qs-testimonial-card__role {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  color: #9ca3af;
}
</style>
