<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Legal" title="Política de Privacidade" description="Última atualização: Janeiro de 2025" />

    <div class="qs-card-section priv-card">
      <p>A Quanta Shop respeita sua privacidade e está comprometida com a proteção dos seus dados pessoais, em conformidade com a Lei Geral de Proteção de Dados (LGPD – Lei 13.709/2018).</p>

      <h3 class="qs-section-title priv-h3">1. Dados coletados</h3>
      <p>Coletamos dados necessários para o funcionamento da plataforma: nome, CPF, e-mail, telefone, dados bancários para saque e dados de navegação para melhoria dos serviços.</p>

      <h3 class="qs-section-title priv-h3">2. Uso dos dados</h3>
      <p>Seus dados são utilizados para: criação e gerenciamento de conta, processamento de transações de cashback, comunicações sobre sua conta e serviços, e cumprimento de obrigações legais.</p>

      <h3 class="qs-section-title priv-h3">3. Compartilhamento</h3>
      <p>Não compartilhamos seus dados com terceiros para fins comerciais. Dados podem ser compartilhados com parceiros de pagamento estritamente para processamento de transações.</p>

      <h3 class="qs-section-title priv-h3">4. Seus direitos</h3>
      <p>Você tem direito a acessar, corrigir, excluir e portar seus dados. Para exercer esses direitos, entre em contato pelo suporte da plataforma.</p>

      <h3 class="qs-section-title priv-h3">5. Contato</h3>
      <p>Dúvidas sobre privacidade: <a href="mailto:privacidade@quantashop.com.br" class="priv-link">privacidade@quantashop.com.br</a></p>
    </div>

    <div class="priv-cta">
      <NuxtLink to="/agencia" class="qs-btn-outline">← Voltar</NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia' });
</script>

<style scoped>
.priv-card p { color: var(--qs-gray-600); line-height: 1.7; margin-bottom: 1rem; }
.priv-card p:last-child { margin-bottom: 0; }
.priv-h3 { margin-top: 1.5rem; margin-bottom: .5rem; font-size: 1rem; }
.priv-link { color: var(--qs-teal); text-decoration: none; }
.priv-link:hover { text-decoration: underline; }
.priv-cta { display: flex; justify-content: center; padding-top: .5rem; }
</style>
