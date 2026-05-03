<template>
  <div class="tp-shop-widget-content">
    <div class="tp-shop-widget-checkbox">
      <ul class="filter-items filter-checkbox">
        <li v-for="(s, i) in status" :key="i" class="filter-item checkbox">
          <input id="on-sale" type="checkbox" name="on-sale" />
          <label @click="handleStatusRoute(s)" :for="s"
            :class="activeQuery === s.toLowerCase().replace('&','').split(' ').join('-') ? 'active' : ''">
            {{ s }}
          </label>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
const route = useRoute();
const router = useRouter();
const status = ref<string[]>(["On sale", "In Stock"]);
const activeQuery = ref<string>("");
const props = defineProps<{ shop_full?: boolean }>();

const handleStatusRoute = (s: string) => {
  const newStatus = s.toLowerCase().replace("&", "").split(" ").join("-");
  const target = props.shop_full ? '/shop-full-width' : '/shop';
  router.push(`${target}?status=${newStatus}`);
};

watch(() => route.query, (q) => { activeQuery.value = q.status as string; });
onMounted(() => { activeQuery.value = route.query.status as string; });
</script>
