<template>
  <section class="qs-hero">
    <div class="qs-hero__photo-wrap">
      <img :src="photoUrl" :alt="photoAlt" class="qs-hero__photo" />
    </div>
    <div class="qs-hero__overlay"></div>
    <div class="qs-hero__symbol-bg">
      <img src="/img/logo/logo-symbol-white.png" alt="" aria-hidden="true" />
    </div>

    <div class="container qs-hero__body">
      <div class="qs-hero__left">
        <span class="qs-hero__pill">
          <span class="qs-hero__pill-dot"></span>
          {{ badge }}
        </span>
        <h1 class="qs-hero__title">
          <slot name="title">
            {{ title }}<br v-if="titleBreak" />
            <span v-if="titleAccent" class="qs-hero__accent">{{ titleAccent }}</span>
          </slot>
        </h1>
        <p class="qs-hero__subtitle">{{ subtitle }}</p>
        <div class="qs-hero__btns">
          <NuxtLink :href="ctaPrimary.href" class="qs-btn-primary">{{ ctaPrimary.text }}</NuxtLink>
          <a v-if="ctaSecondary" :href="ctaSecondary.href" class="qs-btn-outline-hero">{{ ctaSecondary.text }}</a>
        </div>
      </div>

      <div class="qs-hero__right">
        <div class="qs-hero__badges">
          <div v-for="(fb, i) in floatingBadges" :key="i" class="qs-floating-badge" :class="i === 0 ? 'qs-floating-badge--first' : 'qs-floating-badge--second'">
            <div class="qs-floating-badge__icon" v-html="fb.icon"></div>
            <div class="qs-floating-badge__info">
              <span class="qs-floating-badge__label">{{ fb.label }}</span>
              <strong class="qs-floating-badge__value">{{ fb.value }}</strong>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="stats && stats.length" class="qs-hero__stats-bar">
      <div class="container qs-hero__stats-inner">
        <div v-for="(stat, i) in stats" :key="i" class="qs-hero__stat">
          <strong>{{ stat.value }}</strong>
          <span>{{ stat.label }}</span>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
  badge: string;
  title?: string;
  titleAccent?: string;
  titleBreak?: boolean;
  subtitle: string;
  ctaPrimary: { text: string; href: string };
  ctaSecondary?: { text: string; href: string };
  photoUrl: string;
  photoAlt: string;
  floatingBadges?: { icon: string; label: string; value: string }[];
  stats?: { value: string; label: string }[];
}>(), {
  titleBreak: false,
  floatingBadges: () => [],
  stats: () => [],
});
</script>

