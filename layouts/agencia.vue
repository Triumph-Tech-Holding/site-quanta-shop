<template>
  <div class="agencia-portal">
    <header class="ag-portal-header">
      <div class="container d-flex align-items-center justify-content-between py-2">
        <NuxtLink to="/agencia">
          <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" style="height:40px" />
        </NuxtLink>
        <nav class="d-flex gap-3 align-items-center flex-wrap">
          <NuxtLink to="/agencia/quem-somos" class="ag-nav-link">Quem Somos</NuxtLink>
          <NuxtLink to="/agencia/como-funciona" class="ag-nav-link">Como Funciona</NuxtLink>
          <NuxtLink to="/agencia/faq" class="ag-nav-link">FAQ</NuxtLink>
          <NuxtLink to="/agencia/lojas-fisicas" class="ag-nav-link">Lojas</NuxtLink>
          <NuxtLink to="/agencia/login" class="btn btn-sm btn-ag-outline px-3">Entrar</NuxtLink>
          <NuxtLink to="/agencia/cadastro" class="btn btn-sm btn-ag-primary px-3">Cadastrar</NuxtLink>
        </nav>
      </div>
    </header>
    <main>
      <slot />
    </main>
    <footer class="ag-portal-footer mt-auto py-4 text-center">
      <div class="container">
        <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" style="height:32px; opacity:.7" class="mb-2 d-block mx-auto" />
        <div class="d-flex justify-content-center gap-3 flex-wrap mb-2" style="font-size:.85rem">
          <NuxtLink to="/agencia/quem-somos" class="ag-nav-link">Quem Somos</NuxtLink>
          <NuxtLink to="/agencia/como-funciona" class="ag-nav-link">Como Funciona</NuxtLink>
          <NuxtLink to="/agencia/privacidade" class="ag-nav-link">Privacidade</NuxtLink>
          <NuxtLink to="/agencia/faq" class="ag-nav-link">FAQ</NuxtLink>
        </div>
        <p class="mb-0 text-muted" style="font-size:.8rem">&copy; {{ new Date().getFullYear() }} Quanta Shop. Todos os direitos reservados.</p>
      </div>
    </footer>
  </div>
</template>

<style scoped>
.agencia-portal { display:flex; flex-direction:column; min-height:100vh; background:#ecf2f7; }
.ag-portal-header { background:#fff; box-shadow:0 1px 4px rgba(0,0,0,.08); position:sticky; top:0; z-index:100; }
.ag-nav-link { color:#2f7785; text-decoration:none; font-size:.9rem; font-weight:500; }
.ag-nav-link:hover { color:#98c73a; }
.ag-portal-footer { background:#fff; border-top:1px solid #dee2e6; }
</style>
