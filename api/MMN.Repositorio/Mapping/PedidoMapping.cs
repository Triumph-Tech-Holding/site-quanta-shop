using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(e => e.IdPedido);
            
            
            builder.Property(p => p.IdPedido)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.DataPedido).HasColumnType("datetime");
            builder.Property(e => e.DataPagamento).IsRequired(false).HasColumnType("datetime");
            builder.Property(e => e.ValorTaxa).HasColumnType("decimal(18, 8)");
            builder.Property(e => e.Ativo);
            builder.Property(e => e.Codigo);
            builder.Property(e => e.EnderecoDeposito).IsRequired(false);
            builder.Property(e => e.MeioPagamento).IsRequired(true);
            builder.Property(e => e.ValorPago).IsRequired(false).HasColumnType("decimal(18, 8)");
            builder.Property(e => e.ValorPedido).HasColumnType("decimal(18, 8)");
            builder.Property(e => e.UrlPagamento).IsRequired(false);
            builder.Property(e => e.Cashback).IsRequired(false).HasColumnType("decimal(18, 8)");
            builder.HasIndex(e => e.IdVendaZanox).HasDatabaseName("IDX_Pedido_VendaZanox");
            builder.HasIndex(e => e.IdVendaAfilio).HasDatabaseName("IDX_Pedido_VendaAfilio");

            builder.HasOne(d => d.Transacao)
                .WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdTransacao)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.UsuarioComerciante)
                .WithMany(p => p.Vendas)
                .HasForeignKey(p => p.IdUsuarioComerciante)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
