import { readFile, writeFile } from 'node:fs/promises';
import { join } from 'node:path';

const DATA_FILE = join(process.cwd(), 'server/data/home-cms.json');

export default defineEventHandler(async (event) => {
  if (event.method === 'GET') {
    try {
      const content = await readFile(DATA_FILE, 'utf-8');
      return JSON.parse(content);
    } catch {
      throw createError({ statusCode: 404, message: 'home-cms.json not found' });
    }
  }

  if (event.method === 'PUT') {
    const body = await readBody(event);
    await writeFile(DATA_FILE, JSON.stringify(body, null, 2), 'utf-8');
    return { ok: true };
  }

  throw createError({ statusCode: 405, message: 'Method Not Allowed' });
});
