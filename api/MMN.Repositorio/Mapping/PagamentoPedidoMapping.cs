using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Repositorio.Mapping
{
    class PagamentoPedidoMapping : IEntityTypeConfiguration<PagamentoPedido>
    {
        public void Configure(EntityTypeBuilder<PagamentoPedido> builder)
        {
            builder.HasKey(k => new { k.IdPagamento, k.IdPedido });


            builder.Property(p => p.IdPagamento)
                .IsRequired();
            builder.Property(p => p.IdPedido)
                .IsRequired();
            builder.Property(p => p.Ordem)
                .IsRequired();
            builder.Property(p => p.Status)
                .IsRequired();
            builder.Property(p => p.Valor)
                .HasColumnType("decimal(18, 8)");
            builder.Property(p => p.ValorPago)
                .HasColumnType("decimal(18, 8)");

            builder.HasOne(o => o.Pagamento)
                .WithMany(m => m.PagamentoPedido)
                .HasForeignKey(f => f.IdPagamento)
                .HasPrincipalKey(p => p.IdPagamento)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Pedido)
                .WithMany(m => m.PagamentoPedido)
                .HasForeignKey(f => f.IdPedido)
                .HasPrincipalKey(p => p.IdPedido)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
