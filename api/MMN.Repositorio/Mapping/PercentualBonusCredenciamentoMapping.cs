using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class PercentualBonusCredenciamentoMapping : IEntityTypeConfiguration<PercentualBonusCredenciamento>
    {
        public void Configure(EntityTypeBuilder<PercentualBonusCredenciamento> builder)
        {
            builder.HasKey(k => k.IdPercentualBonusCredenciamento);
            builder.HasAlternateKey(k => k.IdProduto);


            builder.Property(p => p.IdPercentualBonusCredenciamento)
                .IsRequired()
                .UseIdentityColumn();
            builder.Property(p => p.IdProduto)
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
