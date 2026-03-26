<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Carrosseis</h1><p>Banners do hero da home — arraste as setas para reordenar</p></div>
      <button class="btn btn-ag-primary" @click="abrirNovo">+ Novo Banner</button>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="importadoMsg" class="alert alert-success d-flex align-items-center gap-2 mb-3 py-2 px-3">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M20 6L9 17l-5-5"/></svg>
        {{ importadoMsg }}
        <button type="button" class="btn-close ms-auto" style="font-size:11px" @click="importadoMsg = ''"></button>
      </div>
      <div v-if="itens.length === 0" class="ag-empty-state">
        <h5>Nenhum banner criado ainda</h5>
        <p class="text-muted">Clique em <strong>+ Novo Banner</strong> para criar o primeiro slide do hero.</p>
        <div class="mt-3 d-flex flex-column align-items-center gap-2">
          <p class="text-muted small mb-0">Ou importe os banners que estão atualmente no ar:</p>
          <button class="btn btn-ag-outline" :disabled="importando" @click="importarDoSistemaAntigo">
            <span v-if="importando">
              <span class="spinner-border spinner-border-sm me-1" />
              Importando...
            </span>
            <span v-else>
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" class="me-1"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="7 10 12 15 17 10"/><line x1="12" y1="15" x2="12" y2="3"/></svg>
              Importar do sistema antigo
            </span>
          </button>
          <div v-if="importErro" class="alert alert-danger py-2 px-3 small mt-1">{{ importErro }}</div>
        </div>
      </div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead>
            <tr>
              <th style="width:70px">Ordem</th>
              <th>Título</th>
              <th>Imagem</th>
              <th>Headline</th>
              <th>Ativo</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, i) in itens" :key="item.id">
              <td>
                <div class="d-flex flex-column gap-1">
                  <button
                    class="btn btn-xs btn-ag-outline car-order-btn"
                    :disabled="i === 0"
                    title="Subir"
                    @click="moverItem(item, 'up')"
                  >↑</button>
                  <button
                    class="btn btn-xs btn-ag-outline car-order-btn"
                    :disabled="i === itens.length - 1"
                    title="Descer"
                    @click="moverItem(item, 'down')"
                  >↓</button>
                </div>
              </td>
              <td class="fw-bold align-middle">{{ item.titulo }}</td>
              <td class="align-middle">
                <img v-if="item.url" :src="item.url" alt="" style="height:40px;width:80px;object-fit:cover;border-radius:4px;" />
                <span v-else class="text-muted">—</span>
              </td>
              <td class="align-middle">
                <span v-if="item.headline" class="text-truncate d-block" style="max-width:160px">{{ item.headline }}</span>
                <span v-else class="text-muted small">Global (CMS)</span>
              </td>
              <td class="align-middle">
                <span class="badge-ag" :class="item.ativo ? 'badge-ag-success' : 'badge-ag-warning'">
                  {{ item.ativo ? 'Ativo' : 'Inativo' }}
                </span>
              </td>
              <td class="align-middle">
                <div class="d-flex gap-1">
                  <button class="btn btn-sm btn-ag-outline" @click="abrirEditar(item)">Editar</button>
                  <button class="btn btn-sm btn-outline-danger" @click="confirmarExcluir(item)">Excluir</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal car-modal-wide">
        <div class="ag-modal-header">
          <h5 class="mb-0">{{ form.id ? 'Editar Banner' : 'Novo Banner' }}</h5>
          <button class="btn-close" @click="fecharModal" />
        </div>

        <div class="car-tabs">
          <button
            v-for="tab in tabs"
            :key="tab.key"
            class="car-tab-btn"
            :class="{ active: abaAtiva === tab.key }"
            @click="abaAtiva = tab.key"
          >
            <span class="car-tab-icon" v-html="tab.icon"></span>
            {{ tab.label }}
          </button>
        </div>

        <div class="ag-modal-body car-modal-body">

          <div v-if="abaAtiva === 'imagem'">
            <div class="car-dim-info mb-3">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
              <span>Dimensões recomendadas: <strong>1920 × 600 px</strong> — formato horizontal, JPG ou WEBP comprimido</span>
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Título interno *</label>
              <input v-model="form.titulo" type="text" class="form-control" placeholder="Ex: Campanha Natal 2025" />
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Imagem do Banner</label>
              <div class="d-flex gap-2 mb-2">
                <button type="button" class="btn btn-sm" :class="modoImagem === 'url' ? 'btn-ag-primary' : 'btn-ag-outline'" @click="modoImagem = 'url'">Usar URL</button>
                <button type="button" class="btn btn-sm" :class="modoImagem === 'arquivo' ? 'btn-ag-primary' : 'btn-ag-outline'" @click="modoImagem = 'arquivo'">Enviar Arquivo</button>
              </div>

              <div v-if="modoImagem === 'url'">
                <input v-model="form.url" type="url" class="form-control" placeholder="https://..." />
                <img v-if="form.url" :src="form.url" alt="preview" class="mt-2 rounded" style="max-height:120px;max-width:100%;object-fit:contain;border:1px solid #dee2e6;" />
              </div>

              <div v-else>
                <div
                  class="upload-drop-area"
                  :class="{ 'drag-over': isDragging }"
                  @dragover.prevent="isDragging = true"
                  @dragleave="isDragging = false"
                  @drop.prevent="onDrop"
                  @click="triggerFileInput"
                >
                  <input ref="fileInputRef" type="file" accept="image/*" style="display:none" @change="onFileChange" />
                  <div v-if="!arquivoPreview" class="upload-drop-placeholder">
                    <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="17 8 12 3 7 8"/><line x1="12" y1="3" x2="12" y2="15"/></svg>
                    <p class="mb-1 mt-2">Arraste uma imagem ou <strong>clique para selecionar</strong></p>
                    <small class="text-muted">PNG, JPG, WEBP — máx. 5MB — ideal 1920×600px</small>
                  </div>
                  <div v-else class="position-relative">
                    <img :src="arquivoPreview" alt="preview" style="max-height:140px;max-width:100%;object-fit:contain;border-radius:4px;" />
                    <button type="button" class="btn btn-sm btn-outline-danger mt-2" @click.stop="limparArquivo">Remover</button>
                  </div>
                </div>
                <button v-if="arquivoFile" type="button" class="btn btn-ag-primary btn-sm mt-2 w-100" :disabled="uploadingFile" @click="fazerUpload">
                  {{ uploadingFile ? 'Enviando...' : 'Enviar imagem' }}
                </button>
                <div v-if="form.url && modoImagem === 'arquivo'" class="mt-2 d-flex align-items-center gap-2">
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#98C73A" stroke-width="2"><path d="M20 6L9 17l-5-5"/></svg>
                  <small class="text-success fw-bold">Imagem enviada com sucesso</small>
                </div>
              </div>
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">URL de Destino</label>
              <input v-model="form.urlDestino" type="url" class="form-control" placeholder="https://..." />
            </div>

            <div class="mb-3">
              <div class="form-check">
                <input v-model="form.ativo" type="checkbox" class="form-check-input" id="car-ativo" />
                <label class="form-check-label" for="car-ativo">Ativo (aparece na home)</label>
              </div>
            </div>
          </div>

          <div v-if="abaAtiva === 'campanha'">
            <div class="car-dim-info mb-3">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
              <span>Campos opcionais. Se vazios, os textos do <strong>CMS Global</strong> serão usados no hero.</span>
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Badge (etiqueta)</label>
              <input v-model="form.badge" type="text" class="form-control" placeholder="Ex: +12.000 usuários economizando" />
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Headline (título principal)</label>
              <input v-model="form.headline" type="text" class="form-control" placeholder="Ex: Seu dinheiro volta a cada compra" />
              <small class="text-muted">Use &lt;highlight&gt;palavra&lt;/highlight&gt; para destacar em verde.</small>
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Subtítulo</label>
              <textarea v-model="form.subtitulo" class="form-control" rows="2" placeholder="Ex: Cashback real em centenas de lojas parceiras." />
            </div>

            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label class="form-label fw-bold">Texto do Botão CTA</label>
                <input v-model="form.ctaTexto" type="text" class="form-control" placeholder="Ex: Criar Conta Grátis" />
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold">Link do Botão CTA</label>
                <input v-model="form.ctaLink" type="text" class="form-control" placeholder="/register" />
              </div>
            </div>

            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label class="form-label fw-bold">Cor do Botão</label>
                <div class="d-flex align-items-center gap-2">
                  <input v-model="form.ctaCor" type="color" class="form-control form-control-color" style="width:48px;height:38px;padding:2px;" />
                  <input v-model="form.ctaCor" type="text" class="form-control form-control-sm" placeholder="#98C73A" style="font-family:monospace;" />
                </div>
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold">Cor do Texto</label>
                <div class="d-flex gap-2">
                  <button type="button" class="btn btn-sm car-text-claro" :class="{ active: form.textoCor === 'light' }" @click="form.textoCor = 'light'">
                    ☀ Claro (branco)
                  </button>
                  <button type="button" class="btn btn-sm car-text-escuro" :class="{ active: form.textoCor === 'dark' }" @click="form.textoCor = 'dark'">
                    ◑ Escuro (preto)
                  </button>
                </div>
              </div>
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Intensidade do Overlay escuro: {{ form.overlayIntensidade }}%</label>
              <input v-model.number="form.overlayIntensidade" type="range" class="form-range" min="0" max="95" step="5" />
              <div class="d-flex justify-content-between">
                <small class="text-muted">Transparente (0%)</small>
                <small class="text-muted">Opaco (95%)</small>
              </div>
            </div>
          </div>

          <div v-if="abaAtiva === 'preview'">
            <p class="text-muted small mb-3">Preview em tempo real de como o slide aparecerá na home. Salve para publicar.</p>
            <div class="car-preview-wrap">
              <div
                class="car-preview"
                :style="{
                  backgroundImage: form.url ? `url(${form.url})` : 'linear-gradient(135deg, #1a4a54, #225F6B)',
                }"
              >
                <div class="car-preview-overlay" :style="{ opacity: form.overlayIntensidade / 100 }"></div>
                <div class="car-preview-content" :class="form.textoCor === 'dark' ? 'text-dark-mode' : 'text-light-mode'">
                  <div v-if="form.badge" class="car-preview-badge">
                    <span class="car-preview-badge-dot"></span>
                    {{ form.badge }}
                  </div>
                  <div v-else class="car-preview-badge car-preview-badge-placeholder">
                    <span class="car-preview-badge-dot"></span>
                    Badge (CMS global)
                  </div>

                  <h2 class="car-preview-title" v-html="previewHeadline"></h2>
                  <p class="car-preview-subtitle">{{ form.subtitulo || 'Subtítulo (CMS global)' }}</p>

                  <a
                    class="car-preview-cta"
                    :style="{ background: form.ctaCor || '#98C73A', color: isCtaColorDark ? '#fff' : '#1a2236' }"
                  >
                    {{ form.ctaTexto || 'Criar Conta Grátis' }}
                    <svg width="13" height="13" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
                  </a>
                </div>
              </div>
              <div v-if="!form.url" class="car-preview-no-img">
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#adb5bd" stroke-width="1.5"><rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="8.5" cy="8.5" r="1.5"/><polyline points="21 15 16 10 5 21"/></svg>
                <span>Adicione uma imagem na aba Imagem para ver o preview completo</span>
              </div>
            </div>
          </div>

          <div v-if="modalError" class="alert alert-danger py-2 mt-3">{{ modalError }}</div>
        </div>

        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Cancelar</button>
          <button v-if="abaAtiva !== 'preview'" class="btn btn-ag-outline" @click="proximaAba">Próximo →</button>
          <button class="btn btn-ag-primary" :disabled="saving" @click="salvar">
            {{ saving ? 'Salvando...' : 'Salvar e Publicar' }}
          </button>
        </div>
      </div>
    </div>

    <div v-if="showConfirm" class="ag-modal-overlay" @click.self="showConfirm = false">
      <div class="ag-modal" style="max-width:420px">
        <div class="ag-modal-header"><h5 class="mb-0">Confirmar exclusão</h5></div>
        <div class="ag-modal-body"><p>Excluir o banner <strong>{{ itemParaExcluir?.titulo }}</strong>?</p></div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="showConfirm = false">Cancelar</button>
          <button class="btn btn-danger" :disabled="saving" @click="excluir">{{ saving ? 'Excluindo...' : 'Excluir' }}</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { extractApiErrorMessage } from '~/types/agencia';
