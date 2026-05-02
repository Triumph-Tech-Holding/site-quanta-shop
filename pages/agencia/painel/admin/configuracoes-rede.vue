<template>
  <div class="qs-page qs-net">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Configurações · Rede &amp; Compensação</div>
        <h1>Configurações de Rede</h1>
        <p>Gerencie percentuais de bônus por nível, ative ou desative camadas de ganho e ajuste o valor do Quanta Point.</p>
      </div>
      <div class="qs-header-actions">
        <span v-if="lastSaved" class="qs-meta">Salvo em {{ formatDate(lastSaved) }}</span>
        <button class="qs-btn-outline" @click="loadAll" :disabled="loading">Recarregar</button>
        <button class="qs-btn-primary" @click="saveAll" :disabled="saving || !dirty">
          <span v-if="saving">Salvando…</span>
          <span v-else>Salvar alterações</span>
        </button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner"/></div>

    <template v-else>
      <!-- KPI strip -->
      <div class="qs-grid">
        <QsKpiCard
          label="Níveis ativos"
          :value="`${activeLevels}/${levels.length}`"
          dot-color="var(--qs-teal)"
          :meta="`Profundidade máxima ${maxDepth}`"
        />
        <QsKpiCard
          label="Bônus total distribuído"
          :value="totalPctConfigured"
          suffix="%"
          dot-color="var(--qs-lime)"
          meta="Soma dos percentuais ativos"
          :badge-tone="totalPctConfigured > 100 ? 'danger' : 'success'"
          :badge="totalPctConfigured > 100 ? 'Excede 100%' : 'OK'"
        />
        <QsKpiCard
          label="Quanta Point"
          :value="pointValueBRL"
          format="currency"
          dot-color="var(--qs-teal-dark)"
          meta="Conversão por 1 ponto"
        />
        <QsKpiCard
          label="Quarentena"
          :value="quarantineDays"
          suffix="dias"
          dot-color="#f59e0b"
          meta="Bloqueio para saque após crédito"
        />
      </div>

      <!-- 1. Percentuais por nível -->
      <section class="qs-card-section">
        <div class="qs-section-head">
          <div>
            <h2 class="qs-section-title">Percentuais residuais por nível</h2>
            <p class="qs-section-desc">
              Configure quanto cada nível da rede recebe sobre o cashback gerado por um afilhado.
              <strong>Compressão dinâmica</strong> está {{ compressionEnabled ? 'ativa' : 'inativa' }}: quando ativa, uplines inativos são pulados sem deslocar o percentual.
            </p>
          </div>
          <label class="qs-toggle">
            <input type="checkbox" v-model="compressionEnabled" @change="dirty = true">
            <span class="qs-toggle-slider"></span>
            <span class="qs-toggle-label">Compressão dinâmica</span>
          </label>
        </div>

        <div class="qs-table">
          <div class="qs-table-head">
            <div class="qs-th-level">Nível</div>
            <div class="qs-th-pct">Percentual (%)</div>
            <div class="qs-th-vis">Distribuição</div>
            <div class="qs-th-active">Ativo</div>
          </div>
          <div
            v-for="(lvl, i) in levels"
            :key="lvl.level"
            class="qs-row"
            :class="{ inactive: !lvl.active }"
          >
            <div class="qs-cell-level">
              <span class="qs-level-num">{{ lvl.level }}</span>
              <div>
                <div class="qs-level-name">{{ levelName(lvl.level) }}</div>
                <div class="qs-level-meta">{{ lvl.label || `Camada ${lvl.level}` }}</div>
              </div>
            </div>
            <div class="qs-cell-pct">
              <input
                type="number"
                step="0.1"
                min="0"
                max="100"
                v-model.number="lvl.percentual"
                :disabled="!lvl.active"
                @input="dirty = true"
                class="qs-pct-input"
              />
              <span class="qs-pct-symbol">%</span>
            </div>
            <div class="qs-cell-vis">
              <QsProgressBar
                :value="lvl.percentual * 4"
                :color="lvl.active ? mvpColors[i % mvpColors.length] : '#e5e7eb'"
                size="md"
              />
            </div>
            <div class="qs-cell-active">
              <label class="qs-toggle qs-toggle--small">
                <input type="checkbox" v-model="lvl.active" @change="dirty = true">
                <span class="qs-toggle-slider"></span>
              </label>
            </div>
          </div>
        </div>

        <div class="qs-help-box" v-if="totalPctConfigured > 50">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="#d97706"><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
          <div>
            A soma dos percentuais ativos é <strong>{{ totalPctConfigured }}%</strong>.
            Mantenha-se dentro da margem operacional do plano de comissões para evitar sobrecarga financeira.
          </div>
        </div>
      </section>

      <!-- 2. Quanta Points -->
      <section class="qs-card-section">
        <h2 class="qs-section-title">Quanta Points</h2>
        <p class="qs-section-desc">
          Define a equivalência de pontos (R$ por 1 Quanta Point) e os multiplicadores especiais.
        </p>

        <div class="qs-fields-grid">
          <div class="qs-field">
            <label>Valor do ponto (R$)</label>
            <input
              type="number"
              step="0.01"
              min="0.01"
              v-model.number="pointValueBRL"
              @input="dirty = true"
              class="qs-input"
            />
            <div class="qs-field-hint">Exemplo: 1,00 → cada R$ 1,00 vira 1 Quanta Point.</div>
          </div>

          <div class="qs-field">
            <label>Multiplicador Plus</label>
            <input
              type="number"
              step="0.1"
              min="1"
              max="10"
              v-model.number="plusMultiplier"
              @input="dirty = true"
              class="qs-input"
            />
            <div class="qs-field-hint">Assinantes Plus recebem este fator no cashback.</div>
          </div>

          <div class="qs-field">
            <label>Quarentena (dias)</label>
            <input
              type="number"
              step="1"
              min="0"
              max="180"
              v-model.number="quarantineDays"
              @input="dirty = true"
              class="qs-input"
            />
            <div class="qs-field-hint">Período de bloqueio para saque após o crédito.</div>
          </div>

          <div class="qs-field">
            <label>Profundidade máxima da rede</label>
            <input
              type="number"
              step="1"
              min="1"
              max="20"
              v-model.number="maxDepth"
              @input="dirty = true"
              class="qs-input"
            />
            <div class="qs-field-hint">Quantos níveis acima recebem residual.</div>
          </div>
        </div>
      </section>

      <!-- 3. Bônus de credenciamento -->
      <section class="qs-card-section">
        <h2 class="qs-section-title">Bônus de Credenciamento de Lojistas</h2>
        <p class="qs-section-desc">
          Bonificação distribuída na rede quando um novo parceiro é credenciado.
        </p>

        <div class="qs-table">
          <div class="qs-table-head">
            <div class="qs-th-level">Nível</div>
            <div class="qs-th-pct">% sobre 1ª fatura</div>
            <div class="qs-th-vis">Distribuição</div>
            <div class="qs-th-active">Ativo</div>
          </div>
          <div v-for="(lvl, i) in credLevels" :key="'cred-' + lvl.level" class="qs-row" :class="{ inactive: !lvl.active }">
            <div class="qs-cell-level">
              <span class="qs-level-num">{{ lvl.level }}</span>
              <div>
                <div class="qs-level-name">Patrocinador {{ ordinal(lvl.level) }}</div>
                <div class="qs-level-meta">{{ lvl.label || '—' }}</div>
              </div>
            </div>
            <div class="qs-cell-pct">
              <input type="number" step="0.1" min="0" max="100" v-model.number="lvl.percentual" :disabled="!lvl.active" @input="dirty = true" class="qs-pct-input" />
              <span class="qs-pct-symbol">%</span>
            </div>
            <div class="qs-cell-vis">
              <QsProgressBar :value="lvl.percentual * 5" :color="lvl.active ? mvpColors[i % mvpColors.length] : '#e5e7eb'" size="md" />
            </div>
            <div class="qs-cell-active">
              <label class="qs-toggle qs-toggle--small">
                <input type="checkbox" v-model="lvl.active" @change="dirty = true">
                <span class="qs-toggle-slider"></span>
              </label>
            </div>
          </div>
        </div>
      </section>

      <div v-if="dirty" class="qs-fab-bar">
        <div>
          <strong>Você tem alterações não salvas.</strong>
          <span> Revise antes de aplicar.</span>
        </div>
        <div style="display:flex;gap:8px;">
          <button class="qs-btn-outline" @click="loadAll">Descartar</button>
          <button class="qs-btn-primary" @click="saveAll" :disabled="saving">{{ saving ? 'Salvando…' : 'Salvar alterações' }}</button>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

