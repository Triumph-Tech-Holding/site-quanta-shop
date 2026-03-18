<template>
  <div
    class="tp-header-top-menu d-flex align-items-center justify-content-center w-100 w-lg-75"
  >
    <div
      v-if="!userStore.isLoggedIn"
      class="d-flex my-2 gap-1 w-100 align-items-center justify-content-center tp-header-top-menu-user"
    >
      <nuxt-link
        href="/login"
        class="w-50 btn btn-outline-success rounded-0 bg-wa"
        style="border-color: #1e5d68; color: #1e5d68"
      >
        <h5 class="tp-header-login-title" style="color: #1e5d68">Entrar</h5>
      </nuxt-link>
      <nuxt-link
        href="/register"
        class="w-50 btn btn-outline-success rounded-0"
        style="border-color: #1e5d68; color: #1e5d68"
      >
        <h5 class="tp-header-login-title" style="color: #1e5d68">
          Cadastrar-se
        </h5>
      </nuxt-link>
    </div>

    <div
      v-if="userStore.isLoggedIn"
      class="d-none d-md-flex my-2 gap-2 w-100 align-items-center justify-content-end tp-header-top-menu-user"
    >
      <div>
        <span>Olá, {{ userStore.user.username }}</span>
      </div>
      <div class="d-flex align-items-center gap-2">
        <div class="px-3" style="border: 1px solid #1e5d68">
          <span
            @click="redirectToAgencia()"
            class="cursor-pointer"
            style="color: #1e5d68"
          >
            <strong>Agência digital</strong>
          </span>
        </div>
        <div>
          <span
            @click="userStore.logout()"
            class="cursor-pointer"
            style="color: #1e5d68"
          >
            Sair
          </span>
        </div>
      </div>
    </div>

    <div
      v-if="userStore.isLoggedIn"
      class="d-sm-flex d-md-none my-2 gap-2 w-100 align-items-center justify-content-end tp-header-top-menu-user border-bottom"
    >
      <div class="w-100 text-center">
        <span>Olá, {{ userStore.user.username }}</span>
      </div>
      <div class="w-100 my-2">
        <div
          class="w-100 text-center py-2 cursor-pointer"
          style="
            border: 1px solid #ffffff;
            background-color: #1e5d68;
            color: #ffffff;
          "
          @click="redirectToAgencia()"
        >
          Acessar minha agência digital
        </div>
      </div>
      <div class="w-100 my-2">
        <div
          @click="userStore.logout()"
          class="w-100 text-center cursor-pointer"
          style="
            border: 1px solid #1e5d68;
            background-color: #ffffff;
            color: #1e5d68;
          "
        >
          Sair
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { useUserStore } from "@/pinia/useUserStore";

const userStore = useUserStore();
const userLoaded = computed(() => userStore.isUserLoaded);
const user = computed(() => userStore.user);

const username = ref<string | null>(null);

onBeforeMount(() => {
  userStore.loadUserFromStorage();
});

function isTokenExpiredLocal(token: string): boolean {
  try {
    const b64url = token.split('.')[1];
    const b64 = b64url.replace(/-/g, '+').replace(/_/g, '/');
    const padded = b64 + '='.repeat((4 - (b64.length % 4)) % 4);
    const payload = JSON.parse(atob(padded));
    return (payload.exp ?? 0) * 1000 < Date.now();
  } catch {
    return true;
  }
}

const redirectToAgencia = () => {
  let agenciaRaw = localStorage.getItem('agencia_user');

  if (agenciaRaw) {
    try {
      const existing = JSON.parse(agenciaRaw);
      const isCompatible = existing?.comerciante === true || existing?.admin === true;
      if (!existing?.token || !isCompatible || isTokenExpiredLocal(existing.token)) {
        localStorage.removeItem('agencia_user');
        agenciaRaw = null;
      }
    } catch {
      localStorage.removeItem('agencia_user');
      agenciaRaw = null;
    }
  }

  if (!agenciaRaw) {
    const mainRaw = localStorage.getItem('user');
    if (mainRaw) {
      try {
        const parsed = JSON.parse(mainRaw);
        const isAgenciaCompatible = parsed?.comerciante === true || parsed?.admin === true;
        if (parsed?.token && isAgenciaCompatible && !isTokenExpiredLocal(parsed.token)) {
          localStorage.setItem('agencia_user', mainRaw);
          agenciaRaw = mainRaw;
        }
      } catch {}
    }
  }

  if (agenciaRaw) {
    window.location.href = '/agencia/painel';
  } else {
    window.location.href = '/agencia/login';
  }
};

let isActive = ref<string>("");
// handle active
const handleActive = (type: string) => {
  if (type === isActive.value) {
    isActive.value = "";
  } else {
    isActive.value = type;
  }
};
</script>
<style scoped>
.tp-header-top-menu-user {
  justify-content: space-between;
}
@media screen and(min-width: 992px) {
  .tp-header-top-menu-user {
    justify-content: flex-end;
  }
}
</style>
