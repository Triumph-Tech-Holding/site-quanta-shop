<template>
  <div>
    <div class="ag-page-header">
      <h1>Painel Geral</h1>
      <p>Bem-vindo de volta, {{ user?.username }}</p>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <template v-else>
      <div class="row g-3 mb-4">
        <div class="col-12 col-sm-6 col-xl-3">
          <div class="ag-stat-card">
            <div class="stat-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
            </div>
            <div>
              <div class="stat-label">Saldo disponível</div>
              <div class="stat-value text-ag-primary">{{ formatCurrency(resumo.saldo) }}</div>
            </div>
          </div>
        </div>
        <div class="col-12 col-sm-6 col-xl-3">
          <div class="ag-stat-card">
            <div class="stat-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M16 6l2.29 2.29-4.88 4.88-4-4L2 16.59 3.41 18l6-6 4 4 6.3-6.29L22 12V6z"/></svg>
            </div>
            <div>
              <div class="stat-label">Total ganho</div>
              <div class="stat-value">{{ formatCurrency(resumo.totalGanhos) }}</div>
            </div>
          </div>
        </div>
        <div class="col-12 col-sm-6 col-xl-3">
          <div class="ag-stat-card">
            <div class="stat-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-8 0c1.66 0 2.99-1.34 2.99-3S9.66 5 8 5C6.34 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
            </div>
            <div>
              <div class="stat-label">Minha rede</div>
              <div class="stat-value">{{ resumo.totalRede }}</div>
            </div>
          </div>
        </div>
        <div class="col-12 col-sm-6 col-xl-3">
          <div class="ag-stat-card">
            <div class="stat-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/></svg>
            </div>
            <div>
              <div class="stat-label">Minhas compras</div>
              <div class="stat-value">{{ resumo.totalCompras }}</div>
            </div>
          </div>
        </div>
      </div>

      <div class="row g-3">
        <div class="col-12 col-lg-6">
          <div class="ag-card">
            <div class="ag-card-title">Link de indicação</div>
            <div class="d-flex align-items-center gap-2 flex-wrap">
              <code class="bg-light p-2 rounded flex-grow-1 text-break" style="font-size:.85rem">
                {{ linkIndicacao }}
              </code>
              <button class="btn btn-ag-outline btn-sm" @click="copiarLink">
                {{ copiado ? '✓ Copiado!' : 'Copiar' }}
              </button>
            </div>
            <div class="mt-2 text-muted" style="font-size:.8rem">
              Compartilhe este link e ganhe cashback quando seus amigos se cadastrarem.
            </div>
          </div>
        </div>

        <div class="col-12 col-lg-6">
          <div class="ag-card">
            <div class="ag-card-title">Últimas movimentações</div>
            <div v-if="ultimasCompras.length === 0" class="text-muted text-center py-3" style="font-size:.875rem">
              Nenhuma movimentação encontrada
            </div>
            <table v-else class="table ag-table table-sm mb-0">
              <thead><tr><th>Descrição</th><th>Data</th><th>Valor</th></tr></thead>
              <tbody>
                <tr v-for="(c, i) in ultimasCompras" :key="i">
                  <td>{{ c.nomeLoja || c.descricao || '—' }}</td>
                  <td>{{ formatDate(c.dataPedido || c.data) }}</td>
                  <td :class="(c.cashback || c.valorCashback || 0) >= 0 ? 'text-ag-secondary fw-bold' : 'text-danger fw-bold'">
                    {{ formatCurrency(c.cashback || c.valorCashback || 0) }}
                  </td>
                </tr>
              </tbody>
            </table>
            <div class="text-end mt-2">
              <NuxtLink to="/agencia/painel/financeiro" class="text-ag-primary" style="font-size:.85rem">
                Ver todas →
              </NuxtLink>
            </div>
          </div>
        </div>
      </div>

      <div class="row g-3 mt-1" v-if="comunicados.length > 0">
        <div class="col-12">
          <div class="ag-card">
            <div class="ag-card-title">Comunicados</div>
            <div v-for="(com, i) in comunicados" :key="i" class="mb-3 p-3 border-start border-3 border-ag-secondary bg-light rounded">
              <div class="fw-bold mb-1">{{ com.titulo || com.Titulo }}</div>
              <div style="font-size:.875rem" v-html="com.mensagem || com.Texto" />
            </div>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();

const user = computed(() => agenciaStore.dadosUser);
const loading = ref(true);
const copiado = ref(false);

const resumo = reactive({
  saldo: 0,
  totalGanhos: 0,
  totalRede: 0,
  totalCompras: 0,
});

import type { Compra, Comunicado } from "~/types/agencia";
const ultimasCompras = ref<Compra[]>([]);
const comunicados = ref<Comunicado[]>([]);

const linkIndicacao = computed(() => {
  const login = user.value?.login || '';
  return `https://quantashop.com.br/register/${login}`;
});

function formatCurrency(v: number | null): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

function formatDate(d: string | null): string {
  if (!d) return '—';
  return new Date(d).toLocaleDateString('pt-BR');
}

async function copiarLink() {
  try {
    await navigator.clipboard.writeText(linkIndicacao.value);
    copiado.value = true;
    setTimeout(() => { copiado.value = false; }, 2000);
  } catch { /**/ }
}

function authHeader() {
  const token = agenciaStore.getToken();
  return { headers: { Authorization: `Bearer ${token}` } };
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  if (!agenciaStore.checkTokenExpiry()) {
    navigateTo('/agencia/login');
    return;
  }

  await Promise.allSettled([
    api.get('/Extrato/obterSaldoPorTipo', authHeader()).then(r => {
      const d = r.data;
      const entradas = d?.totalEntradas ?? 0;
      const saidas = d?.totalSaidas ?? 0;
      resumo.saldo = entradas + saidas;
      resumo.totalGanhos = entradas;
    }),
    api.get('/Dashboard/obterBarraStatus', authHeader()).then(r => {
      const d = r.data;
      resumo.totalRede = d?.Total ?? d?.total ?? 0;
    }),
    api.post('/Extrato/buscarExtrato', {}, authHeader()).then(r => {
      const d = r.data;
      const lancamentos = Array.isArray(d) ? d : [];
      const positivos = lancamentos.filter((l: Record<string, unknown>) => (l.Valor ?? l.valor ?? 0) > 0);
      resumo.totalCompras = positivos.length;
      ultimasCompras.value = positivos.slice(0, 5).map((l: Record<string, unknown>) => ({
        nomeLoja: (l.Descricao ?? l.descricao ?? l.Tipo?.Nome ?? l.tipo?.nome ?? '—') as string,
        dataPedido: (l.DataLancamento ?? l.dataLancamento ?? '') as string,
        cashback: (l.Valor ?? l.valor ?? 0) as number,
      }));
    }),
    api.get('/Comunicado/ObterComunicadosPorGraduacao', authHeader()).then(r => {
      const d = r.data;
      comunicados.value = Array.isArray(d) ? d.slice(0, 3).map((c: Record<string, unknown>) => ({
        titulo: (c.Titulo ?? c.titulo ?? '') as string,
        mensagem: (c.Texto ?? c.texto ?? c.mensagem ?? '') as string,
      })) : [];
    }),
  ]);

  loading.value = false;
});
</script>
