<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">

        <div style="background:linear-gradient(135deg,#2f7785,#1a5c3a);padding:2rem;margin:0 1rem 1.5rem;border-radius:8px;color:#fff;">
          <h1 style="font-size:1.4rem;font-weight:700;text-transform:uppercase;margin-bottom:.5rem;">Ficou com dúvida?</h1>
          <h2 style="font-size:1rem;font-weight:400;margin:0;text-transform:uppercase;">Vamos responder todas</h2>
        </div>

        <div class="px-3 pb-3">
          <div style="background:#fff;border-radius:8px;padding:1rem;box-shadow:0 2px 8px rgba(0,0,0,.06);margin-bottom:1rem;">
            <input
              type="text"
              v-model="pesquisa"
              class="form-control"
              placeholder="Descreva aqui o que deseja saber…"
            />
          </div>

          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

          <template v-else>
            <div v-if="faqFiltrado.length === 0" class="ag-empty-state">
              <h5>Nenhuma dúvida encontrada</h5>
            </div>

            <ul v-else style="list-style:none;padding:0;margin:0;">
              <li v-for="(item, i) in faqFiltrado" :key="i" style="margin-bottom:.75rem;">
                <div
                  style="background:#fff;border-radius:8px;box-shadow:0 2px 8px rgba(0,0,0,.06);cursor:pointer;overflow:hidden;"
                >
                  <div
                    style="padding:.875rem 1rem;display:flex;justify-content:space-between;align-items:center;"
                    @click="item._open = !item._open"
                  >
                    <span style="font-weight:600;color:#225f6b;font-size:.9rem;">{{ item.pergunta }}</span>
                    <svg :style="item._open ? 'transform:rotate(180deg);' : ''" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width:18px;height:18px;flex-shrink:0;transition:.2s;color:#2f7785;"><path d="M7 10l5 5 5-5z"/></svg>
                  </div>
                  <div v-if="item._open" style="padding:.75rem 1rem 1rem;border-top:1px solid #f0f0f0;">
                    <p style="font-size:.875rem;color:#555;margin:0;">{{ item.resposta }}</p>
                  </div>
                </div>
              </li>
            </ul>
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
const pesquisa = ref('');
const faqs = ref<Array<Record<string, unknown>>>([]);

const faqFiltrado = computed(() => {
  const q = pesquisa.value.toLowerCase();
  if (!q) return faqs.value;
  return faqs.value.filter((f) =>
    String(f.pergunta || '').toLowerCase().includes(q) ||
    String(f.resposta || '').toLowerCase().includes(q)
  );
});

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/Faq/obterFaq', authHeader());
    const list = Array.isArray(data) ? data : (data?.items ?? []);
    faqs.value = list.map((f: Record<string, unknown>) => ({ ...f, _open: false }));
  } catch {
    faqs.value = [];
  } finally {
    loading.value = false;
  }
});
</script>
