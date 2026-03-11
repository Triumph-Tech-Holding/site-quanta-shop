import { defineStore } from 'pinia';
import { ref } from 'vue';
import { getCategories, getFeaturedCategories, getProductsCategories } from "@/services/category-service";

export const useCategoryStore = defineStore('categories', () => {
    const categories = ref<Array<any>>([]);
    const featuredCategories = ref<Array<any>>([]);
    const productsCategories = ref<Array<any>>([]);
    const isLoaded = ref(false);
    const isFeaturedCategoriesLoaded = ref(false);
    const isProductsCategoriesLoaded = ref(false);

    async function setCategories(newCategories: Array<any>) {
        categories.value = newCategories;
    }
    
    async function fetchCategories() {
        if (isLoaded.value) return;

        try {
            if (categories.value.length == 0) {
                const response = await getCategories();
                categories.value = response;
                isLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar categorias:', error);
        }
    }

    async function fetchFeaturedCategories() {
        if (isFeaturedCategoriesLoaded.value) return;

        try {
            if (featuredCategories.value.length == 0) {
                const response = await getFeaturedCategories();

                featuredCategories.value = response.data;
                isFeaturedCategoriesLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar categorias em destaque:', error);
        }
    }

    async function fetchProductsCategories() {
        if (isProductsCategoriesLoaded.value) return;

        try {
            if (productsCategories.value.length == 0) {
                const response = await getProductsCategories();
                
                productsCategories.value = response.data;
                isProductsCategoriesLoaded.value = true;
            }
        } catch (error) {
            console.error('Erro ao buscar categorias de produtos:', error);
        }
    }

    return { categories, featuredCategories, productsCategories, fetchCategories, fetchFeaturedCategories, fetchProductsCategories, setCategories }
});