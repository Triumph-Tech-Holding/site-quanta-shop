<template>
  <div>
    <div class="ag-page-header"><h1>Perguntas Frequentes</h1><p>Dúvidas mais comuns sobre a plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div class="mb-3">
        <input v-model="busca" type="text" class="form-control" placeholder="Buscar pergunta..." />
      </div>
      <div class="accordion" id="faqAccordion">
        <div v-for="(item, i) in faqFiltrado" :key="i" class="accordion-item border-0 mb-2">
          <h2 class="accordion-header">
            <button
              class="accordion-button collapsed fw-semibold"
              style="background:#f8f9fa;border-radius:8px;color:#272222;font-size:.95rem"
              type="button"
              @click="toggleFaq(i)"
            >
              {{ item.pergunta || item.titulo }}
            </button>
          </h2>
          <div v-show="openFaq === i" class="accordion-collapse">
            <div class="accordion-body text-muted" style="font-size:.9rem" v-html="item.resposta || item.conteudo" />
          </div>
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
const faqs = ref<any[]>([]);
const busca = ref('');
const openFaq = ref<number | null>(null);
const faqFiltrado = computed(() => !busca.value ? faqs.value : faqs.value.filter(f => (f.pergunta || f.titulo || '').toLowerCase().includes(busca.value.toLowerCase())));
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function toggleFaq(i: number) { openFaq.value = openFaq.value === i ? null : i; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/faq/listar', authHeader());
    faqs.value = Array.isArray(data) ? data : [];
  } catch { /**/ } finally { loading.value = false; }
});
</script>
