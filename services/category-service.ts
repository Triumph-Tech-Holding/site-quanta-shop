const { get, post } = useApi();

export const getCategories = async () => {
    try {
        const response = await post("/Anunciante/obterCategorias/", { nome: "" });
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter categorias:', error)
        throw error
    }
}

export const getPartnersCategories = async () => {
    try {
        const response = await get("/v2/awin-feed/get-partners-categories/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter categorias:', error)
        throw error
    }
}

export const getProductsCategories = async () => {
    try {
        const response = await get("/v2/awin-feed/get-categories/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter categorias:', error)
        throw error
    }
}

export const getFeaturedCategories = async () => {
    try {
        const response = await get("/v2/categories/get-featured-categories/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter categorias de produtos:', error)
        throw error
    }
}