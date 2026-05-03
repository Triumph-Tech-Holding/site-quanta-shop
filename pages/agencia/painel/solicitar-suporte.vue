<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Atendimento" title="Nova Solicitação" description="Descreva o problema ou dúvida e entraremos em contato.">
          <NuxtLink to="/agencia/painel/suporte" class="ss-back-link">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M20 11H7.83l5.59-5.59L12 4l-8 8 8 8 1.41-1.41L7.83 13H20v-2z"/></svg>
            Voltar ao suporte
          </NuxtLink>
        </QsPageHeader>

        <div class="qs-card-section ss-card">
          <form @submit.prevent="enviar" class="ss-form">

            <div class="ss-field">
              <label class="qs-label">Tipo de solicitação *</label>
              <select v-model="form.tipo" class="qs-input" required>
                <option value="">Selecione</option>
                <option value="cashback">Cashback</option>
                <option value="contato">Contato geral</option>
                <option value="pedido">Pedido</option>
                <option value="outros">Outros</option>
              </select>
            </div>

            <div class="ss-field">
              <label class="qs-label">Assunto *</label>
              <input v-model="form.assunto" type="text" class="qs-input" required maxlength="200" placeholder="Resumo da sua solicitação" />
            </div>

            <div class="ss-field">
              <label class="qs-label">Descrição *</label>
              <textarea v-model="form.descricao" class="qs-input ss-textarea" rows="6" required maxlength="2000" placeholder="Descreva detalhadamente sua situação…" />
              <div class="ss-char-count">{{ form.descricao.length }} / 2000</div>
            </div>

            <div class="ss-actions">
              <button type="submit" class="qs-btn-primary ss-submit" :disabled="enviando">
                <svg v-if="enviando" class="ss-spin" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 4V2A10 10 0 0 0 2 12h2a8 8 0 0 1 8-8z"/></svg>
                <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z"/></svg>
                {{ enviando ? 'Enviando...' : 'Enviar Solicitação' }}
              </button>
              <NuxtLink to="/agencia/painel/suporte" class="qs-btn-secondary ss-cancel">Cancelar</NuxtLink>
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
const form = reactive({ tipo: '', assunto: '', descricao: '' });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
async function enviar() {
  enviando.value = true;
  try {
    await api.post('/suporte/criar', form, authHeader());
    $toast?.success('Solicitação enviada com sucesso!');
    navigateTo('/agencia/painel/suporte');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao enviar solicitação.'));
  } finally { enviando.value = false; }
}
onMounted(() => { agenciaStore.loadFromStorage(); });
</script>

<style scoped>
.ss-back-link {
  display: inline-flex;
  align-items: center;
  gap: .35rem;
  font-size: .875rem;
  font-weight: 600;
  color: var(--qs-teal, #2F7785);
  text-decoration: none;
  flex-shrink: 0;
}
.ss-back-link svg { width: 16px; height: 16px; }
.ss-back-link:hover { opacity: .75; }

.ss-card { background: #fff; max-width: 700px; }
.ss-form { display: flex; flex-direction: column; gap: 1.125rem; }
.ss-field { display: flex; flex-direction: column; gap: .375rem; }
.ss-textarea { resize: vertical; min-height: 140px; }
.ss-char-count { font-size: .75rem; color: var(--qs-gray-400, #9ca3af); text-align: right; margin-top: .25rem; }

.ss-actions { display: flex; align-items: center; gap: .75rem; flex-wrap: wrap; }
.ss-submit { display: inline-flex; align-items: center; gap: .4rem; }
.ss-submit svg { width: 16px; height: 16px; }
.ss-spin { animation: ss-spin 1s linear infinite; }
@keyframes ss-spin { to { transform: rotate(360deg); } }
.ss-cancel { text-decoration: none; display: inline-flex; align-items: center; justify-content: center; }

.qs-label { font-size: .75rem; font-weight: 600; color: var(--qs-gray-700, #374151); text-transform: uppercase; letter-spacing: .04em; }
.qs-input { width: 100%; padding: .625rem .875rem; border: 1.5px solid var(--qs-gray-200, #e5e7eb); border-radius: var(--qs-radius-md, 12px); font-size: .875rem; color: var(--qs-ink, #1d1d1f); background: #fff; transition: border-color .15s; box-sizing: border-box; font-family: inherit; }
.qs-input:focus { outline: none; border-color: var(--qs-teal, #2F7785); }
</style>
