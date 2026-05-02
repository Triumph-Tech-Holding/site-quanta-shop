<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">Conteúdo</div>
            <h1>Tutoriais</h1>
            <p>Aprenda a usar a plataforma e maximize seus ganhos.</p>
          </div>
        </div>

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>
          <div v-if="tutoriais.length === 0" class="ag-empty-state">
            <h5>Nenhum tutorial disponível</h5>
          </div>

          <div v-else class="tut-list">
            <div
              v-for="(item, i) in tutoriais"
              :key="i"
              class="tut-item"
              :class="{ 'tut-item--open': item._open }"
            >
              <button class="tut-item__trigger" @click="item._open = !item._open">
                <div class="tut-item__left">
                  <div class="tut-item__icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M8 5v14l11-7z"/></svg>
                  </div>
                  <span class="tut-item__nome">{{ item.nome }}</span>
                </div>
                <svg class="tut-item__chevron" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M7 10l5 5 5-5z"/></svg>
              </button>

              <div v-if="item._open" class="tut-item__body">
                <div class="tut-item__content">
                  <div class="tut-video-wrap">
                    <iframe
                      :src="embedUrl(String(item.url))"
                      class="tut-iframe"
                      allowfullscreen
                    ></iframe>
                  </div>
                  <div class="tut-desc">
                    <strong class="tut-desc__label">Descrição</strong>
                    <p class="tut-desc__text">{{ item.descricao }}</p>
                  </div>
                </div>
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
const tutoriais = ref<Array<Record<string, unknown>>>([]);

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
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
    tutoriais.value = list
      .filter((t: Record<string, unknown>) => t.ativo !== false)
      .map((t: Record<string, unknown>) => ({ ...t, _open: false }));
  } catch {
    tutoriais.value = [];
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.tut-list { display: flex; flex-direction: column; gap: .625rem; }
.tut-item {
  background: #fff;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  overflow: hidden;
  transition: border-color .15s, box-shadow .15s;
}
.tut-item--open {
  border-color: var(--qs-teal, #2F7785);
  box-shadow: 0 0 0 3px rgba(47,119,133,.08);
}
.tut-item__trigger {
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
.tut-item__left { display: flex; align-items: center; gap: .75rem; }
.tut-item__icon {
  width: 36px;
  height: 36px;
  border-radius: 10px;
  background: var(--qs-gradient-btn, linear-gradient(135deg, #225F6B, #2F7785));
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}
.tut-item__icon svg { width: 18px; height: 18px; color: #fff; }
.tut-item__nome { font-size: .9375rem; font-weight: 600; color: var(--qs-teal-dark, #225F6B); }
.tut-item__chevron { width: 20px; height: 20px; flex-shrink: 0; color: var(--qs-teal, #2F7785); transition: transform .2s; }
.tut-item--open .tut-item__chevron { transform: rotate(180deg); }

.tut-item__body { border-top: 1px solid var(--qs-gray-100, #f5f5f7); padding: 1rem 1.125rem; }
.tut-item__content { display: flex; flex-wrap: wrap; gap: 1.25rem; }
.tut-video-wrap {
  flex: 1;
  min-width: 220px;
  position: relative;
  padding-bottom: 56.25%;
  height: 0;
  overflow: hidden;
  border-radius: var(--qs-radius-md, 12px);
}
.tut-iframe { position: absolute; top: 0; left: 0; width: 100%; height: 100%; border: none; }
.tut-desc { flex: 1; min-width: 180px; }
.tut-desc__label { display: block; font-weight: 700; color: var(--qs-teal-dark, #225F6B); margin-bottom: .5rem; font-size: .875rem; }
.tut-desc__text { font-size: .875rem; color: var(--qs-gray-600, #4b5563); line-height: 1.65; margin: 0; }
</style>
