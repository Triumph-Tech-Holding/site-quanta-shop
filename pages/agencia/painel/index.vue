<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page">
            <i>
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width:20px;height:20px;vertical-align:middle;">
                <path d="M19.14 12.94c.04-.3.06-.61.06-.94 0-.32-.02-.64-.07-.94l2.03-1.58c.18-.14.23-.41.12-.61l-1.92-3.32c-.12-.22-.37-.29-.59-.22l-2.39.96c-.5-.38-1.03-.7-1.62-.94l-.36-2.54c-.04-.24-.24-.41-.48-.41h-3.84c-.24 0-.43.17-.47.41l-.36 2.54c-.59.24-1.13.57-1.62.94l-2.39-.96c-.22-.08-.47 0-.59.22L2.74 8.87c-.12.21-.08.47.12.61l2.03 1.58c-.05.3-.09.63-.09.94s.02.64.07.94l-2.03 1.58c-.18.14-.23.41-.12.61l1.92 3.32c.12.22.37.29.59.22l2.39-.96c.5.38 1.03.7 1.62.94l.36 2.54c.05.24.24.41.48.41h3.84c.24 0 .44-.17.47-.41l.36-2.54c.59-.24 1.13-.56 1.62-.94l2.39.96c.22.08.47 0 .59-.22l1.92-3.32c.12-.22.07-.47-.12-.61l-2.01-1.58zM12 15.6c-1.98 0-3.6-1.62-3.6-3.6s1.62-3.6 3.6-3.6 3.6 1.62 3.6 3.6-1.62 3.6-3.6 3.6z"/>
              </svg>
            </i>
            Painel geral
          </h2>
        </div>

        <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

        <template v-else>
          <div class="container mb-5 pb-5">
            <div class="row gap-20 p-0">

              <!-- LEFT COLUMN: col-md-8 -->
              <div class="col-12 col-md-8">
                <div class="row gap-20 p-3">

                  <!-- Videos Section -->
                  <div class="col-12 card bg-white rounded">
                    <div class="row py-3">
                      <div class="col-12 col-sm-2 col-md-2 col-lg-1 col-xl-1 d-flex align-items-center justify-content-center">
                        <img src="/agencia/imgs/quanta-play.gif" alt="" class="cursor-pointer" style="width:3.5rem;" @click="openVideo('https://www.youtube.com/embed/UYJNhfIJ0Lw?si=hL1BgsJ5SDje8qzg', 'Comece por aqui')" />
                      </div>
                      <div class="col-12 col-sm-10 col-md-10 col-lg-11 col-xl-11 d-flex flex-column align-items-center align-items-md-start justify-content-center">
                        <h5 class="font-weight-bold text-green mb-1">Comece por aqui</h5>
                        <p class="mb-0" style="font-size:.85rem;color:#555;">
                          Explore nossa plataforma com os vídeos abaixo. Cada um deles detalha uma parte essencial de como ganhar por minuto, ajudando você a aproveitar ao máximo todas as funcionalidades disponíveis. Vamos juntos transformar suas compras e indicações em uma fonte de renda contínua!
                        </p>
                      </div>
                    </div>
                    <div class="row mt-2 mb-3 px-3">
                      <div class="col-12 col-sm-4 mb-2 mb-sm-0">
                        <div class="embed-container cursor-pointer" @click="openVideo('https://www.youtube.com/embed/oQdZeEagCfU?si=hL1BgsJ5SDje8qzg', 'Bem-vindo')">
                          <img src="/agencia/imgs/bem-vindo.jpg" alt="Bem-vindo" class="cover-image" />
                          <div class="play-overlay"><span class="play-btn">▶</span></div>
                        </div>
                      </div>
                      <div class="col-12 col-sm-4 mb-2 mb-sm-0">
                        <div class="embed-container cursor-pointer" @click="openVideo('https://www.youtube.com/embed/R4QFuyN9yAo?si=hlhSEAgUTzXH17dE', 'Como Funciona')">
                          <img src="/agencia/imgs/como-funciona.jpg" alt="Como funciona" class="cover-image" />
                          <div class="play-overlay"><span class="play-btn">▶</span></div>
                        </div>
                      </div>
                      <div class="col-12 col-sm-4">
                        <div class="embed-container cursor-pointer" @click="openVideo('https://www.youtube.com/embed/CFDNYh_Iigs', 'Como Ganhar')">
                          <img src="/agencia/imgs/como-ganhar.jpg" alt="Como Ganhar" class="cover-image" />
                          <div class="play-overlay"><span class="play-btn">▶</span></div>
                        </div>
                      </div>
                    </div>
                  </div>

                  <!-- Quanta Amizade Section -->
                  <div class="col-12 card bg-white rounded">
                    <div class="row py-3">
                      <div class="col-12 col-sm-2 col-md-2 col-lg-1 col-xl-1 d-flex align-items-center justify-content-center">
                        <img src="/agencia/imgs/comece-por-aqui.png" alt="" style="width:2.5rem;" />
                      </div>
                      <div class="col-12 col-sm-10 col-md-10 col-lg-11 col-xl-11 d-flex flex-column align-items-center align-items-md-start justify-content-center">
                        <h5 class="font-weight-bold text-green mb-1">Quanta Amizade</h5>
                        <p class="mb-0" style="font-size:.85rem;color:#555;">
                          A cada amigo indicado que alcançar os primeiros R$ 50 em cashback, você ganha R$ 25 e ele ganha mais R$ 25! Aproveite a chance de juntar a galera e ganhar juntos um bom dinheiro por minuto. Quanto mais amigos você trouxer, mais você ganha!
                        </p>
                      </div>
                    </div>
                    <div class="row px-3 pb-3">
                      <div class="col-12">
                        <a href="https://bigcash.blob.core.windows.net/documentos/CAMPANHA%20QUANTA%20AMIZADE.pdf" target="_blank">
                          <img src="/agencia/imgs/Banner-Quanta-Bank-01.jpg" alt="Quanta Amizade" class="rounded d-none d-md-block" style="width:100%;" />
                          <img src="/agencia/imgs/BannerMob-Quanta-Bank-01.jpg" alt="Quanta Amizade" class="rounded d-md-none" style="width:100%;" />
                        </a>
                      </div>
                    </div>
                  </div>

                  <!-- Mapa Section -->
                  <div class="col-12 card bg-white rounded">
                    <div class="row py-3 align-items-center">
                      <div class="col-12 col-sm-2 col-md-2 col-lg-1 col-xl-1 text-center mb-3 mb-sm-0">
                        <img src="/agencia/imgs/loja.png" class="img-fluid" alt="Loja" style="max-width:2.5rem;" />
                      </div>
                      <div class="col-12 col-sm-10 col-md-10 col-lg-11 col-xl-11">
                        <h5 class="font-weight-bold text-green mb-1">Encontre as lojas parceiras mais próximas de você!</h5>
                        <p class="mb-0" style="font-size:.85rem;color:#555;">
                          Descubra rapidamente os parceiros locais que oferecem cashback na sua região. Nosso mapa interativo exibe as opções mais próximas, facilitando sua jornada para economizar e ganhar dinheiro enquanto faz compras e indica amigos.
                        </p>
                      </div>
                    </div>
                    <div class="px-3 pb-3">
                      <div class="rounded border" style="overflow:hidden;">
                        <iframe
                          src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3675!2d-46.633!3d-23.55!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zMjPCsDMzJzAwLjAiUyA0NsKwMzgnMDAuMCJX!5e0!3m2!1spt-BR!2sbr!4v1234567890"
                          style="border:0;width:100%;height:50vh;min-height:300px;"
                          loading="lazy"
                          referrerpolicy="no-referrer-when-downgrade"
                          title="Mapa de lojas parceiras"
                        ></iframe>
                      </div>
                    </div>
                  </div>

                </div>
              </div>

              <!-- RIGHT COLUMN: col-md-4 -->
              <div class="col-12 col-md-4">
                <div class="row gap-20 p-3">

                  <!-- Indicadores (Stats) -->
                  <div class="col-12 card bg-white rounded p-0">
                    <div class="card-header-section py-2 px-3">
                      <strong style="font-size:.9rem;color:#225f6b;">Indicadores</strong>
                    </div>
                    <div class="d-flex w-100 h-100 py-3">
                      <div class="col d-flex align-items-center justify-content-center indicador">
                        <img src="https://cdn3.iconfinder.com/data/icons/digital-marketing-27/64/Reach-smartphone-human-people-communication-tablet-256.png" alt="" style="width:40px;height:40px;margin-bottom:.25rem;" />
                        <span class="font-weight-bold" style="font-size:.75rem;text-align:center;">Equipe</span>
                        <span class="mt-1 font-weight-bold" style="font-size:.9rem;">{{ totalizadores.rede }}</span>
                      </div>
                      <div class="col d-flex align-items-center justify-content-center indicador">
                        <img src="https://cdn3.iconfinder.com/data/icons/digital-marketing-27/64/Pay-click-computer-dollar-money-currency-256.png" alt="" style="width:40px;height:40px;margin-bottom:.25rem;" />
                        <span class="font-weight-bold" style="font-size:.75rem;text-align:center;">Saldo</span>
                        <span class="mt-1 font-weight-bold" style="font-size:.8rem;">{{ formatCurrency(totalizadores.saldo) }}</span>
                      </div>
                      <div class="col d-flex align-items-center justify-content-center indicador">
                        <img src="https://cdn3.iconfinder.com/data/icons/digital-marketing-27/64/Human-people-teamwork-network-communication-man-256.png" alt="" style="width:40px;height:40px;margin-bottom:.25rem;" />
                        <span class="font-weight-bold" style="font-size:.75rem;text-align:center;">Encaminhamentos</span>
                        <span class="mt-1 font-weight-bold" style="font-size:.9rem;">{{ totalizadores.ecossistemas }}</span>
                      </div>
                      <div class="col d-flex align-items-center justify-content-center indicador">
                        <img src="https://cdn0.iconfinder.com/data/icons/social-network-107/512/star-512.png" alt="" style="width:40px;height:40px;margin-bottom:.25rem;" />
                        <span class="font-weight-bold" style="font-size:.75rem;text-align:center;">Pontos</span>
                        <span class="mt-1 font-weight-bold" style="font-size:.9rem;">{{ totalizadores.pontos }}</span>
                      </div>
                    </div>
                  </div>

                  <!-- Convide + Credenciar -->
                  <div class="col-12 card bg-white rounded align-items-center p-4">
                    <div class="card-header-section mb-3 w-100">
                      <strong style="font-size:.9rem;color:#225f6b;">Convide</strong>
                    </div>
                    <div class="col-12 col-md-12 text-center">
                      <button class="btn green-button-2 text-uppercase w-100 mb-3" @click="showConvideModal = true" style="font-size:.85rem;">
                        Convide seus amigos
                      </button>
                      <NuxtLink to="/agencia/painel/meus-credenciamentos" class="btn green-button-2 text-uppercase w-100" style="font-size:.85rem;display:block;">
                        Credencie um estabelecimento
                      </NuxtLink>
                    </div>
                  </div>

                  <!-- Meta Minuto -->
                  <div class="col-12 card bg-white rounded">
                    <div class="container bg-white px-3 py-3">
                      <div class="row mb-2">
                        <div class="col-12 d-flex flex-row align-items-center justify-content-center">
                          <img src="/agencia/imgs/meta-minuto.png" alt="Meta Minuto" style="max-width:32px;margin-right:.5rem;" />
                          <h5 class="font-weight-bold text-green mb-0">Meta Minuto</h5>
                        </div>
                      </div>
                      <div v-if="metaCompleted" class="alert alert-success text-center" style="font-size:.8rem;">
                        Parabéns, você já atingiu sua meta diária, continue assim!!
                      </div>
                      <div v-else>
                        <div class="d-flex flex-row">
                          <div class="col-4 d-flex align-items-center justify-content-start">
                            <h4 class="countdown-time mb-0" style="font-size:1.2rem;font-weight:700;color:#225f6b;">{{ countdownTime }}</h4>
                          </div>
                          <div class="col-8 d-flex align-items-center justify-content-end">
                            <h4 class="mb-0 text-danger" style="font-size:1rem;font-weight:700;">{{ formatCurrency(targetValue) }}</h4>
                          </div>
                        </div>
                        <div class="meta-progress mt-2" style="background:#e9ecef;border-radius:4px;height:.5rem;overflow:hidden;">
                          <div :style="{ width: progressValue + '%', background:'#98c73a', height:'100%', borderRadius:'4px', transition:'width .3s' }"></div>
                        </div>
                        <span class="d-block text-right mt-1" style="font-size:.7rem;color:#6c757d;">
                          Sua meta {{ metaTabLabel }} é de {{ formatCurrency(metaTabValue) }}
                        </span>
                        <div class="d-flex justify-content-center mt-2">
                          <span class="green-button-2 meta-minuto-button" @click="metaTab=1" :class="{ 'active-button': metaTab===1 }" style="font-size:.6rem;cursor:pointer;margin:0 2px;padding:.25rem .5rem;">Diário</span>
                          <span class="green-button-2 meta-minuto-button" @click="metaTab=2" :class="{ 'active-button': metaTab===2 }" style="font-size:.6rem;cursor:pointer;margin:0 2px;padding:.25rem .5rem;">Semanal</span>
                          <span class="green-button-2 meta-minuto-button" @click="metaTab=3" :class="{ 'active-button': metaTab===3 }" style="font-size:.6rem;cursor:pointer;margin:0 2px;padding:.25rem .5rem;">Mensal</span>
                        </div>
                      </div>
                    </div>
                  </div>

                  <!-- Assinatura -->
                  <div class="col-12 card bg-white rounded align-items-center p-4">
                    <h6 class="text-uppercase text-center color" style="font-size:.8rem;"><strong>Desbloqueie vantagens exclusivas ao se tornar um assinante!</strong></h6>
                    <p class="text-center" style="font-size:.8rem;">
                      Garanta cashback em dobro e acesso ao nosso assistente virtual inteligente que explica como ganhar por minuto para os seus indicados. Não perca essa chance! Clique no botão abaixo e descubra todos os benefícios agora mesmo.
                    </p>
                    <NuxtLink to="/agencia/painel/assinatura" class="mt-2 btn green-button-2" style="font-size:.8rem;">Aproveite agora mesmo</NuxtLink>
                  </div>

                  <!-- Canal WhatsApp -->
                  <div class="col-12 card bg-white rounded align-items-center px-4 py-4">
                    <div class="card-header-section mb-3 w-100">
                      <strong style="font-size:.9rem;color:#225f6b;">Conheça nosso canal no WhatsApp</strong>
                    </div>
                    <h6 class="text-center color"><strong>Conheça nosso canal</strong></h6>
                    <div class="row my-1 w-100">
                      <div class="col-12 col-xl-4 d-flex align-items-center justify-content-center">
                        <img src="/agencia/imgs/whatsapp.png" alt="" style="max-width:64px;" />
                      </div>
                      <div class="col-12 col-xl-8 d-flex flex-column align-items-center justify-content-center">
                        <p style="font-size:.8rem;text-align:justify;">
                          Participe do nosso canal no WhatsApp e fique por dentro de ofertas especiais, cupons de desconto, notificações diárias e as últimas novidades exclusivas. Não perca a oportunidade de receber vantagens direto no seu celular!
                        </p>
                      </div>
                    </div>
                    <a href="https://whatsapp.com/channel/0029VaqtQAxD38CZ6ykplz0J" target="_blank" class="btn green-button-2" style="font-size:.8rem;">Entrar</a>
                  </div>

                </div>
              </div>

            </div>
          </div>
        </template>
      </div>
    </div>

    <!-- Video Modal -->
    <div v-if="videoModal.show" class="modal-overlay" @click.self="closeVideo">
      <div class="modal-dialog-custom">
        <div class="modal-header-custom">
          <span>{{ videoModal.title }}</span>
          <button @click="closeVideo" style="background:none;border:none;color:#fff;font-size:1.5rem;cursor:pointer;">&times;</button>
        </div>
        <div style="position:relative;padding-bottom:56.25%;height:0;">
          <iframe :src="videoModal.url" style="position:absolute;top:0;left:0;width:100%;height:100%;" frameborder="0" allowfullscreen></iframe>
        </div>
      </div>
    </div>

    <!-- Convide Modal -->
    <div v-if="showConvideModal" class="modal-overlay" @click.self="showConvideModal = false">
      <div class="modal-dialog-custom">
        <div class="modal-header-custom">
          <span>Aumente seus ganhos convidando seus amigos!</span>
          <button @click="showConvideModal = false" style="background:none;border:none;color:#fff;font-size:1.5rem;cursor:pointer;">&times;</button>
        </div>
        <div style="padding:1.5rem;background:#fff;">
          <p><strong>Quer maximizar seus ganhos com nosso programa de cashback?</strong> É simples! Ao compartilhar seu link de indicação exclusivo com amigos, familiares e colegas, você pode ganhar ainda mais com as compras que eles fazem.</p>
          <p><strong>Seu link de indicação:</strong></p>
          <div style="display:flex;gap:.5rem;align-items:center;">
            <input type="text" :value="linkIndicacao" readonly style="flex:1;border:1px solid #d8d8d8;border-radius:6px;padding:.4rem .75rem;font-size:.85rem;background:#f8f9fa;" />
            <button class="bt-default" style="width:auto;padding:0 14px;height:34px;" @click="copiarLink">{{ copiado ? '✓ Copiado!' : 'Copiar' }}</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: 'agencia-auth' });

