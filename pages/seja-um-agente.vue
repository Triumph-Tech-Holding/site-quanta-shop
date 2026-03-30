<template>
  <div>

    <QsHero
      badge="Oportunidade de renda extra"
      title="Seja um Agente e ganhe "
      title-accent="indicando"
      subtitle="Indique lojas e consumidores, acompanhe seus ganhos em tempo real e construa uma renda recorrente sem sair de casa. 100% gratuito."
      :cta-primary="{ text: 'Quero ser agente agora →', href: '/register' }"
      :cta-secondary="{ text: 'Como funciona', href: '#como-funciona' }"
      photo-url="https://images.unsplash.com/photo-1600880292203-757bb62b4baf?w=1400&q=80&auto=format&fit=crop"
      photo-alt="Agente Quanta Shop conquistando renda extra"
      :floating-badges="[
        { icon: earningsIcon, label: 'Ganhos Estimados / mês', value: 'R$ 2.340' },
        { icon: networkIcon, label: 'Rede de Lojas', value: '+2.400' },
      ]"
    />

    <!-- Como funciona -->
    <section class="qs-section" id="como-funciona">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">4 passos simples</span>
          <h2>Como funciona ser agente</h2>
          <p>Em menos de 10 minutos você já pode começar a ganhar</p>
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

    <!-- Níveis de ganho -->
    <section class="qs-section qs-section--dark">
      <div class="container">
        <div class="qs-section-header qs-section-header--white">
          <div class="qs-section-symbol">
            <img src="/img/logo/logo-symbol-white.png" alt="" aria-hidden="true" />
          </div>
          <span class="qs-label qs-label--lime">Programa de comissões</span>
          <h2>Quanto você pode ganhar</h2>
          <p>Suas comissões crescem conforme sua rede cresce</p>
        </div>
        <div class="qs-levels-grid">
          <div v-for="lv in levels" :key="lv.name" class="qs-level-card" :class="{ 'qs-level-card--highlight': lv.highlight }">
            <div class="qs-level-card__name">{{ lv.name }}</div>
            <div class="qs-level-card__pct">{{ lv.pct }}</div>
            <div class="qs-level-card__sub">comissão</div>
            <div class="qs-level-card__desc">{{ lv.desc }}</div>
            <div class="qs-level-card__req">{{ lv.req }}</div>
          </div>
        </div>
      </div>
    </section>

    <!-- Simulador de ganhos -->
    <section class="qs-section qs-section--gray">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Exemplo real</span>
          <h2>Veja um exemplo de ganhos mensais</h2>
          <p>Com apenas 10 lojas e 30 clientes ativos, um agente pode ganhar:</p>
        </div>
        <div class="agent-sim">
          <div class="agent-sim__rows">
            <div v-for="e in earningsSim" :key="e.label" class="agent-sim__row">
              <span class="agent-sim__label">{{ e.label }}</span>
              <div class="agent-sim__bar-wrap">
                <div class="agent-sim__bar" :style="{ width: e.pct + '%' }"></div>
              </div>
              <strong class="agent-sim__value">{{ e.value }}</strong>
            </div>
          </div>
          <div class="agent-sim__total">
            <span>Total estimado por mês</span>
            <strong>R$ 2.340</strong>
          </div>
        </div>
      </div>
    </section>

    <!-- FAQ -->
    <section class="qs-section">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Perguntas frequentes</span>
          <h2>Ficou com dúvida?</h2>
        </div>
        <div class="qs-faq">
          <div v-for="(faq, i) in faqs" :key="i" class="qs-faq-item" @click="openFaq = openFaq === i ? -1 : i">
            <div class="qs-faq-q">
              <span>{{ faq.q }}</span>
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" :style="{ transform: openFaq === i ? 'rotate(180deg)' : 'none', transition: 'transform .2s' }"><polyline points="6 9 12 15 18 9"/></svg>
            </div>
            <div v-if="openFaq === i" class="qs-faq-a">{{ faq.a }}</div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="qs-cta-section">
      <div class="container qs-cta-inner qs-cta-inner--center">
        <h2>Comece hoje mesmo, é gratuito</h2>
        <p>Crie sua conta em 2 minutos e já comece a indicar e ganhar.</p>
        <div class="qs-cta-btns">
          <NuxtLink href="/register" class="qs-page-btn-lime">Criar minha conta agora →</NuxtLink>
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ title: 'Seja um Agente | Quanta Shop', description: 'Indique lojas e consumidores e ganhe comissões recorrentes. 100% gratuito.', canonical: 'https://quantashop.com.br/seja-um-agente' });

