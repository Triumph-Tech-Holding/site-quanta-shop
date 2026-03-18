<template>
  <nav
    :class="['navbar navbar-expand-lg navbar-dark pb_navbar', scrolled ? 'pb_navbar-scrolled' : 'pb_scrolled-light']"
    id="pb-navbar"
  >
    <div class="container">
      <nuxt-link class="navbar-brand m-3 mr-5" to="/">
        <img
          src="/img/logo.png"
          alt="Quanta Shop"
          style="width: 8rem"
        />
      </nuxt-link>
      <button
        class="navbar-toggler ml-auto m-3"
        type="button"
        @click="menuOpen = !menuOpen"
        aria-label="Toggle navigation"
      >
        <span><i class="ion-navicon"></i></span>
      </button>
      <div :class="['collapse navbar-collapse', menuOpen ? 'show' : '']" id="probootstrap-navbar">
        <ul class="navbar-nav">
          <li class="nav-item ml-3">
            <nuxt-link class="nav-link" to="/">Lojas</nuxt-link>
          </li>
          <li class="nav-item ml-3">
            <nuxt-link class="nav-link" to="/quem-somos">Quem somos</nuxt-link>
          </li>
          <li class="nav-item ml-3">
            <nuxt-link class="nav-link" to="/como-funciona">Como funciona</nuxt-link>
          </li>
          <li class="nav-item ml-3">
            <nuxt-link class="nav-link" to="/crendenciamento">Credenciamento</nuxt-link>
          </li>
          <li class="nav-item ml-3">
            <nuxt-link class="nav-link" to="/contact">Contato</nuxt-link>
          </li>
          <li class="nav-item ml-3 cta-btn ml-xl-2 ml-lg-2 ml-md-0 ml-sm-0 ml-0">
            <a class="nav-link" href="https://quantabank.com.br/" target="_blank" rel="noreferrer">
              <span class="pb_rounded-4 px-4">Quanta Bank</span>
            </a>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
const scrolled = ref(false)
const menuOpen = ref(false)

onMounted(() => {
  const handleScroll = () => { scrolled.value = window.scrollY > 50 }
  window.addEventListener('scroll', handleScroll)
  onUnmounted(() => window.removeEventListener('scroll', handleScroll))
})
</script>
