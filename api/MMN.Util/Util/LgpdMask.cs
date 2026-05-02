using System.Linq;

namespace MMN.Util.Util
{
    /// <summary>
    /// Mascaramento de dados pessoais sensiveis para LGPD.
    /// Mantem ultimos digitos visiveis para identificacao, mascara o restante.
    /// Reveal real e feito apenas pelo endpoint Admin/RevelarDadoSensivel (gated por Master).
    /// </summary>
    public static class LgpdMask
    {
        public static string MaskCpfCnpj(string doc)
        {
            if (string.IsNullOrWhiteSpace(doc)) return doc;
            var clean = new string(doc.Where(char.IsDigit).ToArray());
            if (clean.Length == 11)
            {
                return $"***.***.{clean.Substring(6, 3)}-**";
            }
            if (clean.Length == 14)
            {
                return $"**.***.{clean.Substring(5, 3)}/****-**";
            }
            if (clean.Length <= 4) return new string('*', clean.Length);
            return new string('*', clean.Length - 2) + clean.Substring(clean.Length - 2);
        }

        public static string MaskEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return email;
            var at = email.IndexOf('@');
            if (at <= 0) return email;
            var user = email.Substring(0, at);
            var domain = email.Substring(at);
            if (user.Length <= 2) return user[0] + "***" + domain;
            return user.Substring(0, 2) + new string('*', System.Math.Max(3, user.Length - 2)) + domain;
        }

        public static string MaskTelefone(string tel)
        {
            if (string.IsNullOrWhiteSpace(tel)) return tel;
            var clean = new string(tel.Where(char.IsDigit).ToArray());
            if (clean.Length < 4) return new string('*', clean.Length);
            return new string('*', clean.Length - 4) + clean.Substring(clean.Length - 4);
        }

        public static string MaskConta(string conta)
        {
            if (string.IsNullOrWhiteSpace(conta)) return conta;
            var c = conta.Trim();
            if (c.Length <= 2) return new string('*', c.Length);
            return new string('*', c.Length - 2) + c.Substring(c.Length - 2);
        }

        public static string MaskAgencia(string ag)
        {
            if (string.IsNullOrWhiteSpace(ag)) return ag;
            var a = ag.Trim();
            if (a.Length <= 1) return "*";
            return a[0] + new string('*', a.Length - 1);
        }
    }
}
