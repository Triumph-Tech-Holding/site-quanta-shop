<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Admin · CMS" title="Marcas da Home" description="Gerencie os logos exibidos no carrossel da página inicial">
      <button class="qs-btn-primary" @click="abrirNovo">+ Adicionar Marca</button>
    </QsPageHeader>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else-if="marcas.length === 0" class="qs-card-section">
      <div class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="8.5" cy="8.5" r="1.5"/><polyline points="21 15 16 10 5 21"/></svg>
        <h3>Nenhuma marca cadastrada</h3>
        <button class="qs-btn-primary" @click="abrirNovo">Adicionar primeira marca</button>
      </div>
    </div>

    <div v-else class="qs-card-section">
      <div class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Logo</th>
              <th>Nome</th>
              <th>URL</th>
              <th>Ordem</th>
              <th>Ativo</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="marca in marcas" :key="marca.id">
              <td>
                <img :src="marca.logo" :alt="marca.nome" class="qs-brand-logo" @error="hideBrokenImage" />
              </td>
              <td class="qs-cell-bold">{{ marca.nome }}</td>
              <td><a :href="marca.url" target="_blank" class="qs-link qs-link-truncate">{{ marca.url }}</a></td>
              <td>
                <input type="number" v-model.number="marca.ordem" class="qs-order-input" @change="salvar" />
              </td>
              <td>
                <label class="qs-toggle">
                  <input type="checkbox" v-model="marca.ativo" @change="salvar" />
                  <span class="qs-toggle-track"><span class="qs-toggle-thumb"></span></span>
                </label>
              </td>
              <td class="qs-cell-actions">
                <button class="qs-btn-sm-outline" @click="editarMarca(marca)">Editar</button>
                <button class="qs-btn-sm-danger" @click="removerMarca(marca.id)">Remover</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showForm" class="qs-modal-overlay" @click.self="showForm = false">
      <div class="qs-modal" style="max-width:500px">
        <div class="qs-modal-header">
          <h5>{{ editando?.id ? 'Editar Marca' : 'Adicionar Marca' }}</h5>
          <button class="qs-modal-close" @click="showForm = false"><svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg></button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-form-rows">
            <div class="qs-field"><label class="qs-label">Nome</label><input type="text" v-model="editando.nome" class="qs-input" /></div>
            <div class="qs-field"><label class="qs-label">URL da Logo</label><input type="url" v-model="editando.logo" class="qs-input" placeholder="https://logo.clearbit.com/domain.com" /></div>
            <div v-if="editando.logo" class="qs-brand-preview"><img :src="editando.logo" alt="Preview" class="qs-brand-logo-preview" @error="hideBrokenImage" /></div>
            <div class="qs-field"><label class="qs-label">URL de Destino</label><input type="url" v-model="editando.url" class="qs-input" /></div>
            <div class="qs-field"><label class="qs-label">Ordem</label><input type="number" v-model.number="editando.ordem" class="qs-input" /></div>
            <label class="qs-checkbox-label">
              <input type="checkbox" v-model="editando.ativo" class="qs-checkbox" />
              <span>Ativo</span>
            </label>
          </div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="showForm = false">Cancelar</button>
          <button class="qs-btn-primary" @click="salvarMarca">Salvar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ middleware: 'agencia-admin' });
interface Marca { id?: number; nome: string; logo: string; url: string; ativo: boolean; ordem: number; }
const marcas = ref<Marca[]>([]);
const loading = ref(true);
const showForm = ref(false);
const editando = ref<Marca>({ nome: '', logo: '', url: '', ativo: true, ordem: 1 });
onMounted(async () => {
  try { marcas.value = await $fetch<Marca[]>('/data/brands.json'); } catch (e) { console.error('[Marcas] Erro ao carregar:', e); } finally { loading.value = false; }
});
function hideBrokenImage(e: Event) { (e.target as HTMLImageElement).style.display = 'none'; }
function abrirNovo() { editando.value = { nome: '', logo: '', url: '', ativo: true, ordem: (Math.max(...marcas.value.map(m => m.ordem || 0), 0) + 1) }; showForm.value = true; }
function editarMarca(marca: Marca) { editando.value = { ...marca }; showForm.value = true; }
async function salvarMarca() {
  if (!editando.value.nome || !editando.value.logo || !editando.value.url) { alert('Preencha todos os campos obrigatórios'); return; }
  if (editando.value.id) { const idx = marcas.value.findIndex(m => m.id === editando.value.id); if (idx >= 0) marcas.value[idx] = { ...editando.value }; }
  else { editando.value.id = Math.max(...marcas.value.map(m => m.id || 0), 0) + 1; marcas.value.push({ ...editando.value }); }
  await salvar(); showForm.value = false;
}
async function removerMarca(id?: number) {
  if (!id || !confirm('Remover esta marca?')) return;
  marcas.value = marcas.value.filter(m => m.id !== id);
  await salvar();
}
async function salvar() {
  try { await $fetch('/api/admin/brands', { method: 'POST', body: marcas.value }); }
  catch (e) { console.error('[Marcas] Erro ao salvar:', e); }
}
</script>

<style scoped>
.qs-brand-logo { max-height: 36px; max-width: 90px; object-fit: contain; }
.qs-order-input { width: 64px; padding: 6px 8px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 13px; text-align: center; outline: none; }
.qs-order-input:focus { border-color: var(--qs-teal); }
.qs-toggle { position: relative; display: inline-flex; align-items: center; cursor: pointer; }
.qs-toggle input { opacity: 0; width: 0; height: 0; position: absolute; }
.qs-toggle-track { width: 36px; height: 20px; background: var(--qs-gray-200); border-radius: 99px; transition: background 0.2s; display: block; }
.qs-toggle input:checked ~ .qs-toggle-track { background: var(--qs-teal); }
.qs-toggle-thumb { position: absolute; left: 2px; top: 2px; width: 16px; height: 16px; background: #fff; border-radius: 50%; transition: transform 0.2s; display: block; }
.qs-toggle input:checked ~ .qs-toggle-track .qs-toggle-thumb { transform: translateX(16px); }
.qs-form-rows { display: flex; flex-direction: column; gap: 14px; }
.qs-field { display: flex; flex-direction: column; gap: 6px; }
.qs-label { font-size: 13px; font-weight: 600; color: var(--qs-gray-700); }
.qs-checkbox-label { display: flex; align-items: center; gap: 8px; cursor: pointer; font-size: 14px; color: var(--qs-ink); }
.qs-checkbox { width: 16px; height: 16px; accent-color: var(--qs-teal); }
.qs-brand-preview { display: flex; align-items: center; justify-content: center; background: var(--qs-gray-50); border: 1px solid var(--qs-gray-100); border-radius: var(--qs-radius-md); padding: 12px; }
.qs-brand-logo-preview { max-height: 48px; max-width: 160px; object-fit: contain; }
.qs-link { color: var(--qs-teal); text-decoration: none; font-size: 13px; }
.qs-link:hover { text-decoration: underline; }
.qs-link-truncate { display: inline-block; max-width: 160px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; vertical-align: bottom; }
</style>
