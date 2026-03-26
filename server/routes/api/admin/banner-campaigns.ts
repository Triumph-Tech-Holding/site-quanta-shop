import { defineEventHandler, readBody, sendError, createError } from 'h3';
import fs from 'fs/promises';
import path from 'path';

export interface HeroBannerSlide {
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
  objectPosition?: string;
  tituloFontSize?: 'pequeno' | 'medio' | 'grande';
  overlayDirecao?: 'esquerda' | 'direita' | 'centro' | 'uniforme';
  ctaAlinhamento?: 'esquerda' | 'centro' | 'direita';
}

const BANNERS_FILE = path.join(process.cwd(), 'public', 'data', 'hero-banners.json');

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

  const method = event.node.req.method;

  if (method === 'GET') {
    return await readBanners();
  }

  if (method === 'POST') {
    const body = await readBody<unknown>(event);
    if (!body || typeof body !== 'object' || Array.isArray(body)) {
      return sendError(event, createError({ statusCode: 400, message: 'Dados inválidos' }));
    }
    const slide = body as HeroBannerSlide;
    if (!slide.id) {
      return sendError(event, createError({ statusCode: 400, message: 'id é obrigatório' }));
    }
    const banners = await readBanners();
    const idx = banners.findIndex(b => b.id === slide.id);
    if (idx !== -1) {
      banners[idx] = slide;
    } else {
      banners.push(slide);
    }
    await writeBanners(banners);
    return { success: true };
  }

  if (method === 'PATCH') {
    const body = await readBody<unknown>(event);
    if (!body || typeof body !== 'object' || Array.isArray(body)) {
      return sendError(event, createError({ statusCode: 400, message: 'Dados inválidos' }));
    }
    const { orderedIds } = body as { orderedIds: number[] };
    if (!Array.isArray(orderedIds)) {
      return sendError(event, createError({ statusCode: 400, message: 'orderedIds deve ser um array' }));
    }
    const banners = await readBanners();
    const reordered = orderedIds
      .map(id => banners.find(b => b.id === id))
      .filter((b): b is HeroBannerSlide => b !== undefined);
    const unlisted = banners.filter(b => !orderedIds.includes(b.id));
    await writeBanners([...reordered, ...unlisted]);
    return { success: true };
  }

  if (method === 'DELETE') {
    const body = await readBody<unknown>(event);
    if (!body || typeof body !== 'object' || Array.isArray(body)) {
      return sendError(event, createError({ statusCode: 400, message: 'Dados inválidos' }));
    }
    const { id } = body as { id: number };
    if (!id) return sendError(event, createError({ statusCode: 400, message: 'id é obrigatório' }));
    const banners = await readBanners();
    await writeBanners(banners.filter(b => b.id !== id));
    return { success: true };
  }

  return sendError(event, createError({ statusCode: 405, message: 'Método não permitido' }));
});
