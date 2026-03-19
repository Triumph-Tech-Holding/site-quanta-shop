#!/usr/bin/env node
const puppeteer = require('puppeteer');
const fs = require('fs');
const path = require('path');

const htmlContent = `<!DOCTYPE html>
<html lang="pt-BR">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Quanta Shop — Especificação Técnica</title>
  <style>
    * { margin: 0; padding: 0; box-sizing: border-box; }
    body { 
      font-family: 'Segoe UI', Arial, sans-serif; 
      line-height: 1.6; 
      color: #212529; 
      background: white;
    }
    .page { page-break-after: always; padding: 40px; }
    .page:last-child { page-break-after: avoid; }
    h1 { 
      font-size: 2.5em; 
      color: #2f7785; 
      margin: 30px 0 15px; 
      border-bottom: 3px solid #98c73a; 
      padding-bottom: 10px;
    }
    h2 { 
      font-size: 1.8em; 
      color: #225f6b; 
      margin: 25px 0 10px; 
    }
    h3 { 
      font-size: 1.3em; 
      color: #2f7785; 
      margin: 15px 0 8px; 
    }
    h4 { 
      font-size: 1.1em; 
      color: #225f6b; 
      margin: 12px 0 6px; 
    }
    p { margin-bottom: 12px; }
    ul, ol { margin-left: 25px; margin-bottom: 12px; }
    li { margin-bottom: 6px; }
    table { 
      width: 100%; 
      border-collapse: collapse; 
      margin: 15px 0; 
      font-size: 0.95em;
    }
    th { 
      background: #ecf2f7; 
      color: #2f7785; 
      padding: 10px; 
      text-align: left; 
      border: 1px solid #ddd;
      font-weight: bold;
    }
    td { 
      padding: 8px; 
      border: 1px solid #ddd; 
    }
    tr:nth-child(even) { background: #f9fafb; }
    code { 
      background: #f5f5f5; 
      padding: 2px 6px; 
      font-family: 'Courier New', monospace;
      border-radius: 3px;
    }
    .section { margin: 20px 0; }
    .highlight { 
      background: #ecf2f7; 
      padding: 15px; 
      border-left: 4px solid #98c73a; 
      margin: 15px 0;
    }
    .color-box { 
      display: inline-block; 
      width: 30px; 
      height: 30px; 
      margin-right: 8px; 
      vertical-align: middle; 
      border: 1px solid #ddd;
    }
    .page-break { page-break-after: always; }
    .toc { 
      background: #ecf2f7; 
      padding: 20px; 
      margin: 20px 0; 
      border-radius: 5px;
    }
    .toc ul { margin-left: 20px; }
    .footer { 
      margin-top: 40px; 
      padding-top: 20px; 
      border-top: 1px solid #ddd; 
      font-size: 0.9em; 
      color: #666;
    }
  </style>
</head>
<body>

<!-- CAPA -->
<div class="page">
  <div style="text-align: center; margin-top: 80px;">
    <h1 style="border: none; margin-bottom: 20px;">QUANTA SHOP</h1>
    <h2 style="color: #98c73a; margin-bottom: 40px;">Especificação Técnica Completa</h2>
    <p style="font-size: 1.2em; color: #2f7785; margin: 30px 0;">Plataforma de Cashback e Afiliados</p>
    <p style="margin-top: 60px; color: #666;">Documento para migração ao Lovable</p>
    <p style="color: #666; margin-top: 20px;">19 de março de 2026</p>
  </div>
</div>

<!-- SEÇÃO 1: VISÃO GERAL -->
<div class="page">
  <h1>1. Visão Geral do Sistema</h1>
  
  <h3>Sistema</h3>
  <p><strong>Quanta Shop</strong> é uma plataforma de cashback e afiliados que permite usuários ganhar renda passiva através de indicações e compras.</p>
  
  <h3>URL de Produção</h3>
  <p><code>quantashop.com.br</code></p>
  
  <h3>Stack Atual</h3>
  <ul>
    <li><strong>Frontend:</strong> Nuxt 3 (Vue 3 + TypeScript + Bootstrap 5 + SCSS + Pinia)</li>
    <li><strong>Backend:</strong> .NET 8 (MMN.Api, porta 8000, Azure SQL)</li>
    <li><strong>Proxy:</strong> Nitro server-side (<code>/api-proxy/*</code>) para evitar CORS</li>
    <li><strong>Renderização:</strong> SPA client-side (SSR desabilitado)</li>
    <li><strong>Autenticação:</strong> JWT em localStorage (chave: <code>agencia_user</code>)</li>
  </ul>
  
  <h3>Recomendação para Lovable</h3>
  <ul>
    <li><strong>Framework:</strong> React 18 + TypeScript</li>
    <li><strong>Estilização:</strong> Tailwind CSS</li>
    <li><strong>Roteamento:</strong> React Router v6</li>
    <li><strong>Estado Global:</strong> Zustand</li>
    <li><strong>HTTP Client:</strong> Axios (com interceptor de token)</li>
    <li><strong>Formulários:</strong> React Hook Form + Zod</li>
    <li><strong>Tabelas:</strong> TanStack Table</li>
    <li><strong>Gráficos:</strong> Recharts</li>
    <li><strong>Componentes UI:</strong> Headless UI / Radix UI</li>
  </ul>
  
  <div class="highlight">
    <strong>⚠️ IMPORTANTE para integração Replit:</strong> Todas as chamadas de API devem usar paths relativos (<code>/api-proxy/*</code>) para funcionar com o proxy Nitro existente. Não hardcode URLs absolutas. O token JWT deve ser lido de localStorage na chave <code>agencia_user</code>.
  </div>
</div>

<!-- SEÇÃO 2: IDENTIDADE VISUAL -->
<div class="page">
  <h1>2. Identidade Visual</h1>
  
  <table>
    <thead>
      <tr>
        <th>Token</th>
        <th>Valor</th>
        <th>Uso</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><span class="color-box" style="background: #2f7785;"></span>Primary</td>
        <td><code>#2f7785</code></td>
        <td>Botões, links, títulos principais</td>
      </tr>
      <tr>
        <td><span class="color-box" style="background: #225f6b;"></span>Primary Dark</td>
        <td><code>#225f6b</code></td>
        <td>Hover, ênfase, bordas</td>
      </tr>
      <tr>
        <td><span class="color-box" style="background: #98c73a;"></span>Secondary</td>
        <td><code>#98c73a</code></td>
        <td>Acento, sucesso, cashback, fundos</td>
      </tr>
      <tr>
        <td><span class="color-box" style="background: #ecf2f7;"></span>Background</td>
        <td><code>#ecf2f7</code></td>
        <td>Fundo do painel, cards secundários</td>
      </tr>
      <tr>
        <td><span class="color-box" style="background: #ffffff; border: 2px solid #ccc;"></span>White</td>
        <td><code>#ffffff</code></td>
        <td>Cards principais, modais, fundo principal</td>
      </tr>
      <tr>
        <td>Text Principal</td>
        <td><code>#212529</code></td>
        <td>Corpo de texto, parágrafos</td>
      </tr>
    </tbody>
  </table>
</div>

<!-- SEÇÃO 3: PÁGINAS PÚBLICAS -->
<div class="page">
  <h1>3. Páginas Públicas</h1>
  
  <h3>/agencia</h3>
  <p>Landing page institucional da agência Quanta Shop com seções de hero, como funciona, benefícios e CTA para login/cadastro.</p>
  
  <h3>/agencia/login</h3>
  <ul>
    <li><strong>Campos:</strong> Login ou e-mail, Senha</li>
    <li><strong>API:</strong> <code>POST /usuario/autenticar</code></li>
    <li><strong>Armazenamento:</strong> JWT em localStorage (chave: <code>agencia_user</code>)</li>
    <li><strong>Links:</strong> "Esqueci minha senha", "Não tem cadastro?"</li>
    <li><strong>Redirect:</strong> <code>/agencia/painel</code> após sucesso</li>
  </ul>
  
  <h3>/agencia/cadastro</h3>
  <ul>
    <li><strong>Campos:</strong> Nome, login, e-mail, senha, confirmação, CPF, telefone, indicador</li>
    <li><strong>API:</strong> <code>POST /usuario/cadastrar</code></li>
  </ul>
  
  <h3>/agencia/recuperar-senha</h3>
  <ul>
    <li><strong>Campo:</strong> E-mail</li>
    <li><strong>API:</strong> <code>POST /usuario/recuperarSenha</code></li>
  </ul>
  
  <h3>/agencia/quem-somos</h3>
  <p>Página institucional textual sobre a empresa.</p>
  
  <h3>/agencia/como-funciona</h3>
  <p>Explicação do modelo de negócio (cashback, afiliados, ecossistema).</p>
  
  <h3>/agencia/faq</h3>
  <p>Perguntas frequentes em accordeão.</p>
  
  <h3>/agencia/privacidade</h3>
  <p>Política de privacidade.</p>
  
  <h3>/agencia/lojas-fisicas</h3>
  <ul>
    <li><strong>API:</strong> <code>GET /LojasFisicas/obterLojasFisicas</code></li>
    <li><strong>Componente:</strong> Mapa interativo (Google Maps ou Leaflet)</li>
  </ul>
  
  <h3>/agencia/mais-vendas/[[login]]</h3>
  <ul>
    <li><strong>Parâmetro:</strong> Login de afiliado (opcional)</li>
    <li><strong>API:</strong> <code>GET /Ranking/obterMaisVendas/{login?}</code></li>
  </ul>
</div>

<!-- SEÇÃO 4: PAINEL DO AFILIADO -->
<div class="page">
  <h1>4. Painel do Afiliado</h1>
  
  <p><strong>Layout:</strong> Sidebar fixa (esquerda) + TopBar (direita) + área de conteúdo.</p>
  <p><strong>Proteção:</strong> Middleware de autenticação client-side.</p>
  
  <h3>/agencia/painel (Dashboard)</h3>
  <ul>
    <li><strong>Stats (4 cards):</strong> Equipe, Saldo, Ecossistemas, Pontos — <code>GET /v2/dashboard/get-totalizers-user</code></li>
    <li><strong>Meta Minuto:</strong> Contador regressivo — <code>GET /MetaMinuto/obterMetaMinuto</code></li>
    <li><strong>Vídeos:</strong> Modais YouTube (bem-vindo, como funciona, como ganhar)</li>
    <li><strong>Mapa:</strong> Iframe Google Maps</li>
    <li><strong>Link de Indicação:</strong> Input copiável</li>
  </ul>
  
  <h3>/agencia/painel/meus-dados</h3>
  <ul>
    <li><strong>Abas:</strong> Dados pessoais, Endereço, Conta bancária, Senha</li>
    <li><strong>API:</strong> <code>GET /usuario/obterDados</code>, <code>PUT /usuario/alterarDados</code></li>
  </ul>
  
  <h3>/agencia/painel/financeiro (CCR)</h3>
  <ul>
    <li><strong>Aba 1 - Movimentações:</strong> Extrato financeiro — <code>GET /financeiro/listaMovimentacoes</code></li>
    <li><strong>Aba 2 - Solicitar Saque:</strong> Formulário com valor — <code>POST /saque/solicitar</code></li>
    <li><strong>Aba 3 - Histórico de Saques:</strong> Tabela de saques — <code>GET /saque/historico</code></li>
  </ul>
  
  <h3>/agencia/painel/minhas-compras</h3>
  <ul>
    <li><strong>Filtros:</strong> Descrição, Data compra (início/fim)</li>
    <li><strong>Tabela:</strong> Produto, valor, cashback, data, status, detalhes</li>
    <li><strong>Modal Detalhes:</strong> Movimentação + distribuição do cashback</li>
    <li><strong>API:</strong> <code>POST /Pedidos/listaPedidosAfiliados</code></li>
  </ul>
  
  <h3>/agencia/painel/minha-rede</h3>
  <ul>
    <li><strong>Stats:</strong> Total, Diretos, Inativos, VIPs</li>
    <li><strong>Tabela:</strong> Login, nome, nível, status, data</li>
    <li><strong>API:</strong> <code>GET /Rede/obterMinhaRede</code></li>
  </ul>
  
  <h3>/agencia/painel/meus-diretos</h3>
  <ul>
    <li><strong>Tabela:</strong> Login, nome, graduação, status, data</li>
    <li><strong>API:</strong> <code>GET /Rede/obterMeusDiretos</code></li>
  </ul>
  
  <h3>/agencia/painel/performance</h3>
  <ul>
    <li><strong>Filtros:</strong> Radio (10 primeiros/últimos por consumo), login</li>
    <li><strong>Tabela:</strong> Login, nome, graduação, consumo, nível</li>
    <li><strong>API:</strong> <code>POST /Dashboard/obterPerformance</code></li>
  </ul>
  
  <h3>/agencia/painel/suporte</h3>
  <ul>
    <li><strong>Filtros:</strong> Status (1-7), Tipo (1-4), Data (início/fim)</li>
    <li><strong>Status:</strong> 1=Processamento, 2=Finalizado, 3=Cancelado, 4=Aprovação, 5=Recusado, 6=Aprovado, 7=Pagamento</li>
    <li><strong>Tipos:</strong> 1=Contato, 2=Cashback não pago, 3=Cancelamento, 4=Reabertura</li>
    <li><strong>API:</strong> <code>POST /Suporte/listaSuporte</code></li>
  </ul>
  
  <h3>/agencia/painel/solicitar-suporte</h3>
  <ul>
    <li><strong>Campos:</strong> Tipo, assunto, descrição, anexo (opcional)</li>
    <li><strong>API:</strong> <code>POST /Suporte/abrirSuporte</code></li>
  </ul>
  
  <h3>Demais Páginas do Painel</h3>
  <p>Outras páginas incluem: Graduações, Contas Bancárias, Assinatura, Planos, Cupons, Meus Cupons, Gerar Cupons, Inserir Cupom, Material de Apoio, Tutoriais, FAQ, Comerciante, Meus Credenciamentos.</p>
</div>

<!-- SEÇÃO 5: PAINEL ADMIN -->
<div class="page">
  <h1>5. Painel Administrativo</h1>
  
  <p><strong>Rota base:</strong> <code>/agencia/painel/admin/**</code></p>
  <p><strong>Proteção:</strong> Middleware adicional que valida perfil admin no JWT.</p>
  
  <h3>/agencia/painel/admin (Dashboard)</h3>
  <p>Cards com estatísticas: total usuários, compras, cashback pago, estabelecimentos ativos.</p>
  
  <h3>Páginas Admin</h3>
  <ul>
    <li><strong>Usuários:</strong> Busca, tabela, ações (editar, bloquear)</li>
    <li><strong>Credenciamento:</strong> Solicitações (Pendente/Aprovado/Reprovado)</li>
    <li><strong>Compras:</strong> Relatório de compras com filtros</li>
    <li><strong>Pagamentos:</strong> Gestão de pagamentos</li>
    <li><strong>Categorias:</strong> CRUD de categorias</li>
    <li><strong>Ecossistemas:</strong> Gestão de ecossistemas</li>
    <li><strong>Carrosseis:</strong> CRUD de banners</li>
    <li><strong>Comunicados:</strong> Envio de notificações</li>
    <li><strong>Rede:</strong> Visualização da árvore de afiliados</li>
    <li><strong>Suporte Admin:</strong> Gestão de tickets</li>
    <li><strong>Lojas Credenciados:</strong> Lista de estabelecimentos</li>
    <li><strong>Relatórios:</strong> Faturas, Anunciantes, Cashback</li>
    <li><strong>Mais:</strong> Aniversariantes, Acessos, Lançamentos, Material, Assinaturas, Grupos</li>
  </ul>
</div>

<!-- SEÇÃO 6: COMPONENTES COMPARTILHADOS -->
<div class="page">
  <h1>6. Componentes Compartilhados para Lovable</h1>
  
  <table>
    <thead>
      <tr>
        <th>Componente</th>
        <th>Descrição</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>&lt;Sidebar&gt;</td>
        <td>Menu lateral com navegação dinâmica e submenu colapsável</td>
      </tr>
      <tr>
        <td>&lt;TopBar&gt;</td>
        <td>Barra superior com link de indicação e informações do usuário</td>
      </tr>
      <tr>
        <td>&lt;StatCard&gt;</td>
        <td>Card com ícone, label e valor numérico</td>
      </tr>
      <tr>
        <td>&lt;PageHeader&gt;</td>
        <td>Título + subtítulo de página</td>
      </tr>
      <tr>
        <td>&lt;FilterBox&gt;</td>
        <td>Container de filtros com botão aplicar</td>
      </tr>
      <tr>
        <td>&lt;DataTable&gt;</td>
        <td>Tabela responsiva com loading/empty states</td>
      </tr>
      <tr>
        <td>&lt;StatusBadge&gt;</td>
        <td>Badge colorido por status numérico</td>
      </tr>
      <tr>
        <td>&lt;LoadingSpinner&gt;</td>
        <td>Indicador de carregamento</td>
      </tr>
      <tr>
        <td>&lt;EmptyState&gt;</td>
        <td>Estado vazio com ícone e mensagem</td>
      </tr>
      <tr>
        <td>&lt;Modal&gt;</td>
        <td>Modal genérico com header/body/footer</td>
      </tr>
      <tr>
        <td>&lt;ConfirmDialog&gt;</td>
        <td>Dialog de confirmação de ação destrutiva</td>
      </tr>
    </tbody>
  </table>
</div>

<!-- SEÇÃO 7: AUTENTICAÇÃO -->
<div class="page">
  <h1>7. Fluxo de Autenticação</h1>
  
  <ol>
    <li>Usuário submete credenciais → <code>POST /usuario/autenticar</code></li>
    <li>API retorna JWT + dados do usuário</li>
    <li>Frontend armazena em localStorage: chave <code>agencia_user</code> (JSON com token, nome, login, perfil)</li>
    <li>Todas as requisições autenticadas enviam: <code>Authorization: Bearer {token}</code></li>
    <li>Middleware client-side valida token antes de renderizar páginas protegidas</li>
    <li>Token expirado → redirect para <code>/agencia/login</code></li>
    <li>Perfil admin: campo adicional no JWT; verificado para rotas <code>/admin</code></li>
  </ol>
  
  <div class="highlight">
    <strong>Recomendação:</strong> Implementar refresh token (opcional) para melhorar UX e segurança.
  </div>
</div>

<!-- SEÇÃO 8: ENDPOINTS PRINCIPAIS -->
<div class="page">
  <h1>8. Endpoints de API Principais</h1>
  
  <table>
    <thead>
      <tr>
        <th>Método</th>
        <th>Endpoint</th>
        <th>Uso</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>POST</td>
        <td>/usuario/autenticar</td>
        <td>Login</td>
      </tr>
      <tr>
        <td>POST</td>
        <td>/usuario/cadastrar</td>
        <td>Cadastro</td>
      </tr>
      <tr>
        <td>GET</td>
        <td>/usuario/obterDados</td>
        <td>Dados do perfil</td>
      </tr>
      <tr>
        <td>PUT</td>
        <td>/usuario/alterarDados</td>
        <td>Atualizar perfil</td>
      </tr>
      <tr>
        <td>GET</td>
        <td>/v2/dashboard/get-totalizers-user</td>
        <td>Stats do painel</td>
      </tr>
      <tr>
        <td>GET</td>
        <td>/MetaMinuto/obterMetaMinuto</td>
        <td>Meta minuto</td>
      </tr>
      <tr>
        <td>GET</td>
        <td>/financeiro/saldo</td>
        <td>Saldo disponível</td>
      </tr>
      <tr>
        <td>GET</td>
        <td>/financeiro/listaMovimentacoes</td>
        <td>Extrato</td>
      </tr>
      <tr>
        <td>POST</td>
        <td>/saque/solicitar</td>
        <td>Solicitar saque</td>
      </tr>
      <tr>
        <td>GET</td>
        <td>/saque/historico</td>
        <td>Histórico saques</td>
      </tr>
      <tr>
        <td>POST</td>
        <td>/Pedidos/listaPedidosAfiliados</td>
        <td>Minhas compras</td>
      </tr>
      <tr>
        <td>GET</td>
        <td>/Rede/obterMinhaRede</td>
        <td>Minha rede</td>
      </tr>
      <tr>
        <td>POST</td>
        <td>/Dashboard/obterPerformance</td>
        <td>Performance</td>
      </tr>
      <tr>
        <td>POST</td>
        <td>/Suporte/listaSuporte</td>
        <td>Lista suporte</td>
      </tr>
      <tr>
        <td>POST</td>
        <td>/Suporte/abrirSuporte</td>
        <td>Abrir chamado</td>
      </tr>
      <tr>
        <td>GET</td>
        <td>/geral/obterMenu/{perfil}</td>
        <td>Menu dinâmico</td>
      </tr>
    </tbody>
  </table>
</div>

<!-- SEÇÃO 9: INTEGRAÇÃO LOVABLE → REPLIT -->
<div class="page">
  <h1>9. Instruções para Integração Lovable → Replit</h1>
  
  <ol>
    <li><strong>NÃO alterar</strong> a estrutura de rotas da API .NET (mantida no Replit)</li>
    <li><strong>Usar SEMPRE paths relativos</strong> <code>/api-proxy/*</code> (nunca URLs absolutas)</li>
    <li>O proxy Nitro em <code>server/routes/api-proxy/[...path].ts</code> já está configurado</li>
    <li><strong>Manter localStorage</strong> com chave <code>agencia_user</code> para compatibilidade</li>
    <li>Ao integrar no Replit: copiar componentes React para <code>components/lovable/</code> ou criar sub-domínio separado</li>
    <li>CORS na API .NET já ajustado para aceitar o domínio Replit</li>
    <li>Testar autenticação e cache do menu dinâmico antes de deploy</li>
    <li>Validar que o token JWT está sendo enviado em todas as requisições autenticadas</li>
  </ol>
  
  <h3>Estrutura de Pastas Sugerida</h3>
  <pre><code>src/
├── pages/
│   ├── auth/
│   │   ├── Login.tsx
│   │   ├── Register.tsx
│   │   └── RecoverPassword.tsx
│   └── agencia/
│       ├── Painel.tsx
│       ├── MeusDados.tsx
│       ├── Financeiro.tsx
│       └── ...
├── components/
│   ├── layout/
│   │   ├── Sidebar.tsx
│   │   └── TopBar.tsx
│   └── shared/
│       ├── StatCard.tsx
│       ├── DataTable.tsx
│       └── ...
├── hooks/
│   ├── useAuth.ts
│   └── useApi.ts
├── services/
│   └── api.ts
└── store/
    └── authStore.ts</code></pre>
</div>

<!-- SEÇÃO 10: PRÓXIMOS PASSOS -->
<div class="page">
  <h1>10. Próximos Passos</h1>
  
  <h3>Fase 1: Design & Prototipagem</h3>
  <ul>
    <li>Criar design system no Figma com componentes Tailwind</li>
    <li>Validar paleta de cores com stakeholders</li>
    <li>Fazer protótipos de fluxo crítico (login → painel → financeiro)</li>
  </ul>
  
  <h3>Fase 2: Desenvolvimento Lovable</h3>
  <ul>
    <li>Scaffolding do projeto React + Tailwind + Router v6</li>
    <li>Implementar componentes base (Sidebar, TopBar, DataTable)</li>
    <li>Autenticação e interceptor de token</li>
    <li>Páginas públicas (login, cadastro, landing)</li>
    <li>Painel do afiliado (meus dados, financeiro, compras)</li>
    <li>Painel admin (dashboard, usuários, credenciamento)</li>
  </ul>
  
  <h3>Fase 3: Testes & QA</h3>
  <ul>
    <li>Testes unitários dos hooks customizados</li>
    <li>Testes de integração das páginas com API</li>
    <li>Testes E2E dos fluxos críticos</li>
    <li>Validação de responsividade (mobile/tablet/desktop)</li>
  </ul>
  
  <h3>Fase 4: Integração Replit</h3>
  <ul>
    <li>Copiar/importar build do Lovable para Replit</li>
    <li>Configurar proxy API e variáveis de ambiente</li>
    <li>Deploy e validação em produção</li>
  </ul>
  
  <div class="footer">
    <p><strong>Quanta Shop</strong> — Especificação Técnica Completa</p>
    <p>Este documento foi gerado automaticamente e contém informações técnicas para guiar a migração do sistema para a plataforma Lovable.</p>
    <p>Data: 19 de março de 2026</p>
  </div>
</div>

</body>
</html>`;

async function generatePDF() {
  try {
    const browser = await puppeteer.launch({ 
      headless: 'new',
      args: ['--no-sandbox', '--disable-setuid-sandbox'] 
    });
    
    const page = await browser.newPage();
    await page.setContent(htmlContent, { waitUntil: 'networkidle0' });
    
    const outputPath = path.join(__dirname, '..', 'docs', 'quanta-shop-technical-spec.pdf');
    
    await page.pdf({
      path: outputPath,
      format: 'A4',
      printBackground: true,
      margin: {
        top: '20px',
        right: '20px',
        bottom: '20px',
        left: '20px'
      }
    });
    
    await browser.close();
    
    console.log(`✅ PDF gerado com sucesso: ${outputPath}`);
    return outputPath;
  } catch (error) {
    console.error('❌ Erro ao gerar PDF:', error);
    process.exit(1);
  }
}

generatePDF();
