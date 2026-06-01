import PDFDocument from 'pdfkit';
import { createWriteStream, mkdirSync } from 'node:fs';

const TEAL = '#225F6B';
const TEAL2 = '#2F7785';
const LIME = '#98C73A';
const INK = '#1d1d1f';
const GRAY = '#6b7280';
const LINE = '#e5e7eb';
const BGALT = '#f5f5f7';
const TODAY = '01 de Junho de 2026';

mkdirSync('docs', { recursive: true });

const doc = new PDFDocument({ size: 'A4', margin: 0, bufferPages: true });
doc.pipe(createWriteStream('docs/QUANTA_SHOP_SNAPSHOT.pdf'));

const PW = doc.page.width;       // ~595
const PH = doc.page.height;      // ~842
const M = 50;                    // content margin
const CW = PW - M * 2;           // content width
let y = M;

function ensure(h) {
  if (y + h > PH - 40) { doc.addPage(); y = M; }
}
function H2(num, text) {
  ensure(50);
  y += 14;
  if (num) {
    doc.roundedRect(M, y, 24, 24, 5).fill(TEAL2);
    doc.fillColor('#fff').font('Helvetica-Bold').fontSize(13).text(num, M, y + 6, { width: 24, align: 'center' });
    doc.fillColor(TEAL).font('Helvetica-Bold').fontSize(16).text(text, M + 34, y + 4, { width: CW - 34 });
  } else {
    doc.fillColor(TEAL).font('Helvetica-Bold').fontSize(16).text(text, M, y);
  }
  y = doc.y + 4;
  doc.moveTo(M, y).lineTo(M + CW, y).lineWidth(2.5).strokeColor(LIME).stroke();
  y += 12;
}
function H3(text) {
  ensure(28);
  y += 6;
  doc.fillColor(TEAL2).font('Helvetica-Bold').fontSize(12).text(text, M, y);
  y = doc.y + 4;
}
function para(text) {
  doc.fillColor(INK).font('Helvetica').fontSize(9.5);
  const h = doc.heightOfString(text, { width: CW, lineGap: 1.5 });
  ensure(h);
  doc.text(text, M, y, { width: CW, lineGap: 1.5 });
  y = doc.y + 6;
}
function bullets(items) {
  doc.font('Helvetica').fontSize(9.5);
  for (const it of items) {
    const txt = '•  ' + it;
    const h = doc.heightOfString(txt, { width: CW - 8, lineGap: 1.2 });
    ensure(h + 2);
    doc.fillColor(INK).text(txt, M + 4, y, { width: CW - 8, lineGap: 1.2 });
    y = doc.y + 3;
  }
  y += 4;
}
function code(text) {
  doc.font('Courier').fontSize(8.5);
  const inner = CW - 24;
  const h = doc.heightOfString(text, { width: inner, lineGap: 2 });
  ensure(h + 20);
  doc.roundedRect(M, y, CW, h + 16, 6).fill('#1d1d1f');
  doc.fillColor('#e8e8e8').text(text, M + 12, y + 8, { width: inner, lineGap: 2 });
  y += h + 16 + 8;
}
function callout(text, color = '#FFB342', bg = '#fff7ed') {
  doc.font('Helvetica').fontSize(9.5);
  const inner = CW - 24;
  const h = doc.heightOfString(text, { width: inner, lineGap: 1.5 });
  ensure(h + 18);
  doc.roundedRect(M, y, CW, h + 14, 6).fill(bg);
  doc.rect(M, y, 4, h + 14).fill(color);
  doc.fillColor(INK).text(text, M + 14, y + 7, { width: inner, lineGap: 1.5 });
  y += h + 14 + 8;
}
function table(headers, rows, widths) {
  const colW = widths.map((w) => Math.round((w / 100) * CW));
  doc.font('Helvetica-Bold').fontSize(8.5);
  // header
  let hh = 0;
  headers.forEach((hd, i) => { hh = Math.max(hh, doc.heightOfString(hd, { width: colW[i] - 12 })); });
  hh += 10;
  ensure(hh + 4);
  doc.rect(M, y, CW, hh).fill(TEAL);
  let x = M;
  headers.forEach((hd, i) => {
    doc.fillColor('#fff').text(hd, x + 6, y + 5, { width: colW[i] - 12 });
    x += colW[i];
  });
  y += hh;
  // rows
  doc.font('Helvetica').fontSize(8.5);
  rows.forEach((row, ri) => {
    let rh = 0;
    row.forEach((c, i) => { rh = Math.max(rh, doc.heightOfString(String(c), { width: colW[i] - 12 })); });
    rh += 9;
    if (y + rh > PH - 40) {
      doc.addPage(); y = M;
      // re-draw header
      doc.font('Helvetica-Bold').fontSize(8.5);
      doc.rect(M, y, CW, hh).fill(TEAL);
      let xx = M;
      headers.forEach((hd, i) => { doc.fillColor('#fff').text(hd, xx + 6, y + 5, { width: colW[i] - 12 }); xx += colW[i]; });
      y += hh;
      doc.font('Helvetica').fontSize(8.5);
    }
    if (ri % 2 === 1) doc.rect(M, y, CW, rh).fill(BGALT);
    let cx = M;
    row.forEach((c, i) => {
      doc.fillColor(INK).text(String(c), cx + 6, y + 4, { width: colW[i] - 12 });
      cx += colW[i];
    });
    doc.moveTo(M, y + rh).lineTo(M + CW, y + rh).lineWidth(0.5).strokeColor(LINE).stroke();
    y += rh;
  });
  y += 10;
}

