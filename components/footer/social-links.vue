<template>
  <a v-for="item in social_data" :key="item.id" :href="item.link" target="_blank">
    <i :class="item.icon"></i>
  </a>
</template>

<script setup lang="ts">
const social_data: {
  id: number;
  link: string;
  icon: string;
  title: string;
}[] = [
  {
    id: 1,
    link: "https://www.facebook.com/quantabank",
    icon: "fa-brands fa-facebook-f",
    title: "Facebook",
  },
  {
    id: 2,
    link: "https://www.instagram.com/quantashopoficial",
    icon: "fa-brands fa-instagram",
    title: "Instagram",
  },
  {
    id: 3,
    link: "https://wa.me/message/QFVP2ILERA4LD1",
    icon: "fa-brands fa-whatsapp",
    title: "Linkedin",
  },
  {
    id: 4,
    link: "https://www.youtube.com/@quantabank",
    icon: "fa-brands fa-youtube",
    title: "Vimeo",
  },
];
</script>
