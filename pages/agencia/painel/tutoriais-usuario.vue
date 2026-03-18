<template>
  <div>
    <div class="ag-page-header">
      <h1>Tutoriais</h1>
      <p>Aprenda a usar a plataforma com nossos vídeos</p>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <template v-else-if="tutoriais.length === 0">
      <div class="ag-empty-state">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M21 3H3c-1.11 0-2 .89-2 2v12c0 1.1.89 2 2 2h5v2h8v-2h5c1.1 0 1.99-.9 1.99-2L23 5c0-1.11-.9-2-2-2zm0 14H3V5h18v12zm-5-6l-7 4V7l7 4z"/></svg>
        <h5>Nenhum tutorial disponível</h5>
        <p>Em breve teremos vídeos para ajudar você a aproveitar ao máximo a plataforma.</p>
      </div>
    </template>

    <template v-else>
      <div class="row g-3">
        <div v-for="(t, i) in tutoriais" :key="i" class="col-12 col-sm-6 col-lg-4">
          <a :href="t.url" target="_blank" rel="noopener" class="ag-tutorial-card d-block text-decoration-none">
            <div class="ag-tutorial-thumb">
              <img v-if="t.thumb" :src="t.thumb" :alt="t.nome" style="width:100%;height:100%;object-fit:cover;position:absolute;inset:0" />
              <div class="play-icon">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/></svg>
              </div>
            </div>
            <div class="ag-tutorial-body">
              <div class="ag-tutorial-title">{{ t.nome }}</div>
              <div v-if="t.descricao" class="ag-tutorial-desc">{{ t.descricao }}</div>
            </div>
          </a>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();

const loading = ref(true);

interface Tutorial {
  idTutorial?: number;
  nome: string;
  descricao?: string;
  url: string;
  thumb?: string;
  ativo?: boolean;
}

const tutoriais = ref<Tutorial[]>([]);

function getYouTubeThumb(url: string): string | null {
  const match = url.match(/(?:youtube\.com\/watch\?v=|youtu\.be\/)([^&\s]+)/);
  if (match) return `https://img.youtube.com/vi/${match[1]}/mqdefault.jpg`;
  return null;
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const token = agenciaStore.getToken();
    const { data } = await api.get('/Tutorial/ObterTutoriais', {
      headers: { Authorization: `Bearer ${token}` },
    });
    const list = Array.isArray(data) ? data : [];
    tutoriais.value = list
      .filter((t: Record<string, unknown>) => t.Ativo !== false && t.ativo !== false)
      .map((t: Record<string, unknown>) => ({
        idTutorial: (t.IdTutorial ?? t.idTutorial) as number,
        nome: (t.Nome ?? t.nome ?? 'Tutorial') as string,
        descricao: (t.Descricao ?? t.descricao ?? '') as string,
        url: (t.URL ?? t.url ?? '#') as string,
        thumb: getYouTubeThumb((t.URL ?? t.url ?? '') as string) || undefined,
        ativo: (t.Ativo ?? t.ativo ?? true) as boolean,
      }));
  } catch (e) {
    console.error('Erro ao carregar tutoriais:', e);
    tutoriais.value = [];
  } finally {
    loading.value = false;
  }
});
</script>
