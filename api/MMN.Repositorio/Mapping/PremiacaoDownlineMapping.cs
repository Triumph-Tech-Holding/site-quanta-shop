using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class PremiacaoDownlineMapping : IEntityTypeConfiguration<PremiacaoDownline>
    {
        public void Configure(EntityTypeBuilder<PremiacaoDownline> builder)
        {
            builder.HasKey(p => p.IdPremiacaoDownline);
            
            
            builder.Property(p => p.IdPremiacaoDownline)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(p => p.Premio).HasMaxLength(120).IsRequired();
            builder.Property(p => p.Ativo).IsRequired();
            builder.Property(p => p.DataCadastro).IsRequired();
            builder.Property(p => p.Nivel).IsRequired();

            builder.HasOne(p => p.Graduacao)
                .WithMany(p => p.PremiacaoDownline)
                .HasForeignKey(p => p.IdGraduacao)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
