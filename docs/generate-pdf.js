const PDFDocument = require('pdfkit');
const fs = require('fs');

const doc = new PDFDocument({
  size: 'A4',
  margins: { top: 50, bottom: 50, left: 56, right: 56 },
  info: {
    Title: 'Quanta Shop — Especificação Técnica para Migração Lovable',
    Author: 'Quanta Shop',
    Subject: 'Documentação Técnica v1.0.0',
  },
});

const out = fs.createWriteStream('/home/runner/workspace/docs/quanta-shop-technical-spec.pdf');
doc.pipe(out);

const PRIMARY = '#2F7785';
const ACCENT = '#98C73A';
const DARK = '#225F6B';
const TEXT = '#374151';
const MUTED = '#6B7280';
const BG_LIGHT = '#F9FAFB';
const BG_TEAL = '#E8F4F6';
const BORDER = '#E5E7EB';
const WHITE = '#FFFFFF';
const WARN_BG = '#FEFCE8';
const WARN_BORDER = '#FDE68A';
const PAGE_W = 595.28 - 112;
const LINE_H = 16;

function heading1(text) {
  doc.moveDown(0.5)
    .fontSize(20).fillColor(PRIMARY).font('Helvetica-Bold').text(text)
    .moveDown(0.3);
}

function heading2(text) {
  doc.moveDown(0.4)
    .fontSize(13).fillColor(PRIMARY).font('Helvetica-Bold').text(text)
    .moveDown(0.2);
}

function heading3(text) {
  doc.moveDown(0.3)
    .fontSize(11).fillColor(DARK).font('Helvetica-Bold').text(text)
    .moveDown(0.2);
}

function body(text, opts = {}) {
  doc.fontSize(9.5).fillColor(TEXT).font('Helvetica').text(text, { lineGap: 2, ...opts });
}

function bullet(text, indent = 0) {
  const x = doc.page.margins.left + indent;
  doc.fontSize(9.5).fillColor(TEXT).font('Helvetica').text('• ' + text, x, doc.y, { indent: 8, lineGap: 2, width: PAGE_W - indent });
}

function infoBox(title, content, bgColor = BG_TEAL, borderColor = PRIMARY) {
  const startY = doc.y;
  const boxX = doc.page.margins.left;
  const boxW = PAGE_W;
  doc.moveDown(0.3);
  const textStartY = doc.y + 10;
  doc.fontSize(9).font('Helvetica-Bold').fillColor(PRIMARY).text(title, boxX + 12, textStartY, { width: boxW - 24 });
  doc.moveDown(0.1);
  doc.fontSize(9).font('Helvetica').fillColor(TEXT).text(content, boxX + 12, doc.y, { width: boxW - 24, lineGap: 2 });
  const endY = doc.y + 10;
  doc.rect(boxX, textStartY - 10, boxW, endY - textStartY + 10).fillAndStroke(bgColor, borderColor);
  doc.fontSize(9).font('Helvetica-Bold').fillColor(PRIMARY).text(title, boxX + 12, textStartY, { width: boxW - 24 });
  doc.moveDown(0.1);
  doc.fontSize(9).font('Helvetica').fillColor(TEXT).text(content, boxX + 12, doc.y, { width: boxW - 24, lineGap: 2 });
  doc.moveDown(0.5);
}

function divider() {
  doc.moveDown(0.3);
  doc.moveTo(doc.page.margins.left, doc.y).lineTo(doc.page.margins.left + PAGE_W, doc.y).strokeColor(BORDER).stroke();
  doc.moveDown(0.3);
}

function tableHeader(cols) {
  const colW = PAGE_W / cols.length;
  const y = doc.y;
  doc.rect(doc.page.margins.left, y, PAGE_W, 18).fill(PRIMARY);
  cols.forEach((col, i) => {
    doc.fontSize(8).font('Helvetica-Bold').fillColor(WHITE)
      .text(col, doc.page.margins.left + i * colW + 5, y + 5, { width: colW - 10 });
  });
  doc.y = y + 18;
}

function tableRow(cells, isEven) {
  const colW = PAGE_W / cells.length;
  const y = doc.y;
  const PADDING = 5;
  const LINE_GAP = 1;
  // Calculate the tallest cell to set row height dynamically
  let maxCellH = 0;
  cells.forEach(cell => {
    const h = doc.heightOfString(cell, { width: colW - PADDING * 2, lineGap: LINE_GAP, fontSize: 8.5 });
    if (h > maxCellH) maxCellH = h;
  });
  const rowH = maxCellH + PADDING * 2;
  // check if we need a new page
  if (y + rowH > doc.page.height - doc.page.margins.bottom) {
    doc.addPage();
  }
  const rowY = doc.y;
  if (isEven) {
    doc.rect(doc.page.margins.left, rowY, PAGE_W, rowH).fill(BG_LIGHT);
  }
  cells.forEach((cell, i) => {
    doc.fontSize(8.5).font('Helvetica').fillColor(TEXT)
      .text(cell, doc.page.margins.left + i * colW + PADDING, rowY + PADDING, { width: colW - PADDING * 2, lineGap: LINE_GAP });
  });
  doc.y = rowY + rowH;
  doc.moveTo(doc.page.margins.left, doc.y).lineTo(doc.page.margins.left + PAGE_W, doc.y).strokeColor(BORDER).stroke();
}

