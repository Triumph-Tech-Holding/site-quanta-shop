const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({
  size: 'A4',
  margin: 40,
});

const filename = path.join(__dirname, 'public', 'analise-arquitetura-motor-financeiro.pdf');
doc.pipe(fs.createWriteStream(filename));

// Cores
const darkBlue = '#1a3a52';
const green = '#27ae60';
const orange = '#f39c12';

// Título
doc.fontSize(22).fillColor(darkBlue).text('Análise Arquitetural', { align: 'center', fontWeight: 'bold' });
doc.fontSize(18).fillColor(darkBlue).text('Motor Financeiro & Módulo AWIN', { align: 'center', marginBottom: 10 });
doc.fontSize(10).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')} | Fase 1 — Refatoração AWIN`, { align: 'center', marginBottom: 20 });

// Linha divisória
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(15);

// Resumo Executivo
doc.fontSize(12).fillColor(darkBlue).text('Resumo Executivo', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(9).fillColor('#555').text(
  'A Quanta Shop opera um Motor de Distribuição de Riqueza Omnichannel com múltiplas fontes de entrada (AWIN, lojas físicas, lançamentos manuais). ' +
  'A análise identifica que a fundação para convergência já existe (Lancamento, Transacao, Pedido), ' +
  'mas faltam camadas de normalização para garantir consistência. ' +
  'Três mudanças arquiteturais são recomendadas antes de qualquer implementação de código.',
  { align: 'left', lineGap: 2 }
);
doc.moveDown(15);

// Ponto 1: Unified Ledger
doc.fontSize(13).fillColor(darkBlue).text('1. Padronização de Entradas — Unified Ledger', { fontWeight: 'bold' });
doc.moveDown(8);

doc.fontSize(10).fillColor(green).text('✅ A boa notícia: a fundação já existe, só está incompleta.', { fontWeight: 'bold' });
doc.moveDown(6);

doc.fontSize(9).fillColor('#555').text(
  'O código possui a estrutura correta de três camadas:\n' +
  '• Lancamento → o ledger atômico real (linha a linha, por usuário)\n' +
  '• Transacao → o agrupador de evento financeiro (cabeçalho)\n' +
  '• Pedido → a ordem de origem que dispara o processo\n\n' +
  'O problema: não existe uma camada de normalização antes do Pedido. ' +
  'Cada fonte de entrada (AWIN, manual) cria seu Pedido de forma diferente. ' +
  'Uma venda de padaria local e um postback da AWIN chegam por caminhos completamente distintos.',
  { align: 'left', lineGap: 2 }
);
doc.moveDown(10);

doc.fontSize(10).fillColor(darkBlue).text('Solução Proposta: Tabela TransacaoBruta', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(8).fillColor('#333').text('TransacaoBruta (porta de entrada única):', { fontStyle: 'italic' });
doc.moveDown(3);

const transactionBrutaFields = [
  'Id (GUID)',
  'Fonte (AWIN | LOCAL | MANUAL | FUTURO_PARCEIRO)',
  'IdExterno (TransactionId da AWIN, Id do PDV local, etc.)',
  'PayloadOriginal (JSON — preserva o dado bruto para auditoria)',
  'ValorBruto',
  'ValorComissao',
  'ClickRefToken (referência ao token — ver ponto 3)',
  'Status (RECEBIDO → NORMALIZADO → PROCESSADO → ERRO)',
  'IdPedido (preenchido após normalização)',
  'CriadoEm / ProcessadoEm'
];

doc.fontSize(8).fillColor('#666').list(transactionBrutaFields);
doc.moveDown(8);

doc.fontSize(9).fillColor('#555').text(
  'O spc_LancarCashback não muda. Ele continua recebendo um Pedido e processando. ' +
  'O que muda é um estágio anterior: normalização de TransacaoBruta → Pedido. ' +
  'Qualquer fonte nova no futuro só precisa aprender a escrever em TransacaoBruta.',
  { align: 'left', lineGap: 2 }
);

doc.moveDown(12);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Ponto 2: Desacoplamento
doc.fontSize(13).fillColor(darkBlue).text('2. Desacoplamento da Regra de Negócio — Webhook vs. Worker', { fontWeight: 'bold' });
doc.moveDown(8);

doc.fontSize(10).fillColor(green).text('✅ O código já tomou a decisão certa — mas não terminou de implementá-la.', { fontWeight: 'bold' });
doc.moveDown(6);

doc.fontSize(9).fillColor('#555').text(
  'O webhook (AwinWebhookController) hoje apenas persiste o dado bruto na tabela AwinWebhookRequests. ' +
  'Ele não distribui cashback, não chama stored procedure, não faz nada além de gravar. ' +
  'Isso é arquiteturalmente correto.\n\n' +
  'O problema: os dados do webhook nunca são processados. A tabela AwinWebhookRequests é um dead-letter box. ' +
  'O processamento real acontece no background service que consulta a API da AWIN de 4 em 4 horas — ' +
  'completamente ignorando os webhooks que chegaram.',
  { align: 'left', lineGap: 2 }
);

doc.moveDown(10);
doc.fontSize(10).fillColor(red = '#e74c3c').text('❌ Por que NÃO chamar spc_LancarCashback no webhook:', { fontWeight: 'bold' });
doc.moveDown(6);

const risks = [
  'A AWIN aguarda resposta HTTP em < 5 segundos. Cálculo de upline de 12 níveis pode exceder isso.',
  'Se o banco falhar, você perde o evento da AWIN definitivamente — sem retry.',
  'Um webhook lento pode segurar a fila de requisições do servidor inteiro.'
];

doc.fontSize(8).fillColor('#666').list(risks);

doc.moveDown(10);
doc.fontSize(10).fillColor(darkBlue).text('Fluxo Correto: Worker/Serviço de Distribuição', { fontWeight: 'bold' });
doc.moveDown(6);

doc.fontSize(8).fillColor('#333').text('AWIN Webhook', { fontStyle: 'italic', fontWeight: 'bold' });
doc.fontSize(8).fillColor('#666').text('└─► TransacaoBruta (status: RECEBIDO)');
doc.fontSize(8).fillColor('#666').text('        └─► HTTP 200 imediato para a AWIN');

doc.moveDown(6);
doc.fontSize(8).fillColor('#333').text('Worker/Serviço de Distribuição (roda a cada 5-15 min)', { fontStyle: 'italic', fontWeight: 'bold' });
doc.fontSize(8).fillColor('#666').text('└─► Lê TransacaoBruta WHERE status = \'RECEBIDO\'');
doc.fontSize(8).fillColor('#666').text('└─► Normaliza → cria Pedido');
doc.fontSize(8).fillColor('#666').text('└─► Chama spc_LancarCashback');
doc.fontSize(8).fillColor('#666').text('└─► Atualiza status → PROCESSADO ou ERRO (com retry automático)');

doc.moveDown(10);
doc.fontSize(9).fillColor('#555').text(
  'O Worker de Distribuição substitui e unifica o atual background service de polling. ' +
  'Reduz carga no banco e elimina a janela de 4 horas de atraso na concessão de cashback.',
  { align: 'left', lineGap: 2 }
);

doc.moveDown(12);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Ponto 3: ClickRef Token
doc.fontSize(13).fillColor(darkBlue).text('3. Estrutura do ClickRef — Rastreabilidade & Matriz de Bônus', { fontWeight: 'bold' });
doc.moveDown(8);

doc.fontSize(10).fillColor(orange).text('⚠️ Este é o ponto mais crítico e o mais mal resolvido atualmente.', { fontWeight: 'bold' });
doc.moveDown(6);

doc.fontSize(9).fillColor('#555').text(
  'Hoje o clickref carrega apenas o IdUsuario (um GUID). ' +
  'Isso resolve "quem clicou" mas não responde "qual matriz de bônus aplicar".\n\n' +
  'O problema real: ProdutoNivel define os percentuais de distribuição por nível de upline ' +
  'baseado no PRODUTO/PLANO que o usuário possui NO MOMENTO DO CLIQUE. ' +
  'Se o usuário muda de plano entre o clique e a aprovação (que pode levar semanas), ' +
  'o sistema não tem como saber qual matriz era válida no momento do clique.',
  { align: 'left', lineGap: 2 }
);

doc.moveDown(10);
doc.fontSize(10).fillColor(darkBlue).text('Solução: Tabela ClickRefToken', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(8).fillColor('#333').text('ClickRefToken (snapshot do contexto no momento do clique):', { fontStyle: 'italic' });
doc.moveDown(3);

const tokenFields = [
  'Token (GUID — este é o valor enviado como clickref para a AWIN)',
  'IdUsuario',
  'IdAnunciante',
  'IdProdutoAtivo (plano do usuário no momento do clique)',
  'IdGraduacaoAtiva (nível MLM no momento do clique)',
  'CriadoEm',
  'Expirado (bool — tokens antigos podem ser arquivados)'
];

doc.fontSize(8).fillColor('#666').list(tokenFields);

doc.moveDown(8);
doc.fontSize(9).fillColor('#555').text(
  'Por que Token e não dados embutidos? A AWIN limita clickref a 20 caracteres. ' +
  'Um GUID já é arriscado. Com Token como chave de lookup, você passa apenas um GUID ' +
  'e na volta da transação, faz lookup para reconstituir todo o contexto com fidelidade histórica.',
  { align: 'left', lineGap: 2 }
);

doc.moveDown(10);
doc.fontSize(9).fillColor('#333').text('Fluxo completo com o Token:');
doc.moveDown(4);

doc.fontSize(8).fillColor('#666').text('Usuário clica no link');
doc.fontSize(8).fillColor('#666').text('└─► Sistema gera ClickRefToken (snapshot do contexto)');
doc.fontSize(8).fillColor('#666').text('└─► Link: awin1.com/...&clickref={Token}');

doc.moveDown(6);
doc.fontSize(8).fillColor('#666').text('AWIN confirma transação');
doc.fontSize(8).fillColor('#666').text('└─► TransacaoBruta recebe o Token');
doc.fontSize(8).fillColor('#666').text('└─► Worker lê Token → busca contexto (IdProduto, IdGraduacao)');
doc.fontSize(8).fillColor('#666').text('└─► Normaliza → cria Pedido com matriz correta');
doc.fontSize(8).fillColor('#666').text('└─► spc_LancarCashback usa percentuais corretos de ProdutoNivel');

doc.moveDown(15);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Diagrama de Convergência
doc.fontSize(13).fillColor(darkBlue).text('Diagrama de Convergência — Todas as Fontes para o Motor Único', { fontWeight: 'bold' });
doc.moveDown(10);

doc.fontSize(8).fillColor('#666').text('AWIN Webhook ───────────────────┐');
doc.fontSize(8).fillColor('#666').text('AWIN Polling ────────────────────┤');
doc.fontSize(8).fillColor('#666').text('Venda Local (PDV Físico) ────────┼─► TransacaoBruta ─► Worker ─► Pedido ─► spc_LancarCashback ─► Lancamento (Ledger)');
doc.fontSize(8).fillColor('#666').text('Lançamento Manual ────────────────┘                                          └─► Upline via ProdutoNivel');

doc.moveDown(15);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Pré-requisitos
doc.fontSize(13).fillColor(darkBlue).text('Pré-requisitos de Banco — Antes de Qualquer Código', { fontWeight: 'bold' });
doc.moveDown(10);

const preRequisites = [
  'Criar tabela TransacaoBruta com os campos especificados acima',
  'Criar tabela ClickRefToken com os campos especificados acima',
  'Adaptar AwinWebhookController para gravar em TransacaoBruta (ao invés de AwinWebhookRequests)',
  'Criar Worker de Distribuição como IHostedService separado do AwinService existente'
];

doc.fontSize(9).fillColor('#333').list(preRequisites);

doc.moveDown(15);

// Recomendação Final
doc.fontSize(12).fillColor(darkBlue).text('Recomendação Final', { fontWeight: 'bold' });
doc.moveDown(8);
doc.fontSize(9).fillColor('#555').text(
  'Implementar nessa ordem garante que o módulo AWIN se torne parte do motor financeiro coeso e escalável, ' +
  'e não um "Frankenstein" isolado que será difícil de manter ou estender no futuro.\n\n' +
  'As mudanças são bem localizadas no código e no banco — não há refactoring massivo de negócio. ' +
  'Apenas clarificação arquitetural do que já existe.',
  { align: 'left', lineGap: 3 }
);

doc.moveDown(12);
doc.fontSize(9).fillColor('#999').text(
  '---\nAnalysis based on source code inspection of Quanta Shop architecture.',
  { align: 'center' }
);

doc.end();

console.log(`✅ PDF gerado: ${filename}`);
