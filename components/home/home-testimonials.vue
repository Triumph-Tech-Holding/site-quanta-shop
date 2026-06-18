<template>
  <section class="qs-tst" aria-labelledby="qs-tst-title">
    <div class="qs-tst__container">
      <div class="qs-tst__head">
        <span class="qs-tst__eyebrow">Histórias reais</span>
        <h2 id="qs-tst-title" class="qs-tst__h2">Testemunhos de usuários reais</h2>
        <p class="qs-tst__lead">Veja quanto nossos usuários já economizaram.</p>
      </div>

      <div class="qs-tst__grid">
        <div v-for="(t, i) in testimonials" :key="i" class="qs-tst__card">
          <div class="qs-tst__stars" aria-label="5 de 5 estrelas">
            <svg v-for="n in 5" :key="n" viewBox="0 0 24 24" aria-hidden="true"><path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01z" /></svg>
          </div>
          <p class="qs-tst__quote"><strong class="qs-tst__hl">{{ t.highlight }}</strong>{{ t.rest }}</p>
          <div class="qs-tst__who">
            <span class="qs-tst__ini" aria-hidden="true">{{ t.initials }}</span>
            <span class="qs-tst__bio">
              <span class="qs-tst__name">{{ t.name }}</span>
              <span class="qs-tst__meta">{{ t.meta }}</span>
            </span>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface Testimonial { initials: string; name: string; meta: string; highlight: string; rest: string }

const testimonials: Testimonial[] = [
  { initials: 'MC', name: 'Marina Costa', meta: 'Usuária há 8 meses', highlight: 'Já recebi mais de R$ 1.240 de cashback', rest: ' em apenas 6 meses. Nunca mais compro sem a Quanta!' },
  { initials: 'RO', name: 'Rafael Oliveira', meta: 'Usuário há 1 ano', highlight: 'A cada compra, o dinheiro volta.', rest: ' Já acumulei R$ 890 sem nenhum esforço extra. Recomendo demais.' },
  { initials: 'JS', name: 'Juliana Santos', meta: 'Usuária há 1 ano', highlight: 'Minha família economizou R$ 2.100 no último ano.', rest: ' É dinheiro de verdade voltando pro bolso.' },
];
</script>

<style scoped>
.qs-tst {
  padding: 84px 0;
  background: #f7f8fa;
}
.qs-tst__container {
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 24px;
}

.qs-tst__head {
  text-align: center;
  max-width: 680px;
  margin: 0 auto 52px;
}
.qs-tst__eyebrow {
  font-size: 12px;
  font-weight: 700;
  letter-spacing: .16em;
  text-transform: uppercase;
  color: #3A9AAD;
}
.qs-tst__h2 {
  font-family: 'Jost','Inter',sans-serif;
  font-size: clamp(26px, 3.8vw, 44px);
  font-weight: 700;
  color: #225F6B;
  line-height: 1.08;
  letter-spacing: -.02em;
  margin: 12px 0 14px;
}
.qs-tst__lead {
  font-family: 'Inter','Jost',sans-serif;
  font-size: 17px;
  color: #6b7280;
}

.qs-tst__grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 22px;
}

.qs-tst__card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 20px;
  padding: 28px;
  margin: 0;
  box-shadow: 0 2px 10px rgba(1, 15, 28, .06);
  display: flex;
  flex-direction: column;
  gap: 0;
}

.qs-tst__stars {
  display: flex;
  gap: 3px;
  margin-bottom: 14px;
}
.qs-tst__stars svg {
  width: 16px;
  height: 16px;
  fill: #FFB342;
}

.qs-tst__quote {
  font-family: 'Inter','Jost',sans-serif;
  font-size: 15px;
  color: #374151;
  margin: 0 0 18px;
  line-height: 1.65;
  padding: 0;
  border: none;
  background: none;
  flex: 1;
}
.qs-tst__hl {
  color: #225F6B;
  font-weight: 700;
}

.qs-tst__who {
  display: flex;
  align-items: center;
  gap: 12px;
  padding-top: 18px;
  border-top: 1px solid #f0f0f0;
}
.qs-tst__ini {
  width: 44px;
  height: 44px;
  border-radius: 50%;
  background: #2F7785;
  color: #fff;
  display: grid;
  place-items: center;
  font-weight: 700;
  font-size: 14px;
  font-family: 'Jost','Inter',sans-serif;
  flex-shrink: 0;
}
.qs-tst__bio {
  display: flex;
  flex-direction: column;
  gap: 2px;
}
.qs-tst__name {
  color: #1d1d1f;
  font-weight: 600;
  font-size: 14px;
  font-family: 'Inter','Jost',sans-serif;
}
.qs-tst__meta {
  font-size: 13px;
  color: #9ca3af;
  font-family: 'Inter','Jost',sans-serif;
}

@media (max-width: 880px) {
  .qs-tst__grid { grid-template-columns: 1fr; }
}
@media (max-width: 760px) {
  .qs-tst { padding: 48px 0; }
  .qs-tst__head { margin-bottom: 30px; }
}
</style>
