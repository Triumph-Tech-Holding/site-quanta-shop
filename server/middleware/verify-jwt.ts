import jwt from 'jsonwebtoken';
import { defineEventHandler, getCookie, createError } from 'h3';

interface JwtPayload {
  id: string;
  email: string;
  admin?: boolean;
  perfil?: string;
  idUsuario?: number;
  iat?: number;
  exp?: number;
}

declare module 'h3' {
  interface H3EventContext {
    user?: JwtPayload;
  }
}

/**
 * Prefixos de rotas que nunca precisam de autenticação Nuxt (são públicas ou
 * delegam auth para a API .NET via api-proxy).
 */
const PUBLIC_PATH_PREFIXES = [
  '/agencia/login',
  '/agencia/cadastro',
  '/agencia/recuperar-senha',
  '/agencia/reset-password',
  '/agencia/confirm-email',
  '/api-proxy',         // proxy para .NET — a API externa autentica por conta própria
  '/api/auth/logout',   // endpoint de logout não requer token válido
  '/_nuxt',
  '/data',
  '/img',
] as const;

/**
 * Prefixos de rotas Nuxt internas que EXIGEM token válido.
 * Qualquer rota sob esses prefixos retorna 401 se o token estiver ausente ou inválido.
 */
const PROTECTED_PATH_PREFIXES = [
  '/api/admin',
] as const;

export default defineEventHandler(async (event) => {
  const url = event.node.req.url ?? '';

  const isPublicPath = PUBLIC_PATH_PREFIXES.some((prefix) => url.startsWith(prefix));
  if (isPublicPath) return;

  // Extrair token via cookie (SSR) ou header Authorization: Bearer (SPA)
  const cookieToken = getCookie(event, 'agencia_token') ?? getCookie(event, 'auth_token');
  const headerAuth = event.node.req.headers.authorization;
  const bearerToken = headerAuth?.startsWith('Bearer ') ? headerAuth.slice(7) : undefined;
  const token = cookieToken ?? bearerToken;

  const requiresAuth = PROTECTED_PATH_PREFIXES.some((prefix) => url.startsWith(prefix));

  if (!token) {
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

  try {
    const decoded = jwt.verify(token, jwtSecret) as JwtPayload;
    event.context.user = decoded;
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
