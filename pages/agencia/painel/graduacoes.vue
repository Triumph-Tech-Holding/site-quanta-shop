<template>
  <div>
    <div class="ag-page-header"><h1>Graduações</h1><p>Seu progresso e conquistas na plataforma</p></div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <template v-else>
      <div class="ag-card mb-4" v-if="atual">
        <div class="ag-card-title">Graduação Atual</div>
        <div class="d-flex align-items-center gap-3 mb-3">
          <div class="rounded-circle bg-ag-primary text-white d-flex align-items-center justify-content-center" style="width:64px;height:64px;font-size:1.5rem;font-weight:700">
            {{ atual.nivel || '?' }}
          </div>
          <div>
            <h4 class="mb-0 text-ag-primary">{{ atual.nome || atual.graduacao }}</h4>
            <div class="text-muted" style="font-size:.875rem">{{ atual.descricao }}</div>
          </div>
        </div>
        <div v-if="progresso !== null">
          <div class="d-flex justify-content-between mb-1" style="font-size:.85rem">
            <span>Progresso para próxima graduação</span>
            <span class="fw-bold">{{ progresso }}%</span>
          </div>
          <div class="progress" style="height:12px;border-radius:6px">
            <div class="progress-bar bg-ag-secondary" :style="{ width: progresso + '%' }" />
          </div>
        </div>
      </div>
      <div class="ag-card">
        <div class="ag-card-title">Tabela de Graduações</div>
        <div v-if="graduacoes.length === 0" class="ag-empty-state"><h5>Nenhuma graduação disponível</h5></div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead><tr><th>Nível</th><th>Graduação</th><th>Requisito</th><th>Benefício</th><th>Status</th></tr></thead>
            <tbody>
              <tr v-for="(g, i) in graduacoes" :key="i">
                <td><strong>N{{ g.nivel }}</strong></td>
                <td>{{ g.nome }}</td>
                <td>{{ g.requisito || '—' }}</td>
                <td class="text-ag-secondary">{{ g.beneficio || '—' }}</td>
                <td>
                  <span class="badge-ag" :class="g.atingida ? 'badge-ag-success' : 'badge-ag-secondary'">
                    {{ g.atingida ? '✓ Atingida' : 'Pendente' }}
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
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
const graduacoes = ref<any[]>([]);
const atual = ref<any>(null);
const progresso = ref<number | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/graduacoes/listar', authHeader());
    if (Array.isArray(data)) {
      graduacoes.value = data;
      atual.value = data.find(g => g.atual) || data[0];
    } else {
      graduacoes.value = data?.graduacoes || [];
      atual.value = data?.atual || null;
      progresso.value = data?.progresso ?? null;
    }
  } catch { /**/ } finally { loading.value = false; }
});
</script>
