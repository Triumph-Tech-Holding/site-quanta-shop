<template>
  <div>
    <div class="ag-page-header"><h1>Nova Solicitação</h1><p>Descreva o problema ou dúvida que você tem</p></div>
    <div class="ag-card" style="max-width:700px">
      <form @submit.prevent="enviar" class="row g-3">
        <div class="col-12 ag-form-group">
          <label>Tipo de solicitação *</label>
          <select v-model="form.tipo" class="form-select" required>
            <option value="">Selecione</option>
            <option value="cashback">Cashback</option>
            <option value="contato">Contato geral</option>
            <option value="pedido">Pedido</option>
            <option value="outros">Outros</option>
          </select>
        </div>
        <div class="col-12 ag-form-group">
          <label>Assunto *</label>
          <input v-model="form.assunto" type="text" class="form-control" required maxlength="200" />
        </div>
        <div class="col-12 ag-form-group">
          <label>Descrição *</label>
          <textarea v-model="form.descricao" class="form-control" rows="5" required maxlength="2000" />
          <div class="text-muted text-end" style="font-size:.8rem">{{ form.descricao.length }}/2000</div>
        </div>
        <div class="col-12 d-flex gap-2">
          <button type="submit" class="btn btn-ag-primary" :disabled="enviando">
            <span v-if="enviando" class="spinner-border spinner-border-sm me-1" />
            {{ enviando ? 'Enviando...' : 'Enviar Solicitação' }}
          </button>
          <NuxtLink to="/agencia/painel/suporte" class="btn btn-ag-outline">Cancelar</NuxtLink>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });
const agenciaStore = useAgenciaStore();
const api = useApi();
const { $toast } = useNuxtApp() as any;
const enviando = ref(false);
const form = reactive({ tipo: '', assunto: '', descricao: '' });
function authHeader() { return { headers: { Authorization: `Bearer ${agenciaStore.getToken()}` } }; }
async function enviar() {
  enviando.value = true;
  try {
    await api.post('/suporte/criar', form, authHeader());
    $toast?.success('Solicitação enviada com sucesso!');
    navigateTo('/agencia/painel/suporte');
  } catch (e: any) {
    $toast?.error(e?.response?.data?.message || 'Erro ao enviar solicitação.');
  } finally { enviando.value = false; }
}
onMounted(() => { agenciaStore.loadFromStorage(); });
</script>
