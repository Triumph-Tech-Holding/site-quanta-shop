import axios, { type AxiosInstance, type AxiosRequestConfig, type AxiosResponse } from 'axios';

let _apiClient: AxiosInstance | null = null;

function getApiClient(): AxiosInstance {
  if (!_apiClient) {
    const config = useRuntimeConfig();
    const baseURL = (config?.public?.apiBaseUrl as string | undefined) ?? '/api-proxy';

    _apiClient = axios.create({
      baseURL,
      timeout: 30000,
    });

    // ── Interceptador de requisição: injetar Bearer token ──────────────────────
    _apiClient.interceptors.request.use(
      (req) => {
        const store = useAgenciaStore();
        const token = store.getToken();
        if (token) {
          req.headers.Authorization = `Bearer ${token}`;
        }
        return req;
      },
      (error: unknown) => Promise.reject(error),
    );

    // ── Interceptador de resposta: tratar 401 / 403 / 5xx ─────────────────────
    _apiClient.interceptors.response.use(
      (response: AxiosResponse) => response,
      (error: unknown) => {
        if (axios.isAxiosError(error)) {
          const status = error.response?.status;
          const router = useRouter();
          const route = useRoute();
          const store = useAgenciaStore();

          if (status === 401) {
            store.logout();
            const target = route.path.startsWith('/agencia') ? '/agencia/login' : '/login';
            router.push(target);
          } else if (status === 403) {
            console.error('[API] Acesso negado:', error.config?.url);
            router.push('/acesso-negado');
          } else if (status !== undefined && status >= 500) {
            console.error('[API] Erro interno do servidor:', status, error.config?.url);
          }
        }

        return Promise.reject(error);
      },
    );
  }

  return _apiClient;
}

/** Reinicia o cliente axios (útil após logout para limpar estado de auth). */
export function resetApiClient(): void {
  _apiClient = null;
}

export const useApi = () => ({
  get: <T = unknown>(url: string, config?: AxiosRequestConfig) =>
    getApiClient().get<T>(url, config),

  post: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    getApiClient().post<T>(url, data, config),

  put: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    getApiClient().put<T>(url, data, config),

  patch: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    getApiClient().patch<T>(url, data, config),

  delete: <T = unknown>(url: string, config?: AxiosRequestConfig) =>
    getApiClient().delete<T>(url, config),
});
