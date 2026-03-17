using System.Linq;

namespace MMN.Util.Util
{
    public static class UtilBase
    {
        public static bool HasDigit(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            foreach (char c in text)
            {
                if (char.IsDigit(c))
                    return true;
            }
            return false;
        }

        public static bool HasLetter(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                    return true;
            }
            return false;
        }

        public static bool IsCpf(string cpf)
        {
            string valor = cpf.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];
            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                valor[i].ToString());

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;

            }
            else
                if (numeros[10] != 11 - resultado)
                return false;
            return true;
        }

        public static bool IsCnpj(string cnpj)
        {
            string CNPJ = cnpj.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");

            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                     CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] *
                        int.Parse(ftmt.Substring(
                          nrDig + 1, 1)));
                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] *
                        int.Parse(ftmt.Substring(
                          nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == 0);

                    else
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == (
                        11 - resultado[nrDig]));

                }

                return (CNPJOk[0] && CNPJOk[1]);

            }
            catch
            {
                return false;
            }
        }

        public static bool RequisitosSenha(string senha)
        {
            return !string.IsNullOrEmpty(senha) &&
                senha.Length >= 8 &&
                UtilBase.HasDigit(senha) &&
                UtilBase.HasLetter(senha);
        }
        
        public static bool RequisitosLogin(string login)
        {
            return !login.ToLower().Contains("bigcash")
                && !login.ToLower().Contains("bigcashme")
                && !login.ToLower().Contains("cashme")
                && !login.ToLower().Contains("big")
                && !login.ToLower().Contains("cash")
                && !login.ToLower().Contains("admin")
                && !login.ToLower().Contains("admins");
        }

        public static bool IsValidCpfCnpj(string documento)
        {
            if (string.IsNullOrEmpty(documento))
                return false;

            return IsCpf(documento) || IsCnpj(documento);
        }
        
        public static bool EmailDominioValido(string email)
        {
            if (email.Contains(".gov") ||
                email.Contains(".gov.br"))
            {
                return false;
            }

            return true;
        }

        public static bool EmailCorreto(string email)
        {
            if(email.Contains("@") && (email.EndsWith(".com") || email.EndsWith(".com.br")))
            {
                return true;
            }

            return false;
        }

        public static bool LoginValido(string login)
        {
            if (string.IsNullOrEmpty(login)) return false;

            if (login.Contains("bigcash") ||
                login.Contains("bigcashme") ||
                login.Contains("cashme") ||
                login.Contains("big") ||
                login.Contains("cash") ||
                login.Contains("admin") ||
                login.Contains("admins") )
            {
                return false;
            }

            return true;
        }

        public static string FiltrarDigitos(string s)
        {
            return new string(s.Where(w => char.IsNumber(w)).ToArray());
        }
    }
}
