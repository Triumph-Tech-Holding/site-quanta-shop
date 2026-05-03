<template>
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-brand-list d-flex align-items-center justify-content-between flex-wrap">
      <div v-for="item in brands_data.slice(0, 8)" :key="item.id" class="tp-shop-widget-brand-item">
        <a @click.prevent="handleBrandRoute(item.name)" v-if="item.logo"
          class="cursor-pointer d-flex align-items-center justify-content-center w-100 h-100">
          <img :src="item.logo" alt="logo" class="w-50 grayscale" />
        </a>
        <a @click.prevent="handleBrandRoute(item.name)" v-else class="cursor-pointer">{{ item.name }}</a>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useBrandStore } from "@/pinia/useBrandStore";
import type { IBrand } from "@/types/brand-type";

const router = useRouter();
const brandStore = useBrandStore();
const props = defineProps<{ shop_full?: boolean }>();

const handleBrandRoute = (value: string) => {
  const newBrand = value.toLowerCase().replace("&", "").split(" ").join("-");
  const target = props.shop_full ? '/shop-full-width' : '/shop';
  router.push(`${target}?brand=${newBrand}`);
};

const brands_data = ref<IBrand[]>([]);

const updateBrandsData = () => {
  brands_data.value = brandStore.bestBrands.map((brand) => ({
    id: brand.brand_id,
    products: [],
    totalProducts: brand.total_products,
    name: brand.brand_name,
    description: "",
    email: "",
    website: "",
    location: "",
    status: "active",
    logo: brand.brand_image_url,
  }));
};

onMounted(async () => {
  if (!brandStore.bestBrands.length) await brandStore.fetchBestBrands();
  updateBrandsData();
});

watch(() => brandStore.bestBrands, (newVal) => { if (newVal.length) updateBrandsData(); }, { immediate: true });
</script>

<style scoped>
.grayscale { filter: grayscale(100%); }
</style>
