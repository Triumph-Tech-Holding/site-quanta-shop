import { readFile, writeFile } from 'node:fs/promises';
import { join } from 'node:path';

const DATA_FILE = join(process.cwd(), 'server/data/home-cms.json');

export default defineEventHandler(async (event) => {
  if (event.method === 'GET') {
    try {
      const content = await readFile(DATA_FILE, 'utf-8');
      return JSON.parse(content);
    } catch {
      throw createError({ statusCode: 404, message: 'home-cms.json não encontrado' });
    }
  }

  if (event.method === 'PUT') {
    if (!event.context.user?.admin) {
      throw createError({ statusCode: 403, message: 'Acesso negado: requer perfil admin' });
    }

    const body = await readBody(event);
    if (!body || typeof body !== 'object' || Array.isArray(body)) {
      throw createError({ statusCode: 400, message: 'Dados inválidos: esperado objeto JSON' });
    }

    await writeFile(DATA_FILE, JSON.stringify(body, null, 2), 'utf-8');
    return { ok: true };
  }

  throw createError({ statusCode: 405, message: 'Método não permitido' });
});
