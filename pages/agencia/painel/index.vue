<template>
  <div class="p-0">
    <div class="general-content">
      <div class="page-content qs-page">

        <QsPageHeader eyebrow="Dashboard" title="Painel Geral" description="Acompanhe seus indicadores, metas e novidades da Quanta Shop." />

        <template>
          <div class="idx-layout">

            <!-- ── COLUNA ESQUERDA ──────────────────────────────── -->
            <div class="idx-col-main">

              <!-- Vídeos -->
              <div class="qs-card-section idx-card">
                <div class="idx-card-intro">
                  <img src="/agencia/imgs/quanta-play.gif" alt="" class="idx-play-gif" @click="openVideo('https://www.youtube.com/embed/UYJNhfIJ0Lw?si=hL1BgsJ5SDje8qzg', 'Comece por aqui')" />
                  <div>
                    <div class="idx-card-title">Comece por aqui</div>
                    <p class="idx-card-desc">Explore nossa plataforma com os vídeos abaixo. Cada um deles detalha uma parte essencial de como ganhar por minuto, ajudando você a aproveitar ao máximo todas as funcionalidades disponíveis.</p>
                  </div>
                </div>
                <div class="idx-videos-grid">
                  <div class="idx-video-thumb" @click="openVideo('https://www.youtube.com/embed/oQdZeEagCfU?si=hL1BgsJ5SDje8qzg', 'Bem-vindo')">
                    <img src="/agencia/imgs/bem-vindo.jpg" alt="Bem-vindo" />
                    <div class="idx-play-overlay"><span class="idx-play-btn">▶</span></div>
                  </div>
                  <div class="idx-video-thumb" @click="openVideo('https://www.youtube.com/embed/R4QFuyN9yAo?si=hlhSEAgUTzXH17dE', 'Como Funciona')">
                    <img src="/agencia/imgs/como-funciona.jpg" alt="Como funciona" />
                    <div class="idx-play-overlay"><span class="idx-play-btn">▶</span></div>
                  </div>
                  <div class="idx-video-thumb" @click="openVideo('https://www.youtube.com/embed/CFDNYh_Iigs', 'Como Ganhar')">
                    <img src="/agencia/imgs/como-ganhar.jpg" alt="Como Ganhar" />
                    <div class="idx-play-overlay"><span class="idx-play-btn">▶</span></div>
                  </div>
                </div>
              </div>

              <!-- Quanta Amizade -->
              <div class="qs-card-section idx-card">
                <div class="idx-card-intro">
                  <img src="/agencia/imgs/comece-por-aqui.png" alt="" class="idx-icon-img" />
                  <div>
                    <div class="idx-card-title">Quanta Amizade</div>
                    <p class="idx-card-desc">A cada amigo indicado que alcançar os primeiros R$ 50 em cashback, você ganha R$ 25 e ele ganha mais R$ 25! Aproveite a chance de juntar a galera e ganhar juntos.</p>
                  </div>
                </div>
                <a href="https://bigcash.blob.core.windows.net/documentos/CAMPANHA%20QUANTA%20AMIZADE.pdf" target="_blank" class="idx-banner-link">
                  <img src="/agencia/imgs/Banner-Quanta-Bank-01.jpg" alt="Quanta Amizade" class="idx-banner-img idx-banner-img--desk" />
                  <img src="/agencia/imgs/BannerMob-Quanta-Bank-01.jpg" alt="Quanta Amizade" class="idx-banner-img idx-banner-img--mob" />
                </a>
              </div>

              <!-- Mapa -->
              <div class="qs-card-section idx-card">
                <div class="idx-card-intro">
                  <img src="/agencia/imgs/loja.png" alt="Loja" class="idx-icon-img" />
                  <div>
                    <div class="idx-card-title">Lojas parceiras mais próximas de você</div>
                    <p class="idx-card-desc">Descubra rapidamente os parceiros locais que oferecem cashback na sua região.</p>
                  </div>
                </div>
                <div class="idx-map-wrap">
                  <iframe
                    src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3675!2d-46.633!3d-23.55!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zMjPCsDMzJzAwLjAiUyA0NsKwMzgnMDAuMCJX!5e0!3m2!1spt-BR!2sbr!4v1234567890"
                    style="border:0;width:100%;height:48vh;min-height:280px;"
                    loading="lazy"
                    referrerpolicy="no-referrer-when-downgrade"
                    title="Mapa de lojas parceiras"
                  ></iframe>
                </div>
              </div>

            </div>

            <!-- ── COLUNA DIREITA ───────────────────────────────── -->
            <div class="idx-col-side">

              <!-- Indicadores (skeleton → dados reais) -->
              <div class="qs-card-section idx-card idx-card--no-gap">
                <div class="idx-section-label">Indicadores</div>

                <!-- Skeleton loading -->
                <div v-if="loading" class="idx-indicadores">
                  <div v-for="n in 4" :key="n" class="idx-indicador idx-indicador--skeleton">
                    <div class="idx-sk idx-sk--icon"></div>
                    <div class="idx-sk idx-sk--val"></div>
                    <div class="idx-sk idx-sk--lbl"></div>
                  </div>
                </div>

                <!-- Dados reais -->
                <div v-else class="idx-indicadores">
                  <div class="idx-indicador">
                    <div class="idx-indicador__icon idx-indicador__icon--teal">
                      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-8 0c1.66 0 2.99-1.34 2.99-3S9.66 5 8 5C6.34 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
                    </div>
                    <div class="idx-indicador__valor">{{ totalizadores.rede }}</div>
                    <div class="idx-indicador__label">Equipe</div>
                  </div>
                  <div class="idx-indicador">
                    <div class="idx-indicador__icon idx-indicador__icon--lime">
                      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
                    </div>
                    <div class="idx-indicador__valor idx-indicador__valor--small">{{ formatCurrency(totalizadores.saldo) }}</div>
                    <div class="idx-indicador__label">Saldo</div>
                  </div>
                  <div class="idx-indicador">
                    <div class="idx-indicador__icon idx-indicador__icon--gold">
                      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-8 0c1.66 0 2.99-1.34 2.99-3S9.66 5 8 5C6.34 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
                    </div>
                    <div class="idx-indicador__valor">{{ totalizadores.ecossistemas }}</div>
                    <div class="idx-indicador__label">Encaminhamentos</div>
                  </div>
                  <div class="idx-indicador">
                    <div class="idx-indicador__icon idx-indicador__icon--teal">
                      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"/></svg>
                    </div>
                    <div class="idx-indicador__valor">{{ totalizadores.pontos }}</div>
                    <div class="idx-indicador__label">Pontos</div>
                  </div>
                </div>
              </div>

              <!-- Convide -->
              <div class="qs-card-section idx-card">
                <div class="idx-section-label">Convide</div>
                <div class="idx-convide-btns">
                  <button class="qs-btn-primary idx-btn-full" @click="showConvideModal = true">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M18 16.08c-.76 0-1.44.3-1.96.77L8.91 12.7c.05-.23.09-.46.09-.7s-.04-.47-.09-.7l7.05-4.11c.54.5 1.25.81 2.04.81 1.66 0 3-1.34 3-3s-1.34-3-3-3-3 1.34-3 3c0 .24.04.47.09.7L8.04 9.81C7.5 9.31 6.79 9 6 9c-1.66 0-3 1.34-3 3s1.34 3 3 3c.79 0 1.5-.31 2.04-.81l7.12 4.16c-.05.21-.08.43-.08.65 0 1.61 1.31 2.92 2.92 2.92s2.92-1.31 2.92-2.92-1.31-2.92-2.92-2.92z"/></svg>
                    Convide seus amigos
                  </button>
                  <NuxtLink to="/agencia/painel/meus-credenciamentos" class="qs-btn-secondary idx-btn-full">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 3c1.93 0 3.5 1.57 3.5 3.5S13.93 13 12 13s-3.5-1.57-3.5-3.5S10.07 6 12 6zm7 13H5v-.23c0-.62.28-1.2.76-1.58C7.47 15.82 9.64 15 12 15s4.53.82 6.24 2.19c.48.38.76.97.76 1.58V19z"/></svg>
                    Credencie um estabelecimento
                  </NuxtLink>
                </div>
              </div>

              <!-- Meta Minuto -->
              <div class="qs-card-section idx-card">
                <div class="idx-meta-header">
                  <img src="/agencia/imgs/meta-minuto.png" alt="Meta Minuto" class="idx-meta-icon" />
                  <div class="idx-section-label">Meta Minuto</div>
                </div>
                <div v-if="metaCompleted" class="idx-meta-ok">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/></svg>
                  Parabéns, você atingiu sua meta diária!
                </div>
                <div v-else>
                  <div class="idx-meta-row">
                    <span class="idx-countdown">{{ countdownTime }}</span>
                    <span class="idx-meta-target">{{ formatCurrency(targetValue) }}</span>
                  </div>
                  <div class="idx-meta-bar">
                    <div class="idx-meta-fill" :style="{ width: progressValue + '%' }" />
                  </div>
                  <div class="idx-meta-legend">Sua meta {{ metaTabLabel }} é de {{ formatCurrency(metaTabValue) }}</div>
                  <div class="idx-meta-tabs">
                    <button class="idx-meta-tab" :class="{ 'idx-meta-tab--ativo': metaTab===1 }" @click="metaTab=1">Diário</button>
                    <button class="idx-meta-tab" :class="{ 'idx-meta-tab--ativo': metaTab===2 }" @click="metaTab=2">Semanal</button>
                    <button class="idx-meta-tab" :class="{ 'idx-meta-tab--ativo': metaTab===3 }" @click="metaTab=3">Mensal</button>
                  </div>
                </div>
              </div>

              <!-- Assinatura Plus Promo -->
              <div class="qs-card-section idx-card idx-plus-promo">
                <div class="idx-plus-promo__badge">⭐ Plus</div>
                <p class="idx-plus-promo__text">Desbloqueie cashback em dobro e acesso ao assistente virtual inteligente!</p>
                <NuxtLink to="/agencia/painel/assinatura" class="qs-btn-primary idx-btn-full">Aproveite agora mesmo</NuxtLink>
              </div>

              <!-- Status Plus -->
              <div class="qs-card-section idx-card idx-card--no-gap">
                <div class="idx-plus-header">
                  <div class="idx-section-label">Assinatura Plus</div>
                  <span class="qs-badge" :class="plusStatus === 'ativo' ? 'qs-badge-success' : 'qs-badge-warn'">
                    {{ plusStatus === 'ativo' ? '⭐ Plus Ativo' : '⏳ Pendente' }}
                  </span>
                </div>
                <div class="idx-plus-body">
                  <p class="idx-plus-sub">Consumo Mínimo do Mês</p>
                  <QsProgressBar
                    :value="progressoConsumoPlus"
                    size="md"
                    :show-label="true"
                    :left-label="`R$ ${consumoMes} / R$ ${adminConfig.metaConsumoPlus}`"
                  />
                  <p class="idx-plus-hint">Atingir o consumo mínimo destrava o Cashback Residual Dobrado na rede.</p>
                  <QsKpiCard
                    label="Ganhos Extras (Residual Dobrado)"
                    :value="ganhosCashbackDobrado"
                    format="currency"
                    :badge="plusStatus === 'ativo' ? '2×' : undefined"
                    badge-tone="success"
                    dot-color="var(--qs-lime, #98C73A)"
                  />
                  <div class="idx-radar-title">Radar da Rede Plus</div>
                  <ul class="idx-radar-list">
                    <li v-for="(m, i) in radarRedePlus" :key="i" class="idx-radar-item">
                      <span class="idx-radar-nome">{{ m.nome }}</span>
                      <span class="qs-badge" :class="m.status === 'Ativo' ? 'qs-badge-success' : 'qs-badge-danger'">{{ m.status }}</span>
                    </li>
                  </ul>
                </div>
              </div>

              <!-- WhatsApp Canal -->
              <div class="qs-card-section idx-card">
                <div class="idx-section-label">Canal no WhatsApp</div>
                <div class="idx-wpp-row">
                  <img src="/agencia/imgs/whatsapp.png" alt="" class="idx-wpp-img" />
                  <p class="idx-wpp-text">Participe do nosso canal no WhatsApp e fique por dentro de ofertas especiais, cupons e novidades exclusivas.</p>
                </div>
                <a href="https://whatsapp.com/channel/0029VaqtQAxD38CZ6ykplz0J" target="_blank" class="idx-wpp-btn">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/></svg>
                  Entrar no canal
                </a>
              </div>

            </div>
          </div>
        </template>

      </div>
    </div>

    <!-- Modal: Vídeo -->
    <div v-if="videoModal.show" class="qs-modal-overlay" @click.self="closeVideo">
      <div class="qs-modal" style="max-width:700px;">
        <div class="qs-modal-header">
          <span>{{ videoModal.title }}</span>
          <button @click="closeVideo" class="qs-modal-close">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>
          </button>
        </div>
        <div style="position:relative;padding-bottom:56.25%;height:0;">
          <iframe :src="videoModal.url" style="position:absolute;top:0;left:0;width:100%;height:100%;" frameborder="0" allowfullscreen></iframe>
        </div>
      </div>
    </div>

    <!-- Modal: Convide -->
    <div v-if="showConvideModal" class="qs-modal-overlay" @click.self="showConvideModal = false">
      <div class="qs-modal" style="max-width:560px;">
        <div class="qs-modal-header">
          <span>Aumente seus ganhos convidando amigos!</span>
          <button @click="showConvideModal = false" class="qs-modal-close">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>
          </button>
        </div>
        <div class="qs-modal-body">
          <p>Ao compartilhar seu link de indicação exclusivo com amigos, você ganha com as compras que eles fazem.</p>
          <div class="idx-convide-label">Seu link de indicação</div>
          <div class="idx-convide-link-row">
            <input type="text" :value="linkIndicacao" readonly class="qs-input" />
            <button class="qs-btn-primary" style="flex-shrink:0;" @click="copiarLink">
              {{ copiado ? '✓ Copiado!' : 'Copiar' }}
            </button>
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

