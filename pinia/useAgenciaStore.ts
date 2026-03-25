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

  function loadFromStorage(): void {
    if (!import.meta.client) return;

    const raw = localStorage.getItem(STORAGE_KEY);
    if (raw) {
      try { user.value = JSON.parse(raw) as AgenciaUser; } catch { user.value = null; }
    }

    const rawAdmin = localStorage.getItem(ADMIN_STORAGE_KEY);
    if (rawAdmin) {
      try { userAdmin.value = JSON.parse(rawAdmin) as AgenciaUser; } catch { userAdmin.value = null; }
    }
  }

  function setUser(u: AgenciaUser): void {
    user.value = u;
    if (import.meta.client) {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(u));
    }
  }

  function setUserAdmin(u: AgenciaUser): void {
    userAdmin.value = u;
    if (import.meta.client) {
      localStorage.setItem(ADMIN_STORAGE_KEY, JSON.stringify(u));
    }
  }

  function logout(): void {
    user.value = null;
    userAdmin.value = null;
    if (import.meta.client) {
      localStorage.removeItem(STORAGE_KEY);
      localStorage.removeItem(ADMIN_STORAGE_KEY);
    }
  }

  // ── Getters ────────────────────────────────────────────────────────────────
  const isLoggedIn = computed(() => !!user.value?.token);
  const isAdmin = computed(() => !!user.value?.admin);
  const isComerciante = computed(() => !!user.value?.comerciante && user.value?.perfil === 'C');
  const isAcessoRemoto = computed(() => !!userAdmin.value);
  const dadosUser = computed(() => user.value);
  const currentToken = computed(() => user.value?.token ?? null);

  function getToken(): string | null {
    return user.value?.token ?? null;
  }

  // ── JWT helpers ────────────────────────────────────────────────────────────

  function isTokenExpired(token: string): boolean {
    try {
      const b64url = token.split('.')[1];
      if (!b64url) return true;
      const b64 = b64url.replace(/-/g, '+').replace(/_/g, '/');
      const padded = b64 + '='.repeat((4 - (b64.length % 4)) % 4);
      const payload = JSON.parse(atob(padded)) as { exp?: number };
      return (payload.exp ?? 0) * 1000 < Date.now();
    } catch {
      return true;
    }
  }

  /**
   * Verifica se o token armazenado é válido e não expirado.
   * Faz logout automático se expirado.
   * @returns `true` se o token existe e é válido; `false` caso contrário.
   */
  function checkTokenExpiry(): boolean {
    const token = getToken();
    if (!token) return false;
    if (isTokenExpired(token)) {
      logout();
      return false;
    }
    return true;
  }

  /** Computed que retorna `true` se o usuário está logado com token válido e não expirado. */
  const isTokenValid = computed(() => {
    const token = currentToken.value;
    if (!token) return false;
    return !isTokenExpired(token);
  });

  return {
    user,
    userAdmin,
    isLoggedIn,
    isAdmin,
    isComerciante,
    isAcessoRemoto,
    dadosUser,
    currentToken,
    isTokenValid,
    loadFromStorage,
    setUser,
    setUserAdmin,
    logout,
    getToken,
    checkTokenExpiry,
  };
});
