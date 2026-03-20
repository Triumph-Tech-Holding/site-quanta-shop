const PDFDocument = require('pdfkit');
const fs = require('fs');
const path = require('path');

const doc = new PDFDocument({
  size: 'A4',
  margin: 40,
});

const filename = path.join(__dirname, 'public', 'auditoria-arquitetura-quanta-shop.pdf');
doc.pipe(fs.createWriteStream(filename));

// Cores
const darkBlue = '#1a3a52';
const red = '#e74c3c';
const orange = '#f39c12';
const green = '#27ae60';

// Título
doc.fontSize(24).fillColor(darkBlue).text('Auditoria Crítica de Arquitetura', { align: 'center', fontWeight: 'bold' });
doc.fontSize(16).fillColor(darkBlue).text('Quanta Shop — Módulo AWIN', { align: 'center', marginBottom: 10 });
doc.fontSize(10).fillColor('#666').text(`Data: ${new Date().toLocaleDateString('pt-BR')} | Avaliação de Viabilidade Técnica`, { align: 'center', marginBottom: 20 });

// Linha divisória
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#ccc');
doc.moveDown(15);

// 1. Nível de Débito Técnico
doc.fontSize(13).fillColor(darkBlue).text('1. Nível de Débito Técnico', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(11).fillColor(red).text('Nota: 4/10', { fontWeight: 'bold' });
doc.moveDown(6);
doc.fontSize(10).fillColor('#555').text(
  'Não é código espaguete puro, mas está longe de ser saudável. Existem camadas de responsabilidade definidas (Domínio, Negócio, Repositório, API), o que é positivo. O problema é que essas camadas estão sendo violadas com frequência.',
  { align: 'left' }
);
doc.moveDown(10);

doc.fontSize(11).fillColor(darkBlue).text('Piores Gargalos Identificados:', { fontWeight: 'bold' });
doc.moveDown(6);

const gargalos = [
  {
    title: 'Dois ORMs misturados sem critério',
    desc: 'AwinFeedService usa Dapper com SQL bruto e sql.Replace(...) enquanto AnuncianteNegocio usa Entity Framework com LINQ. Dois dialetos de acesso a dados no mesmo módulo.'
  },
  {
    title: 'Sync-over-async anti-pattern',
    desc: 'O serviço principal usa .Wait() e .Result em métodos assíncronos. Pode causar travamento do thread pool em picos de carga.'
  },
  {
    title: 'Valores hardcoded espalhados',
    desc: 'ID de afiliado 689359, cálculo * 0.25 para cashback e regras de negócio fixas no código, não em configurações.'
  },
  {
    title: 'Usuario é um God Object',
    desc: 'Referenciado por praticamente todas as tabelas do sistema — pedidos, transações, cashback, suporte, MLM, carteira. Tocar nele toca em tudo.'
  },
  {
    title: 'Stored Procedures como lógica crítica',
    desc: 'sp_AtualizaGraduacao e sp_InsertQuantaAmizade implementam lógica de negócio fora do código, tornando o sistema opaco e fora de versionamento efetivo.'
  },
  {
    title: 'Catch vazio em notificações',
    desc: 'EnviarNotificacoesAsync tem bloco catch {} vazio. Falhas de e-mail e WhatsApp desaparecem em silêncio.'
  }
];

gargalos.forEach((g, idx) => {
  doc.fontSize(9).fillColor('#333').text(`${idx + 1}. ${g.title}`, { fontWeight: 'bold' });
  doc.fontSize(9).fillColor('#666').text(g.desc, { align: 'left', lineGap: 2 });
  doc.moveDown(4);
});

doc.moveDown(8);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// 2. Risco de Quebra
doc.fontSize(13).fillColor(darkBlue).text('2. Risco de Quebra (Regressão)', { fontWeight: 'bold' });
doc.moveDown(10);

const riscos = [
  {
    title: 'Corrigir o bug do ClickRef',
    level: 'Risco: MÉDIO',
    color: orange,
    desc: 'O bug está localizado em AnuncianteNegocio.cs. Parece pontual, mas esse arquivo injeta 6 dependências e é chamado em múltiplos contextos (loja, painel, parceiros). Uma mudança errada pode afetar a exibição de anunciantes em toda a plataforma. Executável com segurança com testes manuais cuidadosos.'
  },
  {
    title: 'Implementar Cron Job de Datafeed',
    level: 'Risco: MÉDIO-ALTO',
    color: orange,
    desc: 'Adicionar novo ciclo dentro de Awin.cs significa compartilhar o mesmo espaço com o job de sincronização de transações financeiras. Se o novo job travar, pode impactar o processamento de cashback. A forma correta seria um serviço separado.'
  }
];

riscos.forEach((r, idx) => {
  doc.fontSize(10).fillColor(darkBlue).text(`${String.fromCharCode(65 + idx)}. ${r.title}`, { fontWeight: 'bold' });
  doc.fontSize(9).fillColor(r.color).text(r.level, { fontWeight: 'bold' });
  doc.moveDown(4);
  doc.fontSize(9).fillColor('#666').text(r.desc, { align: 'left', lineGap: 2 });
  doc.moveDown(6);
});

doc.moveDown(8);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// 3. Capacidade da IA
doc.fontSize(13).fillColor(darkBlue).text('3. Capacidade da IA para Implementar Busca por EAN', { fontWeight: 'bold' });
doc.moveDown(8);

doc.fontSize(10).fillColor('#333').text('Resposta direta: Consigo implementar, mas o risco é real.', { fontWeight: 'bold' });
doc.moveDown(8);

doc.fontSize(10).fillColor('#555').text(
  'O algoritmo de agrupamento é relativamente simples. O problema é o contexto:\n\n' +
  '• AwinFeed precisa ter coluna EAN (é necessário verificar se os feeds AWIN exportam esse campo)\n\n' +
  '• AwinFeedService usa SQL bruto com CTEs e paginação complexa — qualquer mudança requer atenção cirúrgica\n\n' +
  '• Schema do banco não tem migrations documentadas claramente, risco de inconsistência\n\n' +
  '• Com stored procedures e dois ORMs, manter contexto completo de efeitos colaterais é onde IAs costumam falhar\n\n' +
  'Conclusão: Implementação é viável, mas deve ser feita em escopo controlado e verificada passo a passo, não em um único bloco de código.',
  { align: 'left', lineGap: 3 }
);

doc.moveDown(12);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// 4. Veredito
doc.fontSize(13).fillColor(darkBlue).text('4. Veredito do Arquiteto', { fontWeight: 'bold' });
doc.moveDown(10);

doc.fontSize(11).fillColor(red).text('❌ Opção A (continuar remendando): NÃO RECOMENDADO', { fontWeight: 'bold' });
doc.fontSize(9).fillColor('#666').text('Continuar sem limpar é a razão pela qual o código está em 4/10 hoje. Cada novo recurso aumenta o custo da próxima mudança.');
doc.moveDown(8);

doc.fontSize(11).fillColor(orange).text('⚠️ Opção C (microserviço isolado): PREMATURA', { fontWeight: 'bold' });
doc.fontSize(9).fillColor('#666').text('Criar serviço isolado que conecta no mesmo banco legado resolve metade do problema e cria novos (sincronização de dados, latência, dois contextos para manter).');
doc.moveDown(8);

doc.fontSize(11).fillColor(green).text('✅ Opção B (refactoring focado do módulo AWIN): RECOMENDADO', { fontWeight: 'bold' });
doc.moveDown(6);

const refactorSteps = [
  'Eliminar o anti-padrão sync-over-async no background service',
  'Consolidar acesso a dados (escolher Dapper OU EF Core, não os dois)',
  'Mover valores hardcoded para configuração (appsettings.json)',
  'Separar cron de datafeed do cron de transações (dois serviços)',
  'Fechar o catch vazio de notificações com logging apropriado'
];

refactorSteps.forEach((step, idx) => {
  doc.fontSize(9).fillColor('#333').text(`${idx + 1}. ${step}`);
});

doc.moveDown(10);
doc.fontSize(9).fillColor('#555').text(
  'Escopo controlado, não reescreve sistema de usuários, transforma módulo AWIN em algo que aguenta peso de 2026.',
  { fontStyle: 'italic' }
);

doc.moveDown(12);
doc.moveTo(40, doc.y).lineTo(555, doc.y).stroke('#e0e0e0');
doc.moveDown(12);

// Conclusão
doc.fontSize(13).fillColor(darkBlue).text('Conclusão Final', { fontWeight: 'bold' });
doc.moveDown(8);

doc.fontSize(10).fillColor('#333').text('O alicerce está rachado em pontos específicos, mas não é condenado.', { fontWeight: 'bold' });
doc.moveDown(6);

doc.fontSize(10).fillColor('#555').text(
  'Com 2 a 3 semanas de refactoring cirúrgico no módulo AWIN, o terreno fica seguro para construir os recursos de 2026 sem cair.\n\n' +
  'Implementar ClickRef fix, Cron de datafeed e EAN search em código limpo é exponencialmente mais rápido e menos arriscado do que tentar fazer no código atual.',
  { align: 'left', lineGap: 3 }
);

doc.moveDown(15);

// Rodapé
doc.fontSize(9).fillColor('#999').text(
  '---\nAvaliação realizada por análise de código fonte.\nRelatório gerado automaticamente.',
  { align: 'center' }
);

// Finalizar
doc.end();

console.log(`✅ PDF da auditoria de arquitetura gerado: ${filename}`);