/* ---------------- COVER ---------------- */
doc.rect(0, 0, PW, PH).fill(TEAL);
doc.rect(0, 0, PW, PH).fillOpacity(1).fill(TEAL);
// gradient-ish bands
doc.fillOpacity(0.25).rect(0, PH * 0.5, PW, PH * 0.5).fill('#1a4a54');
doc.fillOpacity(1);
doc.fillColor(LIME).font('Helvetica-Bold').fontSize(12).text('DOCUMENTO DE CONTEXTO TÉCNICO', M, 140, { characterSpacing: 2 });
doc.fillColor('#fff').font('Helvetica-Bold').fontSize(40).text('Quanta Shop', M, 180);
doc.fillColor(LIME).font('Helvetica-Bold').fontSize(40).text('Onde Estamos', M, 226);
doc.fillColor('#fff').font('Helvetica').fontSize(13).text(
  'Snapshot completo do produto — plataforma brasileira de Consumo Inteligente com cashback em rede (MLM). Base factual extraída diretamente do código para construção de skill, onboarding técnico e governança de produto.',
  M, 290, { width: CW - 60, lineGap: 3 });
doc.moveTo(M, 700).lineTo(M + CW, 700).lineWidth(1).strokeColor('#7fb6c0').stroke();
doc.fillColor('#cfe6ea').font('Helvetica').fontSize(11).text(
  'Versão: v1.3.0   ·   Data: ' + TODAY + '   ·   Stack: Nuxt 3 + .NET 8 + SQL Server Azure', M, 716);
doc.text('Produção: quantashop.com.br   ·   API: api.quantashop.com.br', M, 734);
doc.addPage();
y = M;

/* ---------------- SUMÁRIO EXECUTIVO ---------------- */
H2(null, 'Sumário Executivo — Em que momento estamos');
para('O produto está em produção com a fundação e a camada premium de UI concluídas. A frente atual é a padronização visual de todas as páginas no design system qs-* (8 fases) e o motor de rede/compensação (MVP 3, em andamento).');
table(['MVP', 'Escopo', 'Status'],
  [
    ['MVP 1 — Fundação', 'Cadastro/JWT, catálogo de parceiros, cashback base', '100% — Produção'],
    ['MVP 2 — Premium UI', 'Home premium, design system qs-*, blog, admin CMS', '100% — Concluído'],
    ['MVP 3 — Rede & Compensação', '12 níveis residuais, graduações, compressão dinâmica', '~30% — Em andamento'],
    ['MVP 4 — Inteligência', 'Recomendações, personalização', '0% — Backlog'],
  ], [22, 50, 28]);
table(['Padronização qs-* (8 fases)', 'Status'],
  [
    ['Fase 1 LAB · Fase 2 Admin operacional · Fase 3 Admin gestão', 'Concluídas'],
    ['Fase 4 Painel empreendedor · Fase 5 Auth/onboarding · Fase 6 Marketing', 'Concluídas'],
    ['Fase 7 Páginas públicas (tp-*) · Fase 8 Limpeza', 'Pendentes'],
  ], [72, 28]);
callout('Diretriz máxima vigente: ZERO alterações no backend. As tarefas atuais são refatoração visual frontend — lógica, dados e autenticação permanecem intactos.');

