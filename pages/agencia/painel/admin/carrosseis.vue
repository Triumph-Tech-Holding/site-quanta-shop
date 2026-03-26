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

        <div class="ag-modal-body car-modal-body">
          <div class="car-editor-layout">

            <div class="car-editor-left">
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

              <div class="car-editor-form">

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
                  @click="!arquivoPreview && !uploadingFile ? triggerFileInput() : undefined"
                >
                  <input ref="fileInputRef" type="file" accept="image/*" style="display:none" @change="onFileChange" />
                  <div v-if="uploadingFile" class="upload-drop-placeholder">
                    <div class="spinner-border spinner-border-sm text-secondary mb-2" />
                    <p class="mb-0 text-muted">Enviando imagem...</p>
                  </div>
                  <div v-else-if="!arquivoPreview" class="upload-drop-placeholder">
                    <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="17 8 12 3 7 8"/><line x1="12" y1="3" x2="12" y2="15"/></svg>
                    <p class="mb-1 mt-2">Arraste uma imagem ou <strong>clique para selecionar</strong></p>
                    <small class="text-muted">PNG, JPG, WEBP — máx. 5MB — ideal 1920×600px</small>
                  </div>
                  <div v-else class="position-relative">
                    <img :src="arquivoPreview" alt="preview" style="max-height:140px;max-width:100%;object-fit:contain;border-radius:4px;" />
                    <button type="button" class="btn btn-sm btn-outline-danger mt-2" @click.stop="limparArquivo">Remover</button>
                  </div>
                </div>
              </div>
            </div>

            <div v-if="form.url" class="mb-3">
              <label class="form-label fw-bold">Ajustar posição da imagem</label>
              <small class="text-muted d-block mb-2">Arraste para reposicionar o ponto focal do banner.</small>
              <div
                ref="posAdjusterRef"
                class="pos-adjuster"
                :class="{ 'pos-adjuster--dragging': isDraggingPos }"
                :style="{
                  backgroundImage: `url(${form.url})`,
                  backgroundPosition: form.objectPosition || '50% 50%',
                }"
                @mousedown.prevent="onPosMouseDown"
                @mousemove="onPosMouseMove"
                @mouseup="onPosMouseUp"
                @mouseleave="onPosMouseUp"
              >
                <div class="pos-adjuster-hint">
                  <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M5 9l-3 3 3 3M9 5l3-3 3 3M15 19l-3 3-3-3M19 9l3 3-3 3M2 12h20M12 2v20"/></svg>
                  Arraste para reposicionar
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

            <div class="row g-3 mb-3">
              <div class="col-md-8">
                <label class="form-label fw-bold">Badge (etiqueta)</label>
                <input v-model="form.badge" type="text" class="form-control" placeholder="Ex: +12.000 usuários economizando" />
              </div>
              <div class="col-md-4">
                <label class="form-label fw-bold">Cor do Badge</label>
                <div class="d-flex align-items-center gap-2">
                  <input v-model="form.badgeCor" type="color" class="form-control form-control-color" style="width:48px;height:38px;padding:2px;" />
                  <input v-model="form.badgeCor" type="text" class="form-control form-control-sm" placeholder="padrão" style="font-family:monospace;" />
                </div>
                <small class="text-muted">Deixe vazio para cor padrão.</small>
              </div>
            </div>

            <div class="mb-2">
              <label class="form-label fw-bold">Headline (título principal)</label>
              <textarea v-model="form.headline" class="form-control" rows="3" placeholder="Ex: Seu dinheiro volta&#10;a cada compra"></textarea>
              <small class="text-muted">Use &lt;highlight&gt;palavra&lt;/highlight&gt; para destacar em verde. Enter para quebrar linha.</small>
            </div>
            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label class="form-label fw-bold">Cor da Headline</label>
                <div class="d-flex align-items-center gap-2">
                  <input v-model="form.headlineCor" type="color" class="form-control form-control-color" style="width:48px;height:38px;padding:2px;" />
                  <input v-model="form.headlineCor" type="text" class="form-control form-control-sm" placeholder="padrão (branco/escuro)" style="font-family:monospace;" />
                </div>
                <small class="text-muted">Deixe vazio para usar a cor global (Claro/Escuro).</small>
              </div>
            </div>

            <div class="mb-2">
              <label class="form-label fw-bold">Subtítulo</label>
              <textarea v-model="form.subtitulo" class="form-control" rows="2" placeholder="Ex: Cashback real em centenas de lojas parceiras.&#10;Enter para quebrar linha." />
              <small class="text-muted">Enter para quebrar linha.</small>
            </div>
            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label class="form-label fw-bold">Cor do Subtítulo</label>
                <div class="d-flex align-items-center gap-2">
                  <input v-model="form.subtituloCor" type="color" class="form-control form-control-color" style="width:48px;height:38px;padding:2px;" />
                  <input v-model="form.subtituloCor" type="text" class="form-control form-control-sm" placeholder="padrão" style="font-family:monospace;" />
                </div>
                <small class="text-muted">Deixe vazio para usar a cor global.</small>
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold">Tamanho do Subtítulo</label>
                <div class="d-flex gap-2">
                  <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.subtituloFontSize || 'medio') === 'pequeno' }" @click="form.subtituloFontSize = 'pequeno'">Pequeno</button>
                  <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.subtituloFontSize || 'medio') === 'medio' }" @click="form.subtituloFontSize = 'medio'">Médio</button>
                  <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.subtituloFontSize || 'medio') === 'grande' }" @click="form.subtituloFontSize = 'grande'">Grande</button>
                </div>
              </div>
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
                <label class="form-label fw-bold">Tamanho do Botão</label>
                <div class="d-flex gap-2">
                  <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.ctaTamanho || 'medio') === 'pequeno' }" @click="form.ctaTamanho = 'pequeno'">
                    Pequeno
                  </button>
                  <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.ctaTamanho || 'medio') === 'medio' }" @click="form.ctaTamanho = 'medio'">
                    Médio
                  </button>
                  <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.ctaTamanho || 'medio') === 'grande' }" @click="form.ctaTamanho = 'grande'">
                    Grande
                  </button>
                </div>
              </div>
            </div>

            <div class="row g-3 mb-3">
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
              <label class="form-label fw-bold">Tamanho da Fonte do Título</label>
              <div class="d-flex gap-2">
                <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.tituloFontSize || 'medio') === 'pequeno' }" @click="form.tituloFontSize = 'pequeno'">
                  <span style="font-size:10px;font-weight:800">Aa</span> Pequeno
                </button>
                <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.tituloFontSize || 'medio') === 'medio' }" @click="form.tituloFontSize = 'medio'">
                  <span style="font-size:13px;font-weight:800">Aa</span> Médio
                </button>
                <button type="button" class="btn btn-sm car-font-btn" :class="{ active: (form.tituloFontSize || 'medio') === 'grande' }" @click="form.tituloFontSize = 'grande'">
                  <span style="font-size:16px;font-weight:800">Aa</span> Grande
                </button>
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

            <div class="mb-3">
              <label class="form-label fw-bold">Direção do Overlay</label>
              <div class="d-flex gap-2 flex-wrap">
                <button type="button" class="btn btn-sm car-dir-btn" :class="{ active: (form.overlayDirecao || 'esquerda') === 'esquerda' }" @click="form.overlayDirecao = 'esquerda'">
                  <span class="car-dir-icon car-dir-icon--esquerda"></span> Esquerda
                </button>
                <button type="button" class="btn btn-sm car-dir-btn" :class="{ active: form.overlayDirecao === 'direita' }" @click="form.overlayDirecao = 'direita'">
                  <span class="car-dir-icon car-dir-icon--direita"></span> Direita
                </button>
                <button type="button" class="btn btn-sm car-dir-btn" :class="{ active: form.overlayDirecao === 'centro' }" @click="form.overlayDirecao = 'centro'">
                  <span class="car-dir-icon car-dir-icon--centro"></span> Centro
                </button>
                <button type="button" class="btn btn-sm car-dir-btn" :class="{ active: form.overlayDirecao === 'uniforme' }" @click="form.overlayDirecao = 'uniforme'">
                  <span class="car-dir-icon car-dir-icon--uniforme"></span> Uniforme
                </button>
              </div>
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Alinhamento do Botão CTA</label>
              <div class="d-flex gap-2">
                <button type="button" class="btn btn-sm car-align-btn" :class="{ active: (form.ctaAlinhamento || 'esquerda') === 'esquerda' }" @click="form.ctaAlinhamento = 'esquerda'">
                  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><line x1="3" y1="6" x2="21" y2="6"/><line x1="3" y1="12" x2="15" y2="12"/><line x1="3" y1="18" x2="18" y2="18"/></svg>
                  Esquerda
                </button>
                <button type="button" class="btn btn-sm car-align-btn" :class="{ active: form.ctaAlinhamento === 'centro' }" @click="form.ctaAlinhamento = 'centro'">
                  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><line x1="3" y1="6" x2="21" y2="6"/><line x1="6" y1="12" x2="18" y2="12"/><line x1="4" y1="18" x2="20" y2="18"/></svg>
                  Centro
                </button>
                <button type="button" class="btn btn-sm car-align-btn" :class="{ active: form.ctaAlinhamento === 'direita' }" @click="form.ctaAlinhamento = 'direita'">
                  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><line x1="3" y1="6" x2="21" y2="6"/><line x1="9" y1="12" x2="21" y2="12"/><line x1="6" y1="18" x2="21" y2="18"/></svg>
                  Direita
                </button>
              </div>
            </div>
          </div>

              </div>
            </div>

            <div class="car-editor-right">
              <div class="car-preview-label">Preview ao vivo</div>
              <div class="car-preview-wrap">
                <div
                  class="car-preview"
                  :style="{
                    backgroundImage: form.url ? `url(${form.url})` : 'linear-gradient(135deg, #1a4a54, #225F6B)',
                    backgroundPosition: form.objectPosition || '50% 50%',
                  }"
                >
                  <div class="car-preview-overlay" :style="{ background: previewOverlayGradient, opacity: previewOverlayOpacity }"></div>
                  <div
                    class="car-preview-content"
                    :class="form.textoCor === 'dark' ? 'text-dark-mode' : 'text-light-mode'"
                  >
                    <div v-if="form.badge" class="car-preview-badge" :style="previewBadgeStyle">
                      <span class="car-preview-badge-dot" :style="form.badgeCor ? { background: form.badgeCor } : {}"></span>
                      {{ form.badge }}
                    </div>
                    <div v-else class="car-preview-badge car-preview-badge-placeholder">
                      <span class="car-preview-badge-dot"></span>
                      Badge (CMS global)
                    </div>

                    <h2 class="car-preview-title" :style="{ ...previewTitleStyle, ...previewHeadlineStyle }" v-html="previewHeadline"></h2>
                    <p class="car-preview-subtitle" :style="previewSubtitleStyle" v-html="previewSubtitle"></p>

                    <div :style="form.ctaAlinhamento === 'direita' ? { display: 'flex', justifyContent: 'flex-end' } : form.ctaAlinhamento === 'centro' ? { display: 'flex', justifyContent: 'center' } : {}">
                      <a class="car-preview-cta" :style="previewCtaStyle">
                        {{ form.ctaTexto || 'Criar Conta Grátis' }}
                        <svg width="13" height="13" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
                      </a>
                    </div>
                  </div>
                </div>
                <div v-if="!form.url" class="car-preview-no-img">
                  <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#adb5bd" stroke-width="1.5"><rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="8.5" cy="8.5" r="1.5"/><polyline points="21 15 16 10 5 21"/></svg>
                  <span>Adicione uma imagem para ver o preview</span>
                </div>
              </div>
              <p class="car-preview-hint">Atualiza conforme você edita os campos</p>
            </div>

          </div>

          <div v-if="modalError" class="alert alert-danger py-2 mt-3">{{ modalError }}</div>
        </div>

        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Cancelar</button>
          <button v-if="abaAtiva === 'imagem'" class="btn btn-ag-outline" @click="abaAtiva = 'campanha'">Próximo →</button>
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
const isDraggingPos = ref(false);
const dragStartMouseX = ref(0);
const dragStartMouseY = ref(0);
const dragStartBgX = ref(50);
const dragStartBgY = ref(50);
const posAdjusterRef = ref<HTMLElement | null>(null);
const itens = ref<HeroBannerSlide[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<HeroBannerSlide | null>(null);
const modoImagem = ref<'url' | 'arquivo'>('url');
const arquivoFile = ref<File | null>(null);
const arquivoPreview = ref('');
const fileInputRef = ref<HTMLInputElement | null>(null);
const abaAtiva = ref<'imagem' | 'campanha'>('imagem');

const tabs = [
  { key: 'imagem' as const, label: 'Imagem', icon: '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="8.5" cy="8.5" r="1.5"/><polyline points="21 15 16 10 5 21"/></svg>' },
  { key: 'campanha' as const, label: 'Campanha', icon: '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>' },
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
  badgeCor: string;
  headlineCor: string;
  subtituloCor: string;
  subtituloFontSize: 'pequeno' | 'medio' | 'grande';
  ctaTexto: string;
  ctaLink: string;
  ctaCor: string;
  ctaTamanho: 'pequeno' | 'medio' | 'grande';
  textoCor: 'light' | 'dark';
  overlayIntensidade: number;
  objectPosition: string;
  tituloFontSize: 'pequeno' | 'medio' | 'grande';
  overlayDirecao: 'esquerda' | 'direita' | 'centro' | 'uniforme';
  ctaAlinhamento: 'esquerda' | 'centro' | 'direita';
}>({
  titulo: '', url: '', urlDestino: '', ativo: true,
  headline: '', subtitulo: '', badge: '', badgeCor: '',
  headlineCor: '', subtituloCor: '', subtituloFontSize: 'medio',
  ctaTexto: 'Criar Conta Grátis', ctaLink: '/register',
  ctaCor: '#98C73A', ctaTamanho: 'medio', textoCor: 'light', overlayIntensidade: 70,
  objectPosition: '50% 50%',
  tituloFontSize: 'medio', overlayDirecao: 'esquerda',
  ctaAlinhamento: 'esquerda',
});

const previewHeadline = computed(() => {
  const raw = form.headline || 'Headline da campanha (CMS global)';
  return raw
    .replace(/\n/g, '<br>')
    .replace('<highlight>', '<span style="color:#98C73A">')
    .replace('</highlight>', '</span>');
});

const isCtaColorDark = computed(() => {
  const hex = form.ctaCor.replace('#', '');
  if (hex.length < 6) return true;
  const r = parseInt(hex.substring(0, 2), 16);
  const g = parseInt(hex.substring(2, 4), 16);
  const b = parseInt(hex.substring(4, 6), 16);
  return (0.299 * r + 0.587 * g + 0.114 * b) / 255 < 0.5;
});

const previewOverlayOpacity = computed(() => {
  if (form.textoCor === 'dark') return Math.min(form.overlayIntensidade / 100, 0.35);
  return form.overlayIntensidade / 100;
});

const previewOverlayGradient = computed(() => {
  const d = form.overlayDirecao || 'esquerda';
  const dark = 'rgba(15,35,45,0.95)';
  const mid = 'rgba(15,35,45,0.55)';
  const trans = 'rgba(15,35,45,0)';
  if (d === 'esquerda') return `linear-gradient(to right, ${dark} 0%, ${mid} 50%, ${trans} 100%)`;
  if (d === 'direita') return `linear-gradient(to left, ${dark} 0%, ${mid} 50%, ${trans} 100%)`;
  if (d === 'centro') return `linear-gradient(to right, ${trans} 0%, ${dark} 50%, ${trans} 100%)`;
  return `rgba(15,35,45,0.92)`;
});

const previewTitleStyle = computed(() => {
  const size = form.tituloFontSize || 'medio';
  const sizes: Record<string, string> = { pequeno: '13px', medio: '18px', grande: '24px' };
  return { fontSize: sizes[size] || '18px', fontWeight: '800', lineHeight: '1.2' };
});

const previewSubtitle = computed(() => {
  const raw = form.subtitulo || 'Subtítulo (CMS global)';
  return raw.replace(/\n/g, '<br>');
});

const previewHeadlineStyle = computed(() => {
  if (!form.headlineCor) return {};
  return { color: form.headlineCor };
});

const previewSubtitleStyle = computed(() => {
  const sizes: Record<string, string> = { pequeno: '9px', medio: '11px', grande: '13px' };
  const style: Record<string, string> = {};
  if (form.subtituloCor) style.color = form.subtituloCor;
  if (form.subtituloFontSize && form.subtituloFontSize !== 'medio') {
    style.fontSize = sizes[form.subtituloFontSize] || '11px';
  }
  return style;
});

const previewBadgeStyle = computed(() => {
  if (!form.badgeCor) return {};
  return {
    color: form.badgeCor,
    borderColor: form.badgeCor + '55',
    background: form.badgeCor + '1A',
  };
});

const previewCtaStyle = computed(() => {
  const ctaColor = form.ctaCor || '#98C73A';
  const sizes: Record<string, { padding: string; fontSize: string }> = {
    pequeno: { padding: '4px 10px', fontSize: '9px' },
    medio: { padding: '7px 16px', fontSize: '11px' },
    grande: { padding: '11px 22px', fontSize: '13px' },
  };
  const s = sizes[form.ctaTamanho || 'medio'];
  return {
    background: ctaColor,
    color: isCtaColorDark.value ? '#fff' : '#1a2236',
    padding: s.padding,
    fontSize: s.fontSize,
  };
});

function authHeader() {
  const token = agenciaStore.getToken();
  if (!token) return {};
  return { headers: { Authorization: `Bearer ${token}` } };
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
  form.objectPosition = '50% 50%';
  fazerUpload();
}

function parseBgPos(): { x: number; y: number } {
  const parts = (form.objectPosition || '50% 50%').split(' ');
  return { x: parseFloat(parts[0]) || 50, y: parseFloat(parts[1]) || 50 };
}

function onPosMouseDown(e: MouseEvent) {
  isDraggingPos.value = true;
  dragStartMouseX.value = e.clientX;
  dragStartMouseY.value = e.clientY;
  const p = parseBgPos();
  dragStartBgX.value = p.x;
  dragStartBgY.value = p.y;
}

function onPosMouseMove(e: MouseEvent) {
  if (!isDraggingPos.value || !posAdjusterRef.value) return;
  const rect = posAdjusterRef.value.getBoundingClientRect();
  const dx = ((e.clientX - dragStartMouseX.value) / rect.width) * 100 * 1.5;
  const dy = ((e.clientY - dragStartMouseY.value) / rect.height) * 100 * 1.5;
  const newX = Math.max(0, Math.min(100, dragStartBgX.value - dx));
  const newY = Math.max(0, Math.min(100, dragStartBgY.value - dy));
  form.objectPosition = `${newX.toFixed(1)}% ${newY.toFixed(1)}%`;
}

function onPosMouseUp() {
  isDraggingPos.value = false;
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
      ...authHeader(),
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
    headline: '', subtitulo: '', badge: '', badgeCor: '',
    headlineCor: '', subtituloCor: '', subtituloFontSize: 'medio',
    ctaTexto: 'Criar Conta Grátis', ctaLink: '/register',
    ctaCor: '#98C73A', ctaTamanho: 'medio', textoCor: 'light', overlayIntensidade: 70,
    objectPosition: '50% 50%', tituloFontSize: 'medio', overlayDirecao: 'esquerda',
    ctaAlinhamento: 'esquerda',
  });
  modoImagem.value = 'url';
  limparArquivo();
  modalError.value = '';
  abaAtiva.value = 'imagem';
  showModal.value = true;
}

