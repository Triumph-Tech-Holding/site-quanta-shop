<template>
  <div class="p-0">
    <div class="general-content">

      <div class="content-header px-4 pt-3 pb-2">
        <div style="background:#fff;border-radius:8px;padding:.75rem 1rem;box-shadow:0 2px 8px rgba(0,0,0,.06);">
          <small style="color:#555;display:block;margin-bottom:.4rem;">Link de indicação da rede</small>
          <div style="display:flex;gap:.5rem;align-items:center;">
            <input
              type="text"
              :value="linkIndicacao"
              readonly
              style="flex:1;border:1px solid #d8d8d8;border-radius:6px;padding:.4rem .75rem;font-size:.8rem;background:#f8f9fa;"
            />
            <button class="bt-default" style="width:auto;height:34px;flex-shrink:0;" @click="copiarLink">
              <span style="padding:0 14px;font-size:.8rem;">{{ copiado ? '✓ Copiado!' : 'Copiar link' }}</span>
            </button>
          </div>
        </div>
      </div>

      <div class="page-content">
        <div class="header-page">
          <h2 class="title-page">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width:20px;height:20px;"><path d="M19.14 12.94c.04-.3.06-.61.06-.94 0-.32-.02-.64-.07-.94l2.03-1.58c.18-.14.23-.41.12-.61l-1.92-3.32c-.12-.22-.37-.29-.59-.22l-2.39.96c-.5-.38-1.03-.7-1.62-.94l-.36-2.54c-.04-.24-.24-.41-.48-.41h-3.84c-.24 0-.43.17-.47.41l-.36 2.54c-.59.24-1.13.57-1.62.94l-2.39-.96c-.22-.08-.47 0-.59.22L2.74 8.87c-.12.21-.08.47.12.61l2.03 1.58c-.05.3-.09.63-.09.94s.02.64.07.94l-2.03 1.58c-.18.14-.23.41-.12.61l1.92 3.32c.12.22.37.29.59.22l2.39-.96c.5.38 1.03.7 1.62.94l.36 2.54c.05.24.24.41.48.41h3.84c.24 0 .44-.17.47-.41l.36-2.54c.59-.24 1.13-.56 1.62-.94l2.39.96c.22.08.47 0 .59-.22l1.92-3.32c.12-.22.07-.47-.12-.61l-2.01-1.58zM12 15.6c-1.98 0-3.6-1.62-3.6-3.6s1.62-3.6 3.6-3.6 3.6 1.62 3.6 3.6-1.62 3.6-3.6 3.6z"/></svg>
            Painel geral
          </h2>
        </div>

        <div v-if="loading" class="ag-loading"><div class="spinner-border" /></div>

        <template v-else>
          <div class="row-flex px-3" style="gap:1.25rem;">
            <div class="col-main" style="min-width:0;">

              <div class="section-box">
                <div style="display:flex;gap:1rem;flex-wrap:wrap;">
                  <div v-for="stat in stats" :key="stat.label" style="flex:1;min-width:100px;text-align:center;">
                    <div style="font-size:1.5rem;font-weight:700;color:#225f6b;">{{ stat.value }}</div>
                    <div style="font-size:.75rem;color:#6c757d;text-transform:uppercase;">{{ stat.label }}</div>
                  </div>
                </div>
              </div>

              <div class="comecar-section">
                <h3>Comece por aqui!</h3>
                <p style="font-size:.85rem;color:#555;margin-bottom:.75rem;">
                  Explore nossa plataforma com os vídeos abaixo. Cada um deles detalha uma peça essencial de como ganhar por minuto, ajudando você a aproveitar ao máximo as funcionalidades disponíveis.
                </p>
                <div class="videos-row">
                  <a v-for="video in videos" :key="video.label" :href="video.url" target="_blank" class="video-card" style="min-height:100px;">
                    <div style="background:linear-gradient(135deg,#1a5c3a,#2f7785);width:100%;padding:40px 0;display:flex;flex-direction:column;align-items:center;justify-content:center;">
                      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="white" style="width:40px;height:40px;opacity:.9;margin-bottom:.5rem;"><path d="M8 5v14l11-7z"/></svg>
                    </div>
                    <div class="video-label">{{ video.label }}</div>
                  </a>
                </div>
              </div>

              <div class="quanta-amizade-section">
                <h3>Quanta Amizade</h3>
                <p>
                  Cada amigo indicado que alcance os primeiros R$50 em cashback, você ganha R$25 e ele ganha mais R$25. Aproveite a chance de juntar a galera e ganhar muito dinheiro por minuto. Quanto mais amigos você tiver, mais você ganha!
                </p>
                <div class="amizade-banner" style="background:linear-gradient(to right,#1a5c3a,#98c73a);padding:1.5rem;border-radius:8px;color:#fff;display:flex;align-items:center;gap:1rem;flex-wrap:wrap;">
                  <div style="flex:1;min-width:200px;">
                    <div style="font-size:1.1rem;font-weight:700;margin-bottom:.5rem;">Conta de Consumo Remunerada</div>
                    <div style="font-size:.85rem;opacity:.9;">Válida para lojas e clientes</div>
                    <div style="display:flex;gap:1.5rem;margin-top:.75rem;font-size:.8rem;">
                      <div><div style="font-weight:700;font-size:1.1rem;">R$50</div><div>1 indicação</div></div>
                      <div><div style="font-weight:700;font-size:1.1rem;">R$500</div><div>10 indicações</div></div>
                      <div><div style="font-weight:700;font-size:1.1rem;">R$5.000</div><div>100 indicações</div></div>
                    </div>
                  </div>
                </div>
              </div>

              <div class="lojas-map-section">
                <h3>Encontre as lojas parceiras mais próximas de você!</h3>
                <p>Descubra estabelecimentos próximos que oferecem cashback na sua região. Nosso mapa interativo exibe as opções mais próximas, facilitando sua busca para economizar e ganhar dinheiro enquanto faz compras.</p>
                <iframe
                  src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3675!2d-46.633!3d-23.55!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zMjPCsDMzJzAwLjAiUyA0NsKwMzgnMDAuMCJX!5e0!3m2!1spt-BR!2sbr!4v1234567890"
                  loading="lazy"
                  referrerpolicy="no-referrer-when-downgrade"
                  title="Mapa de lojas parceiras"
                ></iframe>
              </div>

            </div>

            <div class="col-side" style="width:280px;flex-shrink:0;">

              <div class="action-buttons" style="margin-bottom:1rem;">
                <button class="btn-action btn-primary-action" @click="compartilharLink">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width:16px;height:16px;"><path d="M18 16.08c-.76 0-1.44.3-1.96.77L8.91 12.7c.05-.23.09-.46.09-.7s-.04-.47-.09-.7l7.05-4.11c.54.5 1.25.81 2.04.81 1.66 0 3-1.34 3-3s-1.34-3-3-3-3 1.34-3 3c0 .24.04.47.09.7L8.04 9.81C7.5 9.31 6.79 9 6 9c-1.66 0-3 1.34-3 3s1.34 3 3 3c.79 0 1.5-.31 2.04-.81l7.12 4.16c-.05.21-.08.43-.08.65 0 1.61 1.31 2.92 2.92 2.92 1.61 0 2.92-1.31 2.92-2.92s-1.31-2.92-2.92-2.92z"/></svg>
                  Convide seus amigos
                </button>
                <NuxtLink to="/agencia/painel/meus-credenciamentos" class="btn-action btn-secondary-action" style="text-align:center;">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width:16px;height:16px;"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm5 11h-4v4h-2v-4H7v-2h4V7h2v4h4v2z"/></svg>
                  Credenciar um estabelecimento
                </NuxtLink>
              </div>

              <div class="meta-minuto-card" v-if="metaMinuto">
                <h4>Meta Minuto</h4>
                <div class="timer-display">{{ timerDisplay }}</div>
                <div class="meta-valor" v-if="metaMinuto.valorPorMinuto">
                  {{ formatCurrency(-Math.abs(metaMinuto.valorPorMinuto)) }} /min
                </div>
                <div class="meta-tabs">
                  <button :class="{active: metaTab==='diario'}" @click="metaTab='diario'">Diário</button>
                  <button :class="{active: metaTab==='semanal'}" @click="metaTab='semanal'">Semanal</button>
                  <button :class="{active: metaTab==='mensal'}" @click="metaTab='mensal'">Mensal</button>
                </div>
                <p>Quanto você precisa acumular por minuto para alcançar sua meta?</p>
              </div>

              <div class="assinante-promo-card" v-if="!user?.assinaturaHabilitada">
                <div class="promo-header">
                  DESCUBRA VANTAGENS EXCLUSIVAS AO SE TORNAR UM ASSINANTE
                </div>
                <p>
                  Ganhe cashback em dobro ao assinar nosso plano mensal da assinatura que a inteligência artificial te explica como ganhar mais para as suas indicações.
                </p>
                <NuxtLink to="/agencia/painel/assinatura" class="btn-assinar">Aproveite agora mesmo</NuxtLink>
              </div>

              <div class="whatsapp-card">
                <h4>Conheça nosso canal!</h4>
                <p>
                  Participe do nosso canal no WhatsApp e fique por dentro de ofertas especiais, cupons de desconto, notificações diárias e as últimas novidades exclusivas. Não perca a oportunidade de receber vantagens direto no celular!
                </p>
                <a href="https://chat.whatsapp.com/quantashop" target="_blank" class="btn-whatsapp">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width:18px;height:18px;"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/></svg>
                  Entrar
                </a>
              </div>

            </div>
          </div>
        </template>
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
const metaTab = ref('diario');
const timerSeconds = ref(0);
let timerInterval: ReturnType<typeof setInterval> | null = null;

