<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Admin · Conteúdo" title="Blog" description="Gerenciar artigos publicados no blog">
      <button class="qs-btn-primary" @click="abrirNovo">+ Novo Artigo</button>
    </QsPageHeader>

    <div v-if="usandoLocal" class="qs-info-banner" :class="isDev ? 'qs-banner-warn' : 'qs-banner-info'">
      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
      <span v-if="isDev"><strong>Modo Dev:</strong> Escritas restritas ao localStorage para proteger o banco de produção.</span>
      <span v-else>Dados salvos localmente no navegador. Serão sincronizados quando o backend estiver disponível.</span>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/></svg>
        <h3>Nenhum artigo encontrado</h3>
        <button class="qs-btn-primary" @click="abrirNovo">Criar primeiro artigo</button>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Imagem</th>
              <th>Título</th>
              <th>Categoria</th>
              <th>Autor</th>
              <th>Data</th>
              <th>Status</th>
              <th>Destaque</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td>
                <img v-if="item.imagemDestaque" :src="item.imagemDestaque" class="qs-blog-thumb" alt="" />
                <div v-else class="qs-blog-thumb-ph" />
              </td>
              <td class="qs-cell-bold">{{ item.titulo }}</td>
              <td>{{ item.categoria || '—' }}</td>
              <td>{{ item.autor || '—' }}</td>
              <td>{{ formatDate(item.dataPublicacao) }}</td>
              <td><span class="qs-badge" :class="item.publicado ? 'qs-badge-success' : 'qs-badge-neutral'">{{ item.publicado ? 'Publicado' : 'Rascunho' }}</span></td>
              <td><span v-if="item.destaque" class="qs-badge qs-badge-teal">Sim</span><span v-else class="qs-text-muted">—</span></td>
              <td class="qs-cell-actions">
                <button class="qs-btn-sm-outline" @click="abrirEditar(item)">Editar</button>
                <button class="qs-btn-sm-danger" @click="confirmarExcluir(item)">Excluir</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:720px">
        <div class="qs-modal-header">
          <h5>{{ form.id ? 'Editar Artigo' : 'Novo Artigo' }}</h5>
          <button class="qs-modal-close" @click="fecharModal"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body qs-modal-scroll">
          <div class="qs-form-rows">
            <div class="qs-field"><label class="qs-label">Título *</label><input v-model="form.titulo" type="text" class="qs-input" @input="gerarSlug" placeholder="Título do artigo" /></div>
            <div class="qs-field"><label class="qs-label">Slug</label><input v-model="form.slug" type="text" class="qs-input" /><small class="qs-field-hint">Gerado automaticamente. Edite se necessário.</small></div>
            <div class="qs-form-grid-2">
              <div class="qs-field">
                <label class="qs-label">Categoria</label>
                <select v-model="form.categoria" class="qs-select-input">
                  <option v-for="cat in categorias" :key="cat" :value="cat">{{ cat }}</option>
                </select>
              </div>
              <div class="qs-field"><label class="qs-label">Autor</label><input v-model="form.autor" type="text" class="qs-input" placeholder="Nome do autor" /></div>
              <div class="qs-field"><label class="qs-label">Data de Publicação</label><input v-model="form.dataPublicacao" type="date" class="qs-input" /></div>
            </div>
            <div class="qs-field">
              <label class="qs-label">Imagem Destaque</label>
              <div class="qs-upload-zone" @click="triggerFileInput" @dragover.prevent @drop.prevent="handleDrop">
                <input ref="fileInputRef" type="file" accept="image/png,image/jpeg,image/jpg" style="display:none" @change="handleFileChange" />
                <div v-if="!form.imagemDestaque" class="qs-upload-placeholder">
                  <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#9ca3af" stroke-width="1.5"><rect x="3" y="3" width="18" height="18" rx="3"/><circle cx="8.5" cy="8.5" r="1.5"/><polyline points="21 15 16 10 5 21"/></svg>
                  <p>Clique para fazer upload ou cole uma URL abaixo<br/><small>PNG, JPG — máx. 5 MB</small></p>
                </div>
                <div v-else class="qs-upload-preview">
                  <img :src="form.imagemDestaque" alt="Preview" class="qs-upload-img" />
                  <button type="button" class="qs-upload-remove" @click.stop="form.imagemDestaque = ''">✕</button>
                </div>
              </div>
              <input v-model="urlManual" type="url" class="qs-input" style="margin-top:8px" placeholder="https://... (ou faça upload acima)" @blur="aplicarUrl" @keydown.enter.prevent="aplicarUrl" />
            </div>
            <div class="qs-field"><label class="qs-label">Resumo</label><textarea v-model="form.resumo" class="qs-textarea" rows="2" placeholder="Breve descrição exibida na listagem" /></div>
            <div class="qs-field"><label class="qs-label">Conteúdo *</label><textarea v-model="form.conteudo" class="qs-textarea" rows="10" placeholder="Conteúdo completo do artigo..." /></div>
            <div class="qs-form-grid-2">
              <label class="qs-checkbox-label"><input v-model="form.publicado" type="checkbox" class="qs-checkbox" /><span>Publicado</span></label>
              <label class="qs-checkbox-label"><input v-model="form.destaque" type="checkbox" class="qs-checkbox" /><span>Artigo em Destaque</span></label>
            </div>
          </div>
          <div v-if="modalError" class="qs-alert-danger" style="margin-top:12px">{{ modalError }}</div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Cancelar</button>
          <button class="qs-btn-primary" :disabled="saving" @click="salvar">{{ saving ? 'Salvando...' : 'Salvar' }}</button>
        </div>
      </div>
    </div>

    <div v-if="showConfirm" class="qs-modal-overlay" @click.self="showConfirm = false">
      <div class="qs-modal" style="max-width:420px">
        <div class="qs-modal-header"><h5>Confirmar exclusão</h5></div>
        <div class="qs-modal-body"><p>Excluir o artigo <strong>{{ itemParaExcluir?.titulo }}</strong>? Esta ação não pode ser desfeita.</p></div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="showConfirm = false">Cancelar</button>
          <button class="qs-btn-danger" :disabled="saving" @click="excluir">{{ saving ? 'Excluindo...' : 'Excluir' }}</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useBlogStore, type BlogArtigo } from '~/stores/blog';

definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const blogStore = useBlogStore();
const agenciaStore = useAgenciaStore();
const api = useApi();

const loading = ref(true);
const saving = ref(false);
const isDev = import.meta.dev;
const usandoLocal = ref(true); // Sempre forçar local por enquanto, conforme T130
const itens = computed(() => blogStore.artigos);

const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<BlogArtigo | null>(null);
const fileInputRef = ref<HTMLInputElement | null>(null);
const urlManual = ref('');

const categorias = ['Economia', 'Cashback', 'Parceiros', 'Dicas', 'Novidades', 'Quanta Shop'];

const emptyForm = () => ({ 
  id: undefined as string | number | undefined, 
  titulo: '', 
  slug: '', 
  resumo: '', 
  conteudo: '', 
  imagemDestaque: '', 
  categoria: 'Economia', 
  autor: '', 
  dataPublicacao: new Date().toISOString().substring(0, 10), 
  publicado: false, 
  destaque: false 
});

const form = reactive(emptyForm());

function formatDate(d?: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }

function gerarSlug() { 
  form.slug = blogStore.generateSlug(form.titulo);
}

function triggerFileInput() { fileInputRef.value?.click(); }
function handleFileChange(e: Event) { const file = (e.target as HTMLInputElement).files?.[0]; if (file) processFile(file); }
function handleDrop(e: DragEvent) { const file = e.dataTransfer?.files?.[0]; if (file && (file.type === 'image/png' || file.type === 'image/jpeg')) processFile(file); }

function processFile(file: File) {
  if (file.size > 5 * 1024 * 1024) { modalError.value = 'Imagem muito grande. Máximo 5 MB.'; return; }
  const reader = new FileReader();
  reader.onload = (e) => { form.imagemDestaque = e.target?.result as string; urlManual.value = ''; modalError.value = ''; };
  reader.readAsDataURL(file);
}

function aplicarUrl() { if (urlManual.value.trim()) { form.imagemDestaque = urlManual.value.trim(); urlManual.value = ''; } }

function abrirNovo() { 
  Object.assign(form, emptyForm()); 
  urlManual.value = ''; 
  modalError.value = ''; 
  showModal.value = true; 
}

function abrirEditar(item: BlogArtigo) { 
  Object.assign(form, { 
    id: item.id, 
    titulo: item.titulo, 
    slug: item.slug, 
    resumo: item.resumo || '', 
    conteudo: item.conteudo, 
    imagemDestaque: item.imagemDestaque || '', 
    categoria: item.categoria || 'Economia', 
    autor: item.autor || '', 
    dataPublicacao: item.dataPublicacao ? item.dataPublicacao.substring(0, 10) : '', 
    publicado: item.publicado, 
    destaque: item.destaque 
  }); 
  urlManual.value = ''; 
  modalError.value = ''; 
  showModal.value = true; 
}

