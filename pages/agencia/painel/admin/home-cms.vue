<template>
  <div class="hcms">
    <div class="hcms__header">
      <h1 class="hcms__title">CMS — Home Page</h1>
      <p class="hcms__sub">Edite os textos e links exibidos na página inicial. As alterações ficam visíveis imediatamente após salvar.</p>
    </div>

    <div v-if="loadError" class="hcms__alert hcms__alert--error">
      Erro ao carregar configuração. Verifique se o arquivo <code>home-config.json</code> existe.
    </div>

    <div v-if="saveSuccess" class="hcms__alert hcms__alert--success">
      Configuração salva com sucesso!
    </div>

    <div v-if="saveError" class="hcms__alert hcms__alert--error">
      Erro ao salvar. Tente novamente.
    </div>

    <ul class="nav nav-tabs hcms__tabs" v-if="form">
      <li class="nav-item" v-for="tab in tabs" :key="tab.key">
        <button
          class="nav-link"
          :class="{ active: activeTab === tab.key }"
          @click="activeTab = tab.key"
        >{{ tab.label }}</button>
      </li>
    </ul>

    <div v-if="form" class="hcms__form-wrap">

      <!-- Hero -->
      <div v-show="activeTab === 'hero'" class="hcms__section">
        <h2 class="hcms__section-title">Seção Hero (Banner Principal)</h2>
        <div class="hcms__field">
          <label class="hcms__label">Texto do Badge</label>
          <input v-model="form.hero.badge" class="hcms__input" placeholder="+12.000 usuários economizando" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">
            Título (use <code>&lt;highlight&gt;palavra&lt;/highlight&gt;</code> para cor verde-limão)
          </label>
          <input v-model="form.hero.title" class="hcms__input" placeholder="Seu dinheiro &lt;highlight&gt;volta&lt;/highlight&gt; a cada compra" />
          <span class="hcms__hint">Pré-visualização: <span v-html="previewHeroTitle"></span></span>
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Subtítulo</label>
          <textarea v-model="form.hero.subtitle" class="hcms__textarea" rows="2"></textarea>
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Texto do Botão Principal</label>
          <input v-model="form.hero.ctaPrimaryText" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Link do Botão Principal</label>
          <input v-model="form.hero.ctaPrimaryLink" class="hcms__input" placeholder="/register" />
        </div>
      </div>

      <!-- Hero Cards -->
      <div v-show="activeTab === 'heroCards'" class="hcms__section">
        <h2 class="hcms__section-title">Cartões Flutuantes do Hero</h2>
        <p class="hcms__sub" style="margin-bottom:20px;">Os 3 cartões que aparecem à direita do banner hero. Você pode ativar/desativar, alterar textos, ícone e cor.</p>
        <div v-for="(card, idx) in form.heroCards" :key="idx" class="hcms__card-editor">
          <div class="hcms__card-editor-header">
            <strong>Cartão {{ idx + 1 }}</strong>
            <label class="hcms__toggle">
              <input type="checkbox" v-model="card.ativo" />
              <span class="hcms__toggle-track">
                <span class="hcms__toggle-thumb"></span>
              </span>
              {{ card.ativo ? 'Ativo' : 'Inativo' }}
            </label>
          </div>
          <div class="hcms__card-editor-body">
            <div class="hcms__field">
              <label class="hcms__label">Rótulo (label pequeno)</label>
              <input v-model="card.label" class="hcms__input" placeholder="Ex: PIX INSTANTÂNEO" />
            </div>
            <div class="hcms__field">
              <label class="hcms__label">Valor (texto principal)</label>
              <input v-model="card.value" class="hcms__input" placeholder="Ex: Saque em segundos ✓" />
            </div>
            <div class="hcms__field">
              <label class="hcms__label">Cor do Valor</label>
              <div class="hcms__btn-group">
                <button type="button" class="hcms__btn-opt" :class="{ active: card.valueColor === 'teal' }" @click="card.valueColor = 'teal'">Escuro</button>
                <button type="button" class="hcms__btn-opt" :class="{ active: card.valueColor === 'green' }" @click="card.valueColor = 'green'">Teal</button>
                <button type="button" class="hcms__btn-opt" :class="{ active: card.valueColor === 'white' }" @click="card.valueColor = 'white'">Branco</button>
              </div>
            </div>
            <div class="hcms__field">
              <label class="hcms__label">Ícone</label>
              <div class="hcms__btn-group hcms__btn-group--wrap">
                <button v-for="ic in ['card','chart','bag','star','percent','gift','users','zap']" :key="ic" type="button" class="hcms__btn-opt" :class="{ active: card.icon === ic }" @click="card.icon = ic">{{ ic }}</button>
              </div>
            </div>
            <div class="hcms__field">
              <label class="hcms__label">Fundo do Ícone</label>
              <div class="hcms__btn-group">
                <button type="button" class="hcms__btn-opt" :class="{ active: card.iconBg === 'teal' }" @click="card.iconBg = 'teal'">Teal claro</button>
                <button type="button" class="hcms__btn-opt" :class="{ active: card.iconBg === 'green' }" @click="card.iconBg = 'green'">Verde-limão</button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Brands -->
      <div v-show="activeTab === 'brands'" class="hcms__section">
        <h2 class="hcms__section-title">Seção Marcas (Carrossel de Logos)</h2>
        <div class="hcms__field">
          <label class="hcms__label">Texto do Rótulo</label>
          <input v-model="form.brands.label" class="hcms__input" />
          <span class="hcms__hint">Exibido acima do carrossel de logos</span>
        </div>
        <div class="hcms__callout">
          Para gerenciar os logos das marcas (adicionar, remover, reordenar), acesse
          <nuxt-link href="/agencia/painel/admin/marcas-home" class="hcms__link">Gerenciar Marcas</nuxt-link>.
        </div>
      </div>

      <!-- Ofertas -->
      <div v-show="activeTab === 'ofertas'" class="hcms__section">
        <h2 class="hcms__section-title">Seção Ofertas do Dia</h2>
        <div class="hcms__field">
          <label class="hcms__label">Título</label>
          <input v-model="form.ofertas.title" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Subtítulo</label>
          <input v-model="form.ofertas.subtitle" class="hcms__input" />
        </div>
      </div>

      <!-- Parceiros Online -->
      <div v-show="activeTab === 'parceirosOnline'" class="hcms__section">
        <h2 class="hcms__section-title">Seção Parceiros Online</h2>
        <div class="hcms__field">
          <label class="hcms__label">Rótulo (label pequeno acima do título)</label>
          <input v-model="form.parceirosOnline.label" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Título</label>
          <input v-model="form.parceirosOnline.title" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Subtítulo</label>
          <input v-model="form.parceirosOnline.subtitle" class="hcms__input" />
        </div>
      </div>

      <!-- Parceiros Locais -->
      <div v-show="activeTab === 'parceirosLocais'" class="hcms__section">
        <h2 class="hcms__section-title">Seção Parceiros Locais</h2>
        <div class="hcms__field">
          <label class="hcms__label">Rótulo</label>
          <input v-model="form.parceirosLocais.label" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Título</label>
          <input v-model="form.parceirosLocais.title" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Subtítulo</label>
          <input v-model="form.parceirosLocais.subtitle" class="hcms__input" />
        </div>
      </div>

      <!-- Testimonials -->
      <div v-show="activeTab === 'testimonials'" class="hcms__section">
        <h2 class="hcms__section-title">Seção Depoimentos</h2>
        <div class="hcms__field">
          <label class="hcms__label">Título</label>
          <input v-model="form.testimonials.title" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Subtítulo</label>
          <input v-model="form.testimonials.subtitle" class="hcms__input" />
        </div>
      </div>

      <!-- Blog -->
      <div v-show="activeTab === 'blog'" class="hcms__section">
        <h2 class="hcms__section-title">Seção Blog & Redes Sociais</h2>
        <div class="hcms__field">
          <label class="hcms__label">Rótulo</label>
          <input v-model="form.blog.label" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Título</label>
          <input v-model="form.blog.title" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Subtítulo</label>
          <input v-model="form.blog.subtitle" class="hcms__input" />
        </div>
      </div>

      <!-- CEO -->
      <div v-show="activeTab === 'ceo'" class="hcms__section">
        <h2 class="hcms__section-title">Seção CEO</h2>

        <div class="hcms__field">
          <label class="hcms__label d-flex align-items-center gap-2">
            <input type="checkbox" v-model="form.ceo.ativo" class="form-check-input m-0" style="width:18px;height:18px;" />
            Exibir seção na home
          </label>
          <small class="text-muted">Desmarque para ocultar esta seção do site.</small>
        </div>

        <div class="hcms__field">
          <label class="hcms__label">Tag (ex: CEO & Founder)</label>
          <input v-model="form.ceo.tag" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Pré-título (ex: Fale com o CEO)</label>
          <input v-model="form.ceo.pre" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Nome</label>
          <input v-model="form.ceo.name" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Descrição</label>
          <textarea v-model="form.ceo.desc" class="hcms__textarea" rows="2"></textarea>
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Texto do Botão</label>
          <input v-model="form.ceo.ctaText" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Imagem de Fundo do Banner</label>
          <div class="hcms__ceo-drop-area" @click="!ceoUploading ? triggerCeoFileInput() : undefined">
            <input ref="ceoFileInput" type="file" accept="image/*" style="display:none" @change="onCeoFileChange" :disabled="ceoUploading" />
            <div v-if="ceoUploading" style="text-align:center;padding:20px 0;">
              <span class="hcms__spinner-sm" style="width:22px;height:22px;border-width:3px;"></span>
              <p style="margin:8px 0 0;font-size:13px;color:#6b7280;">Enviando imagem...</p>
            </div>
            <div v-else-if="form.ceo.imagemFundo || ceoLocalPreview" style="position:relative;">
              <img :src="form.ceo.imagemFundo || ceoLocalPreview" style="width:100%;max-height:160px;object-fit:cover;border-radius:8px;display:block;" alt="Fundo CEO" />
              <div style="position:absolute;inset:0;background:rgba(34,95,107,0.45);border-radius:8px;display:flex;align-items:center;justify-content:center;opacity:0;transition:opacity .2s;" class="hcms__ceo-drop-hover">
                <span style="color:#fff;font-size:13px;font-weight:700;">Clique para trocar</span>
              </div>
              <button type="button" @click.stop="removeCeoImage" style="position:absolute;top:8px;right:8px;background:rgba(0,0,0,0.6);color:#fff;border:0;border-radius:6px;font-size:11px;font-weight:700;padding:4px 10px;cursor:pointer;">&#10005; Remover</button>
            </div>
            <div v-else style="text-align:center;padding:24px 0;">
              <svg width="32" height="32" fill="none" viewBox="0 0 24 24" stroke="#9ca3af" stroke-width="1.5" style="margin:0 auto 8px;display:block;"><path d="M4 16v2a2 2 0 002 2h12a2 2 0 002-2v-2M16 10l-4-4-4 4M12 6v10"/></svg>
              <p style="margin:0;font-size:13px;color:#6b7280;">Clique para escolher uma imagem</p>
              <p style="margin:4px 0 0;font-size:11px;color:#9ca3af;">Recomendado: 1200 × 400px • JPG ou PNG</p>
            </div>
          </div>
          <small v-if="ceoUploadError" class="text-danger d-block mt-1">{{ ceoUploadError }}</small>
          <small class="text-muted d-block mt-1">Sem imagem → usa o gradiente teal padrão.</small>
        </div>

        <div v-if="form.ceo.imagemFundo" class="hcms__field">
          <label class="hcms__label">
            Intensidade do Overlay <span style="color:#2F7785;font-weight:700;">{{ Math.round((form.ceo.overlayOpacity ?? 0.72) * 100) }}%</span>
          </label>
          <div style="display:flex;align-items:center;gap:12px;">
            <span style="font-size:12px;color:#9ca3af;">0% (foto aparece)</span>
            <input
              type="range" min="0" max="1" step="0.04"
              :value="form.ceo.overlayOpacity ?? 0.72"
              @input="(e) => { if(form) form.ceo.overlayOpacity = parseFloat((e.target as HTMLInputElement).value) }"
              style="flex:1;"
            />
            <span style="font-size:12px;color:#9ca3af;">100% (cor sólida)</span>
          </div>
          <div style="margin-top:8px;border-radius:8px;height:32px;background:rgba(34,95,107,var(--ceo-ov));border:1.5px solid #e5e7eb;overflow:hidden;position:relative;">
            <div style="position:absolute;inset:0;background:linear-gradient(135deg,#1a4a54,#225F6B,#2F7785);"></div>
            <div :style="{ position:'absolute', inset:'0', background:`rgba(34,95,107,${form.ceo.overlayOpacity ?? 0.72})` }"></div>
            <div style="position:absolute;inset:0;display:flex;align-items:center;justify-content:center;color:#fff;font-size:11px;font-weight:600;letter-spacing:.04em;">Preview overlay</div>
          </div>
          <small class="text-muted d-block mt-1">Cor do overlay: <code>#225F6B</code></small>
        </div>

        <hr class="my-3" />
        <p class="hcms__label fw-bold mb-2">Cartões de Destaque (direita)</p>

        <div class="row g-2 mb-2">
          <div class="col-6">
            <label class="hcms__label">Cartão 1 — Rótulo</label>
            <input v-model="form.ceo.badge1Label" class="hcms__input" placeholder="ex: Respostas" />
          </div>
          <div class="col-6">
            <label class="hcms__label">Cartão 1 — Valor</label>
            <input v-model="form.ceo.badge1Value" class="hcms__input" placeholder="ex: Em até 24h" />
          </div>
        </div>
        <div class="row g-2">
          <div class="col-6">
            <label class="hcms__label">Cartão 2 — Rótulo</label>
            <input v-model="form.ceo.badge2Label" class="hcms__input" placeholder="ex: Parcerias" />
          </div>
          <div class="col-6">
            <label class="hcms__label">Cartão 2 — Valor</label>
            <input v-model="form.ceo.badge2Value" class="hcms__input" placeholder="ex: +200 fechadas" />
          </div>
        </div>
      </div>

      <!-- Footer CTA -->
      <div v-show="activeTab === 'footerCta'" class="hcms__section">
        <h2 class="hcms__section-title">Seção Footer CTA (Banner Final)</h2>
        <div class="hcms__field">
          <label class="hcms__label">Título</label>
          <input v-model="form.footerCta.title" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Subtítulo</label>
          <input v-model="form.footerCta.subtitle" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Texto Botão Principal</label>
          <input v-model="form.footerCta.primaryText" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Link Botão Principal</label>
          <input v-model="form.footerCta.primaryLink" class="hcms__input" placeholder="/register" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Texto Botão Outline</label>
          <input v-model="form.footerCta.outlineText" class="hcms__input" />
        </div>
        <div class="hcms__field">
          <label class="hcms__label">Link Botão Outline</label>
          <input v-model="form.footerCta.outlineLink" class="hcms__input" placeholder="/login" />
        </div>
      </div>

      <div class="hcms__actions">
        <button class="hcms__btn-save" :disabled="saving" @click="save">
          <svg v-if="!saving" width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M19 21H5a2 2 0 01-2-2V5a2 2 0 012-2h11l5 5v11a2 2 0 01-2 2z"/><polyline points="17 21 17 13 7 13 7 21"/><polyline points="7 3 7 8 15 8"/></svg>
          <span v-if="saving" class="hcms__spinner"></span>
          {{ saving ? 'Salvando...' : 'Salvar Todas as Alterações' }}
        </button>
        <button class="hcms__btn-reset" @click="reset" :disabled="saving">
          Descartar Alterações
        </button>
      </div>
    </div>

    <div v-else-if="!loadError" class="hcms__loading">
      <span class="hcms__spinner"></span> Carregando configuração...
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import type { HomeConfig, HeroCard } from '@/composables/useHomeConfig';
import { useAgenciaStore } from '@/composables/useAgenciaStore';

