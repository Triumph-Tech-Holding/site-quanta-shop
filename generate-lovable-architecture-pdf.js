const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const OUT = path.join(__dirname, 'public', 'lovable-architecture.pdf');
const doc = new PDFDocument({ size: 'A4', margin: 50, bufferPages: true });
doc.pipe(fs.createWriteStream(OUT));

// ── Colors ───────────────────────────────────────────────────────────────────
const TEAL  = '#2f7785';
const GREEN = '#98c73a';
const DARK  = '#1a2b33';
const GRAY  = '#5a6a72';
const LGRAY = '#ecf2f7';
const WHITE = '#ffffff';
const RED   = '#e74c3c';
const AMBER = '#f39c12';

const PW = doc.page.width - 100;

// ── Helpers ───────────────────────────────────────────────────────────────────
function addPage() { doc.addPage(); }

function hRule(color = LGRAY, lw = 0.5) {
  doc.save().strokeColor(color).lineWidth(lw)
     .moveTo(50, doc.y).lineTo(50 + PW, doc.y).stroke().restore();
  doc.moveDown(0.4);
}

function heading1(text) {
  doc.moveDown(0.5);
  const ty = doc.y;
  doc.save().rect(50, ty, PW, 34).fill(TEAL).restore();
  doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(15)
     .text(text, 62, ty + 9, { width: PW - 24 }).restore();
  doc.moveDown(2.6);
}

function heading2(text) {
  doc.moveDown(0.5);
  const ty = doc.y;
  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(12)
     .text(text, 50, ty).restore();
  doc.save().strokeColor(GREEN).lineWidth(2)
     .moveTo(50, doc.y).lineTo(50 + PW * 0.3, doc.y).stroke().restore();
  doc.moveDown(0.7);
}

function heading3(text) {
  doc.moveDown(0.3);
  doc.save().fillColor(GREEN).font('Helvetica-Bold').fontSize(10.5)
     .text(text, 50, doc.y).restore();
  doc.moveDown(0.4);
}

function body(text) {
  doc.save().fillColor(DARK).font('Helvetica').fontSize(9.5)
     .text(text, 50, doc.y, { width: PW, align: 'justify' }).restore();
  doc.moveDown(0.3);
}

function bullet(text, indent = 12) {
  const bx = 50 + indent;
  const ty = doc.y;
  doc.save().fillColor(GREEN).font('Helvetica-Bold').fontSize(9.5)
     .text('•', bx, ty, { width: 10 }).restore();
  doc.save().fillColor(DARK).font('Helvetica').fontSize(9.5)
     .text(text, bx + 12, ty, { width: PW - indent - 12 }).restore();
  doc.moveDown(0.25);
}

function label(key, val) {
  const ty = doc.y;
  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(9)
     .text(key + ':', 50, ty, { width: 105 }).restore();
  doc.save().fillColor(DARK).font('Helvetica').fontSize(9)
     .text(val, 158, ty, { width: PW - 108 }).restore();
  doc.moveDown(0.3);
}

function screenCard(screen) {
  if (doc.y > 640) addPage();
  const cx = 50, cy = doc.y, cw = PW;
  const col2 = cx + Math.floor(cw / 2) + 4;
  const hw = Math.floor(cw / 2) - 8;

  doc.save().rect(cx, cy, cw, 18).fill(TEAL).restore();
  doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(10)
     .text(screen.nome + '  ', cx + 6, cy + 4, { continued: true })
     .fillColor('#c8e6ea').font('Helvetica').fontSize(8)
     .text(screen.rota || '', { width: cw - 16 }).restore();

  const bodyY = cy + 20;
  let lh = bodyY + 4;
  let rh = bodyY + 4;

  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(7.5).text('Propósito', cx + 6, lh).restore();
  lh += 10;
  const propH = doc.heightOfString(screen.proposito, { width: hw, fontSize: 8 });
  doc.save().fillColor(DARK).font('Helvetica').fontSize(8)
     .text(screen.proposito, cx + 6, lh, { width: hw }).restore();
  lh += propH + 6;

  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(7.5).text('Componentes', cx + 6, lh).restore();
  lh += 10;
  const compStr = screen.componentes.join(' · ');
  const compH = doc.heightOfString(compStr, { width: hw, fontSize: 8 });
  doc.save().fillColor(DARK).font('Helvetica').fontSize(8)
     .text(compStr, cx + 6, lh, { width: hw }).restore();
  lh += compH + 4;

  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(7.5).text('Dados exibidos', col2, rh).restore();
  rh += 10;
  const dadosStr = screen.dados.join(' · ');
  const dadosH = doc.heightOfString(dadosStr, { width: hw, fontSize: 8 });
  doc.save().fillColor(DARK).font('Helvetica').fontSize(8)
     .text(dadosStr, col2, rh, { width: hw }).restore();
  rh += dadosH + 6;

  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(7.5).text('Ações do usuário', col2, rh).restore();
  rh += 10;
  const acStr = screen.acoes.join(' · ');
  const acH = doc.heightOfString(acStr, { width: hw, fontSize: 8 });
  doc.save().fillColor(DARK).font('Helvetica').fontSize(8)
     .text(acStr, col2, rh, { width: hw }).restore();
  rh += acH + 4;

  const cardH = Math.max(lh, rh) - cy + 6;
  doc.save().rect(cx, bodyY, cw, cardH - 20).stroke('#d8e6ea').restore();
  doc.y = cy + cardH + 4;
}

function sectionDivider(num, title, sub) {
  addPage();
  doc.save().rect(0, 0, doc.page.width, doc.page.height).fill(LGRAY).restore();
  doc.save().rect(0, doc.page.height / 2 - 70, doc.page.width, 140).fill(TEAL).restore();
  doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(52)
     .text(num, 50, doc.page.height / 2 - 60, { width: doc.page.width - 100, align: 'center' }).restore();
  doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(22)
     .text(title, 50, doc.page.height / 2, { width: doc.page.width - 100, align: 'center' }).restore();
  doc.save().fillColor(LGRAY).font('Helvetica').fontSize(12)
     .text(sub, 50, doc.page.height / 2 + 36, { width: doc.page.width - 100, align: 'center' }).restore();
  addPage();
}

// ════════════════════════════════════════════════════════════════════════════
// COVER
// ════════════════════════════════════════════════════════════════════════════
doc.save().rect(0, 0, doc.page.width, doc.page.height).fill(TEAL).restore();
doc.save().rect(0, doc.page.height - 110, doc.page.width, 110).fill(GREEN).restore();
doc.save().rect(0, doc.page.height - 114, doc.page.width, 4).fill(WHITE).restore();

doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(9)
   .text('DOCUMENTO DE ARQUITETURA DE TELAS', 50, 72, { width: PW, align: 'center', characterSpacing: 2.5 }).restore();

doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(44)
   .text('Quanta Shop', 50, 108, { width: PW, align: 'center' }).restore();

doc.save().fillColor(LGRAY).font('Helvetica').fontSize(16)
   .text('Design Brief para o Lovable', 50, 172, { width: PW, align: 'center' }).restore();

doc.save().strokeColor(WHITE).lineWidth(0.5).opacity(0.35)
   .moveTo(90, 212).lineTo(doc.page.width - 90, 212).stroke().restore();

const toc = [
  ['01', 'Portal Público', '11 telas'],
  ['02', 'Painel do Membro', '22 telas'],
  ['03', 'Painel Administrativo', '25 telas'],
  ['04', 'Loja Pública', '8 telas'],
  ['05', 'Fluxos de Navegação', 'Por perfil'],
  ['06', 'Componentes Reutilizáveis', 'Design System'],
  ['07', 'Oportunidades de UX', 'Por ambiente'],
  ['08', 'Instruções para o Lovable', 'Prompt modelo'],
];
doc.y = 238;
toc.forEach(([n, t, s]) => {
  const ty = doc.y;
  doc.save().fillColor(GREEN).font('Helvetica-Bold').fontSize(10).text(n, 90, ty, { width: 35 }).restore();
  doc.save().fillColor(WHITE).font('Helvetica').fontSize(10).text(t, 130, ty, { width: 200 }).restore();
  doc.save().fillColor(LGRAY).font('Helvetica').fontSize(9).text(s, 340, ty, { width: 140 }).restore();
  doc.moveDown(0.85);
});

doc.save().fillColor(WHITE).font('Helvetica').fontSize(8.5)
   .text('Quanta Shop Plataforma — maio/2026', 50, doc.page.height - 88, { width: PW, align: 'center' }).restore();
doc.save().fillColor(DARK).font('Helvetica-Bold').fontSize(8.5)
   .text('Uso exclusivo para geração de UI no Lovable — não distribuir externamente', 50, doc.page.height - 68, { width: PW, align: 'center' }).restore();

