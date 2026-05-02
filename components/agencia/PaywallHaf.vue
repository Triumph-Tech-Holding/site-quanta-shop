<template>
  <div class="paywall-haf">
    <div class="paywall-haf__icon">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
        <path d="M18 8h-1V6c0-2.76-2.24-5-5-5S7 3.24 7 6v2H6c-1.1 0-2 .9-2 2v10c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V10c0-1.1-.9-2-2-2zm-6 9c-1.1 0-2-.9-2-2s.9-2 2-2 2 .9 2 2-.9 2-2 2zm3.1-9H8.9V6c0-1.71 1.39-3.1 3.1-3.1 1.71 0 3.1 1.39 3.1 3.1v2z"/>
      </svg>
    </div>

    <h2 class="paywall-haf__title">{{ cms.titulo }}</h2>
    <p class="paywall-haf__subtitle">{{ cms.subtitulo }}</p>

    <ul class="paywall-haf__list">
      <li v-for="(b, i) in cms.beneficios" :key="i">
        <span class="paywall-haf__check">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
        </span>
        {{ b }}
      </li>
    </ul>

    <NuxtLink :to="cms.ctaLink" class="paywall-haf__cta">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/></svg>
      {{ cms.ctaTexto }}
    </NuxtLink>

    <p class="paywall-haf__note">Licença HAF · Acesso vitalício ao ecossistema Quanta</p>
  </div>
</template>

<script setup lang="ts">
import { useCmsStore } from '~/pinia/useCmsStore';

const cmsStore = useCmsStore();
const cms = computed(() => cmsStore.config.paywallHaf);

onMounted(() => cmsStore.loadConfig());
</script>

<style scoped>
.paywall-haf {
  background: linear-gradient(135deg, #f0f7f9 0%, #fff 60%, #f5fbf0 100%);
  border: 2px dashed var(--qs-teal, #2F7785);
  border-radius: var(--qs-radius-lg, 20px);
  padding: 2.5rem 2rem;
  text-align: center;
  max-width: 520px;
  margin: 0 auto;
}

.paywall-haf__icon {
  width: 56px;
  height: 56px;
  background: var(--qs-teal, #2F7785);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 1.25rem;
  color: #fff;
}
.paywall-haf__icon svg { width: 28px; height: 28px; }

.paywall-haf__title {
  font-size: 1.25rem;
  font-weight: 700;
  color: var(--qs-teal-dark, #225F6B);
  letter-spacing: -0.02em;
  margin: 0 0 .5rem;
}

.paywall-haf__subtitle {
  font-size: .875rem;
  color: var(--qs-gray-500, #6b7280);
  line-height: 1.55;
  margin: 0 0 1.5rem;
}

.paywall-haf__list {
  list-style: none;
  padding: 0;
  margin: 0 0 1.75rem;
  text-align: left;
  display: inline-flex;
  flex-direction: column;
  gap: .6rem;
}

.paywall-haf__list li {
  display: flex;
  align-items: flex-start;
  gap: .5rem;
  font-size: .875rem;
  color: var(--qs-gray-700, #374151);
}

.paywall-haf__check {
  flex-shrink: 0;
  width: 18px;
  height: 18px;
  background: var(--qs-lime, #98C73A);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-top: 1px;
  color: #fff;
}
.paywall-haf__check svg { width: 12px; height: 12px; }

.paywall-haf__cta {
  display: inline-flex;
  align-items: center;
  gap: .5rem;
  background: var(--qs-gradient-btn, linear-gradient(135deg, #225F6B 0%, #2F7785 100%));
  color: #fff;
  font-weight: 600;
  font-size: .9375rem;
  padding: .75rem 1.75rem;
  border-radius: var(--qs-radius-md, 12px);
  transition: all .25s;
  text-decoration: none;
  margin-bottom: .75rem;
}
.paywall-haf__cta svg { width: 18px; height: 18px; }
.paywall-haf__cta:hover {
  opacity: .88;
  transform: translateY(-1px);
  color: #fff;
}

.paywall-haf__note {
  font-size: .75rem;
  color: var(--qs-gray-400, #9ca3af);
  margin: 0;
}
</style>
