import axios from 'axios';

/**
 * Extrai uma mensagem de erro pt-BR amigável a partir de qualquer falha (axios, fetch, Error genérico).
 * Padrão único do projeto — usar em todas as páginas admin no lugar de blocos try/catch ad-hoc.
 *
 * @example
 * try { await api.post(...) } catch (e) { erro.value = extractApiError(e) }
 */
export function extractApiError(err: unknown, fallback = 'Falha inesperada. Tente novamente.'): string {
  if (axios.isAxiosError(err)) {
    const data: any = err.response?.data;
    if (typeof data === 'string') return data;
    if (data?.erros?.[0]?.mensagem) return String(data.erros[0].mensagem);
    if (data?.message) return String(data.message);
    if (data?.detail) return String(data.detail);
    if (err.response?.status === 401) return 'Sessão expirada. Faça login novamente.';
    if (err.response?.status === 403) return 'Você não tem permissão para essa ação.';
    if (err.response?.status === 429) return 'Muitas tentativas. Aguarde alguns segundos e tente novamente.';
    if (err.response?.status && err.response.status >= 500) return 'Erro interno do servidor. Tente novamente em instantes.';
    if (err.code === 'ECONNABORTED') return 'Tempo de resposta excedido. Verifique sua conexão.';
    if (err.message) return err.message;
  }
  if (err instanceof Error && err.message) return err.message;
  return fallback;
}

/**
 * Composable wrapper conveniente para uso em páginas Vue:
 *   const { extractApiError } = useApiError();
 */
export const useApiError = () => ({ extractApiError });
