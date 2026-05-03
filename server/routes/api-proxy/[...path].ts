import type { HTTPMethod } from 'h3';

const LOCAL_API = 'http://localhost:8000/api';
const REMOTE_API = 'https://api.quantashop.com.br/api';

const FAST_TIMEOUT_MS = 8000;
const FAST_RETRY = 1;
const RETRY_DELAY_MS = 300;

const BROKEN_TTL_MS = 60_000;
const brokenCache = new Map<string, number>();

function isBroken(key: string): boolean {
  const until = brokenCache.get(key);
  if (!until) return false;
  if (Date.now() > until) {
    brokenCache.delete(key);
    return false;
  }
  return true;
}

function markBroken(key: string): void {
  brokenCache.set(key, Date.now() + BROKEN_TTL_MS);
}

async function fetchApi(url: string, method: HTTPMethod, headers: Record<string, string>, body: string | undefined, retry: number) {
  const response = await $fetch.raw(url, {
    method,
    headers,
    body,
    retry,
    retryDelay: RETRY_DELAY_MS,
    timeout: FAST_TIMEOUT_MS,
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

  const config = useRuntimeConfig();
  const useLocalApi = config.useLocalApi === true;

  const buildUrl = (base: string) => {
    const target = `${base}/${path}`;
    return queryString ? `${target}${queryString}` : target;
  };

  const localKey = `LOCAL:${method}:${path}`;
  const remoteKey = `REMOTE:${method}:${path}`;

  if (useLocalApi && !isBroken(localKey)) {
    try {
      const response = await fetchApi(buildUrl(LOCAL_API), method, headers, body, FAST_RETRY);
      const contentType = response.headers.get('content-type');
      if (contentType) setHeader(event, 'Content-Type', contentType);
      return response._data;
    } catch {
      markBroken(localKey);
    }
  }

  if (isBroken(remoteKey)) {
    throw createError({ statusCode: 503, data: { message: `Upstream temporariamente indisponível para ${path}` } });
  }

  try {
    const response = await fetchApi(buildUrl(REMOTE_API), method, headers, body, FAST_RETRY);
    const contentType = response.headers.get('content-type');
    if (contentType) setHeader(event, 'Content-Type', contentType);
    return response._data;
  } catch (error: unknown) {
    markBroken(remoteKey);
    const fetchError = error as { response?: { status?: number; _data?: unknown } };
    const statusCode = fetchError?.response?.status || 502;
    const data = fetchError?.response?._data || { message: `Proxy error for ${path}` };
    throw createError({ statusCode, data });
  }
});
