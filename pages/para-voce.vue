<template>
  <div>

    <QsHero
      badge="Para Consumidores"
      title="Compre nas lojas favoritas e "
      title-accent="ganhe dinheiro de volta"
      subtitle="Cadastre-se grátis, compre normalmente nas lojas parceiras e receba cashback real direto na sua conta. Sem pegadinhas, sem pontos — dinheiro de verdade."
      :cta-primary="{ text: 'Criar conta grátis →', href: '/register' }"
      :cta-secondary="{ text: 'Como funciona', href: '#como-funciona' }"
      photo-url="https://images.unsplash.com/photo-1567446537708-ac4aa75c9c28?w=1400&q=80&auto=format&fit=crop"
      photo-alt="Consumidores felizes usando a Quanta Shop"
      :floating-badges="[
        { icon: cashbackIcon, label: 'Cashback Acumulado', value: 'R$ 187,50' },
        { icon: storesIcon, label: 'Lojas Parceiras', value: '+2.400' },
      ]"
      :stats="[
        { value: '+80 mil', label: 'consumidores ativos' },
        { value: 'R$ 4,2M', label: 'em cashback pago' },
        { value: '+2.400', label: 'lojas parceiras' },
      ]"
    />

    <!-- Como funciona -->
    <section class="qs-section" id="como-funciona">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Simples assim</span>
          <h2>Como funciona para você</h2>
          <p>Três passos para começar a receber cashback nas suas compras hoje mesmo</p>
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

    <!-- Quanto você ganha -->
    <section class="qs-section qs-section--gray">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Exemplos reais</span>
          <h2>Quanto você pode ganhar</h2>
          <p>Veja quanto cashback você receberia em compras do dia a dia</p>
        </div>
        <div class="qs-example-grid">
          <div v-for="ex in examples" :key="ex.category" class="qs-example-card">
            <div class="qs-example-icon" v-html="ex.icon"></div>
            <div class="qs-example-category">{{ ex.category }}</div>
            <div class="qs-example-spend">R$ {{ ex.spend }} gastos</div>
            <div class="qs-example-arrow">→</div>
            <div>
              <span class="qs-example-amount">+ R$ {{ ex.back }}</span>
              <span class="qs-example-back-label"> de volta</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Lojas parceiras -->
    <section class="qs-section">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Onde usar</span>
          <h2>+2.400 lojas parceiras esperando por você</h2>
          <p>De supermercados a eletrônicos, farmácias, restaurantes e muito mais</p>
        </div>
        <div class="qs-categories-grid">
          <div v-for="cat in categories" :key="cat.name" class="qs-category-chip">
            <span class="qs-category-chip__icon" v-html="cat.icon"></span>
            <div class="qs-category-chip__info">
              <div class="qs-category-chip__name">{{ cat.name }}</div>
              <div class="qs-category-chip__count">{{ cat.count }} lojas</div>
            </div>
          </div>
        </div>
        <div class="qs-section-cta">
          <NuxtLink href="/register" class="qs-page-btn-primary">Ver todas as lojas →</NuxtLink>
        </div>
      </div>
    </section>

    <!-- Vantagens -->
    <section class="qs-section" style="background: linear-gradient(135deg, #1a4a54 0%, #225F6B 40%, #2F7785 100%);">
      <div class="container">
        <div class="qs-section-header qs-section-header--white">
          <div class="qs-section-symbol">
            <img src="/img/logo/logo-symbol-white.png" alt="" aria-hidden="true" />
          </div>
          <span class="qs-label qs-label--white">Por que a Quanta Shop</span>
          <h2>Vantagens para você</h2>
          <p>Tudo que você precisa para comprar melhor e ganhar de volta</p>
        </div>
        <div class="qs-benefits-grid">
          <div v-for="b in benefits" :key="b.title" class="qs-benefit-card">
            <div class="qs-benefit-icon" v-html="b.icon"></div>
            <h4>{{ b.title }}</h4>
            <p>{{ b.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Depoimentos -->
    <section class="qs-section qs-section--gray">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Quem já usa</span>
          <h2>O que nossos consumidores dizem</h2>
          <p>Mais de 80 mil pessoas já recebem cashback de verdade</p>
        </div>
        <div class="qs-testimonials">
          <div v-for="t in testimonials" :key="t.name" class="qs-testimonial">
            <div class="qs-testimonial__stars">★★★★★</div>
            <p class="qs-testimonial__text">"{{ t.text }}"</p>
            <div class="qs-testimonial__author">
              <div class="qs-testimonial__avatar">{{ t.name[0] }}</div>
              <div>
                <div class="qs-testimonial__name">{{ t.name }}</div>
                <div class="qs-testimonial__city">{{ t.city }}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA Final -->
    <section class="qs-cta-section">
      <div class="container qs-cta-inner">
        <div>
          <h2>Pronto para ganhar dinheiro comprando?</h2>
          <p>Cadastro gratuito em menos de 2 minutos. Comece a receber cashback hoje.</p>
        </div>
        <div class="qs-cta-btns">
          <NuxtLink href="/register" class="qs-page-btn-lime">Criar minha conta grátis</NuxtLink>
          <NuxtLink href="/contato" class="qs-page-btn-outline-white">Falar com a gente</NuxtLink>
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' });
useSeoMeta({
  title: 'Para Você | Quanta Shop — Cashback de Verdade',
  description: 'Compre nas suas lojas favoritas e ganhe cashback real. Mais de 2.400 lojas parceiras, cadastro grátis, dinheiro de volta na sua conta.',
  canonical: 'https://quantashop.com.br/para-voce'
});

const cashbackIcon = `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>`;
const storesIcon = `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>`;

const steps = [
  {
    title: 'Cadastre-se grátis',
    desc: 'Crie sua conta em menos de 2 minutos. Sem mensalidade, sem taxa de adesão.',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>',
  },
  {
    title: 'Compre nas lojas parceiras',
    desc: 'Mais de 2.400 lojas físicas e online participantes. Compre como sempre faz.',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>',
  },
  {
    title: 'Receba cashback de volta',
    desc: 'O dinheiro cai automaticamente no seu saldo. Saque quando quiser, sem complicação.',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>',
  },
];

const examples = [
  { category: 'Supermercado', spend: '500', back: '25,00', icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>' },
  { category: 'Farmácia', spend: '200', back: '14,00', icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>' },
  { category: 'Eletrônicos', spend: '1.200', back: '72,00', icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><rect x="2" y="3" width="20" height="14" rx="2"/><line x1="8" y1="21" x2="16" y2="21"/><line x1="12" y1="17" x2="12" y2="21"/></svg>' },
  { category: 'Restaurante', spend: '150', back: '9,00', icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M18 8h1a4 4 0 0 1 0 8h-1"/><path d="M2 8h16v9a4 4 0 0 1-4 4H6a4 4 0 0 1-4-4V8z"/><line x1="6" y1="1" x2="6" y2="4"/><line x1="10" y1="1" x2="10" y2="4"/><line x1="14" y1="1" x2="14" y2="4"/></svg>' },
  { category: 'Moda', spend: '350', back: '21,00', icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M20.38 3.46L16 2a4 4 0 0 1-8 0L3.62 3.46a2 2 0 0 0-1.34 2.23l.58 3.57a1 1 0 0 0 .99.84H6v10a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V10h2.15a1 1 0 0 0 .99-.84l.58-3.57a2 2 0 0 0-1.34-2.23z"/></svg>' },
  { category: 'Combustível', spend: '300', back: '15,00', icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M3 22V9a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v13"/><path d="M3 22h18"/><rect x="5" y="11" width="6" height="4" rx="1"/></svg>' },
];

const categories = [
  { name: 'Supermercados', count: '380+', icon: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>' },
  { name: 'Farmácias', count: '210+', icon: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>' },
  { name: 'Restaurantes', count: '520+', icon: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M18 8h1a4 4 0 0 1 0 8h-1"/><path d="M2 8h16v9a4 4 0 0 1-4 4H6a4 4 0 0 1-4-4V8z"/></svg>' },
  { name: 'Eletrônicos', count: '180+', icon: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><rect x="2" y="3" width="20" height="14" rx="2"/><line x1="8" y1="21" x2="16" y2="21"/><line x1="12" y1="17" x2="12" y2="21"/></svg>' },
  { name: 'Moda', count: '290+', icon: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M20.38 3.46L16 2a4 4 0 0 1-8 0L3.62 3.46a2 2 0 0 0-1.34 2.23l.58 3.57a1 1 0 0 0 .99.84H6v10a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V10h2.15a1 1 0 0 0 .99-.84l.58-3.57a2 2 0 0 0-1.34-2.23z"/></svg>' },
  { name: 'Postos', count: '340+', icon: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M3 22V9a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v13"/><path d="M3 22h18"/><rect x="5" y="11" width="6" height="4" rx="1"/></svg>' },
  { name: 'Beleza & Saúde', count: '160+', icon: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/></svg>' },
  { name: 'Viagens', count: '120+', icon: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07A19.5 19.5 0 0 1 4.72 13a19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 3.63 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91A16 16 0 0 0 15 15.91l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"/></svg>' },
];

const benefits = [
  { title: 'Cadastro 100% gratuito', desc: 'Não existe mensalidade, taxa de adesão ou qualquer cobrança. Cadastre-se e comece.', icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2.5"><polyline points="20 6 9 17 4 12"/></svg>' },
  { title: 'Dinheiro de verdade', desc: 'Sem pontos que expiram. Seu cashback é em reais, disponível para saque quando quiser.', icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>' },
  { title: 'Saque fácil e rápido', desc: 'Transfira o saldo para sua conta bancária diretamente pelo app, sem burocracia.', icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><polyline points="23 6 13.5 15.5 8.5 10.5 1 18"/><polyline points="17 6 23 6 23 12"/></svg>' },
  { title: 'Compre como sempre', desc: 'Não precisa mudar nada na sua rotina. Só mostre seu código ou acesse pela plataforma.', icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/></svg>' },
  { title: '+2.400 lojas disponíveis', desc: 'Supermercados, farmácias, restaurantes, combustível, moda, eletrônicos e muito mais.', icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>' },
  { title: 'Indique e ganhe mais', desc: 'Cada amigo que você indicar que comprar também te gera cashback bônus automático.', icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>' },
];

const testimonials = [
  { text: 'Uso o Quanta Shop há 8 meses e já recebi mais de R$ 340 de cashback. Compro em supermercado e farmácia todo mês e o dinheiro vai chegando automaticamente.', name: 'Fernanda Oliveira', city: 'São Paulo, SP' },
  { text: 'O melhor é que o dinheiro é de verdade! Já resgatei duas vezes e caiu direto na minha conta. Indiquei toda a família.', name: 'Carlos Mendes', city: 'Belo Horizonte, MG' },
  { text: 'Achei que era golpe, mas me cadastrei e é real. Já ganhei R$ 180 só comprando onde já comprava antes. Recomendo muito.', name: 'Ana Lima', city: 'Rio de Janeiro, RJ' },
];
</script>
