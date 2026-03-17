using System;

namespace MMN.Dominio.Model
{
    public class MaterialApoio
    {
            public int IdMaterial { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public string URLMaterial { get; set; }
            public bool Ativo { get; set; }
            public DateTime DataCadastro { get; set; }
            public DateTime? UltimaAtualizacao { get; set; }

    }
}