const agenciaStore = useAgenciaStore();
const api = useApi();

const user = computed(() => agenciaStore.dadosUser);
const loading = ref(true);
const copiado = ref(false);
const showConvideModal = ref(false);
const metaTab = ref(1);

const totalizadores = ref({ rede: 0, saldo: 0, ecossistemas: 0, pontos: 0 });
const metaCompleted = ref(false);
const progressValue = ref(0);
const targetValue = ref(0);

const countdownTime = ref('00:00');
let timerInterval: ReturnType<typeof setInterval> | null = null;

const videoModal = ref({ show: false, url: '', title: '' });

const linkIndicacao = computed(() => {
  const login = user.value?.login || '';
  return `https://quantashop.com.br/register/${login}`;
});

const metaTabLabel = computed(() => metaTab.value === 1 ? 'diária' : metaTab.value === 2 ? 'semanal' : 'mensal');
const metaTabValue = computed(() => metaTab.value === 1 ? 14.4 : metaTab.value === 2 ? 100.8 : 432.0);

function formatCurrency(v: number | null | undefined): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

async function copiarLink() {
  try {
    await navigator.clipboard.writeText(linkIndicacao.value);
    copiado.value = true;
    setTimeout(() => { copiado.value = false; }, 2000);
  } catch { /**/ }
}