// ════════════════════════════════════════════════════════════════════════════
// 01 — PORTAL PÚBLICO
// ════════════════════════════════════════════════════════════════════════════
sectionDivider('01', 'Portal Público', '/agencia — Ambiente não autenticado');
heading1('01 · Portal Público');
body('O Portal Público é a face externa da plataforma, acessível sem autenticação. Usa o layout agencia.vue com header sticky (logo + nav) e footer. Paleta principal: teal #2f7785 e verde #98c73a. Objetivo: converter visitantes em membros ou comerciantes credenciados.');
doc.moveDown(0.4);

[
  { nome: 'Landing Page da Agência', rota: '/agencia',
    proposito: 'Porta de entrada do portal. Apresenta cashback, rede de indicações e lojas credenciadas. Converte visitante em membro.',
    componentes: ['Hero com CTA duplo (Começar agora / Já tenho conta)', 'Grid de benefícios (3 cards)', 'Seção Como Funciona (4 steps numerados)', 'CTA final de cadastro'],
    dados: ['Textos de marketing estáticos', 'Features da plataforma', 'Steps ilustrados'],
    acoes: ['"Começar agora" → /agencia/cadastro', '"Já tenho conta" → /agencia/login', '"Saber mais" → /agencia/como-funciona'] },

  { nome: 'Quem Somos', rota: '/agencia/quem-somos',
    proposito: 'Apresenta a empresa, missão, valores e história da Quanta Shop.',
    componentes: ['Seção institucional com foto', 'Cards de missão e valores', 'Timeline da empresa'],
    dados: ['Texto institucional', 'Missão e valores', 'Ano de fundação e marcos'],
    acoes: ['Links de navegação interna', 'CTA para cadastro'] },

  { nome: 'Como Funciona', rota: '/agencia/como-funciona',
    proposito: 'Explica o funcionamento do cashback, rede MLM e modelo de ganhos por minuto.',
    componentes: ['Steps ilustrados', 'Infográfico de rede MLM', 'FAQ inline', 'Player de vídeo YouTube'],
    dados: ['Regras de cashback', 'Percentuais por nível (exemplo)', 'Exemplos de ganho em R$'],
    acoes: ['Assistir vídeo explicativo', 'Ir para cadastro', 'Ver FAQ completo'] },

  { nome: 'FAQ', rota: '/agencia/faq',
    proposito: 'Responde as dúvidas mais frequentes sobre cashback, saques, indicações e credenciamento.',
    componentes: ['Accordion de perguntas/respostas', 'Buscador de FAQ', 'CTA de suporte'],
    dados: ['Perguntas e respostas por categoria'],
    acoes: ['Expandir/recolher pergunta', 'Buscar por palavra-chave', 'Abrir suporte'] },

  { nome: 'Lojas Físicas', rota: '/agencia/lojas-fisicas',
    proposito: 'Lista os estabelecimentos físicos credenciados com filtro por cidade e categoria.',
    componentes: ['Mapa Google Maps embutido', 'Lista de lojas com filtros', 'Card de loja (nome, endereço, % cashback)'],
    dados: ['Nome da loja', 'Endereço completo', 'Categoria', '% cashback'],
    acoes: ['Filtrar por cidade/categoria', 'Ver loja no mapa', 'Compartilhar loja'] },

  { nome: 'Login', rota: '/agencia/login',
    proposito: 'Autenticação de membros via email/senha ou Login Social (Google).',
    componentes: ['Formulário email + senha', 'Botão Google OAuth', 'Link esqueci a senha', 'Link de cadastro'],
    dados: ['Campo email', 'Campo senha', 'JWT armazenado no localStorage após login'],
    acoes: ['Login email/senha', 'Login com Google', 'Ir para recuperar senha', 'Ir para cadastro'] },

  { nome: 'Cadastro', rota: '/agencia/cadastro',
    proposito: 'Registro de novo membro empreendedor com captura do link de indicação (slug do indicante).',
    componentes: ['Formulário multi-step', 'Campo indicador (auto-preenchido via URL)', 'Validação CPF/email em tempo real'],
    dados: ['Nome', 'CPF', 'Email', 'Telefone', 'Senha', 'Login do indicador'],
    acoes: ['Preencher formulário', 'Confirmar email (link enviado)', 'Ir para login'] },

  { nome: 'Recuperar Senha', rota: '/agencia/recuperar-senha',
    proposito: 'Solicita envio de link de redefinição de senha por email.',
    componentes: ['Campo email', 'Botão enviar', 'Mensagem de confirmação de envio'],
    dados: ['Email do usuário'],
    acoes: ['Informar email', 'Receber link', 'Clicar link → /agencia/reset-password/[token]'] },

  { nome: 'Credenciar Estabelecimento', rota: '/agencia/credenciar',
    proposito: 'Formulário para o empreendedor credenciar um comerciante parceiro à rede Quanta Shop.',
    componentes: ['Formulário de dados do estabelecimento', 'Upload de documentos (CNPJ, contrato)', 'Seleção de categoria'],
    dados: ['CNPJ', 'Razão social', 'Categoria', '% cashback proposto', 'Nome do responsável'],
    acoes: ['Preencher formulário', 'Enviar documentos para aprovação', 'Acompanhar status'] },

  { nome: 'Parceiro Direto', rota: '/agencia/parceiro-direto/[slug]',
    proposito: 'Landing page personalizada de um parceiro, usada como link de indicação com cashback.',
    componentes: ['Banner do parceiro', 'Informações da loja', 'CTA de compra com cashback destacado', 'QR Code'],
    dados: ['Logo e nome do parceiro', '% cashback', 'Endereço e contato'],
    acoes: ['Ativar cashback na compra', 'Compartilhar link', 'Escanear QR Code'] },

  { nome: 'Mais Vendas', rota: '/agencia/mais-vendas/[login]',
    proposito: 'Landing page de um empreendedor específico, usada para atrair mais vendas na sua rede. Exibe o link de indicação personalizado, lojas parceiras do empreendedor e CTA para o visitante se cadastrar.',
    componentes: ['Banner personalizado com foto e nome do empreendedor', 'Grid de lojas parceiras vinculadas', 'CTA de cadastro com login do empreendedor auto-preenchido', 'Contador de cashback já gerado pela rede'],
    dados: ['Nome e login do empreendedor', 'Lojas parceiras cadastradas pelo empreendedor', 'Total de cashback gerado', 'Link de cadastro personalizado'],
    acoes: ['Cadastrar-se usando o link do empreendedor', 'Ver lojas parceiras', 'Compartilhar a página'] },

  { nome: 'Privacidade (LGPD)', rota: '/agencia/privacidade',
    proposito: 'Política de privacidade e termos de uso em conformidade com a LGPD brasileira.',
    componentes: ['Documento de política formatado', 'Índice navegável', 'Data da última atualização'],
    dados: ['Texto legal completo', 'Versão e data'],
    acoes: ['Ler seções via índice', 'Baixar PDF'] },
].forEach(s => screenCard(s));

// ════════════════════════════════════════════════════════════════════════════
// 02 — PAINEL DO MEMBRO
// ════════════════════════════════════════════════════════════════════════════
sectionDivider('02', 'Painel do Membro', '/agencia/painel — Empreendedor autenticado');
heading1('02 · Painel do Membro (Empreendedor)');
body('Área autenticada para empreendedor (perfil "E") e comerciante (perfil "C"). Layout agencia-painel.vue: header fixo com logo branca + saldo + ações; menu lateral AgenciaMenu.vue. Middleware: agencia-auth. Dados carregados via API .NET 8 em :8000.');
doc.moveDown(0.4);

