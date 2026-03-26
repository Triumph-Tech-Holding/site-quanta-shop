import { defineEventHandler, readMultipartFormData, sendError, createError } from 'h3';
import { writeFile, mkdir } from 'fs/promises';
import { join, extname } from 'path';

const UPLOADS_DIR = join(process.cwd(), 'public', 'uploads', 'banners');

export default defineEventHandler(async (event) => {
  if (!event.context.user?.admin) {
    return sendError(event, createError({ statusCode: 403, message: 'Acesso negado' }));
  }

  const parts = await readMultipartFormData(event);
  if (!parts || parts.length === 0) {
    return sendError(event, createError({ statusCode: 400, message: 'Nenhum arquivo enviado' }));
  }

  const filePart = parts.find(p => p.name === 'files');
  if (!filePart || !filePart.data) {
    return sendError(event, createError({ statusCode: 400, message: 'Campo "files" não encontrado' }));
  }

  const originalName = filePart.filename || 'banner.jpg';
  const ext = extname(originalName) || '.jpg';
  const safeName = originalName.replace(/[^a-zA-Z0-9._-]/g, '_');
  const uniqueName = `${Date.now()}-${safeName}`;

  await mkdir(UPLOADS_DIR, { recursive: true });
  await writeFile(join(UPLOADS_DIR, uniqueName), filePart.data);

  return { url: `/uploads/banners/${uniqueName}` };
});
