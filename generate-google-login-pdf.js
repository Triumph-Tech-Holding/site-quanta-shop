const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({ size: 'A4', margin: 40 });
const filename = path.join(__dirname, 'public', 'status-google-login.pdf');
doc.pipe(fs.createWriteStream(filename));

const darkBlue = '#1a3a52';
const red = '#e74c3c';
const orange = '#f39c12';
const green = '#27ae60';

// Título
doc.fontSize(20).fillColor(darkBlue).text('Status do Login com Google', { align: 'center', fontWeight: 'bold' });
doc.fontSize(14).fillColor(red).text('Não está funcionando em produção', { align: 'center', marginBottom: 10 });
doc.fontSize(9).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')} | Quanta Shop`, { align: 'center', marginBottom: 20 });

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(15);

// Status Resumido
doc.fontSize(11).fillColor(darkBlue).text('Status Resumido', { fontWeight: 'bold' });
doc.moveDown(8);
doc.fontSize(9).fillColor('#333').text('O código existe e está implementado, mas tem 4 problemas críticos que impedem o funcionamento. Nenhum é de arquitetura — todos são configuração.', { lineGap: 2 });
doc.moveDown(12);

// Os 4 Problemas
doc.fontSize(11).fillColor(darkBlue).text('Os 4 Problemas Críticos', { fontWeight: 'bold' });
doc.moveDown(10);

const problems = [
  {
    num: '1',
    title: 'Dois Client IDs diferentes que não batem',
    frontend: '372294010028-ff1frn14fg81mn0ujhv215lk9rd5t80r.apps.googleusercontent.com',
    backend: '123493812146-gdjfhkeguuon50kjhhd6i3hgf4v172el.apps.googleusercontent.com',
    impact: 'Google rejeita a autenticação porque frontend e backend não são o mesmo cliente.'
  },
  {
    num: '2',
    title: 'URL do backend apontando para localhost',
    code: 'https://localhost:44385/api/v2/users/login-google',
    impact: 'Esse endereço não existe em nenhum navegador além do computador do dev. É uma rota inválida para produção.'
  },
  {
    num: '3',
    title: 'Dois endpoints diferentes para a mesma função',
    frontend_endpoint: '/api/v2/users/login-google',
    backend_endpoint: '/api/UsuarioLogin/autenticacaoGoogle',
    impact: 'Frontend manda para um lugar, backend está esperando em outro. Eles não se comunicam.'
  },
  {
    num: '4',
    title: 'Senha do Google salva no código',
    code: 'Senha = "69fWzUHFrkSE1HIfS8smM-Z-"',
    impact: 'A chave secreta OAuth está visível no repositório. Qualquer pessoa com acesso ao código consegue ler essa credencial.'
  }
];

problems.forEach((p) => {
  doc.fontSize(9.5).fillColor(red).text(`${p.num}. ${p.title}`, { fontWeight: 'bold' });
  doc.moveDown(3);
  
  if (p.frontend) {
    doc.fontSize(8).fillColor('#555').text(`Frontend:  ${p.frontend}`, { lineGap: 1 });
    doc.text(`Backend:   ${p.backend}`);
    doc.moveDown(2);
  }
  
  if (p.code && p.num === '2') {
    doc.fontSize(8).fillColor('#555').text(`Configurado como: ${p.code}`, { lineGap: 1 });
    doc.moveDown(2);
  }
  
  if (p.frontend_endpoint) {
    doc.fontSize(8).fillColor('#555').text(`Frontend envia para: ${p.frontend_endpoint}`, { lineGap: 1 });
    doc.text(`Backend escuta em: ${p.backend_endpoint}`);
    doc.moveDown(2);
  }
  
  if (p.code && p.num === '4') {
    doc.fontSize(8).fillColor('red').text(`No código: ${p.code}`, { lineGap: 1 });
    doc.moveDown(2);
  }
  
  doc.fontSize(8).fillColor(orange).text(`Impacto: ${p.impact}`, { lineGap: 2 });
  doc.moveDown(6);
});

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// O que precisa ser feito
doc.fontSize(11).fillColor(darkBlue).text('O que Precisa Ser Feito', { fontWeight: 'bold' });
doc.moveDown(10);

const steps = [
  '1. Criar credenciais novas no Google Cloud Console — um par de Client ID + Client Secret únicos e ativos',
  '2. Alinhar frontend e backend para usar as mesmas credenciais',
  '3. Corrigir a URL de localhost para o domínio real da sua aplicação',
  '4. Unificar o endpoint para um caminho único',
  '5. Mover o segredo para variável de ambiente protegida (não no código)',
];

doc.fontSize(8.5).fillColor('#333').list(steps, { lineGap: 2 });

doc.moveDown(12);

// Estimativa
doc.fontSize(11).fillColor(darkBlue).text('Estimativa de Trabalho', { fontWeight: 'bold' });
doc.moveDown(8);

const estimW = [150, 150];
let estY = doc.y;
[['Escopo', 'Tempo'], ['Ativar login Google completo', '1 dia de trabalho']].forEach((row, idx) => {
  let estX = 40;
  const estH = 18;
  if (idx === 0) {
    doc.rect(estX, estY, estimW[0], estH).fill(darkBlue);
    doc.fillColor('white').fontSize(8).text(row[0], estX + 3, estY + 3, { width: estimW[0] - 6 });
    doc.rect(estX + estimW[0], estY, estimW[1], estH).fill(darkBlue);
    doc.fillColor('white').fontSize(8).text(row[1], estX + estimW[0] + 3, estY + 3, { width: estimW[1] - 6, align: 'center' });
  } else {
    doc.rect(estX, estY, estimW[0], estH).fill('#e8f5e9');
    doc.fillColor('#333').fontSize(8).text(row[0], estX + 3, estY + 3, { width: estimW[0] - 6 });
    doc.rect(estX + estimW[0], estY, estimW[1], estH).fill('#e8f5e9');
    doc.fillColor(green).fontSize(8).text(row[1], estX + estimW[0] + 3, estY + 3, { width: estimW[1] - 6, align: 'center', fontWeight: 'bold' });
  }
  estY += estH;
});

doc.y = estY + 15;

// Facebook e Apple
doc.fontSize(11).fillColor(darkBlue).text('Facebook e Apple', { fontWeight: 'bold' });
doc.moveDown(8);
doc.fontSize(8.5).fillColor('#555').text('Ambos estão no código mas completamente desabilitados (ocultos com CSS). São botões fantasma por enquanto. A arquitetura suporta, mas o setup não foi feito.', { lineGap: 2 });

doc.moveDown(12);

// Conclusão
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

doc.fontSize(10).fillColor(green).text('✓ Viável e Executável', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(8.5).fillColor('#333').text('Nenhum dos problemas requer mudanças de arquitetura. São ajustes de configuração que o agente pode fazer em poucas horas, sem risco de quebrar nada no backend ou banco de dados.', { lineGap: 2 });

doc.end();
console.log(`✅ PDF gerado: ${filename}`);
