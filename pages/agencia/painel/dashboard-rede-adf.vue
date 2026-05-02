<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <!-- Page Header -->
        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">ADF — Agente de Fidelização</div>
            <h1>Dashboard de Rede e Leads</h1>
            <p>Acompanhe seus leads atribuídos, cliques nos links de indicação e o cashback residual gerado pela sua rede.</p>
          </div>
          <NuxtLink to="/agencia/painel/social-commerce" class="qs-btn-primary">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M18 16.08c-.76 0-1.44.3-1.96.77L8.91 12.7c.05-.23.09-.46.09-.7s-.04-.47-.09-.7l7.05-4.11c.54.5 1.25.81 2.04.81 1.66 0 3-1.34 3-3s-1.34-3-3-3-3 1.34-3 3c0 .24.04.47.09.7L8.04 9.81C7.5 9.31 6.79 9 6 9c-1.66 0-3 1.34-3 3s1.34 3 3 3c.79 0 1.5-.31 2.04-.81l7.12 4.16c-.05.21-.08.43-.08.65 0 1.61 1.31 2.92 2.92 2.92s2.92-1.31 2.92-2.92-1.31-2.92-2.92-2.92z"/></svg>
            Social Commerce
          </NuxtLink>
        </div>

        <!-- Paywall HAF -->
        <template v-if="!hasHaf">
          <PaywallHaf />
        </template>

        <template v-else>
          <!-- KPI Cards -->
          <div v-if="carregando" class="qs-loading"><div class="qs-spinner" /></div>

          <template v-else>
            <div class="qs-grid adf-kpi-grid">
              <!-- Novos Leads -->
              <div class="adf-kpi-card">
                <div class="adf-kpi-card__icon adf-kpi-card__icon--teal">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M15 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm-9-2V7H4v3H1v2h3v3h2v-3h3v-2H6zm9 4c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z"/></svg>
                </div>
                <div class="adf-kpi-card__content">
                  <div class="adf-kpi-card__label">Novos Leads</div>
                  <div class="adf-kpi-card__valor">{{ kpis.novosLeads }}</div>
                  <div class="adf-kpi-card__delta" :class="kpis.novosLeadsDelta >= 0 ? 'adf-kpi-card__delta--up' : 'adf-kpi-card__delta--down'">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
                      <path v-if="kpis.novosLeadsDelta >= 0" d="M7 14l5-5 5 5z"/>
                      <path v-else d="M7 10l5 5 5-5z"/>
                    </svg>
                    {{ Math.abs(kpis.novosLeadsDelta) }}% vs mês anterior
                  </div>
                </div>
              </div>

              <!-- Cliques no Link -->
              <div class="adf-kpi-card">
                <div class="adf-kpi-card__icon adf-kpi-card__icon--lime">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
                </div>
                <div class="adf-kpi-card__content">
                  <div class="adf-kpi-card__label">Cliques no Link</div>
                  <div class="adf-kpi-card__valor">{{ kpis.cliquesLink.toLocaleString('pt-BR') }}</div>
                  <div class="adf-kpi-card__delta" :class="kpis.cliquesDelta >= 0 ? 'adf-kpi-card__delta--up' : 'adf-kpi-card__delta--down'">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
                      <path v-if="kpis.cliquesDelta >= 0" d="M7 14l5-5 5 5z"/>
                      <path v-else d="M7 10l5 5 5-5z"/>
                    </svg>
                    {{ Math.abs(kpis.cliquesDelta) }}% vs mês anterior
                  </div>
                </div>
              </div>

              <!-- Cashback Residual Gerado -->
              <div class="adf-kpi-card">
                <div class="adf-kpi-card__icon adf-kpi-card__icon--gold">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
                </div>
                <div class="adf-kpi-card__content">
                  <div class="adf-kpi-card__label">Cashback Residual Gerado</div>
                  <div class="adf-kpi-card__valor adf-kpi-card__valor--lime">{{ formatCurrency(kpis.cashbackResidual) }}</div>
                  <div class="adf-kpi-card__delta adf-kpi-card__delta--up">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M7 14l5-5 5 5z"/></svg>
                    Este mês
                  </div>
                </div>
              </div>
            </div>

            <!-- Lista de Leads Recentes -->
            <div class="qs-card-section">
              <div class="adf-leads-header">
                <div>
                  <div class="qs-section-title">Leads Recentes</div>
                  <div class="qs-section-desc">Membros que clicaram no seu link e se cadastraram</div>
                </div>
                <span class="adf-leads-count">{{ leads.length }} leads</span>
              </div>

              <div v-if="leads.length === 0" class="ag-empty-state" style="min-height:100px;">
                <h5>Nenhum lead registrado ainda</h5>
                <p>Compartilhe seu link via Social Commerce para atrair leads.</p>
              </div>

              <div v-else class="adf-leads-list">
                <div v-for="lead in leads" :key="lead.id" class="adf-lead-item">
                  <div class="adf-lead-avatar">{{ lead.nome.charAt(0).toUpperCase() }}</div>
                  <div class="adf-lead-info">
                    <div class="adf-lead-nome">{{ lead.nome }}</div>
                    <div class="adf-lead-meta">via {{ lead.parceiro }} · {{ lead.data }}</div>
                  </div>
                  <div class="adf-lead-status">
                    <span class="adf-lead-badge" :class="`adf-lead-badge--${lead.status}`">
                      {{ statusLabel(lead.status) }}
                    </span>
                  </div>
                  <div class="adf-lead-cashback">
                    <div class="adf-lead-cashback-label">Residual</div>
                    <div class="adf-lead-cashback-valor">{{ lead.cashback > 0 ? formatCurrency(lead.cashback) : '—' }}</div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Plus Módulo -->
            <PlusModulo class="mt-3" />
          </template>
        </template>

      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAgenciaStore } from '~/pinia/useAgenciaStore';
