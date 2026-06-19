<template>
  <section class="qs-cps" aria-labelledby="qs-cps-title">
    <div class="container qs-cps__wrap">
      <header class="qs-cps__head">
        <div>
          <span class="qs-cps__eyebrow">🎟️ Atualizado todo dia</span>
          <h2 id="qs-cps-title" class="qs-cps__title">Cupons do dia <span class="qs-cps__hl">com cashback por cima</span></h2>
          <p class="qs-cps__sub">Use o cupom e ainda receba cashback de verdade na mesma compra — até o dobro para assinantes Quanta Plus.</p>
        </div>
        <nuxt-link class="qs-cps__all" to="/cupons">Ver todos os cupons →</nuxt-link>
      </header>

      <div class="qs-cps__rail" role="list">
        <div v-for="c in destaque" :key="c.id" class="qs-cps__item" role="listitem">
          <qs-cupom-card :cupom="c" :dias-restantes="diasRestantes(c)" />
        </div>
      </div>

      <nuxt-link class="qs-cps__all qs-cps__all--mobile" to="/cupons">Ver todos os cupons →</nuxt-link>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useCupons } from '@/composables/useCupons';

const { cupons, loadCupons, diasRestantes } = useCupons();
const destaque = computed(() => cupons.value.slice(0, 8));

onMounted(loadCupons);
</script>

<style scoped>
.qs-cps { background: #f4f6f7; padding: 84px 0; }
.qs-cps__head { display: flex; align-items: flex-end; justify-content: space-between; gap: 24px; margin-bottom: 28px; }
.qs-cps__eyebrow { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 13px; font-weight: 700; color: #2F7785; }
.qs-cps__title { font-family: 'Bruum FY', 'Jost', 'Inter', sans-serif; font-weight: 800; color: #1a2332; font-size: clamp(26px, 3.6vw, 40px); line-height: 1.1; letter-spacing: -.02em; margin: 8px 0 8px; }
.qs-cps__hl { color: #7aad1f; }
.qs-cps__sub { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 16px; color: #4a5b60; max-width: 560px; margin: 0; }
.qs-cps__all { flex-shrink: 0; font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-weight: 700; font-size: 14px; color: #225F6B; text-decoration: none; border: 1.5px solid #cdd9dc; border-radius: 999px; padding: 11px 20px; min-height: 44px; display: inline-flex; align-items: center; transition: all .2s ease; }
.qs-cps__all:hover { background: #225F6B; color: #fff; border-color: #225F6B; }
.qs-cps__all--mobile { display: none; margin: 24px auto 0; }

.qs-cps__rail {
  display: grid; grid-auto-flow: column; grid-auto-columns: minmax(280px, 1fr);
  gap: 20px; overflow-x: auto; padding: 6px 4px 18px; scroll-snap-type: x mandatory;
  -webkit-overflow-scrolling: touch; scrollbar-width: thin;
  -webkit-mask-image: linear-gradient(90deg, transparent 0, #000 18px, #000 calc(100% - 18px), transparent 100%);
  mask-image: linear-gradient(90deg, transparent 0, #000 18px, #000 calc(100% - 18px), transparent 100%);
}
.qs-cps__item { scroll-snap-align: start; }

@media (min-width: 1100px) {
  .qs-cps__rail { grid-auto-columns: minmax(0, 1fr); grid-template-columns: repeat(4, 1fr); grid-auto-flow: row; overflow: visible; -webkit-mask-image: none; mask-image: none; }
}

@media (max-width: 720px) {
  .qs-cps { padding: 56px 0; }
  .qs-cps__head { flex-direction: column; align-items: flex-start; gap: 8px; }
  .qs-cps__all { display: none; }
  .qs-cps__all--mobile { display: inline-flex; justify-content: center; }
}
</style>
