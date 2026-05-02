<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Conteúdo</div>
        <h1>Redes Sociais</h1>
        <p>Posts do Instagram, YouTube, TikTok e outras plataformas exibidos no site</p>
      </div>
      <div class="qs-header-actions">
        <button class="qs-btn-primary" @click="abrirNovo">+ Novo Post</button>
      </div>
    </div>

    <div v-if="usandoLocal" class="qs-info-banner">
      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
      Dados salvos localmente no navegador. Serão sincronizados com o servidor quando o backend estiver disponível.
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M22.54 6.42a2.78 2.78 0 0 0-1.95-1.96C18.88 4 12 4 12 4s-6.88 0-8.59.46a2.78 2.78 0 0 0-1.95 1.96A29 29 0 0 0 1 12a29 29 0 0 0 .46 5.58A2.78 2.78 0 0 0 3.41 19.54C5.12 20 12 20 12 20s6.88 0 8.59-.46a2.78 2.78 0 0 0 1.95-1.96A29 29 0 0 0 23 12a29 29 0 0 0-.46-5.58z"/></svg>
        <h3>Nenhum post cadastrado</h3>
        <button class="qs-btn-primary" @click="abrirNovo">Adicionar primeiro post</button>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead><tr><th>Plataforma</th><th>Título</th><th>URL</th><th>Data</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td><span class="qs-rs-badge" :class="badgeClasse(item.plataforma)">{{ item.plataforma }}</span></td>
              <td class="qs-cell-bold">{{ item.titulo }}</td>
              <td><a :href="item.url" target="_blank" rel="noopener" class="qs-link qs-link-truncate">{{ item.url }}</a></td>
              <td>{{ formatDate(item.dataPublicacao) }}</td>
              <td><span class="qs-badge" :class="item.ativo ? 'qs-badge-success' : 'qs-badge-warn'">{{ item.ativo ? 'Ativo' : 'Inativo' }}</span></td>
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
      <div class="qs-modal" style="max-width:600px">
        <div class="qs-modal-header">
          <h5>{{ form.id ? 'Editar Post' : 'Novo Post' }}</h5>
          <button class="qs-modal-close" @click="fecharModal"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-form-grid-2">
            <div class="qs-field">
              <label class="qs-label">Plataforma *</label>
              <select v-model="form.plataforma" class="qs-select-input">
                <option value="Instagram">Instagram</option>
                <option value="YouTube">YouTube</option>
                <option value="TikTok">TikTok</option>
                <option value="Facebook">Facebook</option>
                <option value="Twitter">Twitter / X</option>
                <option value="Outro">Outro</option>
              </select>
            </div>
            <div class="qs-field">
              <label class="qs-label">Data de Publicação</label>
              <input v-model="form.dataPublicacao" type="date" class="qs-input" />
            </div>
            <div class="qs-field qs-field-full"><label class="qs-label">Título *</label><input v-model="form.titulo" type="text" class="qs-input" placeholder="Título ou legenda do post" /></div>
            <div class="qs-field qs-field-full"><label class="qs-label">URL do Post *</label><input v-model="form.url" type="url" class="qs-input" placeholder="https://www.instagram.com/p/..." /></div>
            <div class="qs-field qs-field-full"><label class="qs-label">URL da Thumbnail</label><input v-model="form.thumbnailUrl" type="url" class="qs-input" placeholder="https://... (URL da imagem de capa)" /></div>
            <div v-if="form.thumbnailUrl" class="qs-field-full">
              <img :src="form.thumbnailUrl" alt="Preview" class="qs-thumb-preview" />
            </div>
            <div class="qs-field qs-field-full"><label class="qs-label">Descrição</label><textarea v-model="form.descricao" class="qs-textarea" rows="3" placeholder="Descrição breve do post..." /></div>
            <div class="qs-field qs-field-full">
              <label class="qs-checkbox-label">
                <input v-model="form.ativo" type="checkbox" class="qs-checkbox" />
                <span>Exibir no site</span>
              </label>
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
        <div class="qs-modal-body"><p>Excluir o post <strong>{{ itemParaExcluir?.titulo }}</strong>?</p></div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="showConfirm = false">Cancelar</button>
          <button class="qs-btn-danger" :disabled="saving" @click="excluir">{{ saving ? 'Excluindo...' : 'Excluir' }}</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useBlogStore, type PostRedeSocial } from '~/stores/blog';

definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

const blogStore = useBlogStore();
const agenciaStore = useAgenciaStore();
const api = useApi();

const loading = ref(true);
const saving = ref(false);
const usandoLocal = ref(true); // Sempre forçar local por enquanto, conforme T130
const itens = computed(() => blogStore.redesSociais);

