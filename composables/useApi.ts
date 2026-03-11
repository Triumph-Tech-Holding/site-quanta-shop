import axios, { type AxiosInstance } from 'axios';

export const useApi = () => {
    const baseURL = 'https://api.quantashop.com.br/api';
    // const baseURL = 'https://api-hlog.quantashop.com.br/api';
    // const baseURL = 'https://localhost:44385/api';

    const apiClient: AxiosInstance = axios.create({
      baseURL,
      headers: {
        // Authorization: `Bearer ${storeUser.token}`
      }
    });

    return {
        get: (url: string, config = {}) => apiClient.get(url, config),
        post: (url: string, data: any, config = {}) => apiClient.post(url, data, config),
        put: (url: string, data: any, config = {}) => apiClient.put(url, data, config),
        delete: (url: string, config = {}) => apiClient.delete(url, config),
        patch: (url: string, data: any, config = {}) => apiClient.patch(url, data, config),
    };
};