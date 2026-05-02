<template>
  <div class="qs-page qs-bi">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Business Intelligence · Financeiro</div>
        <h1>BI Financeiro</h1>
        <p>Faturamento consolidado, métricas de inadimplência e cashback reservado em tempo real.</p>
      </div>
      <div class="qs-header-actions">
        <div class="qs-period-switch">
          <QsFilterChip
            v-for="p in periods"
            :key="p.key"
            :active="period === p.key"
            @click="period = p.key"
          >{{ p.label }}</QsFilterChip>
        </div>
        <button class="qs-btn-outline" @click="loadData" :disabled="loading">Recarregar</button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner"/></div>

    <div v-else-if="errorMsg" class="qs-error-state">
      <svg width="40" height="40" viewBox="0 0 24 24" fill="#dc2626"><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
      <h3>Erro ao carregar BI Financeiro</h3>
      <p>{{ errorMsg }}</p>
      <button class="qs-btn-primary" @click="loadData">Tentar novamente</button>
    </div>

    <template v-else>
      <!-- 1. KPI principais -->
      <div class="qs-grid qs-bi-grid">
        <QsKpiCard
          label="Faturamento total"
          :value="totals.faturamento"
          format="currency"
          :delta="totals.faturamentoDelta"
          delta-suffix="%"
          dot-color="var(--qs-lime)"
          :meta="`${period === 'month' ? 'Este mês' : period === 'quarter' ? 'Trimestre' : 'Ano'}`"
        />
        <QsKpiCard
          label="Cashback reservado"
          :value="totals.cashbackReservado"
          format="currency"
          dot-color="var(--qs-teal)"
          :meta="`A pagar nos próximos ${totals.diasMedio} dias`"
          badge="Provisão"
          badge-tone="warn"
        />
        <QsKpiCard
          label="Inadimplência"
          :value="totals.inadimplencia"
          format="percent"
          :delta="totals.inadimplenciaDelta"
          delta-suffix="pp"
          dot-color="var(--qs-danger)"
          :meta="`${totals.inadimplenciaValor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })} em atraso`"
        />
        <QsKpiCard
          label="Margem operacional"
          :value="totals.margem"
          format="percent"
          dot-color="var(--qs-teal-dark)"
          meta="Após bônus e operacional"
          :badge="totals.margem >= 18 ? 'Saudável' : 'Atenção'"
          :badge-tone="totals.margem >= 18 ? 'success' : 'warn'"
        />
      </div>

      <!-- 2. Faturamento por categoria -->
      <section class="qs-card-section">
        <div class="qs-section-head">
          <div>
            <h2 class="qs-section-title">Faturamento por categoria</h2>
            <p class="qs-section-desc">Composição da receita no período selecionado.</p>
          </div>
          <div class="qs-totalbox">
            <div class="qs-totalbox-label">Total</div>
            <div class="qs-totalbox-value">{{ formatBRL(totals.faturamento) }}</div>
          </div>
        </div>

        <div class="qs-bars">
          <div v-for="cat in categorias" :key="cat.id" class="qs-bar-row">
            <div class="qs-bar-info">
              <div class="qs-bar-label">
                <span class="qs-bar-dot" :style="{ background: cat.color }"></span>
                <span class="qs-bar-name">{{ cat.name }}</span>
                <span class="qs-bar-count">{{ cat.qtd }} transações</span>
              </div>
              <div class="qs-bar-value">
                <span class="qs-bar-amount">{{ formatBRL(cat.valor) }}</span>
                <span class="qs-bar-pct">{{ ((cat.valor / totals.faturamento) * 100).toFixed(1) }}%</span>
              </div>
            </div>
            <QsProgressBar
              :value="(cat.valor / totals.faturamento) * 100"
              :color="cat.color"
              size="thick"
            />
          </div>
        </div>
      </section>

      <!-- 3. Inadimplência detalhada -->
      <section class="qs-card-section">
        <h2 class="qs-section-title">Inadimplência por faixa de atraso</h2>
        <p class="qs-section-desc">Faturas em aberto agrupadas por dias de vencimento.</p>

        <div class="qs-aging-grid">
          <div v-for="ag in aging" :key="ag.bucket" class="qs-aging-card" :class="`risk-${ag.risk}`">
            <div class="qs-aging-bucket">{{ ag.bucket }}</div>
            <div class="qs-aging-value">{{ formatBRL(ag.valor) }}</div>
            <div class="qs-aging-count">{{ ag.qtd }} faturas</div>
            <div class="qs-aging-risk-strip"></div>
          </div>
        </div>
      </section>

      <!-- 4. Cashback reservado por safra -->
      <section class="qs-card-section">
        <div class="qs-section-head">
          <div>
            <h2 class="qs-section-title">Cashback reservado por safra</h2>
            <p class="qs-section-desc">Provisão segregada por mês de geração — saldo a liberar após quarentena.</p>
          </div>
        </div>

        <div class="qs-table">
          <div class="qs-table-head qs-tab-cashback">
            <div>Mês de referência</div>
            <div>Crédito gerado</div>
            <div>Estornado</div>
            <div>Liberado</div>
            <div>A pagar</div>
            <div>Liberação</div>
          </div>
          <div v-for="s in safras" :key="s.mes" class="qs-row qs-tab-cashback">
            <div>
              <div class="qs-mes">{{ s.mes }}</div>
              <div class="qs-mes-meta">{{ s.qtd }} pedidos</div>
            </div>
            <div class="qs-num">{{ formatBRL(s.gerado) }}</div>
            <div class="qs-num qs-num-neg">-{{ formatBRL(s.estornado) }}</div>
            <div class="qs-num qs-num-pos">{{ formatBRL(s.liberado) }}</div>
            <div class="qs-num qs-num-warn">{{ formatBRL(s.aPagar) }}</div>
            <div>
              <span class="qs-status-badge" :class="s.aPagar > 0 ? 'badge-warn' : 'badge-done'">
                {{ s.aPagar > 0 ? `Em ${s.diasParaLiberacao}d` : 'Concluída' }}
              </span>
            </div>
          </div>
        </div>
      </section>

      <!-- 5. Top fontes de receita -->
      <section class="qs-card-section">
        <h2 class="qs-section-title">Top parceiros por faturamento</h2>
        <p class="qs-section-desc">Maiores geradores de receita no período.</p>

        <div class="qs-toplist">
          <div v-for="(p, idx) in topParceiros" :key="p.id" class="qs-top-item">
            <div class="qs-top-rank">#{{ idx + 1 }}</div>
            <div class="qs-top-info">
              <div class="qs-top-name">{{ p.nome }}</div>
              <div class="qs-top-meta">{{ p.transacoes }} transações · {{ p.cashbackPct }}% cashback médio</div>
            </div>
            <div class="qs-top-bar">
              <QsProgressBar :value="(p.valor / topParceiros[0].valor) * 100" color="var(--qs-teal)" size="md" />
            </div>
            <div class="qs-top-value">{{ formatBRL(p.valor) }}</div>
          </div>
        </div>
      </section>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

