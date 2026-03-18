function isValidToken(token: string): boolean {
  try {
    const b64url = token.split('.')[1];
    const b64 = b64url.replace(/-/g, '+').replace(/_/g, '/');
    const padded = b64 + '='.repeat((4 - (b64.length % 4)) % 4);
    const payload = JSON.parse(atob(padded)) as { exp?: number };
    return (payload.exp ?? 0) * 1000 > Date.now();
  } catch {
    return false;
  }
}

export default defineNuxtRouteMiddleware((to) => {
  if (import.meta.server) return;

  const isPainelRoute = to.path.startsWith('/agencia/painel');
  if (!isPainelRoute) return;

  let raw = localStorage.getItem('agencia_user');

  if (!raw) {
    const mainRaw = localStorage.getItem('user');
    if (mainRaw) {
      try {
        const mainUser = JSON.parse(mainRaw);
        if (mainUser?.token && isValidToken(mainUser.token)) {
          localStorage.setItem('agencia_user', mainRaw);
          raw = mainRaw;
        }
      } catch {}
    }
  }

  if (!raw) return navigateTo('/agencia/login');

  try {
    const user = JSON.parse(raw) as { token?: string };
    if (!user?.token || !isValidToken(user.token)) {
      localStorage.removeItem('agencia_user');
      return navigateTo('/agencia/login');
    }
  } catch {
    localStorage.removeItem('agencia_user');
    return navigateTo('/agencia/login');
  }
});
