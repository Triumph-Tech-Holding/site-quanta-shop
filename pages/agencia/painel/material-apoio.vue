<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Conteúdo" title="Material de Apoio" description="Acesse e baixe nossos materiais de apoio para sua equipe." />

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div v-if="items.length === 0" class="ag-empty-state">
            <h5>Nenhum material disponível</h5>
          </div>

          <div v-else class="qs-card-section ma-card">
            <div class="ma-table-wrap">
              <table class="qs-table">
                <thead>
                  <tr>
                    <th>Categoria</th>
                    <th>Nome</th>
                    <th class="tc">Baixar</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, i) in items" :key="i">
                    <td>
                      <span class="ma-cat-badge">{{ item.categoria || item.tipo || 'Geral' }}</span>
                    </td>
                    <td>
                      <div class="ma-nome">
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zm4 18H6V4h7v5h5v11z"/></svg>
                        {{ item.nome || item.titulo || '—' }}
                      </div>
                    </td>
                    <td class="tc">
                      <button
                        class="ma-download-btn"
                        @click="baixarMaterial(String(item.urlMaterial || item.url || ''))"
                      >
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 9h-4V3H9v6H5l7 7 7-7zM5 18v2h14v-2H5z"/></svg>
                        Baixar
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </template>

      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const items = ref<Record<string, unknown>[]>([]);

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function baixarMaterial(url: string) { if (!url) return; window.open(url, '_blank'); }

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/MaterialApoio/listarMateriais', authHeader());
    items.value = Array.isArray(data) ? data : (data?.items ?? []);
  } catch {
    items.value = [];
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.ma-card { background: #fff; }
.ma-table-wrap { overflow-x: auto; }

.qs-table { width: 100%; border-collapse: collapse; font-size: .875rem; }
.qs-table thead tr { border-bottom: 2px solid var(--qs-gray-100, #f5f5f7); }
.qs-table th { padding: .625rem .875rem; font-size: .6875rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-500, #6b7280); white-space: nowrap; }
.qs-table td { padding: .875rem; color: var(--qs-gray-700, #374151); border-bottom: 1px solid var(--qs-gray-100, #f5f5f7); }
.qs-table tbody tr:hover td { background: var(--qs-gray-50, #fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }

.ma-cat-badge {
  display: inline-flex;
  padding: .2rem .6rem;
  background: #dbeafe;
  color: #1d4ed8;
  border-radius: var(--qs-radius-pill, 999px);
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .04em;
}
.ma-nome { display: flex; align-items: center; gap: .5rem; font-weight: 500; }
.ma-nome svg { width: 16px; height: 16px; color: var(--qs-teal, #2F7785); flex-shrink: 0; }

.ma-download-btn {
  display: inline-flex;
  align-items: center;
  gap: .3rem;
  padding: .375rem .875rem;
  background: var(--qs-gradient-btn, linear-gradient(135deg, #225F6B, #2F7785));
  color: #fff;
  border: none;
  border-radius: var(--qs-radius-md, 12px);
  font-size: .75rem;
  font-weight: 700;
  cursor: pointer;
  transition: opacity .2s;
}
.ma-download-btn:hover { opacity: .85; }
.ma-download-btn svg { width: 13px; height: 13px; }
</style>
