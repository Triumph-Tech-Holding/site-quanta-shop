<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Catálogo" title="Produtos" description="Produtos disponíveis com cashback na plataforma.">
          <div class="prod-search-wrap">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0 0 16 9.5 6.5 6.5 0 1 0 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
            <input v-model="busca" type="text" class="prod-search-input" placeholder="Buscar produto..." />
          </div>
        </QsPageHeader>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div v-if="itensFiltrados.length === 0" class="ag-empty-state">
            <h5>Nenhum produto encontrado</h5>
          </div>
          <div v-else class="prod-grid">
            <div class="prod-card" v-for="item in itensFiltrados" :key="item.id">
              <div class="prod-card__img-wrap">
                <img v-if="item.imagem" :src="item.imagem" :alt="item.nome" class="prod-card__img" />
                <div v-else class="prod-card__img-placeholder">🛍️</div>
                <span v-if="item.cashback" class="prod-card__cashback-badge">{{ item.cashback }}% cashback</span>
              </div>
              <div class="prod-card__body">
                <div class="prod-card__nome">{{ item.nome }}</div>
                <p v-if="item.descricao" class="prod-card__desc">{{ item.descricao }}</p>
                <div class="prod-card__footer">
                  <div class="prod-card__preco">{{ formatCurrency(item.preco) }}</div>
                  <button class="prod-card__btn" @click="verProduto(item)">Ver detalhes</button>
                </div>
              </div>
            </div>
          </div>
        </template>

        <!-- Modal -->
        <Teleport to="body">
          <div v-if="showModal && selecionado" class="prod-modal-overlay" @click.self="fecharModal">
            <div class="prod-modal">
              <div class="prod-modal__header">
                <h3 class="prod-modal__title">{{ selecionado.nome }}</h3>
                <button class="prod-modal__close" @click="fecharModal">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>
                </button>
              </div>
              <div class="prod-modal__body">
                <img v-if="selecionado.imagem" :src="selecionado.imagem" :alt="selecionado.nome" class="prod-modal__img" />
                <p v-if="selecionado.descricao" class="prod-modal__desc">{{ selecionado.descricao }}</p>
                <div class="prod-modal__details">
                  <div class="prod-modal__detail">
                    <span class="prod-modal__key">Preço</span>
                    <span class="prod-modal__val prod-modal__val--price">{{ formatCurrency(selecionado.preco) }}</span>
                  </div>
                  <div v-if="selecionado.cashback" class="prod-modal__detail">
                    <span class="prod-modal__key">Cashback</span>
                    <span class="prod-modal__val prod-modal__val--cash">Até {{ selecionado.cashback }}%</span>
                  </div>
                  <div v-if="selecionado.loja" class="prod-modal__detail">
                    <span class="prod-modal__key">Loja</span>
                    <span class="prod-modal__val">{{ selecionado.loja }}</span>
                  </div>
                  <div v-if="selecionado.categoria" class="prod-modal__detail">
                    <span class="prod-modal__key">Categoria</span>
                    <span class="prod-modal__val">{{ selecionado.categoria }}</span>
                  </div>
                </div>
                <a v-if="selecionado.url" :href="selecionado.url" target="_blank" class="qs-btn-primary prod-modal__buy">
                  Comprar agora
                </a>
              </div>
            </div>
          </div>
        </Teleport>

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

interface Produto { id: number; nome: string; descricao?: string; preco: number; cashback?: number; imagem?: string; url?: string; loja?: string; categoria?: string; }

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

