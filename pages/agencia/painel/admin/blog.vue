<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div>
        <h1>Blog</h1>
        <p>Gerenciar artigos publicados no blog</p>
      </div>
      <button class="btn btn-ag-primary" @click="abrirNovo">+ Novo Artigo</button>
    </div>

    <div v-if="usandoLocal" class="alert d-flex align-items-center gap-2 mb-3"
      :class="isDev ? 'alert-warning' : 'alert-info'">
      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
      <span v-if="isDev">
        <strong>Modo Dev (Replit):</strong> Escritas restritas ao localStorage para proteger o banco de produção (LGPD).
        Em produção, os dados serão persistidos na API real.
      </span>
      <span v-else>
        Dados salvos localmente no navegador. Serão sincronizados com o servidor quando o backend estiver disponível.
      </span>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state">
        <h5>Nenhum artigo encontrado</h5>
        <p class="text-muted">Clique em <strong>+ Novo Artigo</strong> para criar o primeiro post do blog.</p>
      </div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
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
                <img v-if="item.imagemDestaque" :src="item.imagemDestaque" class="blog-thumb" alt="" />
                <div v-else class="blog-thumb-placeholder" />
              </td>
              <td class="fw-bold">{{ item.titulo }}</td>
              <td>{{ item.categoria || '—' }}</td>
              <td>{{ item.autor || '—' }}</td>
              <td>{{ formatDate(item.dataPublicacao) }}</td>
              <td>
                <span class="badge-ag" :class="item.publicado ? 'badge-ag-success' : 'badge-ag-warning'">
                  {{ item.publicado ? 'Publicado' : 'Rascunho' }}
                </span>
              </td>
              <td>
                <span v-if="item.destaque" class="badge-ag badge-ag-success">Sim</span>
                <span v-else class="text-muted small">Não</span>
              </td>
              <td class="d-flex gap-1">
                <button class="btn btn-sm btn-ag-outline" @click="abrirEditar(item)">Editar</button>
                <button class="btn btn-sm btn-outline-danger" @click="confirmarExcluir(item)">Excluir</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:720px">
        <div class="ag-modal-header">
          <h5 class="mb-0">{{ form.id ? 'Editar Artigo' : 'Novo Artigo' }}</h5>
          <button class="btn-close" @click="fecharModal" />
        </div>
        <div class="ag-modal-body" style="max-height:75vh;overflow-y:auto">
          <div class="row g-3">
            <div class="col-12">
              <label class="form-label fw-bold">Título *</label>
              <input v-model="form.titulo" type="text" class="form-control" @input="gerarSlug" placeholder="Título do artigo" />
            </div>
            <div class="col-12">
              <label class="form-label fw-bold">Slug</label>
              <input v-model="form.slug" type="text" class="form-control" placeholder="gerado-automaticamente" />
              <div class="form-text">Gerado automaticamente. Edite se necessário.</div>
            </div>
            <div class="col-md-6">
              <label class="form-label fw-bold">Categoria</label>
              <input v-model="form.categoria" type="text" class="form-control" placeholder="Ex: Cashback, Dicas, Novidades" />
            </div>
            <div class="col-md-6">
              <label class="form-label fw-bold">Autor</label>
              <input v-model="form.autor" type="text" class="form-control" placeholder="Nome do autor" />
            </div>
            <div class="col-md-6">
              <label class="form-label fw-bold">Data de Publicação</label>
              <input v-model="form.dataPublicacao" type="date" class="form-control" />
            </div>
            <div class="col-12">
              <label class="form-label fw-bold">Imagem Destaque</label>
              <div class="blog-img-field">
                <div class="blog-img-upload-zone" @click="triggerFileInput" @dragover.prevent @drop.prevent="handleDrop">
                  <input ref="fileInputRef" type="file" accept="image/png,image/jpeg,image/jpg" class="d-none" @change="handleFileChange" />
                  <div v-if="!form.imagemDestaque" class="blog-img-placeholder">
                    <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="#9ca3af" stroke-width="1.5"><rect x="3" y="3" width="18" height="18" rx="3"/><circle cx="8.5" cy="8.5" r="1.5"/><polyline points="21 15 16 10 5 21"/></svg>
                    <p>Clique para fazer upload<br/><span>PNG, JPG — máx. 5 MB</span></p>
                    <p class="blog-img-or">— ou —</p>
                    <p class="blog-img-url-hint">Cole uma URL abaixo</p>
                  </div>
                  <div v-else class="blog-img-preview-wrap">
                    <img :src="form.imagemDestaque" alt="Preview" class="blog-img-preview" />
                    <button type="button" class="blog-img-remove" @click.stop="form.imagemDestaque = ''">
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
                    </button>
                  </div>
                </div>
                <div class="mt-2">
                  <input v-model="urlManual" type="url" class="form-control form-control-sm" placeholder="https://... (ou faça upload acima)" @blur="aplicarUrl" @keydown.enter.prevent="aplicarUrl" />
                </div>
              </div>
            </div>
            <div class="col-12">
              <label class="form-label fw-bold">Resumo</label>
              <textarea v-model="form.resumo" class="form-control" rows="2" placeholder="Breve descrição exibida na listagem do blog" />
            </div>
            <div class="col-12">
              <label class="form-label fw-bold">Conteúdo *</label>
              <textarea v-model="form.conteudo" class="form-control" rows="10" placeholder="Conteúdo completo do artigo... (use linhas em branco para separar parágrafos)" />
              <div class="form-text">Separe os parágrafos com uma linha em branco.</div>
            </div>
            <div class="col-md-6">
              <div class="form-check form-switch">
                <input v-model="form.publicado" type="checkbox" class="form-check-input" id="blog-publicado" role="switch" />
                <label class="form-check-label" for="blog-publicado">Publicado</label>
              </div>
            </div>
            <div class="col-md-6">
              <div class="form-check form-switch">
                <input v-model="form.destaque" type="checkbox" class="form-check-input" id="blog-destaque" role="switch" />
                <label class="form-check-label" for="blog-destaque">Artigo em Destaque</label>
              </div>
            </div>
          </div>
          <div v-if="modalError" class="alert alert-danger mt-3 py-2">{{ modalError }}</div>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Cancelar</button>
          <button class="btn btn-ag-primary" :disabled="saving" @click="salvar">
            {{ saving ? 'Salvando...' : 'Salvar' }}
          </button>
        </div>
      </div>
    </div>

    <div v-if="showConfirm" class="ag-modal-overlay" @click.self="showConfirm = false">
      <div class="ag-modal" style="max-width:420px">
        <div class="ag-modal-header"><h5 class="mb-0">Confirmar exclusão</h5></div>
        <div class="ag-modal-body">
          <p>Excluir o artigo <strong>{{ itemParaExcluir?.titulo }}</strong>? Esta ação não pode ser desfeita.</p>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="showConfirm = false">Cancelar</button>
          <button class="btn btn-danger" :disabled="saving" @click="excluir">
            {{ saving ? 'Excluindo...' : 'Excluir' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { BlogArtigo } from '~/types/agencia';

definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const LS_KEY = 'qs_blog_artigos';

const agenciaStore = useAgenciaStore();
const api = useApi();

const loading = ref(true);
const saving = ref(false);
const isDev = import.meta.dev;
const usandoLocal = ref(isDev); // Em dev: sempre localStorage para escritas (protege produção)
const itens = ref<BlogArtigo[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<BlogArtigo | null>(null);
const fileInputRef = ref<HTMLInputElement | null>(null);
const urlManual = ref('');

const emptyForm = () => ({
  id: undefined as number | undefined,
  titulo: '',
  slug: '',
  resumo: '',
  conteudo: '',
  imagemDestaque: '',
  categoria: '',
  autor: '',
  dataPublicacao: '',
  publicado: false,
  destaque: false,
});

const form = reactive(emptyForm());

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d?: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }

function lsCarregar(): BlogArtigo[] {
  try { return JSON.parse(localStorage.getItem(LS_KEY) || '[]'); } catch { return []; }
}
function lsSalvar(lista: BlogArtigo[]) {
  localStorage.setItem(LS_KEY, JSON.stringify(lista));
}

function gerarSlug() {
  form.slug = form.titulo
    .toLowerCase()
    .normalize('NFD').replace(/[\u0300-\u036f]/g, '')
    .replace(/[^a-z0-9\s-]/g, '')
    .trim()
    .replace(/\s+/g, '-');
}

function triggerFileInput() {
  fileInputRef.value?.click();
}

function handleFileChange(e: Event) {
  const file = (e.target as HTMLInputElement).files?.[0];
  if (file) processFile(file);
}

function handleDrop(e: DragEvent) {
  const file = e.dataTransfer?.files?.[0];
  if (file && (file.type === 'image/png' || file.type === 'image/jpeg')) {
    processFile(file);
  }
}

function processFile(file: File) {
  if (file.size > 5 * 1024 * 1024) {
    modalError.value = 'Imagem muito grande. Máximo 5 MB.';
    return;
  }
  const reader = new FileReader();
  reader.onload = (e) => {
    form.imagemDestaque = e.target?.result as string;
    urlManual.value = '';
    modalError.value = '';
  };
  reader.readAsDataURL(file);
}

function aplicarUrl() {
  if (urlManual.value.trim()) {
    form.imagemDestaque = urlManual.value.trim();
    urlManual.value = '';
  }
}

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
    categoria: item.categoria || '',
    autor: item.autor || '',
    dataPublicacao: item.dataPublicacao ? item.dataPublicacao.substring(0, 10) : '',
    publicado: item.publicado,
    destaque: item.destaque,
  });
  urlManual.value = '';
  modalError.value = '';
  showModal.value = true;
}

