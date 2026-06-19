export interface Cupom {
  id: string | number;
  type: 'voucher' | 'promotion';
  store: { slug: string; name: string; logo: string };
  category: string;
  title: string;
  discount: string;
  cashback: string | null;
  code: string | null;
  description: string;
  terms?: string;
  exclusive?: boolean;
  expiresAt: string;
  url: string;
}

export const CUPOM_CATEGORIES = [
  'Todos', 'Moda', 'Casa & Eletro', 'Mercado',
  'Saúde & Beleza', 'Tech', 'Viagem', 'Bebidas',
] as const;

const L = (slug: string) => `/img/brands/${slug}.png`;

const MOCK: Cupom[] = [
  { id: 'qs-001', type: 'voucher', store: { slug: 'carrefour', name: 'Carrefour', logo: L('carrefour') }, category: 'Mercado', title: 'R$ 30 OFF em compras acima de R$ 200', discount: 'R$ 30 OFF', cashback: '4%', code: 'QUANTA30', description: 'Desconto no mercado online + cashback Quanta na mesma compra.', terms: 'Válido para a primeira compra do app. Não cumulativo com outros cupons.', exclusive: true, expiresAt: '2026-07-15', url: '#' },
  { id: 'qs-002', type: 'voucher', store: { slug: 'casas-bahia', name: 'Casas Bahia', logo: L('casas-bahia') }, category: 'Casa & Eletro', title: '15% OFF em móveis e eletro selecionados', discount: '15% OFF', cashback: '6%', code: 'CASA15', description: 'Renove a casa com desconto e ainda receba cashback de verdade.', terms: 'Sujeito a estoque. Confira produtos participantes.', expiresAt: '2026-06-30', url: '#' },
  { id: 'qs-003', type: 'voucher', store: { slug: 'lacoste', name: 'Lacoste', logo: L('lacoste') }, category: 'Moda', title: '20% OFF em toda a coleção', discount: '20% OFF', cashback: '8%', code: 'LACOSTE20', description: 'Estilo com desconto e o melhor cashback de moda da Quanta.', exclusive: true, expiresAt: '2026-07-05', url: '#' },
  { id: 'qs-004', type: 'promotion', store: { slug: 'lg', name: 'LG', logo: L('lg') }, category: 'Tech', title: 'Até R$ 800 OFF em TVs e monitores', discount: 'Até R$ 800 OFF', cashback: '5%', code: null, description: 'Promoção relâmpago em eletrônicos LG com cashback Quanta.', expiresAt: '2026-06-28', url: '#' },
  { id: 'qs-005', type: 'voucher', store: { slug: 'drogaria-venancio', name: 'Drogaria Venancio', logo: L('drogaria-venancio') }, category: 'Saúde & Beleza', title: '12% OFF em dermocosméticos', discount: '12% OFF', cashback: '7%', code: 'PELE12', description: 'Cuide da pele economizando duas vezes.', expiresAt: '2026-07-10', url: '#' },
  { id: 'qs-006', type: 'voucher', store: { slug: 'ze-delivery', name: 'Zé Delivery', logo: L('ze-delivery') }, category: 'Bebidas', title: 'R$ 15 OFF no primeiro pedido', discount: 'R$ 15 OFF', cashback: '10%', code: 'ZEQUANTA', description: 'Bebida gelada na porta com desconto e cashback.', exclusive: true, expiresAt: '2026-06-25', url: '#' },
  { id: 'qs-007', type: 'voucher', store: { slug: 'diesel', name: 'Diesel', logo: L('diesel') }, category: 'Moda', title: '25% OFF em jeans selecionados', discount: '25% OFF', cashback: '8%', code: 'DENIM25', description: 'A coleção de jeans com o maior desconto da temporada.', expiresAt: '2026-07-20', url: '#' },
  { id: 'qs-008', type: 'promotion', store: { slug: 'electrolux', name: 'Electrolux', logo: L('electrolux') }, category: 'Casa & Eletro', title: 'Frete grátis + cashback turbinado', discount: 'Frete grátis', cashback: '6%', code: null, description: 'Eletrodomésticos com frete grátis e cashback dobrado para Plus.', expiresAt: '2026-07-01', url: '#' },
  { id: 'qs-009', type: 'voucher', store: { slug: 'intimissimi', name: 'Intimissimi', logo: L('intimissimi') }, category: 'Moda', title: '18% OFF em lingerie e pijamas', discount: '18% OFF', cashback: '9%', code: 'INTIMI18', description: 'Conforto e estilo com desconto exclusivo.', expiresAt: '2026-07-12', url: '#' },
  { id: 'qs-010', type: 'voucher', store: { slug: 'panasonic', name: 'Panasonic', logo: L('panasonic') }, category: 'Tech', title: '10% OFF em áudio e câmeras', discount: '10% OFF', cashback: '5%', code: 'PANA10', description: 'Tecnologia Panasonic com desconto e cashback Quanta.', expiresAt: '2026-06-29', url: '#' },
  { id: 'qs-011', type: 'voucher', store: { slug: 'quero-passagem', name: 'Quero Passagem', logo: L('quero-passagem') }, category: 'Viagem', title: 'R$ 60 OFF em passagens nacionais', discount: 'R$ 60 OFF', cashback: '3%', code: 'VIAJA60', description: 'Viaje pagando menos e ainda receba cashback.', exclusive: true, expiresAt: '2026-07-31', url: '#' },
  { id: 'qs-012', type: 'voucher', store: { slug: 'cafe-l-or', name: "Café L'or", logo: L('cafe-l-or') }, category: 'Mercado', title: '22% OFF em cápsulas de café', discount: '22% OFF', cashback: '7%', code: 'LOR22', description: 'O café que você ama com desconto e cashback.', expiresAt: '2026-07-08', url: '#' },
];

const cupons = useState<Cupom[]>('qs-cupons', () => []);
const loaded = useState<boolean>('qs-cupons-loaded', () => false);

function ativos(list: Cupom[]): Cupom[] {
  const hoje = new Date();
  hoje.setHours(0, 0, 0, 0);
  return list.filter((c) => !!c.cashback && new Date(c.expiresAt) >= hoje);
}

export function useCupons() {
  async function loadCupons() {
    if (loaded.value) return;
    cupons.value = ativos(MOCK);
    loaded.value = true;
  }

  function diasRestantes(c: Cupom): number {
    const ms = new Date(c.expiresAt).getTime() - Date.now();
    return Math.max(0, Math.ceil(ms / 86400000));
  }

  return { cupons, loadCupons, diasRestantes, categories: CUPOM_CATEGORIES };
}
