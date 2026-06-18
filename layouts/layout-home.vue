<template>
  <div class="qs-layout-home">
    <a class="qs-layout-home__skip" href="#main">Pular para o conteúdo</a>
    <header-home />
    <main id="main">
      <slot />
    </main>
    <footer-home />
  </div>
</template>

<script setup lang="ts"></script>

<style scoped>
.qs-layout-home { overflow-x: hidden; }
.qs-layout-home__skip { position: absolute; left: -999px; top: 0; background: #98C73A; color: #225F6B; padding: 10px 16px; border-radius: 0 0 6px 0; z-index: 2000; font-weight: 700; font-family: 'Kiye Sans','Inter','Jost',sans-serif; }
.qs-layout-home__skip:focus { left: 0; }
</style>
