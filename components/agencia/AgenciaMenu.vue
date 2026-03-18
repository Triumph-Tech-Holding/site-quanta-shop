<template>
  <div class="ag-sidebar" :class="{ open: isOpen }">
    <div class="ag-sidebar-logo">
      <NuxtLink to="/agencia/painel">
        <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" />
      </NuxtLink>
    </div>

    <nav>
      <NuxtLink v-if="isAdmin || isComerciante" :to="dashboardRoute" class="nav-item-btn" @click="close">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z"/></svg>
        Painel
      </NuxtLink>

      <template v-for="(menu, i) in menus" :key="i">
        <template v-if="!menu.subMenus?.length">
          <NuxtLink :to="prefixRoute(menu.url)" class="nav-item-btn" @click="close">
            {{ menu.texto }}
          </NuxtLink>
        </template>

        <template v-else>
          <button class="nav-item-btn" @click="toggleSubmenu(i)">
            {{ menu.texto }}
            <svg class="caret" :class="{ open: openSubmenus[i] }" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" width="16" height="16"><path d="M7 10l5 5 5-5z"/></svg>
          </button>
          <div v-show="openSubmenus[i]" class="submenu">
            <NuxtLink
              v-for="(sub, j) in menu.subMenus"
              :key="j"
              :to="prefixRoute(sub.url)"
              class="nav-item-btn"
              @click="close"
            >
              {{ sub.texto }}
            </NuxtLink>
          </div>
        </template>
      </template>
    </nav>

    <div class="ag-logout-btn">
      <NuxtLink to="/agencia/logout" @click="close">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" width="18" height="18"><path d="M17 7l-1.41 1.41L18.17 11H8v2h10.17l-2.58 2.58L17 17l5-5zM4 5h8V3H4c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h8v-2H4V5z"/></svg>
        LOGOUT
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { AgenciaMenuItem } from '~/types/agencia';
import { extractApiErrorMessage } from '~/types/agencia';

defineProps<{ isOpen: boolean }>();
const emit = defineEmits(['close']);

const agenciaStore = useAgenciaStore();
const api = useApi();

const isAdmin = computed(() => agenciaStore.isAdmin);
const isComerciante = computed(() => agenciaStore.isComerciante);

const menus = ref<AgenciaMenuItem[]>([]);
const openSubmenus = ref<Record<number, boolean>>({});

const dashboardRoute = computed(() => {
  if (isAdmin.value) return '/agencia/painel/admin';
  if (isComerciante.value) return '/agencia/painel/comerciante';
  return '/agencia/painel';
});

function prefixRoute(url: string): string {
  if (!url) return '/agencia/painel';
  if (url.startsWith('/painel')) return '/agencia' + url;
  if (url.startsWith('/')) return '/agencia' + url;
  return '/agencia/' + url;
}

function toggleSubmenu(i: number) {
  openSubmenus.value[i] = !openSubmenus.value[i];
}

function close() {
  emit('close');
}

async function loadMenu() {
  const user = agenciaStore.dadosUser;
  if (!user?.perfil) return;
  const cached = localStorage.getItem('agencia_menu');
  if (cached) {
    try {
      const parsed: AgenciaMenuItem[] = JSON.parse(cached);
      menus.value = parsed.filter((m) => !m.rotaPublica);
      return;
    } catch {
      localStorage.removeItem('agencia_menu');
    }
  }
  try {
    const token = agenciaStore.getToken();
    const { data } = await api.get<AgenciaMenuItem[]>(`/geral/obterMenu/${user.perfil}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    if (data) {
      menus.value = data.filter((m) => !m.rotaPublica);
      localStorage.setItem('agencia_menu', JSON.stringify(data));
    }
  } catch (err: unknown) {
    console.error('Erro ao carregar menu:', extractApiErrorMessage(err));
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await loadMenu();
});
</script>
