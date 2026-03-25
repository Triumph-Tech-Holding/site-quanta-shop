<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Carrosseis</h1><p>Gerenciar banners e carrosseis</p></div>
      <button class="btn btn-ag-primary" @click="abrirNovo">+ Novo Banner</button>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum banner encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Título</th><th>Imagem</th><th>URL Destino</th><th>Ativo</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.titulo }}</td>
              <td>
                <img v-if="item.url" :src="item.url" alt="" style="height:40px;width:80px;object-fit:cover;border-radius:4px;" />
                <span v-else class="text-muted">—</span>
              </td>
              <td><a v-if="item.urlDestino" :href="(item.urlDestino as string)" target="_blank" class="text-truncate d-block" style="max-width:180px">{{ item.urlDestino }}</a><span v-else>—</span></td>
              <td><span class="badge-ag" :class="item.ativo ? 'badge-ag-success' : 'badge-ag-warning'">{{ item.ativo ? 'Ativo' : 'Inativo' }}</span></td>
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
      <div class="ag-modal" style="max-width:560px">
        <div class="ag-modal-header"><h5 class="mb-0">{{ form.id ? 'Editar Banner' : 'Novo Banner' }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="mb-3">
            <label class="form-label fw-bold">Título *</label>
            <input v-model="form.titulo" type="text" class="form-control" />
          </div>

          <div class="mb-3">
            <label class="form-label fw-bold">Imagem do Banner</label>
            <div class="d-flex gap-2 mb-2">
              <button
                type="button"
                class="btn btn-sm"
                :class="modoImagem === 'url' ? 'btn-ag-primary' : 'btn-ag-outline'"
                @click="modoImagem = 'url'"
              >Usar URL</button>
              <button
                type="button"
                class="btn btn-sm"
                :class="modoImagem === 'arquivo' ? 'btn-ag-primary' : 'btn-ag-outline'"
                @click="modoImagem = 'arquivo'"
              >Enviar Arquivo</button>
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
                  <small class="text-muted">PNG, JPG, WEBP — máx. 5MB</small>
                </div>
                <div v-else class="position-relative">
                  <img :src="arquivoPreview" alt="preview" style="max-height:140px;max-width:100%;object-fit:contain;border-radius:4px;" />
                  <button type="button" class="btn btn-sm btn-outline-danger mt-2" @click.stop="limparArquivo">Remover</button>
                </div>
              </div>
              <button
                v-if="arquivoFile"
                type="button"
                class="btn btn-ag-primary btn-sm mt-2 w-100"
                :disabled="uploadingFile"
                @click="fazerUpload"
              >
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
              <label class="form-check-label" for="car-ativo">Ativo</label>
            </div>
          </div>

          <div v-if="modalError" class="alert alert-danger py-2">{{ modalError }}</div>
        </div>
        <div class="ag-modal-footer">
          <button class="btn btn-secondary" @click="fecharModal">Cancelar</button>
          <button class="btn btn-ag-primary" :disabled="saving" @click="salvar">{{ saving ? 'Salvando...' : 'Salvar' }}</button>
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
import { extractApiErrorMessage } from '~/types/agencia';
import type { CarrosselAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const saving = ref(false);
const uploadingFile = ref(false);
const isDragging = ref(false);
const itens = ref<CarrosselAdmin[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<CarrosselAdmin | null>(null);
const modoImagem = ref<'url' | 'arquivo'>('url');
const arquivoFile = ref<File | null>(null);
const arquivoPreview = ref('');
const fileInputRef = ref<HTMLInputElement | null>(null);
const form = reactive<{ id?: number; titulo: string; url: string; urlDestino: string; ativo: boolean }>({ titulo: '', url: '', urlDestino: '', ativo: true });

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }

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
  Object.assign(form, { id: undefined, titulo: '', url: '', urlDestino: '', ativo: true });
  modoImagem.value = 'url';
  limparArquivo();
  modalError.value = '';
  showModal.value = true;
}

function abrirEditar(item: CarrosselAdmin) {
  Object.assign(form, { id: item.id, titulo: item.titulo, url: item.url || '', urlDestino: (item.urlDestino as string) || '', ativo: item.ativo });
  modoImagem.value = 'url';
  limparArquivo();
  modalError.value = '';
  showModal.value = true;
}

function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: CarrosselAdmin) { itemParaExcluir.value = item; showConfirm.value = true; }

async function salvar() {
  if (!form.titulo.trim()) { modalError.value = 'Título é obrigatório.'; return; }
  if (!form.url.trim()) { modalError.value = 'Imagem é obrigatória. Informe uma URL ou envie um arquivo.'; return; }
  saving.value = true; modalError.value = '';
  try {
    const payload = { titulo: form.titulo, url: form.url, urlDestino: form.urlDestino, ativo: form.ativo };
    if (form.id) {
      await api.put(`/admin/carrosseis/${form.id}`, payload, authHeader());
      const idx = itens.value.findIndex(i => i.id === form.id);
      if (idx !== -1) itens.value[idx] = { ...itens.value[idx], ...payload };
    } else {
      const { data } = await api.post('/admin/carrosseis/criar', payload, authHeader());
      itens.value.unshift(data as CarrosselAdmin);
    }
    fecharModal();
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao salvar banner.'); }
  finally { saving.value = false; }
}

async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    await api.delete(`/admin/carrosseis/${itemParaExcluir.value.id}`, authHeader());
    itens.value = itens.value.filter(i => i.id !== itemParaExcluir.value?.id);
    showConfirm.value = false;
  } catch (e: unknown) { console.error('Erro ao excluir:', extractApiErrorMessage(e)); }
  finally { saving.value = false; }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/carrosseis/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar carrosseis:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>

<style scoped>
.upload-drop-area {
  border: 2px dashed #dee2e6;
  border-radius: 8px;
  padding: 24px;
  text-align: center;
  cursor: pointer;
  transition: border-color 0.2s, background 0.2s;
  background: #fafafa;
  color: #6c757d;
}
.upload-drop-area:hover,
.upload-drop-area.drag-over {
  border-color: #2F7785;
  background: #f0f7f8;
}
.upload-drop-placeholder svg {
  color: #adb5bd;
}
</style>
