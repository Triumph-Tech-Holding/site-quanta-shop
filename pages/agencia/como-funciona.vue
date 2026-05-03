<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Plataforma" title="Como Funciona" description="Em poucos passos você já começa a ganhar cashback com a Quanta Shop." />

    <div class="cf-steps">
      <div v-for="(step, i) in steps" :key="i" class="qs-card-section cf-step-card">
        <div class="cf-step-num">{{ i + 1 }}</div>
        <div class="cf-step-body">
          <div class="cf-step-title">{{ step.titulo }}</div>
          <div class="cf-step-desc">{{ step.descricao }}</div>
        </div>
      </div>
    </div>

    <div class="cf-cta">
      <NuxtLink to="/agencia/cadastro" class="qs-btn-primary">Quero me cadastrar</NuxtLink>
      <NuxtLink to="/agencia" class="qs-btn-outline">Já tenho conta</NuxtLink>
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

<style scoped>
.cf-steps { display: flex; flex-direction: column; gap: 1rem; }
.cf-step-card { display: flex; align-items: flex-start; gap: 1.25rem; }
.cf-step-num {
  flex-shrink: 0; width: 44px; height: 44px; border-radius: 50%;
  background: var(--qs-teal); color: #fff;
  display: flex; align-items: center; justify-content: center;
  font-weight: 700; font-size: 1.1rem;
}
.cf-step-title { font-weight: 600; font-size: .95rem; color: var(--qs-text); margin-bottom: .35rem; }
.cf-step-desc { font-size: .875rem; color: var(--qs-gray-500); line-height: 1.5; }
.cf-cta { display: flex; gap: .75rem; justify-content: center; flex-wrap: wrap; padding-top: .5rem; }
</style>
