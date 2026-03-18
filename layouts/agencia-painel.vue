<template>
  <div id="app" :class="{ flexivel: true }">

    <div class="box-header-panel">
      <div class="flex-header">
        <div class="logo">
          <NuxtLink :to="user?.admin ? '/agencia/painel/admin' : '/agencia/painel'">
            <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" />
          </NuxtLink>
        </div>

        <div class="bar-info-account">
          <div class="box-acesso">
            <div>
              <div class="flex-acesso" v-if="isComerciante && user?.perfil == 'C'">
                <span class="txt-acesso">Acessando como <b>Credenciado</b></span>
              </div>
              <div class="flex-acesso" v-else>
                <span class="txt-acesso">Acessando como <b>Empreendedor</b></span>
              </div>
            </div>

            <div class="flex-notification d-none d-md-flex" v-if="isAcessoRemoto">
              <span class="acesso-remoto-badge">Acesso remoto</span>
            </div>
          </div>

          <div :class="user?.assinaturaHabilitada ? 'info-account-subscriber' : 'info-account'">
            <div class="box-user">
              <div class="about-user">
                <div class="d-flex flex-row align-items-center">
                  <div class="avatar-wrapper mr-3" v-if="user">
                    <img
                      v-if="user.urlImg"
                      :src="user.urlImg"
                      class="b-avatar rounded-circle"
                      style="width:34px;height:34px;object-fit:cover;"
                      alt="avatar"
                    />
                    <span
                      v-else
                      class="b-avatar rounded-circle"
                      style="width:34px;height:34px;background:#ccc;display:flex;align-items:center;justify-content:center;font-weight:700;color:#225f6b;"
                    >
                      {{ user.username?.charAt(0)?.toUpperCase() || 'U' }}
                    </span>
                  </div>

                  <div class="d-flex" v-if="user">
                    <div class="name-user">{{ user.username }}</div>
                    <div class="saldo-user">{{ saldoFormatado }}</div>
                  </div>
                </div>

                <button class="menu-mobile-log btn-hamburger-panel d-flex d-md-none" @click="sidebarOpen = !sidebarOpen" aria-label="Menu">
                  <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"/>
                  </svg>
                </button>
              </div>

              <div class="flex-remot">
                <div class="box-button">
                  <div class="bt-default bt-border">
                    <NuxtLink :to="isComerciante && user?.perfil == 'C' ? '/agencia/painel/comerciante/dados-credenciamento' : '/agencia/painel/meus-dados'">
                      <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" fill="currentColor" class="mr-1" viewBox="0 0 24 24"><path d="M12 12c2.7 0 4.8-2.1 4.8-4.8S14.7 2.4 12 2.4 7.2 4.5 7.2 7.2 9.3 12 12 12zm0 2.4c-3.2 0-9.6 1.6-9.6 4.8v2.4h19.2v-2.4c0-3.2-6.4-4.8-9.6-4.8z"/></svg>
                      Meus dados
                    </NuxtLink>
                  </div>

                  <div class="bt-default bt-border" v-if="!isAcessoRemoto">
                    <NuxtLink to="/agencia/logout">
                      <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" fill="currentColor" class="mr-1" viewBox="0 0 24 24"><path d="M17 7l-1.41 1.41L18.17 11H8v2h10.17l-2.58 2.58L17 17l5-5zM4 5h8V3H4c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h8v-2H4V5z"/></svg>
                      Logout
                    </NuxtLink>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <section class="conteudo-geral-admin">
      <div class="flex-geral">
        <div class="p-0 box-menu-lateral" :class="{ 'sidebar-open': sidebarOpen }">
          <AgenciaMenu :is-open="sidebarOpen" @close="sidebarOpen = false" />
        </div>

        <div v-if="sidebarOpen" class="ag-sidebar-overlay" @click="sidebarOpen = false" />

        <div class="box-conteudo conteudo-logado">
          <slot />
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
const agenciaStore = useAgenciaStore();
const api = useApi();

const sidebarOpen = ref(false);
const user = computed(() => agenciaStore.dadosUser);
const isAcessoRemoto = computed(() => agenciaStore.isAcessoRemoto);
const isComerciante = computed(() => agenciaStore.isComerciante);
const saldo = ref<number | null>(null);

const saldoFormatado = computed(() => {
  if (saldo.value === null) return '';
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(saldo.value);
});

async function loadSaldo() {
  try {
    const token = agenciaStore.getToken();
    const { data } = await api.get<number | { saldo: number }>('/financeiro/saldo', {
      headers: { Authorization: `Bearer ${token}` },
    });
    saldo.value = typeof data === 'number' ? data : (data?.saldo ?? null);
  } catch {
    // silently fail
  }
}

onMounted(() => {
  agenciaStore.loadFromStorage();
  loadSaldo();
});
</script>
