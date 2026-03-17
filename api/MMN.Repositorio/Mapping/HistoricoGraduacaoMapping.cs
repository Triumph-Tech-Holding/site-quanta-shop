using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class HistoricoGraduacaoMapping : IEntityTypeConfiguration<HistoricoGraduacao>
    {
        public void Configure(EntityTypeBuilder<HistoricoGraduacao> builder)
        {
            builder.HasKey(e => e.IdHistorico);

            builder.Property(e => e.DataGraduacao).HasColumnType("datetime");

            builder.HasOne(d => d.Graduacao)
                .WithMany(p => p.HistoricoGraduacao)
                .HasForeignKey(d => d.IdGraduacao)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.HistoricoGraduacao)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
