<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Produtos</h1><p>Produtos disponíveis na plataforma</p></div>
      <input v-model="busca" type="text" class="form-control" style="max-width:240px" placeholder="Buscar produto..." />
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else>
      <div v-if="itensFiltrados.length === 0" class="ag-empty-state"><h5>Nenhum produto encontrado</h5></div>
      <div v-else class="row g-3">
        <div class="col-12 col-sm-6 col-md-4 col-lg-3" v-for="item in itensFiltrados" :key="item.id">
          <div class="ag-card h-100 d-flex flex-column">
            <img v-if="item.imagem" :src="item.imagem" :alt="item.nome" class="rounded mb-2" style="width:100%; height:140px; object-fit:cover;" />
            <div v-else class="rounded mb-2 d-flex align-items-center justify-content-center" style="width:100%; height:140px; background:#ecf2f7; font-size:2.5rem;">🛍️</div>
            <div class="fw-bold mb-1">{{ item.nome }}</div>
            <div class="text-muted mb-2" style="font-size:.85rem; flex:1">{{ item.descricao || '' }}</div>
            <div class="d-flex align-items-center justify-content-between mt-auto">
              <div class="fw-bold text-ag-primary" style="font-size:1.1rem">{{ formatCurrency(item.preco) }}</div>
              <span v-if="item.cashback" class="badge-ag badge-ag-success">{{ item.cashback }}% cashback</span>
            </div>
            <button class="btn btn-ag-outline btn-sm mt-2" @click="verProduto(item)">Ver detalhes</button>
          </div>
        </div>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:520px">
        <div class="ag-modal-header"><h5 class="mb-0">{{ selecionado.nome }}</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <img v-if="selecionado.imagem" :src="selecionado.imagem" :alt="selecionado.nome" class="rounded mb-3 w-100" style="height:200px; object-fit:cover" />
          <p v-if="selecionado.descricao" class="text-muted mb-3">{{ selecionado.descricao }}</p>
          <div class="row g-2">
            <div class="col-6"><strong>Preço:</strong><br/><span class="text-ag-primary fw-bold fs-5">{{ formatCurrency(selecionado.preco) }}</span></div>
            <div v-if="selecionado.cashback" class="col-6"><strong>Cashback:</strong><br/><span class="badge-ag badge-ag-success">{{ selecionado.cashback }}% cashback</span></div>
            <div v-if="selecionado.loja" class="col-12"><strong>Loja:</strong> {{ selecionado.loja }}</div>
            <div v-if="selecionado.categoria" class="col-12"><strong>Categoria:</strong> {{ selecionado.categoria }}</div>
          </div>
          <a v-if="selecionado.url" :href="selecionado.url" target="_blank" class="btn btn-ag-primary mt-3 w-100">Comprar agora</a>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const busca = ref('');
const showModal = ref(false);

interface Produto {
  id: number;
  nome: string;
  descricao?: string;
  preco: number;
  cashback?: number;
  imagem?: string;
  url?: string;
  loja?: string;
  categoria?: string;
}

const itens = ref<Produto[]>([]);
const selecionado = ref<Produto | null>(null);
const itensFiltrados = computed(() => {
  if (!busca.value) return itens.value;
  const b = busca.value.toLowerCase();
  return itens.value.filter(i => i.nome?.toLowerCase().includes(b) || i.descricao?.toLowerCase().includes(b));
});
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function verProduto(item: Produto) { selecionado.value = item; showModal.value = true; }
function fecharModal() { showModal.value = false; selecionado.value = null; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/produtos/listar', authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao carregar produtos:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
});
</script>
