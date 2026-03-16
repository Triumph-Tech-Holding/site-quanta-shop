import axios, { type AxiosInstance } from 'axios';

let _apiClient: AxiosInstance | null = null;

function getApiClient(): AxiosInstance {
    if (!_apiClient) {
        let baseURL = '/api';
        try {
            const config = useRuntimeConfig();
            if (config?.public?.apiBaseUrl) {
                baseURL = config.public.apiBaseUrl as string;
            }
        } catch {
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
        get: (url: string, config = {}) => getApiClient().get(url, config),
        post: (url: string, data: any, config = {}) => getApiClient().post(url, data, config),
        put: (url: string, data: any, config = {}) => getApiClient().put(url, data, config),
        delete: (url: string, config = {}) => getApiClient().delete(url, config),
        patch: (url: string, data: any, config = {}) => getApiClient().patch(url, data, config),
    };
};
