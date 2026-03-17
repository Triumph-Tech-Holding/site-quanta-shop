export default defineNuxtRouteMiddleware((to) => {
  if (import.meta.server) return;

  const isPainelRoute = to.path.startsWith('/agencia/painel');
  if (!isPainelRoute) return;

  const raw = localStorage.getItem('agencia_user');
  if (!raw) {
    return navigateTo('/agencia');
  }

  try {
    const user = JSON.parse(raw);
    if (!user?.token) return navigateTo('/agencia');

    const payload = JSON.parse(atob(user.token.split('.')[1]));
    if (payload.exp * 1000 < Date.now()) {
      localStorage.removeItem('agencia_user');
      return navigateTo('/agencia');
    }
  } catch {
    localStorage.removeItem('agencia_user');
    return navigateTo('/agencia');
  }
});
