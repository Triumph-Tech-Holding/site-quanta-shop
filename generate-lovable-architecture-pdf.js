const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({ size: 'A4', margin: 40 });
const filename = path.join(__dirname, 'public', 'arquitetura-lovable.pdf');
doc.pipe(fs.createWriteStream(filename));

const darkBlue = '#1a3a52';
const teal = '#2f7785';
const lightBg = '#ecf2f7';
const green = '#98c73a';

// Helper to add section
function addSection(title, fontSize = 14) {
  doc.moveDown(8);
  doc.fontSize(fontSize).fillColor(darkBlue).text(title, { fontWeight: 'bold' });
  doc.moveDown(4);
}

function addSubsection(title) {
  doc.moveDown(4);
  doc.fontSize(11).fillColor(teal).text(title, { fontWeight: 'bold' });
  doc.moveDown(2);
}

// Título
doc.fontSize(24).fillColor(darkBlue).text('Arquitetura de Telas para Lovable', { align: 'center', fontWeight: 'bold' });
doc.fontSize(12).fillColor(teal).text('Quanta Shop — Frontend Design System', { align: 'center', marginBottom: 5 });
doc.fontSize(9).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')} | Design Brief para Lovable`, { align: 'center', marginBottom: 20 });
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(12);

// ÍNDICE
addSection('Índice', 12);
const indice = [
  '1. Ambientes da Aplicação',
  '2. Fluxo de Navegação por Perfil',
  '3. Portal Público (/agencia)',
  '4. Painel do Membro (/agencia/painel)',
  '5. Painel Admin (/agencia/painel/admin)',
  '6. Loja Pública (/)',
  '7. Componentes Reutilizáveis',
  '8. Oportunidades de UX',
  '9. Guia para Lovable',
];
doc.fontSize(9).fillColor('#333').list(indice);

// PAGE BREAK
doc.addPage();

// AMBIENTES
addSection('1. Ambientes da Aplicação', 14);
doc.fontSize(9).fillColor('#555').text('A aplicação tem 4 ambientes independentes com layouts e navegação diferentes. Cada um serve um público específico.', { lineGap: 2 });

addSubsection('1.1 Portal Público (/agencia)');
doc.fontSize(8.5).fillColor('#333').text('Para visitantes não autenticados. Sem sidebar, header com navegação horizontal simples.', { lineGap: 2 });
doc.text('Telas: Landing page, Quem Somos, Como Funciona, FAQ, Lojas, Login, Cadastro, Recuperar Senha, Parceiro Direto, Mais Vendas');

addSubsection('1.2 Painel do Membro (/agencia/painel)');
doc.fontSize(8.5).fillColor('#333').text('Para empreendedores logados. Sidebar lateral com menu dinâmico (via API), header com nome do usuário e saldo.', { lineGap: 2 });
doc.text('Telas: 20+ páginas para gerenciar compras, rede, cupons, credienciamentos, assinatura, etc.');

addSubsection('1.3 Painel Admin (/agencia/painel/admin)');
doc.fontSize(8.5).fillColor('#333').text('Para administradores. Mesmo layout do painel do membro, mas com menus de usuários, relatórios, financeiro, etc.', { lineGap: 2 });
doc.text('Telas: 20+ páginas para gerenciar plataforma inteira.');

addSubsection('1.4 Loja Pública (/)');
doc.fontSize(8.5).fillColor('#333').text('E-commerce tradicional. Header com busca e carrinho, estrutura de categoria > produto > detalhes.', { lineGap: 2 });
doc.text('Telas: Home, Shop, Produto, Carrinho, Checkout, Conta, Blog, Parceiros');

// PAGE BREAK
doc.addPage();

// FLUXOS
addSection('2. Fluxo de Navegação por Perfil', 14);

addSubsection('Visitante (sem login)');
doc.fontSize(8.5).fillColor('#333').text('Entra em / ou /agencia → Navega loja e portal → Clica "Cadastro" ou "Entrar" → Login/Cadastro → Redirect para /agencia/painel');

addSubsection('Empreendedor (logado)');
doc.fontSize(8.5).fillColor('#333').text('Vê /agencia/painel como dashboard inicial → Menu lateral com: Painel Geral, Financeiro, Compras, Rede, Cupons, etc. → Pode acessar loja em nova aba (link externo a quantashop.com.br)');

