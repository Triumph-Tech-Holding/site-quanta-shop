<template>
  <div>
    <!-- ── HERO ─────────────────────────────────────────────── -->
    <section class="ag-hero">
      <div class="ag-hero__inner container">
        <div class="ag-hero__text">
          <div class="qs-eyebrow ag-eyebrow">Plataforma de Cashback #1 do Brasil</div>
          <h1 class="ag-hero__h1">
            Seu dinheiro
            <span class="ag-hero__highlight">volta</span>
            a cada compra
          </h1>
          <p class="ag-hero__sub">
            Compre em centenas de lojas parceiras e receba cashback de verdade — simples, transparente e instantâneo.
          </p>
          <div class="ag-hero__ctas">
            <NuxtLink to="/agencia/cadastro" class="ag-btn-primary">
              Criar conta grátis
              <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
            </NuxtLink>
            <NuxtLink to="/agencia/login" class="ag-btn-outline">Já tenho conta</NuxtLink>
          </div>
          <div class="ag-hero__stats">
            <div class="ag-stat">
              <span class="ag-stat__val">+12 mil</span>
              <span class="ag-stat__lbl">usuários ativos</span>
            </div>
            <div class="ag-stat__div"></div>
            <div class="ag-stat">
              <span class="ag-stat__val">+500</span>
              <span class="ag-stat__lbl">lojas parceiras</span>
            </div>
            <div class="ag-stat__div"></div>
            <div class="ag-stat">
              <span class="ag-stat__val">Até 30%</span>
              <span class="ag-stat__lbl">cashback por compra</span>
            </div>
          </div>
        </div>

        <div class="ag-hero__visual">
          <div class="ag-cashback-card">
            <div class="ag-cashback-card__label">Cashback recebido</div>
            <div class="ag-cashback-card__amount">R$ 127,50</div>
            <div class="ag-cashback-card__sub">neste mês</div>
            <div class="ag-cashback-card__badge">
              <svg width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="#fff" stroke-width="2.5"><circle cx="12" cy="12" r="10"/><path d="m9 12 2 2 4-4"/></svg>
              Cashback Garantido
            </div>
          </div>
          <div class="ag-float-pill ag-float-pill--top">
            <span class="ag-float-pill__dot"></span>
            +234 compras hoje
          </div>
          <div class="ag-float-pill ag-float-pill--bot">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="#98c73a"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 14.5v-9l6 4.5-6 4.5z"/></svg>
            Saque liberado em segundos
          </div>
        </div>
      </div>
    </section>

    <!-- ── POR QUE ESCOLHER ─────────────────────────────────── -->
    <section class="qs-section">
      <div class="container">
        <div class="qs-section-header">
          <div class="qs-label qs-label--lime">Vantagens</div>
          <h2>Por que escolher a Quanta Shop?</h2>
          <p>Uma plataforma completa para você ganhar dinheiro em cada compra do seu dia a dia.</p>
        </div>
        <div class="qs-benefits-grid">
          <div class="qs-benefit-card qs-benefit-card--light" v-for="(f, i) in features" :key="i">
            <div class="qs-benefit-icon">{{ f.icon }}</div>
            <h4>{{ f.titulo }}</h4>
            <p>{{ f.descricao }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- ── COMO FUNCIONA ────────────────────────────────────── -->
    <section class="qs-section qs-section--gray">
      <div class="container">
        <div class="qs-section-header">
          <div class="qs-label qs-label--lime">Passo a passo</div>
          <h2>Como funciona</h2>
          <p>Em menos de 5 minutos você já começa a ganhar cashback nas suas compras.</p>
        </div>
        <div class="qs-steps">
          <div class="qs-step-card" v-for="(step, i) in steps" :key="i">
            <div class="qs-step-num">{{ i + 1 }}</div>
            <div class="qs-step-icon">{{ step.icon }}</div>
            <h3>{{ step.titulo }}</h3>
            <p>{{ step.descricao }}</p>
          </div>
        </div>
        <div class="qs-section-cta">
          <NuxtLink to="/agencia/como-funciona" class="qs-page-btn-primary">
            Saber mais
            <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
          </NuxtLink>
        </div>
      </div>
    </section>

    <!-- ── CASHBACK EXEMPLOS ────────────────────────────────── -->
    <section class="qs-section qs-section--teal">
      <div class="container">
        <div class="qs-section-header qs-section-header--white">
          <div class="qs-label qs-label--lime">Ganhos reais</div>
          <h2>Quanto você pode ganhar?</h2>
          <p>Exemplos de cashback em compras do dia a dia</p>
        </div>
        <div class="qs-example-grid">
          <div class="qs-example-card" v-for="(ex, i) in cashbackExamples" :key="i">
            <div class="qs-example-icon">{{ ex.icon }}</div>
            <div class="qs-example-category">{{ ex.categoria }}</div>
            <div class="qs-example-spend">Compra de {{ ex.gasto }}</div>
            <div class="qs-example-arrow">↓</div>
            <span class="qs-example-amount">{{ ex.retorno }}</span>
            <div class="qs-example-back-label">de cashback</div>
          </div>
        </div>
      </div>
    </section>

    <!-- ── CTA FINAL ─────────────────────────────────────────── -->
    <section class="qs-section">
      <div class="container">
        <div class="ag-cta-final">
          <div class="qs-label qs-label--lime" style="margin-bottom:12px">Comece agora</div>
          <h2 class="ag-cta-final__h2">Pronto para começar a ganhar?</h2>
          <p class="ag-cta-final__sub">Cadastre-se gratuitamente e faça parte da maior rede de cashback do Brasil.</p>
          <div class="ag-cta-final__btns">
            <NuxtLink to="/agencia/cadastro" class="ag-btn-primary ag-btn-primary--lg">
              Criar minha conta grátis
              <svg width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
            </NuxtLink>
            <NuxtLink to="/agencia/login" class="ag-btn-outline ag-btn-outline--lg">Já tenho conta</NuxtLink>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia' });

const features = [
  { icon: '💰', titulo: 'Cashback real', descricao: 'Receba de volta uma parte do valor de cada compra realizada em lojas credenciadas. Sem pontos, sem truques.' },
  { icon: '🤝', titulo: 'Rede de indicações', descricao: 'Indique amigos e lojistas para ampliar seus ganhos. Você ganha comissão nas compras deles também.' },
  { icon: '🏪', titulo: 'Lojas credenciadas', descricao: 'Centenas de estabelecimentos físicos e online aceitando a plataforma Quanta Shop.' },
  { icon: '⚡', titulo: 'Saque instantâneo', descricao: 'Solicitou o saque? O valor cai na sua conta em segundos, sem burocracia.' },
  { icon: '🔒', titulo: 'Seguro e transparente', descricao: 'Seus dados protegidos e todo cashback rastreável em tempo real no seu painel.' },
  { icon: '📱', titulo: 'Acesso em qualquer lugar', descricao: 'Plataforma 100% responsiva. Acesse pelo celular, tablet ou computador.' },
];

const steps = [
  { icon: '👤', titulo: 'Crie sua conta grátis', descricao: 'Cadastre-se em minutos com seus dados básicos. Sem custo algum.' },
  { icon: '🛒', titulo: 'Compre em lojas credenciadas', descricao: 'Use seu login Quanta Shop ao comprar nas lojas parceiras físicas ou online.' },
  { icon: '💸', titulo: 'Acumule cashback', descricao: 'Cada compra gera cashback automático no seu saldo, visível em tempo real.' },
  { icon: '🏦', titulo: 'Resgate quando quiser', descricao: 'Solicite o saque do saldo acumulado para sua conta bancária. Rápido e sem taxa.' },
];

const cashbackExamples = [
  { icon: '👟', categoria: 'Moda & Calçados', gasto: 'R$ 200', retorno: 'R$ 24,00' },
  { icon: '📱', categoria: 'Eletrônicos', gasto: 'R$ 1.500', retorno: 'R$ 90,00' },
  { icon: '🍕', categoria: 'Alimentação', gasto: 'R$ 100', retorno: 'R$ 10,00' },
  { icon: '✈️', categoria: 'Viagens', gasto: 'R$ 3.000', retorno: 'R$ 270,00' },
];
</script>

<style scoped>
/* ── Hero ────────────────────────────────────── */
.ag-hero {
  background: linear-gradient(135deg, #0f2e35 0%, #1a4a55 50%, #225f6b 100%);
  padding: 80px 0 72px;
  overflow: hidden;
  position: relative;
}
.ag-hero::before {
  content: '';
  position: absolute;
  top: -40%;
  right: -10%;
  width: 600px;
  height: 600px;
  border-radius: 50%;
  background: radial-gradient(circle, rgba(152,199,58,0.08) 0%, transparent 70%);
  pointer-events: none;
}
.ag-hero__inner {
  display: grid;
  grid-template-columns: 1fr 360px;
  gap: 60px;
  align-items: center;
}
@media (max-width: 900px) {
  .ag-hero__inner { grid-template-columns: 1fr; gap: 40px; }
  .ag-hero { padding: 56px 0 48px; }
}
.ag-eyebrow {
  color: #98c73a !important;
  background: rgba(152,199,58,0.12) !important;
  display: inline-block;
  margin-bottom: 16px;
}
.ag-hero__h1 {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(36px, 5vw, 56px);
  font-weight: 900;
  color: #fff;
  line-height: 1.1;
  letter-spacing: -0.03em;
  margin-bottom: 20px;
}
.ag-hero__highlight { color: #98c73a; }
.ag-hero__sub {
  font-size: 16px;
  color: rgba(255,255,255,0.72);
  line-height: 1.7;
  max-width: 480px;
  margin-bottom: 32px;
}
.ag-hero__ctas { display: flex; gap: 12px; flex-wrap: wrap; margin-bottom: 36px; }
.ag-hero__stats {
  display: flex;
  align-items: center;
  gap: 24px;
  flex-wrap: wrap;
}
.ag-stat { display: flex; flex-direction: column; gap: 2px; }
.ag-stat__val { font-size: 18px; font-weight: 800; color: #fff; }
.ag-stat__lbl { font-size: 12px; color: rgba(255,255,255,0.55); }
.ag-stat__div { width: 1px; height: 36px; background: rgba(255,255,255,0.15); }

/* Buttons */
.ag-btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: #98c73a;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 700;
  padding: 14px 28px;
  border-radius: 10px;
  text-decoration: none;
  transition: all 0.2s;
}
.ag-btn-primary:hover { background: #7aad1f; color: #fff; transform: translateY(-1px); }
.ag-btn-primary--lg { padding: 16px 36px; font-size: 16px; }
.ag-btn-outline {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: transparent;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 600;
  padding: 14px 28px;
  border-radius: 10px;
  border: 2px solid rgba(255,255,255,0.35);
  text-decoration: none;
  transition: all 0.2s;
}
.ag-btn-outline:hover { border-color: #fff; color: #fff; background: rgba(255,255,255,0.08); }
.ag-btn-outline--lg { padding: 16px 36px; font-size: 16px; }

/* Visual hero */
.ag-hero__visual {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 300px;
}
.ag-cashback-card {
  background: rgba(255,255,255,0.08);
  border: 1px solid rgba(255,255,255,0.18);
  backdrop-filter: blur(16px);
  border-radius: 20px;
  padding: 32px 36px;
  text-align: center;
  width: 240px;
}
.ag-cashback-card__label {
  font-size: 12px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: rgba(255,255,255,0.55);
  margin-bottom: 12px;
}
.ag-cashback-card__amount {
  font-size: 42px;
  font-weight: 900;
  color: #98c73a;
  line-height: 1;
  letter-spacing: -0.03em;
  margin-bottom: 4px;
}
.ag-cashback-card__sub { font-size: 13px; color: rgba(255,255,255,0.55); margin-bottom: 20px; }
.ag-cashback-card__badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: rgba(152,199,58,0.2);
  border: 1px solid rgba(152,199,58,0.4);
  color: #98c73a;
  font-size: 12px;
  font-weight: 600;
  padding: 6px 14px;
  border-radius: 999px;
}
.ag-float-pill {
  position: absolute;
  background: #fff;
  border-radius: 999px;
  padding: 8px 16px;
  font-size: 12px;
  font-weight: 600;
  color: #225f6b;
  display: flex;
  align-items: center;
  gap: 8px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.15);
  white-space: nowrap;
}
.ag-float-pill--top { top: 10px; right: -10px; }
.ag-float-pill--bot { bottom: 20px; left: -10px; }
.ag-float-pill__dot {
  width: 8px; height: 8px; border-radius: 50%;
  background: #98c73a;
  box-shadow: 0 0 0 3px rgba(152,199,58,0.25);
}
@media (max-width: 900px) {
  .ag-float-pill--top { right: 0; }
  .ag-float-pill--bot { left: 0; }
}

/* CTA final */
.ag-cta-final {
  text-align: center;
  max-width: 600px;
  margin: 0 auto;
}
.ag-cta-final__h2 {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(28px, 3vw, 40px);
  font-weight: 900;
  color: #225f6b;
  letter-spacing: -0.03em;
  margin-bottom: 12px;
}
.ag-cta-final__sub { font-size: 16px; color: #6b7280; margin-bottom: 32px; line-height: 1.6; }
.ag-cta-final__btns { display: flex; gap: 12px; justify-content: center; flex-wrap: wrap; }
.ag-cta-final .ag-btn-outline { border-color: #2f7785; color: #2f7785; }
.ag-cta-final .ag-btn-outline:hover { background: #2f7785; color: #fff; border-color: #2f7785; }
</style>