[
  { nome: 'Painel Geral (Dashboard)', rota: '/agencia/painel',
    proposito: 'Tela inicial pós-login. Indicadores resumidos, vídeos de onboarding, campanha Quanta Amizade, mapa de lojas e widget Meta Minuto.',
    componentes: ['4 KPI cards (Equipe, Saldo, Encaminhamentos, Pontos)', 'Player YouTube (3 vídeos)', 'Banner Quanta Amizade desktop/mobile', 'Mapa Google Maps', 'Widget Meta Minuto com countdown', 'Card WhatsApp', 'Card Quanta Plus'],
    dados: ['Tamanho da rede', 'Saldo (R$)', 'Ecossistemas', 'Quanta Points', 'Valor/minuto', 'Progress meta diária/semanal/mensal'],
    acoes: ['Assistir vídeo', 'Copiar link de indicação (modal)', 'Convidar amigos', 'Credencie estabelecimento', 'Alternar tabs da meta'] },

  { nome: 'Financeiro', rota: '/agencia/painel/financeiro',
    proposito: 'Extrato completo: cashback recebido, saques solicitados, bônus de rede e saldo em quarentena.',
    componentes: ['Cards de saldo (disponível / pendente / bloqueado)', 'Tabela de extrato paginada', 'Filtros por tipo e período', 'Botão solicitar saque'],
    dados: ['Saldo disponível', 'Saldo em quarentena', 'Histórico (tipo, valor, data, status)'],
    acoes: ['Solicitar saque', 'Filtrar extrato por data/tipo', 'Ver comprovante de transação'] },

  { nome: 'Minhas Compras', rota: '/agencia/painel/minhas-compras',
    proposito: 'Histórico de compras em lojas credenciadas com status do cashback gerado por cada compra.',
    componentes: ['Tabela paginada', 'Status badge (pendente/aprovado/cancelado)', 'Filtro por período e loja'],
    dados: ['Data', 'Loja', 'Valor total (R$)', 'Cashback gerado (R$)', 'Status'],
    acoes: ['Filtrar por data/loja', 'Ver detalhes', 'Contestar compra'] },

  { nome: 'Registrar Compra (Cupom)', rota: '/agencia/painel/inserir-cupom',
    proposito: 'Formulário para registrar compra em loja física usando o cupom ou QR code da loja.',
    componentes: ['Campo CNPJ da loja', 'Campo valor da compra', 'Scanner QR Code (mobile)', 'Confirmação da operação'],
    dados: ['CNPJ ou ID da loja', 'Valor da compra (R$)', 'Data/hora'],
    acoes: ['Digitar ou escanear cupom', 'Confirmar valor', 'Submeter compra'] },

  { nome: 'Minha Rede', rota: '/agencia/painel/minha-rede',
    proposito: 'Visualização da rede de indicados em estrutura de árvore MLM com estatísticas por nível.',
    componentes: ['Árvore hierárquica visual', 'Card por membro (nome, nível, status)', 'KPIs por nível (1º, 2º, 3º…)'],
    dados: ['Nome/login dos indicados', 'Nível na rede', 'Status ativo/inativo', 'Cashback gerado por cada um'],
    acoes: ['Expandir/recolher nó', 'Ver perfil do indicado', 'Filtrar por nível'] },

  { nome: 'Meus Diretos', rota: '/agencia/painel/meus-diretos',
    proposito: 'Lista somente os indicados de primeiro nível (diretos) com status de ativação.',
    componentes: ['Tabela de diretos', 'Status de ativação badge', 'Data de entrada'],
    dados: ['Nome', 'Login', 'Data de cadastro', 'Status', 'Cashback total'],
    acoes: ['Ver detalhes do direto', 'Copiar link personalizado'] },

  { nome: 'Desempenho / Performance', rota: '/agencia/painel/desempenho  ·  /agencia/painel/performance',
    proposito: 'Relatórios de performance individual: cashback gerado, rede ativa, tendência mensal e ranking.',
    componentes: ['Gráficos de linha (cashback por mês)', 'Ranking na rede', 'Badges de conquistas', 'KPIs meta vs. realizado'],
    dados: ['Cashback mensal (série histórica)', 'Posição no ranking', 'Meta vs. realizado'],
    acoes: ['Selecionar período', 'Compartilhar resultado', 'Ver metas sugeridas'] },

  { nome: 'Meus Credenciamentos', rota: '/agencia/painel/meus-credenciamentos',
    proposito: 'Lista os estabelecimentos credenciados pelo empreendedor com status de cada aprovação.',
    componentes: ['Tabela de credenciamentos', 'Status badge', 'Botão novo credenciamento'],
    dados: ['Nome do estabelecimento', 'CNPJ', 'Data de envio', 'Status', 'Obs. do admin'],
    acoes: ['Ver detalhes', 'Reenviar documentos', 'Iniciar novo credenciamento'] },

  { nome: 'Assinatura (Quanta Plus)', rota: '/agencia/painel/assinatura',
    proposito: 'Upgrade para premium com cashback em dobro e acesso ao assistente virtual.',
    componentes: ['Cards Grátis vs Plus', 'Comparativo de benefícios', 'Botão contratar', 'FAQ inline'],
    dados: ['Preço do plano', 'Lista de benefícios', 'Status atual de assinatura'],
    acoes: ['Contratar plano', 'Cancelar assinatura', 'Ver histórico de faturas'] },

  { nome: 'Planos', rota: '/agencia/painel/planos',
    proposito: 'Exibe todos os planos disponíveis e permite upgrade/downgrade.',
    componentes: ['Cards de planos', 'Comparativo de features por plano', 'CTA de contratação'],
    dados: ['Nome do plano', 'Preço mensal', 'Features incluídas', 'Plano atual'],
    acoes: ['Escolher plano', 'Contratar', 'Ver detalhes do plano'] },

  { nome: 'Cupons', rota: '/agencia/painel/cupons  ·  /agencia/painel/meus-cupons',
    proposito: 'Lista e gerencia cupons de desconto disponíveis para uso em compras credenciadas.',
    componentes: ['Grid de cupons disponíveis', 'Cupons utilizados', 'Badge de validade'],
    dados: ['Código do cupom', 'Desconto (%)', 'Loja aplicável', 'Validade', 'Status'],
    acoes: ['Copiar código', 'Ver cupons utilizados', 'Gerar cupom (se habilitado)'] },

  { nome: 'Gerar Cupons', rota: '/agencia/painel/gerar-cupons',
    proposito: 'Permite ao membro gerar cupons personalizados para indicar lojas ou produtos.',
    componentes: ['Seletor de loja/produto', 'Configuração do desconto', 'Preview do cupom gerado'],
    dados: ['Lojas disponíveis', '% desconto configurável'],
    acoes: ['Configurar cupom', 'Gerar e copiar', 'Compartilhar WhatsApp/Instagram'] },

  { nome: 'Contas Bancárias', rota: '/agencia/painel/contas-bancarias',
    proposito: 'Cadastro e gerenciamento de contas bancárias/PIX para saque do saldo.',
    componentes: ['Formulário de conta (banco, agência, conta, tipo)', 'Lista de contas salvas', 'Conta padrão badge'],
    dados: ['Banco', 'Agência', 'Conta', 'Tipo (corrente/poupança)', 'Chave PIX'],
    acoes: ['Adicionar conta', 'Remover conta', 'Definir como padrão', 'Adicionar PIX'] },

  { nome: 'Produtos', rota: '/agencia/painel/produtos',
    proposito: 'Catálogo de produtos com cashback especial disponíveis na plataforma.',
    componentes: ['Grid de produtos', 'Filtro por categoria', 'Card com % cashback destacado'],
    dados: ['Nome', 'Preço', 'Cashback %', 'Loja vinculada', 'Estoque'],
    acoes: ['Ver produto', 'Comprar com cashback', 'Adicionar aos favoritos'] },

  { nome: 'Suporte', rota: '/agencia/painel/suporte',
    proposito: 'Lista de tickets de suporte abertos pelo membro com histórico de respostas.',
    componentes: ['Lista de tickets', 'Status badge', 'Botão abrir novo ticket'],
    dados: ['Número', 'Assunto', 'Data', 'Status', 'Última resposta'],
    acoes: ['Abrir novo ticket', 'Responder', 'Fechar ticket', 'Avaliar atendimento'] },

  { nome: 'Solicitar Suporte', rota: '/agencia/painel/solicitar-suporte',
    proposito: 'Formulário de abertura de ticket com categorização do problema.',
    componentes: ['Seletor de categoria', 'Campo assunto', 'Campo descrição', 'Upload de anexos'],
    dados: ['Categorias disponíveis', 'Prioridade'],
    acoes: ['Preencher formulário', 'Anexar evidências', 'Enviar ticket'] },

  { nome: 'Meus Dados', rota: '/agencia/painel/meus-dados',
    proposito: 'Perfil do membro com edição de dados pessoais, foto, senha e notificações.',
    componentes: ['Avatar com upload', 'Formulário de dados', 'Seção de alteração de senha', 'Preferências de notificação'],
    dados: ['Nome', 'CPF', 'Email', 'Telefone', 'Endereço', 'Data de nascimento'],
    acoes: ['Editar dados', 'Alterar senha', 'Atualizar foto', 'Gerenciar notificações'] },

  { nome: 'Material de Apoio', rota: '/agencia/painel/material-apoio',
    proposito: 'Biblioteca de materiais de marketing para divulgação da Quanta Shop.',
    componentes: ['Grid de materiais (banners, flyers, vídeos)', 'Filtro por tipo/formato', 'Botão download'],
    dados: ['Nome', 'Tipo (imagem/vídeo/PDF)', 'Tamanho do arquivo', 'Data de criação'],
    acoes: ['Baixar material', 'Preview', 'Compartilhar link'] },

  { nome: 'Tutoriais', rota: '/agencia/painel/tutoriais-usuario',
    proposito: 'Vídeos tutoriais sobre como usar a plataforma e maximizar ganhos.',
    componentes: ['Grid de vídeos', 'Player YouTube embutido', 'Barra de progresso de conclusão'],
    dados: ['Título', 'Duração', 'Categoria', '% assistido'],
    acoes: ['Assistir tutorial', 'Marcar como concluído', 'Compartilhar'] },

  { nome: 'FAQ do Membro', rota: '/agencia/painel/faq',
    proposito: 'FAQ específico para membros com dúvidas sobre saque, rede e cashback.',
    componentes: ['Accordion de perguntas', 'Buscador', 'Link para suporte'],
    dados: ['Perguntas/respostas contextuais ao perfil do membro'],
    acoes: ['Expandir pergunta', 'Buscar', 'Abrir ticket de suporte'] },

  { nome: 'Painel do Comerciante', rota: '/agencia/painel/comerciante',
    proposito: 'Dashboard para o perfil Comerciante: vendas, cashback distribuído, relatórios e QR Code.',
    componentes: ['KPIs de vendas', 'Gráfico cashback distribuído', 'Tabela de compras recentes', 'QR Code da loja'],
    dados: ['Total de vendas', 'Cashback total distribuído', 'Clientes únicos', 'Ticket médio'],
    acoes: ['Ver relatório de vendas', 'Imprimir QR Code', 'Gerar cupom de loja'] },

  { nome: 'Graduações', rota: '/agencia/painel/graduacoes',
    proposito: 'Progresso nas graduações/ranks do plano MLM e metas para avançar ao próximo nível.',
    componentes: ['Linha do tempo de graduações', 'Badge do rank atual', 'Metas pendentes para avançar'],
    dados: ['Rank atual', 'Rede ativa exigida', 'Cashback gerado exigido', 'Histórico de graduações'],
    acoes: ['Ver requisitos de cada nível', 'Compartilhar graduação alcançada'] },
].forEach(s => screenCard(s));