function sectionHeader(num, title) {
  doc.addPage();
  const y = doc.y;
  doc.circle(doc.page.margins.left + 14, y + 14, 14).fill(PRIMARY);
  doc.fontSize(14).font('Helvetica-Bold').fillColor(WHITE).text(String(num), doc.page.margins.left + 14 - 4, y + 7);
  doc.fontSize(16).font('Helvetica-Bold').fillColor('#111827').text(title, doc.page.margins.left + 36, y + 7);
  doc.moveDown(1.5);
  divider();
}

// ─── CAPA ───────────────────────────────────────────────────────────────────
doc.rect(0, 0, 595.28, 841.89).fill('#0f232d');
doc.rect(0, 0, 595.28, 841.89).fillOpacity(0.6).fill('#225F6B');

doc.fillOpacity(1);

doc.roundedRect(56, 60, 120, 22, 11).fillAndStroke('rgba(152,199,58,0.15)', ACCENT);
doc.fontSize(9).font('Helvetica-Bold').fillColor(ACCENT).text('DOCUMENTO TÉCNICO CONFIDENCIAL', 68, 67);

doc.fontSize(38).font('Helvetica-Bold').fillColor(WHITE).text('Quanta Shop', 56, 110);
doc.fontSize(28).font('Helvetica-Bold').fillColor(ACCENT).text('Especificação Técnica', 56, 155);
doc.fontSize(28).font('Helvetica-Bold').fillColor(WHITE).text('para Migração Lovable', 56, 188);

doc.rect(56, 230, 60, 3).fill(ACCENT);

doc.fontSize(12).font('Helvetica').fillColor('rgba(255,255,255,0.75)').text(
  'Documentação completa de todas as páginas, componentes, endpoints\nde API e fluxos de autenticação da plataforma Quanta Shop —\npreparada para reprodução na plataforma Lovable (React + TypeScript\n+ Tailwind) com integração ao backend Replit.',
  56, 245, { width: 450, lineGap: 4 }
);

const metaY = 400;
[
  ['VERSÃO', '1.0.0'],
  ['DATA', 'Março 2026'],
  ['PLATAFORMA', 'quantashop.com.br'],
  ['DESTINO', 'Lovable + Replit'],
].forEach(([label, val], i) => {
  const x = 56 + i * 125;
  doc.fontSize(8).font('Helvetica').fillColor('rgba(255,255,255,0.5)').text(label, x, metaY);
  doc.fontSize(11).font('Helvetica-Bold').fillColor(WHITE).text(val, x, metaY + 14);
});

// ─── ÍNDICE ──────────────────────────────────────────────────────────────────
doc.addPage();
heading1('Índice');
divider();

const toc = [
  ['1 — Visão Geral do Sistema', 'Stack atual, recomendação Lovable, restrições de integração'],
  ['2 — Identidade Visual', 'Paleta de cores, tipografia, tokens de design'],
  ['3 — Páginas Públicas', 'Landing, login, cadastro, FAQ, lojas físicas, ranking'],
  ['4 — Painel do Afiliado', 'Dashboard, financeiro, rede, suporte, cupons e mais 10 páginas'],
  ['5 — Painel Administrativo', 'Dashboard admin + 22 rotas de gestão'],
  ['6 — Componentes Compartilhados', '14 componentes React com props documentadas'],
  ['7 — Fluxo de Autenticação', 'JWT, localStorage, guards de rota, Zustand store'],
  ['8 — Endpoints de API', '23 endpoints documentados com método e autenticação'],
  ['9 — Guia de Integração Lovable → Replit', 'Proxy, estratégias, checklist de migração'],
];

toc.forEach(([section, desc], idx) => {
  const y = doc.y;
  doc.fontSize(10).font('Helvetica-Bold').fillColor(PRIMARY).text(section, 56, y, { continued: false, width: 380 });
  doc.fontSize(9).font('Helvetica').fillColor(MUTED).text(desc, 56, doc.y, { width: 400, lineGap: 1 });
  doc.moveTo(56, doc.y + 4).lineTo(539, doc.y + 4).strokeColor(BORDER).stroke();
  doc.moveDown(0.6);
});

// ─── SEÇÃO 1 ─────────────────────────────────────────────────────────────────
sectionHeader(1, 'Visão Geral do Sistema');
heading2('Sistema: Quanta Shop — Plataforma de Cashback e Afiliados');
body('URL de produção: quantashop.com.br — Plataforma B2C/B2B conectando consumidores, lojistas e afiliados por meio de cashback automático e rede de indicações (MLM).');
doc.moveDown(0.5);

