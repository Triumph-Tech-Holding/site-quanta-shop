const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({ size: 'A4', margin: 40 });
const filename = path.join(__dirname, 'public', 'auditoria-sistema-bonus-quanta-shop.pdf');
doc.pipe(fs.createWriteStream(filename));

const darkBlue = '#1a3a52';
const red = '#e74c3c';
const orange = '#f39c12';
const green = '#27ae60';

// Título
doc.fontSize(22).fillColor(darkBlue).text('Auditoria do Sistema de Bônus', { align: 'center', fontWeight: 'bold' });
doc.fontSize(15).fillColor(darkBlue).text('12 Níveis & Motor de Distribuição — Quanta Shop', { align: 'center' });
doc.fontSize(10).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')}`, { align: 'center' });
doc.moveDown(8);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(12);

// Nota geral
doc.fontSize(12).fillColor(darkBlue).text('Nota de Qualidade do Módulo', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(20).fillColor(red).text('3 / 10', { align: 'center', fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(9).fillColor('#555').text(
  'O sistema de lançamentos e bônus está em pior estado do que o módulo AWIN. ' +
  'Regras de negócio espalhadas em 3 camadas distintas (C#, stored procedures, SQL Agent Job), ' +
  'percentuais não configuráveis sem deploy, e parte dos bônus rodando completamente ' +
  'fora do controle da aplicação.',
  { align: 'center', lineGap: 2 }
);
doc.moveDown(12);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Tabela de tipos de bônus
doc.fontSize(12).fillColor(darkBlue).text('Tipos de Bônus Registrados no Sistema (16 tipos)', { fontWeight: 'bold' });
doc.moveDown(8);

const bonusTypes = [
  ['Chave', 'Descrição'],
  ['CHBK', 'Cashback externo (AWIN)'],
  ['DTCSH', 'Cashback residual para upline'],
  ['BDOB', 'Bônus duplo cashback'],
  ['BRCD', 'Bônus duplo residual'],
  ['BDIN', 'Bônus compressão dinâmica'],
  ['CHBLF', 'Cashback de loja física credenciada'],
  ['DCPLJ', 'Cashback residual de loja física'],
  ['DTBAD', 'Bônus adesão'],
  ['DTAD', 'Bônus residual adesão'],
  ['PPOC', 'Bônus credenciamento'],
  ['DPPOC', 'Bônus residual credenciamento'],
  ['LM', 'Lançamento manual'],
  ['EST', 'Estorno'],
  ['SQ', 'Saque'],
  ['DTSLD', 'Taxa de compra de saldo'],
  ['ASNT', 'Assinatura'],
];

const colW = [80, 350];
let startY = doc.y;
bonusTypes.forEach((row, rowIdx) => {
  let x = 40;
  const rowH = 18;
  row.forEach((cell, colIdx) => {
    if (rowIdx === 0) {
      doc.rect(x, startY, colW[colIdx], rowH).fill(darkBlue);
      doc.fillColor('white').fontSize(8).text(cell, x + 4, startY + 5, { width: colW[colIdx] - 8 });
    } else {
      doc.rect(x, startY, colW[colIdx], rowH).fill(rowIdx % 2 === 0 ? '#f9f9f9' : 'white');
      doc.fillColor('#333').fontSize(8).text(cell, x + 4, startY + 5, { width: colW[colIdx] - 8 });
    }
    x += colW[colIdx];
  });
  startY += rowH;
});

doc.y = startY + 12;
doc.moveDown(8);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Problemas encontrados
doc.fontSize(12).fillColor(darkBlue).text('Problemas Críticos Identificados', { fontWeight: 'bold' });
doc.moveDown(10);

// Problema 1
doc.fontSize(10).fillColor(red).text('1. Regra de negócio dentro do Repositório', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(9).fillColor('#555').text(
  'Os percentuais dos 12 níveis estão em um switch/case dentro de LancamentoRepositorio.cs — ' +
  'que é uma camada de acesso a dados, não de negócio. Para alterar um percentual de comissão, ' +
  'é necessário recompilar e fazer deploy da aplicação inteira.',
  { lineGap: 2 }
);
doc.moveDown(4);
doc.fontSize(8).fillColor('#888').text('Trecho do código atual:');
doc.moveDown(2);
doc.rect(40, doc.y, 515, 60).fill('#f5f5f5');
doc.fillColor('#333').fontSize(7.5).text(
  'case 1:\n    valorPorNivel = 5m / 100m * valorTotalDistribuido;   // 5% — hardcoded\ncase 2: ... case 10:\n    valorPorNivel = 0.5m / 100m * valorTotalDistribuido;  // 0.5% — hardcoded\ncase 11:\ncase 12:\n    valorPorNivel = 0.25m / 100m * valorTotalDistribuido; // 0.25% — hardcoded',
  { lineGap: 2, indent: 8 }
);
doc.moveDown(24);

// Problema 2
doc.fontSize(10).fillColor(red).text('2. Mapeamento Produto → Número de Níveis também hardcoded', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(9).fillColor('#555').text(
  'Em LancamentoNegocio.cs, a quantidade de níveis por produto está fixa no código ' +
  '(Produto 1: 0 níveis, Produto 2: 3 níveis, ..., Produto 6: 12 níveis). ' +
  'O banco já possui a tabela ProdutoNivel para guardar exatamente isso — mas o código C# não a utiliza.',
  { lineGap: 2 }
);
doc.moveDown(10);

// Problema 3
doc.fontSize(10).fillColor(red).text('3. N+1 queries para buscar corrente de upline', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(9).fillColor('#555').text(
  'O método ListaUsuariosPatrocinadores faz uma consulta ao banco por nível em loop iterativo. ' +
  'Para um usuário com 12 níveis de upline, são 12 consultas sequenciais antes de qualquer ' +
  'distribuição começar — repetido para cada transação processada.',
  { lineGap: 2 }
);
doc.moveDown(10);

// Problema 4
doc.fontSize(10).fillColor(red).text('4. Busca de usuário do sistema dentro de transação financeira', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(9).fillColor('#555').text(
  'Uma consulta ao banco pelo login literal "triumph" acontece dentro de cada transação de ' +
  'lançamento manual. Se esse usuário for renomeado ou removido, todo o processo de ' +
  'distribuição quebra silenciosamente.',
  { lineGap: 2 }
);
doc.moveDown(10);

// Problema 5
doc.fontSize(10).fillColor(red).text('5. Stored procedures órfãs — bônus binário fora do controle da aplicação', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(9).fillColor('#555').text(
  'spc_processabinario e spc_DistribuicaoCashback não são chamadas por nenhum código C#. ' +
  'São executadas por SQL Agent Job diretamente no banco — fora do versionamento do código, ' +
  'fora dos logs da aplicação, fora de qualquer rastreabilidade do sistema. ' +
  'Parte dos bônus pagos não passa por nenhum controle da aplicação.',
  { lineGap: 2 }
);
doc.moveDown(10);

// Problema 6
doc.fontSize(10).fillColor(orange).text('6. Legado Zanox ainda vivo', { fontWeight: 'bold' });
doc.moveDown(4);
doc.fontSize(9).fillColor('#555').text(
  'spc_DistribuicaoCashback ainda possui o parâmetro @nomeProgramaZanox. ' +
  'A Zanox foi renomeada para AWIN em 2017. Esse procedimento nunca foi modernizado.',
  { lineGap: 2 }
);

doc.moveDown(12);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Caminhos de cálculo
doc.fontSize(12).fillColor(darkBlue).text('5 Caminhos de Cálculo Distintos', { fontWeight: 'bold' });
doc.moveDown(8);

const paths = [
  ['Cashback externo (AWIN)', 'CashbackNegocio.LancarCashback → spc_LancarCashback'],
  ['Lançamento manual', 'LancamentoNegocio → LancamentoRepositorio (switch/case C#)'],
  ['Compra de saldo', 'LancamentoNegocio.GerarLancamentoSaldo'],
  ['Plano adquirido', 'spc_DistribuicaoPlanoAdquirido (SQL Agent Job)'],
  ['Bônus binário', 'spc_processabinario (SQL Agent Job — sem chamada C#)'],
];

const colW2 = [200, 260];
let sy2 = doc.y;
[['Evento', 'Caminho de Execução'], ...paths].forEach((row, rowIdx) => {
  let x = 40;
  const rowH = 20;
  row.forEach((cell, colIdx) => {
    if (rowIdx === 0) {
      doc.rect(x, sy2, colW2[colIdx], rowH).fill(darkBlue);
      doc.fillColor('white').fontSize(8).text(cell, x + 4, sy2 + 6, { width: colW2[colIdx] - 8 });
    } else {
      doc.rect(x, sy2, colW2[colIdx], rowH).fill(rowIdx % 2 === 0 ? '#f9f9f9' : 'white');
      doc.fillColor('#333').fontSize(8).text(cell, x + 4, sy2 + 6, { width: colW2[colIdx] - 8 });
    }
    x += colW2[colIdx];
  });
  sy2 += rowH;
});

doc.y = sy2 + 12;
doc.moveDown(8);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Recomendações
doc.fontSize(12).fillColor(darkBlue).text('Plano de Ação Recomendado', { fontWeight: 'bold' });
doc.moveDown(8);

const actions = [
  'Criar tabela ConfiguracaoNivel para guardar percentual por nível e por produto, eliminando os hardcodes',
  'Mover lógica de distribuição do Repositório para a camada de Negócio',
  'Substituir o N+1 por uma única query com CTE recursiva',
  'Chamar spc_processabinario a partir do Worker C# — trazer o bônus binário para o ciclo rastreável',
  'Remover referências a Zanox nas procedures e modernizar parâmetros',
];

actions.forEach((action, idx) => {
  doc.fontSize(9).fillColor('#333').text(`${idx + 1}. ${action}`, { lineGap: 3 });
  doc.moveDown(3);
});

doc.moveDown(12);
doc.fontSize(9).fillColor('#999').text('---\nRelatório gerado automaticamente a partir de análise de código fonte da Quanta Shop.', { align: 'center' });

doc.end();
console.log(`✅ PDF gerado: ${filename}`);
