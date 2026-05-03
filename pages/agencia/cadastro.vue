<template>
  <div class="agc-page">
    <div class="agc-wrap">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="agc-logo" />

      <!-- Progress steps -->
      <div class="agc-steps">
        <div
          v-for="(s, i) in stepLabels"
          :key="i"
          class="agc-step"
          :class="{
            'agc-step--active': currentStep === i + 1,
            'agc-step--done': currentStep > i + 1,
          }"
        >
          <div class="agc-step__circle">
            <svg v-if="currentStep > i + 1" width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="3"><path d="m5 13 4 4L19 7"/></svg>
            <span v-else>{{ i + 1 }}</span>
          </div>
          <span class="agc-step__label">{{ s }}</span>
          <div v-if="i < stepLabels.length - 1" class="agc-step__line" :class="{ 'agc-step__line--done': currentStep > i + 1 }"></div>
        </div>
      </div>

      <!-- ── Etapa 1: Dados pessoais ── -->
      <transition name="agc-fade" mode="out-in">
        <div v-if="currentStep === 1" key="step1" class="agc-card">
          <h2 class="agc-card__h2">Seus dados</h2>
          <p class="agc-card__sub">Preencha as informações básicas da sua conta.</p>

          <div class="agc-grid">
            <div class="agc-field agc-field--full">
              <label class="agc-label">Nome completo *</label>
              <input v-model="form.nome" type="text" class="agc-input" placeholder="João da Silva" required :disabled="loading" />
            </div>
            <div class="agc-field">
              <label class="agc-label">Login *</label>
              <input v-model="form.login" type="text" class="agc-input" placeholder="joaosilva" required :disabled="loading" />
              <span class="agc-hint">Sem espaços ou caracteres especiais</span>
            </div>
            <div class="agc-field">
              <label class="agc-label">CPF *</label>
              <input v-model="form.cpf" type="text" class="agc-input" maxlength="14" placeholder="000.000.000-00" required :disabled="loading" @input="maskCpf" />
            </div>
            <div class="agc-field agc-field--full">
              <label class="agc-label">E-mail *</label>
              <input v-model="form.email" type="email" class="agc-input" placeholder="joao@email.com" required :disabled="loading" />
            </div>
            <div v-if="indicadorSlug" class="agc-field agc-field--full">
              <label class="agc-label">Código de indicação</label>
              <input v-model="form.loginIndicador" type="text" class="agc-input agc-input--readonly" :disabled="true" />
            </div>
          </div>

          <div v-if="stepError" class="agc-alert agc-alert--danger">
            <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
            {{ stepError }}
          </div>

          <div class="agc-actions">
            <button type="button" class="agc-btn-next" @click="goStep2">
              Próximo
              <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
            </button>
          </div>

          <p class="agc-login-hint">
            Já tem conta?
            <NuxtLink to="/agencia/login" class="agc-link">Entrar agora</NuxtLink>
          </p>
        </div>

        <!-- ── Etapa 2: Segurança ── -->
        <div v-else-if="currentStep === 2" key="step2" class="agc-card">
          <h2 class="agc-card__h2">Crie sua senha</h2>
          <p class="agc-card__sub">Escolha uma senha segura para proteger sua conta.</p>

          <div class="agc-grid">
            <div class="agc-field agc-field--full">
              <label class="agc-label">Senha *</label>
              <div class="agc-input-wrap">
                <input v-model="form.senha" :type="showPassword ? 'text' : 'password'" class="agc-input" placeholder="Mínimo 6 caracteres" required :disabled="loading" />
                <button type="button" class="agc-eye" @click="showPassword = !showPassword" tabindex="-1">
                  <svg v-if="showPassword" width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"/><line x1="1" y1="1" x2="23" y2="23"/></svg>
                  <svg v-else width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                </button>
              </div>
              <!-- Força da senha -->
              <div class="agc-strength">
                <div class="agc-strength__bars">
                  <div v-for="n in 4" :key="n" class="agc-strength__bar" :class="strengthClass(n)" />
                </div>
                <span class="agc-strength__label" :class="`agc-strength__label--${strengthLevel}`">{{ strengthText }}</span>
              </div>
            </div>
            <div class="agc-field agc-field--full">
              <label class="agc-label">Confirmar senha *</label>
              <div class="agc-input-wrap">
                <input v-model="form.confirmarSenha" :type="showPassword2 ? 'text' : 'password'" class="agc-input" placeholder="Repita a senha" required :disabled="loading" />
                <button type="button" class="agc-eye" @click="showPassword2 = !showPassword2" tabindex="-1">
                  <svg v-if="showPassword2" width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"/><line x1="1" y1="1" x2="23" y2="23"/></svg>
                  <svg v-else width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                </button>
              </div>
            </div>
          </div>

          <div v-if="stepError" class="agc-alert agc-alert--danger">
            <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
            {{ stepError }}
          </div>

          <div class="agc-actions">
            <button type="button" class="agc-btn-back" @click="currentStep = 1">
              <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M19 12H5M12 5l-7 7 7 7"/></svg>
              Voltar
            </button>
            <button type="button" class="agc-btn-next" @click="goStep3">
              Próximo
              <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
            </button>
          </div>
        </div>

        <!-- ── Etapa 3: Confirmar e enviar ── -->
        <div v-else-if="currentStep === 3" key="step3" class="agc-card">
          <div v-if="!successMsg">
            <h2 class="agc-card__h2">Confirmar dados</h2>
            <p class="agc-card__sub">Revise as informações antes de criar sua conta.</p>

            <div class="agc-review">
              <div class="agc-review__row">
                <span class="agc-review__lbl">Nome</span>
                <span class="agc-review__val">{{ form.nome }}</span>
              </div>
              <div class="agc-review__row">
                <span class="agc-review__lbl">Login</span>
                <span class="agc-review__val">{{ form.login }}</span>
              </div>
              <div class="agc-review__row">
                <span class="agc-review__lbl">CPF</span>
                <span class="agc-review__val">{{ form.cpf }}</span>
              </div>
              <div class="agc-review__row">
                <span class="agc-review__lbl">E-mail</span>
                <span class="agc-review__val">{{ form.email }}</span>
              </div>
              <div v-if="form.loginIndicador" class="agc-review__row">
                <span class="agc-review__lbl">Indicador</span>
                <span class="agc-review__val">{{ form.loginIndicador }}</span>
              </div>
            </div>

            <div v-if="errorMsg" class="agc-alert agc-alert--danger">
              <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
              {{ errorMsg }}
            </div>

            <div class="agc-actions">
              <button type="button" class="agc-btn-back" @click="currentStep = 2">
                <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M19 12H5M12 5l-7 7 7 7"/></svg>
                Voltar
              </button>
              <button type="button" class="agc-btn-next" @click="cadastrar" :disabled="loading">
                <span v-if="loading" class="agc-spinner" />
                {{ loading ? 'Aguarde...' : 'Criar conta' }}
              </button>
            </div>
          </div>

          <!-- Sucesso -->
          <div v-else class="agc-success">
            <div class="agc-success__icon">
              <svg width="40" height="40" fill="none" viewBox="0 0 24 24" stroke="#16a34a" stroke-width="2"><circle cx="12" cy="12" r="10"/><path d="m9 12 2 2 4-4"/></svg>
            </div>
            <h2 class="agc-success__h2">Conta criada!</h2>
            <p class="agc-success__txt">{{ successMsg }}</p>
            <NuxtLink to="/agencia/login" class="agc-btn-next" style="text-decoration:none; justify-content:center;">
              Fazer login
            </NuxtLink>
          </div>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';

