<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Usuários</h1><p>Gerenciar usuários da plataforma</p></div>
      <div class="d-flex gap-2">
        <input v-model="busca" type="text" class="form-control" style="max-width:240px" placeholder="Buscar por nome ou e-mail..." @keyup.enter="buscarUsuarios" />
        <button class="btn btn-ag-primary" @click="buscarUsuarios">Buscar</button>
      </div>
    </div>
    <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>
    <div v-else class="ag-card">
      <div v-if="itens.length === 0" class="ag-empty-state"><h5>Nenhum usuário encontrado</h5></div>
      <div v-else class="table-responsive">
        <table class="table ag-table">
          <thead><tr><th>Nome</th><th>E-mail</th><th>Login</th><th>Perfil</th><th>Status</th><th></th></tr></thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="fw-bold">{{ item.username || item.login }}</td>
              <td><span :title="isMaster ? item.email : 'Mascarado por LGPD'">{{ maskEmail(item.email) }}</span></td>
              <td>{{ item.login }}</td>
              <td>{{ item.perfil || '—' }}</td>
              <td><span class="badge-ag" :class="item.status === 'Ativo' ? 'badge-ag-success' : 'badge-ag-warning'">{{ item.status || 'Ativo' }}</span></td>
              <td><button class="btn btn-sm btn-ag-outline" @click="verUsuario(item)">Ver</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-if="showModal && selecionado" class="ag-modal-overlay" @click.self="fecharModal">
      <div class="ag-modal" style="max-width:540px">
        <div class="ag-modal-header"><h5 class="mb-0">Detalhes do Usuário</h5><button class="btn-close" @click="fecharModal" /></div>
        <div class="ag-modal-body">
          <div class="row g-3">
            <div class="col-6"><strong>Nome:</strong><br/>{{ selecionado.username }}</div>
            <div class="col-6"><strong>Login:</strong><br/>{{ selecionado.login }}</div>
            <div class="col-12">
              <strong>E-mail:</strong>
              <button v-if="isMaster" type="button" class="btn btn-sm btn-link p-0 ms-2" @click="revelar('email')">
                {{ revelados.email ? 'Ocultar' : 'Revelar' }}
              </button>
              <span v-else class="text-muted small ms-2" title="Apenas Master pode revelar">🔒 mascarado</span>
              <br/>
              <span class="font-monospace">{{ revelados.email ? selecionado.email : maskEmail(selecionado.email) }}</span>
            </div>
            <div class="col-6"><strong>Perfil:</strong><br/>{{ selecionado.perfil || '—' }}</div>
            <div class="col-6"><strong>Status:</strong><br/>{{ selecionado.status || 'Ativo' }}</div>
          </div>
          <div class="alert alert-info py-2 mt-3 mb-0" v-if="isMaster">
            <small>🔓 Você é Master. Cada revelação é registrada na auditoria LGPD.</small>
          </div>
          <div v-if="revelarError" class="alert alert-warning py-2 mt-3 mb-0">{{ revelarError }}</div>
          <div class="mt-4 d-flex gap-2 flex-wrap">
            <button class="btn btn-ag-outline btn-sm" @click="alterarStatus(selecionado, 'Ativo')">Ativar</button>
            <button class="btn btn-outline-warning btn-sm" @click="alterarStatus(selecionado, 'Inativo')">Desativar</button>
            <button class="btn btn-outline-danger btn-sm" @click="alterarStatus(selecionado, 'Bloqueado')">Bloquear</button>
          </div>
          <div v-if="modalError" class="alert alert-danger py-2 mt-3">{{ modalError }}</div>
        </div>
        <div class="ag-modal-footer"><button class="btn btn-secondary" @click="fecharModal">Fechar</button></div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { UsuarioAdmin } from '~/types/agencia';
import { maskEmail } from '~/utils/lgpd-mask';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<UsuarioAdmin[]>([]);
const showModal = ref(false);
const modalError = ref('');
const busca = ref('');
const selecionado = ref<UsuarioAdmin | null>(null);
const revelarError = ref('');
const revelados = reactive<{ email: boolean; cpf: boolean; telefone: boolean }>({ email: false, cpf: false, telefone: false });

const isMaster = computed<boolean>(() => {
  const adm: any = agenciaStore.userAdmin || {};
  return adm.master === true || adm.Master === true || adm.perfil === 'Master';
});

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
function verUsuario(item: UsuarioAdmin) {
  selecionado.value = item;
  modalError.value = '';
  revelarError.value = '';
  revelados.email = false;
  revelados.cpf = false;
  revelados.telefone = false;
  showModal.value = true;
}
function fecharModal() { showModal.value = false; selecionado.value = null; }
async function buscarUsuarios() {
  loading.value = true;
  try {
    const params = busca.value ? `?busca=${encodeURIComponent(busca.value)}` : '';
    const { data } = await api.get(`/admin/usuarios/listar${params}`, authHeader());
    itens.value = Array.isArray(data) ? data : (data?.items || []);
  } catch (e: unknown) { console.error('Erro ao buscar usuários:', extractApiErrorMessage(e)); }
  finally { loading.value = false; }
}
async function alterarStatus(item: UsuarioAdmin, novoStatus: string) {
  modalError.value = '';
  try {
    await api.put(`/admin/usuarios/${item.id}/status`, { status: novoStatus }, authHeader());
    const idx = itens.value.findIndex(u => u.id === item.id);
    if (idx !== -1) itens.value[idx] = { ...itens.value[idx], status: novoStatus };
    if (selecionado.value?.id === item.id) selecionado.value = { ...selecionado.value, status: novoStatus };
  } catch (e: unknown) { modalError.value = extractApiErrorMessage(e, 'Erro ao alterar status.'); }
}
async function revelar(campo: 'email' | 'cpf' | 'telefone') {
  if (!selecionado.value) return;
  if (revelados[campo]) { revelados[campo] = false; return; }
  if (!isMaster.value) {
    revelarError.value = 'Apenas usuários Master podem revelar dados sensíveis.';
    return;
  }
  revelarError.value = '';
  try {
    await api.post('/admin/revelar-dado-sensivel', {
      idUsuarioAlvo: selecionado.value.id,
      campo,
      motivo: 'Consulta admin painel',
    }, authHeader());
    revelados[campo] = true;
  } catch (e: any) {
    revelarError.value = extractApiErrorMessage(e, 'Falha ao registrar/autorizar revelação.');
  }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  await buscarUsuarios();
});
</script>
