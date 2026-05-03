<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="CCR" title="Inserir Cupom Fiscal" description="Registre sua nota fiscal para ganhar cashback." />

        <div class="qs-card-section ic-card">
          <div class="ic-icon-wrap">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M21 5c-1.11-.35-2.33-.5-3.5-.5-1.95 0-4.05.4-5.5 1.5-1.45-1.1-3.55-1.5-5.5-1.5S2.45 4.9 1 6v14.65c0 .25.25.5.5.5.1 0 .15-.05.25-.05C3.1 20.45 5.05 20 6.5 20c1.95 0 4.05.4 5.5 1.5 1.35-.85 3.8-1.5 5.5-1.5 1.65 0 3.35.3 4.75 1.05.1.05.15.05.25.05.25 0 .5-.25.5-.5V6c-.6-.45-1.25-.75-2-1zm0 13.5c-1.1-.35-2.3-.5-3.5-.5-1.7 0-4.15.65-5.5 1.5V8c1.35-.85 3.8-1.5 5.5-1.5 1.2 0 2.4.15 3.5.5v11.5z"/></svg>
          </div>
          <div class="qs-section-title" style="margin-bottom:1.5rem;">Registrar cupom</div>
          <form @submit.prevent="inserir" class="ic-form">
            <div class="ic-field">
              <label class="qs-label">Chave da NFe / Código do cupom *</label>
              <input v-model="form.chave" type="text" class="qs-input" required maxlength="100" placeholder="Digite a chave da nota fiscal" />
              <span class="ic-hint">Cole aqui o código da chave de acesso da sua nota fiscal eletrônica.</span>
            </div>
            <div class="ic-field">
              <label class="qs-label">Loja <span class="ic-optional">(opcional)</span></label>
              <select v-model="form.idLoja" class="qs-input">
                <option value="">Selecione a loja</option>
                <option v-for="l in lojas" :key="l.id" :value="l.id">{{ l.nome || l.razaoSocial }}</option>
              </select>
            </div>
            <div class="ic-actions">
              <button type="submit" class="qs-btn-primary ic-submit-btn" :disabled="enviando">
                <svg v-if="enviando" class="ic-spin" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 4V2A10 10 0 0 0 2 12h2a8 8 0 0 1 8-8z"/></svg>
                <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                {{ enviando ? 'Enviando...' : 'Registrar Cupom' }}
              </button>
            </div>
          </form>
        </div>

      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp();
const enviando = ref(false);
import type { Loja } from "~/types/agencia";
const lojas = ref<Loja[]>([]);
const form = reactive({ chave: '', idLoja: '' });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
async function inserir() {
  enviando.value = true;
  try {
    await api.post('/cupomFiscal/inserir', form, authHeader());
    $toast?.success('Cupom registrado! Aguarde a aprovação.');
    form.chave = ''; form.idLoja = '';
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao registrar cupom.'));
  } finally { enviando.value = false; }
}
onMounted(async () => {
  agenciaStore.loadFromStorage();
  try {
    const { data } = await api.get('/loja/listar', authHeader());
    lojas.value = Array.isArray(data) ? data : [];
  } catch { /**/ }
});
</script>

<style scoped>
.ic-card { background: #fff; max-width: 620px; }
.ic-icon-wrap {
  width: 52px;
  height: 52px;
  border-radius: var(--qs-radius-md,12px);
  background: linear-gradient(135deg, var(--qs-teal,#2F7785), var(--qs-teal-dark,#225F6B));
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 1rem;
}
.ic-icon-wrap svg { width: 26px; height: 26px; color: #fff; }

.ic-form { display: flex; flex-direction: column; gap: 1rem; }
.ic-field { display: flex; flex-direction: column; gap: .375rem; }
.ic-hint { font-size: .75rem; color: var(--qs-gray-400,#9ca3af); line-height: 1.4; }
.ic-optional { font-weight: 400; color: var(--qs-gray-400,#9ca3af); text-transform: none; letter-spacing: 0; }
.ic-actions { display: flex; }
.ic-submit-btn { display: inline-flex; align-items: center; gap: .4rem; }
.ic-submit-btn svg { width: 16px; height: 16px; }
.ic-spin { animation: ic-spin 1s linear infinite; }
@keyframes ic-spin { to { transform: rotate(360deg); } }

.qs-label { font-size: .75rem; font-weight: 600; color: var(--qs-gray-700,#374151); text-transform: uppercase; letter-spacing: .04em; }
.qs-input { width: 100%; padding: .625rem .875rem; border: 1.5px solid var(--qs-gray-200,#e5e7eb); border-radius: var(--qs-radius-md,12px); font-size: .875rem; color: var(--qs-ink,#1d1d1f); background: #fff; transition: border-color .15s; box-sizing: border-box; }
.qs-input:focus { outline: none; border-color: var(--qs-teal,#2F7785); }
</style>
