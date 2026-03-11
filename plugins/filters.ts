import { formatToBRL } from '@/utils/filters';

export default defineNuxtPlugin(nuxtApp => {
  // Registre o filtro globalmente
  nuxtApp.provide('formatToBRL', formatToBRL);
});