const DEFAULT_HERO_CARDS: HeroCard[] = [
  { ativo: true, label: 'PIX INSTANTÂNEO', value: 'Saque em segundos ✓', valueColor: 'green', icon: 'card', iconBg: 'teal' },
  { ativo: true, label: 'CASHBACK RECEBIDO', value: 'R$ 50,00', valueColor: 'green', icon: 'chart', iconBg: 'green' },
  { ativo: true, label: 'MARCAS PARCEIRAS', value: '+500 lojas', valueColor: 'teal', icon: 'bag', iconBg: 'teal' },
];

definePageMeta({ middleware: 'agencia-admin' });

const agenciaStore = useAgenciaStore();
function authHeader() {
  const token = agenciaStore.getToken();
  if (!token) return {};
  return { headers: { Authorization: `Bearer ${token}` } };
}

const tabs = [
  { key: 'hero', label: 'Hero' },
  { key: 'heroCards', label: 'Cartões Hero' },
  { key: 'brands', label: 'Marcas' },
  { key: 'ofertas', label: 'Ofertas' },
  { key: 'parceirosOnline', label: 'Parceiros Online' },
  { key: 'parceirosLocais', label: 'Parceiros Locais' },
  { key: 'testimonials', label: 'Depoimentos' },
  { key: 'blog', label: 'Blog' },
  { key: 'ceo', label: 'CEO' },
  { key: 'footerCta', label: 'Footer CTA' },
];

