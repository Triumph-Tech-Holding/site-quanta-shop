<template>
  <section class="tp-product-gadget-area pt-80 pb-75">
    <div class="container">
      <div class="row">
        <div class="col-12 col-xl-4 col-lg-5">
          <div class="tp-product-gadget-sidebar mb-40">
            <div class="tp-product-gadget-categories p-relative fix mb-10">
              <div class="tp-product-gadget-thumb">
                <img src="/img/product/gadget/54.png" alt="" />
              </div>
              <h3 class="tp-product-gadget-categories-title">
                Super Humano <br />
                Suplementos
              </h3>

              <div class="tp-product-gadget-categories-list">
                <ul>
                  <li>
                    <nuxt-link
                      target="_blank"
                      href="https://www.suplementosuperhumano.com.br/suplementos/"
                      >Cabelo</nuxt-link
                    >
                  </li>
                  <li>
                    <nuxt-link
                      target="_blank"
                      href="https://www.suplementosuperhumano.com.br/suplementos/"
                      >Cérebro</nuxt-link
                    >
                  </li>
                  <li>
                    <nuxt-link
                      target="_blank"
                      href="https://www.suplementosuperhumano.com.br/suplementos/"
                      >Multivitamínico</nuxt-link
                    >
                  </li>
                  <li>
                    <nuxt-link
                      target="_blank"
                      href="https://www.suplementosuperhumano.com.br/saude-e-bem-estar/"
                      >Teste genético</nuxt-link
                    >
                  </li>
                </ul>
              </div>

              <div class="tp-product-gadget-btn">
                <nuxt-link
                  target="_blank"
                  href="https://www.suplementosuperhumano.com.br/"
                  class="tp-link-btn"
                >
                  Mais produtos
                  <SvgRightArrow2 />
                </nuxt-link>
              </div>
            </div>
            <div class="tp-product-gadget-banner">
              <Swiper
                :slidesPerView="1"
                :spaceBetween="0"
                :loop="true"
                :effect="'fade'"
                :pagination="{
                  el: '.tp-product-gadget-banner-slider-dot',
                  clickable: true,
                }"
                :autoplay="{
                  delay: 5000,
                  disableOnInteraction: false,
                  pauseOnMouseEnter: true,
                }"
                :modules="[EffectFade, Pagination, Autoplay]"
                class="tp-product-gadget-banner-slider-active swiper-container rounded"
              >
                <SwiperSlide
                  v-for="(item, i) in bannerProducts"
                  :key="i"
                  class="tp-product-gadget-banner-item banner-area-bg"
                >
                  <div class="container">
                    <nuxt-link :href="item.link">
                      <div class="row align-items-center">
                        <div class="col-12 col-xl-7 col-lg-7 col-md-7">
                          <div
                            class="tp-slider-content p-relative z-index-1 ml-20"
                          >
                            <span class="tp-product-gadget-banner-price">
                              {{ item.texto1 }}
                            </span>

                            <h3
                              class="tp-slider-title tp-product-gadget-banner-title"
                            >
                              {{ item.texto2 }}
                            </h3>
                          </div>
                        </div>
                        <div class="col-12 col-xl-5 col-lg-5 col-md-5">
                          <div
                            class="tp-slider-thumb text-center d-flex align-items-center justify-content-center"
                          >
                            <img :src="item.imagem" alt="slider-img" />
                          </div>
                        </div>
                      </div>
                    </nuxt-link>
                  </div>
                </SwiperSlide>
                <div
                  class="tp-product-gadget-banner-slider-dot tp-swiper-dot"
                ></div>
              </Swiper>
            </div>
          </div>
        </div>
        <div class="col-xl-8 col-lg-7 m-auto">
          <div class="tp-product-gadget-wrapper">
            <div class="row">
              <div
                v-for="item in product_data.slice(0, 3)"
                :key="item.id"
                class="col-xl-4 col-sm-6"
              >
                <super-humano-item :item="item" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { Swiper, SwiperSlide } from "swiper/vue";