import type { HeroBannerSlide } from '~/types/agencia';

definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const agenciaStore = useAgenciaStore();
const loading = ref(true);
const saving = ref(false);
const importando = ref(false);
const importadoMsg = ref('');
const importErro = ref('');
const uploadingFile = ref(false);
const isDragging = ref(false);
const itens = ref<HeroBannerSlide[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<HeroBannerSlide | null>(null);
const modoImagem = ref<'url' | 'arquivo'>('url');
const arquivoFile = ref<File | null>(null);
const arquivoPreview = ref('');
const fileInputRef = ref<HTMLInputElement | null>(null);
const abaAtiva = ref<'imagem' | 'campanha' | 'preview'>('imagem');

const tabs = [
  { key: 'imagem' as const, label: 'Imagem', icon: '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="8.5" cy="8.5" r="1.5"/><polyline points="21 15 16 10 5 21"/></svg>' },
  { key: 'campanha' as const, label: 'Campanha', icon: '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>' },
  { key: 'preview' as const, label: 'Preview', icon: '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>' },
];

const form = reactive<{
  id?: number;
  titulo: string;
  url: string;
  urlDestino: string;
  ativo: boolean;
  headline: string;
  subtitulo: string;
  badge: string;
  ctaTexto: string;
  ctaLink: string;
  ctaCor: string;
  textoCor: 'light' | 'dark';
  overlayIntensidade: number;
}>({
  titulo: '', url: '', urlDestino: '', ativo: true,
  headline: '', subtitulo: '', badge: '',
  ctaTexto: 'Criar Conta Grátis', ctaLink: '/register',
  ctaCor: '#98C73A', textoCor: 'light', overlayIntensidade: 70,
});

const previewHeadline = computed(() => {
  const raw = form.headline || 'Headline da campanha (CMS global)';
  return raw.replace('<highlight>', '<span style="color:#98C73A">').replace('</highlight>', '</span>');
});

const isCtaColorDark = computed(() => {
  const hex = form.ctaCor.replace('#', '');
  if (hex.length < 6) return true;
  const r = parseInt(hex.substring(0, 2), 16);
  const g = parseInt(hex.substring(2, 4), 16);
  const b = parseInt(hex.substring(4, 6), 16);
  return (0.299 * r + 0.587 * g + 0.114 * b) / 255 < 0.5;
});

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function proximaAba() {
  if (abaAtiva.value === 'imagem') abaAtiva.value = 'campanha';
  else if (abaAtiva.value === 'campanha') abaAtiva.value = 'preview';
}

function triggerFileInput() { fileInputRef.value?.click(); }

function onFileChange(e: Event) {
  const file = (e.target as HTMLInputElement).files?.[0];
  if (file) setArquivo(file);
}

function onDrop(e: DragEvent) {
  isDragging.value = false;
  const file = e.dataTransfer?.files?.[0];
  if (file && file.type.startsWith('image/')) setArquivo(file);
}

function setArquivo(file: File) {
  arquivoFile.value = file;
  arquivoPreview.value = URL.createObjectURL(file);
  form.url = '';
}

function limparArquivo() {
  arquivoFile.value = null;
  arquivoPreview.value = '';
  form.url = '';
  if (fileInputRef.value) fileInputRef.value.value = '';
}

async function fazerUpload() {
  if (!arquivoFile.value) return;
  uploadingFile.value = true;
  modalError.value = '';
  try {
    const fd = new FormData();
    fd.append('files', arquivoFile.value);
    const result = await $fetch<{ url: string }>('/api/admin/upload-banner', {
      method: 'POST',
      headers: { Authorization: `Bearer ${agenciaStore.getToken()}` },
      body: fd,
    });
    form.url = result.url;
  } catch (e: unknown) {
    modalError.value = extractApiErrorMessage(e, 'Erro ao enviar imagem.');
  } finally {
    uploadingFile.value = false;
  }
}

function abrirNovo() {
  Object.assign(form, {
    id: undefined, titulo: '', url: '', urlDestino: '', ativo: true,
    headline: '', subtitulo: '', badge: '',
    ctaTexto: 'Criar Conta Grátis', ctaLink: '/register',
    ctaCor: '#98C73A', textoCor: 'light', overlayIntensidade: 70,
  });
  modoImagem.value = 'url';
  limparArquivo();
  modalError.value = '';
  abaAtiva.value = 'imagem';
  showModal.value = true;
}

function abrirEditar(item: HeroBannerSlide) {
  Object.assign(form, {
    id: item.id, titulo: item.titulo, url: item.url, urlDestino: item.urlDestino, ativo: item.ativo,
    headline: item.headline, subtitulo: item.subtitulo, badge: item.badge,
    ctaTexto: item.ctaTexto, ctaLink: item.ctaLink, ctaCor: item.ctaCor,
    textoCor: item.textoCor, overlayIntensidade: item.overlayIntensidade,
  });
  modoImagem.value = 'url';
  limparArquivo();
  modalError.value = '';
  abaAtiva.value = 'imagem';
  showModal.value = true;
}

function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: HeroBannerSlide) { itemParaExcluir.value = item; showConfirm.value = true; }