interface Categoria { id: string; name: string; valor: number; qtd: number; color: string; }
interface AgingBucket { bucket: string; valor: number; qtd: number; risk: 'low' | 'med' | 'high' | 'critical'; }
interface Safra { mes: string; qtd: number; gerado: number; estornado: number; liberado: number; aPagar: number; diasParaLiberacao: number; }
interface Parceiro { id: number; nome: string; transacoes: number; cashbackPct: number; valor: number; }
interface BiTotals {
  faturamento: number; faturamentoDelta: number;
  cashbackReservado: number; diasMedio: number;
  inadimplencia: number; inadimplenciaDelta: number; inadimplenciaValor: number;
  margem: number;
}

const agenciaStore = useAgenciaStore();
const api = useApi();

const loading = ref(true);
const period = ref<'month' | 'quarter' | 'year'>('month');
const periods = [
  { key: 'month', label: 'Mês' },
  { key: 'quarter', label: 'Trimestre' },
  { key: 'year', label: 'Ano' },
];

const totals = ref<BiTotals>({
  faturamento: 0, faturamentoDelta: 0,
  cashbackReservado: 0, diasMedio: 30,
  inadimplencia: 0, inadimplenciaDelta: 0, inadimplenciaValor: 0,
  margem: 0,
});

const categorias = ref<Categoria[]>([]);
const aging = ref<AgingBucket[]>([]);
const safras = ref<Safra[]>([]);
const topParceiros = ref<Parceiro[]>([]);
const errorMsg = ref<string | null>(null);

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }

