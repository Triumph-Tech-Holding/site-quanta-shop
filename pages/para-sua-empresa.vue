<template>
  <div>

    <QsHero
      badge="Para Lojistas e Empresas"
      title="Aumente suas vendas com "
      title-accent="cashback real"
      subtitle="Ofereça recompensas automáticas em cada compra, fidelize seu público e atraia novos clientes sem custo fixo. Só paga quando vende."
      :cta-primary="{ text: 'Credenciar minha empresa →', href: '/agencia/credenciar' }"
      :cta-secondary="{ text: 'Como funciona', href: '#como-funciona' }"
      photo-url="https://images.unsplash.com/photo-1573497019940-1c28c88b4f3e?w=1400&q=80&auto=format&fit=crop"
      photo-alt="Empresária satisfeita usando a Quanta Shop para aumentar vendas"
      :floating-badges="[
        { icon: salesIcon, label: 'Vendas Este Mês', value: '+34%' },
        { icon: customersIcon, label: 'Clientes Fidelizados', value: '1.284' },
      ]"
      :stats="[
        { value: '+2.400', label: 'empresas credenciadas' },
        { value: 'R$ 4,2M', label: 'cashback distribuído' },
        { value: '98%', label: 'satisfação dos parceiros' },
      ]"
    />

    <!-- Como funciona -->
    <section class="qs-section" id="como-funciona">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Simples assim</span>
          <h2>Como funciona para sua empresa</h2>
          <p>Três passos para começar a oferecer cashback para seus clientes hoje mesmo</p>
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

    <!-- Benefícios -->
    <section class="qs-section" style="background: linear-gradient(135deg, #1a4a54 0%, #225F6B 40%, #2F7785 100%);">
      <div class="container">
        <div class="qs-section-header qs-section-header--white">
          <div class="qs-section-symbol">
            <img src="/img/logo/logo-symbol-white.png" alt="" aria-hidden="true" />
          </div>
          <span class="qs-label qs-label--white">Vantagens</span>
          <h2>Tudo que sua empresa precisa</h2>
          <p>Da captação de clientes ao relatório de vendas — tudo numa plataforma</p>
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

    <!-- Plano -->
    <section class="qs-section qs-section--gray">
      <div class="container">
        <div class="qs-section-header">
          <span class="qs-label">Simples e transparente</span>
          <h2>Sem mensalidade. Só paga quando vende.</h2>
          <p>Comissão de 2% sobre o valor transacionado. Sem surpresas, sem taxas escondidas.</p>
        </div>
        <div class="pse-plan-box">
          <div class="pse-plan-badge">Único modelo</div>
          <div class="pse-plan-price">2<span>%</span></div>
          <div class="pse-plan-label">por venda realizada</div>
          <ul class="pse-plan-features">
            <li><span class="pse-check">✓</span> Cadastro gratuito e imediato</li>
            <li><span class="pse-check">✓</span> Painel de gestão completo</li>
            <li><span class="pse-check">✓</span> Relatórios em tempo real</li>
            <li><span class="pse-check">✓</span> Suporte dedicado</li>
            <li><span class="pse-check">✓</span> Sem mensalidade ou taxa de adesão</li>
            <li><span class="pse-check">✓</span> Integração com maquininha e e-commerce</li>
          </ul>
          <NuxtLink href="/agencia/credenciar" class="qs-page-btn-lime" style="width:100%;justify-content:center;margin-top:8px;">Credenciar agora gratuitamente →</NuxtLink>
        </div>
      </div>
    </section>

    <!-- CTA final -->
    <section class="qs-cta-section">
      <div class="container qs-cta-inner">
        <div>
          <h2>Pronto para crescer com cashback?</h2>
          <p>Mais de 2.400 empresas já usam a Quanta Shop para fidelizar clientes e aumentar vendas.</p>
        </div>
        <div class="qs-cta-btns">
          <NuxtLink href="/agencia/credenciar" class="qs-page-btn-lime">Credenciar minha empresa</NuxtLink>
          <NuxtLink href="/contato" class="qs-page-btn-outline-white">Falar com consultor</NuxtLink>
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'layout-home' });
useSeoMeta({ 
  title: 'Para sua Empresa | Quanta Shop — Aumente suas vendas', 
  description: 'Ofereça cashback real aos seus clientes e aumente suas vendas com o Quanta Shop. Cadastro gratuito, fidelize clientes e só pague quando vender.', 
  ogTitle: 'Para sua Empresa | Quanta Shop — Aumente suas vendas',
  ogDescription: 'Ofereça cashback real aos seus clientes e aumente suas vendas com o Quanta Shop. Cadastro gratuito, fidelize clientes e só pague quando vender.',
  ogImage: '/logo.png'
});
useHead({
  link: [
    { rel: 'canonical', href: 'https://quantashop.com.br/para-sua-empresa' }
  ]
});

