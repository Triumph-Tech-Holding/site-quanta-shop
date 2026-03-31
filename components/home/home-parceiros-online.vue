<template>
  <section class="qs-parceiros-online">
    <div class="container">
      <div class="qs-section-header">
        <span class="qs-section-label">{{ config.parceirosOnline.label }}</span>
        <h2 class="qs-section-title">{{ config.parceirosOnline.title }}</h2>
        <p class="qs-section-sub">{{ config.parceirosOnline.subtitle }}</p>
      </div>

      <div v-if="loading" class="qs-parceiros-online__grid">
        <div v-for="n in 8" :key="n" class="qs-partner-card qs-partner-card--skeleton">
          <div class="qs-skeleton" style="height:64px;width:80px;border-radius:8px;margin:0 auto 12px;"></div>
          <div class="qs-skeleton" style="height:14px;width:80%;border-radius:4px;margin:0 auto 6px;"></div>
          <div class="qs-skeleton" style="height:12px;width:60%;border-radius:4px;margin:0 auto 16px;"></div>
          <div class="qs-skeleton" style="height:36px;width:100%;border-radius:8px;"></div>
        </div>
      </div>

      <div v-else class="qs-parceiros-online__grid">
        <div
          v-for="item in displayedPartners"
          :key="item.id"
          class="qs-partner-card"
        >
          <div class="qs-partner-card__logo">
            <img :src="item.imagemPequena || item.imagem || '/img/placeholder.png'" :alt="item.nome" />
          </div>
          <h3 class="qs-partner-card__name">{{ item.nome }}</h3>
          <p class="qs-partner-card__cashback">Até {{ item.percentualCashback || '?' }}% de cashback</p>
          <button @click="handlePartnerClick(item)" class="qs-partner-card__btn">
            Ative seu cashback
          </button>
        </div>
      </div>

      <div v-if="isEmpty" class="qs-parceiros-online__empty">
        <p>Nenhum parceiro disponível no momento. Volte em breve!</p>
      </div>

      <div v-if="!loading && displayedPartners.length > 0" class="qs-parceiros-online__more">
        <nuxt-link href="/partners" class="qs-btn-outline-primary">
          Ver todos os parceiros
          <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </nuxt-link>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { usePartnerStore } from "@/pinia/usePartnerStore";
import { useHomeConfig } from '@/composables/useHomeConfig';
import { useAgenciaStore } from '@/pinia/useAgenciaStore';

const { config, loadConfig } = useHomeConfig();
const partnerStore = usePartnerStore();
const agenciaStore = useAgenciaStore();

const isLoading = ref(true);

onMounted(async () => {
  agenciaStore.loadFromStorage();
  loadConfig();
  try {
    await partnerStore.fetchNewPartners();
  } catch {
    console.warn('[home-parceiros-online] Falha ao carregar parceiros online');
  } finally {
    isLoading.value = false;
  }
});

const loading = computed(() => isLoading.value);
const displayedPartners = computed(() => (partnerStore.newPartners || []));
const isEmpty = computed(() => !isLoading.value && displayedPartners.value.length === 0);

function handlePartnerClick(item: any) {
  if (!agenciaStore.isLoggedIn) {
    navigateTo('/login');
    return;
  }
  
  if (item.link) {
    const url = item.link.replace('{userId}', String(agenciaStore.user?.idUsuario || ''));
    window.open(url, '_blank', 'noopener');
  }
}
</script>

<style scoped>
.qs-parceiros-online {
  padding: 48px 0;
  background: #f7f8fa;
}

.qs-section-header {
  text-align: center;
  margin-bottom: 40px;
}

.qs-section-label {
  display: inline-block;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: #98C73A;
  margin-bottom: 10px;
}

.qs-section-title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(24px, 4vw, 36px);
  font-weight: 800;
  color: #225F6B;
  letter-spacing: -0.03em;
  margin-bottom: 8px;
}

.qs-section-sub {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  color: #2F7785;
  max-width: 500px;
  margin: 0 auto;
}

.qs-parceiros-online__grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
  margin-bottom: 36px;
}

@media (max-width: 991px) { .qs-parceiros-online__grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 575px) { .qs-parceiros-online__grid { grid-template-columns: 1fr; } }

.qs-partner-card {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 20px 16px;
  text-align: center;
  transition: all 0.25s ease;
}

.qs-partner-card:hover {
  border-color: #2F7785;
  box-shadow: 0 12px 32px rgba(47, 119, 133, 0.15);
  transform: translateY(-4px);
}

.qs-partner-card--skeleton {
  pointer-events: none;
}

.qs-partner-card__logo {
  height: 90px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 12px;
}

.qs-partner-card__logo img {
  max-height: 80px;
  max-width: 100%;
  object-fit: contain;
}

.qs-partner-card__name {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 700;
  color: #225F6B;
  margin-bottom: 4px;
}

.qs-partner-card__cashback {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  color: #2F7785;
  margin-bottom: 14px;
}

.qs-partner-card__btn {
  display: block;
  border: 1.5px solid #98C73A;
  color: #2F7785;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  font-weight: 600;
  padding: 8px 12px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.qs-partner-card__btn:hover {
  background: #98C73A;
  border-color: #98C73A;
  color: #fff;
}

.qs-parceiros-online__more {
  text-align: center;
}

.qs-btn-outline-primary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  border: 2px solid #2F7785;
  color: #2F7785;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 600;
  padding: 12px 28px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.qs-btn-outline-primary:hover {
  background: #2F7785;
  color: #fff;
}

.qs-parceiros-online__empty {
  text-align: center;
  padding: 40px 20px;
  color: #2F7785;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  opacity: 0.7;
}

.qs-skeleton {
  background: linear-gradient(90deg, #e8edf0 25%, #f5f8fa 50%, #e8edf0 75%);
  background-size: 200% 100%;
  animation: qs-skeleton-wave 1.5s infinite;
}

@keyframes qs-skeleton-wave {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
</style>
