<template>
  <div class="agencia-login-page" style="align-items:flex-start; padding: 2rem 1rem; background:#ecf2f7; min-height:100vh">
    <div class="login-box" style="max-width:680px; margin:2rem auto">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h2 class="mt-4 mb-3" style="color:#2f7785">Quem Somos</h2>
      <p>A <strong>Quanta Shop</strong> é uma plataforma inovadora de cashback que conecta lojistas e consumidores, recompensando cada compra realizada na nossa rede credenciada.</p>
      <p>Nossa missão é democratizar o acesso ao cashback, permitindo que qualquer estabelecimento possa oferecer recompensas reais para seus clientes, fidelizando-os e aumentando as vendas.</p>
      <h5 class="mt-4" style="color:#2f7785">Nossa Visão</h5>
      <p>Ser a maior plataforma de cashback do Brasil, promovendo um ecossistema financeiro justo e vantajoso para lojistas, consumidores e empreendedores parceiros.</p>
      <h5 class="mt-4" style="color:#2f7785">Nossos Valores</h5>
      <ul>
        <li>Transparência em cada transação</li>
        <li>Comprometimento com o sucesso dos parceiros</li>
        <li>Inovação constante</li>
        <li>Respeito ao consumidor</li>
      </ul>
      <div class="text-center mt-4">
        <NuxtLink to="/agencia" class="btn btn-ag-primary">Acessar plataforma</NuxtLink>
        <NuxtLink to="/agencia/como-funciona" class="btn btn-ag-outline ms-2">Como funciona</NuxtLink>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
definePageMeta({ layout: 'agencia' });
</script>
