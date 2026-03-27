<template>
  <section v-if="config.ceo.ativo !== false" class="qs-ceo">
    <div class="container">
      <div class="qs-ceo__card" :style="config.ceo.imagemFundo ? { backgroundImage: `url(${config.ceo.imagemFundo})`, backgroundSize: 'cover', backgroundPosition: config.ceo.posicaoFundo ?? 'center' } : {}">
        <div v-if="config.ceo.imagemFundo" class="qs-ceo__overlay" :style="{ background: `rgba(34,95,107,${config.ceo.overlayOpacity ?? 0.72})` }"></div>
        <div class="qs-ceo__content">
          <span class="qs-ceo__tag">{{ config.ceo.tag }}</span>
          <p class="qs-ceo__pre">{{ config.ceo.pre }}</p>
          <h2 class="qs-ceo__name">{{ config.ceo.name }}</h2>
          <p class="qs-ceo__desc">{{ config.ceo.desc }}</p>
          <div style="display: flex; gap: 14px; flex-wrap: wrap;">
            <button @click="openModal" class="qs-ceo__btn" style="background: #98C73A; color: #225F6B; border: 0;">
              <svg width="18" height="18" fill="currentColor" viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8zm3.5-9c.83 0 1.5-.67 1.5-1.5S16.33 8 15.5 8 14 8.67 14 9.5s.67 1.5 1.5 1.5zm-7 0c.83 0 1.5-.67 1.5-1.5S9.33 8 8.5 8 7 8.67 7 9.5 7.67 11 8.5 11zm3.5 6.5c2.33 0 4.31-1.46 5.11-3.5H6.89c.8 2.04 2.78 3.5 5.11 3.5z"/></svg>
              {{ config.ceo.ctaText }}
            </button>
          </div>
        </div>
        <!-- Mobile: badges à esquerda + foto à direita -->
        <div v-if="config.ceo.imagemFundo" class="qs-ceo__mobile-row">
          <div class="qs-ceo__mobile-badges">
            <div class="qs-ceo__mbadge">
              <svg width="12" height="12" fill="none" viewBox="0 0 24 24" stroke="rgba(255,255,255,0.75)" stroke-width="2"><path d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"/></svg>
              <div>
                <div class="qs-ceo__mbadge-label">{{ config.ceo.badge1Label || 'Respostas' }}</div>
                <div class="qs-ceo__mbadge-value">{{ config.ceo.badge1Value || 'Em até 24h' }}</div>
              </div>
            </div>
            <div class="qs-ceo__mbadge">
              <svg width="12" height="12" fill="none" viewBox="0 0 24 24" stroke="rgba(255,255,255,0.75)" stroke-width="2"><path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/></svg>
              <div>
                <div class="qs-ceo__mbadge-label">{{ config.ceo.badge2Label || 'Parcerias' }}</div>
                <div class="qs-ceo__mbadge-value">{{ config.ceo.badge2Value || '+200 fechadas' }}</div>
              </div>
            </div>
          </div>
          <div class="qs-ceo__mobile-photo">
            <img :src="config.ceo.imagemFundo" alt="Mauro Triumph" class="qs-ceo__mobile-photo-img" />
            <div class="qs-ceo__mobile-photo-fade"></div>
          </div>
        </div>

        <!-- Desktop: badges originais -->
        <div class="qs-ceo__badges">
          <div class="qs-ceo__badge">
            <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2"><path d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"/></svg>
            <div>
              <div class="qs-ceo__badge-label">{{ config.ceo.badge1Label || 'Respostas' }}</div>
              <div class="qs-ceo__badge-value">{{ config.ceo.badge1Value || 'Em até 24h' }}</div>
            </div>
          </div>
          <div class="qs-ceo__badge">
            <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2"><path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/></svg>
            <div>
              <div class="qs-ceo__badge-label">{{ config.ceo.badge2Label || 'Parcerias' }}</div>
              <div class="qs-ceo__badge-value">{{ config.ceo.badge2Value || '+200 fechadas' }}</div>
            </div>
          </div>
        </div>

      </div>
    </div>

    <!-- Modal IA Chat -->
    <div v-if="showModal" class="qs-modal-overlay" @click="closeModal">
      <div class="qs-modal" @click.stop>
        <div class="qs-modal-header">
          <h3>Conversa com IA</h3>
          <button @click="closeModal" class="qs-modal-close">×</button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-chat">
            <div v-for="(msg, i) in chatMessages" :key="i" :class="['qs-chat-msg', msg.type]">
              {{ msg.text }}
            </div>
          </div>
          <div v-if="currentStep === 0" class="qs-form-group">
            <input v-model="userResponses.name" type="text" placeholder="Qual é seu nome?" class="qs-input" @keyup.enter="nextStep" />
          </div>
          <div v-else-if="currentStep === 1" class="qs-form-group">
            <label v-for="opt in ['Quero cashback', 'Quero credenciar minha loja', 'Quero ser agente']" :key="opt" class="qs-radio-label">
              <input type="radio" v-model="userResponses.interest" :value="opt" />
              {{ opt }}
            </label>
          </div>
          <div v-else-if="currentStep === 2" class="qs-form-group">
            <input v-model="userResponses.email" type="email" placeholder="Seu e-mail" class="qs-input" @keyup.enter="submitChat" />
          </div>
          <div class="qs-modal-footer">
            <button @click="nextStep" v-if="currentStep < 2" class="qs-btn-primary">Próximo</button>
            <button @click="submitChat" v-else class="qs-btn-primary">Enviar</button>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useHomeConfig } from '@/composables/useHomeConfig';

