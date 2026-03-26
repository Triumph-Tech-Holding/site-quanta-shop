<template>
  <section class="qs-locais">
    <div class="container">
      <div class="qs-section-header">
        <span class="qs-section-label">{{ config.parceirosLocais.label }}</span>
        <h2 class="qs-section-title">{{ config.parceirosLocais.title }}</h2>
        <p class="qs-section-sub">{{ config.parceirosLocais.subtitle }}</p>
      </div>

      <div v-if="loading" class="qs-locais__grid">
        <div v-for="n in 6" :key="n" class="qs-local-card qs-local-card--skeleton">
          <div class="qs-skeleton" style="height:14px;width:40%;border-radius:4px;margin-bottom:8px;"></div>
          <div class="qs-skeleton" style="height:18px;width:70%;border-radius:4px;margin-bottom:6px;"></div>
          <div class="qs-skeleton" style="height:13px;width:50%;border-radius:4px;margin-bottom:16px;"></div>
          <div class="qs-skeleton" style="height:13px;width:60%;border-radius:4px;margin-bottom:16px;"></div>
          <div class="qs-skeleton" style="height:36px;width:100%;border-radius:8px;"></div>
        </div>
      </div>

      <div v-else class="qs-locais__grid">
        <div
          v-for="item in displayedLocais"
          :key="item.id"
          class="qs-local-card"
        >
          <div class="qs-local-card__whatsapp">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="#25D366"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/></svg>
            WhatsApp
          </div>
          <div class="qs-local-card__logo" v-if="item.imagemPequena || item.imagem">
            <img :src="item.imagemPequena || item.imagem" :alt="item.nome" />
          </div>
          <div class="qs-local-card__logo qs-local-card__logo--placeholder" v-else>
            <svg width="28" height="28" fill="none" viewBox="0 0 24 24" stroke="#9ca3af" stroke-width="1.5"><path d="M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2z"/></svg>
          </div>
          <p class="qs-local-card__cashback">Até {{ item.percentualCashback || '?' }}% de cashback</p>
          <h3 class="qs-local-card__name">{{ item.nome }}</h3>
          <p class="qs-local-card__location" v-if="item.bairro || item.cidade">
            <svg width="12" height="12" fill="none" viewBox="0 0 24 24" stroke="#6b7280" stroke-width="2"><path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0118 0z"/><circle cx="12" cy="10" r="3"/></svg>
            {{ item.bairro ? item.bairro + ' — ' : '' }}{{ item.cidade }}<span v-if="item.estado">, {{ item.estado }}</span>
          </p>
          <a :href="item.whatsapp || '#'" target="_blank" rel="noopener" class="qs-local-card__btn">
            Ative seu cashback
          </a>
        </div>
      </div>

      <div v-if="!loading" class="qs-locais__more">
        <nuxt-link href="/partners?tipo=LOCAL" class="qs-btn-outline-primary">
          Ver parceiros locais
          <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </nuxt-link>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { usePartnerStore } from "@/pinia/usePartnerStore";
import { useHomeConfig } from '@/composables/useHomeConfig';

const { config, loadConfig } = useHomeConfig();
const partnerStore = usePartnerStore();
const loading = computed(() => !partnerStore.localPartners || partnerStore.localPartners.length === 0);
const displayedLocais = computed(() => (partnerStore.localPartners || []).slice(0, 6));

onMounted(() => loadConfig());
</script>

<style scoped>
.qs-locais {
  padding: 72px 0;
  background: #fff;
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
  max-width: 480px;
  margin: 0 auto;
}

.qs-locais__grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
  margin-bottom: 36px;
}

@media (max-width: 991px) { .qs-locais__grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 575px) { .qs-locais__grid { grid-template-columns: 1fr; } }

.qs-local-card {
  position: relative;
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 20px;
  transition: all 0.25s ease;
}

.qs-local-card:hover {
  border-color: #2F7785;
  box-shadow: 0 12px 32px rgba(47, 119, 133, 0.12);
  transform: translateY(-4px);
}

.qs-local-card--skeleton {
  pointer-events: none;
}

.qs-local-card__whatsapp {
  position: absolute;
  top: 16px;
  right: 16px;
  display: flex;
  align-items: center;
  gap: 5px;
  background: rgba(37, 211, 102, 0.08);
  color: #1a9040;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 600;
  padding: 4px 10px;
  border-radius: 999px;
}

.qs-local-card__logo {
  width: 52px;
  height: 52px;
  border-radius: 10px;
  overflow: hidden;
  margin-bottom: 10px;
  background: #f3f4f6;
  display: flex;
  align-items: center;
  justify-content: center;
}

.qs-local-card__logo img {
  width: 100%;
  height: 100%;
  object-fit: contain;
}

.qs-local-card__logo--placeholder {
  background: #f3f4f6;
}

.qs-local-card__cashback {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 600;
  color: #98C73A;
  margin-bottom: 4px;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.qs-local-card__name {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 700;
  color: #225F6B;
  margin-bottom: 6px;
}

.qs-local-card__location {
  display: flex;
  align-items: center;
  gap: 4px;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  color: #2F7785;
  margin-bottom: 16px;
}

.qs-local-card__btn {
  display: block;
  text-align: center;
  border: 1.5px solid #e5e7eb;
  color: #374151;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 12px;
  font-weight: 600;
  padding: 9px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s;
}

.qs-local-card__btn:hover {
  border-color: #2F7785;
  color: #2F7785;
  background: rgba(47, 119, 133, 0.04);
}

.qs-locais__more {
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
