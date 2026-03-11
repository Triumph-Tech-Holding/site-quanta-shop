import { defineStore } from 'pinia';
import { login as loginService, forgotPassword as forgotPasswordService, registerUser as registerUserService } from '@/services/user-service';
import { toast } from "vue3-toastify";

interface ApiResponse {
  success: boolean;
  data: any[];
}

export const useUserStore = defineStore('user', () => {
  const user = ref<any>(null);
  const userId = ref<any>(null);
  const token = ref<any>(null);
  const isUserLoaded = ref(false);  
  const isLoggedIn = ref(false);

  function setUser(loggedUser: any) {
    user.value = loggedUser;
    token.value = loggedUser.token;
    userId.value = loggedUser.id;
    
    localStorage.setItem('user', JSON.stringify(loggedUser));
  }

  async function loginUser(loginData: any) { 
    isLoggedIn.value = false;
    
    if (isUserLoaded.value) return;

    try {
      const response = await loginService(loginData);      

      if (response) {        
        const loggedUser: any = response;

        setUser(loggedUser);
        isLoggedIn.value = true;
        isUserLoaded.value = true;
      }
    } catch (error : any) {
      // Verifica se o erro possui o array 'erros'
      if (error.response && error.response.data && error.response.data.erros) {
        error.response.data.erros.forEach((err: any) => {
          toast.error(err.mensagem || 'Erro ao recuperar senha');
        });
      } else {
        toast.error('Erro desconhecido');
      }
    }
  }

  async function logout() {
    localStorage.setItem('user', '');
    localStorage.removeItem("user");
    window.location.reload();
  }
  
  async function forgotPassword(login: any) {
    try {
      const response = await forgotPasswordService(login);
      toast.success(`Email enviado, verifique sua caixa de email ou spam`);
    } catch (error : any) {
      // Verifica se o erro possui o array 'erros'
      if (error.response && error.response.data && error.response.data.erros) {
        error.response.data.erros.forEach((err: any) => {
          toast.error(err.mensagem || 'Erro ao recuperar senha');
        });
      } else {
        toast.error('Erro desconhecido');
      }
    }
  }

  async function registerUser(userData: any) {
    try {
      userData.login = userData.login.replace(/\D/g, '');
      userData.documento = userData.documento.replace(/\D/g, '');
      userData.celular = userData.celular.replace(/\D/g, '');

      const userId = await registerUserService(userData);
      toast.success(`Cadastro realizado com sucesso`);
      return userId;      
    } catch (error: any) {
      // Verifica se o erro possui o array 'erros'
      if (error.response && error.response.data && error.response.data.erros) {
        error.response.data.erros.forEach((err: any) => {
          toast.error(err.mensagem || 'Erro ao realizar cadastro');
        });
      } else {
        toast.error('Erro desconhecido');
      }

      return null;
    }
  }

  async function loadUserFromStorage() {
    const storedUser = localStorage.getItem('user');
    
    if (storedUser) {
      const parsedUser = JSON.parse(storedUser);
      user.value = parsedUser;
      isUserLoaded.value = true;
      isLoggedIn.value = true;
      token.value = parsedUser.token;
      userId.value = parsedUser.id;
    }
  }

  watch(isLoggedIn, (newValue, oldValue) => {
    if (newValue && !oldValue) {
      loadUserFromStorage();
    }
  });

  return {
    user,
    token,
    userId,
    isUserLoaded,
    isLoggedIn,
    loginUser,
    logout,
    forgotPassword,
    registerUser,
    loadUserFromStorage
  };
});
