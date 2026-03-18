import type { HTTPMethod } from 'h3';

const LOCAL_API = 'http://localhost:8000/api';
const REMOTE_API = 'https://api.quantashop.com.br/api';

async function fetchApi(url: string, method: HTTPMethod, headers: Record<string, string>, body: string | undefined, retry: number) {
  const response = await $fetch.raw(url, {
    method,
    headers,
    body,
    retry,
    retryDelay: 500,
    timeout: 20000,
  });
  return response;
}

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

  const buildUrl = (base: string) => {
    const target = `${base}/${path}`;
    return queryString ? `${target}${queryString}` : target;
  };

  if (useLocalApi) {
    try {
      const response = await fetchApi(buildUrl(LOCAL_API), method, headers, body, 3);
      const contentType = response.headers.get('content-type');
      if (contentType) setHeader(event, 'Content-Type', contentType);
      return response._data;
    } catch (localError: unknown) {
      console.warn(`[api-proxy] Local API failed for ${path}, falling back to remote`);
    }
  }

  try {
    const response = await fetchApi(buildUrl(REMOTE_API), method, headers, body, 2);
    const contentType = response.headers.get('content-type');
    if (contentType) setHeader(event, 'Content-Type', contentType);
    return response._data;
  } catch (error: unknown) {
    const fetchError = error as { response?: { status?: number; _data?: unknown } };
    const statusCode = fetchError?.response?.status || 502;
    const data = fetchError?.response?._data || { message: `Proxy error for ${path}` };
    throw createError({ statusCode, data });
  }
});