heading3('Stack Atual (Nuxt 3)');
const stack = [
  ['Framework', 'Nuxt 3 + Vue 3 + TypeScript'],
  ['CSS', 'Bootstrap 5 + SCSS'],
  ['Estado', 'Pinia'],
  ['Renderização', 'SPA client-side (SSR desabilitado)'],
  ['Auth', 'JWT em localStorage — chave: agencia_user'],
  ['Backend', '.NET 8 (MMN.Api, porta 8000, Azure SQL)'],
  ['Proxy', 'Nitro server-side /api-proxy/*'],
];
tableHeader(['Camada', 'Tecnologia']);
stack.forEach(([k, v], i) => tableRow([k, v], i % 2 === 0));

doc.moveDown(0.8);
heading3('Stack Recomendada (Lovable)');
const lovable = [
  ['React 18 + TypeScript', 'Framework principal', 'Nuxt 3 / Vue 3'],
  ['Tailwind CSS', 'Estilização utilitária', 'Bootstrap 5 + SCSS'],
  ['React Router v6', 'Roteamento', 'Nuxt Router'],
  ['Zustand', 'Estado global', 'Pinia'],
  ['Axios + interceptor', 'HTTP + token JWT', 'Axios já usado'],
  ['React Hook Form + Zod', 'Formulários + validação', 'VeeValidate'],
  ['TanStack Table', 'Tabelas avançadas', 'Bootstrap Table'],
  ['Recharts', 'Gráficos e dashboards', 'Vue-chartjs'],
  ['Headless UI / Radix UI', 'Modais e dropdowns acessíveis', 'Bootstrap Modal'],
];
tableHeader(['Pacote Lovable', 'Função', 'Equivalente Vue']);
lovable.forEach(([a, b, c], i) => tableRow([a, b, c], i % 2 === 0));

doc.moveDown(0.8);
doc.rect(56, doc.y, PAGE_W, 56).fillAndStroke(WARN_BG, WARN_BORDER);
const warnY = doc.y + 8;
doc.fontSize(9.5).font('Helvetica-Bold').fillColor('#92400e').text('IMPORTANTE — Regras de Integração com Replit', 68, warnY);
doc.fontSize(9).font('Helvetica').fillColor(TEXT).text(
  '• Usar SEMPRE paths relativos /api-proxy/* — nunca URLs absolutas\n• Manter chave localStorage agencia_user (compatibilidade com código existente)\n• NÃO alterar estrutura de rotas da API .NET no backend Replit',
  68, doc.y + 2, { width: PAGE_W - 24, lineGap: 2 }
);
doc.y = warnY + 56;

// ─── SEÇÃO 2 ─────────────────────────────────────────────────────────────────
sectionHeader(2, 'Identidade Visual');
heading2('Paleta de Cores');

const colors = [
  ['$quanta-teal', '#2F7785', 'Cor primária — botões, links, títulos, ícones ativos'],
  ['$quanta-dark', '#225F6B', 'Hover e ênfase — versão escura da primária'],
  ['$quanta-green', '#98C73A', 'Acento e conversão — cashback, CTAs, badges de sucesso'],
  ['$quanta-bg', '#F4F4F5', 'Fundo do painel, superfícies cinzas'],
  ['--bg-painel', '#ECF2F7', 'Área do painel de afiliado, login'],
  ['--footer-dark', '#111827', 'Footer, seções com fundo escuro'],
  ['--text-main', '#1F2937', 'Corpo de texto e headings'],
  ['--text-muted', '#6B7280', 'Labels, subtítulos, placeholders'],
  ['--border', '#E5E7EB', 'Cards, inputs, dividers'],
];
tableHeader(['Token SCSS/CSS', 'Hex', 'Uso']);
colors.forEach(([t, h, u], i) => tableRow([t, h, u], i % 2 === 0));

doc.moveDown(0.8);
heading2('Tipografia');
bullet("Fonte primária: 'Jost' (Google Fonts) — weights 400, 500, 600, 700, 800");
bullet("Fallback: 'Inter', system-ui, sans-serif");
bullet('Tamanhos: H1 42–48px | H2 28–34px | H3 18–22px | Body 14–15px | Caption 11–12px');
bullet('Border-radius: 8px (cards/inputs) | 12–16px (seções) | 999px (pills/badges)');

// ─── SEÇÃO 3 ─────────────────────────────────────────────────────────────────
sectionHeader(3, 'Páginas Públicas (sem autenticação)');

heading2('/agencia — Landing Institucional');
body('Página de entrada da área de negócios. Seções sequenciais:');
bullet('Hero: fundo escuro, título de impacto, CTA "Cadastre-se" (primário) e "Login" (secundário)');
bullet('Como funciona: 4 passos numerados — cadastrar, indicar, comprar, sacar');
bullet('Benefícios: grade de 6 cards com ícones SVG');
bullet('CTA final: fundo teal com chamada para cadastro');

doc.moveDown(0.5);
heading2('/agencia/login — Login do Afiliado');
bullet('Campos: Login ou e-mail, Senha (toggle de visibilidade)');
bullet('Ação: POST /usuario/autenticar → retorna JWT + dados do usuário');
bullet("Storage: localStorage.setItem('agencia_user', JSON.stringify(resp))");
bullet('Redirect pós-login: /agencia/painel');
bullet('Links: "Esqueci minha senha" → /recuperar-senha | "Criar conta" → /cadastro');
bullet('Social: Login com Google (OAuth)');

