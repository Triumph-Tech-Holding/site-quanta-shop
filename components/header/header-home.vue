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

          <nav class="qs-home-header__nav d-none d-xl-flex">
            <nuxt-link href="/para-voce">Para Você</nuxt-link>
            <nuxt-link href="/para-sua-empresa">Para sua Empresa</nuxt-link>
            <nuxt-link href="/seja-um-agente">Seja um Agente</nuxt-link>
            <nuxt-link href="/quanta-amizade" class="qs-nav-icon-link">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
              Quanta Amizade
            </nuxt-link>
            <nuxt-link href="/blog">Blog</nuxt-link>
            <nuxt-link href="/contato" class="qs-nav-icon-link">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="#98C73A"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/></svg>
              Atendimento
            </nuxt-link>
          </nav>

          <div class="qs-home-header__actions">
            <nuxt-link href="/login" class="qs-btn-login">Login</nuxt-link>
            <nuxt-link href="/register" class="qs-btn-cadastro">Cadastro</nuxt-link>
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

    <div v-if="mobileOpen" class="qs-mobile-menu d-xl-none">
      <nav>
        <nuxt-link href="/para-voce" @click="mobileOpen = false">Para Você</nuxt-link>
        <nuxt-link href="/para-sua-empresa" @click="mobileOpen = false">Para sua Empresa</nuxt-link>
        <nuxt-link href="/seja-um-agente" @click="mobileOpen = false">Seja um Agente</nuxt-link>
        <nuxt-link href="/quanta-amizade" @click="mobileOpen = false">Quanta Amizade</nuxt-link>
        <nuxt-link href="/blog" @click="mobileOpen = false">Blog</nuxt-link>
        <nuxt-link href="/contato" @click="mobileOpen = false">Atendimento</nuxt-link>
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
  padding: 5px 0 6px;
  border-bottom: 1px solid #f0f0f0;
}

.qs-home-header__search-inner {
  display: flex;
  align-items: center;
  background: #f7f9fc;
  border: 1.5px solid #e2e8f0;
  border-radius: 999px;
  padding: 0 12px;
  gap: 8px;
  transition: all 0.25s ease;
  max-width: 600px;
  height: 34px;
  margin: 0 auto;
  box-shadow: 0 1px 4px rgba(47, 119, 133, 0.06);
}

.qs-home-header__search-inner:focus-within {
  border-color: #2F7785;
  background: #fff;
  box-shadow: 0 2px 10px rgba(47, 119, 133, 0.12);
}

.qs-home-header__search-inner input {
  flex: 1;
  border: none;
  outline: none;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  line-height: 1;
  color: #374151;
  background: transparent;
  height: 100%;
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

@media (max-width: 575px) {
  .qs-home-header__logo img {
    height: 30px;
  }
  .qs-btn-login,
  .qs-btn-cadastro {
    font-size: 12px;
    padding: 5px 10px;
  }
  .qs-home-header__actions {
    gap: 6px;
  }
}

@media (max-width: 767px) {
  .qs-home-header__main {
    padding: 10px 0;
  }
}
</style>
