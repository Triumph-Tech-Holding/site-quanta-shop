using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MMN.Util.Extensions
{
    public static class StringExtensions
    {
        public static bool TemCaracterEspecial(this string str)
        {
            return Regex.IsMatch(str, "[^0-9a-zA-Z]+");
        }

        public static string Compress(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string Decompress(this string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static string NormalizeString(this string input)
        {
            if (input == null)
                return null;

            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    if (char.IsLetterOrDigit(c))
                    {
                        stringBuilder.Append(char.ToLower(c));
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public static string GenerateSubscriptionCode(string key)
        {
            // Prefixo obrigatório
            const string prefix = "sub_";

            // Gera o hash SHA-256 a partir da chave
            using (var sha256 = SHA256.Create())
            {
                // Converte a chave em bytes
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                // Gera o hash
                byte[] hashBytes = sha256.ComputeHash(keyBytes);

                // Converte o hash em uma string Base64
                string hashBase64 = Convert.ToBase64String(hashBytes);

                // Remove caracteres não alfanuméricos
                string hashBase64Url = hashBase64
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .TrimEnd('=');

                // Retorna a string formatada com o prefixo
                return prefix + hashBase64Url.Substring(0, 15);
            }
        }
        
        public static string GenerateInvoiceCode(string key)
        {
            // Prefixo obrigatório
            const string prefix = "in_";

            // Gera o hash SHA-256 a partir da chave
            using (var sha256 = SHA256.Create())
            {
                // Converte a chave em bytes
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                // Gera o hash
                byte[] hashBytes = sha256.ComputeHash(keyBytes);

                // Converte o hash em uma string Base64
                string hashBase64 = Convert.ToBase64String(hashBytes);

                // Remove caracteres não alfanuméricos
                string hashBase64Url = hashBase64
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .TrimEnd('=');

                // Retorna a string formatada com o prefixo
                return prefix + hashBase64Url.Substring(0, 15);
            }
        }

        public static string GenerateOrderCode()
        {
            var random = new Random();

            // Prefixo fixo
            const string prefix = "PED";

            // Gera a parte de data no formato DDMMYY (Dia, Mês, Ano com 2 dígitos)
            string datePart = DateTime.Now.ToString("ddMMyy");

            // Gera uma sequência numérica de 6 dígitos
            string numberPart = random.Next(100000, 999999).ToString();

            // Gera um sufixo alfanumérico (por exemplo, RN726)
            string suffix = "RN" + random.Next(100, 999).ToString();

            // Combina todas as partes para formar o código final
            string result = $"{prefix}{datePart}{numberPart}_{suffix}";

            return result;
        }
    }
}