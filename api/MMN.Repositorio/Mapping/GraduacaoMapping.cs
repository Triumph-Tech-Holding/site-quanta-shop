using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class GraduacaoMapping : IEntityTypeConfiguration<Graduacao>
    {
        public void Configure(EntityTypeBuilder<Graduacao> builder)
        {
            builder.HasKey(e => e.IdGraduacao);


            builder.Property(p => p.IdGraduacao)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(p => p.Image)
                .HasMaxLength(255);

            builder.Property(e => e.Nivel)
                .IsRequired();

            builder.Property(e => e.Image).IsUnicode(false);
            builder.Property(e => e.PercentualPremiacao).IsRequired(false);
            builder.Property(e => e.QuantidadeDiretos).IsRequired(false);
        }
    }
}
