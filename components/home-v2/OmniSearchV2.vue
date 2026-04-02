<template>
  <section class="v2-search">
    <div class="v2-search__container">
      <div class="v2-search__box">
        <div class="v2-search__row">
          <svg class="v2-search__icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Busque parceiros, lojas ou categorias com cashback..."
            class="v2-search__input"
            @keyup.enter="handleSearch"
          />
          <button @click="handleSearch" class="v2-search__btn">Buscar</button>
        </div>
        <div class="v2-search__tags">
          <span class="v2-search__tags-label">Populares:</span>
          <button
            v-for="tag in popularTags"
            :key="tag"
            @click="searchQuery = tag; handleSearch()"
            class="v2-search__tag"
          >
            {{ tag }}
          </button>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
const searchQuery = ref('')
const popularTags = ['Eletrônicos', 'Moda', 'Restaurantes', 'Supermercados', 'Farmácias']
const handleSearch = () => {
  if (searchQuery.value.trim()) {
    navigateTo(`/shop?q=${encodeURIComponent(searchQuery.value)}`)
  }
}
</script>

<style scoped>
.v2-search {
  position: relative;
  margin-top: -2rem;
  z-index: 10;
  padding: 0 1rem;
}
.v2-search__container {
  max-width: 900px;
  margin: 0 auto;
}
.v2-search__box {
  background: white;
  border-radius: 1.25rem;
  box-shadow: 0 20px 40px rgba(0,0,0,0.12);
  padding: 1.25rem 1.5rem;
}
.v2-search__row {
  display: flex;
  align-items: center;
  gap: 1rem;
}
.v2-search__icon {
  width: 24px;
  height: 24px;
  color: #2F7785;
  flex-shrink: 0;
}
.v2-search__input {
  flex: 1;
  border: none;
  outline: none;
  font-size: 1rem;
  color: #374151;
  background: transparent;
}
.v2-search__input::placeholder { color: #9ca3af; }
.v2-search__btn {
  flex-shrink: 0;
  background: #98C73A;
  color: #225F6B;
  font-weight: 700;
  padding: 0.6rem 1.5rem;
  border-radius: 9999px;
  border: none;
  cursor: pointer;
  transition: background 0.2s;
  font-size: 0.95rem;
}
.v2-search__btn:hover { background: #8ab832; }
.v2-search__tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  align-items: center;
  margin-top: 1rem;
  padding-top: 1rem;
  border-top: 1px solid #f3f4f6;
}
.v2-search__tags-label {
  font-size: 0.85rem;
  color: #9ca3af;
}
.v2-search__tag {
  font-size: 0.85rem;
  color: #2F7785;
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
  transition: color 0.2s;
}
.v2-search__tag:hover { color: #225F6B; text-decoration: underline; }
</style>
