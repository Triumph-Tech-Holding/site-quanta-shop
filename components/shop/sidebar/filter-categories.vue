<template>
  <ui-loading-indicator />
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-categories">
      <ul>
        <li v-for="category in productsCategory_data" :key="category.id">
          <a @click.prevent="handleCategoryRoute(category.parent)"
            :class="`cursor-pointer ${activeQuery === category.parent.toLowerCase().replace('&','').split(' ').join('-') ? 'active' : ''}`">
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
const props = defineProps<{ shop_full?: boolean }>();

const handleCategoryRoute = (value: string) => {
  loadingStore.setLoading(true);
  const newCategory = value.toLowerCase().replace("&", "").split(" ").join("-");
  const queryParamsInRoute = Object.keys(route.params);
  store.category = value;
  store.actualPage = 1;

  const queryString = Object.entries(route.query)
    .filter(([key]) => !queryParamsInRoute.includes(key) && key !== 'category')
    .map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(String(value))}`)
    .join('&');
  const fullQueryString = queryString ? `&${queryString}` : '';

  router.push(`${props.shop_full ? '/shop-full-width' : '/shop'}?category=${newCategory}${fullQueryString}`);
  productStore.fetchProducts(store.totalItensPerPage, store.actualPage, store.priceValues[0], store.priceValues[1], store.selectVal, value);
  setTimeout(() => { loadingStore.setLoading(false); }, 1100);
};

const productsCategory_data = ref<ICategory[]>([]);

const updateProductsCategoryData = () => {
  productsCategory_data.value = categoryStore.productsCategories.map((category) => ({
    id: category.category_id,
    img: category.brand_image_url,
    parent: category.category_name,
    children: [],
    productType: "",
    products: [],
    totalCategories: category.total_categories,
    status: "active",
  }));
};

onMounted(async () => {
  await categoryStore.fetchProductsCategories();
  updateProductsCategoryData();
  activeQuery.value = route.query.category as string;
});

watch(() => categoryStore.productsCategories, (v) => { if (v.length) updateProductsCategoryData(); }, { immediate: true });
watch(() => route.query, (q) => { activeQuery.value = q.category as string; });
</script>
