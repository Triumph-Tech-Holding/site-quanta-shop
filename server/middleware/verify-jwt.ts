import jwt from 'jsonwebtoken';
import { defineEventHandler, getCookie, createError } from 'h3';

const DOTNET_ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

interface JwtPayload {
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'?: string;
  unique_name?: string;
  role?: string;
  jti?: string;
  iat?: number;
  exp?: number;
  [key: string]: unknown;
}

declare module 'h3' {
  interface H3EventContext {
    user?: JwtPayload & { admin: boolean };
  }
}

const PUBLIC_PATH_PREFIXES = [
  '/agencia/login',
  '/agencia/cadastro',
  '/agencia/recuperar-senha',
  '/agencia/reset-password',
  '/agencia/confirm-email',
  '/api-proxy',
  '/api/auth/logout',
  '/_nuxt',
  '/data',
  '/img',
  '/uploads',
] as const;

const PROTECTED_PATH_PREFIXES = [
  '/api/admin',
] as const;

export default defineEventHandler(async (event) => {
  const url = event.node.req.url ?? '';

  const isPublicPath = PUBLIC_PATH_PREFIXES.some((prefix) => url.startsWith(prefix));
  if (isPublicPath) return;

  const cookieToken = getCookie(event, 'agencia_token') ?? getCookie(event, 'auth_token');
  const headerAuth = event.node.req.headers.authorization;
  const bearerToken = headerAuth?.startsWith('Bearer ') ? headerAuth.slice(7) : undefined;
  const rawToken = bearerToken ?? cookieToken;

  const requiresAuth = PROTECTED_PATH_PREFIXES.some((prefix) => url.startsWith(prefix));

  if (!rawToken) {
    if (requiresAuth) {
      throw createError({ statusCode: 401, statusMessage: 'Não autorizado: token ausente.' });
    }
    return;
  }

  const jwtSecret = process.env.NUXT_JWT_SECRET;
  if (!jwtSecret) {
    throw createError({
      statusCode: 500,
      statusMessage: 'Configuração de segurança inválida: NUXT_JWT_SECRET não configurado.',
    });
  }

  const token = decodeURIComponent(rawToken);

  try {
    const decoded = jwt.verify(token, jwtSecret) as JwtPayload;

    const roleClaim = decoded[DOTNET_ROLE_CLAIM] ?? decoded['role'];
    const roleStr = typeof roleClaim === 'string' ? roleClaim.toLowerCase() : '';
    const isAdmin = roleStr === 'admin' || roleStr === 'administrador' || roleStr === 'administrator' || roleStr === 'gestor';

    event.context.user = { ...decoded, admin: isAdmin };
  } catch (err: unknown) {
    if (err instanceof Error) {
      if (err.name === 'TokenExpiredError') {
        throw createError({ statusCode: 401, statusMessage: 'Não autorizado: token expirado.' });
      }
      if (err.name === 'JsonWebTokenError') {
        throw createError({ statusCode: 401, statusMessage: 'Não autorizado: token inválido.' });
      }
    }
    throw err;
  }
});
