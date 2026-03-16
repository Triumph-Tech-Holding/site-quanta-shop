import type { HTTPMethod } from 'h3';

export default defineEventHandler(async (event) => {
  const path = event.context.params?.path || '';
  const targetUrl = `https://api.quantashop.com.br/api/${path}`;

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
  const fullUrl = queryString ? `${targetUrl}${queryString}` : targetUrl;

  let body: string | undefined = undefined;
  if (['POST', 'PUT', 'PATCH'].includes(method)) {
    const rawBody = await readBody(event);
    body = typeof rawBody === 'string' ? rawBody : JSON.stringify(rawBody);
  }

  try {
    const response = await $fetch.raw(fullUrl, {
      method,
      headers,
      body,
      retry: 2,
      retryDelay: 500,
      timeout: 15000,
    });

    const contentType = response.headers.get('content-type');
    if (contentType) {
      setHeader(event, 'Content-Type', contentType);
    }

    return response._data;
  } catch (error: unknown) {
    const fetchError = error as { response?: { status?: number; _data?: unknown } };
    const statusCode = fetchError?.response?.status || 500;
    const data = fetchError?.response?._data || { message: 'Proxy error' };

    throw createError({
      statusCode,
      data,
    });
  }
});
