<template>
  <div class="hcon">
    <QsPageHeader eyebrow="Admin · CMS" title="Conteúdo da Home" description="Edite os textos e conteúdo das seções da página inicial. As alterações ficam visíveis após salvar.">
      <template v-if="form">
        <nuxt-link to="/" target="_blank" class="qs-btn-outline">Ver Home ↗</nuxt-link>
        <button class="qs-btn-outline" @click="reset" :disabled="saving">Descartar</button>
        <button class="qs-btn-primary" :disabled="saving" @click="save">
          <span v-if="saving" class="hcon__spin" />
          {{ saving ? 'Salvando...' : 'Salvar' }}
        </button>
      </template>
    </QsPageHeader>

    <div v-if="loadError" class="qs-alert-danger" style="margin-bottom:16px">
      Erro ao carregar configuração. Tente recarregar a página.
    </div>
    <div v-if="saveSuccess" class="qs-alert-success" style="margin-bottom:16px">
      Configuração salva com sucesso!
    </div>
    <div v-if="saveError" class="qs-alert-danger" style="margin-bottom:16px">
      Erro ao salvar. Tente novamente.
    </div>

    <div v-if="form" class="hcon__wrap">
      <div class="hcon__tabs">
        <button
          v-for="tab in tabs"
          :key="tab.key"
          class="hcon__tab"
          :class="{ active: activeTab === tab.key }"
          @click="activeTab = tab.key"
        >{{ tab.label }}</button>
      </div>

      <!-- ─── Hero ─────────────────────────────────────────────── -->
      <div v-show="activeTab === 'hero'" class="hcon__section">
        <h2 class="hcon__section-title">Seção Hero</h2>
        <div class="hcon__field">
          <label class="hcon__label">Texto do Badge</label>
          <input v-model="form.hero.badge" class="hcon__input" placeholder="+12.000 usuários economizando" />
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Título <small style="color:#6b7280">(use &lt;highlight&gt;palavra&lt;/highlight&gt; para verde)</small></label>
          <input v-model="form.hero.title" class="hcon__input" />
          <span class="hcon__hint">Preview: <span v-html="previewHeroTitle"></span></span>
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Subtítulo</label>
          <textarea v-model="form.hero.subtitle" class="hcon__textarea" rows="2"></textarea>
        </div>
        <div class="hcon__row">
          <div class="hcon__field">
            <label class="hcon__label">Botão Principal — Texto</label>
            <input v-model="form.hero.ctaPrimaryText" class="hcon__input" />
          </div>
          <div class="hcon__field">
            <label class="hcon__label">Botão Principal — Link</label>
            <input v-model="form.hero.ctaPrimaryLink" class="hcon__input" placeholder="/register" />
          </div>
        </div>
      </div>

      <!-- ─── Testemunhos ───────────────────────────────────────── -->
      <div v-show="activeTab === 'testimonials'" class="hcon__section">
        <h2 class="hcon__section-title">Seção Depoimentos</h2>
        <div class="hcon__field">
          <label class="hcon__label">Título da Seção</label>
          <input v-model="form.testimonials.title" class="hcon__input" />
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Subtítulo da Seção</label>
          <input v-model="form.testimonials.subtitle" class="hcon__input" />
        </div>

        <div v-for="(t, idx) in form.testimonials.items" :key="idx" class="hcon__card-editor">
          <div class="hcon__card-editor-header">
            <strong>Depoimento {{ idx + 1 }}</strong>
          </div>
          <div class="hcon__card-editor-body">
            <div class="hcon__row">
              <div class="hcon__field">
                <label class="hcon__label">Nome</label>
                <input v-model="t.name" class="hcon__input" placeholder="Ex: Marina Costa" />
              </div>
              <div class="hcon__field">
                <label class="hcon__label">Iniciais (avatar)</label>
                <input v-model="t.initials" class="hcon__input" placeholder="Ex: MC" maxlength="3" style="text-transform:uppercase" />
              </div>
            </div>
            <div class="hcon__field">
              <label class="hcon__label">Cargo / Descrição</label>
              <input v-model="t.role" class="hcon__input" placeholder="Ex: Usuária há 8 meses" />
            </div>
            <div class="hcon__field">
              <label class="hcon__label">Trecho em destaque</label>
              <input v-model="t.highlight" class="hcon__input" placeholder="Ex: Já recebi mais de R$ 1.240 de cashback" />
            </div>
            <div class="hcon__field">
              <label class="hcon__label">Continuação do texto</label>
              <textarea v-model="t.text" class="hcon__textarea" rows="2" placeholder="Ex:  em apenas 6 meses. Nunca mais compro sem a Quanta!"></textarea>
            </div>
          </div>
        </div>
      </div>

      <!-- ─── Blog ──────────────────────────────────────────────── -->
      <div v-show="activeTab === 'blog'" class="hcon__section">
        <h2 class="hcon__section-title">Seção Blog & Redes Sociais</h2>
        <div class="hcon__field">
          <label class="hcon__label">Rótulo (texto pequeno acima do título)</label>
          <input v-model="form.blog.label" class="hcon__input" />
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Título da Seção</label>
          <input v-model="form.blog.title" class="hcon__input" />
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Subtítulo da Seção</label>
          <input v-model="form.blog.subtitle" class="hcon__input" />
        </div>

        <p class="hcon__hint" style="margin-bottom:16px">Os posts abaixo são exibidos quando não há artigos cadastrados no Blog Admin ou posts de redes sociais.</p>

        <div v-for="(post, idx) in form.blog.posts" :key="idx" class="hcon__card-editor">
          <div class="hcon__card-editor-header">
            <strong>Post {{ idx + 1 }}</strong>
          </div>
          <div class="hcon__card-editor-body">
            <div class="hcon__field">
              <label class="hcon__label">Título</label>
              <input v-model="post.title" class="hcon__input" />
            </div>
            <div class="hcon__field">
              <label class="hcon__label">Resumo</label>
              <textarea v-model="post.excerpt" class="hcon__textarea" rows="2"></textarea>
            </div>
            <div class="hcon__row">
              <div class="hcon__field">
                <label class="hcon__label">Data</label>
                <input v-model="post.date" class="hcon__input" placeholder="Ex: 01 Mai 2025" />
              </div>
              <div class="hcon__field">
                <label class="hcon__label">Slug (URL)</label>
                <input v-model="post.slug" class="hcon__input" placeholder="ex: titulo-do-post" />
              </div>
            </div>
            <div class="hcon__field">
              <label class="hcon__label">URL da Imagem</label>
              <input v-model="post.img" class="hcon__input" placeholder="https://..." />
              <div v-if="post.img" style="margin-top:8px">
                <img :src="post.img" alt="preview" style="height:80px;object-fit:cover;border-radius:6px;border:1px solid #e5e7eb;" />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- ─── CEO ───────────────────────────────────────────────── -->
      <div v-show="activeTab === 'ceo'" class="hcon__section">
        <h2 class="hcon__section-title">Seção CEO</h2>
        <div class="hcon__field">
          <label class="hcon__label d-flex align-items-center gap-2" style="cursor:pointer">
            <input type="checkbox" v-model="form.ceo.ativo" style="width:16px;height:16px;" />
            Exibir esta seção na home
          </label>
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Tag</label>
          <input v-model="form.ceo.tag" class="hcon__input" placeholder="CEO & Founder" />
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Pré-título</label>
          <input v-model="form.ceo.pre" class="hcon__input" placeholder="Fale com o CEO" />
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Nome</label>
          <input v-model="form.ceo.name" class="hcon__input" />
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Descrição</label>
          <textarea v-model="form.ceo.desc" class="hcon__textarea" rows="2"></textarea>
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Texto do Botão</label>
          <input v-model="form.ceo.ctaText" class="hcon__input" />
        </div>
        <div class="hcon__row">
          <div class="hcon__field">
            <label class="hcon__label">Badge 1 — Rótulo</label>
            <input v-model="form.ceo.badge1Label" class="hcon__input" placeholder="Respostas" />
          </div>
          <div class="hcon__field">
            <label class="hcon__label">Badge 1 — Valor</label>
            <input v-model="form.ceo.badge1Value" class="hcon__input" placeholder="Em até 24h" />
          </div>
        </div>
        <div class="hcon__row">
          <div class="hcon__field">
            <label class="hcon__label">Badge 2 — Rótulo</label>
            <input v-model="form.ceo.badge2Label" class="hcon__input" placeholder="Parcerias" />
          </div>
          <div class="hcon__field">
            <label class="hcon__label">Badge 2 — Valor</label>
            <input v-model="form.ceo.badge2Value" class="hcon__input" placeholder="+200 fechadas" />
          </div>
        </div>
      </div>

      <!-- ─── Footer CTA ─────────────────────────────────────────── -->
      <div v-show="activeTab === 'footerCta'" class="hcon__section">
        <h2 class="hcon__section-title">Banner Final (Footer CTA)</h2>
        <div class="hcon__field">
          <label class="hcon__label">Título</label>
          <input v-model="form.footerCta.title" class="hcon__input" />
        </div>
        <div class="hcon__field">
          <label class="hcon__label">Subtítulo</label>
          <input v-model="form.footerCta.subtitle" class="hcon__input" />
        </div>
        <div class="hcon__row">
          <div class="hcon__field">
            <label class="hcon__label">Botão Principal — Texto</label>
            <input v-model="form.footerCta.primaryText" class="hcon__input" />
          </div>
          <div class="hcon__field">
            <label class="hcon__label">Botão Principal — Link</label>
            <input v-model="form.footerCta.primaryLink" class="hcon__input" placeholder="/register" />
          </div>
        </div>
        <div class="hcon__row">
          <div class="hcon__field">
            <label class="hcon__label">Botão Outline — Texto</label>
            <input v-model="form.footerCta.outlineText" class="hcon__input" />
          </div>
          <div class="hcon__field">
            <label class="hcon__label">Botão Outline — Link</label>
            <input v-model="form.footerCta.outlineLink" class="hcon__input" placeholder="/login" />
          </div>
        </div>
      </div>

      <div class="hcon__footer-actions">
        <button class="qs-btn-outline" @click="reset" :disabled="saving">Descartar Alterações</button>
        <button class="qs-btn-primary" :disabled="saving" @click="save" style="display:inline-flex;align-items:center;gap:8px">
          <svg v-if="!saving" width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M19 21H5a2 2 0 01-2-2V5a2 2 0 012-2h11l5 5v11a2 2 0 01-2 2z"/><polyline points="17 21 17 13 7 13 7 21"/><polyline points="7 3 7 8 15 8"/></svg>
          <span v-if="saving" class="hcon__spin" />
          {{ saving ? 'Salvando...' : 'Salvar Todas as Alterações' }}
        </button>
      </div>
    </div>

    <div v-else-if="!loadError" class="qs-loading">
      <div class="qs-spinner" /> Carregando configuração...
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useHomeCmsStore } from '@/pinia/useHomeCmsStore';
import { useAgenciaStore } from '~/pinia/useAgenciaStore';
import type { HomeConfig } from '@/composables/useHomeConfig';
import { DEFAULT_CONFIG } from '@/composables/useHomeConfig';

definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const homeCmsStore = useHomeCmsStore();
const agenciaStore = useAgenciaStore();

const tabs = [
  { key: 'hero', label: 'Hero' },
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
    await homeCmsStore.fetchConfig();
    const src = homeCmsStore.config ?? DEFAULT_CONFIG;
    const data: HomeConfig = JSON.parse(JSON.stringify(src));
    if (!data.testimonials?.items?.length) {
      data.testimonials = { ...DEFAULT_CONFIG.testimonials };
    }
    if (!data.blog?.posts?.length) {
      data.blog = { ...DEFAULT_CONFIG.blog };
    }
    form.value = data;
    originalJson.value = JSON.stringify(data);
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
    const token = agenciaStore.getToken();
    await homeCmsStore.saveConfig(form.value, token);
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

onMounted(() => load());
</script>

<style scoped>
.hcon {
  max-width: 780px;
  margin: 0 auto;
  padding: 32px 24px 80px;
}

.hcon__wrap {
  margin-top: 0;
}

.hcon__tabs {
  display: flex;
  flex-wrap: wrap;
  gap: 2px;
  border-bottom: 2px solid #e5e7eb;
  margin-bottom: 28px;
}

.hcon__tab {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 600;
  color: #6b7280;
  border: none;
  border-bottom: 2px solid transparent;
  padding: 8px 16px;
  margin-bottom: -2px;
  background: transparent;
  cursor: pointer;
  transition: all 0.15s ease;
}

.hcon__tab:hover { color: #2F7785; }

.hcon__tab.active {
  color: #2F7785;
  border-bottom-color: #2F7785;
}

.hcon__section {
  padding: 4px 0 24px;
}

.hcon__section-title {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 16px;
  font-weight: 700;
  color: #111827;
  margin-bottom: 20px;
  padding-bottom: 10px;
  border-bottom: 1px solid #f0f0f0;
}

.hcon__field {
  margin-bottom: 16px;
}

.hcon__row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
}

@media (max-width: 575px) {
  .hcon__row { grid-template-columns: 1fr; }
}

.hcon__label {
  display: block;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  font-weight: 600;
  color: #374151;
  margin-bottom: 6px;
}

.hcon__input,
.hcon__textarea {
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

.hcon__input:focus,
.hcon__textarea:focus {
  border-color: #2F7785;
  box-shadow: 0 0 0 3px rgba(47, 119, 133, 0.10);
}

.hcon__hint {
  display: block;
  margin-top: 6px;
  font-size: 12px;
  color: #6b7280;
}

.hcon__card-editor {
  border: 1.5px solid #e5e7eb;
  border-radius: 10px;
  margin-bottom: 16px;
  overflow: hidden;
}

.hcon__card-editor-header {
  background: #f9fafb;
  padding: 10px 16px;
  border-bottom: 1px solid #e5e7eb;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 13px;
  color: #374151;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.hcon__card-editor-body {
  padding: 16px;
}

.hcon__footer-actions {
  display: flex;
  gap: 12px;
  justify-content: flex-end;
  margin-top: 32px;
  padding-top: 20px;
  border-top: 1px solid #e5e7eb;
}

.hcon__spin {
  display: inline-block;
  width: 14px;
  height: 14px;
  border: 2px solid rgba(255,255,255,0.4);
  border-top-color: #fff;
  border-radius: 50%;
  animation: hcon-spin 0.7s linear infinite;
}

@keyframes hcon-spin {
  to { transform: rotate(360deg); }
}
</style>