async function loadData() {
  loading.value = true;
  errorMsg.value = null;
  try {
    const { data } = await api.get(`/admin/bi-financeiro?periodo=${period.value}`, authHeader());
    if (!data || !data.totals) {
      throw new Error('Resposta inválida do servidor.');
    }
    totals.value = {
      faturamento: Number(data.totals.faturamento || 0),
      faturamentoDelta: Number(data.totals.faturamentoDelta || 0),
      cashbackReservado: Number(data.totals.cashbackReservado || 0),
      diasMedio: Number(data.totals.diasMedio || 30),
      inadimplencia: Number(data.totals.inadimplencia || 0),
      inadimplenciaDelta: Number(data.totals.inadimplenciaDelta || 0),
      inadimplenciaValor: Number(data.totals.inadimplenciaValor || 0),
      margem: Number(data.totals.margem || 0),
    };
    categorias.value = Array.isArray(data.categorias) ? data.categorias : [];
    aging.value = Array.isArray(data.aging) ? data.aging : [];
    safras.value = Array.isArray(data.safras) ? data.safras : [];
    topParceiros.value = Array.isArray(data.topParceiros) ? data.topParceiros : [];
  } catch (e: any) {
    errorMsg.value = e?.response?.data?.message
      || e?.message
      || 'Não foi possível carregar o BI financeiro.';
    totals.value = { faturamento: 0, faturamentoDelta: 0, cashbackReservado: 0, diasMedio: 30, inadimplencia: 0, inadimplenciaDelta: 0, inadimplenciaValor: 0, margem: 0 };
    categorias.value = [];
    aging.value = [];
    safras.value = [];
    topParceiros.value = [];
  } finally {
    loading.value = false;
  }
}

watch(period, loadData);
onMounted(loadData);

function formatBRL(v: number): string {
  return (v || 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
</script>

<style scoped>
.qs-bi { padding-bottom: 64px; }
.qs-header-actions { display: flex; align-items: center; gap: 12px; flex-shrink: 0; flex-wrap: wrap; }
.qs-header-text { max-width: 720px; }
.qs-page-header h1 { margin: 4px 0 8px; }
.qs-period-switch { display: flex; gap: 6px; }

.qs-bi-grid { grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); }

.qs-section-head {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 24px;
  flex-wrap: wrap;
  margin-bottom: 20px;
}
.qs-section-head .qs-section-desc { margin-bottom: 0; }

.qs-totalbox {
  text-align: right;
  background: var(--qs-bg);
  padding: 12px 18px;
  border-radius: var(--qs-radius-md);
}
.qs-totalbox-label {
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--qs-gray-500);
}
.qs-totalbox-value {
  font-size: 20px;
  font-weight: 700;
  color: var(--qs-ink);
  letter-spacing: -0.015em;
  font-variant-numeric: tabular-nums;
  margin-top: 2px;
}

/* Bars (categorias) */
.qs-bars { display: flex; flex-direction: column; gap: 18px; }
.qs-bar-row { }
.qs-bar-info {
  display: flex;
  justify-content: space-between;
  align-items: baseline;
  margin-bottom: 6px;
  gap: 12px;
  flex-wrap: wrap;
}
.qs-bar-label { display: flex; align-items: center; gap: 10px; }
.qs-bar-dot { width: 10px; height: 10px; border-radius: 50%; flex-shrink: 0; }
.qs-bar-name {
  font-size: 14px;
  font-weight: 600;
  color: var(--qs-ink);
}
.qs-bar-count {
  font-size: 12px;
  color: var(--qs-gray-400);
}
.qs-bar-value { display: flex; gap: 12px; align-items: baseline; }
.qs-bar-amount {
  font-size: 14px;
  font-weight: 600;
  color: var(--qs-ink);
  font-variant-numeric: tabular-nums;
}
.qs-bar-pct {
  font-size: 12px;
  color: var(--qs-gray-500);
  min-width: 48px;
  text-align: right;
  font-variant-numeric: tabular-nums;
}

