<template>
  <div
    class="tp-login-social mb-10 d-flex flex-wrap align-items-center justify-content-center"
  >
    <div class="tp-login-option-item has-google">
      <!-- <a href="#">
        <img src="/img/icon/login/google.svg" alt="" />
        Entrar com Google
      </a> -->
      <div
        id="g_id_onload"
        data-client_id="372294010028-ff1frn14fg81mn0ujhv215lk9rd5t80r.apps.googleusercontent.com"
        data-login_uri="https://localhost:44385/api/v2/users/login-google"
        data-auto_prompt="false"
      ></div>
      <div
        class="g_id_signin"
        data-type="standard"
        data-size="large"
        data-theme="outline"
        data-text="sign_in_with"
        data-shape="rectangular"
        data-logo_alignment="left"
      ></div>
    </div>
    <div class="tp-login-option-item d-none">
      <a href="#">
        <img src="/img/icon/login/facebook.svg" alt="" />
      </a>
    </div>
    <div class="tp-login-option-item d-none">
      <a href="#">
        <img class="apple" src="/img/icon/login/apple.svg" alt="" />
      </a>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";

onMounted(() => {
  const script = document.createElement("script");
  script.src = "https://accounts.google.com/gsi/client";
  script.async = true;
  document.head.appendChild(script);
});
</script>
