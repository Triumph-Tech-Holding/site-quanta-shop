import { defineStore } from 'pinia';

export interface BlogArtigo {
  id: string | number;
  titulo: string;
  slug: string;
  resumo?: string;
  conteudo: string;
  imagemDestaque?: string;
  categoria?: string;
  autor?: string;
  dataPublicacao?: string;
  publicado: boolean;
  destaque: boolean;
  criadoEm: string;
}

export interface PostRedeSocial {
  id: string | number;
  plataforma: 'Instagram' | 'YouTube' | 'TikTok' | 'Facebook' | 'Twitter' | 'Outro';
  titulo: string;
  url: string;
  thumbnailUrl?: string;
  descricao?: string;
  dataPublicacao?: string;
  ativo: boolean;
}

export const useBlogStore = defineStore('blog', () => {
  const ARTIGOS_KEY = 'qs_blog_artigos';
  const REDES_KEY = 'qs_redes_sociais';

  const artigos = ref<BlogArtigo[]>([]);
  const redesSociais = ref<PostRedeSocial[]>([]);

  function generateId() {
    return Date.now().toString(36) + Math.random().toString(36).slice(2);
  }

  function generateSlug(titulo: string) {
    return titulo
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/^-+|-+$/g, '');
  }

  // Artigos
  function carregarArtigos() {
    if (typeof window === 'undefined') return;
    try {
      const data = localStorage.getItem(ARTIGOS_KEY);
      artigos.value = data ? JSON.parse(data) : [];
    } catch (e) {
      console.error('Erro ao carregar artigos do localStorage', e);
      artigos.value = [];
    }
  }

  function salvarArtigos() {
    if (typeof window === 'undefined') return;
    localStorage.setItem(ARTIGOS_KEY, JSON.stringify(artigos.value));
  }

  function criarArtigo(artigo: Omit<BlogArtigo, 'id' | 'criadoEm'>) {
    const novo: BlogArtigo = {
      ...artigo,
      id: generateId(),
      criadoEm: new Date().toISOString()
    };
    artigos.value.unshift(novo);
    salvarArtigos();
    return novo;
  }

  function atualizarArtigo(id: string | number, dados: Partial<BlogArtigo>) {
    const idx = artigos.value.findIndex(a => a.id === id);
    if (idx !== -1) {
      artigos.value[idx] = { ...artigos.value[idx], ...dados };
      salvarArtigos();
    }
  }

  function excluirArtigo(id: string | number) {
    artigos.value = artigos.value.filter(a => a.id !== id);
    salvarArtigos();
  }

  function buscarArtigoPorSlug(slug: string) {
    return artigos.value.find(a => a.slug === slug);
  }

  // Redes Sociais
  function carregarRedesSociais() {
    if (typeof window === 'undefined') return;
    try {
      const data = localStorage.getItem(REDES_KEY);
      redesSociais.value = data ? JSON.parse(data) : [];
    } catch (e) {
      console.error('Erro ao carregar redes sociais do localStorage', e);
      redesSociais.value = [];
    }
  }

  function salvarRedesSociais() {
    if (typeof window === 'undefined') return;
    localStorage.setItem(REDES_KEY, JSON.stringify(redesSociais.value));
  }

  function criarPostRedeSocial(post: Omit<PostRedeSocial, 'id'>) {
    const novo: PostRedeSocial = {
      ...post,
      id: generateId()
    };
    redesSociais.value.unshift(novo);
    salvarRedesSociais();
    return novo;
  }

  function atualizarPostRedeSocial(id: string | number, dados: Partial<PostRedeSocial>) {
    const idx = redesSociais.value.findIndex(p => p.id === id);
    if (idx !== -1) {
      redesSociais.value[idx] = { ...redesSociais.value[idx], ...dados };
      salvarRedesSociais();
    }
  }

  function excluirPostRedeSocial(id: string | number) {
    redesSociais.value = redesSociais.value.filter(p => p.id !== id);
    salvarRedesSociais();
  }

  return {
    artigos,
    redesSociais,
    generateSlug,
    carregarArtigos,
    criarArtigo,
    atualizarArtigo,
    excluirArtigo,
    buscarArtigoPorSlug,
    carregarRedesSociais,
    criarPostRedeSocial,
    atualizarPostRedeSocial,
    excluirPostRedeSocial
  };
});
