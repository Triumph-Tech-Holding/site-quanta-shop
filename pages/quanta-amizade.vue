<template>
  <div class="qa-page">

    <!-- Hero -->
    <section class="qa-hero">
      <div class="container qa-hero__inner">
        <div class="qa-hero__left">
          <span class="qa-badge">
            <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
            Programa de Indicações
          </span>
          <h1>Indique amigos e <span class="qa-accent">ganhe juntos</span></h1>
          <p>Compartilhe seu link, seus amigos se cadastram e compram — você ganha cashback bônus em cada compra deles. Quanto mais amigos, mais você ganha.</p>
          <div class="qa-hero__btns">
            <nuxt-link href="/register" class="qa-btn-primary">Começar a indicar →</nuxt-link>
            <a href="#como-funciona" class="qa-btn-outline">Como funciona</a>
          </div>
        </div>
        <div class="qa-hero__right">
          <div class="qa-stat-cards">
            <div class="qa-stat-card">
              <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
              <div>
                <strong>+47.000</strong>
                <span>amigos indicados no último mês</span>
              </div>
            </div>
            <div class="qa-stat-card">
              <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>
              <div>
                <strong>R$ 128,40</strong>
                <span>cashback médio ganho por indicação</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Como funciona -->
    <section class="qa-section" id="como-funciona">
      <div class="container">
        <div class="qa-section__header">
          <span class="qa-label">Simples assim</span>
          <h2>Como funciona o Quanta Amizade</h2>
          <p>Três passos para começar a ganhar com suas indicações</p>
        </div>
        <div class="qa-steps">
          <div v-for="(step, i) in steps" :key="i" class="qa-step">
            <div class="qa-step__num">{{ i + 1 }}</div>
            <div class="qa-step__icon" v-html="step.icon"></div>
            <h3>{{ step.title }}</h3>
            <p>{{ step.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Recompensas -->
    <section class="qa-section qa-section--gray">
      <div class="container">
        <div class="qa-section__header">
          <span class="qa-label">Tabela de recompensas</span>
          <h2>Quanto você ganha por amigo</h2>
          <p>Seus ganhos crescem conforme sua rede cresce</p>
        </div>
        <div class="qa-rewards">
          <div v-for="r in rewards" :key="r.tier" class="qa-reward-card" :class="{ 'qa-reward-card--highlight': r.highlight }">
            <div class="qa-reward-tier">{{ r.tier }}</div>
            <div class="qa-reward-range">{{ r.range }}</div>
            <div class="qa-reward-value">{{ r.value }}</div>
            <div class="qa-reward-label">por compra do indicado</div>
            <div class="qa-reward-extra">{{ r.extra }}</div>
          </div>
        </div>
      </div>
    </section>

    <!-- Link de convite -->
    <section class="qa-section">
      <div class="container">
        <div class="qa-invite">
          <div class="qa-invite__text">
            <span class="qa-label">Compartilhe agora</span>
            <h2>Seu link de indicação</h2>
            <p>Copie e envie para seus amigos pelo WhatsApp, Instagram ou e-mail. Cada amigo que se cadastrar e fizer uma compra gera cashback para você.</p>
          </div>
          <div class="qa-invite__link">
            <div class="qa-link-box">
              <span class="qa-link-url">quantashop.com.br/r/<strong>{{ userName }}</strong></span>
              <button class="qa-link-copy" @click="copyLink">
                <svg v-if="!copied" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="9" y="9" width="13" height="13" rx="2"/><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"/></svg>
                <svg v-else width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2.5"><polyline points="20 6 9 17 4 12"/></svg>
                {{ copied ? 'Copiado!' : 'Copiar' }}
              </button>
            </div>
            <div class="qa-share-btns">
              <a href="https://wa.me/?text=Ei%2C%20conhe%C3%A7a%20a%20Quanta%20Shop%20e%20ganhe%20cashback%20em%20qualquer%20compra%21%20quantashop.com.br%2Fr%2Fvocê" target="_blank" class="qa-share-btn qa-share-btn--whatsapp">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 0 1-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 0 1-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 0 1 2.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0 0 12.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 0 0 5.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 0 0-3.48-8.413z"/></svg>
                WhatsApp
              </a>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="qa-cta">
      <div class="container qa-cta__inner">
        <h2>Ainda não tem conta? Crie agora</h2>
        <p>Cadastre-se gratuitamente e receba seu link de indicação em segundos.</p>
        <nuxt-link href="/register" class="qa-btn-primary">Criar conta grátis →</nuxt-link>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ title: "Quanta Amizade | Quanta Shop", description: "Indique amigos para a Quanta Shop e ganhe cashback bônus em cada compra deles." });

