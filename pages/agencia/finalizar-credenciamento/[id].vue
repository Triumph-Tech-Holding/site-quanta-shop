<template>
  <div class="agencia-login-page">
    <div class="login-box">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" class="logo-login" />
      <div v-if="loading" class="text-center"><div class="spinner-border text-secondary mt-3" /></div>
      <div v-else-if="credenciamento">
        <h5 class="text-center mb-3">Finalizar Credenciamento</h5>
        <div class="alert alert-info" style="font-size:.875rem">
          <strong>Parceiro:</strong> {{ credenciamento.nome || credenciamento.nomeEmpresa }}<br/>
          <strong>CNPJ:</strong> {{ credenciamento.cnpj }}
        </div>
        <div class="text-center mt-3">
          <button class="btn btn-ag-primary w-100" @click="finalizar" :disabled="finalizando">
            <span v-if="finalizando" class="spinner-border spinner-border-sm me-2" />
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
definePageMeta({ layout: 'agencia-login' });
const route = useRoute();
const api = useApi();
const { $toast } = useNuxtApp();
const loading = ref(true);
const finalizando = ref(false);
const credenciamento = ref<any>(null);
onMounted(async () => {
  try {
    const { data } = await api.get(`/credenciamento/obter/${route.params.id}`);
    credenciamento.value = data;
  } catch { } finally { loading.value = false; }
});
async function finalizar() {
  finalizando.value = true;
  try {
    await api.post(`/credenciamento/finalizar/${route.params.id}`, {});
    $toast?.success('Credenciamento finalizado com sucesso!');
    navigateTo('/agencia');
  } catch (e: any) {
    $toast?.error(e?.response?.data?.message || 'Erro ao finalizar credenciamento.');
  } finally { finalizando.value = false; }
}
</script>
