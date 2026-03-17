import { defineStore } from 'pinia';

export interface AgenciaUser {
  login: string;
  username: string;
  token: string;
  refreshToken: string;
  perfil: string;
  admin: boolean;
  comerciante: boolean;
  preCadastro: boolean;
  termosDeAceite: boolean;
  idUsuario?: number;
  [key: string]: unknown;
}

const STORAGE_KEY = 'agencia_user';
const ADMIN_STORAGE_KEY = 'agencia_userAdmin';

export const useAgenciaStore = defineStore('agencia', () => {
  const user = ref<AgenciaUser | null>(null);
  const userAdmin = ref<AgenciaUser | null>(null);

  function loadFromStorage() {
    if (import.meta.client) {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (raw) {
        try { user.value = JSON.parse(raw); } catch { user.value = null; }
      }
      const rawAdmin = localStorage.getItem(ADMIN_STORAGE_KEY);
      if (rawAdmin) {
        try { userAdmin.value = JSON.parse(rawAdmin); } catch { userAdmin.value = null; }
      }
    }
  }

  function setUser(u: AgenciaUser) {
    user.value = u;
    if (import.meta.client) {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(u));
    }
  }

  function setUserAdmin(u: AgenciaUser) {
    userAdmin.value = u;
    if (import.meta.client) {
      localStorage.setItem(ADMIN_STORAGE_KEY, JSON.stringify(u));
    }
  }

  function logout() {
    user.value = null;
    userAdmin.value = null;
    if (import.meta.client) {
      localStorage.removeItem(STORAGE_KEY);
      localStorage.removeItem(ADMIN_STORAGE_KEY);
    }
  }

  const isLoggedIn = computed(() => !!user.value?.token);
  const isAdmin = computed(() => !!user.value?.admin);
  const isComerciante = computed(() => !!user.value?.comerciante && user.value?.perfil === 'C');
  const isAcessoRemoto = computed(() => !!userAdmin.value);
  const dadosUser = computed(() => user.value);

  function getToken(): string | null {
    return user.value?.token || null;
  }

  function isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp * 1000 < Date.now();
    } catch {
      return true;
    }
  }

  function checkTokenExpiry(): boolean {
    const token = getToken();
    if (!token) return false;
    if (isTokenExpired(token)) {
      logout();
      return false;
    }
    return true;
  }

  return {
    user,
    userAdmin,
    isLoggedIn,
    isAdmin,
    isComerciante,
    isAcessoRemoto,
    dadosUser,
    loadFromStorage,
    setUser,
    setUserAdmin,
    logout,
    getToken,
    checkTokenExpiry,
  };
});