definePageMeta({ layout: 'agencia-login' });

const route = useRoute();
const api = useApi();

const indicadorSlug = computed(() => String(route.query.indicador ?? ''));
const form = reactive({
  nome: '', login: '', cpf: '', email: '', senha: '', confirmarSenha: '',
  loginIndicador: indicadorSlug.value,
});

const currentStep = ref(1);
const loading = ref(false);
const errorMsg = ref('');
const stepError = ref('');
const successMsg = ref('');
const showPassword = ref(false);
const showPassword2 = ref(false);

const stepLabels = ['Dados pessoais', 'Segurança', 'Confirmação'];

function maskCpf() {
  let v = form.cpf.replace(/\D/g, '').slice(0, 11);
  if (v.length > 9) v = v.replace(/(\d{3})(\d{3})(\d{3})(\d{1,2})/, '$1.$2.$3-$4');
  else if (v.length > 6) v = v.replace(/(\d{3})(\d{3})(\d{1,3})/, '$1.$2.$3');
  else if (v.length > 3) v = v.replace(/(\d{3})(\d{1,3})/, '$1.$2');
  form.cpf = v;
}

const passwordScore = computed(() => {
  const p = form.senha;
  if (!p) return 0;
  let score = 0;
  if (p.length >= 8) score++;
  if (/[A-Z]/.test(p)) score++;
  if (/[0-9]/.test(p)) score++;
  if (/[^A-Za-z0-9]/.test(p)) score++;
  return score;
});

const strengthLevel = computed(() => {
  const s = passwordScore.value;
  if (s <= 1) return 'weak';
  if (s === 2) return 'fair';
  if (s === 3) return 'good';
  return 'strong';
});

