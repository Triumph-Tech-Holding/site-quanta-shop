<template>
  <section class="v2-local">
    <div class="v2-local__map-pattern" aria-hidden="true">
      <svg width="100%" height="100%" viewBox="0 0 100 100" preserveAspectRatio="none">
        <defs>
          <pattern id="v2-map" patternUnits="userSpaceOnUse" width="20" height="20">
            <circle cx="10" cy="10" r="1" fill="#2F7785" />
            <path d="M0 10 L20 10 M10 0 L10 20" stroke="#2F7785" stroke-width="0.2" />
          </pattern>
        </defs>
        <rect fill="url(#v2-map)" width="100%" height="100%" />
      </svg>
    </div>

    <div class="v2-local__container">
      <div class="v2-local__header">
        <div>
          <h2 class="v2-local__title">Ganhe Consumindo Localmente.</h2>
          <p class="v2-local__subtitle">Estabelecimentos perto de você com cashback exclusivo</p>
        </div>
        <NuxtLink to="/partners" class="v2-local__map-link">
          Ver todos
          <svg width="20" height="20" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
          </svg>
        </NuxtLink>
      </div>

      <div v-if="loading" class="v2-local__grid">
        <div v-for="i in 3" :key="i" class="v2-local__skeleton"></div>
      </div>

      <div v-else-if="displayPartners.length === 0" class="v2-local__empty">
        <p>Parceiros locais serão exibidos em breve.</p>
      </div>

      <div v-else class="v2-local__grid">
        <div
          v-for="partner in displayPartners"
          :key="partner.id"
          class="v2-local__card"
        >
          <div class="v2-local__card-image">
            <img
              :src="partner.imagem || partner.imagemPequena"
              :alt="partner.nome"
              class="v2-local__card-img"
              @error="(e) => (e.target as HTMLImageElement).src = '/img/ui/no-image.png'"
            />
            <div class="v2-local__cashback-badge">
              {{ partner.percentualCashback }}% Cashback
            </div>
            <div v-if="partner.bairro || partner.cidade" class="v2-local__location-badge">
              {{ partner.bairro || partner.cidade }}
            </div>
          </div>
          <div class="v2-local__card-body">
            <h3 class="v2-local__card-name">{{ partner.nome }}</h3>
            <p v-if="partner.cidade" class="v2-local__card-address">
              {{ [partner.bairro, partner.cidade, partner.estado].filter(Boolean).join(', ') }}
            </p>
            <a
              v-if="partner.whatsapp"
              :href="`https://wa.me/55${partner.whatsapp.replace(/\D/g, '')}`"
              target="_blank"
              rel="noopener noreferrer"
              class="v2-local__card-link"
            >
              <svg width="18" height="18" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
              </svg>
              Entrar em Contato
            </a>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { usePartnerStore } from '@/pinia/usePartnerStore'

const partnerStore = usePartnerStore()
const loading = ref(true)

onMounted(async () => {
  if (!partnerStore.isLocalPartnersLoaded) {
    await partnerStore.fetchLocalPartners()
  }
  loading.value = false
})

const displayPartners = computed(() => {
  return (partnerStore.localPartners || []).slice(0, 6)
})
</script>

<style scoped>
.v2-local {
  background: white;
  padding: 4rem 1rem;
  position: relative;
  overflow: hidden;
}
@media (min-width: 1024px) { .v2-local { padding: 5rem 2rem; } }
.v2-local__map-pattern {
  position: absolute;
  inset: 0;
  opacity: 0.04;
  pointer-events: none;
}
.v2-local__container {
  max-width: 1280px;
  margin: 0 auto;
  position: relative;
}
.v2-local__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}
.v2-local__title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #225F6B;
  margin-bottom: 0.25rem;
}
@media (min-width: 1024px) { .v2-local__title { font-size: 1.875rem; } }
.v2-local__subtitle { color: #6b7280; font-size: 0.95rem; }
.v2-local__map-link {
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
  color: #2F7785;
  font-weight: 600;
  text-decoration: none;
  white-space: nowrap;
  transition: color 0.2s;
  flex-shrink: 0;
}
.v2-local__map-link:hover { color: #225F6B; }
.v2-local__grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
}
@media (min-width: 640px) { .v2-local__grid { grid-template-columns: repeat(2, 1fr); } }
@media (min-width: 1024px) { .v2-local__grid { grid-template-columns: repeat(3, 1fr); } }
.v2-local__card {
  background: white;
  border-radius: 1.25rem;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  transition: box-shadow 0.3s;
  border: 1px solid #f3f4f6;
}
.v2-local__card:hover { box-shadow: 0 12px 30px rgba(0,0,0,0.12); }
.v2-local__card-image {
  position: relative;
  height: 160px;
  overflow: hidden;
  background: #f9fafb;
}
.v2-local__card-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
.v2-local__cashback-badge {
  position: absolute;
  top: 0.75rem;
  right: 0.75rem;
  background: #98C73A;
  color: white;
  font-size: 0.8rem;
  font-weight: 700;
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
}
.v2-local__location-badge {
  position: absolute;
  bottom: 0.75rem;
  left: 0.75rem;
  background: rgba(255,255,255,0.9);
  backdrop-filter: blur(4px);
  color: #374151;
  font-size: 0.75rem;
  font-weight: 600;
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
}
.v2-local__card-body { padding: 1rem; }
.v2-local__card-name {
  font-weight: 600;
  font-size: 1rem;
  color: #1f2937;
  margin-bottom: 0.25rem;
}
.v2-local__card-address {
  color: #9ca3af;
  font-size: 0.85rem;
  margin-bottom: 1rem;
}
.v2-local__card-link {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  color: #2F7785;
  font-weight: 600;
  font-size: 0.9rem;
  text-decoration: none;
  transition: color 0.2s;
}
.v2-local__card-link:hover { color: #225F6B; }
.v2-local__skeleton {
  height: 280px;
  background: linear-gradient(90deg, #f0f0f0 25%, #e8e8e8 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: shimmer 1.5s infinite;
  border-radius: 1.25rem;
}
@keyframes shimmer {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
.v2-local__empty {
  text-align: center;
  color: #9ca3af;
  padding: 3rem;
}
</style>