const copied = ref(false);
const userName = ref('seu-usuario');

function copyLink() {
  navigator.clipboard.writeText(`https://quantashop.com.br/r/${userName.value}`).catch(() => {});
  copied.value = true;
  setTimeout(() => { copied.value = false; }, 2500);
}

const steps = [
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M4 12v8a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-8"/><polyline points="16 6 12 2 8 6"/><line x1="12" y1="2" x2="12" y2="15"/></svg>',
    title: 'Copie seu link único',
    desc: 'Após se cadastrar, você recebe um link pessoal de indicação para compartilhar.',
  },
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>',
    title: 'Seus amigos se cadastram',
    desc: 'Eles criam a conta usando seu link e passam a fazer compras nas lojas parceiras.',
  },
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>',
    title: 'Você ganha cashback bônus',
    desc: 'A cada compra que seus indicados fazem, você recebe um percentual direto na sua carteira.',
  },
];

const rewards = [
  { tier: 'Nível 1', range: '1–5 amigos', value: '+0,5%', extra: 'Sobre cada compra dos indicados', highlight: false },
  { tier: 'Nível 2', range: '6–20 amigos', value: '+1%', extra: 'Sobre cada compra dos indicados', highlight: true },
  { tier: 'Nível 3', range: '21–50 amigos', value: '+1,5%', extra: '+ R$ 10 bônus por novo indicado', highlight: false },
  { tier: 'Nível 4', range: '51+ amigos', value: '+2%', extra: '+ R$ 20 bônus + bônus de equipe', highlight: false },
];
</script>

<style scoped>
.qa-page { font-family: 'Inter', 'Jost', sans-serif; }