const strengthText = computed(() => {
  const map: Record<string, string> = { weak: 'Fraca', fair: 'Razoável', good: 'Boa', strong: 'Forte' };
  return form.senha ? map[strengthLevel.value] : '';
});

function strengthClass(n: number) {
  if (!form.senha || passwordScore.value < n) return 'agc-strength__bar--empty';
  const cls: Record<string, string> = { weak: 'agc-strength__bar--weak', fair: 'agc-strength__bar--fair', good: 'agc-strength__bar--good', strong: 'agc-strength__bar--strong' };
  return cls[strengthLevel.value];
}

function goStep2() {
  stepError.value = '';
  if (!form.nome.trim()) { stepError.value = 'Nome é obrigatório.'; return; }
  if (!form.login.trim()) { stepError.value = 'Login é obrigatório.'; return; }
  if (!form.cpf.trim()) { stepError.value = 'CPF é obrigatório.'; return; }
  if (!form.email.trim()) { stepError.value = 'E-mail é obrigatório.'; return; }
  currentStep.value = 2;
}

function goStep3() {
  stepError.value = '';
  if (!form.senha) { stepError.value = 'Senha é obrigatória.'; return; }
  if (form.senha.length < 6) { stepError.value = 'Senha deve ter ao menos 6 caracteres.'; return; }
  if (form.senha !== form.confirmarSenha) { stepError.value = 'As senhas não conferem.'; return; }
  currentStep.value = 3;
}

