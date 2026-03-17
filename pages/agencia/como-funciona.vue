<template>
  <div class="agencia-login-page" style="align-items:flex-start; padding: 2rem 1rem; background:#ecf2f7; min-height:100vh">
    <div class="login-box" style="max-width:680px; margin:2rem auto">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h2 class="mt-4 mb-3" style="color:#2f7785">Como Funciona</h2>
      <div class="row g-3 mb-4">
        <div class="col-12" v-for="(step, i) in steps" :key="i">
          <div class="ag-card d-flex align-items-start gap-3">
            <div style="min-width:40px; height:40px; border-radius:50%; background:#2f7785; color:#fff; display:flex; align-items:center; justify-content:center; font-weight:bold; font-size:1.1rem">{{ i + 1 }}</div>
            <div>
              <div class="fw-bold mb-1">{{ step.titulo }}</div>
              <div class="text-muted" style="font-size:.9rem">{{ step.descricao }}</div>
            </div>
          </div>
        </div>
      </div>
      <div class="text-center mt-2">
        <NuxtLink to="/agencia/cadastro" class="btn btn-ag-primary">Quero me cadastrar</NuxtLink>
        <NuxtLink to="/agencia" class="btn btn-ag-outline ms-2">Já tenho conta</NuxtLink>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
definePageMeta({ layout: 'agencia' });
const steps = [
  { titulo: 'Cadastre-se gratuitamente', descricao: 'Crie sua conta em menos de 2 minutos e acesse o painel completo de empreendedor Quanta Shop.' },
  { titulo: 'Indique lojas e pessoas', descricao: 'Compartilhe seu link de indicação e ganhe comissões quando seus indicados realizarem compras.' },
  { titulo: 'Clientes compram e acumulam cashback', descricao: 'Cada compra realizada em lojas credenciadas gera cashback automático para o consumidor.' },
  { titulo: 'Resgate seu saldo', descricao: 'Solicite o saque do seu saldo acumulado diretamente para sua conta bancária, com processamento rápido.' },
];
</script>
