<template>
  <div class="qs-page">

        <!-- Page Header -->
        <QsPageHeader eyebrow="Indicação Rastreável" :title="cms.titulo" :description="cms.subtitulo">
          <NuxtLink to="/agencia/painel/dashboard-rede-adf" class="qs-btn-secondary lk-header-btn">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z"/></svg>
            Ver Dashboard ADF
          </NuxtLink>
        </QsPageHeader>

        <!-- Paywall: usuários sem HAF veem apenas o upsell -->
        <template v-if="!hasHaf">
          <PaywallHaf />
        </template>

        <template v-else>

          <!-- ─── HERO: Gerador de Link ──────────────────────────── -->
          <div class="lk-hero-card qs-card-section">

            <!-- Badge QR decorativo + título -->
            <div class="lk-hero-top">
              <div class="lk-hero-left">
                <div class="lk-hero-label">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M17 12h-5v5h5v-5zM16 1v2H8V1H6v2H5c-1.11 0-1.99.9-1.99 2L3 19c0 1.1.89 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2h-1V1h-2zm3 18H5V8h14v11z"/></svg>
                  Link exclusivo e rastreável
                </div>
                <p class="lk-hero-desc">{{ cms.descLink }}</p>
              </div>

              <!-- QR Code SVG decorativo -->
              <div class="lk-qr-wrapper" :class="{ 'lk-qr-wrapper--parceiro': parceiroSelecionado }">
                <svg class="lk-qr-svg" viewBox="0 0 64 64" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <rect x="2" y="2" width="24" height="24" rx="3" fill="var(--qs-teal,#2F7785)" opacity=".15"/>
                  <rect x="6" y="6" width="16" height="16" rx="2" fill="var(--qs-teal,#2F7785)" opacity=".35"/>
                  <rect x="9" y="9" width="10" height="10" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="38" y="2" width="24" height="24" rx="3" fill="var(--qs-teal,#2F7785)" opacity=".15"/>
                  <rect x="42" y="6" width="16" height="16" rx="2" fill="var(--qs-teal,#2F7785)" opacity=".35"/>
                  <rect x="45" y="9" width="10" height="10" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="2" y="38" width="24" height="24" rx="3" fill="var(--qs-teal,#2F7785)" opacity=".15"/>
                  <rect x="6" y="42" width="16" height="16" rx="2" fill="var(--qs-teal,#2F7785)" opacity=".35"/>
                  <rect x="9" y="45" width="10" height="10" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="30" y="30" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="36" y="30" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="42" y="30" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="48" y="30" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="54" y="30" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="30" y="36" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="36" y="36" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="42" y="36" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="48" y="36" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="54" y="36" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="30" y="42" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="36" y="42" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="42" y="42" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="48" y="42" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="30" y="48" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="36" y="48" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="42" y="48" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="48" y="48" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="54" y="48" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="30" y="54" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="36" y="54" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="42" y="54" width="4" height="4" rx="1" fill="var(--qs-teal,#2F7785)"/>
                  <rect x="48" y="54" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <rect x="54" y="54" width="4" height="4" rx="1" fill="var(--qs-lime,#98C73A)"/>
                  <!-- ícone central -->
                  <circle cx="16" cy="48" r="6" fill="var(--qs-lime,#98C73A)" opacity=".12"/>
                </svg>
                <div v-if="parceiroSelecionado" class="lk-qr-parceiro-badge">
                  <span>{{ parceiroSelecionado.nome.charAt(0) }}</span>
                </div>
              </div>
            </div>

            <!-- Campo do link -->
            <div class="lk-link-label">{{ cms.labelLink }}</div>
            <div class="lk-link-row">
              <div class="lk-link-field">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
                <input
                  ref="linkInputRef"
                  :value="linkIndicacao"
                  readonly
                  class="lk-link-input"
                  @focus="(e) => (e.target as HTMLInputElement).select()"
                />
              </div>
              <button class="lk-copy-btn" :class="{ 'lk-copy-btn--ok': copiado }" @click="copiarLink">
                <svg v-if="!copiado" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 1H4c-1.1 0-2 .9-2 2v14h2V3h12V1zm3 4H8c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h11c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2zm0 16H8V7h11v14z"/></svg>
                <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                <span>{{ copiado ? 'Copiado!' : 'Copiar' }}</span>
              </button>
            </div>

            <!-- Parceiro selecionado info -->
            <Transition name="lk-fade">
              <div v-if="parceiroSelecionado" class="lk-parceiro-chip">
                <span class="lk-parceiro-chip__avatar">{{ parceiroSelecionado.nome.charAt(0) }}</span>
                Link personalizado para <strong>{{ parceiroSelecionado.nome }}</strong>
                <span class="lk-parceiro-chip__cashback">{{ parceiroSelecionado.cashbackPct }}% cashback</span>
                <button class="lk-parceiro-chip__clear" @click="parceiroSelecionado = null" aria-label="Remover parceiro">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>
                </button>
              </div>
            </Transition>

            <!-- Botão WhatsApp -->
            <a :href="whatsappLink" target="_blank" rel="noopener" class="lk-whatsapp-btn">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
                <path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/>
              </svg>
              Compartilhar via WhatsApp
              <span class="lk-whatsapp-btn__arrow">→</span>
            </a>

            <!-- Dica CMS -->
            <p class="lk-dica">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
              {{ cms.dicaCopy }}
            </p>
          </div>

          <!-- ─── STATS ROW ──────────────────────────────────────── -->
          <div class="lk-stats-row">
            <div class="lk-stat-card">
              <div class="lk-stat-card__icon lk-stat-card__icon--teal">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 4.5C7 4.5 2.73 7.61 1 12c1.73 4.39 6 7.5 11 7.5s9.27-3.11 11-7.5c-1.73-4.39-6-7.5-11-7.5zM12 17c-2.76 0-5-2.24-5-5s2.24-5 5-5 5 2.24 5 5-2.24 5-5 5zm0-8c-1.66 0-3 1.34-3 3s1.34 3 3 3 3-1.34 3-3-1.34-3-3-3z"/></svg>
              </div>
              <div class="lk-stat-card__body">
                <div class="lk-stat-card__label">{{ cms.labelCliques }}</div>
                <div class="lk-stat-card__valor">{{ stats.cliquesHoje }}</div>
                <div class="lk-stat-card__sub lk-stat-card__sub--up">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M7 14l5-5 5 5z"/></svg>
                  +{{ stats.cliquesHojeDelta }}% hoje
                </div>
              </div>
            </div>

            <div class="lk-stat-card">
              <div class="lk-stat-card__icon lk-stat-card__icon--lime">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M15 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm-9-2V7H4v3H1v2h3v3h2v-3h3v-2H6zm9 4c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z"/></svg>
              </div>
              <div class="lk-stat-card__body">
                <div class="lk-stat-card__label">{{ cms.labelConversoes }}</div>
                <div class="lk-stat-card__valor">{{ stats.conversoes }}</div>
                <div class="lk-stat-card__sub lk-stat-card__sub--up">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M7 14l5-5 5 5z"/></svg>
                  {{ stats.taxaConversao }}% conversão
                </div>
              </div>
            </div>

            <div class="lk-stat-card">
              <div class="lk-stat-card__icon lk-stat-card__icon--gold">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
              </div>
              <div class="lk-stat-card__body">
                <div class="lk-stat-card__label">{{ cms.labelCashback }}</div>
                <div class="lk-stat-card__valor lk-stat-card__valor--lime">{{ formatCurrency(stats.cashbackGerado) }}</div>
                <div class="lk-stat-card__sub">Este mês</div>
              </div>
            </div>
          </div>

          <!-- ─── PARCEIROS ──────────────────────────────────────── -->
          <div class="qs-card-section lk-parceiros-section">
            <div class="lk-parceiros-header">
              <div>
                <div class="qs-section-title">Personalizar por Parceiro</div>
                <div class="qs-section-desc">Selecione uma loja para gerar um link rastreável com a oferta específica daquele parceiro.</div>
              </div>
              <Transition name="lk-fade">
                <span v-if="parceiroSelecionado" class="lk-parceiros-clear-all" @click="parceiroSelecionado = null">
                  Limpar seleção
                </span>
              </Transition>
            </div>

            <div class="lk-parceiros-grid">
              <button
                v-for="p in cms.parceiros"
                :key="p.id"
                class="lk-parceiro-card"
                :class="{ 'lk-parceiro-card--ativo': parceiroSelecionado?.id === p.id }"
                @click="toggleParceiro(p)"
              >
                <div v-if="p.destaque" class="lk-destaque-badge">⭐</div>

                <div class="lk-parceiro-logo">
                  <img
                    :src="p.logo"
                    :alt="p.nome"
                    @error="(e) => ((e.target as HTMLImageElement).style.display = 'none')"
                  />
                  <span class="lk-parceiro-inicial">{{ p.nome.charAt(0) }}</span>
                </div>

                <div class="lk-parceiro-nome">{{ p.nome }}</div>
                <div class="lk-parceiro-cat">{{ p.categoria }}</div>
                <div class="lk-parceiro-cashback">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
                  {{ p.cashbackPct }}% cb
                </div>

                <div class="lk-parceiro-check" :class="{ 'lk-parceiro-check--ativo': parceiroSelecionado?.id === p.id }">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                </div>
              </button>
            </div>
          </div>

          <!-- ─── PLUS MÓDULO ─────────────────────────────────────── -->
          <PlusModulo class="lk-plus-mt" />

        </template>

  </div>
