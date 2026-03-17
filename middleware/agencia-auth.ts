export default defineNuxtRouteMiddleware((to) => {
  if (import.meta.server) return;

  const isPainelRoute = to.path.startsWith('/agencia/painel');
  if (!isPainelRoute) return;

  const raw = localStorage.getItem('agencia_user');
  if (!raw) return navigateTo('/agencia/login');

  try {
    const user = JSON.parse(raw) as { token?: string; admin?: boolean };
    if (!user?.token) return navigateTo('/agencia/login');

    const payload = JSON.parse(atob(user.token.split('.')[1])) as { exp?: number };
    if ((payload.exp ?? 0) * 1000 < Date.now()) {
      localStorage.removeItem('agencia_user');
      return navigateTo('/agencia/login');
    }
  } catch {
    localStorage.removeItem('agencia_user');
    return navigateTo('/agencia/login');
  }
});