const activeTab = ref('hero');
const form = ref<HomeConfig | null>(null);
const originalJson = ref('');
const saving = ref(false);
const saveSuccess = ref(false);
const saveError = ref(false);
const loadError = ref(false);

const previewHeroTitle = computed(() => {
  if (!form.value) return '';
  return form.value.hero.title
    .replace('<highlight>', '<span style="color:#98C73A;font-weight:800;">')
    .replace('</highlight>', '</span>');
});

async function load() {
  loadError.value = false;
  try {
    const data = await $fetch<HomeConfig>('/api/admin/home-config', { ...authHeader() });
    const parsed: HomeConfig = JSON.parse(JSON.stringify(data));
    if (!parsed.heroCards || parsed.heroCards.length === 0) {
      parsed.heroCards = JSON.parse(JSON.stringify(DEFAULT_HERO_CARDS));
    }
    form.value = parsed;
    originalJson.value = JSON.stringify(parsed);
  } catch {
    loadError.value = true;
  }
}

async function save() {
  if (!form.value) return;
  saving.value = true;
  saveSuccess.value = false;
  saveError.value = false;
  try {
    await $fetch('/api/admin/home-config', {
      method: 'POST',
      body: form.value,
      ...authHeader(),
    });
    originalJson.value = JSON.stringify(form.value);
    saveSuccess.value = true;
    setTimeout(() => { saveSuccess.value = false; }, 4000);
  } catch {
    saveError.value = true;
    setTimeout(() => { saveError.value = false; }, 5000);
  } finally {
    saving.value = false;
  }
}

