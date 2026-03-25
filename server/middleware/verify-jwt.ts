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
] as const;

export default defineEventHandler(async (event) => {
  const url = event.node.req.url ?? '';

  const isPublicPath = PUBLIC_PATH_PREFIXES.some((prefix) => url.startsWith(prefix));
  if (isPublicPath) return;

  // Aceitar token via cookie (SSR) ou header Authorization (SPA/API calls)
  const cookieToken = getCookie(event, 'agencia_token') ?? getCookie(event, 'auth_token');
  const headerAuth = event.node.req.headers.authorization;
  const bearerToken = headerAuth?.startsWith('Bearer ') ? headerAuth.slice(7) : undefined;
  const token = cookieToken ?? bearerToken;

  // Rotas de admin de dados exigem token; rotas públicas de leitura passam sem token
  const requiresAuth = url.startsWith('/api/admin');
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
    const isJwtError = err instanceof Error && (
      err.name === 'TokenExpiredError' || err.name === 'JsonWebTokenError'
    );

    if (err instanceof Error && err.name === 'TokenExpiredError') {
      throw createError({ statusCode: 401, statusMessage: 'Não autorizado: token expirado.' });
    }

    if (isJwtError) {
      throw createError({ statusCode: 401, statusMessage: 'Não autorizado: token inválido.' });
    }

    throw err;
  }
});
