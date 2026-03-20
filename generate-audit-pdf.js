const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({
  size: 'A4',
  margin: 40,
});

const filename = path.join(__dirname, 'public', 'auditoria-awin-quanta-shop.pdf');
doc.pipe(fs.createWriteStream(filename));

// Cores
const darkBlue = '#1a3a52';
const lightGray = '#f5f5f5';
const green = '#27ae60';
const orange = '#f39c12';
const red = '#e74c3c';

// Título
doc.fontSize(24).fillColor(darkBlue).text('Auditoria Técnica', { align: 'center', fontWeight: 'bold' });
doc.fontSize(20).fillColor(darkBlue).text('Integração AWIN — Quanta Shop', { align: 'center', marginBottom: 10 });

// Data
doc.fontSize(10).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')}`, { align: 'center', marginBottom: 20 });

// Linha divisória
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(15);

// Resumo executivo (tabela no início)
doc.fontSize(12).fillColor(darkBlue).text('Resumo Executivo', { fontWeight: 'bold' });
doc.moveDown(8);

const summaryTable = [
  ['Item', 'Status'],
  ['Consumo de Datafeeds', '⚠️ Parcial'],
  ['API de Promoções', '✅ Funcionando'],
  ['Postback/Webhook', '✅ Funcionando'],
  ['Rastreio (ClickRef)', '⚠️ Funcionando (bug)'],
  ['Buscador c/ EAN', '❌ Não iniciado'],
  ['Consulta de Transações', '✅ Funcionando'],
];

drawTable(doc, summaryTable, 40, doc.y, [250, 150]);
doc.moveDown(20);

