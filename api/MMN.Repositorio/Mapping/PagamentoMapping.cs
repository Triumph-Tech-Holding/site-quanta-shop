using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(e => e.IdPagamento);

            builder.Property(p => p.IdPagamento)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.IdPedido).IsRequired();
            builder.Property(e => e.DataValidade).IsRequired();

            builder.HasOne(o => o.Pedido)
                .WithMany(m => m.Pagamentos)
                .HasForeignKey(f => f.IdPedido)
                .HasPrincipalKey(p => p.IdPedido)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