<style scoped>
.qs-hero {
  position: relative;
  min-height: 520px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.qs-hero__photo-wrap {
  position: absolute;
  inset: 0;
  z-index: 0;
}
.qs-hero__photo {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center 30%;
  display: block;
}

.qs-hero__overlay {
  position: absolute;
  inset: 0;
  z-index: 1;
  background: linear-gradient(
    100deg,
    rgba(20, 55, 65, 0.88) 0%,
    rgba(34, 95, 107, 0.80) 45%,
    rgba(47, 119, 133, 0.60) 100%
  );
}

.qs-hero__symbol-bg {
  position: absolute;
  right: -2%;
  bottom: -8%;
  z-index: 2;
  opacity: 0.06;
  pointer-events: none;
  width: 380px;
}
.qs-hero__symbol-bg img {
  width: 100%;
  display: block;
  filter: brightness(0) invert(1);
}

.qs-hero__body {
  position: relative;
  z-index: 3;
  display: flex;
  align-items: center;
  gap: 48px;
  padding-top: 80px;
  padding-bottom: 64px;
  flex: 1;
}

.qs-hero__left {
  flex: 1;
  min-width: 0;
}

.qs-hero__right {
  flex: 0 0 320px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.qs-hero__pill {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: rgba(255, 255, 255, 0.12);
  border: 1px solid rgba(255, 255, 255, 0.25);
  border-radius: 999px;
  padding: 5px 14px 5px 10px;
  font-size: 13px;
  font-weight: 500;
  color: rgba(255, 255, 255, 0.92);
  margin-bottom: 22px;
  backdrop-filter: blur(8px);
}
.qs-hero__pill-dot {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  background: #98C73A;
  flex-shrink: 0;
}

.qs-hero__title {
  font-size: clamp(28px, 3.5vw, 52px);
  font-weight: 800;
  color: #fff;
  line-height: 1.18;
  margin-bottom: 18px;
  letter-spacing: -0.02em;
}
.qs-hero__accent {
  color: #98C73A;
}

.qs-hero__subtitle {
  font-size: 17px;
  color: rgba(255, 255, 255, 0.78);
  line-height: 1.65;
  margin-bottom: 32px;
  max-width: 500px;
}

.qs-hero__btns {
  display: flex;
  align-items: center;
  gap: 14px;
  flex-wrap: wrap;
}
.qs-btn-primary {
  display: inline-flex;
  align-items: center;
  background: #98C73A;
  color: #fff;
  font-size: 15px;
  font-weight: 600;
  padding: 13px 28px;
  border-radius: 8px;
  text-decoration: none;
  transition: background 0.2s, transform 0.15s;
  border: none;
  cursor: pointer;
  letter-spacing: 0.01em;
}
.qs-btn-primary:hover {
  background: #7aad1f;
  color: #fff;
  transform: translateY(-1px);
}
.qs-btn-outline-hero {
  display: inline-flex;
  align-items: center;
  background: transparent;
  color: rgba(255, 255, 255, 0.92);
  font-size: 15px;
  font-weight: 500;
  padding: 13px 24px;
  border-radius: 8px;
  border: 1.5px solid rgba(255, 255, 255, 0.35);
  text-decoration: none;
  transition: all 0.2s;
}
.qs-btn-outline-hero:hover {
  border-color: rgba(255, 255, 255, 0.8);
  background: rgba(255, 255, 255, 0.08);
  color: #fff;
}

.qs-hero__badges {
  display: flex;
  flex-direction: column;
  gap: 16px;
  width: 100%;
}
.qs-floating-badge {
  display: flex;
  align-items: center;
  gap: 14px;
  background: rgba(15, 30, 40, 0.55);
  backdrop-filter: blur(14px);
  -webkit-backdrop-filter: blur(14px);
  border: 1px solid rgba(255, 255, 255, 0.18);
  border-radius: 14px;
  padding: 16px 20px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.25);
}
.qs-floating-badge--second {
  margin-left: 24px;
}
.qs-floating-badge__icon {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  background: rgba(47, 119, 133, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}
.qs-floating-badge__info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}
.qs-floating-badge__label {
  font-size: 10px;
  font-weight: 600;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: rgba(255, 255, 255, 0.55);
}
.qs-floating-badge__value {
  font-size: 20px;
  font-weight: 700;
  color: #fff;
  line-height: 1.2;
}

.qs-hero__stats-bar {
  position: relative;
  z-index: 3;
  background: rgba(20, 50, 60, 0.6);
  backdrop-filter: blur(10px);
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}
.qs-hero__stats-inner {
  display: flex;
  align-items: center;
  gap: 0;
  padding: 18px 0;
}
.qs-hero__stat {
  flex: 1;
  text-align: center;
  padding: 6px 12px;
  border-right: 1px solid rgba(255, 255, 255, 0.12);
}
.qs-hero__stat:last-child { border-right: none; }
.qs-hero__stat strong {
  display: block;
  font-size: 22px;
  font-weight: 800;
  color: #98C73A;
  letter-spacing: -0.01em;
}
.qs-hero__stat span {
  font-size: 12px;
  color: rgba(255, 255, 255, 0.6);
  margin-top: 2px;
  display: block;
}

@media (max-width: 900px) {
  .qs-hero__body {
    flex-direction: column;
    padding-top: 56px;
    padding-bottom: 48px;
    gap: 32px;
  }
  .qs-hero__right { flex: none; width: 100%; }
  .qs-hero__badges { flex-direction: row; flex-wrap: wrap; }
  .qs-floating-badge--second { margin-left: 0; }
  .qs-floating-badge { flex: 1; min-width: 140px; }
  .qs-hero__symbol-bg { width: 200px; }
}
@media (max-width: 560px) {
  .qs-hero__title { font-size: 28px; }
  .qs-hero__subtitle { font-size: 15px; }
  .qs-hero__stats-inner { flex-wrap: wrap; }
  .qs-hero__stat { border-right: none; flex: 0 0 50%; border-bottom: 1px solid rgba(255,255,255,0.1); }
}
</style>