doc.moveDown(0.5);
heading2('/agencia/cadastro — Cadastro de Afiliado');
bullet('Campos: nome completo, login (único), e-mail, senha, confirmação, CPF (máscara), telefone (máscara), indicador (pré-preenchido via query param)');
bullet('Ação: POST /usuario/cadastrar');
bullet('Validação: CPF válido, e-mail único, senha ≥ 8 chars, confirmação igual');
bullet('Feedback: Toast de sucesso → redirect para login');

doc.moveDown(0.5);
heading2('Demais Páginas Públicas');
const publicPages = [
  ['/agencia/recuperar-senha', 'Campo e-mail → enviar link de recuperação', 'POST /usuario/recuperarSenha'],
  ['/agencia/quem-somos', 'Missão, visão, valores, história — layout texto + imagem', 'Estático'],
  ['/agencia/como-funciona', 'Modelo de negócio: cashback, afiliados, ecossistema', 'Estático'],
  ['/agencia/faq', 'Acordeão com perguntas por categoria (Cadastro, Cashback, Saques)', 'Estático'],
  ['/agencia/privacidade', 'Política de privacidade LGPD completa com âncoras internas', 'Estático'],
  ['/agencia/lojas-fisicas', 'Mapa Google Maps/Leaflet + lista de lojas com cashback %', 'GET /LojasFisicas/obterLojasFisicas'],
  ['/agencia/mais-vendas/[[login]]', 'Ranking de afiliados por vendas, parâmetro login opcional', 'GET /Ranking/obterMaisVendas/{login?}'],
];
tableHeader(['Rota', 'Conteúdo', 'API / Dados']);
publicPages.forEach(([a, b, c], i) => tableRow([a, b, c], i % 2 === 0));

// ─── SEÇÃO 4 ─────────────────────────────────────────────────────────────────
sectionHeader(4, 'Painel do Afiliado (autenticação obrigatória)');

doc.rect(56, doc.y, PAGE_W, 44).fillAndStroke(BG_TEAL, PRIMARY);
const infoY = doc.y + 6;
doc.fontSize(9.5).font('Helvetica-Bold').fillColor(PRIMARY).text('Layout base do painel', 68, infoY);
doc.fontSize(9).font('Helvetica').fillColor(TEXT).text(
  'Sidebar fixa (esquerda): Logo + menu colapsável + avatar do usuário com nome e graduação\nTopBar: Link de indicação copiável + notificações + logout\nConteúdo (direita): Breadcrumb + título da página + área de conteúdo',
  68, infoY + 14, { width: PAGE_W - 24, lineGap: 2 }
);
doc.y = infoY + 44;
doc.moveDown(0.5);

heading2('/agencia/painel — Dashboard Principal');
heading3('4 Cards de Estatísticas — GET /v2/dashboard/get-totalizers-user');
bullet('Equipe: Total de afiliados na rede do usuário');
bullet('Saldo: Valor disponível para saque (R$)');
bullet('Ecossistemas: Ecossistemas de pontos ativos');
bullet('Pontos: Pontuação acumulada no período');

heading3('Outros elementos do dashboard');
bullet('Meta Minuto: Contador regressivo até próxima meta — GET /MetaMinuto/obterMetaMinuto');
bullet('3 Vídeos YouTube: Boas-vindas, Como funciona, Como ganhar (thumbnails com modal)');
bullet('Mapa Google Maps: Iframe com lojas credenciadas próximas');
bullet("Link de indicação: Input copiável com URL /register/{login} + Modal 'Convide seus amigos'");

doc.moveDown(0.5);
heading2('/agencia/painel/meus-dados — 4 abas');
const dadosTabs = [
  ['Dados pessoais', 'Nome, CPF (mascarado), data nasc., e-mail, telefone, foto de perfil (upload)'],
  ['Endereço', 'CEP (auto-fill ViaCEP), rua, número, complemento, bairro, cidade, estado'],
  ['Conta bancária', 'Banco (select), agência, conta, dígito, tipo corrente/poupança, CPF titular'],
  ['Senha', 'Senha atual, nova senha, confirmação de nova senha'],
];
tableHeader(['Aba', 'Campos']);
dadosTabs.forEach(([a, b], i) => tableRow([a, b], i % 2 === 0));
body('API: GET /usuario/obterDados | PUT /usuario/alterarDados');
bullet('Alerta: Banner de aviso quando perfil está incompleto (pré-cadastro pendente)');