function reset() {
  if (originalJson.value) {
    form.value = JSON.parse(originalJson.value);
  }
}

const ceoUploading = ref(false);
const ceoUploadError = ref('');
const ceoLocalPreview = ref('');
const ceoFileInput = ref<HTMLInputElement | null>(null);

function triggerCeoFileInput() { ceoFileInput.value?.click(); }

function onCeoFileChange(event: Event) {
  const input = event.target as HTMLInputElement;
  const file = input.files?.[0];
  if (!file) return;
  ceoLocalPreview.value = URL.createObjectURL(file);
  uploadCeoImage(file, input);
}

async function uploadCeoImage(file: File, input: HTMLInputElement) {
  if (!form.value) return;
  ceoUploading.value = true;
  ceoUploadError.value = '';
  try {
    const token = agenciaStore.getToken();
    const fd = new FormData();
    fd.append('files', file);
    const res = await $fetch<{ url: string }>('/api/admin/upload-banner', {
      method: 'POST',
      body: fd,
      headers: token ? { Authorization: `Bearer ${token}` } : {},
    });
    form.value.ceo.imagemFundo = res.url;
    ceoLocalPreview.value = '';
  } catch {
    ceoUploadError.value = 'Erro ao fazer upload. Tente novamente.';
    ceoLocalPreview.value = '';
  } finally {
    ceoUploading.value = false;
    input.value = '';
  }
}

