import { type IMenuItem, type IMobileType } from "@/types/menu-d-type";

export const menu_data: IMenuItem[] = [
  {
    id: 1,
    link: '/shop',
    title: 'Produtos',
    target: '_self'
  },
  {
    id: 2,
    link: '/partners',
    title: 'Parceiros',
    target: '_self'
  },
  {
    id: 3,
    link: '/agencia/cadastro',
    title: 'Crendenciamento',
    target: '_self'
  },
  {
    id: 4,
    link: 'https://api.whatsapp.com/send?phone=5521996983881&text=Preciso%20de%20atendimento&app_absent=0',
    title: 'Fale conosco',
    target: '_blank'
  },
  {
    id: 5,
    link: 'https://bigcash.blob.core.windows.net/documentos/CAMPANHA%20QUANTA%20AMIZADE.pdf',
    title: 'Quanta Amizade',
    target: '_blank'
  },
]

// mobile menu data 
export const mobile_menu: IMobileType[] = [
  {
    id: 1,
    link: '/shop',
    title: 'Produtos',
    target: '_self'
  },
  {
    id: 2,
    link: '/partners',
    title: 'Parceiros',
    target: '_self'
  },
  {
    id: 3,
    link: '/agencia/cadastro',
    title: 'Crendenciamento',
    target: '_self'
  },
  {
    id: 4,
    link: 'https://api.whatsapp.com/send?phone=5521996983881&text=Preciso%20de%20atendimento&app_absent=0',
    title: 'Fale conosco',
    target: '_blank'
  },
  {
    id: 5,
    link: 'https://bigcash.blob.core.windows.net/documentos/CAMPANHA%20QUANTA%20AMIZADE.pdf',
    title: 'Quanta Amizade',
    target: '_blank'
  },
]