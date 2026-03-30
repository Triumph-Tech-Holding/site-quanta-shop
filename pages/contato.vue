<template>
  <div class="qct-page">

    <!-- Hero -->
    <section class="qct-hero-new">
      <div class="qct-hero-new__photo-wrap">
        <img
          src="https://images.unsplash.com/photo-1577563908411-5077b6dc7624?w=1400&q=80&auto=format&fit=crop"
          alt="Equipe de atendimento Quanta Shop"
          class="qct-hero-new__photo"
        />
      </div>
      <div class="qct-hero-new__overlay"></div>
      <div class="container qct-hero-new__body">
        <span class="qct-hero-new__pill">
          <span class="qct-hero-new__pill-dot"></span>
          Atendimento
        </span>
        <h1>Fale com a <span class="qct-hero-new__accent">Quanta Shop</span></h1>
        <p>Tem uma dúvida, sugestão ou precisa de suporte? Nossa equipe está pronta para ajudar.</p>
        <div class="qct-hero-new__badges">
          <div class="qct-hero-new__badge">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/></svg>
            <div>
              <span>Resposta em</span>
              <strong>5 minutos</strong>
            </div>
          </div>
          <div class="qct-hero-new__badge">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
            <div>
              <span>Atendimento</span>
              <strong>Seg–Sex 8h–18h</strong>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Canais rápidos -->
    <section class="qct-channels">
      <div class="container">
        <div class="qct-channel-cards">
          <a href="https://wa.me/5511999999999" target="_blank" class="qct-channel-card qct-channel-card--whatsapp">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="currentColor"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 0 1-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 0 1-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 0 1 2.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0 0 12.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 0 0 5.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 0 0-3.48-8.413z"/></svg>
            <div>
              <strong>WhatsApp</strong>
              <span>Resposta em até 5 minutos</span>
            </div>
          </a>
          <a href="mailto:contato@quantashop.com.br" class="qct-channel-card">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"/><polyline points="22,6 12,13 2,6"/></svg>
            <div>
              <strong>E-mail</strong>
              <span>contato@quantashop.com.br</span>
            </div>
          </a>
          <div class="qct-channel-card">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/></svg>
            <div>
              <strong>Horário de atendimento</strong>
              <span>Seg–Sex, 8h–18h</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Formulário + Info -->
    <section class="qct-main">
      <div class="container qct-main__grid">

        <!-- Formulário -->
        <div class="qct-form-box">
          <h2>Envie sua mensagem</h2>
          <p>Responderemos em até 24 horas úteis.</p>

          <div v-if="sent" class="qct-success">
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2.5"><polyline points="20 6 9 17 4 12"/></svg>
            Mensagem enviada com sucesso! Retornaremos em breve.
          </div>

          <form v-else class="qct-form" @submit.prevent="submitForm">
            <div class="qct-form-row">
              <div class="qct-form-group">
                <label>Nome completo *</label>
                <input v-model="form.name" type="text" placeholder="Seu nome" required />
              </div>
              <div class="qct-form-group">
                <label>E-mail *</label>
                <input v-model="form.email" type="email" placeholder="seu@email.com" required />
              </div>
            </div>
            <div class="qct-form-row">
              <div class="qct-form-group">
                <label>Telefone</label>
                <input v-model="form.phone" type="tel" placeholder="(11) 9 9999-9999" />
              </div>
              <div class="qct-form-group">
                <label>Assunto *</label>
                <select v-model="form.subject" required>
                  <option value="" disabled>Selecione</option>
                  <option>Suporte técnico</option>
                  <option>Credenciamento de loja</option>
                  <option>Saque e pagamentos</option>
                  <option>Quero ser agente</option>
                  <option>Parceria comercial</option>
                  <option>Outro</option>
                </select>
              </div>
            </div>
            <div class="qct-form-group">
              <label>Mensagem *</label>
              <textarea v-model="form.message" rows="5" placeholder="Descreva sua dúvida ou solicitação..." required></textarea>
            </div>
            <button type="submit" class="qct-submit" :disabled="loading">
              <span v-if="!loading">Enviar mensagem →</span>
              <span v-else>Enviando...</span>
            </button>
          </form>
        </div>

        <!-- Informações -->
        <div class="qct-info-box">
          <h3>Informações de contato</h3>
          <ul class="qct-info-list">
            <li>
              <div class="qct-info-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/></svg>
              </div>
              <div>
                <strong>Endereço</strong>
                <span>Av. Paulista, 1000 — São Paulo, SP</span>
              </div>
            </li>
            <li>
              <div class="qct-info-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07A19.5 19.5 0 0 1 4.69 13a19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 3.6 2h3a2 2 0 0 1 2 1.72c.127.96.361 1.903.7 2.81a2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0 1 22 16.92z"/></svg>
              </div>
              <div>
                <strong>Telefone</strong>
                <span>(11) 3000-0000</span>
              </div>
            </li>
            <li>
              <div class="qct-info-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"/><polyline points="22,6 12,13 2,6"/></svg>
              </div>
              <div>
                <strong>E-mail</strong>
                <span>contato@quantashop.com.br</span>
              </div>
            </li>
            <li>
              <div class="qct-info-icon" style="color: #25D366;">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 0 1-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 0 1-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 0 1 2.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0 0 12.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 0 0 5.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 0 0-3.48-8.413z"/></svg>
              </div>
              <div>
                <strong>WhatsApp</strong>
                <span>(11) 9 9999-9999</span>
              </div>
            </li>
          </ul>

          <!-- Mapa placeholder -->
          <div class="qct-map">
            <div class="qct-map__inner">
              <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.5"><path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/></svg>
              <p>Av. Paulista, 1000<br>São Paulo, SP</p>
            </div>
          </div>
        </div>

      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ title: "Fale Conosco | Quanta Shop", description: "Entre em contato com a equipe Quanta Shop. Suporte, parceria e atendimento.", canonical: "https://quantashop.com.br/contato" });

