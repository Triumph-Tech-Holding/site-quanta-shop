using System.ComponentModel;

namespace MMN.Util.Enum
{
    public enum StatusCredenciamento
    {
        [Description("Pendente")]
        Pendente,
        [Description("Pré cadastro")]
        PreCadastro,
        [Description("Aprovado")]
        Aprovado,
        [Description("Reprovado")]
        Reprovado
    }
}
