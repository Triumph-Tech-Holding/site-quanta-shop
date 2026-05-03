<template>
  <div class="qs-page qs-search">
    <div class="container">
      <!-- Header -->
      <div class="qs-page-header">
        <div class="qs-header-text">
          <div class="qs-eyebrow">Busca Inteligente · Cashback &amp; Proximidade</div>
          <h1>Encontre o melhor cashback perto de você</h1>
          <p>Filtre produtos e lojistas por percentual de cashback, distância da sua localização e categoria.</p>
        </div>
      </div>

      <!-- Search bar -->
      <div class="qs-card-section qs-search-card">
        <div class="qs-search-bar">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="var(--qs-gray-400)" class="qs-search-icon">
            <path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0 0 16 9.5 6.5 6.5 0 1 0 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/>
          </svg>
          <input
            type="text"
            v-model="query"
            @keyup.enter="runSearch"
            placeholder="O que você procura? Ex: tênis, mercado, eletrônicos…"
            class="qs-search-input"
          />
          <button class="qs-btn-primary qs-search-btn" @click="runSearch" :disabled="loading">
            {{ loading ? 'Buscando…' : 'Buscar' }}
          </button>
        </div>

        <div class="qs-quick-filters">
          <span class="qs-quick-label">Sugestões:</span>
          <QsFilterChip
            v-for="s in quickSuggestions"
            :key="s"
            :active="query === s"
            @click="query = s; runSearch()"
          >{{ s }}</QsFilterChip>
        </div>
      </div>

      <!-- Filtros -->
      <div class="qs-search-grid">
        <aside class="qs-filters">
          <div class="qs-filter-card">
            <h3 class="qs-filter-title">Cashback mínimo</h3>
            <div class="qs-cashback-display">
              <span class="qs-cashback-value">{{ minCashback }}%</span>
              <span class="qs-cashback-label">ou mais</span>
            </div>
            <input
              type="range"
              :min="0"
              :max="20"
              step="0.5"
              v-model.number="minCashback"
              class="qs-range qs-range--lime"
            />
            <div class="qs-range-marks">
              <span>0%</span><span>5%</span><span>10%</span><span>15%</span><span>20%</span>
            </div>
          </div>

          <div class="qs-filter-card">
            <h3 class="qs-filter-title">Distância máxima</h3>
            <div class="qs-cashback-display">
              <span class="qs-cashback-value">{{ maxDistance }}<small>km</small></span>
              <span class="qs-cashback-label">do meu endereço</span>
            </div>
            <input
              type="range"
              :min="1"
              :max="50"
              step="1"
              v-model.number="maxDistance"
              class="qs-range"
            />
            <div class="qs-range-marks">
              <span>1km</span><span>10km</span><span>25km</span><span>50km</span>
            </div>
            <button class="qs-geo-btn" @click="useMyLocation" :disabled="geoLoading">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5a2.5 2.5 0 0 1 0-5 2.5 2.5 0 0 1 0 5z"/></svg>
              {{ geoLoading ? 'Localizando…' : geoActive ? 'Localização ativa' : 'Usar minha localização' }}
            </button>
          </div>

          <div class="qs-filter-card">
            <h3 class="qs-filter-title">Categoria</h3>
            <div class="qs-cat-list">
              <QsFilterChip
                v-for="c in categorias"
                :key="c.id"
                :active="activeCategoria === c.id"
                @click="activeCategoria = activeCategoria === c.id ? '' : c.id"
              >
                <template #icon>{{ c.icon }}</template>
                {{ c.nome }}
              </QsFilterChip>
            </div>
          </div>

          <div class="qs-filter-card">
            <h3 class="qs-filter-title">Ordenar por</h3>
            <select v-model="sortBy" class="qs-select">
              <option value="cashback">Maior cashback</option>
              <option value="distance">Menor distância</option>
              <option value="popular">Mais populares</option>
              <option value="price-asc">Menor preço</option>
              <option value="price-desc">Maior preço</option>
            </select>
          </div>

          <button class="qs-btn-outline qs-clear-btn" @click="clearFilters">
            Limpar filtros
          </button>
        </aside>

        <main class="qs-results">
          <div class="qs-results-head">
            <div>
              <strong>{{ filteredResults.length }}</strong>
              <span> {{ filteredResults.length === 1 ? 'resultado' : 'resultados' }} encontrados</span>
            </div>
            <div class="qs-view-toggle">
              <button :class="{ active: viewMode === 'grid' }" @click="viewMode = 'grid'" aria-label="Grid">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor"><path d="M3 3h8v8H3V3zm10 0h8v8h-8V3zM3 13h8v8H3v-8zm10 0h8v8h-8v-8z"/></svg>
              </button>
              <button :class="{ active: viewMode === 'list' }" @click="viewMode = 'list'" aria-label="Lista">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor"><path d="M4 6h16v2H4V6zm0 5h16v2H4v-2zm0 5h16v2H4v-2z"/></svg>
              </button>
            </div>
          </div>

          <!-- Skeleton loaders (proibido spinner) -->
          <div v-if="loading" :class="['qs-results-list', `view-${viewMode}`]">
            <div v-for="n in 6" :key="n" class="qs-result-card qs-skeleton-card">
              <div class="qs-skeleton qs-sk-img" />
              <div class="qs-result-body" style="gap:10px">
                <div class="qs-skeleton qs-sk-line" style="width:40%;height:10px" />
                <div class="qs-skeleton qs-sk-line" style="width:75%;height:16px" />
                <div class="qs-skeleton qs-sk-line" style="width:90%;height:11px" />
                <div class="qs-skeleton qs-sk-line" style="width:60%;height:11px" />
              </div>
              <div class="qs-result-actions">
                <div class="qs-skeleton qs-sk-line" style="width:60px;height:22px" />
                <div class="qs-skeleton qs-sk-btn" />
              </div>
            </div>
          </div>

          <!-- Empty state canônico -->
          <div v-else-if="filteredResults.length === 0" class="ag-empty-state">
            <svg width="48" height="48" viewBox="0 0 24 24" fill="var(--qs-gray-300)"><path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0 0 16 9.5 6.5 6.5 0 1 0 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
            <h3>Nenhum resultado encontrado</h3>
            <p>Tente ajustar seus filtros ou ampliar o raio de busca.</p>
            <button class="qs-btn-outline" @click="clearFilters">Limpar filtros</button>
          </div>

          <div v-else :class="['qs-results-list', `view-${viewMode}`]">
            <article
              v-for="r in filteredResults"
              :key="r.id"
              class="qs-result-card"
            >
              <div class="qs-result-img">
                <div class="qs-result-img-placeholder" :style="{ background: r.color }">
                  {{ r.nome.charAt(0).toUpperCase() }}
                </div>
                <span class="qs-cashback-badge">{{ r.cashback }}% cashback</span>
              </div>
              <div class="qs-result-body">
                <div class="qs-result-cat">{{ catName(r.categoria) }}</div>
                <h3 class="qs-result-name">{{ r.nome }}</h3>
                <p class="qs-result-desc">{{ r.descricao }}</p>
                <div class="qs-result-meta">
                  <span class="qs-meta-item">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5a2.5 2.5 0 0 1 0-5 2.5 2.5 0 0 1 0 5z"/></svg>
                    {{ r.distancia.toFixed(1) }} km
                  </span>
                  <span class="qs-meta-item">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="#f59e0b"><path d="M12 17.27 18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"/></svg>
                    {{ r.rating.toFixed(1) }}
                  </span>
                  <span class="qs-meta-item">{{ r.transacoes }} compras</span>
                </div>
              </div>
              <div class="qs-result-actions">
                <div class="qs-result-price">
                  <span v-if="r.preco" class="qs-price-value">R$ {{ r.preco.toFixed(2).replace('.', ',') }}</span>
                  <span v-if="r.preco" class="qs-price-cashback">+ R$ {{ ((r.preco * r.cashback) / 100).toFixed(2).replace('.', ',') }} de volta</span>
                </div>
                <button class="qs-btn-primary qs-result-cta">
                  {{ r.tipo === 'loja' ? 'Ver loja' : 'Ver oferta' }}
                </button>
              </div>
            </article>
          </div>
        </main>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
