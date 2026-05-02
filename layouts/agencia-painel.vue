<template>
  <div id="app" :class="{ flexivel: true }">

    <div class="box-header-panel">
      <div class="flex-header">
        <div class="logo">
          <NuxtLink to="/">
            <img src="/agencia/imgs/quanta-shop-branca.png" alt="Quanta Shop" />
          </NuxtLink>
        </div>

        <div class="bar-info-account">
          <div class="box-acesso">
            <div class="flex-acesso" v-if="isComerciante && user?.perfil == 'C'">
              <span class="txt-acesso">Acessando como <b>Credenciado</b></span>
            </div>
            <div class="flex-acesso" v-else>
              <span class="txt-acesso">Acessando como <b>Empreendedor</b></span>
            </div>
            <div v-if="isAcessoRemoto">
              <span class="acesso-remoto-badge">Acesso remoto</span>
            </div>
          </div>

          <div :class="user?.assinaturaHabilitada ? 'info-account-subscriber' : 'info-account'">
            <div class="box-user">
              <div class="about-user">
                <div class="d-flex" style="flex-direction:row;align-items:center;gap:12px;">
                  <div v-if="user">
                    <span
                      class="b-avatar rounded-circle"
                      style="width:34px;height:34px;background:#ccc;display:flex;align-items:center;justify-content:center;font-weight:700;color:#225f6b;font-size:14px;"
                    >
                      {{ user.username?.charAt(0)?.toUpperCase() || 'U' }}
                    </span>
                  </div>
                  <div class="d-flex" v-if="user">
                    <div class="name-user">{{ user.username }}</div>
                    <div class="saldo-user">{{ saldoFormatado }}</div>
                  </div>
                </div>
              </div>

              <div class="flex-remot">
                <div class="box-button">
                  <div class="bt-default">
                    <a href="https://quantashop.com.br/assistente-virtual" target="_blank">
                      <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" fill="currentColor" class="mr-1" viewBox="0 0 24 24"><path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm-2 12H6v-2h12v2zm0-3H6V9h12v2zm0-3H6V6h12v2z"/></svg>
                      Assistente virtual
                    </a>
                  </div>
                  <div class="bt-default bt-border">
                    <NuxtLink to="/agencia/painel/meus-dados">
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
        <div class="p-0 box-menu-lateral">
          <AgenciaMenu />
        </div>
        <div class="box-conteudo conteudo-logado">
          <div style="padding:0 1rem;">
            <div class="referral-link" style="padding:10px 0;">
              <div class="global-box" style="background:#fff;border-radius:8px;padding:.6rem 1rem;box-shadow:0 1px 4px rgba(0,0,0,.08);">
                <small>Link de indicação da rede</small>
                <div class="flex-referral" style="display:flex;gap:.5rem;align-items:center;margin-top:.3rem;">
                  <input type="text" :value="linkIndicacao" disabled class="form-control" style="font-size:.8rem;flex:1;border:1px solid #d8d8d8;border-radius:6px;padding:.4rem .75rem;background:#f8f9fa;" />
                  <div class="bt-default" style="margin-left:.5rem;">
                    <button @click="copiarLink">{{ copiado ? '✓ Copiado!' : 'Copiar link' }}</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <slot />
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { useAgenciaStore } from '~/composables/useAgenciaStore';
import { useApi } from '~/composables/useApi';
const agenciaStore = useAgenciaStore();
const api = useApi();

const user = computed(() => agenciaStore.dadosUser);
const isAcessoRemoto = computed(() => agenciaStore.isAcessoRemoto);
const isComerciante = computed(() => agenciaStore.isComerciante);
const saldo = ref<number | null>(null);
const copiado = ref(false);

const saldoFormatado = computed(() => {
  if (saldo.value === null) return '';
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(saldo.value);
});

const linkIndicacao = computed(() => {
  const login = user.value?.login || '';
  return `https://quantashop.com.br/register/${login}`;
});

async function copiarLink() {
  try {
    await navigator.clipboard.writeText(linkIndicacao.value);
    copiado.value = true;
    setTimeout(() => { copiado.value = false; }, 2000);
  } catch { /**/ }
}

async function loadSaldo() {
  try {
    const token = agenciaStore.getToken();
    const { data } = await api.get<number | { saldo: number }>('/financeiro/saldo', {
      headers: { Authorization: `Bearer ${token}` },
    });
    saldo.value = typeof data === 'number' ? data : (data?.saldo ?? null);
  } catch { /**/ }
}

onMounted(() => {
  agenciaStore.loadFromStorage();
  loadSaldo();
});
</script>
