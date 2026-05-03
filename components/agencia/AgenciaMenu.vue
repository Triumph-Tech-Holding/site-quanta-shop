<template>
  <div class="bg-menu">
    <div class="navbar-nav-vertical" id="nav-collapse">

      <a href="https://quantashop.com.br" class="listMenu" target="_blank" rel="noopener">
        Lojas
      </a>

      <NuxtLink :to="dashboardRoute" class="listMenu">
        Painel Geral
      </NuxtLink>

      <NuxtLink to="/agencia/painel/planos" class="listMenu">
        Planos
      </NuxtLink>

      <NuxtLink to="/agencia/painel/inserir-cupom" class="listMenu">
        Registrar Compra
      </NuxtLink>

      <NuxtLink to="/agencia/painel/minhas-compras" class="listMenu">
        Minhas Compras
      </NuxtLink>

      <NuxtLink to="/agencia/painel/assinatura" class="listMenu">
        Assine Quanta Plus
      </NuxtLink>

      <NuxtLink to="/agencia/painel/financeiro" class="listMenu">
        Financeiro
      </NuxtLink>

      <a href="https://quantashop.com.br/redi" class="listMenu" target="_blank" rel="noopener">
        Redi
      </a>

      <NuxtLink to="/agencia/links" class="listMenu">
        Meu Link de Indicação
      </NuxtLink>

      <NuxtLink to="/agencia/painel/social-commerce" class="listMenu">
        Social Commerce
      </NuxtLink>

      <NuxtLink to="/agencia/painel/dashboard-rede-adf" class="listMenu">
        Dashboard de Rede (ADF)
      </NuxtLink>

      <NuxtLink to="/agencia/painel/performance" class="listMenu">
        Performance
      </NuxtLink>

      <NuxtLink to="/agencia/painel/suporte" class="listMenu">
        Suporte
      </NuxtLink>

      <NuxtLink to="/agencia/painel/tutoriais-usuario" class="listMenu">
        Tutoriais
      </NuxtLink>

      <NuxtLink to="/agencia/painel/material-apoio" class="listMenu">
        Material de Apoio
      </NuxtLink>

      <NuxtLink to="/agencia/painel/faq" class="listMenu">
        FAQ
      </NuxtLink>

      <template v-if="isAdmin">
        <div class="menu-admin-divider">Admin</div>
        <NuxtLink to="/agencia/painel/admin" class="listMenu listMenu--admin">
          Painel Admin
        </NuxtLink>
        <NuxtLink to="/agencia/painel/admin/progresso" class="listMenu listMenu--admin">
          🚀 Progresso
        </NuxtLink>
        <NuxtLink to="/agencia/painel/admin/features" class="listMenu listMenu--admin">
          🎯 Features &amp; MVP
        </NuxtLink>
        <NuxtLink to="/agencia/painel/admin/configuracoes-rede" class="listMenu listMenu--admin">
          ⚙️ Configurações de Rede
        </NuxtLink>
        <NuxtLink to="/agencia/painel/admin/bi-financeiro" class="listMenu listMenu--admin">
          📈 BI Financeiro
        </NuxtLink>
        <NuxtLink to="/agencia/painel/admin/docs" class="listMenu listMenu--admin">
          📋 Documentação
        </NuxtLink>
        <div class="menu-admin-divider">LAB</div>
        <NuxtLink to="/lab" class="listMenu listMenu--admin">
          🧪 LAB · Cockpit Técnico
        </NuxtLink>
      </template>

      <NuxtLink to="/agencia/logout" class="listMenu logout-item">
        Logout
      </NuxtLink>

    </div>
  </div>
</template>

<script setup lang="ts">
import { useAgenciaStore } from '~/pinia/useAgenciaStore';
defineProps<{ open?: boolean }>();

const agenciaStore = useAgenciaStore();

const isAdmin = computed(() => agenciaStore.isAdmin);
const isComerciante = computed(() => agenciaStore.isComerciante);

const dashboardRoute = computed(() => {
  if (isAdmin.value) return '/agencia/painel/admin';
  if (isComerciante.value) return '/agencia/painel/comerciante';
  return '/agencia/painel';
});

onMounted(() => {
  agenciaStore.loadFromStorage();
});
</script>
