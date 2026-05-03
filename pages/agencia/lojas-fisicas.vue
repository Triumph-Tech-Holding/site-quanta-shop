<template>
  <div class="qs-page">
    <QsPageHeader eyebrow="Rede" title="Lojas Físicas" description="Encontre estabelecimentos credenciados Quanta Shop perto de você." />

    <div class="qs-card-section lf-filter-card">
      <form @submit.prevent="buscar" class="lf-filter-row">
        <div class="lf-field">
          <label class="lf-label">Cidade</label>
          <input v-model="filtro.cidade" type="text" class="qs-input" placeholder="Ex: São Paulo" />
        </div>
        <div class="lf-field">
          <label class="lf-label">Estado</label>
          <select v-model="filtro.estado" class="qs-input">
            <option value="">Todos</option>
            <option v-for="e in estados" :key="e" :value="e">{{ e }}</option>
          </select>
        </div>
        <button type="submit" class="qs-btn-primary lf-btn">Buscar</button>
      </form>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <template v-else>
      <QsEmptyState v-if="lojas.length === 0" icon="search" title="Nenhuma loja encontrada" description="Tente buscar por outra cidade ou estado." />
      <div v-else class="lf-grid">
        <div v-for="(loja, i) in lojas" :key="i" class="qs-card-section lf-card">
          <div class="lf-card-name">{{ loja.nomeFantasia || loja.nome }}</div>
          <div class="lf-card-addr">{{ loja.endereco }}</div>
          <div class="lf-card-city">{{ loja.cidade }} / {{ loja.estado }}</div>
          <div class="lf-card-badge">
            <span class="qs-badge qs-badge-lime">Até {{ loja.cashback }}% de cashback</span>
          </div>
        </div>
      </div>
    </template>

    <div class="lf-back">
      <NuxtLink to="/agencia" class="qs-btn-outline">← Voltar</NuxtLink>
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

<style scoped>
.lf-label { font-size: .75rem; font-weight: 600; color: var(--qs-gray-700, #374151); margin-bottom: .35rem; display: block; }
.lf-filter-card { padding: 1.25rem 1.5rem; }
.lf-filter-row { display: flex; gap: 1rem; align-items: flex-end; flex-wrap: wrap; }
.lf-field { flex: 1; min-width: 160px; }
.lf-btn { flex-shrink: 0; align-self: flex-end; }
.lf-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 1rem; }
.lf-card { display: flex; flex-direction: column; gap: .25rem; }
.lf-card-name { font-weight: 600; color: var(--qs-text); font-size: .95rem; }
.lf-card-addr, .lf-card-city { font-size: .825rem; color: var(--qs-gray-500); }
.lf-card-badge { margin-top: .5rem; }
.lf-back { display: flex; justify-content: center; padding-top: .5rem; }
</style>
