<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page" style="display:flex;justify-content:space-between;align-items:center;">
          <h2 class="title-page">Solicitações de suporte</h2>
          <NuxtLink to="/agencia/painel/solicitar-suporte" class="btn-abrir-chamado">
            + Abrir chamado
          </NuxtLink>
        </div>

        <div class="px-3 pb-3">
          <div class="box-filter">
            <h2>Filtros</h2>
            <form @submit.prevent="buscarSuporte">
              <div style="display:flex;flex-wrap:wrap;gap:1rem;margin-bottom:1rem;">
                <div style="flex:1;min-width:150px;">
                  <label>Status</label>
                  <select v-model="filtro.idStatus" class="form-control">
                    <option value="">Todos</option>
                    <option v-for="s in statusOptions" :key="s.value" :value="s.value">{{ s.text }}</option>
                  </select>
                </div>
                <div style="flex:1;min-width:150px;">
                  <label>Tipo</label>
                  <select v-model="filtro.idTipo" class="form-control">
                    <option value="">Todos</option>
                    <option v-for="t in tipoOptions" :key="t.value" :value="t.value">{{ t.text }}</option>
                  </select>
                </div>
                <div style="flex:1;min-width:150px;">
                  <label>Data de abertura (início)</label>
                  <input type="date" v-model="filtro.dataInicio" class="form-control" />
                </div>
                <div style="flex:1;min-width:150px;">
                  <label>Data de abertura (fim)</label>
                  <input type="date" v-model="filtro.dataFim" class="form-control" />
                </div>
                <div style="display:flex;align-items:flex-end;">
                  <button type="submit" class="btn-filtrar">Filtrar</button>
                </div>
              </div>
            </form>
          </div>

          <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

          <template v-else>
            <div style="background:#fff;border-radius:8px;box-shadow:0 2px 8px rgba(0,0,0,.06);overflow:hidden;">
              <div style="padding:1rem;display:flex;justify-content:space-between;align-items:center;border-bottom:1px solid #eee;">
                <span style="font-weight:700;color:#225f6b;">Solicitações</span>
                <span style="font-size:.8rem;color:#6c757d;">{{ items.length }} resultado(s)</span>
              </div>

              <div v-if="items.length === 0" class="ag-empty-state">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2z"/></svg>
                <h5>Nenhuma solicitação encontrada</h5>
                <p>Tente ajustar os filtros ou <NuxtLink to="/agencia/painel/solicitar-suporte">abra um novo chamado</NuxtLink></p>
              </div>

              <div v-else style="overflow-x:auto;">
                <table class="table-custom" style="width:100%;">
                  <thead>
                    <tr>
                      <th>#</th>
                      <th>Título</th>
                      <th>Tipo</th>
                      <th style="text-align:center;">Data abertura</th>
                      <th style="text-align:center;">Status</th>
                      <th style="text-align:center;">Ações</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(item, i) in items" :key="i">
                      <td>{{ item.idSuporte }}</td>
                      <td>{{ item.titulo || item.assunto || '—' }}</td>
                      <td>{{ tipoLabel(Number(item.tipo)) }}</td>
                      <td style="text-align:center;">{{ formatDate(String(item.dataAbertura || item.dataCriacao || '')) }}</td>
                      <td style="text-align:center;">
                        <span class="status-badge" :class="statusBadgeClass(Number(item.status))">
                          {{ statusLabel(Number(item.status)) }}
                        </span>
                      </td>
                      <td style="text-align:center;">
                        <NuxtLink :to="`/agencia/painel/suporte/${item.idSuporte}`" style="color:#2f7785;font-weight:600;font-size:.8rem;">
                          Ver detalhes
                        </NuxtLink>
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
const loading = ref(false);
const items = ref<Record<string, unknown>[]>([]);

const statusOptions = [
  { value: 1, text: 'Em processamento' },
  { value: 2, text: 'Finalizado' },
  { value: 3, text: 'Cancelado' },
  { value: 4, text: 'Em aprovação' },
  { value: 5, text: 'Recusado' },
  { value: 6, text: 'Aprovado' },
  { value: 7, text: 'Aguardando pagamento de fatura' },
];
const tipoOptions = [
  { value: 1, text: 'Contato' },
  { value: 2, text: 'Cashback não pago' },
  { value: 3, text: 'Cancelamento do parcelamento' },
  { value: 4, text: 'Reabertura do parcelamento' },
];

const filtro = reactive({
  idStatus: '',
  idTipo: '',
  dataInicio: '',
  dataFim: '',
});

function authHeader() {
  return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } };
}

function formatDate(d: string): string {
  if (!d) return '—';
  return new Date(d).toLocaleDateString('pt-BR');
}

const TIPO_MAP: Record<number, string> = {
  1: 'Contato',
  2: 'Cashback não pago',
  3: 'Cancelamento do parcelamento',
  4: 'Reabertura do parcelamento',
};

function tipoLabel(code: number): string {
  return TIPO_MAP[code] ?? String(code || '—');
}

const STATUS_MAP: Record<number, { label: string; cls: string }> = {
  1: { label: 'Em processamento', cls: 'pendente' },
  2: { label: 'Finalizado', cls: 'aprovado' },
  3: { label: 'Cancelado', cls: 'recusado' },
  4: { label: 'Em aprovação', cls: 'pendente' },
  5: { label: 'Recusado', cls: 'recusado' },
  6: { label: 'Aprovado', cls: 'aprovado' },
  7: { label: 'Aguardando pagamento', cls: 'pendente' },
};

function statusLabel(code: number): string {
  return STATUS_MAP[code]?.label ?? String(code || '—');
}
function statusBadgeClass(code: number): string {
  return STATUS_MAP[code]?.cls ?? 'pendente';
}

async function buscarSuporte() {
  loading.value = true;
  try {
    const body: Record<string, unknown> = {};
    if (filtro.idStatus) body.idStatus = filtro.idStatus;
    if (filtro.idTipo) body.idTipo = filtro.idTipo;
    if (filtro.dataInicio) body.dataInicio = new Date(filtro.dataInicio).toISOString();
    if (filtro.dataFim) body.dataFim = new Date(filtro.dataFim + 'T23:59:59').toISOString();

    const { data } = await api.post('/Suporte/listaSuporte', body, authHeader());
    items.value = Array.isArray(data) ? data : [];
  } catch {
    try {
      const { data } = await api.get('/Suporte/listaSuporte', authHeader());
      items.value = Array.isArray(data) ? data : [];
    } catch {
      items.value = [];
    }
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  agenciaStore.loadFromStorage();
  buscarSuporte();
});
</script>