import { Autoplay, EffectFade, Pagination } from "swiper/modules";
import { useCarouselStore } from "@/pinia/useCarouselStore";

const carouselStore = useCarouselStore();
const bannerProducts = computed(() => transformData());

const transformData = (): any => {
  const carousels = carouselStore.carousels;
  const filteredCarousels = carousels.filter(
    (carousel) => carousel.posicao === "3"
  );

  if (filteredCarousels && filteredCarousels.length > 0) {
    return filteredCarousels;
  } else {
    return [];
  }
};

const product_data = [
  {
    id: "unique-product-id-001",
    sku: "SH-MULTI-001",
    img: "https://cdn.sistemawbuy.com.br/arquivos/effcdca7f240485078954630f43b0ef7/produtos/RAE6ZOU8/sh-multivitaminico-mockup-1-6238cc94bc5a4.png",
    title: "SH Multivitamínico",
    slug: "sh-multivitaminico",
    unit: "unidade",
    url: "https://www.suplementosuperhumano.com.br/sh-termogenico-m1-890795/",
    imageURLs: [
      {
        img: "https://cdn.sistemawbuy.com.br/arquivos/effcdca7f240485078954630f43b0ef7/produtos/RAE6ZOU8/sh-multivitaminico-mockup-1-6238cc94bc5a4.png",
      },
    ],
    parent: "multivitamínico",
    children: "none",
    price: 96.3,
    discount: 0,
    quantity: 100,
    brand: {
      name: "SUPERHUMANO SUPLEMENTOS",
    },
    category: {
      name: "Multivitamínico",
    },
    status: "available",
    reviews: [
      {
        name: "João Silva",
        date: "2024-08-10",
        rating: 5,
        comment:
          "Excelente multivitamínico! Senti uma grande diferença na minha energia e disposição. Recomendo muito.",
      },
      {
        name: "Maria Oliveira",
        date: "2024-08-12",
        rating: 4,
        comment:
          "Bom suplemento, mas o preço poderia ser um pouco mais acessível. No geral, estou satisfeita com os resultados.",
      },
      {
        name: "Carlos Pereira",
        date: "2024-08-14",
        rating: 3,
        comment:
          "O produto é bom, mas não percebi grandes mudanças em minha saúde. Pode funcionar melhor para outras pessoas.",
      },
      {
        name: "Ana Costa",
        date: "2024-08-13",
        rating: 5,
        comment:
          "Ótimo produto! A qualidade é excelente e os benefícios são visíveis. Voltarei a comprar com certeza.",
      },
      {
        name: "Roberto Santos",
        date: "2024-08-11",
        rating: 4,
        comment:
          "Multivitamínico eficaz. A embalagem poderia ser melhor, mas o produto em si é muito bom e cumpre o que promete.",
      },
    ],
    productType: "suplemento",
    description: "Descrição do SH Multivitamínico.",
    orderQuantity: 1,
    additionalInformation: [
      {
        key: "Preço à vista",
        value: "96,30",
      },
      {
        key: "Preço no boleto ou cartão",
        value: "99,20",
      },
    ],
    featured: false,
    sellCount: 0,
    offerDate: {
      startDate: "2024-08-15",
      endDate: "2024-09-15",
    },
    tags: ["multivitamínico", "suplemento"],
    videoId: "",
    sizes: [],
  },
  {
    id: "unique-product-id-002",
    sku: "SH-CEREBRO-001",
    img: "https://cdn.sistemawbuy.com.br/arquivos/effcdca7f240485078954630f43b0ef7/produtos/TIO4BOU5/shcerebro-m1-61dc523d7adef.png",
    title: "SH Cérebro M1",
    slug: "sh-cerebro-m1",
    unit: "unidade",
    url: "https://www.suplementosuperhumano.com.br/sh-cerebro-m1/",
    imageURLs: [
      {
        img: "https://cdn.sistemawbuy.com.br/arquivos/effcdca7f240485078954630f43b0ef7/produtos/TIO4BOU5/shcerebro-m1-61dc523d7adef.png",
      },
    ],
    parent: "cérebro",
    children: "none",
    price: 139.63,
    discount: 0,
    quantity: 100,
    brand: {
      name: "SUPERHUMANO SUPLEMENTOS",
    },
    category: {
      name: "Cérebro",
    },
    status: "available",
    reviews: [
      {
        name: "Gabriela Oliveira",
        date: "2024-08-01",
        rating: 5,
        comment:
          "Incrível! Melhorei muito meu foco e concentração desde que comecei a usar. Super recomendado.",
      },
      {
        name: "Ricardo Almeida",
        date: "2024-08-02",
        rating: 4,
        comment:
          "Bom produto. Notei uma melhora no meu desempenho cognitivo, mas o efeito poderia ser um pouco mais rápido.",
      },
      {
        name: "Mariana Costa",
        date: "2024-08-03",
        rating: 5,
        comment:
          "Excelente suplemento para clareza mental e concentração. Senti uma grande diferença no meu trabalho.",
      },
      {
        name: "Pedro Silva",
        date: "2024-08-04",
        rating: 3,
        comment:
          "O produto é bom, mas não teve o impacto que eu esperava. Talvez funcione melhor para outras pessoas.",
      },
      {
        name: "Tatiane Santos",
        date: "2024-08-05",
        rating: 4,
        comment:
          "Resultados positivos na memória e concentração. O preço é um pouco alto, mas o efeito compensa.",
      },
      {
        name: "João Paulo",
        date: "2024-08-06",
        rating: 5,
        comment:
          "Transformador! Minha capacidade de foco e raciocínio melhorou muito. Vale o investimento.",
      },
      {
        name: "Cláudia Martins",
        date: "2024-08-07",
        rating: 4,
        comment:
          "Ótimo suplemento. Notei melhorias, especialmente em tarefas que exigem concentração prolongada.",
      },
      {
        name: "Bruno Ferreira",
        date: "2024-08-08",
        rating: 3,
        comment:
          "Produto bom, mas esperava resultados mais rápidos. Pode demorar um pouco para notar mudanças significativas.",
      },
      {
        name: "Patrícia Lima",
        date: "2024-08-09",
        rating: 5,
        comment:
          "Muito satisfeito! O SH Cérebro M1 realmente ajudou na minha capacidade de concentração e clareza mental.",
      },
      {
        name: "Felipe Rodrigues",
        date: "2024-08-10",
        rating: 4,
        comment:
          "Boa eficácia e melhora na função cognitiva. O preço poderia ser um pouco mais acessível.",
      },
      {
        name: "Ana Beatriz",
        date: "2024-08-11",
        rating: 5,
        comment:
          "Excelente para quem precisa de um impulso na concentração. Recomendo fortemente para quem trabalha com prazos apertados.",
      },
      {
        name: "Gustavo Pereira",
        date: "2024-08-12",
        rating: 4,
        comment:
          "Produto de boa qualidade com efeitos visíveis. A embalagem é um pouco difícil de abrir, mas o produto é eficaz.",
      },
    ],
    productType: "suplemento",
    description: "Descrição do SH Cérebro M1.",
    orderQuantity: 1,
    additionalInformation: [
      {
        key: "Preço à vista",
        value: "139,63",
      },
      {
        key: "Preço no boleto ou cartão",
        value: "142,53",
      },
    ],
    featured: false,
    sellCount: 0,
    offerDate: {
      startDate: "2024-08-15",
      endDate: "2024-09-15",
    },
    tags: ["cérebro", "suplemento"],
    videoId: "",
    sizes: [],
  },
  {
    id: "unique-product-id-003",
    sku: "SH-SKIN-001",
    img: "https://cdn.sistemawbuy.com.br/arquivos/effcdca7f240485078954630f43b0ef7/produtos/QEI6PIO6/shskin-hair-m1-61dc51cf33e0f.png",
    title: "SH Skin hair M1",
    slug: "sh-skin-hair-m1",
    unit: "unidade",
    url: "https://www.suplementosuperhumano.com.br/shskin-hair-m1/",
    imageURLs: [
      {
        img: "https://cdn.sistemawbuy.com.br/arquivos/effcdca7f240485078954630f43b0ef7/produtos/QEI6PIO6/shskin-hair-m1-61dc51cf33e0f.png",
      },
    ],
    parent: "cabelo",
    children: "none",
    price: 131.63,
    discount: 0,
    quantity: 100,
    brand: {
      name: "SUPERHUMANO SUPLEMENTOS",
    },
    category: {
      name: "Cabelo",
    },
    status: "available",
    reviews: [
      {
        name: "Fernanda Lima",
        date: "2024-08-05",
        rating: 5,
        comment:
          "Estou adorando! Meu cabelo e pele melhoraram muito desde que comecei a usar. Super recomendo.",
      },
      {
        name: "Lucas Almeida",
        date: "2024-08-07",
        rating: 4,
        comment:
          "Bom suplemento, com ótimos resultados. Apenas a embalagem que poderia ser mais prática.",
      },
      {
        name: "Patrícia Duarte",
        date: "2024-08-09",
        rating: 5,
        comment:
          "Excelente! Notei uma grande diferença na saúde do meu cabelo e na minha pele. Vale cada centavo.",
      },
      {
        name: "Renato Silva",
        date: "2024-08-10",
        rating: 3,
        comment:
          "Produto bom, mas não vi resultados tão rápidos quanto esperava. Continuarei usando para ver se melhora.",
      },
      {
        name: "Juliana Costa",
        date: "2024-08-12",
        rating: 4,
        comment:
          "Bom efeito geral, mas o preço é um pouco alto. No entanto, o produto é de qualidade e eficaz.",
      },
      {
        name: "Marcos Pereira",
        date: "2024-08-13",
        rating: 4,
        comment:
          "Resultados positivos, especialmente na pele. Cabelo ainda está em processo de melhora. Satisfeito no geral.",
      },
      {
        name: "Luciana Fernandes",
        date: "2024-08-14",
        rating: 5,
        comment:
          "Estou impressionada com a melhora na minha pele e cabelo. Definitivamente vou continuar usando.",
      },
      {
        name: "André Gomes",
        date: "2024-08-06",
        rating: 2,
        comment:
          "Não vi muitos resultados. Talvez funcione melhor para outras pessoas. Para mim, não fez muita diferença.",
      },
      {
        name: "Isabela Martins",
        date: "2024-08-08",
        rating: 4,
        comment:
          "Produto de boa qualidade com bons resultados. A única observação é que poderia ser mais barato.",
      },
    ],
    productType: "suplemento",
    description: "Descrição do SH Skin hair M1.",
    orderQuantity: 1,
    additionalInformation: [
      {
        key: "Preço à vista",
        value: "131,63",
      },
      {
        key: "Preço no boleto ou cartão",
        value: "134,53",
      },
    ],
    featured: false,
    sellCount: 0,
    offerDate: {
      startDate: "2024-08-15",
      endDate: "2024-09-15",
    },
    tags: ["cabelo", "suplemento"],
    videoId: "",
    sizes: [],
  },
];
</script>

<style scoped>
.banner-area-bg {
  background: rgb(61, 72, 150);
  background: linear-gradient(
    90deg,
    rgba(61, 72, 150, 1) 0%,
    rgba(211, 56, 149, 1) 100%
  );
}

.tp-slider-thumb img {
  max-height: 160px;
}

.tp-product-gadget-thumb img {
  max-height: 200px;
}

@media screen and (max-width: 768px) {
  .tp-slider-content {
    padding: 40px 20px;
    margin-left: 0;
    margin-top: 20px;
    margin-bottom: 0;
  }

  .tp-slider-thumb img {
    height: 100%;
    max-height: 100%;
    margin-top: 0;
  }

  .tp-product-gadget-thumb img {
    display: none;
  }
}
</style>