// ════════════════════════════════════════════════════════════════════════════
// 03 — PAINEL ADMIN
// ════════════════════════════════════════════════════════════════════════════
sectionDivider('03', 'Painel Administrativo', '/agencia/painel/admin — Acesso restrito a administradores');
heading1('03 · Painel Administrativo');
body('Área restrita aos administradores. Mesmo layout do painel, com middleware agencia-auth + agencia-admin. Comunica com /Admin/* na API .NET 8. Inclui 25 telas especializadas para operação total da plataforma.');
doc.moveDown(0.4);

[
  { nome: 'Painel Admin (Home)', rota: '/agencia/painel/admin',
    proposito: 'Dashboard central com KPIs globais e acesso rápido a todas as seções administrativas.',
    componentes: ['4 KPI cards (usuários, compras pendentes, saques pendentes, tickets)', 'Grid de links para as 20+ seções admin'],
    dados: ['Total de usuários', 'Compras pendentes', 'Saques pendentes', 'Tickets abertos'],
    acoes: ['Navegar para seção', 'Ver alertas críticos'] },

  { nome: 'Gerenciar Usuários', rota: '/agencia/painel/admin/usuarios',
    proposito: 'Lista completa de membros com filtros avançados, busca e ações administrativas.',
    componentes: ['Tabela paginada de usuários', 'Filtros (perfil, status, data)', 'Buscador nome/CPF/email', 'Menu de ações (editar, bloquear, acesso remoto)'],
    dados: ['Nome', 'CPF', 'Email', 'Perfil', 'Status', 'Data de cadastro', 'Saldo'],
    acoes: ['Buscar usuário', 'Editar dados', 'Bloquear/desbloquear', 'Acesso remoto', 'Ver rede'] },

  { nome: 'Alterar Dados de Usuário', rota: '/agencia/painel/admin/alterar-dados-usuario',
    proposito: 'Formulário para o admin editar qualquer campo de um usuário específico.',
    componentes: ['Buscador de usuário', 'Formulário de edição completo', 'Histórico de alterações'],
    dados: ['Todos os campos do perfil do usuário'],
    acoes: ['Buscar usuário', 'Editar campos', 'Salvar', 'Ver histórico de alterações'] },

  { nome: 'Pagamentos / Saques', rota: '/agencia/painel/admin/pagamentos',
    proposito: 'Fila de aprovação dos saques solicitados pelos membros com dados bancários.',
    componentes: ['Fila de saques pendentes', 'Detalhes bancários do solicitante', 'Botões aprovar/reprovar/devolver'],
    dados: ['Nome', 'Valor solicitado', 'Conta bancária/PIX', 'Data da solicitação', 'Status'],
    acoes: ['Aprovar saque', 'Reprovar com motivo', 'Processar em lote', 'Exportar para banco'] },

  { nome: 'Compras Admin', rota: '/agencia/painel/admin/compras',
    proposito: 'Listagem e aprovação de todas as compras registradas nas lojas credenciadas.',
    componentes: ['Tabela de compras', 'Filtros (status, loja, período)', 'Ações por linha'],
    dados: ['Membro', 'Loja', 'Valor', 'Cashback calculado', 'Status', 'Data'],
    acoes: ['Aprovar compra', 'Cancelar compra', 'Estornar cashback', 'Ver comprovante'] },

  { nome: 'Credenciamento Admin', rota: '/agencia/painel/admin/credenciamento',
    proposito: 'Análise e aprovação de novos estabelecimentos para ingresso na rede.',
    componentes: ['Fila de credenciamentos pendentes', 'Documentos enviados', 'Formulário de aprovação'],
    dados: ['CNPJ', 'Razão social', 'Responsável', 'Documentos', 'Empreendedor indicante'],
    acoes: ['Aprovar', 'Reprovar com obs.', 'Solicitar mais documentos', 'Configurar % cashback'] },

  { nome: 'Categorias', rota: '/agencia/painel/admin/categorias',
    proposito: 'CRUD completo de categorias de lojas/produtos usadas em todo o sistema.',
    componentes: ['Tabela de categorias', 'Formulário inline de edição', 'Upload de ícone'],
    dados: ['Nome', 'Slug', 'Ícone', 'Status ativo/inativo'],
    acoes: ['Criar categoria', 'Editar', 'Desativar', 'Reordenar'] },

  { nome: 'Ecossistemas', rota: '/agencia/painel/admin/ecossistemas',
    proposito: 'Gerencia grupos de lojas/serviços integrados (ecossistemas) com vínculos entre eles.',
    componentes: ['Lista de ecossistemas', 'Formulário de cadastro', 'Lojas vinculadas ao ecossistema'],
    dados: ['Nome', 'Descrição', 'Lojas vinculadas', 'Status'],
    acoes: ['Criar ecossistema', 'Editar', 'Vincular/desvincular lojas', 'Ativar/desativar'] },

  { nome: 'Carrosséis / Banners', rota: '/agencia/painel/admin/carrosseis',
    proposito: 'Gerencia os banners rotativos da home e portal com upload de imagem e link destino.',
    componentes: ['Lista de banners com preview', 'Upload desktop/mobile', 'Campo link de destino', 'Ordenação drag-and-drop'],
    dados: ['Imagem desktop', 'Imagem mobile', 'Link', 'Ordem', 'Status', 'Período de exibição'],
    acoes: ['Upload de banner', 'Editar link', 'Reordenar', 'Ativar/desativar', 'Definir período'] },

  { nome: 'Comunicados', rota: '/agencia/painel/admin/comunicados',
    proposito: 'Envio de comunicados e notificações push para todos os membros ou segmentos.',
    componentes: ['Editor rich text', 'Seletor de audiência', 'Agendamento', 'Histórico de enviados'],
    dados: ['Título', 'Conteúdo HTML', 'Público-alvo', 'Data de envio', 'Status'],
    acoes: ['Criar comunicado', 'Selecionar audiência', 'Agendar', 'Enviar agora', 'Ver métricas'] },

  { nome: 'Rede (Admin)', rota: '/agencia/painel/admin/rede',
    proposito: 'Visualização global da rede MLM com métricas por nível e identificação de nós críticos.',
    componentes: ['Árvore de rede interativa', 'Estatísticas globais por nível', 'Filtro por usuário raiz'],
    dados: ['Total por nível', 'Membros ativos/inativos', 'Cashback gerado por nível'],
    acoes: ['Expandir nó', 'Filtrar por usuário', 'Exportar relatório de rede'] },

  { nome: 'Suporte Admin', rota: '/agencia/painel/admin/suporte',
    proposito: 'Painel de atendimento de tickets de suporte com fila por status e chat integrado.',
    componentes: ['Fila de tickets por status', 'Chat de atendimento', 'Atribuição a atendente', 'Respostas prontas'],
    dados: ['Número', 'Membro', 'Assunto', 'Prioridade', 'Atendente', 'Tempo aberto'],
    acoes: ['Responder', 'Atribuir atendente', 'Escalar prioridade', 'Fechar ticket'] },

  { nome: 'Lojas Físicas Admin', rota: '/agencia/painel/admin/lojas-credenciados',
    proposito: 'CRUD das lojas físicas credenciadas exibidas no mapa público.',
    componentes: ['Tabela de lojas', 'Formulário de cadastro/edição', 'Preview no mapa'],
    dados: ['Nome', 'CNPJ', 'Endereço', 'Geolocalização', 'Categoria', '% cashback', 'Foto'],
    acoes: ['Adicionar loja', 'Editar dados', 'Definir localização no mapa', 'Ativar/desativar'] },

  { nome: 'Relatório de Faturas', rota: '/agencia/painel/admin/relatorio-de-faturas',
    proposito: 'Relatórios financeiros globais com faturamento, cashback distribuído e comparativos.',
    componentes: ['Filtros de período', 'Gráficos de faturamento', 'Tabela exportável', 'Resumo por categoria'],
    dados: ['Faturamento bruto', 'Cashback distribuído', 'Inadimplência', 'Margem por período'],
    acoes: ['Filtrar por período', 'Exportar CSV/PDF', 'Ver por categoria'] },

  { nome: 'BI Financeiro', rota: '/agencia/painel/admin/bi-financeiro',
    proposito: 'Business Intelligence: cashback reservado por tipo, inadimplência e faturamento por categoria.',
    componentes: ['KPIs: cashback gerado, CB reservado, taxa inadimplência', 'Gráfico por categoria', 'Top 10 credenciados por volume'],
    dados: ['Total CB+DTCSH gerado', 'CB reservado disponível', 'Taxa inadimplência (%)', 'Faturamento por categoria'],
    acoes: ['Filtrar por período', 'Drill-down por categoria', 'Exportar relatório'] },

  { nome: 'Configurações de Rede', rota: '/agencia/painel/admin/configuracoes-rede',
    proposito: 'Configura percentuais por nível MLM, ativação de camadas, Quanta Points e dias de quarentena.',
    componentes: ['Formulário percentuais por nível (1º ao 10º)', 'Toggle ativação por camada', 'Campo quarentena (dias)', 'Config Quanta Points'],
    dados: ['Percentual por nível', 'Camadas ativas', 'Dias de quarentena', 'Razão de conversão QP'],
    acoes: ['Editar percentuais', 'Ativar/desativar camadas', 'Salvar configuração'] },

  { nome: 'Home CMS', rota: '/agencia/painel/admin/home-cms',
    proposito: 'Editor das seções estáticas da Home pública sem necessidade de deploy.',
    componentes: ['Campos editáveis por seção', 'Preview em tempo real', 'Histórico de versões'],
    dados: ['Textos de cada seção', 'Imagens', 'Links de CTA', 'Status publicado/rascunho'],
    acoes: ['Editar texto/imagem', 'Preview', 'Publicar', 'Reverter versão'] },

  { nome: 'Marcas da Home', rota: '/agencia/painel/admin/marcas-home',
    proposito: 'Gerencia logos das marcas parceiras no carrossel de marcas da home.',
    componentes: ['Grid de logos', 'Upload SVG/PNG', 'Ordenação', 'Campo link da marca'],
    dados: ['Logo (imagem)', 'Nome da marca', 'Link externo', 'Ordem de exibição'],
    acoes: ['Adicionar logo', 'Reordenar', 'Editar link', 'Remover'] },

  { nome: 'Blog Admin', rota: '/agencia/painel/admin/blog',
    proposito: 'CRUD de artigos do blog com editor rico e campos de SEO configuráveis.',
    componentes: ['Lista de artigos', 'Editor rich text', 'Campo SEO (título, meta, slug)', 'Upload thumbnail'],
    dados: ['Título', 'Conteúdo HTML', 'Slug', 'Thumbnail', 'Autor', 'Data', 'Status'],
    acoes: ['Criar artigo', 'Editar', 'Publicar/despublicar', 'Deletar', 'Preview'] },

  { nome: 'Redes Sociais Admin', rota: '/agencia/painel/admin/redes-sociais',
    proposito: 'Gerencia posts de Instagram, YouTube e TikTok da seção de redes sociais da home.',
    componentes: ['Grid de posts por rede', 'Formulário de adição (URL + thumbnail)', 'Ordenação'],
    dados: ['URL do post', 'Thumbnail', 'Tipo de rede', 'Data', 'Likes'],
    acoes: ['Adicionar post', 'Remover', 'Reordenar', 'Ativar/desativar seção'] },

  { nome: 'Relatório de Cashback', rota: '/agencia/painel/admin/relatorio-cashback',
    proposito: 'Relatório detalhado de cashback distribuído por usuário, loja e período.',
    componentes: ['Filtros avançados', 'Tabela detalhada', 'Totalizadores por coluna', 'Exportação'],
    dados: ['Usuário', 'Loja', 'Valor compra', 'CB%', 'CB (R$)', 'Data', 'Status'],
    acoes: ['Filtrar', 'Ordenar por coluna', 'Exportar CSV'] },

  { nome: 'Assinaturas Admin', rota: '/agencia/painel/admin/assinaturas',
    proposito: 'Gerencia assinaturas Quanta Plus ativas, canceladas e em atraso.',
    componentes: ['Tabela de assinaturas', 'Filtros por status/plano', 'Ações de cancelamento/reativação'],
    dados: ['Membro', 'Plano', 'Data de início', 'Valor', 'Status', 'Próxima cobrança'],
    acoes: ['Cancelar assinatura', 'Reativar', 'Desconto manual', 'Ver histórico de pagamentos'] },

  { nome: 'Relatório de Anunciantes', rota: '/agencia/painel/admin/relatorio-de-anunciantes',
    proposito: 'Performance dos anunciantes/parceiros: cliques, conversões e receita gerada.',
    componentes: ['Tabela de anunciantes com KPIs', 'Gráfico de performance', 'Exportação'],
    dados: ['Nome do anunciante', 'Cliques', 'Conversões', 'Receita gerada (R$)', 'ROI'],
    acoes: ['Filtrar por período', 'Exportar', 'Ver detalhes por anunciante'] },

  { nome: 'Acessos (Log de Segurança)', rota: '/agencia/painel/admin/acessos',
    proposito: 'Auditoria de acessos remotos feitos pelo admin em contas de usuários.',
    componentes: ['Tabela de log de acessos', 'Filtros por data/admin/usuário'],
    dados: ['Admin que acessou', 'Usuário acessado', 'IP', 'Data/hora', 'Ações realizadas'],
    acoes: ['Filtrar log', 'Exportar log de auditoria'] },

  { nome: 'Documentação Técnica', rota: '/agencia/painel/admin/docs',
    proposito: 'Visualizador dos documentos técnicos do projeto com botão Baixar PDF.',
    componentes: ['Lista de 6 documentos', 'Visualizador de Markdown renderizado', 'Botão Baixar PDF (always visible)'],
    dados: ['CLAUDE.md', 'replit.md', 'DATA_DICTIONARY.md', 'DESIGN_SYSTEM.md', 'FEATURES.md', 'CHANGELOG.md'],
    acoes: ['Selecionar documento', 'Baixar como PDF', 'Imprimir'] },
].forEach(s => screenCard(s));

