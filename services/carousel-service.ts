const { get } = useApi();

export const getCarousels = async () => {
    try {
        const response = await get("v2/carousels");

        return response.data;
    } catch (error) {
        console.error('Erro ao obter carrosséis:', error)
        throw error
    }
}