const stats = ref([
  { label: 'Equipe', value: '—' },
  { label: 'Saldo', value: '—' },
  { label: 'Encaminhamento', value: '—' },
  { label: 'Pontos', value: '—' },
]);

const metaMinuto = ref<{ valorPorMinuto?: number } | null>(null);

const videos = [
  { label: 'Bem-vindo', url: 'https://www.youtube.com/watch?v=bem-vindo' },
  { label: 'Como funciona', url: 'https://www.youtube.com/watch?v=como-funciona' },
  { label: 'Como ganhar', url: 'https://www.youtube.com/watch?v=como-ganhar' },
];

const linkIndicacao = computed(() => {
  const login = user.value?.login || '';
  return `https://quantashop.com.br/register/${login}`;
});

const timerDisplay = computed(() => {
  const m = Math.floor(timerSeconds.value / 60).toString().padStart(2, '0');
  const s = (timerSeconds.value % 60).toString().padStart(2, '0');
  return `${m}:${s}`;
});

function formatCurrency(v: number | null): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
}

async function copiarLink() {
  try {
    await navigator.clipboard.writeText(linkIndicacao.value);
    copiado.value = true;
    setTimeout(() => { copiado.value = false; }, 2000);
  } catch { /**/ }
}

function compartilharLink() {
  copiarLink();
}