const sent = ref(false);
const loading = ref(false);

const form = reactive({
  name: '', email: '', phone: '', subject: '', message: '',
});

async function submitForm() {
  loading.value = true;
  await new Promise(r => setTimeout(r, 1200));
  loading.value = false;
  sent.value = true;
}
</script>

<style scoped>
.qct-page { font-family: 'Inter', 'Jost', sans-serif; }

/* ─── New hero ───────────────────────────────────────────── */
.qct-hero-new {
  position: relative;
  min-height: 360px;
  display: flex;
  align-items: center;
  overflow: hidden;
  text-align: center;
}
.qct-hero-new__photo-wrap {
  position: absolute;
  inset: 0;
  z-index: 0;
}
.qct-hero-new__photo {
  width: 100%; height: 100%;
  object-fit: cover; object-position: center 30%;
  display: block;
}
.qct-hero-new__overlay {
  position: absolute; inset: 0; z-index: 1;
  background: linear-gradient(100deg, rgba(20,55,65,0.90) 0%, rgba(34,95,107,0.82) 100%);
}
.qct-hero-new__body {
  position: relative; z-index: 2;
  padding: 72px 0 64px;
  display: flex; flex-direction: column; align-items: center;
}
.qct-hero-new__pill {
  display: inline-flex; align-items: center; gap: 8px;
  background: rgba(255,255,255,0.12);
  border: 1px solid rgba(255,255,255,0.25);
  border-radius: 999px; padding: 5px 14px 5px 10px;
  font-size: 13px; font-weight: 500; color: rgba(255,255,255,0.92);
  margin-bottom: 20px;
}
.qct-hero-new__pill-dot {
  width: 7px; height: 7px; border-radius: 50%; background: #98C73A; flex-shrink: 0;
}
.qct-hero-new__body h1 {
  font-size: clamp(26px, 3vw, 46px); font-weight: 800; color: #fff;
  margin: 0 0 14px; letter-spacing: -0.02em;
}
.qct-hero-new__accent { color: #98C73A; }
.qct-hero-new__body > p {
  font-size: 17px; color: rgba(255,255,255,0.75);
  max-width: 520px; margin: 0 auto 32px; line-height: 1.65;
}
.qct-hero-new__badges {
  display: flex; gap: 14px; flex-wrap: wrap; justify-content: center;
}
.qct-hero-new__badge {
  display: flex; align-items: center; gap: 12px;
  background: rgba(15,30,40,0.55); backdrop-filter: blur(12px);
  border: 1px solid rgba(255,255,255,0.18); border-radius: 12px;
  padding: 12px 18px;
}
.qct-hero-new__badge span { display: block; font-size: 11px; color: rgba(255,255,255,0.55); text-transform: uppercase; letter-spacing: 0.06em; }
.qct-hero-new__badge strong { display: block; font-size: 15px; font-weight: 700; color: #fff; }

.qct-label { display: inline-block; font-size: 12px; font-weight: 600; letter-spacing: 0.08em; text-transform: uppercase; color: #2F7785; background: rgba(47,119,133,0.08); padding: 4px 12px; border-radius: 999px; margin-bottom: 12px; }
.qct-label--hero { color: #98C73A; background: rgba(152,199,58,0.12); }

/* Channels */
.qct-channels { background: #fff; padding: 40px 0; border-bottom: 1px solid #f0f0f0; }
.qct-channel-cards { display: flex; gap: 20px; flex-wrap: wrap; }
.qct-channel-card { flex: 1; min-width: 200px; display: flex; align-items: center; gap: 14px; background: #f7f8fa; border: 1.5px solid #e5e7eb; border-radius: 14px; padding: 18px 20px; text-decoration: none; color: inherit; transition: border-color 0.2s, box-shadow 0.2s; }
.qct-channel-card:hover { border-color: #2F7785; box-shadow: 0 4px 16px rgba(47,119,133,0.1); }
.qct-channel-card--whatsapp { border-color: #25D366; }
.qct-channel-card--whatsapp svg { color: #25D366; }
.qct-channel-card strong { display: block; font-size: 14px; font-weight: 600; color: #111827; margin-bottom: 2px; }
.qct-channel-card span { font-size: 12px; color: #6b7280; }

/* Main */
.qct-main { background: #f7f8fa; padding: 72px 0; }
.qct-main__grid { display: grid; grid-template-columns: 1fr 380px; gap: 48px; align-items: start; }
@media (max-width: 900px) { .qct-main__grid { grid-template-columns: 1fr; } }

.qct-form-box { background: #fff; border-radius: 20px; padding: 40px; box-shadow: 0 2px 20px rgba(0,0,0,0.06); }
.qct-form-box h2 { font-size: 22px; font-weight: 700; color: #111827; margin-bottom: 4px; }
.qct-form-box > p { font-size: 14px; color: #6b7280; margin-bottom: 28px; }

.qct-form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
@media (max-width: 600px) { .qct-form-row { grid-template-columns: 1fr; } }
.qct-form-group { display: flex; flex-direction: column; margin-bottom: 16px; }
.qct-form-group label { font-size: 13px; font-weight: 500; color: #374151; margin-bottom: 6px; }
.qct-form-group input,
.qct-form-group select,
.qct-form-group textarea {
  font-family: 'Inter','Jost',sans-serif;
  font-size: 14px;
  color: #374151;
  background: #f7f8fa;
  border: 1.5px solid #e5e7eb;
  border-radius: 8px;
  padding: 10px 14px;
  outline: none;
  transition: border-color 0.2s;
  resize: vertical;
}
.qct-form-group input:focus,
.qct-form-group select:focus,
.qct-form-group textarea:focus { border-color: #2F7785; background: #fff; }

.qct-submit {
  width: 100%;
  background: #98C73A;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 14px 24px;
  font-family: 'Inter','Jost',sans-serif;
  font-size: 15px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
  margin-top: 4px;
}
.qct-submit:hover:not(:disabled) { background: #7aad1f; }
.qct-submit:disabled { opacity: 0.7; cursor: not-allowed; }

.qct-success {
  display: flex;
  align-items: center;
  gap: 12px;
  background: rgba(152,199,58,0.1);
  border: 1.5px solid #98C73A;
  border-radius: 10px;
  padding: 16px 20px;
  font-size: 15px;
  color: #374151;
  margin-top: 8px;
}

/* Info box */
.qct-info-box { background: #fff; border-radius: 20px; padding: 36px; box-shadow: 0 2px 20px rgba(0,0,0,0.06); }
.qct-info-box h3 { font-size: 18px; font-weight: 700; color: #111827; margin-bottom: 24px; }
.qct-info-list { list-style: none; padding: 0; margin: 0; }
.qct-info-list li { display: flex; align-items: flex-start; gap: 14px; padding: 14px 0; border-bottom: 1px solid #f3f4f6; }
.qct-info-list li:last-child { border-bottom: none; }
.qct-info-icon { width: 36px; height: 36px; border-radius: 8px; background: rgba(47,119,133,0.08); display: flex; align-items: center; justify-content: center; flex-shrink: 0; color: #2F7785; }
.qct-info-list strong { display: block; font-size: 13px; font-weight: 600; color: #111827; margin-bottom: 2px; }
.qct-info-list span { font-size: 13px; color: #6b7280; }

.qct-map {
  margin-top: 20px;
  background: #f0f9f7;
  border: 1.5px solid #d1e8e5;
  border-radius: 12px;
  height: 140px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.qct-map__inner { text-align: center; }
.qct-map__inner p { font-size: 13px; color: #6b7280; margin: 8px 0 0; line-height: 1.5; }
</style>