function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: BlogArtigo) { itemParaExcluir.value = item; showConfirm.value = true; }

async function salvar() {
  if (!form.titulo.trim() || !form.conteudo.trim()) {
    modalError.value = 'Título e conteúdo são obrigatórios.';
    return;
  }
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
    destaque: form.destaque,
  };

  // Em dev (Replit): nunca escreve na API — protege banco de produção (LGPD)
  // Em produção: usa API real se não estiver em modo local
  if (!isDev && !usandoLocal.value) {
    try {
      if (form.id) {
        await api.put(`/admin/blog/artigos/${form.id}`, payload, authHeader());
        const idx = itens.value.findIndex(i => i.id === form.id);
        if (idx !== -1) itens.value[idx] = { ...itens.value[idx], ...payload } as BlogArtigo;
      } else {
        const { data } = await api.post('/admin/blog/artigos', payload, authHeader());
        itens.value.unshift(data as BlogArtigo);
      }
      saving.value = false;
      fecharModal();
      return;
    } catch {
      usandoLocal.value = true;
    }
  }

  const lista = lsCarregar();
  if (form.id) {
    const idx = lista.findIndex(i => i.id === form.id);
    if (idx !== -1) lista[idx] = { ...lista[idx], ...payload } as BlogArtigo;
    const vi = itens.value.findIndex(i => i.id === form.id);
    if (vi !== -1) itens.value[vi] = { ...itens.value[vi], ...payload } as BlogArtigo;
  } else {
    const novo: BlogArtigo = { id: Date.now(), ...payload } as BlogArtigo;
    lista.unshift(novo);
    itens.value.unshift(novo);
  }
  lsSalvar(lista);
  saving.value = false;
  fecharModal();
}

