import VueTheMask from 'vue-the-mask';

export default defineNuxtPlugin((nuxtApp: any) => {
  nuxtApp.vueApp.use(VueTheMask);
});