function abrirEditar(item: HeroBannerSlide) {
  limparArquivo();
  Object.assign(form, {
    id: item.id, titulo: item.titulo, url: item.url, urlDestino: item.urlDestino, ativo: item.ativo,
    headline: item.headline, subtitulo: item.subtitulo, badge: item.badge, badgeCor: item.badgeCor || '',
    headlineCor: item.headlineCor || '', subtituloCor: item.subtituloCor || '',
    subtituloFontSize: item.subtituloFontSize || 'medio',
    ctaTexto: item.ctaTexto, ctaLink: item.ctaLink, ctaCor: item.ctaCor || '#98C73A',
    ctaTamanho: item.ctaTamanho || 'medio',
    textoCor: item.textoCor ?? 'light', overlayIntensidade: item.overlayIntensidade ?? 70,
    objectPosition: item.objectPosition || '50% 50%',
    tituloFontSize: item.tituloFontSize || 'medio',
    overlayDirecao: item.overlayDirecao || 'esquerda',
    ctaAlinhamento: item.ctaAlinhamento || 'esquerda',
  });
  modoImagem.value = 'url';
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
      badgeCor: form.badgeCor,
      headlineCor: form.headlineCor,
      subtituloCor: form.subtituloCor,
      subtituloFontSize: form.subtituloFontSize,
      ctaTexto: form.ctaTexto,
      ctaLink: form.ctaLink,
      ctaCor: form.ctaCor,
      ctaTamanho: form.ctaTamanho,
      textoCor: form.textoCor,
      overlayIntensidade: form.overlayIntensidade,
      objectPosition: form.objectPosition || '50% 50%',
      tituloFontSize: form.tituloFontSize,
      overlayDirecao: form.overlayDirecao,
      ctaAlinhamento: form.ctaAlinhamento,
    };

    await $fetch('/api/admin/banner-campaigns', {
      method: 'POST',
      ...authHeader(),
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
      ...authHeader(),
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
      ...authHeader(),
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
.car-modal-wide { max-width: 1080px; width: 100%; }
.car-modal-body { min-height: 420px; }

.car-editor-layout { display: flex; gap: 24px; align-items: flex-start; }
.car-editor-left { flex: 1; min-width: 0; overflow-y: auto; max-height: calc(80vh - 120px); padding-right: 4px; }
.car-editor-right { width: 420px; flex-shrink: 0; position: sticky; top: 0; }
.car-editor-form { padding-top: 12px; }
.car-preview-label { font-size: 11px; font-weight: 700; color: #6c757d; text-transform: uppercase; letter-spacing: 0.06em; margin-bottom: 8px; }
.car-preview-hint { font-size: 11px; color: #adb5bd; text-align: center; margin-top: 6px; margin-bottom: 0; }

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
.car-preview { position: relative; width: 100%; aspect-ratio: 16 / 6; background-size: cover; display: flex; align-items: center; }
.car-preview-overlay { position: absolute; inset: 0; }

.car-font-btn {
  background: #f8f9fa; color: #495057;
  border: 2px solid #dee2e6; font-size: 12px;
  display: inline-flex; align-items: center; gap: 4px;
}
.car-font-btn.active { border-color: #2F7785; background: #e8f4f6; color: #2F7785; }
.car-font-btn:hover { border-color: #2F7785; }

.car-dir-btn {
  background: #f8f9fa; color: #495057;
  border: 2px solid #dee2e6; font-size: 12px;
  display: inline-flex; align-items: center; gap: 5px;
}
.car-dir-btn.active { border-color: #2F7785; background: #e8f4f6; color: #2F7785; }
.car-dir-btn:hover { border-color: #2F7785; }

.car-dir-icon {
  display: inline-block; width: 20px; height: 12px;
  border-radius: 2px; flex-shrink: 0;
}
.car-dir-icon--esquerda { background: linear-gradient(to right, #225F6B, transparent); }
.car-dir-icon--direita { background: linear-gradient(to left, #225F6B, transparent); }
.car-dir-icon--centro { background: linear-gradient(to right, transparent, #225F6B, transparent); }
.car-dir-icon--uniforme { background: #225F6B; }
.car-preview-content { position: relative; z-index: 2; padding: 24px 28px; width: 85%; }
.text-light-mode { color: #fff; }
.text-dark-mode { color: #1a2236; }
.text-dark-mode .car-preview-title { color: #1a2236 !important; }
.text-dark-mode .car-preview-subtitle { color: rgba(26,34,54,0.85) !important; }

.car-align-btn {
  background: #f8f9fa; color: #495057;
  border: 2px solid #dee2e6; font-size: 12px;
  display: inline-flex; align-items: center; gap: 5px;
}
.car-align-btn.active { border-color: #2F7785; background: #e8f4f6; color: #2F7785; }
.car-align-btn:hover { border-color: #2F7785; }

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

.pos-adjuster {
  width: 100%;
  aspect-ratio: 16 / 3;
  border-radius: 8px;
  overflow: hidden;
  position: relative;
  border: 2px solid #dee2e6;
  background-size: cover;
  cursor: grab;
  user-select: none;
  transition: border-color 0.2s;
}
.pos-adjuster:hover { border-color: #2F7785; }
.pos-adjuster--dragging { cursor: grabbing; }

.pos-adjuster-hint {
  position: absolute;
  bottom: 8px;
  left: 50%;
  transform: translateX(-50%);
  background: rgba(0, 0, 0, 0.55);
  color: #fff;
  padding: 4px 12px;
  border-radius: 999px;
  font-size: 11px;
  display: flex;
  align-items: center;
  gap: 6px;
  pointer-events: none;
  white-space: nowrap;
  backdrop-filter: blur(4px);
}
</style>
