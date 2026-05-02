<template>
  <div class="print-root">
    <div v-if="carregando" class="print-loading">
      <p>Preparando documento para impressão...</p>
    </div>
    <div v-else-if="erro" class="print-erro">
      <p>{{ erro }}</p>
    </div>
    <div v-else class="print-body" v-html="html"></div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: false });

const route = useRoute();
const arquivo = computed(() => String(route.query.arquivo || ''));
const nome = computed(() => String(route.query.nome || arquivo.value));

const carregando = ref(true);
const erro = ref('');
const html = ref('');

function esc(s: string): string {
  return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

function inlineMd(s: string): string {
  return esc(s)
    .replace(/\*\*\*(.+?)\*\*\*/g, '<strong><em>$1</em></strong>')
    .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
    .replace(/\*(.+?)\*/g, '<em>$1</em>')
    .replace(/`([^`]+)`/g, '<code>$1</code>')
    .replace(/\[([^\]]+)\]\(([^)]+)\)/g, '<a href="$2">$1</a>');
}

function renderMd(raw: string): string {
  const lines = raw.split('\n');
  const out: string[] = [];
  let inCode = false;
  let codeLang = '';
  let codeLines: string[] = [];
  let inTable = false;
  let tableRows: string[] = [];
  let listLines: string[] = [];

  function flushTable() {
    if (!tableRows.length) return;
    const rows = tableRows; tableRows = []; inTable = false;
    const thCells = rows[0].split('|').filter((_, i, a) => i > 0 && i < a.length - 1);
    const thead = '<tr>' + thCells.map(c => `<th>${inlineMd(c.trim())}</th>`).join('') + '</tr>';
    const tbody = rows.slice(2).map(r => {
      const cells = r.split('|').filter((_, i, a) => i > 0 && i < a.length - 1);
      return '<tr>' + cells.map(c => `<td>${inlineMd(c.trim())}</td>`).join('') + '</tr>';
    }).join('');
    out.push(`<table><thead>${thead}</thead><tbody>${tbody}</tbody></table>`);
  }

  function flushList() {
    if (!listLines.length) return;
    out.push('<ul>' + listLines.map(l => `<li>${inlineMd(l)}</li>`).join('') + '</ul>');
    listLines = [];
  }

  for (const line of lines) {
    if (line.startsWith('```')) {
      if (!inCode) { flushTable(); flushList(); inCode = true; codeLang = line.slice(3).trim(); codeLines = []; }
      else { out.push(`<pre><code>${codeLines.map(esc).join('\n')}</code></pre>`); inCode = false; codeLines = []; codeLang = ''; }
      continue;
    }
    if (inCode) { codeLines.push(line); continue; }

    if (line.includes('|')) { flushList(); if (!inTable) inTable = true; tableRows.push(line); continue; }
    else if (inTable) flushTable();

    if (/^(-{3,}|\*{3,}|_{3,})$/.test(line.trim())) { flushList(); out.push('<hr>'); continue; }

    const h4 = line.match(/^####\s+(.*)/); if (h4) { flushList(); out.push(`<h4>${inlineMd(h4[1])}</h4>`); continue; }
    const h3 = line.match(/^###\s+(.*)/);  if (h3) { flushList(); out.push(`<h3>${inlineMd(h3[1])}</h3>`); continue; }
    const h2 = line.match(/^##\s+(.*)/);   if (h2) { flushList(); out.push(`<h2>${inlineMd(h2[1])}</h2>`); continue; }
    const h1 = line.match(/^#\s+(.*)/);    if (h1) { flushList(); out.push(`<h1>${inlineMd(h1[1])}</h1>`); continue; }

    if (line.startsWith('> ')) { flushList(); out.push(`<blockquote>${inlineMd(line.slice(2))}</blockquote>`); continue; }

    const ol = line.match(/^\d+\.\s+(.*)/);
    if (ol) { flushList(); out.push(`<ol><li>${inlineMd(ol[1])}</li></ol>`); continue; }

    const ul = line.match(/^[-*]\s+(.*)/);
    if (ul) { listLines.push(ul[1]); continue; }
    else if (listLines.length) flushList();

    if (line.trim() === '') { out.push('<div class="spacer"></div>'); continue; }
    out.push(`<p>${inlineMd(line)}</p>`);
  }

  flushTable(); flushList();
  if (inCode && codeLines.length) out.push(`<pre><code>${codeLines.map(esc).join('\n')}</code></pre>`);
  return out.join('');
}

onMounted(async () => {
  if (!arquivo.value) {
    erro.value = 'Nenhum arquivo especificado.';
    carregando.value = false;
    return;
  }
  try {
    const texto = await $fetch<string>(`/docs/${arquivo.value}`, { responseType: 'text' });
    html.value = renderMd(texto);
    carregando.value = false;
    await nextTick();
    window.print();
  } catch {
    erro.value = `Não foi possível carregar "${arquivo.value}".`;
    carregando.value = false;
  }
});
</script>

<style>
*, *::before, *::after { box-sizing: border-box; }

body {
  font-family: -apple-system, BlinkMacSystemFont, 'Inter', Georgia, serif;
  font-size: 11pt;
  line-height: 1.7;
  color: #1d1d1f;
  margin: 0;
  padding: 0;
  background: #fff;
}

.print-loading, .print-erro {
  display: flex; align-items: center; justify-content: center;
  min-height: 100vh; font-size: 14px; color: #6c757d;
}

.print-body {
  max-width: 720px;
  margin: 0 auto;
  padding: 24px 32px;
}

.print-body h1 {
  font-size: 22pt; font-weight: 800; color: #1d1d1f;
  border-bottom: 2px solid #2F7785; padding-bottom: 6px; margin: 0 0 16px;
  page-break-after: avoid;
}
.print-body h2 {
  font-size: 15pt; font-weight: 700; color: #2F7785;
  border-bottom: 1px solid rgba(47,119,133,.3); padding-bottom: 4px; margin: 20px 0 8px;
  page-break-after: avoid;
}
.print-body h3 {
  font-size: 12pt; font-weight: 700; color: #225F6B; margin: 16px 0 4px;
  page-break-after: avoid;
}
.print-body h4 {
  font-size: 9pt; font-weight: 700; color: #374151;
  text-transform: uppercase; letter-spacing: .06em; margin: 12px 0 2px;
  page-break-after: avoid;
}
.print-body p { margin: 0 0 6px; }
.print-body .spacer { height: 6px; }
.print-body hr { border: none; border-top: 1px solid #ddd; margin: 14px 0; }

.print-body code {
  background: rgba(47,119,133,.1); color: #225F6B;
  padding: 1px 5px; border-radius: 4px;
  font-family: 'SF Mono', 'Fira Code', 'Courier New', monospace; font-size: .85em;
}
.print-body pre {
  background: #1d1d1f; color: #98C73A;
  border-radius: 6px; padding: 12px 16px; margin: 10px 0;
  font-family: 'SF Mono', 'Fira Code', 'Courier New', monospace;
  font-size: 9pt; line-height: 1.5; white-space: pre-wrap;
  page-break-inside: avoid;
}
.print-body pre code { background: none; color: inherit; padding: 0; }

.print-body blockquote {
  border-left: 3px solid #2F7785; background: rgba(47,119,133,.05);
  padding: 8px 14px; margin: 8px 0; border-radius: 0 6px 6px 0; color: #495057;
}

.print-body ul, .print-body ol { padding-left: 18px; margin: 4px 0 8px; }
.print-body li { margin-bottom: 2px; }
.print-body a { color: #2F7785; }

.print-body table {
  width: 100%; border-collapse: collapse; font-size: 9.5pt; margin: 10px 0;
  page-break-inside: avoid;
}
.print-body th {
  background: rgba(47,119,133,.1); color: #225F6B; font-weight: 700;
  padding: 6px 10px; text-align: left; border-bottom: 2px solid rgba(47,119,133,.2);
}
.print-body td { padding: 5px 10px; border-bottom: 1px solid #eee; }
.print-body tr:nth-child(even) td { background: rgba(0,0,0,.02); }

@media print {
  body { margin: 0; }
  .print-body { max-width: 100%; padding: 0; }
  .print-loading, .print-erro { display: none; }
  h1, h2, h3, h4 { page-break-after: avoid; }
  pre, table, blockquote { page-break-inside: avoid; }
  @page { margin: 18mm 16mm; size: A4; }
}
</style>
