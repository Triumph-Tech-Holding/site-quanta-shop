<template>
  <div class="qs-kpi" :class="{ 'qs-kpi--clickable': clickable }">
    <div class="qs-kpi-header">
      <span v-if="dotColor" class="qs-kpi-dot" :style="{ background: dotColor }" />
      <span class="qs-kpi-label">{{ label }}</span>
      <span v-if="badge" class="qs-kpi-badge" :class="`qs-kpi-badge--${badgeTone}`">{{ badge }}</span>
    </div>
    <div class="qs-kpi-value">
      <span>{{ formattedValue }}</span>
      <span v-if="suffix" class="qs-kpi-suffix">{{ suffix }}</span>
    </div>
    <div v-if="hasProgress" class="qs-kpi-progress">
      <QsProgressBar :value="progress" :color="dotColor" size="thin" />
    </div>
    <div v-if="meta || delta !== undefined" class="qs-kpi-foot">
      <span v-if="delta !== undefined" class="qs-kpi-delta" :class="deltaClass">
        {{ delta > 0 ? '▲' : delta < 0 ? '▼' : '■' }} {{ Math.abs(delta) }}{{ deltaSuffix }}
      </span>
      <span v-if="meta" class="qs-kpi-meta">{{ meta }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  label: string;
  value: string | number;
  suffix?: string;
  meta?: string;
  dotColor?: string;
  badge?: string;
  badgeTone?: 'neutral' | 'success' | 'warn' | 'danger';
  progress?: number;
  delta?: number;
  deltaSuffix?: string;
  clickable?: boolean;
  format?: 'currency' | 'percent' | 'number' | 'raw';
}
const props = withDefaults(defineProps<Props>(), {
  suffix: '',
  meta: '',
  dotColor: '',
  badge: '',
  badgeTone: 'neutral',
  progress: undefined,
  delta: undefined,
  deltaSuffix: '%',
  clickable: false,
  format: 'raw',
});

const hasProgress = computed(() => typeof props.progress === 'number');

const formattedValue = computed(() => {
  if (props.format === 'raw' || typeof props.value === 'string') return props.value;
  const n = Number(props.value);
  if (Number.isNaN(n)) return props.value;
  if (props.format === 'currency') {
    return n.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }
  if (props.format === 'percent') return `${n.toLocaleString('pt-BR', { maximumFractionDigits: 1 })}%`;
  if (props.format === 'number') return n.toLocaleString('pt-BR');
  return n.toString();
});

const deltaClass = computed(() => {
  if (props.delta === undefined) return '';
  if (props.delta > 0) return 'qs-kpi-delta--up';
  if (props.delta < 0) return 'qs-kpi-delta--down';
  return 'qs-kpi-delta--flat';
});
</script>

<style scoped>
.qs-kpi {
  background: #fff;
  border-radius: var(--qs-radius-lg, 16px);
  padding: 24px;
  box-shadow: var(--qs-shadow-sm, 0 2px 8px rgba(15, 23, 42, .06));
  transition: transform .2s cubic-bezier(0.4, 0, 0.2, 1), box-shadow .2s cubic-bezier(0.4, 0, 0.2, 1);
}
.qs-kpi--clickable { cursor: pointer; }
.qs-kpi:hover {
  transform: translateY(-2px);
  box-shadow: var(--qs-shadow-md, 0 8px 24px rgba(15, 23, 42, .08));
}
.qs-kpi-header {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 12px;
  min-height: 18px;
}
.qs-kpi-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  flex-shrink: 0;
}
.qs-kpi-label {
  font-size: 13px;
  font-weight: 500;
  color: var(--qs-gray-500, #6b7280);
  letter-spacing: 0;
}
.qs-kpi-badge {
  margin-left: auto;
  font-size: 10px;
  font-weight: 700;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  padding: 2px 8px;
  border-radius: 999px;
}
.qs-kpi-badge--neutral { background: var(--qs-gray-100, #f3f4f6); color: var(--qs-gray-500, #6b7280); }
.qs-kpi-badge--success { background: #dcfce7; color: var(--qs-success, #16a34a); }
.qs-kpi-badge--warn { background: #fef3c7; color: var(--qs-warn, #d97706); }
.qs-kpi-badge--danger { background: #fee2e2; color: var(--qs-danger, #dc2626); }

.qs-kpi-value {
  font-size: 30px;
  font-weight: 700;
  color: var(--qs-ink, #1d1d1f);
  letter-spacing: -0.02em;
  line-height: 1;
  display: flex;
  align-items: baseline;
  gap: 6px;
  font-variant-numeric: tabular-nums;
}
.qs-kpi-suffix {
  font-size: 14px;
  font-weight: 500;
  color: var(--qs-gray-500, #6b7280);
}
.qs-kpi-progress { margin-top: 14px; }
.qs-kpi-foot {
  margin-top: 10px;
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 12px;
}
.qs-kpi-meta {
  color: var(--qs-gray-400, #9ca3af);
}
.qs-kpi-delta {
  font-weight: 600;
  font-size: 11px;
  padding: 2px 6px;
  border-radius: 4px;
  letter-spacing: 0.02em;
}
.qs-kpi-delta--up { background: #dcfce7; color: var(--qs-success, #16a34a); }
.qs-kpi-delta--down { background: #fee2e2; color: var(--qs-danger, #dc2626); }
.qs-kpi-delta--flat { background: var(--qs-gray-100, #f3f4f6); color: var(--qs-gray-500, #6b7280); }
</style>
