<template>
  <div>
    <div class="ag-page-header"><h1>Carrosséis</h1><p>Gerenciar banners e carrosséis</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum item encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th v-for="(col, i) in cols" :key="i">{{ col.label }}</th><th></th></tr></thead>
          <tbody>
            <tr v-for="(item, i) in itens" :key="i">
              <td v-for="(col, j) in cols" :key="j">{{ item[col.key] || '—' }}</td>
              <td><button class="btn btn-sm btn-ag-outline" @click="ver(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
import type { CarrosselAdmin } from "~/types/agencia";
const itens = ref<CarrosselAdmin[]>([]);
const cols = [{ label: 'Nome', key: 'nome' }, { label: 'Status', key: 'status' }];
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function ver(item: CarrosselAdmin) { console.log('ver', item); }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  if (!agenciaStore.isAdmin) { navigateTo('/agencia/painel'); return; }
  try {
    const { data } = await api.get('/admin/carrossel/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch { } finally { loading.value = false; }
});
</script>
