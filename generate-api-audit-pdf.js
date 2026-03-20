const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({ size: 'A4', margin: 40 });
const filename = path.join(__dirname, 'public', 'auditoria-api-quanta-shop.pdf');
doc.pipe(fs.createWriteStream(filename));

const darkBlue = '#1a3a52';
const red = '#e74c3c';
const orange = '#f39c12';
const green = '#27ae60';

// Título
doc.fontSize(22).fillColor(darkBlue).text('Auditoria da Camada de API', { align: 'center', fontWeight: 'bold' });
doc.fontSize(15).fillColor(darkBlue).text('Quanta Shop — .NET Core API', { align: 'center', marginBottom: 10 });
doc.fontSize(9).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')} | Análise de Segurança, Qualidade e Escalabilidade`, { align: 'center', marginBottom: 20 });

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(15);

// Dimensão da API
doc.fontSize(12).fillColor(darkBlue).text('Dimensão da API', { fontWeight: 'bold' });
doc.moveDown(8);

const dimW = [100, 100, 100];
let dY = doc.y;
[['', 'V1', 'V2'], ['Controllers', '32', '14'], ['Endpoints', '~240', '~63'], ['Total', '272', '77']].forEach((row, idx) => {
  let dX = 40;
  const dH = 18;
  row.forEach((cell, cIdx) => {
    if (idx === 0) {
      doc.rect(dX, dY, dimW[cIdx], dH).fill(darkBlue);
      doc.fillColor('white').fontSize(8).text(cell, dX + 3, dY + 4, { width: dimW[cIdx] - 6, align: 'center' });
    } else {
      doc.rect(dX, dY, dimW[cIdx], dH).fill(idx % 2 === 0 ? '#f9f9f9' : 'white');
      doc.fillColor('#333').fontSize(8).text(cell, dX + 3, dY + 4, { width: dimW[cIdx] - 6, align: 'center' });
    }
    dX += dimW[cIdx];
  });
  dY += dH;
});

doc.y = dY + 12;
doc.moveDown(8);
doc.fontSize(9).fillColor('#555').text('Uma API de escala real. O problema é que 32 controllers em V1 nunca foram migrados para V2, e os dois vivem lado a lado sem estratégia de versioning definida.', { lineGap: 2 });
doc.moveDown(12);

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Segurança — Os Problemas Mais Graves
doc.fontSize(12).fillColor(darkBlue).text('Segurança — Os Problemas Mais Graves', { fontWeight: 'bold' });
doc.moveDown(10);

const security = [
  {
    num: '1',
    title: 'CORS completamente aberto',
    desc: 'SetIsOriginAllowed(host => true) — qualquer domínio pode chamar a API. Em produção, qualquer site externo pode fazer chamadas autenticadas. Deveria ter allowlist de domínios.'
  },
  {
    num: '2',
    title: 'JWT com validação enfraquecida',
    desc: 'ValidateIssuer = false e ValidateAudience = false desativados. Token não verifica origem nem destino. Em um sistema financeiro com cashback e saques, isso é superfície de ataque desnecessária.'
  },
  {
    num: '3',
    title: 'Token com validade de 24 horas',
    desc: 'Sessão muito longa. Se um token for comprometido, atacante tem janela grande de ação. Padrão recomendado: 15-60 minutos com refresh token.'
  },
  {
    num: '4',
    title: 'Nenhum rate limiting em nenhum endpoint',
    desc: 'Endpoints de login, cadastro e recuperação completamente expostos. Sem proteção contra força bruta e enumeração de usuários.'
  },
  {
    num: '5',
    title: 'Cookie de debug expõe stack traces em produção',
    desc: 'Cookie "oh_vida_oh_ceus" força erros e retorna stack trace completo — expõe nomes de classes, estrutura do banco, e caminhos internos.'
  },
  {
    num: '6',
    title: 'ZanoxConnectId hardcoded no código',
    desc: 'ID de credencial de afiliado literal no LoggedControllerBase — herdado por praticamente todos os controllers da V1.'
  }
];

security.forEach((s) => {
  doc.fontSize(9).fillColor(red).text(`${s.num}. ${s.title}`, { fontWeight: 'bold' });
  doc.moveDown(2);
  doc.fontSize(8.5).fillColor('#555').text(s.desc, { lineGap: 2 });
  doc.moveDown(5);
});

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Qualidade de Código — Padrões Perigosos
doc.fontSize(12).fillColor(darkBlue).text('Qualidade de Código — Padrões Perigosos', { fontWeight: 'bold' });
doc.moveDown(10);

