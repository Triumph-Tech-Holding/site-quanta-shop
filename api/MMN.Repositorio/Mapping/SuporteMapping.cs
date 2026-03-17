using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class SuporteMapping : IEntityTypeConfiguration<Suporte>
    {
        public void Configure(EntityTypeBuilder<Suporte> builder)
        {
            builder.HasKey(e => e.IdSuporte);
            builder.Property(e => e.DataCompra).IsRequired();
            builder.Property(e => e.SiteCompra).IsRequired().HasMaxLength(100);
            builder.Property(e => e.NumeroPedido).IsRequired(false).HasMaxLength(80);
            builder.Property(e => e.ValorPedido).IsRequired().HasColumnType("decimal(18, 8)");
            builder.Property(e => e.Observacao).IsRequired(false).HasMaxLength(250);
            builder.Property(e => e.UrlComprovante).HasMaxLength(150);
            builder.Property(e => e.DataSolicitacao).IsRequired();
            builder.Property(e => e.DataAtualizacao).IsRequired(false);
            builder.Property(e => e.ObservacaoAdmin).IsRequired(false).HasMaxLength(250);

            builder.HasOne(e => e.Usuario)
                .WithMany(e => e.Suporte)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UsuarioAcao)
                .WithMany(e => e.SuporteAdmin)
                .HasForeignKey(e => e.IdUsuarioAcao)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Status)
               .WithMany(e => e.Suporte)
               .HasForeignKey(e => e.IdStatus)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
