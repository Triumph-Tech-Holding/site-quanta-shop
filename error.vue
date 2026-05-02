<template>
  <nuxt-layout name="default">
    <section class="tp-error-area pt-110 pb-110">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-xl-6 col-lg-8 col-md-10">
            <div class="tp-error-content text-center">
              <div class="tp-error-thumb">
                <img src="/img/error/error.png" alt="" />
              </div>
  
              <h5 class="tp-error-title">{{ error.statusCode }}</h5>
              <p>
                Whoops, isso é constrangedor. Parece que a página que você estava procurando não foi encontrada.
              </p>
  
              <nuxt-link href="/" class="tp-error-btn">Voltar para a página inicial</nuxt-link>
            </div>
          </div>
        </div>
      </div>
    </section>
  </nuxt-layout>
</template>

<script setup lang="ts">
useSeoMeta({ 
  title: "Página não encontrada | Quanta Shop",
  description: "Desculpe, a página que você está procurando não existe ou foi movida."
});
const props = defineProps(["error"]);

onMounted(() => {
  if (props.error?.statusCode === 404) {
    // Nuxt 3 lida com isso se lançado via createError, mas garantimos aqui
  }
});
</script>