</template>

<script setup lang="ts">
import PaywallHaf from '~/components/agencia/PaywallHaf.vue';
import PlusModulo from '~/components/agencia/PlusModulo.vue';
import { useCmsStore } from '~/pinia/useCmsStore';
import { useAgenciaStore } from '~/pinia/useAgenciaStore';
import type { CmsParceiro } from '~/pinia/useCmsStore';

definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const cmsStore = useCmsStore();
const agenciaStore = useAgenciaStore();

const cms = computed(() => cmsStore.config.vitrinLinks);
const parceiros = computed(() => cmsStore.config.socialCommerce.parceiros);

const hasHaf = computed(() => {
  const u = agenciaStore.user as any;
  return !!(u?.hafAtiva || u?.licencaAtiva || u?.admin);
});

const copiado = ref(false);
const parceiroSelecionado = ref<CmsParceiro | null>(null);
const linkInputRef = ref<HTMLInputElement | null>(null);

const stats = ref({
  cliquesHoje: 47,
  cliquesHojeDelta: 12,
  conversoes: 12,
  taxaConversao: 25.5,
  cashbackGerado: 38.90,
});

const baseUrl = computed(() => {
  if (!import.meta.client) return 'https://quantashop.com.br';
  return window.location.origin;
});

const linkIndicacao = computed(() => {
  const login = (agenciaStore.user as any)?.login || (agenciaStore.user as any)?.username || 'meu-usuario';
  const suffix = parceiroSelecionado.value ? `?p=${parceiroSelecionado.value.id}` : '';
  return `${baseUrl.value}/r/${login}${suffix}`;
});