// Seção 1: Datafeeds
doc.fontSize(12).fillColor(darkBlue).text('1. Consumo de Datafeeds (Product Feeds)', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(10).fillColor('#333').text('Status: ⚠️ Parcial', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(10).fillColor('#555').text(
  'A ingestão dos produtos do feed AWIN está implementada através da tabela AwinFeed e do serviço AwinFeedService.cs. ' +
  'Os links de produto incluem awinaffid e clickref do usuário.\n\n' +
  'Observação importante: Não foi localizado um cron job exclusivamente dedicado à importação/atualização periódica ' +
  'dos arquivos XML/CSV da AWIN. O serviço de sincronização existente (Awin.cs) é focado em transações, não em atualização de catálogo.',
  { align: 'left' }
);
doc.moveDown(12);

// Seção 2: Promotions API
doc.fontSize(12).fillColor(darkBlue).text('2. API de Promoções (Offers API)', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(10).fillColor('#333').text('Status: ✅ Funcionando', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(10).fillColor('#555').text(
  'Existe método ObterCuponsOfertasAsync que realiza chamadas à API AWIN buscando promoções ativas para a região BR. ' +
  'O clickref do usuário é adicionado aos links de promoção.\n\n' +
  'Ressalva: O token de acesso está fixo no código (hardcoded). Recomenda-se mover para variável de ambiente por questões de segurança.',
  { align: 'left' }
);
doc.moveDown(12);

// Seção 3: Webhook
doc.fontSize(12).fillColor(darkBlue).text('3. Sistema de Postback/Webhook', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(10).fillColor('#333').text('Status: ✅ Funcionando', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(10).fillColor('#555').text(
  'Endpoint dedicado em POST api/v2/awin-webhook (AwinWebhookController.cs) recebe notificações em tempo real da AWIN. ' +
  'Dados como TransactionId, SaleAmount, Commission e ClickRef são armazenados na tabela AwinWebhookRequests.\n\n' +
  'Ação recomendada: Confirmar na interface da AWIN se a URL de produção do "Transaction Notification v2" aponta corretamente para este endpoint.',
  { align: 'left' }
);
doc.moveDown(12);

// Seção 4: ClickRef
doc.fontSize(12).fillColor(darkBlue).text('4. Rastreio via ClickRef', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(10).fillColor('#333').text('Status: ⚠️ Funcionando (com bug identificado)', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(10).fillColor('#555').text(
  'O clickref está sendo preenchido com o IdUsuario (GUID interno) nos links de produto e anunciante. ' +
  'Na reconciliação, o sistema lê clickRef e clickRef2 da resposta AWIN e associa ao usuário interno via GUID.\n\n' +
  'Bug identificado: Em AnuncianteNegocio.cs, um link usa placeholder literal [IdUsuario] (com colchetes) ' +
  'ao invés de interpolar o valor real, gerando links sem o ID do usuário. Requer correção.',
  { align: 'left' }
);
doc.moveDown(12);

// Seção 5: EAN
doc.fontSize(12).fillColor(darkBlue).text('5. Buscador e Agrupamento por EAN', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(10).fillColor('#333').text('Status: ❌ Não iniciado', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(10).fillColor('#555').text(
  'Não foi encontrada lógica de agrupamento por código EAN no buscador ou em AwinFeedService.cs. ' +
  'A busca atual filtra por texto, categoria e preço, mas NÃO agrupa o mesmo produto de lojistas diferentes pelo EAN ' +
  'para permitir comparação de cashback entre redes. ' +
  'Esta funcionalidade precisa ser desenvolvida do zero.',
  { align: 'left' }
);
doc.moveDown(12);

// Seção 6: Transaction Query API
doc.fontSize(12).fillColor(darkBlue).text('6. API de Consulta de Transações (Transaction Query)', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(10).fillColor('#333').text('Status: ✅ Funcionando', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(10).fillColor('#555').text(
  'Integração completa implementada. O serviço Awin.cs sincroniza transações automaticamente a cada 4 horas, ' +
  'buscando tanto por data de transação quanto de validação (últimos 12 meses). ' +
  'Administradores podem buscar transações sincronizadas em TransactionsController.cs com filtros e joins entre Transacao, Pedido e Anunciante. ' +
  'O status (pendente/aprovada/negada) é atualizado e cashback processado automaticamente.',
  { align: 'left' }
);

doc.moveDown(20);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(15);

// Recomendações finais
doc.fontSize(12).fillColor(darkBlue).text('Próximas Ações Recomendadas', { fontWeight: 'bold' });
doc.moveDown(8);
doc.fontSize(10).fillColor('#555').list([
  'Implementar cron job para sincronização periódica de catálogo/estoque do feed AWIN',
  'Mover token de acesso da API de Promotions para variável de ambiente',
  'Corrigir bug do placeholder [IdUsuario] em AnuncianteNegocio.cs',
  'Validar URL de postback do Transaction Notification v2 na interface AWIN',
  'Implementar lógica de agrupamento por EAN no buscador e serviço de produtos',
  'Testar fluxo completo end-to-end em ambiente de staging',
]);

doc.moveDown(15);

// Rodapé
doc.fontSize(9).fillColor('#999').text(
  '---\nRelatório gerado automaticamente. Para dúvidas técnicas, consulte a documentação e os arquivos do projeto.',
  { align: 'center' }
);

// Finalizar PDF
doc.end();

console.log(`✅ PDF gerado com sucesso: ${filename}`);

// Helper function para desenhar tabela
function drawTable(doc, data, x, y, colWidths) {
  const rowHeight = 25;
  const cellPadding = 5;
  let currentY = y;

  data.forEach((row, rowIndex) => {
    let currentX = x;

    row.forEach((cell, colIndex) => {
      // Background para header
      if (rowIndex === 0) {
        doc.rect(currentX, currentY, colWidths[colIndex], rowHeight).fill('#1a3a52');
        doc.fillColor('white').fontSize(9).text(cell, currentX + cellPadding, currentY + cellPadding, {
          width: colWidths[colIndex] - cellPadding * 2,
          align: 'left',
        });
      } else {
        // Alternating row colors
        if (rowIndex % 2 === 0) {
          doc.rect(currentX, currentY, colWidths[colIndex], rowHeight).fill('#f9f9f9');
        } else {
          doc.rect(currentX, currentY, colWidths[colIndex], rowHeight).fill('white');
        }
        doc.fillColor('#333').fontSize(9).text(cell, currentX + cellPadding, currentY + cellPadding, {
          width: colWidths[colIndex] - cellPadding * 2,
          align: 'left',
        });
      }

      currentX += colWidths[colIndex];
    });

    currentY += rowHeight;
  });

  doc.fillColor('#333');
  return currentY;
}