const codeQuality = [
  ['1. Sync-over-async espalhado', '.Result/.Wait() em métodos async criam risco de deadlock. API pode travar sob carga.'],
  ['2. Fire-and-forget sem tratamento', '_ = EnviarEmailConfirmacao(...) — erro é descartado. Sem log, sem retry, sem alerta.'],
  ['3. async void em background service', 'AtualizaDadosAfilio.cs usa async override void TimerTick. Exceção derruba processo inteiro.'],
  ['4. N+1 queries em controllers', 'foreach + query dentro = 100 credenciamentos = 100 queries. Padrão correto não foi aplicado globalmente.'],
  ['5. Controllers com lógica pesada', 'UsuarioController: 31 endpoints com validação manual, serviços externos, formatação. Tudo em um lugar.'],
  ['6. Respostas inconsistentes', 'DTOs tipados, objetos anônimos, strings brutas. Sem envelope padronizado. Impossibilita SDK futuro.'],
  ['7. Acessar variável de ambiente no controller', 'Environment.GetEnvironmentVariable("SENDGRID_API_KEY") diretamente. Deve ser injetado via IOptions<>.'],
];

const codeW = [130, 310];
let cY = doc.y;
codeQuality.forEach((row, idx) => {
  let cX = 40;
  const cH = 22;
  doc.rect(cX, cY, codeW[0], cH).fill('#f9f9f9');
  doc.fillColor('#333').fontSize(7.5).text(row[0], cX + 3, cY + 2, { width: codeW[0] - 6, align: 'left' });
  
  doc.rect(cX + codeW[0], cY, codeW[1], cH).fill('#f9f9f9');
  doc.fillColor('#555').fontSize(7.5).text(row[1], cX + codeW[0] + 3, cY + 2, { width: codeW[1] - 6, align: 'left' });
  
  cY += cH;
});

doc.y = cY + 12;
doc.moveDown(8);

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Matriz de Qualidade
doc.fontSize(12).fillColor(darkBlue).text('Matriz de Qualidade', { fontWeight: 'bold' });
doc.moveDown(10);

const qualityData = [
  ['Escopo e cobertura de endpoints', '8/10'],
  ['Segurança (CORS, JWT, rate limit)', '3/10'],
  ['Qualidade do código assíncrono', '3/10'],
  ['Consistência das respostas', '4/10'],
  ['Separação de responsabilidades', '4/10'],
  ['Observabilidade (logging)', '3/10'],
  ['Estratégia de versioning (V1/V2)', '4/10'],
];

const qW = [280, 95];
let qY = doc.y;
[['Aspecto', 'Nota'], ...qualityData].forEach((row, idx) => {
  let qX = 40;
  const qH = 16;
  if (idx === 0) {
    doc.rect(qX, qY, qW[0], qH).fill(darkBlue);
    doc.fillColor('white').fontSize(7.5).text(row[0], qX + 3, qY + 2, { width: qW[0] - 6 });
    doc.rect(qX + qW[0], qY, qW[1], qH).fill(darkBlue);
    doc.fillColor('white').fontSize(7.5).text(row[1], qX + qW[0] + 3, qY + 2, { width: qW[1] - 6, align: 'center' });
  } else {
    doc.rect(qX, qY, qW[0], qH).fill('#f9f9f9');
    doc.fillColor('#333').fontSize(7.5).text(row[0], qX + 3, qY + 2, { width: qW[0] - 6 });
    doc.rect(qX + qW[0], qY, qW[1], qH).fill('#f9f9f9');
    const noteColor = row[1].includes('3/10') ? red : row[1].includes('4/10') ? orange : green;
    doc.fillColor(noteColor).fontSize(8).text(row[1], qX + qW[0] + 3, qY + 1, { width: qW[1] - 6, align: 'center', fontWeight: 'bold' });
  }
  qY += qH;
});

doc.y = qY + 15;

// Conclusão
doc.fontSize(11).fillColor(darkBlue).text('Prioridades Imediatas', { fontWeight: 'bold' });
doc.moveDown(8);

const priorities = [
  '🔴 Rate limiting nos endpoints de auth (Brute force risk)',
  '🔴 Restringir CORS para domínios autorizados (Qualquer site pode chamar)',
  '🔴 Remover/proteger cookie de debug (Exposição de internals)',
  '🔴 Reduzir JWT validade + ValidateIssuer (Janela de ataque)',
  '🟡 Eliminar .Result/.Wait() (Deadlock sob carga)',
  '🟡 Padronizar envelope de resposta (Quebras no frontend)',
  '🟡 Logging estruturado (Zero visibilidade em produção)',
];

doc.fontSize(8.5).fillColor('#333').list(priorities);

doc.moveDown(10);
doc.fontSize(8).fillColor('#999').text('---\nAnálise gerada a partir de inspeção completa do código da API .NET Core.\nTotal de controllers auditados: 46 | Endpoints analisados: 300+', { align: 'center' });

doc.end();
console.log(`✅ PDF gerado: ${filename}`);
