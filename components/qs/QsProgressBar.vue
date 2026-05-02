<template>
  <div class="qs-pb" :class="[`qs-pb--${size}`]">
    <div class="qs-pb-track">
      <div
        class="qs-pb-fill"
        :style="{ width: clamped + '%', background: resolvedColor }"
      />
    </div>
    <div v-if="showLabel" class="qs-pb-label">
      <span>{{ leftLabel }}</span>
      <span class="qs-pb-pct">{{ clamped }}%</span>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  value: number;
  size?: 'thin' | 'md' | 'thick';
  color?: string;
  showLabel?: boolean;
  leftLabel?: string;
}
const props = withDefaults(defineProps<Props>(), {
  size: 'md',
  color: '',
  showLabel: false,
  leftLabel: '',
});

const clamped = computed(() => Math.max(0, Math.min(100, Math.round(props.value || 0))));
const resolvedColor = computed(() => props.color || 'var(--qs-lime, #98C73A)');
</script>

<style scoped>
.qs-pb { width: 100%; }
.qs-pb-track {
  background: var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-pill, 999px);
  overflow: hidden;
}
.qs-pb--thin .qs-pb-track { height: 4px; }
.qs-pb--md .qs-pb-track { height: 6px; }
.qs-pb--thick .qs-pb-track { height: 8px; }
.qs-pb-fill {
  height: 100%;
  border-radius: var(--qs-radius-pill, 999px);
  transition: width 400ms cubic-bezier(0.4, 0, 0.2, 1);
}
.qs-pb-label {
  display: flex;
  justify-content: space-between;
  font-size: 12px;
  color: var(--qs-gray-500, #6b7280);
  margin-top: 6px;
  font-weight: 500;
}
.qs-pb-pct { font-weight: 600; color: var(--qs-ink, #1d1d1f); }
</style>