const adminConfig = { metaConsumoPlus: 200 };
const plusStatus = ref<'ativo' | 'pendente'>('pendente');
const consumoMes = ref(0);
const ganhosCashbackDobrado = ref(0);
const radarRedePlus = ref([
  { nome: 'Ana Lima', status: 'Ativo' },
  { nome: 'Carlos Mendes', status: 'Inadimplente' },
  { nome: 'Fernanda Costa', status: 'Ativo' },
  { nome: 'Roberto Silva', status: 'Inadimplente' },
  { nome: 'Juliana Reis', status: 'Ativo' },
]);
const progressoConsumoPlus = computed(() =>
  Math.min(100, Math.round((consumoMes.value / adminConfig.metaConsumoPlus) * 100))
);

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
/* Layout */
.idx-layout {
  display: grid;
  grid-template-columns: 1fr 340px;
  gap: 1.25rem;
  align-items: start;
}
@media (max-width: 900px) {
  .idx-layout { grid-template-columns: 1fr; }
  .idx-col-side { order: -1; }
}

.idx-col-main { display: flex; flex-direction: column; gap: 1.25rem; }
.idx-col-side { display: flex; flex-direction: column; gap: 1rem; }

/* Card base */
.idx-card { background: #fff; }
.idx-card--no-gap { padding-bottom: 0; }

/* Card intro row */
.idx-card-intro {
  display: flex;
  align-items: flex-start;
  gap: .875rem;
  margin-bottom: 1rem;
}
.idx-play-gif { width: 52px; height: 52px; object-fit: contain; cursor: pointer; flex-shrink: 0; }
.idx-icon-img { width: 40px; height: 40px; object-fit: contain; flex-shrink: 0; }
.idx-card-title {
  font-size: 1rem;
  font-weight: 700;
  color: var(--qs-teal-dark, #225F6B);
  margin-bottom: .25rem;
}
.idx-card-desc { font-size: .8125rem; color: var(--qs-gray-500, #6b7280); line-height: 1.5; margin: 0; }

/* Videos */
.idx-videos-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: .625rem;
}
@media (max-width: 480px) { .idx-videos-grid { grid-template-columns: 1fr; } }
.idx-video-thumb {
  position: relative;
  border-radius: var(--qs-radius-md, 12px);
  overflow: hidden;
  cursor: pointer;
}
.idx-video-thumb img { width: 100%; height: 130px; object-fit: cover; display: block; }
.idx-play-overlay {
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0,0,0,.3);
  transition: background .2s;
}
.idx-video-thumb:hover .idx-play-overlay { background: rgba(0,0,0,.5); }
.idx-play-btn { color: #fff; font-size: 2rem; line-height: 1; }

/* Banner */
.idx-banner-link { display: block; border-radius: var(--qs-radius-md, 12px); overflow: hidden; }
.idx-banner-img { width: 100%; display: none; }
.idx-banner-img--desk { display: block; }
@media (max-width: 600px) {
  .idx-banner-img--desk { display: none; }
  .idx-banner-img--mob { display: block; }
}

/* Map */
.idx-map-wrap { border-radius: var(--qs-radius-md, 12px); overflow: hidden; border: 1px solid var(--qs-gray-200, #e5e7eb); }

/* Section label */
.idx-section-label {
  font-size: .75rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .07em;
  color: var(--qs-teal-dark, #225F6B);
  margin-bottom: .875rem;
}

/* Skeleton */
.idx-sk {
  border-radius: 6px;
  background: linear-gradient(90deg, #f0f0f0 25%, #f8f8f8 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: idx-shimmer 1.4s infinite;
}
@keyframes idx-shimmer {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
.idx-sk--icon { width: 36px; height: 36px; border-radius: 9px; margin-bottom: .2rem; }
.idx-sk--val { width: 48px; height: 14px; margin-bottom: .3rem; }
.idx-sk--lbl { width: 36px; height: 9px; }
.idx-indicador--skeleton { pointer-events: none; }

/* Indicadores */
.idx-indicadores {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 0;
}
@media (max-width: 400px) { .idx-indicadores { grid-template-columns: repeat(2, 1fr); } }
.idx-indicador {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: .25rem;
  padding: .75rem .5rem;
  text-align: center;
  border-right: 1px solid var(--qs-gray-100, #f5f5f7);
}
.idx-indicador:last-child { border-right: none; }
.idx-indicador__icon {
  width: 36px;
  height: 36px;
  border-radius: 9px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: .2rem;
}
.idx-indicador__icon svg { width: 18px; height: 18px; color: #fff; }
.idx-indicador__icon--teal { background: var(--qs-teal, #2F7785); }
.idx-indicador__icon--lime { background: var(--qs-lime, #98C73A); }
.idx-indicador__icon--gold { background: #FFB342; }
.idx-indicador__valor {
  font-size: 1rem;
  font-weight: 700;
  color: var(--qs-ink, #1d1d1f);
  line-height: 1.1;
}
.idx-indicador__valor--small { font-size: .8125rem; }
.idx-indicador__label { font-size: .6rem; font-weight: 600; text-transform: uppercase; letter-spacing: .04em; color: var(--qs-gray-400, #9ca3af); }

/* Convide */
.idx-convide-btns { display: flex; flex-direction: column; gap: .625rem; }
.idx-btn-full {
  width: 100%;
  justify-content: center;
  display: inline-flex;
  align-items: center;
  gap: .4rem;
  font-size: .8125rem;
}
.idx-btn-full svg { width: 15px; height: 15px; }

/* Meta Minuto */
.idx-meta-header { display: flex; align-items: center; gap: .5rem; margin-bottom: .75rem; }
.idx-meta-icon { width: 28px; height: 28px; object-fit: contain; }
.idx-meta-ok {
  display: flex;
  align-items: center;
  gap: .4rem;
  background: #dcfce7;
  color: #16a34a;
  padding: .625rem .875rem;
  border-radius: var(--qs-radius-md, 12px);
  font-size: .8125rem;
  font-weight: 600;
}
.idx-meta-ok svg { width: 16px; height: 16px; flex-shrink: 0; }
.idx-meta-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: .5rem; }
.idx-countdown { font-size: 1.25rem; font-weight: 700; color: var(--qs-teal, #2F7785); font-variant-numeric: tabular-nums; }
.idx-meta-target { font-size: 1rem; font-weight: 700; color: var(--qs-danger, #dc2626); }
.idx-meta-bar { height: 6px; background: var(--qs-gray-100, #f5f5f7); border-radius: var(--qs-radius-pill, 999px); overflow: hidden; margin-bottom: .35rem; }
.idx-meta-fill { height: 100%; background: var(--qs-lime, #98C73A); border-radius: var(--qs-radius-pill, 999px); transition: width .3s; }
.idx-meta-legend { font-size: .6875rem; color: var(--qs-gray-400, #9ca3af); text-align: right; margin-bottom: .625rem; }
.idx-meta-tabs { display: flex; justify-content: center; gap: .25rem; }
.idx-meta-tab {
  padding: .2rem .6rem;
  border: 1px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-pill, 999px);
  background: transparent;
  font-size: .6875rem;
  font-weight: 600;
  color: var(--qs-gray-500, #6b7280);
  cursor: pointer;
  transition: all .15s;
}
.idx-meta-tab--ativo { background: var(--qs-teal, #2F7785); border-color: var(--qs-teal, #2F7785); color: #fff; }

/* Plus promo */
.idx-plus-promo { text-align: center; background: linear-gradient(135deg, #f0f7f9 0%, #f5fbf0 100%); border: 1.5px solid rgba(152,199,58,.2); }
.idx-plus-promo__badge {
  display: inline-block;
  background: linear-gradient(135deg, #e8950a, #FFB342);
  color: #fff;
  font-size: .625rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .08em;
  padding: .2rem .6rem;
  border-radius: var(--qs-radius-pill, 999px);
  margin-bottom: .75rem;
}
.idx-plus-promo__text { font-size: .8125rem; color: var(--qs-gray-700, #374151); margin-bottom: 1rem; line-height: 1.5; }

/* Status Plus */
.idx-plus-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1.125rem 1.25rem .875rem;
  border-bottom: 1px solid var(--qs-gray-100, #f5f5f7);
}
.idx-plus-body { padding: 1rem 1.25rem 1.25rem; display: flex; flex-direction: column; gap: .875rem; }
.idx-plus-sub { font-size: .75rem; color: var(--qs-gray-500, #6b7280); margin: 0; }
.idx-plus-hint { font-size: .6875rem; color: var(--qs-gray-400, #9ca3af); margin: 0; line-height: 1.4; }
.idx-radar-title { font-size: .8125rem; font-weight: 700; color: var(--qs-teal-dark, #225F6B); }
.idx-radar-list { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 0; }
.idx-radar-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: .4rem 0;
  border-bottom: 1px solid var(--qs-gray-100, #f5f5f7);
  font-size: .8rem;
}
.idx-radar-item:last-child { border-bottom: none; }
.idx-radar-nome { color: var(--qs-gray-700, #374151); }

/* WhatsApp */
.idx-wpp-row { display: flex; align-items: flex-start; gap: .875rem; margin-bottom: 1rem; }
.idx-wpp-img { width: 52px; height: 52px; object-fit: contain; flex-shrink: 0; }
.idx-wpp-text { font-size: .8125rem; color: var(--qs-gray-500, #6b7280); line-height: 1.5; margin: 0; }
.idx-wpp-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: .5rem;
  background: #25D366;
  color: #fff;
  font-weight: 600;
  font-size: .875rem;
  padding: .625rem 1.25rem;
  border-radius: var(--qs-radius-md, 12px);
  text-decoration: none;
  transition: all .2s;
  width: 100%;
  box-shadow: 0 3px 10px rgba(37,211,102,.3);
}
.idx-wpp-btn svg { width: 18px; height: 18px; }
.idx-wpp-btn:hover { background: #1ebe5d; color: #fff; transform: translateY(-1px); }

/* Badges */
.qs-badge {
  display: inline-flex;
  padding: .2rem .55rem;
  border-radius: var(--qs-radius-pill, 999px);
  font-size: .6875rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: .04em;
  white-space: nowrap;
}
/* Modal */
.qs-modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,.6);
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 1rem;
}
.qs-modal {
  background: #fff;
  border-radius: var(--qs-radius-lg, 20px);
  width: 100%;
  overflow: hidden;
  box-shadow: var(--qs-shadow-lg, 0 8px 40px rgba(1,15,28,.16));
}
.qs-modal-header {
  background: var(--qs-gradient-btn, linear-gradient(135deg, #225F6B, #2F7785));
  color: #fff;
  padding: .875rem 1.25rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-weight: 600;
  font-size: .9375rem;
}
.qs-modal-close {
  background: none;
  border: none;
  color: #fff;
  cursor: pointer;
  padding: 0;
  display: flex;
  align-items: center;
}
.qs-modal-close svg { width: 20px; height: 20px; }
.qs-modal-body { padding: 1.5rem; }

/* Convide link */
.idx-convide-label {
  font-size: .75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: .05em;
  color: var(--qs-gray-500, #6b7280);
  margin-bottom: .5rem;
}
.idx-convide-link-row { display: flex; gap: .5rem; align-items: center; }

/* Shared */
.qs-input {
  width: 100%;
  padding: .625rem .875rem;
  border: 1.5px solid var(--qs-gray-200, #e5e7eb);
  border-radius: var(--qs-radius-md, 12px);
  font-size: .875rem;
  color: var(--qs-ink, #1d1d1f);
  background: #fff;
  box-sizing: border-box;
}
</style>
