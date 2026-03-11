const { get, post } = useApi();

export const login = async (loginData: any) => {
    try {
        const response = await post("/usuarioLogin/autenticacao", loginData);
       
        return response.data;
    } catch (error) {
        console.error('Erro ao realizar login: ', error);
        throw error;
    }
}

export const forgotPassword = async (login: any) => {
    try {
        const response = await get(`/usuarioLogin/esqueciMinhaSenha/${login}`);
        return response.data;
    } catch (error) {
        console.error('Erro ao recuperar senha: ', error);
        throw error;
    }
}

export const registerUser = async (userData: any): Promise<any> => {
    try {
        const response = await post('/user/registrar', userData);
        return response.data;
    } catch (error) {
        console.error('Erro ao registrar usuário: ', error);
        throw error;
    }
}

export const confirmEmail = async (token: string): Promise<any> => {
    try {
        const response = await post(`/v2/users/confirm-email?token=${token}`, {});
        return response.data;
    } catch (error) {
        console.error('Erro ao confirmar email: ', error);
        throw error;
    }
}