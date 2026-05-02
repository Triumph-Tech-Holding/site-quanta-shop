<template>
  <div class="qs-page">
    <div class="qs-page-header">
      <div class="qs-header-text">
        <div class="qs-eyebrow">Admin · Usuários</div>
        <h1>Usuários</h1>
        <p>Gerenciar usuários da plataforma</p>
      </div>
      <div class="qs-header-actions">
        <div class="qs-search-field">
          <svg class="qs-search-icon" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
          <input v-model="busca" type="text" class="qs-search-input" placeholder="Nome, e-mail ou login..." @keyup.enter="buscarUsuarios" />
        </div>
        <button class="qs-btn-primary" @click="buscarUsuarios" :disabled="loading">Buscar</button>
      </div>
    </div>

    <div v-if="loading" class="qs-loading"><div class="qs-spinner" /></div>

    <div v-else class="qs-card-section">
      <div v-if="itens.length === 0" class="qs-empty-state">
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="var(--qs-gray-300)" stroke-width="1.5"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
        <h3>Nenhum usuário encontrado</h3>
        <p class="qs-empty-hint">Faça uma busca pelo nome, e-mail ou login do usuário.</p>
      </div>
      <div v-else class="qs-table-wrap">
        <table class="qs-table">
          <thead>
            <tr>
              <th>Nome</th>
              <th>E-mail</th>
              <th>Login</th>
              <th>Perfil</th>
              <th>Status</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in itens" :key="item.id">
              <td class="qs-cell-bold">{{ item.username || item.login }}</td>
              <td>
                <span class="qs-masked-field" :title="isMaster ? 'Clique em Ver para revelar' : 'Dado protegido por LGPD'">
                  {{ maskEmail(item.email) }}
                  <span v-if="!isMaster" class="qs-lgpd-lock" title="LGPD — apenas Master pode revelar">🔒</span>
                </span>
              </td>
              <td>{{ item.login }}</td>
              <td>{{ item.perfil || '—' }}</td>
              <td>
                <span class="qs-badge" :class="item.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-warn'">
                  {{ item.status || 'Ativo' }}
                </span>
              </td>
              <td>
                <button class="qs-btn-sm-outline" @click="verUsuario(item)">Ver</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showModal && selecionado" class="qs-modal-overlay" @click.self="fecharModal">
      <div class="qs-modal" style="max-width:540px">
        <div class="qs-modal-header">
          <h5>{{ selecionado.username || selecionado.login }}</h5>
          <button class="qs-modal-close" @click="fecharModal">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <div class="qs-info-grid">
            <div class="qs-info-item">
              <span class="qs-info-label">Login</span>
              <span class="qs-info-value">{{ selecionado.login }}</span>
            </div>
            <div class="qs-info-item">
              <span class="qs-info-label">Perfil</span>
              <span class="qs-info-value">{{ selecionado.perfil || '—' }}</span>
            </div>
            <div class="qs-info-item">
              <span class="qs-info-label">Status</span>
              <span class="qs-badge" :class="selecionado.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-warn'">{{ selecionado.status || 'Ativo' }}</span>
            </div>
          </div>

          <div class="qs-lgpd-section">
            <div class="qs-lgpd-title">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="var(--qs-teal)"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>
              Dados sensíveis (LGPD)
            </div>

            <div v-for="campo in camposSensiveis" :key="campo.key" class="qs-lgpd-row">
              <span class="qs-lgpd-campo-label">{{ campo.label }}</span>
              <span class="qs-lgpd-valor">
                <span v-if="revelados[campo.key as SensitiveKey]" class="qs-lgpd-revealed">{{ getCampoValor(campo.key) }}</span>
                <span v-else class="qs-lgpd-masked">{{ getMasked(campo.key) }}</span>
              </span>
              <button
                v-if="isMaster"
                class="qs-btn-sm-lgpd"
                :disabled="revelarLoading[campo.key as SensitiveKey]"
                @click="revelar(campo.key as SensitiveKey)"
              >
                <span v-if="revelarLoading[campo.key as SensitiveKey]" class="qs-spinner-xs" />
                <span v-else>{{ revelados[campo.key as SensitiveKey] ? 'Ocultar' : 'Revelar' }}</span>
              </button>
              <span v-else class="qs-lgpd-lock-inline" title="Apenas Master pode revelar dados sensíveis">🔒</span>
            </div>

            <div v-if="isMaster" class="qs-lgpd-master-note">
              <svg width="13" height="13" viewBox="0 0 24 24" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
              Você é Master. Cada revelação é registrada na auditoria LGPD.
            </div>

            <div v-if="revelarError" class="qs-alert-danger qs-mt-sm">{{ revelarError }}</div>
          </div>

          <div class="qs-modal-actions-inline">
            <button class="qs-btn-sm-outline" @click="alterarStatus(selecionado, 'Ativo')">Ativar</button>
            <button class="qs-btn-sm-warn" @click="alterarStatus(selecionado, 'Inativo')">Desativar</button>
            <button class="qs-btn-sm-danger" @click="alterarStatus(selecionado, 'Bloqueado')">Bloquear</button>
          </div>

          <div v-if="modalError" class="qs-alert-danger qs-mt-sm">{{ modalError }}</div>
        </div>
        <div class="qs-modal-footer">
          <button class="qs-btn-secondary" @click="fecharModal">Fechar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
