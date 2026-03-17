<template>
  <div>
    <PrimeiraCompraTheNavbar />

    <section
      class="pb_cover_v3 cover-bg-indigo cover-bg-opacity text-left pb_gradient_v1 pb_slant-light"
      id="section-home"
      style="min-height: 95vh; height: auto"
    >
      <div class="container">
        <div
          class="row align-items-center justify-content-center"
          style="min-height: 95vh; height: auto; padding-top: 100px; padding-bottom: 3rem"
        >
          <div class="col-md-7">
            <h2 class="heading mb-3">
              Cadastre-se no <strong>Quanta Shop</strong> e receba cashback nas
              suas compras!
            </h2>
            <div class="sub-heading">
              <p class="mb-4">
                Aproveite a oportunidade de economizar com o Quanta Shop! Ao se
                cadastrar, você receberá cashback das suas compras.
              </p>
              <p class="mb-4">
                É simples, rápido e você ganha de volta uma parte do valor
                gasto. Preencha o formulário e comece a economizar hoje mesmo.
              </p>
              <p class="mb-4">
                Não perca essa chance de fazer suas compras valerem ainda mais
                com o Quanta Shop!
              </p>
            </div>
          </div>

          <div class="col-md-5 mt-5 relative align-self-center">
            <PrimeiraCompraRegistrationForm :cnpj="cnpj" />
          </div>
        </div>
      </div>
    </section>

    <footer class="pb_footer" style="background: #1a1a2e; padding: 2rem 0; margin-top: 2rem">
      <div class="container text-center text-white">
        <p class="mb-0" style="opacity: 0.7; font-size: 0.9rem">
          &copy; {{ new Date().getFullYear() }} Quanta Shop. Todos os direitos reservados.
        </p>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'primeira-compra' })

const route = useRoute()

const cnpj = computed<string | undefined>(() => {
  if (route.params.cnpj) return route.params.cnpj as string
  if (route.query.cnpj) return route.query.cnpj as string
  return undefined
})

useHead({
  title: 'Primeira Compra — Quanta Shop',
  meta: [
    { name: 'description', content: 'Cadastre-se no Quanta Shop e receba cashback nas suas compras.' }
  ]
})
</script>

<style lang="scss">
@import '@/assets/scss/primeira-compra.scss';
</style>
