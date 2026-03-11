<template>
  <div v-if="isLoading" class="loading-overlay">
    <div class="loading-content">
      <img src="/img/ui/loading.gif" width="64" alt="Carregando..." />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue';
import { useLoadingStore } from "@/pinia/useLoadingStore";

const loadingStore = useLoadingStore();
const isLoading = computed(() => loadingStore.isLoading);

// Monitorando mudanças no estado de loading
watch(
  () => loadingStore.isLoading,
  (newValue) => {
    if (newValue) {
      document.body.classList.add("no-scroll"); // Adiciona a classe quando loading começa
    } else {
      document.body.classList.remove("no-scroll"); // Remove a classe quando loading termina
    }
  }
);
</script>

<style>
.no-scroll {
  overflow: hidden;
}

.loading-overlay {
  position: fixed; 
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: 9999;
  display: flex;
  justify-content: center;
  align-items: center;
  overflow: hidden;
}

.loading-content {
  text-align: center;
}
</style>
