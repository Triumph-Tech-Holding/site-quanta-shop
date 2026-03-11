<template>
  <div>
      <!-- breadcrumb start -->
      <breadcrumb-4 title="Login" subtitle="Login" :center="true" />
      <!-- breadcrumb end -->

      <!-- login area start -->
      <login-area/>
      <!-- login area end -->
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from "@/pinia/useUserStore";

useSeoMeta({ title: "Entrar | Quanta Shop" });

const userStore = useUserStore();

onBeforeMount(() => {
  console.log("Login page mounted");

  const userFromLocalStorage = localStorage.getItem("user");

  if (userFromLocalStorage) {
    console.log("User from local storage:", userFromLocalStorage);

    const user = JSON.parse(userFromLocalStorage);

    if (user) {
      console.log("User:", user);
    }

    // logout user
    userStore.logout();

    // clear session storage
    //sessionStorage.clear();
  }
});
</script>
