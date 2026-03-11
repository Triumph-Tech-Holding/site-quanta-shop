<template>
  <ui-loading-indicator />

  <div v-if="product">
    <product-details-breadcrumb :product="product" />
    <product-details-area :product="product" />
    <product-related
      :product-id="product.id"
      :category="product.category.name"
    />
  </div>
</template>

<script setup lang="ts">
import { useLoadingStore } from "@/pinia/useLoadingStore";
import { useProductStore } from "@/pinia/useProductStore";

const loadingStore = useLoadingStore();
const productStore = useProductStore();
const route = useRoute();
const product = computed(() => productStore.product);

async function loadProduct(productId: string) {
  loadingStore.setLoading(true);

  try {
    await productStore.fetchProduct(productId);
  } catch (error) {
    console.error("Error fetching product:", error);
  } finally {
    loadingStore.setLoading(false);
  }
}

onBeforeMount(async () => {
  productStore.product = undefined;
  await loadProduct(route.params.id as string);
});

onMounted(() => {
  useHead({
      title: computed(() => product.value ? `${product.value.title} | Quanta Shop` : 'Produto | Quanta Shop'),
      meta: [
        { name: 'description', content: computed(() => product.value?.description || 'Descrição do produto na Quanta Shop') },
        { property: 'og:title', content: computed(() => product.value ? `${product.value.title} | Quanta Shop` : 'Produto | Quanta Shop') },
        { property: 'og:description', content: computed(() => product.value?.description || 'Descrição do produto na Quanta Shop') },
        { property: 'og:image', content: computed(() => product.value?.img || 'https://quantashop.com.br/default-product-image.jpg') },
        { property: 'og:url', content: computed(() => `https://quantashop.com.br/product-details/${route.params.id}`) },
        { property: 'og:type', content: 'product' },
        { property: 'og:site_name', content: product.value?.merchant.name },
        { name: 'twitter:card', content: 'summary_large_image' },
        { name: 'twitter:title', content: computed(() => product.value ? `${product.value.title} | Quanta Shop` : 'Produto | Quanta Shop') },
        { name: 'twitter:description', content: computed(() => product.value?.description || 'Descrição do produto na Quanta Shop') },
        { name: 'twitter:image', content: computed(() => product.value?.img || 'https://quantashop.com.br/default-product-image.jpg') },
    ],
    link: [
      { rel: 'canonical', href: computed(() => `https://quantashop.com.br/product-details/${route.params.id}`) }
    ]
  });
});


watch(product, (newProduct) => {
  if (newProduct) {
    useHead({
      script: [
        {
          type: 'application/ld+json',
          children: JSON.stringify({
            '@context': 'https://schema.org',
            '@type': 'Product',
            name: newProduct.title,
            description: newProduct.description,
            image: newProduct.img,
            sku: newProduct.id,
            offers: {
              '@type': 'Offer',
              price: newProduct.price,
              priceCurrency: 'BRL', // Ajuste conforme necessário
              availability: 'https://schema.org/InStock', // Ajuste conforme o status do produto
              url: `https://quantashop.com.br/product-details/${newProduct.id}`
            }
          })
        }
      ]
    });
  }
}, { immediate: true });
</script>