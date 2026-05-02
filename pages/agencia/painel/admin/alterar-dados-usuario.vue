<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Usuários</div>
        <h1>Alterar Dados de Usuário</h1>
        <p>Editar informações de usuários da plataforma</p>
      </div>
    </div>

    <div class="qs-card-section qs-search-card">
      <div class="qs-search-row">
        <input v-model="busca" type="text" class="qs-input" placeholder="Login, e-mail ou nome..." @keyup.enter="buscarUsuario" />
        <button class="qs-btn-primary" :disabled="buscando" @click="buscarUsuario">
          <span v-if="buscando" class="qs-spinner-sm" />
          {{ buscando ? 'Buscando...' : 'Buscar' }}
        </button>
      </div>
      <div v-if="erroSalvar && !usuarioSelecionado" class="qs-alert-danger">{{ erroSalvar }}</div>
    </div>

    <div v-if="usuarioSelecionado" class="qs-card-section">
      <div class="qs-section-title" style="margin-bottom:20px">
        Editando: <strong>{{ usuarioSelecionado.username || usuarioSelecionado.login }}</strong>
      </div>
      <div class="qs-form-grid">
        <div class="qs-field">
          <label class="qs-label">Nome</label>
          <input v-model="form.username" type="text" class="qs-input" />
        </div>
        <div class="qs-field">
          <label class="qs-label">E-mail</label>
          <input v-model="form.email" type="email" class="qs-input" />
        </div>
        <div class="qs-field">
          <label class="qs-label">Login</label>
          <input v-model="form.login" type="text" class="qs-input" />
        </div>
        <div class="qs-field">
          <label class="qs-label">Perfil</label>
          <input v-model="form.perfil" type="text" class="qs-input" />
        </div>
        <div class="qs-field qs-field-full">
          <label class="qs-label">Nova Senha <span class="qs-label-hint">(deixe em branco para não alterar)</span></label>
          <input v-model="form.senha" type="password" class="qs-input" autocomplete="new-password" />
        </div>
      </div>
      <div v-if="erroSalvar" class="qs-alert-danger" style="margin-top:16px">{{ erroSalvar }}</div>
      <div v-if="sucesso" class="qs-alert-success" style="margin-top:16px">Dados atualizados com sucesso!</div>
      <div class="qs-form-actions">
        <button class="qs-btn-primary" :disabled="salvando" @click="salvarAlteracoes">
          <span v-if="salvando" class="qs-spinner-sm" />
          {{ salvando ? 'Salvando...' : 'Salvar Alterações' }}
        </button>
        <button class="qs-btn-secondary" @click="usuarioSelecionado = null; sucesso = false; erroSalvar = ''">Cancelar</button>
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
  buscando.value = true; erroSalvar.value = '';
  try {
    const { data } = await api.get<UsuarioAdmin[]>(`/admin/usuarios/listar?busca=${encodeURIComponent(busca.value)}`, authHeader());
    const lista = Array.isArray(data) ? data : (data as { items?: UsuarioAdmin[] })?.items || [];
    if (lista.length > 0) {
      usuarioSelecionado.value = lista[0];
      Object.assign(form, { username: lista[0].username || '', email: lista[0].email || '', login: lista[0].login || '', perfil: lista[0].perfil || '', senha: '' });
      sucesso.value = false;
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
    sucesso.value = true; form.senha = '';
  } catch (e: unknown) { erroSalvar.value = extractApiErrorMessage(e, 'Erro ao salvar alterações.'); }
  finally { salvando.value = false; }
}
onMounted(() => { agenciaStore.loadFromStorage(); });
</script>

<style scoped>
.qs-search-card { padding: 20px 24px; }
.qs-search-row { display: flex; gap: 12px; align-items: center; }
.qs-form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.qs-field { display: flex; flex-direction: column; gap: 6px; }
.qs-field-full { grid-column: 1 / -1; }
.qs-label { font-size: 13px; font-weight: 600; color: var(--qs-gray-700); }
.qs-label-hint { font-weight: 400; color: var(--qs-gray-400); }
.qs-form-actions { display: flex; gap: 12px; margin-top: 24px; }
.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; }
.qs-alert-success { background: #f0fdf4; color: #16a34a; border: 1px solid #bbf7d0; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; }
.qs-spinner-sm { display: inline-block; width: 14px; height: 14px; border: 2px solid rgba(255,255,255,.4); border-top-color: #fff; border-radius: 50%; animation: spin .7s linear infinite; vertical-align: middle; margin-right: 6px; }
@keyframes spin { to { transform: rotate(360deg); } }
@media (max-width: 640px) { .qs-form-grid { grid-template-columns: 1fr; } }
</style>
