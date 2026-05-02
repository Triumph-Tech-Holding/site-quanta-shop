<template>
  <div class="qs-checkout-verify">
    <!-- Cliente recorrente -->
    <div class="qs-cv-item">
      <p class="qs-cv-reveal">
        Já é cliente?
        <button @click="openLogin = !openLogin" type="button" class="qs-cv-btn">
          Clique aqui para entrar
        </button>
      </p>
      <div v-if="openLogin" class="qs-cv-content">
        <forms-login-form />
      </div>
    </div>

    <!-- Cupom de desconto -->
    <div class="qs-cv-item">
      <p class="qs-cv-reveal">
        Possui um cupom?
        <button @click="openCoupon = !openCoupon" type="button" class="qs-cv-btn">
          Clique aqui para inserir o código
        </button>
      </p>
      <div v-if="openCoupon" class="qs-cv-content">
        <form @submit.prevent="applyCoupon" class="qs-coupon-form">
          <div class="qs-coupon-input-wrap">
            <input
              type="text"
              v-model.trim="couponCode"
              placeholder="Ex: QUANTA10"
              :disabled="loading || appliedCoupon !== null"
              class="qs-coupon-input"
              autocomplete="off"
            />
            <button
              v-if="appliedCoupon"
              type="button"
              @click="removeCoupon"
              class="qs-coupon-clear"
              aria-label="Remover cupom"
            >×</button>
          </div>
          <button
            v-if="!appliedCoupon"
            type="submit"
            class="qs-coupon-btn"
            :disabled="loading || !couponCode"
          >
            <span v-if="loading">Validando…</span>
            <span v-else>Aplicar</span>
          </button>
          <button
            v-else
            type="button"
            @click="removeCoupon"
            class="qs-coupon-btn qs-coupon-btn--remove"
          >Remover</button>
        </form>

        <div v-if="errorMsg" class="qs-coupon-msg qs-coupon-msg--err">
          {{ errorMsg }}
        </div>
        <div v-if="appliedCoupon" class="qs-coupon-msg qs-coupon-msg--ok">
          <strong>{{ appliedCoupon.codigo }}</strong> aplicado:
          <span v-if="appliedCoupon.tipo === 'percent'">{{ appliedCoupon.valor }}% de desconto</span>
          <span v-else>R$ {{ appliedCoupon.valor.toFixed(2).replace('.', ',') }} de desconto</span>
        </div>
      </div>
    </div>

    <!-- Quanta Points (resgate) -->
    <div class="qs-cv-item">
      <p class="qs-cv-reveal">
        Tem Quanta Points?
        <button @click="openPoints = !openPoints" type="button" class="qs-cv-btn">
          Clique aqui para usar saldo
        </button>
      </p>
      <div v-if="openPoints" class="qs-cv-content">
        <div class="qs-points-summary">
          <div class="qs-points-saldo">
            <span class="qs-points-label">Saldo disponível</span>
            <span class="qs-points-value">{{ pointsBalance.toLocaleString('pt-BR') }} pts</span>
            <span class="qs-points-equiv">≈ R$ {{ (pointsBalance * pointValue).toFixed(2).replace('.', ',') }}</span>
          </div>
          <label class="qs-points-toggle">
            <input type="checkbox" v-model="usePoints" @change="emitPoints">
            <span class="qs-toggle-slider"></span>
            <span>Usar pontos</span>
          </label>
        </div>
        <div v-if="usePoints" class="qs-points-input-wrap">
          <label>Quantos pontos resgatar</label>
          <input
            type="range"
            :min="0"
            :max="pointsBalance"
            step="10"
            v-model.number="pointsToUse"
            @input="emitPoints"
            class="qs-range"
          />
          <div class="qs-points-applied">
            <strong>{{ pointsToUse.toLocaleString('pt-BR') }} pts</strong>
            <span> = R$ {{ (pointsToUse * pointValue).toFixed(2).replace('.', ',') }} de desconto</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";

interface AppliedCoupon { codigo: string; tipo: 'percent' | 'fixed'; valor: number; }

