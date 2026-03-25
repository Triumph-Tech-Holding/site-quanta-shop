<template>
  <div class="admin-marcas">
    <div class="admin-marcas__header">
      <h2>Gerenciar Marcas da Home</h2>
      <button @click="showForm = true" class="btn btn-primary">+ Adicionar Marca</button>
    </div>

    <div v-if="loading" class="text-center p-4">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Carregando...</span>
      </div>
    </div>

    <div v-else-if="marcas.length" class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>Logo</th>
            <th>Nome</th>
            <th>URL</th>
            <th>Ordem</th>
            <th>Ativo</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="marca in marcas" :key="marca.id">
            <td>
              <img :src="marca.logo" :alt="marca.nome" style="max-height: 40px; max-width: 100px; object-fit: contain;" @error="hideBrokenImage" />
            </td>
            <td>{{ marca.nome }}</td>
            <td><small>{{ marca.url }}</small></td>
            <td><input type="number" v-model.number="marca.ordem" class="form-control form-control-sm" style="width: 80px;" @change="salvar" /></td>
            <td>
              <div class="form-check form-switch">
                <input class="form-check-input" type="checkbox" v-model="marca.ativo" @change="salvar" />
              </div>
            </td>
            <td>
              <button @click="editarMarca(marca)" class="btn btn-sm btn-outline-secondary me-2">Editar</button>
              <button @click="removerMarca(marca.id)" class="btn btn-sm btn-outline-danger">Remover</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-else class="alert alert-info">Nenhuma marca cadastrada</div>

    <!-- Modal de Edição -->
    <div v-if="showForm" class="modal d-block" style="background: rgba(0,0,0,0.5);">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ editando?.id ? 'Editar Marca' : 'Adicionar Marca' }}</h5>
            <button type="button" class="btn-close" @click="showForm = false"></button>
          </div>
          <div class="modal-body">
            <div class="mb-3">
              <label class="form-label">Nome</label>
              <input type="text" v-model="editando.nome" class="form-control" />
            </div>
            <div class="mb-3">
              <label class="form-label">URL da Logo (Clearbit API ou URL direto)</label>
              <input type="url" v-model="editando.logo" class="form-control" placeholder="https://logo.clearbit.com/domain.com" />
            </div>
            <div class="mb-3">
              <label class="form-label">URL de Destino</label>
              <input type="url" v-model="editando.url" class="form-control" />
            </div>
            <div class="mb-3">
              <label class="form-label">Ordem</label>
              <input type="number" v-model.number="editando.ordem" class="form-control" />
            </div>
            <div class="form-check">
              <input class="form-check-input" type="checkbox" v-model="editando.ativo" id="ativoCheck" />
              <label class="form-check-label" for="ativoCheck">Ativo</label>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" @click="showForm = false">Cancelar</button>
            <button type="button" class="btn btn-primary" @click="salvarMarca">Salvar</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';

definePageMeta({
  middleware: 'agencia-admin',
});

interface Marca {
  id?: number;
  nome: string;
  logo: string;
  url: string;
  ativo: boolean;
  ordem: number;
}

const marcas = ref<Marca[]>([]);
const loading = ref(true);
const showForm = ref(false);
const editando = ref<Marca>({ nome: '', logo: '', url: '', ativo: true, ordem: 1 });

onMounted(async () => {
  try {
    marcas.value = await $fetch<Marca[]>('/data/brands.json');
  } catch (error) {
    console.error('[Marcas] Erro ao carregar:', error);
  } finally {
    loading.value = false;
  }
});

function hideBrokenImage(event: Event) {
  const img = event.target as HTMLImageElement;
  img.style.display = 'none';
}

function editarMarca(marca: Marca) {
  editando.value = { ...marca };
  showForm.value = true;
}

async function salvarMarca() {
  if (!editando.value.nome || !editando.value.logo || !editando.value.url) {
    alert('Preencha todos os campos');
    return;
  }

  if (editando.value.id) {
    const idx = marcas.value.findIndex(m => m.id === editando.value.id);
    if (idx >= 0) marcas.value[idx] = { ...editando.value };
  } else {
    editando.value.id = Math.max(...marcas.value.map(m => m.id || 0), 0) + 1;
    marcas.value.push({ ...editando.value });
  }

  await salvar();
  showForm.value = false;
  editando.value = { nome: '', logo: '', url: '', ativo: true, ordem: 1 };
}

async function removerMarca(id?: number) {
  if (!id || !confirm('Tem certeza que deseja remover esta marca?')) return;
  marcas.value = marcas.value.filter(m => m.id !== id);
  await salvar();
}

async function salvar() {
  try {
    await $fetch('/api/admin/brands', {
      method: 'POST',
      body: marcas.value,
    });
  } catch (error) {
    console.error('[Marcas] Erro ao salvar:', error);
    alert('Erro ao salvar as marcas');
  }
}
</script>

<style scoped>
.admin-marcas {
  padding: 24px;
}

.admin-marcas__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.admin-marcas__header h2 {
  margin: 0;
  font-size: 24px;
  color: #111;
}

.modal.d-block {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1050;
}

.modal-dialog {
  position: relative;
  width: auto;
  max-width: 500px;
  margin: auto;
}
</style>
