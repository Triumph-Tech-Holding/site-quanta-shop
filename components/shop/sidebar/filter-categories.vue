<template>
  <ui-loading-indicator />
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-categories">
      <ul>
        <li v-for="category in productsCategory_data" :key="category.id">
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
            {{ category.parent }}
            <span>{{ category.totalCategories }}</span>
          </a>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useCategoryStore } from "@/pinia/useCategoryStore";
import type { ICategory } from "@/types/category-type";
import { useProductFilterStore } from "@/pinia/useProductFilterStore";
import { useProductStore } from "@/pinia/useProductStore";
import { useLoadingStore } from "@/pinia/useLoadingStore";

const loadingStore = useLoadingStore();

const categoryStore = useCategoryStore();
const store = useProductFilterStore();
const productStore = useProductStore();
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
const handleCategoryRoute = (value: string) => {
  loadingStore.setLoading(true);
  const newCategory = value.toLowerCase().replace("&", "").split(" ").join("-");
  const queryParamsInRoute = Object.keys(route.params);
  
  store.category = value;
  store.actualPage = 1;

  // Remover 'category' da query se já existir
  const queryString = Object.entries(route.query)
    .filter(([key]) => !queryParamsInRoute.includes(key) && key !== 'category') // Filtrar 'category'
    .map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(String(value))}`)
    .join('&');

  const fullQueryString = queryString ? `&${queryString}` : '';

  if (props.filter_dropdown) {
    router.push(`/shop-filter-dropdown?category=${newCategory}${fullQueryString}`);
  } else if (props.filter_offcanvas) {
    router.push(`/shop-filter-offcanvas?category=${newCategory}${fullQueryString}`);
  } else if (props.load_more) {
    router.push(`/shop-load-more?category=${newCategory}${fullQueryString}`);
  } else if (props.right_side) {
    router.push(`/shop-right-sidebar?category=${newCategory}${fullQueryString}`);
  } else if (props.shop_full) {
    router.push(`/shop-full-width?category=${newCategory}${fullQueryString}`);
  } else if (props.shop_1600) {
    router.push(`/shop-1600?category=${newCategory}${fullQueryString}`);
  } else {
    router.push(`/shop?category=${newCategory}${fullQueryString}`);
  }

  productStore.fetchProducts(store.totalItensPerPage, store.actualPage, store.priceValues[0], store.priceValues[1], store.selectVal, value);

  setTimeout(() =>{
    loadingStore.setLoading(false);
  }, 1100);
};

const productsCategory_data = ref<ICategory[]>([]);

const updateProductsCategoryData = () => {
  productsCategory_data.value = categoryStore.productsCategories.map(
    (category) => ({
      id: category.category_id,
      img: category.brand_image_url,
      parent: category.category_name,
      children: [],
      productType: "",
      products: [],
      totalCategories: category.total_categories,
      status: "active",
    })
  );
};

const fetchAndUpdateProductsCategories = async () => {
  await categoryStore.fetchProductsCategories();
  if (!categoryStore.productsCategories.length) {
  }
  updateProductsCategoryData();
};

onMounted(() => {
  fetchAndUpdateProductsCategories();
});

watch(
  () => categoryStore.productsCategories,
  (newproductsCategories) => {
    if (newproductsCategories.length) {
      updateProductsCategoryData();
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
onMounted(() => {
  activeQuery.value = route.query.category;
});
</script>
