<template>
  <section class="v2-brands">
    <div class="v2-brands__container">
      <h2 class="v2-brands__title">Grandes Marcas Que Você Ama, Com Cashback Quanta.</h2>
      <div class="v2-brands__track-wrap">
        <div class="v2-brands__track" ref="trackRef">
          <div
            v-for="(partner, index) in [...displayPartners, ...displayPartners]"
            :key="`${partner.id}-${index}`"
            class="v2-brands__item"
            @click="handleClick(partner)"
            :title="partner.nome"
          >
            <img
              :src="partner.imagem || partner.imagemPequena"
              :alt="partner.nome"
              class="v2-brands__logo"
              @error="(e) => (e.target as HTMLImageElement).style.display = 'none'"
            />
          </div>
        </div>
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

onMounted(async () => {
  if (!partnerStore.isNewPartnersLoaded) {
    await partnerStore.fetchNewPartners()
  }
})

const displayPartners = computed(() => {
  const partners = partnerStore.newPartners
  if (!partners || partners.length === 0) return []
  return partners.filter((p: any) => p.imagem || p.imagemPequena).slice(0, 20)
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
</script>

<style scoped>
.v2-brands {
  background: white;
  padding: 3rem 1rem;
}
@media (min-width: 1024px) { .v2-brands { padding: 4rem 2rem; } }
.v2-brands__container {
  max-width: 1280px;
  margin: 0 auto;
}
.v2-brands__title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #225F6B;
  text-align: center;
  margin-bottom: 2.5rem;
}
@media (min-width: 1024px) { .v2-brands__title { font-size: 1.875rem; } }
.v2-brands__track-wrap {
  overflow: hidden;
  position: relative;
}
.v2-brands__track-wrap::before,
.v2-brands__track-wrap::after {
  content: '';
  position: absolute;
  top: 0;
  bottom: 0;
  width: 80px;
  z-index: 1;
}
.v2-brands__track-wrap::before {
  left: 0;
  background: linear-gradient(to right, white, transparent);
}
.v2-brands__track-wrap::after {
  right: 0;
  background: linear-gradient(to left, white, transparent);
}
.v2-brands__track {
  display: flex;
  animation: v2-scroll 40s linear infinite;
  width: max-content;
}
.v2-brands__track:hover { animation-play-state: paused; }
@keyframes v2-scroll {
  0% { transform: translateX(0); }
  100% { transform: translateX(-50%); }
}
.v2-brands__item {
  flex-shrink: 0;
  padding: 0 2rem;
  cursor: pointer;
  filter: grayscale(100%);
  opacity: 0.5;
  transition: filter 0.3s, opacity 0.3s;
  display: flex;
  align-items: center;
}
.v2-brands__item:hover { filter: grayscale(0); opacity: 1; }
.v2-brands__logo {
  height: 48px;
  width: auto;
  max-width: 120px;
  object-fit: contain;
}
@media (min-width: 1024px) { .v2-brands__logo { height: 64px; max-width: 150px; } }
</style>
