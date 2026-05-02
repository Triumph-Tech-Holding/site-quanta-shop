<template>
  <button
    type="button"
    class="qs-chip"
    :class="{ active }"
    @click="$emit('click', $event)"
  >
    <span v-if="$slots.icon" class="qs-chip-icon"><slot name="icon" /></span>
    <span class="qs-chip-label"><slot>{{ label }}</slot></span>
    <span v-if="count !== undefined" class="qs-chip-count">{{ count }}</span>
  </button>
</template>

<script setup lang="ts">
interface Props {
  label?: string;
  active?: boolean;
  count?: number;
}
withDefaults(defineProps<Props>(), {
  label: '',
  active: false,
  count: undefined,
});
defineEmits<{ (e: 'click', ev: MouseEvent): void; }>();
</script>

<style scoped>
.qs-chip {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-family: inherit;
  font-size: 13px;
  font-weight: 500;
  padding: 6px 14px;
  border-radius: var(--qs-radius-pill, 999px);
  border: 1px solid var(--qs-gray-200, #e5e7eb);
  background: var(--qs-gray-50, #fafafa);
  color: var(--qs-gray-700, #374151);
  cursor: pointer;
  transition: all .2s cubic-bezier(0.4, 0, 0.2, 1);
}
.qs-chip:hover {
  border-color: var(--qs-teal, #2F7785);
  color: var(--qs-teal, #2F7785);
}
.qs-chip.active {
  background: var(--qs-teal, #2F7785);
  border-color: var(--qs-teal, #2F7785);
  color: #fff;
}
.qs-chip-icon { display: inline-flex; }
.qs-chip-count {
  background: rgba(0, 0, 0, 0.08);
  border-radius: 999px;
  padding: 1px 8px;
  font-size: 11px;
  font-weight: 600;
}
.qs-chip.active .qs-chip-count {
  background: rgba(255, 255, 255, 0.25);
}
</style>
