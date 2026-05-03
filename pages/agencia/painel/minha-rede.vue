<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Rede" title="Minha Rede" description="Visualize e acompanhe os membros da sua rede de indicados." />

        <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

        <template v-else>

          <!-- KPIs -->
          <div class="mr-kpi-grid">
            <div class="mr-kpi">
              <div class="mr-kpi__icon mr-kpi__icon--teal">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-8 0c1.66 0 2.99-1.34 2.99-3S9.66 5 8 5C6.34 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
              </div>
              <div class="mr-kpi__valor">{{ resumo.total }}</div>
              <div class="mr-kpi__label">Total na rede</div>
            </div>
            <div class="mr-kpi">
              <div class="mr-kpi__icon mr-kpi__icon--lime">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/></svg>
              </div>
              <div class="mr-kpi__valor mr-kpi__valor--lime">{{ resumo.ativos }}</div>
              <div class="mr-kpi__label">Ativos</div>
            </div>
            <div class="mr-kpi">
              <div class="mr-kpi__icon mr-kpi__icon--gray">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
              </div>
              <div class="mr-kpi__valor">{{ resumo.inativos }}</div>
              <div class="mr-kpi__label">Inativos</div>
            </div>
            <div class="mr-kpi">
              <div class="mr-kpi__icon mr-kpi__icon--gold">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
              </div>
              <div class="mr-kpi__valor mr-kpi__valor--teal">{{ formatCurrency(resumo.ganhos) }}</div>
              <div class="mr-kpi__label">Ganhos da rede</div>
            </div>
          </div>

          <!-- Membros -->
          <div class="qs-card-section mr-card">
            <div class="mr-card-header">
              <div class="qs-section-title">Membros da Rede</div>
              <div class="mr-search-wrap">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0 0 16 9.5 6.5 6.5 0 1 0 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
                <input v-model="busca" type="text" class="mr-search" placeholder="Buscar por nome ou e-mail..." />
              </div>
            </div>

            <div v-if="redeFiltrada.length === 0" class="ag-empty-state" style="min-height:120px;">
              <h5>Nenhum membro encontrado</h5>
            </div>
            <div v-else class="mr-table-wrap">
              <table class="qs-table">
                <thead>
                  <tr>
                    <th>Nome</th>
                    <th>Login</th>
                    <th class="tc">Nível</th>
                    <th class="tc">Cadastro</th>
                    <th class="tc">Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(m, i) in redeFiltrada" :key="i">
                    <td>
                      <div class="mr-member">
                        <div class="mr-avatar">{{ (m.nome || m.username || '?').charAt(0).toUpperCase() }}</div>
                        {{ m.nome || m.username }}
                      </div>
                    </td>
                    <td class="mr-login">{{ m.login || m.email }}</td>
                    <td class="tc"><span class="mr-nivel-badge">N{{ m.nivel || 1 }}</span></td>
                    <td class="tc">{{ formatDate(m.dataCadastro || m.createdAt) }}</td>
                    <td class="tc">
                      <span class="qs-badge" :class="m.ativo ? 'qs-badge--success' : 'qs-badge--secondary'">
                        {{ m.ativo ? 'Ativo' : 'Inativo' }}
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
const busca = ref('');
import type { MembroRede } from "~/types/agencia";
const rede = ref<MembroRede[]>([]);
const resumo = reactive({ total: 0, ativos: 0, inativos: 0, ganhos: 0 });
const redeFiltrada = computed(() =>
  rede.value.filter(m => {
    const q = busca.value.toLowerCase();
    return !q || (m.nome || m.username || '').toLowerCase().includes(q) || (m.login || m.email || '').toLowerCase().includes(q);
  })
);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
onMounted(async () => {
  agenciaStore.loadFromStorage();
  const user = agenciaStore.dadosUser as Record<string, unknown> | null;
  const userId = user?.Id ?? user?.id ?? user?.IdUsuario ?? user?.idUsuario ?? '';
  try {
    const { data } = await api.get(`/Rede/obterDiretos/${userId}`, authHeader());
    const lista = Array.isArray(data) ? data : [];
    rede.value = lista.map((m: Record<string, unknown>) => ({
      nome: (m.Nome ?? m.nome ?? '') as string,
      login: (m.Login ?? m.login ?? m.Email ?? m.email ?? '') as string,
      nivel: 1,
      dataCadastro: (m.DataCadastro ?? m.dataCadastro ?? '') as string,
      ativo: !!(m.AssinaturaHabilitada ?? m.ativo ?? m.Ativo),
    }));
    resumo.total = rede.value.length;
    resumo.ativos = rede.value.filter(m => m.ativo).length;
    resumo.inativos = resumo.total - resumo.ativos;
  } catch { /**/ } finally { loading.value = false; }
});
</script>

