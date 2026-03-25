import { defineEventHandler, readMultipartFormData, sendError, createError } from 'h3';

const REMOTE_API = 'https://api.quantashop.com.br/api';
const LOCAL_API = 'http://localhost:8000/api';

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

  const authHeader = getHeader(event, 'authorization') ?? '';
  const filename = filePart.filename || 'banner.jpg';
  const contentType = filePart.type || 'image/jpeg';

  const formData = new FormData();
  formData.append('files', new Blob([filePart.data], { type: contentType }), filename);

  const config = useRuntimeConfig();
  const useLocalApi = config.useLocalApi === true;
  const baseApi = useLocalApi ? LOCAL_API : REMOTE_API;

  try {
    const response = await $fetch<string>(`${baseApi}/Image`, {
      method: 'POST',
      headers: { Authorization: authHeader },
      body: formData,
    });
    return { url: response };
  } catch (err: unknown) {
    const e = err as { response?: { status?: number; _data?: unknown } };
    throw createError({
      statusCode: e?.response?.status ?? 502,
      message: 'Erro ao enviar imagem para o servidor',
    });
  }
});