useSeoMeta({
  title: 'Busca Inteligente | Quanta Shop — Cashback Perto de Você',
  description: 'Encontre produtos e lojistas com o melhor cashback perto de você. Filtre por distância, categoria e percentual de economia.',
  ogTitle: 'Busca Inteligente | Quanta Shop — Cashback Perto de Você',
  ogDescription: 'Encontre produtos e lojistas com o melhor cashback perto de você. Filtre por distância, categoria e percentual de economia.',
  ogImage: '/logo.png'
});
useHead({
  link: [
    { rel: 'canonical', href: 'https://quantashop.com.br/busca-inteligente' }
  ]
});

interface SearchResult {
  id: number;
  nome: string;
  descricao: string;
  categoria: string;
  cashback: number;
  distancia: number;
  rating: number;
  transacoes: number;
  preco?: number;
  tipo: 'loja' | 'produto';
  color: string;
}

interface Categoria { id: string; nome: string; icon: string; }

const query = ref('');
const minCashback = ref(0);
const maxDistance = ref(25);
const activeCategoria = ref('');
const sortBy = ref<'cashback' | 'distance' | 'popular' | 'price-asc' | 'price-desc'>('cashback');
const viewMode = ref<'grid' | 'list'>('grid');
const loading = ref(false);
const geoLoading = ref(false);
const geoActive = ref(false);
const userLat = ref<number | null>(null);
const userLng = ref<number | null>(null);

