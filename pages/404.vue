<template>
  <div>
    <section class="tp-error-area pt-110 pb-110">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-xl-6 col-lg-8 col-md-10">
            <div class="tp-error-content text-center">
              <div class="tp-error-thumb">
                <img src="/img/error/error.png" alt="" />
              </div>

              <h5 class="tp-error-title">Oops! Página não encontrada</h5>
              <p>
                Desculpe, mas a página que você está procurando não foi encontrada.
              </p>
              <nuxt-link href="/" class="tp-error-btn">Voltar para a página inicial</nuxt-link>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
useSeoMeta({ title: "404" });
</script>
