<template>
  <section class="tp-login-area pb-140 p-relative z-index-1 fix">
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-xl-5 col-lg-7 col-md-9">
          <div class="tp-login-wrapper">
            <div class="tp-login-top text-center mb-30">
              <div class="qs-login-icon mb-16">
                <span class="qs-login-icon-ring">
                  <i class="fa-solid fa-user"></i>
                </span>
              </div>
              <h3 class="tp-login-title">Bem-vindo de volta!</h3>
              <p class="qs-login-subtitle">Entre na sua conta e ative seu cashback</p>
            </div>
            <div class="tp-login-option">
              <forms-login-form />
            </div>
            <div class="qs-login-footer text-center mt-24">
              <p>
                Não tem conta?
                <nuxt-link href="/register" class="qs-link">Cadastre-se grátis</nuxt-link>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
</script>

<style scoped>
.qs-login-icon {
  display: flex;
  justify-content: center;
  margin-bottom: 16px;
}

.qs-login-icon-ring {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 64px;
  height: 64px;
  background: linear-gradient(135deg, #225F6B 0%, #2F7785 100%);
  border-radius: 50%;
  color: #fff;
  font-size: 24px;
  box-shadow: 0 4px 20px rgba(47, 119, 133, 0.30);
}

.qs-login-subtitle {
  font-family: 'Inter', 'Jost', sans-serif;
  color: #718096;
  font-size: 15px;
  margin-top: 4px;
  margin-bottom: 0;
}

.qs-login-footer p {
  font-family: 'Inter', 'Jost', sans-serif;
  color: #718096;
  font-size: 14px;
  margin-bottom: 0;
}

.qs-link {
  color: #1e5d68;
  font-weight: 600;
  text-decoration: underline;
  text-underline-offset: 2px;
}

.qs-link:hover {
  color: #2a8a9a;
}

.mb-16 {
  margin-bottom: 16px;
}

.mt-24 {
  margin-top: 24px;
}
</style>
