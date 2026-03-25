<template>
  <header class="qs-home-header">
    <div class="qs-home-header__main">
      <div class="container">
        <div class="qs-home-header__row">
          <div class="qs-home-header__logo">
            <nuxt-link href="/">
              <img src="/img/logo/logo-trimmed.png" alt="Quanta Shop" />
            </nuxt-link>
          </div>

          <nav class="qs-home-header__nav d-none d-md-flex">
            <nuxt-link href="/">Para Você</nuxt-link>
            <nuxt-link href="/para-sua-empresa">Para sua Empresa</nuxt-link>
            <nuxt-link href="/seja-um-agente">Seja um Agente</nuxt-link>
            <nuxt-link href="/quanta-amizade" class="qs-nav-icon-link">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
              Quanta Amizade
            </nuxt-link>
            <nuxt-link href="/blog">Blog</nuxt-link>
            <nuxt-link href="/contato" class="qs-nav-icon-link">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07A19.5 19.5 0 0 1 4.69 13a19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 3.6 2h3a2 2 0 0 1 2 1.72c.127.96.361 1.903.7 2.81a2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0 1 22 16.92z"/></svg>
              Fale Conosco
            </nuxt-link>
          </nav>

          <div class="qs-home-header__actions">
            <nuxt-link href="/login" class="qs-btn-login">Login</nuxt-link>
            <nuxt-link href="/register" class="qs-btn-cadastro">Cadastro</nuxt-link>
            <button class="qs-mobile-menu-btn d-md-none" @click="toggleMobile">
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
          <button class="qs-search-mic" aria-label="Busca por voz">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#9ca3af" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M12 1a3 3 0 0 0-3 3v8a3 3 0 0 0 6 0V4a3 3 0 0 0-3-3z"/>
              <path d="M19 10v2a7 7 0 0 1-14 0v-2"/>
              <line x1="12" y1="19" x2="12" y2="23"/>
              <line x1="8" y1="23" x2="16" y2="23"/>
            </svg>
          </button>
        </div>
      </div>
    </div>

    <div v-if="mobileOpen" class="qs-mobile-menu d-md-none">
      <nav>
        <nuxt-link href="/" @click="mobileOpen = false">Para Você</nuxt-link>
        <nuxt-link href="/para-sua-empresa" @click="mobileOpen = false">Para sua Empresa</nuxt-link>
        <nuxt-link href="/seja-um-agente" @click="mobileOpen = false">Seja um Agente</nuxt-link>
        <nuxt-link href="/quanta-amizade" @click="mobileOpen = false">Quanta Amizade</nuxt-link>
        <nuxt-link href="/blog" @click="mobileOpen = false">Blog</nuxt-link>
        <nuxt-link href="/contato" @click="mobileOpen = false">Fale Conosco</nuxt-link>
        <div class="qs-mobile-menu__actions">
          <nuxt-link href="/login" class="qs-btn-login" @click="mobileOpen = false">Login</nuxt-link>
          <nuxt-link href="/register" class="qs-btn-cadastro" @click="mobileOpen = false">Cadastro</nuxt-link>
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
  gap: 12px;
}

.qs-home-header__logo img {
  height: 38px;
  width: auto;
}

.qs-home-header__nav {
  flex: 1;
  align-items: center;
  gap: 4px;
}

.qs-home-header__nav a {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 500;
  color: #374151;
  padding: 6px 8px;
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

.qs-nav-icon-link {
  display: inline-flex !important;
  align-items: center;
  gap: 5px;
}

.qs-home-header__actions {
  display: flex;
  align-items: center;
  gap: 10px;
  flex-shrink: 0;
}

.qs-btn-login {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 500;
  color: #2F7785;
  border: 1.5px solid #2F7785;
  background: transparent;
  border-radius: 6px;
  padding: 6px 14px;
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
  font-size: 13px;
  font-weight: 600;
  color: #fff;
  background: #98C73A;
  border: 1.5px solid #98C73A;
  border-radius: 6px;
  padding: 6px 14px;
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
  background: #fff;
  padding: 4px 0;
  border-bottom: 1px solid #f0f0f0;
}

.qs-home-header__search-inner {
  display: flex;
  align-items: center;
  background: #f7f9fc;
  border: 1.5px solid #e2e8f0;
  border-radius: 999px;
  padding: 4px 16px;
  gap: 10px;
  transition: all 0.25s ease;
  max-width: 640px;
  margin: 0 auto;
  box-shadow: 0 2px 8px rgba(47, 119, 133, 0.06);
}

.qs-home-header__search-inner:focus-within {
  border-color: #2F7785;
  background: #fff;
  box-shadow: 0 4px 16px rgba(47, 119, 133, 0.14);
}

.qs-home-header__search-inner input {
  flex: 1;
  border: none;
  outline: none;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
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

.qs-search-mic {
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
  line-height: 1;
  display: flex;
  align-items: center;
  opacity: 0.6;
  transition: opacity 0.2s;
}

.qs-search-mic:hover {
  opacity: 1;
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
