using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class PedidoDetalheMapping : IEntityTypeConfiguration<PedidoDetalhe>
    {
        public void Configure(EntityTypeBuilder<PedidoDetalhe> builder)
        {
            builder.HasKey(e => e.IdPedidoDetalhe);

            builder.Property(e => e.IdPedidoDetalhe)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Descricao).IsRequired(false);
            builder.Property(e => e.DataAtualizacao).IsRequired().HasColumnType("datetime");
            builder.Property(e => e.Ativo).IsRequired();
            builder.Property(e => e.IdUsuario);

            builder.HasOne(d => d.Pedido)
                .WithMany(p => p.PedidoDetalhe)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Status)
                .WithMany(p => p.PedidoDetalhe)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Usuario)
                .WithMany()
                .HasForeignKey(f => f.IdUsuario)
                .HasPrincipalKey(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
