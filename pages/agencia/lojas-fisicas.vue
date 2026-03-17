<template>
  <div class="agencia-login-page" style="align-items:flex-start; padding:2rem 1rem; background:#ecf2f7; min-height:100vh">
    <div class="login-box" style="max-width:720px; margin:2rem auto">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <h2 class="mt-4 mb-3" style="color:#2f7785">Lojas Físicas</h2>
      <div class="ag-card mb-3">
        <form @submit.prevent="buscar" class="row g-2 align-items-end">
          <div class="col-12 col-md-5 ag-form-group mb-0"><label>Cidade</label><input v-model="filtro.cidade" type="text" class="form-control" placeholder="Ex: São Paulo" /></div>
          <div class="col-12 col-md-4 ag-form-group mb-0"><label>Estado</label><select v-model="filtro.estado" class="form-select"><option value="">Todos</option><option v-for="e in estados" :key="e" :value="e">{{ e }}</option></select></div>
          <div class="col-12 col-md-3"><button type="submit" class="btn btn-ag-primary w-100">Buscar</button></div>
        </form>
      </div>
      <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
      <template v-else>
        <div v-if="lojas.length === 0" class="ag-empty-state"><h5>Nenhuma loja encontrada</h5></div>
        <div class="row g-3" v-else>
          <div class="col-12 col-md-6" v-for="(loja, i) in lojas" :key="i">
            <div class="ag-card h-100">
              <div class="fw-bold mb-1" style="color:#2f7785">{{ loja.nomeFantasia || loja.nome }}</div>
              <div class="text-muted mb-1" style="font-size:.85rem">{{ loja.endereco }}</div>
              <div class="text-muted" style="font-size:.85rem">{{ loja.cidade }} / {{ loja.estado }}</div>
              <div class="mt-2"><span class="badge bg-ag-secondary" style="background:#98c73a; color:#fff; padding:.3rem .8rem; border-radius:20px; font-size:.8rem">{{ loja.cashback }}% cashback</span></div>
            </div>
          </div>
        </div>
      </template>
      <div class="text-center mt-4">
        <NuxtLink to="/agencia" class="btn btn-ag-outline">← Voltar</NuxtLink>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia' });
const api = useApi();
const loading = ref(false);
interface LojaFisica { id: number; nomeFantasia?: string; nome?: string; endereco: string; cidade: string; estado: string; cashback: number; [key: string]: unknown; }
const lojas = ref<LojaFisica[]>([]);
const filtro = reactive({ cidade: '', estado: '' });
const estados = ['AC','AL','AM','AP','BA','CE','DF','ES','GO','MA','MG','MS','MT','PA','PB','PE','PI','PR','RJ','RN','RO','RR','RS','SC','SE','SP','TO'];
async function buscar() {
  loading.value = true;
  try {
    const params = new URLSearchParams({ ...(filtro.cidade && { cidade: filtro.cidade }), ...(filtro.estado && { estado: filtro.estado }) });
    const { data } = await api.get<LojaFisica[]>(`/geral/lojasFisicas?${params}`);
    lojas.value = Array.isArray(data) ? data : [];
  } catch (err: unknown) {
    console.error('Erro ao carregar lojas:', extractApiErrorMessage(err));
  } finally { loading.value = false; }
}
onMounted(() => buscar());
</script>