import type { UsuarioAdmin } from '~/types/agencia';
import { maskEmail, maskCpfCnpj, maskTelefone } from '~/utils/lgpd-mask';
import type { SensitiveField } from '~/utils/lgpd-mask';

definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });

type SensitiveKey = 'email' | 'cpf' | 'telefone';

const agenciaStore = useAgenciaStore();
const api = useApi();
const loading = ref(true);
const itens = ref<UsuarioAdmin[]>([]);
const showModal = ref(false);
const modalError = ref('');
const busca = ref('');
const selecionado = ref<UsuarioAdmin | null>(null);
const revelarError = ref('');
const revelados = reactive<Record<SensitiveKey, boolean>>({ email: false, cpf: false, telefone: false });
const revelarLoading = reactive<Record<SensitiveKey, boolean>>({ email: false, cpf: false, telefone: false });

const camposSensiveis = [
  { key: 'email', label: 'E-mail' },
  { key: 'cpf', label: 'CPF / Doc.' },
  { key: 'telefone', label: 'Telefone' },
] as const;

const isMaster = computed<boolean>(() => {
  const adm: Record<string, unknown> = (agenciaStore.userAdmin as Record<string, unknown>) || {};
  return adm.master === true || adm.Master === true || adm.perfil === 'Master';
});

function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }

function getCampoValor(key: string): string {
  if (!selecionado.value) return '';
  const u = selecionado.value as Record<string, unknown>;
  if (key === 'email') return String(u.email || '');
  if (key === 'cpf') return String(u.documento || u.cpf || '');
  if (key === 'telefone') return String(u.celular || u.telefone || '');
  return '';
}

function getMasked(key: string): string {
  const val = getCampoValor(key);
  if (key === 'email') return maskEmail(val);
  if (key === 'cpf') return maskCpfCnpj(val);
  if (key === 'telefone') return maskTelefone(val);
  return '••••••••';
}

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

async function revelar(campo: SensitiveKey) {
  if (!selecionado.value) return;
  if (revelados[campo]) { revelados[campo] = false; return; }
  if (!isMaster.value) { revelarError.value = 'Apenas usuários Master podem revelar dados sensíveis.'; return; }
  revelarError.value = '';
  revelarLoading[campo] = true;
  try {
    const apiCampo: Record<SensitiveKey, string> = { email: 'EMAIL', cpf: 'CPF', telefone: 'TELEFONE' };
    await api.post('/admin/revelar-dado-sensivel', {
      idUsuarioAlvo: selecionado.value.id,
      campo: apiCampo[campo],
      motivo: 'Consulta admin painel',
    }, authHeader());
    revelados[campo] = true;
  } catch (e: unknown) {
    revelarError.value = extractApiErrorMessage(e, 'Falha ao registrar/autorizar revelação.');
  } finally {
    revelarLoading[campo] = false;
  }
}

onMounted(async () => { agenciaStore.loadFromStorage(); await buscarUsuarios(); });
</script>

