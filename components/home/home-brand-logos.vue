<template>
  <section class="qs-brand-logos" v-if="logos.length > 0">
    <div class="container">
      <p class="qs-brand-logos__title">As maiores marcas confiam na Quanta</p>
      <div class="qs-brand-logos__track-wrap">
        <div class="qs-brand-logos__track">
          <div v-for="(logo, i) in [...logos, ...logos]" :key="i" class="qs-brand-logos__item">
            <img
              :src="logo.imagemPequena || logo.imagem"
              :alt="logo.nome"
              @error="(e: any) => e.target.style.display='none'"
            />
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { usePartnerStore } from "@/pinia/usePartnerStore";

const partnerStore = usePartnerStore();

const logos = computed(() => {
  const all = [
    ...(partnerStore.featuredPartners || []),
    ...(partnerStore.newPartners || []),
  ];
  const seen = new Set<string>();
  return all.filter(p => {
    const img = p.imagemPequena || p.imagem;
    if (!img || seen.has(img)) return false;
    seen.add(img);
    return true;
  }).slice(0, 12);
});
</script>

<style scoped>
.qs-brand-logos {
  padding: 32px 0;
  background: #fff;
  border-top: 1px solid #f0f0f0;
  border-bottom: 1px solid #f0f0f0;
  overflow: hidden;
}

.qs-brand-logos__title {
  text-align: center;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  font-weight: 600;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: #9ca3af;
  margin-bottom: 24px;
}

.qs-brand-logos__track-wrap {
  overflow: hidden;
  -webkit-mask-image: linear-gradient(to right, transparent 0%, black 8%, black 92%, transparent 100%);
  mask-image: linear-gradient(to right, transparent 0%, black 8%, black 92%, transparent 100%);
}

.qs-brand-logos__track {
  display: flex;
  gap: 40px;
  align-items: center;
  animation: qs-logos-scroll 28s linear infinite;
  width: max-content;
}

.qs-brand-logos__track:hover {
  animation-play-state: paused;
}

.qs-brand-logos__item {
  flex-shrink: 0;
  opacity: 0.60;
  transition: opacity 0.2s;
}

.qs-brand-logos__item:hover {
  opacity: 1;
}

.qs-brand-logos__item img {
  height: 32px;
  width: auto;
  max-width: 100px;
  object-fit: contain;
  filter: grayscale(0.6);
  transition: filter 0.2s;
}

.qs-brand-logos__item:hover img {
  filter: none;
}

@keyframes qs-logos-scroll {
  from { transform: translateX(0); }
  to { transform: translateX(-50%); }
}
</style>
