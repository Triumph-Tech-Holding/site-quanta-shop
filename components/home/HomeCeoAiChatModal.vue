<template>
  <div v-if="show" class="qs-modal-overlay" @click="$emit('close')">
    <div class="qs-modal" @click.stop>
      <div class="qs-modal-header">
        <h3>Conversa com IA</h3>
        <button @click="$emit('close')" class="qs-modal-close">×</button>
      </div>
      <div class="qs-modal-body">
        <div class="qs-chat">
          <div v-for="(msg, i) in chatMessages" :key="i" :class="['qs-chat-msg', msg.type]">
            {{ msg.text }}
          </div>
        </div>
        <div v-if="currentStep === 0" class="qs-form-group">
          <input v-model="responses.name" type="text" placeholder="Qual é seu nome?" class="qs-input" @keyup.enter="nextStep" />
        </div>
        <div v-else-if="currentStep === 1" class="qs-form-group">
          <label v-for="opt in ['Quero cashback', 'Quero credenciar minha loja', 'Quero ser agente']" :key="opt" class="qs-radio-label">
            <input type="radio" v-model="responses.interest" :value="opt" />
            {{ opt }}
          </label>
        </div>
        <div v-else-if="currentStep === 2" class="qs-form-group">
          <input v-model="responses.email" type="email" placeholder="Seu e-mail" class="qs-input" @keyup.enter="submitChat" />
        </div>
        <div class="qs-modal-footer">
          <button @click="nextStep" v-if="currentStep < 2" class="qs-btn-primary">Próximo</button>
          <button @click="submitChat" v-else class="qs-btn-primary">Enviar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';

const props = defineProps<{ show: boolean }>();
const emit = defineEmits<{ (e: 'close'): void }>();

const questions = ['Qual é seu interesse?', 'Qual é seu e-mail?'];
const currentStep = ref(0);
const chatMessages = ref([{ type: 'bot', text: 'Olá! Qual é seu nome?' }]);
const responses = ref({ name: '', interest: '', email: '' });

watch(() => props.show, (val) => {
  if (val) {
    currentStep.value = 0;
    responses.value = { name: '', interest: '', email: '' };
    chatMessages.value = [{ type: 'bot', text: 'Olá! Qual é seu nome?' }];
  }
});

function nextStep() {
  if (currentStep.value === 0 && !responses.value.name) return;
  if (currentStep.value === 1 && !responses.value.interest) return;
  if (currentStep.value === 0) {
    chatMessages.value.push({ type: 'user', text: responses.value.name });
    chatMessages.value.push({ type: 'bot', text: questions[0] });
  } else if (currentStep.value === 1) {
    chatMessages.value.push({ type: 'user', text: responses.value.interest });
    chatMessages.value.push({ type: 'bot', text: questions[1] });
  }
  currentStep.value++;
}

function submitChat() {
  if (!responses.value.email) return;
  chatMessages.value.push({ type: 'user', text: responses.value.email });
  chatMessages.value.push({ type: 'bot', text: 'Obrigado! Em breve entraremos em contato.' });
  setTimeout(() => emit('close'), 2000);
}
</script>

<style scoped>
.qs-modal-overlay {
  position: fixed; inset: 0; background: rgba(0,0,0,0.5);
  display: flex; align-items: center; justify-content: center; z-index: 9999;
}
.qs-modal {
  background: #1f2937; border-radius: 12px; width: 90%; max-width: 420px;
  max-height: 600px; display: flex; flex-direction: column; overflow: hidden;
  box-shadow: 0 20px 60px rgba(0,0,0,0.3); animation: slideUp 0.3s ease;
}
@keyframes slideUp {
  from { opacity: 0; transform: translateY(20px); }
  to   { opacity: 1; transform: translateY(0); }
}
.qs-modal-header {
  padding: 20px; border-bottom: 1px solid rgba(255,255,255,0.1);
  display: flex; justify-content: space-between; align-items: center;
}
.qs-modal-header h3 { color: #fff; font-size: 18px; font-weight: 700; margin: 0; }
.qs-modal-close {
  background: transparent; border: 0; color: rgba(255,255,255,0.5);
  font-size: 28px; cursor: pointer; padding: 0;
  width: 30px; height: 30px; display: flex; align-items: center; justify-content: center;
}
.qs-modal-close:hover { color: #fff; }
.qs-modal-body { padding: 20px; overflow-y: auto; flex: 1; }
.qs-chat { margin-bottom: 20px; max-height: 200px; overflow-y: auto; }
.qs-chat-msg { margin-bottom: 10px; padding: 10px 12px; border-radius: 8px; font-size: 13px; line-height: 1.4; }
.qs-chat-msg.bot  { background: rgba(152,199,58,0.15); color: #a8d84e; border-left: 3px solid #98C73A; }
.qs-chat-msg.user { background: rgba(47,119,133,0.15); color: #a8d7e8; border-left: 3px solid #2F7785; margin-left: auto; max-width: 80%; }
.qs-form-group { display: flex; flex-direction: column; gap: 10px; margin-bottom: 15px; }
.qs-input {
  padding: 10px 12px; border: 1px solid rgba(255,255,255,0.2); border-radius: 6px;
  background: rgba(255,255,255,0.05); color: #fff; font-size: 13px; font-family: 'Inter','Jost',sans-serif;
}
.qs-input::placeholder { color: rgba(255,255,255,0.4); }
.qs-input:focus { outline: none; background: rgba(255,255,255,0.08); border-color: #98C73A; }
.qs-radio-label {
  display: flex; align-items: center; gap: 8px; padding: 10px;
  border-radius: 6px; border: 1px solid rgba(255,255,255,0.1);
  cursor: pointer; color: rgba(255,255,255,0.8); font-size: 13px; transition: all .2s;
}
.qs-radio-label:hover { background: rgba(255,255,255,0.05); border-color: #98C73A; }
.qs-modal-footer { padding: 15px 20px; border-top: 1px solid rgba(255,255,255,0.1); display: flex; gap: 10px; }
.qs-btn-primary {
  flex: 1; padding: 10px 16px; background: #98C73A; color: #225F6B; border: 0;
  border-radius: 6px; font-weight: 700; font-size: 13px; font-family: 'Inter','Jost',sans-serif;
  cursor: pointer; transition: all .2s;
}
.qs-btn-primary:hover { background: #a8d84e; transform: translateY(-2px); }
</style>
