import axios, { type AxiosInstance, type AxiosRequestConfig, type AxiosError } from 'axios';

let _apiClient: AxiosInstance | null = null;

function getApiClient(): AxiosInstance {
    if (!_apiClient) {
        let baseURL = '/api-proxy';
        const config = useRuntimeConfig();
        if (config?.public?.apiBaseUrl) {
            baseURL = config.public.apiBaseUrl as string;
        }
        _apiClient = axios.create({
            baseURL,
            headers: {},
            timeout: 30000,
        });

        // Interceptador de requisição: injetar token
        _apiClient.interceptors.request.use(
            (config) => {
                const agenciaStore = useAgenciaStore();
                const token = agenciaStore.getToken();
                
                if (token) {
                    config.headers.Authorization = `Bearer ${token}`;
                }
                return config;
            },
            (error) => Promise.reject(error)
        );

        // Interceptador de resposta: tratar erros
        _apiClient.interceptors.response.use(
            (response) => response,
            (error: AxiosError) => {
                const statusCode = error.response?.status;
                const router = useRouter();
                const agenciaStore = useAgenciaStore();

                if (statusCode === 401) {
                    agenciaStore.logout();
                    
                    const route = useRoute();
                    if (route.path.startsWith('/agencia')) {
                        router.push('/agencia/login');
                    } else {
                        router.push('/login');
                    }
                } else if (statusCode === 403) {
                    console.error('[API] Acesso negado:', error.config?.url);
                    router.push('/acesso-negado');
                } else if (statusCode && statusCode >= 500) {
                    console.error('[API] Erro do servidor:', error.response?.statusText);
                }

                return Promise.reject(error);
            }
        );
    }
    return _apiClient;
}

export const useApi = () => {
    return {
        get: (url: string, config: AxiosRequestConfig = {}) => getApiClient().get(url, config),
        post: (url: string, data: unknown, config: AxiosRequestConfig = {}) => getApiClient().post(url, data, config),
        put: (url: string, data: unknown, config: AxiosRequestConfig = {}) => getApiClient().put(url, data, config),
        delete: (url: string, config: AxiosRequestConfig = {}) => getApiClient().delete(url, config),
        patch: (url: string, data: unknown, config: AxiosRequestConfig = {}) => getApiClient().patch(url, data, config),
    };
};