async function salvar() {
  if (!form.titulo.trim()) { modalError.value = 'Título é obrigatório.'; abaAtiva.value = 'imagem'; return; }
  if (!form.url.trim()) { modalError.value = 'Imagem é obrigatória.'; abaAtiva.value = 'imagem'; return; }
  saving.value = true;
  modalError.value = '';
  try {
    const isNew = !form.id;
    const slide: HeroBannerSlide = {
      id: form.id || Date.now(),
      titulo: form.titulo,
      url: form.url,
      urlDestino: form.urlDestino,
      ativo: form.ativo,
      headline: form.headline,
      subtitulo: form.subtitulo,
      badge: form.badge,
      ctaTexto: form.ctaTexto,
      ctaLink: form.ctaLink,
      ctaCor: form.ctaCor,
      textoCor: form.textoCor,
      overlayIntensidade: form.overlayIntensidade,
    };

    await $fetch('/api/admin/banner-campaigns', {
      method: 'POST',
      headers: { Authorization: `Bearer ${agenciaStore.getToken()}` },
      body: slide,
    });

    if (isNew) {
      itens.value.push(slide);
    } else {
      const idx = itens.value.findIndex(i => i.id === slide.id);
      if (idx !== -1) itens.value[idx] = slide;
    }

    fecharModal();
  } catch (e: unknown) {
    modalError.value = extractApiErrorMessage(e, 'Erro ao salvar banner.');
  } finally {
    saving.value = false;
  }
}

