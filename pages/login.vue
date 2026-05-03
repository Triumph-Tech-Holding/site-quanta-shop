<template>
  <div>
    <login-area/>
  </div>
</template>

<script setup lang="ts">
import { useAgenciaStore } from "@/pinia/useAgenciaStore";

useSeoMeta({ title: "Entrar | Quanta Shop" });

const agenciaStore = useAgenciaStore();
const router = useRouter();

onBeforeMount(() => {
  agenciaStore.loadFromStorage();

  if (agenciaStore.user && agenciaStore.checkTokenExpiry()) {
    router.replace('/agencia/painel');
  }
});
</script>
