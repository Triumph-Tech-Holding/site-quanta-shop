<template>
  <div class="plus-modulo">

    <!-- Badge de Status -->
    <div class="plus-modulo__header">
      <div class="plus-modulo__badge" :class="isPlus ? 'plus-modulo__badge--ativo' : 'plus-modulo__badge--inativo'">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
          <path d="M11.99 2C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zm4.24 16L12 15.45 7.77 18l1.12-4.81-3.73-3.23 4.92-.42L12 5l1.92 4.53 4.92.42-3.73 3.23L16.23 18z"/>
        </svg>
        {{ isPlus ? cms.labelBadgeAtivo : cms.labelBadgeInativo }}
      </div>

      <NuxtLink v-if="!isPlus" to="/agencia/painel/assinatura" class="plus-modulo__cta-link">
        Saiba mais →
      </NuxtLink>
    </div>

    <!-- Barra de Progresso -->
    <div class="plus-modulo__progresso">
      <div class="plus-modulo__prog-header">
        <span class="plus-modulo__prog-label">{{ cms.tituloProgresso }}</span>
        <span class="plus-modulo__prog-valor">
          {{ formatCurrency(consumoAtual) }} / {{ formatCurrency(metaConsumo) }}
        </span>
      </div>
      <div class="plus-modulo__prog-bar">
        <div class="plus-modulo__prog-fill" :style="{ width: progressoPct + '%' }" />
      </div>
      <p class="plus-modulo__prog-legenda">{{ cms.legendaProgresso }}</p>
    </div>

    <!-- KPI Ganhos Extras -->
    <div class="plus-modulo__kpi">
      <div class="plus-modulo__kpi-icon">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
          <path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/>
        </svg>
      </div>
      <div>
        <div class="plus-modulo__kpi-label">{{ cms.tituloKpi }}</div>
        <div class="plus-modulo__kpi-valor" :class="isPlus ? '' : 'plus-modulo__kpi-valor--bloqueado'">
          {{ isPlus ? formatCurrency(ganhosExtras) : 'Assine o Plus para desbloquear' }}
        </div>
      </div>
    </div>

    <!-- Radar de Assinaturas da Rede -->
    <div class="plus-modulo__radar">
      <div class="plus-modulo__radar-header">
        <span class="plus-modulo__radar-title">{{ cms.tituloRadar }}</span>
        <span class="plus-modulo__radar-badge">{{ radarList.length }} membros</span>
      </div>
      <p class="plus-modulo__radar-legenda">{{ cms.legendaRadar }}</p>

      <div v-if="radarList.length === 0" class="plus-modulo__radar-empty">
        Nenhum membro Plus na sua rede ainda.
      </div>

      <ul v-else class="plus-modulo__radar-list">
        <li v-for="m in radarList" :key="m.login" class="plus-modulo__radar-item">
          <span class="plus-modulo__radar-avatar">{{ m.nome.charAt(0).toUpperCase() }}</span>
          <span class="plus-modulo__radar-nome">{{ m.nome }}</span>
          <span class="plus-modulo__radar-nivel">N{{ m.nivel }}</span>
          <span class="plus-modulo__radar-star">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"/></svg>
          </span>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useCmsStore } from '~/pinia/useCmsStore';
import { useAgenciaStore } from '~/pinia/useAgenciaStore';

const cmsStore = useCmsStore();
const agenciaStore = useAgenciaStore();
const api = useApi();

const cms = computed(() => cmsStore.config.plusModulo);

const isPlus = computed(() => !!(agenciaStore.user as any)?.assinaturaHabilitada);

const metaConsumo = ref(cms.value.metaConsumoPlus);
const consumoAtual = ref(0);
const ganhosExtras = ref(0);

interface RadarMembro { login: string; nome: string; nivel: number }
const radarList = ref<RadarMembro[]>([]);

const progressoPct = computed(() => {
  if (!metaConsumo.value) return 0;
  return Math.min(100, Math.round((consumoAtual.value / metaConsumo.value) * 100));
});

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v);
}

onMounted(async () => {
  await cmsStore.loadConfig();
  metaConsumo.value = cmsStore.config.plusModulo.metaConsumoPlus;

  try {
    const cfg = await api.get('/Admin/ConfiguracoesRede').catch(() => null) as any;
    if (cfg?.metaConsumoPlus) metaConsumo.value = Number(cfg.metaConsumoPlus);
  } catch { /* usa valor do CMS */ }

  try {
    const fin = await api.get('/financeiro/saldo').catch(() => null) as any;
    consumoAtual.value = Number(fin?.consumoMes ?? 0);
    ganhosExtras.value = Number(fin?.ganhosExtrasPlus ?? 0);
  } catch { /* mock */ }

  radarList.value = [
    { login: 'ana.silva', nome: 'Ana Silva', nivel: 1 },
    { login: 'carlos.m', nome: 'Carlos M.', nivel: 2 },
    { login: 'patricia.r', nome: 'Patrícia R.', nivel: 3 },
  ];
});
</script>

<style scoped>
.plus-modulo {
  background: #fff;
  border-radius: var(--qs-radius-lg, 20px);
  box-shadow: var(--qs-shadow-sm, 0 2px 8px rgba(1,15,28,.08));
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

/* Header */
.plus-modulo__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: .5rem;
}

