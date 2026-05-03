<template>
  <div class="qs-page-header">
    <div class="qs-header-text">
      <div v-if="$slots.eyebrow || eyebrow" class="qs-eyebrow">
        <slot name="eyebrow">{{ eyebrow }}</slot>
      </div>
      <h1><slot name="title">{{ title }}</slot></h1>
      <p v-if="description || $slots.description">
        <slot name="description">{{ description }}</slot>
      </p>
    </div>
    <div v-if="$slots.default" class="qs-header-actions">
      <slot />
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  eyebrow?: string;
  title?: string;
  description?: string;
}
withDefaults(defineProps<Props>(), {
  eyebrow: '',
  title: '',
  description: '',
});
</script>

<style scoped>
.qs-header-text h1 {
  font-size: clamp(24px, 4vw, 36px);
  font-weight: 700;
  letter-spacing: -0.02em;
  color: var(--qs-ink, #1d1d1f);
  margin: 0 0 6px;
  line-height: 1.15;
}
.qs-header-text p {
  font-size: 15px;
  color: var(--qs-gray-500, #6b7280);
  margin: 0;
  line-height: 1.55;
  max-width: 680px;
}
.qs-header-actions {
  display: flex;
  align-items: center;
  gap: 10px;
  flex-wrap: wrap;
  flex-shrink: 0;
}
@media (max-width: 640px) {
  .qs-header-actions { width: 100%; }
}
</style>