const salesIcon = `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><polyline points="23 6 13.5 15.5 8.5 10.5 1 18"/><polyline points="17 6 23 6 23 12"/></svg>`;
const customersIcon = `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>`;

const steps = [
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><line x1="19" y1="8" x2="19" y2="14"/><line x1="22" y1="11" x2="16" y2="11"/></svg>', title: 'Cadastre sua empresa', desc: 'Preencha o formulário em 2 minutos. Aprovação rápida, sem burocracia.' },
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><rect x="2" y="3" width="20" height="14" rx="2"/><line x1="8" y1="21" x2="16" y2="21"/><line x1="12" y1="17" x2="12" y2="21"/></svg>', title: 'Configure o cashback', desc: 'Defina o percentual de cashback para seus clientes. Você tem controle total.' },
  { icon: '<svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#2F7785" stroke-width="2"><polyline points="23 6 13.5 15.5 8.5 10.5 1 18"/><polyline points="17 6 23 6 23 12"/></svg>', title: 'Venda e fidelize', desc: 'Clientes ganham cashback automático em cada compra e voltam para gastar mais.' },
];

const benefits = [
  { icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>', title: 'Zero custo fixo', desc: 'Sem mensalidade ou taxa de adesão. Você só paga 2% sobre o que vender.' },
  { icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><line x1="18" y1="20" x2="18" y2="10"/><line x1="12" y1="20" x2="12" y2="4"/><line x1="6" y1="20" x2="6" y2="14"/></svg>', title: 'Relatórios completos', desc: 'Veja vendas, cashback gerado e clientes ativos em tempo real no seu painel.' },
  { icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>', title: 'Rede de clientes', desc: 'Seu estabelecimento aparece para todos os usuários da Quanta Shop na sua região.' },
  { icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><rect x="1" y="4" width="22" height="16" rx="2"/><line x1="1" y1="10" x2="23" y2="10"/></svg>', title: 'Maquininha integrada', desc: 'Integração com as principais maquininhas do mercado. Cashback automático na venda.' },
  { icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/></svg>', title: 'Cashback instantâneo', desc: 'O cashback é creditado na carteira do cliente imediatamente após a compra.' },
  { icon: '<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07A19.5 19.5 0 0 1 4.69 13a19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 3.6 2h3a2 2 0 0 1 2 1.72c.127.96.361 1.903.7 2.81a2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0 1 22 16.92z"/></svg>', title: 'Suporte dedicado', desc: 'Time de suporte disponível por WhatsApp para ajudar em qualquer dúvida.' },
];
</script>

<style scoped>
.pse-plan-box {
  max-width: 440px;
  margin: 0 auto;
  background: #fff;
  border: 2px solid #eef0f2;
  border-radius: 24px;
  padding: 44px 40px;
  text-align: center;
  box-shadow: 0 8px 40px rgba(0,0,0,0.08);
}
.pse-plan-badge {
  display: inline-block;
  background: rgba(47,119,133,0.1);
  color: #2F7785;
  font-size: 12px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  padding: 4px 14px;
  border-radius: 999px;
  margin-bottom: 20px;
}
.pse-plan-price {
  font-size: 80px;
  font-weight: 900;
  color: #0f1c23;
  line-height: 1;
  letter-spacing: -0.04em;
}
.pse-plan-price span { font-size: 36px; font-weight: 700; color: #2F7785; }
.pse-plan-label { font-size: 14px; color: #6b7280; margin-bottom: 28px; margin-top: 6px; }
.pse-plan-features {
  list-style: none;
  padding: 0;
  margin: 0 0 28px;
  text-align: left;
  display: flex;
  flex-direction: column;
  gap: 10px;
}
.pse-plan-features li {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 14px;
  color: #374151;
}
.pse-check { color: #98C73A; font-weight: 700; flex-shrink: 0; }
</style>
