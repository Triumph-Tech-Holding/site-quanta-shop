const { get, post } = useApi();

export const getProduct = async (id: string) => {
    try {
        const params = new URLSearchParams();
        if (id) params.append('id', id);

        const response = await get(`/v2/awin-feed/get-product/?${params.toString()}`);
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter produto:', error)
        throw error
    }
}

export const getProducts = async (quantity = 12, page = 1, minPrice: number | null = null, maxPrice: number | null = null, order:any, category: any) => {
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
        console.error('Erro ao obter produtos:', error)
        throw error
    }
}

export const getSearchedProducts = async (quantity = 12, page = 1, minPrice: number | null = null, maxPrice: number | null = null, order:any, searchText: string | null = null) => {
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
        console.error('Erro ao obter produtos:', error)
        throw error
    }
}