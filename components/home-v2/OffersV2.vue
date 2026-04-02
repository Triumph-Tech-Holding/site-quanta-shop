<template>
  <section class="v2-offers">
    <div class="v2-offers__container">
      <div class="v2-offers__header">
        <h2 class="v2-offers__title">Parceiros com Maior Cashback Hoje.</h2>
        <NuxtLink to="/partners" class="v2-offers__see-all">
          Ver todos
          <svg width="20" height="20" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
          </svg>
        </NuxtLink>
      </div>

      <div v-if="loading" class="v2-offers__loading">
        <div v-for="i in 4" :key="i" class="v2-offers__skeleton"></div>
      </div>

      <div v-else class="v2-offers__grid">
        <div
          v-for="partner in displayPartners"
          :key="partner.id"
          class="v2-offers__card"
          @click="handleClick(partner)"
        >
          <div class="v2-offers__card-image">
            <img
              :src="partner.imagem || partner.imagemPequena"
              :alt="partner.nome"
              class="v2-offers__card-img"
              @error="(e) => handleImgError(e)"
            />
            <div class="v2-offers__cashback-badge">
              Até {{ partner.percentualCashback }}% Cashback
            </div>
          </div>
          <div class="v2-offers__card-body">
            <p class="v2-offers__card-category">{{ partner.categoria || 'Parceiro Quanta' }}</p>
            <h3 class="v2-offers__card-name">{{ partner.nome }}</h3>
            <button class="v2-offers__card-btn">Aproveitar Agora</button>
          </div>
        </div>
      </div>

      <div class="v2-offers__mobile-all">
        <NuxtLink to="/partners" class="v2-offers__see-all">
          Ver todos os parceiros
          <svg width="20" height="20" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
          </svg>
        </NuxtLink>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { usePartnerStore } from '@/pinia/usePartnerStore'
import { useUserStore } from '@/pinia/useUserStore'

const partnerStore = usePartnerStore()
const userStore = useUserStore()
const router = useRouter()
const loading = ref(true)

onMounted(async () => {
  if (!partnerStore.isFeaturedPartnersLoaded) {
    await partnerStore.fetchFeaturedPartners()
  }
  loading.value = false
})

const displayPartners = computed(() => {
  return (partnerStore.featuredPartners || []).slice(0, 8)
})

function handleClick(partner: any) {
  if (!userStore.isLoggedIn) {
    router.push('/login')
    return
  }
  if (partner.link) {
    const userId = userStore.userId
    const url = partner.link.replace ? partner.link.replace('{userId}', userId || '') : partner.link
    window.open(url, '_blank', 'noopener,noreferrer')
  }
}

function handleImgError(e: Event) {
  const img = e.target as HTMLImageElement
  img.src = '/img/ui/no-image.png'
}
</script>

<style scoped>
.v2-offers {
  background: #F4F4F5;
  padding: 3rem 1rem;
}
@media (min-width: 1024px) { .v2-offers { padding: 5rem 2rem; } }
.v2-offers__container {
  max-width: 1280px;
  margin: 0 auto;
}
.v2-offers__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}
.v2-offers__title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #225F6B;
  margin: 0;
}
@media (min-width: 1024px) { .v2-offers__title { font-size: 1.875rem; } }
.v2-offers__see-all {
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
  color: #2F7785;
  font-weight: 600;
  text-decoration: none;
  transition: color 0.2s;
}
.v2-offers__see-all:hover { color: #225F6B; }
.v2-offers__grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
}
@media (min-width: 768px) { .v2-offers__grid { grid-template-columns: repeat(3, 1fr); gap: 1.5rem; } }
@media (min-width: 1024px) { .v2-offers__grid { grid-template-columns: repeat(4, 1fr); gap: 1.5rem; } }
.v2-offers__card {
  background: white;
  border-radius: 1.25rem;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  transition: box-shadow 0.3s, transform 0.2s;
  cursor: pointer;
}
.v2-offers__card:hover {
  box-shadow: 0 12px 30px rgba(0,0,0,0.15);
  transform: translateY(-2px);
}
.v2-offers__card-image {
  position: relative;
  height: 160px;
  overflow: hidden;
  background: #f9fafb;
}
.v2-offers__card-img {
  width: 100%;
  height: 100%;
  object-fit: contain;
  padding: 1rem;
  transition: transform 0.3s;
}
.v2-offers__card:hover .v2-offers__card-img { transform: scale(1.05); }
.v2-offers__cashback-badge {
  position: absolute;
  top: 0.75rem;
  right: 0.75rem;
  background: #98C73A;
  color: white;
  font-size: 0.75rem;
  font-weight: 700;
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
}
.v2-offers__card-body {
  padding: 1rem;
}
.v2-offers__card-category {
  font-size: 0.75rem;
  color: #9ca3af;
  margin-bottom: 0.25rem;
}
.v2-offers__card-name {
  font-weight: 600;
  color: #1f2937;
  font-size: 0.95rem;
  margin-bottom: 1rem;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
.v2-offers__card-btn {
  display: block;
  width: 100%;
  background: #2F7785;
  color: white;
  font-weight: 600;
  font-size: 0.9rem;
  padding: 0.6rem 1rem;
  border-radius: 9999px;
  border: none;
  cursor: pointer;
  text-align: center;
  transition: background 0.2s;
}
.v2-offers__card-btn:hover { background: #225F6B; }
.v2-offers__loading {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
}
@media (min-width: 1024px) { .v2-offers__loading { grid-template-columns: repeat(4, 1fr); } }
.v2-offers__skeleton {
  height: 240px;
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: shimmer 1.5s infinite;
  border-radius: 1.25rem;
}
@keyframes shimmer {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
.v2-offers__mobile-all {
  text-align: center;
  margin-top: 1.5rem;
  display: block;
}
@media (min-width: 1024px) { .v2-offers__mobile-all { display: none; } }
</style>
