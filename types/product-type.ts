export type IReview = {
  user?:string;
  name:string;
  email:string;
  rating:number;
  review:string;
  date:string;
}

export interface IProduct {
  id: string;
  sku: string;
  img: string;
  title: string;
  slug: string;
  unit: string;
  url: string;
  imageURLs: {
    color?: {
      name: string;
      clrCode: string;
    };
    img: string;
  }[];
  parent: string;
  children: string;
  price: number;
  discount: number;
  quantity: number;
  brand: {
    name: string;
  };
  merchant: {
    id: string;
    name: string;
    cashback: number;
  };
  category: {
    name: string;
  };
  status: string;
  reviews?: IReview[];
  productType: string;
  description: string;
  orderQuantity?: number;
  additionalInformation: {
    key: string;
    value: string;
  }[];
  featured?: boolean;
  sellCount: number;
  offerDate?:{
    startDate:string;
    endDate:string;
  }
  tags?: string[];
  videoId?:string;
  sizes?:string[];
  link?:string;
  type?:string;
}