/* Aging */
.qs-aging-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 16px;
}
.qs-aging-card {
  background: #fff;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  padding: 18px;
  position: relative;
  overflow: hidden;
}
.qs-aging-bucket {
  font-size: 12px;
  font-weight: 600;
  color: var(--qs-gray-500);
  letter-spacing: 0.04em;
  text-transform: uppercase;
}
.qs-aging-value {
  font-size: 22px;
  font-weight: 700;
  color: var(--qs-ink);
  margin-top: 8px;
  letter-spacing: -0.015em;
  font-variant-numeric: tabular-nums;
}
.qs-aging-count {
  font-size: 12px;
  color: var(--qs-gray-400);
  margin-top: 4px;
}
.qs-aging-risk-strip {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  height: 3px;
}
.risk-low .qs-aging-risk-strip { background: #16a34a; }
.risk-med .qs-aging-risk-strip { background: #f59e0b; }
.risk-high .qs-aging-risk-strip { background: #ea580c; }
.risk-critical .qs-aging-risk-strip { background: #dc2626; }

/* Cashback safras table */
.qs-table {
  border-radius: var(--qs-radius-md);
  overflow: hidden;
  border: 1px solid var(--qs-gray-200);
}
.qs-table-head, .qs-row.qs-tab-cashback {
  display: grid;
  grid-template-columns: 1.4fr 1fr 1fr 1fr 1fr 1fr;
  gap: 12px;
  padding: 12px 20px;
  align-items: center;
}
.qs-table-head {
  background: var(--qs-bg);
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--qs-gray-500);
}
.qs-row.qs-tab-cashback {
  border-top: 1px solid var(--qs-gray-100);
  background: #fff;
  transition: background var(--qs-duration) var(--qs-ease);
}
.qs-row.qs-tab-cashback:hover { background: var(--qs-gray-50); }

.qs-mes { font-size: 14px; font-weight: 600; color: var(--qs-ink); }
.qs-mes-meta { font-size: 11px; color: var(--qs-gray-400); margin-top: 2px; }
.qs-num {
  font-size: 14px;
  color: var(--qs-gray-700);
  font-variant-numeric: tabular-nums;
}
.qs-num-pos { color: #16a34a; font-weight: 600; }
.qs-num-neg { color: #dc2626; }
.qs-num-warn { color: #d97706; font-weight: 600; }

.qs-status-badge {
  font-size: 11px;
  font-weight: 600;
  padding: 4px 10px;
  border-radius: var(--qs-radius-pill);
  letter-spacing: 0.02em;
  white-space: nowrap;
  display: inline-block;
}
.badge-done { background: #dcfce7; color: #16a34a; }
.badge-warn { background: #fef3c7; color: #d97706; }

.qs-error-state {
  background: #fff;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  padding: 48px 24px;
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  margin: 32px 0;
}
.qs-error-state h3 { font-size: 18px; color: var(--qs-ink); margin: 0; }
.qs-error-state p { color: var(--qs-gray-600); margin: 0; max-width: 480px; }

/* Top parceiros */
.qs-toplist { display: flex; flex-direction: column; gap: 10px; }
.qs-top-item {
  display: grid;
  grid-template-columns: 40px 1.6fr 2fr 1fr;
  gap: 16px;
  align-items: center;
  padding: 12px 16px;
  background: #fff;
  border: 1px solid var(--qs-gray-100);
  border-radius: var(--qs-radius-md);
  transition: border-color var(--qs-duration) var(--qs-ease), transform var(--qs-duration) var(--qs-ease);
}
.qs-top-item:hover {
  border-color: var(--qs-teal);
  transform: translateX(2px);
}
.qs-top-rank {
  font-size: 12px;
  font-weight: 700;
  color: var(--qs-gray-400);
  letter-spacing: 0.04em;
}
.qs-top-name {
  font-size: 14px;
  font-weight: 600;
  color: var(--qs-ink);
}
.qs-top-meta {
  font-size: 11px;
  color: var(--qs-gray-500);
  margin-top: 2px;
}
.qs-top-bar { padding: 0 4px; }
.qs-top-value {
  font-size: 14px;
  font-weight: 700;
  color: var(--qs-ink);
  text-align: right;
  font-variant-numeric: tabular-nums;
}

@media (max-width: 768px) {
  .qs-page-header h1 { font-size: 28px; }
  .qs-table-head, .qs-row.qs-tab-cashback { grid-template-columns: 1fr 1fr 1fr; }
  .qs-table-head > :nth-child(2), .qs-row.qs-tab-cashback > :nth-child(2),
  .qs-table-head > :nth-child(3), .qs-row.qs-tab-cashback > :nth-child(3) { display: none; }
  .qs-top-item { grid-template-columns: 32px 1fr 1fr; }
  .qs-top-bar { display: none; }
}
</style>
