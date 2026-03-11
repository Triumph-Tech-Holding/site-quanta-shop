import { defineStore } from 'pinia';
import { ref, computed, watch, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useProductStore } from '@/pinia/useProductStore'; // Ajuste o caminho se necessário
import type { IProduct } from '@/types/product-type';
import { useLoadingStore } from "@/pinia/useLoadingStore";


export const useProductFilterStore = defineStore('product_filter', () => {
  const productStore = useProductStore();
  const loadingStore = useLoadingStore();
  const route = useRoute();
  const router = useRouter();
  const selectVal = ref<string>('');
  const totalItensPerPage = ref<number>(12);
  const actualPage = ref<number>(1);
  const totalItems = ref<number>(0);
  const category = ref<string>('');

  const initializeStore = async () => {
    //await productStore.fetchProducts(12, 1, null, null, null, null);
    priceValues.value = ([0, maxProductPrice.value]);
  };

  const product_deal = computed<IProduct[]>(() => productStore.products);
  const product_data = computed<IProduct[]>(() => productStore.products);
  const totalCount = computed<number>(() => productStore.totalCount);
  const priceInterval = computed<any>(() => productStore.priceInterval);

  const maxProductPrice = computed(() =>
    priceInterval.value?.maxValue ?? 0
  );

  const priceValues = ref([0, maxProductPrice.value]);

  const handleSelectFilter = async (e: { value: string; text: string }) => {
    selectVal.value = e.value;

    if (route.query.searchText)
      await handleSearch();
    else
      await productStore.fetchProducts(totalItensPerPage.value, actualPage.value, priceValues.value[0], priceValues.value[1], selectVal.value, category.value)
  };

  const handlePriceChange = (value: number[]) => {
    loadingStore.setLoading(true);
    new Promise((resolve) => setTimeout(resolve, 1100));
    priceValues.value = value;
    loadingStore.setLoading(false);
  };

  const handleResetFilter = async () => {
    actualPage.value = 1;
    priceValues.value = [0, maxProductPrice.value];
    category.value = '';
    selectVal.value = '';    
  };

  // Compute filtered products based on various criteria
  const filteredProducts = computed(() => {
    let products = product_data.value;

    if (route.query.minPrice && route.query.maxPrice) {
      products = products.filter(
        (p) =>
          p.price >= Number(route.query.minPrice) &&
          p.price <= Number(route.query.maxPrice)
      );
    }

    if (route.query.status) {
      if (route.query.status === 'on-sale') {
        products = products.filter((p) => p.discount > 0);
      } else if (route.query.status === 'in-stock') {
        products = products.filter((p) => p.status === 'in-stock');
      }
    } else if (route.query.category) {
      products = products.filter(
        (p) => {
          const parent = p.parent.toLowerCase().replace('&', '').split(' ').join('-');
          const category = route.query.category?.toString().toLowerCase().replace('&', '').split(' ').join('-');

          return parent === category;
        }
      );
    } else if (route.query.subCategory) {
      products = products.filter(
        (p) =>
          p.children.toLowerCase().replace('&', '').split(' ').join('-') ===
          route.query.subCategory
      );
    } else if (route.query.brand) {
      products = products.filter(
        (p) =>
          p.brand.name.toLowerCase().replace('&', '').split(' ').join('-') ===
          route.query.brand
      );
    }

    // Sorting based on selectVal
    if (selectVal.value) {
      if (selectVal.value === 'default-sorting') {
        return products;
      } else if (selectVal.value === 'low-to-high') {
        return [...products].sort((a, b) => a.price - b.price);
      } else if (selectVal.value === 'high-to-low') {
        return [...products].sort((a, b) => b.price - a.price);
      } else if (selectVal.value === 'new-added') {
        return [...products].slice(-8);
      } else if (selectVal.value === 'on-sale') {
        return products.filter((p) => p.discount > 0);
      }
    }

    return products;
  });

  const searchFilteredItems = ref<IProduct[]>([]);

  const handleSearch = async () => {
    await productStore.fetchSearchedProducts(
      totalItensPerPage.value,
      1,
      0,
      maxProductPrice.value,
      selectVal.value,
      route.query.searchText ? route.query.searchText.toString() : ''
    );

    searchFilteredItems.value = productStore.products;
    totalItems.value = productStore.totalCount;
  }

  // Watch route changes to potentially reinitialize store or react to route changes
  watch(
    () => [route.query.searchText, route.query.productType],
    async (newVal, oldVal) => {
      if (route.query.searchText) {
        await handleSearch();
      } else
        await initializeStore();
    },
    { immediate: true }
  );

  onMounted(async () => {
    //await initializeStore();
  });

  return {
    product_deal,
    maxProductPrice,
    priceValues,
    totalItensPerPage,
    totalItems,
    actualPage,
    handleSelectFilter,
    filteredProducts,
    handlePriceChange,
    handleResetFilter,
    selectVal,
    category,
    searchFilteredItems,
  };
});
