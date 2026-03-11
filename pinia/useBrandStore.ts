import { defineStore } from 'pinia';
import { ref } from 'vue';
import { getBestBrands } from "@/services/brand-service";

export const useBrandStore = defineStore('brands-store', () => {
    const bestBrands = ref<Array<any>>([]);
    const isBestBrandsLoaded = ref(false);

    async function fetchBestBrands() {
        if (isBestBrandsLoaded.value) return;

        try {
            if (bestBrands.value.length == 0) {
                const response = await getBestBrands();

                bestBrands.value = response.data;
                isBestBrandsLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar melhores marcas:', error);
        }
    }

    return { bestBrands, fetchBestBrands }
});