async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  const id = itemParaExcluir.value.id;

  // Em dev (Replit): nunca deleta via API — protege banco de produção (LGPD)
  if (!isDev && !usandoLocal.value) {
    try {
      await api.delete(`/admin/blog/artigos/${id}`, authHeader());
      itens.value = itens.value.filter(i => i.id !== id);
      showConfirm.value = false;
      saving.value = false;
      return;
    } catch {
      usandoLocal.value = true;
    }
  }

  const lista = lsCarregar().filter(i => i.id !== id);
  lsSalvar(lista);
  itens.value = itens.value.filter(i => i.id !== id);
  showConfirm.value = false;
  saving.value = false;
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  const localData = lsCarregar();

  // Em dev: sempre começa em modo local (escritas nunca vão à API)
  if (localData.length > 0) {
    itens.value = localData;
    usandoLocal.value = true; // forçado até confirmar via API
    loading.value = false;
  }

  try {
    const { data } = await api.get('/admin/blog/artigos', authHeader());
    const apiData: BlogArtigo[] = Array.isArray(data) ? data : (data?.items || []);
    if (apiData.length > 0) {
      itens.value = apiData;
      // Em dev: mostra dados da API para leitura, mas escritas continuam no localStorage
      // Em prod: desbloqueia escrita via API
      usandoLocal.value = isDev ? true : false;
    } else if (localData.length === 0) {
      usandoLocal.value = true;
    }
  } catch {
    if (localData.length === 0) itens.value = lsCarregar();
    usandoLocal.value = true;
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.blog-thumb {
  width: 48px;
  height: 36px;
  object-fit: cover;
  border-radius: 6px;
  border: 1px solid #f0f0f0;
}
.blog-thumb-placeholder {
  width: 48px;
  height: 36px;
  border-radius: 6px;
  background: linear-gradient(135deg, #2F7785, #98C73A);
  opacity: 0.3;
}

.blog-img-field {}

.blog-img-upload-zone {
  border: 2px dashed #d1d5db;
  border-radius: 12px;
  cursor: pointer;
  transition: border-color 0.2s, background 0.2s;
  overflow: hidden;
  min-height: 120px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.blog-img-upload-zone:hover { border-color: #2F7785; background: rgba(47,119,133,0.03); }

.blog-img-placeholder {
  text-align: center;
  padding: 20px;
  color: #9ca3af;
}
.blog-img-placeholder p { font-size: 13px; margin: 8px 0 4px; line-height: 1.5; }
.blog-img-placeholder span { font-size: 11px; }
.blog-img-or { color: #d1d5db; margin: 6px 0 !important; font-size: 11px !important; }
.blog-img-url-hint { color: #2F7785 !important; font-size: 12px !important; font-weight: 600; }

.blog-img-preview-wrap { position: relative; width: 100%; }
.blog-img-preview { width: 100%; max-height: 200px; object-fit: cover; display: block; }
.blog-img-remove {
  position: absolute;
  top: 8px;
  right: 8px;
  background: rgba(0,0,0,0.6);
  border: none;
  color: #fff;
  border-radius: 50%;
  width: 28px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background 0.2s;
}
.blog-img-remove:hover { background: rgba(220,38,38,0.85); }
</style>
