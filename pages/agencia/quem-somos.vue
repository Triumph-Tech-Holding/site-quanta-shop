<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Institucional" title="Quem Somos" description="Conheça a Quanta Shop e nossa missão de democratizar o cashback no Brasil." />

    <div class="qs-card-section qs-body-card">
      <p>A <strong>Quanta Shop</strong> é uma plataforma inovadora de cashback que conecta lojistas e consumidores, recompensando cada compra realizada na nossa rede credenciada.</p>
      <p>Nossa missão é democratizar o acesso ao cashback, permitindo que qualquer estabelecimento possa oferecer recompensas reais para seus clientes, fidelizando-os e aumentando as vendas.</p>

      <h3 class="qs-section-title" style="margin-top:1.5rem;">Nossa Visão</h3>
      <p>Ser a maior plataforma de cashback do Brasil, promovendo um ecossistema financeiro justo e vantajoso para lojistas, consumidores e empreendedores parceiros.</p>

      <h3 class="qs-section-title" style="margin-top:1.5rem;">Nossos Valores</h3>
      <ul class="qs-values-list">
        <li>Transparência em cada transação</li>
        <li>Comprometimento com o sucesso dos parceiros</li>
        <li>Inovação constante</li>
        <li>Respeito ao consumidor</li>
      </ul>
    </div>

    <div class="qs-body-cta">
      <NuxtLink to="/agencia" class="qs-btn-primary">Acessar plataforma</NuxtLink>
      <NuxtLink to="/agencia/como-funciona" class="qs-btn-outline">Como funciona</NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia' });
</script>

<style scoped>
.qs-body-card p { color: var(--qs-gray-600); line-height: 1.7; margin-bottom: 1rem; }
.qs-body-card p:last-child { margin-bottom: 0; }
.qs-values-list { color: var(--qs-gray-600); line-height: 2; padding-left: 1.25rem; }
.qs-body-cta { display: flex; gap: .75rem; justify-content: center; flex-wrap: wrap; padding-top: .5rem; }
</style>
