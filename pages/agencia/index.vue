<template>
  <div>
    <section class="ag-hero py-5">
      <div class="container text-center py-4">
        <h1 class="display-5 fw-bold mb-3" style="color:#2f7785">
          Ganhe cashback em<br/><span style="color:#98c73a">cada compra</span>
        </h1>
        <p class="lead mb-4 text-muted" style="max-width:560px; margin:0 auto">
          A Quanta Shop é a plataforma que recompensa você e seus clientes em cada transação realizada na rede credenciada.
        </p>
        <div class="d-flex justify-content-center gap-3 flex-wrap">
          <NuxtLink to="/agencia/cadastro" class="btn btn-ag-primary px-4 py-2" style="font-size:1rem">Começar agora</NuxtLink>
          <NuxtLink to="/agencia/login" class="btn btn-ag-outline px-4 py-2" style="font-size:1rem">Já tenho conta</NuxtLink>
        </div>
      </div>
    </section>

    <section class="py-5 bg-white">
      <div class="container">
        <h2 class="text-center mb-5" style="color:#2f7785">Por que escolher a Quanta Shop?</h2>
        <div class="row g-4">
          <div class="col-12 col-md-4" v-for="(f, i) in features" :key="i">
            <div class="ag-card h-100 text-center p-4">
              <div class="mb-3" style="font-size:2.5rem">{{ f.icon }}</div>
              <h5 class="mb-2" style="color:#2f7785">{{ f.titulo }}</h5>
              <p class="text-muted mb-0" style="font-size:.9rem">{{ f.descricao }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <section class="py-5" style="background:#ecf2f7">
      <div class="container">
        <h2 class="text-center mb-5" style="color:#2f7785">Como funciona</h2>
        <div class="row g-4 align-items-center">
          <div class="col-12 col-md-6" v-for="(step, i) in steps" :key="i">
            <div class="d-flex gap-3">
              <div style="min-width:48px; height:48px; border-radius:50%; background:#2f7785; color:#fff; display:flex; align-items:center; justify-content:center; font-size:1.3rem; font-weight:bold; flex-shrink:0">{{ i+1 }}</div>
              <div>
                <div class="fw-bold mb-1" style="color:#2f7785">{{ step.titulo }}</div>
                <div class="text-muted" style="font-size:.9rem">{{ step.descricao }}</div>
              </div>
            </div>
          </div>
        </div>
        <div class="text-center mt-5">
          <NuxtLink to="/agencia/como-funciona" class="btn btn-ag-outline px-4">Saber mais</NuxtLink>
        </div>
      </div>
    </section>

    <section class="py-5 bg-white text-center">
      <div class="container">
        <h2 class="mb-3" style="color:#2f7785">Pronto para começar?</h2>
        <p class="text-muted mb-4">Cadastre-se gratuitamente e faça parte da maior rede de cashback do Brasil.</p>
        <NuxtLink to="/agencia/cadastro" class="btn btn-ag-primary px-5 py-2" style="font-size:1rem">Criar minha conta</NuxtLink>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia' });

const features = [
  { icon: '💰', titulo: 'Cashback real', descricao: 'Receba de volta uma parte do valor de cada compra realizada em lojas credenciadas.' },
  { icon: '🤝', titulo: 'Rede de indicações', descricao: 'Indique amigos e lojistas para ampliar seus ganhos com comissões em rede.' },
  { icon: '🏪', titulo: 'Lojas credenciadas', descricao: 'Centenas de estabelecimentos físicos e online aceitando a plataforma Quanta Shop.' },
];

const steps = [
  { titulo: 'Crie sua conta grátis', descricao: 'Cadastre-se em minutos com seus dados básicos.' },
  { titulo: 'Compre em lojas credenciadas', descricao: 'Use seu login Quanta Shop ao comprar nas lojas parceiras.' },
  { titulo: 'Acumule cashback', descricao: 'Cada compra gera cashback automático no seu saldo.' },
  { titulo: 'Resgate quando quiser', descricao: 'Solicite o saque do saldo acumulado para sua conta bancária.' },
];
</script>

<style scoped>
.ag-hero {
  background: linear-gradient(135deg, #ecf2f7 0%, #d4e8ed 100%);
  min-height: 60vh;
  display: flex;
  align-items: center;
}
</style>
