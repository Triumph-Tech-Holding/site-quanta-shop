<template>
  <div class="cem-page">
    <div class="cem-box">
      <div v-if="isSuccess" class="cem-content">
        <img src="/img/success/success-2.png" class="cem-img" alt="Sucesso" />
        <h5 class="cem-title">Seu endereço de e-mail foi validado com sucesso</h5>
        <p class="cem-desc">
          Obrigado por confirmar seu cadastro. Agora você pode acessar todos os recursos disponíveis.
        </p>
        <NuxtLink href="/" class="qs-btn-primary">Ir para a página inicial</NuxtLink>
      </div>
      <div v-if="isError" class="cem-content">
        <img src="/img/error/error-2.png" class="cem-img" alt="Erro" />
        <h5 class="cem-title">Oops! Falha na confirmação do seu e-mail</h5>
        <p class="cem-desc">
          Não conseguimos validar seu endereço de e-mail. Verifique o link enviado ou entre em contato com o suporte.
        </p>
        <NuxtLink href="/" class="qs-btn-primary">Voltar para a página inicial</NuxtLink>
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

useSeoMeta({ title: "Confirmar E-mail | Quanta Shop" });

onMounted(async () => {
  if (!route.params.token) {
    alert('Não há token');
    return navigateTo('/404', { redirectCode: 404 });
  }

  try {
    await confirmEmail(route.params.token.toString());
    isSuccess.value = true;
    isError.value = false;
    setTimeout(() => { navigateTo("/"); }, 2000);
  } catch (error) {
    isSuccess.value = false;
    isError.value = true;
  }
});
</script>

<style scoped>
.cem-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 60vh;
  padding: 4rem 1rem;
}
.cem-box {
  max-width: 680px;
  width: 100%;
}
.cem-content {
  text-align: center;
}
.cem-img {
  width: 220px;
  max-width: 100%;
  margin-bottom: 1.5rem;
}
.cem-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #0f1c23;
  margin-bottom: .75rem;
}
.cem-desc {
  color: #6b7280;
  font-size: 1rem;
  max-width: 480px;
  margin: 0 auto 1.5rem;
  line-height: 1.6;
}
</style>
