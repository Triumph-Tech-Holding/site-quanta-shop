<template>
  <div class="docs-viewer">

    <QsPageHeader eyebrow="LAB · Arquitetura" title="Documentação Técnica" description="Consulte arquitetura, padrões e decisões do projeto Quanta Shop.">
      <span class="docs-env-pill" :class="isDev ? 'docs-env-pill--dev' : 'docs-env-pill--prod'">
        <span class="docs-env-dot"></span>
        {{ isDev ? 'Dev (Replit)' : 'Produção' }}
      </span>
      <button
        class="qs-btn-outline"
        :disabled="!activeDoc || gerandoPdf"
        @click="gerarPdf(activeDoc)"
      >
        <span v-if="gerandoPdf" class="qs-spinner-sm"></span>
        <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/></svg>
        {{ gerandoPdf ? 'Gerando...' : 'Baixar PDF' }}
      </button>
    </QsPageHeader>

    <!-- 2-column layout -->
    <div class="docs-layout">

      <!-- ─── LEFT: document list ─────────────────────────── -->
      <div class="docs-sidebar">
        <div class="docs-search-wrap">
          <svg class="docs-search-icon" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
          <input
            v-model="busca"
            class="docs-search"
            type="text"
            placeholder="Buscar documentos..."
          />
        </div>

        <div class="docs-list">
          <template v-if="docsFiltrados.length === 0">
            <div class="docs-empty">Nenhum documento encontrado.</div>
          </template>

          <div
            v-for="doc in docsFiltrados"
            :key="doc.id"
            class="docs-card"
            :class="{ 'docs-card--active': activeDoc?.id === doc.id }"
            @click="selecionarDoc(doc)"
          >
            <div class="docs-card__top">
              <div class="docs-card__icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8" y2="17"/><polyline points="10 9 9 9 8 9"/></svg>
              </div>
              <div class="docs-card__info">
                <div class="docs-card__name">{{ doc.nome }}</div>
                <div class="docs-card__sub">{{ doc.descricao }}</div>
              </div>
              <span class="docs-card__badge" :class="`docs-card__badge--${doc.tipo}`">{{ doc.tipo.toUpperCase() }}</span>
            </div>

            <div class="docs-card__meta">
              <span>{{ doc.tamanho }}</span>
              <span class="docs-card__sep">·</span>
              <span>{{ doc.data }}</span>
            </div>

            <div class="docs-card__actions" @click.stop>
              <button class="docs-btn-action" :class="{ 'docs-btn-action--active': activeDoc?.id === doc.id }" @click="selecionarDoc(doc)">
                <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                Visualizar
              </button>
              <button
                class="docs-btn-action docs-btn-pdf"
                :disabled="gerandoPdf"
                @click.stop="gerarPdf(doc)"
              >
                <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/></svg>
                Baixar PDF
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- ─── RIGHT: preview ──────────────────────────────── -->
      <div class="docs-preview">
        <!-- Empty state -->
        <div v-if="!activeDoc && !carregando" class="docs-preview__empty">
          <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#ccc" stroke-width="1.2"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8" y2="17"/></svg>
          <p>Selecione um documento para visualizar</p>
        </div>

        <!-- Loading -->
        <div v-else-if="carregando" class="docs-preview__loading">
          <div class="qs-spinner-sm"></div>
          <span>Carregando documento...</span>
        </div>

        <!-- Preview content -->
        <template v-else-if="activeDoc">
          <div class="docs-preview__header">
            <div>
              <div class="docs-preview__title">{{ activeDoc.nome }}</div>
              <div class="docs-preview__meta">{{ activeDoc.tamanho }} · Atualizado {{ activeDoc.data }}</div>
            </div>
            <button
              class="qs-btn-primary"
              :disabled="gerandoPdf"
              @click="gerarPdf(activeDoc)"
            >
              <span v-if="gerandoPdf" class="qs-spinner-sm"></span>
              <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/></svg>
              {{ gerandoPdf ? 'Gerando...' : 'Baixar PDF' }}
            </button>
          </div>
          <div class="docs-preview__body" v-html="markdownRenderizado"></div>
        </template>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: 'agencia-painel', middleware: ['agencia-auth', 'agencia-admin'] });