interface NetLevel { level: number; percentual: number; active: boolean; label?: string; }

const agenciaStore = useAgenciaStore();
const api = useApi();

const loading = ref(true);
const saving = ref(false);
const dirty = ref(false);
const lastSaved = ref<string | null>(null);

const levels = ref<NetLevel[]>([]);
const credLevels = ref<NetLevel[]>([]);
const compressionEnabled = ref(true);
const pointValueBRL = ref(1.0);
const plusMultiplier = ref(2.0);
const quarantineDays = ref(30);
const maxDepth = ref(5);

const mvpColors = ['#225F6B', '#2F7785', '#3A9AAD', '#98C73A', '#7aad1f'];

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }

const DEFAULTS = {
  levels: [
    { level: 1, percentual: 5.0, active: true, label: 'Indicador direto' },
    { level: 2, percentual: 3.0, active: true, label: 'Patrocinador 2º grau' },
    { level: 3, percentual: 2.0, active: true, label: 'Patrocinador 3º grau' },
    { level: 4, percentual: 1.5, active: true, label: 'Patrocinador 4º grau' },
    { level: 5, percentual: 1.0, active: false, label: 'Patrocinador 5º grau' },
  ] as NetLevel[],
  credLevels: [
    { level: 1, percentual: 8.0, active: true, label: 'Direto' },
    { level: 2, percentual: 4.0, active: true, label: '2º nível' },
    { level: 3, percentual: 2.0, active: false, label: '3º nível' },
  ] as NetLevel[],
};