<style scoped>
/* KPIs */
.mr-kpi-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
  gap: .875rem;
  margin-bottom: 1.25rem;
}
.mr-kpi {
  background: #fff;
  border-radius: var(--qs-radius-md, 12px);
  box-shadow: var(--qs-shadow-sm, 0 2px 8px rgba(1,15,28,.08));
  padding: 1.125rem 1rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: .4rem;
  text-align: center;
  transition: all .2s;
}
.mr-kpi:hover { box-shadow: var(--qs-shadow-md); transform: translateY(-2px); }
.mr-kpi__icon {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: .2rem;
}
.mr-kpi__icon svg { width: 20px; height: 20px; color: #fff; }
.mr-kpi__icon--teal { background: var(--qs-teal, #2F7785); }
.mr-kpi__icon--lime { background: var(--qs-lime, #98C73A); }
.mr-kpi__icon--gray { background: var(--qs-gray-400, #9ca3af); }
.mr-kpi__icon--gold { background: #FFB342; }
.mr-kpi__valor {
  font-size: 1.625rem;
  font-weight: 800;
  color: var(--qs-ink, #1d1d1f);
  letter-spacing: -0.02em;
  line-height: 1;
}
.mr-kpi__valor--lime { color: var(--qs-lime-dark, #7aad1f); }
.mr-kpi__valor--teal { color: var(--qs-teal, #2F7785); }
.mr-kpi__label {
  font-size: .625rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .07em;
  color: var(--qs-gray-400, #9ca3af);
}

/* Card */
.mr-card { background: #fff; }
.mr-card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: .875rem;
  margin-bottom: 1rem;
}
.mr-search-wrap {
  display: flex;
  align-items: center;
  gap: .5rem;
  background: var(--qs-gray-50, #fafafa);
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  padding: .4rem .75rem;
  max-width: 340px;
  width: 100%;
}
.mr-search-wrap svg { width: 16px; height: 16px; color: var(--qs-gray-400, #9ca3af); flex-shrink: 0; }
.mr-search {
  border: none;
  background: transparent;
  font-size: .875rem;
  color: var(--qs-ink, #1d1d1f);
  width: 100%;
  outline: none;
}

/* Table */
.mr-table-wrap { overflow-x: auto; }
.qs-table { width: 100%; border-collapse: collapse; font-size: .875rem; }
.qs-table thead tr { border-bottom: 2px solid var(--qs-gray-100, #f5f5f7); }
.qs-table th {
  padding: .625rem .875rem;
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .06em;
  color: var(--qs-gray-500, #6b7280);
  white-space: nowrap;
}
.qs-table td {
  padding: .75rem .875rem;
  color: var(--qs-gray-700, #374151);
  border-bottom: 1px solid var(--qs-gray-100, #f5f5f7);
}
.qs-table tbody tr:hover td { background: var(--qs-gray-50, #fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }
.mr-login { color: var(--qs-gray-400, #9ca3af); font-size: .8125rem; }

/* Member row */
.mr-member { display: flex; align-items: center; gap: .625rem; }
.mr-avatar {
  width: 30px;
  height: 30px;
  border-radius: 50%;
  background: var(--qs-gradient-btn, linear-gradient(135deg, #225F6B, #2F7785));
  color: #fff;
  font-size: .75rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

/* Nivel badge */
.mr-nivel-badge {
  display: inline-flex;
  padding: .2rem .5rem;
  background: #dbeafe;
  color: #1d4ed8;
  border-radius: var(--qs-radius-pill, 999px);
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
}

/* Badges */
.qs-badge {
  display: inline-flex;
  padding: .2rem .55rem;
  border-radius: var(--qs-radius-pill, 999px);
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .04em;
  white-space: nowrap;
}
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--secondary { background: var(--qs-gray-100, #f5f5f7); color: var(--qs-gray-500, #6b7280); }
</style>
