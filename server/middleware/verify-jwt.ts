import jwt from 'jsonwebtoken';
import { defineEventHandler, getCookie, createError } from 'h3';

declare module 'h3' {
  interface H3EventContext {
    user?: {
      id: string;
      email: string;
      admin?: boolean;
      [key: string]: any;
    };
  }
}

export default defineEventHandler(async (event) => {
  const publicPaths = [
    '/agencia/login',
    '/agencia/cadastro',
    '/agencia/recuperar-senha',
    '/agencia/reset-password',
    '/agencia/confirm-email',
    '/api-proxy',
    '/auth/logout',
  ];

  const isPublicPath = publicPaths.some((path) =>
    event.node.req.url?.startsWith(path)
  );

  if (isPublicPath) {
    return;
  }

  const token =
    getCookie(event, 'agencia_token') ||
    getCookie(event, 'auth_token') ||
    event.node.req.headers.authorization?.replace('Bearer ', '');

  if (!token) {
    return;
  }

  try {
    const jwtSecret = process.env.NUXT_JWT_SECRET;

    if (!jwtSecret) {
      throw createError({
        statusCode: 500,
        statusMessage: 'Configuração de segurança inválida: NUXT_JWT_SECRET não está configurado.',
      });
    }

    const decoded = jwt.verify(token, jwtSecret) as {
      id: string;
      email: string;
      admin?: boolean;
      [key: string]: any;
    };

    event.context.user = decoded;
  } catch (error: any) {
    if (error.name === 'TokenExpiredError') {
      throw createError({
        statusCode: 401,
        statusMessage: 'Não autorizado: Token expirado.',
      });
    }

    if (error.name === 'JsonWebTokenError') {
      throw createError({
        statusCode: 401,
        statusMessage: 'Não autorizado: Token inválido.',
      });
    }

    throw error;
  }
});
