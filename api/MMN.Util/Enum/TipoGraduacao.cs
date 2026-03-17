using System.ComponentModel;

namespace MMN.Util.Enum
{
    public enum TipoGraduacao
    {
        [Description("Não Graduado")]
        NaoGraduado = 1,
        [Description("Empreendedor")]
        Empreendedor = 2,
        [Description("Supervisor")]
        Supervisor = 3,
        [Description("Gerente 1")]
        Gerente1 = 4,
        [Description("Gerente 2")]
        Gerente2 = 5,
        [Description("Gerente 3")]
        Gerente3 = 6,
        [Description("Diretor 1")]
        Diretor1 = 7,
        [Description("Diretor 2")]
        Diretor2 = 8,
        [Description("Vice-Presidente")]
        VicePresidente = 9,
        [Description("Presidente")]
        Presidente = 10
    }
}
