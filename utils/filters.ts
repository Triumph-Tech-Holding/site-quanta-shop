/**
 * Formata um valor numérico para uma string com base nas opções de formato fornecidas.
 *
 * @param {number} value - O valor numérico a ser formatado.
 * @param {number} [decimalPlaces=2] - O número de casas decimais para o formato.
 * @returns {string} - O valor formatado como uma string.
 */
export const formatToBRL = (value: number) => {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(value);
};

/**
 * Formata um valor numérico para uma string com base nas opções de formato fornecidas.
 *
 * @param {number} value - O valor numérico a ser formatado.
 * @param {number} [decimalPlaces=2] - O número de casas decimais para o formato.
 * @returns {string} - O valor formatado como uma string.
 */
export const formatToPercentage = (value: number, decimalPlaces: number = 2): string => {
  if (value === undefined || value === null) return "0%";

  const percentageValue = value / 100;

  return new Intl.NumberFormat("pt-BR", {
    style: "percent",
    minimumFractionDigits: decimalPlaces,
    maximumFractionDigits: decimalPlaces,
  }).format(percentageValue);
};

/**
 * Formata uma data para uma string com base nas opções de formato fornecidas.
 *
 * @param {Date} date - A data a ser formatada.
 * @param {Intl.DateTimeFormatOptions} [options] - Opções de formato para a data.
 * @returns {string} - A data formatada como uma string.
 */
export const formatToDate = (date: Date, options?: Intl.DateTimeFormatOptions): string => {
  return new Intl.DateTimeFormat("pt-BR", {
    ...options,
  }).format(date);
};

/**
 * Preenche o texto à esquerda com espaços até alcançar a quantidade de caracteres desejada.
 *
 * @param {string} text - O texto a ser preenchido.
 * @param {number} length - O comprimento total desejado do texto após o preenchimento.
 * @param {string} [char='0'] - O caractere a ser usado para preenchimento.
 * @returns {string} - O texto preenchido à esquerda.
 */
export const padLeft = (text: string, length: number, char: string = '0'): string => {
  if (text.length >= length) {
    return text;
  }

  const padding = char.repeat(length - text.length);

  return padding + text;
};

/**
 * Formata um número como uma string percentual com duas casas decimais.
 *
 * @param {number} value - O valor a ser formatado.
 * @returns {string} - O valor formatado como percentual.
 */
export const formatPercentage = (value: number) => {
  return `${value.toFixed(2).replace('.', ',')}%`;
};