<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Ajuda" title="Perguntas Frequentes" description="Ficou com dúvida? Vamos responder todas." />

        <div class="qs-card-section faq-search-card">
          <div class="faq-search-wrap">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0 0 16 9.5 6.5 6.5 0 1 0 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
            <input
              type="text"
              v-model="pesquisa"
              class="faq-search-input"
              placeholder="Descreva aqui o que deseja saber…"
            />
          </div>
        </div>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div v-if="faqFiltrado.length === 0" class="ag-empty-state">
            <h5>Nenhuma dúvida encontrada</h5>
          </div>

          <div v-else class="faq-list">
            <div
              v-for="(item, i) in faqFiltrado"
              :key="i"
              class="faq-item"
              :class="{ 'faq-item--open': item._open }"
            >
              <button class="faq-item__trigger" @click="item._open = !item._open">
                <span class="faq-item__q">{{ item.pergunta }}</span>
                <svg class="faq-item__chevron" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M7 10l5 5 5-5z"/></svg>
              </button>
              <div v-if="item._open" class="faq-item__answer">
                <p>{{ item.resposta }}</p>
              </div>
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
const pesquisa = ref('');
const faqs = ref<Array<Record<string, unknown>>>([]);

const faqFiltrado = computed(() => {
  const q = pesquisa.value.toLowerCase();
  if (!q) return faqs.value;
  return faqs.value.filter(f =>
    String(f.pergunta || '').toLowerCase().includes(q) ||
    String(f.resposta || '').toLowerCase().includes(q)
  );
});

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }

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

<style scoped>
.faq-search-card { background: #fff; }
.faq-search-wrap {
  display: flex;
  align-items: center;
  gap: .625rem;
  background: var(--qs-gray-50, #fafafa);
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  padding: .625rem 1rem;
}
.faq-search-wrap svg { width: 18px; height: 18px; color: var(--qs-gray-400, #9ca3af); flex-shrink: 0; }
.faq-search-input { border: none; background: transparent; font-size: .9375rem; color: var(--qs-ink, #1d1d1f); width: 100%; outline: none; }

.faq-list { display: flex; flex-direction: column; gap: .625rem; }
.faq-item {
  background: #fff;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  overflow: hidden;
  transition: border-color .15s, box-shadow .15s;
}
.faq-item--open {
  border-color: var(--qs-teal, #2F7785);
  box-shadow: 0 0 0 3px rgba(47,119,133,.08);
}
.faq-item__trigger {
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  padding: 1rem 1.125rem;
  background: none;
  border: none;
  cursor: pointer;
  text-align: left;
}
.faq-item__q {
  font-size: .9375rem;
  font-weight: 600;
  color: var(--qs-teal-dark, #225F6B);
  line-height: 1.4;
}
.faq-item__chevron {
  width: 20px;
  height: 20px;
  flex-shrink: 0;
  color: var(--qs-teal, #2F7785);
  transition: transform .2s;
}
.faq-item--open .faq-item__chevron { transform: rotate(180deg); }
.faq-item__answer {
  padding: .75rem 1.125rem 1rem;
  border-top: 1px solid var(--qs-gray-100, #f5f5f7);
}
.faq-item__answer p {
  font-size: .875rem;
  color: var(--qs-gray-600, #4b5563);
  line-height: 1.65;
  margin: 0;
}
</style>
