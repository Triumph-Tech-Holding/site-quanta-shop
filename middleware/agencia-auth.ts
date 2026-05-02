import { useAgenciaStore } from '~/pinia/useAgenciaStore';

export default defineNuxtRouteMiddleware((to) => {
  // Middleware roda somente no cliente (Pinia + localStorage)
  if (import.meta.server) return;

  const store = useAgenciaStore();
  store.loadFromStorage();

  // Apenas rotas do painel são protegidas
  if (!to.path.startsWith('/agencia/painel')) return;

  // Verificar se o token existe e não expirou (logout automático em caso de expiração)
  if (!store.checkTokenExpiry()) {
    return navigateTo('/agencia/login');
  }

  // Rotas de admin exigem flag `admin`
  if (to.path.startsWith('/agencia/painel/admin') && !store.isAdmin) {
    return navigateTo('/agencia/painel');
  }
});