const showModal = ref(false);
const showConfirm = ref(false);
const modalError = ref('');
const itemParaExcluir = ref<PostRedeSocial | null>(null);

const platforms = ['Instagram', 'YouTube', 'TikTok', 'Facebook', 'Twitter', 'Outro'];

const emptyForm = () => ({ 
  id: undefined as string | number | undefined, 
  plataforma: 'Instagram' as any, 
  titulo: '', 
  url: '', 
  thumbnailUrl: '', 
  descricao: '', 
  dataPublicacao: new Date().toISOString().substring(0, 10), 
  ativo: true 
});

const form = reactive(emptyForm());

function formatDate(d?: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }

function badgeClasse(p: string) {
  const m: Record<string, string> = { Instagram: 'rs-instagram', YouTube: 'rs-youtube', TikTok: 'rs-tiktok', Facebook: 'rs-facebook', Twitter: 'rs-twitter', Outro: 'rs-outro' };
  return m[p] ?? 'rs-outro';
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
    ativo: item.ativo 
  }); 
  modalError.value = ''; 
  showModal.value = true; 
}

function fecharModal() { showModal.value = false; }

function confirmarExcluir(item: PostRedeSocial) { 
  itemParaExcluir.value = item; 
  showConfirm.value = true; 
}

async function salvar() {
  if (!form.titulo.trim() || !form.url.trim()) { modalError.value = 'Título e URL são obrigatórios.'; return; }
  saving.value = true; 
  modalError.value = '';
  
  const payload = { 
    plataforma: form.plataforma, 
    titulo: form.titulo, 
    url: form.url, 
    thumbnailUrl: form.thumbnailUrl || null, 
    descricao: form.descricao || null, 
    dataPublicacao: form.dataPublicacao || null, 
    ativo: form.ativo 
  };

  try {
    if (form.id) {
      blogStore.atualizarPostRedeSocial(form.id, payload as any);
    } else {
      blogStore.criarPostRedeSocial(payload as any);
    }
    saving.value = false; 
    fecharModal();
  } catch (e) {
    modalError.value = 'Erro ao salvar o post.';
    saving.value = false;
  }
}

async function excluir() {
  if (!itemParaExcluir.value) return;
  saving.value = true;
  try {
    blogStore.excluirPostRedeSocial(itemParaExcluir.value.id);
    showConfirm.value = false; 
    saving.value = false;
  } catch (e) {
    saving.value = false;
  }
}

onMounted(() => {
  agenciaStore.loadFromStorage();
  blogStore.carregarRedesSociais();
  loading.value = false;
});
</script>

<style scoped>
.qs-info-banner { display: flex; align-items: center; gap: 10px; background: #eff6ff; color: #1d4ed8; border: 1px solid #bfdbfe; border-radius: var(--qs-radius-md); padding: 12px 16px; font-size: 14px; margin-bottom: 20px; }
.qs-form-grid-2 { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.qs-field { display: flex; flex-direction: column; gap: 6px; }
.qs-field-full { grid-column: 1 / -1; }
.qs-label { font-size: 13px; font-weight: 600; color: var(--qs-gray-700); }
.qs-select-input { padding: 9px 12px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 14px; background: #fff; outline: none; cursor: pointer; }
.qs-textarea { width: 100%; padding: 10px 12px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 14px; resize: vertical; outline: none; }
.qs-checkbox-label { display: flex; align-items: center; gap: 8px; cursor: pointer; font-size: 14px; color: var(--qs-ink); }
.qs-checkbox { width: 16px; height: 16px; accent-color: var(--qs-teal); }
.qs-thumb-preview { height: 80px; border-radius: var(--qs-radius-md); border: 1px solid var(--qs-gray-200); object-fit: cover; }
.qs-link { color: var(--qs-teal); text-decoration: none; font-size: 13px; }
.qs-link:hover { text-decoration: underline; }
.qs-link-truncate { display: inline-block; max-width: 180px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; vertical-align: bottom; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; }
.qs-rs-badge { display: inline-block; padding: 3px 10px; border-radius: 999px; font-size: 11px; font-weight: 700; color: #fff; }
.rs-instagram { background: linear-gradient(135deg, #f09433, #e6683c, #dc2743, #cc2366, #bc1888); }
.rs-youtube { background: #ff0000; }
.rs-tiktok { background: #010101; }
.rs-facebook { background: #1877f2; }
.rs-twitter { background: #1da1f2; }
.rs-outro { background: var(--qs-gray-500); }
@media (max-width: 640px) { .qs-form-grid-2 { grid-template-columns: 1fr; } }
</style>