/* ---------------- 1 STACK ---------------- */
H2('1', 'Stack técnica completa');
H3('Frontend');
table(['Categoria', 'Tecnologia', 'Versão'],
  [
    ['Framework', 'Nuxt 3 (SSR off / SPA)', '^3.8.1'],
    ['UI', 'Vue 3 — Composition API, <script setup>', '—'],
    ['Estado', 'Pinia (@pinia/nuxt)', '^2.1.6'],
    ['CSS framework', 'Bootstrap', '^5.3.0'],
    ['Pré-processador', 'Sass/SCSS', '^1.64.1'],
    ['Utility CSS', 'Tailwind — escopo restrito a .v2-page', '^6.14.0'],
    ['Carrossel/slider', 'Swiper / vue3-carousel / @vueform/slider', '^10 / ^0.3.1'],
    ['Gráficos', 'apexcharts + vue3-apexcharts', '^5.10 / ^1.11'],
    ['Validação', 'vee-validate + yup', '^4.11 / ^1.2'],
    ['HTTP', 'axios (via composables/useApi)', '^1.7.2'],
    ['JWT', 'jsonwebtoken (server middleware)', '^9.0.2'],
    ['PDF client', 'html2pdf.js / pdfkit', '^0.14 / ^0.18'],
    ['PWA', '@vite-pwa/nuxt (selfDestroying)', '^0.10.5'],
    ['Analytics', 'nuxt-gtag (GA4 G-3YM68FHXJW)', '^3.0.1'],
    ['Server runtime', 'Nitro preset node-server', '—'],
    ['Datas/Máscaras/Toasts', 'dayjs / vue-the-mask / vue3-toastify', '—'],
    ['Fontes', 'Inter / Jost (Google Fonts)', '—'],
  ], [26, 52, 22]);
H3('Backend — api/ (.NET 8 / ASP.NET Core, solução Bigcash.sln)');
bullets([
  'MMN.Api — controllers REST (/Controllers/v1/), Program/Startup, SignalR (Hubs), Swagger, HealthChecks',
  'MMN.Dominio — entidades (Model/), enums, exceções, ViewModels, WebHooks',
  'MMN.Negocio / MMN.INegocio — regras de negócio (*Negocio.cs) + Services/',
  'MMN.Repositorio / MMN.IRepositorio — EF Core: Contexto, Mapping, Migrations, Seed, repositórios',
  'MMN.Integracoes — afiliados externos (Awin, Afilio, Zanox), Google OAuth2',
  'MMN.Util — helpers (LGPD mask, templates de e-mail) · MMN.Tests — xUnit',
]);
para('Banco: SQL Server (Azure)  ·  Auth: JWT + Refresh Token  ·  Realtime: SignalR');
H3('Integrações externas');
para('Redes de afiliados Awin, Afilio, Zanox; Google OAuth2 + Apple Sign In; e-mail transacional (templates HTML).');

/* ---------------- 2 ESTRUTURA ---------------- */
H2('2', 'Estrutura de diretórios');
H3('Frontend (raiz)');
table(['Pasta', 'Conteúdo'],
  [
    ['pages/', 'Rotas file-based: públicas na raiz, agencia/, agencia/painel/admin/, lab/'],
    ['components/', 'home/ (V1), qs/ (design system), agencia/, checkout/, primeira-compra/, home-v2/ (abandonado)'],
    ['layouts/', 'layout-home, agencia-painel, agencia-login, primeira-compra, layout-one…four'],
    ['composables/', 'useApi, useAgenciaStore (re-export), useHomeConfig, useMask, useSticky'],
    ['pinia/ · stores/', 'Stores Pinia (auto-importadas via imports.dirs)'],
    ['middleware/', 'agencia-auth.ts, agencia-admin.ts'],
    ['plugins/ · directives/', 'directives, filters, mask, apexcharts, toastify'],
    ['assets/scss/', 'main.scss, quanta-premium.scss (qs-*), agencia.scss (ag-*), primeira-compra.scss'],
    ['server/', 'Nitro: api-proxy/[...path].ts, api/admin/*, middleware/verify-jwt.ts'],
    ['types/ · utils/ · data/', 'Tipos TS, helpers client (lgpd-mask), dados estáticos'],
    ['public/', 'Assets; public/data/*.json (config/mock); public/docs/*.md (lidos pelo LAB)'],
  ], [22, 78]);
H3('Backend (api/) — camadas');
code('MMN.Api (apresentacao)  ->  MMN.Negocio (regras)  ->  MMN.Repositorio (EF Core)  ->  MMN.Dominio (entidades)\nMMN.Integracoes (3rd-party)     MMN.Util (helpers)     MMN.Tests (xUnit)');

