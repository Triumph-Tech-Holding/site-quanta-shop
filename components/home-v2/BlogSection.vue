<template>
  <section class="bg-white py-14 lg:py-20">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <h2 class="text-2xl lg:text-3xl font-bold text-[#225F6B] text-center mb-10">
        Quanta em Tempo Real
      </h2>

      <div class="grid lg:grid-cols-2 gap-8 lg:gap-12">

        <!-- LEFT: Blog posts -->
        <div class="bg-[#F4F4F5] rounded-3xl p-6 lg:p-8">
          <div class="flex items-center gap-2 mb-6">
            <div class="w-1 h-6 bg-[#98C73A] rounded-full"></div>
            <h3 class="text-lg font-bold text-[#225F6B]">Morre o Blog</h3>
          </div>

          <div class="space-y-5">
            <article
              v-for="post in posts"
              :key="post.id"
              class="flex gap-4 p-3 bg-white rounded-2xl shadow-sm hover:shadow-md transition-shadow group cursor-pointer"
            >
              <div class="flex-shrink-0 w-16 h-16 rounded-xl overflow-hidden bg-gray-100">
                <img
                  :src="post.thumbnail"
                  :alt="post.title"
                  class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                />
              </div>
              <div class="flex-1 min-w-0">
                <span class="text-[10px] font-semibold text-[#2F7785] uppercase tracking-wide">{{ post.category }}</span>
                <h4 class="text-sm font-bold text-[#225F6B] leading-snug mt-0.5 line-clamp-2 group-hover:text-[#2F7785] transition-colors">
                  {{ post.title }}
                </h4>
                <p class="text-xs text-gray-500 mt-1 line-clamp-2">{{ post.summary }}</p>
              </div>
            </article>
          </div>

          <div class="mt-6 text-center">
            <NuxtLink
              to="/blog"
              class="inline-flex items-center gap-2 border-2 border-[#225F6B] text-[#225F6B] font-semibold text-sm px-6 py-2.5 rounded-full hover:bg-[#225F6B] hover:text-white transition-colors"
            >
              Ver Todos os Posts
            </NuxtLink>
          </div>
        </div>

        <!-- RIGHT: Fankoo Brands -->
        <div class="relative bg-gradient-to-br from-[#2F7785] to-[#225F6B] rounded-3xl overflow-hidden">
          <div class="absolute top-4 right-4">
            <span class="bg-red-500 text-white text-[10px] font-bold px-2 py-0.5 rounded-full">AO VIVO</span>
          </div>

          <div class="relative h-full flex flex-col">
            <!-- Image area -->
            <div class="relative flex-1 min-h-[260px]">
              <img
                src="/images/fankoo/influencer.png"
                alt="Fankoo Brands"
                class="absolute inset-0 w-full h-full object-cover object-top"
              />
              <div class="absolute inset-0 bg-gradient-to-t from-[#225F6B] via-transparent to-transparent"></div>
            </div>

            <!-- Content -->
            <div class="p-6 lg:p-8">
              <div class="flex items-center gap-2 mb-3">
                <div class="w-1 h-5 bg-[#98C73A] rounded-full"></div>
                <h3 class="text-base font-bold text-white">Fankoo Brands</h3>
              </div>
              <p class="text-sm text-white/80 leading-relaxed mb-5">
                Acredite: ganhe R$ 100,00 de seu saldo Quanta Shop receba cashback real em centenas de compras online. Sem pegadinhas.
              </p>

              <!-- Carousel dots -->
              <div class="flex items-center justify-between">
                <div class="flex gap-1.5">
                  <span class="w-5 h-1.5 bg-[#98C73A] rounded-full"></span>
                  <span class="w-1.5 h-1.5 bg-white/40 rounded-full"></span>
                  <span class="w-1.5 h-1.5 bg-white/40 rounded-full"></span>
                </div>
                <NuxtLink
                  to="/cadastro"
                  class="inline-flex items-center gap-1.5 bg-[#98C73A] text-[#225F6B] font-bold text-sm px-5 py-2 rounded-full hover:bg-[#8ab832] transition-colors"
                >
                  Insgir um Saldo
                  <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M17 8l4 4m0 0l-4 4m4-4H3" />
                  </svg>
                </NuxtLink>
              </div>
            </div>
          </div>
        </div>

      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Post {
  id: number
  title: string
  summary: string
  thumbnail: string
  category: string
}

const posts: Post[] = [
  {
    id: 1,
    title: 'Como Maximizar Seu Cashback em Compras Online',
    summary: 'Aprenda as melhores estratégias para acumular mais saldo CCR em cada compra que você faz.',
    thumbnail: '/images/blog/post1.png',
    category: 'Dicas',
  },
  {
    id: 2,
    title: 'Guia Completo de Finanças Pessoais',
    summary: 'Organize suas finanças e use o cashback como parte da sua estratégia de independência financeira.',
    thumbnail: '/images/blog/post2.png',
    category: 'Finanças',
  },
  {
    id: 3,
    title: 'As Melhores Dicas para Compras Inteligentes',
    summary: 'Veja como combinar cupons, cashback e promoções para economizar muito mais no dia a dia.',
    thumbnail: '/images/blog/post3.png',
    category: 'Economia',
  },
]
</script>
