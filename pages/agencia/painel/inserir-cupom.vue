<template>
  <div>
    <div class="ag-page-header"><h1>Inserir Cupom Fiscal</h1><p>Registre sua nota fiscal para ganhar cashback</p></div>
    <div class="ag-card" style="max-width:600px">
      <form @submit.prevent="inserir" class="row g-3">
        <div class="col-12 ag-form-group">
          <label>Chave da NFe / Código do cupom *</label>
          <input v-model="form.chave" type="text" class="form-control" required maxlength="100" placeholder="Digite a chave da nota fiscal" />
        </div>
        <div class="col-12 ag-form-group">
          <label>Loja</label>
          <select v-model="form.idLoja" class="form-select">
            <option value="">Selecione (opcional)</option>
            <option v-for="l in lojas" :key="l.id" :value="l.id">{{ l.nome || l.razaoSocial }}</option>
          </select>
        </div>
        <div class="col-12">
          <button type="submit" class="btn btn-ag-primary" :disabled="enviando">
            <span v-if="enviando" class="spinner-border spinner-border-sm me-1" />
            {{ enviando ? 'Enviando...' : 'Registrar Cupom' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp();
const enviando = ref(false);
import type { Loja } from "~/types/agencia";
const lojas = ref<Loja[]>([]);
const form = reactive({ chave: '', idLoja: '' });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
async function inserir() {
  enviando.value = true;
  try {
    await api.post('/cupomFiscal/inserir', form, authHeader());
    $toast?.success('Cupom registrado! Aguarde a aprovação.');
    form.chave = ''; form.idLoja = '';
  } catch (e: any) {
    $toast?.error(e?.response?.data?.message || 'Erro ao registrar cupom.');
  } finally { enviando.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/loja/listar', authHeader());
    lojas.value = Array.isArray(data) ? data : [];
  } catch { /**/ }
});
</script>
