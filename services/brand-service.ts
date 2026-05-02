export const getBestBrands = async () => {
    const { get } = useApi();
    try {
        const response = await get("/v2/awin-feed/get-best-brands/");
       
        return response.data;
    } catch (error) {
        console.error('Erro ao obter melhores marcas:', error)
        throw error
    }
}