const emit = defineEmits<{
  (e: 'coupon-applied', coupon: AppliedCoupon | null): void;
  (e: 'points-applied', payload: { points: number; valueBRL: number }): void;
}>();

const openLogin = ref(false);
const openCoupon = ref(false);
const openPoints = ref(false);

const couponCode = ref('');
const loading = ref(false);
const errorMsg = ref('');
const appliedCoupon = ref<AppliedCoupon | null>(null);

// Quanta Points
const pointsBalance = ref(1280);
const pointValue = ref(1.0);
const usePoints = ref(false);
const pointsToUse = ref(0);

const agenciaStore = useAgenciaStore();
const api = useApi();

function authHeader() {
  const token = agenciaStore.getToken?.();
  return token ? { headers: { Authorization: `Bearer ${token}` } } : {};
}

async function applyCoupon() {
  errorMsg.value = '';
  if (!couponCode.value) return;
  loading.value = true;
  try {
    const { data } = await api.post('/cupom/validar', { codigo: couponCode.value }, authHeader());
    if (data?.valido) {
      appliedCoupon.value = { codigo: couponCode.value.toUpperCase(), tipo: data.tipo || 'percent', valor: Number(data.valor || 0) };
      emit('coupon-applied', appliedCoupon.value);
    } else {
      errorMsg.value = data?.mensagem || 'Cupom inválido ou expirado.';
    }
  } catch {
    // Fallback simulado para dev sem backend
    const code = couponCode.value.toUpperCase();
    if (code === 'QUANTA10') { appliedCoupon.value = { codigo: code, tipo: 'percent', valor: 10 }; emit('coupon-applied', appliedCoupon.value); }
    else if (code === 'BEMVINDO') { appliedCoupon.value = { codigo: code, tipo: 'fixed', valor: 25 }; emit('coupon-applied', appliedCoupon.value); }
    else { errorMsg.value = 'Cupom inválido ou expirado.'; }
  } finally {
    loading.value = false;
  }
}

function removeCoupon() {
  appliedCoupon.value = null;
  couponCode.value = '';
  errorMsg.value = '';
  emit('coupon-applied', null);
}

function emitPoints() {
  if (!usePoints.value) {
    pointsToUse.value = 0;
    emit('points-applied', { points: 0, valueBRL: 0 });
    return;
  }
  emit('points-applied', { points: pointsToUse.value, valueBRL: pointsToUse.value * pointValue.value });
}

onMounted(async () => {
  try {
    const { data } = await api.get('/usuario/quanta-points', authHeader());
    if (data) {
      pointsBalance.value = data.saldo ?? pointsBalance.value;
      pointValue.value = data.valorPonto ?? pointValue.value;
    }
  } catch { /* mantém defaults */ }
});
</script>

