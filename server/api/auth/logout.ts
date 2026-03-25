import { defineEventHandler, deleteCookie } from 'h3';

export default defineEventHandler((event) => {
  const cookieOptions = { path: '/', httpOnly: true, sameSite: 'lax' } as const;

  deleteCookie(event, 'agencia_token', cookieOptions);
  deleteCookie(event, 'auth_token', cookieOptions);

  return {
    success: true,
    message: 'Logout realizado com sucesso.',
  };
});
