export const getCategories = async () => {
    const { get, post } = useApi();
    try {
        const response = await post("/Anunciante/obterCategorias/", { nome: "" });
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter categorias:', error)
        throw error
    }
}

export const getPartnersCategories = async () => {
    const { get } = useApi();
    try {
        const response = await get("/v2/awin-feed/get-partners-categories/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter categorias:', error)
        throw error
    }
}

export const getProductsCategories = async () => {
    const { get } = useApi();
    try {
        const response = await get("/v2/awin-feed/get-categories/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter categorias:', error)
        throw error
    }
}

export const getFeaturedCategories = async () => {
    const { get } = useApi();
    try {
        const response = await get("/v2/categories/get-featured-categories/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter categorias de produtos:', error)
        throw error
    }
}
