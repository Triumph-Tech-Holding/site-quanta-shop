<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page">Material de apoio</h2>
        </div>

        <div style="background:linear-gradient(135deg,#2f7785,#1a5c3a);padding:2rem;margin:0 1rem 1.5rem;border-radius:8px;color:#fff;">
          <h1 style="font-size:1.4rem;font-weight:700;text-transform:uppercase;margin-bottom:.5rem;">Material de apoio</h1>
          <h2 style="font-size:1rem;font-weight:400;margin:0;">Aqui você poderá acessar nossos materiais de apoio</h2>
        </div>

        <div class="px-3 pb-3">
          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

          <template v-else>
            <div v-if="items.length === 0" class="ag-empty-state">
              <h5>Nenhum material disponível</h5>
            </div>

            <div v-else style="background:#fff;border-radius:8px;box-shadow:0 2px 8px rgba(0,0,0,.06);overflow:hidden;">
              <div style="overflow-x:auto;">
                <table class="table-custom" style="width:100%;">
                  <thead>
                    <tr>
                      <th>Categoria</th>
                      <th>Nome</th>
                      <th style="text-align:center;">Ações</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(item, i) in items" :key="i">
                      <td>{{ item.categoria || item.tipo || '—' }}</td>
                      <td>{{ item.nome || item.titulo || '—' }}</td>
                      <td style="text-align:center;">
                        <button
                          class="btn-abrir-chamado"
                          style="font-size:.75rem;padding:.3rem .75rem;"
                          @click="baixarMaterial(String(item.urlMaterial || item.url || ''))"
                        >
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
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const items = ref<Record<string, unknown>[]>([]);

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function baixarMaterial(url: string) {
  if (!url) return;
  window.open(url, '_blank');
}

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
