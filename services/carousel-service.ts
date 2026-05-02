export const getCarousels = async () => {
    const { get } = useApi();
    try {
        const response = await get("v2/carousels");

        return response.data;
    } catch (error) {
        console.error('Erro ao obter carrosséis:', error)
        throw error
    }
}
