<template>
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-types">
      <div class="form-check gap-2 d-flex align-items-center">
        <input
          class="form-check-input"
          type="checkbox"
          value=""
          id="cbx-local"
          v-model="selectedTypes.local"
        />
        <label class="form-check-label" for="cbx-local">LOCAL</label>
      </div>
      <div class="form-check gap-2 d-flex align-items-center">
        <input
          class="form-check-input"
          type="checkbox"
          value=""
          id="cbx-online"
          v-model="selectedTypes.online"
        />
        <label class="form-check-label" for="cbx-online">ONLINE</label>
      </div>

      <button type="button" class="tp-btn w-100 mt-3" @click="handleTypeFilter">
        Filtrar
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useCategoryStore } from "@/pinia/useCategoryStore";
import { usePartnerStore } from "@/pinia/usePartnerStore";
import type { ICategory } from "@/types/category-type";

const categoryStore = useCategoryStore();
const partnerStore = usePartnerStore();
const router = useRouter();
const route = useRoute();
const activeQuery = ref<string>("");
const props = defineProps<{
  filter_dropdown?: boolean;
  filter_offcanvas?: boolean;
  load_more?: boolean;
  right_side?: boolean;
  shop_full?: boolean;
  shop_1600?: boolean;
}>();

const selectedTypes = ref({
  local: true,
  online: true,
});

// handle category route
const handleTypeFilter = async () => {
  // if (props.filter_dropdown) {
  //   router.push(`/partners-filter-dropdown?category=${newCategory}`);
  // } else if (props.filter_offcanvas) {
  //   router.push(`/partners-filter-offcanvas?category=${newCategory}`);
  // } else if (props.load_more) {
  //   router.push(`/partners-load-more?category=${newCategory}`);
  // } else if (props.right_side) {
  //   router.push(`/shopartnersp-right-sidebar?category=${newCategory}`);
  // } else if (props.shop_full) {
  //   router.push(`/partners-full-width?category=${newCategory}`);
  // } else if (props.shop_1600) {
  //   router.push(`/partners-1600?category=${newCategory}`);
  // } else {
  //   router.push(`/partners?type=${newCategory}`);
  // }

  const type =
    selectedTypes.value.local && selectedTypes.value.online
      ? null
      : selectedTypes.value.local
      ? "local"
      : selectedTypes.value.online
      ? "online"
      : null;

  router.push(`/partners?type=${type ?? "all"}`);

  await partnerStore.fetchPartners(type, null, null, 1, 24);
};

onMounted(() => {
  activeQuery.value = route.query.category;
});

watch(
  () => categoryStore.categories,
  (newCategories) => {
    if (newCategories.length) {
    }
  },
  { immediate: true }
);

watch(
  () => route.query,
  (newStatus) => {
    activeQuery.value = newStatus.category;
  }
);
</script>

<style scoped>
.form-check-input:checked {
  background-color: #1e5d68; /* Cor de fundo quando selecionado */
  border-color: #1e5d68; /* Cor da borda quando selecionado */
}

/* Muda a cor do checkmark */
.form-check-input:checked:after {
  color: white; /* Cor do checkmark */
}
</style>