const quickSuggestions = ['Mercado', 'Farmácia', 'Restaurantes', 'Eletrônicos', 'Moda'];

const categorias: Categoria[] = [
  { id: 'mercado', nome: 'Mercado', icon: '🛒' },
  { id: 'farmacia', nome: 'Farmácia', icon: '💊' },
  { id: 'restaurante', nome: 'Restaurantes', icon: '🍽️' },
  { id: 'moda', nome: 'Moda', icon: '👕' },
  { id: 'eletronicos', nome: 'Eletrônicos', icon: '📱' },
  { id: 'beleza', nome: 'Beleza', icon: '💄' },
  { id: 'casa', nome: 'Casa', icon: '🏠' },
];

const MOCK_RESULTS: SearchResult[] = [
  { id: 1, nome: 'Centauro', descricao: 'Tênis e artigos esportivos', categoria: 'moda', cashback: 8.0, distancia: 1.2, rating: 4.6, transacoes: 1840, preco: 299.90, tipo: 'loja', color: '#FF6B35' },
  { id: 2, nome: 'Magazine Luiza', descricao: 'Eletrônicos e eletrodomésticos', categoria: 'eletronicos', cashback: 6.5, distancia: 2.8, rating: 4.4, transacoes: 4210, preco: 1899.00, tipo: 'loja', color: '#0086FF' },
  { id: 3, nome: 'Drogaria São Paulo', descricao: 'Farmácia 24h com tele-entrega', categoria: 'farmacia', cashback: 5.0, distancia: 0.4, rating: 4.7, transacoes: 980, tipo: 'loja', color: '#00A859' },
  { id: 4, nome: 'Sonda Supermercados', descricao: 'Mercado completo, frete grátis acima de R$ 150', categoria: 'mercado', cashback: 4.0, distancia: 3.1, rating: 4.5, transacoes: 2340, tipo: 'loja', color: '#E50914' },
  { id: 5, nome: 'Smartphone Galaxy A55', descricao: 'Samsung Galaxy A55 5G 256GB', categoria: 'eletronicos', cashback: 12.0, distancia: 5.2, rating: 4.8, transacoes: 421, preco: 2499.00, tipo: 'produto', color: '#225F6B' },
  { id: 6, nome: 'Renner', descricao: 'Moda feminina, masculina e infantil', categoria: 'moda', cashback: 7.5, distancia: 4.6, rating: 4.5, transacoes: 1650, preco: 159.90, tipo: 'loja', color: '#E4002B' },
  { id: 7, nome: 'Outback Steakhouse', descricao: 'Culinária australiana, ambiente família', categoria: 'restaurante', cashback: 10.0, distancia: 6.8, rating: 4.7, transacoes: 720, preco: 89.00, tipo: 'loja', color: '#7B3F00' },
  { id: 8, nome: 'Sephora', descricao: 'Cosméticos e perfumaria importada', categoria: 'beleza', cashback: 9.0, distancia: 8.1, rating: 4.6, transacoes: 540, preco: 219.00, tipo: 'loja', color: '#000000' },
  { id: 9, nome: 'Fast Shop', descricao: 'Linha branca e tecnologia', categoria: 'eletronicos', cashback: 5.5, distancia: 12.4, rating: 4.3, transacoes: 1100, preco: 3499.00, tipo: 'loja', color: '#FF8C00' },
  { id: 10, nome: 'Madeira Madeira', descricao: 'Móveis e decoração para casa', categoria: 'casa', cashback: 11.0, distancia: 18.3, rating: 4.4, transacoes: 380, preco: 899.00, tipo: 'loja', color: '#8B4513' },
];

