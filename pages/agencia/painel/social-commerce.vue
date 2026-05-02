<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <!-- Page Header — textos 100% do CMS -->
        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">Social Commerce</div>
            <h1>{{ cms.titulo }}</h1>
            <p>{{ cms.subtitulo }}</p>
          </div>
        </div>

        <!-- Paywall: usuários sem HAF veem apenas o upsell -->
        <template v-if="!hasHaf">
          <PaywallHaf />
        </template>

        <template v-else>
          <!-- Incentivo copy do CMS -->
          <div class="sc-incentivo">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11 17h2v-6h-2v6zm1-15C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8zM11 9h2V7h-2v2z"/></svg>
            {{ cms.incentivoCopy }}
          </div>

          <!-- Meu Link de Indicação -->
          <div class="qs-card-section sc-link-card">
            <div class="qs-section-title">{{ cms.labelLink }}</div>
            <div class="sc-link-box">
              <input
                ref="linkInput"
                :value="linkIndicacao"
                readonly
                class="sc-link-input"
                @focus="(e) => (e.target as HTMLInputElement).select()"
              />
              <button class="qs-btn-primary sc-link-copy" @click="copiarLink">
                <svg v-if="!copiado" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 1H4c-1.1 0-2 .9-2 2v14h2V3h12V1zm3 4H8c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h11c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2zm0 16H8V7h11v14z"/></svg>
                <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                {{ copiado ? 'Copiado!' : 'Copiar' }}
              </button>
            </div>
            <!-- Botão WhatsApp Share -->
            <a :href="whatsappLink" target="_blank" rel="noopener" class="sc-whatsapp-btn">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
                <path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/>
              </svg>
              Compartilhar via WhatsApp
            </a>
          </div>

          <!-- Grade de Parceiros — logos e textos via CMS -->
          <div class="qs-card-section">
            <div class="qs-section-title">Escolha um parceiro para compartilhar</div>
            <div class="qs-section-desc">Selecione o parceiro, gere seu link personalizado e compartilhe. Imagens e cashback vindos do CMS.</div>

            <div v-if="carregando" class="qs-loading"><div class="qs-spinner" /></div>

            <div v-else class="sc-parceiros-grid">
              <button
                v-for="p in cms.parceiros"
                :key="p.id"
                class="sc-parceiro-card"
                :class="{ 'sc-parceiro-card--ativo': parceiroSelecionado?.id === p.id }"
                @click="selecionarParceiro(p)"
              >
                <div v-if="p.destaque" class="sc-parceiro-destaque">Destaque</div>
                <div class="sc-parceiro-logo">
                  <img
                    :src="p.logo"
                    :alt="p.nome"
                    @error="(e) => ((e.target as HTMLImageElement).style.display = 'none')"
                  />
                  <span class="sc-parceiro-inicial">{{ p.nome.charAt(0) }}</span>
                </div>
                <div class="sc-parceiro-nome">{{ p.nome }}</div>
                <div class="sc-parceiro-cat">{{ p.categoria }}</div>
                <div class="sc-parceiro-cashback">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
                  {{ p.cashbackPct }}% cashback
                </div>
              </button>
            </div>
          </div>

          <!-- Plus Módulo -->
          <PlusModulo class="mt-3" />
        </template>

      </div>
    </div>
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

const cms = computed(() => cmsStore.config.socialCommerce);
const hasHaf = computed(() => {
  const u = agenciaStore.user as any;
  return !!(u?.hafAtiva || u?.licencaAtiva || u?.admin);
});

const carregando = ref(true);
const copiado = ref(false);
const parceiroSelecionado = ref<CmsParceiro | null>(null);

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
  const p = parceiroSelecionado.value;
  const texto = cms.value.whatsappCopy
    .replace('{parceiro}', p?.nome ?? 'Quanta Shop')
    .replace('{link}', linkIndicacao.value);
  return `https://wa.me/?text=${encodeURIComponent(texto)}`;
});

function selecionarParceiro(p: CmsParceiro) {
  parceiroSelecionado.value = parceiroSelecionado.value?.id === p.id ? null : p;
}

async function copiarLink() {
  try {
    await navigator.clipboard.writeText(linkIndicacao.value);
    copiado.value = true;
    setTimeout(() => { copiado.value = false; }, 2200);
  } catch {
    const input = document.querySelector('.sc-link-input') as HTMLInputElement;
    if (input) { input.select(); document.execCommand('copy'); }
    copiado.value = true;
    setTimeout(() => { copiado.value = false; }, 2200);
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await cmsStore.loadConfig();
  cmsStore.applyBrandCssVars();
  carregando.value = false;
});
</script>

