export default defineEventHandler(async (event) => {
  const path = event.context.params?.path || '';
  const targetUrl = `https://api.quantashop.com.br/api/${path}`;

  const method = event.method;
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

  let body: any = undefined;
  if (['POST', 'PUT', 'PATCH'].includes(method)) {
    body = await readBody(event);
  }

  try {
    const response = await $fetch.raw(fullUrl, {
      method: method as any,
      headers,
      body: body ? JSON.stringify(body) : undefined,
      retry: 2,
      retryDelay: 500,
      timeout: 15000,
    });

    const responseHeaders = response.headers;
    const contentType = responseHeaders.get('content-type');
    if (contentType) {
      setHeader(event, 'Content-Type', contentType);
    }

    return response._data;
  } catch (error: any) {
    const statusCode = error?.response?.status || 500;
    const data = error?.response?._data || { message: 'Proxy error' };

    throw createError({
      statusCode,
      data,
    });
  }
});
