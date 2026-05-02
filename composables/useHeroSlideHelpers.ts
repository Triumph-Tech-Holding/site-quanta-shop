import type { HeroBannerSlide } from '~/types/agencia';
import type { HeroCard } from '@/composables/useHomeConfig';

export const DEFAULT_HERO_CARDS: HeroCard[] = [
  { ativo: true, label: 'PIX INSTANTÂNEO', value: 'Saque em segundos ✓', valueColor: 'green', icon: 'card', iconBg: 'teal' },
  { ativo: true, label: 'CASHBACK RECEBIDO', value: 'R$ 50,00', valueColor: 'green', icon: 'chart', iconBg: 'green' },
  { ativo: true, label: 'MARCAS PARCEIRAS', value: '+500 lojas', valueColor: 'teal', icon: 'bag', iconBg: 'teal' },
];

export const ICON_PATHS: Record<string, string> = {
  card:    '<rect x="2" y="5" width="20" height="14" rx="2"/><path d="M2 10h20"/>',
  chart:   '<polyline points="22 7 13.5 15.5 8.5 10.5 2 17"/><polyline points="16 7 22 7 22 13"/>',
  bag:     '<path d="M6 2L3 6v14a2 2 0 002 2h14a2 2 0 002-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 01-8 0"/>',
  star:    '<polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/>',
  percent: '<line x1="19" y1="5" x2="5" y2="19"/><circle cx="6.5" cy="6.5" r="2.5"/><circle cx="17.5" cy="17.5" r="2.5"/>',
  gift:    '<polyline points="20 12 20 22 4 22 4 12"/><rect x="2" y="7" width="20" height="5"/><path d="M12 22V7"/><path d="M12 7H7.5a2.5 2.5 0 010-5C11 2 12 7 12 7z"/><path d="M12 7h4.5a2.5 2.5 0 000-5C13 2 12 7 12 7z"/>',
  users:   '<path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 00-3-3.87"/><path d="M16 3.13a4 4 0 010 7.75"/>',
  zap:     '<polygon points="13 2 3 14 12 14 11 22 21 10 12 10 13 2"/>',
};

export function getCardIconSvg(icon: string, iconBg: string): string {
  const paths = ICON_PATHS[icon] ?? ICON_PATHS.card;
  const stroke = iconBg === 'green' ? '#fff' : '#2F7785';
  return `<svg width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="${stroke}" stroke-width="2">${paths}</svg>`;
}

export function hexToRgba(hex: string, alpha: number): string {
  const h = hex.replace('#', '');
  const r = parseInt(h.substring(0, 2), 16);
  const g = parseInt(h.substring(2, 4), 16);
  const b = parseInt(h.substring(4, 6), 16);
  return `rgba(${r},${g},${b},${alpha})`;
}

export function getCtaStyle(item: HeroBannerSlide): Record<string, string> {
  const style: Record<string, string> = {};
  if (item.ctaCor) { style.background = item.ctaCor; style.borderColor = item.ctaCor; }
  if (item.ctaTextoCor) style.color = item.ctaTextoCor;
  if (item.ctaTamanho === 'pequeno') { style.padding = '10px 20px'; style.fontSize = '13px'; }
  else if (item.ctaTamanho === 'grande') { style.padding = '18px 36px'; style.fontSize = '18px'; }
  return style;
}

export function getSlideTitle(item: HeroBannerSlide, fallback: string): string {
  const raw = item.headline || fallback;
  return raw
    .replace(/\n/g, '<br>')
    .replace('<highlight>', '<span style="color:#98C73A">')
    .replace('</highlight>', '</span>');
}

export function getOverlayGradient(item: HeroBannerSlide): string {
  const d = item.overlayDirecao || 'esquerda';
  const cor = item.overlayCor && item.overlayCor.startsWith('#') ? item.overlayCor : null;
  const hi  = cor ? hexToRgba(cor, 0.88) : 'rgba(15,35,45,0.88)';
  const mid = cor ? hexToRgba(cor, 0.65) : 'rgba(15,35,45,0.65)';
  const lo  = cor ? hexToRgba(cor, 0.30) : 'rgba(15,35,45,0.30)';
  const loAlt = cor ? hexToRgba(cor, 0.20) : 'rgba(15,35,45,0.20)';
  if (d === 'direita')   return `linear-gradient(to left,  ${hi} 0%, ${mid} 45%, ${lo} 100%)`;
  if (d === 'centro')    return `linear-gradient(to right, ${loAlt} 0%, ${hi} 50%, ${loAlt} 100%)`;
  if (d === 'uniforme')  return hi;
  return `linear-gradient(to right, ${hi} 0%, ${mid} 45%, ${lo} 100%)`;
}

export function getTitleFontSize(item: HeroBannerSlide): string {
  if (item.tituloFontSize === 'pequeno') return 'clamp(22px, 3.5vw, 36px)';
  if (item.tituloFontSize === 'grande')  return 'clamp(42px, 6.5vw, 68px)';
  return 'clamp(32px, 5vw, 54px)';
}

export function getTitleLineHeight(item: HeroBannerSlide): string {
  if (item.headlineEspacamento === 'normal') return '1.02';
  if (item.headlineEspacamento === 'amplo')  return '1.18';
  return '0.95';
}

export function getSlideSubtitle(item: HeroBannerSlide, fallback: string): string {
  const raw = item.subtitulo || fallback;
  return raw.replace(/\n/g, '<br>');
}

export function getSubtitleStyle(item: HeroBannerSlide): Record<string, string> {
  const style: Record<string, string> = {};
  if (item.subtituloCor) style.color = item.subtituloCor;
  if (item.subtituloFontSize === 'pequeno') style.fontSize = 'clamp(12px, 1.5vw, 14px)';
  else if (item.subtituloFontSize === 'grande') style.fontSize = 'clamp(18px, 2.5vw, 24px)';
  return style;
}