function removeCeoImage() {
  if (form.value) {
    form.value.ceo.imagemFundo = '';
    ceoLocalPreview.value = '';
    ceoUploadError.value = '';
  }
}

onMounted(() => load());
</script>

<style scoped>
.hcms {
  max-width: 860px;
  margin: 0 auto;
  padding: 32px 24px 80px;
}

.hcms__header {
  margin-bottom: 28px;
}

.hcms__title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 24px;
  font-weight: 800;
  color: #111827;
  letter-spacing: -0.02em;
  margin-bottom: 6px;
}

.hcms__sub {
  font-size: 14px;
  color: #6b7280;
}

.hcms__alert {
  padding: 12px 16px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 600;
  margin-bottom: 20px;
}

.hcms__alert--success {
  background: #d1fae5;
  color: #065f46;
  border: 1px solid #6ee7b7;
}

.hcms__alert--error {
  background: #fee2e2;
  color: #991b1b;
  border: 1px solid #fca5a5;
}

.hcms__tabs {
  border-bottom: 2px solid #e5e7eb;
  margin-bottom: 28px;
  flex-wrap: wrap;
  gap: 2px;
}

.hcms__tabs .nav-link {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 600;
  color: #6b7280;
  border: none;
  border-bottom: 2px solid transparent;
  padding: 8px 14px;
  margin-bottom: -2px;
  background: transparent;
  cursor: pointer;
  transition: all 0.15s ease;
  border-radius: 0;
}

