<template>
  <div class="tp-header-category tp-category-menu tp-header-category-toggle">
    <button @click="handleActive" class="tp-category-menu-btn tp-category-menu-toggle">
      <span>
        <SvgDropdown />
      </span>
      Todas as categorias
    </button>
    <Transition>
      <nav v-if="isActive" class="tp-category-menu-content">
        <ul>
          <li v-for="(item, i) in category_items" class="has-dropdown" :key="i">
            <a class="cursor-pointer" @click="handleParentCategory(item.nome)">
              <!-- <span v-if="item.img">
                <img
                  :src="item.img"
                  alt="cate img"
                  style="width: 50px; height: 50px; object-fit: contain"
                />
              </span> -->
              {{ item.nome }}
            </a>

            <!-- <ul v-if="item.children" class="tp-submenu">
              <li v-for="(child, i) in item.children" :key="i">
                <a class="cursor-pointer" @click="handleSubCategory(child)">{{ child }}</a>
              </li>
            </ul> -->
          </li>
        </ul>
      </nav>
    </Transition>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useCategoryStore } from "@/pinia/useCategoryStore";

const router = useRouter();
const categoryStore = useCategoryStore();
const category_items = computed(() => categoryStore.categories);

let isActive = ref<boolean>(false);

// handle active
const handleActive = () => isActive.value = !isActive.value;

// handle parent
const handleParentCategory = (value: string) => {
  const newCategory = value.toLowerCase().replace("&", "").split(" ").join("-");
  router.push(`/partners?categoria=${newCategory}`);
}

// handle parent
const handleSubCategory = (value: string) => {
  const newCategory = value.toLowerCase().replace("&", "").split(" ").join("-");
  router.push(`/partners?categoria=${newCategory}`);
}
</script>

<style scoped>
.v-enter-active,
.v-leave-active {
  transition: opacity 0.5s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
}
</style>