const allResults = ref<SearchResult[]>([]);

const agenciaStore = useAgenciaStore();
const api = useApi();

async function runSearch() {
  loading.value = true;
  try {
    const params = new URLSearchParams({
      q: query.value || '',
      minCashback: String(minCashback.value),
      maxDistance: String(maxDistance.value),
      categoria: activeCategoria.value,
      sort: sortBy.value,
    });
    if (geoActive.value && userLat.value !== null && userLng.value !== null) {
      params.set('lat', String(userLat.value));
      params.set('lng', String(userLng.value));
    }
    const token = agenciaStore.getToken?.();
    const headers = token ? { Authorization: `Bearer ${token}` } : undefined;
    const { data } = await api.get(`/busca-inteligente?${params.toString()}`, { headers });
    // backend retorna { items, total, page, pageSize }
    allResults.value = data?.items && Array.isArray(data.items)
      ? data.items
      : (Array.isArray(data) ? data : MOCK_RESULTS);
  } catch {
    allResults.value = MOCK_RESULTS;
  } finally {
    loading.value = false;
  }
}

function useMyLocation() {
  if (!('geolocation' in navigator)) {
    alert('Geolocalização não suportada no seu navegador.');
    return;
  }
  geoLoading.value = true;
  navigator.geolocation.getCurrentPosition(
    (pos) => {
      userLat.value = pos.coords.latitude;
      userLng.value = pos.coords.longitude;
      geoActive.value = true;
      geoLoading.value = false;
      runSearch();
    },
    () => { geoLoading.value = false; alert('Não foi possível obter sua localização.'); },
    { timeout: 8000, enableHighAccuracy: false }
  );
}

function clearFilters() {
  query.value = '';
  minCashback.value = 0;
  maxDistance.value = 25;
  activeCategoria.value = '';
  sortBy.value = 'cashback';
  runSearch();
}

// A API já filtra/ordena server-side — apenas expõe o resultado
const filteredResults = computed(() => allResults.value);

function catName(id: string): string {
  return categorias.find(c => c.id === id)?.nome ?? id;
}

// Debounce helper
let _debounceTimer: ReturnType<typeof setTimeout> | null = null;
function debounceSearch() {
  if (_debounceTimer) clearTimeout(_debounceTimer);
  _debounceTimer = setTimeout(() => runSearch(), 400);
}

// Watchers para re-buscar ao mudar qualquer filtro
watch(minCashback, debounceSearch);
watch(maxDistance, debounceSearch);
watch(activeCategoria, () => runSearch());
watch(sortBy, () => runSearch());

onMounted(runSearch);
</script>

<style scoped>
.qs-search { padding: 32px 0 64px; background: var(--qs-bg); min-height: 100vh; }
.qs-page-header h1 { margin: 4px 0 8px; }

.qs-search-card { padding: 24px; }
.qs-search-bar {
  display: flex;
  gap: 8px;
  align-items: center;
  background: var(--qs-bg);
  padding: 6px 6px 6px 16px;
  border-radius: var(--qs-radius-lg);
  border: 2px solid transparent;
  transition: border-color .2s, background .2s;
}
.qs-search-bar:focus-within {
  background: #fff;
  border-color: var(--qs-teal);
}
.qs-search-icon { flex-shrink: 0; }
.qs-search-input {
  flex: 1;
  border: none;
  background: transparent;
  padding: 12px 8px;
  font-size: 15px;
  font-family: inherit;
  color: var(--qs-ink);
  outline: none;
  min-width: 0;
}
.qs-search-input::placeholder { color: var(--qs-gray-400); }
.qs-search-btn { padding: 12px 24px; }