.hcms__tabs .nav-link:hover {
  color: #2F7785;
}

.hcms__tabs .nav-link.active {
  color: #2F7785;
  border-bottom-color: #2F7785;
  background: transparent;
}

.hcms__section-title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 16px;
  font-weight: 700;
  color: #111827;
  margin-bottom: 20px;
  padding-bottom: 10px;
  border-bottom: 1px solid #f0f0f0;
}

.hcms__field {
  margin-bottom: 18px;
}

.hcms__label {
  display: block;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 600;
  color: #374151;
  margin-bottom: 6px;
}

.hcms__input,
.hcms__textarea {
  width: 100%;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  color: #111827;
  background: #fff;
  border: 1.5px solid #d1d5db;
  border-radius: 8px;
  padding: 10px 14px;
  outline: none;
  transition: border-color 0.15s ease;
  resize: vertical;
}

.hcms__ceo-drop-area {
  border: 1.5px dashed #d1d5db;
  border-radius: 10px;
  background: #f9fafb;
  cursor: pointer;
  overflow: hidden;
  transition: border-color 0.15s, background 0.15s;
}

.hcms__ceo-drop-area:hover {
  border-color: #2F7785;
  background: #f0fafa;
}

.hcms__ceo-drop-area:hover .hcms__ceo-drop-hover {
  opacity: 1 !important;
}

.hcms__upload-btn {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  background: #f3f4f6;
  border: 1.5px dashed #d1d5db;
  border-radius: 8px;
  padding: 9px 18px;
  font-size: 13px;
  font-weight: 600;
  color: #374151;
  cursor: pointer;
  transition: all 0.15s ease;
  margin-top: 6px;
}

.hcms__upload-btn:hover:not(.hcms__upload-btn--loading) {
  border-color: #2F7785;
  color: #2F7785;
  background: #f0fafa;
}

.hcms__upload-btn--loading {
  opacity: 0.7;
  cursor: not-allowed;
}

.hcms__img-preview-wrap {
  position: relative;
  display: inline-block;
  margin-bottom: 8px;
}

.hcms__img-preview {
  display: block;
  width: 100%;
  max-height: 120px;
  object-fit: cover;
  border-radius: 8px;
  border: 1.5px solid #e5e7eb;
}

.hcms__img-remove {
  position: absolute;
  top: 6px;
  right: 6px;
  background: rgba(0,0,0,0.65);
  color: #fff;
  border: 0;
  border-radius: 6px;
  font-size: 11px;
  font-weight: 700;
  padding: 4px 8px;
  cursor: pointer;
  transition: background 0.15s;
}

.hcms__img-remove:hover {
  background: rgba(180,0,0,0.8);
}

.hcms__spinner-sm {
  display: inline-block;
  width: 13px;
  height: 13px;
  border: 2px solid rgba(47,119,133,0.25);
  border-top-color: #2F7785;
  border-radius: 50%;
  animation: hcms-spin 0.7s linear infinite;
}

