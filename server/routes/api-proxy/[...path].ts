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
      retry: useLocalApi ? 0 : 2,
      retryDelay: 500,
      timeout: 15000,
    });

    const contentType = response.headers.get('content-type');
    if (contentType) {
      setHeader(event, 'Content-Type', contentType);
    }

    return response._data;
  } catch (error: unknown) {
    if (useLocalApi) {
      console.warn(`[api-proxy] Local API failed for ${path}, falling back to remote API`);
      const remoteUrl = `${REMOTE_API}/${path}${queryString || ''}`;
      try {
        const fallback = await $fetch.raw(remoteUrl, {
          method,
          headers,
          body,
          retry: 2,
          retryDelay: 500,
          timeout: 15000,
        });
        const contentType = fallback.headers.get('content-type');
        if (contentType) setHeader(event, 'Content-Type', contentType);
        return fallback._data;
      } catch (fallbackError: unknown) {
        const fe = fallbackError as { response?: { status?: number; _data?: unknown } };
        throw createError({ statusCode: fe?.response?.status || 500, data: fe?.response?._data || { message: 'Proxy error' } });
      }
    }

    const fetchError = error as { response?: { status?: number; _data?: unknown } };
    const statusCode = fetchError?.response?.status || 500;
    const data = fetchError?.response?._data || { message: 'Proxy error' };
    throw createError({ statusCode, data });
  }
});
