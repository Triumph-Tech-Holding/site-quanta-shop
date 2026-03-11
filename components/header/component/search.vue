<template>
  <div class="tp-header-search search-menu-mobile">
    <form @submit.prevent="handleSubmit">
      <div class="tp-header-search-wrapper d-flex align-items-center bg-white">
        <div class="tp-header-search-box">
          <input type="text" placeholder="Buscar produtos, marcas e muito mais…" v-model="searchText" />
        </div>
        
        <div class="tp-header-search-btn">
          <button class="bg-white" type="submit">
            <SvgSearch />
          </button>
        </div>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
const router = useRouter();
let searchText = ref<string>('');
let productType = ref<string>('');

const changeHandler = (e: { value: string; text: string }) => {
  productType.value = e.value;
};
// handleSubmit
const handleSubmit = () => {
  if(!searchText.value && !productType.value){
    return
  }
  else if(searchText.value && productType.value){
    router.push(`/search?searchText=${searchText.value}&productType=${productType.value}`)
  }
  else if(searchText.value && !productType.value){
    router.push(`/search?searchText=${searchText.value}`)
  }
  else if(!searchText.value && productType.value){
    router.push(`/search?productType=${productType.value}`)
  }
  else{
    router.push(`/search`)
  }
}
</script>
<style scoped>
.tp-header-search-box {
    width: 100%;
}
.tp-header-search-wrapper {
    margin-left: 0px;
  }
@media screen and(min-width: 992px) {
  .search-menu-mobile{
    padding-left: 70px;
  }
  .tp-header-search-box {
    width: 58%;
  }
  .tp-header-search-wrapper {
    margin-left: 10px;
  }
}
</style>