/* ---------------- 3 FLUXO ---------------- */
H2('3', 'Fluxo obrigatório de entrega de feature');
callout('Hoje: backend congelado (ZERO alterações). O fluxo completo abaixo vale quando o backend for liberado; a maioria das tasks atuais é frontend-only.');
bullets([
  '1. Spec primeiro — ler FEATURES.md, STORIES.md, DATA_DICTIONARY.md, DESIGN_SYSTEM.md, TESTING.md',
  '2. Modelagem (backend) — entidade em MMN.Dominio/Model + mapping + DbSet + seed',
  '3. Migration — dotnet ef migrations add <Nome> --project MMN.Repositorio --startup-project MMN.Api (nunca contra prod do Replit)',
  '4. Regra de negócio — *Negocio.cs / Services puro e testável (ex. CashbackDistribuicaoService)',
  '5. Controller — endpoint em MMN.Api/Controllers/v1/ herdando LoggedControllerBase',
  '6. Teste xUnit — gate obrigatório para cashback/compensação/compressão',
  '7. Frontend — consumir via composables/useApi (proxy Nitro) com fallback dev',
  '8. UI no design system — classes qs-* (admin: ag-*/qs-*)',
  '9. Validação visual — mobile 375px sem overflow + screenshot antes/depois',
  '10. Atualizar docs — CHANGELOG.md, features.json/FEATURES.md, replit.md',
]);

/* ---------------- 4 CHECKLIST ---------------- */
H2('4', 'Checklist de fechamento de task');
H3('Comandos');
code('npm run build:nuxt                                   # build Nuxt deve passar\ncd api && dotnet build                               # 0 erros (warnings reais OK)\ncd api && dotnet test MMN.Tests/MMN.Tests.csproj     # se tocou motor/cashback');
H3('Arquivos a atualizar');
bullets([
  'CHANGELOG.md (semver, sempre) · public/docs/features.json + FEATURES.md (nova feature)',
  'replit.md (histórico operacional) · CLAUDE.md (mudança arquitetural)',
  'Migration documentada com comando de aplicação em prod · .local/.commit_message',
]);
H3('Verificações');
bullets([
  'Workflows Start application + Start API rodando, sem erro no console do browser',
  'Mobile 375px sem overflow horizontal · Lógica/dados/auth inalterados (tasks visuais)',
  'Nenhum dado sensível em localStorage',
]);

/* ---------------- 5 RESTRIÇÕES ---------------- */
H2('5', 'Restrições da plataforma (Replit)');
bullets([
  'Host/porta: dev 0.0.0.0:5000; API 8000. Workflows: Start application (npm run dev) e Start API (bash api/start-api.sh)',
  'Vite allowedHosts: true — obrigatório (preview é iframe proxiado por outra origem)',
  'Watch ignora api/** — mudanças .NET não recarregam o Nuxt; reiniciar workflow da API manualmente',
  'Banco de produção NUNCA acessado do dev — escritas de teste do admin vão para localStorage (blog híbrido)',
  'Proxy dev tem fallback para produção (api.quantashop.com.br) — gatear escritas com import.meta.dev',
  'SSR desabilitado (SPA) · usar $REPLIT_DEV_DOMAIN e URLs relativas, nunca localhost hardcoded',
  'Secrets via gerenciador do Replit (NUXT_JWT_SECRET real só em prod), nunca commitados',
]);

/* ---------------- 6 PADRÕES ---------------- */
H2('6', 'Padrões arquiteturais');
bullets([
  'Layered / N-tier (.NET): Apresentação -> Negócio -> Repositório -> Domínio',
  'Repository Pattern (interfaces MMN.IRepositorio / MMN.INegocio)',
  'Service / Domain Service para o motor MLM (CashbackDistribuicaoService — puro, configurável via banco)',
  'Controller base compartilhado (LoggedControllerBase) para auth/contexto do usuário logado',
  'Config-driven — percentuais, splits, multiplicadores na tabela Configuracao (chaves Rede.*), não hardcoded',
  'Frontend: Composition API + composables como serviço; Pinia para estado/auth; proxy Nitro como BFF; componentizar quando repetir 3x',
]);
table(['Tipo de código', 'Onde fica'],
  [
    ['Entidade', 'MMN.Dominio/Model'],
    ['Regra de negócio', 'MMN.Negocio/Negocio/*Negocio.cs'],
    ['Cálculo puro / estratégia (cashback)', 'MMN.Negocio/Services (sem DB, testável)'],
    ['Acesso a dados', 'MMN.Repositorio/Repositorio + interfaces em MMN.IRepositorio'],
    ['Mapeamento EF', 'MMN.Repositorio/Mapping'],
  ], [42, 58]);