.plus-modulo__badge {
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  font-size: .6875rem;
  font-weight: 700;
  letter-spacing: .06em;
  text-transform: uppercase;
  padding: .3rem .75rem;
  border-radius: var(--qs-radius-pill, 999px);
}
.plus-modulo__badge svg { width: 14px; height: 14px; }
.plus-modulo__badge--ativo {
  background: linear-gradient(135deg, #e8950a, #FFB342);
  color: #fff;
  box-shadow: 0 2px 8px rgba(255,179,66,.35);
}
.plus-modulo__badge--inativo {
  background: var(--qs-gray-100, #f5f5f7);
  color: var(--qs-gray-500, #6b7280);
}

.plus-modulo__cta-link {
  font-size: .8125rem;
  font-weight: 600;
  color: var(--qs-teal, #2F7785);
  text-decoration: none;
}
.plus-modulo__cta-link:hover { text-decoration: underline; }

/* Progresso */
.plus-modulo__prog-header {
  display: flex;
  justify-content: space-between;
  align-items: baseline;
  margin-bottom: .5rem;
}
.plus-modulo__prog-label {
  font-size: .8125rem;
  font-weight: 600;
  color: var(--qs-ink, #1d1d1f);
}
.plus-modulo__prog-valor {
  font-size: .75rem;
  color: var(--qs-gray-500, #6b7280);
}
.plus-modulo__prog-bar {
  height: 8px;
  background: var(--qs-gray-100, #f5f5f7);
  border-radius: var(--qs-radius-pill, 999px);
  overflow: hidden;
}
.plus-modulo__prog-fill {
  height: 100%;
  background: linear-gradient(90deg, var(--qs-teal, #2F7785), var(--qs-lime, #98C73A));
  border-radius: var(--qs-radius-pill, 999px);
  transition: width .6s ease;
}
.plus-modulo__prog-legenda {
  font-size: .6875rem;
  color: var(--qs-gray-400, #9ca3af);
  margin: .4rem 0 0;
  line-height: 1.4;
}

/* KPI */
.plus-modulo__kpi {
  display: flex;
  align-items: center;
  gap: .875rem;
  background: var(--qs-gray-50, #fafafa);
  border-radius: var(--qs-radius-md, 12px);
  padding: .875rem 1rem;
}
.plus-modulo__kpi-icon {
  width: 40px;
  height: 40px;
  background: var(--qs-gradient-lime, linear-gradient(135deg, #7aad1f, #98C73A));
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  color: #fff;
}
.plus-modulo__kpi-icon svg { width: 20px; height: 20px; }
.plus-modulo__kpi-label {
  font-size: .75rem;
  color: var(--qs-gray-500, #6b7280);
  text-transform: uppercase;
  letter-spacing: .05em;
  font-weight: 600;
  margin-bottom: .2rem;
}
.plus-modulo__kpi-valor {
  font-size: 1.1875rem;
  font-weight: 700;
  color: var(--qs-lime-dark, #7aad1f);
}
.plus-modulo__kpi-valor--bloqueado {
  font-size: .8125rem;
  font-weight: 500;
  color: var(--qs-gray-400, #9ca3af);
}

/* Radar */
.plus-modulo__radar-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: .25rem;
}
.plus-modulo__radar-title {
  font-size: .875rem;
  font-weight: 700;
  color: var(--qs-ink, #1d1d1f);
}
.plus-modulo__radar-badge {
  font-size: .6875rem;
  font-weight: 600;
  background: var(--qs-teal, #2F7785);
  color: #fff;
  padding: .15rem .5rem;
  border-radius: var(--qs-radius-pill, 999px);
}
.plus-modulo__radar-legenda {
  font-size: .75rem;
  color: var(--qs-gray-400, #9ca3af);
  margin: 0 0 .75rem;
}
.plus-modulo__radar-empty {
  font-size: .8125rem;
  color: var(--qs-gray-400, #9ca3af);
  padding: .75rem 0;
}
.plus-modulo__radar-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: .5rem;
}
.plus-modulo__radar-item {
  display: flex;
  align-items: center;
  gap: .625rem;
  padding: .5rem .625rem;
  background: var(--qs-gray-50, #fafafa);
  border-radius: var(--qs-radius-sm, 6px);
}
.plus-modulo__radar-avatar {
  width: 28px;
  height: 28px;
  background: var(--qs-teal, #2F7785);
  color: #fff;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: .75rem;
  font-weight: 700;
  flex-shrink: 0;
}
.plus-modulo__radar-nome {
  flex: 1;
  font-size: .8125rem;
  font-weight: 500;
  color: var(--qs-ink, #1d1d1f);
}
.plus-modulo__radar-nivel {
  font-size: .6875rem;
  font-weight: 600;
  color: var(--qs-gray-500, #6b7280);
  background: var(--qs-gray-200, #e5e7eb);
  padding: .1rem .4rem;
  border-radius: 4px;
}
.plus-modulo__radar-star {
  color: #FFB342;
}
.plus-modulo__radar-star svg { width: 14px; height: 14px; }
</style>