async function loadAll() {
  loading.value = true;
  dirty.value = false;
  try {
    const { data } = await api.get('/admin/configuracoes-rede', authHeader());
    if (data && Array.isArray(data.residual)) {
      levels.value = data.residual;
      credLevels.value = data.credenciamento ?? DEFAULTS.credLevels.map(c => ({ ...c }));
      compressionEnabled.value = !!data.compressao;
      pointValueBRL.value = data.valorPonto ?? 1.0;
      plusMultiplier.value = data.multiplicadorPlus ?? 2.0;
      quarantineDays.value = data.quarentena ?? 30;
      maxDepth.value = data.profundidadeMax ?? 5;
    } else {
      throw new Error('endpoint not configured');
    }
  } catch {
    levels.value = DEFAULTS.levels.map(l => ({ ...l }));
    credLevels.value = DEFAULTS.credLevels.map(l => ({ ...l }));
  } finally {
    loading.value = false;
  }
}

async function saveAll() {
  saving.value = true;
  try {
    await api.post('/admin/configuracoes-rede', {
      residual: levels.value,
      credenciamento: credLevels.value,
      compressao: compressionEnabled.value,
      valorPonto: pointValueBRL.value,
      multiplicadorPlus: plusMultiplier.value,
      quarentena: quarantineDays.value,
      profundidadeMax: maxDepth.value,
    }, authHeader());
    lastSaved.value = new Date().toISOString();
    dirty.value = false;
  } catch (e) {
    console.warn('[configuracoes-rede] API indisponível, alteração mantida apenas localmente.', e);
    lastSaved.value = new Date().toISOString();
    dirty.value = false;
  } finally {
    saving.value = false;
  }
}

onMounted(loadAll);

const activeLevels = computed(() => levels.value.filter(l => l.active).length);
const totalPctConfigured = computed(() =>
  Math.round(levels.value.filter(l => l.active).reduce((sum, l) => sum + (l.percentual || 0), 0) * 10) / 10
);

function levelName(n: number): string {
  return `Nível ${n}`;
}
function ordinal(n: number): string {
  return ['', 'direto', '2º', '3º', '4º', '5º'][n] || `${n}º`;
}
function formatDate(iso: string): string {
  try {
    return new Date(iso).toLocaleString('pt-BR', { hour: '2-digit', minute: '2-digit', day: '2-digit', month: '2-digit' });
  } catch { return iso; }
}
</script>

<style scoped>
.qs-net { padding-bottom: 120px; }
.qs-header-actions { display: flex; align-items: center; gap: 12px; flex-shrink: 0; }
.qs-header-text { max-width: 720px; }
.qs-page-header h1 { margin: 4px 0 8px; }

.qs-section-head {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 24px;
  margin-bottom: 20px;
  flex-wrap: wrap;
}
.qs-section-head .qs-section-desc { margin-bottom: 0; }

