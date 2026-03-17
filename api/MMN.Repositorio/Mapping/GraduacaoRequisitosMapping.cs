using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class GraduacaoRequisitosMapping : IEntityTypeConfiguration<GraduacaoRequisitos>
    {
        public void Configure(EntityTypeBuilder<GraduacaoRequisitos> builder)
        {
            builder.HasKey(e => e.IdGraduacaoRequisitos);


            builder.Property(p => p.IdGraduacaoRequisitos)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Quantidade).IsRequired();

            builder.Property(e => e.Ativo).IsRequired();

            builder.HasOne(d => d.Graduacao)
                .WithMany(p => p.GraduacaoRequisitos)
                .HasForeignKey(d => d.IdGraduacao)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.GraduacaoObrigatorio)
                .WithMany(p => p.GraduacaoRequisitosSecundario)
                .HasForeignKey(d => d.IdGraduacaoObrigatorio)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