// ════════════════════════════════════════════════════════════════════════════
// 04 — LOJA PÚBLICA
// ════════════════════════════════════════════════════════════════════════════
sectionDivider('04', 'Loja Pública', '/ — E-commerce com cashback para consumidores finais');
heading1('04 · Loja Pública (E-commerce)');
body('Ambiente de e-commerce acessível ao público geral e a membros autenticados. Layout default.vue com HeaderTwo e FooterOne. Integra AWIN para rastreamento de afiliados. Cashback calculado automaticamente para membros logados.');
doc.moveDown(0.4);

[
  { nome: 'Home da Loja', rota: '/',
    proposito: 'Página inicial do e-commerce com todas as seções de conversão: hero, marcas, ofertas, parceiros, depoimentos, blog e CTA final.',
    componentes: ['Hero com buscador e CTA', 'Carrossel de marcas parceiras', 'Grid de ofertas do dia', 'Seção parceiros online', 'Seção parceiros locais', 'Carrossel de depoimentos', 'Preview do blog', 'Footer CTA de cadastro'],
    dados: ['Ofertas (produto, preço, cashback)', 'Parceiros (logo, nome, % cashback)', 'Depoimentos (nome, foto, texto)', 'Posts do blog (título, thumbnail, data)'],
    acoes: ['Buscar produto', 'Clicar em oferta', 'Acessar parceiro', 'Ler artigo do blog', 'Cadastrar-se'] },

  { nome: 'Loja (Shop)', rota: '/shop',
    proposito: 'Listagem de produtos com filtros por categoria, preço, marca e cashback.',
    componentes: ['Sidebar de filtros', 'Grid de produtos', 'Paginação', 'Ordenação (preço/relevância/cashback)', 'Badge cashback por produto'],
    dados: ['Produtos (nome, preço, foto, cashback%)', 'Filtros disponíveis', 'Total de resultados'],
    acoes: ['Aplicar filtros', 'Ordenar', 'Adicionar ao carrinho', 'Adicionar à wishlist'] },

  { nome: 'Detalhe do Produto', rota: '/product-details/[id]',
    proposito: 'Página completa do produto com galeria, variações, cashback em destaque e avaliações.',
    componentes: ['Galeria de fotos', 'Seletor de variações (cor/tamanho)', 'Badge cashback em destaque', 'Botão adicionar ao carrinho', 'Avaliações de clientes', 'Produtos relacionados'],
    dados: ['Nome', 'Preço', 'Fotos', 'Variações', 'Estoque', 'Cashback (R$ e %)', 'Avaliações (nota, texto, autor)'],
    acoes: ['Selecionar variação', 'Adicionar ao carrinho', 'Comprar agora', 'Adicionar à wishlist', 'Compartilhar'] },

  { nome: 'Parceiros', rota: '/partners',
    proposito: 'Diretório de todas as marcas e parceiros com cashback, filtrado por categoria.',
    componentes: ['Grid de cards de parceiros', 'Filtro por categoria', 'Buscador por nome', 'Badge cashback por parceiro'],
    dados: ['Logo', 'Nome', '% cashback', 'Categoria', 'Tipo (online/físico)'],
    acoes: ['Buscar parceiro', 'Filtrar por categoria', 'Acessar loja do parceiro', 'Ver condições'] },

  { nome: 'Carrinho', rota: '/cart',
    proposito: 'Resumo dos itens com cálculo de total, cashback acumulado e campos de cupom.',
    componentes: ['Lista de itens', 'Campo de cupom', 'Resumo de valores (subtotal, frete, cashback, total)', 'Botão finalizar compra'],
    dados: ['Itens (produto, qtd, preço, foto)', 'Valor subtotal', 'Frete estimado', 'Cashback total estimado'],
    acoes: ['Alterar quantidade', 'Remover item', 'Aplicar cupom', 'Ir para checkout', 'Continuar comprando'] },

  { nome: 'Checkout', rota: '/checkout',
    proposito: 'Fluxo de finalização de compra: endereço, pagamento e confirmação.',
    componentes: ['Step 1: Dados do comprador', 'Step 2: Endereço de entrega', 'Step 3: Forma de pagamento', 'Step 4: Revisão + confirmação', 'Resumo do pedido fixo lateral'],
    dados: ['Dados pessoais', 'CEP/endereço', 'Opções de frete', 'Métodos de pagamento (cartão/PIX/boleto)', 'Cashback a receber'],
    acoes: ['Preencher endereço', 'Selecionar frete', 'Selecionar pagamento', 'Confirmar pedido'] },

  { nome: 'Pedidos (Order)', rota: '/order',
    proposito: 'Histórico de pedidos do cliente com status e rastreamento de entrega.',
    componentes: ['Lista de pedidos paginada', 'Status badge', 'Link de rastreamento'],
    dados: ['Número do pedido', 'Data', 'Itens', 'Total', 'Status', 'Cashback recebido'],
    acoes: ['Ver detalhes do pedido', 'Rastrear entrega', 'Solicitar devolução'] },

  { nome: 'Conta do Cliente', rota: '/profile',
    proposito: 'Área de conta do cliente da loja: dados pessoais, endereços salvos, histórico de pedidos e cashback acumulado.',
    componentes: ['Tabs: Meus Dados / Endereços / Pedidos / Cashback', 'Formulário de dados pessoais editável', 'Lista de endereços cadastrados', 'Histórico de pedidos com status', 'Saldo de cashback da loja'],
    dados: ['Nome', 'Email', 'Telefone', 'Endereços salvos', 'Pedidos (número, data, total, status)', 'Saldo de cashback acumulado (R$)'],
    acoes: ['Editar dados pessoais', 'Adicionar/remover endereço', 'Ver detalhe do pedido', 'Solicitar devolução', 'Ver cashback disponível'] },

  { nome: 'Blog', rota: '/blog',
    proposito: 'Blog editorial com artigos sobre economia, cashback e dicas de compra.',
    componentes: ['Grid de artigos', 'Destaque do artigo mais recente', 'Paginação', 'Buscador'],
    dados: ['Artigos (título, thumbnail, autor, data, categoria, resumo)'],
    acoes: ['Ler artigo', 'Buscar por palavra-chave', 'Filtrar por categoria', 'Compartilhar'] },
].forEach(s => screenCard(s));