useSeoMeta({ title: 'Documentação Técnica | Admin Quanta Shop' });

const isDev = import.meta.dev;

interface DocItem {
  id: string;
  nome: string;
  descricao: string;
  arquivo: string;
  tipo: string;
  tamanho: string;
  data: string;
  conteudo?: string;
}

const hoje = new Date().toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', year: 'numeric' });

const docs = ref<DocItem[]>([
  {
    id: 'claude',
    nome: 'CLAUDE.md — Arquitetura',
    descricao: 'Documento Mestre: stack, padrões, regras invioláveis e estrutura do projeto',
    arquivo: 'CLAUDE.md',
    tipo: 'ao vivo',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'architecture',
    nome: 'ARCHITECTURE.md — Desenho do Sistema',
    descricao: 'Visão técnica completa: módulos, fluxos, componentes, decisões arquiteturais e estrutura de diretórios',
    arquivo: 'ARCHITECTURE.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'deploy',
    nome: 'DEPLOY.md — Guia de Deploy',
    descricao: 'Passo a passo para publicar no Replit: env vars, build, checklist, rollback e monitoramento',
    arquivo: 'DEPLOY.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'features',
    nome: 'FEATURES.md — Catálogo de Features',
    descricao: 'Mapa de 24 funcionalidades por MVP, público e status — source of truth do roadmap',
    arquivo: 'FEATURES.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'historias',
    nome: 'Histórias — Personas e User Stories',
    descricao: 'Personas oficiais, histórias (US-01–US-308) e regras de negócio críticas por MVP',
    arquivo: 'historia.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'testing',
    nome: 'TESTING.md — Diretrizes de QA',
    descricao: 'Motor financeiro (xUnit), E2E matrix, responsividade mobile-first, LGPD e critérios de Done',
    arquivo: 'TESTING.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'data-dict',
    nome: 'DATA_DICTIONARY.md — Dicionário de Dados',
    descricao: 'Entidades, campos, LGPD, relações e práticas de conformidade da API .NET 8',
    arquivo: 'DATA_DICTIONARY.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'design',
    nome: 'DESIGN_SYSTEM.md — Design System',
    descricao: 'Paleta de cores, tipografia, tokens CSS qs-* e componentes reutilizáveis',
    arquivo: 'DESIGN_SYSTEM.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'changelog',
    nome: 'CHANGELOG.md — Histórico de Versões',
    descricao: 'Keep a Changelog + SemVer — todas as versões, sprints e hotfixes desde 2025',
    arquivo: 'CHANGELOG.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
  {
    id: 'replit',
    nome: 'replit.md — Contexto Replit',
    descricao: 'Configuração e contexto do ambiente Replit para o projeto',
    arquivo: 'replit.md',
    tipo: 'md',
    tamanho: '…',
    data: hoje,
  },
]);

const busca = ref('');
const activeDoc = ref<DocItem | null>(null);
const carregando = ref(false);
const gerandoPdf = ref(false);

const docsFiltrados = computed(() => {
  if (!busca.value.trim()) return docs.value;
  const q = busca.value.toLowerCase();
  return docs.value.filter(d =>
    d.nome.toLowerCase().includes(q) ||
    d.descricao.toLowerCase().includes(q) ||
    d.arquivo.toLowerCase().includes(q)
  );
});

async function selecionarDoc(doc: DocItem) {
  if (activeDoc.value?.id === doc.id && doc.conteudo) return;
  activeDoc.value = doc;
  if (!doc.conteudo) {
    carregando.value = true;
    try {
      const resp = await fetch(`/docs/${doc.arquivo}`);
      if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
      const texto = await resp.text();
      doc.conteudo = texto;
      doc.tamanho = formatBytes(new TextEncoder().encode(texto).length);
      const idx = docs.value.findIndex(d => d.id === doc.id);
      if (idx !== -1) docs.value[idx] = { ...doc };
    } catch (e: unknown) {
      const msg = e instanceof Error ? e.message : String(e);
      doc.conteudo = `> ⚠️ Erro ao carregar o documento: **${msg}**\n> Verifique se o arquivo está em \`public/docs/\`.`;
    } finally {
      carregando.value = false;
    }
  }
}

async function gerarPdf(doc: DocItem | null) {
  if (!doc?.conteudo || gerandoPdf.value) return;
  gerandoPdf.value = true;
  try {
    await nextTick();
    const previewBody = document.querySelector('.docs-preview__body') as HTMLElement | null;
    if (!previewBody) return;
    const html2pdf = (await import('html2pdf.js')).default;
    await html2pdf()
      .set({
        margin: [12, 14, 12, 14],
        filename: `${doc.arquivo.replace(/\.md$/, '')}.pdf`,
        image: { type: 'jpeg', quality: 0.95 },
        html2canvas: { scale: 2, useCORS: true, logging: false },
        jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' },
        pagebreak: { mode: ['avoid-all', 'css', 'legacy'] },
      })
      .from(previewBody)
      .save();
  } catch (e) {
    console.error('[docs] Erro ao gerar PDF:', e);
  } finally {
    gerandoPdf.value = false;
  }
}

function formatBytes(bytes: number): string {
  if (bytes < 1024) return bytes + ' B';
  return (bytes / 1024).toFixed(1) + ' KB';
}

// ── Markdown renderer ────────────────────────────────────
const markdownRenderizado = computed(() => {
  if (!activeDoc.value?.conteudo) return '';
  return renderMd(activeDoc.value.conteudo);
});

function esc(s: string): string {
  return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

function inlineMd(s: string): string {
  return esc(s)
    .replace(/\*\*\*(.+?)\*\*\*/g, '<strong><em>$1</em></strong>')
    .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
    .replace(/\*(.+?)\*/g, '<em>$1</em>')
    .replace(/`([^`]+)`/g, '<code class="md-code">$1</code>')
    .replace(/\[([^\]]+)\]\(([^)]+)\)/g, '<a href="$2" target="_blank" rel="noopener" class="md-link">$1</a>');
}

function renderMd(raw: string): string {
  const lines = raw.split('\n');
  const out: string[] = [];
  let inCode = false;
  let codeLang = '';
  let codeLines: string[] = [];
  let inTable = false;
  let tableRows: string[] = [];
  let inList = false;
  let listLines: string[] = [];

  function flushTable() {
    if (tableRows.length === 0) return;
    const rows = tableRows;
    tableRows = [];
    inTable = false;
    const header = rows[0];
    const body = rows.slice(2);
    const thCells = header.split('|').filter((_, i, a) => i > 0 && i < a.length - 1);
    const thead = '<tr>' + thCells.map(c => `<th>${inlineMd(c.trim())}</th>`).join('') + '</tr>';
    const tbody = body.map(r => {
      const cells = r.split('|').filter((_, i, a) => i > 0 && i < a.length - 1);
      return '<tr>' + cells.map(c => `<td>${inlineMd(c.trim())}</td>`).join('') + '</tr>';
    }).join('');
    out.push(`<div class="md-table-wrap"><table class="md-table"><thead>${thead}</thead><tbody>${tbody}</tbody></table></div>`);
  }

  function flushList() {
    if (listLines.length === 0) return;
    out.push('<ul class="md-ul">' + listLines.map(l => `<li>${inlineMd(l)}</li>`).join('') + '</ul>');
    listLines = [];
    inList = false;
  }

  for (let i = 0; i < lines.length; i++) {
    const line = lines[i];

    if (line.startsWith('```')) {
      if (!inCode) {
        flushTable(); flushList();
        inCode = true;
        codeLang = line.slice(3).trim();
        codeLines = [];
      } else {
        out.push(`<pre class="md-pre"><code class="md-codeblock${codeLang ? ' lang-' + codeLang : ''}">${codeLines.map(esc).join('\n')}</code></pre>`);
        inCode = false; codeLines = []; codeLang = '';
      }
      continue;
    }
    if (inCode) { codeLines.push(line); continue; }

    if (line.includes('|')) {
      flushList();
      if (!inTable) inTable = true;
      tableRows.push(line);
      continue;
    } else if (inTable) {
      flushTable();
    }

    if (/^(-{3,}|\*{3,}|_{3,})$/.test(line.trim())) {
      flushList();
      out.push('<hr class="md-hr">');
      continue;
    }

    const h4 = line.match(/^####\s+(.*)/);
    const h3 = line.match(/^###\s+(.*)/);
    const h2 = line.match(/^##\s+(.*)/);
    const h1 = line.match(/^#\s+(.*)/);
    if (h4) { flushList(); out.push(`<h4 class="md-h4">${inlineMd(h4[1])}</h4>`); continue; }
    if (h3) { flushList(); out.push(`<h3 class="md-h3">${inlineMd(h3[1])}</h3>`); continue; }
    if (h2) { flushList(); out.push(`<h2 class="md-h2">${inlineMd(h2[1])}</h2>`); continue; }
    if (h1) { flushList(); out.push(`<h1 class="md-h1">${inlineMd(h1[1])}</h1>`); continue; }

    if (line.startsWith('> ')) {
      flushList();
      out.push(`<blockquote class="md-blockquote">${inlineMd(line.slice(2))}</blockquote>`);
      continue;
    }

    const ol = line.match(/^\d+\.\s+(.*)/);
    if (ol) {
      flushList();
      out.push(`<ol class="md-ol" start="${line.match(/^(\d+)/)?.[1] ?? 1}"><li>${inlineMd(ol[1])}</li></ol>`);
      continue;
    }

    const ul = line.match(/^[-*]\s+(.*)/);
    if (ul) {
      listLines.push(ul[1]);
      inList = true;
      continue;
    } else if (inList) {
      flushList();
    }

    if (line.trim() === '') {
      out.push('<div class="md-spacer"></div>');
      continue;
    }

    out.push(`<p class="md-p">${inlineMd(line)}</p>`);
  }

  flushTable();
  flushList();
  if (inCode && codeLines.length) {
    out.push(`<pre class="md-pre"><code>${codeLines.map(esc).join('\n')}</code></pre>`);
  }

  return out.join('');
}

onMounted(async () => {
  await selecionarDoc(docs.value[0]);
});
</script>

<style scoped>
.docs-viewer {
  font-family: var(--qs-font, 'Inter', sans-serif);
}
.qs-header-text h1 { font-size: 1.6rem; font-weight: 700; color: var(--qs-ink); margin: .25rem 0 .4rem; }
.qs-header-text p  { font-size: .9rem; color: var(--qs-gray-400); margin: 0; }
.qs-header-actions { display: flex; align-items: center; gap: .75rem; flex-wrap: wrap; }
.qs-spinner-sm {
  display: inline-block;
  width: 14px; height: 14px;
  border: 2px solid rgba(255,255,255,.4);
  border-top-color: currentColor;
  border-radius: 50%;
  animation: docs-spin 0.7s linear infinite;
}
@keyframes docs-spin { to { transform: rotate(360deg); } }
.docs-layout {
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: 20px;
  min-height: calc(100vh - 200px);
}
@media (max-width: 900px) {
  .docs-layout { grid-template-columns: 1fr; }
}

.docs-env-pill {
  display: inline-flex; align-items: center; gap: 6px;
  padding: 4px 12px; border-radius: 999px; font-size: 12px; font-weight: 600;
}
.docs-env-pill--dev { background: rgba(255,193,7,.15); color: #856404; border: 1px solid rgba(255,193,7,.35); }
.docs-env-pill--prod { background: rgba(25,135,84,.1); color: #0a6640; border: 1px solid rgba(25,135,84,.25); }
.docs-env-dot { width: 7px; height: 7px; border-radius: 50%; background: currentColor; animation: blink 1.8s infinite; }
@keyframes blink { 0%,100%{opacity:1}50%{opacity:.3} }

.docs-sidebar {
  display: flex; flex-direction: column; gap: 12px;
}

.docs-search-wrap { position: relative; }
.docs-search-icon {
  position: absolute; left: 12px; top: 50%; transform: translateY(-50%); color: #9ca3af; pointer-events: none;
}
.docs-search {
  width: 100%; padding: 9px 12px 9px 36px;
  border: 1px solid #e5e7eb; border-radius: 10px;
  font-size: 13.5px; outline: none; color: #1d1d1f;
  background: #fff; transition: border-color .2s;
}
.docs-search:focus { border-color: #2F7785; box-shadow: 0 0 0 3px rgba(47,119,133,.1); }

.docs-list { display: flex; flex-direction: column; gap: 10px; }
.docs-empty { text-align: center; color: #9ca3af; font-size: 13px; padding: 20px; }

.docs-card {
  background: #fff;
  border: 1.5px solid rgba(0,0,0,.07);
  border-radius: 12px;
  padding: 14px 14px 10px;
  cursor: pointer;
  transition: border-color .15s, box-shadow .15s;
}
.docs-card:hover { border-color: #2F7785; box-shadow: 0 2px 10px rgba(47,119,133,.1); }
.docs-card--active { border-color: #2F7785; background: rgba(47,119,133,.04); box-shadow: 0 0 0 3px rgba(47,119,133,.12); }

.docs-card__top { display: flex; align-items: flex-start; gap: 10px; margin-bottom: 8px; }
.docs-card__icon { color: #2F7785; flex-shrink: 0; margin-top: 2px; }
.docs-card__info { flex: 1; min-width: 0; }
.docs-card__name { font-weight: 600; font-size: .88rem; color: #1d1d1f; line-height: 1.3; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
.docs-card__sub { font-size: .75rem; color: #6c757d; margin-top: 2px; line-height: 1.4; }

.docs-card__badge { flex-shrink: 0; font-size: 10px; font-weight: 700; letter-spacing: .04em; padding: 2px 7px; border-radius: 999px; }
.docs-card__badge--ao\ vivo { background: rgba(47,119,133,.12); color: #2F7785; border: 1px solid rgba(47,119,133,.25); }
.docs-card__badge--md { background: rgba(152,199,58,.12); color: #5a7a14; border: 1px solid rgba(152,199,58,.3); }

.docs-card__meta { font-size: 11px; color: #9ca3af; margin-bottom: 10px; display: flex; align-items: center; gap: 4px; }
.docs-card__sep { color: #d1d5db; }

.docs-card__actions { display: flex; gap: 8px; flex-wrap: wrap; }
.docs-btn-action {
  display: inline-flex; align-items: center; gap: 5px;
  padding: 5px 11px; border-radius: 7px; font-size: 12px; font-weight: 500;
  border: 1px solid #e5e7eb; background: #f9fafb; color: #374151;
  cursor: pointer; text-decoration: none; transition: all .15s;
}
.docs-btn-action:hover { border-color: #2F7785; color: #2F7785; background: rgba(47,119,133,.05); }
.docs-btn-action--active { border-color: #2F7785; color: #2F7785; background: rgba(47,119,133,.08); }
.docs-btn-pdf {
  border-color: rgba(47,119,133,.35); color: #2F7785; background: rgba(47,119,133,.06);
}
.docs-btn-pdf:hover { border-color: #2F7785; background: rgba(47,119,133,.12); }

.docs-preview {
  background: #fff;
  border: 1px solid rgba(0,0,0,.07);
  border-radius: 14px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  min-height: 500px;
}

.docs-preview__empty {
  flex: 1; display: flex; flex-direction: column; align-items: center; justify-content: center;
  color: #9ca3af; gap: 12px; font-size: 14px;
}

.docs-preview__loading {
  flex: 1; display: flex; align-items: center; justify-content: center; gap: 10px;
  color: #6c757d; font-size: 14px;
}

.docs-preview__header {
  display: flex; align-items: center; justify-content: space-between; gap: 12px;
  padding: 16px 24px; border-bottom: 1px solid rgba(0,0,0,.07);
  background: #fafafa; flex-wrap: wrap;
}
.docs-preview__title { font-weight: 700; font-size: .95rem; color: #1d1d1f; }
.docs-preview__meta { font-size: 12px; color: #9ca3af; margin-top: 2px; }

.docs-preview__body {
  flex: 1; padding: 28px 32px; overflow-y: auto;
  font-size: 14px; line-height: 1.75; color: #1d1d1f;
}

.docs-preview__body :deep(.md-h1) {
  font-size: 1.8rem; font-weight: 800; color: #1d1d1f; margin: 28px 0 6px;
  border-bottom: 2px solid rgba(47,119,133,.2); padding-bottom: 8px;
}
.docs-preview__body :deep(.md-h2) {
  font-size: 1.3rem; font-weight: 700; color: #2F7785; margin: 24px 0 6px;
  border-bottom: 1px solid rgba(47,119,133,.15); padding-bottom: 5px;
}
.docs-preview__body :deep(.md-h3) {
  font-size: 1.05rem; font-weight: 700; color: #225F6B; margin: 18px 0 4px;
}
.docs-preview__body :deep(.md-h4) {
  font-weight: 700; color: #374151; margin: 14px 0 2px;
  text-transform: uppercase; letter-spacing: .05em; font-size: .75rem;
}
.docs-preview__body :deep(.md-p) { margin: 0 0 4px; }
.docs-preview__body :deep(.md-spacer) { height: 8px; }
.docs-preview__body :deep(.md-hr) { border: none; border-top: 1px solid #e5e7eb; margin: 20px 0; }
.docs-preview__body :deep(.md-code) {
  background: rgba(47,119,133,.09); color: #225F6B;
  padding: 1px 6px; border-radius: 5px; font-family: 'SF Mono', 'Fira Code', monospace; font-size: .82em;
}
.docs-preview__body :deep(.md-pre) {
  background: #1d1d1f; border-radius: 10px; padding: 16px 20px; margin: 12px 0; overflow-x: auto;
}
.docs-preview__body :deep(.md-codeblock) {
  color: #98C73A; font-family: 'SF Mono', 'Fira Code', monospace; font-size: .82rem; line-height: 1.6;
  white-space: pre; display: block;
}
.docs-preview__body :deep(.md-blockquote) {
  border-left: 3px solid #2F7785; background: rgba(47,119,133,.05);
  padding: 10px 16px; margin: 8px 0; border-radius: 0 8px 8px 0; color: #495057;
}
.docs-preview__body :deep(.md-ul) { padding-left: 20px; margin: 4px 0 8px; }
.docs-preview__body :deep(.md-ul li) { margin-bottom: 3px; }
.docs-preview__body :deep(.md-ol) { padding-left: 20px; margin: 4px 0 8px; }
.docs-preview__body :deep(.md-link) { color: #2F7785; text-decoration: underline; }
.docs-preview__body :deep(.md-table-wrap) { overflow-x: auto; margin: 12px 0; }
.docs-preview__body :deep(.md-table) { width: 100%; border-collapse: collapse; font-size: .85rem; }
.docs-preview__body :deep(.md-table th) {
  background: rgba(47,119,133,.08); color: #225F6B; font-weight: 700;
  padding: 8px 12px; text-align: left; border-bottom: 2px solid rgba(47,119,133,.2);
  white-space: nowrap;
}
.docs-preview__body :deep(.md-table td) {
  padding: 7px 12px; border-bottom: 1px solid #f3f4f6; color: #374151;
}
.docs-preview__body :deep(.md-table tr:hover td) { background: rgba(47,119,133,.03); }
</style>