.qs-quick-filters {
  display: flex;
  gap: 8px;
  align-items: center;
  flex-wrap: wrap;
  margin-top: 16px;
}
.qs-quick-label {
  font-size: 12px;
  color: var(--qs-gray-500);
  font-weight: 500;
}

.qs-search-grid {
  display: grid;
  grid-template-columns: 280px 1fr;
  gap: 24px;
  margin-top: 24px;
}

.qs-filters { display: flex; flex-direction: column; gap: 16px; }
.qs-filter-card {
  background: #fff;
  border-radius: var(--qs-radius-md);
  padding: 18px 20px;
  box-shadow: var(--qs-shadow-xs);
}
.qs-filter-title {
  font-size: 13px;
  font-weight: 600;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  color: var(--qs-gray-700);
  margin: 0 0 12px;
}

.qs-cashback-display {
  display: flex;
  align-items: baseline;
  gap: 8px;
  margin-bottom: 12px;
}
.qs-cashback-value {
  font-size: 24px;
  font-weight: 700;
  color: var(--qs-teal-dark);
  letter-spacing: -0.02em;
  font-variant-numeric: tabular-nums;
}
.qs-cashback-value small { font-size: 13px; font-weight: 500; color: var(--qs-gray-500); margin-left: 2px; }
.qs-cashback-label { font-size: 12px; color: var(--qs-gray-500); }

.qs-range {
  width: 100%;
  -webkit-appearance: none;
  appearance: none;
  height: 4px;
  background: var(--qs-gray-200);
  border-radius: 999px;
  outline: none;
}
.qs-range::-webkit-slider-thumb {
  -webkit-appearance: none;
  appearance: none;
  width: 18px; height: 18px;
  border-radius: 50%;
  background: var(--qs-teal);
  cursor: pointer;
  box-shadow: 0 2px 6px rgba(47, 119, 133, 0.4);
}
.qs-range::-moz-range-thumb {
  width: 18px; height: 18px;
  border-radius: 50%;
  background: var(--qs-teal);
  cursor: pointer;
  border: none;
}
.qs-range--lime::-webkit-slider-thumb { background: var(--qs-lime); box-shadow: 0 2px 6px rgba(152, 199, 58, 0.5); }
.qs-range--lime::-moz-range-thumb { background: var(--qs-lime); }

.qs-range-marks {
  display: flex;
  justify-content: space-between;
  margin-top: 6px;
  font-size: 10px;
  color: var(--qs-gray-400);
}

.qs-geo-btn {
  margin-top: 14px;
  display: inline-flex;
  align-items: center;
  gap: 6px;
  width: 100%;
  justify-content: center;
  background: #fff;
  border: 1px solid var(--qs-teal);
  color: var(--qs-teal);
  padding: 8px 12px;
  border-radius: var(--qs-radius-md);
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  transition: all .2s;
  font-family: inherit;
}
.qs-geo-btn:hover:not(:disabled) {
  background: var(--qs-teal);
  color: #fff;
}

.qs-cat-list {
  display: flex;
  gap: 6px;
  flex-wrap: wrap;
}

.qs-select {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid var(--qs-gray-200);
  border-radius: var(--qs-radius-md);
  font-family: inherit;
  font-size: 14px;
  background: #fff;
  color: var(--qs-ink);
}
.qs-select:focus {
  outline: none;
  border-color: var(--qs-teal);
  box-shadow: 0 0 0 3px rgba(47, 119, 133, 0.12);
}

.qs-clear-btn { width: 100%; justify-content: center; }

/* Results */
.qs-results-head {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
  font-size: 14px;
  color: var(--qs-gray-700);
}
.qs-results-head strong { color: var(--qs-ink); font-size: 16px; }