doc.moveDown(0.5);
heading2('/agencia/painel/financeiro — CCR (Conta de Consumo Remunerada)');
body('Resumo no topo: Saldo disponível | Total recebido | Total sacado');
const finTabs = [
  ['Movimentações', 'Tabela: descrição, data, valor, tipo (CRÉDITO verde / DÉBITO vermelho)', 'GET /financeiro/listaMovimentacoes'],
  ['Solicitar Saque', 'Campo de valor + botão solicitar + aviso de prazo de processamento', 'POST /saque/solicitar'],
  ['Histórico de Saques', 'Tabela: data solicitação, valor, status badge colorido', 'GET /saque/historico'],
];
tableHeader(['Aba', 'Conteúdo', 'API']);
finTabs.forEach(([a, b, c], i) => tableRow([a, b, c], i % 2 === 0));

doc.moveDown(0.5);
heading2('/agencia/painel/minhas-compras');
bullet('Filtros: Descrição (texto), Data compra inicial, Data compra final');
bullet('Tabela: Descrição, Valor da compra, Cashback a receber, Data, Status badge, Botão Detalhes');
bullet('Modal Detalhes: Movimentação detalhada + Distribuição do cashback por nível de rede');
body('API: POST /Pedidos/listaPedidosAfiliados');

doc.moveDown(0.5);
heading2('Rede, Performance e Graduações');
const redePages = [
  ['/painel/minha-rede', 'Cards: Total, Diretos, Inativos, VIPs. Tabela: login, nome, nível, status, data entrada', 'GET /Rede/obterMinhaRede'],
  ['/painel/meus-diretos', 'Tabela: login, nome, graduação, status, data', 'GET /Rede/obterMeusDiretos'],
  ['/painel/performance', 'Radio top/bottom 10. Tabela: Login, Nome, Graduação, Consumo, Nível. Exportar Excel.', 'POST /Dashboard/obterPerformance'],
  ['/painel/graduacoes', 'Lista de níveis: nome, ícone, requisitos de pontos, benefícios', 'GET /Graduacoes/obterGraduacoes'],
];
tableHeader(['Rota', 'Conteúdo', 'API']);
redePages.forEach(([a, b, c], i) => tableRow([a, b, c], i % 2 === 0));

doc.moveDown(0.5);
heading2('Suporte e Chamados');
body('Status: 1=Em proc. | 2=Finalizado | 3=Cancelado | 4=Em aprovação | 5=Recusado | 6=Aprovado | 7=Aguard. pgto');
body('Tipos: 1=Contato | 2=Cashback não pago | 3=Cancelamento parcelamento | 4=Reabertura parcelamento');
const suportePages = [
  ['/painel/suporte', 'Filtros por status/tipo/data. Tabela: #, Título, Tipo, Data, Status, Ações', 'POST /Suporte/listaSuporte'],
  ['/painel/solicitar-suporte', 'Campos: tipo, assunto, descrição, anexo opcional', 'POST /Suporte/abrirSuporte'],
  ['/painel/contas-bancarias', 'CRUD de contas para saque. Modal add/edit, confirm para remover', 'CRUD interno'],
];
tableHeader(['Rota', 'Conteúdo', 'API']);
suportePages.forEach(([a, b, c], i) => tableRow([a, b, c], i % 2 === 0));

doc.moveDown(0.5);
heading2('Demais Páginas do Painel do Afiliado');
const demaisPages = [
  ['/painel/assinatura', 'Plano ativo, data vencimento, histórico de pagamentos'],
  ['/painel/planos', 'Grid de planos com preços, benefícios e CTA de contratação'],
  ['/painel/cupons', 'Cupons de cashback disponíveis para ativar'],
  ['/painel/meus-cupons', 'Tabela: código, valor, data, status (ativo/usado/expirado)'],
  ['/painel/gerar-cupons', 'Formulário: CNPJ estabelecimento, valor, data validade'],
  ['/painel/inserir-cupom', 'Campo de chave NF-e para validar cashback de compra física'],
  ['/painel/material-apoio', 'Biblioteca de arquivos de marketing — download por categoria'],
  ['/painel/tutoriais-usuario', 'Vídeos tutoriais categorizados com player YouTube embutido'],
  ['/painel/faq', 'FAQ em formato acordeão para usuários autenticados'],
  ['/painel/comerciante', 'Dados de credenciamento, QR code de cobrança, transações recebidas'],
  ['/painel/meus-credenciamentos', 'Pedidos de credenciamento: nome, CNPJ, data, status'],
];
tableHeader(['Rota', 'Descrição']);
demaisPages.forEach(([a, b], i) => tableRow([a, b], i % 2 === 0));

// ─── SEÇÃO 5 ─────────────────────────────────────────────────────────────────
sectionHeader(5, 'Painel Administrativo (/agencia/painel/admin)');

doc.rect(56, doc.y, PAGE_W, 32).fillAndStroke(WARN_BG, WARN_BORDER);
const warnY2 = doc.y + 6;
doc.fontSize(9.5).font('Helvetica-Bold').fillColor('#92400e').text('Acesso Restrito — Perfil Admin', 68, warnY2);
doc.fontSize(9).font('Helvetica').fillColor(TEXT).text(
  "Middleware verifica campo 'perfil' ou 'role' no JWT. Redireciona para /agencia/painel se não for admin.",
  68, warnY2 + 14, { width: PAGE_W - 24 }
);
doc.y = warnY2 + 32;
doc.moveDown(0.5);

