import { defineStore } from 'pinia';
import { ref } from 'vue';
import { getProduct, getProducts, getSearchedProducts } from '@/services/product-service';
import type { IProduct } from '@/types/product-type';
import { useUserStore } from '@/pinia/useUserStore';
import { useLoadingStore } from '@/pinia/useLoadingStore';
import { useProductFilterStore } from '@/pinia/useProductFilterStore';

interface ApiResponse {
  success: boolean;
  data: {
    products: any[],
    product: any,
    totalCount: number,
    priceInterval: any
  };
}

export const useProductStore = defineStore('product', () => {
  const product = ref<IProduct>();
  const products = ref<IProduct[]>([]);
  const totalCount = ref<number>(0);
  const priceInterval = ref<any>({});
  const isProductsLoaded = ref(false);
  const userStore = useUserStore();
  const loadingStore = useLoadingStore();
  const productFilterStore = useProductFilterStore();

  let activeImg = ref<string>('');
  let openFilterDropdown = ref<boolean>(false);
  let openFilterOffcanvas = ref<boolean>(false);

  function setProduct(newProduct: IProduct) {
    product.value = newProduct;
  }

  function setProducts(newProducts: IProduct[]) {
    products.value = newProducts;
  }

  function setTotalCount(newCount: number) {
    totalCount.value = newCount;
  }

  async function fetchProduct(id: string) {
    // if (isProductsLoaded.value) return;

    try {
      const response = await getProduct(id);

      if (response.success) {
        userStore.loadUserFromStorage();

        const product = response.data;

        const newProduct: IProduct = {
          id: product.aw_product_id,
          sku: product.aw_product_id,
          img: product.merchant_image_url,
          title: product.product_name,
          slug: '',
          unit: '',
          url: '',
          imageURLs: [],
          parent: product.category_name,
          children: '',
          price: parseFloat(product.search_price),
          discount: 0,
          quantity: 0,
          merchant: {
            id: product.merchant_id,
            name: product.merchant_name,
            cashback: product.cashback
          },
          brand: {
            name: product.brand_name
          },
          category: {
            name: product.category_name
          },
          status: 'out-of-stock',
          productType: product.category_name,
          description: product.description,
          additionalInformation: [],
          sellCount: 0,
          link: userStore.userId ? product.aw_deep_link.replace('{userId}', userStore.userId) : null
        };

        setProduct(newProduct);
        setTotalCount(response.data.totalCount);

        activeImg.value = newProduct.img;

      }
    } catch (error) {
      console.error('Erro ao buscar produtos:', error);
    }
  }

  async function fetchProducts(quantity: number, page: number, minPrice: any, maxPrice: any, order: any, category: any) {
    loadingStore.setLoading(true);

    try {
      const response: ApiResponse = await getProducts(quantity, page, minPrice, maxPrice, order, category);

      if (response.success) {
        userStore.loadUserFromStorage();

        const newProducts: IProduct[] = response.data.products.map(product => ({
          id: product.aw_product_id,
          sku: product.aw_product_id,
          img: product.merchant_image_url,
          title: product.product_name,
          slug: '',
          unit: '',
          url: '',
          imageURLs: [product.merchant_image_url],
          parent: product.category_name,
          children: '',
          price: parseFloat(product.search_price),
          discount: 0,
          quantity: 0,
          merchant: {
            id: product.merchant_id,
            name: product.merchant_name,
            cashback: product.cashback
          },
          brand: {
            name: product.brand_name
          },
          category: {
            name: product.category_name
          },
          status: 'out-of-stock',
          productType: product.category_name,
          description: product.description,
          additionalInformation: [],
          sellCount: 0,
          link: userStore.userId ? product.aw_deep_link.replace('{userId}', userStore.userId) : null
        }));

        setProducts(newProducts);
        setTotalCount(response.data.totalCount);

        activeImg.value = newProducts[0].img;

        priceInterval.value = response.data.priceInterval;
        productFilterStore.priceValues = ([Number(priceInterval.value.minValue), Number(priceInterval.value.maxValue)]);

        isProductsLoaded.value = true;
      }
    } catch (error) {
      console.error('Erro ao buscar produtos:', error);
    }
    finally {
      loadingStore.setLoading(false);
    }
  }

  async function fetchSearchedProducts(quantity: number, page: number, minPrice: any, maxPrice: any, order: any, searchText: string) {
    loadingStore.setLoading(true);

    try {
      const response: ApiResponse = await getSearchedProducts(quantity, page, minPrice, maxPrice, order, searchText);

      if (response.success) {
        userStore.loadUserFromStorage();

        const newProducts: IProduct[] = response.data.products.map(product => ({
          id: product.aw_product_id,
          sku: product.aw_product_id,
          img: product.merchant_image_url,
          title: product.product_name,
          slug: '',
          unit: '',
          url: '',
          imageURLs: [],
          parent: product.category_name,
          children: '',
          price: parseFloat(product.search_price),
          discount: 0,
          quantity: 0,
          merchant: {
            id: product.merchant_id,
            name: product.merchant_name,
            cashback: product.cashback
          },
          brand: {
            name: product.brand_name
          },
          category: {
            name: product.category_name
          },
          status: 'out-of-stock',
          productType: product.category_name,
          description: product.description,
          additionalInformation: [],
          sellCount: 0,
          link: userStore.userId ? product.aw_deep_link.replace('{userId}', userStore.userId) : null,
          type: product.type
        }));

        setProducts(newProducts);
        setTotalCount(response.data.totalCount);

        //activeImg.value = newProducts[0].img;

        priceInterval.value = response.data.priceInterval;        
        productFilterStore.priceValues = ([Number(priceInterval.value.minValue), Number(priceInterval.value.maxValue)]);

        isProductsLoaded.value = true;
      }
    } catch (error) {
      console.error('Erro ao buscar produtos:', error);
    }
    finally {
      loadingStore.setLoading(false);
    }
  }

  // handle image active
  const handleImageActive = (img: string) => {
    activeImg.value = img;
  };

  // handle image active
  const handleOpenFilterDropdown = () => {
    openFilterDropdown.value = !openFilterDropdown.value
  };

  // handle image active
  const handleOpenFilterOffcanvas = () => {
    openFilterOffcanvas.value = !openFilterOffcanvas.value
  };

  return {
    product,
    products,
    totalCount,
    priceInterval,
    fetchProduct,
    fetchProducts,
    fetchSearchedProducts,
    activeImg,
    handleImageActive,
    handleOpenFilterDropdown,
    openFilterDropdown,
    openFilterOffcanvas,
    handleOpenFilterOffcanvas,
  };
});
