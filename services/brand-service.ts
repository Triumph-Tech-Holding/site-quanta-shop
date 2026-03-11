const { get, post } = useApi();

export const getBestBrands = async () => {
    try {
        const response = await get("/v2/awin-feed/get-best-brands/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter melhores marcas:', error)
        throw error
    }
}