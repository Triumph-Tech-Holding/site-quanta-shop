using System;

namespace MMN.Dominio.Model
{
    public class HistoricoGraduacao
    {
        public long IdHistorico { get; set; }
        public int IdGraduacao { get; set; }
        public Guid IdUsuario { get; set; }
        public DateTime DataGraduacao { get; set; }

        public virtual Graduacao Graduacao { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
