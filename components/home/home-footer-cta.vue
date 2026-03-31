<template>
  <section class="qs-footer-cta">
    <div class="container">
      <h2 class="qs-footer-cta__title">{{ config.footerCta.title }}</h2>
      <p class="qs-footer-cta__sub">{{ config.footerCta.subtitle }}</p>
      <div class="qs-footer-cta__actions">
        <nuxt-link :href="config.footerCta.primaryLink" class="qs-footer-cta__btn-primary">
          {{ config.footerCta.primaryText }}
          <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </nuxt-link>
        <nuxt-link :href="config.footerCta.outlineLink" class="qs-footer-cta__btn-outline">
          {{ config.footerCta.outlineText }}
        </nuxt-link>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';

const { config, loadConfig } = useHomeConfig();

onMounted(() => loadConfig());
</script>

<style scoped>
.qs-footer-cta {
  padding: 48px 0;
  background: linear-gradient(135deg, #f7f8fa 0%, #fff 50%, #f7f8fa 100%);
  text-align: center;
  border-top: 1px solid #e5e7eb;
}

.qs-footer-cta__title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(28px, 4vw, 42px);
  font-weight: 800;
  color: #225F6B;
  letter-spacing: -0.03em;
  margin-bottom: 12px;
}

.qs-footer-cta__sub {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 16px;
  color: #2F7785;
  margin-bottom: 32px;
}

.qs-footer-cta__actions {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 14px;
  flex-wrap: wrap;
}

.qs-footer-cta__btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 700;
  padding: 14px 32px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
  box-shadow: 0 4px 20px rgba(152, 199, 58, 0.35);
}

.qs-footer-cta__btn-primary:hover {
  background: #7aad1f;
  color: #fff;
  transform: translateY(-2px);
}

.qs-footer-cta__btn-outline {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  border: 2px solid #e5e7eb;
  color: #374151;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 600;
  padding: 14px 32px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.qs-footer-cta__btn-outline:hover {
  border-color: #2F7785;
  color: #2F7785;
}
</style>
