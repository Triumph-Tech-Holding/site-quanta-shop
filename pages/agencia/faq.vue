<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Ajuda" title="Perguntas Frequentes" description="Tire todas as suas dúvidas sobre a plataforma Quanta Shop." />

    <div class="qs-card-section ag-faq-card">
      <div v-for="(item, i) in faqs" :key="i" class="ag-faq-item">
        <button class="ag-faq-trigger" @click="toggle(i)">
          <span>{{ item.pergunta }}</span>
          <svg :style="{ transform: open[i] ? 'rotate(180deg)' : 'none', transition:'transform .2s' }" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" width="20" height="20"><path d="M7 10l5 5 5-5z"/></svg>
        </button>
        <div v-show="open[i]" class="ag-faq-body">{{ item.resposta }}</div>
      </div>
    </div>

    <div class="ag-faq-cta">
      <NuxtLink to="/agencia" class="qs-btn-primary">Acessar plataforma</NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia' });
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

<style scoped>
.ag-faq-card { padding: 0; overflow: hidden; }
.ag-faq-item { border-bottom: 1px solid var(--qs-border); }
.ag-faq-item:last-child { border-bottom: none; }
.ag-faq-trigger {
  width: 100%; display: flex; justify-content: space-between; align-items: center;
  gap: 1rem; padding: 1.1rem 1.5rem; background: none; border: none; cursor: pointer;
  font-weight: 600; font-size: .95rem; color: var(--qs-text); text-align: left;
}
.ag-faq-trigger:hover { background: var(--qs-bg); }
.ag-faq-body { padding: .75rem 1.5rem 1.25rem; font-size: .9rem; color: var(--qs-gray-500); line-height: 1.6; }
.ag-faq-cta { display: flex; justify-content: center; padding-top: .5rem; }
</style>
