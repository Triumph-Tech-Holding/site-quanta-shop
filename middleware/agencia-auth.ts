export default defineNuxtRouteMiddleware((to) => {
  if (import.meta.server) return;

  const agenciaStore = useAgenciaStore();
  agenciaStore.loadFromStorage();

  // Proteger rotas do painel
  const isPainelRoute = to.path.startsWith('/agencia/painel');
  if (!isPainelRoute) return;

  // Verificar token
  if (!agenciaStore.getToken()) {
    return navigateTo('/agencia/login');
  }

  // Verificar expiração
  if (!agenciaStore.checkTokenExpiry()) {
    return navigateTo('/agencia/login');
  }

  // Proteger rotas admin
  if (to.path.startsWith('/agencia/painel/admin')) {
    if (!agenciaStore.isAdmin) {
      return navigateTo('/agencia/painel');
    }
  }
});
