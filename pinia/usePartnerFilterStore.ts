import { defineStore } from 'pinia';
import { ref, computed, watch, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useProductStore } from '@/pinia/useProductStore'; // Ajuste o caminho se necessário
import type { IProduct } from '@/types/product-type';

export const usePartnerFilterStore = defineStore('product_filter', () => {
  const productStore = useProductStore();
  const route = useRoute();
  const router = useRouter();
  const selectVal = ref<string>('');

  // const initializeStore = async () => {
  //   if (productStore.products.length === 0) {
  //     await productStore.fetchProducts();
  //   }
  // };

  const product_data = computed<IProduct[]>(() => productStore.products);

  const maxProductPrice = computed(() => {
    return product_data.value.reduce((max, product) =>
      product.price > max ? product.price : max,
      0
    );
  });

  const priceValues = ref([0, maxProductPrice.value]);

  const handleSelectFilter = (e: { value: string; text: string }) => {
    console.log('handle select', e);
    selectVal.value = e.value;
  };

  const handlePriceChange = (value: number[]) => {
    priceValues.value = value;
  };

  const handleResetFilter = () => {
    priceValues.value = [0, maxProductPrice.value];
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

  // const searchFilteredItems = computed(() => {
  //   let filteredProducts = [...product_data.value];

  //   if (route.query.searchText && !route.query.productType) {
  //     filteredProducts = filteredProducts.filter((prd) =>
  //       prd.title.toLowerCase().includes(String(route.query.searchText).toLowerCase())
  //     );
  //   }

  //   if (!route.query.searchText && route.query.productType) {
  //     filteredProducts = filteredProducts.filter(
  //       (prd) => prd.productType.toLowerCase() === String(route.query.productType).toLowerCase()
  //     );
  //   }

  //   if (route.query.searchText && route.query.productType) {
  //     filteredProducts = filteredProducts
  //       .filter(
  //         (prd) => prd.productType.toLowerCase() === String(route.query.productType).toLowerCase()
  //       )
  //       .filter(p => p.title.toLowerCase().includes(String(route.query.searchText).toLowerCase()));
  //   }

  //   switch (selectVal.value) {
  //     case 'default-sorting':
  //       break;
  //     case 'low-to-high':
  //       filteredProducts = filteredProducts.slice().sort((a, b) => a.price - b.price);
  //       break;
  //     case 'high-to-low':
  //       filteredProducts = filteredProducts.slice().sort((a, b) => b.price - a.price);
  //       break;
  //     case 'new-added':
  //       filteredProducts = filteredProducts.slice(-6);
  //       break;
  //     case 'on-sale':
  //       filteredProducts = filteredProducts.filter((p) => p.discount > 0);
  //       break;
  //     default:
  //   }
  //   return filteredProducts;
  // });

  // Watch route changes to potentially reinitialize store or react to route changes
  watch(
    () => route.query || route.path,
    () => { /* Implement logic if needed */ }
  );

  onMounted(async () => {
    // await initializeStore();
  });

  return {
    maxProductPrice,
    priceValues,
    handleSelectFilter,
    filteredProducts,
    handlePriceChange,
    handleResetFilter,
    selectVal,
    // searchFilteredItems,
  };
});
