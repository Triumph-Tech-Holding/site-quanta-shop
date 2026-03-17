export default defineNuxtRouteMiddleware((to) => {
  if (import.meta.server) return;

  const isAdminRoute = to.path.startsWith('/agencia/painel/admin');
  if (!isAdminRoute) return;

  const raw = localStorage.getItem('agencia_user');
  if (!raw) return navigateTo('/agencia');

  try {
    const user = JSON.parse(raw) as { token?: string; admin?: boolean };
    if (!user?.token) return navigateTo('/agencia');

    const payload = JSON.parse(atob(user.token.split('.')[1])) as { exp?: number };
    if ((payload.exp ?? 0) * 1000 < Date.now()) {
      localStorage.removeItem('agencia_user');
      return navigateTo('/agencia');
    }

    if (!user.admin) {
      return navigateTo('/agencia/no-permission');
    }
  } catch {
    return navigateTo('/agencia');
  }
});
