import { defineStore } from 'pinia';

export const useLoadingStore = defineStore('loading-store', () => {
    const isLoading = ref(false);

    async function setLoading(value: boolean) {
      isLoading.value = value;
    }

    return { isLoading, setLoading }
});