const whatsappLink = computed(() => {
  const texto = cms.value.whatsappCopy.replace('{link}', linkIndicacao.value);
  return `https://wa.me/?text=${encodeURIComponent(texto)}`;
});

function toggleParceiro(p: CmsParceiro) {
  parceiroSelecionado.value = parceiroSelecionado.value?.id === p.id ? null : p;
}

async function copiarLink() {
  try {
    await navigator.clipboard.writeText(linkIndicacao.value);
  } catch {
    const input = linkInputRef.value;
    if (input) { input.select(); document.execCommand('copy'); }
  }
  copiado.value = true;
  setTimeout(() => { copiado.value = false; }, 2400);
}

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v);
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await cmsStore.loadConfig();
  cmsStore.applyBrandCssVars();
});
</script>

<style scoped>
/* ── Header button ── */
.lk-header-btn {
  display: inline-flex;
  align-items: center;
  gap: .4rem;
  font-size: .8125rem;
  white-space: nowrap;
  flex-shrink: 0;
}
.lk-header-btn svg { width: 16px; height: 16px; }

/* ── Hero Card ── */
.lk-hero-card {
  background: #fff;
  position: relative;
  overflow: hidden;
}
.lk-hero-card::before {
  content: '';
  position: absolute;
  top: -40px;
  right: -40px;
  width: 200px;
  height: 200px;
  background: radial-gradient(circle, rgba(47,119,133,.06) 0%, transparent 70%);
  pointer-events: none;
}