// ════════════════════════════════════════════════════════════════════════════
// 05 — FLUXOS
// ════════════════════════════════════════════════════════════════════════════
sectionDivider('05', 'Fluxos de Navegação', 'Jornada por perfil de usuário');
heading1('05 · Fluxos de Navegação por Perfil');

[
  { nome: 'Visitante (não autenticado)', cor: GRAY,
    jornada: [
      'Acessa / (Home da Loja) ou /agencia (Portal Público)',
      'Explora parceiros, produtos e benefícios',
      'Lê "Como Funciona" e "Quem Somos"',
      'Decide se cadastrar → /agencia/cadastro (com slug de indicação na URL)',
      'Confirma email → /agencia/confirm-email/[token]',
      'Faz login → /agencia/login → redirecionado ao Painel',
    ],
    acesso: 'Todas as páginas públicas: /, /agencia, /shop, /blog, /partners, /agencia/como-funciona, /agencia/faq, /agencia/lojas-fisicas, /agencia/quem-somos, /agencia/privacidade.' },
  { nome: 'Empreendedor (perfil E)', cor: TEAL,
    jornada: [
      'Login em /agencia/login → JWT salvo no localStorage',
      'Redirecionado para /agencia/painel (Dashboard com KPIs e Meta Minuto)',
      'Acessa /agencia/painel/financeiro para ver extrato e solicitar saque',
      'Acessa /agencia/painel/minha-rede para ver e gerir indicados',
      'Credencia novos parceiros em /agencia/painel/meus-credenciamentos',
      'Registra compras em lojas físicas em /agencia/painel/inserir-cupom',
      'Acompanha performance e graduações',
    ],
    acesso: 'Todo o /agencia/painel/* exceto /agencia/painel/admin/*' },
  { nome: 'Comerciante (perfil C)', cor: AMBER,
    jornada: [
      'Login em /agencia/login → redirecionado ao Painel Comerciante',
      'Acessa /agencia/painel/comerciante (dashboard de vendas)',
      'Acompanha cashback distribuído para seus clientes',
      'Consulta relatório de compras realizadas na loja',
      'Gera QR Code para uso em caixa físico',
      'Solicita saque em /agencia/painel/financeiro',
    ],
    acesso: '/agencia/painel/comerciante, /agencia/painel/financeiro, /agencia/painel/meus-dados' },
  { nome: 'Administrador (perfil Admin)', cor: RED,
    jornada: [
      'Login em /agencia/login → redirecionado para /agencia/painel/admin',
      'Vê KPIs globais: usuários, compras, saques pendentes, suporte',
      'Aprova saques em /agencia/painel/admin/pagamentos',
      'Aprova compras em /agencia/painel/admin/compras',
      'Analisa e aprova credenciamentos em /agencia/painel/admin/credenciamento',
      'Gerencia conteúdo: banners, blog, comunicados, redes sociais',
      'Configura BI Financeiro e regras de Rede MLM',
      'Faz acesso remoto em contas de membros com log de auditoria',
    ],
    acesso: 'Todo o /agencia/painel/* incluindo /agencia/painel/admin/*' },
].forEach(p => {
  if (doc.y > 580) addPage();
  doc.moveDown(0.4);
  const ty = doc.y;
  doc.save().rect(50, ty, PW, 16).fill(p.cor).restore();
  doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(11)
     .text(`Perfil: ${p.nome}`, 58, ty + 2, { width: PW - 16 }).restore();
  doc.moveDown(1.5);
  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(9).text('Jornada principal:', 50, doc.y).restore();
  doc.moveDown(0.4);
  p.jornada.forEach((step, i) => bullet(`${i + 1}. ${step}`));
  doc.moveDown(0.3);
  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(9).text('Rotas com acesso:', 50, doc.y).restore();
  doc.moveDown(0.3);
  doc.save().fillColor(GRAY).font('Helvetica').fontSize(8.5)
     .text(p.acesso, 62, doc.y, { width: PW - 12 }).restore();
  doc.moveDown(0.8);
  hRule();
});

