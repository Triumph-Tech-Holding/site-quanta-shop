export default defineNuxtRouteMiddleware((to) => {
  if (import.meta.server) return;

  const isAdminRoute = to.path.startsWith('/agencia/painel/admin');
  if (!isAdminRoute) return;

  const agenciaStore = useAgenciaStore();
  agenciaStore.loadFromStorage();

  if (!agenciaStore.getToken()) {
    return navigateTo('/agencia/login');
  }

  if (!agenciaStore.checkTokenExpiry()) {
    return navigateTo('/agencia/login');
  }

  if (!agenciaStore.isAdmin) {
    return navigateTo('/agencia/no-permission');
  }
});
