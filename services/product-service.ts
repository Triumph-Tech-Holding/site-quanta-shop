export const getProduct = async (id: string) => {
    const { get } = useApi();
    try {
        const params = new URLSearchParams();
        if (id) params.append('id', id);

        const response = await get(`/v2/awin-feed/get-product/?${params.toString()}`);

        return response.data;
    } catch (error) {
        console.warn('[product-service] getProduct indisponível, consumidor deve usar fallback');
        throw error;
    }
}

export const getProducts = async (quantity = 12, page = 1, minPrice: number | null = null, maxPrice: number | null = null, order:any, category: any) => {
    const { get } = useApi();
    try {
        const params = new URLSearchParams();
        if (quantity) params.append('quantity', quantity.toString());
        if (page) params.append('page', page.toString());
        if (minPrice) params.append('minPrice', minPrice.toString());
        if (maxPrice) params.append('maxPrice', maxPrice.toString());
        if (order) params.append('order', order.toString());
        if (category) params.append('category', category.toString());

        const response = await get(`/v2/awin-feed/get-products/?${params.toString()}`);

        return response.data;
    } catch (error) {
        console.warn('[product-service] getProducts indisponível, consumidor deve usar fallback');
        throw error;
    }
}

export const getSearchedProducts = async (quantity = 12, page = 1, minPrice: number | null = null, maxPrice: number | null = null, order:any, searchText: string | null = null) => {
    const { get } = useApi();
    try {
        const params = new URLSearchParams();
        if (quantity) params.append('quantity', quantity.toString());
        if (page) params.append('page', page.toString());
        if (minPrice) params.append('minPrice', minPrice.toString());
        if (maxPrice) params.append('maxPrice', maxPrice.toString());
        if (order) params.append('order', order.toString());
        if (searchText) params.append('searchText', searchText.toString());

        const response = await get(`/v2/awin-feed/get-searched-products/?${params.toString()}`);

        return response.data;
    } catch (error) {
        console.warn('[product-service] getSearchedProducts indisponível, consumidor deve usar fallback');
        throw error;
    }
}