function openVideo(url: string, title: string) {
  videoModal.value = { show: true, url, title };
}

function closeVideo() {
  videoModal.value = { show: false, url: '', title: '' };
}

function authHeader() {
  const token = agenciaStore.getToken();
  return { headers: { Authorization: `Bearer ${token}` } };
}

function startTimer() {
  const now = new Date();
  const midnight = new Date(now);
  midnight.setHours(24, 0, 0, 0);
  let remaining = Math.floor((midnight.getTime() - now.getTime()) / 1000);

  function tick() {
    if (remaining <= 0) { remaining = 86400; }
    const m = Math.floor(remaining / 60).toString().padStart(2, '0');
    const s = (remaining % 60).toString().padStart(2, '0');
    countdownTime.value = `${m}:${s}`;
    remaining--;
  }
  tick();
  timerInterval = setInterval(tick, 1000);
}

onMounted(async () => {
  agenciaStore.loadFromStorage();
  if (!agenciaStore.checkTokenExpiry()) {
    window.location.href = '/agencia/login';
    return;
  }

  startTimer();

  await Promise.allSettled([
    api.get('/Dashboard/obterBarraStatus', authHeader()).then(r => {
      const d = r.data?.data ?? r.data;
      if (d) totalizadores.value = { rede: d.rede ?? 0, saldo: d.saldo ?? 0, ecossistemas: d.ecossistemas ?? 0, pontos: d.pontos ?? 0 };
    }).catch(() =>
      api.get('/v2/dashboard/get-totalizers-user', authHeader()).then(r => {
        const d = r.data?.data ?? r.data;
        if (d) totalizadores.value = { rede: d.rede ?? 0, saldo: d.saldo ?? 0, ecossistemas: d.ecossistemas ?? 0, pontos: d.pontos ?? 0 };
      }).catch(() => {})
    ),
    api.get('/MetaMinuto/obterMetaMinuto', authHeader()).then(r => {
      const d = r.data;
      if (d) {
        const val = d.valorPorMinuto ?? d.ValorPorMinuto ?? 0;
        targetValue.value = Math.abs(val);
        const meta = metaTabValue.value;
        const acumulado = Math.abs(val) * (d.minutos ?? 1);
        progressValue.value = Math.min(100, (acumulado / meta) * 100);
        metaCompleted.value = progressValue.value >= 100;
      }
    }).catch(() => {}),
  ]);

  loading.value = false;
});

