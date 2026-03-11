<template>
  <form @submit="onSubmit">
    <div class="tp-login-input-wrapper">
      <!-- <div class="tp-login-input-box">
        <div class="tp-login-input">
          <input
            id="login"
            type="text"
            placeholder="Seu login"
            v-bind="login"
          />
        </div>
        <div class="tp-login-input-title">
          <label for="login">Login</label>
        </div>
        <err-message :msg="errors.login" />
      </div> -->
      <div class="tp-login-input-box">
        <div class="tp-login-input">
          <input id="name" type="text" placeholder="Seu nome" v-bind="name" />
        </div>
        <div class="tp-login-input-title">
          <label for="name">Nome</label>
        </div>
        <err-message :msg="errors.name" />
      </div>
      <div class="tp-login-input-box">
        <div class="tp-login-input">
          <input
            type="text"
            v-mask="'(##) #####-####'"
            placeholder="(xx) xxxxx-xxxx"
            v-bind="celular"
          />
        </div>
        <div class="tp-login-input-title">
          <label for="Celular">Celular</label>
        </div>
        <err-message :msg="errors.celular" />
      </div>
      <div class="tp-login-input-box">
        <div class="tp-login-input">
          <input
            type="text"
            v-mask="'###.###.###-##'"
            placeholder="###-###-###-##"
            v-bind="documento"
          />
        </div>
        <div class="tp-login-input-title">
          <label for="Documento">CPF</label>
        </div>
        <err-message :msg="errors.documento" />
      </div>
      <div class="tp-login-input-box">
        <div class="tp-login-input">
          <input
            id="email"
            type="text"
            placeholder="seu@email.com"
            v-bind="email"
          />
        </div>
        <div class="tp-login-input-title">
          <label for="Email">Email</label>
        </div>
        <err-message :msg="errors.email" />
      </div>
      <div class="tp-login-input-box">
        <div class="p-relative">
          <div class="tp-login-input">
            <input
              id="tp_password"
              :type="showPass ? 'text' : 'password'"
              name="password"
              placeholder="Mínimo 6 caracteres"
              v-bind="password"
            />
          </div>
          <div class="tp-login-input-eye" id="password-show-toggle">
            <span class="open-eye" @click="togglePasswordVisibility">
              <template v-if="showPass">
                <svg-open-eye />
              </template>
              <template v-else>
                <svg-close-eye />
              </template>
            </span>
          </div>
          <div class="tp-login-input-title">
            <label for="tp_password">Senha</label>
          </div>
        </div>
        <err-message :msg="errors.password" />
      </div>
      <div class="tp-login-input-box">
        <div class="tp-login-input">
          <input
            id="loginPatrocinador"
            type="text"
            placeholder="Login do seu indicador"
            v-bind="loginPatrocinador"
            :disabled="!!loginPatrocinadorParam"
            :class="{ 'input-disabled': !!loginPatrocinadorParam }"
          />
        </div>
        <div class="tp-login-input-title">
          <label for="name">Indicado por</label>
        </div>
        <err-message :msg="errors.loginPatrocinador" />
      </div>
      <div class="tp-login-input-box" style="margin-bottom: 5px">
        <div class="form-group form-check">
          <Field
            name="termos"
            type="checkbox"
            id="termos"
            value="true"
            class="form-check-input"
          />
          <label for="termos" class="form-check-label"
            >Eu aceito os
            <span class="tp-login-forgot"
              ><nuxt-link
                href="https://bigcash.blob.core.windows.net/documentos/QB%20-%20TERMOS%20DE%20USO%20E%20BONIFICA%C3%87%C3%83O%20ESPEC%C3%8DFICA%20v102023.pdf"
                >termos e condições</nuxt-link
              ></span
            ></label
          >
          <div class="invalid-feedback">{{ errors.termos }}</div>
        </div>
        <err-message :msg="errors.termos" />
      </div>
      <div class="tp-login-input-box">
        <div class="form-group form-check">
          <Field
            name="politicas"
            type="checkbox"
            id="politicas"
            value="true"
            class="form-check-input"
          />
          <label for="politicas" class="form-check-label"
            >Eu aceito as
            <span class="tp-login-forgot"
              ><nuxt-link href="https://quantashop.com.br/QB-LGPD.pdf"
                >políticas de privacidade</nuxt-link
              ></span
            ></label
          >
          <div class="invalid-feedback">{{ errors.politicas }}</div>
        </div>
        <err-message :msg="errors.politicas" />
      </div>
    </div>
    <div class="tp-login-bottom">
      <button type="submit" class="tp-login-btn w-100">Cadastrar</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { useForm, Field } from "vee-validate";
import * as yup from "yup";
import useMask from "@/composables/useMask";
import { useUserStore } from "@/pinia/useUserStore";

const userStore = useUserStore();
const { registerUser } = userStore;

let showPass = ref<boolean>(false);

const route = useRoute();
const loginPatrocinadorParam = route.params.loginPatrocinador || null;

interface IFormValues {
  name?: string | null;
  email?: string | null;
  password?: string | null;
  login?: string | null;
  loginPatrocinador?: string | null;
  celular?: string | null;
  documento?: string | null;
  politicas: boolean | null;
  termos: boolean | null;
}

const { errors, handleSubmit, defineInputBinds, resetForm } =
  useForm<IFormValues>({
    initialValues: {
      loginPatrocinador: loginPatrocinadorParam,
    },
    validationSchema: yup.object({
      name: yup.string().required("O campo Nome é obrigatório").label("Nome"),
      // login: yup
      //   .string()
      //   .required("O campo Login é obrigatório")
      //   .label("Login"),
      documento: yup
        .string()
        .required("O campo CPF é obrigatório")
        .label("Documento"),
      celular: yup
        .string()
        .required("O campo celular é obrigatório")
        .label("Celular"),
      email: yup
        .string()
        .required("O campo Email é obrigatório")
        .email("O email deve ser um email válido")
        .label("Email"),
      termos: yup
        .string()
        .required("É necessário aceitar os termos e condições")
        .label("termos"),
      politicas: yup
        .string()
        .required("É necessário aceitar as politicas de privacidade")
        .label("politicas"),
      password: yup
        .string()
        .required("O campo Senha é obrigatório")
        .min(6, "A Senha deve ter no mínimo 6 caracteres")
        .label("Password"),
    }),
  });

const onSubmit = handleSubmit(async (values) => {
  try {
    const userId = await registerUser({
      nome: values.name,
      login: values.documento,
      documento: values.documento,
      celular: values.celular,
      email: values.email,
      loginPatrocinador: values.loginPatrocinador,
      senha: values.password,
    });

    if (userId) resetForm();
  } catch (error) {
    console.error("Erro durante o login:", error);
  }
});

const togglePasswordVisibility = () => {
  showPass.value = !showPass.value;
};

const name = defineInputBinds("name");
const login = defineInputBinds("login");
const email = defineInputBinds("email");
const documento = defineInputBinds("documento");
const celular = defineInputBinds("celular");
const loginPatrocinador = defineInputBinds("loginPatrocinador");
const password = defineInputBinds("password");
const termos = defineInputBinds("termos");
const politicas = defineInputBinds("politicas");
</script>

<style scoped>
.tp-login-input input.input-disabled {
  background-color: #f0f0f0;
  color: #666;
  cursor: not-allowed;
}

.tp-login-input input.input-disabled::placeholder {
  color: #999;
}
</style>
