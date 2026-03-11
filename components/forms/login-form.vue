<template>
  <form @submit="onSubmit">
    <div class="tp-login-input-wrapper">
      <div class="tp-login-input-box">
        <div class="tp-login-input">
          <input
            id="email"
            type="text"
            placeholder="seu@email.com"
            v-bind="login"
            autocomplete="username"
          />
        </div>
        <div class="tp-login-input-title">
          <label for="email">Login</label>
        </div>
        <err-message :msg="errors.login" />
      </div>
      <div class="tp-login-input-box">
        <div class="p-relative">
          <div class="tp-login-input">
            <input
              id="tp_password"
              :type="showPass ? 'text' : 'password'"
              name="senha"
              placeholder="Mínimo 6 caracteres"
              v-bind="password"
              autocomplete="current-password"
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
    </div>
    <div
      class="tp-login-suggetions d-sm-flex align-items-center justify-content-between mb-20"
    >
      <div class="tp-login-forgot">
        <p>
          Não possui cadastro?
          <span><nuxt-link href="/register">Clique aqui</nuxt-link></span>
        </p>
      </div>
      <div class="tp-login-forgot">
        <p>
          <nuxt-link href="/forgot">Esqueci minha senha</nuxt-link>
        </p>
      </div>
    </div>
    <div class="tp-login-bottom">
      <button type="submit" class="tp-login-btn w-100">Acessar</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { useRoute } from "vue-router";
import { useForm } from "vee-validate";
import * as yup from "yup";
import { useUserStore } from "@/pinia/useUserStore";

let showPass = ref<boolean>(false);

const userStore = useUserStore();
const { loginUser } = userStore;

const router = useRouter();
const route = useRoute();
interface IFormValues {
  login?: string | null;
  password?: string | null;
}

const { errors, handleSubmit, defineInputBinds, resetForm } =
  useForm<IFormValues>({
    validationSchema: yup.object({
      login: yup
        .string()
        .required("O campo Login é obrigatório")
        .label("Login"),
      password: yup
        .string()
        .required("O campo Senha é obrigatório")
        .min(6, "A Senha deve ter no mínimo 6 caracteres")
        .label("Password"),
    }),
  });

  const onSubmit = handleSubmit(async (values) => {
  try {
    await loginUser({
      login: values.login,
      senha: values.password,
    });

    // Verifica se o usuário está logado verificando o localStorage
    if (isLoggedIn()) {
      resetForm();

      if (route.query.redirect) {
        router.push(route.query.redirect as string);
      } else {
        router.push("/");
      }
    } else {
      throw new Error("Falha no login: verifique suas credenciais.");
    }
  } catch (error) {
    console.error("Erro durante o login:", error);
  }
});

const togglePasswordVisibility = () => {
  showPass.value = !showPass.value;
};

function isLoggedIn() {
  const user = localStorage.getItem('user');
  return user !== null;
}

const login = defineInputBinds("login");
const password = defineInputBinds("password");
</script>
