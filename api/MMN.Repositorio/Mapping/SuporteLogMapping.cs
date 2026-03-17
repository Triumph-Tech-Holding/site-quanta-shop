using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class SuporteLogMapping : IEntityTypeConfiguration<SuporteLog>
    {
        public void Configure(EntityTypeBuilder<SuporteLog> builder)
        {
            builder.HasKey(e => e.IdSuporteLog);
            builder.Property(e => e.DataCompra).IsRequired();
            builder.Property(e => e.SiteCompra).IsRequired().HasMaxLength(100);
            builder.Property(e => e.NumeroPedido).IsRequired(false).HasMaxLength(80);
            builder.Property(e => e.ValorPedido).IsRequired().HasColumnType("decimal(18, 8)");
            builder.Property(e => e.Observacao).IsRequired(false).HasMaxLength(250);
            builder.Property(e => e.UrlComprovante).IsRequired().HasMaxLength(150);
            builder.Property(e => e.DataSolicitacao).IsRequired();
            builder.Property(e => e.DataAtualizacao).IsRequired(false);
            builder.Property(e => e.ObservacaoAdmin).IsRequired(false).HasMaxLength(250);
            builder.Property(e => e.DataUpdate).IsRequired();

            builder.HasOne(e => e.Suporte)
                .WithMany(e => e.SuporteLog)
                .HasForeignKey(e => e.IdSuporte)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
