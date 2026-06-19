<template>
  <div class="qs-cpg">
    <section class="qs-cpg__hero">
      <div class="container">
        <span class="qs-cpg__eyebrow">🎟️ Cupons Quanta Shop</span>
        <h1 class="qs-cpg__h1">Cupons e promoções <span class="qs-cpg__hl">com cashback de verdade</span></h1>
        <p class="qs-cpg__lead">Os melhores códigos de desconto das suas lojas favoritas — e você ainda recebe cashback por cima, sacável via PIX.</p>

        <form class="qs-cpg__search" role="search" @submit.prevent>
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" aria-hidden="true"><circle cx="11" cy="11" r="7" /><path d="m21 21-4.3-4.3" /></svg>
          <input v-model="q" type="search" class="qs-cpg__input" aria-label="Buscar cupom por loja ou oferta" placeholder="Buscar por loja ou oferta (ex.: Carrefour, jeans, frete grátis)" />
        </form>

        <div class="qs-cpg__chips" role="tablist" aria-label="Filtrar por categoria">
          <button
            v-for="cat in categories" :key="cat" type="button" role="tab"
            class="qs-cpg__chip" :class="{ active: cat === categoria }"
            :aria-selected="cat === categoria" @click="categoria = cat"
          >{{ cat }}</button>
        </div>
      </div>
    </section>

    <section class="qs-cpg__list">
      <div class="container">
        <p class="qs-cpg__count">
          {{ filtrados.length }} {{ filtrados.length === 1 ? 'cupom encontrado' : 'cupons encontrados' }}<span v-if="categoria !== 'Todos'"> em {{ categoria }}</span>
        </p>

        <div v-if="filtrados.length" class="qs-cpg__grid">
          <qs-cupom-card v-for="c in filtrados" :key="c.id" :cupom="c" :dias-restantes="diasRestantes(c)" />
        </div>
        <div v-else class="qs-cpg__empty">
          <p>Nenhum cupom para esse filtro agora. 😕</p>
          <button type="button" class="qs-cpg__reset" @click="resetar">Limpar filtros</button>
        </div>

        <p class="qs-cpg__note">* "Até X% cashback" indica o teto: o percentual varia conforme o momento e o tipo de conta — assinantes Quanta Plus recebem o dobro. Cupom e cashback acumulam quando a loja permite. Validade e regras conforme cada oferta; a lista atualiza automaticamente.</p>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useCupons } from '@/composables/useCupons';

definePageMeta({ layout: 'layout-home' });

const { cupons, loadCupons, diasRestantes, categories } = useCupons();
const q = ref('');
const categoria = ref<string>('Todos');

onMounted(loadCupons);

const filtrados = computed(() => {
  const term = q.value.trim().toLowerCase();
  return cupons.value.filter((c) => {
    const okCat = categoria.value === 'Todos' || c.category === categoria.value;
    const okTerm = !term || `${c.store.name} ${c.title} ${c.discount} ${c.category}`.toLowerCase().includes(term);
    return okCat && okTerm;
  });
});

function resetar() { q.value = ''; categoria.value = 'Todos'; }

const SITE = 'https://quantashop.com.br';
useSeoMeta({
  title: 'Cupons de desconto com cashback | Quanta Shop',
  description: 'Cupons e promoções das melhores lojas com cashback de verdade por cima — sacável via PIX. Atualizados todos os dias na Quanta Shop.',
  ogTitle: 'Cupons de desconto com cashback | Quanta Shop',
  ogDescription: 'Use o cupom e ainda receba cashback. Veja os códigos do dia das suas lojas favoritas.',
  ogImage: `${SITE}/og-cover.jpg`,
  ogType: 'website',
});
useHead({
  link: [{ rel: 'canonical', href: `${SITE}/cupons` }],
  script: [{
    type: 'application/ld+json',
    innerHTML: JSON.stringify({
      '@context': 'https://schema.org',
      '@type': 'ItemList',
      name: 'Cupons e promoções — Quanta Shop',
      itemListElement: cupons.value.slice(0, 20).map((c, i) => ({
        '@type': 'ListItem',
        position: i + 1,
        item: {
          '@type': 'Offer',
          name: `${c.discount} em ${c.store.name}`,
          description: c.title,
          category: c.category,
          ...(c.code ? { priceSpecification: { '@type': 'UnitPriceSpecification' } } : {}),
          availabilityEnds: c.expiresAt,
          seller: { '@type': 'Organization', name: c.store.name },
        },
      })),
    }),
  }],
});
</script>

<style scoped>
.qs-cpg__hero {
  color: #eaf3f5; padding: clamp(48px, 7vw, 84px) 0 clamp(36px, 5vw, 56px);
  background:
    radial-gradient(900px 500px at 80% -10%, rgba(58, 154, 173, .5), transparent 60%),
    linear-gradient(160deg, #0f2730 0%, #1a2332 55%, #0c1c24 100%);
}
.qs-cpg__eyebrow { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 13px; font-weight: 700; color: #9fd3a6; }
.qs-cpg__h1 { font-family: 'Bruum FY', 'Jost', 'Inter', sans-serif; font-weight: 800; color: #fff; font-size: clamp(28px, 5vw, 50px); line-height: 1.08; letter-spacing: -.02em; margin: 10px 0 12px; }
.qs-cpg__hl { color: #98C73A; }
.qs-cpg__lead { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: clamp(15px, 2vw, 18px); line-height: 1.55; color: #c8dde0; max-width: 620px; margin: 0 0 26px; }

.qs-cpg__search { display: flex; align-items: center; gap: 10px; background: #fff; border-radius: 16px; padding: 4px 6px 4px 16px; max-width: 620px; box-shadow: 0 18px 44px rgba(1, 15, 28, .28); }
.qs-cpg__search svg { width: 20px; height: 20px; color: #6b7c81; flex-shrink: 0; }
.qs-cpg__input { flex: 1; min-width: 0; border: 0; outline: 0; background: transparent; font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 16px; color: #1a2332; padding: 13px 8px; }
.qs-cpg__input::placeholder { color: #9ca3af; }

.qs-cpg__chips { display: flex; flex-wrap: wrap; gap: 8px; margin-top: 18px; }
.qs-cpg__chip { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 13px; font-weight: 600; color: #dff3e0; background: rgba(255, 255, 255, .08); border: 1px solid rgba(255, 255, 255, .18); border-radius: 999px; padding: 8px 15px; min-height: 38px; cursor: pointer; transition: all .2s ease; }
.qs-cpg__chip:hover { background: rgba(255, 255, 255, .16); }
.qs-cpg__chip.active { background: #98C73A; border-color: #98C73A; color: #173a0a; font-weight: 700; }

.qs-cpg__list { background: #f4f6f7; padding: clamp(36px, 5vw, 64px) 0 clamp(56px, 7vw, 88px); }
.qs-cpg__count { font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 14px; color: #4a5b60; margin: 0 0 20px; }
.qs-cpg__grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(270px, 1fr)); gap: 22px; }

.qs-cpg__empty { text-align: center; padding: 48px 0; font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; color: #4a5b60; }
.qs-cpg__reset { margin-top: 12px; border: 0; cursor: pointer; background: #225F6B; color: #fff; border-radius: 999px; padding: 11px 22px; font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-weight: 700; font-size: 14px; min-height: 44px; }

.qs-cpg__note { margin: 28px 0 0; font-family: 'Kiye Sans', 'Inter', 'Jost', sans-serif; font-size: 12px; color: #8a9ba0; line-height: 1.5; }
</style>
