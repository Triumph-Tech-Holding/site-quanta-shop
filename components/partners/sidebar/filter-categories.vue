<template>
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-categories">
      <ul>
        <li v-for="category in categories_data" :key="category.id">
          <a
            @click.prevent="handleCategoryRoute(category.parent)"
            :class="`cursor-pointer ${
              activeQuery ===
              category.parent
                .toLowerCase()
                .replace('&', '')
                .split(' ')
                .join('-')
                ? 'active'
                : ''
            }`"
          >
            {{ category.parent.toUpperCase() }}
            <span>{{ category.totalCategories }}</span>
          </a>
        </li>
      </ul>
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

// handle category route
const handleCategoryRoute = async (value: string) => {
  const newCategory = value.toLowerCase().replace("&", "").split(" ").join("-");
  if (props.filter_dropdown) {
    router.push(`/partners-filter-dropdown?category=${newCategory}`);
  } else if (props.filter_offcanvas) {
    router.push(`/partners-filter-offcanvas?category=${newCategory}`);
  } else if (props.load_more) {
    router.push(`/partners-load-more?category=${newCategory}`);
  } else if (props.right_side) {
    router.push(`/shopartnersp-right-sidebar?category=${newCategory}`);
  } else if (props.shop_full) {
    router.push(`/partners-full-width?category=${newCategory}`);
  } else if (props.shop_1600) {
    router.push(`/partners-1600?category=${newCategory}`);
  } else {
    router.push(`/partners?category=${newCategory}`);
  }

  await partnerStore.fetchPartners(null, null, newCategory, 1, 24);
};

const categories_data = ref<ICategory[]>([]);

const updateCategoriesData = async () => {
  categories_data.value = categoryStore.categories.map((category) => {
    return {
      id: category.idCategoria,
      img: "",
      parent: category.nome,
      children: [],
      productType: "",
      products: [],
      totalCategories: category.totalCadastros,
      status: category.ativo ? "active" : "inactive",
    };
  });
};

const fetchAndUpdateProductsCategories = async () => {
  if (!categoryStore.categories.length) {
    await categoryStore.fetchCategories();
    updateCategoriesData();
  }
};

onMounted(() => {
  fetchAndUpdateProductsCategories();
  activeQuery.value = route.query.category;
});

watch(
  () => categoryStore.categories,
  (newCategories) => {
    if (newCategories.length) {
      updateCategoriesData();
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