.hcms__input:focus,
.hcms__textarea:focus {
  border-color: #2F7785;
  box-shadow: 0 0 0 3px rgba(47, 119, 133, 0.12);
}

.hcms__hint {
  display: block;
  font-size: 12px;
  color: #9ca3af;
  margin-top: 5px;
}

.hcms__callout {
  background: #f0fdf4;
  border: 1px solid #bbf7d0;
  border-radius: 8px;
  padding: 14px 16px;
  font-size: 14px;
  color: #166534;
  margin-top: 10px;
}

.hcms__link {
  color: #2F7785;
  font-weight: 600;
  text-decoration: underline;
}

.hcms__actions {
  display: flex;
  gap: 12px;
  margin-top: 32px;
  padding-top: 24px;
  border-top: 1px solid #e5e7eb;
  flex-wrap: wrap;
}

.hcms__btn-save {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 700;
  padding: 12px 28px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  box-shadow: 0 4px 16px rgba(152, 199, 58, 0.3);
}

.hcms__btn-save:hover:not(:disabled) {
  background: #7aad1f;
  transform: translateY(-1px);
}

.hcms__btn-save:disabled {
  opacity: 0.65;
  cursor: not-allowed;
}

.hcms__btn-reset {
  display: inline-flex;
  align-items: center;
  background: transparent;
  color: #6b7280;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 600;
  padding: 12px 20px;
  border-radius: 8px;
  border: 1.5px solid #e5e7eb;
  cursor: pointer;
  transition: all 0.15s ease;
}

.hcms__btn-reset:hover:not(:disabled) {
  border-color: #d1d5db;
  color: #374151;
}

.hcms__btn-reset:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.hcms__loading {
  display: flex;
  align-items: center;
  gap: 10px;
  color: #6b7280;
  font-size: 14px;
  padding: 32px 0;
}

.hcms__spinner {
  display: inline-block;
  width: 16px;
  height: 16px;
  border: 2px solid rgba(47, 119, 133, 0.25);
  border-top-color: #2F7785;
  border-radius: 50%;
  animation: hcms-spin 0.7s linear infinite;
  flex-shrink: 0;
}

@keyframes hcms-spin {
  to { transform: rotate(360deg); }
}

.hcms__card-editor {
  border: 1.5px solid #e5e7eb;
  border-radius: 12px;
  margin-bottom: 20px;
  overflow: hidden;
}

.hcms__card-editor-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 18px;
  background: #f9fafb;
  border-bottom: 1px solid #e5e7eb;
  font-size: 14px;
  font-weight: 700;
  color: #111827;
}

.hcms__card-editor-body {
  padding: 18px;
}

.hcms__toggle {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13px;
  font-weight: 600;
  color: #374151;
  cursor: pointer;
  user-select: none;
}

.hcms__toggle input {
  display: none;
}

.hcms__toggle-track {
  width: 36px;
  height: 20px;
  border-radius: 999px;
  background: #d1d5db;
  position: relative;
  transition: background 0.2s;
}

.hcms__toggle input:checked ~ .hcms__toggle-track {
  background: #2F7785;
}

.hcms__toggle-thumb {
  position: absolute;
  top: 2px;
  left: 2px;
  width: 16px;
  height: 16px;
  border-radius: 50%;
  background: #fff;
  transition: transform 0.2s;
  box-shadow: 0 1px 3px rgba(0,0,0,0.15);
}

.hcms__toggle input:checked ~ .hcms__toggle-track .hcms__toggle-thumb {
  transform: translateX(16px);
}

.hcms__btn-group {
  display: flex;
  gap: 6px;
}

.hcms__btn-group--wrap {
  flex-wrap: wrap;
}

.hcms__btn-opt {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  padding: 6px 12px;
  border: 1.5px solid #d1d5db;
  border-radius: 6px;
  background: #fff;
  font-size: 12px;
  font-weight: 600;
  color: #374151;
  cursor: pointer;
  transition: all 0.15s;
  font-family: 'Inter', 'Jost', sans-serif;
}

.hcms__btn-opt:hover {
  border-color: #2F7785;
  color: #2F7785;
}

.hcms__btn-opt.active {
  border-color: #2F7785;
  background: #2F7785;
  color: #fff;
}
</style>
