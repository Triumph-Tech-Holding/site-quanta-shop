<template>
  <div class="agencia-login-page" style="align-items:flex-start; padding:2rem 1rem; background:#ecf2f7; min-height:100vh">
    <div class="login-box" style="max-width:720px; margin:2rem auto">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h2 class="mt-4 mb-3" style="color:#2f7785">Perguntas Frequentes</h2>
      <div v-for="(item, i) in faqs" :key="i" class="mb-3">
        <button class="btn w-100 text-start d-flex justify-content-between align-items-center"
          style="background:#fff; border:1px solid #dee2e6; border-radius:8px; padding:.9rem 1.2rem; font-weight:600; color:#2f7785"
          @click="toggle(i)">
          <span>{{ item.pergunta }}</span>
          <svg :style="{ transform: open[i] ? 'rotate(180deg)' : 'none', transition:'transform .2s' }" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" width="20" height="20"><path d="M7 10l5 5 5-5z"/></svg>
        </button>
        <div v-show="open[i]" class="ag-card mt-1" style="border-radius:0 0 8px 8px; border-top:none; font-size:.9rem; color:#495057">
          {{ item.resposta }}
        </div>
      </div>
      <div class="text-center mt-4">
        <NuxtLink to="/agencia" class="btn btn-ag-primary">Acessar plataforma</NuxtLink>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
definePageMeta({ layout: 'agencia-login' });
const open = ref<Record<number, boolean>>({});
function toggle(i: number) { open.value[i] = !open.value[i]; }
const faqs = [
  { pergunta: 'O que é cashback?', resposta: 'Cashback é o retorno de uma parte do valor gasto em compras. Na Quanta Shop, cada compra realizada em lojas credenciadas gera um percentual de volta para o consumidor.' },
  { pergunta: 'Como me cadastro?', resposta: 'Clique em "Criar Conta", preencha seus dados, confirme seu e-mail e pronto! Sua conta estará ativa para usar a plataforma.' },
  { pergunta: 'Como funciona o saque?', resposta: 'Acesse o painel, vá em "Financeiro", solicite o saque do seu saldo disponível e informe seus dados bancários. O prazo de processamento é de até 5 dias úteis.' },
  { pergunta: 'Como indico outras pessoas?', resposta: 'Após fazer login, acesse "Minha Rede" para obter seu link de indicação. Compartilhe com amigos e ganhe comissões nas compras deles.' },
  { pergunta: 'Posso credenciar minha loja?', resposta: 'Sim! Acesse a área de Credenciamento e cadastre seu estabelecimento. Após aprovação, sua loja aparecerá na rede Quanta Shop.' },
  { pergunta: 'O cadastro tem custo?', resposta: 'O cadastro básico é gratuito. Existem planos premium com benefícios adicionais disponíveis na área de Planos e Assinatura.' },
  { pergunta: 'Esqueci minha senha, o que faço?', resposta: 'Na tela de login, clique em "Recuperar Senha", informe seu e-mail cadastrado e enviaremos um link para redefinição.' },
  { pergunta: 'Há um aplicativo mobile?', resposta: 'Sim! A Quanta Shop possui aplicativo para Android e iOS disponível nas respectivas lojas de aplicativos.' },
];
</script>
