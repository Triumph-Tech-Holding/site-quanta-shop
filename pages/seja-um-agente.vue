<template>
  <div class="sua-page">

    <!-- Hero -->
    <section class="sua-hero">
      <div class="container sua-hero__content">
        <div class="sua-hero__left">
          <span class="sua-badge">
            <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><polyline points="23 6 13.5 15.5 8.5 10.5 1 18"/><polyline points="17 6 23 6 23 12"/></svg>
            Oportunidade de renda extra
          </span>
          <h1>Seja um Agente <span class="sua-accent">Quanta Shop</span> e ganhe indicando</h1>
          <p>Indique lojas e consumidores, acompanhe seus ganhos em tempo real e construa uma renda recorrente sem sair de casa. 100% gratuito.</p>
          <div class="sua-hero__btns">
            <nuxt-link href="/register" class="sua-btn-primary">Quero ser agente agora →</nuxt-link>
            <a href="#como-funciona" class="sua-btn-outline">Ver como funciona</a>
          </div>
        </div>
        <div class="sua-hero__right">
          <div class="sua-earnings-card">
            <div class="sua-earnings-title">Exemplo de ganhos mensais</div>
            <div class="sua-earnings-rows">
              <div class="sua-earning-row" v-for="e in earningsSim" :key="e.label">
                <span>{{ e.label }}</span>
                <div class="sua-earning-bar-wrap">
                  <div class="sua-earning-bar" :style="{ width: e.pct + '%' }"></div>
                </div>
                <strong>{{ e.value }}</strong>
              </div>
            </div>
            <div class="sua-earnings-total">
              <span>Total estimado</span>
              <strong class="sua-earnings-total-val">R$ 2.340/mês</strong>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Como funciona -->
    <section class="sua-section" id="como-funciona">
      <div class="container">
        <div class="sua-section__header">
          <span class="sua-label">4 passos simples</span>
          <h2>Como funciona ser agente</h2>
          <p>Em menos de 10 minutos você já pode começar a ganhar</p>
        </div>
        <div class="sua-steps">
          <div v-for="(step, i) in steps" :key="i" class="sua-step">
            <div class="sua-step__num">{{ i + 1 }}</div>
            <div class="sua-step__icon" v-html="step.icon"></div>
            <h3>{{ step.title }}</h3>
            <p>{{ step.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Níveis de ganho -->
    <section class="sua-section sua-section--dark">
      <div class="container">
        <div class="sua-section__header sua-section__header--white">
          <span class="sua-label sua-label--lime">Programa de comissões</span>
          <h2>Quanto você pode ganhar</h2>
          <p>Suas comissões crescem conforme sua rede cresce</p>
        </div>
        <div class="sua-levels">
          <div v-for="lv in levels" :key="lv.name" class="sua-level" :class="{ 'sua-level--highlight': lv.highlight }">
            <div class="sua-level__name">{{ lv.name }}</div>
            <div class="sua-level__pct">{{ lv.pct }}</div>
            <div class="sua-level__label">comissão</div>
            <div class="sua-level__desc">{{ lv.desc }}</div>
            <div class="sua-level__req">{{ lv.req }}</div>
          </div>
        </div>
      </div>
    </section>

    <!-- FAQ -->
    <section class="sua-section">
      <div class="container">
        <div class="sua-section__header">
          <span class="sua-label">Perguntas frequentes</span>
          <h2>Ficou com dúvida?</h2>
        </div>
        <div class="sua-faq">
          <div v-for="(faq, i) in faqs" :key="i" class="sua-faq-item" @click="openFaq = openFaq === i ? -1 : i">
            <div class="sua-faq-q">
              <span>{{ faq.q }}</span>
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" :style="{ transform: openFaq === i ? 'rotate(180deg)' : 'none', transition: 'transform .2s' }"><polyline points="6 9 12 15 18 9"/></svg>
            </div>
            <div v-if="openFaq === i" class="sua-faq-a">{{ faq.a }}</div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="sua-cta">
      <div class="container sua-cta__inner">
        <h2>Comece hoje mesmo, é gratuito</h2>
        <p>Crie sua conta em 2 minutos e já comece a indicar e ganhar.</p>
        <nuxt-link href="/register" class="sua-btn-primary">Criar minha conta agora →</nuxt-link>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ title: "Seja um Agente | Quanta Shop", description: "Indique lojas e consumidores e ganhe comissões recorrentes. 100% gratuito." });

const openFaq = ref(-1);

const earningsSim = [
  { label: '10 lojas indicadas', value: 'R$ 480', pct: 45 },
  { label: '30 clientes ativos', value: 'R$ 960', pct: 70 },
  { label: 'Bônus de equipe', value: 'R$ 900', pct: 90 },
];