const adminPages = [
  ['/admin (Dashboard)', '4 cards: total usuários, compras, cashback pago, estabelecimentos. Gráficos de crescimento.'],
  ['/admin/usuarios', 'Busca por nome/e-mail. Tabela: avatar, login, nome, graduação, status. Ações: perfil, editar, bloquear, impersonar.'],
  ['/admin/credenciamento', 'Tabs: Pendente/Aprovado/Reprovado. Ações: aprovar (modal), reprovar com motivo, ver detalhes.'],
  ['/admin/compras', 'Relatório geral de compras. Filtros: data, usuário, estabelecimento, status. Exportar Excel.'],
  ['/admin/pagamentos', 'Cobranças e assinaturas. Ações: confirmar, cancelar, reenviar boleto.'],
  ['/admin/categorias', 'CRUD de categorias de lojas. Drag-and-drop reordenar. Modal: nome, ícone, descrição.'],
  ['/admin/ecossistemas', 'Gestão de ecossistemas de pontos com regras de pontuação.'],
  ['/admin/carrosseis', 'CRUD de banners home. Upload desktop/mobile, link, ordem, ativo. Preview em tempo real.'],
  ['/admin/comunicados', 'Notificações para usuários. Segmentação: todos/grupo/individual. Agendamento.'],
  ['/admin/rede', 'Árvore binária interativa. Busca por login. Zoom e pan.'],
  ['/admin/suporte', 'Todos os tickets. Atribuir agente, responder, fechar. Filtros completos.'],
  ['/admin/lojas-credenciados', 'Lista de estabelecimentos ativos com dados e métricas.'],
  ['/admin/alterar-dados-usuario', 'Formulário admin para editar qualquer usuário por login.'],
  ['/admin/relatorio-de-faturas', 'Faturas financeiras por período. Exportação Excel.'],
  ['/admin/relatorio-de-anunciantes', 'Desempenho e métricas dos parceiros anunciantes.'],
  ['/admin/relatorio-cashback', 'Cashback distribuído por período, usuário, loja. Exportação.'],
  ['/admin/aniversariantes', 'Usuários com aniversário no período selecionado.'],
  ['/admin/acessos', 'Log de acessos: usuário, data/hora, IP, dispositivo, sessão.'],
  ['/admin/lancamentos', 'Lançamentos manuais de saldo com justificativa e aprovação.'],
  ['/admin/material-apoio-admin', 'Upload e gestão de arquivos de marketing para afiliados.'],
  ['/admin/assinaturas', 'Gerenciar assinaturas ativas, vencidas, canceladas.'],
  ['/admin/gerenciar-grupos', 'CRUD de grupos de usuários para segmentação de comunicados.'],
];
tableHeader(['Rota', 'Conteúdo e Funcionalidade']);
adminPages.forEach(([a, b], i) => tableRow([a, b], i % 2 === 0));

// ─── SEÇÃO 6 ─────────────────────────────────────────────────────────────────
sectionHeader(6, 'Componentes Compartilhados (Lovable)');

const components = [
  ['Sidebar', 'menuItems, collapsed, user', 'Menu lateral com submenu colapsável, avatar, graduação do usuário'],
  ['TopBar', 'user, indicationLink', 'Link de indicação copiável, notificações, logout'],
  ['StatCard', 'icon, label, value, trend, color', 'Card de estatística com ícone colorido e tendência (seta ↑/↓)'],
  ['PageHeader', 'title, subtitle, breadcrumb[]', 'Título + subtítulo + breadcrumb de navegação'],
  ['FilterBox', 'children, onApply, onReset', 'Container de filtros com botões Aplicar e Limpar'],
  ['DataTable', 'columns, data, loading, empty, pagination', 'Tabela responsiva: skeleton loading, estado vazio, paginação'],
  ['StatusBadge', 'status (number|string), map', 'Badge colorido mapeado por valor de status numérico'],
  ['LoadingSpinner', 'size, color, fullscreen', 'Indicador de carregamento circular'],
  ['EmptyState', 'icon, title, message, action', 'Estado vazio: ícone SVG + mensagem + botão opcional'],
  ['Modal', 'open, onClose, title, size, footer', 'Modal genérico: overlay, header, body, footer, fechar com ESC'],
  ['ConfirmDialog', 'open, message, onConfirm, onCancel', 'Dialog de confirmação para ações destrutivas'],
  ['Toast', 'type, message, duration', 'Notificação temporária: success (verde), error (vermelho), info'],
  ['CopyInput', 'value, label', 'Input read-only com botão Copiar e feedback visual Copiado!'],
  ['FileUpload', 'accept, maxSize, onUpload', 'Drag-and-drop para upload de imagens e arquivos'],
];
tableHeader(['Componente', 'Props', 'Descrição']);
components.forEach(([a, b, c], i) => tableRow([a, b, c], i % 2 === 0));

