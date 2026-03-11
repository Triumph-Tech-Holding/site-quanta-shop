<template>
  <div>
    <ui-loading-indicator />

    <!-- breadcrumb start -->
    <breadcrumb-1 title="Resultado da pesquisa" subtitle="Busca" />
    <!-- breadcrumb end -->

    <!-- shop area start -->
    <section class="tp-shop-area pb-120">
      <div class="container">
        <div class="row">
          <div class="col-xl-12 col-lg-12">
            <div class="tp-shop-main-wrapper">
              <div class="tp-shop-top mb-45">
                <div class="row">
                  <div class="col-xl-6">
                    <div class="tp-shop-top-left d-flex align-items-center">
                      <div class="tp-shop-top-result">
                        <p>
                          Exibindo 1–{{
                            store.searchFilteredItems?.slice(0, perView).length
                          }}
                          de {{ store.totalItems }} resultados
                        </p>
                      </div>
                    </div>
                  </div>
                  <div class="col-xl-6">
                    <shop-sidebar-filter-select
                      @handle-select-filter="store.handleSelectFilter"
                    />
                  </div>
                </div>
              </div>
              <div class="tp-shop-items-wrapper tp-shop-item-primary">
                <div>
                  <div class="row infinite-container">
                    <div
                      v-for="item in store.searchFilteredItems?.slice(
                        0,
                        perView
                      )"
                      :key="item.id"
                      class="col-xl-3 col-md-6 col-sm-6 infinite-item"
                    >
                      <search-item :item="item" :spacing="true" />
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div class="text-center" v-if="!loadingStore.isLoading">
              <button
                v-if="
                  store.searchFilteredItems && perView < productStore.totalCount
                "
                @click="handlePerView"
                type="button"
                class="btn-loadmore tp-btn tp-btn-border tp-btn-border-primary"
              >
                Carregar mais resultados
              </button>

              <p v-if="!store.searchFilteredItems.length" class="btn-loadmore-text">Nenhum resultado encontrado</p>
            </div>
          </div>
        </div>
      </div>
    </section>
    <!-- shop area end -->
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from "vue-router";
import { useProductFilterStore } from "@/pinia/useProductFilterStore";
import { useProductStore } from "@/pinia/useProductStore";
import { useLoadingStore } from "@/pinia/useLoadingStore";

const loadingStore = useLoadingStore();
const store = useProductFilterStore();
const productStore = useProductStore();
const route = useRoute();
const router = useRouter();

let perView = ref<number>(8);
let page = ref<number>(1);

async function handlePerView() {
  loadingStore.setLoading(true);

  try {
    await new Promise((resolve) => setTimeout(resolve, 1100));
    if (perView.value % 12 === 0) {
      page.value++;

      await productStore.fetchSearchedProducts(
        store.totalItensPerPage,
        page.value,
        0,
        store.maxProductPrice.value,
        store.selectVal,
        route.query.searchText ? route.query.searchText.toString() : ""
      );

      store.searchFilteredItems.push(...productStore.products);
      store.totalItems = productStore.totalCount;
    }

    perView.value = perView.value + 4;
  } catch (error) {
    console.error("Error fetching product:", error);
  } finally {
    loadingStore.setLoading(false);
  }
}

onBeforeMount(() => {
  if (route.query.searchText) {
    useSeoMeta({ title: `${route.query.searchText} | Quanta Shop` });
  }
});

onMounted(async() => {
  if (!route.query.searchText) {
    router.push("/");
  }
});
</script>
