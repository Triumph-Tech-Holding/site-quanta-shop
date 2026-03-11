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
            @click="redirectToOldDomain()"
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
          @click="redirectToOldDomain()"
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

const redirectToOldDomain = () => {
  if (userStore.token) {
    window.location.href = `https://agencia.quantashop.com.br/auth/?token=${userStore.token}`;
  } else {
    console.error("Usuário não encontrado no localStorage.");
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
