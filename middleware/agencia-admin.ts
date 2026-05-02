import { useAgenciaStore } from '~/composables/useAgenciaStore';

export default defineNuxtRouteMiddleware((to) => {
  // Middleware roda somente no cliente (Pinia + localStorage)
  if (import.meta.server) return;

  if (!to.path.startsWith('/agencia/painel/admin')) return;

  const store = useAgenciaStore();
  store.loadFromStorage();

  // Token deve existir e ser válido
  if (!store.checkTokenExpiry()) {
    return navigateTo('/agencia/login');
  }

  // Usuário deve ter flag de admin
  if (!store.isAdmin) {
    return navigateTo('/agencia/no-permission');
  }
});
