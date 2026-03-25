import { defineEventHandler, readBody, sendError, createError } from 'h3';
import fs from 'fs/promises';
import path from 'path';

interface Brand {
  id: number;
  nome: string;
  logo: string;
  url: string;
  ativo: boolean;
  ordem: number;
}

const BRANDS_FILE = path.join(process.cwd(), 'public', 'data', 'brands.json');

export default defineEventHandler(async (event) => {
  if (!event.context.user?.admin) {
    return sendError(event, createError({ statusCode: 403, message: 'Acesso negado' }));
  }

  const method = event.node.req.method;

  if (method === 'GET') {
    try {
      const data = await fs.readFile(BRANDS_FILE, 'utf-8');
      return JSON.parse(data) as Brand[];
    } catch {
      return sendError(event, createError({ statusCode: 500, message: 'Erro ao ler marcas' }));
    }
  }

  if (method === 'POST') {
    try {
      const body = await readBody<unknown>(event);
      if (!Array.isArray(body)) {
        return sendError(event, createError({ statusCode: 400, message: 'Dados inválidos: esperado array' }));
      }

      const brands = body as Brand[];
      await fs.writeFile(BRANDS_FILE, JSON.stringify(brands, null, 2), 'utf-8');
      return { success: true, message: 'Marcas salvas com sucesso' };
    } catch {
      return sendError(event, createError({ statusCode: 500, message: 'Erro ao salvar marcas' }));
    }
  }

  return sendError(event, createError({ statusCode: 405, message: 'Método não permitido' }));
});