// ════════════════════════════════════════════════════════════════════════════
// 06 — COMPONENTES REUTILIZÁVEIS
// ════════════════════════════════════════════════════════════════════════════
sectionDivider('06', 'Componentes Reutilizáveis', 'Design System base para o Lovable');
heading1('06 · Componentes Reutilizáveis (Design System)');
body('Crie estes componentes primeiro no Lovable, antes de qualquer tela. Eles aparecem em múltiplas telas e garantem consistência visual. Cada componente deve funcionar de forma isolada e receber dados via props.');
doc.moveDown(0.4);

[
  { nome: 'AgenciaMenu — Menu Lateral', onde: 'Todas as telas /agencia/painel/*',
    desc: 'Menu lateral com links de navegação. Seção Admin condicional (isAdmin). Fundo #2f7785, links brancos com hover verde. Mobile: colapsável com hamburguer.',
    variacoes: ['Com seção Admin visível', 'Sem Admin (empreendedor comum)', 'Mobile colapsável'],
    props: ['isAdmin: boolean', 'isComerciante: boolean', 'currentRoute: string'] },
  { nome: 'Header do Painel', onde: 'Todas as telas /agencia/painel/*',
    desc: 'Header fixo: logo branca + badge "Acessando como Empreendedor/Credenciado" + avatar + nome + saldo em BRL + botões (Assistente Virtual / Meus Dados / Logout). Badge especial para Acesso Remoto.',
    variacoes: ['Com badge Acesso Remoto', 'Com badge Assinante Plus', 'Mobile compacto'],
    props: ['user: object', 'saldo: number', 'isAcessoRemoto: boolean', 'isAssinante: boolean'] },
  { nome: 'KPI Card (Card de Saldo)', onde: 'Dashboard, Admin, BI Financeiro',
    desc: 'Card com ícone, label e valor em destaque. Variações de cor: verde (positivo), vermelho (risco), teal (neutro). Valor formatado em BRL ou número/percentual.',
    variacoes: ['Saldo disponível (verde)', 'Taxa inadimplência (vermelho)', 'Total usuários (teal)'],
    props: ['label: string', 'valor: string|number', 'tipo: "moeda"|"numero"|"percent"', 'cor: "green"|"red"|"teal"'] },
  { nome: 'Tabela Paginada', onde: 'Compras, Financeiro, Usuários Admin, Relatórios',
    desc: 'Tabela responsiva com colunas configuráveis, paginação server-side, filtros e exportação CSV.',
    variacoes: ['Com ações por linha (editar/excluir)', 'Somente leitura', 'Com seleção múltipla para lote'],
    props: ['columns: array', 'data: array', 'totalPages: number', 'onPageChange: function'] },
  { nome: 'Status Badge', onde: 'Compras, Saques, Credenciamentos, Suporte',
    desc: 'Badge colorido de status. Verde=aprovado/ativo, Amarelo=pendente, Vermelho=cancelado/reprovado, Azul=em andamento.',
    variacoes: ['Aprovado (verde)', 'Pendente (amarelo)', 'Cancelado (vermelho)', 'Em análise (azul)'],
    props: ['status: string', 'tamanho: "sm"|"md"'] },
  { nome: 'Header Portal (Agência)', onde: 'Todas as páginas /agencia/* não autenticadas',
    desc: 'Header sticky com logo colorida, links de navegação (Quem Somos, Como Funciona, FAQ, Lojas) e botões CTA (Entrar, Cadastrar).',
    variacoes: ['Desktop horizontal', 'Mobile hamburguer', 'Link ativo destacado'],
    props: ['currentRoute: string'] },
  { nome: 'Card de Produto', onde: 'Shop, Home (ofertas), Produtos do Painel',
    desc: 'Card com thumbnail, nome, preço, badge de desconto, badge de cashback em % e botão "Adicionar ao carrinho".',
    variacoes: ['Grid padrão', 'Lista horizontal', 'Compacto (home)'],
    props: ['produto: object', 'onAddToCart: function', 'showCashback: boolean'] },
  { nome: 'Card de Parceiro', onde: 'Home, /partners, Dashboard',
    desc: 'Card com logo, nome, categoria e % cashback em destaque. Variação para parceiro online e físico.',
    variacoes: ['Online', 'Físico (com endereço)', 'Destaque/featured'],
    props: ['parceiro: object', 'tipo: "online"|"fisico"'] },
  { nome: 'Meta Minuto Widget', onde: 'Dashboard do Membro',
    desc: 'Countdown regressivo até meia-noite, barra de progresso da meta e valor por minuto. Tabs Diário/Semanal/Mensal.',
    variacoes: ['Meta não atingida (com countdown e progress bar)', 'Meta atingida (mensagem de parabéns)'],
    props: ['targetValue: number', 'progressValue: number', 'metaCompleted: boolean'] },
  { nome: 'Link de Indicação', onde: 'Header do Painel, Dashboard',
    desc: 'Campo readonly com o link de indicação personalizado e botão "Copiar link" com feedback visual "Copiado!".',
    variacoes: ['Inline no header', 'Card standalone no dashboard', 'Modal de compartilhamento completo'],
    props: ['linkIndicacao: string'] },
  { nome: 'Modal de Confirmação', onde: 'Aprovações admin, Saques, Cancelamentos',
    desc: 'Modal overlay com título, mensagem, botão confirmar (vermelho se destrutivo, verde se positivo) e botão cancelar.',
    variacoes: ['Destrutivo (vermelho)', 'Confirmação (verde)', 'Com campo de motivo obrigatório'],
    props: ['titulo: string', 'mensagem: string', 'tipo: "destrutivo"|"confirmacao"', 'onConfirm: function'] },
  { nome: 'Loader / Spinner', onde: 'Todas as telas (estado de carregamento)',
    desc: 'Spinner centralizado com overlay semi-transparente enquanto dados carregam. Cor #2f7785.',
    variacoes: ['Full page', 'Inline (dentro de seção)', 'Botão em estado loading'],
    props: ['size: "sm"|"md"|"lg"', 'overlay: boolean'] },
].forEach(c => {
  if (doc.y > 610) addPage();
  doc.moveDown(0.3);
  heading3(c.nome);
  label('Aparece em', c.onde);
  body(c.desc);
  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(8.5).text('Variações:', 50, doc.y).restore();
  doc.moveDown(0.3);
  c.variacoes.forEach(v => bullet(v));
  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(8.5).text('Props principais:', 50, doc.y).restore();
  doc.moveDown(0.3);
  c.props.forEach(p => bullet(p));
  hRule(LGRAY, 0.5);
});

// ════════════════════════════════════════════════════════════════════════════
// 07 — OPORTUNIDADES DE UX
// ════════════════════════════════════════════════════════════════════════════
sectionDivider('07', 'Oportunidades de UX', 'Melhorias visuais e de experiência por ambiente');
heading1('07 · Oportunidades de UX por Ambiente');
body('Principais oportunidades de melhoria de experiência identificadas em cada ambiente. O Lovable deve priorizar estas sugestões ao gerar os componentes visuais.');
doc.moveDown(0.4);

