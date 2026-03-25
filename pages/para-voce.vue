<template>
  <div class="pv-page">

    <!-- Hero -->
    <section class="pv-hero">
      <div class="pv-hero__bg"></div>
      <div class="container pv-hero__content">
        <div class="pv-hero__left">
          <span class="pv-badge">
            <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/></svg>
            Para Consumidores
          </span>
          <h1>Compre nas suas lojas favoritas e <span class="pv-accent">ganhe dinheiro de volta</span></h1>
          <p>Cadastre-se grátis, compre normalmente nas lojas parceiras e receba cashback real direto na sua conta. Sem pegadinhas, sem pontos — dinheiro de verdade.</p>
          <div class="pv-hero__btns">
            <nuxt-link href="/register" class="pv-btn-primary">Criar conta grátis →</nuxt-link>
            <nuxt-link href="#como-funciona" class="pv-btn-outline">Como funciona</nuxt-link>
          </div>
          <div class="pv-hero__stats">
            <div class="pv-stat"><strong>+80 mil</strong><span>consumidores ativos</span></div>
            <div class="pv-stat-div"></div>
            <div class="pv-stat"><strong>R$ 4,2M</strong><span>em cashback pago</span></div>
            <div class="pv-stat-div"></div>
            <div class="pv-stat"><strong>+2.400</strong><span>lojas parceiras</span></div>
          </div>
        </div>
        <div class="pv-hero__right">
          <div class="pv-hero__mockup">
            <div class="pv-mockup-card pv-mockup-card--main">
              <div class="pv-mockup-label">Seu saldo de cashback</div>
              <div class="pv-mockup-value">R$ 187,50</div>
              <div class="pv-mockup-sub">↑ + R$ 32,00 este mês</div>
            </div>
            <div class="pv-mockup-row">
              <div class="pv-mockup-card pv-mockup-card--sm">
                <div class="pv-mockup-icon">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>
                </div>
                <div class="pv-mockup-card-label">Última compra</div>
                <div class="pv-mockup-card-val">+ R$ 18,00</div>
              </div>
              <div class="pv-mockup-card pv-mockup-card--sm">
                <div class="pv-mockup-icon">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>
                </div>
                <div class="pv-mockup-card-label">Disponível para saque</div>
                <div class="pv-mockup-card-val">R$ 150,00</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Como funciona -->
    <section class="pv-section" id="como-funciona">
      <div class="container">
        <div class="pv-section__header">
          <span class="pv-label">Simples assim</span>
          <h2>Como funciona para você</h2>
          <p>Três passos para começar a receber cashback nas suas compras hoje mesmo</p>
        </div>
        <div class="pv-steps">
          <div v-for="(step, i) in steps" :key="i" class="pv-step">
            <div class="pv-step__num">{{ i + 1 }}</div>
            <div class="pv-step__icon" v-html="step.icon"></div>
            <h3>{{ step.title }}</h3>
            <p>{{ step.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Quanto você ganha -->
    <section class="pv-section pv-section--gray">
      <div class="container">
        <div class="pv-section__header">
          <span class="pv-label">Exemplos reais</span>
          <h2>Quanto você pode ganhar</h2>
          <p>Veja quanto cashback você receberia em compras do dia a dia</p>
        </div>
        <div class="pv-examples">
          <div v-for="ex in examples" :key="ex.category" class="pv-example-card">
            <div class="pv-example-icon" v-html="ex.icon"></div>
            <div class="pv-example-category">{{ ex.category }}</div>
            <div class="pv-example-spend">R$ {{ ex.spend }} gastos</div>
            <div class="pv-example-arrow">→</div>
            <div class="pv-example-back">
              <span class="pv-example-amount">+ R$ {{ ex.back }}</span>
              <span class="pv-example-back-label">de volta</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Lojas parceiras -->
    <section class="pv-section">
      <div class="container">
        <div class="pv-section__header">
          <span class="pv-label">Onde usar</span>
          <h2>+2.400 lojas parceiras esperando por você</h2>
          <p>De supermercados a eletrônicos, farmácias, restaurantes e muito mais</p>
        </div>
        <div class="pv-categories">
          <div v-for="cat in categories" :key="cat.name" class="pv-category-card">
            <div class="pv-category-icon" v-html="cat.icon"></div>
            <div class="pv-category-name">{{ cat.name }}</div>
            <div class="pv-category-count">{{ cat.count }} lojas</div>
          </div>
        </div>
        <div class="pv-section__cta">
          <nuxt-link href="/register" class="pv-btn-primary">Ver todas as lojas →</nuxt-link>
        </div>
      </div>
    </section>

    <!-- Vantagens -->
    <section class="pv-section pv-section--teal">
      <div class="container">
        <div class="pv-section__header pv-section__header--white">
          <span class="pv-label pv-label--white">Por que a Quanta Shop</span>
          <h2>Vantagens para você</h2>
        </div>
        <div class="pv-benefits">
          <div v-for="b in benefits" :key="b.title" class="pv-benefit-card">
            <div class="pv-benefit-icon" v-html="b.icon"></div>
            <h4>{{ b.title }}</h4>
            <p>{{ b.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Depoimentos -->
    <section class="pv-section">
      <div class="container">
        <div class="pv-section__header">
          <span class="pv-label">Quem já usa</span>
          <h2>O que nossos consumidores dizem</h2>
        </div>
        <div class="pv-testimonials">
          <div v-for="t in testimonials" :key="t.name" class="pv-testimonial">
            <div class="pv-testimonial__stars">★★★★★</div>
            <p class="pv-testimonial__text">"{{ t.text }}"</p>
            <div class="pv-testimonial__author">
              <div class="pv-testimonial__avatar">{{ t.name[0] }}</div>
              <div>
                <div class="pv-testimonial__name">{{ t.name }}</div>
                <div class="pv-testimonial__city">{{ t.city }}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA Final -->
    <section class="pv-cta">
      <div class="container pv-cta__inner">
        <div>
          <h2>Pronto para ganhar dinheiro comprando?</h2>
          <p>Cadastro gratuito em menos de 2 minutos. Comece a receber cashback hoje.</p>
        </div>
        <div class="pv-cta__btns">
          <nuxt-link href="/register" class="pv-btn-primary">Criar minha conta grátis</nuxt-link>
          <nuxt-link href="/contato" class="pv-btn-outline pv-btn-outline--white">Falar com a gente</nuxt-link>
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
});

const steps = [
  {
    title: 'Cadastre-se grátis',
    desc: 'Crie sua conta em menos de 2 minutos. Sem mensalidade, sem taxa de adesão.',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>',
  },
  {
    title: 'Compre nas lojas parceiras',
    desc: 'Mais de 2.400 lojas físicas e online participantes. Compre como sempre faz.',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>',
  },
  {
    title: 'Receba cashback de volta',
    desc: 'O dinheiro cai automaticamente no seu saldo. Saque quando quiser, sem complicação.',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>',
  },
];

const examples = [
  {
    category: 'Supermercado',
    spend: '500',
    back: '25,00',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>',
  },
  {
    category: 'Farmácia',
    spend: '200',
    back: '14,00',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>',
  },
  {
    category: 'Eletrônicos',
    spend: '1.200',
    back: '72,00',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><rect x="2" y="3" width="20" height="14" rx="2" ry="2"/><line x1="8" y1="21" x2="16" y2="21"/><line x1="12" y1="17" x2="12" y2="21"/></svg>',
  },
  {
    category: 'Restaurante',
    spend: '150',
    back: '9,00',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M18 8h1a4 4 0 0 1 0 8h-1"/><path d="M2 8h16v9a4 4 0 0 1-4 4H6a4 4 0 0 1-4-4V8z"/><line x1="6" y1="1" x2="6" y2="4"/><line x1="10" y1="1" x2="10" y2="4"/><line x1="14" y1="1" x2="14" y2="4"/></svg>',
  },
  {
    category: 'Moda & Vestuário',
    spend: '350',
    back: '21,00',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M20.38 3.46L16 2a4 4 0 0 1-8 0L3.62 3.46a2 2 0 0 0-1.34 2.23l.58 3.57a1 1 0 0 0 .99.84H6v10a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V10h2.15a1 1 0 0 0 .99-.84l.58-3.57a2 2 0 0 0-1.34-2.23z"/></svg>',
  },
  {
    category: 'Combustível',
    spend: '300',
    back: '15,00',
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="1.8"><path d="M3 22V9a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v13"/><path d="M21 22V10.5a.5.5 0 0 0-.5-.5H17"/><path d="M3 22h18"/><path d="M17 10V5a1 1 0 0 0-1-1h-1"/><rect x="5" y="11" width="6" height="4" rx="1"/></svg>',
  },
];

const categories = [
  { name: 'Supermercados', count: '380+', icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>' },
  { name: 'Farmácias', count: '210+', icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>' },
  { name: 'Restaurantes', count: '520+', icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M18 8h1a4 4 0 0 1 0 8h-1"/><path d="M2 8h16v9a4 4 0 0 1-4 4H6a4 4 0 0 1-4-4V8z"/><line x1="6" y1="1" x2="6" y2="4"/><line x1="10" y1="1" x2="10" y2="4"/><line x1="14" y1="1" x2="14" y2="4"/></svg>' },
  { name: 'Eletrônicos', count: '180+', icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><rect x="2" y="3" width="20" height="14" rx="2"/><line x1="8" y1="21" x2="16" y2="21"/><line x1="12" y1="17" x2="12" y2="21"/></svg>' },
  { name: 'Moda', count: '290+', icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M20.38 3.46L16 2a4 4 0 0 1-8 0L3.62 3.46a2 2 0 0 0-1.34 2.23l.58 3.57a1 1 0 0 0 .99.84H6v10a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V10h2.15a1 1 0 0 0 .99-.84l.58-3.57a2 2 0 0 0-1.34-2.23z"/></svg>' },
  { name: 'Postos de Gasolina', count: '340+', icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M3 22V9a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v13"/><path d="M21 22V10.5a.5.5 0 0 0-.5-.5H17"/><path d="M3 22h18"/><rect x="5" y="11" width="6" height="4" rx="1"/></svg>' },
  { name: 'Beleza & Saúde', count: '160+', icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/></svg>' },
  { name: 'Viagens & Turismo', count: '120+', icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07A19.5 19.5 0 0 1 4.72 13a19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 3.63 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91A16 16 0 0 0 15 15.91l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"/></svg>' },
];

const benefits = [
  {
    title: 'Cadastro 100% gratuito',
    desc: 'Não existe mensalidade, taxa de adesão ou qualquer cobrança. Cadastre-se e comece a usar.',
    icon: '<svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><polyline points="20 6 9 17 4 12"/></svg>',
  },
  {
    title: 'Dinheiro de verdade',
    desc: 'Sem pontos confusos que expiram. Seu cashback é em reais, disponível para saque quando quiser.',
    icon: '<svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>',
  },
  {
    title: 'Saque fácil e rápido',
    desc: 'Transfira o saldo para sua conta bancária diretamente pelo app, sem burocracia.',
    icon: '<svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><polyline points="23 6 13.5 15.5 8.5 10.5 1 18"/><polyline points="17 6 23 6 23 12"/></svg>',
  },
  {
    title: 'Compre como sempre',
    desc: 'Não precisa mudar nada na sua rotina. Só mostre seu código ou acesse pela plataforma.',
    icon: '<svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/></svg>',
  },
  {
    title: '+2.400 lojas disponíveis',
    desc: 'Supermercados, farmácias, restaurantes, combustível, moda, eletrônicos e muito mais.',
    icon: '<svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>',
  },
  {
    title: 'Indique e ganhe mais',
    desc: 'Cada amigo que você indicar que comprar também te gera cashback bônus automático.',
    icon: '<svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>',
  },
];

const testimonials = [
  {
    name: 'Camila Rodrigues',
    city: 'São Paulo, SP',
    text: 'Em 3 meses já resgatei mais de R$ 200 só fazendo as compras que faço todo mês. Não acreditei quando o dinheiro caiu na conta!',
  },
  {
    name: 'Marcos Oliveira',
    city: 'Belo Horizonte, MG',
    text: 'Uso no posto de gasolina todo dia. No mês passado resgatei quase R$ 80 reais sem fazer nada diferente. Recomendo demais.',
  },
  {
    name: 'Ana Paula Silva',
    city: 'Curitiba, PR',
    text: 'Indicar amigos foi o melhor. Cada compra deles me gera cashback também. Já atingi R$ 500 de saldo acumulado esse ano.',
  },
];
</script>

<style scoped>
.pv-page { font-family: 'Jost', sans-serif; }

/* Hero */
.pv-hero {
  position: relative;
  background: linear-gradient(135deg, #1a4f59 0%, #2F7785 55%, #225F6B 100%);
  padding: 80px 0 60px;
  overflow: hidden;
}
.pv-hero__bg {
  position: absolute; inset: 0;
  background: radial-gradient(ellipse at 70% 50%, rgba(152,199,58,0.12) 0%, transparent 65%);
  pointer-events: none;
}
.pv-hero__content {
  position: relative; z-index: 1;
  display: flex; align-items: center; gap: 64px;
}
.pv-hero__left { flex: 1; color: #fff; }
.pv-hero__right { flex: 0 0 340px; }
.pv-badge {
  display: inline-flex; align-items: center; gap: 6px;
  background: rgba(152,199,58,0.18); border: 1px solid rgba(152,199,58,0.4);
  color: #98C73A; padding: 5px 14px; border-radius: 100px;
  font-size: 13px; font-weight: 600; margin-bottom: 20px;
}
.pv-hero__left h1 {
  font-size: clamp(26px, 3.2vw, 46px); font-weight: 800;
  line-height: 1.15; margin-bottom: 18px; color: #fff;
}
.pv-accent { color: #98C73A; }
.pv-hero__left p { font-size: 17px; color: rgba(255,255,255,0.82); margin-bottom: 28px; line-height: 1.6; }
.pv-hero__btns { display: flex; gap: 12px; flex-wrap: wrap; margin-bottom: 32px; }
.pv-btn-primary {
  display: inline-flex; align-items: center; gap: 6px;
  background: #98C73A; color: #1a3a00; font-weight: 700;
  padding: 13px 26px; border-radius: 8px; text-decoration: none;
  font-size: 15px; transition: background 0.2s, transform 0.15s;
}
.pv-btn-primary:hover { background: #89b534; transform: translateY(-1px); }
.pv-btn-outline {
  display: inline-flex; align-items: center; gap: 6px;
  border: 2px solid rgba(255,255,255,0.4); color: #fff;
  padding: 12px 24px; border-radius: 8px; text-decoration: none;
  font-size: 15px; font-weight: 600; transition: border-color 0.2s, background 0.2s;
}
.pv-btn-outline:hover { border-color: #fff; background: rgba(255,255,255,0.08); }
.pv-hero__stats { display: flex; align-items: center; gap: 20px; flex-wrap: wrap; }
.pv-stat { display: flex; flex-direction: column; }
.pv-stat strong { font-size: 22px; font-weight: 800; color: #fff; }
.pv-stat span { font-size: 12px; color: rgba(255,255,255,0.65); }
.pv-stat-div { width: 1px; height: 36px; background: rgba(255,255,255,0.25); }

/* Mockup */
.pv-hero__mockup { display: flex; flex-direction: column; gap: 12px; }
.pv-mockup-card {
  background: rgba(255,255,255,0.12); backdrop-filter: blur(12px);
  border: 1px solid rgba(255,255,255,0.2); border-radius: 16px; padding: 20px 24px; color: #fff;
}
.pv-mockup-card--main { }
.pv-mockup-label { font-size: 13px; color: rgba(255,255,255,0.7); margin-bottom: 6px; }
.pv-mockup-value { font-size: 32px; font-weight: 800; color: #fff; }
.pv-mockup-sub { font-size: 13px; color: #98C73A; margin-top: 4px; }
.pv-mockup-row { display: flex; gap: 12px; }
.pv-mockup-card--sm { flex: 1; padding: 16px; }
.pv-mockup-icon { margin-bottom: 8px; }
.pv-mockup-card-label { font-size: 11px; color: rgba(255,255,255,0.65); }
.pv-mockup-card-val { font-size: 16px; font-weight: 700; color: #98C73A; }

/* Sections */
.pv-section { padding: 80px 0; }
.pv-section--gray { background: #f8f9fa; }
.pv-section--teal { background: linear-gradient(135deg, #225F6B 0%, #2F7785 100%); }
.pv-section__header { text-align: center; margin-bottom: 52px; }
.pv-section__header--white h2, .pv-section__header--white p { color: #fff; }
.pv-section__header h2 { font-size: clamp(22px, 2.5vw, 34px); font-weight: 700; color: #111827; margin-bottom: 12px; }
.pv-section__header p { font-size: 16px; color: #6b7280; max-width: 540px; margin: 0 auto; }
.pv-label {
  display: inline-block; background: rgba(47,119,133,0.1); color: #2F7785;
  font-size: 12px; font-weight: 700; letter-spacing: 1px; text-transform: uppercase;
  padding: 4px 14px; border-radius: 100px; margin-bottom: 14px;
}
.pv-label--white { background: rgba(255,255,255,0.15); color: #fff; }

/* Steps */
.pv-steps { display: grid; grid-template-columns: repeat(3, 1fr); gap: 32px; }
.pv-step {
  text-align: center; padding: 32px 24px;
  background: #fff; border-radius: 16px;
  box-shadow: 0 2px 16px rgba(0,0,0,0.06);
  border: 1px solid #f0f0f0;
}
.pv-step__num {
  width: 40px; height: 40px; border-radius: 50%;
  background: linear-gradient(135deg, #2F7785, #98C73A);
  color: #fff; font-weight: 800; font-size: 18px;
  display: flex; align-items: center; justify-content: center;
  margin: 0 auto 16px;
}
.pv-step__icon { color: #2F7785; margin-bottom: 12px; }
.pv-step h3 { font-size: 17px; font-weight: 700; color: #111827; margin-bottom: 8px; }
.pv-step p { font-size: 14px; color: #6b7280; line-height: 1.6; }

/* Examples */
.pv-examples { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; }
.pv-example-card {
  background: #fff; border-radius: 16px; padding: 24px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.06); border: 1px solid #f0f0f0;
  display: flex; flex-direction: column; align-items: center; text-align: center; gap: 8px;
}
.pv-example-icon { color: #2F7785; }
.pv-example-category { font-weight: 700; font-size: 15px; color: #111827; }
.pv-example-spend { font-size: 13px; color: #6b7280; }
.pv-example-arrow { font-size: 20px; color: #2F7785; font-weight: 700; }
.pv-example-back { display: flex; flex-direction: column; align-items: center; }
.pv-example-amount { font-size: 22px; font-weight: 800; color: #98C73A; }
.pv-example-back-label { font-size: 12px; color: #6b7280; }

/* Categories */
.pv-categories { display: grid; grid-template-columns: repeat(4, 1fr); gap: 16px; margin-bottom: 40px; }
.pv-category-card {
  background: #f8f9fa; border-radius: 12px; padding: 20px 16px;
  text-align: center; border: 1px solid #e9ecef; transition: border-color 0.2s, background 0.2s;
}
.pv-category-card:hover { border-color: #2F7785; background: #f0f7f8; }
.pv-category-icon { color: #2F7785; margin-bottom: 10px; }
.pv-category-name { font-weight: 600; font-size: 14px; color: #111827; margin-bottom: 4px; }
.pv-category-count { font-size: 12px; color: #2F7785; font-weight: 600; }
.pv-section__cta { text-align: center; }

/* Benefits */
.pv-benefits { display: grid; grid-template-columns: repeat(3, 1fr); gap: 24px; }
.pv-benefit-card {
  background: rgba(255,255,255,0.1); border: 1px solid rgba(255,255,255,0.15);
  border-radius: 16px; padding: 28px 24px;
}
.pv-benefit-icon { margin-bottom: 14px; }
.pv-benefit-card h4 { font-size: 16px; font-weight: 700; color: #fff; margin-bottom: 8px; }
.pv-benefit-card p { font-size: 14px; color: rgba(255,255,255,0.72); line-height: 1.6; }

/* Testimonials */
.pv-testimonials { display: grid; grid-template-columns: repeat(3, 1fr); gap: 24px; }
.pv-testimonial {
  background: #fff; border-radius: 16px; padding: 28px 24px;
  box-shadow: 0 2px 16px rgba(0,0,0,0.07); border: 1px solid #f0f0f0;
}
.pv-testimonial__stars { color: #98C73A; font-size: 16px; margin-bottom: 12px; letter-spacing: 2px; }
.pv-testimonial__text { font-size: 15px; color: #374151; line-height: 1.65; margin-bottom: 20px; font-style: italic; }
.pv-testimonial__author { display: flex; align-items: center; gap: 12px; }
.pv-testimonial__avatar {
  width: 40px; height: 40px; border-radius: 50%;
  background: linear-gradient(135deg, #2F7785, #98C73A);
  color: #fff; font-weight: 700; font-size: 16px;
  display: flex; align-items: center; justify-content: center; flex-shrink: 0;
}
.pv-testimonial__name { font-weight: 700; font-size: 14px; color: #111827; }
.pv-testimonial__city { font-size: 12px; color: #9ca3af; }

/* CTA Final */
.pv-cta {
  background: linear-gradient(135deg, #1a3a00 0%, #2a5800 100%);
  padding: 72px 0;
}
.pv-cta__inner {
  display: flex; align-items: center; justify-content: space-between; gap: 40px; flex-wrap: wrap;
}
.pv-cta__inner h2 { font-size: clamp(22px, 2.5vw, 32px); font-weight: 800; color: #fff; margin-bottom: 8px; }
.pv-cta__inner p { font-size: 16px; color: rgba(255,255,255,0.72); }
.pv-cta__btns { display: flex; gap: 12px; flex-wrap: wrap; }
.pv-btn-outline--white {
  border-color: rgba(255,255,255,0.35); color: #fff;
}
.pv-btn-outline--white:hover { border-color: #fff; }

/* Responsive */
@media (max-width: 1100px) {
  .pv-hero__content { gap: 40px; }
  .pv-hero__right { flex: 0 0 280px; }
  .pv-examples { grid-template-columns: repeat(2, 1fr); }
  .pv-categories { grid-template-columns: repeat(4, 1fr); }
}
@media (max-width: 900px) {
  .pv-hero__content { flex-direction: column; }
  .pv-hero__right { flex: unset; width: 100%; max-width: 400px; }
  .pv-steps { grid-template-columns: 1fr 1fr; }
  .pv-benefits { grid-template-columns: 1fr 1fr; }
  .pv-testimonials { grid-template-columns: 1fr 1fr; }
  .pv-categories { grid-template-columns: repeat(4, 1fr); }
}
@media (max-width: 600px) {
  .pv-hero { padding: 52px 0 40px; }
  .pv-steps { grid-template-columns: 1fr; }
  .pv-examples { grid-template-columns: 1fr 1fr; }
  .pv-categories { grid-template-columns: repeat(2, 1fr); }
  .pv-benefits { grid-template-columns: 1fr; }
  .pv-testimonials { grid-template-columns: 1fr; }
  .pv-cta__inner { flex-direction: column; text-align: center; }
  .pv-cta__btns { justify-content: center; }
}
</style>