doc.moveDown(0.8);
doc.rect(56, doc.y, PAGE_W, 52).fillAndStroke(BG_TEAL, PRIMARY);
const hookY = doc.y + 8;
doc.fontSize(9.5).font('Helvetica-Bold').fillColor(PRIMARY).text('Hook useApi — Axios configurado para o proxy Replit', 68, hookY);
doc.fontSize(8.5).font('Helvetica').fillColor(TEXT).text(
  "Criar src/lib/api.ts com instância Axios (baseURL: '/api-proxy') e interceptor de request que lê localStorage.getItem('agencia_user'), extrai o token e adiciona o header Authorization: Bearer TOKEN em toda requisição.\nInterceptor de response: se HTTP 401, limpar localStorage e redirecionar para /agencia/login.",
  68, hookY + 16, { width: PAGE_W - 24, lineGap: 2 }
);
doc.y = hookY + 52;

// ─── SEÇÃO 7 ─────────────────────────────────────────────────────────────────
sectionHeader(7, 'Fluxo de Autenticação JWT');

const authSteps = [
  ['1. Submeter login', "Usuário preenche login/e-mail + senha → POST /usuario/autenticar"],
  ['2. Receber resposta', "API retorna JSON com token (JWT), nome, login, perfil e dados do usuário"],
  ['3. Armazenar', "localStorage.setItem('agencia_user', JSON.stringify(responseData))"],
  ['4. Usar token', "Todo request autenticado: header Authorization: Bearer {token} via interceptor Axios"],
  ['5. Guard de rota', "Antes de renderizar página protegida: verificar agencia_user e decodificar JWT (checar campo exp)"],
  ['6. Token expirado', "localStorage.removeItem('agencia_user') → redirect para /agencia/login"],
  ['7. Perfil admin', "Verificar campo perfil no payload JWT — se não for admin, bloquear /admin/* e redirecionar"],
];
tableHeader(['Passo', 'Ação']);
authSteps.forEach(([a, b], i) => tableRow([a, b], i % 2 === 0));

doc.moveDown(0.8);
heading2('Zustand Auth Store (equivalente ao Pinia atual)');
const zustandStore = [
  ['user', 'User | null', 'Objeto completo do usuário autenticado'],
  ['token', 'string | null', 'JWT ativo atual'],
  ['isAdmin', 'boolean', 'true se perfil admin (verificado no JWT)'],
  ['isAuthenticated', 'computed boolean', 'true se token existir e não estiver expirado'],
  ['setUser(data)', 'action', 'Popula store + persiste em localStorage'],
  ['logout()', 'action', 'Limpa store + localStorage + redireciona para /login'],
  ['initFromStorage()', 'action', 'Inicializa store ao montar o app lendo localStorage'],
];
tableHeader(['State / Action', 'Tipo', 'Descrição']);
zustandStore.forEach(([a, b, c], i) => tableRow([a, b, c], i % 2 === 0));

// ─── SEÇÃO 8 ─────────────────────────────────────────────────────────────────
sectionHeader(8, 'Endpoints de API');
body("Todos os endpoints são chamados via /api-proxy/{endpoint} no frontend. O proxy Nitro encaminha para http://localhost:8000/{endpoint} com os headers originais (incluindo Authorization).");
doc.moveDown(0.4);

const endpoints = [
  ['POST', '/usuario/autenticar', '—', 'Login — retorna JWT + dados do usuário'],
  ['POST', '/usuario/cadastrar', '—', 'Cadastro de novo afiliado'],
  ['POST', '/usuario/recuperarSenha', '—', 'Enviar link de recuperação de senha'],
  ['GET', '/usuario/obterDados', 'JWT', 'Dados completos do perfil do usuário logado'],
  ['PUT', '/usuario/alterarDados', 'JWT', 'Atualizar dados do perfil (qualquer campo)'],
  ['GET', '/v2/dashboard/get-totalizers-user', 'JWT', 'Cards do dashboard: equipe, saldo, ecossistemas, pontos'],
  ['GET', '/MetaMinuto/obterMetaMinuto', 'JWT', 'Dados do contador Meta Minuto'],
  ['GET', '/financeiro/saldo', 'JWT', 'Saldo disponível para saque (R$)'],
  ['GET', '/financeiro/listaMovimentacoes', 'JWT', 'Extrato financeiro paginado'],
  ['POST', '/saque/solicitar', 'JWT', 'Solicitar saque para conta bancária cadastrada'],
  ['GET', '/saque/historico', 'JWT', 'Histórico de saques com status'],
  ['POST', '/Pedidos/listaPedidosAfiliados', 'JWT', 'Minhas compras com filtros de data e paginação'],
  ['GET', '/Rede/obterMinhaRede', 'JWT', 'Rede completa de indicados (todos os níveis)'],
  ['GET', '/Rede/obterMeusDiretos', 'JWT', 'Somente indicados diretos (nível 1)'],
  ['POST', '/Dashboard/obterPerformance', 'JWT', 'Tabela de performance da rede com filtros'],
  ['GET', '/Graduacoes/obterGraduacoes', 'JWT', 'Lista de graduações/níveis do sistema'],
  ['POST', '/Suporte/listaSuporte', 'JWT', 'Tickets de suporte com filtros de status/tipo/data'],
  ['POST', '/Suporte/abrirSuporte', 'JWT', 'Abrir novo ticket com tipo, assunto, descrição, anexo'],
  ['GET', '/geral/obterMenu/{perfil}', 'JWT', 'Menu dinâmico personalizado por perfil do usuário'],
  ['GET', '/LojasFisicas/obterLojasFisicas', '—', 'Lojas físicas credenciadas com coordenadas'],
  ['GET', '/Ranking/obterMaisVendas/{login?}', '—', 'Ranking de afiliados por volume de vendas'],
  ['GET', '/parceiros/obterParceiros', '—', 'Lista de parceiros online para a home'],
  ['GET', '/carousel/obterCarousel', '—', 'Banners do carrossel da home page'],
];
tableHeader(['Método', 'Endpoint', 'Auth', 'Uso']);
endpoints.forEach(([a, b, c, d], i) => tableRow([a, b, c, d], i % 2 === 0));

