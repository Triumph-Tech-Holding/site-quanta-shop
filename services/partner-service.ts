const { get, post } = useApi();

export const getPartners = async (type = null, name = null, category = null, page = 1, limit = 12) => {
    try {
        const params = new URLSearchParams();
        if (type) params.append('type', type);
        if (name) params.append('name', name);
        if (category) params.append('category', category);
        if (page) params.append('page', page.toString());
        if (limit) params.append('limit', limit.toString());
        
        const response = await get(`/v2/partners/get-partners/?${params.toString()}`);
        
        return response.data;
    } catch (error) {
        console.error('Erro ao obter parceiros:', error);
        throw error;
    }
}


export const getNewPartners = async () => {
    try {
        const response = await get("/v2/partners/get-new-partners/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter novos parceiros:', error)
        throw error
    }
}

export const getFeaturedPartners = async () => {
    try {
        const response = await get("/v2/partners/get-featured-partners/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter parceiros em destaque:', error)
        throw error
    }
}

export const getTopSellersPartners = async () => {
    try {
        const response = await get("/v2/partners/get-top-sellers-partners/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter parceiros que mais vendem em destaque:', error)
        throw error
    }
}

export const getLocalPartners = async () => {
    try {
        const response = await get("/v2/partners/get-local-partners/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter parceiros locais:', error)
        throw error
    }
}

export const getBestDiscountsLocalPartners = async () => {
    try {
        const response = await get("/v2/partners/get-best-discounts-local-partners/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter melhores ofertas de parceiros locais:', error)
        throw error
    }
}

export const getFeaturedLocalPartners = async () => {
    try {
        const response = await get("/v2/partners/get-featured-local-partners/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter parceiros locais em destaque:', error)
        throw error
    }
}

export const getTopSellersLocalPartners = async () => {
    try {
        const response = await get("/v2/partners/get-top-sellers-local-partners/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter parceiros locais que mais vendem em destaque:', error)
        throw error
    }
}