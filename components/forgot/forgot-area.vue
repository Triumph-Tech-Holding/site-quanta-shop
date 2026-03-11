<template>
 <section class="tp-login-area pb-140 p-relative z-index-1 fix">
    <div class="tp-login-shape">
        <img class="tp-login-shape-1" src="/img/login/login-shape-1.png" alt="shape">
        <img class="tp-login-shape-2" src="/img/login/login-shape-2.png" alt="shape">
        <img class="tp-login-shape-3" src="/img/login/login-shape-3.png" alt="shape">
        <img class="tp-login-shape-4" src="/img/login/login-shape-4.png" alt="shape">
    </div>
    <div class="container">
        <div class="row justify-content-center">
          <div class="col-xl-6 col-lg-8">
              <div class="tp-login-wrapper">
                <div class="tp-login-top text-center mb-30">
                    <h3 class="tp-login-title">Recuperar minha senha</h3>
                    <p>Você receberá um email com as instruções para redefinir sua senha</p>
                </div>
                <div class="tp-login-option">
                    <form @submit="onSubmit">
                      <div class="tp-login-input-wrapper">
                        <div class="tp-login-input-box">
                            <div class="tp-login-input">
                              <input id="login" type="text" placeholder="Login cadastrado" v-bind="login">
                            </div>
                            <div class="tp-login-input-title">
                              <label for="Login">Login</label>
                            </div>
                            <err-message :msg="errors.login" />
                        </div>
                      </div>
                      <div class="tp-login-bottom mb-15">
                        <button type="submit" class="tp-login-btn w-100">Recuperar senha</button>
                      </div>
                      <div class="tp-login-suggetions d-sm-flex align-items-center justify-content-center">
                        <div class="tp-login-forgot">
                            <span>Lembrou sua senha? <nuxt-link href="/login"> Entrar</nuxt-link></span>
                        </div>
                      </div>
                    </form>
                </div>
              </div>
          </div>
        </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { useForm } from "vee-validate";
import * as yup from "yup";
import { useUserStore } from "@/pinia/useUserStore";


const userStore = useUserStore();
const { forgotPassword } = userStore; 


interface IFormValues {
  login?: string | null;
}
const { errors, handleSubmit, defineInputBinds, resetForm } =
  useForm<IFormValues>({
    validationSchema: yup.object({
      login: yup.string().required("O campo Login é obrigatório").label("Login")
    }),
  });

const onSubmit = handleSubmit(async (values) => {
  
  try {  
    await forgotPassword(
      values.login
    );    
    // resetForm();
  } catch (error) {
    console.error("Erro durante o login:", error);
  }  
  //resetForm();
});

const login = defineInputBinds("login");
</script>
