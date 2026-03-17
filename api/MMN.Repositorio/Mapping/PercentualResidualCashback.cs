using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class PercentualResidualCashbackMapping : IEntityTypeConfiguration<PercentualResidualCashback>
    {
        public void Configure(EntityTypeBuilder<PercentualResidualCashback> builder)
        {
            builder.HasKey(k => k.IdPercentualResidualCashback);
            builder.HasAlternateKey(k => k.Nivel);


            builder.Property(p => p.IdPercentualResidualCashback)
                .IsRequired()
                .UseIdentityColumn();
            builder.Property(p => p.Nivel)
                .IsRequired();
            builder.Property(e => e.Percentual)
                .HasColumnType("decimal(18, 8)")
                .IsRequired();
        }
    }
}
