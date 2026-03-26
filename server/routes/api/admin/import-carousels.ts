import { defineEventHandler, sendError, createError } from 'h3';
import fs from 'fs/promises';
import path from 'path';

interface OldCarousel {
  idCarrossel: number;
  imagem: string;
  link: string;
  posicao: string;
  ativo: boolean;
  ordemExibicao: number;
  titulo?: string;
}

interface HeroBannerSlide {
  id: number;
  titulo: string;
  url: string;
  urlDestino: string;
  ativo: boolean;
  headline: string;
  subtitulo: string;
  badge: string;
  ctaTexto: string;
  ctaLink: string;
  ctaCor: string;
  textoCor: 'light' | 'dark';
  overlayIntensidade: number;
}

const BANNERS_FILE = path.join(process.cwd(), 'public', 'data', 'hero-banners.json');
const LOCAL_API = 'http://localhost:8000/api';

async function readBanners(): Promise<HeroBannerSlide[]> {
  try {
    const data = await fs.readFile(BANNERS_FILE, 'utf-8');
    return JSON.parse(data) as HeroBannerSlide[];
  } catch {
    return [];
  }
}

async function writeBanners(banners: HeroBannerSlide[]): Promise<void> {
  await fs.writeFile(BANNERS_FILE, JSON.stringify(banners, null, 2), 'utf-8');
}

export default defineEventHandler(async (event) => {
  if (!event.context.user?.admin) {
    return sendError(event, createError({ statusCode: 403, message: 'Acesso negado' }));
  }

  const existing = await readBanners();
  if (existing.length > 0) {
    return { imported: 0, slides: existing, message: 'Banners já existem — nenhuma importação necessária.' };
  }

  let oldCarousels: OldCarousel[] = [];
  try {
    const response = await $fetch<{ data: OldCarousel[] } | OldCarousel[]>(`${LOCAL_API}/v2/carousels`);
    const raw = Array.isArray(response) ? response : response?.data;
    oldCarousels = Array.isArray(raw) ? raw : [];
  } catch (e) {
    return sendError(event, createError({ statusCode: 502, message: 'Não foi possível conectar ao sistema antigo para importar os carrosseis.' }));
  }

  const heroOld = oldCarousels
    .filter(c => String(c.posicao) === '1' && c.ativo === true)
    .sort((a, b) => (a.ordemExibicao ?? 0) - (b.ordemExibicao ?? 0));

  if (heroOld.length === 0) {
    return { imported: 0, slides: [], message: 'Nenhum banner do hero encontrado no sistema antigo.' };
  }

  const now = Date.now();
  const slides: HeroBannerSlide[] = heroOld.map((c, idx): HeroBannerSlide => ({
    id: now + idx,
    titulo: c.titulo?.trim() || `Banner Hero ${idx + 1}`,
    url: c.imagem || '',
    urlDestino: c.link || '',
    ativo: true,
    headline: '',
    subtitulo: '',
    badge: '',
    ctaTexto: '',
    ctaLink: c.link || '',
    ctaCor: '#98C73A',
    textoCor: 'light',
    overlayIntensidade: 70,
  }));

  await writeBanners(slides);

  return { imported: slides.length, slides };
});
