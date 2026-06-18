<template>
  <component :is="to ? 'nuxt-link' : 'span'" :href="to || undefined" class="qs-logo" :aria-label="alt">
    <img class="qs-logo__img" :src="src" :alt="alt" :style="{ height: size + 'px' }" decoding="async" />
  </component>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = withDefaults(defineProps<{
  variant?: 'icon' | 'full';
  tone?: 'color' | 'white';
  size?: number;
  to?: string;
}>(), { variant: 'icon', tone: 'color', size: 40 });

const alt = 'Quanta Shop';
const src = computed(() => {
  if (props.variant === 'full') return '/img/logo/logo-trimmed.png';
  return props.tone === 'white' ? '/img/logo/logo-symbol-white.png' : '/img/logo/logo-symbol.png';
});
</script>

<style scoped>
.qs-logo { display: inline-flex; align-items: center; text-decoration: none; }
.qs-logo__img { width: auto; display: block; }
</style>
