<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Carreira" title="Graduações" description="Seu progresso e conquistas na plataforma Quanta Shop." />

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>

          <!-- Atual -->
          <div v-if="atual" class="qs-card-section grad-atual-card">
            <div class="grad-atual-inner">
              <div class="grad-atual__nivel">{{ atual.nivel || '?' }}</div>
              <div class="grad-atual__info">
                <div class="qs-eyebrow" style="margin-bottom:.25rem;">Sua graduação atual</div>
                <h2 class="grad-atual__nome">{{ atual.nome || atual.graduacao }}</h2>
                <p class="grad-atual__desc">{{ atual.descricao || '' }}</p>
              </div>
            </div>
            <div v-if="progresso !== null" class="grad-progress-wrap">
              <div class="grad-progress-labels">
                <span>Progresso para próxima graduação</span>
                <span class="grad-progress-pct">{{ progresso }}%</span>
              </div>
              <div class="grad-progress-bar">
                <div class="grad-progress-fill" :style="{ width: progresso + '%' }" />
              </div>
            </div>
          </div>

          <!-- Tabela -->
          <div class="qs-card-section grad-tabela-card">
            <div class="qs-section-title" style="margin-bottom:1rem;">Tabela de Graduações</div>
            <div v-if="graduacoes.length === 0" class="ag-empty-state" style="min-height:100px;">
              <h5>Nenhuma graduação disponível</h5>
            </div>
            <div v-else class="grad-table-wrap">
              <table class="qs-table">
                <thead>
                  <tr>
                    <th>Nível</th>
                    <th>Graduação</th>
                    <th>Requisito</th>
                    <th>Benefício</th>
                    <th class="tc">Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(g, i) in graduacoes" :key="i" :class="{ 'grad-row--atual': g.atual }">
                    <td>
                      <div class="grad-nivel-badge" :class="g.atingida ? 'grad-nivel-badge--ok' : ''">
                        N{{ g.nivel }}
                      </div>
                    </td>
                    <td class="grad-nome-cell">{{ g.nome }}</td>
                    <td class="grad-req-cell">{{ g.requisito || '—' }}</td>
                    <td class="grad-ben-cell">{{ g.beneficio || '—' }}</td>
                    <td class="tc">
                      <span class="qs-badge" :class="g.atingida ? 'qs-badge--success' : 'qs-badge--secondary'">
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
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
import type { Graduacao } from "~/types/agencia";
const graduacoes = ref<Graduacao[]>([]);
const atual = ref<Graduacao | null>(null);
const progresso = ref<number | null>(null);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/graduacoes/listar', authHeader());
    if (Array.isArray(data)) {
      graduacoes.value = data;
      atual.value = data.find((g: Graduacao) => g.atual) || data[0];
    } else {
      graduacoes.value = data?.graduacoes || [];
      atual.value = data?.atual || null;
      progresso.value = data?.progresso ?? null;
    }
  } catch { /**/ } finally { loading.value = false; }
});
</script>

<style scoped>
/* Atual */
.grad-atual-card { background: linear-gradient(135deg, var(--qs-teal-dark, #225F6B), var(--qs-teal, #2F7785)); }
.grad-atual-inner { display: flex; align-items: flex-start; gap: 1.25rem; margin-bottom: 1.25rem; }
.grad-atual__nivel {
  width: 64px;
  height: 64px;
  border-radius: 16px;
  background: rgba(255,255,255,.2);
  color: #fff;
  font-size: 1.5rem;
  font-weight: 800;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  border: 2px solid rgba(255,255,255,.3);
}
.grad-atual__info .qs-eyebrow { color: rgba(255,255,255,.7); }
.grad-atual__nome { font-size: 1.25rem; font-weight: 800; color: #fff; margin: 0 0 .25rem; }
.grad-atual__desc { font-size: .875rem; color: rgba(255,255,255,.75); margin: 0; }

.grad-progress-wrap { }
.grad-progress-labels { display: flex; justify-content: space-between; font-size: .8125rem; color: rgba(255,255,255,.85); margin-bottom: .5rem; }
.grad-progress-pct { font-weight: 700; color: var(--qs-lime, #98C73A); }
.grad-progress-bar { height: 10px; background: rgba(255,255,255,.2); border-radius: 999px; overflow: hidden; }
.grad-progress-fill { height: 100%; background: var(--qs-lime, #98C73A); border-radius: 999px; transition: width .4s ease; }

/* Tabela */
.grad-tabela-card { background: #fff; }
.grad-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: .875rem; }
.qs-table thead tr { border-bottom: 2px solid var(--qs-gray-100, #f5f5f7); }
.qs-table th { padding: .625rem .875rem; font-size: .6875rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-500, #6b7280); white-space: nowrap; }
.qs-table td { padding: .75rem .875rem; color: var(--qs-gray-700, #374151); border-bottom: 1px solid var(--qs-gray-100, #f5f5f7); }
.qs-table tbody tr:hover td { background: var(--qs-gray-50, #fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }
.grad-row--atual td { background: rgba(47,119,133,.04); }

.grad-nivel-badge {
  display: inline-flex;
  width: 32px;
  height: 32px;
  border-radius: 8px;
  align-items: center;
  justify-content: center;
  font-size: .75rem;
  font-weight: 700;
  background: var(--qs-gray-100, #f5f5f7);
  color: var(--qs-gray-500, #6b7280);
}
.grad-nivel-badge--ok { background: var(--qs-teal, #2F7785); color: #fff; }
.grad-nome-cell { font-weight: 600; color: var(--qs-teal-dark, #225F6B); }
.grad-ben-cell { color: var(--qs-lime-dark, #7aad1f); font-weight: 500; }
.grad-req-cell { color: var(--qs-gray-500, #6b7280); font-size: .8125rem; }

.qs-badge { display: inline-flex; padding: .2rem .55rem; border-radius: var(--qs-radius-pill, 999px); font-size: .6875rem; font-weight: 700; text-transform: uppercase; white-space: nowrap; }
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--secondary { background: var(--qs-gray-100, #f5f5f7); color: var(--qs-gray-500, #6b7280); }
</style>
