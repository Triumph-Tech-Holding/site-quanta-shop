<template>
  <div class="d-flex justify-content-center align-items-center pb-110">
    <div class="alert" style="color: white; width: 800px; text-align: center">
      <div v-if="isSuccess">
        <section class="tp-error-area">
          <div class="container">
            <div class="row justify-content-center">
              <div class="col-12">
                <div class="tp-error-content text-center">
                  <div class="tp-error-thumb">
                    <img
                      src="/img/success/success-2.png"
                      style="width: 250px"
                      alt=""
                    />
                  </div>

                  <h5 class="tp-error-title">
                    Seu endereço de email foi validado com sucesso
                  </h5>
                  <p>
                    Obrigado por confirmar seu cadastro. Agora você pode acessar
                    todos os recursos disponíveis.
                  </p>
                  <nuxt-link href="/" class="tp-error-btn">
                    Ir para a página inicial
                  </nuxt-link>
                </div>
              </div>
            </div>
          </div>
        </section>
      </div>
      <div v-if="isError">
        <div>
          <section class="tp-error-area">
            <div class="container">
              <div class="row justify-content-center">
                <div class="col-12">
                  <div class="tp-error-content text-center">
                    <div class="tp-error-thumb">
                      <img
                        src="/img/error/error-2.png"
                        style="width: 250px"
                        alt=""
                      />
                    </div>

                    <h5 class="tp-error-title">
                      Oops! Falha na confirmação do seu e-mail
                    </h5>
                    <p>
                      Não conseguimos validar seu endereço de email. Verifique o
                      link enviado ou entre em contato com o suporte.
                    </p>
                    <nuxt-link href="/" class="tp-error-btn">
                      Voltar para a página inicial
                    </nuxt-link>
                  </div>
                </div>
              </div>
            </div>
          </section>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { confirmEmail } from "@/services/user-service";
import { useRoute } from "vue-router";

const route = useRoute();
const isSuccess = ref(false);
const isError = ref(false);

useSeoMeta({ title: "Quanta Shop" });

onMounted(async() => {
  if (!route.params.token) {
    // Redirecionar para 404 se não houver token
    alert('Não há token');
    return navigateTo('/404', { redirectCode: 404 });
  }

  try {
    await confirmEmail(route.params.token.toString());
    isSuccess.value = true;
    isError.value = false;

    setTimeout(() => {
      navigateTo("/");
    }, 2000);
  } catch (error) {
    isSuccess.value = false;
    isError.value = true;
  }
});
</script>

<style></style>
