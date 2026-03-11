<template>
    <div class="tp-shop-top-select text-md-end">
      <ui-nice-select
        :options="[
          { value: 'default-sorting', text: 'Ordenação padrão' },
          { value: 'low-to-high', text: 'Mais barato para mais caro' },
          { value: 'high-to-low', text: 'Mais caro para mais barato' },
          { value: 'new-added', text: 'Adicionados recentemente' },
          // { value: 'on-sale', text: 'On Sale' },
        ]"
        name="Select Category"
        :default-current="0"
        @onChange="handleSelect"
      />
    </div>
</template>

<script setup lang="ts">
const emit = defineEmits(['handleSelectFilter'])
const handleSelect = (e: { value: string; text: string }) => {
  emit('handleSelectFilter', e);
};
</script>
