using FluentValidation;
using MMN.Dominio.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioExportViewModel
    {
        public int Nivel { get; set; }
        public string Login { get; set; }
        public string LoginFilho { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Graduacao { get; set; }
        public string DataCadastro { get; set; }
    }
}
