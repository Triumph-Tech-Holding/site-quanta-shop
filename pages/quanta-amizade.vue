<template>
  <div>

    <QsHero
      badge="Programa de Indicações"
      title="Indique amigos e "
      title-accent="ganhe juntos"
      subtitle="Compartilhe seu link, seus amigos se cadastram e compram — você ganha cashback bônus em cada compra deles. Quanto mais amigos, mais você ganha."
      :cta-primary="{ text: 'Começar a indicar →', href: '/register' }"
      :cta-secondary="{ text: 'Como funciona', href: '#como-funciona' }"
      photo-url="https://images.unsplash.com/photo-1529156069898-49953e39b3ac?w=1400&q=80&auto=format&fit=crop"
      photo-alt="Amigos usando a Quanta Shop juntos"
      :floating-badges="[
        { icon: friendsIcon, label: 'Amigos Indicados / mês', value: '+47.000' },
        { icon: cashIcon, label: 'Cashback Médio / indicação', value: 'R$ 128' },
      ]"
    />

    <!-- Como funciona -->
    <section class="qs-section" id="como-funciona">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Simples assim</span>
          <h2>Como funciona o Quanta Amizade</h2>
          <p>Três passos para começar a ganhar com suas indicações</p>
        </div>
        <div class="qs-steps">
          <div v-for="(step, i) in steps" :key="i" class="qs-step-card">
            <div class="qs-step-num">{{ i + 1 }}</div>
            <div class="qs-step-icon" v-html="step.icon"></div>
            <h3>{{ step.title }}</h3>
            <p>{{ step.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Tabela de recompensas -->
    <section class="qs-section qs-section--gray">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Tabela de recompensas</span>
          <h2>Quanto você ganha por amigo</h2>
          <p>Seus ganhos crescem conforme sua rede cresce</p>
        </div>
        <div class="qs-rewards-grid">
          <div v-for="r in rewards" :key="r.tier" class="qs-reward-card" :class="{ 'qs-reward-card--highlight': r.highlight }">
            <div class="qs-reward-tier">{{ r.tier }}</div>
            <div class="qs-reward-range">{{ r.range }}</div>
            <div class="qs-reward-value">{{ r.value }}</div>
            <div class="qs-reward-label">por compra do indicado</div>
            <div class="qs-reward-extra">{{ r.extra }}</div>
          </div>
        </div>
      </div>
    </section>

    <!-- Link de convite -->
    <section class="qs-section">
      <div class="container">
        <div class="qs-invite-wrap">
          <div class="qs-invite-text">
            <div class="qs-section-symbol" style="justify-content:flex-start;">
              <img src="/img/logo/logo-symbol.png" alt="" aria-hidden="true" />
            </div>
            <span class="qs-label">Compartilhe agora</span>
            <h2>Seu link de indicação</h2>
            <p>Copie e envie para seus amigos pelo WhatsApp, Instagram ou e-mail. Cada amigo que se cadastrar e fizer uma compra gera cashback para você.</p>
          </div>
          <div class="qs-invite-ui">
            <div class="qs-link-box">
              <span class="qs-link-url">quantashop.com.br/r/<strong>{{ userName }}</strong></span>
              <button class="qs-link-copy" @click="copyLink">
                <svg v-if="!copied" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="9" y="9" width="13" height="13" rx="2"/><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"/></svg>
                <svg v-else width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2.5"><polyline points="20 6 9 17 4 12"/></svg>
                {{ copied ? 'Copiado!' : 'Copiar' }}
              </button>
            </div>
            <div class="qs-share-btns">
              <a href="https://wa.me/?text=Ei%2C%20conhe%C3%A7a%20a%20Quanta%20Shop%20e%20ganhe%20cashback!" target="_blank" class="qs-share-btn qs-share-btn--whatsapp">
                <svg width="15" height="15" viewBox="0 0 24 24" fill="currentColor"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 0 1-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 0 1-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 0 1 2.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0 0 12.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 0 0 5.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 0 0-3.48-8.413z"/></svg>
                WhatsApp
              </a>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="qs-cta-section">
      <div class="container qs-cta-inner qs-cta-inner--center">
        <h2>Ainda não tem conta? Crie agora</h2>
        <p>Cadastre-se gratuitamente e receba seu link de indicação em segundos.</p>
        <div class="qs-cta-btns">
          <NuxtLink href="/register" class="qs-page-btn-lime">Criar conta grátis →</NuxtLink>
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ title: 'Quanta Amizade | Quanta Shop', description: 'Indique amigos para a Quanta Shop e ganhe cashback bônus em cada compra deles.', canonical: 'https://quantashop.com.br/quanta-amizade' });

const copied = ref(false);
const userName = ref('seu-usuario');

function copyLink() {
  navigator.clipboard.writeText(`https://quantashop.com.br/r/${userName.value}`).catch(() => {});
  copied.value = true;
  setTimeout(() => { copied.value = false; }, 2500);
}

const friendsIcon = `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>`;
const cashIcon = `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>`;

const steps = [
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M4 12v8a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-8"/><polyline points="16 6 12 2 8 6"/><line x1="12" y1="2" x2="12" y2="15"/></svg>', title: 'Copie seu link único', desc: 'Após se cadastrar, você recebe um link pessoal de indicação para compartilhar.' },
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>', title: 'Seus amigos se cadastram', desc: 'Eles criam a conta usando seu link e passam a fazer compras nas lojas parceiras.' },
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>', title: 'Você ganha cashback bônus', desc: 'A cada compra que seus indicados fazem, você recebe um percentual na sua carteira.' },
];

const rewards = [
  { tier: 'Nível 1', range: '1–5 amigos', value: '+0,5%', extra: 'Sobre cada compra dos indicados', highlight: false },
  { tier: 'Nível 2', range: '6–20 amigos', value: '+1%', extra: 'Sobre cada compra dos indicados', highlight: true },
  { tier: 'Nível 3', range: '21–50 amigos', value: '+1,5%', extra: '+ R$ 10 bônus por novo indicado', highlight: false },
  { tier: 'Nível 4', range: '51+ amigos', value: '+2%', extra: '+ R$ 20 bônus + bônus de equipe', highlight: false },
];
</script>
