import { defineEventHandler, deleteCookie } from 'h3';

export default defineEventHandler(async (event) => {
  deleteCookie(event, 'agencia_token', { path: '/' });
  deleteCookie(event, 'auth_token', { path: '/' });

  return {
    success: true,
    message: 'Logout realizado com sucesso.',
  };
});