import { useCmsStore } from '~/pinia/useCmsStore';

definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const cmsStore = useCmsStore();
const api = useApi();

const hasHaf = computed(() => {
  const u = agenciaStore.user as any;
  return !!(u?.hafAtiva || u?.licencaAtiva || u?.admin);
});

const carregando = ref(true);

interface Kpis {
  novosLeads: number;
  novosLeadsDelta: number;
  cliquesLink: number;
  cliquesDelta: number;
  cashbackResidual: number;
}

const kpis = ref<Kpis>({
  novosLeads: 0,
  novosLeadsDelta: 0,
  cliquesLink: 0,
  cliquesDelta: 0,
  cashbackResidual: 0,
});

interface Lead {
  id: string;
  nome: string;
  parceiro: string;
  data: string;
  status: 'ativo' | 'pendente' | 'inativo';
  cashback: number;
}
const leads = ref<Lead[]>([]);

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v);
}

function statusLabel(s: string): string {
  return { ativo: 'Ativo', pendente: 'Pendente', inativo: 'Inativo' }[s] ?? s;
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await cmsStore.loadConfig();
  cmsStore.applyBrandCssVars();

  try {
    const data = await api.get('/Usuario/DashboardAdf').catch(() => null) as any;
    if (data) {
      kpis.value = {
        novosLeads: Number(data.novosLeads ?? 0),
        novosLeadsDelta: Number(data.novosLeadsDelta ?? 0),
        cliquesLink: Number(data.cliquesLink ?? 0),
        cliquesDelta: Number(data.cliquesDelta ?? 0),
        cashbackResidual: Number(data.cashbackResidual ?? 0),
      };
      leads.value = data.leads ?? [];
    }
  } catch { /* usa mock abaixo */ }

  if (kpis.value.novosLeads === 0) {
    kpis.value = { novosLeads: 12, novosLeadsDelta: 18, cliquesLink: 347, cliquesDelta: 23, cashbackResidual: 84.50 };
    leads.value = [
      { id: '1', nome: 'Ana Lima', parceiro: 'Amazon', data: '02/05/2026', status: 'ativo', cashback: 12.40 },
      { id: '2', nome: 'Bruno Ferreira', parceiro: 'iFood', data: '01/05/2026', status: 'pendente', cashback: 0 },
      { id: '3', nome: 'Carla Souza', parceiro: 'Magalu', data: '30/04/2026', status: 'ativo', cashback: 8.75 },
      { id: '4', nome: 'Diego Martins', parceiro: 'Netshoes', data: '29/04/2026', status: 'inativo', cashback: 0 },
      { id: '5', nome: 'Elaine Costa', parceiro: 'Amazon', data: '28/04/2026', status: 'ativo', cashback: 21.30 },
    ];
  }

  carregando.value = false;
});
</script>

<style scoped>
/* KPI Grid */
.adf-kpi-grid {
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
}

