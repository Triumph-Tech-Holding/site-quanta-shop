<template>
  <nav>
    <ul>
      <!-- Botão para página anterior -->
      <li @click="setPage(currentPage-1)" :class="currentPage === 1 ? 'disable' : ''">
        <a class="tp-pagination-prev prev page-numbers cursor-pointer">
          <svg-paginate-prev />
        </a>
      </li>

      <!-- Exibição dinâmica de páginas -->
      <li v-for="n in paginatedPages" :key="n" @click="setPage(n)">
        <a :class="`cursor-pointer ${currentPage === n ? 'current' : ''}`">{{ n }}</a>
      </li>

      <!-- Botão para página seguinte -->
      <li @click="setPage(currentPage+1)" :class="currentPage === totalPages ? 'disable' : ''">
        <a class="next page-numbers cursor-pointer">
          <svg-paginate-next />
        </a>
      </li>
    </ul>
  </nav>
</template>

<script setup lang="ts">
import { computed, ref, onMounted, watch } from "vue";
import { type IProduct } from "@/types/product-type";
import { type IBlogType } from "@/types/blog-type";

const emit = defineEmits(["handlePaginate"]);
const route = useRoute();

type ItemDataType<T> = {
  data: T[];
  itemsPerPage: number;
  totalCount: number;
};
const props = defineProps<ItemDataType<IProduct | IBlogType>>();
const currentPage = ref<number>(1);
const maxVisiblePages = ref<number>(10); // Máximo de páginas visíveis

// Total de páginas
const totalPages = computed(() => {
  return props.data.length > 0
    ? Math.ceil(props.data.length / props.itemsPerPage)
    : Math.ceil(props.totalCount / props.itemsPerPage);
});

// Páginas visíveis, limitando a 5 páginas de cada vez
const paginatedPages = computed(() => {
  const pages = [];
  const start = Math.max(currentPage.value - Math.floor(maxVisiblePages.value / 2), 1);
  const end = Math.min(start + maxVisiblePages.value - 1, totalPages.value);

  for (let i = start; i <= end; i++) {
    pages.push(i);
  }

  return pages;
});

// Índices de início e fim para os dados
const startIndex = computed(() => (currentPage.value - 1) * props.itemsPerPage);
const endIndex = computed(() => startIndex.value + props.itemsPerPage);

// Alterar página
const setPage = (idx: number) => {
  if (idx <= 0 || idx > totalPages.value) {
    return;
  }
  window.scrollTo(0, 0);
  currentPage.value = idx;
  emit("handlePaginate", props.data, startIndex.value, endIndex.value);
};

// Paginação inicial
onMounted(() => {
  //emit("handlePaginate", props.data, startIndex.value, endIndex.value);
});

// Redefinir página ao alterar rota
watch(() => route.query || route.params, () => {
  currentPage.value = 1;
});
</script>