addSubsection('Comerciante (logado + credenciado)');
doc.fontSize(8.5).fillColor('#333').text('Painel específico com telas de credenciamento → Pode gerenciar dados da loja credenciada → Menu reduzido comparado ao empreendedor');

addSubsection('Admin');
doc.fontSize(8.5).fillColor('#333').text('Entra em /agencia/painel/admin → Menu completo com: Usuários, Relatórios, Financeiro, Operações, etc. → Pode gerenciar toda a plataforma');

// PAGE BREAK
doc.addPage();

// PORTAL PÚBLICO
addSection('3. Portal Público (/agencia)', 14);

const portalTelas = [
  { nome: 'Landing Page', rota: '/agencia', desc: 'Hero section, videos de tutorial, CTA para cadastro/login' },
  { nome: 'Quem Somos', rota: '/agencia/quem-somos', desc: 'About section, história, time, valores' },
  { nome: 'Como Funciona', rota: '/agencia/como-funciona', desc: 'Explicação do sistema de cashback e rede MLM' },
  { nome: 'FAQ', rota: '/agencia/faq', desc: 'Perguntas frequentes sobre plataforma' },
  { nome: 'Lojas Físicas', rota: '/agencia/lojas-fisicas', desc: 'Mapa de lojas parceiras' },
  { nome: 'Login', rota: '/agencia/login', desc: 'Formulário de login com Google social login' },
  { nome: 'Cadastro', rota: '/agencia/cadastro', desc: 'Formulário de registro com 2 opções: individual ou indicado' },
  { nome: 'Recuperar Senha', rota: '/agencia/recuperar-senha', desc: 'Reset de senha via email' },
  { nome: 'Parceiro Direto', rota: '/agencia/parceiro-direto/[slug]', desc: 'Página publica do parceiro (referral landing page)' },
  { nome: 'Mais Vendas', rota: '/agencia/mais-vendas/[login]', desc: 'Landing page de mais vendidos por afiliado' },
];

portalTelas.forEach(t => {
  addSubsection(t.nome);
  doc.fontSize(8).fillColor('#666').text(`Rota: ${t.rota}`);
  doc.fontSize(8.5).fillColor('#333').text(t.desc, { lineGap: 1 });
  doc.moveDown(1);
});

// PAGE BREAK
doc.addPage();

// PAINEL MEMBRO
addSection('4. Painel do Membro (/agencia/painel)', 14);

const painelTelas = [
  { cat: 'Dashboard', telas: ['Painel Geral'] },
  { cat: 'Financeiro', telas: ['Saldo/Extrato', 'Contas Bancárias', 'Saques', 'Faturas'] },
  { cat: 'Compras', telas: ['Minhas Compras', 'Histórico de Pedidos'] },
  { cat: 'Rede', telas: ['Minha Rede', 'Meus Diretos', 'Árvore de Downline'] },
  { cat: 'Desempenho', telas: ['Desempenho', 'Performance', 'Graduações'] },
  { cat: 'Credenciamentos', telas: ['Meus Credenciamentos', 'Finalizar Credenciamento'] },
  { cat: 'Planos', telas: ['Assinatura', 'Planos', 'Histórico de Assinatura'] },
  { cat: 'Cupons', telas: ['Meus Cupons', 'Inserir Cupom', 'Gerar Cupons'] },
  { cat: 'Produtos', telas: ['Catálogo de Produtos (AWIN)'] },
  { cat: 'Conteúdo', telas: ['Material de Apoio', 'Tutoriais'] },
  { cat: 'Ajuda', telas: ['FAQ do Painel', 'Solicitar Suporte', 'Chat de Suporte'] },
  { cat: 'Perfil', telas: ['Meus Dados', 'Editar Dados'] },
];

painelTelas.forEach(cat => {
  addSubsection(cat.cat);
  doc.fontSize(8.5).fillColor('#333').list(cat.telas);
  doc.moveDown(2);
});

// PAGE BREAK
doc.addPage();

// PAINEL ADMIN
addSection('5. Painel Admin (/agencia/painel/admin)', 14);

