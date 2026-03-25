<template>
  <section class="qs-ceo">
    <div class="container">
      <div class="qs-ceo__card">
        <div class="qs-ceo__content">
          <span class="qs-ceo__tag">CEO & Founder</span>
          <p class="qs-ceo__pre">Fale com o CEO</p>
          <h2 class="qs-ceo__name">Mauro Triumph</h2>
          <p class="qs-ceo__desc">Clareza estratégica sem rodeios, sem burocracia, sem perda de tempo.</p>
          <a href="https://api.whatsapp.com/send/?phone=552140404866&text&type=phone_number&app_absent=0" target="_blank" rel="noopener" class="qs-ceo__btn">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor"><path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/></svg>
            Iniciar Conversa
          </a>
        </div>
        <div class="qs-ceo__badges">
          <div class="qs-ceo__badge">
            <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2"><path d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"/></svg>
            <div>
              <div class="qs-ceo__badge-label">Respostas</div>
              <div class="qs-ceo__badge-value">Em até 24h</div>
            </div>
          </div>
          <div class="qs-ceo__badge">
            <svg width="16" height="16" fill="none" viewBox="0 0 24 24" stroke="#2F7785" stroke-width="2"><path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/></svg>
            <div>
              <div class="qs-ceo__badge-label">Parcerias</div>
              <div class="qs-ceo__badge-value">+200 fechadas</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.qs-ceo {
  padding: 72px 0;
  background: #fff;
}

.qs-ceo__card {
  background: linear-gradient(135deg, #1a4a54 0%, #225F6B 50%, #2F7785 100%);
  border-radius: 20px;
  padding: 52px 56px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 40px;
  position: relative;
  overflow: hidden;
}

.qs-ceo__card::before {
  content: '';
  position: absolute;
  right: -60px;
  top: -60px;
  width: 280px;
  height: 280px;
  border-radius: 50%;
  background: rgba(255,255,255,0.04);
}

@media (max-width: 767px) {
  .qs-ceo__card {
    flex-direction: column;
    padding: 36px 24px;
    text-align: center;
  }
}

.qs-ceo__content {
  position: relative;
  z-index: 1;
}

.qs-ceo__tag {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: rgba(255,255,255,0.12);
  border: 1px solid rgba(255,255,255,0.20);
  border-radius: 999px;
  color: rgba(255,255,255,0.80);
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  font-weight: 600;
  padding: 4px 14px;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  margin-bottom: 12px;
}

.qs-ceo__pre {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 500;
  color: rgba(255,255,255,0.70);
  margin-bottom: 4px;
  text-transform: uppercase;
  letter-spacing: 0.08em;
}

.qs-ceo__name {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: clamp(28px, 4vw, 40px);
  font-weight: 800;
  color: #98C73A;
  letter-spacing: -0.02em;
  margin-bottom: 12px;
}

.qs-ceo__desc {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  color: rgba(255,255,255,0.75);
  line-height: 1.5;
  max-width: 360px;
  margin-bottom: 28px;
}

.qs-ceo__btn {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: #98C73A;
  color: #fff;
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 14px;
  font-weight: 700;
  padding: 13px 24px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.qs-ceo__btn:hover {
  background: #7aad1f;
  color: #fff;
  transform: translateY(-2px);
}

.qs-ceo__badges {
  display: flex;
  flex-direction: column;
  gap: 12px;
  flex-shrink: 0;
  position: relative;
  z-index: 1;
}

.qs-ceo__badge {
  display: flex;
  align-items: center;
  gap: 12px;
  background: rgba(255,255,255,0.95);
  backdrop-filter: blur(8px);
  border-radius: 12px;
  padding: 14px 20px;
  min-width: 180px;
}

.qs-ceo__badge-label {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 11px;
  color: #6b7280;
  margin-bottom: 2px;
}

.qs-ceo__badge-value {
  font-family: 'Inter', 'Jost', sans-serif;
  font-size: 15px;
  font-weight: 700;
  color: #111827;
}
</style>
