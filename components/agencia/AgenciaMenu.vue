<template>
  <div class="ag-sidebar" :class="{ open: isOpen }">
    <div class="ag-sidebar-logo">
      <img src="/agencia/imgs/quanta-shop.png" alt="Quanta Shop" />
    </div>

    <div class="ag-user-card d-flex align-items-start flex-column" v-if="user">
      <div class="d-flex align-items-center w-100 mb-1">
        <div class="avatar">{{ user.username?.charAt(0)?.toUpperCase() || 'U' }}</div>
        <div>
          <div class="user-name">{{ user.username }}</div>
          <div class="user-perfil">{{ perfilLabel }}</div>
        </div>
      </div>
      <div v-if="saldo !== null" class="ag-saldo">
        Saldo: {{ formatCurrency(saldo) }}
      </div>
    </div>

    <nav>
      <NuxtLink v-if="isAdmin || isComerciante" :to="dashboardRoute" class="nav-item-btn" @click="close">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z"/></svg>
        Painel
      </NuxtLink>

      <template v-for="(menu, i) in menus" :key="i">
        <template v-if="!menu.subMenus?.length">
          <NuxtLink :to="prefixRoute(menu.url)" class="nav-item-btn" @click="close">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="nav-icon"><path d="M9 3L5 6.99h3V14h2V6.99h3L9 3zm7 14.01V10h-2v7.01h-3L15 21l4-3.99h-3z"/></svg>
            {{ menu.texto }}
          </NuxtLink>
        </template>

        <template v-else>
          <button class="nav-item-btn" @click="toggleSubmenu(i)">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="nav-icon"><path d="M4 6h16v2H4zm0 5h16v2H4zm0 5h16v2H4z"/></svg>
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
        Sair
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

const user = computed(() => agenciaStore.dadosUser);
const isAdmin = computed(() => agenciaStore.isAdmin);
const isComerciante = computed(() => agenciaStore.isComerciante);

const menus = ref<AgenciaMenuItem[]>([]);
const openSubmenus = ref<Record<number, boolean>>({});
const saldo = ref<number | null>(null);

const dashboardRoute = computed(() => {
  if (isAdmin.value) return '/agencia/painel/admin';
  if (isComerciante.value) return '/agencia/painel/comerciante';
  return '/agencia/painel';
});

const perfilLabel = computed(() => {
  const p = user.value?.perfil;
  if (user.value?.admin) return 'Administrador';
  if (p === 'C' && user.value?.comerciante) return 'Comerciante';
  if (p === 'E') return 'Empreendedor';
  return 'Usuário';
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

function formatCurrency(v: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v);
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

async function loadSaldo() {
  try {
    const token = agenciaStore.getToken();
    const { data } = await api.get<number | { saldo: number }>('/financeiro/saldo', {
      headers: { Authorization: `Bearer ${token}` },
    });
    saldo.value = typeof data === 'number' ? data : (data?.saldo ?? null);
  } catch (err: unknown) {
    console.error('Erro ao carregar saldo:', extractApiErrorMessage(err));
  }
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  await loadMenu();
  await loadSaldo();
});
</script>
