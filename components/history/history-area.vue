<template>
  <section
    class="tp-history-area pt-140 pb-140"
    style="background-color: #f8f8f8"
  >
    <div class="container">
      <div class="row">
        <div class="col-xl-12">
          <div class="tp-history-slider mb-50">
            <Swiper
              :slidesPerView="1"
              :spaceBetween="0"
              :thumbs="{ swiper: thumbsSwiper }"
              :modules="[FreeMode, Thumbs, EffectFade]"
              :effect="'fade'"
              class="tp-history-slider-active swiper-container"
            >
              <SwiperSlide
                v-for="(item, i) in slider_data"
                :key="i"
                class="tp-thistory-item"
                style="background-color: #f8f8f8"
              >
                <div class="row">
                  <div class="col-xl-5 col-lg-6 col-md-6">
                    <div class="tp-history-wrapper pr-45">
                      <div class="tp-history-content mb-40">
                        <h3 class="tp-history-title" v-html="item.title"></h3>
                        <p class="text-justify">
                          {{ item.subtitle_1 }}
                        </p>
                        <p class="text-justify">
                          {{ item.subtitle_2 }}
                        </p>
                      </div>
                      <div class="tp-history-year">
                        <p>{{ item.year }}</p>
                      </div>
                    </div>
                  </div>
                  <div class="col-xl-7 col-lg-6 col-md-6">
                    <div class="tp-history-thumb-wrapper ml-150 p-relative">
                      <div class="tp-history-thumb-text">
                        <p v-html="item.thumb_text"></p>
                      </div>
                      <div class="tp-history-thumb m-img">
                        <img :src="item.img" alt="thumb_img" />
                      </div>
                    </div>
                  </div>
                </div>
              </SwiperSlide>
            </Swiper>
          </div>
        </div>
      </div>

      <div class="tp-history-nav">
        <div class="row">
          <div class="col-xl-12">
            <Swiper
              class="tp-history-nav-active swiper-container"
              @swiper="setThumbsSwiper"
              :spaceBetween="120"
              :slidesPerView="7"
              :freeMode="true"
              :watchSlidesProgress="true"
              :modules="[FreeMode, Thumbs]"
            >
              <SwiperSlide
                v-for="(num, i) in slider_nav_data"
                :key="i"
                class="tp-history-nav-year text-center"
              >
                <p>{{ num }}</p>
              </SwiperSlide>
            </Swiper>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { Swiper, SwiperSlide } from "swiper/vue";
import { FreeMode, Thumbs, EffectFade } from "swiper/modules";

let thumbsSwiper = ref(null);

const setThumbsSwiper = (swiper: any) => {
  thumbsSwiper.value = swiper;
};

// slider data
const slider_data: {
  subtitle_1: string;
  subtitle_2: string;
  title: string;
  img: string;
  thumb_text: string;
  year: number;
}[] = [
  // {
  //   title: "Nossa trajetória em <br> 2017",
  //   subtitle_1:
  //     "Em 2017, nossa empresa alcançou a marca de mais de um milhão de cupons utilizados. Esse marco refletiu a crescente aceitação de nossos serviços no mercado.",
  //   subtitle_2:
  //     "O sucesso do ano foi impulsionado pela expansão do mercado de cupons de desconto no país, que permitiu um crescimento notável.",
  //   img: "/img/history/history-1.jpg",
  //   thumb_text: "Rumo ao sucesso <br> em 2017",
  //   year: 2017,
  // },
  // {
  //   title: "Avanços de <br> 2018",
  //   subtitle_1:
  //     "Em 2018, continuamos a nossa trajetória de crescimento com um aumento significativo no uso de cupons. O mercado de cupons de desconto no país expandiu ainda mais, contribuindo para nosso sucesso.",
  //   subtitle_2:
  //     "Com um crescimento de mais de 100% ao ano, estamos entusiasmados com nosso progresso e comprometidos em oferecer mais valor aos nossos usuários.",
  //   img: "/img/history/history-2.jpg",
  //   thumb_text: "Impulsionando o crescimento <br> em 2018",
  //   year: 2018,
  // },
  // {
  //   title: "Marcos de <br> 2019",
  //   subtitle_1:
  //     "Em 2019, atingimos a marca de mais de um milhão de cupons utilizados, solidificando nossa posição no mercado. A expansão contínua do setor de cupons de desconto contribuiu significativamente para esse resultado.",
  //   subtitle_2:
  //     "Estamos animados com nossas conquistas e continuaremos a trabalhar para oferecer oportunidades cada vez melhores para nossos usuários.",
  //   img: "/img/history/history-3.jpg",
  //   thumb_text: "Celebrando <br> nossas conquistas de 2019",
  //   year: 2019,
  // },
  // {
  //   title: "Conquistas de <br> 2020",
  //   subtitle_1:
  //     "Em 2020, nossa empresa superou a marca de um milhão de cupons utilizados novamente, demonstrando a sólida expansão do mercado e o crescimento contínuo da nossa base de usuários.",
  //   subtitle_2:
  //     "Estamos entusiasmados com os resultados e continuaremos a oferecer o melhor para nossos clientes enquanto exploramos novas oportunidades de crescimento.",
  //   img: "/img/history/history-4.jpg",
  //   thumb_text: "Avançando com <br> nossas realizações de 2020",
  //   year: 2020,
  // },
  {
    title: "Expansão e <br> inovações de 2021",
    subtitle_1:
      "Em 2021, nossa empresa não apenas manteve o ritmo de crescimento, mas também introduziu novas inovações no mercado de cupons. A ampliação das nossas ofertas e serviços nos ajudou a alcançar novos patamares.",
    subtitle_2:
      "Com um aumento contínuo na utilização de cupons e novas parcerias estratégicas, estamos ansiosos para o futuro e para as novas oportunidades que ele trará.",
    img: "/img/history/history-5.jpg",
    thumb_text: "Explorando novas fronteiras <br> em 2021",
    year: 2021,
  },
  {
    title: "Desenvolvimento e <br> crescimento em 2022",
    subtitle_1:
      "Em 2022, continuamos a crescer de forma robusta, com a utilização de cupons atingindo novos recordes. A nossa capacidade de adaptação às tendências do mercado foi crucial para o nosso sucesso contínuo.",
    subtitle_2:
      "Estamos empolgados com as inovações implementadas e com o impacto positivo que tiveram na experiência dos nossos usuários.",
    img: "/img/history/history-6.jpg",
    thumb_text: "Inovações e sucessos <br> em 2022",
    year: 2022,
  },
  {
    title: "Conquistas de <br> 2023",
    subtitle_1:
      "Em 2023, celebramos um ano de grande sucesso, com a utilização de cupons alcançando novos patamares. A nossa dedicação ao crescimento e à inovação nos permitiu expandir significativamente nossa presença no mercado.",
    subtitle_2:
      "Estamos ansiosos para continuar nossa trajetória de sucesso e oferecer ainda mais valor aos nossos clientes e parceiros.",
    img: "/img/history/history-7.jpg",
    thumb_text: "Rumo ao futuro <br> em 2023",
    year: 2023,
  },
];

// slider nav data
const slider_nav_data: number[] = [2021, 2022, 2023];
</script>

<style scoped>
.tp-thistory-item {
  height: 435px;
}
</style>
