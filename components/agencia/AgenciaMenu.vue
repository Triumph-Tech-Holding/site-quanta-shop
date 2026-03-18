<template>
  <div class="bg-menu">
    <div class="sidebar-logo-block">
      <NuxtLink :to="dashboardRoute">
        <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" />
      </NuxtLink>
    </div>

    <div class="navbar-nav-vertical" id="nav-collapse">
      <a href="https://quantashop.com.br" class="listMenu" target="_blank">
        Lojas
      </a>

      <NuxtLink :to="dashboardRoute" class="listMenu">
        Painel Geral
      </NuxtLink>

      <template v-for="(menu, i) in menus" :key="i">
        <template v-if="!menu.subMenus?.length">
          <NuxtLink :to="prefixRoute(menu.url)" class="listMenu">
            {{ menu.texto }}
          </NuxtLink>
        </template>

        <template v-else>
          <div class="listMenu has-submenu" @click="toggleSubmenu(i)">
            <span>{{ menu.texto }}</span>
            <svg class="sort-icon" :class="{ open: openSubmenus[i] }" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" width="14" height="14">
              <path d="M7 10l5 5 5-5z"/>
            </svg>
          </div>
          <div v-show="openSubmenus[i]" class="sub-menu-list">
            <NuxtLink
              v-for="(sub, j) in menu.subMenus"
              :key="j"
              :to="prefixRoute(sub.url)"
              class="listMenu sub-item"
            >
              {{ sub.texto }}
            </NuxtLink>
          </div>
        </template>
      </template>

      <NuxtLink to="/agencia/logout" class="listMenu logout-item">
        Logout
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { AgenciaMenuItem } from '~/types/agencia';
import { extractApiErrorMessage } from '~/types/agencia';

defineProps<{ open?: boolean }>();

const agenciaStore = useAgenciaStore();
const api = useApi();

const isAdmin = computed(() => agenciaStore.isAdmin);
const isComerciante = computed(() => agenciaStore.isComerciante);
const user = computed(() => agenciaStore.dadosUser);

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

async function loadMenu() {
  if (!user.value?.perfil) return;
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
    const { data } = await api.get<AgenciaMenuItem[]>(`/geral/obterMenu/${user.value.perfil}`, {
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