const adminTelas = [
  { cat: 'Usuários', telas: ['Listar Usuários', 'Alterar Dados de Usuário', 'Aniversariantes', 'Acessos/Permissões'] },
  { cat: 'Financeiro', telas: ['Pagamentos', 'Lançamentos Manuais', 'Gestão de Saque'] },
  { cat: 'Relatórios', telas: ['Relatório Cashback', 'Relatório Faturas', 'Relatório Anunciantes/Afiliados'] },
  { cat: 'Operações', telas: ['Credenciamento', 'Compras', 'Assinaturas'] },
  { cat: 'Rede', telas: ['Rede Geral', 'Árvore Completa'] },
  { cat: 'Conteúdo', telas: ['Comunicados', 'Carrosséis/Banners', 'Material de Apoio', 'Categorias'] },
  { cat: 'Sistema', telas: ['Ecossistemas', 'Gerenciar Grupos', 'Lojas Credenciadas', 'Suporte'] },
];

adminTelas.forEach(cat => {
  addSubsection(cat.cat);
  doc.fontSize(8.5).fillColor('#333').list(cat.telas);
  doc.moveDown(2);
});

// PAGE BREAK
doc.addPage();

// LOJA PÚBLICA
addSection('6. Loja Pública (/)', 14);

const lojaTelas = [
  { nome: 'Home', rota: '/', desc: 'Hero, categorias, produtos em destaque, reviews' },
  { nome: 'Shop', rota: '/shop', desc: 'Listagem de produtos com filtros e paginação' },
  { nome: 'Detalhes do Produto', rota: '/product-details/[id]', desc: 'Imagens, descrição, preço, adicionar ao carrinho' },
  { nome: 'Busca', rota: '/search', desc: 'Resultados de busca por texto' },
  { nome: 'Carrinho', rota: '/cart', desc: 'Itens no carrinho, editar qtd, remover, total' },
  { nome: 'Checkout', rota: '/checkout', desc: 'Endereço, método de pagamento, confirmação' },
  { nome: 'Pedidos', rota: '/order', desc: 'Histórico de pedidos do usuário' },
  { nome: 'Parceiros', rota: '/partners', desc: 'Listagem de lojas parceiras' },
  { nome: 'Detalhes Parceiro', rota: '/partners/[id]', desc: 'Informações da loja parceira, produtos' },
  { nome: 'Perfil do Usuário', rota: '/profile', desc: 'Dados da conta, endereços, métodos de pagamento' },
  { nome: 'Blog', rota: '/blog', desc: 'Artigos e conteúdo' },
  { nome: 'Contato', rota: '/contact', desc: 'Formulário de contato' },
];

lojaTelas.forEach(t => {
  addSubsection(t.nome);
  doc.fontSize(8).fillColor('#666').text(`Rota: ${t.rota}`);
  doc.fontSize(8.5).fillColor('#333').text(t.desc, { lineGap: 1 });
  doc.moveDown(1);
});

// PAGE BREAK
doc.addPage();

// COMPONENTES
addSection('7. Componentes Reutilizáveis', 14);
doc.fontSize(9).fillColor('#555').text('Estes componentes aparecem em múltiplas telas e devem ser criados como design system no Lovable:', { lineGap: 2 });

const componentes = [
  '• Header com Logo, Busca, Carrinho, Login (loja pública)',
  '• Header do Painel com Nome do Usuário, Saldo, Menu Hamburguer (painel)',
  '• Sidebar com Menu Lateral Dinâmico e Submenu (painel)',
  '• Card de Saldo (exibe R$ com ícone de carteira)',
  '• Card de Transação (exibe data, tipo, valor, status)',
  '• Tabela de Produtos com Paginação e Filtros',
  '• Card de Produto (imagem, nome, preço, CTA)',
  '• Modal de Confirmação (logout, ações críticas)',
  '• Formulário com Validação (campos obrigatórios, erros)',
  '• Breadcrumb de Navegação',
  '• Badge de Status (ativo, inativo, pendente)',
  '• Loading Spinner (skeleton loading para tabelas)',
  '• Toast de Notificação (sucesso, erro, info)',
  '• Paginação (anterior, próxima, ir para página)',
  '• Filtro por Data (date picker)',
  '• Dropdown de Seleção',
  '• Link de Indicação (copiável)',
];

