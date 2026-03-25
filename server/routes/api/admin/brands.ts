import { defineEventHandler, readBody, sendError } from 'h3';
import fs from 'fs/promises';
import path from 'path';

const BRANDS_FILE = path.join(process.cwd(), 'public', 'data', 'brands.json');

export default defineEventHandler(async (event) => {
  // Verificar autenticação (middleware já faz isso, mas vamos garantir)
  if (!event.context.user?.admin) {
    return sendError(event, createError({ statusCode: 403, message: 'Acesso negado' }));
  }

  if (event.node.req.method === 'GET') {
    try {
      const data = await fs.readFile(BRANDS_FILE, 'utf-8');
      return JSON.parse(data);
    } catch (error) {
      console.error('[API] Erro ao ler brands.json:', error);
      return sendError(event, createError({ statusCode: 500, message: 'Erro ao ler marcas' }));
    }
  }

  if (event.node.req.method === 'POST') {
    try {
      const body = await readBody(event);
      if (!Array.isArray(body)) {
        return sendError(event, createError({ statusCode: 400, message: 'Dados inválidos' }));
      }

      await fs.writeFile(BRANDS_FILE, JSON.stringify(body, null, 2), 'utf-8');
      return { success: true, message: 'Marcas salvas com sucesso' };
    } catch (error) {
      console.error('[API] Erro ao salvar brands.json:', error);
      return sendError(event, createError({ statusCode: 500, message: 'Erro ao salvar marcas' }));
    }
  }

  return sendError(event, createError({ statusCode: 405, message: 'Método não permitido' }));
});