<style scoped>
.qs-checkout-verify {
  background: var(--qs-bg, #F4F4F5);
  border-radius: var(--qs-radius-lg, 16px);
  padding: 4px;
  margin-bottom: 24px;
}
.qs-cv-item {
  background: #fff;
  border-radius: var(--qs-radius-md, 12px);
  padding: 16px 20px;
  margin: 4px;
  border: 1px solid var(--qs-gray-100, #f5f5f7);
}
.qs-cv-reveal {
  margin: 0;
  font-size: 14px;
  font-weight: 500;
  color: var(--qs-gray-700, #374151);
}
.qs-cv-btn {
  background: none;
  border: none;
  color: var(--qs-teal, #2F7785);
  font-weight: 600;
  cursor: pointer;
  padding: 0 0 0 6px;
  font-size: 14px;
  text-decoration: underline;
}
.qs-cv-btn:hover { color: var(--qs-teal-dark, #225F6B); }
.qs-cv-content {
  margin-top: 12px;
  padding-top: 16px;
  border-top: 1px solid var(--qs-gray-100, #f5f5f7);
}

.qs-coupon-form { display: flex; gap: 8px; align-items: stretch; }
.qs-coupon-input-wrap { flex: 1; position: relative; }
.qs-coupon-input {
  width: 100%;
  padding: 10px 14px;
  border: 1px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  font-family: inherit;
  font-size: 14px;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  transition: border-color .2s, box-shadow .2s;
}
.qs-coupon-input:focus {
  outline: none;
  border-color: var(--qs-teal, #2F7785);
  box-shadow: 0 0 0 3px rgba(47, 119, 133, 0.12);
}
.qs-coupon-clear {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: var(--qs-gray-400, #9ca3af);
  font-size: 22px;
  line-height: 1;
  cursor: pointer;
  padding: 0 4px;
}
.qs-coupon-btn {
  padding: 0 18px;
  background: var(--qs-teal, #2F7785);
  color: #fff;
  border: none;
  border-radius: var(--qs-radius-md, 12px);
  font-weight: 600;
  font-size: 14px;
  cursor: pointer;
  transition: background .2s;
}
.qs-coupon-btn:hover:not(:disabled) { background: var(--qs-teal-dark, #225F6B); }
.qs-coupon-btn:disabled { opacity: .5; cursor: not-allowed; }
.qs-coupon-btn--remove {
  background: #fff;
  color: var(--qs-danger, #dc2626);
  border: 1px solid var(--qs-danger, #dc2626);
}
.qs-coupon-msg {
  margin-top: 10px;
  padding: 8px 12px;
  border-radius: 8px;
  font-size: 13px;
}
.qs-coupon-msg--err { background: #fee2e2; color: #b91c1c; }
.qs-coupon-msg--ok { background: #dcfce7; color: #15803d; }

.qs-points-summary {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 16px;
  flex-wrap: wrap;
}
.qs-points-saldo { display: flex; flex-direction: column; gap: 2px; }
.qs-points-label {
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--qs-gray-500, #6b7280);
}
.qs-points-value {
  font-size: 22px;
  font-weight: 700;
  color: var(--qs-ink, #1d1d1f);
  letter-spacing: -0.02em;
  font-variant-numeric: tabular-nums;
}
.qs-points-equiv {
  font-size: 12px;
  color: var(--qs-gray-500, #6b7280);
}

.qs-points-toggle { display: inline-flex; align-items: center; gap: 8px; cursor: pointer; font-size: 13px; }
.qs-points-toggle input { display: none; }
.qs-toggle-slider {
  width: 38px; height: 22px;
  background: var(--qs-gray-200, #e5e7eb);
  border-radius: 999px;
  position: relative;
  transition: background .2s;
}
.qs-toggle-slider::after {
  content: '';
  position: absolute;
  top: 2px; left: 2px;
  width: 18px; height: 18px;
  background: #fff;
  border-radius: 50%;
  box-shadow: 0 1px 3px rgba(0,0,0,.2);
  transition: left .2s;
}
.qs-points-toggle input:checked + .qs-toggle-slider { background: var(--qs-teal, #2F7785); }
.qs-points-toggle input:checked + .qs-toggle-slider::after { left: 18px; }

.qs-points-input-wrap { margin-top: 16px; }
.qs-points-input-wrap label {
  display: block;
  font-size: 12px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  color: var(--qs-gray-700, #374151);
  margin-bottom: 8px;
}
.qs-range {
  width: 100%;
  -webkit-appearance: none;
  appearance: none;
  height: 4px;
  background: var(--qs-gray-200, #e5e7eb);
  border-radius: 999px;
  outline: none;
}
.qs-range::-webkit-slider-thumb {
  -webkit-appearance: none;
  appearance: none;
  width: 18px; height: 18px;
  border-radius: 50%;
  background: var(--qs-teal, #2F7785);
  cursor: pointer;
  box-shadow: 0 2px 6px rgba(47,119,133,.4);
}
.qs-range::-moz-range-thumb {
  width: 18px; height: 18px;
  border-radius: 50%;
  background: var(--qs-teal, #2F7785);
  cursor: pointer;
  border: none;
}
.qs-points-applied { margin-top: 8px; font-size: 13px; color: var(--qs-gray-700, #374151); }
.qs-points-applied strong { color: var(--qs-teal-dark, #225F6B); }
</style>