const steps = [
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><line x1="19" y1="8" x2="19" y2="14"/><line x1="22" y1="11" x2="16" y2="11"/></svg>',
    title: 'Crie sua conta grátis',
    desc: 'Cadastre-se em 2 minutos. Sem mensalidade, sem taxa de entrada.',
  },
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M4 12v8a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-8"/><polyline points="16 6 12 2 8 6"/><line x1="12" y1="2" x2="12" y2="15"/></svg>',
    title: 'Compartilhe seu link',
    desc: 'Indique lojas para se credenciar e consumidores para se cadastrar usando seu link único.',
  },
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><line x1="18" y1="20" x2="18" y2="10"/><line x1="12" y1="20" x2="12" y2="4"/><line x1="6" y1="20" x2="6" y2="14"/></svg>',
    title: 'Acompanhe seus ganhos',
    desc: 'Veja em tempo real cada comissão gerada pelas suas indicações diretas e indiretas.',
  },
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><rect x="1" y="4" width="22" height="16" rx="2"/><line x1="1" y1="10" x2="23" y2="10"/></svg>',
    title: 'Saque quando quiser',
    desc: 'Solicite saque direto para sua conta bancária a partir de R$ 50,00 acumulados.',
  },
];

const levels = [
  { name: 'Agente Inicial', pct: '2%', desc: 'Sobre cada venda de lojas que você indicou', req: 'Do 1º dia', highlight: false },
  { name: 'Agente Prata', pct: '3%', desc: 'Mais bônus de rede de 1% sobre indicados diretos', req: 'A partir de 5 lojas ativas', highlight: true },
  { name: 'Agente Ouro', pct: '4%', desc: 'Mais bônus de rede de 2% + comissão de equipe', req: 'A partir de 15 lojas ativas', highlight: false },
];

const faqs = [
  { q: 'Preciso pagar alguma taxa para ser agente?', a: 'Não. O cadastro como agente Quanta Shop é 100% gratuito. Você só precisa criar uma conta e já pode começar a indicar e ganhar comissões.' },
  { q: 'Como recebo minhas comissões?', a: 'As comissões são creditadas automaticamente na sua carteira digital assim que uma venda é realizada na rede de lojas que você indicou. Você pode solicitar saque para sua conta bancária a qualquer momento, a partir de R$ 50,00.' },
  { q: 'Preciso vender algo ou ter estoque?', a: 'Não. Você não precisa vender nada nem ter estoque. Seu trabalho é indicar lojas para se credenciarem na plataforma e consumidores para se cadastrarem. As vendas acontecem naturalmente nas lojas parceiras.' },
];
</script>

<style scoped>
.sua-page { font-family: 'Inter', 'Jost', sans-serif; }

