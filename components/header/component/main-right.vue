<template>
  <div
    class="tp-header-main-right d-flex align-items-center justify-content-end"
  >
    <div class="tp-header-login d-none d-lg-block">
      <div class="d-flex align-items-center">
        <div v-if="!userStore.isLoggedIn" class="tp-header-login-icon">
          <span class="text-white">
            <SvgUser />            
          </span>
        </div>
        <div v-else class="profile__main-thumb">
          <img :src="userStore.user.urlImg" alt="">
        </div>
        <div v-if="!userStore.isLoggedIn" class="tp-header-login-content d-none d-xl-block">
          <span class="text-white"> 
            <nuxt-link href="/login">Entrar</nuxt-link>            
          </span>
          <nuxt-link href="/register"><h5 class="tp-header-login-title text-white">Ou cadastrar-se</h5></nuxt-link>
        </div>
        <div v-else class="tp-header-login-content d-none d-xl-block">
          <span class="text-white">Olá,             
            <span class="text-white">{{ userStore.user.username }}</span>
          </span>
          <div class="d-flex gap-1">
            <h5 @click="redirectToAgencia()" class="tp-header-login-title text-white cursor-pointer">Agência digital</h5>
            <div class="vr bg-white"></div>
            <h5 @click="userStore.logout()" class="tp-header-login-title text-white cursor-pointer">Sair</h5>                 
          </div>
        </div>
      </div>
    </div>
    <div class="tp-header-action d-flex align-items-center ml-50">
      <div class="tp-header-action-item d-lg-none">
        <button
          @click="utilsStore.handleOpenMobileMenu"
          type="button"
          class="tp-header-action-btn tp-offcanvas-open-btn"
        >
          <SvgUser class="text-white mr-5 rounded-circle" />
          <SvgMenuIcon class="text-white" />
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useCartStore } from "@/pinia/useCartStore";
import { useWishlistStore } from "@/pinia/useWishlistStore";
import { useUtilityStore } from "@/pinia/useUtilityStore";
import { useUserStore } from "@/pinia/useUserStore";
import { ref, onMounted } from "vue";

const cartStore = useCartStore();
const wishlistStore = useWishlistStore();
const utilsStore = useUtilityStore();
const userStore = useUserStore();
const user = computed(() => userStore.user);
const userLoaded = computed(() => userStore.isUserLoaded);

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

</script>
<style scoped>
  .profile__main-thumb img{
   width: 40px; 
   height: 40px;
  }
</style>
