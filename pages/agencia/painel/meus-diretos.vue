<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <div class="qs-page-header">
          <div>
            <div class="qs-eyebrow">Rede</div>
            <h1>Meus Diretos</h1>
            <p>Pessoas que você indicou diretamente para a Quanta Shop.</p>
          </div>
        </div>

        <!-- Link -->
        <div class="qs-card-section dir-link-card">
          <div class="dir-link-label">Seu link de indicação</div>
          <div class="dir-link-row">
            <code class="dir-link-code">{{ linkIndicacao }}</code>
            <button class="qs-btn-secondary dir-copy-btn" @click="copiar">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 1H4c-1.1 0-2 .9-2 2v14h2V3h12V1zm3 4H8c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h11c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2zm0 16H8V7h11v14z"/></svg>
              {{ copiado ? '✓ Copiado!' : 'Copiar' }}
            </button>
          </div>
        </div>

        <!-- Tabela -->
        <div class="qs-card-section dir-card">
          <div class="dir-results-header">
            <div class="qs-section-title">Diretos</div>
            <span class="dir-count-badge">{{ diretos.length }} pessoa(s)</span>
          </div>

          <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>
          <div v-else-if="diretos.length === 0" class="ag-empty-state" style="min-height:120px;">
            <h5>Nenhum direto encontrado</h5>
            <p>Compartilhe seu link e comece a construir sua rede.</p>
          </div>
          <div v-else class="dir-table-wrap">
            <table class="qs-table">
              <thead>
                <tr>
                  <th>Nome</th>
                  <th>Login</th>
                  <th class="tc">Cadastro</th>
                  <th class="tc">Status</th>
                  <th class="tr">Cashback gerado</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(d, i) in diretos" :key="i">
                  <td>
                    <div class="dir-member">
                      <div class="dir-avatar">{{ (d.nome || d.username || '?').charAt(0).toUpperCase() }}</div>
                      {{ d.nome || d.username }}
                    </div>
                  </td>
                  <td class="dir-login">{{ d.login }}</td>
                  <td class="tc">{{ formatDate(d.dataCadastro) }}</td>
                  <td class="tc">
                    <span class="qs-badge" :class="d.ativo ? 'qs-badge--success' : 'qs-badge--secondary'">
                      {{ d.ativo ? 'Ativo' : 'Inativo' }}
                    </span>
                  </td>
                  <td class="tr dir-cashback">{{ formatCurrency(d.cashbackGerado || 0) }}</td>
                </tr>
              </tbody>
            </table>
          </div>
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
import type { Direto } from "~/types/agencia";
const diretos = ref<Direto[]>([]);
const copiado = ref(false);
const linkIndicacao = computed(() => `https://quantashop.com.br/register/${agenciaStore.dadosUser?.login || ''}`);
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function formatCurrency(v: number) { return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v); }
async function copiar() {
  try { await navigator.clipboard.writeText(linkIndicacao.value); copiado.value = true; setTimeout(() => { copiado.value = false; }, 2000); } catch { /**/ }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/user/diretosUsaurioLogado', authHeader());
    const lista = Array.isArray(data) ? data : [];
    diretos.value = lista.map((u: Record<string, unknown>) => ({
      id: u.IdUsuario ?? u.idUsuario ?? 0,
      nome: (u.Nome ?? u.nome ?? '') as string,
      login: (u.Login ?? u.login ?? '') as string,
      status: (u.produtoAtivo ?? '') as string,
      dataCadastro: (u.DataCadastro ?? u.dataCadastro ?? '') as string,
      ativo: true,
      cashbackGerado: 0,
    }));
  } catch { /**/ } finally { loading.value = false; }
});
</script>

<style scoped>
.dir-link-card { background: #fff; }
.dir-link-label { font-size: .75rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-500,#6b7280); margin-bottom: .625rem; }
.dir-link-row { display: flex; align-items: center; gap: .75rem; flex-wrap: wrap; }
.dir-link-code { font-size: .8125rem; color: var(--qs-teal-dark,#225F6B); background: var(--qs-gray-50,#fafafa); border: 1px solid var(--qs-gray-200,#e5e7eb); border-radius: var(--qs-radius-md,12px); padding: .5rem .875rem; flex: 1; word-break: break-all; font-family: monospace; }
.dir-copy-btn { flex-shrink: 0; display: inline-flex; align-items: center; gap: .35rem; font-size: .8125rem; }
.dir-copy-btn svg { width: 14px; height: 14px; }

.dir-card { background: #fff; }
.dir-results-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 1rem; flex-wrap: wrap; gap: .5rem; }
.dir-count-badge { font-size: .75rem; font-weight: 600; background: var(--qs-gray-100,#f5f5f7); color: var(--qs-gray-500,#6b7280); padding: .2rem .625rem; border-radius: var(--qs-radius-pill,999px); }
.dir-table-wrap { overflow-x: auto; }

.qs-table { width: 100%; border-collapse: collapse; font-size: .875rem; }
.qs-table thead tr { border-bottom: 2px solid var(--qs-gray-100,#f5f5f7); }
.qs-table th { padding: .625rem .875rem; font-size: .6875rem; font-weight: 700; text-transform: uppercase; letter-spacing: .06em; color: var(--qs-gray-500,#6b7280); white-space: nowrap; }
.qs-table td { padding: .75rem .875rem; color: var(--qs-gray-700,#374151); border-bottom: 1px solid var(--qs-gray-100,#f5f5f7); }
.qs-table tbody tr:hover td { background: var(--qs-gray-50,#fafafa); }
.qs-table tbody tr:last-child td { border-bottom: none; }
.tc { text-align: center !important; }
.tr { text-align: right !important; }
.dir-login { color: var(--qs-gray-400,#9ca3af); font-size: .8125rem; }
.dir-cashback { color: var(--qs-lime-dark,#7aad1f) !important; font-weight: 700; }

.dir-member { display: flex; align-items: center; gap: .625rem; }
.dir-avatar { width: 30px; height: 30px; border-radius: 50%; background: var(--qs-gradient-btn,linear-gradient(135deg,#225F6B,#2F7785)); color: #fff; font-size: .75rem; font-weight: 700; display: flex; align-items: center; justify-content: center; flex-shrink: 0; }

.qs-badge { display: inline-flex; padding: .2rem .55rem; border-radius: var(--qs-radius-pill,999px); font-size: .6875rem; font-weight: 700; text-transform: uppercase; white-space: nowrap; }
.qs-badge--success { background: #dcfce7; color: #16a34a; }
.qs-badge--secondary { background: var(--qs-gray-100,#f5f5f7); color: var(--qs-gray-500,#6b7280); }
</style>
