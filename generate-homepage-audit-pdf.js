const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({ size: 'A4', margin: 40 });
const filename = path.join(__dirname, 'public', 'auditoria-homepage-quanta-shop.pdf');
doc.pipe(fs.createWriteStream(filename));

const darkBlue = '#1a3a52';
const red = '#e74c3c';
const orange = '#f39c12';
const green = '#27ae60';

// Título
doc.fontSize(22).fillColor(darkBlue).text('Auditoria da HOME', { align: 'center', fontWeight: 'bold' });
doc.fontSize(16).fillColor(darkBlue).text('Quanta Shop — Padrão Mundial', { align: 'center', marginBottom: 10 });
doc.fontSize(9).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')} | Análise UX/UI vs Méliuz, Rakuten, Shopee, Amazon`, { align: 'center', marginBottom: 20 });

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(15);

// O que a captura revela
doc.fontSize(12).fillColor(darkBlue).text('O que a Captura de Tela Já Revela', { fontWeight: 'bold' });
doc.moveDown(8);

doc.fontSize(9).fillColor(red).text('1. Spinner nu no hero — conteúdo carrega com círculo giratório', { fontWeight: 'bold' });
doc.moveDown(3);
doc.fontSize(8.5).fillColor('#555').text('Plataformas de referência usam skeleton loading — blocos cinzas no formato do conteúdo que aparecem instantaneamente. Usuário nunca vê tela vazia.');
doc.moveDown(6);

doc.fontSize(9).fillColor(red).text('2. Modal de instalação do app bloqueia a HOME', { fontWeight: 'bold' });
doc.moveDown(3);
doc.fontSize(8.5).fillColor('#555').text('Prompt de PWA aparece sobreposto sem o usuário ter tido chance de ver o valor da plataforma. É pedir casamento no primeiro segundo de conversa.');
doc.moveDown(12);

doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Auditoria seção por seção
doc.fontSize(12).fillColor(darkBlue).text('Auditoria Seção por Seção', { fontWeight: 'bold' });
doc.moveDown(10);

const sections = [
  ['Seção', 'Estado Atual', 'Padrão Mundial'],
  ['Hero Banner', 'Swiper com spinner', 'Banner com proposição de valor clara + CTA + cashback em destaque'],
  ['Funcionalidades', '4 cards estáticos, ícones genéricos', 'Prova social dinâmica (R$2,4M distribuído hoje)'],
  ['Parceiros online', 'Tabs estáticas (Destaque/Top/Novos)', 'Personalização por histórico + Top cashback em tempo real'],
  ['Ofertas do dia', 'Carrossel com 8 produtos', 'Produtos com timer countdown + % cashback em destaque'],
  ['Categorias', 'Círculos com ícones', 'OK, mas falta indicador de cashback por categoria'],
  ['Super Humano', 'Dados hardcoded em TypeScript', 'Seção dinâmica do banco, gerenciável pelo admin'],
  ['Blog', 'Dados hardcoded em TypeScript', 'CMS real ou endpoint administrativo'],
  ['Parceiros locais', 'Carrossel simples', 'Mapa de proximidade + cashback por localização'],
];

const colW = [90, 150, 185];
let y = doc.y;
sections.forEach((row, idx) => {
  let x = 40;
  const rowH = idx === 0 ? 20 : 22;
  row.forEach((cell, colIdx) => {
    if (idx === 0) {
      doc.rect(x, y, colW[colIdx], rowH).fill(darkBlue);
      doc.fillColor('white').fontSize(7).text(cell, x + 3, y + 4, { width: colW[colIdx] - 6, align: 'left' });
    } else {
      doc.rect(x, y, colW[colIdx], rowH).fill(idx % 2 === 0 ? '#f9f9f9' : 'white');
      doc.fillColor('#333').fontSize(7).text(cell, x + 3, y + 4, { width: colW[colIdx] - 6, align: 'left' });
    }
    x += colW[colIdx];
  });
  y += rowH;
});

doc.y = y + 12;
doc.moveDown(8);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// 7 Gaps Críticos
doc.fontSize(12).fillColor(darkBlue).text('Os 7 Gaps Críticos para Padrão Mundial', { fontWeight: 'bold' });
doc.moveDown(10);

