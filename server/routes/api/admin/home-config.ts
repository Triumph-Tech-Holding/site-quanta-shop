import { defineEventHandler, readBody, sendError, createError } from 'h3';
import fs from 'fs/promises';
import path from 'path';

interface HeroCard {
  ativo: boolean;
  label: string;
  value: string;
  valueColor: string;
  icon: string;
  iconBg: string;
}

interface HeroSection {
  badge: string;
  title: string;
  subtitle: string;
  ctaPrimaryText: string;
  ctaPrimaryLink: string;
}

interface SimpleLabelSection {
  label: string;
}

interface TitleSubtitleSection {
  title: string;
  subtitle: string;
}

interface LabelTitleSubtitleSection {
  label: string;
  title: string;
  subtitle: string;
}

interface CeoSection {
  tag: string;
  pre: string;
  name: string;
  desc: string;
  ctaText: string;
  whatsappLink: string;
}

interface FooterCtaSection {
  title: string;
  subtitle: string;
  primaryText: string;
  primaryLink: string;
  outlineText: string;
  outlineLink: string;
}

interface HomeConfig {
  hero: HeroSection;
  heroCards?: HeroCard[];
  brands: SimpleLabelSection;
  ofertas: TitleSubtitleSection;
  parceirosOnline: LabelTitleSubtitleSection;
  parceirosLocais: LabelTitleSubtitleSection;
  testimonials: TitleSubtitleSection;
  blog: LabelTitleSubtitleSection;
  ceo: CeoSection;
  footerCta: FooterCtaSection;
}

const CONFIG_FILE = path.join(process.cwd(), 'public', 'data', 'home-config.json');

export default defineEventHandler(async (event) => {
  if (!event.context.user?.admin) {
    return sendError(event, createError({ statusCode: 403, message: 'Acesso negado' }));
  }

  const method = event.node.req.method;

  if (method === 'GET') {
    try {
      const data = await fs.readFile(CONFIG_FILE, 'utf-8');
      return JSON.parse(data) as HomeConfig;
    } catch {
      return sendError(event, createError({ statusCode: 500, message: 'Erro ao ler configuração' }));
    }
  }

  if (method === 'POST') {
    try {
      const body = await readBody<unknown>(event);
      if (!body || typeof body !== 'object' || Array.isArray(body)) {
        return sendError(event, createError({ statusCode: 400, message: 'Dados inválidos: esperado objeto' }));
      }
      const config = body as HomeConfig;
      await fs.writeFile(CONFIG_FILE, JSON.stringify(config, null, 2), 'utf-8');
      return { success: true, message: 'Configuração salva com sucesso' };
    } catch {
      return sendError(event, createError({ statusCode: 500, message: 'Erro ao salvar configuração' }));
    }
  }

  return sendError(event, createError({ statusCode: 405, message: 'Método não permitido' }));
});
