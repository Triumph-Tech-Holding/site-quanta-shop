<template>
  <div class="pse-page">

    <!-- Hero -->
    <section class="pse-hero">
      <div class="pse-hero__bg"></div>
      <div class="container pse-hero__content">
        <div class="pse-hero__left">
          <span class="pse-badge">
            <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><rect x="2" y="7" width="20" height="14" rx="2"/><path d="M16 21V5a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16"/></svg>
            Para Lojistas e Empresas
          </span>
          <h1>Aumente suas vendas com <span class="pse-accent">cashback real</span> para seus clientes</h1>
          <p>Ofereça recompensas automáticas em cada compra, fidelize seu público e atraia novos clientes sem custo fixo. Só paga quando vende.</p>
          <div class="pse-hero__btns">
            <nuxt-link href="/agencia/credenciar" class="pse-btn-primary">Credenciar minha empresa →</nuxt-link>
            <nuxt-link href="#como-funciona" class="pse-btn-outline">Como funciona</nuxt-link>
          </div>
          <div class="pse-hero__stats">
            <div class="pse-stat"><strong>+2.400</strong><span>empresas credenciadas</span></div>
            <div class="pse-stat-div"></div>
            <div class="pse-stat"><strong>R$ 4,2M</strong><span>cashback distribuído</span></div>
            <div class="pse-stat-div"></div>
            <div class="pse-stat"><strong>98%</strong><span>satisfação de parceiros</span></div>
          </div>
        </div>
        <div class="pse-hero__right">
          <div class="pse-hero__mockup">
            <div class="pse-mockup-card">
              <div class="pse-mockup-row">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="#98C73A" stroke="none"><path d="M12 2l3.09 6.26L22 9.27l-5 4.87L18.18 21 12 17.77 5.82 21 7 14.14 2 9.27l6.91-1.01L12 2z"/></svg>
                <span>Vendas hoje</span>
              </div>
              <div class="pse-mockup-value">R$ 3.847,00</div>
              <div class="pse-mockup-sub">↑ 12% vs ontem</div>
            </div>
            <div class="pse-mockup-card pse-mockup-card--sm">
              <div class="pse-mockup-row">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
                <span>Clientes ativos</span>
              </div>
              <div class="pse-mockup-value pse-mockup-value--sm">1.284</div>
            </div>
            <div class="pse-mockup-card pse-mockup-card--sm">
              <div class="pse-mockup-row">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 1 0 0 7h5a3.5 3.5 0 1 1 0 7H6"/></svg>
                <span>Cashback gerado</span>
              </div>
              <div class="pse-mockup-value pse-mockup-value--sm">R$ 384,70</div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Como funciona -->
    <section class="pse-section" id="como-funciona">
      <div class="container">
        <div class="pse-section__header">
          <span class="pse-label">Simples assim</span>
          <h2>Como funciona para sua empresa</h2>
          <p>Três passos para começar a oferecer cashback para seus clientes hoje mesmo</p>
        </div>
        <div class="pse-steps">
          <div v-for="(step, i) in steps" :key="i" class="pse-step">
            <div class="pse-step__num">{{ i + 1 }}</div>
            <div class="pse-step__icon" v-html="step.icon"></div>
            <h3>{{ step.title }}</h3>
            <p>{{ step.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Benefícios -->
    <section class="pse-section pse-section--gray">
      <div class="container">
        <div class="pse-section__header">
          <span class="pse-label">Vantagens</span>
          <h2>Tudo que sua empresa precisa</h2>
        </div>
        <div class="pse-benefits">
          <div v-for="b in benefits" :key="b.title" class="pse-benefit-card">
            <div class="pse-benefit-icon" v-html="b.icon"></div>
            <h4>{{ b.title }}</h4>
            <p>{{ b.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Planos -->
    <section class="pse-section">
      <div class="container">
        <div class="pse-section__header">
          <span class="pse-label">Simples e transparente</span>
          <h2>Sem mensalidade. Só paga quando vende.</h2>
          <p>Comissão de 2% sobre o valor transacionado. Sem surpresas.</p>
        </div>
        <div class="pse-plan">
          <div class="pse-plan__box">
            <div class="pse-plan__badge">Único modelo</div>
            <div class="pse-plan__price">2<span>%</span></div>
            <div class="pse-plan__label">por venda realizada</div>
            <ul class="pse-plan__features">
              <li>✓ Cadastro gratuito e imediato</li>
              <li>✓ Painel de gestão completo</li>
              <li>✓ Relatórios em tempo real</li>
              <li>✓ Suporte dedicado</li>
              <li>✓ Sem mensalidade ou taxa de adesão</li>
              <li>✓ Integração com maquininha e e-commerce</li>
            </ul>
            <nuxt-link href="/agencia/credenciar" class="pse-btn-primary pse-btn-full">Credenciar agora gratuitamente →</nuxt-link>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA final -->
    <section class="pse-cta">
      <div class="container pse-cta__inner">
        <div>
          <h2>Pronto para crescer com cashback?</h2>
          <p>Mais de 2.400 empresas já usam a Quanta Shop para fidelizar clientes e aumentar vendas.</p>
        </div>
        <div class="pse-cta__btns">
          <nuxt-link href="/agencia/credenciar" class="pse-btn-primary">Credenciar minha empresa</nuxt-link>
          <nuxt-link href="/contato" class="pse-btn-outline pse-btn-outline--white">Falar com consultor</nuxt-link>
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ title: "Para sua Empresa | Quanta Shop", description: "Ofereça cashback real aos seus clientes. Aumente vendas, fidelize clientes. Só paga quando vende.", canonical: "https://quantashop.com.br/para-sua-empresa" });

const steps = [
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><line x1="19" y1="8" x2="19" y2="14"/><line x1="22" y1="11" x2="16" y2="11"/></svg>',
    title: 'Cadastre sua empresa',
    desc: 'Preencha o formulário em 2 minutos. Aprovação rápida, sem burocracia.',
  },
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><rect x="2" y="3" width="20" height="14" rx="2"/><line x1="8" y1="21" x2="16" y2="21"/><line x1="12" y1="17" x2="12" y2="21"/></svg>',
    title: 'Configure o cashback',
    desc: 'Defina o percentual de cashback para seus clientes. Você tem controle total.',
  },
  {
    icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><polyline points="23 6 13.5 15.5 8.5 10.5 1 18"/><polyline points="17 6 23 6 23 12"/></svg>',
    title: 'Venda e fidelize',
    desc: 'Clientes ganham cashback automático em cada compra e voltam para gastar mais.',
  },
];

