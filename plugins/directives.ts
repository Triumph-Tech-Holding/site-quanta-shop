import backgroundImageDirective from '@/directives/backgroundImage';

export default defineNuxtPlugin(nuxtApp => {
  nuxtApp.vueApp.directive('background-image', backgroundImageDirective);
});