.qs-view-toggle {
  display: flex;
  background: #fff;
  border-radius: var(--qs-radius-md);
  border: 1px solid var(--qs-gray-200);
  overflow: hidden;
}
.qs-view-toggle button {
  padding: 6px 10px;
  background: #fff;
  border: none;
  color: var(--qs-gray-400);
  cursor: pointer;
}
.qs-view-toggle button.active { background: var(--qs-teal); color: #fff; }

.qs-results-list { display: grid; gap: 16px; }
.qs-results-list.view-grid { grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); }
.qs-results-list.view-list { grid-template-columns: 1fr; }

.qs-result-card {
  background: #fff;
  border-radius: var(--qs-radius-lg);
  overflow: hidden;
  box-shadow: var(--qs-shadow-sm);
  display: flex;
  flex-direction: column;
  transition: transform .2s, box-shadow .2s;
  cursor: pointer;
}
.qs-result-card:hover {
  transform: translateY(-2px);
  box-shadow: var(--qs-shadow-md);
}
.view-list .qs-result-card { flex-direction: row; align-items: stretch; }

.qs-result-img {
  position: relative;
  aspect-ratio: 16 / 10;
  flex-shrink: 0;
}
.view-list .qs-result-img { aspect-ratio: 1; width: 140px; }

.qs-result-img-placeholder {
  width: 100%; height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #fff;
  font-size: 48px;
  font-weight: 800;
  letter-spacing: -0.02em;
}
.view-list .qs-result-img-placeholder { font-size: 36px; }

.qs-cashback-badge {
  position: absolute;
  top: 12px;
  left: 12px;
  background: var(--qs-lime);
  color: #1a3a00;
  font-size: 11px;
  font-weight: 700;
  padding: 4px 10px;
  border-radius: var(--qs-radius-pill);
  letter-spacing: 0.02em;
  box-shadow: 0 2px 8px rgba(0,0,0,0.12);
}

.qs-result-body {
  padding: 16px;
  flex: 1;
  display: flex;
  flex-direction: column;
}
.qs-result-cat {
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--qs-teal);
  margin-bottom: 4px;
}
.qs-result-name {
  font-size: 16px;
  font-weight: 700;
  color: var(--qs-ink);
  margin: 0 0 4px;
  letter-spacing: -0.01em;
}
.qs-result-desc {
  font-size: 13px;
  color: var(--qs-gray-500);
  margin: 0 0 12px;
  line-height: 1.4;
  flex: 1;
}
.qs-result-meta {
  display: flex;
  gap: 12px;
  font-size: 12px;
  color: var(--qs-gray-500);
  flex-wrap: wrap;
}
.qs-meta-item { display: inline-flex; align-items: center; gap: 4px; }

.qs-result-actions {
  padding: 0 16px 16px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 12px;
}
.view-list .qs-result-actions {
  flex-direction: column;
  align-items: flex-end;
  padding: 16px 16px 16px 0;
  justify-content: center;
}
.qs-result-price { display: flex; flex-direction: column; }
.qs-price-value {
  font-size: 16px;
  font-weight: 700;
  color: var(--qs-ink);
  font-variant-numeric: tabular-nums;
}
.qs-price-cashback {
  font-size: 11px;
  color: var(--qs-lime-dark, #7aad1f);
  font-weight: 600;
}
.qs-result-cta { padding: 8px 14px; font-size: 13px; }

/* Skeleton card */
.qs-skeleton-card { pointer-events: none; }
.qs-sk-img {
  aspect-ratio: 16 / 10;
  width: 100%;
  border-radius: 0;
}
.view-list .qs-sk-img { aspect-ratio: 1; width: 140px; flex-shrink: 0; border-radius: 0; }
.qs-sk-line { border-radius: var(--qs-radius-sm); }
.qs-sk-btn {
  width: 90px;
  height: 34px;
  border-radius: var(--qs-radius-md);
}
.qs-result-body { display: flex; flex-direction: column; padding: 16px; flex: 1; }

@media (max-width: 768px) {
  .qs-search-grid { grid-template-columns: 1fr; }
  .qs-search-bar { flex-wrap: wrap; }
  .qs-search-btn { width: 100%; padding: 12px; }
  .view-list .qs-result-card { flex-direction: column; }
  .view-list .qs-result-img { width: 100%; aspect-ratio: 16/10; }
  .view-list .qs-result-actions { flex-direction: row; padding: 0 16px 16px; }
}
</style>