async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    const id = itemParaExcluir.value.id;
    await $fetch('/api/admin/banner-campaigns', {
      method: 'DELETE',
      headers: { Authorization: `Bearer ${agenciaStore.getToken()}` },
      body: { id },
    });
    itens.value = itens.value.filter(i => i.id !== id);
    showConfirm.value = false;
  } catch (e: unknown) {
    console.error('Erro ao excluir:', extractApiErrorMessage(e));
  } finally {
    saving.value = false;
  }
}

async function moverItem(item: HeroBannerSlide, direction: 'up' | 'down') {
  const idx = itens.value.findIndex(i => i.id === item.id);
  const newIdx = direction === 'up' ? idx - 1 : idx + 1;
  if (newIdx < 0 || newIdx >= itens.value.length) return;
  const arr = [...itens.value];
  [arr[idx], arr[newIdx]] = [arr[newIdx], arr[idx]];
  itens.value = arr;
  try {
    await $fetch('/api/admin/banner-campaigns', {
      method: 'PATCH',
      headers: { Authorization: `Bearer ${agenciaStore.getToken()}` },
      body: { orderedIds: arr.map(i => i.id) },
    });
  } catch (e: unknown) {
    console.error('Erro ao reordenar:', extractApiErrorMessage(e));
  }
}