const benefits = [
  {
    icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>',
    title: 'Zero custo fixo',
    desc: 'Sem mensalidade ou taxa de adesão. Você só paga 2% sobre o que vender.',
  },
  {
    icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><line x1="18" y1="20" x2="18" y2="10"/><line x1="12" y1="20" x2="12" y2="4"/><line x1="6" y1="20" x2="6" y2="14"/></svg>',
    title: 'Relatórios completos',
    desc: 'Veja vendas, cashback gerado e clientes ativos em tempo real no seu painel.',
  },
  {
    icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>',
    title: 'Rede de clientes',
    desc: 'Seu estabelecimento aparece para todos os usuários da Quanta Shop na sua região.',
  },
  {
    icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><rect x="1" y="4" width="22" height="16" rx="2"/><line x1="1" y1="10" x2="23" y2="10"/></svg>',
    title: 'Maquininha integrada',
    desc: 'Integração com as principais maquininhas do mercado. Cashback automático na venda.',
  },
  {
    icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/></svg>',
    title: 'Cashback instantâneo',
    desc: 'O cashback é creditado na carteira do cliente imediatamente após a compra.',
  },
  {
    icon: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07A19.5 19.5 0 0 1 4.69 13a19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 3.6 2h3a2 2 0 0 1 2 1.72c.127.96.361 1.903.7 2.81a2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0 1 22 16.92z"/></svg>',
    title: 'Suporte dedicado',
    desc: 'Time de suporte disponível por WhatsApp para ajudar em qualquer dúvida.',
  },
];
</script>