const { config, loadConfig } = useHomeConfig();

onMounted(() => loadConfig());

const showModal = ref(false);
const currentStep = ref(0);
const chatMessages = ref([
  { type: 'bot', text: 'Olá! Qual é seu nome?' }
]);
const userResponses = ref({
  name: '',
  interest: '',
  email: ''
});

const questions = [
  'Qual é seu interesse?',
  'Qual é seu e-mail?'
];

function openModal() {
  showModal.value = true;
  currentStep.value = 0;
  userResponses.value = { name: '', interest: '', email: '' };
  chatMessages.value = [{ type: 'bot', text: 'Olá! Qual é seu nome?' }];
}

function closeModal() {
  showModal.value = false;
}

function nextStep() {
  if (currentStep.value === 0 && !userResponses.value.name) return;
  if (currentStep.value === 1 && !userResponses.value.interest) return;

  if (currentStep.value === 0) {
    chatMessages.value.push({ type: 'user', text: userResponses.value.name });
    chatMessages.value.push({ type: 'bot', text: questions[0] });
  } else if (currentStep.value === 1) {
    chatMessages.value.push({ type: 'user', text: userResponses.value.interest });
    chatMessages.value.push({ type: 'bot', text: questions[1] });
  }
  
  currentStep.value++;
}

function submitChat() {
  if (!userResponses.value.email) return;
  chatMessages.value.push({ type: 'user', text: userResponses.value.email });
  chatMessages.value.push({ type: 'bot', text: 'Obrigado! Em breve entraremos em contato.' });
  setTimeout(() => closeModal(), 2000);
}
</script>

<style scoped>
.qs-ceo {
  padding: 72px 0;
  background: #fff;
}