/* ---------------- 7 DESIGN SYSTEM ---------------- */
H2('7', 'Design system (quanta-premium.scss)');
callout('Nunca hardcodar cores, raios, sombras ou tipografia — sempre tokens CSS.', '#dc2626', '#fef2f2');
code('Cores:    --qs-primary #2F7785 . --qs-primary-dark #225F6B . --qs-primary-mid #3A9AAD\n          --qs-lime #98C73A . --qs-lime-dark #7aad1f . --qs-gold #FFB342\n          --qs-near-black #1a2332 . --qs-ink #1d1d1f . --qs-bg-brand #F4F4F5\nAliases:  --qs-teal / --qs-teal-dark / --qs-bg     Cinzas: --qs-gray-50..700\nEstados:  --qs-success #16a34a . --qs-warn #d97706 . --qs-danger #dc2626\nGradients:--qs-gradient-primary / -lime / -btn / -hero\nSombras:  --qs-shadow-xs..lg . --qs-shadow-green\nRaios:    --qs-radius-sm 6 / md 12 / lg 20 / pill 999\nMotion:   --qs-transition .25s . --qs-transition-fast .15s . --qs-ease . --qs-duration 200ms\nFonte:    --qs-font: Inter, Jost, sans-serif');
para('Classes compartilhadas: qs-page, qs-page-header, qs-eyebrow, qs-section, qs-container, qs-grid, qs-card / qs-card-section, qs-benefit-card, qs-kpi-card, qs-table / qs-table-wrap, qs-badge*, qs-modal-*, qs-btn-primary/outline/gold/secondary/danger, qs-input / qs-select, qs-alert-danger/success/warn, qs-loading / qs-spinner / qs-skeleton, qs-empty-state. SFCs: QsHero, QsKpiCard, QsProgressBar, QsFilterChip, QsPageHeader.');
para('Admin legado: classes ag-* (em migração para qs-*). Bootstrap utilities sendo removidas em favor de scoped qs-*.');

/* ---------------- 8 PITFALLS ---------------- */
H2('8', 'Pitfalls — erros reais que geraram retrabalho');
table(['Pitfall', 'Por que acontece', 'Como evitar'],
  [
    ['Auto-import useAgenciaStore/useApi falha', 'Unimport não resolve aliases ~/ em re-exports', 'Import explícito nas páginas admin'],
    ["@import 'variables' quebra build", 'Sass exige @forward antes de qualquer regra', '@import depois dos @forward em main.scss'],
    ['pages/blog.vue conflita com subrotas', 'Nuxt 3 conflita arquivo único com blog/[id].vue', 'Usar blog/index.vue + blog/[id].vue'],
    ['Escrita admin contamina prod em dev', 'Proxy dev faz fallback para api.quantashop.com.br', 'Gatear writes com import.meta.dev -> localStorage'],
    ['VitePwaManifest "Failed to resolve"', 'Componente só existe com módulo PWA ativo', 'Remover <VitePwaManifest /> do app.vue'],
    ['manifest-route-rule floods + erros import', 'Auto-import instável do @nuxt/kit embutido', "imports.dirs ['pinia','composables','composables/**']"],
    ['dotnet build FAILED', 'Projetos fantasma na .sln', '.sln limpa + Directory.Build.props'],
    ['background-image com URL externa quebra hero', 'URLs externas não renderizam como bg', 'Usar <img> + overlay div'],
    ['Tailwind não funciona fora da home V2', "important: '.v2-page' + preflight:false", 'Bootstrap + qs-*/ag-* nas demais páginas'],
    ['localhost hardcoded falha no preview', 'Preview é iframe proxiado', 'URLs relativas; $REPLIT_DEV_DOMAIN no shell'],
    ['Mudança .NET não reflete', 'watch ignora api/**', 'Reiniciar workflow Start API'],
    ['Migration gera coluna faltante em prod', 'Modelo tem campos pendentes no DB', 'Documentar dotnet ef database update'],
    ['Code review loop infinito', 'Reviewer adiciona sugestões opcionais a cada rodada', 'Entregar núcleo + skip_validation_reason justificado'],
  ], [30, 38, 32]);

