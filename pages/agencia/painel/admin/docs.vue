<template>
  <div class="docs-page">

    <!-- Header -->
    <div class="ag-page-header d-flex align-items-start justify-content-between flex-wrap gap-3 no-print">
      <div>
        <h1>Documentação Técnica</h1>
        <p>Arquitetura, padrões e status do projeto Quanta Shop — atualizado Mai/2026</p>
      </div>
      <div class="d-flex gap-2 flex-wrap">
        <button class="btn btn-ag-outline" @click="copiarClaudeMd">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" class="me-1"><rect x="9" y="9" width="13" height="13" rx="2"/><path d="M5 15H4a2 2 0 01-2-2V4a2 2 0 012-2h9a2 2 0 012 2v1"/></svg>
          {{ copiado ? 'Copiado!' : 'Copiar CLAUDE.md' }}
        </button>
        <button class="btn btn-ag-primary" @click="exportarPDF">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" class="me-1"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="12" y1="18" x2="12" y2="12"/><line x1="9" y1="15" x2="15" y2="15"/></svg>
          Baixar PDF
        </button>
      </div>
    </div>

    <!-- Alerta de ambiente -->
    <div class="docs-env-badge no-print mb-4">
      <span class="badge-env" :class="isDev ? 'badge-env--dev' : 'badge-env--prod'">
        <span class="badge-env__dot"></span>
        {{ isDev ? 'Ambiente de Desenvolvimento (Replit)' : 'Ambiente de Produção' }}
      </span>
      <span class="docs-env-note" v-if="isDev">
        Escritas no admin ficam restritas ao localStorage — banco de produção protegido.
      </span>
    </div>

    <!-- Seção 1: Visão Geral -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('visao')">
        <div class="docs-card__title">
          <span class="docs-icon">🏗️</span>
          Arquitetura &amp; Stack
          <span class="badge-ag badge-ag-success ms-2">Ativo</span>
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.visao }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.visao" class="docs-card__body">
        <div class="docs-grid-2">
          <div>
            <h6 class="docs-section-label">Frontend</h6>
            <ul class="docs-list">
              <li><strong>Nuxt 3.8.1</strong> + Vue 3 Composition API</li>
              <li>Pinia 2.1.6 (estado global)</li>
              <li>Bootstrap 5.3.0 (CSS principal)</li>
              <li>Swiper.js 10.0.4 (carrosséis)</li>
              <li>SCSS customizado (<code>assets/scss/</code>)</li>
              <li>Tailwind CSS (escopo restrito a <code>.v2-page</code>)</li>
            </ul>
          </div>
          <div>
            <h6 class="docs-section-label">Backend</h6>
            <ul class="docs-list">
              <li><strong>.NET 8</strong> (ASP.NET Core)</li>
              <li><strong>PostgreSQL</strong> (banco de produção)</li>
              <li>JWT Authentication</li>
              <li>Nitro proxy (<code>server/routes/api-proxy/</code>)</li>
              <li>API Produção: <code>api.quantashop.com.br</code></li>
              <li>API Local Dev: <code>localhost:8000</code></li>
            </ul>
          </div>
        </div>
        <div class="docs-highlight mt-3">
          <strong>Proxy de API (risco mapeado):</strong> Em dev, o proxy tenta a API local e, ao falhar, cai para a API de produção automaticamente.
          Por isso, escritas do admin no Replit ficam restritas ao <code>localStorage</code> via <code>import.meta.dev</code>.
        </div>
      </div>
    </div>

    <!-- Seção 2: Design System -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('design')">
        <div class="docs-card__title">
          <span class="docs-icon">🎨</span>
          Design System
          <span class="badge-ag badge-ag-success ms-2">Ativo</span>
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.design }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.design" class="docs-card__body">
        <div class="docs-grid-2">
          <div>
            <h6 class="docs-section-label">Paleta de Cores</h6>
            <div class="docs-colors">
              <div class="docs-color-item">
                <div class="docs-color-swatch" style="background:#2F7785"></div>
                <div><strong>Teal</strong><br><code>#2F7785</code></div>
              </div>
              <div class="docs-color-item">
                <div class="docs-color-swatch" style="background:#225F6B"></div>
                <div><strong>Dark Teal</strong><br><code>#225F6B</code></div>
              </div>
              <div class="docs-color-item">
                <div class="docs-color-swatch" style="background:#98C73A"></div>
                <div><strong>Lime</strong><br><code>#98C73A</code></div>
              </div>
              <div class="docs-color-item">
                <div class="docs-color-swatch" style="background:#1d1d1f"></div>
                <div><strong>Apple Black</strong><br><code>#1d1d1f</code></div>
              </div>
            </div>
          </div>
          <div>
            <h6 class="docs-section-label">Classes Utilitárias</h6>
            <ul class="docs-list">
              <li><code>.ag-card</code> — card do painel admin</li>
              <li><code>.ag-table</code> — tabela estilizada</li>
              <li><code>.ag-page-header</code> — cabeçalho de página</li>
              <li><code>.badge-ag-success/warning/danger</code> — badges</li>
              <li><code>.btn-ag-primary/outline</code> — botões</li>
              <li><code>.qs-*</code> — design system público</li>
              <li><code>.qb-*</code> — componentes do blog</li>
              <li><code>.qbd-*</code> — componentes do detalhe do blog</li>
            </ul>
          </div>
        </div>
        <div class="mt-3">
          <h6 class="docs-section-label">Tipografia</h6>
          <ul class="docs-list docs-list--inline">
            <li>Páginas públicas: <code>Inter</code>, <code>Jost</code>, <code>sans-serif</code></li>
            <li>Blog (Apple-style): <code>-apple-system</code>, <code>BlinkMacSystemFont</code>, <code>Inter</code></li>
            <li>Admin: herança Bootstrap 5</li>
          </ul>
        </div>
      </div>
    </div>

    <!-- Seção 3: Páginas Públicas -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('paginas')">
        <div class="docs-card__title">
          <span class="docs-icon">📄</span>
          Páginas Públicas
          <span class="badge-ag badge-ag-success ms-2">{{ paginasPublicas.length }} rotas</span>
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.paginas }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.paginas" class="docs-card__body">
        <div class="table-responsive">
          <table class="table ag-table">
            <thead>
              <tr><th>Rota</th><th>Descrição</th><th>Layout</th><th>Status</th></tr>
            </thead>
            <tbody>
              <tr v-for="p in paginasPublicas" :key="p.rota">
                <td><code>{{ p.rota }}</code></td>
                <td>{{ p.desc }}</td>
                <td><code>{{ p.layout }}</code></td>
                <td><span class="badge-ag" :class="p.status === 'Ativo' ? 'badge-ag-success' : 'badge-ag-warning'">{{ p.status }}</span></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Seção 4: Admin -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('admin')">
        <div class="docs-card__title">
          <span class="docs-icon">🔧</span>
          Painel Admin
          <span class="badge-ag badge-ag-warning ms-2">{{ adminLinks.length }} painéis</span>
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.admin }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.admin" class="docs-card__body">
        <div class="docs-highlight mb-3">
          <strong>Padrão obrigatório:</strong>
          <code>definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] })</code>
        </div>
        <div class="docs-grid-3">
          <div v-for="link in adminLinks" :key="link.rota" class="docs-admin-item">
            <div class="docs-admin-item__rota"><code>{{ link.rota }}</code></div>
            <div class="docs-admin-item__desc">{{ link.desc }}</div>
          </div>
        </div>
      </div>
    </div>

    <!-- Seção 5: Blog -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('blog')">
        <div class="docs-card__title">
          <span class="docs-icon">📝</span>
          Blog — Arquitetura Híbrida
          <span class="badge-ag badge-ag-success ms-2">Ativo</span>
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.blog }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.blog" class="docs-card__body">
        <div class="docs-grid-2">
          <div>
            <h6 class="docs-section-label">Fluxo Dev (Replit)</h6>
            <div class="docs-flow">
              <div class="docs-flow__item docs-flow__item--warn">
                <strong>Escrita Admin</strong> → localStorage apenas<br>
                <small>Protege banco de produção (LGPD)</small>
              </div>
              <div class="docs-flow__item docs-flow__item--ok">
                <strong>Leitura Admin/Público</strong> → API → fallback localStorage
              </div>
            </div>
          </div>
          <div>
            <h6 class="docs-section-label">Fluxo Produção</h6>
            <div class="docs-flow">
              <div class="docs-flow__item docs-flow__item--ok">
                <strong>Escrita Admin</strong> → API .NET 8 → PostgreSQL
              </div>
              <div class="docs-flow__item docs-flow__item--ok">
                <strong>Leitura Público</strong> → API .NET 8 → PostgreSQL
              </div>
            </div>
          </div>
        </div>
        <div class="docs-highlight mt-3">
          <strong>⚠️ Regra crítica de roteamento Nuxt 3:</strong>
          Sempre usar <code>pages/blog/index.vue</code> + <code>pages/blog/[id].vue</code>.
          Nunca criar <code>pages/blog.vue</code> — conflita com subrotas dinâmicas.
        </div>
        <ul class="docs-list mt-3">
          <li>localStorage key: <code>qs_blog_artigos</code></li>
          <li>IDs em dev: <code>Date.now()</code> (ex: <code>1775252000000</code>)</li>
          <li>Imagens: base64 via FileReader (PNG/JPG, drag &amp; drop)</li>
          <li>Detecção de ambiente: <code>import.meta.dev</code></li>
        </ul>
      </div>
    </div>

    <!-- Seção 6: API & Proxy -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('api')">
        <div class="docs-card__title">
          <span class="docs-icon">🔌</span>
          API &amp; Proxy
          <span class="badge-ag badge-ag-warning ms-2">Atenção</span>
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.api }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.api" class="docs-card__body">
        <div class="docs-grid-2">
          <div>
            <h6 class="docs-section-label">Variáveis de Ambiente</h6>
            <table class="table ag-table" style="font-size:.8rem">
              <thead><tr><th>Variável</th><th>Dev</th><th>Prod</th></tr></thead>
              <tbody>
                <tr><td><code>NUXT_API_BASE_URL</code></td><td>/api-proxy</td><td>https://api.quantashop.com.br</td></tr>
                <tr><td><code>NUXT_USE_LOCAL_API</code></td><td>true (default)</td><td>false</td></tr>
                <tr><td><code>NUXT_JWT_SECRET</code></td><td>dev-secret-key-*</td><td>⚠️ secret real</td></tr>
              </tbody>
            </table>
          </div>
          <div>
            <h6 class="docs-section-label">Endpoints da API (.NET 8)</h6>
            <ul class="docs-list" style="font-size:.85rem">
              <li><code>GET /admin/blog/artigos</code> — lista artigos (admin)</li>
              <li><code>POST /admin/blog/artigos</code> — cria artigo</li>
              <li><code>PUT /admin/blog/artigos/:id</code> — edita artigo</li>
              <li><code>DELETE /admin/blog/artigos/:id</code> — remove artigo</li>
              <li><code>GET /blog/artigos</code> — listagem pública</li>
              <li><code>POST /auth/login</code> — autenticação JWT</li>
              <li><code>GET /admin/painel/resumo</code> — stats do dashboard</li>
            </ul>
          </div>
        </div>
        <div class="docs-highlight mt-3" style="border-color:#dc3545;background:rgba(220,53,69,.08)">
          <strong>🔒 Regra de Segurança LGPD:</strong> O banco de produção não deve receber dados de teste.
          Nunca desabilitar o isolamento via <code>import.meta.dev</code> sem uma análise de impacto.
          Dados sensíveis (CPF, dados bancários) trafegam apenas via API autenticada + HTTPS.
        </div>
      </div>
    </div>

    <!-- Seção 7: Timeline de Tasks -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('tasks')">
        <div class="docs-card__title">
          <span class="docs-icon">✅</span>
          Histórico de Tasks Concluídas
          <span class="badge-ag badge-ag-success ms-2">101 tasks</span>
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.tasks }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.tasks" class="docs-card__body">
        <div class="docs-timeline">
          <div v-for="sprint in sprints" :key="sprint.nome" class="docs-timeline__sprint">
            <div class="docs-timeline__sprint-header">
              <span class="docs-timeline__sprint-badge">{{ sprint.tasks }}</span>
              {{ sprint.nome }}
            </div>
            <ul class="docs-timeline__list">
              <li v-for="item in sprint.itens" :key="item">{{ item }}</li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <!-- Seção 8: Pendentes -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('pendente')">
        <div class="docs-card__title">
          <span class="docs-icon">🚧</span>
          Em Andamento / Pendente
          <span class="badge-ag badge-ag-warning ms-2">Próximos passos</span>
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.pendente }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.pendente" class="docs-card__body">
        <div class="docs-grid-2">
          <div v-for="item in pendentes" :key="item.task" class="docs-pending-card">
            <div class="docs-pending-card__header">
              <span class="badge-ag" :class="item.prioridade === 'Alta' ? 'badge-ag-danger' : item.prioridade === 'Média' ? 'badge-ag-warning' : 'badge-ag-success'">
                {{ item.prioridade }}
              </span>
              <strong>{{ item.task }}</strong>
            </div>
            <p class="docs-pending-card__desc">{{ item.desc }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Seção 9: Convenções -->
    <div class="docs-card mb-3">
      <div class="docs-card__header" @click="toggle('conv')">
        <div class="docs-card__title">
          <span class="docs-icon">📋</span>
          Convenções de Código
        </div>
        <svg class="docs-chevron" :class="{ open: expanded.conv }" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"/></svg>
      </div>
      <div v-show="expanded.conv" class="docs-card__body">
        <div class="docs-grid-2">
          <div>
            <h6 class="docs-section-label">Vue / Nuxt</h6>
            <ul class="docs-list">
              <li><code>&lt;script setup lang="ts"&gt;</code> obrigatório</li>
              <li><code>definePageMeta</code> sempre no topo</li>
              <li>Não usar Options API</li>
              <li>Imports: <code>useApi()</code>, <code>useAgenciaStore()</code></li>
            </ul>
          </div>
          <div>
            <h6 class="docs-section-label">Nomenclatura</h6>
            <ul class="docs-list">
              <li>Páginas: <code>kebab-case.vue</code></li>
              <li>Componentes: <code>PascalCase.vue</code></li>
              <li>Composables: <code>useNomeCamelCase.ts</code></li>
              <li>Stores: <code>useNomeStore.ts</code></li>
              <li>Commits: <code>feat:</code>, <code>fix:</code>, <code>style:</code>, <code>docs:</code></li>
            </ul>
          </div>
          <div>
            <h6 class="docs-section-label">CSS</h6>
            <ul class="docs-list">
              <li>Público: scoped CSS/SCSS vanilla</li>
              <li>Admin: Bootstrap 5 + <code>.ag-*</code></li>
              <li>Design system: <code>.qs-*</code> (quanta-premium.scss)</li>
              <li>Tailwind: SOMENTE em <code>.v2-page</code></li>
            </ul>
          </div>
          <div>
            <h6 class="docs-section-label">Imagens</h6>
            <ul class="docs-list">
              <li>Externas: sempre via <code>&lt;img&gt;</code>, nunca <code>background-image</code></li>
              <li>Upload admin: base64 DataURL via FileReader</li>
              <li>Drag &amp; drop suportado nos formulários do admin</li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <!-- Footer -->
    <div class="docs-footer no-print">
      <p>Gerado automaticamente pelo agente Replit • Quanta Shop © {{ new Date().getFullYear() }}</p>
      <p><code>CLAUDE.md</code> disponível na raiz do projeto para contexto do Claude Code.</p>
    </div>

    <!-- Print footer -->
    <div class="docs-print-footer print-only">
      <p>Quanta Shop — Documentação Técnica — {{ new Date().toLocaleDateString('pt-BR') }}</p>
    </div>

  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';

definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
useSeoMeta({ title: 'Documentação Técnica | Admin Quanta Shop' });

const isDev = import.meta.dev;
const copiado = ref(false);

const expanded = reactive({
  visao: true,
  design: false,
  paginas: false,
  admin: false,
  blog: true,
  api: false,
  tasks: false,
  pendente: false,
  conv: false,
});

function toggle(key: keyof typeof expanded) {
  expanded[key] = !expanded[key];
}

function exportarPDF() {
  window.print();
}

async function copiarClaudeMd() {
  const md = `# CLAUDE.md — Quanta Shop Platform
Plataforma financeira de cashback + MLM.
Stack: Nuxt 3.8.1 + Vue 3 + Pinia + Bootstrap 5 + .NET 8 + PostgreSQL
Paleta: #2F7785 teal | #98C73A lime | #225F6B dark teal
Dev: npm run dev (porta 5000) | API: bash api/start-api.sh (porta 8000)
Admin: layout agencia-painel, middleware agencia-auth+agencia-admin
Blog: pages/blog/index.vue + pages/blog/[id].vue (NUNCA pages/blog.vue)
Escrita dev: localStorage only via import.meta.dev (protege produção)
Tailwind: SOMENTE em .v2-page (components/home-v2)
LGPD: sem dados sensíveis em localStorage, API com JWT + HTTPS
Ver CLAUDE.md na raiz para documentação completa.`;
  try {
    await navigator.clipboard.writeText(md);
    copiado.value = true;
    setTimeout(() => { copiado.value = false; }, 2500);
  } catch { /**/ }
}

const paginasPublicas = [
  { rota: '/', desc: 'Home V1 (padrão ativo)', layout: 'layout-home', status: 'Ativo' },
  { rota: '/para-voce', desc: 'Para Consumidores', layout: 'layout-home', status: 'Ativo' },
  { rota: '/para-sua-empresa', desc: 'Para Lojistas', layout: 'layout-home', status: 'Ativo' },
  { rota: '/seja-um-agente', desc: 'Seja um Agente', layout: 'layout-home', status: 'Ativo' },
  { rota: '/quanta-amizade', desc: 'Programa de Indicações', layout: 'layout-home', status: 'Ativo' },
  { rota: '/blog', desc: 'Listagem do Blog (Apple-style)', layout: 'layout-home', status: 'Ativo' },
  { rota: '/blog/:id', desc: 'Artigo do Blog', layout: 'layout-home', status: 'Ativo' },
  { rota: '/shop', desc: 'Loja de Produtos', layout: 'layout-home', status: 'Ativo' },
  { rota: '/partners', desc: 'Parceiros', layout: 'layout-home', status: 'Ativo' },
  { rota: '/agencia', desc: 'Landing da Agência', layout: 'agencia', status: 'Ativo' },
  { rota: '/agencia/login', desc: 'Login do Agente', layout: 'agencia-login', status: 'Ativo' },
  { rota: '/agencia/cadastro', desc: 'Cadastro do Agente', layout: 'agencia', status: 'Ativo' },
  { rota: '/home-v2', desc: 'Home V2 (ABANDONADA)', layout: 'layout-home', status: 'Inativo' },
];

const adminLinks = [
  { rota: '/agencia/painel/admin', desc: 'Dashboard — stats e acesso rápido' },
  { rota: '/agencia/painel/admin/usuarios', desc: 'Gestão de usuários cadastrados' },
  { rota: '/agencia/painel/admin/pagamentos', desc: 'Aprovação de pagamentos e saques' },
  { rota: '/agencia/painel/admin/compras', desc: 'Gestão de compras' },
  { rota: '/agencia/painel/admin/credenciamento', desc: 'Credenciamento de parceiros/lojas' },
  { rota: '/agencia/painel/admin/categorias', desc: 'Categorias de lojas' },
  { rota: '/agencia/painel/admin/ecossistemas', desc: 'Ecossistemas da plataforma' },
  { rota: '/agencia/painel/admin/carrosseis', desc: 'Banners e carrosséis do portal' },
  { rota: '/agencia/painel/admin/home-cms', desc: 'CMS — textos e seções da home' },
  { rota: '/agencia/painel/admin/marcas-home', desc: 'Logos do carrossel de marcas' },
  { rota: '/agencia/painel/admin/blog', desc: 'CRUD do blog com upload de imagem' },
  { rota: '/agencia/painel/admin/redes-sociais', desc: 'Posts Instagram, YouTube, TikTok' },
  { rota: '/agencia/painel/admin/suporte', desc: 'Tickets de suporte' },
  { rota: '/agencia/painel/admin/rede', desc: 'Visualização da rede MLM multinível' },
  { rota: '/agencia/painel/admin/comunicados', desc: 'Comunicados para usuários' },
  { rota: '/agencia/painel/admin/docs', desc: '📋 Esta documentação técnica' },
];

const sprints = [
  {
    nome: 'Sprint Infraestrutura (#1–#24)',
    tasks: '#1–24',
    itens: [
      'Migração Vue 2 → Nuxt 3 (SSR + SPA)',
      'Deploy unificado Nuxt + .NET 8 API',
      'Configuração JWT real em produção',
      'Correção de links hardcoded',
      'Restauração do layout da agência',
      'Autenticação JWT (login, middleware, guards)',
      'Proxy reverso API com fallback remoto',
    ]
  },
  {
    nome: 'Sprint Documentação (#26–#27)',
    tasks: '#26–27',
    itens: [
      'Documento técnico PDF (Quanta Shop → Lovable)',
      'Auditoria Técnica AWIN em PDF',
    ]
  },
  {
    nome: 'Sprint Design Premium (#75–#101)',
    tasks: '#75–101',
    itens: [
      'Design System — componente QsHero fullscreen',
      'Redesign: Para Você, Para sua Empresa, Seja um Agente, Quanta Amizade',
      'Admin: home-cms.vue (textos editáveis)',
      'Admin: marcas-home.vue (logos)',
      'Admin: carrosséis com hero banners JSON',
      'Blog completo: listagem + detalhe + admin + upload imagem PNG/JPG',
      'Correção roteamento blog (blog.vue → blog/index.vue)',
      'Blog redesign Apple-style (hero preto, cards clean)',
      'Fix home-blog.vue (resolveComponent NuxtLink)',
      'Remoção imagem de fundo hardcoded do hero',
      '#102 e #103: documentação + blog API híbrido',
    ]
  },
];

const pendentes = [
  { task: '#81 — CMS de Copy', prioridade: 'Alta', desc: 'Textos editáveis em todas as páginas públicas via painel admin.' },
  { task: '#97 — Remover seção CEO', prioridade: 'Média', desc: 'Eliminar seção CEO da home V1.' },
  { task: '#103 — Blog → API real', prioridade: 'Alta', desc: 'Conectar leitura pública do blog à API .NET 8 com fallback localStorage.' },
  { task: '#28 — Arquitetura para Lovable', prioridade: 'Baixa', desc: 'Documento de arquitetura de telas para migração ao Lovable.' },
];
</script>

<style scoped>
/* ── Base ──────────────────────────────────────────────── */
.docs-page {
  font-family: -apple-system, BlinkMacSystemFont, 'Inter', sans-serif;
}

/* ── Env Badge ─────────────────────────────────────────── */
.docs-env-badge {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}
.badge-env {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 6px 16px;
  border-radius: 999px;
  font-size: 13px;
  font-weight: 600;
}
.badge-env--dev { background: rgba(255,193,7,.15); color: #856404; border: 1px solid rgba(255,193,7,.4); }
.badge-env--prod { background: rgba(25,135,84,.12); color: #0a6640; border: 1px solid rgba(25,135,84,.3); }
.badge-env__dot {
  width: 8px; height: 8px; border-radius: 50%; background: currentColor;
  animation: pulse-dot 1.8s infinite;
}
@keyframes pulse-dot { 0%,100%{opacity:1;transform:scale(1)}50%{opacity:.5;transform:scale(1.3)} }
.docs-env-note { font-size: 12px; color: #6c757d; }

/* ── Card ──────────────────────────────────────────────── */
.docs-card {
  background: #fff;
  border-radius: 12px;
  border: 1px solid rgba(0,0,0,.07);
  box-shadow: 0 1px 4px rgba(0,0,0,.05);
  overflow: hidden;
}
.docs-card__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 20px;
  cursor: pointer;
  user-select: none;
  transition: background .15s;
}
.docs-card__header:hover { background: rgba(0,0,0,.02); }
.docs-card__title {
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
  font-size: .95rem;
  color: #1d1d1f;
}
.docs-icon { font-size: 1.1rem; }
.docs-chevron {
  transition: transform .2s;
  color: #6c757d;
  flex-shrink: 0;
}
.docs-chevron.open { transform: rotate(180deg); }
.docs-card__body {
  padding: 20px;
  border-top: 1px solid rgba(0,0,0,.06);
}

/* ── Layouts ───────────────────────────────────────────── */
.docs-grid-2 { display: grid; grid-template-columns: 1fr 1fr; gap: 24px; }
.docs-grid-3 { display: grid; grid-template-columns: repeat(3, 1fr); gap: 12px; }
@media (max-width: 768px) {
  .docs-grid-2, .docs-grid-3 { grid-template-columns: 1fr; }
}

/* ── Section Label ─────────────────────────────────────── */
.docs-section-label {
  font-size: 11px;
  font-weight: 700;
  letter-spacing: .07em;
  text-transform: uppercase;
  color: #6c757d;
  margin-bottom: 10px;
}

/* ── List ──────────────────────────────────────────────── */
.docs-list { padding-left: 18px; margin: 0; font-size: .875rem; line-height: 1.8; }
.docs-list code { background: rgba(47,119,133,.08); padding: 1px 5px; border-radius: 4px; font-size: .8rem; }
.docs-list--inline { columns: 2; }

/* ── Highlight ─────────────────────────────────────────── */
.docs-highlight {
  background: rgba(47,119,133,.07);
  border-left: 3px solid #2F7785;
  border-radius: 0 8px 8px 0;
  padding: 12px 16px;
  font-size: .85rem;
  line-height: 1.6;
}
.docs-highlight code { background: rgba(47,119,133,.12); padding: 1px 5px; border-radius: 4px; }

/* ── Colors ────────────────────────────────────────────── */
.docs-colors { display: flex; gap: 16px; flex-wrap: wrap; }
.docs-color-item { display: flex; align-items: center; gap: 10px; font-size: .85rem; }
.docs-color-swatch { width: 32px; height: 32px; border-radius: 8px; border: 1px solid rgba(0,0,0,.1); flex-shrink: 0; }

/* ── Admin items ───────────────────────────────────────── */
.docs-admin-item {
  background: #f8f9fa;
  border-radius: 8px;
  padding: 10px 12px;
  font-size: .8rem;
}
.docs-admin-item__rota code { color: #2F7785; font-size: .75rem; }
.docs-admin-item__desc { color: #6c757d; margin-top: 2px; }

/* ── Flow ──────────────────────────────────────────────── */
.docs-flow { display: flex; flex-direction: column; gap: 8px; }
.docs-flow__item {
  padding: 10px 14px;
  border-radius: 8px;
  font-size: .85rem;
  line-height: 1.5;
}
.docs-flow__item--ok { background: rgba(25,135,84,.08); border-left: 3px solid #198754; }
.docs-flow__item--warn { background: rgba(255,193,7,.12); border-left: 3px solid #ffc107; }

/* ── Timeline ──────────────────────────────────────────── */
.docs-timeline { display: flex; flex-direction: column; gap: 20px; }
.docs-timeline__sprint-header {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: 600;
  font-size: .9rem;
  margin-bottom: 8px;
}
.docs-timeline__sprint-badge {
  background: #2F7785;
  color: #fff;
  font-size: .7rem;
  padding: 2px 8px;
  border-radius: 999px;
  font-weight: 700;
}
.docs-timeline__list { padding-left: 18px; margin: 0; font-size: .85rem; line-height: 1.9; color: #495057; }

/* ── Pending ───────────────────────────────────────────── */
.docs-pending-card {
  background: #f8f9fa;
  border-radius: 10px;
  padding: 14px 16px;
}
.docs-pending-card__header { display: flex; align-items: center; gap: 8px; margin-bottom: 6px; font-size: .9rem; }
.docs-pending-card__desc { font-size: .83rem; color: #6c757d; margin: 0; line-height: 1.5; }

/* ── Footer ────────────────────────────────────────────── */
.docs-footer {
  margin-top: 32px;
  padding: 20px;
  text-align: center;
  color: #9ca3af;
  font-size: .8rem;
  border-top: 1px solid rgba(0,0,0,.06);
}

/* ── Print ─────────────────────────────────────────────── */
.print-only { display: none; }

@media print {
  .no-print { display: none !important; }
  .print-only { display: block; }

  .docs-card { break-inside: avoid; page-break-inside: avoid; margin-bottom: 16px !important; }
  .docs-card__header { cursor: default; }
  .docs-chevron { display: none; }
  .docs-card__body { display: block !important; }

  body { font-size: 10pt; color: #000; }

  .docs-print-footer {
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    text-align: center;
    font-size: 9pt;
    color: #666;
    border-top: 1px solid #ccc;
    padding: 4px;
  }
}
</style>
