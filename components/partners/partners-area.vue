<template>
  <section
    :class="`tp-shop-area pb-120 ${
      full_width ? 'tp-shop-full-width-padding' : ''
    }`"
  >
    <div
      :class="`${
        full_width
          ? 'container-fluid'
          : partners_1600
          ? 'container-shop'
          : 'container'
      }`"
    >
      <div class="row">
        <div
          v-if="!partners_right_side && !partners_no_side"
          class="col-xl-3 col-lg-4"
        >
          <!-- partners sidebar start -->
          <partners-sidebar
            :shop_full="full_width"
            :partners_1600="partners_1600"
          />
          <!-- partners sidebar end -->
        </div>
        <div :class="`${partners_no_side ? 'col-xl-12' : 'col-xl-9 col-lg-8'}`">
          <div class="tp-shop-main-wrapper">
            <div class="tp-shop-top mb-45">
              <div class="row">
                <div class="col-xl-6">
                  <div class="tp-shop-top-left d-flex align-items-center">
                    <div class="tp-shop-top-tab tp-tab">
                      <ul class="nav nav-tabs" id="productTab" role="tablist">
                        <li class="nav-item" role="presentation">
                          <button
                            :class="`nav-link ${
                              active_tab === 'grid' ? 'active' : ''
                            }`"
                            @click="handleActiveTab('grid')"
                          >
                            <svg-grid />
                          </button>
                        </li>
                        <li class="nav-item" role="presentation">
                          <button
                            :class="`nav-link ${
                              active_tab === 'list' ? 'active' : ''
                            }`"
                            @click="handleActiveTab('list')"
                          >
                            <svg-list />
                          </button>
                        </li>
                      </ul>
                    </div>
                    <div class="tp-shop-top-result">
                      <p>
                        Exibindo {{ startIndex + 1 }}–{{
                          startIndex +
                          partners_data?.slice(startIndex, endIndex).length
                        }}
                        de {{ partners_data.length }} resultados
                      </p>
                    </div>
                  </div>
                </div>
                <div class="col-xl-6">
                  <shop-sidebar-filter-select
                    @handle-select-filter="
                      partnerFilterStore.handleSelectFilter
                    "
                  />
                </div>
              </div>
            </div>
            <div class="tp-shop-items-wrapper tp-shop-item-primary">
              <!-- grid -->
              <div v-if="active_tab === 'grid'">
                <div class="row infinite-container">
                  <div
                    v-for="item in partners_data.slice(startIndex, endIndex)"
                    :key="item.id"
                    class="col-xl-4 col-md-6 col-sm-6 infinite-item"
                  >
                    <partners-item :item="item" :spacing="true" />
                  </div>
                </div>
              </div>
              <!-- grid -->

              <!-- list -->
              <div v-if="active_tab === 'list'">
                <div class="row">
                  <div class="col-xl-12">
                    <partners-list-item
                      v-for="item in partners_data.slice(startIndex, endIndex)"
                      :key="item.id"
                      :item="item"
                    />
                  </div>
                </div>
              </div>
              <!-- list -->
            </div>

            <!-- pagination -->
            <div class="tp-shop-pagination mt-20">
              <div
                v-if="partners_data && partners_data.length > 24"
                class="tp-pagination d-flex justify-content-center"
              >
                <ui-pagination
                  :items-per-page="24"
                  :data="partners_data || []"
                  :total-count="partners_data?.length || 0"
                  @handle-paginate="handlePagination"
                />
              </div>
            </div>
            <!-- pagination -->
          </div>
        </div>

        <div
          v-if="partners_right_side && !partners_no_side"
          class="col-xl-3 col-lg-4"
        >
          <!-- shop sidebar start -->
          <shop-sidebar :shop_right="partners_right_side" />
          <!-- shop sidebar end -->
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { usePartnerFilterStore } from "@/pinia/usePartnerFilterStore";
import { usePartnerStore } from "@/pinia/usePartnerStore";

const route = useRoute();
const props = defineProps<{
  list_style?: boolean;
  full_width?: boolean;
  partners_1600?: boolean;
  partners_right_side?: boolean;
  partners_no_side?: boolean;
}>();

const partners_data = computed(() => partnerStore.partners);
const active_tab = ref<string>(props.list_style ? "list" : "grid");
const partnerFilterStore = usePartnerFilterStore();
const partnerStore = usePartnerStore();

let startIndex = ref<number>(0);
let endIndex = ref<number>(24);

const handlePagination = (data: any[], start: number, end: number) => {
  startIndex.value = start;
  endIndex.value = end;
};

function handleActiveTab(tab: string) {
  active_tab.value = tab;
}

const initializeData = async () => {
  const { query } = route;
  const type = query.tipo || null;
  const name = query.nome || null;
  const category = query.category || null;
  const page = query.page || 1;
  const limit = query.limite || 24;

  // Partners
  await partnerStore.fetchPartners(type, name, category, page, limit);
};

onMounted(async () => {
  await initializeData();
});

watch(
  () => {
    route.query || route.params;
  },
  (newStatus) => {
    startIndex.value = 0;
    endIndex.value =
      partners_data && partners_data.length > 24 ? 24 : partners_data?.length!;
  }
);
</script>
