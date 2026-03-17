<template>
  <div>
    <div class="ag-page-header"><h1>Usuários</h1><p>Gerenciar usuários cadastrados na plataforma</p></div>
    <div class="ag-card mb-3">
      <form @submit.prevent="buscar" class="row g-3 align-items-end">
        <div class="col-12 col-md-4 ag-form-group mb-0"><label>Nome / Login / CPF</label><input v-model="filtro.q" type="text" class="form-control" /></div>
        <div class="col-12 col-md-3 ag-form-group mb-0"><label>Perfil</label><select v-model="filtro.perfil" class="form-select"><option value="">Todos</option><option value="C">Comerciante</option><option value="E">Empreendedor</option><option value="A">Admin</option></select></div>
        <div class="col-12 col-md-2"><button type="submit" class="btn btn-ag-primary w-100">Buscar</button></div>
      </form>
    </div>
    <div class="ag-card">
      <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Nome</th><th>Login</th><th>E-mail</th><th>Perfil</th><th>Cadastro</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="(u, i) in usuarios" :key="i">
              <td>{{ u.nome || u.username }}</td>
              <td>{{ u.login }}</td>
              <td class="text-muted">{{ u.email }}</td>
              <td>{{ u.perfil }}</td>
              <td>{{ formatDate(u.dataCadastro) }}</td>
              <td><span class="badge-ag" :class="u.ativo ? 'badge-ag-success' : 'badge-ag-secondary'">{{ u.ativo ? 'Ativo' : 'Inativo' }}</span></td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verUsuario(u)">Ver</button></td>
            </tr>
          </tbody>
        </table>
        <div class="d-flex gap-2 justify-content-end mt-2">
          <button class="btn btn-ag-outline btn-sm" :disabled="page<=1" @click="changePage(page-1)">← Ant</button>
          <span class="align-self-center">{{ page }}/{{ totalPages }}</span>
          <button class="btn btn-ag-outline btn-sm" :disabled="page>=totalPages" @click="changePage(page+1)">Próx →</button>
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
const usuarios = ref<any[]>([]);
const filtro = reactive({ q: '', perfil: '' });
const page = ref(1);
const total = ref(0);
const pageSize = 20;
const totalPages = computed(() => Math.max(1, Math.ceil(total.value / pageSize)));
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function formatDate(d: string) { return d ? new Date(d).toLocaleDateString('pt-BR') : '—'; }
function verUsuario(u: any) { navigateTo(`/agencia/painel/admin/alterar-dados-usuario?id=${u.id || u.idUsuario}`); }
async function buscar() { page.value = 1; await load(); }
async function changePage(p: number) { page.value = p; await load(); }
async function load() {
  loading.value = true;
  try {
    const params = new URLSearchParams({ page: String(page.value), pageSize: String(pageSize), ...(filtro.q && { q: filtro.q }), ...(filtro.perfil && { perfil: filtro.perfil }) });
    const { data } = await api.get(`/admin/usuarios/listar?${params}`, authHeader());
    usuarios.value = Array.isArray(data) ? data : (data?.items || data?.usuarios || []);
    total.value = data?.total || usuarios.value.length;
  } catch { /**/ } finally { loading.value = false; }
}
onMounted(async () => { agenciaStore.loadFromStorage(); if (!agenciaStore.isAdmin) { navigateTo('/agencia/painel'); return; } await load(); });
</script>
