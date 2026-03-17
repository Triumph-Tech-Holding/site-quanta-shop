<template>
  <div>
    <div class="ag-page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
      <div><h1>Alterar Dados de Usuário</h1><p>Editar informações de usuários da plataforma</p></div>
    </div>
    <div class="ag-card mb-3">
      <div class="d-flex gap-2">
        <input v-model="busca" type="text" class="form-control" placeholder="Login, e-mail ou nome..." @keyup.enter="buscarUsuario" />
        <button class="btn btn-ag-primary" :disabled="buscando" @click="buscarUsuario">{{ buscando ? 'Buscando...' : 'Buscar' }}</button>
      </div>
    </div>
    <div v-if="usuarioSelecionado" class="ag-card">
      <div class="ag-card-title mb-3">Editando: {{ usuarioSelecionado.username || usuarioSelecionado.login }}</div>
      <div class="row g-3">
        <div class="col-md-6"><label class="form-label fw-bold">Nome</label><input v-model="form.username" type="text" class="form-control" /></div>
        <div class="col-md-6"><label class="form-label fw-bold">E-mail</label><input v-model="form.email" type="email" class="form-control" /></div>
        <div class="col-md-6"><label class="form-label fw-bold">Login</label><input v-model="form.login" type="text" class="form-control" /></div>
        <div class="col-md-6"><label class="form-label fw-bold">Perfil</label><input v-model="form.perfil" type="text" class="form-control" /></div>
        <div class="col-12"><label class="form-label fw-bold">Nova Senha <span class="text-muted fw-normal">(deixe em branco para não alterar)</span></label><input v-model="form.senha" type="password" class="form-control" autocomplete="new-password" /></div>
      </div>
      <div v-if="erroSalvar" class="alert alert-danger mt-3 py-2">{{ erroSalvar }}</div>
      <div v-if="sucesso" class="alert alert-success mt-3 py-2">Dados atualizados com sucesso!</div>
      <div class="mt-3 d-flex gap-2">
        <button class="btn btn-ag-primary" :disabled="salvando" @click="salvarAlteracoes">{{ salvando ? 'Salvando...' : 'Salvar Alterações' }}</button>
        <button class="btn btn-secondary" @click="usuarioSelecionado = null; sucesso = false;">Cancelar</button>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { UsuarioAdmin } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
const agenciaStore = useAgenciaStore();
const api = useApi();
const busca = ref('');
const buscando = ref(false);
const salvando = ref(false);
const erroSalvar = ref('');
const sucesso = ref(false);
const usuarioSelecionado = ref<UsuarioAdmin | null>(null);
const form = reactive({ username: '', email: '', login: '', perfil: '', senha: '' });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
async function buscarUsuario() {
  if (!busca.value.trim()) return;
  buscando.value = true;
  try {
    const { data } = await api.get<UsuarioAdmin[]>(`/admin/usuarios/listar?busca=${encodeURIComponent(busca.value)}`, authHeader());
    const lista = Array.isArray(data) ? data : (data as { items?: UsuarioAdmin[] })?.items || [];
    if (lista.length > 0) {
      usuarioSelecionado.value = lista[0];
      Object.assign(form, { username: lista[0].username || '', email: lista[0].email || '', login: lista[0].login || '', perfil: lista[0].perfil || '', senha: '' });
      sucesso.value = false; erroSalvar.value = '';
    } else { erroSalvar.value = 'Usuário não encontrado.'; }
  } catch (e: unknown) { erroSalvar.value = extractApiErrorMessage(e, 'Erro ao buscar usuário.'); }
  finally { buscando.value = false; }
}
async function salvarAlteracoes() {
  if (!usuarioSelecionado.value) return;
  salvando.value = true; erroSalvar.value = ''; sucesso.value = false;
  try {
    const payload: Record<string, string> = { username: form.username, email: form.email, login: form.login, perfil: form.perfil };
    if (form.senha) payload.senha = form.senha;
    await api.put(`/admin/usuarios/${usuarioSelecionado.value.id}`, payload, authHeader());
    sucesso.value = true;
    form.senha = '';
  } catch (e: unknown) { erroSalvar.value = extractApiErrorMessage(e, 'Erro ao salvar alterações.'); }
  finally { salvando.value = false; }
}
onMounted(() => { agenciaStore.loadFromStorage(); });
</script>
