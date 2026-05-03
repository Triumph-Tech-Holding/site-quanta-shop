<template>
  <section class="qs-auth-split">
    <div class="qs-auth-brand qs-auth-brand--register">
      <div class="qs-auth-brand__inner">
        <nuxt-link href="/" class="qs-auth-brand__logo-link">
          <img src="/img/logo/logo-white.png" alt="Quanta Shop" />
        </nuxt-link>
        <h2 class="qs-auth-brand__headline">Comece a ganhar cashback hoje mesmo</h2>
        <p class="qs-auth-brand__sub">Crie sua conta gratuita e acesse centenas de lojas parceiras com cashback real.</p>
        <ul class="qs-auth-brand__benefits">
          <li>
            <span class="qs-auth-brand__check">✓</span>
            Cadastro 100% gratuito, sem taxas
          </li>
          <li>
            <span class="qs-auth-brand__check">✓</span>
            Cashback em compras online e físicas
          </li>
          <li>
            <span class="qs-auth-brand__check">✓</span>
            Saque via PIX quando quiser
          </li>
          <li>
            <span class="qs-auth-brand__check">✓</span>
            Indique amigos e ganhe ainda mais
          </li>
        </ul>
      </div>
    </div>

    <div class="qs-auth-form-panel">
      <div class="qs-auth-form-panel__inner">
        <div class="tp-login-top text-center mb-30">
          <h3 class="tp-login-title">Crie sua conta</h3>
          <p class="qs-login-subtitle">
            Já tem cadastro?
            <nuxt-link href="/login" class="qs-link">Entrar</nuxt-link>
          </p>
        </div>
        <div class="tp-login-option">
          <forms-register-form />
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
</script>

<style scoped>
.qs-auth-split {
  display: grid;
  grid-template-columns: 40% 60%;
  min-height: calc(100vh - 160px);
}

.qs-auth-brand {
  background: linear-gradient(135deg, #1a4a54 0%, #225F6B 40%, #2F7785 100%);
  display: flex;
  align-items: center;
  padding: 60px 48px;
  position: relative;
  overflow: hidden;
}

.qs-auth-brand--register {
  background: linear-gradient(135deg, #0d3d47 0%, #1a4a54 30%, #225F6B 70%, #2F7785 100%);
}

.qs-auth-brand::before {
  content: '';
  position: absolute;
  top: -30%;
  right: -20%;
  width: 80%;
  height: 80%;
  background: rgba(255, 255, 255, 0.04);
  border-radius: 50%;
  pointer-events: none;
}

.qs-auth-brand::after {
  content: '';
  position: absolute;
  bottom: -20%;
  left: -15%;
  width: 60%;
  height: 60%;
  background: rgba(152, 199, 58, 0.06);
  border-radius: 50%;
  pointer-events: none;
}

.qs-auth-brand__inner {
  position: relative;
  z-index: 1;
  max-width: 380px;
}

.qs-auth-brand__logo-link {
  display: block;
  margin-bottom: 48px;
}

.qs-auth-brand__logo-link img {
  height: 38px;
  width: auto;
}

.qs-auth-brand__headline {
  font-family: 'Inter', sans-serif;
  font-size: clamp(22px, 2.4vw, 34px);
  font-weight: 800;
  color: #fff;
  letter-spacing: -0.03em;
  line-height: 1.2;
  margin-bottom: 16px;
}

.qs-auth-brand__sub {
  font-size: 16px;
  color: rgba(255, 255, 255, 0.72);
  line-height: 1.65;
  margin-bottom: 40px;
  font-family: 'Inter', sans-serif;
}

.qs-auth-brand__benefits {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.qs-auth-brand__benefits li {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 15px;
  color: rgba(255, 255, 255, 0.85);
  font-family: 'Inter', sans-serif;
}

.qs-auth-brand__check {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 22px;
  height: 22px;
  min-width: 22px;
  background: #98C73A;
  color: #fff;
  border-radius: 50%;
  font-size: 11px;
  font-weight: 800;
}

.qs-auth-form-panel {
  background: #fff;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding: 60px 48px;
  overflow-y: auto;
}

.qs-auth-form-panel__inner {
  width: 100%;
  max-width: 480px;
}

.qs-login-subtitle {
  font-family: 'Inter', sans-serif;
  color: #718096;
  font-size: 15px;
  margin-top: 4px;
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

@media (max-width: 900px) {
  .qs-auth-split {
    grid-template-columns: 1fr;
    min-height: auto;
  }

  .qs-auth-brand {
    padding: 40px 28px;
  }

  .qs-auth-brand__logo-link {
    margin-bottom: 24px;
  }

  .qs-auth-brand__headline {
    font-size: 22px;
  }

  .qs-auth-brand__sub {
    margin-bottom: 24px;
    font-size: 14px;
  }

  .qs-auth-form-panel {
    padding: 40px 24px;
  }
}
</style>