async function cadastrar() {
  loading.value = true;
  errorMsg.value = '';
  try {
    await api.post('/UsuarioLogin/cadastro', form);
    successMsg.value = 'Verifique seu e-mail para ativar a conta e depois faça login.';
  } catch (err: unknown) {
    errorMsg.value = extractApiErrorMessage(err, 'Não foi possível criar a conta. Tente novamente.');
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.agc-page {
  min-height: 100vh;
  background: #ecf2f7;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding: 40px 16px 80px;
}
.agc-wrap { width: 100%; max-width: 540px; }
.agc-logo { display: block; width: 130px; margin: 0 auto 28px; }

/* Steps */
.agc-steps {
  display: flex;
  align-items: flex-start;
  justify-content: center;
  gap: 0;
  margin-bottom: 28px;
}
.agc-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  flex: 1;
}
.agc-step__circle {
  width: 32px; height: 32px;
  border-radius: 50%;
  background: #e5e7eb;
  color: #9ca3af;
  font-size: 13px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  z-index: 1;
  transition: all 0.3s;
}
.agc-step--active .agc-step__circle { background: #2f7785; color: #fff; box-shadow: 0 0 0 4px rgba(47,119,133,0.15); }
.agc-step--done .agc-step__circle { background: #98c73a; color: #fff; }
.agc-step__label {
  font-size: 11px;
  font-weight: 600;
  color: #9ca3af;
  margin-top: 6px;
  text-align: center;
  white-space: nowrap;
  transition: color 0.3s;
}
.agc-step--active .agc-step__label { color: #2f7785; }
.agc-step--done .agc-step__label { color: #98c73a; }
.agc-step__line {
  position: absolute;
  top: 16px;
  left: calc(50% + 16px);
  right: calc(-50% + 16px);
  height: 2px;
  background: #e5e7eb;
  transition: background 0.3s;
}
.agc-step__line--done { background: #98c73a; }

/* Card */
.agc-card {
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 4px 24px rgba(0,0,0,0.09);
  padding: 32px;
}
.agc-card__h2 { font-size: 22px; font-weight: 800; color: #225f6b; margin-bottom: 6px; letter-spacing: -0.02em; }
.agc-card__sub { font-size: 14px; color: #6b7280; margin-bottom: 24px; }

/* Grid */
.agc-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; margin-bottom: 20px; }
@media (max-width: 480px) { .agc-grid { grid-template-columns: 1fr; } }
.agc-field { display: flex; flex-direction: column; }
.agc-field--full { grid-column: 1 / -1; }
.agc-label { font-size: 13px; font-weight: 600; color: #374151; margin-bottom: 6px; }
.agc-input {
  padding: 11px 14px;
  border: 1.5px solid #d1d5db;
  border-radius: 10px;
  font-size: 14px;
  color: #111;
  background: #f9fafb;
  box-sizing: border-box;
  outline: none;
  width: 100%;
  transition: border-color 0.2s, box-shadow 0.2s;
}
.agc-input:focus { border-color: #2f7785; box-shadow: 0 0 0 3px rgba(47,119,133,0.1); background: #fff; }
.agc-input--readonly { background: #f3f4f6; color: #6b7280; cursor: not-allowed; }
.agc-hint { font-size: 11px; color: #9ca3af; margin-top: 4px; }

.agc-input-wrap { position: relative; }
.agc-input-wrap .agc-input { padding-right: 40px; }
.agc-eye {
  position: absolute; right: 10px; top: 50%; transform: translateY(-50%);
  background: none; border: none; cursor: pointer; color: #9ca3af; padding: 4px;
  display: flex; align-items: center;
}
.agc-eye:hover { color: #2f7785; }

/* Password strength */
.agc-strength { display: flex; align-items: center; gap: 10px; margin-top: 8px; }
.agc-strength__bars { display: flex; gap: 4px; }
.agc-strength__bar { width: 36px; height: 4px; border-radius: 2px; transition: background 0.3s; }
.agc-strength__bar--empty { background: #e5e7eb; }
.agc-strength__bar--weak { background: #ef4444; }
.agc-strength__bar--fair { background: #f59e0b; }
.agc-strength__bar--good { background: #3b82f6; }
.agc-strength__bar--strong { background: #22c55e; }
.agc-strength__label { font-size: 11px; font-weight: 600; }
.agc-strength__label--weak { color: #ef4444; }
.agc-strength__label--fair { color: #f59e0b; }
.agc-strength__label--good { color: #3b82f6; }
.agc-strength__label--strong { color: #22c55e; }

/* Review */
.agc-review { background: #f9fafb; border-radius: 10px; border: 1px solid #e5e7eb; margin-bottom: 20px; overflow: hidden; }
.agc-review__row { display: flex; justify-content: space-between; padding: 12px 16px; border-bottom: 1px solid #f0f0f0; }
.agc-review__row:last-child { border-bottom: none; }
.agc-review__lbl { font-size: 13px; color: #9ca3af; font-weight: 500; }
.agc-review__val { font-size: 13px; color: #374151; font-weight: 600; text-align: right; word-break: break-all; }

/* Alert */
.agc-alert {
  display: flex; align-items: center; gap: 8px;
  padding: 10px 14px; border-radius: 8px;
  font-size: 13px; margin-bottom: 16px; font-weight: 500;
}
.agc-alert--danger { background: #fee2e2; color: #dc2626; }

/* Actions */
.agc-actions { display: flex; gap: 10px; justify-content: flex-end; }
.agc-btn-next {
  display: inline-flex; align-items: center; gap: 8px;
  background: linear-gradient(135deg, #225f6b, #2f7785);
  color: #fff; border: none; border-radius: 10px;
  padding: 12px 24px; font-size: 14px; font-weight: 700;
  cursor: pointer; transition: all 0.2s;
}
.agc-btn-next:hover:not(:disabled) { transform: translateY(-1px); box-shadow: 0 6px 20px rgba(34,95,107,0.3); }
.agc-btn-next:disabled { opacity: 0.6; cursor: not-allowed; }
.agc-btn-back {
  display: inline-flex; align-items: center; gap: 6px;
  background: transparent; color: #6b7280;
  border: 1.5px solid #d1d5db; border-radius: 10px;
  padding: 12px 20px; font-size: 14px; font-weight: 600;
  cursor: pointer; transition: all 0.2s;
}
.agc-btn-back:hover { border-color: #2f7785; color: #2f7785; }

.agc-spinner {
  width: 14px; height: 14px;
  border: 2px solid rgba(255,255,255,0.4);
  border-top-color: #fff;
  border-radius: 50%;
  animation: agc-spin 0.7s linear infinite;
}
@keyframes agc-spin { to { transform: rotate(360deg); } }

/* Login hint */
.agc-login-hint { text-align: center; margin-top: 20px; font-size: 13.5px; color: #6b7280; }
.agc-link { color: #2f7785; font-weight: 600; text-decoration: none; }
.agc-link:hover { text-decoration: underline; color: #98c73a; }

/* Success */
.agc-success { text-align: center; padding: 20px 0; }
.agc-success__icon {
  width: 72px; height: 72px;
  background: #dcfce7; border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  margin: 0 auto 20px;
}
.agc-success__h2 { font-size: 24px; font-weight: 800; color: #225f6b; margin-bottom: 10px; }
.agc-success__txt { font-size: 15px; color: #6b7280; line-height: 1.6; margin-bottom: 28px; }

/* Fade transition */
.agc-fade-enter-active, .agc-fade-leave-active { transition: opacity 0.2s, transform 0.2s; }
.agc-fade-enter-from { opacity: 0; transform: translateX(12px); }
.agc-fade-leave-to { opacity: 0; transform: translateX(-12px); }
</style>
