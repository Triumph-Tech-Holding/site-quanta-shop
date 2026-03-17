namespace MMN.Dominio.ViewModel
{
    public class GraduacaoRequisitosViewModel
    {
        public int IdGraduacaoRequisitos { get; set; }
        public int IdGraduacao { get; set; }
        public int IdGraduacaoObrigatorio { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }
        //public GraduacaoViewModel Graduacao { get; set; }
        public GraduacaoViewModel GraduacaoObrigatorio { get; set; }
    }
}