.lk-hero-top {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.lk-hero-left { flex: 1; min-width: 0; }

.lk-hero-label {
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .08em;
  color: var(--qs-teal, #2F7785);
  background: rgba(47,119,133,.08);
  padding: .3rem .7rem;
  border-radius: var(--qs-radius-pill, 999px);
  margin-bottom: .625rem;
}
.lk-hero-label svg { width: 13px; height: 13px; }

.lk-hero-desc {
  font-size: .8125rem;
  color: var(--qs-gray-500, #6b7280);
  line-height: 1.5;
  margin: 0;
}

/* QR code decorativo */
.lk-qr-wrapper {
  position: relative;
  flex-shrink: 0;
  width: 72px;
  height: 72px;
  border-radius: var(--qs-radius-md, 12px);
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  padding: 4px;
  background: #fff;
  transition: border-color .2s;
}
.lk-qr-wrapper--parceiro {
  border-color: var(--qs-lime, #98C73A);
  box-shadow: 0 0 0 3px rgba(152,199,58,.15);
}
.lk-qr-svg { width: 100%; height: 100%; }

.lk-qr-parceiro-badge {
  position: absolute;
  bottom: -8px;
  right: -8px;
  width: 22px;
  height: 22px;
  background: var(--qs-lime, #98C73A);
  border-radius: 50%;
  border: 2px solid #fff;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: .5625rem;
  font-weight: 700;
  color: #fff;
}

/* Link field */
.lk-link-label {
  font-size: .75rem;
  font-weight: 600;
  color: var(--qs-gray-700, #374151);
  margin-bottom: .5rem;
  text-transform: uppercase;
  letter-spacing: .05em;
}

.lk-link-row {
  display: flex;
  gap: .5rem;
  margin-bottom: .875rem;
}

.lk-link-field {
  flex: 1;
  min-width: 0;
  display: flex;
  align-items: center;
  gap: .5rem;
  padding: .625rem .875rem;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  background: var(--qs-gray-50, #fafafa);
  transition: border-color .2s;
}
.lk-link-field:focus-within {
  border-color: var(--qs-teal, #2F7785);
  background: #fff;
}
.lk-link-field svg {
  width: 16px;
  height: 16px;
  color: var(--qs-gray-400, #9ca3af);
  flex-shrink: 0;
}
.lk-link-input {
  flex: 1;
  min-width: 0;
  border: none;
  outline: none;
  background: transparent;
  font-size: .8125rem;
  font-family: 'Inter', monospace;
  color: var(--qs-gray-700, #374151);
  cursor: text;
}

.lk-copy-btn {
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  padding: .625rem 1.125rem;
  border: none;
  border-radius: var(--qs-radius-md, 12px);
  background: var(--qs-gradient-btn, linear-gradient(135deg, #225F6B 0%, #2F7785 100%));
  color: #fff;
  font-size: .8125rem;
  font-weight: 600;
  cursor: pointer;
  white-space: nowrap;
  flex-shrink: 0;
  transition: all .22s;
}
.lk-copy-btn svg { width: 15px; height: 15px; }
.lk-copy-btn:hover { opacity: .88; transform: translateY(-1px); }
.lk-copy-btn--ok {
  background: linear-gradient(135deg, #16a34a, #22c55e) !important;
}

/* Parceiro chip */
.lk-parceiro-chip {
  display: flex;
  align-items: center;
  gap: .5rem;
  font-size: .8125rem;
  color: var(--qs-gray-700, #374151);
  background: rgba(152,199,58,.08);
  border: 1px solid rgba(152,199,58,.25);
  border-radius: var(--qs-radius-pill, 999px);
  padding: .35rem .75rem .35rem .5rem;
  margin-bottom: .875rem;
  flex-wrap: wrap;
}
.lk-parceiro-chip__avatar {
  width: 22px;
  height: 22px;
  background: var(--qs-lime, #98C73A);
  color: #fff;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: .625rem;
  font-weight: 700;
  flex-shrink: 0;
}
.lk-parceiro-chip__cashback {
  margin-left: auto;
  font-size: .6875rem;
  font-weight: 700;
  color: var(--qs-lime-dark, #7aad1f);
}
.lk-parceiro-chip__clear {
  background: none;
  border: none;
  padding: 0;
  cursor: pointer;
  color: var(--qs-gray-400, #9ca3af);
  display: flex;
  align-items: center;
  margin-left: .25rem;
}
.lk-parceiro-chip__clear svg { width: 14px; height: 14px; }
.lk-parceiro-chip__clear:hover { color: var(--qs-danger, #dc2626); }

/* WhatsApp button */
.lk-whatsapp-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: .625rem;
  background: #25D366;
  color: #fff;
  font-weight: 700;
  font-size: 1rem;
  padding: .875rem 1.5rem;
  border-radius: var(--qs-radius-md, 12px);
  text-decoration: none;
  transition: all .25s;
  box-shadow: 0 4px 18px rgba(37,211,102,.35);
  position: relative;
  width: 100%;
  margin-bottom: .875rem;
}
.lk-whatsapp-btn svg { width: 22px; height: 22px; }
.lk-whatsapp-btn:hover {
  background: #1ebe5d;
  color: #fff;
  transform: translateY(-2px);
  box-shadow: 0 6px 24px rgba(37,211,102,.45);
}
.lk-whatsapp-btn__arrow {
  margin-left: auto;
  font-size: 1.125rem;
  opacity: .7;
}

/* Dica */
.lk-dica {
  display: flex;
  align-items: flex-start;
  gap: .4rem;
  font-size: .75rem;
  color: var(--qs-gray-500, #6b7280);
  line-height: 1.5;
  margin: 0;
  padding-top: .125rem;
}
.lk-dica svg { width: 14px; height: 14px; flex-shrink: 0; color: var(--qs-teal, #2F7785); margin-top: 1px; }

/* ── Stats Row ── */
.lk-stats-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
  gap: .75rem;
  margin-bottom: 1.5rem;
}

.lk-stat-card {
  background: #fff;
  border-radius: var(--qs-radius-md, 12px);
  box-shadow: var(--qs-shadow-sm, 0 2px 8px rgba(1,15,28,.08));
  padding: 1.125rem;
  display: flex;
  align-items: flex-start;
  gap: .875rem;
  transition: all .2s;
}
.lk-stat-card:hover {
  box-shadow: var(--qs-shadow-md, 0 4px 20px rgba(1,15,28,.12));
  transform: translateY(-2px);
}

.lk-stat-card__icon {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  color: #fff;
}
.lk-stat-card__icon svg { width: 20px; height: 20px; }
.lk-stat-card__icon--teal { background: var(--qs-teal, #2F7785); }
.lk-stat-card__icon--lime { background: var(--qs-lime, #98C73A); }
.lk-stat-card__icon--gold { background: #FFB342; }

.lk-stat-card__body { flex: 1; min-width: 0; }

.lk-stat-card__label {
  font-size: .625rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .07em;
  color: var(--qs-gray-400, #9ca3af);
  margin-bottom: .2rem;
}
.lk-stat-card__valor {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--qs-ink, #1d1d1f);
  letter-spacing: -0.02em;
  line-height: 1.1;
  margin-bottom: .25rem;
}
.lk-stat-card__valor--lime { color: var(--qs-lime-dark, #7aad1f); }

.lk-stat-card__sub {
  display: flex;
  align-items: center;
  gap: .15rem;
  font-size: .6875rem;
  font-weight: 500;
  color: var(--qs-gray-400, #9ca3af);
}
.lk-stat-card__sub svg { width: 13px; height: 13px; }
.lk-stat-card__sub--up { color: var(--qs-success, #16a34a); }

/* ── Parceiros ── */
.lk-parceiros-section { background: #fff; }

.lk-parceiros-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: .5rem;
  margin-bottom: 1rem;
}

.lk-parceiros-clear-all {
  font-size: .75rem;
  font-weight: 600;
  color: var(--qs-danger, #dc2626);
  cursor: pointer;
  text-decoration: underline;
  padding-top: .25rem;
}

.lk-parceiros-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
  gap: .75rem;
}
@media (max-width: 375px) {
  .lk-parceiros-grid { grid-template-columns: repeat(2, 1fr); }
}

.lk-parceiro-card {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: .35rem;
  padding: 1rem .75rem .875rem;
  background: var(--qs-gray-50, #fafafa);
  border: 2px solid var(--qs-gray-100, #f5f5f7);
  border-radius: var(--qs-radius-md, 12px);
  cursor: pointer;
  transition: all .2s;
  text-align: center;
}
.lk-parceiro-card:hover {
  border-color: var(--qs-teal, #2F7785);
  background: #fff;
  box-shadow: var(--qs-shadow-sm, 0 2px 8px rgba(1,15,28,.08));
  transform: translateY(-2px);
}
.lk-parceiro-card--ativo {
  border-color: var(--qs-lime, #98C73A) !important;
  background: #f5fbf0 !important;
  box-shadow: 0 0 0 4px rgba(152,199,58,.15) !important;
}

.lk-destaque-badge {
  position: absolute;
  top: 4px;
  right: 6px;
  font-size: .65rem;
}

.lk-parceiro-logo {
  position: relative;
  width: 48px;
  height: 48px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.lk-parceiro-logo img { max-width: 100%; max-height: 100%; object-fit: contain; }
.lk-parceiro-inicial {
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.25rem;
  font-weight: 700;
  color: var(--qs-teal, #2F7785);
  background: var(--qs-gray-100, #f5f5f7);
  border-radius: 50%;
}
.lk-parceiro-logo img:not([style*='display: none']) + .lk-parceiro-inicial { display: none; }

.lk-parceiro-nome {
  font-size: .75rem;
  font-weight: 600;
  color: var(--qs-ink, #1d1d1f);
}
.lk-parceiro-cat {
  font-size: .625rem;
  color: var(--qs-gray-400, #9ca3af);
  text-transform: uppercase;
  letter-spacing: .04em;
  font-weight: 600;
}
.lk-parceiro-cashback {
  display: flex;
  align-items: center;
  gap: .15rem;
  font-size: .625rem;
  font-weight: 700;
  color: var(--qs-lime-dark, #7aad1f);
}
.lk-parceiro-cashback svg { width: 10px; height: 10px; }

.lk-parceiro-check {
  width: 18px;
  height: 18px;
  border: 2px solid var(--qs-gray-200, #e5e7eb);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all .15s;
  margin-top: .15rem;
}
.lk-parceiro-check svg { width: 10px; height: 10px; color: transparent; }
.lk-parceiro-check--ativo {
  background: var(--qs-lime, #98C73A);
  border-color: var(--qs-lime, #98C73A);
}
.lk-parceiro-check--ativo svg { color: #fff; }

/* ── Plus spacing ── */
.lk-plus-mt { margin-top: 1rem; }

/* ── Transitions ── */
.lk-fade-enter-active,
.lk-fade-leave-active { transition: all .22s ease; }
.lk-fade-enter-from,
.lk-fade-leave-to { opacity: 0; transform: translateY(-4px); }
</style>
