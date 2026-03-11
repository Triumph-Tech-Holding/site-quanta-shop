<template>
  <div class="offcanvas__category pb-20">
    <button @click="toggleCategoryActive" class="tp-offcanvas-category-toggle">
      <i class="fa-solid fa-bars"></i>
      Todas as categorias
    </button>
    <div class="tp-category-mobile-menu">
      <nav
        :class="`tp-category-menu-content ${isCategoryActive ? 'active' : ''}`"
      >
        <ul :class="isCategoryActive ? 'active' : ''">
          <li
            v-for="(item, i) in filterCategories"
            :key="i"
            class="has-dropdown"
            @click="handleParentCategory(item.nome)"
          >
            <a class="cursor-pointer">
              <span v-if="item.img">
                <img
                  :src="item.img"
                  alt="cate img"
                  style="width: 50px; height: 50px; object-fit: contain"
                />
              </span>
              <span>{{ item.nome }}</span>
              <button v-if="item.children" class="dropdown-toggle-btn">
                <i class="fa-regular fa-angle-right"></i>
              </button>
            </a>

            <ul
              v-if="item.children"
              :class="`tp-submenu ${
                openCategory === item.parent ? 'active' : ''
              }`"
            >
              <li v-for="(child, i) in item.children" :key="i">
                <a class="cursor-pointer">{{ child }}</a>
              </li>
            </ul>
          </li>
        </ul>
      </nav>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useCategoryStore } from "@/pinia/useCategoryStore";

const router = useRouter();
const categoryStore = useCategoryStore();
const filterCategories = computed(() => categoryStore.categories);

let isCategoryActive = ref<boolean>(false);
let openCategory = ref<string>("");

const handleParentCategory = (value: string) => {
  const newCategory = value.toLowerCase().replace("&", "").split(" ").join("-");
  router.push(`/shop?c=${newCategory}`);
};
const toggleCategoryActive = () => {
  isCategoryActive.value = !isCategoryActive.value;
};
</script>
