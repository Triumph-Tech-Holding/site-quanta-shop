export function maskCpfCnpj(doc: string | null | undefined): string {
  if (!doc) return '';
  const clean = String(doc).replace(/\D/g, '');
  if (clean.length === 11) return `***.***.${clean.substring(6, 9)}-**`;
  if (clean.length === 14) return `**.***.${clean.substring(5, 8)}/****-**`;
  if (clean.length <= 4) return '*'.repeat(clean.length);
  return '*'.repeat(clean.length - 2) + clean.substring(clean.length - 2);
}

export function maskEmail(email: string | null | undefined): string {
  if (!email) return '';
  const at = email.indexOf('@');
  if (at <= 0) return email;
  const user = email.substring(0, at);
  const domain = email.substring(at);
  if (user.length <= 2) return user[0] + '***' + domain;
  return user.substring(0, 2) + '*'.repeat(Math.max(3, user.length - 2)) + domain;
}

export function maskTelefone(tel: string | null | undefined): string {
  if (!tel) return '';
  const clean = String(tel).replace(/\D/g, '');
  if (clean.length < 4) return '*'.repeat(clean.length);
  return '*'.repeat(clean.length - 4) + clean.substring(clean.length - 4);
}

export function maskConta(conta: string | null | undefined): string {
  if (!conta) return '';
  const c = String(conta).trim();
  if (c.length <= 2) return '*'.repeat(c.length);
  return '*'.repeat(c.length - 2) + c.substring(c.length - 2);
}

export function maskAgencia(ag: string | null | undefined): string {
  if (!ag) return '';
  const a = String(ag).trim();
  if (a.length <= 1) return '*';
  return a[0] + '*'.repeat(a.length - 1);
}

export type SensitiveField = 'cpf' | 'cnpj' | 'email' | 'telefone' | 'conta' | 'agencia';

export function applyMask(field: SensitiveField, value: string | null | undefined): string {
  switch (field) {
    case 'cpf':
    case 'cnpj': return maskCpfCnpj(value);
    case 'email': return maskEmail(value);
    case 'telefone': return maskTelefone(value);
    case 'conta': return maskConta(value);
    case 'agencia': return maskAgencia(value);
    default: return value || '';
  }
}