<style scoped>
.pse-page { font-family: 'Inter', 'Jost', sans-serif; }

/* Hero */
.pse-hero {
  position: relative;
  background: linear-gradient(135deg, #0f232d 0%, #1a3f4f 50%, #225F6B 100%);
  min-height: 520px;
  display: flex;
  align-items: center;
  overflow: hidden;
}
.pse-hero::after {
  content: '';
  position: absolute;
  inset: 0;
  background: url('https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=1400&q=75&auto=format&fit=crop') center/cover;
  opacity: 0.12;
}
.pse-hero__content {
  position: relative;
  z-index: 1;
  display: flex;
  align-items: center;
  gap: 64px;
  padding: 72px 0;
}
.pse-hero__left { flex: 1; }
.pse-hero__right { flex: 0 0 340px; }

.pse-badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: rgba(152, 199, 58, 0.15);
  color: #98C73A;
  border: 1px solid rgba(152, 199, 58, 0.3);
  border-radius: 999px;
  padding: 5px 14px;
  font-size: 13px;
  font-weight: 500;
  margin-bottom: 20px;
}

.pse-hero__left h1 {
  font-size: clamp(28px, 3.5vw, 44px);
  font-weight: 800;
  color: #fff;
  line-height: 1.18;
  margin-bottom: 16px;
}
.pse-accent { color: #98C73A; }

.pse-hero__left p {
  font-size: 16px;
  color: rgba(255,255,255,0.75);
  line-height: 1.6;
  margin-bottom: 28px;
  max-width: 480px;
}

.pse-hero__btns { display: flex; gap: 12px; flex-wrap: wrap; margin-bottom: 36px; }

.pse-btn-primary {
  display: inline-flex;
  align-items: center;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 600;
  padding: 12px 24px;
  border-radius: 8px;
  text-decoration: none;
  transition: background 0.2s;
  border: none;
  cursor: pointer;
}
.pse-btn-primary:hover { background: #7aad1f; color: #fff; }
.pse-btn-full { width: 100%; justify-content: center; margin-top: 16px; }

.pse-btn-outline {
  display: inline-flex;
  align-items: center;
  background: transparent;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 500;
  padding: 12px 24px;
  border-radius: 8px;
  border: 1.5px solid rgba(255,255,255,0.4);
  text-decoration: none;
  transition: border-color 0.2s, background 0.2s;
}
.pse-btn-outline:hover { border-color: #fff; background: rgba(255,255,255,0.08); }

.pse-hero__stats { display: flex; align-items: center; gap: 20px; }
.pse-stat { display: flex; flex-direction: column; }
.pse-stat strong { font-size: 20px; font-weight: 700; color: #fff; }
.pse-stat span { font-size: 12px; color: rgba(255,255,255,0.55); }
.pse-stat-div { width: 1px; height: 36px; background: rgba(255,255,255,0.2); }

/* Mockup cards */
.pse-hero__mockup { display: flex; flex-direction: column; gap: 12px; }
.pse-mockup-card {
  background: rgba(255,255,255,0.12);
  backdrop-filter: blur(16px);
  border: 1px solid rgba(255,255,255,0.2);
  border-radius: 14px;
  padding: 18px 20px;
}
.pse-mockup-card--sm { padding: 14px 18px; }
.pse-mockup-row { display: flex; align-items: center; gap: 8px; font-size: 13px; color: rgba(255,255,255,0.7); margin-bottom: 8px; }
.pse-mockup-value { font-size: 24px; font-weight: 700; color: #fff; }
.pse-mockup-value--sm { font-size: 18px; }
.pse-mockup-sub { font-size: 12px; color: #98C73A; margin-top: 4px; }

/* Sections */
.pse-section { padding: 80px 0; background: #fff; }
.pse-section--gray { background: #f7f8fa; }
.pse-section__header { text-align: center; margin-bottom: 52px; }
.pse-label {
  display: inline-block;
  font-size: 12px;
  font-weight: 600;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: #2F7785;
  background: rgba(47, 119, 133, 0.08);
  padding: 4px 12px;
  border-radius: 999px;
  margin-bottom: 12px;
}
.pse-section__header h2 { font-size: clamp(22px, 2.5vw, 34px); font-weight: 700; color: #111827; margin-bottom: 12px; }
.pse-section__header p { font-size: 16px; color: #6b7280; max-width: 540px; margin: 0 auto; }

/* Steps */
.pse-steps { display: grid; grid-template-columns: repeat(3, 1fr); gap: 32px; }
@media (max-width: 768px) { .pse-steps { grid-template-columns: 1fr; } }
.pse-step {
  background: #fff;
  border: 1.5px solid #f0f0f0;
  border-radius: 16px;
  padding: 32px 28px;
  text-align: center;
  position: relative;
  box-shadow: 0 2px 12px rgba(0,0,0,0.04);
}
.pse-step__num {
  position: absolute;
  top: -16px;
  left: 50%;
  transform: translateX(-50%);
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background: #2F7785;
  color: #fff;
  font-size: 14px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
}
.pse-step__icon { display: flex; justify-content: center; margin: 12px 0 14px; }
.pse-step h3 { font-size: 17px; font-weight: 600; color: #111827; margin-bottom: 8px; }
.pse-step p { font-size: 14px; color: #6b7280; line-height: 1.6; }

/* Benefits */
.pse-benefits { display: grid; grid-template-columns: repeat(3, 1fr); gap: 24px; }
@media (max-width: 900px) { .pse-benefits { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 560px) { .pse-benefits { grid-template-columns: 1fr; } }
.pse-benefit-card {
  background: #fff;
  border-radius: 14px;
  padding: 28px 24px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.05);
}
.pse-benefit-icon { margin-bottom: 12px; }
.pse-benefit-card h4 { font-size: 16px; font-weight: 600; color: #111827; margin-bottom: 6px; }
.pse-benefit-card p { font-size: 14px; color: #6b7280; line-height: 1.6; }

/* Plan */
.pse-plan { display: flex; justify-content: center; }
.pse-plan__box {
  background: linear-gradient(135deg, #f0f9f7 0%, #fff 100%);
  border: 2px solid #2F7785;
  border-radius: 20px;
  padding: 48px 52px;
  text-align: center;
  max-width: 420px;
  width: 100%;
  position: relative;
}
.pse-plan__badge {
  position: absolute;
  top: -14px;
  left: 50%;
  transform: translateX(-50%);
  background: #98C73A;
  color: #fff;
  font-size: 12px;
  font-weight: 600;
  padding: 4px 16px;
  border-radius: 999px;
}
.pse-plan__price { font-size: 72px; font-weight: 800; color: #2F7785; line-height: 1; }
.pse-plan__price span { font-size: 36px; }
.pse-plan__label { font-size: 15px; color: #6b7280; margin-bottom: 28px; }
.pse-plan__features { list-style: none; padding: 0; margin: 0 0 8px; text-align: left; }
.pse-plan__features li { font-size: 14px; color: #374151; padding: 8px 0; border-bottom: 1px solid #f3f4f6; }
.pse-plan__features li:last-child { border-bottom: none; }

/* CTA */
.pse-cta {
  background: linear-gradient(135deg, #225F6B 0%, #2F7785 100%);
  padding: 72px 0;
}
.pse-cta__inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 40px;
  flex-wrap: wrap;
}
.pse-cta__inner h2 { font-size: 28px; font-weight: 700; color: #fff; margin-bottom: 8px; }
.pse-cta__inner p { font-size: 15px; color: rgba(255,255,255,0.75); }
.pse-cta__btns { display: flex; gap: 12px; flex-wrap: wrap; }
.pse-btn-outline--white { color: #fff; border-color: rgba(255,255,255,0.5); }
.pse-btn-outline--white:hover { background: rgba(255,255,255,0.12); border-color: #fff; }
</style>