/* ---------------- 9 HOLDING ---------------- */
H2('9', 'Dependências de outros projetos da holding (Triumph Tech)');
callout('Conclusão factual: não há dependência de código entre Quanta Shop e Quanta Flow / TTHM / outro projeto da holding.', LIME, '#f3faea');
bullets([
  'As únicas ocorrências de "Triumph" referem-se ao CEO Mauro Triumph (seção CEO da home), não a um sistema.',
  'Nenhuma referência a "Quanta Flow" ou "TTHM" foi encontrada em código, configs ou docs.',
  'Dependências externas reais: redes de afiliados (Awin, Afilio, Zanox) e Google/Apple (login social) via MMN.Integracoes — terceiros, não projetos da holding.',
  'Tratar Quanta Shop como self-contained. Integração futura entre apps da holding precisaria ser especificada — não existe hoje.',
]);

/* ---------------- 10 ÍNDICE DE ROTAS ---------------- */
H2('10', 'Índice completo de rotas e endpoints');
H3('Site — páginas públicas (usuário final)');
para('index (home), para-voce, para-sua-empresa, seja-um-agente, quanta-amizade, shop, shop-categories, shop-full-width, cart, checkout, wishlist, compare, coupons, order, profile, search, busca-inteligente, about, contato, contact, login, forgot, blog/index, blog/[slug], 404.');
H3('LAB (governança técnica) — /lab/*');
para('index (cockpit), docs, features, progresso, bi-financeiro, flow-standard.');
H3('Agência — páginas públicas/landing');
para('index, login, cadastro, cadastro-google, recuperar-senha, como-funciona, quem-somos, faq, privacidade, lojas-fisicas, links, credenciar, no-permission, logout.');
H3('Agência — painel do empreendedor (/agencia/painel/*)');
para('index, dashboard-rede-adf, desempenho, performance, financeiro, contas-bancarias, assinatura, planos, minha-rede, meus-diretos, graduacoes, cupons, meus-cupons, gerar-cupons, inserir-cupom, produtos, social-commerce, comerciante, meus-credenciamentos, minhas-compras, meus-dados, material-apoio, tutoriais-usuario, faq, suporte, solicitar-suporte.');
H3('Agência — admin (/agencia/painel/admin/*)');
para('index, usuarios, alterar-dados-usuario, pagamentos, assinaturas, compras, lancamentos, credenciamento, lojas-credenciados, acessos, gerenciar-grupos, aniversariantes, rede, suporte, categorias, ecossistemas, carrosseis, comunicados, marcas-home, redes-sociais, home-cms, home-conteudo, blog, material-apoio-admin, configuracoes-rede, relatorio-cashback, relatorio-de-anunciantes, relatorio-de-faturas, bi-financeiro, features, progresso, flow-standard, docs.');
H3('API .NET — controllers (/api/v1/*)');
para('Admin (Config, Financeiro, Relatorios, Suporte, Usuarios), Usuario (Auth, Financeiro, Perfil, Rede), UsuarioLogin, UsuarioBanco, UsuarioEndereco, Banco, Carrossel, Categoria, Compra, Comunicado, Config, Contato, Credenciamento, Cupom, Dashboard, Ecossistema, Extrato, Faq, Fatura, Geral, Grupo, Image, Lancamento, MaterialApoio, Pagamento, Parceiros, Pedidos, Produto, QuantaPoints, Rede, Relatorios, Saque, Search, Tutorial, Anunciante, Values, Venda, WebHookSubscription.');
H3('Proxy/BFF Nitro (server/)');
para('routes/api-proxy/[...path].ts (proxy reverso), routes/api/admin/{brands, import-carousels, upload-banner, home-config, banner-campaigns}, routes/home-cms.ts, middleware/verify-jwt.ts, api/auth/logout.ts.');

/* ---------------- FOOTER on all pages ---------------- */
const range = doc.bufferedPageRange();
for (let i = 1; i < range.count; i++) {
  doc.switchToPage(i);
  doc.fillColor(GRAY).font('Helvetica').fontSize(7.5).text(
    'Quanta Shop · Snapshot técnico · ' + TODAY + ' · v1.3.0',
    M, PH - 28, { width: CW - 40, align: 'left' });
  doc.text(String(i + 1), M, PH - 28, { width: CW, align: 'right' });
}

doc.end();
console.log('PDF gerado: docs/QUANTA_SHOP_SNAPSHOT.pdf');
