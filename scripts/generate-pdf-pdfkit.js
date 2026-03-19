#!/usr/bin/env node
const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

function generatePDF() {
  const doc = new PDFDocument({ margin: 40 });
  const outputPath = path.join(__dirname, '..', 'docs', 'quanta-shop-technical-spec.pdf');
  
  // Criar stream de saída
  const stream = fs.createWriteStream(outputPath);
  doc.pipe(stream);
  
  // CAPA
  doc.fontSize(48).fillColor('#2f7785').text('QUANTA SHOP', { align: 'center' });
  doc.moveDown();
  doc.fontSize(28).fillColor('#98c73a').text('Especificação Técnica Completa', { align: 'center' });
  doc.moveDown(2);
  doc.fontSize(14).fillColor('#000').text('Plataforma de Cashback e Afiliados', { align: 'center' });
  doc.moveDown(3);
  doc.fontSize(11).fillColor('#666').text('Documento para migração ao Lovable', { align: 'center' });
  doc.fontSize(11).text('19 de março de 2026', { align: 'center' });
  
  doc.addPage();
  
  // SEÇÃO 1
  doc.fontSize(24).fillColor('#2f7785').text('1. Visão Geral do Sistema', { underline: true });
  doc.moveDown(0.5);
  
  doc.fontSize(12).fillColor('#000');
  doc.text('Quanta Shop é uma plataforma de cashback e afiliados que permite usuários ganhar renda passiva através de indicações e compras.');
  doc.moveDown();
  doc.fontSize(11).font('Helvetica-Bold').text('URL de Produção:');
  doc.font('Helvetica').text('quantashop.com.br');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').text('Stack Atual:');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Frontend: Nuxt 3 (Vue 3 + TypeScript + Bootstrap 5 + SCSS + Pinia)');
  doc.text('• Backend: .NET 8 (MMN.Api, porta 8000, Azure SQL)');
  doc.text('• Proxy: Nitro server-side (/api-proxy/*) para evitar CORS');
  doc.text('• Renderização: SPA client-side (SSR desabilitado)');
  doc.text('• Autenticação: JWT em localStorage (chave: agencia_user)');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').text('Recomendação para Lovable:');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Framework: React 18 + TypeScript');
  doc.text('• Estilização: Tailwind CSS');
  doc.text('• Roteamento: React Router v6');
  doc.text('• Estado Global: Zustand');
  doc.text('• HTTP Client: Axios (com interceptor de token)');
  doc.text('• Formulários: React Hook Form + Zod');
  doc.text('• Tabelas: TanStack Table');
  doc.text('• Gráficos: Recharts');
  doc.text('• Componentes UI: Headless UI / Radix UI');
  doc.moveDown();
  
  doc.fontSize(10).fillColor('#d32f2f').text('⚠️ IMPORTANTE para integração Replit: Todas as chamadas de API devem usar paths relativos (/api-proxy/*) para funcionar com o proxy Nitro existente. Não hardcode URLs absolutas.');
  
  doc.addPage();
  
  // SEÇÃO 2
  doc.fontSize(24).fillColor('#2f7785').text('2. Identidade Visual', { underline: true });
  doc.moveDown(0.5);
  doc.fontSize(11).fillColor('#000').font('Helvetica-Bold').text('Paleta de Cores:');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Primary: #2f7785 (Teal) — Botões, links, títulos principais');
  doc.text('• Primary Dark: #225f6b — Hover, ênfase, bordas');
  doc.text('• Secondary: #98c73a (Verde Limão) — Acento, sucesso, cashback');
  doc.text('• Background: #ecf2f7 — Fundo do painel, cards secundários');
  doc.text('• White: #ffffff — Cards principais, modais');
  doc.text('• Text Principal: #212529 — Corpo de texto, parágrafos');
  
  doc.addPage();
  
  // SEÇÃO 3
  doc.fontSize(24).fillColor('#2f7785').text('3. Páginas Públicas', { underline: true });
  doc.moveDown(0.5);
  doc.fontSize(11).fillColor('#000').font('Helvetica-Bold').text('/agencia');
  doc.font('Helvetica').fontSize(10).text('Landing page institucional com seções de hero, como funciona, benefícios e CTA.');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').fillColor('#000').text('/agencia/login');
  doc.font('Helvetica').fontSize(10).text('Formulário de login (login/email + senha) → POST /usuario/autenticar → JWT em localStorage');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').fillColor('#000').text('/agencia/cadastro');
  doc.font('Helvetica').fontSize(10).text('Registro de novo afiliado com validação → POST /usuario/cadastrar');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').fillColor('#000').text('/agencia/recuperar-senha');
  doc.font('Helvetica').fontSize(10).text('Recuperação de senha → POST /usuario/recuperarSenha');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').fillColor('#000').text('Demais páginas públicas:');
  doc.font('Helvetica').fontSize(10).text('/quem-somos, /como-funciona, /faq, /privacidade, /lojas-fisicas, /mais-vendas');
  
  doc.addPage();
  
  // SEÇÃO 4
  doc.fontSize(24).fillColor('#2f7785').text('4. Painel do Afiliado', { underline: true });
  doc.moveDown(0.5);
  doc.fontSize(11).fillColor('#000').font('Helvetica-Bold').text('Layout:');
  doc.font('Helvetica').fontSize(10).text('Sidebar fixa (esquerda) + TopBar (direita) + área de conteúdo');
  doc.text('Todas as rotas protegidas por middleware de autenticação client-side');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').fillColor('#000').text('/agencia/painel (Dashboard)');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Stats (4 cards): Equipe, Saldo, Ecossistemas, Pontos');
  doc.text('• Meta Minuto: Contador regressivo');
  doc.text('• Vídeos: Modais YouTube');
  doc.text('• Link de indicação: Input copiável');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').fillColor('#000').text('Demais páginas do painel:');
  doc.font('Helvetica').fontSize(10);
  doc.text('• /meus-dados — Editar perfil, endereço, conta bancária, senha');
  doc.text('• /financeiro — CCR com 3 abas: movimentações, saque, histórico');
  doc.text('• /minhas-compras — Tabela de compras com modal de detalhes');
  doc.text('• /minha-rede — Rede de indicados com stats');
  doc.text('• /performance — Ranking de desempenho');
  doc.text('• /suporte — Tickets de suporte com filtros');
  doc.text('• Demais: graduações, contas-bancarias, assinatura, planos, cupons, material-apoio, etc.');
  
  doc.addPage();
  
  // SEÇÃO 5
  doc.fontSize(24).fillColor('#2f7785').text('5. Painel Administrativo', { underline: true });
  doc.moveDown(0.5);
  doc.fontSize(11).fillColor('#000').font('Helvetica-Bold').text('Rota base:');
  doc.font('Helvetica').fontSize(10).text('/agencia/painel/admin/**');
  doc.text('Requer validação de perfil admin no JWT');
  doc.moveDown();
  
  doc.fontSize(11).font('Helvetica-Bold').fillColor('#000').text('Páginas:');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Dashboard — Stats gerais (usuários, compras, cashback, estabelecimentos)');
  doc.text('• Usuários — Busca, tabela, ações (editar, bloquear)');
  doc.text('• Credenciamento — Solicitações (Pendente/Aprovado/Reprovado)');
  doc.text('• Compras, Pagamentos, Categorias, Ecossistemas, Carrosseis');
  doc.text('• Comunicados, Rede, Suporte, Lojas credenciados');
  doc.text('• Relatórios: Faturas, Anunciantes, Cashback');
  doc.text('• Mais: Aniversariantes, Acessos, Lançamentos, Material, Assinaturas, Grupos');
  
  doc.addPage();
  
  // SEÇÃO 6
  doc.fontSize(24).fillColor('#2f7785').text('6. Componentes Compartilhados', { underline: true });
  doc.moveDown(0.5);
  doc.fontSize(10).fillColor('#000');
  doc.text('Componentes recomendados para Lovable:');
  doc.moveDown();
  
  doc.fontSize(9).font('Helvetica-Bold');
  doc.text('• Sidebar — Menu lateral com navegação dinâmica');
  doc.text('• TopBar — Barra superior com user info');
  doc.text('• StatCard — Card com ícone, label, valor');
  doc.text('• PageHeader — Título + subtítulo');
  doc.text('• FilterBox — Container de filtros');
  doc.text('• DataTable — Tabela responsiva com loading/empty states');
  doc.text('• StatusBadge — Badge colorido por status');
  doc.text('• Modal — Modal genérico com header/body/footer');
  doc.text('• ConfirmDialog — Dialog de confirmação');
  
  doc.addPage();
  
  // SEÇÃO 7
  doc.fontSize(24).fillColor('#2f7785').text('7. Fluxo de Autenticação', { underline: true });
  doc.moveDown(0.5);
  doc.fontSize(10).fillColor('#000');
  doc.text('1. Usuário submete credenciais → POST /usuario/autenticar');
  doc.text('2. API retorna JWT + dados do usuário');
  doc.text('3. Frontend armazena em localStorage (chave: agencia_user)');
  doc.text('4. Todas as requisições autenticadas enviam: Authorization: Bearer {token}');
  doc.text('5. Middleware client-side valida token antes de renderizar');
  doc.text('6. Token expirado → redirect para /agencia/login');
  doc.text('7. Perfil admin: campo adicional no JWT');
  
  doc.addPage();
  
  // SEÇÃO 8
  doc.fontSize(24).fillColor('#2f7785').text('8. Endpoints de API Principais', { underline: true });
  doc.moveDown(0.5);
  doc.fontSize(9).fillColor('#000');
  doc.text('POST /usuario/autenticar — Login');
  doc.text('POST /usuario/cadastrar — Cadastro');
  doc.text('GET /usuario/obterDados — Dados do perfil');
  doc.text('PUT /usuario/alterarDados — Atualizar perfil');
  doc.text('GET /v2/dashboard/get-totalizers-user — Stats painel');
  doc.text('GET /MetaMinuto/obterMetaMinuto — Meta minuto');
  doc.text('GET /financeiro/listaMovimentacoes — Extrato');
  doc.text('POST /saque/solicitar — Solicitar saque');
  doc.text('GET /saque/historico — Histórico saques');
  doc.text('POST /Pedidos/listaPedidosAfiliados — Minhas compras');
  doc.text('GET /Rede/obterMinhaRede — Minha rede');
  doc.text('POST /Dashboard/obterPerformance — Performance');
  doc.text('POST /Suporte/listaSuporte — Lista suporte');
  doc.text('POST /Suporte/abrirSuporte — Abrir chamado');
  doc.text('GET /geral/obterMenu/{perfil} — Menu dinâmico');
  
  doc.addPage();
  
  // SEÇÃO 9
  doc.fontSize(24).fillColor('#2f7785').text('9. Integração Lovable → Replit', { underline: true });
  doc.moveDown(0.5);
  doc.fontSize(11).fillColor('#000').font('Helvetica-Bold').text('Instruções:');
  doc.font('Helvetica').fontSize(10);
  doc.text('1. NÃO alterar estrutura de rotas da API .NET');
  doc.text('2. Usar SEMPRE paths relativos /api-proxy/* (nunca URLs absolutas)');
  doc.text('3. Proxy Nitro já está configurado em server/routes/api-proxy/[...path].ts');
  doc.text('4. Manter localStorage com chave agencia_user para compatibilidade');
  doc.text('5. Ao integrar no Replit: copiar componentes React para components/lovable/');
  doc.text('6. CORS na API .NET já ajustado para aceitar domínio Replit');
  doc.text('7. Testar autenticação e cache do menu dinâmico antes de deploy');
  doc.text('8. Validar que JWT está sendo enviado em todas as requisições autenticadas');
  
  doc.addPage();
  
  // SEÇÃO 10
  doc.fontSize(24).fillColor('#2f7785').text('10. Próximos Passos', { underline: true });
  doc.moveDown(0.5);
  
  doc.fontSize(12).font('Helvetica-Bold').fillColor('#000').text('Fase 1: Design & Prototipagem');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Criar design system no Figma com componentes Tailwind');
  doc.text('• Validar paleta de cores com stakeholders');
  doc.text('• Fazer protótipos de fluxo crítico');
  doc.moveDown();
  
  doc.fontSize(12).font('Helvetica-Bold').fillColor('#000').text('Fase 2: Desenvolvimento Lovable');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Scaffolding do projeto React + Tailwind + Router v6');
  doc.text('• Componentes base (Sidebar, TopBar, DataTable)');
  doc.text('• Autenticação e interceptor de token');
  doc.text('• Páginas públicas e painel do afiliado');
  doc.moveDown();
  
  doc.fontSize(12).font('Helvetica-Bold').fillColor('#000').text('Fase 3: Testes & QA');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Testes unitários e integração');
  doc.text('• Testes E2E dos fluxos críticos');
  doc.text('• Validação de responsividade');
  doc.moveDown();
  
  doc.fontSize(12).font('Helvetica-Bold').fillColor('#000').text('Fase 4: Integração Replit');
  doc.font('Helvetica').fontSize(10);
  doc.text('• Importar build do Lovable para Replit');
  doc.text('• Configurar proxy API e variáveis de ambiente');
  doc.text('• Deploy e validação em produção');
  
  doc.moveDown(3);
  doc.fontSize(9).fillColor('#666').text('—');
  doc.fontSize(9).text('Quanta Shop — Especificação Técnica Completa');
  doc.fontSize(8).text('Este documento contém informações técnicas para guiar a migração do sistema para Lovable.');
  doc.fontSize(8).text('Gerado em 19 de março de 2026');
  
  doc.end();
  
  return new Promise((resolve, reject) => {
    stream.on('finish', () => {
      console.log(`✅ PDF gerado com sucesso: ${outputPath}`);
      resolve(outputPath);
    });
    stream.on('error', reject);
  });
}

generatePDF().catch(err => {
  console.error('❌ Erro ao gerar PDF:', err);
  process.exit(1);
});