async function importarDoSistemaAntigo() {
  importando.value = true;
  importErro.value = '';
  try {
    const result = await $fetch<{ imported: number; slides: HeroBannerSlide[]; message?: string }>('/api/admin/import-carousels', {
      method: 'POST',
      ...authHeader(),
    });
    if (result.imported > 0) {
      itens.value = result.slides;
      importadoMsg.value = `${result.imported} banner(s) importado(s) do sistema antigo com sucesso. Edite cada um para personalizar os textos.`;
    } else {
      importErro.value = result.message || 'Nenhum banner encontrado no sistema antigo.';
    }
  } catch (e: unknown) {
    importErro.value = extractApiErrorMessage(e, 'Erro ao importar banners do sistema antigo.');
  } finally {
    importando.value = false;
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const slides = await $fetch<HeroBannerSlide[]>('/api/admin/banner-campaigns', authHeader());
    itens.value = Array.isArray(slides) ? slides : [];
  } catch (e: unknown) {
    console.error('Erro ao carregar banners:', extractApiErrorMessage(e));
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.car-modal-wide { max-width: 680px; width: 100%; }
.car-modal-body { min-height: 320px; }

.car-order-btn {
  padding: 0 6px;
  font-size: 12px;
  line-height: 1.4;
  min-width: 28px;
}

.car-order-btn:disabled { opacity: 0.3; cursor: not-allowed; }

.car-tabs {
  display: flex;
  border-bottom: 2px solid #f0f4f8;
  background: #f8fafc;
  padding: 0 20px;
}

.car-tab-btn {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 10px 16px;
  border: none;
  background: transparent;
  font-size: 13px;
  font-weight: 500;
  color: #6c757d;
  cursor: pointer;
  border-bottom: 2px solid transparent;
  margin-bottom: -2px;
  transition: color 0.2s, border-color 0.2s;
}

.car-tab-btn:hover { color: #2F7785; }
.car-tab-btn.active { color: #2F7785; border-bottom-color: #2F7785; }
.car-tab-icon { display: flex; align-items: center; }

.car-dim-info {
  display: flex;
  align-items: flex-start;
  gap: 8px;
  background: #f0f7ff;
  border: 1px solid #c8dff7;
  border-radius: 8px;
  padding: 10px 14px;
  font-size: 13px;
  color: #2c5282;
}

.car-text-claro {
  background: #1a2236; color: #fff;
  border: 2px solid transparent; font-size: 12px;
}
.car-text-escuro {
  background: #f8f9fa; color: #1a2236;
  border: 2px solid #dee2e6; font-size: 12px;
}
.car-text-claro.active { border-color: #2F7785; }
.car-text-escuro.active { border-color: #2F7785; }

.car-preview-wrap { position: relative; border-radius: 10px; overflow: hidden; background: #1a2236; }
.car-preview { position: relative; width: 100%; aspect-ratio: 16 / 5; background-size: cover; background-position: center; display: flex; align-items: center; }
.car-preview-overlay { position: absolute; inset: 0; background: linear-gradient(to right, rgba(15,35,45,1) 0%, rgba(15,35,45,0.7) 50%, rgba(15,35,45,0.3) 100%); }
.car-preview-content { position: relative; z-index: 2; padding: 24px 28px; max-width: 65%; }
.text-light-mode { color: #fff; }
.text-dark-mode { color: #1a2236; }

.car-preview-badge {
  display: inline-flex; align-items: center; gap: 6px;
  background: rgba(255,255,255,0.12); border: 1px solid rgba(255,255,255,0.18);
  border-radius: 999px; padding: 3px 10px; font-size: 10px; font-weight: 500;
  margin-bottom: 10px; backdrop-filter: blur(4px);
}
.text-dark-mode .car-preview-badge { background: rgba(0,0,0,0.08); border-color: rgba(0,0,0,0.15); }
.car-preview-badge-placeholder { opacity: 0.5; }
.car-preview-badge-dot { width: 5px; height: 5px; border-radius: 50%; background: #98C73A; flex-shrink: 0; }
.car-preview-title { font-size: 18px; font-weight: 800; line-height: 1.2; margin: 0 0 6px; }
.car-preview-subtitle { font-size: 11px; opacity: 0.85; margin: 0 0 12px; line-height: 1.5; }
.car-preview-cta { display: inline-flex; align-items: center; gap: 6px; padding: 7px 16px; border-radius: 999px; font-size: 11px; font-weight: 700; text-decoration: none; cursor: default; }

.car-preview-no-img {
  display: flex; align-items: center; justify-content: center; gap: 8px;
  padding: 8px 12px; background: rgba(0,0,0,0.35); font-size: 11px; color: #adb5bd;
  position: absolute; bottom: 0; left: 0; right: 0;
}

.upload-drop-area {
  border: 2px dashed #dee2e6; border-radius: 8px; padding: 24px; text-align: center;
  cursor: pointer; transition: border-color 0.2s, background 0.2s; background: #fafafa; color: #6c757d;
}
.upload-drop-area:hover, .upload-drop-area.drag-over { border-color: #2F7785; background: #f0f7f8; }
.upload-drop-placeholder svg { color: #adb5bd; }
</style>