<style scoped>
/* Incentivo */
.sc-incentivo {
  display: flex;
  align-items: flex-start;
  gap: .625rem;
  background: linear-gradient(135deg, #f0f7f9, #f5fbf0);
  border-left: 4px solid var(--qs-lime, #98C73A);
  border-radius: 0 var(--qs-radius-md, 12px) var(--qs-radius-md, 12px) 0;
  padding: .875rem 1rem;
  margin-bottom: 1.5rem;
  font-size: .875rem;
  color: var(--qs-gray-700, #374151);
  line-height: 1.5;
}
.sc-incentivo svg { width: 18px; height: 18px; color: var(--qs-lime, #98C73A); flex-shrink: 0; margin-top: 1px; }

/* Link Card */
.sc-link-card { background: #fff; }

.sc-link-box {
  display: flex;
  gap: .5rem;
  margin-bottom: 1rem;
}

.sc-link-input {
  flex: 1;
  padding: .625rem .875rem;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  font-size: .8125rem;
  font-family: 'Inter', monospace;
  color: var(--qs-gray-700, #374151);
  background: var(--qs-gray-50, #fafafa);
  cursor: text;
  min-width: 0;
}
.sc-link-input:focus { outline: 2px solid var(--qs-teal, #2F7785); }

.sc-link-copy {
  flex-shrink: 0;
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  padding: .625rem 1rem;
  font-size: .8125rem;
  white-space: nowrap;
}
.sc-link-copy svg { width: 15px; height: 15px; }

/* WhatsApp Button */
.sc-whatsapp-btn {
  display: inline-flex;
  align-items: center;
  gap: .5rem;
  background: #25D366;
  color: #fff;
  font-weight: 600;
  font-size: .9375rem;
  padding: .75rem 1.5rem;
  border-radius: var(--qs-radius-md, 12px);
  text-decoration: none;
  transition: all .25s;
  width: 100%;
  justify-content: center;
  box-shadow: 0 4px 14px rgba(37, 211, 102, .35);
}
.sc-whatsapp-btn svg { width: 20px; height: 20px; }
.sc-whatsapp-btn:hover { background: #1ebe5d; color: #fff; transform: translateY(-1px); }

/* Parceiros Grid */
.sc-parceiros-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(130px, 1fr));
  gap: .75rem;
  margin-top: .5rem;
}

@media (max-width: 375px) {
  .sc-parceiros-grid { grid-template-columns: repeat(2, 1fr); }
}

.sc-parceiro-card {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: .4rem;
  padding: 1rem .75rem .875rem;
  background: var(--qs-gray-50, #fafafa);
  border: 2px solid var(--qs-gray-100, #f5f5f7);
  border-radius: var(--qs-radius-md, 12px);
  cursor: pointer;
  transition: all .2s;
  text-align: center;
}
.sc-parceiro-card:hover {
  border-color: var(--qs-teal, #2F7785);
  background: #fff;
  box-shadow: var(--qs-shadow-sm, 0 2px 8px rgba(1,15,28,.08));
  transform: translateY(-2px);
}
.sc-parceiro-card--ativo {
  border-color: var(--qs-lime, #98C73A) !important;
  background: #f5fbf0 !important;
  box-shadow: 0 0 0 4px rgba(152, 199, 58, .15) !important;
}

.sc-parceiro-destaque {
  position: absolute;
  top: -1px;
  right: -1px;
  font-size: .5625rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .05em;
  background: var(--qs-lime, #98C73A);
  color: #fff;
  padding: .15rem .45rem;
  border-radius: 0 var(--qs-radius-md, 12px) 0 var(--qs-radius-sm, 6px);
}

.sc-parceiro-logo {
  position: relative;
  width: 52px;
  height: 52px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.sc-parceiro-logo img {
  max-width: 100%;
  max-height: 100%;
  object-fit: contain;
}
.sc-parceiro-inicial {
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.375rem;
  font-weight: 700;
  color: var(--qs-teal, #2F7785);
  background: var(--qs-gray-100, #f5f5f7);
  border-radius: 50%;
}
.sc-parceiro-logo img:not([style*='display: none']) + .sc-parceiro-inicial {
  display: none;
}

.sc-parceiro-nome {
  font-size: .8125rem;
  font-weight: 600;
  color: var(--qs-ink, #1d1d1f);
}
.sc-parceiro-cat {
  font-size: .6875rem;
  color: var(--qs-gray-400, #9ca3af);
  text-transform: uppercase;
  letter-spacing: .05em;
  font-weight: 600;
}
.sc-parceiro-cashback {
  display: flex;
  align-items: center;
  gap: .2rem;
  font-size: .6875rem;
  font-weight: 700;
  color: var(--qs-lime-dark, #7aad1f);
}
.sc-parceiro-cashback svg { width: 11px; height: 11px; }

.mt-3 { margin-top: 1rem; }
</style>