doc.fontSize(8.5).fillColor('#333').list(componentes, { lineGap: 1 });

// PAGE BREAK
doc.addPage();

// UX OPORTUNIDADES
addSection('8. Oportunidades de UX a Implementar', 14);

const oportunidades = [
  {
    ambiente: 'Portal Público',
    melhorias: [
      'Adicionar testimonios de usuários reais no hero',
      'Destacar cashback em números (ex: "Ganhe 5-10% em cada compra")',
      'Mostrar loja integrada no portal (botão "Ir para Loja" em destaque)',
      'Simplificar login social (Google, Facebook, Apple)',
    ]
  },
  {
    ambiente: 'Painel do Membro',
    melhorias: [
      'Dashboard com cards de KPI (saldo, cashback pendente, rede, últimas compras)',
      'Gráfico de ganhos por mês (visual progressivo)',
      'Link de referência com copy automático (já existe, melhorar visual)',
      'Notificações de compras feitas pela rede em tempo real',
      'Skeleton loading nas tabelas (melhora percepção de velocidade)',
    ]
  },
  {
    ambiente: 'Painel Admin',
    melhorias: [
      'Dashboard com KPI da plataforma (total de usuários, transações, revenue)',
      'Gráficos de crescimento e tendências',
      'Filtros avançados nas tabelas (data, status, valor)',
      'Export para CSV nas listas',
      'Search global de usuários',
    ]
  },
  {
    ambiente: 'Loja Pública',
    melhorias: [
      'Destaque do percentual de cashback para cada produto',
      'Carrossel de "Mais vendidos" ou "Trending"',
      'Recomendação de produtos relacionados no detalhe',
      'Reviews com fotos de usuários',
      'Aviso de frete + cashback + desconto simulado no carrinho',
    ]
  },
];

oportunidades.forEach(op => {
  addSubsection(op.ambiente);
  doc.fontSize(8.5).fillColor('#333').list(op.melhorias, { lineGap: 1 });
  doc.moveDown(2);
});

// PAGE BREAK
doc.addPage();

// GUIA LOVABLE
addSection('9. Guia para Usar este Documento no Lovable', 14);

doc.fontSize(9.5).fillColor('#333').text('Passo a passo para trazer este design para seu projeto:', { fontWeight: 'bold', lineGap: 2 });
doc.moveDown(6);

const passos = [
  {
    passo: '1. Acesse o Lovable',
    desc: 'Faça login em https://lovable.dev com sua conta'
  },
  {
    passo: '2. Crie um novo projeto',
    desc: 'Nome: "Quanta Shop" ou similar'
  },
  {
    passo: '3. Use este documento como brief',
    desc: 'Copie este prompt e passe para o Lovable na área de "Instructions":\n\n"Crie as telas de um sistema de e-commerce integrado com cashback e rede MLM. Use a arquitetura descrita: 4 ambientes (Portal, Painel Membro, Painel Admin, Loja). Cada tela deve ter botões, formulários, tabelas e componentes conforme documentado. Mantenha design limpo, cores em teal (#2f7785) e brancos. Crie componentes reutilizáveis."'
  },
  {
    passo: '4. Design as telas no Lovable',
    desc: 'Comece pelo Portal Público (mais simples), depois Painel Membro, depois Admin. Deixa a Loja por último.'
  },
  {
    passo: '5. Export do Lovable',
    desc: 'Quando estiver pronto, export o código Vue/React do Lovable'
  },
  {
    passo: '6. Integre no Replit',
    desc: 'Copie os componentes gerados do Lovable para o projeto Replit. Conecte com as APIs já existentes.'
  },
];

passos.forEach((p, idx) => {
  doc.fontSize(9).fillColor(teal).text(p.passo, { fontWeight: 'bold' });
  doc.fontSize(8.5).fillColor('#555').text(p.desc, { lineGap: 2 });
  doc.moveDown(4);
});

// FINAL
doc.moveDown(8);
doc.fontSize(8).fillColor('#999').text('---');
doc.fontSize(8).fillColor('#999').text('Este documento foi gerado automaticamente a partir da análise das telas atuais.\nÚltima atualização: ' + new Date().toLocaleDateString('pt-BR'));

doc.end();
console.log(`✅ PDF gerado: ${filename}`);