.adf-kpi-card {
  background: #fff;
  border-radius: var(--qs-radius-md, 12px);
  box-shadow: var(--qs-shadow-sm, 0 2px 8px rgba(1,15,28,.08));
  padding: 1.25rem;
  display: flex;
  align-items: flex-start;
  gap: 1rem;
  transition: all .2s;
}
.adf-kpi-card:hover {
  box-shadow: var(--qs-shadow-md, 0 4px 20px rgba(1,15,28,.12));
  transform: translateY(-2px);
}

.adf-kpi-card__icon {
  width: 44px;
  height: 44px;
  border-radius: 11px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  color: #fff;
}
.adf-kpi-card__icon svg { width: 22px; height: 22px; }
.adf-kpi-card__icon--teal { background: var(--qs-teal, #2F7785); }
.adf-kpi-card__icon--lime { background: var(--qs-lime, #98C73A); }
.adf-kpi-card__icon--gold { background: #FFB342; }

.adf-kpi-card__content { flex: 1; min-width: 0; }

.adf-kpi-card__label {
  font-size: .6875rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: .06em;
  color: var(--qs-gray-400, #9ca3af);
  margin-bottom: .25rem;
}
.adf-kpi-card__valor {
  font-size: 1.625rem;
  font-weight: 700;
  color: var(--qs-ink, #1d1d1f);
  letter-spacing: -0.02em;
  line-height: 1.1;
  margin-bottom: .3rem;
}
.adf-kpi-card__valor--lime { color: var(--qs-lime-dark, #7aad1f); }

.adf-kpi-card__delta {
  display: flex;
  align-items: center;
  gap: .2rem;
  font-size: .75rem;
  font-weight: 500;
}
.adf-kpi-card__delta svg { width: 14px; height: 14px; }
.adf-kpi-card__delta--up { color: var(--qs-success, #16a34a); }
.adf-kpi-card__delta--down { color: var(--qs-danger, #dc2626); }

/* Leads */
.adf-leads-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-bottom: 1rem;
  flex-wrap: wrap;
  gap: .5rem;
}
.adf-leads-count {
  font-size: .75rem;
  font-weight: 700;
  background: var(--qs-teal, #2F7785);
  color: #fff;
  padding: .2rem .625rem;
  border-radius: var(--qs-radius-pill, 999px);
}

.adf-leads-list {
  display: flex;
  flex-direction: column;
  gap: .5rem;
}

.adf-lead-item {
  display: grid;
  grid-template-columns: auto 1fr auto auto;
  align-items: center;
  gap: .75rem;
  padding: .75rem .875rem;
  background: var(--qs-gray-50, #fafafa);
  border-radius: var(--qs-radius-md, 12px);
  transition: background .15s;
}
.adf-lead-item:hover { background: #fff; box-shadow: var(--qs-shadow-xs, 0 1px 3px rgba(1,15,28,.06)); }

.adf-lead-avatar {
  width: 36px;
  height: 36px;
  background: var(--qs-teal, #2F7785);
  color: #fff;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: .875rem;
  font-weight: 700;
  flex-shrink: 0;
}

.adf-lead-nome {
  font-size: .875rem;
  font-weight: 600;
  color: var(--qs-ink, #1d1d1f);
}
.adf-lead-meta {
  font-size: .75rem;
  color: var(--qs-gray-400, #9ca3af);
  margin-top: .1rem;
}

.adf-lead-badge {
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .05em;
  padding: .2rem .5rem;
  border-radius: var(--qs-radius-pill, 999px);
  white-space: nowrap;
}
.adf-lead-badge--ativo { background: #dcfce7; color: #16a34a; }
.adf-lead-badge--pendente { background: #fef9c3; color: #ca8a04; }
.adf-lead-badge--inativo { background: var(--qs-gray-100, #f5f5f7); color: var(--qs-gray-500, #6b7280); }

.adf-lead-cashback { text-align: right; }
.adf-lead-cashback-label {
  font-size: .6875rem;
  color: var(--qs-gray-400, #9ca3af);
  text-transform: uppercase;
  letter-spacing: .04em;
}
.adf-lead-cashback-valor {
  font-size: .875rem;
  font-weight: 700;
  color: var(--qs-lime-dark, #7aad1f);
}

@media (max-width: 480px) {
  .adf-lead-item {
    grid-template-columns: auto 1fr;
    grid-template-rows: auto auto;
  }
  .adf-lead-status { grid-column: 2; }
  .adf-lead-cashback { grid-column: 1 / -1; text-align: left; padding-left: calc(36px + .75rem); }
}

.mt-3 { margin-top: 1rem; }
</style>