onUnmounted(() => {
  if (timerInterval) clearInterval(timerInterval);
});
</script>

<style scoped>
.text-green { color: #98c73a !important; }
.color { color: #98c73a !important; }

.gap-20 { gap: 1.25rem; }

.card {
  border: 1px solid rgba(0,0,0,.125);
  border-radius: .5rem;
  margin-bottom: 1.25rem;
  padding: .75rem 1rem;
}

.card-header-section {
  border-bottom: 1px solid #f0f0f0;
  padding-bottom: .5rem;
  margin-bottom: .75rem;
}

.indicador {
  flex-direction: column;
  text-align: center;
  padding: .5rem .25rem;
  border-right: 1px solid #f0f0f0;
}
.indicador:last-child { border-right: none; }

.embed-container {
  position: relative;
  overflow: hidden;
  border-radius: 8px;
}
.cover-image {
  width: 100%;
  height: 140px;
  object-fit: cover;
  display: block;
  border-radius: 8px;
}
.play-overlay {
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0,0,0,.3);
  border-radius: 8px;
  transition: background .2s;
}
.embed-container:hover .play-overlay { background: rgba(0,0,0,.5); }
.play-btn {
  color: #fff;
  font-size: 2rem;
  line-height: 1;
}
.cursor-pointer { cursor: pointer; }

.green-button-2 {
  background-color: #98c73a;
  color: #fff;
  border: none;
  border-radius: 4px;
  padding: .5rem 1rem;
  font-weight: 600;
  cursor: pointer;
  text-decoration: none;
  display: inline-block;
  text-align: center;
  transition: background .2s;
}
.green-button-2:hover { background-color: #82ac2e; color: #fff; }

.meta-minuto-button { margin: 0 3px; }
.active-button { background-color: #2f7785 !important; }

.countdown-time { color: #2f7785; }

.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,.7);
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 1rem;
}
.modal-dialog-custom {
  background: #fff;
  border-radius: 8px;
  width: 100%;
  max-width: 700px;
  overflow: hidden;
}
.modal-header-custom {
  background: #2f7785;
  color: #fff;
  padding: .75rem 1rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-weight: 600;
}

.ag-loading {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 200px;
}
</style>
