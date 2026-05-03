<template>
  <div class="agencia-login-page">
    <div class="login-box">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div v-if="loading" class="text-center"><div class="qs-spinner" style="margin:16px auto" /></div>
      <div v-else-if="credenciamento">
        <h5 class="text-center mb-3">Finalizar Credenciamento</h5>
        <div class="qs-alert-warn mb-3" style="font-size:.875rem">
          <strong>Parceiro:</strong> {{ credenciamento.nome || credenciamento.nomeEmpresa }}<br/>
          <strong>CNPJ:</strong> {{ credenciamento.cnpj }}
        </div>
        <div class="text-center mt-3">
          <button class="btn btn-ag-primary w-100" @click="finalizar" :disabled="finalizando">
            <span v-if="finalizando" class="fc-spinner" />
            {{ finalizando ? 'Finalizando...' : 'Confirmar Credenciamento' }}
          </button>
        </div>
      </div>
      <div v-else class="text-center">
        <p class="text-muted">Credenciamento não encontrado ou expirado.</p>
        <NuxtLink to="/agencia" class="btn btn-ag-outline">Voltar</NuxtLink>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { extractApiErrorMessage } from '~/types/agencia';
definePageMeta({ layout: 'agencia-login' });
const route = useRoute();
const api = useApi();
const { $toast } = useNuxtApp();
const loading = ref(true);
const finalizando = ref(false);
interface CredenciamentoItem { nome?: string; nomeEmpresa?: string; cnpj?: string; [key: string]: unknown; }
const credenciamento = ref<CredenciamentoItem | null>(null);
onMounted(async () => {
  try {
    const { data } = await api.get(`/credenciamento/obter/${route.params.id}`);
    credenciamento.value = data;
  } catch(e: unknown) { console.error("Erro ao carregar credenciamento:", extractApiErrorMessage(e)); } finally { loading.value = false; }
});
async function finalizar() {
  finalizando.value = true;
  try {
    await api.post(`/credenciamento/finalizar/${route.params.id}`, {});
    $toast?.success('Credenciamento finalizado com sucesso!');
    navigateTo('/agencia/painel');
  } catch (e: unknown) {
    $toast?.error(extractApiErrorMessage(e, 'Erro ao finalizar credenciamento.'));
  } finally { finalizando.value = false; }
}
</script>

<style scoped>
.fc-spinner {
  display: inline-block;
  width: 14px; height: 14px;
  border: 2px solid rgba(255,255,255,.4);
  border-top-color: currentColor;
  border-radius: 50%;
  animation: fc-spin .7s linear infinite;
  vertical-align: middle;
  margin-right: 6px;
}
@keyframes fc-spin { to { transform: rotate(360deg); } }
</style>
