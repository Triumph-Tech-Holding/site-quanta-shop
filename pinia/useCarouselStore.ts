import { defineStore } from 'pinia';
import { ref } from 'vue';
import { getCarousels } from "@/services/carousel-service";

export const useCarouselStore = defineStore('carousels', () => {
    const carousels = ref<Array<any>>([]);
    const isCarouselsLoaded = ref(false);

    async function setCarousels(newCarousels: Array<any>) {
        carousels.value = newCarousels;
    }

    async function fetchCarousels() {
        if (isCarouselsLoaded.value) return;

        try {
            if (carousels.value.length == 0) {
                const response = await getCarousels();
                carousels.value = response.data;
                isCarouselsLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar carrosséis:', error);
        }
    }

    return { carousels, setCarousels, fetchCarousels }
});