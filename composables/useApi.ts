import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios';

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
            headers: {}
        });
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
