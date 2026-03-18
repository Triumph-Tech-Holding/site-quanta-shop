<template>
  <div>
    <div class="ag-page-header">
      <h1>Minha Rede</h1>
      <p>Visualize e gerencie sua rede de indicados</p>
    </div>

    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

    <template v-else>
      <div class="row g-3 mb-4">
        <div class="col-12 col-md-3">
          <div class="ag-stat-card">
            <div class="stat-icon">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-8 0c1.66 0 2.99-1.34 2.99-3S9.66 5 8 5C6.34 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
            </div>
            <div><div class="stat-label">Total na rede</div><div class="stat-value">{{ resumo.total }}</div></div>
          </div>
        </div>
        <div class="col-12 col-md-3">
          <div class="ag-stat-card">
            <div class="stat-icon"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/></svg></div>
            <div><div class="stat-label">Ativos</div><div class="stat-value text-ag-secondary">{{ resumo.ativos }}</div></div>
          </div>
        </div>
        <div class="col-12 col-md-3">
          <div class="ag-stat-card">
            <div class="stat-icon"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg></div>
            <div><div class="stat-label">Inativos</div><div class="stat-value">{{ resumo.inativos }}</div></div>
          </div>
        </div>
        <div class="col-12 col-md-3">
          <div class="ag-stat-card">
            <div class="stat-icon"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg></div>
            <div><div class="stat-label">Ganhos da rede</div><div class="stat-value text-ag-primary">{{ formatCurrency(resumo.ganhos) }}</div></div>
          </div>
        </div>
      </div>

      <div class="ag-card">
        <div class="ag-card-title">Membros da Rede</div>
        <div class="mb-3">
          <input v-model="busca" type="text" class="form-control" placeholder="Buscar por nome ou e-mail..." style="max-width:360px" />
        </div>
        <div v-if="redeFiltrada.length === 0" class="ag-empty-state">
          <h5>Nenhum membro encontrado</h5>
        </div>
        <div v-else class="table-responsive">
          <table class="table ag-table">
            <thead><tr><th>Nome</th><th>Login</th><th>Nível</th><th>Cadastro</th><th>Status</th></tr></thead>
            <tbody>
              <tr v-for="(m, i) in redeFiltrada" :key="i">
                <td>{{ m.nome || m.username }}</td>
                <td class="text-muted">{{ m.login || m.email }}</td>
                <td><span class="badge-ag badge-ag-info">N{{ m.nivel || 1 }}</span></td>
                <td>{{ formatDate(m.dataCadastro || m.createdAt) }}</td>
                <td>
                  <span class="badge-ag" :class="m.ativo ? 'badge-ag-success' : 'badge-ag-secondary'">
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
