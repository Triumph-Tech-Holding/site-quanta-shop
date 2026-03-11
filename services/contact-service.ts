const { get, post } = useApi();

export const contact = async (contactData: any) => {
    try {
        const response = await post("/contato/EnviarContato", contactData);
       
        return response.data;
    } catch (error) {
        console.error('Erro ao realizar contato:', error);
        throw error;
    }
}