<template>
  <section class="bg-[#2F7785] py-12 lg:py-20">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="grid lg:grid-cols-2 gap-8 lg:gap-12 items-center">
        <!-- Image -->
        <div class="order-2 lg:order-1 flex justify-center lg:justify-start">
          <div class="relative">
            <div class="absolute inset-0 bg-[#98C73A]/30 rounded-3xl transform -rotate-3"></div>
            <img 
              src="/img/users/mauro-triumph.jpg" 
              alt="Mauro Triumph - CEO da Quanta"
              class="relative rounded-3xl shadow-2xl w-72 sm:w-80 lg:w-96 h-auto object-cover"
            />
          </div>
        </div>

        <!-- Content -->
        <div class="order-1 lg:order-2 text-center lg:text-left">
          <h2 class="text-2xl sm:text-3xl lg:text-4xl font-bold text-white leading-tight mb-4">
            Sua Experiencia Segura e Inovadora com Nosso CEO.
          </h2>
          <p class="text-lg text-white/90 mb-8 max-w-lg mx-auto lg:mx-0">
            Mauro Triumph garante: tecnologia de ponta, transparencia total e o melhor cashback do Brasil para o seu dinheiro. Estamos construindo o futuro do consumo.
          </p>
          
          <NuxtLink 
            to="/fale-com-ceo"
            class="inline-flex items-center bg-[#F5C518] text-[#225F6B] font-semibold px-8 py-3.5 rounded-full hover:bg-[#e6b800] transition-colors text-lg"
          >
            Falar com o CEO
            <svg class="w-5 h-5 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z" />
            </svg>
          </NuxtLink>

          <!-- Trust Badges -->
          <div class="flex flex-wrap justify-center lg:justify-start gap-6 mt-8">
            <div class="flex items-center text-white/80">
              <svg class="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
              </svg>
              <span class="text-sm">100% Seguro</span>
            </div>
            <div class="flex items-center text-white/80">
              <svg class="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
              </svg>
              <span class="text-sm">Transparencia Total</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
