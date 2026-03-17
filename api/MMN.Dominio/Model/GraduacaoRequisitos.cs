namespace MMN.Dominio.Model
{
    public class GraduacaoRequisitos
    {
        public int IdGraduacaoRequisitos { get; set; }
        public int IdGraduacao { get; set; }
        public int IdGraduacaoObrigatorio { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }
        
        public Graduacao Graduacao { get; set; }
        public Graduacao GraduacaoObrigatorio { get; set; }
    }
}