function authHeader() {
  const token = agenciaStore.getToken();
  return { headers: { Authorization: `Bearer ${token}` } };
}

function startTimer() {
  timerInterval = setInterval(() => {
    timerSeconds.value++;
  }, 1000);
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
      const d = r.data;
      if (d) {
        stats.value = [
          { label: 'Equipe', value: String(d.Total ?? d.total ?? d.equipe ?? '—') },
          { label: 'Saldo', value: formatCurrency(d.Saldo ?? d.saldo ?? null) },
          { label: 'Encaminhamento', value: String(d.Encaminhamento ?? d.encaminhamento ?? d.TotalEncaminhamento ?? '—') },
          { label: 'Pontos', value: String(d.Pontos ?? d.pontos ?? '—') },
        ];
      }
    }),
    api.get('/MetaMinuto/obterMetaMinuto', authHeader()).then(r => {
      metaMinuto.value = r.data ?? null;
    }).catch(() => { metaMinuto.value = null; }),
    api.get('/Tutorial/obterTutoriais', authHeader()).then(r => {
      const d = r.data;
      if (Array.isArray(d) && d.length > 0) {
        videos.splice(0, videos.length, ...d.slice(0, 3).map((v: Record<string, unknown>) => ({
          label: (v.nome ?? v.titulo ?? v.Titulo ?? 'Tutorial') as string,
          url: (v.url ?? v.Url ?? '#') as string,
        })));
      }
    }).catch(() => { /* keep defaults */ }),
  ]);

  loading.value = false;
});

onUnmounted(() => {
  if (timerInterval) clearInterval(timerInterval);
});
</script>
