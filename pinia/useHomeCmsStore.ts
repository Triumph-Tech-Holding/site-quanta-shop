import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { HomeConfig } from '@/composables/useHomeConfig';

export const useHomeCmsStore = defineStore('homeCms', () => {
  const config = ref<HomeConfig | null>(null);
  const loaded = ref(false);

  async function fetchConfig(): Promise<void> {
    if (loaded.value) return;
    try {
      const data = await $fetch<HomeConfig>('/home-cms');
      config.value = data;
    } catch {
      config.value = null;
    } finally {
      loaded.value = true;
    }
  }

  async function saveConfig(data: HomeConfig, token?: string | null): Promise<void> {
    await $fetch('/home-cms', {
      method: 'PUT',
      body: data,
      headers: token ? { Authorization: `Bearer ${token}` } : {},
    });
    config.value = data;
  }

  return { config, loaded, fetchConfig, saveConfig };
});
