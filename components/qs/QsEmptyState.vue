<template>
  <div class="qs-empty-state">
    <div v-if="$slots.icon || icon" class="qs-empty-state__icon">
      <slot name="icon">
        <svg v-if="icon === 'search'" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
        <svg v-else-if="icon === 'box'" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M21 16V8a2 2 0 00-1-1.73l-7-4a2 2 0 00-2 0l-7 4A2 2 0 003 8v8a2 2 0 001 1.73l7 4a2 2 0 002 0l7-4A2 2 0 0021 16z"/></svg>
        <svg v-else-if="icon === 'doc'" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/></svg>
        <svg v-else-if="icon === 'users'" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 00-3-3.87M16 3.13a4 4 0 010 7.75"/></svg>
        <svg v-else width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
      </slot>
    </div>
    <h3 v-if="title" class="qs-empty-state__title">{{ title }}</h3>
    <p v-if="description" class="qs-empty-state__desc">{{ description }}</p>
    <div v-if="$slots.actions || ctaLabel" class="qs-empty-state__actions">
      <slot name="actions">
        <a v-if="ctaLabel && ctaHref" :href="ctaHref" class="qs-btn-primary">{{ ctaLabel }}</a>
        <button v-else-if="ctaLabel" class="qs-btn-primary" @click="$emit('cta')">{{ ctaLabel }}</button>
      </slot>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  icon?: 'search' | 'box' | 'doc' | 'users' | 'info';
  title?: string;
  description?: string;
  ctaLabel?: string;
  ctaHref?: string;
}
withDefaults(defineProps<Props>(), {
  icon: 'info',
  title: '',
  description: '',
  ctaLabel: '',
  ctaHref: '',
});
defineEmits<{ (e: 'cta'): void }>();
</script>

<style scoped>
.qs-empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 48px 24px;
  gap: 10px;
}
.qs-empty-state__icon {
  color: var(--qs-gray-300, #d1d5db);
  margin-bottom: 4px;
}
.qs-empty-state__title {
  font-size: 17px;
  font-weight: 600;
  color: var(--qs-ink, #1d1d1f);
  margin: 0;
}
.qs-empty-state__desc {
  font-size: 14px;
  color: var(--qs-gray-500, #6b7280);
  margin: 0;
  max-width: 380px;
  line-height: 1.55;
}
.qs-empty-state__actions {
  margin-top: 8px;
}
</style>
