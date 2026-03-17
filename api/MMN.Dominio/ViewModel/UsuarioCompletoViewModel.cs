using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioCompletoViewModel
    {
        public Guid IdUsuario { get; set; }
        public Guid? IdUsuarioPai { get; set; }
        public int IdGrupo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public byte[] SaltKey { get; set; }
        public string Documento { get; set; }
        public bool Ativo { get; set; }
        public string Celular { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public short? TentativasIncorretas { get; set; }
        public string AssinaturaEletronica { get; set; }
        public int? IdGraduacao { get; set; }
        public short? PosicaoBinario { get; set; }
        public string Cultura { get; set; }
        public DateTime? DataReferencia { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string LoginPatrocinador { get; set; }
        
        public int IdEndereco { get; set; }
        public int IdCidade { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
    }
}
