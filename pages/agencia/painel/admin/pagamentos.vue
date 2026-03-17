<template>
  <div>
    <div class="ag-page-header"><h1>Pagamentos</h1><p>Gerenciar pagamentos e saques pendentes</p></div>
    <div class="ag-card mb-3">
      <ul class="nav nav-tabs ag-tabs mb-4">
        <li class="nav-item"><button class="nav-link" :class="{active:tab==='saques'}" @click="tab='saques';loadSaques()">Saques Pendentes</button></li>
        <li class="nav-item"><button class="nav-link" :class="{active:tab==='aprovados'}" @click="tab='aprovados';loadAprovados()">Aprovados</button></li>
        <li class="nav-item"><button class="nav-link" :class="{active:tab==='aguardando'}" @click="tab='aguardando';loadAguardando()">Aguardando Pgto</button></li>
      </ul>
      <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Data</th><th>Usuário</th><th>Valor</th><th>Banco</th><th>Status</th><th>Ação</th></tr></thead>
          <tbody>
            <tr v-for="(p, i) in itens" :key="i">
              <td>{{ formatDate(p.data || p.dataSolicitacao) }}</td>
              <td>{{ p.nome || p.usuario }}</td>
              <td class="fw-bold">{{ formatCurrency(p.valor) }}</td>
              <td>{{ p.banco || '—' }}</td>
              <td><span class="badge-ag" :class="statusClass(p.status)">{{ p.status || 'Pendente' }}</span></td>
              <td>
                <button v-if="tab==='saques'" class="btn btn-sm btn-ag-secondary me-1" @click="aprovar(p)">Aprovar</button>
                <button v-if="tab==='saques'" class="btn btn-sm btn-sm text-danger" @click="recusar(p)">Recusar</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp();
const tab = ref('saques');
const loading = ref(true);
import type { PagamentoAdmin } from "~/types/agencia";
import { extractApiErrorMessage } from "~/types/agencia";
const itens = ref<PagamentoAdmin[]>([]);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0); }
function statusClass(s: string) { return {'Aprovado':'badge-ag-success','Pago':'badge-ag-success','Pendente':'badge-ag-warning','Recusado':'badge-ag-danger'}[s]||'badge-ag-secondary'; }
async function loadSaques() { loading.value=true; try { const {data} = await api.get('/admin/pagamentos/saquesPendentes', authHeader()); itens.value=Array.isArray(data)?data:data?.items||[]; } catch(e: unknown) { console.error(e); } finally{loading.value=false;} }
async function loadAprovados() { loading.value=true; try { const {data} = await api.get('/admin/pagamentos/aprovados', authHeader()); itens.value=Array.isArray(data)?data:data?.items||[]; } catch(e: unknown) { console.error(e); } finally{loading.value=false;} }
async function loadAguardando() { loading.value=true; try { const {data} = await api.get('/admin/pagamentos/aguardandoPagamento', authHeader()); itens.value=Array.isArray(data)?data:data?.items||[]; } catch(e: unknown) { console.error(e); } finally{loading.value=false;} }
async function aprovar(p: PagamentoAdmin) { try { await api.post('/admin/pagamentos/aprovar', { id: p.id }, authHeader()); $toast?.success('Aprovado!'); await loadSaques(); } catch(e: unknown){ $toast?.error(extractApiErrorMessage(e, 'Erro ao processar.')); } }
async function recusar(p: PagamentoAdmin) { if(!confirm('Recusar?')) return; try { await api.post('/admin/pagamentos/recusar', { id: p.id }, authHeader()); $toast?.success('Recusado.'); await loadSaques(); } catch(e: unknown){ $toast?.error(extractApiErrorMessage(e, 'Erro ao processar.')); } }
onMounted(async () => { agenciaStore.loadFromStorage(); if (!agenciaStore.isAdmin) { navigateTo('/agencia/painel'); return; } await loadSaques(); });
</script>
