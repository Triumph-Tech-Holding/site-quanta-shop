const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({ size: 'A4', margin: 40 });
const filename = path.join(__dirname, 'public', 'auditoria-agencia-quanta-shop.pdf');
doc.pipe(fs.createWriteStream(filename));

const darkBlue = '#1a3a52';
const red = '#e74c3c';
const orange = '#f39c12';
const green = '#27ae60';

// Título
doc.fontSize(22).fillColor(darkBlue).text('Auditoria da Agência', { align: 'center', fontWeight: 'bold' });
doc.fontSize(15).fillColor(darkBlue).text('Escritório Virtual — Quanta Shop', { align: 'center', marginBottom: 10 });
doc.fontSize(9).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')} | Análise de Painel, UX e Manutenibilidade`, { align: 'center', marginBottom: 20 });

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(15);

// Visão Geral
doc.fontSize(12).fillColor(darkBlue).text('Visão Geral da Plataforma', { fontWeight: 'bold' });
doc.moveDown(8);

doc.fontSize(9).fillColor('#555').text(
  'A agência é um portal completo com 3 perfis de acesso: Empreendedor (16 páginas de painel), ' +
  'Comerciante (painel próprio), Administrador (18 páginas de back-office). É uma plataforma robusta em escopo. ' +
  'O problema não é o que falta — é como o que existe está construído.',
  { lineGap: 2 }
);
doc.moveDown(12);

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Problemas Críticos
doc.fontSize(12).fillColor(darkBlue).text('Problemas Críticos Encontrados no Código', { fontWeight: 'bold' });
doc.moveDown(10);

const problems = [
  {
    num: '1',
    title: 'Mapa do Google com coordenadas fixas de São Paulo',
    desc: 'O mapa para "lojas mais próximas de você" tem coordenadas hardcoded para região central de SP (-23.55, -46.633). Usuários de Recife, Belém, Porto Alegre veem mapa de São Paulo. Funcionalidade completamente inútil fora de SP.'
  },
  {
    num: '2',
    title: 'Metas do Meta Minuto hardcoded no código',
    desc: 'Valores fixos: Diária R$14,40 | Semanal R$100,80 | Mensal R$432,00. Mudança de regra de negócio = deploy necessário. Devem vir da API.'
  },
  {
    num: '3',
    title: 'Ícones carregados de CDN externo (iconfinder.com)',
    desc: 'Os 4 ícones dos indicadores vêm de cdn3.iconfinder.com. Se o serviço estiver lento ou offline, dashboard aparece quebrado. Ativos de UI devem ser locais.'
  },
  {
    num: '4',
    title: 'Vídeos tutoriais hardcoded no template Vue',
    desc: 'URLs dos 3 vídeos de onboarding fixas no componente. Trocar vídeo = deploy. Admin deveria gerenciar via painel (como Hotmart/Eduzz).'
  },
  {
    num: '5',
    title: 'Link de indicação com domínio hardcoded',
    desc: 'quantashop.com.br/register/${login} literal no código. Mudança de domínio = bug em todos os links de indicação de todos usuários.'
  },
  {
    num: '6',
    title: 'Saldo sem atualização em tempo real',
    desc: 'Carregado uma vez no onMounted e não atualiza mais. Usuário recebe cashback no painel aberto? Número não muda sem recarregar. Deve usar polling ou WebSocket.'
  }
];

problems.forEach((p, idx) => {
  doc.fontSize(9).fillColor(red).text(`${p.num}. ${p.title}`, { fontWeight: 'bold' });
  doc.moveDown(3);
  doc.fontSize(8.5).fillColor('#555').text(p.desc, { lineGap: 2 });
  doc.moveDown(6);
});

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// O que está bom
doc.fontSize(12).fillColor(darkBlue).text('O que Está Bom e Deve Ser Preservado', { fontWeight: 'bold' });
doc.moveDown(8);

const good = [
  'Estrutura de 2 colunas (conteúdo + sidebar de ação) — ergonomicamente correta',
  'Meta Minuto é diferencial criativo — merece mais destaque',
  'Seção de vídeos de onboarding é excelente para ativação de novos usuários',
  'Modal de convite com link copiável funciona bem',
  'Sistema de suporte integrado ao painel é profissional'
];

doc.fontSize(9).fillColor('#333').list(good, { lineGap: 2 });

doc.moveDown(12);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// O que está faltando
doc.fontSize(12).fillColor(darkBlue).text('O que Está Faltando para Padrão Mundial', { fontWeight: 'bold' });
doc.moveDown(10);