.sua-hero {
  background: linear-gradient(135deg, #0f232d 0%, #225F6B 60%, #2F7785 100%);
  min-height: 500px;
  display: flex;
  align-items: center;
  position: relative;
  overflow: hidden;
}
.sua-hero::after {
  content: '';
  position: absolute;
  inset: 0;
  background: url('https://images.unsplash.com/photo-1521737604893-d14cc237f11d?w=1400&q=70&auto=format&fit=crop') center/cover;
  opacity: 0.1;
}
.sua-hero__content {
  position: relative;
  z-index: 1;
  display: flex;
  align-items: center;
  gap: 60px;
  padding: 72px 0;
}
.sua-hero__left { flex: 1; }
.sua-hero__right { flex: 0 0 340px; }

.sua-badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: rgba(152,199,58,0.15);
  color: #98C73A;
  border: 1px solid rgba(152,199,58,0.3);
  border-radius: 999px;
  padding: 5px 14px;
  font-size: 13px;
  font-weight: 500;
  margin-bottom: 20px;
}
.sua-hero__left h1 { font-size: clamp(26px, 3.2vw, 42px); font-weight: 800; color: #fff; line-height: 1.2; margin-bottom: 16px; }
.sua-accent { color: #98C73A; }
.sua-hero__left p { font-size: 16px; color: rgba(255,255,255,0.75); line-height: 1.6; margin-bottom: 28px; max-width: 480px; }
.sua-hero__btns { display: flex; gap: 12px; flex-wrap: wrap; }

.sua-btn-primary {
  display: inline-flex;
  align-items: center;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter','Jost',sans-serif;
  font-size: 15px;
  font-weight: 600;
  padding: 12px 24px;
  border-radius: 8px;
  text-decoration: none;
  transition: background 0.2s;
  border: none;
  cursor: pointer;
}
.sua-btn-primary:hover { background: #7aad1f; color: #fff; }

.sua-btn-outline {
  display: inline-flex;
  align-items: center;
  background: transparent;
  color: #fff;
  font-family: 'Inter','Jost',sans-serif;
  font-size: 15px;
  font-weight: 500;
  padding: 12px 24px;
  border-radius: 8px;
  border: 1.5px solid rgba(255,255,255,0.4);
  text-decoration: none;
  transition: all 0.2s;
}
.sua-btn-outline:hover { border-color: #fff; background: rgba(255,255,255,0.08); }

/* Earnings card */
.sua-earnings-card {
  background: rgba(255,255,255,0.1);
  backdrop-filter: blur(16px);
  border: 1px solid rgba(255,255,255,0.2);
  border-radius: 16px;
  padding: 24px 22px;
}
.sua-earnings-title { font-size: 13px; color: rgba(255,255,255,0.6); margin-bottom: 16px; }
.sua-earning-row { display: flex; align-items: center; gap: 10px; margin-bottom: 12px; }
.sua-earning-row span { font-size: 12px; color: rgba(255,255,255,0.7); min-width: 130px; }
.sua-earning-bar-wrap { flex: 1; height: 6px; background: rgba(255,255,255,0.15); border-radius: 999px; overflow: hidden; }
.sua-earning-bar { height: 100%; background: #98C73A; border-radius: 999px; }
.sua-earning-row strong { font-size: 13px; color: #fff; min-width: 72px; text-align: right; }
.sua-earnings-total { display: flex; justify-content: space-between; align-items: center; margin-top: 16px; padding-top: 14px; border-top: 1px solid rgba(255,255,255,0.15); }
.sua-earnings-total span { font-size: 13px; color: rgba(255,255,255,0.6); }
.sua-earnings-total-val { font-size: 20px; font-weight: 700; color: #98C73A; }

.sua-section { padding: 80px 0; background: #fff; }
.sua-section--dark { background: #111827; }
.sua-section__header { text-align: center; margin-bottom: 52px; }
.sua-section__header--white h2, .sua-section__header--white p { color: #fff; }
.sua-section__header--white p { color: rgba(255,255,255,0.6); }

.sua-label {
  display: inline-block;
  font-size: 12px;
  font-weight: 600;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: #2F7785;
  background: rgba(47,119,133,0.08);
  padding: 4px 12px;
  border-radius: 999px;
  margin-bottom: 12px;
}
.sua-label--lime { color: #98C73A; background: rgba(152,199,58,0.12); }
.sua-section__header h2 { font-size: clamp(22px,2.5vw,34px); font-weight: 700; color: #111827; margin-bottom: 12px; }
.sua-section__header p { font-size: 16px; color: #6b7280; max-width: 520px; margin: 0 auto; }

/* Steps */
.sua-steps { display: grid; grid-template-columns: repeat(4, 1fr); gap: 24px; }
@media (max-width: 900px) { .sua-steps { grid-template-columns: repeat(2,1fr); } }
@media (max-width: 560px) { .sua-steps { grid-template-columns: 1fr; } }
.sua-step { background: #fff; border: 1.5px solid #f0f0f0; border-radius: 16px; padding: 28px 20px; text-align: center; position: relative; box-shadow: 0 2px 12px rgba(0,0,0,0.04); }
.sua-step__num { position: absolute; top: -14px; left: 50%; transform: translateX(-50%); width: 28px; height: 28px; border-radius: 50%; background: #2F7785; color: #fff; font-size: 13px; font-weight: 700; display: flex; align-items: center; justify-content: center; }
.sua-step__icon { display: flex; justify-content: center; margin: 12px 0 12px; }
.sua-step h3 { font-size: 15px; font-weight: 600; color: #111827; margin-bottom: 6px; }
.sua-step p { font-size: 13px; color: #6b7280; line-height: 1.6; }

/* Levels */
.sua-levels { display: grid; grid-template-columns: repeat(3,1fr); gap: 20px; }
@media (max-width: 768px) { .sua-levels { grid-template-columns: 1fr; } }
.sua-level { background: rgba(255,255,255,0.06); border: 1.5px solid rgba(255,255,255,0.1); border-radius: 16px; padding: 32px 24px; text-align: center; }
.sua-level--highlight { background: rgba(47,119,133,0.2); border-color: #2F7785; }
.sua-level__name { font-size: 15px; font-weight: 600; color: rgba(255,255,255,0.8); margin-bottom: 12px; }
.sua-level__pct { font-size: 52px; font-weight: 800; color: #fff; line-height: 1; }
.sua-level__label { font-size: 13px; color: rgba(255,255,255,0.5); margin-bottom: 12px; }
.sua-level__desc { font-size: 13px; color: rgba(255,255,255,0.65); line-height: 1.5; margin-bottom: 10px; }
.sua-level__req { font-size: 12px; color: #98C73A; font-weight: 600; }

/* FAQ */
.sua-faq { max-width: 680px; margin: 0 auto; }
.sua-faq-item { border: 1.5px solid #f0f0f0; border-radius: 10px; margin-bottom: 10px; overflow: hidden; cursor: pointer; }
.sua-faq-q { display: flex; justify-content: space-between; align-items: center; padding: 18px 20px; font-size: 15px; font-weight: 500; color: #111827; }
.sua-faq-a { padding: 0 20px 18px; font-size: 14px; color: #6b7280; line-height: 1.7; }

/* CTA */
.sua-cta { background: linear-gradient(135deg, #225F6B 0%, #2F7785 100%); padding: 72px 0; }
.sua-cta__inner { text-align: center; }
.sua-cta__inner h2 { font-size: 30px; font-weight: 700; color: #fff; margin-bottom: 10px; }
.sua-cta__inner p { font-size: 16px; color: rgba(255,255,255,0.75); margin-bottom: 28px; }
</style>
