export interface IBrand {
  id: string;
  products?: string[];
  totalProducts: number;
  name: string;
  description?: string;
  email: string;
  website?: string;
  location?: string;
  status?: string;
  logo?: string;
}