const gaps = [
  {
    num: '1',
    title: 'Proposta de valor não aparece acima da dobra',
    desc: 'Visitante novo não consegue responder "o que eu ganho?" em 3 segundos. Honey, Rakuten, Méliuz colocam cashback acumulado, # lojas e CTA antes de scroll.'
  },
  {
    num: '2',
    title: 'Zero personalização',
    desc: 'HOME é idêntica para logado e anônimo. Amazon, Shopee, Méliuz personalizam 3+ seções por comportamento. Até básico ("viu X, goste de Y") eleva conversão.'
  },
  {
    num: '3',
    title: 'Cashback invisível nos produtos',
    desc: 'Cards mostram preço, cashback não é elemento principal. Em referências, cashback é maior que preço. Méliuz coloca "ATÉ X%" em fonte maior que nome da loja.'
  },
  {
    num: '4',
    title: 'Dados hardcoded no código',
    desc: 'Blog e Super Humano em TypeScript. Qualquer atualização exige desenvolvedor e deploy. Inaceitável para reagir a campanhas em tempo real.'
  },
  {
    num: '5',
    title: 'Performance percebida baixa',
    desc: 'Spinner visível indica LCP alto. Google penaliza no ranking. Skeleton loading resolve sem mudar backend. Impacto direto em SEO.'
  },
  {
    num: '6',
    title: 'Ausência de prova social e urgência',
    desc: 'Sem contadores dinâmicos ("X pessoas compraram hoje"), avaliações, ou indicador de quantidade limitada. Elementos que aumentam conversão mensurável.'
  },
  {
    num: '7',
    title: 'Search bar sem inteligência',
    desc: 'Input simples. Referências mostram: buscas trending, histórico do usuário, sugestões por categoria, resultados instantâneos enquanto digita.'
  },
];

gaps.forEach((gap) => {
  doc.fontSize(9).fillColor(darkBlue).text(`${gap.num}. ${gap.title}`, { fontWeight: 'bold' });
  doc.moveDown(3);
  doc.fontSize(8.5).fillColor('#555').text(gap.desc, { lineGap: 2 });
  doc.moveDown(5);
});

doc.moveDown(8);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Matriz de Prioridade
doc.fontSize(12).fillColor(darkBlue).text('Matriz de Prioridade de Implementação', { fontWeight: 'bold' });
doc.moveDown(10);

const priorities = [
  ['🔴 ALTA', 'Skeleton loading no hero e produtos', 'UX + SEO'],
  ['🔴 ALTA', 'Cashback em destaque nos cards de produto', 'Conversão'],
  ['🔴 ALTA', 'Hero com proposição de valor clara + CTA', 'Conversão'],
  ['🔴 ALTA', 'Remover modal PWA do primeiro acesso', 'Retenção'],
  ['🟡 MÉDIA', 'Blog e Super Humano com dados dinâmicos', 'Agilidade'],
  ['🟡 MÉDIA', 'Seção de prova social com números reais', 'Confiança'],
  ['🟡 MÉDIA', 'Search com sugestões e trending', 'Engajamento'],
  ['🟢 FUTURA', 'Personalização por histórico de navegação', 'Retenção avançada'],
  ['🟢 FUTURA', 'Parceiros locais com mapa de proximidade', 'Diferenciação'],
];

const pW = [100, 300, 95];
let py = doc.y;
priorities.forEach((row, idx) => {
  let px = 40;
  const rH = 18;
  row.forEach((cell, cIdx) => {
    if (idx > 0) {
      doc.rect(px, py, pW[cIdx], rH).fill('#fafafa');
      doc.fillColor('#333').fontSize(8).text(cell, px + 3, py + 3, { width: pW[cIdx] - 6 });
    }
    px += pW[cIdx];
  });
  py += rH;
});

doc.y = py + 15;

// Conclusão
doc.fontSize(11).fillColor(darkBlue).text('Recomendação Final', { fontWeight: 'bold' });
doc.moveDown(8);
doc.fontSize(9).fillColor('#555').text(
  'As 4 mudanças de alta prioridade (skeleton loading, cashback em destaque, proposta de valor, remover PWA modal) ' +
  'são implementáveis em 2-3 semanas e têm impacto imediato mensurável em conversão e SEO. ' +
  'As mudanças de média prioridade desbloqueiam agilidade operacional. ' +
  'As futuras completam a modernização para nível Rakuten/Shopee.',
  { lineGap: 2 }
);

doc.moveDown(12);
doc.fontSize(8).fillColor('#999').text('---\nAnálise gerada a partir de screenshot e inspeção do código da página home.\nComparativas: Méliuz, Rakuten, Shopee, Amazon, Honey.', { align: 'center' });

doc.end();
console.log(`✅ PDF gerado: ${filename}`);