<style scoped>
.qs-empty-hint { font-size: 13px; color: var(--qs-gray-400); margin: 0; }

.qs-search-field { position: relative; }
.qs-search-icon { position: absolute; left: 10px; top: 50%; transform: translateY(-50%); color: var(--qs-gray-400); pointer-events: none; }
.qs-search-input { padding: 8px 12px 8px 32px; border: 1px solid var(--qs-gray-200); border-radius: var(--qs-radius-md); font-size: 14px; outline: none; min-width: 240px; }
.qs-search-input:focus { border-color: var(--qs-teal); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }

.qs-masked-field { display: inline-flex; align-items: center; gap: 4px; font-family: 'SFMono-Regular', 'Consolas', monospace; font-size: 13px; color: var(--qs-gray-500); }
.qs-lgpd-lock { font-size: 11px; }

.qs-info-grid { display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 16px; margin-bottom: 20px; }
.qs-info-item { display: flex; flex-direction: column; gap: 4px; }
.qs-info-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; color: var(--qs-gray-400); }
.qs-info-value { font-size: 14px; color: var(--qs-ink); font-weight: 500; }

.qs-lgpd-section { background: var(--qs-gray-50); border: 1px solid var(--qs-gray-100); border-radius: var(--qs-radius-md); padding: 14px 16px; margin-bottom: 16px; }
.qs-lgpd-title { display: flex; align-items: center; gap: 6px; font-size: 12px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.06em; color: var(--qs-teal-dark); margin-bottom: 12px; }
.qs-lgpd-row { display: grid; grid-template-columns: 80px 1fr auto; gap: 10px; align-items: center; padding: 8px 0; border-bottom: 1px solid var(--qs-gray-100); }
.qs-lgpd-row:last-of-type { border-bottom: none; }
.qs-lgpd-campo-label { font-size: 11px; font-weight: 600; color: var(--qs-gray-500); text-transform: uppercase; letter-spacing: 0.04em; }
.qs-lgpd-valor { font-size: 13px; min-width: 0; }
.qs-lgpd-masked { font-family: 'SFMono-Regular', 'Consolas', monospace; color: var(--qs-gray-400); letter-spacing: 0.05em; }
.qs-lgpd-revealed { font-family: 'SFMono-Regular', 'Consolas', monospace; color: var(--qs-ink); font-weight: 500; word-break: break-all; }
.qs-lgpd-lock-inline { font-size: 14px; }

.qs-btn-sm-lgpd { padding: 4px 10px; font-size: 11px; font-weight: 700; border: 1px solid var(--qs-teal); color: var(--qs-teal); background: #fff; border-radius: var(--qs-radius-md); cursor: pointer; transition: all 0.15s; white-space: nowrap; display: inline-flex; align-items: center; gap: 5px; }
.qs-btn-sm-lgpd:hover:not(:disabled) { background: var(--qs-teal); color: #fff; }
.qs-btn-sm-lgpd:disabled { opacity: 0.5; cursor: not-allowed; }

.qs-lgpd-master-note { display: flex; align-items: center; gap: 6px; font-size: 11px; color: var(--qs-teal); margin-top: 10px; padding-top: 10px; border-top: 1px solid var(--qs-gray-100); }

.qs-modal-actions-inline { display: flex; gap: 8px; flex-wrap: wrap; margin-top: 16px; }
.qs-btn-sm-warn { padding: 6px 14px; border: 1px solid #f59e0b; color: #d97706; background: #fff; border-radius: var(--qs-radius-md); font-size: 13px; font-weight: 600; cursor: pointer; transition: background 0.15s; }
.qs-btn-sm-warn:hover { background: #fef3c7; }

.qs-alert-danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; border-radius: var(--qs-radius-md); padding: 10px 14px; font-size: 14px; }
.qs-mt-sm { margin-top: 12px; }

.qs-spinner-xs { display: inline-block; width: 10px; height: 10px; border: 2px solid rgba(47,119,133,.3); border-top-color: var(--qs-teal); border-radius: 50%; animation: spin .7s linear infinite; }
@keyframes spin { to { transform: rotate(360deg); } }
</style>