// ─── SEÇÃO 9 ─────────────────────────────────────────────────────────────────
sectionHeader(9, 'Guia de Integração Lovable → Replit');

doc.rect(56, doc.y, PAGE_W, 40).fillAndStroke(WARN_BG, WARN_BORDER);
const w3Y = doc.y + 6;
doc.fontSize(9.5).font('Helvetica-Bold').fillColor('#92400e').text('Regras Inegociáveis', 68, w3Y);
doc.fontSize(9).font('Helvetica').fillColor(TEXT).text(
  "• Usar SEMPRE paths relativos /api-proxy/* — nunca URLs absolutas (https://api.quantashop...)\n• Manter chave localStorage agencia_user | • NÃO alterar rotas da API .NET no backend Replit",
  68, w3Y + 14, { width: PAGE_W - 24, lineGap: 2 }
);
doc.y = w3Y + 40;
doc.moveDown(0.5);

heading2('O Proxy Nitro (já configurado no Replit)');
body('O arquivo server/routes/api-proxy/[...path].ts recebe /api-proxy/ENDPOINT e faz proxy para http://localhost:8000/ENDPOINT, passando todos os headers (incluindo Authorization). CORS já está resolvido nesta camada — o frontend não precisa se preocupar com isso.');

doc.moveDown(0.5);
heading2('Estratégias de Migração');
const strategies = [
  ['A — Substituição gradual', 'Migrar página a página, usando ClientOnly para componentes React dentro do Nuxt', 'Média'],
  ['B — Frontend separado (recomendada)', 'Novo Repl apenas para frontend React. Configurar proxy apontando para o Repl original.', 'Baixa'],
  ['C — Substituição completa', 'Migrar todo o frontend de uma vez, mantendo apenas proxy + backend no Replit original', 'Alta'],
];
tableHeader(['Estratégia', 'Descrição', 'Dificuldade']);
strategies.forEach(([a, b, c], i) => tableRow([a, b, c], i % 2 === 0));

doc.moveDown(0.5);
heading2('Checklist de Migração Lovable');
heading3('Fase 1 — Estrutura base');
['Configurar React + TypeScript + Tailwind no Lovable',
 "Criar src/lib/api.ts com Axios + interceptores JWT",
 'Criar Zustand auth store com initFromStorage()',
 'Implementar React Router v6 com PrivateRoute',
 'Construir Sidebar + TopBar + Layout base do painel',
 'Criar componentes: StatCard, DataTable, Modal, Toast, CopyInput'
].forEach(t => bullet('☐ ' + t));

doc.moveDown(0.3);
heading3('Fase 2 — Páginas com dados reais');
['Páginas públicas: login, cadastro, recuperar-senha',
 'Dashboard do afiliado com API real (4 cards + Meta Minuto)',
 'Módulo Financeiro completo (3 abas)',
 'Módulo Rede: Minha Rede + Meus Diretos + Performance',
 'Módulo Suporte completo (lista + abertura)',
 'Painel Admin com todas as 22 rotas',
 'Testar autenticação end-to-end com API real',
 'Validar responsividade mobile em todos os módulos',
].forEach(t => bullet('☐ ' + t));

// ─── RODAPÉ ──────────────────────────────────────────────────────────────────
const range = doc.bufferedPageRange();
for (let i = range.start; i < range.start + range.count; i++) {
  doc.switchToPage(i);
  const footerY = doc.page.height - 30;
  doc.rect(0, footerY - 8, 595.28, 38).fill('#111827');
  doc.fontSize(8).font('Helvetica').fillColor('rgba(255,255,255,0.5)')
    .text(
      'Quanta Shop — Documento Técnico Confidencial | v1.0.0 | Março 2026 | quantashop.com.br',
      56, footerY,
      { align: 'center', width: 483 }
    );
}

doc.end();

out.on('finish', () => {
  const stats = require('fs').statSync('/home/runner/workspace/docs/quanta-shop-technical-spec.pdf');
  console.log('PDF gerado com sucesso! Tamanho:', Math.round(stats.size / 1024), 'KB');
});