const openFaq = ref(-1);

const earningsIcon = `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><polyline points="23 6 13.5 15.5 8.5 10.5 1 18"/><polyline points="17 6 23 6 23 12"/></svg>`;
const networkIcon = `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/></svg>`;

const earningsSim = [
  { label: '10 lojas indicadas', value: 'R$ 480', pct: 45 },
  { label: '30 clientes ativos', value: 'R$ 960', pct: 70 },
  { label: 'Bônus de equipe', value: 'R$ 900', pct: 90 },
];

const steps = [
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><line x1="19" y1="8" x2="19" y2="14"/><line x1="22" y1="11" x2="16" y2="11"/></svg>', title: 'Crie sua conta grátis', desc: 'Cadastre-se em 2 minutos. Sem mensalidade, sem taxa de entrada.' },
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M4 12v8a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-8"/><polyline points="16 6 12 2 8 6"/><line x1="12" y1="2" x2="12" y2="15"/></svg>', title: 'Compartilhe seu link', desc: 'Indique lojas para se credenciar e consumidores para se cadastrar.' },
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><line x1="18" y1="20" x2="18" y2="10"/><line x1="12" y1="20" x2="12" y2="4"/><line x1="6" y1="20" x2="6" y2="14"/></svg>', title: 'Acompanhe seus ganhos', desc: 'Veja em tempo real cada comissão gerada pelas suas indicações.' },
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><rect x="1" y="4" width="22" height="16" rx="2"/><line x1="1" y1="10" x2="23" y2="10"/></svg>', title: 'Saque quando quiser', desc: 'Solicite saque para sua conta bancária a partir de R$ 50,00.' },
];

const levels = [
  { name: 'Agente Inicial', pct: '2%', desc: 'Sobre cada venda de lojas que você indicou', req: 'Do 1º dia', highlight: false },
  { name: 'Agente Prata', pct: '3%', desc: 'Mais bônus de rede de 1% sobre indicados diretos', req: 'A partir de 5 lojas ativas', highlight: true },
  { name: 'Agente Ouro', pct: '4%', desc: 'Mais bônus de rede de 2% + comissão de equipe', req: 'A partir de 15 lojas ativas', highlight: false },
];

const faqs = [
  { q: 'Preciso pagar alguma taxa para ser agente?', a: 'Não. O cadastro como agente Quanta Shop é 100% gratuito. Você só precisa criar uma conta e já pode começar a indicar e ganhar comissões.' },
  { q: 'Como recebo minhas comissões?', a: 'As comissões são creditadas automaticamente na sua carteira digital assim que uma venda é realizada na rede de lojas que você indicou. Você pode solicitar saque para sua conta bancária a qualquer momento, a partir de R$ 50,00.' },
  { q: 'Preciso vender algo ou ter estoque?', a: 'Não. Você não precisa vender nada nem ter estoque. Seu trabalho é indicar lojas para se credenciarem na plataforma e consumidores para se cadastrarem.' },
];
</script>

<style scoped>
.agent-sim {
  max-width: 640px;
  margin: 0 auto;
  background: #fff;
  border: 1.5px solid #eef0f2;
  border-radius: 20px;
  padding: 36px 32px;
  box-shadow: 0 4px 24px rgba(0,0,0,0.06);
}
.agent-sim__rows { display: flex; flex-direction: column; gap: 18px; margin-bottom: 28px; }
.agent-sim__row { display: flex; align-items: center; gap: 14px; }
.agent-sim__label { font-size: 13px; color: #6b7280; min-width: 160px; }
.agent-sim__bar-wrap { flex: 1; height: 8px; background: #f0f0f0; border-radius: 999px; overflow: hidden; }
.agent-sim__bar { height: 100%; background: linear-gradient(90deg, #2F7785, #98C73A); border-radius: 999px; transition: width 1s ease; }
.agent-sim__value { font-size: 14px; font-weight: 700; color: #0f1c23; min-width: 80px; text-align: right; }
.agent-sim__total { display: flex; justify-content: space-between; align-items: center; padding-top: 20px; border-top: 1.5px solid #eef0f2; }
.agent-sim__total span { font-size: 14px; color: #6b7280; }
.agent-sim__total strong { font-size: 26px; font-weight: 800; color: #98C73A; }
@media (max-width: 560px) {
  .agent-sim { padding: 24px 20px; }
  .agent-sim__label { min-width: 110px; font-size: 12px; }
}
</style>
