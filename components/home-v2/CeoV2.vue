<template>
  <section class="v2-ceo">
    <div class="v2-ceo__container">
      <div class="v2-ceo__grid">
        <div class="v2-ceo__image-col">
          <div class="v2-ceo__image-wrap">
            <div class="v2-ceo__image-bg"></div>
            <img
              src="/img/users/mauro-triumph.jpg"
              alt="Mauro Triumph - CEO da Quanta"
              class="v2-ceo__image"
            />
          </div>
        </div>
        <div class="v2-ceo__content">
          <h2 class="v2-ceo__title">Sua Experiência Segura e Inovadora com Nosso CEO.</h2>
          <p class="v2-ceo__text">
            Mauro Triumph garante: tecnologia de ponta, transparência total e o melhor cashback do Brasil para o seu dinheiro. Estamos construindo o futuro do consumo.
          </p>
          <NuxtLink to="/contato" class="v2-ceo__cta">
            Falar com o CEO
            <svg width="20" height="20" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z" />
            </svg>
          </NuxtLink>
          <div class="v2-ceo__badges">
            <div class="v2-ceo__badge">
              <svg width="22" height="22" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
              </svg>
              <span>100% Seguro</span>
            </div>
            <div class="v2-ceo__badge">
              <svg width="22" height="22" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
              </svg>
              <span>Transparência Total</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.v2-ceo {
  background: #2F7785;
  padding: 4rem 1rem;
}
@media (min-width: 1024px) { .v2-ceo { padding: 5rem 2rem; } }
.v2-ceo__container {
  max-width: 1280px;
  margin: 0 auto;
}
.v2-ceo__grid {
  display: grid;
  gap: 2rem;
  align-items: center;
}
@media (min-width: 1024px) {
  .v2-ceo__grid {
    grid-template-columns: 1fr 1fr;
    gap: 3rem;
  }
}
.v2-ceo__image-col {
  order: 2;
  display: flex;
  justify-content: center;
}
@media (min-width: 1024px) { .v2-ceo__image-col { order: 1; justify-content: flex-start; } }
.v2-ceo__image-wrap { position: relative; display: inline-block; }
.v2-ceo__image-bg {
  position: absolute;
  inset: 0;
  background: rgba(152, 199, 58, 0.3);
  border-radius: 1.5rem;
  transform: rotate(-3deg);
}
.v2-ceo__image {
  position: relative;
  width: 280px;
  height: auto;
  border-radius: 1.5rem;
  box-shadow: 0 25px 50px rgba(0,0,0,0.25);
  object-fit: cover;
}
@media (min-width: 768px) { .v2-ceo__image { width: 320px; } }
@media (min-width: 1024px) { .v2-ceo__image { width: 380px; } }
.v2-ceo__content {
  order: 1;
  text-align: center;
}
@media (min-width: 1024px) { .v2-ceo__content { order: 2; text-align: left; } }
.v2-ceo__title {
  font-size: 1.75rem;
  font-weight: 700;
  color: white;
  line-height: 1.2;
  margin-bottom: 1rem;
}
@media (min-width: 1024px) { .v2-ceo__title { font-size: 2.25rem; } }
.v2-ceo__text {
  font-size: 1.1rem;
  color: rgba(255,255,255,0.9);
  margin-bottom: 2rem;
  max-width: 480px;
  margin-left: auto;
  margin-right: auto;
}
@media (min-width: 1024px) {
  .v2-ceo__text { margin-left: 0; margin-right: 0; }
}
.v2-ceo__cta {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  background: #F5C518;
  color: #225F6B;
  font-weight: 700;
  font-size: 1.05rem;
  padding: 0.9rem 2rem;
  border-radius: 9999px;
  text-decoration: none;
  transition: background 0.2s;
}
.v2-ceo__cta:hover { background: #e6b800; color: #225F6B; }
.v2-ceo__badges {
  display: flex;
  flex-wrap: wrap;
  gap: 1.5rem;
  margin-top: 2rem;
  justify-content: center;
}
@media (min-width: 1024px) { .v2-ceo__badges { justify-content: flex-start; } }
.v2-ceo__badge {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: rgba(255,255,255,0.8);
  font-size: 0.9rem;
}
</style>