.qs-ceo__card {
  background: linear-gradient(135deg, #1a4a54 0%, #225F6B 50%, #2F7785 100%);
  border-radius: 20px;
  padding: 52px 56px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 40px;
  position: relative;
  overflow: hidden;
}

.qs-ceo__overlay {
  position: absolute;
  inset: 0;
  border-radius: 20px;
  pointer-events: none;
  z-index: 1;
}

.qs-ceo__card::before {
  content: '';
  position: absolute;
  right: -60px;
  top: -60px;
  width: 280px;
  height: 280px;
  border-radius: 50%;
  background: rgba(255,255,255,0.04);
}

.qs-ceo__mobile-row {
  display: none;
}

@media (max-width: 767px) {
  .qs-ceo__card {
    flex-direction: column;
    padding: 28px 24px 0;
    text-align: center;
    background: linear-gradient(160deg, #1a4a54 0%, #225F6B 55%, #2a7078 100%) !important;
    overflow: hidden;
  }

  .qs-ceo__overlay {
    display: none !important;
  }

  .qs-ceo__badges {
    display: none !important;
  }

  .qs-ceo__mobile-row {
    display: flex;
    flex-direction: row;
    width: calc(100% + 48px);
    margin-left: -24px;
    height: 175px;
    margin-top: 18px;
    flex-shrink: 0;
    overflow: hidden;
  }

  .qs-ceo__mobile-badges {
    display: flex;
    flex-direction: column;
    justify-content: center;
    gap: 8px;
    padding: 0 8px 0 16px;
    flex-shrink: 0;
    width: 155px;
    z-index: 2;
  }

  .qs-ceo__mbadge {
    display: flex;
    align-items: center;
    gap: 6px;
    background: rgba(255, 255, 255, 0.13);
    backdrop-filter: blur(16px);
    -webkit-backdrop-filter: blur(16px);
    border: 1px solid rgba(255, 255, 255, 0.20);
    border-radius: 10px;
    padding: 6px 10px;
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.22);
  }

  .qs-ceo__mbadge-label {
    color: rgba(255, 255, 255, 0.60);
    font-size: 7px;
    font-weight: 500;
    text-transform: uppercase;
    letter-spacing: 0.04em;
    margin-bottom: 1px;
    white-space: nowrap;
    font-family: 'Inter', 'Jost', sans-serif;
  }

  .qs-ceo__mbadge-value {
    color: #fff;
    font-size: 11px;
    font-weight: 700;
    white-space: nowrap;
    font-family: 'Inter', 'Jost', sans-serif;
  }

  .qs-ceo__mobile-photo {
    flex: 1;
    position: relative;
    overflow: hidden;
  }

  .qs-ceo__mobile-photo-img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    object-position: top center;
    display: block;
  }

  .qs-ceo__mobile-photo-fade {
    position: absolute;
    inset: 0;
    background:
      linear-gradient(to right, #225F6B 0%, rgba(34, 95, 107, 0.60) 38%, rgba(34, 95, 107, 0.00) 100%),
      linear-gradient(to bottom, rgba(22, 68, 80, 0.30) 0%, rgba(22, 68, 80, 0.55) 100%);
  }
}

.qs-ceo__content {
  position: relative;
  z-index: 2;
}

.qs-ceo__tag {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: rgba(255,255,255,0.12);
  border: 1px solid rgba(255,255,255,0.20);
  border-radius: 999px;
  color: rgba(255,255,255,0.80);
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 600;
  padding: 4px 14px;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  margin-bottom: 12px;
}

.qs-ceo__pre {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 500;
  color: rgba(255,255,255,0.70);
  margin-bottom: 4px;
  text-transform: uppercase;
  letter-spacing: 0.08em;
}

.qs-ceo__name {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(28px, 4vw, 40px);
  font-weight: 800;
  color: #98C73A;
  letter-spacing: -0.02em;
  margin-bottom: 12px;
}

.qs-ceo__desc {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  color: rgba(255,255,255,0.75);
  line-height: 1.5;
  max-width: 360px;
  margin-bottom: 28px;
}

.qs-ceo__btn {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 700;
  padding: 13px 24px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.qs-ceo__btn:hover {
  background: #7aad1f;
  color: #fff;
  transform: translateY(-2px);
}

.qs-ceo__badges {
  display: flex;
  flex-direction: column;
  gap: 12px;
  flex-shrink: 0;
  position: relative;
  z-index: 2;
}

.qs-ceo__badge {
  display: flex;
  align-items: center;
  gap: 12px;
  background: rgba(255,255,255,0.95);
  backdrop-filter: blur(8px);
  border-radius: 12px;
  padding: 14px 20px;
  min-width: 180px;
}

.qs-ceo__badge-label {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  color: #6b7280;
  margin-bottom: 2px;
}

.qs-ceo__badge-value {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 700;
  color: #111827;
}

.qs-ceo__btn-secondary {
  padding: 10px 24px;
  border: 1px solid rgba(255,255,255,0.30);
  border-radius: 8px;
  background: transparent;
  color: #fff;
  font-weight: 600;
  font-size: 13px;
  font-family: 'Inter', 'Jost', sans-serif;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
}

.qs-ceo__btn-secondary:hover {
  background: rgba(255,255,255,0.1);
  border-color: rgba(255,255,255,0.50);
}

/* Modal */
.qs-modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
}

.qs-modal {
  background: #1f2937;
  border-radius: 12px;
  width: 90%;
  max-width: 420px;
  max-height: 600px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  box-shadow: 0 20px 60px rgba(0,0,0,0.3);
  animation: slideUp 0.3s ease;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.qs-modal-header {
  padding: 20px;
  border-bottom: 1px solid rgba(255,255,255,0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.qs-modal-header h3 {
  color: #fff;
  font-size: 18px;
  font-weight: 700;
  margin: 0;
}

.qs-modal-close {
  background: transparent;
  border: 0;
  color: rgba(255,255,255,0.5);
  font-size: 28px;
  cursor: pointer;
  padding: 0;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.qs-modal-close:hover {
  color: #fff;
}

.qs-modal-body {
  padding: 20px;
  overflow-y: auto;
  flex: 1;
}

.qs-chat {
  margin-bottom: 20px;
  max-height: 200px;
  overflow-y: auto;
}

.qs-chat-msg {
  margin-bottom: 10px;
  padding: 10px 12px;
  border-radius: 8px;
  font-size: 13px;
  line-height: 1.4;
}

.qs-chat-msg.bot {
  background: rgba(152,199,58,0.15);
  color: #a8d84e;
  border-left: 3px solid #98C73A;
}

.qs-chat-msg.user {
  background: rgba(47,119,133,0.15);
  color: #a8d7e8;
  border-left: 3px solid #2F7785;
  margin-left: auto;
  max-width: 80%;
}

.qs-form-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
  margin-bottom: 15px;
}

.qs-input {
  padding: 10px 12px;
  border: 1px solid rgba(255,255,255,0.2);
  border-radius: 6px;
  background: rgba(255,255,255,0.05);
  color: #fff;
  font-size: 13px;
  font-family: 'Inter', 'Jost', sans-serif;
}

.qs-input::placeholder {
  color: rgba(255,255,255,0.4);
}

.qs-input:focus {
  outline: none;
  background: rgba(255,255,255,0.08);
  border-color: #98C73A;
}

.qs-radio-label {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px;
  border-radius: 6px;
  border: 1px solid rgba(255,255,255,0.1);
  cursor: pointer;
  color: rgba(255,255,255,0.8);
  font-size: 13px;
  transition: all 0.2s ease;
}

.qs-radio-label:hover {
  background: rgba(255,255,255,0.05);
  border-color: #98C73A;
}

.qs-radio-label input {
  cursor: pointer;
}

.qs-modal-footer {
  padding: 15px 20px;
  border-top: 1px solid rgba(255,255,255,0.1);
  display: flex;
  gap: 10px;
}

.qs-btn-primary {
  flex: 1;
  padding: 10px 16px;
  background: #98C73A;
  color: #225F6B;
  border: 0;
  border-radius: 6px;
  font-weight: 700;
  font-size: 13px;
  font-family: 'Inter', 'Jost', sans-serif;
  cursor: pointer;
  transition: all 0.2s ease;
}

.qs-btn-primary:hover {
  background: #a8d84e;
  transform: translateY(-2px);
}

.qs-btn-primary:active {
  transform: translateY(0);
}
</style>