.qa-hero {
  background: linear-gradient(135deg, #0f232d 0%, #225F6B 60%, #2F7785 100%);
  padding: 72px 0;
  position: relative;
  overflow: hidden;
}
.qa-hero::after {
  content: '';
  position: absolute;
  inset: 0;
  background: url('https://images.unsplash.com/photo-1529156069898-49953e39b3ac?w=1400&q=70&auto=format&fit=crop') center/cover;
  opacity: 0.1;
}
.qa-hero__inner { position: relative; z-index: 1; display: flex; align-items: center; gap: 60px; }
.qa-hero__left { flex: 1; }
.qa-hero__right { flex: 0 0 320px; }

.qa-badge { display: inline-flex; align-items: center; gap: 6px; background: rgba(152,199,58,0.15); color: #98C73A; border: 1px solid rgba(152,199,58,0.3); border-radius: 999px; padding: 5px 14px; font-size: 13px; font-weight: 500; margin-bottom: 20px; }
.qa-hero__left h1 { font-size: clamp(26px,3.2vw,42px); font-weight: 800; color: #fff; line-height: 1.2; margin-bottom: 16px; }
.qa-accent { color: #98C73A; }
.qa-hero__left p { font-size: 16px; color: rgba(255,255,255,0.75); line-height: 1.6; margin-bottom: 28px; max-width: 460px; }
.qa-hero__btns { display: flex; gap: 12px; flex-wrap: wrap; }

.qa-btn-primary { display: inline-flex; align-items: center; background: #98C73A; color: #fff; font-family: 'Inter','Jost',sans-serif; font-size: 15px; font-weight: 600; padding: 12px 24px; border-radius: 8px; text-decoration: none; transition: background 0.2s; border: none; cursor: pointer; }
.qa-btn-primary:hover { background: #7aad1f; color: #fff; }
.qa-btn-outline { display: inline-flex; align-items: center; background: transparent; color: #fff; font-family: 'Inter','Jost',sans-serif; font-size: 15px; font-weight: 500; padding: 12px 24px; border-radius: 8px; border: 1.5px solid rgba(255,255,255,0.4); text-decoration: none; transition: all 0.2s; }
.qa-btn-outline:hover { border-color: #fff; background: rgba(255,255,255,0.08); }

.qa-stat-cards { display: flex; flex-direction: column; gap: 14px; }
.qa-stat-card { display: flex; align-items: center; gap: 14px; background: rgba(255,255,255,0.1); backdrop-filter: blur(12px); border: 1px solid rgba(255,255,255,0.2); border-radius: 14px; padding: 18px 20px; }
.qa-stat-card strong { display: block; font-size: 22px; font-weight: 700; color: #fff; }
.qa-stat-card span { font-size: 12px; color: rgba(255,255,255,0.6); line-height: 1.4; }

.qa-section { padding: 80px 0; background: #fff; }
.qa-section--gray { background: #f7f8fa; }
.qa-section__header { text-align: center; margin-bottom: 52px; }
.qa-label { display: inline-block; font-size: 12px; font-weight: 600; letter-spacing: 0.08em; text-transform: uppercase; color: #2F7785; background: rgba(47,119,133,0.08); padding: 4px 12px; border-radius: 999px; margin-bottom: 12px; }
.qa-section__header h2 { font-size: clamp(22px,2.5vw,34px); font-weight: 700; color: #111827; margin-bottom: 12px; }
.qa-section__header p { font-size: 16px; color: #6b7280; max-width: 520px; margin: 0 auto; }

.qa-steps { display: grid; grid-template-columns: repeat(3, 1fr); gap: 32px; }
@media (max-width: 768px) { .qa-steps { grid-template-columns: 1fr; } }
.qa-step { background: #fff; border: 1.5px solid #f0f0f0; border-radius: 16px; padding: 32px 24px; text-align: center; position: relative; box-shadow: 0 2px 12px rgba(0,0,0,0.04); }
.qa-step__num { position: absolute; top: -14px; left: 50%; transform: translateX(-50%); width: 28px; height: 28px; border-radius: 50%; background: #2F7785; color: #fff; font-size: 13px; font-weight: 700; display: flex; align-items: center; justify-content: center; }
.qa-step__icon { display: flex; justify-content: center; margin: 12px 0; }
.qa-step h3 { font-size: 16px; font-weight: 600; color: #111827; margin-bottom: 8px; }
.qa-step p { font-size: 14px; color: #6b7280; line-height: 1.6; }

.qa-rewards { display: grid; grid-template-columns: repeat(4, 1fr); gap: 20px; }
@media (max-width: 900px) { .qa-rewards { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 560px) { .qa-rewards { grid-template-columns: 1fr; } }
.qa-reward-card { background: #fff; border: 1.5px solid #e5e7eb; border-radius: 16px; padding: 28px 20px; text-align: center; box-shadow: 0 2px 10px rgba(0,0,0,0.04); }
.qa-reward-card--highlight { border-color: #2F7785; box-shadow: 0 4px 20px rgba(47,119,133,0.15); }
.qa-reward-tier { font-size: 13px; font-weight: 600; color: #2F7785; text-transform: uppercase; letter-spacing: 0.06em; margin-bottom: 4px; }
.qa-reward-range { font-size: 12px; color: #9ca3af; margin-bottom: 16px; }
.qa-reward-value { font-size: 36px; font-weight: 800; color: #111827; }
.qa-reward-label { font-size: 12px; color: #9ca3af; margin-bottom: 12px; }
.qa-reward-extra { font-size: 12px; color: #6b7280; line-height: 1.5; }

/* Invite */
.qa-invite { display: flex; align-items: center; gap: 80px; flex-wrap: wrap; }
.qa-invite__text { flex: 1; }
.qa-invite__text h2 { font-size: clamp(22px,2.5vw,32px); font-weight: 700; color: #111827; margin: 12px 0 12px; }
.qa-invite__text p { font-size: 15px; color: #6b7280; line-height: 1.7; max-width: 420px; }
.qa-invite__link { flex: 0 0 340px; }
.qa-link-box { display: flex; align-items: center; background: #f7f8fa; border: 1.5px solid #e5e7eb; border-radius: 10px; padding: 12px 16px; gap: 10px; margin-bottom: 14px; }
.qa-link-url { flex: 1; font-size: 14px; color: #374151; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.qa-link-url strong { color: #2F7785; }
.qa-link-copy { display: flex; align-items: center; gap: 6px; background: #2F7785; color: #fff; border: none; border-radius: 7px; padding: 8px 14px; font-size: 13px; font-weight: 600; cursor: pointer; white-space: nowrap; transition: background 0.2s; }
.qa-link-copy:hover { background: #225F6B; }
.qa-share-btns { display: flex; gap: 10px; }
.qa-share-btn { display: inline-flex; align-items: center; gap: 8px; padding: 9px 16px; border-radius: 8px; font-size: 13px; font-weight: 600; text-decoration: none; transition: opacity 0.2s; }
.qa-share-btn--whatsapp { background: #25D366; color: #fff; }
.qa-share-btn:hover { opacity: 0.85; }

.qa-cta { background: linear-gradient(135deg, #225F6B 0%, #2F7785 100%); padding: 72px 0; }
.qa-cta__inner { text-align: center; }
.qa-cta__inner h2 { font-size: 28px; font-weight: 700; color: #fff; margin-bottom: 10px; }
.qa-cta__inner p { font-size: 15px; color: rgba(255,255,255,0.75); margin-bottom: 28px; }
</style>
