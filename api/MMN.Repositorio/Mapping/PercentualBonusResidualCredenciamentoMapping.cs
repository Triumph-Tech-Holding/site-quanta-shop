using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class PercentualBonusResidualCredenciamentoMapping : IEntityTypeConfiguration<PercentualBonusResidualCredenciamento>
    {
        public void Configure(EntityTypeBuilder<PercentualBonusResidualCredenciamento> builder)
        {
            builder.HasKey(k => k.IdPercentualBonusResidualCredenciamento);
            builder.HasAlternateKey(k => new { k.IdProduto, k.Nivel });


            builder.Property(p => p.IdPercentualBonusResidualCredenciamento)
                .IsRequired()
                .UseIdentityColumn();
            builder.Property(p => p.IdProduto)
                .IsRequired();
            builder.Property(p => p.Nivel)
                .IsRequired();
            builder.Property(e => e.Percentual)
                .HasColumnType("decimal(18, 8)")
                .IsRequired();

            builder.HasOne(o => o.Produto)
                .WithMany()
                .HasForeignKey(f => f.IdProduto)
                .HasPrincipalKey(p=>p.IdProduto)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
