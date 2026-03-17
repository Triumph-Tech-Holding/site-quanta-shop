<template>
  <div>
    <div class="ag-page-header"><h1>Material de Apoio</h1><p>Materiais e tutoriais para te ajudar a vender mais</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="row g-3">
      <div v-for="(m, i) in materiais" :key="i" class="col-12 col-md-4">
        <div class="ag-card h-100">
          <div class="ag-card-title">{{ m.titulo }}</div>
          <p class="text-muted" style="font-size:.875rem">{{ m.descricao }}</p>
          <a v-if="m.url" :href="m.url" target="_blank" class="btn btn-ag-outline btn-sm">Acessar</a>
          <a v-if="m.arquivo" :href="m.arquivo" target="_blank" class="btn btn-ag-primary btn-sm ms-2">Baixar PDF</a>
        </div>
      </div>
      <div v-if="materiais.length === 0" class="col-12">
        <div class="ag-empty-state"><h5>Nenhum material disponível</h5></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const materiais = ref<any[]>([]);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/materialApoio/listar', authHeader());
    materiais.value = Array.isArray(data) ? data : [];
  } catch { /**/ } finally { loading.value = false; }
});
</script>