function fecharModal() { showModal.value = false; }

function confirmarExcluir(item: BlogArtigo) { 
  itemParaExcluir.value = item; 
  showConfirm.value = true; 
}

async function salvar() {
  if (!form.titulo.trim() || !form.conteudo.trim()) { modalError.value = 'Título e conteúdo são obrigatórios.'; return; }
  saving.value = true; 
  modalError.value = '';
  
  const payload = { 
    titulo: form.titulo, 
    slug: form.slug, 
    resumo: form.resumo, 
    conteudo: form.conteudo, 
    imagemDestaque: form.imagemDestaque, 
    categoria: form.categoria, 
    autor: form.autor, 
    dataPublicacao: form.dataPublicacao || null, 
    publicado: form.publicado, 
    destaque: form.destaque 
  };

  try {
    if (form.id) {
      blogStore.atualizarArtigo(form.id, payload);
    } else {
      blogStore.criarArtigo(payload);
    }
    saving.value = false; 
    fecharModal();
  } catch (e) {
    modalError.value = 'Erro ao salvar o artigo.';
    saving.value = false;
  }
}

async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    blogStore.excluirArtigo(itemParaExcluir.value.id);
    showConfirm.value = false; 
    saving.value = false;
  } catch (e) {
    saving.value = false;
  }
}

onMounted(() => {
  agenciaStore.loadFromStorage();
  blogStore.carregarArtigos();
  loading.value = false;
});
</script>

<style scoped>
.qs-info-banner { display: flex; align-items: center; gap: 10px; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-bottom: 20px; border: 1px solid; }
.qs-banner-warn { background: #fffbeb; color: #92400e; border-color: #fde68a; }
.qs-banner-info { background: #eff6ff; color: #1d4ed8; border-color: #bfdbfe; }
.qs-text-muted { color: var(--qs-gray-400); font-size: 13px; }
.qs-modal-scroll { max-height: 75vh; overflow-y: auto; }
.qs-form-rows { display: flex; flex-direction: column; gap: 14px; }
.qs-form-grid-2 { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.qs-field { display: flex; flex-direction: column; gap: 6px; }
.qs-label { font-size: 13px; font-weight: 600; color: var(--qs-gray-700); }
.qs-field-hint { font-size: 11px; color: var(--qs-gray-400); margin-top: 2px; }
.qs-textarea { width: 100%; padding: 10px 12px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 14px; resize: vertical; outline: none; }
.qs-textarea:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }
.qs-checkbox-label { display: flex; align-items: center; gap: 8px; cursor: pointer; font-size: 14px; color: var(--qs-ink); }
.qs-checkbox { width: 16px; height: 16px; accent-color: var(--qs-teal); }
.qs-upload-zone { border: 2px dashed var(--qs-gray-200); border-radius: 10px; cursor: pointer; min-height: 110px; display: flex; align-items: center; justify-content: center; overflow: hidden; transition: border-color 0.2s; }
.qs-upload-zone:hover { border-color: var(--qs-teal); }
.qs-upload-placeholder { text-align: center; padding: 20px; color: var(--qs-gray-400); }
.qs-upload-placeholder p { margin: 8px 0 0; font-size: 13px; line-height: 1.5; }
.qs-upload-placeholder small { font-size: 11px; }
.qs-upload-preview { position: relative; width: 100%; }
.qs-upload-img { width: 100%; max-height: 180px; object-fit: cover; display: block; }
.qs-upload-remove { position: absolute; top: 8px; right: 8px; background: rgba(0,0,0,.6); border: none; color: #fff; border-radius: 50%; width: 26px; height: 26px; cursor: pointer; font-size: 13px; display: flex; align-items: center; justify-content: center; }
.qs-blog-thumb { width: 48px; height: 36px; object-fit: cover; border-radius: 6px; border: 1px solid var(--qs-gray-100); }
.qs-blog-thumb-ph { width: 48px; height: 36px; border-radius: 6px; background: linear-gradient(135deg, #2F7785, #98C73A); opacity: 0.3; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; }
@media (max-width: 640px) { .qs-form-grid-2 { grid-template-columns: 1fr; } }
</style>
