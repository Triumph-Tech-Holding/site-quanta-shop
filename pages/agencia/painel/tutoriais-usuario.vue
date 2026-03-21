<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page">Tutoriais</h2>
        </div>

        <div style="background:linear-gradient(135deg,#2f7785,#1a5c3a);padding:2rem;margin:0 1rem 1.5rem;border-radius:8px;color:#fff;">
          <h1 style="font-size:1.4rem;font-weight:700;text-transform:uppercase;margin-bottom:.5rem;">Tutoriais</h1>
          <h2 style="font-size:1rem;font-weight:400;margin:0;">Aprenda a usar a plataforma e maximize seus ganhos</h2>
        </div>

        <div class="px-3 pb-3">
          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

          <template v-else>
            <div v-if="tutoriais.length === 0" class="ag-empty-state">
              <h5>Nenhum tutorial disponível</h5>
            </div>

            <div v-for="(item, i) in tutoriais" :key="i" class="ag-card" style="margin-bottom:1rem;">
              <div
                style="cursor:pointer;display:flex;justify-content:space-between;align-items:center;"
                @click="item._open = !item._open"
              >
                <span style="font-weight:600;color:#225f6b;">{{ item.nome }}</span>
                <svg :style="item._open ? 'transform:rotate(180deg);' : ''" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width:20px;height:20px;transition:.2s;color:#2f7785;"><path d="M7 10l5 5 5-5z"/></svg>
              </div>

              <div v-if="item._open" style="margin-top:1rem;">
                <div style="display:flex;flex-wrap:wrap;gap:1rem;">
                  <div style="flex:1;min-width:200px;">
                    <div style="position:relative;padding-bottom:56.25%;height:0;overflow:hidden;border-radius:8px;">
                      <iframe
                        :src="embedUrl(String(item.url))"
                        style="position:absolute;top:0;left:0;width:100%;height:100%;border:none;"
                        allowfullscreen
                      ></iframe>
                    </div>
                  </div>
                  <div style="flex:1;min-width:180px;">
                    <strong style="display:block;margin-bottom:.5rem;color:#225f6b;">Descrição</strong>
                    <p style="font-size:.875rem;color:#555;">{{ item.descricao }}</p>
                  </div>
                </div>
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
const tutoriais = ref<Array<Record<string, unknown>>>([]);

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function embedUrl(url: string): string {
  if (!url) return '';
  const yt = url.match(/(?:youtube\.com\/watch\?v=|youtu\.be\/)([^&?]+)/);
  if (yt) return `https://www.youtube.com/embed/${yt[1]}`;
  return url;
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/Tutorial/obterTutoriais', authHeader());
    const list = Array.isArray(data) ? data : (data?.items ?? []);
    tutoriais.value = list.filter((t: Record<string, unknown>) => t.ativo !== false).map((t: Record<string, unknown>) => ({ ...t, _open: false }));
  } catch {
    tutoriais.value = [];
  } finally {
    loading.value = false;
  }
});
</script>