const missing = [
  ['Gráfico de ganhos (30/60/90 dias)', 'Hotmart, Eduzz, Monetizze'],
  ['Histórico recente de cashback na home', 'Méliuz, Rakuten'],
  ['Barra de progresso de graduação/nível', 'Nubank, XP'],
  ['Notificações em tempo real', 'Shopee, Amazon'],
  ['Resumo da rede (diretos ativos vs inativos)', 'Plataformas MLM modernas'],
  ['Conquistas / badges de engajamento', 'Duolingo, Nubank'],
  ['Ranking dos melhores da rede (gamificação)', 'Hotmart, Pepper'],
  ['Quick actions flutuantes', 'Méliuz mobile'],
  ['Saudação com nome + foto de perfil', 'Qualquer SaaS B2C'],
  ['Dark mode', 'Padrão 2024+'],
];

const mW = [230, 165];
let my = doc.y;
[['Feature Ausente', 'Referência'], ...missing].forEach((row, idx) => {
  let mx = 40;
  const mH = 16;
  row.forEach((cell, cIdx) => {
    if (idx === 0) {
      doc.rect(mx, my, mW[cIdx], mH).fill(darkBlue);
      doc.fillColor('white').fontSize(7.5).text(cell, mx + 3, my + 2, { width: mW[cIdx] - 6 });
    } else {
      doc.rect(mx, my, mW[cIdx], mH).fill(idx % 2 === 0 ? '#f9f9f9' : 'white');
      doc.fillColor('#333').fontSize(7.5).text(cell, mx + 3, my + 2, { width: mW[cIdx] - 6 });
    }
    mx += mW[cIdx];
  });
  my += mH;
});

doc.y = my + 12;
doc.moveDown(8);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Matriz de Qualidade
doc.fontSize(12).fillColor(darkBlue).text('Matriz de Qualidade Geral', { fontWeight: 'bold' });
doc.moveDown(10);

const quality = [
  { aspecto: 'Escopo e funcionalidades disponíveis', nota: '8/10', cor: green },
  { aspecto: 'Qualidade do código do dashboard', nota: '4/10', cor: orange },
  { aspecto: 'Experiência do usuário (UX)', nota: '5/10', cor: orange },
  { aspecto: 'Manutenibilidade (sem hardcodes)', nota: '3/10', cor: red },
  { aspecto: 'Padrão visual vs referências mundiais', nota: '4/10', cor: orange },
];

const qW = [260, 100];
let qy = doc.y;

// Header
doc.rect(40, qy, qW[0], 18).fill(darkBlue);
doc.fillColor('white').fontSize(8).text('Aspecto', 43, qy + 3, { width: qW[0] - 6 });
doc.rect(40 + qW[0], qy, qW[1], 18).fill(darkBlue);
doc.fillColor('white').fontSize(8).text('Nota', 43 + qW[0], qy + 3, { width: qW[1] - 6 });
qy += 18;

// Rows
quality.forEach((item, idx) => {
  const bgColor = idx % 2 === 0 ? '#f9f9f9' : 'white';
  doc.rect(40, qy, qW[0], 18).fill(bgColor);
  doc.fillColor('#333').fontSize(8).text(item.aspecto, 43, qy + 3, { width: qW[0] - 6 });
  
  doc.rect(40 + qW[0], qy, qW[1], 18).fill(bgColor);
  doc.fillColor(item.cor).fontSize(8.5).text(item.nota, 43 + qW[0], qy + 3, { width: qW[1] - 6, align: 'center', fontWeight: 'bold' });
  
  qy += 18;
});

doc.y = qy + 15;

// Conclusão
doc.fontSize(11).fillColor(darkBlue).text('Conclusão Final', { fontWeight: 'bold' });
doc.moveDown(8);
doc.fontSize(9).fillColor('#555').text(
  'A plataforma tem músculo, mas a interface não comunica isso. Um usuário que entra no painel hoje não consegue, ' +
  'em 10 segundos, entender quanto ganhou este mês, qual é seu nível atual e o que precisa fazer para subir. ' +
  'Essas três informações deveriam ser a primeira coisa visível no dashboard.\n\n' +
  'O maior gap está na manutenibilidade — hardcodes espalhados por todo o código impedem que o negócio ' +
  'seja ágil em mudanças de configuração, domínio ou regras de meta.',
  { lineGap: 2 }
);

doc.moveDown(12);
doc.fontSize(8).fillColor('#999').text('---\nAnálise gerada a partir de inspeção do código do painel da agência.\nComparativas: Hotmart, Eduzz, Monetizze, Méliuz, Rakuten, Shopee, Amazon, Nubank.', { align: 'center' });

doc.end();
console.log(`✅ PDF gerado: ${filename}`);