<style scoped>
.prod-search-wrap {
  display: flex;
  align-items: center;
  gap: .5rem;
  background: var(--qs-gray-50, #fafafa);
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  padding: .5rem .875rem;
  max-width: 260px;
  width: 100%;
}
.prod-search-wrap svg { width: 16px; height: 16px; color: var(--qs-gray-400, #9ca3af); flex-shrink: 0; }
.prod-search-input { border: none; background: transparent; font-size: .875rem; color: var(--qs-ink, #1d1d1f); width: 100%; outline: none; }

.prod-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 1rem;
}
.prod-card {
  background: #fff;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-lg, 20px);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  transition: all .2s;
  box-shadow: var(--qs-shadow-sm);
}
.prod-card:hover { box-shadow: var(--qs-shadow-md); transform: translateY(-3px); }
.prod-card__img-wrap { position: relative; height: 140px; flex-shrink: 0; }
.prod-card__img { width: 100%; height: 100%; object-fit: cover; }
.prod-card__img-placeholder { width: 100%; height: 100%; background: var(--qs-gray-100, #f5f5f7); display: flex; align-items: center; justify-content: center; font-size: 2.5rem; }
.prod-card__cashback-badge {
  position: absolute;
  top: .5rem;
  right: .5rem;
  background: var(--qs-lime, #98C73A);
  color: #fff;
  font-size: .625rem;
  font-weight: 800;
  text-transform: uppercase;
  padding: .2rem .5rem;
  border-radius: var(--qs-radius-pill, 999px);
  letter-spacing: .04em;
}
.prod-card__body { padding: 1rem; display: flex; flex-direction: column; gap: .375rem; flex: 1; }
.prod-card__nome { font-size: .9375rem; font-weight: 700; color: var(--qs-teal-dark, #225F6B); line-height: 1.3; }
.prod-card__desc { font-size: .8125rem; color: var(--qs-gray-500, #6b7280); line-height: 1.5; margin: 0; flex: 1; }
.prod-card__footer { display: flex; align-items: center; justify-content: space-between; gap: .5rem; margin-top: auto; padding-top: .5rem; }
.prod-card__preco { font-size: 1rem; font-weight: 800; color: var(--qs-teal, #2F7785); }
.prod-card__btn {
  padding: .35rem .75rem;
  background: var(--qs-gray-100, #f5f5f7);
  border: none;
  border-radius: var(--qs-radius-md, 12px);
  font-size: .75rem;
  font-weight: 700;
  color: var(--qs-teal-dark, #225F6B);
  cursor: pointer;
  transition: background .15s;
  white-space: nowrap;
}
.prod-card__btn:hover { background: var(--qs-gray-200, #e5e7eb); }

/* Modal */
.prod-modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,.45);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  padding: 1rem;
  backdrop-filter: blur(4px);
}
.prod-modal {
  background: #fff;
  border-radius: var(--qs-radius-lg, 20px);
  max-width: 520px;
  width: 100%;
  overflow: hidden;
  box-shadow: 0 20px 60px rgba(0,0,0,.2);
}
.prod-modal__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1.25rem 1.5rem;
  border-bottom: 1px solid var(--qs-gray-100, #f5f5f7);
}
.prod-modal__title { font-size: 1rem; font-weight: 700; color: var(--qs-teal-dark, #225F6B); margin: 0; }
.prod-modal__close { background: none; border: none; cursor: pointer; color: var(--qs-gray-400, #9ca3af); padding: 0; display: flex; }
.prod-modal__close svg { width: 20px; height: 20px; }
.prod-modal__body { padding: 1.5rem; display: flex; flex-direction: column; gap: 1rem; }
.prod-modal__img { width: 100%; height: 200px; object-fit: cover; border-radius: var(--qs-radius-md, 12px); }
.prod-modal__desc { font-size: .875rem; color: var(--qs-gray-600, #4b5563); line-height: 1.6; margin: 0; }
.prod-modal__details { display: grid; grid-template-columns: 1fr 1fr; gap: .75rem; }
.prod-modal__detail { display: flex; flex-direction: column; gap: .2rem; }
.prod-modal__key { font-size: .6875rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-400, #9ca3af); }
.prod-modal__val { font-size: .9375rem; font-weight: 600; color: var(--qs-gray-700, #374151); }
.prod-modal__val--price { color: var(--qs-teal, #2F7785); font-size: 1.25rem; font-weight: 800; }
.prod-modal__val--cash { color: var(--qs-lime-dark, #7aad1f); font-weight: 700; }
.prod-modal__buy { width: 100%; display: flex; align-items: center; justify-content: center; text-decoration: none; }
</style>