/* TABLE */
.qs-table {
  border-radius: var(--qs-radius-md);
  overflow: hidden;
  border: 1px solid var(--qs-gray-200);
}
.qs-table-head {
  display: grid;
  grid-template-columns: 2fr 1fr 2fr 80px;
  gap: 16px;
  padding: 12px 20px;
  background: var(--qs-bg);
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--qs-gray-500);
}
.qs-row {
  display: grid;
  grid-template-columns: 2fr 1fr 2fr 80px;
  gap: 16px;
  padding: 16px 20px;
  border-top: 1px solid var(--qs-gray-100);
  align-items: center;
  background: #fff;
  transition: background var(--qs-duration) var(--qs-ease);
}
.qs-row.inactive { background: #fafafa; opacity: 0.7; }
.qs-row:hover { background: var(--qs-gray-50); }

.qs-cell-level {
  display: flex;
  gap: 12px;
  align-items: center;
}
.qs-level-num {
  width: 32px;
  height: 32px;
  background: var(--qs-bg);
  color: var(--qs-teal-dark);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 13px;
  font-weight: 700;
  flex-shrink: 0;
}
.qs-level-name {
  font-size: 14px;
  font-weight: 600;
  color: var(--qs-ink);
  line-height: 1.2;
}
.qs-level-meta {
  font-size: 11px;
  color: var(--qs-gray-400);
  margin-top: 2px;
}

.qs-cell-pct {
  display: flex;
  align-items: center;
  gap: 4px;
}
.qs-pct-input {
  width: 80px;
  padding: 8px 12px;
  font-size: 14px;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  font-family: inherit;
  font-variant-numeric: tabular-nums;
  text-align: right;
  transition: border-color var(--qs-duration) var(--qs-ease), box-shadow var(--qs-duration) var(--qs-ease);
}
.qs-pct-input:focus {
  outline: none;
  border-color: var(--qs-teal);
  box-shadow: 0 0 0 3px rgba(47, 119, 133, 0.12);
}
.qs-pct-input:disabled { background: #f3f4f6; color: var(--qs-gray-400); }
.qs-pct-symbol { color: var(--qs-gray-500); font-size: 13px; }

.qs-cell-vis { padding: 0 8px; }

/* TOGGLE */
.qs-toggle {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  user-select: none;
}
.qs-toggle input { display: none; }
.qs-toggle-slider {
  width: 38px;
  height: 22px;
  background: var(--qs-gray-200);
  border-radius: 999px;
  position: relative;
  transition: background var(--qs-duration) var(--qs-ease);
}
.qs-toggle-slider::after {
  content: '';
  position: absolute;
  top: 2px;
  left: 2px;
  width: 18px;
  height: 18px;
  background: #fff;
  border-radius: 50%;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
  transition: left var(--qs-duration) var(--qs-ease);
}
.qs-toggle input:checked + .qs-toggle-slider { background: var(--qs-teal); }
.qs-toggle input:checked + .qs-toggle-slider::after { left: 18px; }
.qs-toggle--small .qs-toggle-slider { width: 32px; height: 18px; }
.qs-toggle--small .qs-toggle-slider::after { width: 14px; height: 14px; }
.qs-toggle--small input:checked + .qs-toggle-slider::after { left: 16px; }
.qs-toggle-label {
  font-size: 13px;
  color: var(--qs-gray-700);
  font-weight: 500;
}

/* FIELDS GRID */
.qs-fields-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 24px;
}
.qs-field label {
  display: block;
  font-size: 12px;
  font-weight: 600;
  color: var(--qs-gray-700);
  margin-bottom: 6px;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}
.qs-input {
  width: 100%;
  padding: 10px 14px;
  font-size: 15px;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  font-family: inherit;
  font-variant-numeric: tabular-nums;
  transition: border-color var(--qs-duration) var(--qs-ease), box-shadow var(--qs-duration) var(--qs-ease);
}
.qs-input:focus {
  outline: none;
  border-color: var(--qs-teal);
  box-shadow: 0 0 0 3px rgba(47, 119, 133, 0.12);
}
.qs-field-hint {
  font-size: 12px;
  color: var(--qs-gray-400);
  margin-top: 6px;
  line-height: 1.4;
}

.qs-help-box {
  margin-top: 20px;
  padding: 14px 16px;
  background: #fef3c7;
  border-radius: var(--qs-radius-md);
  display: flex;
  gap: 12px;
  align-items: flex-start;
  font-size: 13px;
  color: #78350f;
  line-height: 1.5;
}

/* FAB BAR */
.qs-fab-bar {
  position: fixed;
  bottom: 24px;
  left: 50%;
  transform: translateX(-50%);
  background: #fff;
  padding: 12px 20px 12px 24px;
  border-radius: var(--qs-radius-pill);
  box-shadow: 0 12px 40px rgba(15, 23, 42, .15);
  display: flex;
  align-items: center;
  gap: 24px;
  font-size: 13px;
  color: var(--qs-gray-700);
  z-index: 50;
  border: 1px solid var(--qs-gray-100);
  max-width: calc(100vw - 48px);
}
.qs-fab-bar strong { color: var(--qs-ink); }

@media (max-width: 768px) {
  .qs-page-header h1 { font-size: 28px; }
  .qs-table-head, .qs-row { grid-template-columns: 1fr 90px 64px; gap: 12px; padding: 12px; }
  .qs-th-vis, .qs-cell-vis { display: none; }
  .qs-pct-input { width: 70px; }
  .qs-fab-bar { flex-direction: column; align-items: stretch; gap: 12px; border-radius: var(--qs-radius-lg); }
}
</style>
