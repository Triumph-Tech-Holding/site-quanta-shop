<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div>
        <h1>Redes Sociais</h1>
        <p>Configure os posts do Instagram, YouTube, TikTok e outras plataformas exibidos no site</p>
      </div>
      <button class="btn btn-ag-primary" @click="abrirNovo">+ Novo Post</button>
    </div>

    <div v-if="apiIndisponivel" class="alert alert-warning d-flex align-items-center gap-2 mb-3">
      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
      Endpoint de redes sociais ainda não disponível no servidor. Os posts aparecerão aqui assim que o backend for implementado.
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state">
        <h5>Nenhum post cadastrado</h5>
        <p class="text-muted">Clique em <strong>+ Novo Post</strong> para adicionar o primeiro post de rede social.</p>
      </div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead>
            <tr>
              <th>Plataforma</th>
              <th>Título</th>
              <th>URL</th>
              <th>Data</th>
              <th>Ativo</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td>
                <span class="badge-ag" :class="badgeClasse(item.plataforma)">{{ item.plataforma }}</span>
              </td>
              <td class="fw-bold">{{ item.titulo }}</td>
              <td>
                <a :href="item.url" target="_blank" rel="noopener" class="text-truncate d-inline-block" style="max-width:180px">{{ item.url }}</a>
              </td>
              <td>{{ formatDate(item.dataPublicacao) }}</td>
              <td>
                <span class="badge-ag" :class="item.ativo ? 'badge-ag-success' : 'badge-ag-warning'">
                  {{ item.ativo ? 'Ativo' : 'Inativo' }}
                </span>
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
      <div class="ag-modal" style="max-width:600px">
        <div class="ag-modal-header">
          <h5 class="mb-0">{{ form.id ? 'Editar Post' : 'Novo Post' }}</h5>
          <button class="btn-close" @click="fecharModal" />
        </div>
        <div class="ag-modal-body">
          <div class="row g-3">
            <div class="col-md-6">
              <label class="form-label fw-bold">Plataforma *</label>
              <select v-model="form.plataforma" class="form-select">
                <option value="Instagram">Instagram</option>
                <option value="YouTube">YouTube</option>
                <option value="TikTok">TikTok</option>
                <option value="Facebook">Facebook</option>
                <option value="Twitter">Twitter / X</option>
                <option value="Outro">Outro</option>
              </select>
            </div>
            <div class="col-md-6">
              <label class="form-label fw-bold">Data de Publicação</label>
              <input v-model="form.dataPublicacao" type="date" class="form-control" />
            </div>
            <div class="col-12">
              <label class="form-label fw-bold">Título *</label>
              <input v-model="form.titulo" type="text" class="form-control" placeholder="Título ou legenda do post" />
            </div>
            <div class="col-12">
              <label class="form-label fw-bold">URL do Post *</label>
              <input v-model="form.url" type="url" class="form-control" placeholder="https://www.instagram.com/p/..." />
            </div>
            <div class="col-12">
              <label class="form-label fw-bold">URL da Thumbnail</label>
              <input v-model="form.thumbnailUrl" type="url" class="form-control" placeholder="https://... (URL da imagem de capa)" />
            </div>
            <div class="col-12" v-if="form.thumbnailUrl">
              <img :src="form.thumbnailUrl" alt="Preview" class="img-thumbnail" style="max-height:120px;object-fit:cover" />
            </div>
            <div class="col-12">
              <label class="form-label fw-bold">Descrição</label>
              <textarea v-model="form.descricao" class="form-control" rows="3" placeholder="Descrição breve do post..." />
            </div>
            <div class="col-12">
              <div class="form-check form-switch">
                <input v-model="form.ativo" type="checkbox" class="form-check-input" id="rs-ativo" role="switch" />
                <label class="form-check-label" for="rs-ativo">Exibir no site</label>
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
          <p>Excluir o post <strong>{{ itemParaExcluir?.titulo }}</strong>?</p>
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
import { extractApiErrorMessage } from '~/types/agencia';
import type { PostRedeSocial, PlataformaSocial } from '~/types/agencia';

definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const agenciaStore = useAgenciaStore();
const api = useApi();

const loading = ref(true);
const saving = ref(false);
const apiIndisponivel = ref(false);
const itens = ref<PostRedeSocial[]>([]);
const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<PostRedeSocial | null>(null);

const emptyForm = () => ({
  id: undefined as number | undefined,
  plataforma: 'Instagram' as PlataformaSocial,
  titulo: '',
  url: '',
  thumbnailUrl: '',
  descricao: '',
  dataPublicacao: '',
  ativo: true,
});

const form = reactive(emptyForm());

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }

function formatDate(d?: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }

function badgeClasse(plataforma: PlataformaSocial) {
  const map: Record<PlataformaSocial, string> = {
    Instagram: 'badge-ag-instagram',
    YouTube: 'badge-ag-youtube',
    TikTok: 'badge-ag-tiktok',
    Facebook: 'badge-ag-facebook',
    Twitter: 'badge-ag-twitter',
    Outro: 'badge-ag-warning',
  };
  return map[plataforma] ?? 'badge-ag-warning';
}

function abrirNovo() {
  Object.assign(form, emptyForm());
  modalError.value = '';
  showModal.value = true;
}

function abrirEditar(item: PostRedeSocial) {
  Object.assign(form, {
    id: item.id,
    plataforma: item.plataforma,
    titulo: item.titulo,
    url: item.url,
    thumbnailUrl: item.thumbnailUrl || '',
    descricao: item.descricao || '',
    dataPublicacao: item.dataPublicacao ? item.dataPublicacao.substring(0, 10) : '',
    ativo: item.ativo,
  });
  modalError.value = '';
  showModal.value = true;
}

function fecharModal() { showModal.value = false; }
function confirmarExcluir(item: PostRedeSocial) { itemParaExcluir.value = item; showConfirm.value = true; }

async function salvar() {
  if (!form.titulo.trim() || !form.url.trim()) {
    modalError.value = 'Plataforma, título e URL são obrigatórios.';
    return;
  }
  saving.value = true;
  modalError.value = '';
  const payload = {
    plataforma: form.plataforma,
    titulo: form.titulo,
    url: form.url,
    thumbnailUrl: form.thumbnailUrl || null,
    descricao: form.descricao || null,
    dataPublicacao: form.dataPublicacao || null,
    ativo: form.ativo,
  };
  try {
    if (form.id) {
      await api.put(`/admin/redes-sociais/${form.id}`, payload, authHeader());
      const idx = itens.value.findIndex(i => i.id === form.id);
      if (idx !== -1) itens.value[idx] = { ...itens.value[idx], ...payload } as PostRedeSocial;
    } else {
      const { data } = await api.post('/admin/redes-sociais', payload, authHeader());
      itens.value.unshift(data as PostRedeSocial);
    }
    fecharModal();
  } catch (e: unknown) {
    modalError.value = extractApiErrorMessage(e, 'Erro ao salvar post.');
  } finally {
    saving.value = false;
  }
}

async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    await api.delete(`/admin/redes-sociais/${itemParaExcluir.value.id}`, authHeader());
    itens.value = itens.value.filter(i => i.id !== itemParaExcluir.value?.id);
    showConfirm.value = false;
  } catch (e: unknown) {
    console.error('Erro ao excluir post:', extractApiErrorMessage(e));
  } finally {
    saving.value = false;
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/admin/redes-sociais', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) {
    const err = e as { response?: { status?: number } };
    if (err?.response?.status === 404 || err?.response?.status === undefined) {
      apiIndisponivel.value = true;
    }
    console.error('Erro ao carregar posts:', extractApiErrorMessage(e));
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.badge-ag-instagram { background: linear-gradient(135deg, #f09433, #e6683c, #dc2743, #cc2366, #bc1888); color: #fff; }
.badge-ag-youtube   { background: #ff0000; color: #fff; }
.badge-ag-tiktok    { background: #010101; color: #fff; }
.badge-ag-facebook  { background: #1877f2; color: #fff; }
.badge-ag-twitter   { background: #1da1f2; color: #fff; }
</style>