[
  { ambiente: 'Portal Público (/agencia)', melhorias: [
    'Hero animado com contador em tempo real de membros ativos e cashback total distribuído — prova social que converte visitantes.',
    'Calculadora interativa de cashback: visitante informa valor de compra mensal e vê quanto receberia de volta em cada cenário.',
    'Mapa de lojas na landing page com preview estático e CTA "Ver lojas perto de mim" — reduz friction até a ação de cadastro.',
    'Depoimentos em vídeo (carousel com play) em vez de texto puro — maior credibilidade e autenticidade.',
    'Indicador visual de progresso do cadastro multi-step com estimativa de tempo ("2 minutos para concluir").',
  ]},
  { ambiente: 'Painel do Membro', melhorias: [
    'Dashboard personalizável: o membro arrasta e redimensiona os widgets (meta minuto, rede, saldo) conforme preferência.',
    'Notificações in-app com badge no menu lateral para cashback aprovado, nova mensagem de rede e comunicados — sem precisar de email.',
    'Gamificação visual: barra de progresso para próxima graduação e conquistas desbloqueadas com micro-animação.',
    'Histórico financeiro com gráfico de área (cashback acumulado por mês) em vez de somente tabela plana.',
    '"Quick Actions" flutuante (+) para ações frequentes: registrar compra, copiar link, solicitar saque.',
  ]},
  { ambiente: 'Painel Administrativo', melhorias: [
    'Dashboard admin com mapa de calor de atividade por região do Brasil — identifica oportunidades de expansão.',
    'Fila de aprovações unificada (saques + compras + credenciamentos) em uma única tela com filtro por tipo.',
    'Alertas automáticos visuais no topo para saques aguardando >72h ou tickets sem resposta >48h.',
    'Modo de revisão de credenciamento com split-screen: documentos à esquerda, formulário de aprovação à direita.',
    'BI Financeiro com drill-down: clique em um KPI abre tabela detalhada sem mudar de página (modal/painel deslizante).',
  ]},
  { ambiente: 'Loja Pública (E-commerce)', melhorias: [
    'Badge "Cashback X%" em destaque no card do produto — deve ser tão visível quanto o preço para diferenciar da concorrência.',
    'Totalizador de cashback no carrinho atualizado em tempo real enquanto o usuário adiciona ou remove produtos.',
    'Busca inteligente com sugestões de autocomplete (produto, categoria, parceiro) e resultados instantâneos sem recarregar.',
    'Página "Para Você" com produtos recomendados baseados no histórico de compras do membro logado.',
    'Checkout de 1 clique para membros com endereço e pagamento salvos — reduz drasticamente o abandono de carrinho.',
  ]},
].forEach(u => {
  if (doc.y > 600) addPage();
  heading2(u.ambiente);
  u.melhorias.forEach(m => bullet(m));
  doc.moveDown(0.4);
});

// ════════════════════════════════════════════════════════════════════════════
// 08 — INSTRUÇÕES PARA O LOVABLE
// ════════════════════════════════════════════════════════════════════════════
sectionDivider('08', 'Instruções para o Lovable', 'Como usar este documento para gerar as telas');
heading1('08 · Como Usar Este Documento no Lovable');

heading2('8.1 · Processo em 5 Passos');
[
  { n: '1', t: 'Comece pelo Design System',
    d: 'Antes de criar telas individuais, instrua o Lovable a criar os 12 componentes base listados na Seção 06. Isso garante consistência visual em todas as telas geradas depois.' },
  { n: '2', t: 'Defina a paleta de cores',
    d: 'Informe ao Lovable: Primary #2f7785 (teal), Secondary #98c73a (verde), Background #ecf2f7, Text #1a2b33, Danger #e74c3c, Warning #f39c12, White #ffffff. Use estas cores sem variação.' },
  { n: '3', t: 'Crie tela por tela',
    d: 'Para cada tela, copie o prompt modelo da seção 8.2 e preencha com as informações das Seções 01 a 04. Comece pelas mais simples (FAQ, Privacidade) e evolua para as complexas (Dashboard, Admin).' },
  { n: '4', t: 'Itere e refine',
    d: 'Após o Lovable gerar a tela, peça refinamentos específicos: "adicione o badge de cashback em verde", "aumente o contraste do menu lateral", "torne o botão mais prominente".' },
  { n: '5', t: 'Exporte e integre',
    d: 'Quando aprovar o design, exporte o componente e passe para o time de desenvolvimento integrar no projeto Nuxt/Vue no Replit. O código React gerado pelo Lovable será adaptado para Vue.' },
].forEach(s => {
  if (doc.y > 640) addPage();
  const ty = doc.y;
  doc.save().circle(58, ty + 7, 9).fill(TEAL).restore();
  doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(10).text(s.n, 55, ty + 2).restore();
  doc.save().fillColor(TEAL).font('Helvetica-Bold').fontSize(10.5).text(s.t, 76, ty).restore();
  doc.moveDown(0.5);
  doc.save().fillColor(DARK).font('Helvetica').fontSize(9.5).text(s.d, 76, doc.y, { width: PW - 26 }).restore();
  doc.moveDown(0.8);
});

heading2('8.2 · Prompt Modelo para o Lovable');
body('Copie, adapte e cole este prompt no Lovable para cada tela que deseja criar:');
doc.moveDown(0.3);

const PROMPT = `Crie uma tela de [NOME DA TELA] para a plataforma Quanta Shop,
um e-commerce de cashback brasileiro com estrutura MLM.

CONTEXTO DA MARCA:
• Paleta: Teal #2f7785 (primária), Verde #98c73a (secundária/destaque), Fundo #ecf2f7
• Tipografia: Inter ou similar, sans-serif moderna, textos em português brasileiro
• Tom: confiável, moderno, acessível. Não use inglês nos textos visíveis.

ROTA: [ROTA DA TELA — ex: /agencia/painel/financeiro]
PROPÓSITO: [descreva o objetivo da tela em 1-2 frases]

COMPONENTES QUE PRECISO:
- [componente 1 com detalhes visuais]
- [componente 2 com detalhes visuais]

DADOS QUE APARECEM NA TELA:
- [dado 1 com formato — ex: "Saldo disponível em R$ formatado como moeda BRL"]
- [dado 2]

AÇÕES DO USUÁRIO:
- [ação 1 — ex: "Botão Solicitar Saque abre modal de confirmação"]
- [ação 2]

ESTADOS QUE PRECISO:
- Loading: spinner centralizado cor #2f7785 enquanto dados carregam
- Vazio: mensagem amigável em português quando não há dados
- Erro: banner no topo com botão "Tentar novamente"

RESPONSIVIDADE: Mobile first.
Em mobile o menu lateral é colapsável (hamburguer).
Tabelas viram cards empilhados em telas < 768px.

OBSERVAÇÕES:
- Use os componentes do design system já criados
- Textos de exemplo em português brasileiro
- Não inclua dados sensíveis reais nos mocks`;

const pY = doc.y;
doc.save().rect(50, pY, PW, 10).fill(TEAL).restore();
doc.save().fillColor(WHITE).font('Helvetica-Bold').fontSize(8)
   .text('PROMPT MODELO — copie e personalize para cada tela', 56, pY + 1).restore();
doc.moveDown(0.8);
const promptBodyY = doc.y;
const promptH = doc.heightOfString(PROMPT, { width: PW - 20, fontSize: 8 });
doc.save().rect(50, promptBodyY, PW, promptH + 18).fill('#eef6f8').restore();
doc.save().fillColor(DARK).font('Courier').fontSize(7.8)
   .text(PROMPT, 58, promptBodyY + 8, { width: PW - 20 }).restore();
doc.moveDown(1.2);

heading2('8.3 · Ordem Sugerida de Criação');
[
  '1. Design System: 12 componentes base (KPI Card, Tabela, Status Badge, Menu, Header…)',
  '2. Layouts: Portal Público, Painel Logado, Admin, Loja Pública',
  '3. Telas simples: FAQ, Privacidade, Quem Somos, Como Funciona',
  '4. Autenticação: Login, Cadastro, Recuperar Senha',
  '5. Dashboard do Membro — tela mais visitada da plataforma',
  '6. Financeiro, Compras, Minha Rede, Meus Diretos',
  '7. Admin: Index, Usuários, Pagamentos, Compras, Credenciamento',
  '8. BI Financeiro e Configurações de Rede',
  '9. Loja: Home, Shop, Produto, Carrinho, Checkout',
  '10. Blog, Parceiros, Material de Apoio, Tutoriais',
].forEach(o => bullet(o));

heading2('8.4 · Dicas para Melhores Resultados');
[
  'Forneça screenshots de referência visual junto com o prompt para alinhar o estilo desejado.',
  'Crie componentes isolados antes de telas completas — é mais fácil iterar e revisar.',
  'Se o Lovable gerar textos em inglês, adicione ao prompt: "todos os textos visíveis em português brasileiro, use vírgula como separador decimal".',
  'Para variações de design, peça: "gere 3 versões do card de produto com estilos diferentes" e escolha a melhor.',
  'Pequenos ajustes não precisam regenerar a tela: use o modo "continuar editando" do Lovable.',
  'Exporte componente por componente para facilitar a integração incremental no Replit.',
  'Após exportar, avise o time de dev com: qual componente é, qual rota usa, quais dados reais deve exibir (ver Seções 01-04).',
].forEach(d => bullet(d));

// ════════════════════════════════════════════════════════════════════════════
// PAGE NUMBERS
// ════════════════════════════════════════════════════════════════════════════
const range = doc.bufferedPageRange();
for (let i = 0; i < range.count; i++) {
  doc.switchToPage(range.start + i);
  if (i === 0) continue; // skip cover
  doc.save().fillColor(GRAY).font('Helvetica').fontSize(7.5)
     .text(
       `Quanta Shop — Documento de Arquitetura para Lovable — Página ${i + 1} de ${range.count}`,
       50, doc.page.height - 28, { width: PW, align: 'center' }
     ).restore();
}

doc.end();
console.log('PDF gerado:', OUT);
