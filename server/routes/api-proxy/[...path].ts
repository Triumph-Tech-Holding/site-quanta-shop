import type { HTTPMethod } from 'h3';

const LOCAL_API = 'http://localhost:8000/api';
const REMOTE_API = 'https://api.quantashop.com.br/api';

export default defineEventHandler(async (event) => {
  const path = event.context.params?.path || '';
  const method = event.method as HTTPMethod;

  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  };

  const authHeader = getHeader(event, 'authorization');
  if (authHeader) {
    headers['Authorization'] = authHeader;
  }

  const queryString = getRequestURL(event).search;

  let body: string | undefined = undefined;
  if (['POST', 'PUT', 'PATCH'].includes(method)) {
    const rawBody = await readBody(event);
    body = typeof rawBody === 'string' ? rawBody : JSON.stringify(rawBody);
  }

  const useLocalApi = process.env.USE_LOCAL_API === 'true';
  const baseUrl = useLocalApi ? LOCAL_API : REMOTE_API;
  const targetUrl = `${baseUrl}/${path}`;
  const fullUrl = queryString ? `${targetUrl}${queryString}` : targetUrl;

  try {
    const response = await $fetch.raw(fullUrl, {
      method,
      headers,
      body,
      retry: useLocalApi ? 3 : 2,
      retryDelay: 500,
      timeout: 20000,
    });

    const contentType = response.headers.get('content-type');
    if (contentType) {
      setHeader(event, 'Content-Type', contentType);
    }

    return response._data;
  } catch (error: unknown) {
    if (!useLocalApi) {
      const fetchError = error as { response?: { status?: number; _data?: unknown } };
      const statusCode = fetchError?.response?.status || 500;
      const data = fetchError?.response?._data || { message: 'Proxy error' };
      throw createError({ statusCode, data });
    }

    const fetchError = error as { response?: { status?: number; _data?: unknown } };
    const statusCode = fetchError?.response?.status || 502;
    const data = fetchError?.response?._data || { message: `API local indisponível em ${path}` };
    console.error(`[api-proxy] Local API failed after retries for ${path}: ${statusCode}`);
    throw createError({ statusCode, data });
  }
});
