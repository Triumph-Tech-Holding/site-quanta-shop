<template>
  <header class="qs-home-header">
    <div class="qs-home-header__main">
      <div class="container">
        <div class="qs-home-header__row">
          <div class="qs-home-header__logo">
            <nuxt-link href="/">
              <img src="/img/logo/logo.png" alt="Quanta Shop" />
            </nuxt-link>
          </div>

          <nav class="qs-home-header__nav d-none d-xl-flex">
            <nuxt-link href="/">Para Você</nuxt-link>
            <nuxt-link href="/agencia/como-funciona">Para sua Empresa</nuxt-link>
            <nuxt-link href="/primeira-compra">Seja um Agente</nuxt-link>
            <nuxt-link href="/about">Quanta Amizade</nuxt-link>
            <nuxt-link href="/blog">Blog</nuxt-link>
            <nuxt-link href="/contact">Fale Conosco</nuxt-link>
          </nav>

          <div class="qs-home-header__actions">
            <nuxt-link href="/login" class="qs-btn-login">Login</nuxt-link>
            <nuxt-link href="/register" class="qs-btn-cadastro">Cadastre-se</nuxt-link>
            <button class="qs-mobile-menu-btn d-xl-none" @click="toggleMobile">
              <span></span><span></span><span></span>
            </button>
          </div>
        </div>
      </div>
    </div>

    <div class="qs-home-header__search">
      <div class="container">
        <div class="qs-home-header__search-inner">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#999" stroke-width="2">
            <circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/>
          </svg>
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Buscar marcas, produtos, cashback..."
            @keyup.enter="handleSearch"
          />
          <button v-if="searchQuery" @click="searchQuery = ''" class="qs-search-clear">✕</button>
        </div>
      </div>
    </div>

    <div v-if="mobileOpen" class="qs-mobile-menu d-xl-none">
      <nav>
        <nuxt-link href="/" @click="mobileOpen = false">Para Você</nuxt-link>
        <nuxt-link href="/agencia/como-funciona" @click="mobileOpen = false">Para sua Empresa</nuxt-link>
        <nuxt-link href="/primeira-compra" @click="mobileOpen = false">Seja um Agente</nuxt-link>
        <nuxt-link href="/about" @click="mobileOpen = false">Quanta Amizade</nuxt-link>
        <nuxt-link href="/blog" @click="mobileOpen = false">Blog</nuxt-link>
        <nuxt-link href="/contact" @click="mobileOpen = false">Fale Conosco</nuxt-link>
        <div class="qs-mobile-menu__actions">
          <nuxt-link href="/login" class="qs-btn-login" @click="mobileOpen = false">Login</nuxt-link>
          <nuxt-link href="/register" class="qs-btn-cadastro" @click="mobileOpen = false">Cadastre-se</nuxt-link>
        </div>
      </nav>
    </div>
  </header>

  <offcanvas-cart-sidebar />
</template>

<script setup lang="ts">
import { ref } from 'vue';
const router = useRouter();
const searchQuery = ref('');
const mobileOpen = ref(false);

function toggleMobile() {
  mobileOpen.value = !mobileOpen.value;
}

function handleSearch() {
  if (searchQuery.value.trim()) {
    router.push(`/partners?nome=${encodeURIComponent(searchQuery.value.trim())}`);
  }
}
</script>

<style scoped>
.qs-home-header {
  position: relative;
  z-index: 1000;
}

.qs-home-header__main {
  background: #fff;
  border-bottom: 1px solid #f0f0f0;
  padding: 14px 0;
}

.qs-home-header__row {
  display: flex;
  align-items: center;
  gap: 24px;
}

.qs-home-header__logo img {
  height: 38px;
  width: auto;
}

.qs-home-header__nav {
  flex: 1;
  align-items: center;
  gap: 6px;
}

.qs-home-header__nav a {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 500;
  color: #374151;
  padding: 6px 12px;
  border-radius: 6px;
  text-decoration: none;
  white-space: nowrap;
  transition: all 0.2s ease;
}

.qs-home-header__nav a:hover,
.qs-home-header__nav a.router-link-active {
  color: #2F7785;
  background: rgba(47, 119, 133, 0.06);
}

.qs-home-header__actions {
  display: flex;
  align-items: center;
  gap: 10px;
  flex-shrink: 0;
}

.qs-btn-login {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 500;
  color: #2F7785;
  border: 1.5px solid #2F7785;
  background: transparent;
  border-radius: 6px;
  padding: 7px 20px;
  text-decoration: none;
  transition: all 0.2s ease;
  white-space: nowrap;
}

.qs-btn-login:hover {
  background: #2F7785;
  color: #fff;
}

.qs-btn-cadastro {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 600;
  color: #fff;
  background: #98C73A;
  border: 1.5px solid #98C73A;
  border-radius: 6px;
  padding: 7px 20px;
  text-decoration: none;
  transition: all 0.2s ease;
  white-space: nowrap;
}

.qs-btn-cadastro:hover {
  background: #7aad1f;
  border-color: #7aad1f;
  color: #fff;
}

.qs-mobile-menu-btn {
  display: flex;
  flex-direction: column;
  gap: 5px;
  background: none;
  border: none;
  cursor: pointer;
  padding: 4px;
}

.qs-mobile-menu-btn span {
  display: block;
  width: 24px;
  height: 2px;
  background: #374151;
  border-radius: 2px;
  transition: all 0.2s;
}

.qs-home-header__search {
  background: #f7f8fa;
  padding: 10px 0;
  border-bottom: 1px solid #eee;
}

.qs-home-header__search-inner {
  display: flex;
  align-items: center;
  background: #fff;
  border: 1.5px solid #e5e7eb;
  border-radius: 999px;
  padding: 9px 18px;
  gap: 10px;
  transition: border-color 0.2s;
}

.qs-home-header__search-inner:focus-within {
  border-color: #2F7785;
}

.qs-home-header__search-inner input {
  flex: 1;
  border: none;
  outline: none;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  color: #374151;
  background: transparent;
}

.qs-home-header__search-inner input::placeholder {
  color: #9ca3af;
}

.qs-search-clear {
  background: none;
  border: none;
  cursor: pointer;
  color: #9ca3af;
  font-size: 14px;
  padding: 0;
  line-height: 1;
}

.qs-mobile-menu {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  background: #fff;
  box-shadow: 0 8px 30px rgba(0,0,0,0.12);
  z-index: 999;
}

.qs-mobile-menu nav {
  display: flex;
  flex-direction: column;
  padding: 12px 0;
}

.qs-mobile-menu nav a {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 500;
  color: #374151;
  padding: 12px 24px;
  text-decoration: none;
  border-bottom: 1px solid #f3f4f6;
}

.qs-mobile-menu nav a:hover {
  color: #2F7785;
  background: rgba(47, 119, 133, 0.04);
}

.qs-mobile-menu__actions {
  display: flex;
  gap: 12px;
  padding: 16px 24px;
}
</style>
